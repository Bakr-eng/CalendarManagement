
export interface User {
  id: number;
  name: string;
}

export async function getUsers(): Promise<User[]> {
  const response = await fetch("/api/users");

  if (!response.ok) {
    throw new Error("Failed to fetch users");
  }

  return response.json();
}
