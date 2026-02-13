using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SayiTahminOyunu
{
    class Program
    {
        static string skorDosyasi = "highscores.txt";

        static void Main(string[] args)
        {
            // Tüm skorları hafızaya almak için bir Sözlük (Dictionary) kullanıyoruz
            Dictionary<string, int> skorlar = SkorlariYukle();

            Console.WriteLine("--- Sayı Tahmin Oyununa Hoş Geldiniz! ---");
            Console.Write("Lütfen kullanıcı adınızı giriniz: ");
            string kullaniciAdi = Console.ReadLine().Trim().ToLower();

            // Kullanıcı daha önce oynamış mı kontrol et
            if (skorlar.ContainsKey(kullaniciAdi))
            {
                Console.WriteLine($"Tekrar hoş geldin {kullaniciAdi}! Şu anki rekorun: {skorlar[kullaniciAdi]} deneme.");
            }
            else
            {
                Console.WriteLine($"Merhaba {kullaniciAdi}, ilk oyunun için başarılar!");
            }

            Random rastgele = new Random();
            int hedefSayi = rastgele.Next(1, 101);
            int denemeSayisi = 0;

            while (true)
            {
                Console.Write("1-100 arası bir tahmin girin: ");
                if (!int.TryParse(Console.ReadLine(), out int tahmin))
                {
                    Console.WriteLine("Lütfen geçerli bir sayı girin!");
                    continue;
                }

                denemeSayisi++;

                if (tahmin < hedefSayi)
                    Console.WriteLine("Daha büyük!");
                else if (tahmin > hedefSayi)
                    Console.WriteLine("Daha küçük!");
                else
                {
                    Console.WriteLine($"\nTebrikler {kullaniciAdi}! {denemeSayisi} denemede bildin.");
                    
                    // Yeni rekor kontrolü
                    if (!skorlar.ContainsKey(kullaniciAdi) || denemeSayisi < skorlar[kullaniciAdi])
                    {
                        skorlar[kullaniciAdi] = denemeSayisi;
                        SkorlariKaydet(skorlar);
                        Console.WriteLine("YENİ KİŞİSEL REKOR KAYDEDİLDİ!");
                    }
                    break;
                }
            }

            Console.WriteLine("\n--- SKOR TABLOSU (TOP 5) ---");

// Dictionary'deki skorları küçükten büyüğe (en az deneme en üste) sıralıyoruz
var siraliSkorlar = skorlar.OrderBy(x => x.Value).Take(5).ToList();

for (int i = 0; i < siraliSkorlar.Count; i++)
{
    string derece = (i == 0) ? "En İyi Oyuncu" : $"{i + 1}. Oyuncu";
    Console.WriteLine($"{derece}");
    Console.WriteLine($"{siraliSkorlar[i].Key.ToUpper()}: {siraliSkorlar[i].Value} deneme");
    Console.WriteLine("---------------------------");
}

            Console.WriteLine("\nÇıkmak için bir tuşa basın...");
            Console.ReadKey();
        }

        static Dictionary<string, int> SkorlariYukle()
        {
            var yuklenenSkorlar = new Dictionary<string, int>();
            if (File.Exists(skorDosyasi))
            {
                foreach (var satir in File.ReadAllLines(skorDosyasi))
                {
                    var parcalar = satir.Split(':');
                    if (parcalar.Length == 2 && int.TryParse(parcalar[1], out int skor))
                    {
                        yuklenenSkorlar[parcalar[0]] = skor;
                    }
                }
            }
            return yuklenenSkorlar;
        }

        static void SkorlariKaydet(Dictionary<string, int> skorlar)
        {
            var satirlar = skorlar.Select(x => $"{x.Key}:{x.Value}");
            File.WriteAllLines(skorDosyasi, satirlar);
        }
    }
}