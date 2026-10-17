"use client";

import { FormEvent, useState } from "react";
import { Button } from "@/components/layout/button";
import { Field } from "@/components/layout/field";
import { useCreateTeam } from "@/hooks/equipos/use-create-team";
import { failureNotice } from "@/components/layout/toaster";
import { useShellStore } from "@/stores/use-shell-store";

export function TeamForm({ onCreated }: { onCreated: () => void }) {
  const [name, setName] = useState("");
  const create = useCreateTeam();
  const notify = useShellStore((state) => state.notify);

  function submit(event: FormEvent) {
    event.preventDefault();
    create.mutate(name.trim(), {
      onSuccess: () => {
        setName("");
        notify({ tone: "ok", text: "Equipo creado." });
        onCreated();
      },
      onError: (failure) => notify(failureNotice(failure, "No se pudo crear el equipo.")),
    });
  }

  return (
    <form onSubmit={submit} className="grid gap-4">
      <Field label="Nombre">
        <input value={name} onChange={(event) => setName(event.target.value)} maxLength={80} required />
      </Field>
      <Button type="submit" disabled={create.isPending}>
        {create.isPending ? "Guardando..." : "Crear equipo"}
      </Button>
    </form>
  );
}
