"use client";

import { Button } from "@/components/layout/button";

export function DataPager({
  page,
  totalPages,
  totalRecords,
  onPage,
}: {
  page: number;
  totalPages: number;
  totalRecords: number;
  onPage: (page: number) => void;
}) {
  return (
    <div className="mt-4 flex flex-wrap items-center justify-between gap-3 rounded-2xl border border-line bg-panel px-4 py-2 text-sm text-ink backdrop-blur">
      <span className="text-quiet">{totalRecords} registros</span>
      <div className="flex items-center gap-2">
        <Button tone="ghost" disabled={page <= 1} onClick={() => onPage(page - 1)}>
          Anterior
        </Button>
        <span className="min-w-14 text-center tabular-nums">
          {page} / {Math.max(totalPages, 1)}
        </span>
        <Button tone="ghost" disabled={page >= totalPages} onClick={() => onPage(page + 1)}>
          Siguiente
        </Button>
      </div>
    </div>
  );
}
