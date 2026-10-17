"use client";

import { FormEvent } from "react";
import { Button } from "@/components/layout/button";
import { DateTimeField } from "@/components/layout/date-time-field";
import { SearchSelect } from "@/components/layout/search-select";
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
  const clearDrafts = useShellStore((state) => state.clearDrafts);
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

  function clear() {
    clearDrafts([keys.from, keys.to, keys.teamId, keys.status]);
    onApply({ from: "", to: "", teamId: "", status: "" });
  }

  return (
    <form onSubmit={submit} className="mb-4 grid gap-2 md:grid-cols-[1fr_1fr_1fr_1fr_auto]">
      <DateTimeField label="Desde" value={drafts[keys.from] ?? ""} onChange={(value) => setDraft(keys.from, value)} />
      <DateTimeField label="Hasta" value={drafts[keys.to] ?? ""} onChange={(value) => setDraft(keys.to, value)} />
      <SearchSelect
        label="Equipo"
        placeholder="Todos los equipos"
        emptyLabel="Todos los equipos"
        value={drafts[keys.teamId] ?? ""}
        onChange={(value) => setDraft(keys.teamId, value)}
        options={(teams.data?.data ?? []).map((team) => ({ value: team.id, label: team.name }))}
      />
      <SearchSelect
        label="Estado"
        placeholder="Cualquier estado"
        emptyLabel="Cualquier estado"
        value={drafts[keys.status] ?? ""}
        onChange={(value) => setDraft(keys.status, value)}
        options={[
          { value: "Programado", label: "Programado" },
          { value: "Finalizado", label: "Finalizado" },
        ]}
      />
      <div className="flex gap-2">
        <Button type="submit" tone="ghost">
          Aplicar
        </Button>
        <Button type="button" tone="ghost" onClick={clear}>
          Limpiar
        </Button>
      </div>
    </form>
  );
}
