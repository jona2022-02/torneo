"use client";

import { FormEvent } from "react";
import { Button } from "@/components/layout/button";
import { SearchSelect } from "@/components/layout/search-select";
import { useTeams } from "@/hooks/equipos/use-teams";
import { useShellStore } from "@/stores/use-shell-store";

const draftKey = "scorers.teamId";

export function ScorerFilters({ onApply }: { onApply: (teamId: string) => void }) {
  const draft = useShellStore((state) => state.drafts[draftKey] ?? "");
  const setDraft = useShellStore((state) => state.setDraft);
  const clearDrafts = useShellStore((state) => state.clearDrafts);
  const teams = useTeams({ pageNumber: 1, pageSize: 50, sortBy: "name", sortDirection: "asc" });

  function submit(event: FormEvent) {
    event.preventDefault();
    onApply(draft);
  }

  function clear() {
    clearDrafts([draftKey]);
    onApply("");
  }

  return (
    <form onSubmit={submit} className="mb-4 flex gap-2">
      <div className="min-w-0 flex-1">
        <SearchSelect
          label="Equipo"
          placeholder="Todos los equipos"
          emptyLabel="Todos los equipos"
          value={draft}
          onChange={(value) => setDraft(draftKey, value)}
          options={(teams.data?.data ?? []).map((team) => ({ value: team.id, label: team.name }))}
        />
      </div>
      <Button type="submit" tone="ghost">
        Aplicar
      </Button>
      <Button type="button" tone="ghost" onClick={clear}>
        Limpiar
      </Button>
    </form>
  );
}
