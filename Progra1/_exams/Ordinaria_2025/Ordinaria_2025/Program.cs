namespace Ordinaria_2025
{
    internal class Program
    {
        const int N = 4; // tamaño del tablero de ejemplo
        // IMPORTANTE: se debe cambir el tamaño de N para que corresponda
        // con el tamaño de nuestra matriz cuadrada de tamaño NxN

        static void Main(string[] args)
        {
            char[,] tab;
            tab = new char[N, N] { // tablero del ejemplo
                {'.','1','.','0'}, // fila 0
                {'.','.','0','.'}, // fila 1
                {'.','0','.','.'}, // etc
                {'1','1','.','0'}
            };

            bool[,] fijas = new bool[N, N]; // matriz de posiciones fijas
            int fil, col; // fila y columna de la casilla activa

            Inicializa(tab, fijas, out fil, out col);
            Renderiza(tab, fijas, fil, col);
        }

        static void Inicializa(char[,] tab, bool[,] fijas, out int fil, out int col)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (tab[i, j] == '.') { fijas[i, j] = false; }
                    else { fijas[i, j] = true; }
                }
            }
            fil = col = 0;
        }

        static void Renderiza(char[,] tab, bool[,] fijas, int fil, int col)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    // Color del texto
                    if (fijas[i, j] == true) { Console.ForegroundColor = ConsoleColor.Blue; }
                    else { Console.ForegroundColor = ConsoleColor.Yellow; }

                    // Color del fondo dependiendo de la posición del jugador
                    if (col == i && fil == j)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write(tab[i, j]);
                        Console.ResetColor();
                        Console.Write(" ");
                    }
                    else { Console.Write(tab[i, j] + " "); }
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        static char LeeInput()
        {
            char d = ' ';
            string tecla = Console.ReadKey(true).Key.ToString();

            switch (tecla)
            {
                // Movimiento del cursor 	
                case "LeftArrow": d = 'l'; break;
                case "UpArrow": d = 'u'; break;
                case "RightArrow": d = 'r'; break;
                case "DownArrow": d = 'd'; break;

                // Pulsación de casilla (click)
                case "Spacebar": d = 'c'; break;
                case "D0": d = '0'; break;
                case "D1": d = '1'; break;

                // Terminar juego
                case "Escape": case "q": d = 'q'; break;

                default: d = ' '; break;
            }
            return d;
        }

        static void ProcesaInput(char c, char[,] tab, bool[,] fijas, int fil, int col)
        {

        }
    }
}