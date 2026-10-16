import { api } from "@/services/http/api";
import { Page, toQuery } from "@/services/http/types";
import { Team, TeamQuery } from "@/services/equipos/team.types";

export function getTeams(query: TeamQuery) {
  return api<Page<Team>>(`/api/teams${toQuery(query)}`);
}
