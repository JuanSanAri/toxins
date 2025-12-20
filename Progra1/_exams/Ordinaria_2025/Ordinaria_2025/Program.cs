namespace Ordinaria_2025
{
    internal class Program
    {

        static void Main(string[] args)
        {
            const int N = 4; // tamaño del tablero de ejemplo
            char[,] tab;
            tab = new char[N, N] { // tablero del ejemplo
                {'.','1','.','0'}, // fila 0
                {'.','.','0','.'}, // fila 1
                {'.','0','.','.'}, // etc
                {'1','1','.','0'}
            };

            bool[,] fijas = new bool[N, N]; // matriz de posiciones fijas
            int fil, col; // fila y columna de la casilla activa
            Console.WriteLine("Hello, World!");
        }
    }
}