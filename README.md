# CalendarManagement

Kalender- och aktivitetsapp för Mankan. Backend i ASP.NET Core (Clean Architecture, Minimal API), frontend i React + TypeScript.

## Krav

- .NET 10 SDK
- SQL Server (Express eller LocalDB)
- Node.js 22
- Visual Studio 2026 (backend) och VS Code (frontend)

## Kom igång – backend

### 1. Connection string (User Secrets)

Connection stringen ligger **inte** i `appsettings.json`, eftersom varje utvecklare har sin egen SQL Server. Den sparas i User Secrets på din egen dator och pushas aldrig till Git.

Högerklicka på **CalendarManagement.API → Manage User Secrets** och lägg in (byt `Server` mot din egen):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLExpress;Database=CalendarManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Eller i terminalen, från mappen `CalendarManagementBackend/src/CalendarManagement.API`:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.\SQLExpress;Database=CalendarManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

### 2. Skapa databasen

Package Manager Console, med **Default project: CalendarManagement.Infrastructure**:

```
Update-Database
```

Kör det igen varje gång du har pullat nya migrations.

### 3. Starta API:t

Sätt **CalendarManagement.API** som startprojekt och tryck F5. API:t körs på `http://localhost:5196`.

## Kom igång – frontend

```bash
cd CalendarManagementFrontend
npm install
npm run dev
```

Öppna `http://localhost:5173`. Anrop till `/api` skickas vidare till backend via Vite-proxyn i `vite.config.ts`.

## Konfiguration

| Inställning | Var | Beskrivning |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | User Secrets | Anslutning till SQL Server |
| `Cors:AllowedOrigins` | `appsettings.json` | Adresser som får anropa API:t |

## Tester

```bash
cd CalendarManagementBackend
dotnet test CalendarManagementBackend.slnx
```

## Arbetsflöde

- `main` är skyddad. Skapa en branch per uppgift och gör en pull request.
- GitHub Actions kör build, test och lint på varje pull request.
- Kör `npm run lint` och `npm run build` innan du pushar frontend.