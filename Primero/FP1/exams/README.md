# TIERLIST DE EXÁMENES FP1

##### No es por señalar lo obvio pero si sacas FP1 en la ordinaria mucho mejor. Las extras suelen ser bastante más complejas a la hora de entendimiento general de programación, además de que Jaime considera que venís de un cuatrimestre entero dando FP2 y por ello es justificable que no haga un paseo de examen en la extra.

---

## 1. 🟢 SOLITARIO *(Ordinaria 23)*
**Métodos clave:** Ninguno (todo MUY mecánico)

Array unidimensional, métodos mecánicos. Lo único "complicado" sería sacar los palos y los valores de las cartas con `k/10`, `k%10`, pero te lo da el enunciado así que la única parte que puede conllevar algo de biq ya te la resuelven.

---

## 2. 🟢 NIM *(Ordinaria 24)*
**Métodos clave:** JuegaHumano

Parece stacked pero en realidad es en gran parte mecánico. Requiere algunos truquillos (rotar turno, palíndromo) pero nada que no se pueda sacar con boli y papel.

---

## 3. 🟡 TAKUZU *(Ordinaria 25)*
**Métodos clave:** Ninguno (el método con más chicha es un ConsoleWrite simulator)

Lo pongo después que los dos anteriores porque eran muy fáciles. Conlleva un poco más de dificultad porque hay como 3 métodos que convergen en 1 (MuestraResultado), pero es MUY mecánico a niveles exagerados. Sacar filas, comprobar x cosa en misma fila/col, etc.

---

## 4. 🟡 HITORI *(Extra 23)*
**Métodos clave:** RepetidosMatriz

Para ser extraordinaria es muy calmadito, lo que te esperas de un examen simple. Mirar filas, comparar, etc. Todo muy simple, no hay mucha complicación.

---

## 5. 🟠 PIEZAS DESLIZANTES *(Extra 24)*
**Métodos clave:** Desordena, LocalizaHueco, Desliza

Empieza con uso de struct, que si sabes usarlo es lo más simple del examen. Luego requiere manejar el cambio entre índice unidimensional y bidimensional - una bobada si lo tienes, pero te pilla si no. Por último, encontrar adyacente con biq es lo que más se puede complicar: si lo haces a saco ese método queda horrible. En resumen, es un examen muy complicado que si vas con pocas horas de sueño no lo sacas, pero tiene partes mecánicas una vez lo pillas.

---

## 6. 🔴 LIFE GAME *(Extra 25)*
**Métodos clave:** TODOS - si cae un examen parecido recuerda priorizar el bucle principal y olvídate del resto si el tiempo aprieta (en este caso, si te sobra algo de tiempo escribe al menos Lee. BuscaPatron y Menu los últimos)

CON DIFERENCIA el examen más jodido de toda la historia de FP1 en esta carrera:

- Tienes que declarar un `Inicializa` con variables que le preguntamos al jugador, que no vienen dadas por ejemplo o declaraadas de antemano como siempre.
- Para los métodos más fáciles tienes que saber cómo rotar al ser tablero toroidal (pasar del último valor del array al primero sumando).
- Tablero toroidal + casillas diagonales como vecinas + en `NumVecinos` hay que declarar los datos de los arrays MANUALMENTE con `Prev` y `Next`.
- No hay `LeeInput` ni `ProcesaInput` que al fin y al cabo son puntos y tiempo regalados, que en este examen son sustituidos por otros métodos que si vas a tardar en hacer.
- No hay `Guarda`; y `Lee` vale 1 punto. Encima hay un método `Menu` donde metes el inicio del bucle main convencional.
- `SiguienteGen` parece simple pero en el main no puedes modificar `mat` directamente porque necesitas compararlo con `Estable`, tienes que devolver una matriz nueva, y además el `else` en el for es obligatorio para que funcione bien.
- Lo más fastidiado: `BuscaPatron`, que necesita 4 FOR ANIDADOS (2 en el primer método y otros 2 en el siguiente) y una iluminación divina si no sabes plantear la búsqueda de un array bidimensional dentro de otro.

En resumen, este examen es inclusive más difícil que algunos de FP2, y con diferencia.