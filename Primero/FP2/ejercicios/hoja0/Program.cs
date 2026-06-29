using System.Globalization;

namespace hoja0
{
    internal class Program
    {

        const int N = 50; // constante para límite de alumnos
        struct Alumno
        {
            public string Nombre, Apellido1, Apellido2;
            public int Telefono;
        }
        struct Listado
        {
            public int numAls; // número de alumnos del listado O primer hueco libre en el array
            public Alumno[] v; // array de alumnos
        }


        static void Main(string[] args)
        {
            Console.Write("Quieres cargar una lista? Responde s/n: ");
            char a = char.Parse(Console.ReadLine());

            Listado lst;
            if (a == 's') { Recupera(out lst); }
            else Crea(out lst, N);

            int opcion;
            do
            {
                Console.WriteLine();
                Console.WriteLine("1. Insertar alumno");
                Console.WriteLine("2. Buscar alumno");
                Console.WriteLine("3. Eliminar alumno");
                Console.WriteLine("4. Cambiar tel. de alumno");
                Console.WriteLine("5. Guardar lista en un .txt");
                Console.WriteLine("0. Salir");
                opcion = int.Parse(Console.ReadLine());

                switch (opcion)
                {
                    // Si pone cualquier otro número:
                    default:
                        Console.WriteLine("Opción no válida");
                        break;
                    // Ahora las opciones
                    case 0:
                        Console.WriteLine("Hasta luego!"); // 0 = Nos vamos
                        break;
                    case 1:
                        Alumno al; // Hay que declarar variables locales de alumno para cada case
                        LeeAlumno(out al);
                        Inserta(ref lst, al);
                        break;
                    case 2:
                        Alumno al_b;
                        LeeNombre(out al_b); // Introducimos Nombre, Ap1, Ap2
                        int tel = Busca(lst, al_b); // Compara lo anterior con cada uno en la lista
                        if (tel != -1) Console.WriteLine("Teléf. del alumno: " + tel); // Encontrado
                        else Console.WriteLine("El alumno no está en la lista");
                        break;
                    case 3:
                        Alumno al_c;
                        LeeAlumno(out al_c);
                        Elimina(ref lst, al_c);
                        break;
                    case 4:
                        Alumno al_d;
                        LeeAlumno(out al_d); // Pedimos datos alumno
                        tel = PedirNumTel(); // Pedimos nuevo número tel.
                        CambiaTel(ref lst, al_d, tel);
                        break;
                    case 5:
                        Vuelca(lst); // Guardar lista
                        Console.WriteLine("Lista actualizada.");
                        break;
    }
            } while (opcion != 0);
        }


        static void Crea(out Listado lst, int n)
        {
            lst.v = new Alumno[n]; // listado de tamaño N
            lst.numAls = 0; // lista vacía
        }

        static int Compara(Alumno al1, Alumno al2)
        {
            // Comparamos apellido1 primero
            int resultado = String.Compare(al1.Apellido1, al2.Apellido1, new CultureInfo("es-ES"), CompareOptions.IgnoreCase);
            if (resultado == 0)
            {
                // Si son iguales, apellido2
                resultado = String.Compare(al1.Apellido2, al2.Apellido2, new CultureInfo("es-ES"), CompareOptions.IgnoreCase);
                if (resultado == 0)
                {
                    // Finalmente nombre si ambos apellidos son iguales
                    resultado = String.Compare(al1.Nombre, al2.Nombre, new CultureInfo("es-ES"), CompareOptions.IgnoreCase);
                }
            }
            // Nota: el método String.Compare devuleve un valor negativo, 0, o uno positivo
            // pero el enunciado nos EXIGE 1, 0 ó -1, por eso esto y el condicional anidado de arriba
            if (resultado > 0) return 1; // al1 va después de al2 en la lista
            else if (resultado < 0) return -1; // al1 va antes de al2 en la lista
            else return 0;
        }

        static void Inserta(ref Listado lst, Alumno al)
        {
            // Primero comprobamos que la lista no esté llena
            if (lst.numAls == N)
            {
                Console.WriteLine("Error: la lista está llena");
                return;
            }

            // Ahora creamos una booleana que nos ayudará a evitar repeticiones
            // en el bucle poniéndolo como una especie de "salida" del bucle
            // y para evitar el caso de que metamos un al. que sea el último
            bool insertado = false;

            for (int i = 0; i < lst.numAls && !insertado; i++) // Recorremos desde 0
            {
                if (Compara(al, lst.v[i]) == -1) // Si el nuevo al. va antes que la actual it...
                {
                    for (int j = lst.numAls - 1; j >= i; j--) // Primero desplazamos todos los i++
                        lst.v[j + 1] = lst.v[j];              // una posición a la derecha
                    lst.v[i] = al; // Ahora ya asignamos el valor
                    insertado = true; // Salimos del bucle
                }
                else if (Compara(al, lst.v[i]) == 0) // Si es repetido
                {
                    Console.WriteLine("Error: ese nombre ya está");
                    return;
                }
            }

            // Si no se ha insertado significa que es el último en la fila (porque su letra es la
            // mayor de la lista) y la insertamos al final.
            if (!insertado) lst.v[lst.numAls] = al;

            // Ahora sumamos uno al número de alumnos
            lst.numAls++;
        }

