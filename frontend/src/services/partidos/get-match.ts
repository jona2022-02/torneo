import { api } from "@/services/http/api";
import { Page } from "@/services/http/page";
import { MatchDetail } from "@/services/partidos/match.types";

export async function getMatch(id: string) {
  const page = await api<Page<MatchDetail>>(`/api/matches/${id}`);
  return page.data[0];
}
