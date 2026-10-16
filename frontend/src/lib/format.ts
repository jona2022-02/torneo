import { tz } from "@date-fns/tz";
import { format, isValid, parse } from "date-fns";
import { es } from "date-fns/locale/es";

const salvador = tz("America/El_Salvador");
const shown = "d MMM yyyy, h:mm a";
const local = "yyyy-MM-dd'T'HH:mm";

export function formatWhen(value: string) {
  const date = new Date(value);
  if (!isValid(date)) return value;
  return format(date, shown, { locale: es, in: salvador });
}

export function parseLocal(value: string) {
  if (!value) return null;
  const date = parse(value, local, new Date());
  return isValid(date) ? date : null;
}

export function toLocalValue(date: Date) {
  return format(date, local);
}

export function toApiDate(value: string) {
  const date = parseLocal(value);
  return date ? date.toISOString() : undefined;
}
