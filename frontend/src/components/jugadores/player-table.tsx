"use client";

import { type LegacyColumnDef } from "@tanstack/react-table/legacy";
import { useMemo } from "react";
import { Button } from "@/components/ui/button";
import { DataTable } from "@/components/layout/data-table";
import { Monogram } from "@/components/layout/monogram";
import { Badge } from "@/components/ui/badge";
import { Player } from "@/services/jugadores/player.types";

export function PlayerTable({ players, onEdit }: { players: Player[]; onEdit: (player: Player) => void }) {
  const columns = useMemo<LegacyColumnDef<Player>[]>(
    () => [
      {
        accessorKey: "shirtNumber",
        header: "Número",
        cell: ({ row }) => <Badge>{row.original.shirtNumber}</Badge>,
      },
      {
        accessorKey: "name",
        header: "Jugador",
        cell: ({ row }) => (
          <span className="flex items-center gap-3 font-medium">
            <Monogram name={row.original.name} />
            {row.original.name}
          </span>
        ),
      },
      {
        accessorKey: "role",
        header: "Puesto",
        cell: ({ row }) => <span>{row.original.role}</span>,
      },
      {
        accessorKey: "teamName",
        header: "Equipo",
        cell: ({ row }) => <span className="text-muted-foreground">{row.original.teamName}</span>,
      },
      {
        id: "edit",
        header: "",
        cell: ({ row }) => (
          <Button variant="ghost" size="sm" onClick={() => onEdit(row.original)}>
            Editar
          </Button>
        ),
      },
    ],
    [onEdit],
  );

  return (
    <>
      <ul className="grid gap-2 md:hidden">
        {players.map((player) => (
          <li key={player.id} className="board flex items-center gap-3 px-4 py-3">
            <Badge>{player.shirtNumber}</Badge>
            <Monogram name={player.name} />
            <span className="min-w-0 flex-1">
              <span className="block truncate font-medium">{player.name}</span>
              <span className="block truncate text-sm text-muted-foreground">
                {player.role} · {player.teamName}
              </span>
            </span>
            <Button variant="ghost" size="sm" onClick={() => onEdit(player)}>
              Editar
            </Button>
          </li>
        ))}
      </ul>
      <div className="hidden md:block">
        <DataTable columns={columns} data={players} />
      </div>
    </>
  );
}
