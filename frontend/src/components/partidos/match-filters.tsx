"use client";

import { FormEvent } from "react";
import { Button } from "@/components/layout/button";
import { DateTimeField } from "@/components/layout/date-time-field";
import { useTeams } from "@/hooks/equipos/use-teams";
import { useShellStore } from "@/stores/use-shell-store";

export type MatchDraft = {
  from: string;
  to: string;
  teamId: string;
  status: string;
};

const keys = {
  from: "matches.from",
  to: "matches.to",
  teamId: "matches.teamId",
  status: "matches.status",
};

export function MatchFilters({ onApply }: { onApply: (draft: MatchDraft) => void }) {
  const drafts = useShellStore((state) => state.drafts);
  const setDraft = useShellStore((state) => state.setDraft);
  const teams = useTeams({ pageNumber: 1, pageSize: 50, sortBy: "name", sortDirection: "asc" });

  function submit(event: FormEvent) {
    event.preventDefault();
    onApply({
      from: drafts[keys.from] ?? "",
      to: drafts[keys.to] ?? "",
      teamId: drafts[keys.teamId] ?? "",
      status: drafts[keys.status] ?? "",
    });
  }

  return (
    <form onSubmit={submit} className="mb-4 grid gap-2 md:grid-cols-[1fr_1fr_1fr_1fr_auto]">
      <DateTimeField label="Desde" value={drafts[keys.from] ?? ""} onChange={(value) => setDraft(keys.from, value)} />
      <DateTimeField label="Hasta" value={drafts[keys.to] ?? ""} onChange={(value) => setDraft(keys.to, value)} />
      <select aria-label="Equipo" value={drafts[keys.teamId] ?? ""} onChange={(event) => setDraft(keys.teamId, event.target.value)}>
        <option value="">Todos los equipos</option>
        {teams.data?.data.map((team) => (
          <option key={team.id} value={team.id}>
            {team.name}
          </option>
        ))}
      </select>
      <select aria-label="Estado" value={drafts[keys.status] ?? ""} onChange={(event) => setDraft(keys.status, event.target.value)}>
        <option value="">Cualquier estado</option>
        <option value="Programado">Programado</option>
        <option value="Finalizado">Finalizado</option>
      </select>
      <Button type="submit" tone="ghost">
        Aplicar
      </Button>
    </form>
  );
}
