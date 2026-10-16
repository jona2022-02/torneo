export function EmptyState({ title, action }: { title: string; action?: React.ReactNode }) {
  return (
    <div className="board px-4 py-16 text-center">
      <p className="text-sm text-quiet">{title}</p>
      {action ? <div className="mt-4">{action}</div> : null}
    </div>
  );
}
