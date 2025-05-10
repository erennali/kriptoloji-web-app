# Kriptoloji Web Uygulaması

Bu proje, modern kriptografi algoritmalarını kullanarak güvenli veri şifreleme ve şifre çözme işlemlerini gerçekleştiren bir web uygulamasıdır. AES, RSA ve SHA-256 gibi güçlü kriptografi algoritmalarını kullanarak verilerinizi güvenli bir şekilde korumanızı sağlar.

## 🛡️ Kullanılan Kriptografi Algoritmaları

### AES (Advanced Encryption Standard)
AES, simetrik şifreleme algoritmasıdır ve günümüzde en yaygın kullanılan şifreleme standartlarından biridir.

#### Özellikleri:
- 128, 192 veya 256 bit anahtar uzunluğu
- Blok şifreleme (128 bit blok boyutu)
- Yüksek güvenlik ve performans
- Simetrik şifreleme (aynı anahtar ile şifreleme ve şifre çözme)

#### Kullanım Alanları:
- Dosya şifreleme
- Veri depolama
- Güvenli iletişim
- Hassas bilgilerin korunması

### RSA (Rivest-Shamir-Adleman)
RSA, asimetrik şifreleme algoritmasıdır ve açık anahtar kriptografisinin temelini oluşturur.

#### Özellikleri:
- Asimetrik şifreleme (farklı anahtarlar ile şifreleme ve şifre çözme)
- Açık anahtar ve özel anahtar çifti
- Sayısal imza oluşturma
- Güvenli anahtar değişimi

#### Kullanım Alanları:
- Dijital imzalar
- SSL/TLS protokolleri
- Güvenli e-posta
- Anahtar değişimi

### SHA-256 (Secure Hash Algorithm 256-bit)
SHA-256, kriptografik hash fonksiyonudur ve veri bütünlüğünü sağlamak için kullanılır.

#### Özellikleri:
- 256-bit çıktı uzunluğu
- Tek yönlü hash fonksiyonu
- Çarpışmaya karşı dirençli
- Veri bütünlüğü doğrulama

#### Kullanım Alanları:
- Dijital imzalar
- Veri bütünlüğü kontrolü
- Blockchain teknolojisi
- Şifre depolama

## 🚀 Proje Özellikleri

### Şifreleme İşlemleri
- AES ile simetrik şifreleme
- RSA ile asimetrik şifreleme
- SHA-256 ile hash oluşturma

### Şifre Çözme İşlemleri
- AES ile simetrik şifre çözme
- RSA ile asimetrik şifre çözme
- Hash doğrulama

### Güvenlik Özellikleri
- Güvenli anahtar yönetimi
- Rastgele anahtar üretimi
- Güvenli veri depolama
- Şifreleme anahtarlarının güvenli saklanması

## 💻 Teknik Detaylar

### AES Şifreleme
```javascript
// AES şifreleme örneği
const encryptedData = await aesEncrypt(data, key);
```

### RSA Şifreleme
```javascript
// RSA şifreleme örneği
const encryptedData = await rsaEncrypt(data, publicKey);
```

### SHA-256 Hash
```javascript
// SHA-256 hash örneği
const hash = await sha256(data);
```

## 🔧 Kurulum

Projeyi klonlayın:
```bash
git clone https://github.com/erennali/kriptoloji-web-app.git
```


## 📝 Kullanım

1. Web arayüzüne erişin
2. Şifrelemek istediğiniz veriyi girin
3. Kullanmak istediğiniz algoritmayı seçin
4. Şifreleme/şifre çözme işlemini gerçekleştirin

## 🔐 Güvenlik Önerileri

- Anahtarlarınızı güvenli bir şekilde saklayın
- Düzenli olarak anahtarlarınızı değiştirin
- Güçlü şifreler kullanın
- Sisteminizi güncel tutun

## 📚 Kaynaklar

- [AES Standard](https://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.197.pdf)
- [RSA Algorithm](https://tools.ietf.org/html/rfc8017)
- [SHA-256 Standard](https://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.180-4.pdf)

## 🤝 Katkıda Bulunma

1. Bu depoyu fork edin
2. Yeni bir branch oluşturun (`git checkout -b feature/yeniOzellik`)
3. Değişikliklerinizi commit edin (`git commit -am 'Yeni özellik eklendi'`)
4. Branch'inizi push edin (`git push origin feature/yeniOzellik`)
5. Pull Request oluşturun


## 📞 İletişim

Sorularınız veya önerileriniz için:
- E-posta: info@erenalikoca.com.tr
- GitHub Issues: [Proje Issues](https://github.com/erennali/kriptoloji-web-app/issues) 