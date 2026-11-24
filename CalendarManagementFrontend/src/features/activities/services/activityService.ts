// Samma form som ActivityDto i backend
export interface Activity {
  id: number;
  date: string;
  type: string;
  responsiblePersonId: number | null;
  responsiblePersonName: string | null;
  customerId: number | null;
  customerName: string | null;
  locationId: number | null;
  locationName: string | null;
  locationCity: string | null;
}

// Samma form som CreateActivityRequest i backend
export interface CreateActivityRequest {
  date: string;
  type: string;
  responsiblePersonId: number | null;
  customerId: number | null;
  locationId: number | null;
}

// Samma form som ActivityFilter i backend – alla fält är frivilliga
export interface ActivityFilter {
  search?: string;
  customerId?: number | null;
  locationId?: number | null;
  responsiblePersonId?: number | null;
}

export async function getActivities(filter: ActivityFilter = {}): Promise<Activity[]> {
  // Bygg t.ex. ?search=live&customerId=2 – bara de filter som har ett värde
  const params = new URLSearchParams();

  if (filter.search?.trim()) params.set("search", filter.search.trim());
  if (filter.customerId) params.set("customerId", String(filter.customerId));
  if (filter.locationId) params.set("locationId", String(filter.locationId));
  if (filter.responsiblePersonId) {
    params.set("responsiblePersonId", String(filter.responsiblePersonId));
  }

  const query = params.toString();
  const response = await fetch(query ? `/api/activities?${query}` : "/api/activities");

  if (!response.ok) {
    throw new Error("Kunde inte hämta aktiviteter");
  }

  return response.json();
}

export async function createActivity(activity: CreateActivityRequest): Promise<Activity> {
  const response = await fetch("/api/activities", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(activity),
  });

  if (!response.ok) {
    throw new Error("Kunde inte skapa aktiviteten");
  }

  return response.json();
}

// "Scandinavium, Göteborg · Live Nation" – det som visas under typen
export function formatActivityDetails(activity: Activity): string {
  const parts: string[] = [];

  if (activity.locationName) {
    parts.push(
      activity.locationCity
        ? `${activity.locationName}, ${activity.locationCity}`
        : activity.locationName
    );
  }

  if (activity.customerName) {
    parts.push(activity.customerName);
  }

  return parts.join(" · ");
}