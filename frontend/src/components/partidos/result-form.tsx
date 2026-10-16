"use client";

import { FormEvent, useState } from "react";
import { Button } from "@/components/layout/button";
import { Field } from "@/components/layout/field";
import { usePlayers } from "@/hooks/jugadores/use-players";
import { useRegisterResult } from "@/hooks/partidos/use-register-result";
import { isApiError } from "@/services/http/api";
import { MatchDetail } from "@/services/partidos/match.types";
import { Player } from "@/services/jugadores/player.types";
import { useShellStore } from "@/stores/use-shell-store";

type GoalDraft = { playerId: string; minute: string };

export function ResultForm({ match, onSaved }: { match: MatchDetail; onSaved: () => void }) {
  const home = usePlayers({ teamId: match.homeTeamId, pageNumber: 1, pageSize: 50 });
  const away = usePlayers({ teamId: match.awayTeamId, pageNumber: 1, pageSize: 50 });
  const players = [...(home.data?.data ?? []), ...(away.data?.data ?? [])];
  const [homeScore, setHomeScore] = useState(String(match.homeScore ?? 0));
  const [awayScore, setAwayScore] = useState(String(match.awayScore ?? 0));
  const [goals, setGoals] = useState<GoalDraft[]>([{ playerId: "", minute: "1" }]);
  const [error, setError] = useState<string | null>(null);
  const save = useRegisterResult(match.id);
  const notify = useShellStore((state) => state.notify);

  function submit(event: FormEvent) {
    event.preventDefault();
    setError(null);
    const lines = goals.filter((goal) => goal.playerId).map((goal) => ({ playerId: goal.playerId, minute: Number(goal.minute) }));
    save.mutate(
      { homeScore: Number(homeScore), awayScore: Number(awayScore), goals: lines },
      {
        onSuccess: () => {
          notify({ tone: "ok", text: "Resultado registrado. La tabla y los goleadores ya se actualizaron." });
          onSaved();
        },
        onError: (failure) => {
          const code = isApiError(failure) ? failure.code : "error";
          const message = failure instanceof Error ? failure.message : "No se pudo registrar el resultado.";
          setError(`${code}: ${message}`);
        },
      },
    );
  }

  return (
    <form onSubmit={submit} className="grid gap-4">
      <div className="grid grid-cols-2 gap-3">
        <Field label={match.homeTeamName}>
          <input type="number" min={0} value={homeScore} onChange={(event) => setHomeScore(event.target.value)} required />
        </Field>
        <Field label={match.awayTeamName}>
          <input type="number" min={0} value={awayScore} onChange={(event) => setAwayScore(event.target.value)} required />
        </Field>
      </div>
      <div className="grid gap-2">
        <p className="text-[11px] uppercase tracking-[0.14em] text-quiet">Goles</p>
        {goals.map((goal, index) => (
          <GoalLine key={index} goal={goal} players={players} onChange={(next) => setGoals(goals.map((item, itemIndex) => (itemIndex === index ? next : item)))} />
        ))}
        <Button type="button" tone="ghost" onClick={() => setGoals([...goals, { playerId: "", minute: "1" }])}>
          Agregar gol
        </Button>
      </div>
      <p className="text-xs text-quiet">El marcador tiene que coincidir con los goles de cada equipo.</p>
      {error ? <p className="text-sm text-danger">{error}</p> : null}
      <Button type="submit" disabled={save.isPending}>
        {save.isPending ? "Guardando..." : "Guardar resultado"}
      </Button>
    </form>
  );
}

function GoalLine({
  goal,
  players,
  onChange,
}: {
  goal: GoalDraft;
  players: Player[];
  onChange: (goal: GoalDraft) => void;
}) {
  return (
    <div className="grid grid-cols-[1fr_5rem] gap-2">
      <select value={goal.playerId} onChange={(event) => onChange({ ...goal, playerId: event.target.value })} aria-label="Goleador">
        <option value="">Jugador</option>
        {players.map((player) => (
          <option key={player.id} value={player.id}>
            {player.shirtNumber} {player.name}
          </option>
        ))}
      </select>
      <input aria-label="Minuto" type="number" min={1} max={120} value={goal.minute} onChange={(event) => onChange({ ...goal, minute: event.target.value })} />
    </div>
  );
}
