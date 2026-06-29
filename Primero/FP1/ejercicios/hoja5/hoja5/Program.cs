//Juan Sánchez Arias
using System;

namespace hoja5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            //Ejercicio 1
            Console.Write("Introduce el número que quieras factorizar: "); //pedimos n, que lo vamos a usar en el apartado 1 y 2
            int n = int.Parse(Console.ReadLine());

            Console.Write("Introduce la base de la potencia (a): ");
            int a = int.Parse(Console.ReadLine());
            Console.Write("Introduce el exponente de la potencia (b): ");
            int b = int.Parse(Console.ReadLine());
            Console.WriteLine();

            int PrimerPrimo = PrimFactor(n);
            Console.WriteLine($"El primer número primo divisible de {n} es: {PrimerPrimo}");

            int MaxExp = ExpFactor(n, PrimerPrimo);
            Console.WriteLine($"El máximo exponente 'e' tal que {PrimerPrimo}^e divide a {n} es: {MaxExp}");

            int ElevaRes = Eleva(a, b);
            Console.WriteLine($"El resultado de elevar {a} a la {b} es: {ElevaRes}");


            int PrimFactor(int n)
            {
                if (n % 2 == 0) // si n es par, el primer primo divisible siempre va a ser 2
                {
                    return 2;
                }

                else
                {
                    for (int i = 3; i <= n; i++) // desde el 3, hasta n
                    {
                        if (n % i == 0) // si el resto de n / i = 0, entonces i es el primer primo divisible, por reglas matemáticas generales suelen ser (2, 3, 5, 7, 11...)
                        {               // por ejemplo aunque sea divisble por 4, 6, 8... ya hemos dicho que el 2 es el primer primo, otro ejemplo: por 9, ya hemos dicho que es el 3, etc.
                            return i;
                        }
                    }
                }
                return n; //si aú así no tiene ninguno (imposible prácticamente), devolvemos el número
            }


            int ExpFactor(int n, int fact)
            {
                fact = PrimerPrimo; // fact es el primerprimo, como nos dicta el enunciado

                for (int e = n; e <= n; e--) // ahora fact^e, como queremos que sea el máximo empezamos desde arriba, desde n, y vamos restando
                {
                    if (n % Math.Pow(fact, e) == 0) // a la que encontremos un resto 0, tenemos nuestro exponente que estábamos buscando.
                    {
                        return e;
                    }
                }
                return -1; // devolvemos -1 para indicar un fallo, ya que aunque e no pueda ser ni 2, ni 3, etc. siempre como mínimo va a ser 1.
            }


            int Eleva(int a, int b)
            {
                int resultado = 1; // 1 porque si ponemos 0 sale 0 la multiplicación del bucle

                for (int i = 0; i < b; i++) // tantas vueltas como el número que hemos introducido en b
                {
                    resultado = resultado * a; // resultado se guarda cada vuelta que cada, entonces lo que hacemos multiplicar 1 vez + por a en cada vuelta
                }
                return resultado;
            }
            */



            /*
            //Ejercicio 2
            Console.Write("Introduce m (mayor o igual a 0): ");
            int m = int.Parse(Console.ReadLine());
            while (m < 0) // comprobación m >= 0
            {
                Console.Write("ERROR: Por favor introduce un número >= 0 para n: ");
                m = int.Parse(Console.ReadLine());
            }

            Console.Write("Introduce n (mayor o igual que m): ");
            int n = int.Parse(Console.ReadLine());
            while (m > n) // comprobación n >= m
            {
                Console.Write("ERROR: Recuerda que n tiene que ser mayor o igual que m, vuelve a introducirlo: ");
                n = int.Parse(Console.ReadLine());
            }


            int mFact = Factorial(m); // sacamos los factoriales de cada número
            int nFact = Factorial(n);

            int resultado = nFact / Denominador(mFact, nFact); // operación
            Console.WriteLine($"El resultado para el número combinatorio (n m) es: {resultado}");

            // Métodos
            int Denominador (int a, int b) // método para hacer el denominador y quede todo más limpio (m! * (n-m)!)
            {
                a = mFact;
                b = Factorial(n - m);

                int resultado = a * b;
                return resultado;
            }

            int Factorial(int numero) // método para hacer el factorial de numero
            {
                for (int i = numero - 1; i > 0; i--) //desde numero - 1, restando -1, hasta 1.   ej (m = 4): numero = m: numero = 4*3, 12*2, 24*1 = 24//
                {
                    numero = numero * i;
                }
                return numero;
            }
            */



            /*
            //Ejercicio 3
            Console.Write("Introduce el número del cual quieras saber la parte entera de su ráiz cuadrada: ");
            int n = int.Parse(Console.ReadLine());
            while (n < 0)
            {
                Console.Write("ERROR: Recuerda que n tiene que ser >= a 0: ");
                n = int.Parse(Console.ReadLine());
            }
            int raizEntera = EncontrarRaiz(n);
            Console.WriteLine(raizEntera);


            int EncontrarRaiz(int n)
            {
                int m = 0;
                while ((m + 1) * m < n) // empezamos proponiendo matemáticamente que (raíz de n) = m, lo que significa que n = m^2
                {                       // entonces simplemente vamos multiplicando m * m hasta que esa multiplicación da >= n
                    m++;                // En el momento que eso ocurre ya tenemos la parte entera de nuestra raíz (m)
                }                       // aunque le sumamos + 1 a una de las m para que se dé bien (ej: raíz de 17 su parte entera es 4, pero como
                if (m * m >= n)         // 4 * 4 = 16 < 17 no va a escribirlo y va a dar otra vuelta, entonces va a ser 5, enotnces ese + 1 es importante para esto)
                {
                    return m;
                }
                return m;
            }
            */



            /*
            //Ejercicio 4
            Console.Write("Introduce el número para hacer la dígito reducción: ");
            int n = int.Parse(Console.ReadLine());
            while (n < 0)
            {
                Console.Write("ERROR: Introduce un núemro mayor que 0: ");
                n = int.Parse(Console.ReadLine());
            }

            int nSuma = SumaDigs(n);
            int nSuma2 = SumaDigs(nSuma);

            if (nSuma < 10)
            {
                Console.WriteLine($"EL número final aplicando dígito reducción es: {nSuma}");
            }
            else
            {
                Console.WriteLine($"EL número final aplicando dígito reducción (2 veces) es: {nSuma2}");
            }


            int SumaDigs(int n)
            {
                int suma = 0;

                while (n > 0)
                {
                    int dig = n % 10;
                    suma = suma + dig;
                    n = n / 10;
                }
                return suma;
            }
            */



            //
            //Ejercicio 5
            Console.Write("Introduce a (mayor que 0): ");
            int a = int.Parse(Console.ReadLine());
            while (a < 1)
            {
                Console.Write("ERROR: Por favor introduce a > 0: ");
                a = int.Parse(Console.ReadLine());
            }

            Console.Write("Introduce b (mayor que 0): ");
            int b = int.Parse(Console.ReadLine());
            while (b < 1)
            {
                Console.Write("ERROR: Por favor introduce b > 0: ");
                b = int.Parse(Console.ReadLine());
            }
            Console.WriteLine();

            EjercicioEntero(a, b);

            //Métodos
            int MCD(int a, int b)
            {
                while (b != 0) //ej: a = 48, b = 18
                {
                    int aux = b;    // asignamos variable auxiliar a numero b (aux = 18)
                    b = a % b;      // b se convierte ahora en el resto de la división (12)
                    a = aux;        // ahora reasignamos aux (b antes de la división = 18) = a para la siguiente vuelta
                }                   // en la siguiente vuelta nos quedaría a = 18, b = 12, así hasta que b (el resto) = 0
                return a;
            }

            int MCM(int a, int b)
            {
                int mcm = (a * b) / MCD(a, b);
                return mcm;
            }

            bool Primos(int a, int b)
            {
                return MCD(a, b) == 1;
            }

            void EjercicioEntero(int a, int b)
            {
                int mcd = MCD(a, b);
                Console.WriteLine($"- El máximo común divisor de {a} y {b} es: {mcd}");
                int mcm = MCM(a, b);
                Console.WriteLine($"- El mínimo común múltiplo de {a} y {b} es: {mcm}");
                bool primos = Primos(a, b);
                Console.WriteLine($"- ¿Son primos entre sí {a} y {b}? (es decir, que el mcd de ambos es 1): {primos}");
            }
            //
        }
    }
}
