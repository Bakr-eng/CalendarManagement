export type ChecklistStatus =
  | "NotStarted"
  | "InProgress"
  | "Complete";

export function getChecklistStatus(
  totalItems: number,
  completedItems: number
) {
  if (totalItems === 0) {
    return {
      status: "NotStarted" as ChecklistStatus,
      text: "Ej påbörjad",
      color: "#dc3545",
    };
  }

  if (completedItems === totalItems) {
    return {
      status: "Complete" as ChecklistStatus,
      text: "Klar",
      color: "#28a745",
    };
  }

  return {
    status: "InProgress" as ChecklistStatus,
    text: "Pågående",
    color: "#ffc107",
  };
}