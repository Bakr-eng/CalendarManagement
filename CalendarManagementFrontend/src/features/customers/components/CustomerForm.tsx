import { useState, type SubmitEvent } from "react";
import {
  customerTypeLabels,
  identityNumberLabels,
  type CreateCustomerRequest,
  type Customer,
  type CustomerType,
} from "../types/customer";
import "../../../styles/form.css";

interface CustomerFormProps {
  onSubmit: (request: CreateCustomerRequest) => Promise<void>;
  initialCustomer?: Customer; // finns = redigera, saknas = ny kund
  onCancel?: () => void;
}

// Enkel kontroll av formatet innan vi skickar till backend.
// Backend kontrollerar också (t.ex. att datumet finns).
const identityNumberPatterns: Record<CustomerType, RegExp> = {
  Company: /^\d{6}-?\d{4}$/, // 556012-5790
  Private: /^\d{8}-?\d{4}$/, // 19121212-1212
};

const identityNumberPlaceholders: Record<CustomerType, string> = {
  Company: "NNNNNN-NNNN",
  Private: "ÅÅÅÅMMDD-XXXX",
};

export function CustomerForm({ onSubmit, initialCustomer, onCancel }: CustomerFormProps) {
  const isEditing = initialCustomer !== undefined;

  const [name, setName] = useState(initialCustomer?.name ?? "");
  const [customerType, setCustomerType] = useState<CustomerType>(
    initialCustomer?.customerType ?? "Company",
  );
  const [identityNumber, setIdentityNumber] = useState(initialCustomer?.identityNumber ?? "");
  const [contactPerson, setContactPerson] = useState(initialCustomer?.contactPerson ?? "");
  const [email, setEmail] = useState(initialCustomer?.email ?? "");
  const [phone, setPhone] = useState(initialCustomer?.phone ?? "");
  const [error, setError] = useState<string | null>(null);
  const [isSaving, setIsSaving] = useState(false);

  function handleCustomerTypeChange(newType: CustomerType) {
    setCustomerType(newType);
    // Ett organisationsnummer passar inte som personnummer och tvärtom
    setIdentityNumber("");
  }

  async function handleSubmit(event: SubmitEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!name.trim()) {
      setError("Namn måste fyllas i.");
      return;
    }

    const trimmedNumber = identityNumber.trim();
    if (trimmedNumber && !identityNumberPatterns[customerType].test(trimmedNumber)) {
      setError(
        `${identityNumberLabels[customerType]} ska skrivas ${identityNumberPlaceholders[customerType]}.`,
      );
      return;
    }

    setIsSaving(true);
    setError(null);

    try {
      await onSubmit({
        name,
        customerType,
        identityNumber: trimmedNumber || undefined,
        contactPerson: contactPerson || undefined,
        email: email || undefined,
        phone: phone || undefined,
      });

      // Töm bara fälten när man skapar en ny kund.
      // Vid redigering stänger sidan formuläret istället.
      if (!isEditing) {
        setName("");
        setCustomerType("Company");
        setIdentityNumber("");
        setContactPerson("");
        setEmail("");
        setPhone("");
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : "Något gick fel");
    } finally {
      setIsSaving(false);
    }
  }

  return (
    <form onSubmit={handleSubmit} className="form">
      <h2>{isEditing ? "Redigera kund" : "Ny kund"}</h2>

      <div className="form-field">
        <label htmlFor="customer-name">
          Namn <span className="form-required">*</span>
        </label>
        <input
          id="customer-name"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
      </div>

      <div className="form-field">
        <label htmlFor="customer-type">
          Kundtyp <span className="form-required">*</span>
        </label>
        <select
          id="customer-type"
          value={customerType}
          onChange={(e) => handleCustomerTypeChange(e.target.value as CustomerType)}
        >
          <option value="Company">{customerTypeLabels.Company}</option>
          <option value="Private">{customerTypeLabels.Private}</option>
        </select>
      </div>

      <div className="form-field">
        {/* Rubriken byts beroende på kundtyp */}
        <label htmlFor="customer-identity-number">{identityNumberLabels[customerType]}</label>
        <input
          id="customer-identity-number"
          value={identityNumber}
          placeholder={identityNumberPlaceholders[customerType]}
          inputMode="numeric"
          autoComplete="off"
          onChange={(e) => setIdentityNumber(e.target.value)}
        />
      </div>

      <div className="form-field">
        <label htmlFor="customer-contact">Kontaktperson</label>
        <input
          id="customer-contact"
          value={contactPerson}
          onChange={(e) => setContactPerson(e.target.value)}
        />
      </div>

      <div className="form-field">
        <label htmlFor="customer-email">E-post</label>
        <input
          id="customer-email"
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
      </div>

      <div className="form-field">
        <label htmlFor="customer-phone">Telefon</label>
        <input
          id="customer-phone"
          type="tel"
          value={phone}
          onChange={(e) => setPhone(e.target.value)}
        />
      </div>

      {error && (
        <p className="form-error" role="alert">
          {error}
        </p>
      )}

      <div className="form-actions">
        <button type="submit" className="form-submit" disabled={isSaving}>
          {isSaving ? "Sparar..." : isEditing ? "Spara ändringar" : "Spara kund"}
        </button>

        {onCancel && (
          <button type="button" className="form-cancel" onClick={onCancel}>
            Avbryt
          </button>
        )}
      </div>
    </form>
  );
}