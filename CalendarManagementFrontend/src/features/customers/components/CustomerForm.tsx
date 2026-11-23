import { useState, type SubmitEvent } from "react";
import type { CreateCustomerRequest } from "../types/customer";
import "./CustomerForm.css";

interface CustomerFormProps {
  onSubmit: (request: CreateCustomerRequest) => Promise<void>;
}

export function CustomerForm({ onSubmit }: CustomerFormProps) {
  const [name, setName] = useState("");
  const [contactPerson, setContactPerson] = useState("");
  const [email, setEmail] = useState("");
  const [phone, setPhone] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [isSaving, setIsSaving] = useState(false);

   async function handleSubmit(event: SubmitEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!name.trim()) {
      setError("Namn måste fyllas i.");
      return;
    }

    setIsSaving(true);
    setError(null);

    try {
      await onSubmit({
        name,
        contactPerson: contactPerson || undefined,
        email: email || undefined,
        phone: phone || undefined,
      });

      setName("");
      setContactPerson("");
      setEmail("");
      setPhone("");
    } catch (err) {
      setError(err instanceof Error ? err.message : "Något gick fel");
    } finally {
      setIsSaving(false);
    }
  }
  
  return (
    <form onSubmit={handleSubmit} className="customer-form">
      <h2>Ny kund</h2>

      <div className="customer-field">
        <label htmlFor="customer-name">
          Namn <span className="customer-required">*</span>
        </label>
        <input
          id="customer-name"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
      </div>

      <div className="customer-field">
        <label htmlFor="customer-contact">Kontaktperson</label>
        <input
          id="customer-contact"
          value={contactPerson}
          onChange={(e) => setContactPerson(e.target.value)}
        />
      </div>

      <div className="customer-field">
        <label htmlFor="customer-email">E-post</label>
        <input
          id="customer-email"
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
      </div>

      <div className="customer-field">
        <label htmlFor="customer-phone">Telefon</label>
        <input
          id="customer-phone"
          type="tel"
          value={phone}
          onChange={(e) => setPhone(e.target.value)}
        />
      </div>

      {error && (
        <p className="customer-error" role="alert">
          {error}
        </p>
      )}

      <button type="submit" className="customer-submit" disabled={isSaving}>
        {isSaving ? "Sparar..." : "Spara kund"}
      </button>
    </form>
  );
}