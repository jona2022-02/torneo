"use client";

import { useEffect } from "react";
import { CircleAlertIcon, CircleCheckIcon, XIcon } from "lucide-react";
import { AnimatePresence, motion, useReducedMotion } from "motion/react";
import { isApiError } from "@/services/http/api";
import { Notice, useShellStore } from "@/stores/use-shell-store";

export function failureNotice(error: unknown, fallback: string): Omit<Notice, "id"> {
  return {
    tone: "error",
    code: isApiError(error) ? error.code : "error",
    text: error instanceof Error ? error.message : fallback,
  };
}

export function Toaster() {
  const toasts = useShellStore((state) => state.toasts);
  const dismiss = useShellStore((state) => state.dismiss);
  const reduce = useReducedMotion();

  return (
    <div className="pointer-events-none fixed right-4 bottom-4 z-[80] flex w-[min(100%-2rem,22rem)] flex-col gap-2">
      <AnimatePresence initial={false}>
        {toasts.map((toast) => (
          <ToastCard key={toast.id} toast={toast} reduce={Boolean(reduce)} onDismiss={() => dismiss(toast.id)} />
        ))}
      </AnimatePresence>
    </div>
  );
}

function ToastCard({ toast, reduce, onDismiss }: { toast: Notice; reduce: boolean; onDismiss: () => void }) {
  const dismiss = useShellStore((state) => state.dismiss);

  useEffect(() => {
    const timer = setTimeout(() => dismiss(toast.id), 4200);
    return () => clearTimeout(timer);
  }, [dismiss, toast.id]);

  const ok = toast.tone === "ok";
  const Icon = ok ? CircleCheckIcon : CircleAlertIcon;

  return (
    <motion.div
      role={ok ? "status" : "alert"}
      initial={reduce ? false : { opacity: 0, y: 12 }}
      animate={{ opacity: 1, y: 0 }}
      exit={reduce ? undefined : { opacity: 0, y: 8 }}
      className={`pointer-events-auto flex gap-3 rounded-2xl border bg-panel px-4 py-3 shadow-2xl backdrop-blur-md ${ok ? "border-gold/40" : "border-danger/50"}`}
    >
      <Icon className={`mt-0.5 size-4 shrink-0 ${ok ? "text-gold" : "text-danger"}`} aria-hidden />
      <div className="min-w-0 flex-1">
        <p className="text-sm font-medium text-ink">{ok ? "Listo" : "No se pudo"}</p>
        <p className="mt-0.5 text-sm text-quiet">{toast.text}</p>
        {toast.code ? <p className="mt-1 font-mono text-[11px] text-quiet">{toast.code}</p> : null}
      </div>
      <button type="button" className="text-quiet" aria-label="Cerrar aviso" onClick={onDismiss}>
        <XIcon className="size-4" />
      </button>
    </motion.div>
  );
}
