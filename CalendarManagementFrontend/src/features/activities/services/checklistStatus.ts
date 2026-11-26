export function getChecklistStatus(
  totalItems: number,
  completedItems: number
) {
  if (totalItems === 0) {
    return {
      text: "Ej påbörjad",
      color: "#dc3545",
    };
  }

  if (completedItems === totalItems) {
    return {
      text: "Klar",
      color: "#28a745",
    };
  }

  return {
    text: "Pågående",
    color: "#ffc107",
  };
}