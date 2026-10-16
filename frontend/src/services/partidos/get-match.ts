import { api } from "@/services/http/api";
import { MatchDetail } from "@/services/partidos/match.types";

export function getMatch(id: string) {
  return api<MatchDetail>(`/api/matches/${id}`);
}
