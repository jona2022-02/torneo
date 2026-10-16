export function PageHeader({
  kicker,
  title,
  action,
}: {
  kicker: string;
  title: string;
  action?: React.ReactNode;
}) {
  return (
    <header className="mb-6 flex flex-wrap items-end justify-between gap-4">
      <div>
        <p className="text-[11px] font-semibold uppercase tracking-[0.22em] text-gold">{kicker}</p>
        <h1 className="mt-1 font-display text-5xl leading-none tracking-tight">{title}</h1>
      </div>
      {action}
    </header>
  );
}
