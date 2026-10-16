import { useQueryClient } from "@tanstack/react-query";
import { getMatch } from "@/services/partidos/get-match";

export function useMatchPrefetch() {
  const queryClient = useQueryClient();
  return (id: string) => {
    void queryClient.prefetchQuery({
      queryKey: ["matches", "detail", id],
      queryFn: () => getMatch(id),
      staleTime: 30_000,
    });
  };
}
