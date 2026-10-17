import { api } from "@/services/http/api";
import { Page } from "@/services/http/page";
import { Team } from "@/services/equipos/team.types";

export async function getTeam(id: string) {
  const page = await api<Page<Team>>(`/api/teams/${id}`);
  return page.data[0];
}
