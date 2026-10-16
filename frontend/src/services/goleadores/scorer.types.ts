import { PageParams } from "@/services/http/types";

export type Scorer = {
  playerId: string;
  playerName: string;
  teamId: string;
  teamName: string;
  goals: number;
};

export type ScorerQuery = PageParams & {
  teamId?: string;
};
