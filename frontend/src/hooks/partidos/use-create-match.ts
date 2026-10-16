import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createMatch } from "@/services/partidos/create-match";

export function useCreateMatch() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: createMatch,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["matches"] }),
  });
}
