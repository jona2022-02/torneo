"use client";

import { type LegacyColumnDef } from "@tanstack/react-table/legacy";
import { useMemo } from "react";
import { DataTable } from "@/components/layout/data-table";
import { Monogram } from "@/components/layout/monogram";
import { Badge } from "@/components/ui/badge";
import { Standing } from "@/services/tabla/standing.types";

const stats = [
  ["played", "PJ"],
  ["won", "PG"],
  ["drawn", "PE"],
  ["lost", "PP"],
  ["goalsFor", "GF"],
  ["goalsAgainst", "GC"],
  ["goalDifference", "DG"],
] as const;

export function StandingsTable({ rows, offset }: { rows: Standing[]; offset: number }) {
  const columns = useMemo<LegacyColumnDef<Standing>[]>(
    () => [
      {
        id: "place",
        header: "#",
        cell: ({ row }) => <Badge variant={offset + row.index === 0 ? "default" : "secondary"}>{offset + row.index + 1}</Badge>,
      },
      {
        accessorKey: "teamName",
        header: "Equipo",
        cell: ({ row }) => (
          <span className="flex items-center gap-3 font-medium">
            <Monogram name={row.original.teamName} />
            {row.original.teamName}
          </span>
        ),
      },
      ...stats.map(
        ([key, label]): LegacyColumnDef<Standing> => ({
          accessorKey: key,
          header: () => <span className="block text-right">{label}</span>,
          cell: ({ row }) => <span className="block text-right tabular-nums text-muted-foreground">{row.original[key]}</span>,
        }),
      ),
      {
        accessorKey: "points",
        header: () => <span className="block text-right">Pts</span>,
        cell: ({ row }) => (
          <span className="block text-right">
            <Badge>{row.original.points}</Badge>
          </span>
        ),
      },
    ],
    [offset],
  );

  return <DataTable columns={columns} data={rows} />;
}
