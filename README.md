# Herb Project

**Herb Project** adalah proyek game **2D Medieval Herbalist & Apothecary Shop Simulation** yang dibuat menggunakan **Unity dan C#**.

Dalam game ini, pemain mengelola sebuah toko herbal/apotek dengan menerima pelanggan, memproses pesanan, mengelola inventory, serta menggunakan data resep, bahan, dan pelanggan untuk menjalankan aktivitas toko.

Project ini dibuat sebagai latihan untuk membangun sistem gameplay yang **data-driven, modular, dan efisien**, dengan memanfaatkan **ScriptableObject, Unity Addressables, event-driven communication, inventory system, dan optimized database lookup**.

> **Fokus utama proyek:** Membangun sistem simulasi toko yang terstruktur dengan pemisahan antara data, gameplay logic, UI, dan asset management.

---

##  Fitur Utama

*  Sistem toko herbal / apotek
*  Sistem customer spawning
*  Sistem customer order
*  Customer waiting & countdown system
*  Customer reaction ketika waktu tunggu habis
*  Sistem resep dan bahan
*  Inventory dengan slot dan stack system
*  Item stacking berdasarkan jenis item
*  Database berbasis ScriptableObject
*  Optimized dictionary lookup
*  Unity Addressables untuk asset pelanggan
*  Sistem waktu operasional toko
*  Feedback audio berdasarkan event gameplay
*  UI management
*  Event-based communication antar sistem

---

##  Konsep yang Dipelajari

Project ini berfokus pada bagaimana membuat sistem game yang tidak hanya berfungsi, tetapi juga memiliki struktur yang **mudah dikembangkan dan dipelihara**.

Beberapa konsep utama yang diterapkan:

* Object-Oriented Programming
* Separation of Concerns
* ScriptableObject
* Data-Driven Design
* Event-Driven Programming
* Dictionary & Hash Lookup
* Inventory System
* Addressables
* Asynchronous Asset Loading
* Coroutine
* Runtime Data Management
* UI Synchronization

---

##  Customer System

Salah satu sistem utama dalam project ini adalah **Customer System**.

Customer dapat muncul secara otomatis melalui sistem spawner dan memiliki data yang berasal dari database.

Alur sederhananya:

```text
┌──────────────────┐
│ CustomerSpawner  │
└────────┬─────────┘
         │
         │ Spawn Customer
         ▼
┌──────────────────┐
│ CustomerController│
└────────┬─────────┘
         │
         ├───────────────┐
         │               │
         ▼               ▼
    Load Data        Create Order
         │               │
         └───────┬───────┘
                 ▼
          Waiting for Service
                 │
          ┌──────┴──────┐
          ▼             ▼
       Served       Time Expired
                        │
                        ▼
                 Customer Angry
```

Setiap customer memiliki waktu tunggu yang dapat dihitung secara dinamis menggunakan countdown coroutine.

Customer juga dapat memberikan feedback melalui event seperti:

*  `OnCustomerArrived`
*  `OnCustomerAngry`

Hal ini memungkinkan sistem audio atau UI merespons kejadian tersebut tanpa harus memiliki ketergantungan langsung dengan `CustomerController`.

---

## 📋 Order System

Project memiliki sistem pemesanan yang memungkinkan customer memiliki tipe pesanan yang berbeda.

Saat ini terdapat konsep:

###  Direct Order

Customer langsung meminta resep tertentu.

```text
Customer
   │
   ▼
Request Recipe
   │
   ▼
Recipe Database
   │
   ▼
RecipeSO
   │
   ├── Ingredient
   ├── Quantity
   └── Other Data
```

###  Symptom-Based Order

Customer datang dengan membawa keluhan atau gejala tertentu.

Konsep ini menjadi fondasi untuk pengembangan mekanik **diagnosis**, di mana pemain nantinya dapat menentukan resep atau obat yang sesuai berdasarkan gejala customer.

---

##  Inventory System

Project memiliki sistem inventory berbasis slot dengan dukungan **item stacking**.

Inventory menggunakan slot dengan kapasitas tertentu:

```text
┌────────────────────────────────────┐
│            INVENTORY               │
├────────┬────────┬────────┬────────┤
│ Herb A │ Herb A │ Herb B │ Empty  │
│  x10   │  x5    │  x8    │        │
└────────┴────────┴────────┴────────┘
```

Ketika item ditambahkan:

