export type PageInfo = {
  pageNumber: number;
  pageSize: number;
  totalRecords: number;
  totalPages: number;
};

export type Page<T> = PageInfo & {
  data: T[];
};
