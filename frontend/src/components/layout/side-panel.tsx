"use client";

export function SidePanel({
  open,
  title,
  onClose,
  children,
}: {
  open: boolean;
  title: string;
  onClose: () => void;
  children: React.ReactNode;
}) {
  if (!open) return null;

  return (
    <>
      <button className="fixed inset-0 z-30 bg-black/60 backdrop-blur-sm" aria-label="Cerrar panel" onClick={onClose} />
      <aside className="fixed top-3 right-3 bottom-3 z-40 flex w-[min(100%-1.5rem,28rem)] flex-col gap-5 overflow-auto rounded-[1.6rem] border border-line bg-panel p-6 shadow-2xl">
        <div className="flex items-center justify-between">
          <h2 className="font-display text-3xl">{title}</h2>
          <button className="rounded-full px-3 py-1 text-sm text-quiet" onClick={onClose}>
            Cerrar
          </button>
        </div>
        {children}
      </aside>
    </>
  );
}
