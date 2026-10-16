"use client";

import { FormEvent, useState } from "react";
import { Button } from "@/components/layout/button";
import { DateTimeField } from "@/components/layout/date-time-field";
import { Field } from "@/components/layout/field";
import { SearchSelect } from "@/components/layout/search-select";
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
        <SearchSelect
          label="Local"
          placeholder="Elegir equipo"
          value={home}
          onChange={setHome}
          options={(teams.data?.data ?? []).map((team) => ({ value: team.id, label: team.name }))}
        />
      </Field>
      <Field label="Visita">
        <SearchSelect
          label="Visita"
          placeholder="Elegir equipo"
          value={away}
          onChange={setAway}
          options={(teams.data?.data ?? []).map((team) => ({ value: team.id, label: team.name }))}
        />
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
