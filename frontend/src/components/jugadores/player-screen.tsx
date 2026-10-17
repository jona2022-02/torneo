"use client";

import Link from "next/link";
import { useState } from "react";
import { useParams } from "next/navigation";
import { Button } from "@/components/layout/button";
import { EmptyState } from "@/components/layout/empty-state";
import { ErrorState } from "@/components/layout/error-state";
import { PageHeader } from "@/components/layout/page-header";
import { SidePanel } from "@/components/layout/side-panel";
import { TableSkeleton } from "@/components/layout/table-skeleton";
import { Lineup } from "@/components/jugadores/lineup";
import { PlayerForm } from "@/components/jugadores/player-form";
import { PlayerTable } from "@/components/jugadores/player-table";
import { RoleForm } from "@/components/jugadores/role-form";
import { useTeam } from "@/hooks/equipos/use-team";
import { usePlayers } from "@/hooks/jugadores/use-players";
import { Player } from "@/services/jugadores/player.types";

export function PlayerScreen() {
  const params = useParams<{ id: string }>();
  const teamId = params.id;
  const team = useTeam(teamId);
  const players = usePlayers({ teamId, pageNumber: 1, pageSize: 50, sortBy: "shirtNumber", sortDirection: "asc" });
  const [open, setOpen] = useState(false);
  const [editing, setEditing] = useState<Player | null>(null);

  return (
    <>
      <Link href="/equipos" className="mb-3 inline-block text-sm text-quiet">
        Volver a equipos
      </Link>
      <PageHeader
        kicker="Plantel"
        title={team.data?.name ?? "Equipo"}
        action={<Button onClick={() => setOpen(true)}>Nuevo jugador</Button>}
      />
      {team.isError ? <ErrorState error={team.error} /> : null}
      {players.isPending ? <TableSkeleton /> : null}
      {players.isError ? <ErrorState error={players.error} /> : null}
      {players.data && players.data.data.length === 0 ? (
        <EmptyState title="Este equipo todavía no tiene jugadores." action={<Button onClick={() => setOpen(true)}>Registrar jugador</Button>} />
      ) : null}
      {players.data && players.data.data.length > 0 ? (
        <>
          <Lineup players={players.data.data} />
          <PlayerTable players={players.data.data} onEdit={setEditing} />
        </>
      ) : null}
      <SidePanel open={open} title="Nuevo jugador" onClose={() => setOpen(false)}>
        <PlayerForm teamId={teamId} onCreated={() => setOpen(false)} />
      </SidePanel>
      <SidePanel open={editing !== null} title="Cambiar puesto" onClose={() => setEditing(null)}>
        {editing ? <RoleForm key={editing.id} player={editing} onSaved={() => setEditing(null)} /> : null}
      </SidePanel>
    </>
  );
}
