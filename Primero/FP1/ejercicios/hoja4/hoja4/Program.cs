//Juan Sánchez Arias
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace hoja4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            //Ejercicio 1
            int i = 1; //Variable auxiliar para realizar incremento
            int res = 1; //Variable utilizada para el resultado
            Console.Write("Escribe el número el cual quieres factorizar: ");
            int num = int.Parse(Console.ReadLine());

            //Comprobamos que el usuario introduzca un número positivo
            if (num < 0)
            {
                Console.WriteLine("Por favor introduce un número válido");
            }
            else
            {
                while (i <= num) //Mientras que el número introducido sea mayor o igual que 1
                {
                    res *= i; //Multiplicamos por la variable incremental: i, i+1, (i+1)+1...
                    i++;//Incrementa hasta que llega al número introducido
                }

                Console.WriteLine("El número factorizado es: " + res);
            }
            */



            /*
            //Ejercicio 2
            int i = 0; //Variable que utilizaremos como contador
            Console.Write("Introduce el número: ");
            int num = int.Parse(Console.ReadLine());

            if (num < 0)
            {
                Console.WriteLine("Por favor introduce un número válido");
            }
            else
            {
                while (num > 0)//Mientras sea mayor que 0, es decir, mientras queden dígitos válidos
                {
                    int dig = num % 10;//El resto al dividir entre 10 es el último dígito de la cifra
                    if (dig == 3)//Comprobamos si es un 3 y si lo es, sumamos 1 al contador.
                    {
                        i++;
                    }
                    num = num / 10;//Para finalizar eliminamos el último dígito para comenzar otra vuelta pero cogiendo el siguiente dígito.
                }
                Console.WriteLine("El número de 3 en total en el número introducido es: " + i);
            }
            */



            /*
            //Ejercicio 3
            Console.Write("Introduce el número: ");
            int num = int.Parse(Console.ReadLine());
            bool hay = true;

            if (num < 0)
            {
                Console.WriteLine("Por favor introduce un número válido");
            }
            else
            {
                while (num > 0)//Mientras sea mayor que 0, es decir, mientras queden dígitos válidos
                {
                    int dig = num % 10;//El resto al dividir entre 10 es el último dígito de la cifra
                    if (dig == 3)//Comprobamos si es un 3 y si lo es, acabamos el while.
                    {
                        hay = true;
                        break;
                    }
                    else
                    {
                        hay = false;
                    }
                    num = num / 10;//Para finalizar eliminamos el último dígito para comenzar otra vuelta pero cogiendo el siguiente dígito.
                }
                if (hay == true)
                {
                    Console.WriteLine("Hay 3");
                }
                else
                {
                    Console.WriteLine("No hay 3");
                }
            }
            */



            /*
            //Ejercicio 4
            string invertido = "";
            Console.Write("Por favor introduce un número que quieras invertir: ");
            int num = int.Parse(Console.ReadLine());

            if (num == 0)
            {
                invertido = "0";
            }

            while (num < 0)
            {
                Console.Write("Por favor introduce un número válido: ");
                num = int.Parse(Console.ReadLine());
            }

            while (num > 0)
            {
                invertido = invertido + num % 10;
                num = num / 10;
            }

            Console.WriteLine("El número invertido es: " + invertido);
            */



            /*
            //Ejercicio 5
            string dig = "";
            Console.Write("Introduce el número: ");
            int num = int.Parse(Console.ReadLine());
            int orig = num;

            while (num < 0)
            {
                Console.WriteLine("Por favor introduce un número válido");
                num = int.Parse(Console.ReadLine());
            }

            if (num == 0)
            {
                dig = "0";
            }

            while (num > 0)
            {
                dig += num % 10;
                num /= 10;
            }

            if (Convert.ToInt32(dig) == orig)
            {
                Console.WriteLine("Es capicúa");
            }
            else
            {
                Console.WriteLine("No es capicúa.");
            }
            */



            /*
            //Ejercicio 6
            Console.Write("Introduce tu número entero positivo: ");
            int num = int.Parse(Console.ReadLine());

            divide3(num);
            divide11(num);

            //método para comprobar divisibilidad entre 3
            static void divide3(int num)
            {
                int suma = 0;
                while (num > 0)
                {
                    suma = suma + (num % 10); //voy sumando dígito a dígito
                    num = num / 10;
                }
                if (suma > 9)
                {
                    suma = (suma % 10) + (suma / 10); //si me queda un número de 2 dígitos, volvemos a hacer lo mismo
                }

                if (suma % 3 == 0) //escribimos si es divisible o no
                {
                    Console.WriteLine($"EL numero final {suma} ES divisble entre 3");
                }
                else
                {
                    Console.WriteLine($"EL numero final {suma} NO ES divisble entre 3");
                }
            }

            //método para comprobar divisibilidad entre 11
            static void divide11(int num) 
            {
                int sumaPar = 0, sumaImpar = 0; //en vez de ir sumando todos los dígitos, ahora vamos a ir dividiéndolos en pares e impares, por lo que necesitamos 2 variables
                bool par = true;
                while (num > 0)
                {
                    if (par)
                    {
                        sumaPar = sumaPar + (num % 10);
                    }
                    else
                    {
                        sumaImpar = sumaImpar + (num % 10);
                    }

                    num = num / 10;
                    par = !par; //uno par, el siguiente impar, luego par, impar, etc...
                }
                int sumaF = sumaPar - sumaImpar;

                if (sumaF > 9 || sumaF < -9) //añadimos en este la condición de <-9 ya que hay restas y se puede ir a negativo
                {
                    sumaF = (sumaF / 10) - (sumaF % 10);
                }

                if (sumaF % 11 == 0)
                {
                    Console.WriteLine($"EL numero final {sumaF} ES divisble entre 11");
                }
                else
                {
                    Console.WriteLine($"EL numero final {sumaF} NO ES divisble entre 11");
                }
            }
            */



            /*
            //Ejercicio 7
            Console.Write("Introduce n: ");
            int n = int.Parse(Console.ReadLine());

            while (n < 0)
            {
                Console.Write("Por favor introuduce un número válido (n > 0): ");
                n = int.Parse(Console.ReadLine());
            }

            Console.WriteLine($"Las ternas de (a, b, c) hasta {n} son:");
            calculoTernas(n);

            static void calculoTernas(int n)
            {
                for (int a = 1; a <= n; a++) //empezamos en a=1, hasta n
                {
                    for (int b = a; b <= n; b++) // b comienza en a para evitar redundancias, hasta b
                    {
                        int AporB = a * a + b * b;
                        int c = (int)Math.Sqrt(AporB); // para sacar c, simplemente hacemos el cuadrado de a*a + b*b

                        if (c * c == AporB && c <= n)
                        {
                            Console.WriteLine($"({a}, {b}, {c})");
                        }
                    }
                }
            }
            */



            /*
            //Ejercicio 8
            Console.Write("Por favor introduce la altura del árbol: ");
            int altura = int.Parse(Console.ReadLine());
            int fila = 1; //siempre empezamos por la primera fila

            while (fila <= altura) // mientras no hagamos todas las filas hasta la altura indicada
            {
                int blancos = altura - fila; // el primer asterisco se escribirá dsps de n espacios (blancos), luego n-1 espacios en la siguiente vuelta, n-2, etc...
                while (blancos > 0) //blancos iniciales de la fila
                {
                    Console.Write(" ");
                    blancos--;
                }
                int asterisco = fila * 2 - 1; // formula asteriscos por fila = altura * 2 - 1:   1a fila: 1, 2a fila: 3, etc...
                while (asterisco > 0) //asteriscos de la fila
                {
                    Console.Write("*");
                    asterisco--;
                }
                Console.WriteLine(""); //nueva fila, salto de línea en consola
                fila++; //pasamos de fila tb en código
            }
            */



            /*
            //Ejercicio 9
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    Console.Write((i * j).ToString().PadLeft(4));
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            //este ejercicio habría que repetirlo para que quede igual que en el enunciado
            */



            /*
            //Ejercicio 10
            Console.Write("Introduce el decimal que quieres convertir a binario: ");
            int n = int.Parse(Console.ReadLine());
            string bin = "";

            while (n < 0)
            {
                Console.WriteLine("Por favor introduce n > 0: ");
                n = int.Parse(Console.ReadLine());
            }
            if (n == 0)
            {
                bin = "0";
            }
            while (n > 0)
            {
                int dig = n % 2;
                bin = dig + bin;
                n = n / 2;
            }
            Console.WriteLine("La representación en binario del número decimal introducido es: " + bin);
            */



            /*
            //Ejercicio 11
            Console.Write("Introduce el decimal que quieres convertir: ");
            int n = int.Parse(Console.ReadLine());
            Console.Write("Introduce la representación de base: ");
            int b = int.Parse(Console.ReadLine());
            string bin = "";
            string dig = "0123456789ABCDEF";

            while (n < 0)
            {
                Console.WriteLine("Por favor introduce n > 0: ");
                n = int.Parse(Console.ReadLine());
            }
            while (b < 2 || b > 16)
            {
                Console.WriteLine("Por favor introduce una base válida (2 <= b >= 16): ");
                b = int.Parse(Console.ReadLine());
            }
            if (n == 0)
            {
                bin = "0";
            }

            while (n > 0)
            {
                int resto = n % b;
                bin = dig[resto] + bin;
                n = n / b;
            }
            Console.WriteLine("La representación en binario del número decimal introducido es: " + bin);
            */



            /*
            //Ejercicio 12
            Console.Write("Introduce n: ");
            int n = int.Parse(Console.ReadLine());
            int primoCount = 0;// empezamos la cuenta de números divisibles en 0

            if (n % 2 == 1) //comprobamos que es impar
            {
                for (int i = 1; i <= Math.Sqrt(n); i++) //desde i = 1, hasta la raíz de n
                {
                    if (n % i == 0)
                    {
                        primoCount++; //si ese n es divisible entre 1, la cuenta +1
                    }
                }
                if (primoCount <= 2) //si la cuenta es 2 o menor, es primo
                {
                    Console.WriteLine($"Tu número {n} es primo usando divisores hasta {Math.Sqrt(n)}");
                }
                else { Console.WriteLine($"Tu número {n} NO es primo usando divisores hasta {Math.Sqrt(n)}"); }
            }
            */
        }
    }
}