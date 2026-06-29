namespace hoja3
{
    internal class Program
    {
        static Random rnd = new Random();

        // Ejercicio 1
        class Complejo
        {
            private float Re;
            private float Im;

            public Complejo() // Constructor sin argumentos
            {
                Re = 0;
                Im = 0;
            }

            public Complejo(float re, float im)  // Constructor con argumentos
            {
                Re = re;
                Im = im;
            }

            // Sobrecarga de operadores: STATIC, operator (signo), return operación
            // Suma
            public static Complejo operator +(Complejo num1, Complejo num2)
            {
                return new Complejo(num1.Re + num2.Re, num1.Im + num2.Im);
            }

            // Resta
            public static Complejo operator -(Complejo num1, Complejo num2)
            {
                return new Complejo(num1.Re - num2.Re, num1.Im - num2.Im);
            }

            // Multiplicación
            public static Complejo operator *(Complejo num1, Complejo num2)
            {
                // (a + bi) · (c + di) = (ac − bd) + (ad + bc)i
                Complejo mult = new Complejo();

                mult.Re = (num1.Re * num2.Re - num1.Im * num2.Im);
                mult.Im = (num1.Re * num2.Im + num1.Im * num2.Re);

                return mult;
            }

            public void Lee()
            {
                Console.Write("Introduce la parte real del primer complejo: ");
                Re = float.Parse(Console.ReadLine());
                Console.Write("Introduce la parte imaginaria del primer complejo: ");
                Im = float.Parse(Console.ReadLine());
            }

            public void Escribe()
            {
                string a = "";
                if (Im < 0) { a = " - "; }
                else a = " + ";
                Console.WriteLine("El complejo resultado es: " + Re + a + Math.Abs(Im) + "i");
            }
        }

        // Ejercicio 2
        struct Monomio
        {
            public double coef;
            public int exp;
        }
        class Polinomio
        {
            private const int N = 100; // tamaño de los arrays de monomios
            private Monomio[] mon; // array de monomios
            private int nMons; // num de monomios = primera pos libre en el array mon

            public Polinomio()
            {
                mon = new Monomio[N];
                nMons = 0;
            }

            public Polinomio Derivada()
            {
                Polinomio resultado = new Polinomio();
                int j = 0;

                for (int i = 0; i < nMons; i++)
                {
                    if (mon[i].exp != 0)
                    {
                        Monomio m;
                        m.coef = mon[i].coef * mon[i].exp;
                        m.exp = mon[i].exp - 1;
                        resultado.Inserta(m);
                    }
                }
                return resultado;
            }

            public void Inserta(Monomio m)
            {
                if (nMons >= N)
                {
                    Console.WriteLine("ERROR: Se ha alcanzado el límite de monomios.");
                    return;
                }

                bool added = false;

                for (int i = 0; i <= nMons && !added; i++)
                {
                    if (m.exp == mon[i].exp)
                    {
                        mon[i].coef = m.coef + mon[i].coef;
                        if (mon[i].coef == 0)
                        {
                            DesplazaIzq(i);
                            nMons--;
                        }
                        added = true;
                    }
                    else if (m.exp < mon[i].exp)
                    {
                        DesplazaDer(i);
                        mon[i] = m;
                        nMons++;
                        added = true;
                    }
                }

                if (!added)
                {
                    mon[nMons] = m;
                    nMons++;
                }
            }
            private void DesplazaDer(int pos)
            {
                if (nMons == mon.Length)
                {
                    Console.WriteLine("ERROR: no hay hueco");
                    return;
                }

                for (int i = nMons; i > pos; i--)
                {
                    mon[i] = mon[i - 1];
                }
            }
            private void DesplazaIzq(int pos)
            {
                for (int i = pos; i < nMons - 1; i++)
                {
                    mon[i] = mon[i + 1];
                }
            }

            private static void LeeMonomio(out Monomio m)
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

            public void Lee()
            {
                int n;
                Console.Write("Número de monomios: ");
                n = int.Parse(Console.ReadLine());

                for (int i = 0; i < n; i++)
                {
                    Monomio m;
                    LeeMonomio(out m);
                    Inserta(m);
                }
            }

            public void Escribe()
            {
                for (int i = 0; i < nMons; i++)
                {
                    double coef = mon[i].coef;
                    int exp = mon[i].exp;

                    // Signo
                    if (i == 0)
                    {
                        if (coef < 0) Console.Write("-");
                    }
                    else
                    {
                        if (coef < 0) Console.Write(" - ");
                        else Console.Write(" + ");
                    }

                    double coefAbs = Math.Abs(coef);
                    if (exp == 0) Console.Write(coefAbs);
                    else if (coefAbs != 1) Console.Write(coefAbs);

                    if (exp == 1) Console.Write("x");
                    else if (exp > 1) Console.Write($"x^{exp}");
                }
                Console.WriteLine();
            }

            public int Grado()
            {
                return mon[nMons - 1].exp;
            }

            public static Polinomio operator +(Polinomio p1, Polinomio p2)
            {
                Polinomio suma = new Polinomio();
                for (int j = 0; j < p1.nMons; j++)
                    suma.Inserta(p1.mon[j]);
                for (int i = 0; i < p2.nMons; i++)
                    suma.Inserta(p2.mon[i]);
                return suma;
            }

            public static Polinomio operator -(Polinomio p1, Polinomio p2)
            {
                Polinomio resta = new Polinomio();
                for (int j = 0; j < p1.nMons; j++)
                    resta.Inserta(p1.mon[j]);
                for (int i = 0; i < p2.nMons; i++)
                {
                    Monomio m;
                    m.coef = -p2.mon[i].coef;  // invertimoss
                    m.exp = p2.mon[i].exp;
                    resta.Inserta(m);
                }
                return resta;
            }

            public bool Iguales(Polinomio otro)
            {
                if (nMons != otro.nMons) return false;

                for (int i = 0; i < nMons; i++)
                {
                    if (mon[i].coef != otro.mon[i].coef || mon[i].exp != otro.mon[i].exp) return false;
                }
                return true;
            }
            // Llamar este método en el main: bool iguales = p1.Iguales(p2);

            public void Extiende()
            {
                Monomio[] nuevo = new Monomio[mon.Length * 2];
                for (int i = 0; i < nMons; i++)
                {
                    nuevo[i] = mon[i];
                }
                mon = nuevo;
            }


            // GuardaLee de toda la vida
            public void Salva()
            {
                StreamWriter file = new StreamWriter("salida.txt");

                file.WriteLine(nMons);
                for (int i = 0; i < nMons; i++)
                {
                    file.WriteLine(mon[i].coef);
                    file.WriteLine(mon[i].exp);
                }
                file.Close();
            }
            public void Restaura()
            {
                StreamReader file = new StreamReader("salida.txt");

                int n = int.Parse(file.ReadLine());

                for (int i = 0; i < n; i++)
                {
                    Monomio m;
                    m.coef = double.Parse(file.ReadLine());
                    m.exp = int.Parse(file.ReadLine());
                    Inserta(m);
                }
                file.Close();
            }
        }

        // Ejercicio 3
        class Matriz
        {
            private int fils;
            private int cols;
            private int[,] mat;

            public Matriz(int n, int m)
            {
                fils = n;
                cols = m;
                mat = new int[n, m];
            }

            public void Inicializa(int[] elems)
            {
                int aux = 0;
                for (int i = 0; i < fils; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        mat[i, j] = elems[aux];
                        aux++;
                    }
                }
            }

            public static Matriz operator +(Matriz a, Matriz b)
            {
                Matriz suma = new Matriz(a.fils, a.cols);

                for (int i = 0; i < a.fils; i++)
                {
                    for (int j = 0; j < a.cols; j++)
                    {
                        suma.mat[i, j] = a.mat[i, j] + b.mat[i, j];
                    }
                }
                return suma;
            }
            // el resto de métodos es más de lo mismo

            public void EscribeMatriz()
            {
                for (int i = 0; i < fils; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        Console.Write(mat[i, j]);
                        Console.Write(" ");
                    }
                    Console.WriteLine();
                }
            }
        }


        static void Main(string[] args)
        {
            // Main del ejercicio 1
            /*
            Complejo a = new Complejo();
            Complejo b = new Complejo();
            Complejo c; // No hace falta inicializarlo xq luego es a + b

            // Complejo factor 1
            a.Lee();
            // Complejo factor 2
            b.Lee();

            // Suma
            c = a + b;

            c.Escribe();
            */

            // Main del ejericico 2
            /*
            // Declarar
            Polinomio p1 = new Polinomio();
            Polinomio p2 = new Polinomio();
            // Pedirlos por consola
            p1.Lee();
            // p2.Lee();
            // Operaciones
            Polinomio p3 = p1.Derivada();
            // Textotexto
            Console.WriteLine();
            Console.Write("Resultado: ");
            p3.Escribe();
            */

            // Main del ejericico 3
            /*
            int[] arrayEj = new int[16];
            int[] arrayEj2 = new int[16];
            for (int i = 0; i < arrayEj.Length; i++)
            {
                arrayEj[i] = rnd.Next(1, 5);
                arrayEj2[i] = rnd.Next(1, 5);
            }

            Matriz u = new Matriz(3, 3);
            u.Inicializa(arrayEj);
            Matriz v = new Matriz(3, 3);
            v.Inicializa(arrayEj2);
            Matriz res;

            res = u + v;
            u.EscribeMatriz();
            Console.WriteLine();
            v.EscribeMatriz();
            Console.WriteLine();
            res.EscribeMatriz();
            */
        }
    }
}