"use client";

import { FormEvent, useState } from "react";
import { Button } from "@/components/layout/button";
import { DateTimeField } from "@/components/layout/date-time-field";
import { Field } from "@/components/layout/field";
import { SearchSelect } from "@/components/layout/search-select";
import { useTeams } from "@/hooks/equipos/use-teams";
import { useCreateMatch } from "@/hooks/partidos/use-create-match";
import { toApiDate } from "@/lib/format";
import { failureNotice } from "@/components/layout/toaster";
import { useShellStore } from "@/stores/use-shell-store";

export function MatchForm({ onCreated }: { onCreated: () => void }) {
  const teams = useTeams({ pageNumber: 1, pageSize: 50, sortBy: "name", sortDirection: "asc" });
  const [home, setHome] = useState("");
  const [away, setAway] = useState("");
  const [kickoff, setKickoff] = useState("");
  const create = useCreateMatch();
  const notify = useShellStore((state) => state.notify);

  function submit(event: FormEvent) {
    event.preventDefault();
    const when = toApiDate(kickoff);
    if (!when) return;
    create.mutate(
      { homeTeamId: home, awayTeamId: away, kickoff: when },
      {
        onSuccess: () => {
          notify({ tone: "ok", text: "Partido programado." });
          onCreated();
        },
        onError: (failure) => notify(failureNotice(failure, "No se pudo programar el partido.")),
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
      <Button type="submit" disabled={create.isPending}>
        {create.isPending ? "Guardando..." : "Programar partido"}
      </Button>
    </form>
  );
}
