// Samma värden som enumen CustomerType i backend
export type CustomerType = "Company" | "Private";

// Svenska texter att visa i appen
export const customerTypeLabels: Record<CustomerType, string> = {
  Company: "Företag",
  Private: "Privatperson",
};

// Olika rubrik beroende på kundtyp
export const identityNumberLabels: Record<CustomerType, string> = {
  Company: "Organisationsnummer",
  Private: "Personnummer",
};

// Samma form som CustomerDto
export interface Customer {
  id: number;
  name: string;
  contactPerson: string | null;
  email: string | null;
  phone: string | null;
  customerType: CustomerType;
  // I listan är personnummer maskerat (19121212-****), i detaljerna visas hela
  identityNumber: string | null;
}

export interface CreateCustomerRequest {
  name: string;
  contactPerson?: string;
  email?: string;
  phone?: string;
  customerType: CustomerType;
  identityNumber?: string;
}

// Samma fält som när man skapar en kund
export type UpdateCustomerRequest = CreateCustomerRequest;