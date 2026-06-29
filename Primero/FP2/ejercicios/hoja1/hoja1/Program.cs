namespace hoja1
{
    internal class Program
    {
        const int N = 100; // tamaño de los arrays de monomios
        struct Monomio
        { // coeficiente y exponente
            public double coef;
            public int exp;
        }
        struct Polinomio
        {
            public Monomio[] mon; // array de monomios
            public int nMons; // num de monomios = primera pos libre en el array mon
        }


        static void Main(string[] args)
        {
            Polinomio p1, p2;

            Lee(out p1);
            //Lee(out p2);

            Polinomio p3 = Derivada(p1);
            Console.WriteLine();
            Escribe(p3);
        }


        static Polinomio Derivada(Polinomio p)
        {
            // Creamos nuevo polinomio ya que el método tiene que devolver uno
            Polinomio resultado;
            resultado.mon = new Monomio[p.nMons]; // El array, del tamaño del array polinomio
                                                  // que queremos derivar como mínimo
            resultado.nMons = 0; // Declaramos 0, según haya una iteración le sumamos uno

            int j = 0; // Auxiliar para el bucle, ya que el enunciado dice que no puede haber
            // ningún monomio dentro del polinomio que tenga exp = 0, y claro, ¿qué ocurre si
            // derivo un entero (exp = 0)? Exacto, se va, ya que la derivada de un entero siempre es 0.
            // Entonces la derivación de ese monomio no llega al polinomio resultado, entonces claro,
            // si uso i para ambos (p.mon[i], resultado.mon[i]), me doy cuenta de que si, en p tengo ese
            // monomio que es un entero, pero a la hora de recibirlo en resultado no llega porque al poner
            // el condicional no lo vamos a hacer, entonces se quedaría un hueco vacío en el array de 
            // resultado, ¿se ve?, un ejemplo:
            // p.mon[0]: 3x^0 -> se salta   resultado.mon[0] -> vacío
            // p.mon[1]: 5x^1               resultado.mon[1] -> se rellena
            // p.mon[2]: 2x^3               resultado.mon[2] -> se rellena

            for (int i = 0; i < p.nMons; i++)
            {
                if (p.mon[i].exp != 0) // El condicional del q hablo arriba, mientras exp!=0 derivo
                {
                    // Derivada
                    resultado.mon[j].coef = p.mon[i].coef * p.mon[i].exp;
                    resultado.mon[j].exp = p.mon[i].exp - 1;

                    resultado.nMons++; // Un monomio a la cuenta
                    j++; // Solo si se ha ejecutado la derivada avanzamos en el array de resultado
                }
            }
            // Nota: reultado.nMons y j como se ve siempre es lo mismo, pero por ahora uso j para que
            // se entienda, pero se podría usar resultado.mon[resultado.nMons].coef/exp perfectamente
            return resultado;
        }

        static void Inserta(Monomio m, ref Polinomio p) // TIER SSS MÉTODO, SIN ESTE NO HACES NADA
        {
            if (p.nMons >= N) // Si la cantidad de monomios ya es el límite (N), salimos
            {
                Console.WriteLine("ERROR: Se ha alcanzado el límite de monomios.");
                return;
            }

            // Booleana como condicón de salida del bucle
            bool added = false; // Pongo added en vez de añadido xq no me gusta ñ en código

            for (int i = 0; i <= p.nMons && !added; i++) // Recorremos array mientras no se haya insertado el mon.
            {
                if (m.exp == p.mon[i].exp) // Si los exp. son iguales
                {
                    p.mon[i].coef = m.coef + p.mon[i].coef; // Sumamos los coefs.
                    if (p.mon[i].coef == 0) // Pero si su suma da 0
                    {
                        DesplazaIzq(ref p, i); // Hay que eliminarlo
                        p.nMons--; // Y eso significa q nos cargamos un monomio
                    }
                    added = true; // Salimos
                }
                else if (m.exp < p.mon[i].exp) // Recorremos mientras sea menor el exp. del m
                {
                    DesplazaDer(ref p, i); // Cuando lo encuentra tenemos q dejarle hueco
                    p.mon[i] = m; // El hueco vacío ahora se lo asignamos
                    p.nMons++;
                    added = true;
                }
            }

            if (!added) // Si no se ha añadido (su exp es >>), lo añadimos en la última posicón
            {
                p.mon[p.nMons] = m;
                p.nMons++;
            }
        }

        static void DesplazaDer(ref Polinomio p, int pos) // Hacemos hueco a un mon. nuevo
        {
            // Lo primero si no hay hueco en el array, error y salimos
            if (p.nMons == p.mon.Length)
            {
                Console.WriteLine("ERROR: no hay hueco");
                return;
            }

            // Desde la derecha
            for (int i = p.nMons; i > pos; i--)
            {
                p.mon[i] = p.mon[i - 1];
            }
        }

        static void DesplazaIzq(ref Polinomio p, int pos) // Nos cargamos a un mon. del array
        {
            // Desde la izquierda
            for (int i = pos; i < p.nMons - 1; i++)
            {
                p.mon[i] = p.mon[i + 1];
            }
        }

        static void LeeMonomio(out Monomio m) // Sacado de clase + mi mejora
        {
            do
            {
                Console.Write("Coeficiente (distinto de 0): ");
                m.coef = double.Parse(Console.ReadLine());
            } while (m.coef == 0);

            do
            {
                Console.Write("Exponente (>= 0): ");
                m.exp = int.Parse(Console.ReadLine());
            } while (m.exp < 0);
        }

        static void Lee(out Polinomio p)
        {
            p.mon = new Monomio[N];
            p.nMons = 0;

            int n;
            Console.Write("Número de monomios: ");
            n = int.Parse(Console.ReadLine());

            for (int i = 0; i < n; i++)
            {
                Monomio m; // Cada vuelta creo un nuevo monomio
                LeeMonomio(out m);
                Inserta(m, ref p); // Aquí se hace todo el trabajo: ordenarlo, sumar/restar nMons...
            }
        }

        static void Escribe(Polinomio p)
        {
            for (int i = 0; i < p.nMons; i++)
            {
                double coef = p.mon[i].coef; // Variables xq es un coñazo escribir esto tol rato sino
                int exp = p.mon[i].exp;

                // Signo
                if (i == 0) // Si es el primero solo lleva signo si es negativo
                {
                    if (coef < 0) Console.Write("-");
                }
                else
                {
                    if (coef < 0) Console.Write(" - ");
                    else Console.Write(" + ");
                }

                // Val. absolutos de coefs. xq el signo ya lo ponemos arriba
                double coefAbs = Math.Abs(coef);
                if (exp == 0) Console.Write(coefAbs); // Si exponente es 0, lo escribimos igualmente
                else if (coefAbs != 1) Console.Write(coefAbs); // Si coef != [1,-1], lo escribimos

                // Exponente
                if (exp == 1) Console.Write("x"); // Sin elevao xq es 1
                else if (exp > 1) Console.Write($"x^{exp}");
            }
            Console.WriteLine();
        }

        static int Grado(Polinomio p)
        {
            return p.mon[p.nMons - 1].exp;
        }

        static Polinomio Suma(Polinomio p1, Polinomio p2) // Más fácil de lo que parece xq tenemos Inserta
        {
            Polinomio suma;
            suma.mon = new Monomio[N]; // Máx. de monomios permitidos siempre es N
            // Vamos a copiar p1, y luego insertarle p2
            suma.nMons = p1.nMons; // nMons
            for (int j = 0; j < p1.nMons; j++) // monomios del array
            {
                suma.mon[j] = p1.mon[j];
            }

            // Ahora ya insertamos p2 en suma (copia de p1)
            for (int i = 0; i < p2.nMons; i++)
            {
                Inserta(p2.mon[i], ref suma);
            }
            // Y si hay algún error de desbrodamiento ya lo gestiona Inserta
            return suma;
        }

        static Polinomio Resta(Polinomio p1, Polinomio p2) // Lo mismo que suma, pero con un toque
        {
            Polinomio resta;
            resta.mon = new Monomio[N];
            resta.nMons = p1.nMons; // nMons
            for (int j = 0; j < p1.nMons; j++) // monomios del array
            {
                resta.mon[j] = p1.mon[j];
            }

            // Aquí está la diferencia entre Suma y Resta
            for (int i = 0; i < p2.nMons; i++)
            {
                Monomio m;
                m.coef = -p2.mon[i].coef; // Invierto SOLO coeficiente
                m.exp = p2.mon[i].exp;
                Inserta(m, ref resta);
            }
            return resta;
        }

        static double Evalua(Polinomio p, double v)
        {
            double resultado = 0;

            for (int i = 0; i < p.nMons; i++)
            {
                resultado = resultado + (p.mon[i].coef * Math.Pow(v, p.mon[i].exp));
            }
            return resultado;
        }

        static Polinomio Multiplica(Polinomio p1, Polinomio p2)
        {
            Polinomio p3;
            p3.mon = new Monomio[N];
            p3.nMons = 0;

            // Multiplicamos todos los monomios de p1 por todos los de p2 con un doble bucle,
            // es como distributiva
            for (int i = 0; i < p1.nMons; i++) // Cogemos monomio a monomio p1
            {
                for (int j = 0; j < p2.nMons; j++) // Y monomio a monomio p2
                {
                    Monomio m;
                    m.coef = p1.mon[i].coef * p2.mon[j].coef;
                    m.exp = p1.mon[i].exp + p2.mon[j].exp;
                    Inserta(m, ref p3); // De todo lo chungo ya se encarga Inserta com siempre
                }
            }
            return p3;
        }

        static bool Iguales(Polinomio p1, Polinomio p2)
        {
            // Antes de meternos en el bucle podemos hacer está comprobación rápida
            if (p1.nMons != p2.nMons) return false;

            // Como están ordenados comparamos 1 a 1
            for (int i = 0; i < p1.nMons; i++)
            {
                // Si el coeficiente o el exponente es diferente no son iguales
                if (p1.mon[i].coef != p2.mon[i].coef || p1.mon[i].exp != p2.mon[i].exp) return false;
            }
            // Si no se cumple ningún condicional de arriba, son iguales
            return true;
        }

        static void Extiende(ref Polinomio p)
        {
            // Creamos un array nuevo del doble de tamaño que el actual
            Monomio[] nuevo = new Monomio[p.mon.Length * 2];
            // Copiamos los monomios existentes al nuevo array, hasta nMons
            for (int i = 0; i < p.nMons; i++)
            {
                nuevo[i] = p.mon[i];
            }
            // Sustituimos el array viejo por el nuevo
            p.mon = nuevo;
        }


        // GuardaLee de toda la vida
        static void Salva(Polinomio p)
        {
            StreamWriter file = new StreamWriter("salida.txt");

            file.WriteLine(p.nMons);
            for (int i = 0; i < p.nMons; i++)
            {
                file.WriteLine(p.mon[i].coef);
                file.WriteLine(p.mon[i].exp);
            }
            file.Close();
        }
        static void Restaura(out Polinomio p)
        {
            StreamReader file = new StreamReader("salida.txt");

            int n = int.Parse(file.ReadLine()); // Warning. nMons del archivo lo pongo aquí

            // Tenemos que construir un polinomio desde 0 con Inserta
            p.nMons = 0;
            p.mon = new Monomio[N];

            for (int i = 0; i < n; i++)
            {
                Monomio m;
                m.coef = double.Parse(file.ReadLine());
                m.exp = int.Parse(file.ReadLine());
                Inserta(m, ref p);
            }
            file.Close(); // Cerramos
        }


        // SIN HACER (los veía un poco useless)
        void Divide(Polinomio p1, Polinomio p2, Polinomio c, Polinomio r)
        {
            // Literalmente ni idea, es muy complejo matemáticamente.
            // No cae en examen ni de broma, es pérdida de tiempo pero puede
            // ser interesante sacarlo, (yo es q ni sé cómo se dividen polinomios)
        }
        Polinomio Convierte(Polinomio p)
        {
            Polinomio convertido;
            convertido.nMons = p.nMons;
            convertido.mon = new Monomio[N];

            return convertido;
        }
    }
}