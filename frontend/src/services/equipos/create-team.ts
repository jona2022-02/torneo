import { api } from "@/services/http/api";

export function createTeam(name: string) {
  return api<{ id: string }>("/api/teams", {
    method: "POST",
    body: JSON.stringify({ name }),
  });
}
