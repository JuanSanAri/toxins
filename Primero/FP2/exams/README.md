# TIERLIST DE EXÁMENES FP2

###### IMPORTANTE: YO NO HAGO RECURSIÓN!! Siempre me dejo los 1-2pts. porque siempre llego justo justo de tiempo.
###### Dicho esto, en cuanto a los exámenes de FP2 Jaime no se suele pasar, son exámenes simples, pero evalúan parte técnica a buena escala normalmente, en FP1 es más saber sacar con biq un método complicado, que en los exámenes de FP2 lo puede haber oye, pero generalmente está más orientado a saber si de verdad conoces como funciona el cotarro entre clases.


---

## 1. 🟢 KAKURASU *(Ordinaria 24)*
**Métodos clave:** Ninguno (todo muy mecánico)

El más simple de los tres con diferencia. Sumas por fila/columna, enums para los estados de casilla, constructor directo con la matriz de datos. No hay ningún método que te haga pensar demasiado. `Incorrectas` con `out` y `SumaFila`/`SumaCol` son mecánicos puros. Si llevas la base bien, es un paseo.

---

## 2. 🟡 SUDOKU *(Ordinaria 25)*
**Métodos clave:** Esquina, Posibles

Sube un escalón respecto a Kakurasu. `Esquina` con `ref` requiere pillar el truco de la división entera (`fil/3*3`), que no es obvio si no lo has visto. `Posibles` te obliga a pensar en tres restricciones a la vez (fila + columna + subcuadrícula), pero una vez pillas el patrón de "tachar en el array de booleanos", todo cae por su propio peso. `PonNumero` con excepciones (`throw`) y `DamePosibles` con listas enlazadas son directos si dominas los conceptos.

---

## 3. 🟠 LIGHTS OUT *(Extra 24)*
**Métodos clave:** TODOS — este examen no va de saber hacer un método concreto, va de saber programar

El examen más técnicamente completo de FP2. No es que haya un método especialmente difícil, es que necesitas dominar de TODO:

- Sobrecarga de constructoras (una con strings, otra con `Random` para tableros aleatorios).
- Undo con lista enlazada y codificación de coordenadas (`k = 10*fil + col`, recuperar con `/10` y `%10`).
- Guardar/restaurar partida con `StreamWriter`/`StreamReader`, validación de formato y dimensiones con `try/catch/finally`.
- Conmutar casillas adyacentes respetando límites del tablero.
- Integrar todo en un `ProcesaInput` que gestione 8+ inputs distintos.

En resumen: si vas bien preparado técnicamente lo sacas, pero si fallas en alguno de los pilares (listas, archivos, excepciones, constructoras) se te va a acumular. Es un examen de resistencia, no de dificultad pura.
