# 🛒 DXMarket – Webáruház és PC Konfigurátor

A **DXMarket** egy modern, reszponzív tech webáruház, amely lehetővé teszi számítástechnikai eszközök böngészését, értékelését és megvásárlását. A projekt különlegessége egy **valós idejű PC konfigurátor modul**, amely a kiválasztott alkatrészek alapján automatikusan szűri a kompatibilis komponenseket.

## 📌 Funkciók

- 🔍 Termékkategóriák böngészése és keresés
- 📦 Kosárkezelés (hozzáadás, törlés)
- ⭐ Vásárlói értékelések és vélemények
- 🔐 Felhasználói regisztráció és bejelentkezés (Email + Google OAuth + GitHub OAuth)
- 🖥️ PC Builder modul (valós idejű szűréssel)
- 🌦️ Időjárás-előrejelzés funkció (külső API-ból)
- 📱 Reszponzív dizájn mobilra és asztali nézetre

## 🛠️ Technológiai háttér

| Terület           | Technológia                                |
|-------------------|--------------------------------------------|
| **Frontend**      | HTML, CSS, JavaScript                      |
| **Backend**       | ASP.NET Core MVC + REST API                |
| **Adatbázis**     | Microsoft SQL Server + Entity Framework    |
| **Autentikáció**  | ASP.NET Identity + Google OAuth            |

## ⚙️ Telepítés (Fejlesztői környezet)

1. **Adatbázis létrehozása**
   - SQL Server-ben hozd létre az adatbázist (pl. `DXMarketDB`)
   - Futtasd az `update-database` parancsot a `Package Manager Console`-ban

2. **Backend futtatása**
   ```bash
   dotnet run
Frontend megnyitása

Nyisd meg index.html fájlt a böngészőben


## 📂 Projekt felépítése
DXMarket/
├── Controllers/
│   └── ProductController.cs
├── Models/
│   └── Product.cs, UserData.cs, etc.
├── Views/
│   └── Terméklista, Kosár, Részletek, stb.
├── wwwroot/
│   └── css/, js/, images/
└── README.md

## 💡 Kiemelt modul: PC Konfigurátor
A konfigurátor logikája:

Az első kiválasztott alkatrész (pl. CPU) alapján a többi legördülő menü dinamikusan szűrődik.

Csak kompatibilis alkatrészek jelennek meg (pl. CPU foglalat alapján szűrt alaplapok).

A kiválasztott elemeket az API mindig figyelembe veszi.

## 🧪 Tesztelés

- A projekt **localhoston** futtatható Visual Studio segítségével.
- Tesztfelhasználók és admin elérhetőség megadható `appsettings.json` fájlban vagy seed adatként.


## 🎓 Készült a 2025-ös szoftvertechnikus vizsgára
Fejlesztők: Kovács Dániel, Mester Xavér, Adorján Marcell
Iskola: Vak Bottyán János Katolikus Műszaki és Közgazdasági Technikum

## 📜 Licenc
Ez a projekt oktatási célokra készült. Minden jog fenntartva.