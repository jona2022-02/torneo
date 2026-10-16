import { api } from "@/services/http/api";
import { NewMatch } from "@/services/partidos/match.types";

export function createMatch(match: NewMatch) {
  return api<{ id: string }>("/api/matches", {
    method: "POST",
    body: JSON.stringify(match),
  });
}
