"use client";

import { type LegacyColumnDef } from "@tanstack/react-table/legacy";
import { useMemo } from "react";
import { DataTable } from "@/components/layout/data-table";
import { Monogram } from "@/components/layout/monogram";
import { Badge } from "@/components/ui/badge";
import { Player } from "@/services/jugadores/player.types";

export function PlayerTable({ players }: { players: Player[] }) {
  const columns = useMemo<LegacyColumnDef<Player>[]>(
    () => [
      {
        accessorKey: "shirtNumber",
        header: "Dorsal",
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
    ],
    [],
  );

  return <DataTable columns={columns} data={players} />;
}
