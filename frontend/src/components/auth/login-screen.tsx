"use client";

import { FormEvent, useState } from "react";
import { useRouter } from "next/navigation";
import { Button } from "@/components/layout/button";
import { Field } from "@/components/layout/field";
import { login } from "@/services/auth/login";
import { writeSession } from "@/services/auth/session";
import { isApiError } from "@/services/http/api";
import { useSessionStore } from "@/stores/use-session-store";

export function LoginScreen() {
  const router = useRouter();
  const setSession = useSessionStore((state) => state.setSession);
  const [username, setUsername] = useState("admin");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [pending, setPending] = useState(false);

  async function submit(event: FormEvent) {
    event.preventDefault();
    setPending(true);
    setError(null);
    try {
      const session = await login(username, password);
      writeSession(session);
      setSession(session);
      router.replace("/equipos");
    } catch (caught) {
      setError(isApiError(caught) ? caught.message : "No se pudo entrar.");
    } finally {
      setPending(false);
    }
  }

  return (
    <main className="flex min-h-screen items-center justify-center px-4">
      <form onSubmit={submit} className="board w-full max-w-md p-8">
        <p className="text-[11px] font-semibold uppercase tracking-[0.22em] text-gold">Mundialito</p>
        <h1 className="mt-2 font-display text-4xl">Entrar</h1>
        <p className="mt-2 text-sm text-quiet">Operador del torneo. Usuario admin, contraseña TorneoDev_2026!</p>
        <div className="mt-6 grid gap-4">
          <Field label="Usuario">
            <input value={username} onChange={(event) => setUsername(event.target.value)} autoComplete="username" required />
          </Field>
          <Field label="Contraseña">
            <input value={password} onChange={(event) => setPassword(event.target.value)} type="password" autoComplete="current-password" required />
          </Field>
          {error ? <p className="text-sm text-danger">{error}</p> : null}
          <Button type="submit" disabled={pending}>{pending ? "Entrando…" : "Entrar"}</Button>
        </div>
      </form>
    </main>
  );
}
