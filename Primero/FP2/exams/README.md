# TIERLIST DE EXÁMENES FP2

###### IMPORTANTE: YO NO HAGO RECURSIÓN!! Siempre me dejo los 1-2pts. porque siempre llego justo justo de tiempo.
###### Dicho esto, en cuanto a los exámenes de FP2 Jaime no se suele pasar, son exámenes simples, pero evalúan parte técnica a buena escala normalmente, en FP1 es más saber sacar con biq métodos complicados, que en los exámenes de FP2 lo puede haber oye, pero generalmente está más orientado a saber si de verdad conoces como funciona el cotarro entre métodos, constructoras, listas, etc.


---

## 1. 🟢 KAKURASU *(Ordinaria 24)*
**Métodos clave:** Ninguno (todo muy mecánico)

Sin mucho que añadir, muy simplón, mecánicamente y técnicamente, no tiene ni comprobaciones de vecinos para ser tablero que es en donde se suelen complicar los exámenes de tablero, si vas con base es un paseo.

---

## 2. 🟡 SUDOKU *(Ordinaria 25)*
**Métodos clave:** Esquina, Posibles

Realmente le subo una tier en dificultad porque a lo mejor te tiras 20 minutos sacando la fórmula para el método `Esquina`, que queda bonito, pero te puede fastidiar si no estás fresco en el examen (aunque hay maneras más toscas de sacarlo si no sale esa), luego en `Posibles` tienes ciertas restricciones pero en general bastante mecánico, normalito en el resto de métodos, al ser un tablero tan simple los métodos generales (constructor, render, lee, etc.) salen muy rápido así que compensa mucho, pero tiene su twist. Tampoco nada del otro mundo, examen chill.

---

## 3. 🟠 LIGHTS OUT *(Extra 24)*
**Métodos clave:** TODOS

Este examen no va de saber hacer un método concreto, va de saber programar, de los que tengo y he hecho, es el más completo técnicamente, necesitas entender TODO lo que estás haciendo, si no, es probable que metas la pata.

- Sobrecarga de constructoras.
- Undo con lista enlazada y codificación de coordenadas (aunque eso al menos te lo da el enunciado).
- `StreamWriter`(Guarda) muy simple, pero el `StreamReader`(Restaura) es rarete porque en este examen se trata más como un "checkpoint" que como lo que suele ser y por ello lo hacemos en la clase de tablero junto al Guarda.
- El `ClickCasillaMemo` y el `DeshacerJugada` te los pintan en el enunciado como el mismísimo diablo pero si tienes papel y boli te das cuenta al instante que es una bobadita, pero hay que estar espabilao.
- El `ProcesaInput` gestiona más inputs de lo normal y se va extendiendo a lo largo que hacemos, también tratamos las excepciones ahí al no integrar el Restaura en el Main, así que ojito.

En resumen: si vas bien preparado técnicamente lo sacas, pero si fallas en alguno de los pilares (listas, archivos, excepciones, constructoras) se te va a acumular, es importantísimo que tengas muy claro todo, también es un examen de resistencia, no de dificultad pura ya que 2/3 del tiempo del examen lo inviertes creando métodos que llaman a otros métodos, gestionando las listas, o reconstruyendo métodos que ya habías "terminado", pero eso es todo, el Program no es nada, el Main es muy simplón de hecho.
