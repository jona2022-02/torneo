import { PageParams } from "@/services/http/types";

export type MatchSummary = {
  id: string;
  homeTeamId: string;
  homeTeamName: string;
  awayTeamId: string;
  awayTeamName: string;
  kickoff: string;
  status: string;
  homeScore: number | null;
  awayScore: number | null;
};

export type MatchGoal = {
  playerId: string;
  playerName: string;
  teamId: string;
  minute: number;
};

export type MatchDetail = MatchSummary & {
  goals: MatchGoal[];
};

export type MatchQuery = PageParams & {
  from?: string;
  to?: string;
  teamId?: string;
  status?: string;
};

export type NewMatch = {
  homeTeamId: string;
  awayTeamId: string;
  kickoff: string;
};

export type GoalInput = {
  playerId: string;
  minute: number;
};

export type MatchResult = {
  homeScore: number;
  awayScore: number;
  goals: GoalInput[];
};
