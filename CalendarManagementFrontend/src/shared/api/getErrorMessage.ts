// Läser felmeddelandet från backend.
// Vid 400 ValidationProblem ser svaret ut så här:
// { "errors": { "name": ["Namn måste fyllas i."] } }
export async function getErrorMessage(response: Response, fallback: string): Promise<string> {
  try {
    const body = await response.json();

    if (body?.errors) {
      const messages = Object.values(body.errors as Record<string, string[]>).flat();
      if (messages.length > 0) {
        return messages.join(" ");
      }
    }

    return body?.detail ?? body?.title ?? fallback;
  } catch {
    return fallback; // svaret var inte JSON, till exempel 404 utan innehåll
  }
}