import { api } from "@/services/http/api";
import { Page, toQuery } from "@/services/http/types";
import { Standing, StandingQuery } from "@/services/tabla/standing.types";

export function getStandings(query: StandingQuery) {
  return api<Page<Standing>>(`/api/standings${toQuery(query)}`);
}
