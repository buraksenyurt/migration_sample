# .Net 4.7.2 Tabanlı bir Uygulamalar Bütününü Net 5'e Nasıl Taşırız?

Malum Net 5, .Net Framework dönemlerinden gelen bazı konuları artık içermiyor/desteklemiyor. WPF, WCF, WWF, .Net Remoting buna örnek olarak verilebilir.Herhalde 2020ler Net 5'in yılları olacak. Bu noktada 2000lerden gelen ve evrilerek .Net 4.8'e kadar çıkan birçok uygulama olduğunu da ifade etsek yeridir. Dolayısıyla bu çözümleri Net 5'e taşımak isteyebiliriz. Bu pratikte .Net 4.7.2 tabanlı bir solution içeriğini Net 5'e taşımakla ilgili çalışmalar yer alacak. 

İki klasörümüz var. Birisi var olan uygulamamız diğeri de Net 5'e çevrilmiş hali.

## ClassicGames

.Net Framework 4.7.2'yi merkezine alan Solution. _(Özellikle projelerin packages.config, csproj dosya içeriklerini takip edin)_

- ClassicGames.Models: Model sınıfları olan Game ve GameReview tiplerini içerip, Newtonsoft'u referans eden Class Library türevidir.
- ClassicGames.DAL: Data Access Layer rolünü üstlenen Class Library. Autofac _(IoC Container için)_, Serilog _(Loglama için)_ ve EntityFramework _(ORM entegrasyonu için)_ NuGet paketlerinin .Net Framework 4.7.2 için uyumlu olan sürümlerini kullanıyor. Oyun ve oyun yorumları ile ilgili gerekli CRUD operasyonlarını sağlayan bir kütüphane.
  - ClassicGames.WebClient: Asp.Net MVC 5 tipinden bir web istemcisidir. Repository üstünden oyun ve oyun yorumları ile ilgili operasyonları icra eder. Bunun için ClassicGames.DAL ve ClassicGames.Models projelerini kullanır. Ayrıca, Antlr, Autoface, EntityFramework, Bootstrap gibi Nuget paketleri kullanmaktadır.
- ClassicGames.Dashboard: WPF tabanlı bir Administrator uygulamasıdır. _Yazılacak_

MVC 5 tabanlı web uygulamasına ait birkaç ekran görüntüsü...

Bizi karşılayan boş bir lobi sayfası.
![screenshot_1.png](./assets/screenshot_1.png)

Commdore64DBInitializer sınıfı ile ilk etapta eklenen iki oyun bilgisinin gösterildiğin oyunlar sayfası.
![screenshot_2.png](./assets/screenshot_2.png)

Bir oyun için detaya gittiğimizde karşımıza çıkmasını beklediğimiz sayfa.
![screenshot_3.png](./assets/screenshot_3.png)

Oyunla ilgili değerlendirmelerin toplu olarak görüldüğü sayfa.
![screenshot_4.png](./assets/screenshot_4.png)

Bir değerlendirmeyi silmek istediğimizde karşımıza çıkan sevimli sayfa :P
![screenshot_5.png](./assets/screenshot_5.png)

_Devam Edecek_

## Migration İşlemleri

_Yazılacak_