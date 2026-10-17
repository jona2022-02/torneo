"use client";

import { es } from "date-fns/locale/es";
import { forwardRef, ReactNode } from "react";
import { createPortal } from "react-dom";
import DatePicker, { registerLocale } from "react-datepicker";
import { parseLocal, toLocalValue } from "@/lib/format";
import "react-datepicker/dist/react-datepicker.css";
import "./date-time-field.css";

registerLocale("es", es);

function PopperBox({ children }: { children?: ReactNode }) {
  if (typeof document === "undefined") return children;
  return createPortal(children, document.body);
}

const PickerInput = forwardRef<HTMLInputElement, React.ComponentProps<"input">>(function PickerInput(props, ref) {
  return <input {...props} ref={ref} readOnly className="cursor-pointer" />;
});

export function DateTimeField({
  value,
  onChange,
  label,
  required,
}: {
  value: string;
  onChange: (value: string) => void;
  label: string;
  required?: boolean;
}) {
  return (
    <DatePicker
      customInput={<PickerInput />}
      selected={parseLocal(value)}
      onChange={(date: Date | null) => onChange(date ? toLocalValue(date) : "")}
      showTimeSelect
      timeIntervals={15}
      timeCaption="Hora"
      dateFormat="d MMM yyyy, h:mm a"
      locale="es"
      placeholderText="Elegir fecha y hora"
      required={required}
      aria-label={label}
      wrapperClassName="block w-full"
      calendarClassName="when-calendar"
      popperClassName="when-popper"
      popperPlacement="bottom-end"
      popperContainer={PopperBox}
    />
  );
}
