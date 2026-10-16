"use client";

import { flexRender, type RowData } from "@tanstack/react-table";
import { getCoreRowModel, useLegacyTable, type LegacyColumnDef } from "@tanstack/react-table/legacy";
import { motion, useReducedMotion } from "motion/react";
import { Card } from "@/components/ui/card";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";

const MotionRow = motion.create(TableRow);

export function DataTable<T extends RowData>({
  columns,
  data,
  onRowClick,
  onRowHover,
}: {
  columns: LegacyColumnDef<T, unknown>[];
  data: T[];
  onRowClick?: (row: T) => void;
  onRowHover?: (row: T) => void;
}) {
  const reduce = useReducedMotion();
  const table = useLegacyTable({ data, columns, getCoreRowModel: getCoreRowModel() });

  return (
    <Card className="gap-0 py-0">
      <Table>
        <TableHeader>
          {table.getHeaderGroups().map((group) => (
            <TableRow key={group.id}>
              {group.headers.map((header) => (
                <TableHead key={header.id}>
                  {header.isPlaceholder ? null : flexRender(header.column.columnDef.header, header.getContext())}
                </TableHead>
              ))}
            </TableRow>
          ))}
        </TableHeader>
        <TableBody>
          {table.getRowModel().rows.map((row, index) => (
            <MotionRow
              key={row.id}
              initial={reduce ? false : { opacity: 0, y: 6 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.18, delay: reduce ? 0 : index * 0.03 }}
              className={onRowClick ? "cursor-pointer" : undefined}
              onClick={() => onRowClick?.(row.original)}
              onMouseEnter={() => onRowHover?.(row.original)}
            >
              {row.getVisibleCells().map((cell) => (
                <TableCell key={cell.id}>{flexRender(cell.column.columnDef.cell, cell.getContext())}</TableCell>
              ))}
            </MotionRow>
          ))}
        </TableBody>
      </Table>
    </Card>
  );
}
