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
            //...
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

                // Casillas
                case "Spacebar": d = 'c'; break;
                case "D0": d = '0'; break;
                case "D1": d = '1'; break;

                // Terminar juego
                case "Escape": case "q": d = 'q'; break;

                default: d = ' '; break;
            }
            return d;
        }

        // end
    }
}