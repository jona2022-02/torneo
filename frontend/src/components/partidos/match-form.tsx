"use client";

import { FormEvent, useState } from "react";
import { Button } from "@/components/layout/button";
import { DateTimeField } from "@/components/layout/date-time-field";
import { Field } from "@/components/layout/field";
import { useTeams } from "@/hooks/equipos/use-teams";
import { useCreateMatch } from "@/hooks/partidos/use-create-match";
import { toApiDate } from "@/lib/format";
import { isApiError } from "@/services/http/api";
import { useShellStore } from "@/stores/use-shell-store";

export function MatchForm({ onCreated }: { onCreated: () => void }) {
  const teams = useTeams({ pageNumber: 1, pageSize: 50, sortBy: "name", sortDirection: "asc" });
  const [home, setHome] = useState("");
  const [away, setAway] = useState("");
  const [kickoff, setKickoff] = useState("");
  const [error, setError] = useState<string | null>(null);
  const create = useCreateMatch();
  const notify = useShellStore((state) => state.notify);

  function submit(event: FormEvent) {
    event.preventDefault();
    const when = toApiDate(kickoff);
    if (!when) return;
    setError(null);
    create.mutate(
      { homeTeamId: home, awayTeamId: away, kickoff: when },
      {
        onSuccess: () => {
          notify({ tone: "ok", text: "Partido programado." });
          onCreated();
        },
        onError: (failure) => {
          const code = isApiError(failure) ? failure.code : "error";
          const message = failure instanceof Error ? failure.message : "No se pudo programar el partido.";
          setError(`${code}: ${message}`);
        },
      },
    );
  }

  return (
    <form onSubmit={submit} className="grid gap-4">
      <Field label="Local">
        <TeamSelect value={home} onChange={setHome} teams={teams.data?.data ?? []} />
      </Field>
      <Field label="Visita">
        <TeamSelect value={away} onChange={setAway} teams={teams.data?.data ?? []} />
      </Field>
      <Field label="Fecha y hora">
        <DateTimeField value={kickoff} onChange={setKickoff} label="Fecha y hora" required />
      </Field>
      {error ? <p className="text-sm text-danger">{error}</p> : null}
      <Button type="submit" disabled={create.isPending}>
        {create.isPending ? "Guardando..." : "Programar partido"}
      </Button>
    </form>
  );
}

function TeamSelect({
  value,
  onChange,
  teams,
}: {
  value: string;
  onChange: (value: string) => void;
  teams: { id: string; name: string }[];
}) {
  return (
    <select value={value} onChange={(event) => onChange(event.target.value)} required>
      <option value="">Elegir equipo</option>
      {teams.map((team) => (
        <option key={team.id} value={team.id}>
          {team.name}
        </option>
      ))}
    </select>
  );
}
