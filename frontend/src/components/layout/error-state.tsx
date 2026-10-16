import { isApiError } from "@/services/http/api";

export function ErrorState({ error }: { error: unknown }) {
  const code = isApiError(error) ? error.code : "error";
  const message = error instanceof Error ? error.message : "No se pudo cargar la información.";
  return (
    <div className="rounded-2xl border border-danger/30 bg-danger/10 px-4 py-3">
      <p className="font-mono text-xs text-danger">{code}</p>
      <p className="mt-1 text-sm">{message}</p>
    </div>
  );
}
