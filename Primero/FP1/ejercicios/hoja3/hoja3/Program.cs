//Juan Sánchez Arias
using System;
using System.Net;
using System.Numerics;
using System.Runtime.CompilerServices;
namespace hoja3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            //Ejercicio 1
            int x, y, z, w, max, min;
            Console.Write("Primer entero: ");
            x = int.Parse(Console.ReadLine());
            Console.Write("Segundo entero: ");
            y = int.Parse(Console.ReadLine());
            Console.Write("Tercer entero: ");
            z = int.Parse(Console.ReadLine());
            Console.Write("Cuarto entero: ");
            w = int.Parse(Console.ReadLine());
            Console.WriteLine("");

            max = Math.Max(Math.Max(x, y), Math.Max(z, w));
            Console.WriteLine("El número más grande es: " + max);

            min = Math.Min(Math.Min(x, y), Math.Min(z, w));
            Console.WriteLine("El número más pequeño es: " + min);
            */



            /*
            //Ejercicio 2
            float x;
            Console.Write("Nota alumno (si es decimal introduce coma): ");
            x = float.Parse(Console.ReadLine());

            bool valido;
            if (x >= 0 && x <= 10) valido = true;
            else valido = false;

            if (valido)
            {
                if (x == 10)
                {
                    Console.WriteLine("MH");
                }
                else if (x < 10 && x >= 9)
                {
                    Console.WriteLine("SB");
                }
                else if (x < 9 && x >= 7)
                {
                    Console.WriteLine("NT");
                }
                else if (x < 7 && x >= 5)
                {
                    Console.WriteLine("AP");
                }
                else
                {
                    Console.WriteLine("SS");
                }
            }
            else { Console.WriteLine("Introduce un número válido."); }
            */



            /*
            //Ejercicio 6
            Console.WriteLine("Ecuación de segundo grado tipo ax^2 + bx + c:");
            Console.Write("Introduce a: ");
            float a = float.Parse(Console.ReadLine());
            Console.Write("Introduce b: ");
            float b = float.Parse(Console.ReadLine());
            Console.Write("Introduce c: ");
            float c = float.Parse(Console.ReadLine());


            if (a == 0)
            {
                Console.WriteLine("La ecuación no tiene una solución válida, ya que estamos dividiendo entre 0.");
            }
            else
            {
                double discriminante = b * b - 4 * a * c;

                if (discriminante >= 0)
                {
                    double totalA = (discriminante - b) / (2 * a);
                    double totalB = (-discriminante - b) / (2 * a);

                    Console.WriteLine("El primer resultado a la ecuación es: " + totalA.ToString("F2"));
                    Console.WriteLine("El segundo resultado a la ecuación es: " + totalB.ToString("F2"));
                }
                else
                {
                    // La clase complex usa la forma rectangular para representar complejos:
                    // a + bi, donde a es la parte real y b la imaginaria
                    // ej: raíz de 16 será 4 + 0i,
                    // en cambio raíz de -16 = 0 + 4i, por lo que el número es puramente imaginario
                    Complex imaginario = Complex.Sqrt(new Complex(discriminante, 0));
                    // En este caso ponemos el 0 como parte imaginaria para indicar que estamos
                    // tratando con un número real (discriminante)

                    Complex totalA = (imaginario - b) / (2 * a);
                    Complex totalB = (-imaginario - b) / (2 * a);

                    Console.WriteLine("El primer resultado a la ecuación es el número imaginario: " + totalA.ToString("F2"));
                    Console.WriteLine("El segundo resultado a la ecuación es el número imaginario: " + totalB.ToString("F2"));
                }
                // NOTA:
                // Podemos descomponer el complejo en parte real e imaginaria si es necesario:
                // REAL: Console.WriteLine(raizN.Real);
                // IMAGINARIO: Console.WriteLine(raizN.Imaginary);
            }
            */



            /*
            //Ejercicio 7
            Console.Write("Nota del examen: ");
            float examen = float.Parse(Console.ReadLine());
            Console.Write("Nota de la primera práctica: ");
            float p1 = float.Parse(Console.ReadLine());
            Console.Write("Nota de la segunda práctica: ");
            float p2 = float.Parse(Console.ReadLine());
            Console.Write("Nota del laboratorio: ");
            float lab = float.Parse(Console.ReadLine());

            bool todoAprobado = (p1 >= 5 && p2 >= 5 && examen >= 5);

            if (todoAprobado)
            {
                float pTotal = (p1 + p2) / 2;
                float total = examen * 0.7f + pTotal * 0.2f + lab * 0.1f;
                Console.WriteLine("La nota final de FP es: " + total.ToString("F1"));
            }
            else
            {
                Console.WriteLine("Algo tienes que recuperar, a junio macho.");
            }
            */


            /*
            //Ejercicio 8
            Console.Write("Por favor ingresa el dígito hexadecimal [0-9][A-F]: ");
            char hex = Console.ReadKey().KeyChar;

            int dec = HexToDec(hex);

            Console.WriteLine("");
            Console.WriteLine("");

            if (dec == -1)
            {
                Console.WriteLine("ERROR: Introduce un carácter válido.");
            }
            else
            {
                Console.WriteLine($"El carácter hexadecimal {hex} es {dec} en decimal.");
            }

            static int HexToDec(char hex)
            {
                if (hex >= 'a' && hex <= 'f')
                {
                    return hex - 'a' + 10;
                }

                else if (hex >= '0' && hex <= '9')
                {
                    return hex - '0';
                }

                else
                {
                    return -1; //-1 para saber que da error;
                }
            }
            */




            //Ejercicio 9
            float uno = 0, dos = 0, tres = 0, cuatro = 0, total = 0, agua;

            do
            {
                Console.Write("Introduce consumo: ");
                agua = float.Parse(Console.ReadLine());
            } while (agua < 0);

            if (agua < 101)
            {
                uno = agua;
            }
            else if (agua < 501)
            {
                uno = 100;
                dos = agua - 100;
            }
            else if (agua < 1001)
            {
                uno = 100;
                dos = 400;
                tres = agua - 500;
            }
            else
            {
                uno = 100;
                dos = 400;
                tres = 500;
                cuatro = agua - 1000;
            }

            total = uno * 0.15f + dos * 0.2f + tres * 0.35f + cuatro * 0.8f;
            double totalr = Math.Round(total, 2);

            Console.WriteLine("");
            Console.WriteLine("Consumo desglosado: " + uno + " * 0.15 + " + dos + " * 0.20 + " + tres + " * 0.35 + " + cuatro + " * 0.80");
            Console.WriteLine("A pagar: " + totalr + " euros");




            /*
            //Ejercicio 10
            Console.Write("Introduce tipo de pizza, vegetariana o normal: ");
            string pizzaTipo = Console.ReadLine()?.ToLower();
            Console.WriteLine("");
            string ingrediente = "";

            while (pizzaTipo != "vegetariana" && pizzaTipo != "normal")
            {
                Console.Write("Por favor, introduce vegetariana o normal: ");
                pizzaTipo = Console.ReadLine()?.ToLower();
            }

            if (pizzaTipo == "vegetariana")
            {
                Console.WriteLine("Has elegido pizza vegetariana, selecciona tu ingrediente entre:");
                Console.WriteLine("- Pimiento");
                Console.WriteLine("- Tofu");
                Console.WriteLine("");
                Console.Write("Introduce el ingrediente: ");
                ingrediente = Console.ReadLine()?.ToLower();

                while (ingrediente != "tofu" && ingrediente != "pimiento")
                {
                    Console.Write("Por favor, selecciona un ingrediente válido (tofu o pimiento): ");
                    ingrediente = Console.ReadLine()?.ToLower();
                }
            }

            else if (pizzaTipo == "normal")
            {
                Console.WriteLine("Has elegido pizza normal, selecciona tu ingrediente entre:");
                Console.WriteLine("- Jamón");
                Console.WriteLine("- Peperoni");
                Console.WriteLine("- Salmón");
                Console.WriteLine("");
                Console.Write("Introduce el ingrediente: ");
                ingrediente = Console.ReadLine()?.ToLower();

                while (ingrediente != "jamón" && ingrediente != "peperoni" && ingrediente != "salmón")
                {
                    Console.Write("Por favor, selecciona un ingrediente válido (jamón, peperoni o salmón): ");
                    ingrediente = Console.ReadLine()?.ToLower();
                }
            }

            Console.WriteLine($"Gracias por tu pedido! Tu pizza {pizzaTipo} con tomate, mozzarella y {ingrediente} saldrá en breves.");
            */
        }
    }
}

