# Tarantule - Webová aplikace pro chovatele sklípkanů :spider_web: :spider: :spider_web:

- Smyslem aplikace je vytvořit prostředí pro komunikaci mezi chovateli sklípkanů. Uživatelé budou moci přidávat informace o svých sklípkanech, prohlížet si údaje o sklípkanech ostatních uživatelů, vést diskuze, nabízet sklípkany k prodeji a získávat přístup k databázi druhů a návodů na chov jednotlivých druhů.

- Komunikace mezi chovateli je klíčová pro rozvoj chovu. Celkovým cílem aplikace je umožnit chovatelům snadný a přehledný přístup k informacím, poznat jiné chovatele s možností spolupráce, a také nabídnout možnost prodeje sklípkanů a vést diskuse.

## Procesy aplikace

- Webová aplikace řeší správu uživatelů a přístupová práva na úrovni administrátora, běžného uživatele i moderátora. Zajišťuje operace přidání, zobrazení, úpravu a smazaní (CRUD) položek, například druhu sklípkana, prodeje sklípkana, a dalších. Aplikace automaticky doplňuje informace o prodejci k jednotlivým prodejům. Proces psaní komentářů je také součástí implementace.

## Technologie

- Webová aplikace je postavena na technologii ASP.NET MVC. Jako datové uložiště slouží MS SQL Server. Pro mapování databáze je využíván NHibernate. K vytvoření uživatelského rozhraní a stylování jsou použity prvky frameworku Bootstrap.

## Zabezpečení

- Aplikace má zabezpečení na úrovni uživatelských rolí, které je dosaženo pomocí autorizace jednotlivých procesů. Uživatelé mohou přistoupit pouze k tomu, na co mají oprávnění. Veškerá hesla jsou převedena do hash podoby před uložením do databáze. Vnější vstup má nastaveny podmínky pro to, aby mohl být přijat, a vkládá se pouze do určených míst pro vstup. NHibernate s využitím předpřipravených metod nám umožňuje bezpečný přístup k databázi.

## Video

- Login údaje: - jagos 1234 (admin)
               - jakub 1234 (moderátor)
               - nova 12345 (chovatel)




