"use client";

import { type LegacyColumnDef } from "@tanstack/react-table/legacy";
import { useMemo } from "react";
import { DataTable } from "@/components/layout/data-table";
import { Monogram } from "@/components/layout/monogram";
import { Badge } from "@/components/ui/badge";
import { Scorer } from "@/services/goleadores/scorer.types";

export function ScorerTable({ rows, offset }: { rows: Scorer[]; offset: number }) {
  const columns = useMemo<LegacyColumnDef<Scorer>[]>(
    () => [
      {
        id: "place",
        header: "#",
        cell: ({ row }) => <Badge variant={offset + row.index === 0 ? "default" : "secondary"}>{offset + row.index + 1}</Badge>,
      },
      {
        accessorKey: "playerName",
        header: "Jugador",
        cell: ({ row }) => (
          <span className="flex items-center gap-3 font-medium">
            <Monogram name={row.original.playerName} />
            {row.original.playerName}
          </span>
        ),
      },
      {
        accessorKey: "teamName",
        header: "Equipo",
        cell: ({ row }) => <span className="text-muted-foreground">{row.original.teamName}</span>,
      },
      {
        accessorKey: "goals",
        header: () => <span className="block text-right">Goles</span>,
        cell: ({ row }) => (
          <span className="block text-right">
            <Badge>{row.original.goals}</Badge>
          </span>
        ),
      },
    ],
    [offset],
  );

  return <DataTable columns={columns} data={rows} />;
}
