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



## 📂 Projekt felépítése
## 📦 WebshopWeb/
├── 📂 Controllers/ │ ├── 📝 ProductController.cs
│ ├── 📝 HomeController.cs
│ └── 📝 AuthenticationController.cs
├── 📂 Data/
│ └── 📝 products.json
├── 📂 Views/
│ └── 📂 Authentication/
│ ├── 📝 Login.cshtml
│ └── 📝 Register.cshtml
├── 📂 wwwroot/
│ ├── 📂 css/
│ ├── 📂 js/
│ ├── 📂 images/
│ └── 📂 templates/
└── 📝 README.md

## 📦 Webshop.EntityFramework/
├── 📂 Data/
│   ├── 📝 UserData.cs
│   ├── 📝 Review.cs
│   ├── 📝 Products.cs
│   ├── 📝 Address.cs
│   ├── 📝 OrderItem.cs
│   ├── 📝 CartItem.cs
│   ├── 📝 Orders.cs
│   └── 📝 ShoppingCart.cs
├── 📂 Managers/
│   ├── 📂 Carts/
│   │   ├── 📝 CartManager.cs
│   │   ├── 📝 CartRepository.cs
│   │   ├── 📝 ICartManager.cs
│   │   ├── 📝 ICartRemover.cs
│   │   ├── 📝 ICartRepository.cs
│   │   └── 📝 IGetCart.cs
│   ├── 📂 Order/
│   │   ├── 📝 IOrderManager.cs
│   │   ├── 📝 IOrderReader.cs
│   │   ├── 📝 IOrderRemover.cs
│   │   ├── 📝 IOrderRepository.cs
│   │   ├── 📝 OrderManager.cs
│   │   └── 📝 OrderRepository.cs
│   ├── 📂 Product/
│   │   ├── 📝 IProductManager.cs
│   │   ├── 📝 IProductReader.cs
│   │   ├── 📝 IProductRepository.cs
│   │   ├── 📝 ProductManager.cs
│   │   └── 📝 ProductRepository.cs
│   ├── 📂 Reviews/
│   │   ├── 📝 IReviewManager.cs
│   │   ├── 📝 IReviewRespository.cs
│   │   ├── 📝 ReviewManager.cs
│   │   └── 📝 ReviewRepository.cs
│   └── 📂 User/
│       ├── 📝 IUserEditor.cs
│       ├── 📝 IUserManager.cs
│       ├── 📝 IUserReader.cs
│       ├── 📝 IUserRemover.cs
│       ├── 📝 IUserRepository.cs
│       ├── 📝 UserManager.cs
│       └── 📝 UserRepository.cs
└── 📝 GlobalDbContext.cs

## 📦 Webshop.Services/
├── 📂 Interfaces/
│   ├── 📝 IAuthenticationManager.cs
│   ├── 📝 ICompatibilityService.cs
│   ├── 📝 IEmailService.cs
│   ├── 📝 IEncryptManager.cs
│   ├── 📝 IOrderServices.cs
│   ├── 📝 IProductServices.cs
│   └── 📝 IValidationManager.cs
├── 📂 Services/
│   ├── 📂 Authentication/
│   │   └── 📝 AuthenticationService.cs
│   ├── 📂 Compatibility/
│   │   └── 📝 CompatibilityService.cs
│   ├── 📂 Email/
│   │   └── 📝 EmailService.cs
│   ├── 📂 Order/
│   │   └── 📝 OrderServices.cs
│   ├── 📂 ProductService/
│   │   └── 📝 ProductService.cs
│   ├── 📂 Security/
│   │   └── 📝 SHA256Encrypter.cs
│   └── 📂 Validators/
│       ├── 📝 EmailValidator.cs
│       ├── 📝 PasswordValidator.cs
│       └── 📝 UsernameValidator.cs
└── 📂 ViewModel/
    ├── 📝 ForgotPasswordViewModel.cs
    ├── 📝 OrderSummaryViewModel.cs
    ├── 📝 PasswordChangeViewModel.cs
    ├── 📝 PcBuilderViewModel.cs
    ├── 📝 PersonalDataViewModel.cs
    └── 📝 ProductFilterViewModel.cs

## 📦 Webshop.UnitTests/
├── 📝 AuthenticatorServiceTests.cs
├── 📝 CartManagerTests.cs
└── 📝 CompatibilityServiceTests.cs


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