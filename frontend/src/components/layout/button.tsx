import { ButtonHTMLAttributes } from "react";
import { Button as UiButton } from "@/components/ui/button";

type Props = ButtonHTMLAttributes<HTMLButtonElement> & {
  tone?: "primary" | "ghost";
};

export function Button({ tone = "primary", className, ...props }: Props) {
  return <UiButton variant={tone === "ghost" ? "outline" : "default"} className={className} {...props} />;
}