        static int Busca(Listado lst, Alumno al)
        {
            // Recorremos array de alumnos
            for (int i = 0; i < lst.numAls; i++)
            {
                // Si coinciden nombre y apellidos devolvemos el tel.
                if (Compara(al, lst.v[i]) == 0) return lst.v[i].Telefono;
            }
            return -1; // Si no, devolvemos -1 para indicar que no está
        }

        static void Elimina(ref Listado lst, Alumno al)
        {
            // El enunciado no lo pide, pero queda limpio añadir un comprobante bool
            bool encontrado = false; // Prácticamente igual al método inserta

            for (int i = 0; i < lst.numAls && !encontrado; i++) // Desde 0 hasta numAls - 1
            {
                if (Compara(al, lst.v[i]) == 0) // Si encuentra el alumno
                {
                    for (int j = i; j < lst.numAls - 1; j++) // Desde i, hasta la última posición
                                                             // antes del último alumno
                    {
                        lst.v[j] = lst.v[j + 1]; // Nos los vamos trayendo a la izquierda
                    }
                    lst.numAls--; // Una vez todos movidos, restamos el n total de alumnos
                    encontrado = true; // Salimos del bucle
                }
            }
        }

        static void CambiaTel(ref Listado lst, Alumno al, int tel)
        {
            bool encontrado = false; // Condición de salida del bucle + checker

            for (int i = 0; i < lst.numAls && !encontrado; i++) // Recorremos la lista
            {
                if (Compara(al, lst.v[i]) == 0) // Si lo encuentra
                {
                    lst.v[i].Telefono = tel; // Introducimos en ese campo el nuevo tel
                    encontrado = true; // Salimos
                }
            }
            if (!encontrado) Console.WriteLine("No existe dicho alumno en la lista");
        }

        // Lo de siempre, como guarda-lee en FP1
        static void Vuelca(Listado lst)
        {
            StreamWriter file = new StreamWriter("salida.txt"); // Creamos archivo

            file.WriteLine(lst.numAls); // Asignamos a la primera línea el número de alumnos total

            // Escribimos, el orden de campos da igual mientras lo pongamos igual en el recupera
            for (int i = 0; i < lst.numAls; i++)
            {
                file.WriteLine(lst.v[i].Nombre);
                file.WriteLine(lst.v[i].Apellido1);
                file.WriteLine(lst.v[i].Apellido2);
                file.WriteLine(lst.v[i].Telefono);
            }

            file.Close(); // Cerramos
        }

        static void Recupera(out Listado lst)
        {
            StreamReader file = new StreamReader("salida.txt"); // Accedemos archivo

            Crea(out lst, N); // Creamos un listado vacío

            lst.numAls = int.Parse(file.ReadLine()); // Primera línea del txt es el numAls

            // Leemos en el orden que hemos guardado los campos en el vuelca
            for (int i = 0; i < lst.numAls; i++)
            {
                lst.v[i].Nombre = file.ReadLine();
                lst.v[i].Apellido1 = file.ReadLine();
                lst.v[i].Apellido2 = file.ReadLine();
                lst.v[i].Telefono = int.Parse(file.ReadLine());
            }

            file.Close(); // Cerramos
        }

        // ---- BONUS BONUS ---- para la agenda
        static void LeeAlumno(out Alumno al)
        {
            Console.Write("Nombre del alumno: ");
            al.Nombre = Console.ReadLine();

            Console.Write("Primer apellido del alumno: ");
            al.Apellido1 = Console.ReadLine();

            Console.Write("Segundo apellido del alumno: ");
            al.Apellido2 = Console.ReadLine();

            Console.Write("Número tel. del alumno: ");
            al.Telefono = int.Parse(Console.ReadLine());
        }

        static void LeeNombre(out Alumno al) // Lo mismo pero para buscar solo por nombre y apellidos
        {
            Console.Write("Nombre del alumno: ");
            al.Nombre = Console.ReadLine();

            Console.Write("Primer apellido del alumno: ");
            al.Apellido1 = Console.ReadLine();

            Console.Write("Segundo apellido del alumno: ");
            al.Apellido2 = Console.ReadLine();

            al.Telefono = 0;
        }

        // Método exclusivo para exigir el nuevo número de telf. para el CambiaTel
        static int PedirNumTel ()
        {
            Console.WriteLine("Introduce el nuevo número de teléfono: ");
            return int.Parse(Console.ReadLine());
        }
    }
}