1.  Sistem mencari slot yang sudah memiliki item yang sama.
2.  Jika masih tersedia kapasitas stack, item ditambahkan ke slot tersebut.
3.  Jika stack sudah penuh, sistem mencari slot kosong.
4.  Jika seluruh slot tidak tersedia, item tidak dapat ditambahkan.

Pendekatan ini membantu membuat inventory lebih efisien dibandingkan selalu membuat slot baru untuk setiap item.

Inventory juga menggunakan event `OnInventoryUpdate` untuk memberi tahu UI ketika isi inventory berubah sehingga tampilan dapat diperbarui secara langsung.

---

##  ScriptableObject Database

Data yang bersifat statis dipisahkan menggunakan **ScriptableObject**.

Beberapa ScriptableObject yang digunakan antara lain:

```text
ScriptableObjects/
├── CustomerDataSO.cs
├── DB_CustomerDatabaseSO.cs
├── DB_RecipeDatabaseSO.cs
├── EnemyDataSO.cs
├── IngredientSO.cs
└── RecipeSO.cs
```

Struktur tersebut memungkinkan data game dipisahkan dari gameplay logic sehingga konfigurasi dapat dilakukan melalui Unity Inspector tanpa harus mengubah kode secara langsung.

---

##  Optimized Database Lookup

Database customer dan recipe menggunakan `Dictionary<string, T>` untuk mempercepat pencarian berdasarkan ID.

Konsepnya:

```text
Database
   │
   ▼
Initialize Dictionary
   │
   ▼
┌───────────────────────┐
│ ID → Data              │
├───────────────────────┤
│ herb_001 → Ingredient │
│ herb_002 → Ingredient │
│ recipe_01 → Recipe    │
│ recipe_02 → Recipe    │
└───────────────────────┘
```

Dengan pendekatan dictionary, pencarian berdasarkan ID dapat dilakukan dengan kompleksitas rata-rata **O(1)** dibandingkan harus melakukan pencarian linear terhadap seluruh daftar data.

Project juga menggunakan **lazy initialization**, sehingga dictionary hanya dipersiapkan ketika diperlukan.

---

##  Unity Addressables

Asset visual customer dikelola menggunakan **Unity Addressables** melalui `AssetReferenceSprite`.

Customer dapat melakukan loading sprite secara asynchronous:

```text
Customer
   │
   ▼
AssetReferenceSprite
   │
   ▼
LoadAssetAsync()
   │
   ▼
Customer Portrait
```

Ketika object customer dihancurkan, asset juga dapat dilepas menggunakan:

```csharp
Addressables.Release()
```

Pendekatan ini membantu mengontrol penggunaan memory karena asset tidak harus selalu dimuat secara permanen selama permainan berjalan.

---

##  Time System

Project memiliki sistem waktu melalui `TimeController`.

Waktu dalam game berjalan menggunakan multiplier:

```text
Game Time
    │
    ▼
Time.deltaTime
    │
    ×
    │
Time Multiplier
    │
    ▼
In-Game Clock
```

Sistem ini memungkinkan waktu operasional toko berjalan lebih cepat daripada waktu sebenarnya.

Waktu kemudian ditampilkan menggunakan format digital:

```text
HH:mm
```

Contohnya:

```text
08:00
08:15
08:30
09:00
```

Sistem ini dapat menjadi fondasi untuk mekanik seperti:

*  Jam buka toko
*  Jam tutup toko
*  Customer schedule
*  Batas waktu pesanan
*  Event berdasarkan waktu

---

##  Audio System

Audio dikelola melalui `AudioManager`.

Sistem audio dapat merespons event gameplay tertentu seperti:

*  Customer datang
*  Customer marah
*  Interaksi toko
*  Feedback gameplay

Pemisahan audio manager dari gameplay logic memungkinkan sistem gameplay memicu feedback audio tanpa harus mengatur implementasi audio secara langsung.

---

##  Shop System

Sistem toko dipisahkan menjadi beberapa controller:

```text
Shop/
├── CashierController.cs
├── CustomerController.cs
├── OrderController.cs
├── ShopManager.cs
└── Spawner/
```

Setiap komponen memiliki tanggung jawab yang berbeda:

| Sistem               | Tanggung Jawab                        |
| -------------------- | ------------------------------------- |
| `ShopManager`        | Mengatur sistem utama toko            |
| `CashierController`  | Menangani proses pembayaran / cashier |
| `CustomerController` | Mengatur perilaku customer            |
| `OrderController`    | Mengatur pesanan customer             |
| `Spawner`            | Mengatur spawning object              |

