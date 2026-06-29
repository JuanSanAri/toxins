//Juan Sánchez Arias
using System.Runtime.CompilerServices;

namespace hoja2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            //Ejercicio 1
            Console.Write("Introduce las horas: ");
            int h = int.Parse(Console.ReadLine());
            Console.Write("Introduce los minutos: ");
            int m = int.Parse(Console.ReadLine());
            Console.Write("Introduce los segundos: ");
            int s = int.Parse(Console.ReadLine());

            s = h * 3600 + m * 60 + s;

            Console.WriteLine("El número total de segundos del tiempo introducido es: " + s);
            */



            /*
            //Ejercicio 2
            Console.Write("Introduce la edad del primer alumno: ");
            int a = int.Parse(Console.ReadLine());
            Console.Write("Introduce la edad del segundo alumno: ");
            int b = int.Parse(Console.ReadLine());
            Console.Write("Introduce la edad del tercer alumno: ");
            int c = int.Parse(Console.ReadLine());

            float media = (a + b + c) / 3;
            Console.WriteLine("La edad media de los alumnos es: " + media);
            */



            /*
            //Ejercicio 3
            //examen 70%, prácticas 20%, act.adicional 10%
            Console.Write("Introduce la nota del examen: ");
            double e = double.Parse(Console.ReadLine());
            Console.Write("Introduce la nota de la primera práctica: ");
            double p1 = double.Parse(Console.ReadLine());
            Console.Write("Introduce la nota de la segunda práctica: ");
            double p2 = double.Parse(Console.ReadLine());
            Console.Write("Introduce la nota de la acividad adicional: ");
            double aa = double.Parse(Console.ReadLine());

            double mediap = (p1 + p2) / 2;
            double media = (e * 0.7) + (p1 * 0.2) + (aa * 0.1);
            Console.WriteLine("La media del curso es: " + Math.Round (media, 2));
            */



            /*
            //Ejercicio 4
            Console.Write("Introduce el número de 4 cifras: ");
            string aux = Console.ReadLine();

            if (aux.Length == 4)
            {
                int num = int.Parse(aux);
                int d1 = num / 1000;
                int d2 = (num / 100) % 10;
                int d3 = (num / 10) % 10;
                int d4 = num % 10;

                int cont3 = 0;
                if (d1 == 3) { cont3++; }
                if (d2 == 3) { cont3++; }
                if (d3 == 3) { cont3++; }
                if (d4 == 3) { cont3++; }


                if (cont3 == 0)
                {
                    Console.WriteLine("No hay ninguna cifra que sea un 3.");
                }

                //alguna de sus cifras es un 3
                else if (cont3 == 1)
                {
                    Console.WriteLine("Hay un solo 3.");
                }
                //al menos 2 de sus cifras son un 3
                else if (cont3 > 1)
                {
                    Console.WriteLine("Al menos 2 de sus cifras son la cifra 3.");
                    //2 de ellos son consecutivos
                    if (cont3 == 2)
                    {
                        Console.Write("El número tiene exactamente dos 3, además son consecutivos ");
                        if (d1 == d2) { Console.Write("en el primer y el segundo dígito."); }
                        if (d2 == d3) { Console.Write("en el segundo y el tercer dígito."); }
                        if (d3 == d4) { Console.Write("en el tercer y el cuarto dígito."); }
                        Console.WriteLine("");
                    }
                }
                //si es capicúa
                bool capicua = (d1 == d4 && d2 == d3);
                if (capicua)
                {
                    Console.WriteLine("El número intrdoducido ES capicúa.");
                }
                else
                {
                    Console.WriteLine("El número introducido NO es capicúa.");
                }
            }
            else
            {
                Console.WriteLine("Por favor introduce un número de 4 cifras.");
            }
            */



            /*
            //Ejercicio 5
            Console.Write("Introduce el número de 4 cifras para convertirlo a binario: ");
            string aux = Console.ReadLine();

            if (aux.Length == 4) //comprobamos que el string tiene 4 caracteres
            {
                int num = int.Parse(aux); // si los tiene los pasa a un int
                //cogemos las cifras por separado
                int d1 = num / 1000;
                int d2 = (num / 100) % 10;
                int d3 = (num / 10) % 10;
                int d4 = num % 10;

                bool binValido = true;//booleana que utilizaremos más adelante para que si una de las cifras != 0,1, que dé error

                if (d1 == 1) { d1 = 8; } //si el primer dígito es 1, pues suma 2^3 (8)
                else if (d1 == 0) { d1 = 0; } //si es 0, suma 0
                else { binValido = false; } // si no es ni 0 ni 1, activamos el error del booleano

                if (d2 == 1) { d2 = 4; }
                else if (d2 == 0) { d2 = 0; }
                else { binValido = false; }

                if (d3 == 1) { d3 = 2; }
                else if (d3 == 0) { d3 = 0; }
                else { binValido = false; }

                if (d4 == 1) { d4 = 1; }
                else if (d4 == 0) { d4 = 0; }
                else { binValido = false; }


                int dt = d1 + d2 + d3 + d4;

                if (binValido)
                {
                    Console.WriteLine("El número binario introducido pasado a decimal es: " + dt);
                }
                else
                {
                    Console.WriteLine("Por favor introduce el número en binario correctamente.");
                }
            }

            else
            { 
                Console.WriteLine("Recuerda introducir un número de 4 cifras.");
            }
            */



            /*
            //Ejercicio 6
            Console.Write("Por favor introduce el número de euros: ");
            int eur = int.Parse(Console.ReadLine());
            Console.Write("Por favor introduce el número de céntimos: ");
            int cent = int.Parse(Console.ReadLine());
            Console.WriteLine("Dinero a cambiar: " + eur + "," + cent + " euros.");
            Console.WriteLine("");

            cent = (eur * 100) + cent;


            if (cent >= 50000) //500 euros
            {
                int e500 = cent / 50000;
                cent = cent - (50000 * e500);
                Console.WriteLine("Billetes de 500 euros: " + e500);
            }
            if (cent >= 20000) //200 euros
            {
                int e200 = cent / 20000;
                cent = cent - (20000 * e200);
                Console.WriteLine("Billetes de 200 euros: " + e200);
            }
            if (cent >= 10000) //100 euros
            {
                int e100 = cent / 10000;
                cent = cent - (10000 * e100);
                Console.WriteLine("Billetes de 100 euros: " + e100);
            }
            if (cent >= 5000) //50 euros
            {
                int e50 = cent / 5000;
                cent = cent - (5000 * e50);
                Console.WriteLine("Billetes de 50 euros: " + e50);
            }
            if (cent >= 2000) //20 euros
            {
                int e20 = cent / 2000;
                cent = cent - (2000 * e20);
                Console.WriteLine("Billetes de 20 euros: " + e20);
            }
            if (cent >= 1000) //10 euros
            {
                int e10 = cent / 1000;
                cent = cent - (1000 * 1000);
                Console.WriteLine("Billetes de 10 euros: " + e10);
            }
            if (cent >= 500) //5 euros
            {
                int e5 = cent / 500;
                cent = cent - (500 * e5);
                Console.WriteLine("Billetes de 5 euros: " + e5);
            }
            if (cent >= 200) //2 euros
            {
                int e2 = cent / 200;
                cent = cent - (200 * e2);
                Console.WriteLine("Monedas de 2 euros: " + e2);
            }
            if (cent >= 100) //1 euro
            {
                int e1 = cent / 100;
                cent = cent - (100 * e1);
                Console.WriteLine("Monedas de 1 euro: " + e1);
            }
            if (cent >= 50) //50 cents
            {
                int c50 = cent / 50;
                cent = cent - (50 * c50);
                Console.WriteLine("Monedas de 50 céntmios: " + c50);
            }
            if (cent >= 20) //20 cents
            {
                int c20 = cent / 20;
                cent = cent - (20 * c20);
                Console.WriteLine("Monedas de 20 céntimos: " + c20);
            }
            if (cent >= 10) //10 cents
            {
                int c10 = cent / 10;
                cent = cent - (10 * c10);
                Console.WriteLine("Monedas de 10 céntimos: " + c10);
            }
            if (cent >= 5) //5 cents
            {
                int c5 = cent / 5;
                cent = cent - (5 * c5);
                Console.WriteLine("Monedas de 5 céntimos: " + c5);
            }
            if (cent >= 2) //2 cents
            {
                int c2 = cent / 2;
                cent = cent - (2 * c2);
                Console.WriteLine("Monedas de 2 céntimos: " + c2);
            }
            if (cent >= 1) //1 cent
            {
                int c1 = cent / 1;
                cent = cent - (1 * c1);
                Console.WriteLine("Monedas de 1 céntimo: " + c1);
            }
            */



            /*
            //Ejercicio 7
            Console.Write("Por favor introduce el año el cual quieres comprobar si es bisisesto: ");
            int year = int.Parse(Console.ReadLine());

            //es bisiesto
            //1. si es div entre 400
            //2. si es div entre 4 pero no entre 100
            bool esBisiesto = (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0));

            if (esBisiesto)
            {
                Console.WriteLine("ES bisiesto");
            }
            else
            {
                Console.WriteLine("NO es bisiesto");
            }
            */



            /*
            //Ejercicio 8
            Console.Write("Introduce la distancia del primer lado: ");
            int lado1 = int.Parse(Console.ReadLine());
            Console.Write("Introduce la distancia del segundo lado: ");
            int lado2 = int.Parse(Console.ReadLine());
            Console.Write("Introduce la distancia del tercer lado: ");
            int lado3 = int.Parse(Console.ReadLine());
            Console.WriteLine("");

            bool tri = (lado1 > 0 && lado2 > 0 && lado3 > 0);
            bool equi = (lado1 == lado2 && lado2 == lado3);
            bool isos = ((lado1 == lado2 && lado1 != lado3) || (lado3 == lado2 && lado1 != lado3) || (lado1 == lado3 && lado1 != lado2));
            bool esc = (lado1 != lado2 && lado2 != lado3 && lado1 != lado3);
            bool rect = ((lado1 * lado1 == lado2 * lado2 + lado3 * lado3) || (lado2 * lado2 == lado1 * lado1 + lado3 * lado3) || (lado3 * lado3 == lado2 * lado2 + lado1 * lado1));

            if (tri == false)
            {
                Console.WriteLine("Por favor introduce datos válidos.");
            }
            else
            {
                Console.WriteLine("Triángulo: " + tri);
                Console.WriteLine("Equilátero: " + equi);
                Console.WriteLine("Isósceles: " + isos);
                Console.WriteLine("Escaleno: " + esc);
                Console.WriteLine("Retángulo: " + rect);
            }
            */



            /*
            //Ejercicio 9
            const double EPSILON = 1e-10;
            Console.Write("Introduce la distancia del primer lado: ");
            string l1aux = Console.ReadLine();
            Console.Write("Introduce la distancia del segundo lado: ");
            string l2aux = Console.ReadLine();
            Console.Write("Introduce la distancia del tercer lado: ");
            string l3aux = Console.ReadLine();

            double lado1 = double.Parse(l1aux);
            double lado2 = double.Parse(l2aux);
            double lado3 = double.Parse(l3aux);

            double difAbs1 = Math.Abs(lado1 - lado2);
            double difAbs2 = Math.Abs(lado1 - lado3);
            double difAbs3 = Math.Abs(lado2 - lado3);

            if (difAbs1 < EPSILON) { lado1 = lado2; }
            if (difAbs2 < EPSILON) { lado1 = lado3; }
            if (difAbs3 < EPSILON) { lado2 = lado3; }

            bool tri = (lado1 > 0 && lado2 > 0 && lado3 > 0);
            bool equi = (lado1 == lado2 && lado2 == lado3);
            bool isos = ((lado1 == lado2 && lado1 != lado3) || (lado3 == lado2 && lado1 != lado3) || (lado1 == lado3 && lado1 != lado2));
            bool esc = (lado1 != lado2 && lado2 != lado3 && lado1 != lado3);
            bool rect = ((lado1 * lado1 == lado2 * lado2 + lado3 * lado3) || (lado2 * lado2 == lado1 * lado1 + lado3 * lado3) || (lado3 * lado3 == lado2 * lado2 + lado1 * lado1));

            if (tri == false)
            {
                Console.WriteLine("Por favor introduce datos válidos.");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Datos introducidos: ");
                Console.WriteLine("Lado 1: " + lado1);
                Console.WriteLine("Lado 2: " + lado2);
                Console.WriteLine("Lado 3: " + lado3);
                Console.WriteLine("");
                Console.WriteLine("Triángulo: " + tri);
                Console.WriteLine("Equilátero: " + equi);
                Console.WriteLine("Isósceles: " + isos);
                Console.WriteLine("Escaleno: " + esc);
                Console.WriteLine("Retángulo: " + rect);
            }
            */



            /*
            //Ejercicio 10
            Console.Write("Introduce el capital prestado: ");
            double cap = double.Parse(Console.ReadLine());
            Console.Write("Introduce el interés anual: ");
            double intIni = double.Parse(Console.ReadLine());
            Console.Write("Introduce el plazo de años: ");
            double plaIni = double.Parse(Console.ReadLine());
            Console.WriteLine("");

            double intFin = intIni / 12; //interés final: el interés introducido es anual, / 12 para hacerlo mensual
            double plaFin = plaIni * 12; //lo mismo con el plazo final, pero de de anual a mensual ( * 12)

            double intDecimal = intFin / 100; //variable auxiliar del interés para facilitar la operación de la cuota

            double cuotaA = cap * intFin; //numerador de la cuota
            double cuotaB = 100 * (1 - Math.Pow (1 + intDecimal, -plaFin)); //denominador de la cuota
            double cuotaFin = cuotaA / cuotaB; //cuota mensual
            double total = cuotaFin * plaFin; //el total es la cuota * la cantidad de meses (plazo mensual)
            double intereses = total - cap; //el total es el capital + intereses, resta del total - capital = intereses

            Console.Clear();
            Console.WriteLine("Capital: " + cap + " euros");
            Console.WriteLine("Interés: " + intIni + "% anual");
            Console.WriteLine("Plazo: " + plaIni + " años");
            Console.WriteLine("");
            Console.WriteLine("Resultado:");
            Console.WriteLine("Cuota: " + cuotaFin.ToString("F2") + " euros");
            Console.WriteLine("Total: " + total.ToString("F2") + " euros");
            Console.WriteLine("Intereses: " + intereses.ToString("F2") + " euros");
            */



            /*
            //Ejercicio 11
            Console.Write("Introduce tu número n: ");
            int n = int.Parse(Console.ReadLine());
            Console.WriteLine("");

            //a
            int auxA = 0;
            if (n % 2 == 0)
            {
                auxA = n - 2;
            }
            else
            {
                auxA = n - 1;
            }
            Console.WriteLine("El mayor número par inferior a n es: " + auxA);

            //b
            int auxB = 0;
            if (n % 2 == 0)
            {
                auxB = n;
            }
            else
            {
                auxB = n + 1;
            }
            Console.WriteLine("El primer número par mayor o igual que n es: " + auxB);

            //c
            int auxC = 0;
            if (n % 2 != 0)
            {
                auxC = n;
            }
            else
            {
                auxC = n + 1;
            }
            Console.WriteLine("El primer número impar mayor o igual que n es: " + auxC);
            */
        }
    }
}

