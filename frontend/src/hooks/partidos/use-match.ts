import { useQuery } from "@tanstack/react-query";
import { getMatch } from "@/services/partidos/get-match";

export function useMatch(id: string) {
  return useQuery({
    queryKey: ["matches", "detail", id],
    queryFn: () => getMatch(id),
    enabled: Boolean(id),
  });
}
