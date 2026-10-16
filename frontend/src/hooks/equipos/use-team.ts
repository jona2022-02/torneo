import { useQuery } from "@tanstack/react-query";
import { getTeam } from "@/services/equipos/get-team";

export function useTeam(id: string) {
  return useQuery({
    queryKey: ["teams", "detail", id],
    queryFn: () => getTeam(id),
    enabled: Boolean(id),
  });
}
