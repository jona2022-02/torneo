"use client";

import { FormEvent, useState } from "react";
import { Button } from "@/components/layout/button";
import { Field } from "@/components/layout/field";
import { SearchSelect } from "@/components/layout/search-select";
import { useCreatePlayer } from "@/hooks/jugadores/use-create-player";
import { failureNotice } from "@/components/layout/toaster";
import { useShellStore } from "@/stores/use-shell-store";

export function PlayerForm({ teamId, onCreated }: { teamId: string; onCreated: () => void }) {
  const [name, setName] = useState("");
  const [shirt, setShirt] = useState("1");
  const [role, setRole] = useState("Defensa");
  const create = useCreatePlayer();
  const notify = useShellStore((state) => state.notify);

  function submit(event: FormEvent) {
    event.preventDefault();
    create.mutate(
      { teamId, name: name.trim(), shirtNumber: Number(shirt), role },
      {
        onSuccess: () => {
          notify({ tone: "ok", text: "Jugador registrado." });
          onCreated();
        },
        onError: (failure) => notify(failureNotice(failure, "No se pudo registrar el jugador.")),
      },
    );
  }

  return (
    <form onSubmit={submit} className="grid gap-4">
      <Field label="Nombre">
        <input value={name} onChange={(event) => setName(event.target.value)} maxLength={80} required />
      </Field>
      <Field label="Número">
        <input value={shirt} onChange={(event) => setShirt(event.target.value)} type="number" min={1} max={99} required />
      </Field>
      <Field label="Puesto">
        <SearchSelect
          label="Puesto"
          placeholder="Elegir puesto"
          value={role}
          onChange={setRole}
          options={[
            { value: "Portero", label: "Portero" },
            { value: "Defensa", label: "Defensa" },
            { value: "Volante", label: "Volante" },
            { value: "Delantero", label: "Delantero" },
          ]}
        />
      </Field>
      <Button type="submit" disabled={create.isPending}>
        {create.isPending ? "Guardando..." : "Registrar jugador"}
      </Button>
    </form>
  );
}
