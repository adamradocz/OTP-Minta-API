# OTP Minta API

## Követelmények

- [Visual Studio 2019](https://visualstudio.microsoft.com/vs/)
- API: [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet/current)
- Windows Forms: [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472)

## Struktúra

| Projekt | Infó |
| :--- | :--- |
| `Otp.API` | ASP.NET Core Web API |
| `Otp.API.Tests` | ASP.NET Core Web API tesztek |
| `Otp.WindowsForms` | Windows Forms Kliens applikáció |

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