Pemisahan ini membantu menghindari satu class memiliki terlalu banyak tanggung jawab.

---

##  UI System

UI dipisahkan dari gameplay logic melalui beberapa controller:

```text
UIs/
├── MainMenuUIController.cs
├── ShopUIController.cs
└── UIManager.cs
```

Dengan struktur ini, UI memiliki layer tersendiri untuk menangani:

*  Main Menu
*  Shop UI
*  Inventory
*  Order information
*  UI navigation

Hal ini membantu memisahkan **presentation layer** dari gameplay logic.

---

##  Struktur Project

```text
Assets/
└── _Project/
    └── Scripts/
        │
        ├── Audio/
        │   └── AudioManager.cs
        │
        ├── ScriptableObjects/
        │   ├── CustomerDataSO.cs
        │   ├── DB_CustomerDatabaseSO.cs
        │   ├── DB_RecipeDatabaseSO.cs
        │   ├── EnemyDataSO.cs
        │   ├── IngredientSO.cs
        │   └── RecipeSO.cs
        │
        ├── Shop/
        │   ├── Spawner/
        │   ├── CashierController.cs
        │   ├── CustomerController.cs
        │   ├── OrderController.cs
        │   └── ShopManager.cs
        │
        ├── UIs/
        │   ├── Shop/
        │   ├── MainMenuUIController.cs
        │   ├── ShopUIController.cs
        │   └── UIManager.cs
        │
        ├── Enemy/
        │
        ├── Player/
        │
        └── TimeController.cs
```

Struktur project menggunakan folder `_Project` untuk memisahkan asset dan kode yang dibuat khusus untuk project dari asset eksternal. Di dalam `Scripts`, sistem kemudian dipisahkan berdasarkan domain seperti Audio, Shop, UI, ScriptableObject, Player, dan Enemy.

---

##  Arsitektur Data-Driven

Salah satu pendekatan utama project ini adalah memisahkan **data** dari **logic**.

Contohnya:

```text
              ┌─────────────────────┐
              │   ScriptableObject   │
              │       Database      │
              └──────────┬──────────┘
                         │
                         │ Data
                         ▼
              ┌─────────────────────┐
              │    Game Systems     │
              │                     │
              │ Customer            │
              │ Order               │
              │ Inventory           │
              │ Shop                │
              └──────────┬──────────┘
                         │
                         │ Events
                         ▼
              ┌─────────────────────┐
              │         UI          │
              │       & Audio       │
              └─────────────────────┘
```

Dengan pendekatan ini, perubahan data seperti menambahkan resep, ingredient, atau customer baru tidak harus selalu membutuhkan perubahan pada gameplay logic.

---

## 🛠️ Teknologi yang Digunakan

* **Unity**
* **C#**
* Unity 2D
* Unity ScriptableObject
* Unity Addressables
* TextMeshPro
* C# Events
* C# Dictionary
* Unity Coroutine
* Async Asset Loading
* Git
* GitHub

---

##  Tujuan Project

Project ini dibuat untuk meningkatkan pemahaman mengenai **C# dan Unity Game Development**, terutama dalam membangun sistem simulasi yang terdiri dari banyak data dan gameplay system.

Fokusnya bukan hanya membuat gameplay berjalan, tetapi juga mempelajari bagaimana membuat sistem yang:

*  Modular
*  Reusable
*  Data-driven
*  Efisien
*  Loosely coupled
*  Mudah dikembangkan

Project ini juga menjadi latihan untuk menerapkan konsep software engineering ke dalam pengembangan game secara langsung.

---

##  Hal yang Dipelajari

Melalui project ini, saya memperdalam pemahaman mengenai:

*  Game Simulation System
*  ScriptableObject Database
*  Dictionary Lookup
*  Inventory & Stacking System
*  Customer Lifecycle
*  Order Management
*  In-Game Time System
*  Unity Addressables
*  Event-Based Audio
*  UI Architecture
*  Data-Driven Design
*  Separation of Concerns
*  Asynchronous Asset Loading

---

##  Pengembangan Selanjutnya

Beberapa sistem yang dapat dikembangkan lebih lanjut:

*  Sistem diagnosis berdasarkan gejala
*  Sistem crafting / pembuatan obat
*  Quest dan customer request
*  Economy & pricing system
*  Reputation system
*  Event berdasarkan kalender game
*  Upgrade toko
*  Save & Load System
*  Statistik dan analytics gameplay
*  Customer AI yang lebih kompleks
