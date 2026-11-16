import { useState, type FormEvent } from "react";
import type { CreateCustomerRequest } from "../types/customer";

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

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
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
    <form onSubmit={handleSubmit}>
      <h2>Ny kund</h2>

      <label>
        Namn *
        <input value={name} onChange={(e) => setName(e.target.value)} />
      </label>

      <label>
        Kontaktperson
        <input value={contactPerson} onChange={(e) => setContactPerson(e.target.value)} />
      </label>

      <label>
        E-post
        <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} />
      </label>

      <label>
        Telefon
        <input value={phone} onChange={(e) => setPhone(e.target.value)} />
      </label>

      {error && <p role="alert">{error}</p>}

      <button type="submit" disabled={isSaving}>
        {isSaving ? "Sparar..." : "Spara kund"}
      </button>
    </form>
  );
}