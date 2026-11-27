using System.Globalization;

namespace CalendarManagement.Application.Validation;

// Kontrollerar och formaterar svenska personnummer och organisationsnummer.
// Metoderna returnerar det formaterade numret, eller null om numret är ogiltigt.
public static class IdentityNumberValidator
{
    // Personnummer: 12 siffror (ÅÅÅÅMMDDXXXX), med eller utan bindestreck.
    // Returnerar t.ex. "19121212-1212"
    public static string? NormalizePersonalNumber(string input)
    {
        var digits = DigitsOnly(input);
        if (digits is null || digits.Length != 12)
        {
            return null;
        }

        // De första 8 siffrorna måste vara ett riktigt datum som inte ligger i framtiden
        var isDate = DateTime.TryParseExact(
            digits[..8], "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var birthDate);

        if (!isDate || birthDate > DateTime.Today)
        {
            return null;
        }

        return $"{digits[..8]}-{digits[8..]}";
    }

    // Organisationsnummer: 10 siffror (NNNNNNXXXX), med eller utan bindestreck.
    // Returnerar t.ex. "556012-5790"
    public static string? NormalizeOrganisationNumber(string input)
    {
        var digits = DigitsOnly(input);
        if (digits is null || digits.Length != 10)
        {
            return null;
        }

        // Tredje siffran är alltid 2 eller högre i ett organisationsnummer
        if (digits[2] < '2')
        {
            return null;
        }


        return $"{digits[..6]}-{digits[6..]}";
    }

    // "19121212-1212" -> "19121212-****"
    public static string MaskPersonalNumber(string personalNumber) =>
        personalNumber.Length > 9 ? personalNumber[..9] + "****" : personalNumber;

    // Tar bort bindestreck och mellanslag. Returnerar null om något annat än siffror finns kvar.
    private static string? DigitsOnly(string input)
    {
        var cleaned = input.Replace("-", "").Replace(" ", "");
        return cleaned.Length > 0 && cleaned.All(char.IsAsciiDigit) ? cleaned : null;
    }

   
}