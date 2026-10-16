import { api } from "@/services/http/api";
import { Team } from "@/services/equipos/team.types";

export function getTeam(id: string) {
  return api<Team>(`/api/teams/${id}`);
}
