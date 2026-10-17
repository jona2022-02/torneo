"use client";

import { FormEvent, useState } from "react";
import { Button } from "@/components/layout/button";
import { Field } from "@/components/layout/field";
import { SearchSelect } from "@/components/layout/search-select";
import { failureNotice } from "@/components/layout/toaster";
import { playerRoles } from "@/components/jugadores/player-roles";
import { useUpdatePlayer } from "@/hooks/jugadores/use-update-player";
import { Player } from "@/services/jugadores/player.types";
import { useShellStore } from "@/stores/use-shell-store";

export function RoleForm({ player, onSaved }: { player: Player; onSaved: () => void }) {
  const [role, setRole] = useState(player.role);
  const update = useUpdatePlayer();
  const notify = useShellStore((state) => state.notify);

  function submit(event: FormEvent) {
    event.preventDefault();
    update.mutate(
      { id: player.id, role },
      {
        onSuccess: () => {
          notify({ tone: "ok", text: "Puesto actualizado." });
          onSaved();
        },
        onError: (failure) => notify(failureNotice(failure, "No se pudo cambiar el puesto.")),
      },
    );
  }

  return (
    <form onSubmit={submit} className="grid gap-4">
      <p className="text-sm text-quiet">{player.name}</p>
      <Field label="Puesto">
        <SearchSelect label="Puesto" placeholder="Elegir puesto" value={role} onChange={setRole} options={playerRoles} />
      </Field>
      <Button type="submit" disabled={update.isPending || role === player.role}>
        {update.isPending ? "Guardando..." : "Guardar puesto"}
      </Button>
    </form>
  );
}
