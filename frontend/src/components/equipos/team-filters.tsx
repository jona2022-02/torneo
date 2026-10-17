"use client";

import { FormEvent } from "react";
import { Button } from "@/components/layout/button";
import { useShellStore } from "@/stores/use-shell-store";

const draftKey = "teams.name";

export function TeamFilters({ onApply }: { onApply: (name: string) => void }) {
  const draft = useShellStore((state) => state.drafts[draftKey] ?? "");
  const setDraft = useShellStore((state) => state.setDraft);
  const clearDrafts = useShellStore((state) => state.clearDrafts);

  function submit(event: FormEvent) {
    event.preventDefault();
    onApply(draft.trim());
  }

  function clear() {
    clearDrafts([draftKey]);
    onApply("");
  }

  return (
    <form onSubmit={submit} className="mb-4 flex gap-2">
      <input
        value={draft}
        onChange={(event) => setDraft(draftKey, event.target.value)}
        placeholder="Buscar por nombre"
        aria-label="Nombre del equipo"
        className="min-w-0 flex-1"
      />
      <Button type="submit" tone="ghost">
        Aplicar
      </Button>
      <Button type="button" tone="ghost" onClick={clear}>
        Limpiar
      </Button>
    </form>
  );
}
