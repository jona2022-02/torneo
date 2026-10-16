import { api } from "@/services/http/api";
import { Page, toQuery } from "@/services/http/types";
import { MatchQuery, MatchSummary } from "@/services/partidos/match.types";

export function getMatches(query: MatchQuery) {
  return api<Page<MatchSummary>>(`/api/matches${toQuery(query)}`);
}
