namespace hoja2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // NOTA IMPORTANTE:
            // Todos los tiempos escritos en las respuestas de teoría se han
            // obtenido experimentando con el vector de tamaño 1000 a no ser que
            // se indique lo contrario.


            // Generamos un array que rellenaremos con números random
            Random rnd = new Random();
            int[] v = new int[10000];

            // Le damos valor a cada posición del array
            for (int i = 0; i < v.Length; i++)
            {
                v[i] = rnd.Next(0, 100000);
            }

            // Ejercicio 1
            int[] ins = (int[])v.Clone();
            int[] sel = (int[])v.Clone();
            int[] bur = (int[])v.Clone();

            // Ejercicio 2
            int[] dobleSel = (int[])v.Clone();

            // Ejercicio 3
            int[] vEj3 = new int[1000];
            for (int i = 0; i < vEj3.Length; i++)
            {
                vEj3[i] = rnd.Next(0, 1000);
            }


            // Llamada a métodos
            Insercion(ins);
            Seleccion(sel);
            Burbuja(bur);

            // Escribir los números de los arrays
            /* for (int i = 0; i < v.Length; i++)
            {
                // Inserción
                //Console.Write(ins[i] + " ");

                // Selección
                //Console.Write(sel[i] + " ");

                // Burbuja
                //Console.Write(bur[i] + " ");

                if (i > 1 & i % 9 == 0) Console.WriteLine();
            }*/

            // Cronómetro
            TimeSpan start = new TimeSpan(DateTime.Now.Ticks);
            Insercion(sel);
            TimeSpan end = new TimeSpan(DateTime.Now.Ticks);
            double elapsed = end.Subtract(start).TotalMilliseconds;
            Console.WriteLine("Tiempo algoritmo 1: " + elapsed + "ms");

            TimeSpan start2 = new TimeSpan(DateTime.Now.Ticks);
            InsercionEj4(sel);
            TimeSpan end2 = new TimeSpan(DateTime.Now.Ticks);
            double elapsed2 = end2.Subtract(start2).TotalMilliseconds;
            Console.WriteLine("Tiempo algoritmo 2: " + elapsed2 + "ms");

        }

        // Ejercicio 1
        // a) Estudiar los algoritmos de Ins, Sel, y Burbuja
        // b) Pregunta teórica
        static void Swap(ref int x, ref int y)
        {
            int tmp = x; // temp = x
            x = y; // x pilla valor de y
            y = tmp; // y pilla valor de tmp (x)
        }
        static void Insercion(int[] v)
        // "Para cada elemento, insértalo ordenadamente entre los anteriores".
        {
            int n = v.Length;
            // v[0] ya está ordenado, para cada uno desde 1 hasta n:
            for (int i = 1; i < n; i++)
            {
                int tmp = v[i];
                int j = i - 1;
                // Desplazamos estos a la derecha abriendo hueco para v[i]
                while (j >= 0 && v[j] > tmp)
                {
                    v[j + 1] = v[j];
                    j--;
                }
                v[j + 1] = tmp;
            }
        }
        static void Seleccion(int[] v)
        {
            // "Busca el mínimo del vector y lo coloca en v[i], repite hasta ordenar todo"
            int n = v.Length;
            // En cada posición i=0..n−1 ponemos el menor de v[i..n−1]
            // Empezamos i = 0 hasta n - 2 (v[n−1] ya está en su sitio al ser el último)
            for (int i = 0; i < n - 1; i++)
            {
                // Buscamos el menor en v[i..n)
                int min = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (v[j] < v[min]) min = j; // Con menor estricto es estable
                }
                // Ponemos el menor en v[i], y v[i] en la pos del menor
                Swap(ref v[i], ref v[min]);
            }
        }
        static void Burbuja(int[] v)
        {
            // "Se recorre el array de principio a fin, comparando los elementos dos a
            // dos (contiguos) e intercambiándolos si no están en orden"
            int n = v.Length;
            // damos n vueltas, en cada una v[i] queda en su sitio, lo que vamos cambiando es v[j],[j-1]
            for (int i = 0; i < n; i++)
            {
                for (int j = n - 1; j > i; j--)
                {
                    if (v[j - 1] > v[j])
                    {
                        Swap(ref v[j - 1], ref v[j]);
                    }
                }
            }
        }
        /* Pregunta teórica ejercicio 1
         *
         * Inserción:
         * Mejor caso: array ya ordenado -> O(n), el while interior nunca se ejecuta
         * Peor caso: array al revés -> O(n²), desplaza todo en cada iteración
         *
         * Selección:
         * Siempre O(n²) sin importar el orden, porque SIEMPRE recorre todo buscando el mínimo.
         *
         * Burbuja:
         * Peor caso: array al revés -> O(n²)
         * Burbuja mejorado: mejor caso O(n) si ya está ordenado, para en la primera vuelta
         *
         * Conclusión: inserción y burbuja mejorado son sensibles al orden inicial
         * y mejoran mucho con arrays casi ordenados.
         * Selección es totalmente insensible, siempre tarda lo mismo.
        */


        // Ejercicio 2
        // a) Crear un algoritmo de doble selección
        // b) Hacer comparativas de tiempo
        static void SeleccionDoble(int[] v)
        {
            int n = v.Length;
            for (int i = 0; i < n / 2; i++) // n / 2 porque nos encontramos en el medio
            {
                int min = i;
                int max = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (v[j] < v[min]) min = j; // Si v[i + 1] < v[i]
                    if (v[j] > v[max]) max = j; // Si v[i + 1] > v[i]
                }
                Swap(ref v[i], ref v[min]);
                Swap(ref v[n - 1 - i], ref v[max]);
            }
        }
        /* // Pregunta teórica ejercicio 2
         * Al realizar las pruebas en el crono me ha salido de media en la selección normal ~4ms
         * en cambio en la doble ~1,5ms, esto se podría justificar porque en la selección doble nos
         * ahorramos la mitad de vueltas en el bucle principal, mientras solo añadimos
         * un condicional dentro del segundo bloque.
        */


        // Ejercicio 3
        static void Ordena(int[] v)
        {
            int max = 999;
            int[] t = new int[max + 1]; // array de frecuencias
            for (int i = 0; i <= t.Length; i++)
            {
                t[v[i]]++; // Por ejemplo si v[0] = 5, hacemos t[5]++.
                           // Así al final t[5] contiene cuántas veces aparece el 5 en v
            }

            int j = 0;
            for (int i = 0; i <= t.Length; i++)
            {
                while (t[i] > 0) // mientras queden repeticiones de i
                {
                    v[j] = i;
                    j++;
                    t[i]--;
                }
            }
        }
        /* // Pregunta teórica ejercicio 3
         * ¿Es mejor que los algoritmos anteriores? Sí, es O (n + max) que para rangos
         * pequeños es muchísimo más rápido que O(n^2). Aunque usamos memoria extra para t, 
         * y este método es específico para enteros en un rango acotado.
        */


        // Ejericico 4
        static bool BusquedaTrinaria(int[] v, int e)
        {
            int ini = 0, fin = v.Length - 1;
            bool enc = false;
            while (ini <= fin && !enc)
            {
                int ter1 = ini + (ini + fin) / 3;
                int ter2 = ini + 2 * (ini + fin) / 3;

                if (e < v[ter1]) fin = ter1 - 1;
                else if (e > v[ter1] && e < v[ter2])
                {
                    ini = ter1 + 1;
                    fin = ter2 - 1;
                }
                else if (e > v[ter2]) ini = ter2 + 1;
                else enc = true;
            }
            return enc;
        }
        /* // Pregunta teórica ejercicio 4
         * Aunque a simple vista parezca que como acotamos más el rango que en binaria va
         * a ser más beneficioso, no nos equivoquemos, al añadir otro segmento estamos haciendo
         * dos comparativas por iteración, que a la larga es incluso peor
        */


        // Ejercicio 5
        static int PrimerMayorIgual(int[] v, int ini, int fin, int e)
        {
            // Búsqueda binaria que devuelve el ÍNDICE del primer elemento >= e
            while (ini <= fin)
            {
                int med = (ini + fin) / 2;

                if (e <= v[med]) fin = med - 1;
                else ini = med + 1;
            }
            if (ini > fin) return -1; // no existe ningún elemento >= e
            return ini;
        }
        static void InsercionEj4(int[] v)
        {
            int n = v.Length;
            for (int i = 1; i < n; i++)
            {
                int tmp = v[i]; // Elemento a insertar
                int pos = PrimerMayorIgual(v, 0, i - 1, tmp); // índice donde vamos a insertar v[i]
                if (pos == -1) pos = i; // va al final, no hay nadie mayor
                for (int j = i; j > pos; j--)
                    v[j] = v[j - 1]; // desplazamos a la derecha
                v[pos] = tmp;
            }
        }
        /* // Pregunta teórica ejercicio 5
         * Hay una mejoría increíble, pasando de unos ~3,5ms de la inserción normal a
         * no más de ~0,5ms en la inserción con búsqueda binaria, incluso con 10k de tamaño
         * en el array se nota una diferencia abismal, +-85% de mejora, una pasada si me
         * preguntas, seguramente se debe a eliminar el while secuencial de insercion
        */


        // Ejercicio 6
        static int BuscaRot(int[] v)
        {
            int ini = 0;
            int end = v.Length - 1;

            while (ini <= end)
            {
                int med = (ini + end) / 2;

                if (v[med] > v[med + 1]) return med + 1;
                else if (v[ini] > v[med]) end = med;
                else ini = med + 1;
            }
            return -1;
        }


        // Ejercicio 7
        static void Secuencia(int[] v, int e, out int pri, out int ult)
        {
            pri = -1;
            ult = -1;
            int ini = 0, fin = v.Length - 1;

            // Primero
            while (ini <= fin)
            {
                int med = (ini + fin) / 2;

                if (e < v[med]) fin = med - 1;
                else if (e > v[med]) ini = med + 1;
                else
                {
                    pri = med;        // candidato, pero puede haber más a la izquierda
                    fin = med - 1;    // seguimos buscando a la izquierda
                }
            }
            // reset antes del bucle de último
            ini = 0;
            fin = v.Length - 1;

            // Último
            while (ini <= fin)
            {
                int med = (ini + fin) / 2;

                if (e < v[med]) fin = med - 1;
                else if (e > v[med]) ini = med + 1;
                else
                {
                    ult = med;        // candidato, pero puede haber más a la derecha
                    ini = med + 1;    // seguimos buscando a la derecha
                }
            }
        }


        // Ejercicio 8 (tocho)
        (int, int) Busca(int[,] mat, int e)
        {

            int fils = mat.GetLength(0);
            int cols = mat.GetLength(1);
            int ini = 0, fin = fils * cols - 1;

            while (ini <= fin)
            {
                int med = (ini + fin) / 2;
                // Aquí hay que hacer una sacada de BIQ matemático
                // La división entera te da cuántas filas completas han pasado,
                // y el módulo la posición restante dentro de esa fila.
                // Que se te ocurra esto es básicamente el ejerciio, no tiene nada más
                int fil = med / cols;  // convertimos med a fila
                int col = med % cols;  // convertimos med a columna

                if (e < mat[fil, col]) fin = med - 1;
                else if (e > mat[fil, col]) ini = med + 1;
                else return (fil, col); // encontrado
            }
            return (-1, -1); // no encontrado

        }
    }
}