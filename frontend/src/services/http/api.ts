type ErrorBody = { code?: string; message?: string };

export class ApiError extends Error {
  readonly code: string;
  readonly status: number;

  constructor(code: string, message: string, status: number) {
    super(message);
    this.name = "ApiError";
    this.code = code;
    this.status = status;
  }
}

export function isApiError(error: unknown): error is ApiError {
  return error instanceof ApiError;
}

const baseUrl = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:8080";

export async function api<T>(path: string, options: RequestInit = {}): Promise<T> {
  const headers = new Headers(options.headers);
  if (options.body) headers.set("Content-Type", "application/json");
  if ((options.method ?? "GET").toUpperCase() === "POST") {
    headers.set("Idempotency-Key", crypto.randomUUID());
  }

  const response = await fetch(`${baseUrl}${path}`, { ...options, headers });
  if (response.status === 204) return undefined as T;

  const text = await response.text();
  const payload = parse(text);
  if (!response.ok) {
    throw new ApiError(payload?.code ?? "error", payload?.message ?? "No se pudo completar la operación.", response.status);
  }

  return payload as T;
}

function parse(text: string): ErrorBody | null {
  if (!text) return null;
  try {
    return JSON.parse(text) as ErrorBody;
  } catch {
    return null;
  }
}
