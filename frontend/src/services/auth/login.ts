import { api } from "@/services/http/api";
import type { Session } from "@/services/auth/session";

export function login(username: string, password: string) {
  return api<Session>("/api/auth/login", {
    method: "POST",
    body: JSON.stringify({ username, password }),
  });
}
