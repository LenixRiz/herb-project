## Medieval Herbalist & Apothecary Shop Simulation (Unity)
Repositori ini berisi implementasi sistem manajemen toko herbal/apotek (apothecary shop management) berbasis 2D di Unity. Proyek ini berfokus pada arsitektur pemesanan dinamis (customer order lifecycle), runtime inventory slot stacking, Addressables asset management, siklus waktu dalam game (in-game clock system), serta pemanfaatan ScriptableObject database dengan optimasi dictionary lookup.

# Fitur Utama
1. Siklus Pelanggan & Antrean Dinamis (CustomerSpawner & CustomerController):
Sistem spawning pelanggan otomatis dengan variasi data acak dari database.
Menghitung batas waktu tunggu (wait duration) secara dinamis per pelanggan menggunakan countdown coroutine.
Feedback audio kontekstual (OnCustomerArrived, OnCustomerAngry) saat kedatangan atau saat pelanggan kehabisan waktu tunggu.

2. Integrasi Unity Addressables (AssetReferenceSprite):
Potret visual pelanggan dimuat secara asinkron (LoadAssetAsync<Sprite>()) melalui Addressables.
Pelepasan memori otomatis (Addressables.Release) saat GameObject pelanggan dihancurkan untuk mencegah kebocoran memori (memory leak).
Sistem Pemesanan Adaptif (OrderController):
Mendukung dua tipe pesanan via CustomerOrderType:
DirectOrder: Pelanggan langsung meminta resep spesifik beserta visualisasi bahan baku (IngredientSO).
SymptomBasedOrder: Pelanggan datang membawa keluhan/gejala penyakit (fondasi untuk mekanik diagnosa).

3. Inventory & Slot Stacking System (ShopInventory & InventorySlot):
Manajemen slot inventaris berbasis array dengan batasan kapasitas stack (MaxStack).
Logika otomatis: memprioritaskan slot yang sudah terisi item identik sebelum mengalokasikan ke slot kosong baru.
Event OnInventoryUpdate untuk sinkronisasi instan ke antarmuka pengguna (UI).

4. Sistem Waktu & Multiplier Jam Game (TimeController):
Pengendali jam operasional toko dengan konversi waktu dinamis (Time.deltaTime * _timeMultiplier).
Menampilkan format jam real-time digital (HH:mm) ke TextMeshPro UI.

5. Database Lookup Teroptimasi (CustomerDatabaseSO & RecipeDatabaseSO):
Data statis disimpan dalam format ScriptableObject.
Dilengkapi inisialisasi lazy Dictionary<string, T> untuk pencarian instan berdasar ID (O(1)) sekaligus metode acak untuk variasi gameplay.
