# 🛡️ AEGIS – Secure Vault

**Aegis**, Windows için geliştirilmiş, **WPF (.NET)** tabanlı, **offline çalışan** ve **güvenlik odaklı** bir parola yöneticisidir.  
Amaç; modern, sade ve premium bir kullanıcı arayüzü ile birlikte **güvenli parola ve hassas veri yönetimi** sunmaktır.

> 🔒 Tüm veriler yerel olarak şifrelenir  
> 🌙 Dark / premium UI  
> 🧠 Basit ama sağlam mimari

---

## ✨ Özellikler

### 🔐 Güvenlik
- Master Password ile korunan encrypted vault
- Auto-lock (zaman aşımı sonrası otomatik kilitleme)
- Clipboard auto-clear
- Verileri tamamen sıfırlama (geri döndürülemez)
- Offline çalışma (cloud / sync yok)

### 📂 Vault Yönetimi
- Çoklu entry desteği
- Entry alanları:
  - Display Name
  - Username
  - Password
  - Notes
  - Kart / banka bilgileri
- Entry ekleme, düzenleme, silme
- Entry’ye özel görsel (image) ekleme
  - Dosya kilitlemeden yükleme
  - Uygulama açıldığında otomatik geri yükleme

### 🎨 Kullanıcı Arayüzü
- Modern dark / premium tasarım
- İki panelli yapı (liste + detay)
- Minimal animasyonlar
- Blur / acrylic etkiler
- Sade ve net UX

### ⚙️ Ayarlar
- Auto-lock süresi
- Clipboard davranışı
- Verileri Temizle (Factory Reset)

---

## 🧠 Teknik Detaylar

### Mimari
- MVVM (Model–View–ViewModel)
- Servis tabanlı yapı
- UI ve iş mantığı ayrımı

### Veri Saklama
- Vault verileri şifreli olarak saklanır
- Görseller uygulama dizinine kopyalanır
- BitmapImage nesneleri serialize edilmez

### Kullanılan Teknolojiler
- .NET (WPF)
- C#
- XAML
- MVVM pattern

---

## 🚀 Kurulum

1. Depoyu klonla:
   ```bash
   git clone https://github.com/kullaniciadi/aegis.git
   ```
2. Visual Studio ile aç
3. .NET Desktop Development workload yüklü olsun
4. Derle ve çalıştır

---

## 📌 Proje Durumu

**Durum:** Tamamlandı  
Bu sürüm bilinçli olarak sonlandırılmıştır.

---

## 📜 Lisans

Eğitim ve kişisel kullanım amaçlıdır.
