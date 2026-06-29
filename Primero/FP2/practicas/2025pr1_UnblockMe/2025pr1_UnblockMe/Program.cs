namespace practica1
{
    internal class Program
    {

        // Constante del tamaño
        const int longitud = 8;
        // Constante de la cantidad de niveles para los records
        const int niveles = 8;
        // coordenadas (x,y) para representar posiciones y direcciones de desplazamiento
        struct Coor
        {
            public int x, y;
        }
        struct Estado
        { // estado del juego
            public char[,] mat; // ’#’ muro; ’.’ libre; letras ’a’,’b’ ... bloques
            public char obj; // char correspondiente al bloque objetivo (el que hay que sacar)
            public Coor act, sal; // posiciones del cursor y de la salida
            public bool sel; // idica si hay bloque seleccionado para mover o no
        }

        // Apartado de deshacer jugada
        const int MAX_JUGADAS = 200;
        struct Jugada
        {
            public Coor posJugada;
            public Coor dirJugada;
        }
        struct Memoria
        {
            public Jugada[] historial;
            public int indP;
        }


        static void Main(string[] args)
        {
            // Deshacer jugada
            Memoria memoria;
            memoria.historial = new Jugada[MAX_JUGADAS];
            memoria.indP = 0;

            // El game
            bool terminado = false;

            Console.Write("Introduce el nivel (0 - 7) : ");
            int n = int.Parse(Console.ReadLine());
            Estado est = LeeNivel("levels.txt", n);
            MarcaSalida(ref est);
            Render(est);

            while (!terminado)
            {
                char c = LeeInput();
                ProcesaInput(ref est, c, /*DeshacerJugada*/ref memoria);
                Render(est);

                // Ganar
                if (est.mat[est.sal.x, est.sal.y] == est.obj)
                {
                    Console.Clear();
                    Console.Write("Enhorabuena!!");
                    Console.WriteLine();
                    GuardaRecord(n, memoria.indP);
                    terminado = true;
                }

                // Salir
                if (c == 'q')
                {
                    Console.Clear();
                    Console.WriteLine("chao chao");
                    terminado = true;
                }
            }
        }


        static Estado LeeNivel(string file, int n)
        {
            // Inicializar struct
            Estado infoNivel;
            // Matriz
            infoNivel.mat = new char[longitud, longitud];
            // Bloque que hay que sacar (obj)
            infoNivel.obj = ' ';
            // Coor los dos, act es pos. del cursor y sal es pos, de la salida
            infoNivel.act.x = 1;
            infoNivel.act.y = 1;
            infoNivel.sal.x = 0;
            infoNivel.sal.y = 0;
            // Booleana de selección de bloque
            infoNivel.sel = false;

            // Leer archivo
            StreamReader sr = new StreamReader(file); // Abrimos archivo

            string linea;
            while ((linea = sr.ReadLine()) != null)
            {
                if (linea == "level " + n)
                {
                    infoNivel.obj = char.Parse(sr.ReadLine());
                    for (int i = 0; i < longitud; i++)
                    {
                        // Filas de los bordes de arriba y abajo
                        if (i == 0 || i == longitud - 1)
                        {
                            for (int j = 0; j < longitud; j++)
                                infoNivel.mat[i, j] = '#';
                        }
                        else // Filas de la 1 a la 6, que son las que su info. rellenamos con el txt
                        {
                            linea = sr.ReadLine(); // Vamos avanzando líneas, reutilizando linea
                            for (int j = 0; j < longitud; j++)
                            {
                                // Columnas de los bordes de izquierda y derecha
                                if (j == 0 || j == longitud - 1)
                                    infoNivel.mat[i, j] = '#';
                                // Columnas 1 - 6, los 6 caracteres en el txt serían linea[0], linea[1]...
                                else infoNivel.mat[i, j] = linea[j - 1]; // por eso j - 1
                            }
                        }
                    }
                }
            }
            // Cerramos archivo y devolvemos el valor del struct
            sr.Close();
            return infoNivel;
        }

        static int BloqueToInt(char c) // ’a’->1, ’b’->2... descartar el 0=negro
        {
            // Ejemplo: letra F (6 en el abcdario) - A(=1) + 1 = 6, si hacemos 
            // Console.BackgroundColor = colores[BloqueToInt('f')], en este caso es 6 en 
            // el array de colores de la consola (el cual hemos declarado al incio de render)
            return ((int)c) - ((int)'a') + 1;
        }
        static void Render(Estado est)
        {
            // Devuelve para cada valor del array un color predeterminado
            ConsoleColor[] colores = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
            Console.ForegroundColor = ConsoleColor.White; // Color cursor

            // Render
            Console.Clear();
            for (int i = 0; i < longitud; i++)
            {
                for (int j = 0; j < longitud; j++)
                {
                    // Colores fondo
                    if (est.mat[i, j] == '#') Console.BackgroundColor = colores[colores.Length - 1]; // blanco
                    else if (est.mat[i, j] == '.') Console.BackgroundColor = colores[0]; // negro
                    else Console.BackgroundColor = colores[BloqueToInt(est.mat[i, j])];

                    // Cursor
                    if (i == est.act.x && j == est.act.y)
                    {
                        if (!est.sel) Console.Write("**");
                        else Console.Write("<>");
                    }
                    else Console.Write("  ");
                }
                Console.WriteLine();
            }
            Console.ResetColor();

            // Escribir color del bloque objetivo
            Console.WriteLine();
            Console.ForegroundColor = colores[BloqueToInt(est.obj)];
            Console.Write("Color del bloque objetivo: " + est.obj);
            Console.ResetColor();
        }

        static void MarcaSalida(ref Estado est)
        {
            bool encontrado = false;

            for (int i = 1; i < longitud - 1 && !encontrado; i++)
            {
                for (int j = 1; j < longitud - 1 && !encontrado; j++)
                {
                    if (est.mat[i, j] == est.obj && est.mat[i, j + 1] == est.obj)
                    {
                        // horizontal, hay q salir del bucle y poner que salida está en [i, j = 7]
                        est.sal.x = i;
                        est.sal.y = longitud - 1;
                        est.mat[est.sal.x, est.sal.y] = '.';
                        encontrado = true;
                    }
                    else if (est.mat[i, j] == est.obj && est.mat[i + 1, j] == est.obj)
                    {
                        // vertical
                        est.sal.x = longitud - 1;
                        est.sal.y = j;
                        est.mat[est.sal.x, est.sal.y] = '.';
                        encontrado = true;
                    }
                }
            }
        }

        static void MueveCursor(ref Estado est, Coor dir)
        {
            // Posición a la que queremos ir
            int nuevaX = est.act.x + dir.x;
            int nuevaY = est.act.y + dir.y;

            // Ojo estas condiciones
            if (est.mat[nuevaX, nuevaY] != '#') // Si adonde quiero ir NO es celda...
            {
                if (!(nuevaX == est.sal.x && nuevaY == est.sal.y)) // Ni la salida...
                {
                    // Me puedo mover libremente (excepto salida y celda)
                    est.act.x = nuevaX;
                    est.act.y = nuevaY;
                }

                else if (est.sel) // Pero si estoy seleccionado
                {
                    // Puedo moverme, aunque sea la salida
                    est.act.x = nuevaX;
                    est.act.y = nuevaY;
                }
            }
        }


        static Coor BuscaCabeza(ref Estado est, Coor dir)
        {
            // Encuentra la posición más avanzada del bloque en la dirección dir.
            // Por ejemplo si tenemos un bloque horizontal aaa y vamos a moverlo a la derecha,
            // la cabeza es la a más a la derecha. La idea es: desde el cursor, 
            // avanza en la dirección dir mientras sigas encontrando el mismo bloque
            // recordar que dir siempre va a ser (1,0),(0,1),(-1,0),(0,-1)
            Coor cabeza = est.act;
            // Mientras el siguiente caracter en el eje con el que estemos trabajando sea igual
            while (est.mat[cabeza.x + dir.x, cabeza.y + dir.y] == est.mat[est.act.x, est.act.y])
            {
                // Avanzamos en el bloque
                cabeza.x += dir.x;
                cabeza.y += dir.y;
            }
            return cabeza;
        }
        static Coor BuscaCola(ref Estado est, Coor dir) // Método no en el enunciado pero muy útil
        {
            // Lo mismo que cabeza pero para el extremo contrario
            Coor cola = est.act;
            // Mientras el siguiente caracter en el eje con el que estemos trabajando sea igual,
            // y esta vez al contrario, porque vamos en sentido opuesto a lo que dice dir...
            while (est.mat[cola.x - dir.x, cola.y - dir.y] == est.mat[est.act.x, est.act.y])
            {
                // Retrocedemos
                cola.x -= dir.x;
                cola.y -= dir.y;
            }
            return cola;
        }
        static void MueveBloque(ref Estado est, Coor dir, /*DeshacerJugada*/ ref Memoria memoria)
        {
            // Vamos con el deshacer jugada, inicializamos y registramos una jugada
            // cada vez que el jugador mueve un bloque
            Jugada jug;
            jug.posJugada.x = est.act.x;
            jug.posJugada.y = est.act.y;
            jug.dirJugada.x = dir.x;
            jug.dirJugada.y = dir.y;
            // Hasta aquí + el guardar en el array más abajo dentro del if

            // Primero declaramos la cabeza de nuestro bicho
            Coor cabeza = BuscaCabeza(ref est, dir);
            // La coordenada/caracter que vamos a sobreescribir al avanzar nuestro bloque
            int nuevaX = cabeza.x + dir.x;
            int nuevaY = cabeza.y + dir.y;
            // Y la cola
            Coor cola = BuscaCola(ref est, dir);

            // Verificamos si es horizontal o vertical para luego restringir mov. en seleccionado
            bool esHorizontal = est.mat[est.act.x, est.act.y] == est.mat[est.act.x, est.act.y + 1] ||
                est.mat[est.act.x, est.act.y] == est.mat[est.act.x, est.act.y - 1];

            // Si a la coor a la que vamos NO es una celda NI otro bloque, y lo queremos mover en su eje
            if (est.sel && (est.mat[nuevaX, nuevaY] == '.') && ((esHorizontal && dir.x == 0) || (!esHorizontal && dir.y == 0)))
            {
                // Sobreescribimos espacio vacío
                est.mat[nuevaX, nuevaY] = est.mat[est.act.x, est.act.y];
                est.mat[cola.x, cola.y] = '.'; // Eliminamos cola anterior
                MueveCursor(ref est, dir);

                // Se van guardando en nuestro array de jugadas si el mov. es válido
                memoria.historial[memoria.indP] = jug;
                memoria.indP++;
            }
        }

        static void ProcesaInput(ref Estado est, char c, /*DeshacerJugada*/ ref Memoria memoria)
        {
            Coor dir;
            dir.x = 0;
            dir.y = 0;

            switch (c)
            {
                case 'l': dir.y = -1; break;
                case 'r': dir.y = 1; break;
                case 'u': dir.x = -1; break;
                case 'd': dir.x = 1; break;
                case 's': if (est.mat[est.act.x, est.act.y] != '.') est.sel = !est.sel; break;

                // Deshacer jugada
                case 'z':
                    if (memoria.indP > 0) // Si tenemos al menos una jugada hecha
                    {
                        // Recolocamos el cursor en la posición donde se hizo la jugada
                        est.act.x = memoria.historial[memoria.indP - 1].posJugada.x;
                        est.act.y = memoria.historial[memoria.indP - 1].posJugada.y;
                        // Invertimos la dirección del movimiento original para deshacerlo
                        // (si el bloque se movió a la derecha, ahora lo movemos a la izquierda)
                        dir.x = -memoria.historial[memoria.indP - 1].dirJugada.x;
                        dir.y = -memoria.historial[memoria.indP - 1].dirJugada.y;

                        // Me quiero quitar de encima la última jugada que hemos hecho
                        // (guardada en jugada[indP - 1]), para ello voy justo a esa posición
                        // como hago aquí con el decremento y ahora cuando haga MueveBloque
                        // la jugada que había en esa posición la sobreescribe con su inverso (-dir),
                        memoria.indP--;
                        // Aplicamos el movimiento inverso para deshacer la jugada
                        est.sel = true; // Para que no haya problemas con el if
                        MueveBloque(ref est, dir, ref memoria);
                        est.sel = false;

                        // No nos olvidemos de que MueveBloque añade automáticamente una jugada
                        // al historial, así que también la eliminamos, y es como si la última jugada
                        // nunca hubiese existido
                        memoria.indP--;
                    }
                    break;
            }

            if (c == 'l' || c == 'r' || c == 'u' || c == 'd')
            {
                if (est.sel) MueveBloque(ref est, dir, /*DeshacerJugada*/ ref memoria);
                else MueveCursor(ref est, dir);
            }
        }

        static char LeeInput()
        {
            char d = ' ';
            while (d == ' ')
            {
                if (Console.KeyAvailable)
                {
                    string tecla = Console.ReadKey().Key.ToString();
                    switch (tecla)
                    {
                        case "LeftArrow": d = 'l'; break; // direccones
                        case "UpArrow": d = 'u'; break;
                        case "RightArrow": d = 'r'; break;
                        case "DownArrow": d = 'd'; break;

                        case "Delete": d = 'z'; break; // deshacer jugada
                        case "Escape": d = 'q'; break; // salir
                        case "Spacebar": d = 's'; break; // selección de bloque
                    }
                }
            }
            return d;
        }

        static void GuardaRecord(int nivel, int movimientos)
        {
            // Leer todos los records existentes
            int[] records = new int[niveles];
            for (int i = 0; i < niveles; i++) records[i] = -1; // -1 = sin record

            try // Sacamos la info del archivo
            {
                StreamReader sr = new StreamReader("records.txt");
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    string[] partes = linea.Split(' ');
                    int niv = int.Parse(partes[0]);
                    records[niv] = int.Parse(partes[1]);
                }
                sr.Close();
            }
            catch { } // Si el archivo no existe simplemente seguimos con records vacíos


            // Primera vez que se supera o actualizar record
            if (records[nivel] == -1 || movimientos < records[nivel])
            {
                records[nivel] = movimientos;
                Console.WriteLine("Nuevo record: " + movimientos + " movimientos!");
            }

            // Reescribir el archivo con la nueva info.
            StreamWriter sw = new StreamWriter("records.txt");
            for (int i = 0; i < niveles; i++)
            {
                if (records[i] != -1)
                    sw.WriteLine(i + " " + records[i]);
            }
            sw.Close();
        }
    }
}