"use client";

import { FormEvent, useState } from "react";
import { Button } from "@/components/layout/button";
import { Field } from "@/components/layout/field";
import { useCreateTeam } from "@/hooks/equipos/use-create-team";
import { isApiError } from "@/services/http/api";
import { useShellStore } from "@/stores/use-shell-store";

export function TeamForm({ onCreated }: { onCreated: () => void }) {
  const [name, setName] = useState("");
  const [error, setError] = useState<string | null>(null);
  const create = useCreateTeam();
  const notify = useShellStore((state) => state.notify);

  function submit(event: FormEvent) {
    event.preventDefault();
    setError(null);
    create.mutate(name.trim(), {
      onSuccess: () => {
        setName("");
        notify({ tone: "ok", text: "Equipo creado." });
        onCreated();
      },
      onError: (failure) => {
        const code = isApiError(failure) ? failure.code : "error";
        const message = failure instanceof Error ? failure.message : "No se pudo crear el equipo.";
        setError(`${code}: ${message}`);
      },
    });
  }

  return (
    <form onSubmit={submit} className="grid gap-4">
      <Field label="Nombre">
        <input value={name} onChange={(event) => setName(event.target.value)} maxLength={80} required />
      </Field>
      {error ? <p className="text-sm text-danger">{error}</p> : null}
      <Button type="submit" disabled={create.isPending}>
        {create.isPending ? "Guardando..." : "Crear equipo"}
      </Button>
    </form>
  );
}
