"use client";

import { FormEvent } from "react";
import { Button } from "@/components/layout/button";
import { useTeams } from "@/hooks/equipos/use-teams";
import { useShellStore } from "@/stores/use-shell-store";

const draftKey = "scorers.teamId";

export function ScorerFilters({ onApply }: { onApply: (teamId: string) => void }) {
  const draft = useShellStore((state) => state.drafts[draftKey] ?? "");
  const setDraft = useShellStore((state) => state.setDraft);
  const teams = useTeams({ pageNumber: 1, pageSize: 50, sortBy: "name", sortDirection: "asc" });

  function submit(event: FormEvent) {
    event.preventDefault();
    onApply(draft);
  }

  return (
    <form onSubmit={submit} className="mb-4 flex gap-2">
      <select aria-label="Equipo" value={draft} onChange={(event) => setDraft(draftKey, event.target.value)} className="min-w-0 flex-1">
        <option value="">Todos los equipos</option>
        {teams.data?.data.map((team) => (
          <option key={team.id} value={team.id}>
            {team.name}
          </option>
        ))}
      </select>
      <Button type="submit" tone="ghost">
        Aplicar
      </Button>
    </form>
  );
}
