import { api } from "@/services/http/api";
import { MatchResult } from "@/services/partidos/match.types";

export function registerResult(matchId: string, result: MatchResult) {
  return api<null>(`/api/matches/${matchId}/result`, {
    method: "PUT",
    body: JSON.stringify(result),
  });
}
