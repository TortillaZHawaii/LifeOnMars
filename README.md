# Grafika 3D - Dawid Wysocki

Projekt przedstawia uproszczony układ planetarny ze słońcem, marsem krążącym wokół słońca i satelitą.

## Wymagania
Solucja korzysta z .NET w wersji 6.0.

## Obiekty

Słońce znajduję się na środku sceny.
Wokół niego krąży mars. Oba te obiekty są gładkie i zostały stworzone za pomocą klasy SphereUV.

Trzecim obiektem jest satelita. Satelita krąży wokół poruszającego się marsa oraz zmniejsza i zwiększa swoją wysokość względem marsa.

Słońce posiada wysoki współczynnik ambient, mars współczynnik diffuse, a satelita współczynnik specular.

## Światła

Scena posiada trzy źródła światła:
 - punktowe światło znajduję się w środku sceny w słońcu,
 - reflektor satelity o mocy 50 i kolorze niebieskim,
 - reflektor satelity o mocy 100 i kolorze zielonym.

Oba reflektory są animowane (zmiana kierunku światła) tak aby "skanować" planetę mars.

## Cieniowanie

Istnieją trzy dostępne tryby cieniowania:
 - flat,
 - Gouraud,
 - Phong.
Można je wybrać z poziomu menu z prawej strony okienka aplikacji.

## Kamery

Istnieją trzy dostępne kamery:
 - stała skupiona na słońcu,
 - podążająca za marsem,
 - "przyklejona" do satelity.

Można je wybrać z poziomu menu z prawej strony okienka aplikacji.

## Uwagi techniczne

Solucja składa się z dwóch projektów:
 1. Jednosc - silnik do renderowania grafiki 3D.
 2. LifeOnMars - aplikacja wyświetlająca grafikę 3D i animująca scenę.

Potok renderowania znajdziemy w klasie RendererMultiThread.
Przekazujemy do niego obiekt klasy Scene, który zawiera wszystkie obiekty, światła i kamerę.

RendererMultiThread posiada:
 - backface culling,
 - efekt mgły (szczególnie widoczny w kamerze podążającej za marsem),
 - zbuffer,
 - rzutowanie z kostki na współrzędne ekranu,
 - wycinanie trójkątów spoza kostki renderowania, (za poleceniem Pana Kotowskiego z wykładu usuwamy cały trójkąt który leży poza kostką),
 - rysowanie trójkątów na wielu wątkach z wykorzystaniem współrzędnych barycentrycznych.

RendererMultiThread korzysta z IShader, który oblicza położenie trójkąta w kostce renderowania i ustawia odpowiedni kolor pikseli. Aby umożliwić zmianę shadera w trakcie renderowania RendererMutltiThread przyjmuję fabrykę IShaderFactory.

Potok ten jest wzorowany na sposobie działania OpenGL.

Z uwagi na tworzenie dużych obiektów na początku programu, pokazanie się okienka może zająć trochę czasu.
