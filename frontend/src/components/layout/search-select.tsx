"use client";

import {
  Combobox,
  ComboboxContent,
  ComboboxEmpty,
  ComboboxInput,
  ComboboxItem,
  ComboboxList,
} from "@/components/ui/combobox";

export type SearchOption = {
  value: string;
  label: string;
};

export function SearchSelect({
  value,
  onChange,
  options,
  label,
  placeholder,
  emptyLabel,
}: {
  value: string;
  onChange: (value: string) => void;
  options: SearchOption[];
  label: string;
  placeholder: string;
  emptyLabel?: string;
}) {
  const items = emptyLabel ? [{ value: "", label: emptyLabel }, ...options] : options;
  const selected = items.find((item) => item.value === value) ?? null;

  return (
    <Combobox
      items={items}
      value={selected}
      onValueChange={(item) => onChange(item?.value ?? "")}
      isItemEqualToValue={(left, right) => left.value === right.value}
    >
      <ComboboxInput aria-label={label} placeholder={placeholder} className="w-full" />
      <ComboboxContent>
        <ComboboxEmpty>Sin resultados.</ComboboxEmpty>
        <ComboboxList>
          {(item: SearchOption) => (
            <ComboboxItem key={item.value || "empty"} value={item}>
              {item.label}
            </ComboboxItem>
          )}
        </ComboboxList>
      </ComboboxContent>
    </Combobox>
  );
}
