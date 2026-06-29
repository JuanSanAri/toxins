namespace letrasDeslizantes
{
    class Program
    {
        static Random rnd = new Random(); // generador de números aleatorios

        struct Coor
        { // Coordenadas para representar posiciones
            public int fil, col;
        }

        static void Main()
        {
            char[,] tab; // tablero de juego
            Coor pos; // posicion actual del cursor

            // Variables y cosas
            Console.CursorVisible = false;
            bool terminado = false;
            string resp;
            string file = "letras";
            char input;
            int fils = 4;
            int cols = 5;

            // Carga o no
            do
            {
                Console.Write("Quieres partida de archivo o random? [a/r]: ");
                resp = Console.ReadLine();
            } while (resp != "a" && resp != "r");
            if (resp == "a") Lee(file, out tab, out pos);
            else
            {
                Inicializa(fils, cols, out tab, out pos);
                Desordena(tab);
            }
            // Bucle
            while (!terminado)
            {
                Render(tab, pos);
                input = LeeInput();
                ProcesaInput(tab, ref pos, input);

                if (Terminado(tab))
                {
                    terminado = true;
                    Console.Clear();
                    Console.WriteLine("Has ganado!!");
                }
                else if (input == 'q')
                {
                    terminado = true;
                    Console.Clear();
                    Console.WriteLine("Has salido con ESC.");
                }
            }
        }


        static void Inicializa(int fils, int cols, out char[,] tab, out Coor pos)
        {
            tab = new char[fils, cols];
            char a = 'a';
            for (int i = 0; i < fils; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    tab[i, j] = a;
                    a++;
                }
            }
            // Cambiamos el último carácter al blanco
            tab[fils - 1, cols - 1] = ' ';
            // Inicializar coor de posición
            pos.fil = 0;
            pos.col = 0;
        }

        // Muy bonito este:
        // Hay una regla con matrices bidimensionales, cuando tenemos la posición de un
        // elemento en índice (unidimensional), su pos en la matriz es (i/cols, i%cols):
        // p.e: elemento posición 8 en una matriz de 3x5: (8/5, 8%5) = (1,3)
        // y efectivamente la posición del elemento 8 (9o elemento de la matriz bidim.)
        // se encontraría en la posición (1,3) de cualquier matriz con cols = 5;
        static void Desordena(char[,] tab)
        {
            int fils = tab.GetLength(0);
            int cols = tab.GetLength(1);
            int total = fils * cols;

            for (int i = 0; i < total; i++)
            {
                int j = rnd.Next(i, total);
                // Coors del elemento j y coors de elemento i
                int fi = i / cols; int ci = i % cols;
                int fj = j / cols; int cj = j % cols;
                // Los switcheo
                char aux = tab[fi, ci];
                tab[fi, ci] = tab[fj, cj];
                tab[fj, cj] = aux;
            }
        }

        static void Render(char[,] tab, Coor pos)
        {
            Console.Clear();
            for (int i = 0; i < tab.GetLength(0); i++)
            {
                for (int j = 0; j < tab.GetLength(1); j++)
                {
                    // Cursor
                    if (pos.fil == i && pos.col == j)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.Write(tab[i, j]);
                    Console.ResetColor();
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(2 * pos.col, pos.fil);
        }

        static char LeeInput()
        {
            char d = ' ';
            //if (Console.KeyAvailable)
            {
                string tecla = Console.ReadKey(true).Key.ToString();
                switch (tecla)
                {
                    /* INPUTS ELEMENTALES PARA EL JUEGO BÁSICO */
                    // movimiento del cursor 	
                    case "LeftArrow": d = 'l'; break;
                    case "UpArrow": d = 'u'; break;
                    case "RightArrow": d = 'r'; break;
                    case "DownArrow": d = 'd'; break;
                    case "Spacebar": d = 's'; break;

                    // terminar juego
                    case "Escape": case "q": case "Q": d = 'q'; break;

                    default: d = ' '; break;
                }
            }
            return d;
        }

        static void ProcesaInput(char[,] tab, ref Coor pos, char c)
        {
            int limit = tab.GetLength(0); // tam de filas
            int limitb = tab.GetLength(1); // tam de cols
            switch (c)
            {
                case 'u': if (pos.fil > 0) pos.fil--; break;
                case 'd': if (pos.fil < limit - 1) pos.fil++; break;
                case 'l': if (pos.col > 0) pos.col--; break;
                case 'r': if (pos.col < limitb - 1) pos.col++; break;
            }
            if (c == 's') Desliza(tab, pos);
        }

        static Coor LocalizaHueco(char[,] tab)
        {
            Coor blanco;
            int fils = tab.GetLength(0);
            int cols = tab.GetLength(1);
            int total = fils * cols;
            int i = 0;

            while (tab[i / cols, i % cols] != ' ')
            {
                i++;
            }
            blanco.fil = i / cols;
            blanco.col = i % cols;

            return blanco;
        }

        // Tier SSS de battleIQ
        static void Desliza(char[,] tab, Coor pos)
        {
            Coor hueco = LocalizaHueco(tab);
            bool adyacente = (Math.Abs(hueco.fil - pos.fil) + Math.Abs(hueco.col - pos.col)) == 1;

            if (adyacente)
            {
                char aux = tab[hueco.fil, hueco.col];
                tab[hueco.fil, hueco.col] = tab[pos.fil, pos.col];
                tab[pos.fil, pos.col] = aux;
            }
        }

        // En el enunciado está mal, necesitamos tab como argumento siosi
        static bool Terminado(char[,] tab)
        {
            int fils = tab.GetLength(0);
            int cols = tab.GetLength(1);
            int total = fils * cols;
            char c = 'a';
            int i = 0;

            while (i < total - 1 && tab[i / cols, i % cols] == c)
            {
                c++;
                i++;
            }
            return i == total - 1 && tab[fils - 1, cols - 1] == ' ';
        }

        static void Lee(string file, out char[,] tab, out Coor pos)
        {
            StreamReader sr = new StreamReader(file);

            // Tamaño de fils y cols y declarar tab
            string s = sr.ReadLine();
            int fils = int.Parse(s);
            s = sr.ReadLine();
            int cols = int.Parse(s);
            tab = new char[fils, cols];

            // Datos de tab
            for (int i = 0; i < fils; i++)
            {
                s = sr.ReadLine();
                for (int j = 0; j < cols; j++)
                {
                    tab[i, j] = s[j * 2]; // No podemos hacer Split por el espacio blanco
                                          // que tiene el archivo para casilla vacía
                }
            }
            pos.fil = pos.col = 0;
            sr.Close();
        }
    }
}