namespace hoja6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            //Ejercicio 1
            Console.Write("Qué tamaño tiene la lista de números?: ");
            int tam = int.Parse(Console.ReadLine());
            int[] v = new int[tam];
            for (int i = 0; i < v.Length; i++)
            {
                Console.Write($"Número {i + 1}: ");
                v[i] = int.Parse(Console.ReadLine());
            }
            Console.WriteLine();
            Console.Write("Ahora introduce el número que quieres contear: ");
            int n = int.Parse(Console.ReadLine());
            Console.WriteLine();

            int resultado = Cuenta(v, n);
            Console.WriteLine($"-- El número {n} se repite {resultado} veces en la lista.");

            int Cuenta(int[] v, int n)
            {
                int conteo = 0;
                for (int i = 0; i < v.Length; i++)
                {
                    if (v[i] == n)
                    {
                        conteo++;
                    }
                }
                return conteo;
            }
            */



            /*
            //Ejercicio 2 y 3
            Console.Write("Introduce la longitud de los vectores: ");
            int tamaño = int.Parse(Console.ReadLine());
            Console.WriteLine();

            double[] u = new double[tamaño];
            double[] v = new double[tamaño];

            for (int i = 0; i < u.Length; i++)
            {
                Console.Write($"Introduce el componente {i + 1} del vector u: ");
                u[i] = double.Parse(Console.ReadLine());
            }
            Console.WriteLine();

            for (int i = 0; i < v.Length; i++)
            {
                Console.Write($"Introduce el componente {i + 1} del vector v: ");
                v[i] = double.Parse(Console.ReadLine());
            }
            Console.WriteLine();

            double resultadoF = ProdEsc(u, v);
            Console.WriteLine($"-- El producto escalar 'k' de los vectores 'u' y 'v', con longitud {tamaño}, es: {resultadoF}");

            bool sonIguales = Iguales(u, v);
            if (sonIguales)
            {
                Console.WriteLine("-- Además todos sus números son iguales dos a dos.");
            }

            //método ejercicio 2
            double ProdEsc(double[] u, double[] v)
            {
                double resultado = 0;
                for (int i = 0; i < u.Length; i++)
                {
                    resultado = u[i] * v[i] + resultado;
                }
                return resultado;
            }

            //método ejercicio 3
            bool Iguales(double[] u, double[] v)
            {
                for (int i = 0; i < u.Length; i++)
                {
                    if (u[i] != v[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            */



            /*
            //Ejercicio 4
            Console.Write("Inrtoduce la longitud del vector u: ");
            int tamañoU = int.Parse(Console.ReadLine());
            Console.Write("Introduce la longitud del vector v (mayor que el de u: ");
            int tamañoV = int.Parse(Console.ReadLine());
            if (tamañoU > tamañoV)
            {
                Console.WriteLine("ERROR: no se puede realizar la comprobación debido a que la longitud de u > longitud de v.");
            }

            int[] u = new int[tamañoU];
            int[] v = new int[tamañoV];

            for (int i = 0; i < u.Length; i++)
            {
                Console.Write($"Introduce el componente {i + 1} del vector u: ");
                u[i] = int.Parse(Console.ReadLine());
            }
            Console.WriteLine();

            for (int i = 0; i < v.Length; i++)
            {
                Console.Write($"Introduce el componente {i + 1} del vector v: ");
                v[i] = int.Parse(Console.ReadLine());
            }
            Console.WriteLine();

            Array.Sort(u);
            Array.Sort(v);

            bool existe = Contenido(u, v);
            Console.WriteLine($"-- ¿Todos los elementos de u existen en v?: {existe}");


            bool Contenido(int[] u, int[] v)
            {
                int i = 0, j = 0;
                while (i < u.Length && j < v.Length)
                {
                    if (u[i] == v[j])
                    {
                        i++;
                        j++;
                    }
                    else if (v[j] < u[i])
                    {
                        j++;
                    }
                    else { return false; }
                }
                return i == u.Length;
            }
            */



            /*
            //Ejercicio 5
            Console.Write("Introduce la longitud del vector: ");
            int longi = int.Parse(Console.ReadLine());
            Console.WriteLine();

            int[] v = new int[longi];

            for (int i = 0; i < v.Length; i++)
            {
                Console.Write($"Introduce el valor del componente {i + 1}: ");
                v[i] = int.Parse(Console.ReadLine());
            }
            Console.WriteLine();

            int consecutivos = MaxSubsecNula(v);
            Console.WriteLine("-- La mayor cadena de nulos ha sido de: " + consecutivos);


            int MaxSubsecNula(int[] v)
            {
                int maxConteo = 0, conteo = 0;

                for (int i = 0; i < v.Length; i++)
                {
                    if (v[i] == 0)
                    {
                        conteo++;
                    }
                    else
                    {
                        maxConteo = Math.Max(maxConteo, conteo);
                        conteo = 0;
                    }
                }
                return maxConteo;
            }
            */



            //
            //Ejercicio 6
            int[] valores = {50000,20000,10000,5000,2000,1000,500, // billetes en unidades de céntimo
                             200,100,50,20,10,5,2,1}; // monedas en unidades de céntimo
            int[] unidades = new int[valores.Length]; // cantidad de cada billete/moneda del cambio

            Console.Write("Introduce la cantidad de dinero ('euros, céntimos'): ");
            double cantidadIni = double.Parse(Console.ReadLine()); //cantidad en double, puede haber más de 2 decimales
            int cantidad = Convert.ToInt32(Math.Round(cantidadIni * 100, 0)); // convertir a int (redondeo + 0 decimales)
            Console.Clear();

            Console.WriteLine($"Cantidad exigida: {cantidadIni:F2} eu");
            Console.WriteLine();


            int i = 0;
            while (i < valores.Length) // para que recorra todo el array, aunq tb se puede poner cantidad > 0 (mientras no sea 0 la cantidad)
            {
                if (cantidad >= valores[i]) // si cantidad es mayor que el valor de la posición, unidad + 1
                {
                    cantidad = cantidad - valores[i]; // y actualizamos la cantidad que no se nos olvide
                    unidades[i]++;
                }
                else if (cantidad < valores[i]) // si es menor, simplemente pasamos a siguiente valor
                {
                    i++;
                }
            }

            for (i = 0; i < unidades.Length; i++)
            {
                if (unidades[i] > 0) // para que solo escriba si la unidad es mayor que 0
                {
                    if (i <= 6) // escribe 'billetes' hasta la posición 6 [0-6].
                    {
                        Console.WriteLine($"Billetes de {valores[i] / 100} eu: {unidades[i]}");
                    }
                    else if (i >= 7 && i <= 8) // escribe 'monedas de euro' posiciones [7, 8], monedas de 2 y 1 eur respectivamente
                    {
                        Console.WriteLine($"Monedas de {valores[i] / 100} eu: {unidades[i]}");
                    }
                    else // escribe 'monedas de cent' posiciones [9, 10, 11, 12, 13 y 14], monedas de 50, 20, 10, 5, 2 y 1 cent respectivamente
                         // no las tengo que dividir para pasar bien el valor
                    {
                        Console.WriteLine($"Monedas de {valores[i]} cent: {unidades[i]}");
                    }
                }
            }
        }
    }
}
