"use client";

import { FormEvent, useState } from "react";
import { Button } from "@/components/layout/button";
import { Field } from "@/components/layout/field";
import { useCreatePlayer } from "@/hooks/jugadores/use-create-player";
import { isApiError } from "@/services/http/api";
import { useShellStore } from "@/stores/use-shell-store";

export function PlayerForm({ teamId, onCreated }: { teamId: string; onCreated: () => void }) {
  const [name, setName] = useState("");
  const [shirt, setShirt] = useState("1");
  const [error, setError] = useState<string | null>(null);
  const create = useCreatePlayer();
  const notify = useShellStore((state) => state.notify);

  function submit(event: FormEvent) {
    event.preventDefault();
    setError(null);
    create.mutate(
      { teamId, name: name.trim(), shirtNumber: Number(shirt) },
      {
        onSuccess: () => {
          notify({ tone: "ok", text: "Jugador registrado." });
          onCreated();
        },
        onError: (failure) => {
          const code = isApiError(failure) ? failure.code : "error";
          const message = failure instanceof Error ? failure.message : "No se pudo registrar el jugador.";
          setError(`${code}: ${message}`);
        },
      },
    );
  }

  return (
    <form onSubmit={submit} className="grid gap-4">
      <Field label="Nombre">
        <input value={name} onChange={(event) => setName(event.target.value)} maxLength={80} required />
      </Field>
      <Field label="Dorsal">
        <input value={shirt} onChange={(event) => setShirt(event.target.value)} type="number" min={1} max={99} required />
      </Field>
      {error ? <p className="text-sm text-danger">{error}</p> : null}
      <Button type="submit" disabled={create.isPending}>
        {create.isPending ? "Guardando..." : "Registrar jugador"}
      </Button>
    </form>
  );
}
