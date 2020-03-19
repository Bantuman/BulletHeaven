# Bullet-Hell Level Editor

DIGDIG Projekt, 2020, Neo Nemeth & Alexander Granquist.
En level editor som kommer kunna användas för att skapa och designa skottmönster till bullet-hell spel.
Level editorn är i grund och botten en tidslinje som man kan placera events på, vid dessa events så hade skott / skottmönster skapas. Med hjälp av detta hade man kunnat synka upp skott med musik, eller bara använda den för att skapa tradionella bullet-hell nivåer.

### Utförande & Resultat

Level Editorn är skapad i Monogame XNA ramverket på grund av hur lätt det är att sätta upp, och hur effektivt det funkar. Idén var också att skapa ett template-spel i samma ramverk för att kunna testa levlarna, detta blev inte av.
Projektet började med en massa forskning på hur andra bullet-hell spel gör sina level-editors, och mer specifikt, hur andra bullet-hell spel hanterar komprimeringen av level-filerna för att hålla en acceptabel filstorlek. Eftersom ingen användbar information kunde hittas så gjordes allt från egen överensstämmelse. Filstorleken visa sig att inte vara ett problem i det minsta och UI designen blev en kombination av OSU! och Touhou spelen.

Eftersom vi inte hann bli klara med ett spel som vi kunde använda för att presentera levlarna så skapades istället ett spelkoncept i form av bilder. Där finns däremot en koncept level som man kan ladda ner och infoga i level-editorn om man kompilerar den. Annars går det också att öppna level-filen i en text-editor och kolla strukturen på så sätt.

* [Level Fil](https://github.com/Bantuman/BulletHeaven/releases/download/Level/Level.dmk)
* [Level Editor](https://github.com/Bantuman/BulletHeaven/releases/download/EDITOR/Editor.rar)

Grafiken i Level Editorn är förnuvarandet väldigt grundläggande och alla assets är place-holders. Därför är level-editorn nästan omöjlig att använda utan någon hjälp.
Nedan för ser du en bild-guide för hur man laddar in levlar, sparar levlar och placerar events.

![a](https://i.imgur.com/YnTVkyV.png)
![b](https://i.imgur.com/80tsqAd.png)

### Omdöme

Mycket i projektets gång gick bra men där är en del saker jag hade gjort annorlunda, som t.ex att fokusera mer på UI design än på att hantera filstorleken. Skapelsen av skott borde också gjorts enklare, den originella planen var att man skulle kunna designa skott-mönster i ett seperat fönster och sedan importera dessa skott-mönsterna in i level-editorn, just nu placerar man alla skott individuellt vilket gör det jobbigt att skapa komplicerade levlar.
Mer av grafikens arbete borde också att ha användts, just nu används bara place-holders, att byta ut allt detta med ordentlig grafik hade varit optimallt.
Allt som allt gick projektet bra och jag är hyffsat nöjd med hur projektet är just nu.
