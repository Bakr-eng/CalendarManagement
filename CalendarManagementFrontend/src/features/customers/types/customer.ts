// samma form som customreDto-------------
export interface Customer {
  id: number;
  name: string;
  contactPerson: string | null;
  email: string | null;
  phone: string | null;
}

export interface CreateCustomerRequest {
  name: string;
  contactPerson?: string;
  email?: string;
  phone?: string;
}