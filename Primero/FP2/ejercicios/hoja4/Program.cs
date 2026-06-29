namespace hoja4
{
    internal class Program
    {
        // Ejercicios 1 y 2
        class Rational
        {
            int num;
            int den;

            public Rational(int num, int den) // Constructor
            {
                // EJERCICIO 1, SIN SETTER
                /*
                if (den == 0) // El error de denominador negativo lo corregimos al normalizar
                {
                    Console.WriteLine("Error Rational: denominador igual a 0");
                    this.num = 0;
                    this.den = 1;
                    Console.WriteLine("Se ha creado el racional 0/1");
                }
                else
                {
                    this.num = num;
                    this.den = den;
                    Normalize();
                }*/

                // EJERCICIO 2
                // settea los datos en la centralita
                Num = num;
                Den = den;
            }

            private void Normalize()
            {
                if (den < 0) // Invertimos
                {
                    den = -den;
                    num = -num;
                }
                int m = MCD(Math.Abs(num), den); // num puede ser negativo al invertir
                num = num / m;
                den = den / m;
            }

            private int MCD(int a, int b)
            {
                int aux;
                while (b != 0)
                {
                    aux = b; // guardamos valor de b
                    b = a % b; // nuevo valor de b = resto de a/b
                    a = aux; // a toma el valor que tenía b al principio
                }
                return a;
            }

            public static Rational Lee()
            {
                Console.Write("Introduce un racional (num/den): ");
                string s = Console.ReadLine();
                string[] partes = s.Split('/');
                return new Rational(int.Parse(partes[0]), int.Parse(partes[1]));
            }

            public void Escribe()
            {
                Console.WriteLine(num + "/" + den);
            }

            // Ejercicio 2
            public int Num
            {
                get { return num; } // getter
                set { num = value; Normalize(); } // setter
            }
            public int Den
            {
                get { return den; }
                set
                {
                    if (value == 0) throw new Exception("ERROR: Denominador 0");
                    den = value;
                    Normalize();
                }
            }
            /* EJEMPLO DE USO EN MAIN
            Rational r = new Rational(1, 2);
            int n = r.Num;    // llama al getter, n = 1
            r.Num = 5;        // llama al setter, value = 5
            */


            // Operador - unario (para poder invertir directamente un racional)
            public static Rational operator -(Rational r)
            {
                return new Rational(-r.Num, r.Den);
            }
            // Suma
            public static Rational operator +(Rational a, Rational b)
            {
                return new Rational(a.Num * b.Den + b.Num * a.Den, a.Den * b.Den);
            }
            // Resta
            public static Rational operator -(Rational r1, Rational r2)
            {
                return r1 + (-r2);
            }
            // "Sobreescritura" del método ToString para poder escribir racionales directamente con Console.Write(c);
            // este método construye la cadena que queremos imprimir, que será el valor de retorno
            public override string ToString()
            {
                return $"Racional: {num}/{den}";
            }

            // Multiplicación, división, etc.
        }

        // Ejercicio 3
        class Coordenada
        {
            double x, y;

            public Coordenada() // vacía
            {
                x = 0; y = 0;
            }

            public Coordenada(double x, double y)
            {
                X = x;
                Y = y;
            }

            public double X
            {
                get { return x; }
                set { x = value; }
            }
            public double Y
            {
                get { return y; }
                set { y = value; }
            }

            public double DistanciaAlOrigen()
            {
                return Math.Sqrt(X * X + Y * Y);
            }

            public double DistanciaA(Coordenada otra)
            {
                double dx = otra.X - X;
                double dy = otra.Y - Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }

            public void Mover(double dx, double dy)
            {
                x += dx;
                y += dy;
            }

            public override string ToString()
            {
                return $"({x}, {y})";
            }
        }


        static void Main(string[] args)
        {
            // Ejercicios 1 y 2
            /*
            Rational r1 = Rational.Lee();
            Rational r2 = Rational.Lee();
            Rational r3 = r1 + r2;

            // Ejercicio 1 (método escribe): r3.Escribe();

            // Con el override ToString
            Console.WriteLine(r3);
            */

            // Ejercicio 3
            Coordenada cor1 = new Coordenada(4,5);
            Coordenada cor2 = new Coordenada(20,-2);

            Console.WriteLine("Coordenada 1: " + cor1);
            Console.WriteLine("Coordenada 2: " + cor2);
            Console.WriteLine("Distancia entre puntos: " + cor1.DistanciaA(cor2));
            Console.WriteLine("Distancia entre cor1 y origen: " + cor1.DistanciaAlOrigen());
            Console.WriteLine("Distancia entre cor2 y origen: " + cor2.DistanciaAlOrigen());
        }

    }
}