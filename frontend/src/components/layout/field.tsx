export function Field({ label, children }: { label: string; children: React.ReactNode }) {
  return (
    <label className="grid gap-1 text-sm">
      <span className="text-[11px] font-medium uppercase tracking-[0.14em] text-quiet">{label}</span>
      {children}
    </label>
  );
}
