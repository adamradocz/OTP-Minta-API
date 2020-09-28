# OTP Minta API

## Változások

- `Otp.WindowsForms` .NET Core-t használ, ezért nincs szükség 3rd party JSON libraray-ra, plusz képes a fájlok méretének lekérdezésére.
- `Otp.WindowsForms` MVP pattern-t használ, így ha szükséges az üzleti logika tesztelhető.
- `DokumentumokController` tesztek hozzáadva.
- `DokumentumokService` tesztek párhuzamos futtatásra optimalizálva.
- `FileProcessor` .NET Standard Library hozzáadva, hogy a kódolás könnyen cserélhető legyen más formátumra.
- `CancellationToken` használatának a lehetősége az API oldalon, dokumentumok feltöltése és letöltése esetén.

## Követelmények

- [Visual Studio 2019](https://visualstudio.microsoft.com/vs/)
- [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet/current)

## Struktúra

| Projekt | Infó |
| :--- | :--- |
| `FileProcessor` | .NET Standard 2.1 Library. Közös kód a fájlok kódolására.|
| `Otp.API` | ASP.NET Core Web API |
| `Otp.API.Tests` | ASP.NET Core Web API tesztek |
| `Otp.WindowsForms` | .NET Core Windows Forms Kliens applikáció |

## Konfiguráció

Az alapértelmezett "Dokumentumok" mappa: `c:\Temp\`
Az `Otp.API` konfigurációs fájlként az `appsettings.json` fájlt használja. A `DokumentumokConfiguration` szekcióban állítható be a működési mappa.
A konfigurációs fájl olvasásához [Options pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-3.1) van használva.

## Működés

### API

| Infó | Végpont |
| :--- | :--- |
| Fájlok listázása | GET http://localhost:80/api/dokumentumok/ |
| Fájl letöltése | GET http://localhost:80/api/dokumentumok/{fájlnév} |
| Fájlméret letöltése | HEAD http://localhost:80/api/dokumentumok/{fájlnév} |
| Fájl feltöltése | POST http://localhost:80/api/dokumentumok/{fájlnév} |

Támogatott formátumok:
- JSON
- XML

### WindowsForms Kliens

A Kliens hagyja az érvénytelen inputot is megadni.