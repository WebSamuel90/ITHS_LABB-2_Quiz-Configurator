# ITHS_LABB-2_Quiz-Configurator

### Tekniska lösningar och utmaningar

**Återanvändbara svarsknappar**

Till en början hade jag fyra separata svarsknappar i XAML, där varje knapp hade egna bindings och triggers för att visa rätt eller fel svar. Det gjorde att XAML-filen blev väldigt lång och att jag behövde hantera bland annat `SelectedAnswer` och olika index i min ViewModel.

Jag valde därför att refaktorera lösningen och skapa en `AnswerOptionViewModel` för varje svarsalternativ. Med hjälp av `ItemsControl` och `DataTemplate` kan jag nu generera alla svarsknappar från en lista, istället för att definiera varje knapp separat. Jag använder också `UniformGrid` för att placera knapparna i ett 2×2-rutnät.

Refaktoreringen gjorde koden lättare att förstå och underhålla, samtidigt som jag kunde ta bort över 200 rader XAML.

**Problem och felsökning**

Redan när jag byggde den första versionen av `PlayerView` märkte jag att ikonerna för rätt och fel svar hamnade ovanpå svarstexten istället för uppe i knappens vänstra hörn. Jag valde att skjuta upp problemet och fokusera på att få quizets funktionalitet att fungera först.

När jag senare tog tag i layouten använde jag tillfälliga färgade ramar runt elementen för att se hur mycket utrymme de tog upp. Då upptäckte jag att `Grid` inuti knappen inte fyllde hela knappens yta, utan bara tog upp så mycket plats som innehållet behövde.

Jag löste problemet genom att sätta `HorizontalContentAlignment="Stretch"` och `VerticalContentAlignment="Stretch"` på knappen. Därefter kunde jag placera ikonerna och texten oberoende av varandra.