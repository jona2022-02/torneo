import { api } from "@/services/http/api";
import { Page, toQuery } from "@/services/http/types";
import { Scorer, ScorerQuery } from "@/services/goleadores/scorer.types";

export function getScorers(query: ScorerQuery) {
  return api<Page<Scorer>>(`/api/scorers${toQuery(query)}`);
}
