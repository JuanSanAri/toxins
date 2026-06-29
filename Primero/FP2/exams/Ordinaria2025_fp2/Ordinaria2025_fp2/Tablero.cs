using Coords;
using Listas;

namespace Sudoku
{

    public class Tablero
    {
        // estructura para representar las casillas del tablero
        struct Casilla
        {
            public int num;    // numero en la casilla
            public bool fija;  // casilla fijada inicialmente (true) o vacia (false)
        }

        Casilla[,] mat;  // matriz de casillas

        Coor cursor; // posicion actual del cursor en el tablero

        // Constante añadida (longitud)
        const int N = 9;

        public Tablero(int[,] sud)
        {
            mat = new Casilla[N, N];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    mat[i, j].num = sud[i, j];
                    if (mat[i, j].num != 0) mat[i, j].fija = true;
                    else mat[i, j].fija = false;
                }
            }

            // Cursor
            cursor = new Coor();
        }

        public void Render()
        {
            Console.Clear();
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    // Color número
                    if (mat[i, j].fija) { Console.ForegroundColor = ConsoleColor.Yellow; }
                    else Console.ForegroundColor = ConsoleColor.Green;
                    // Color cursor
                    if (i == cursor.Fil && j == cursor.Col) Console.BackgroundColor = ConsoleColor.Red;

                    // Escritura
                    if (mat[i, j].num == 0) { Console.Write("·"); }
                    else Console.Write(mat[i, j].num);
                    // Espacio entre nums
                    Console.ResetColor();
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        public void MueveCursor(char d)
        {
            switch (d)
            {
                case 'u': if (cursor.Fil > 0) cursor.Fil--; break;
                case 'd': if (cursor.Fil < N - 1) cursor.Fil++; break;
                case 'l': if (cursor.Col > 0) cursor.Col--; break;
                case 'r': if (cursor.Col < N - 1) cursor.Col++; break;
            }
        }

        private void Esquina(ref int fil, ref int col)
        {
            // fila
            if (fil / 3 == 0) fil = 0;
            else if (fil / 3 == 1) fil = 3;
            else fil = 6;
            // col
            if (col / 3 == 0) col = 0;
            else if (col / 3 == 1) col = 3;
            else col = 6;
        }

        private bool[] Posibles()
        {
            bool[] pos = new bool[N + 1];
            for (int i = 0; i <= N; i++) pos[i] = true;

            // filas y cols
            for (int i = 0; i < N; i++)
            {
                pos[mat[i, cursor.Col].num] = false;
                pos[mat[cursor.Fil, i].num] = false;
            }
            // submatriz (uno de los cuadraos)
            int f = cursor.Fil;
            int c = cursor.Col;
            Esquina(ref f, ref c);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    pos[mat[f + i, c + j].num] = false;
                }
            }
            pos[0] = true; // El 0 siempre se puede poner, es como borrar

            return pos;
        }

        private bool Comprueba(int num)
        {
            bool[] aux = Posibles();
            if (aux[num]) return true;

            return false;
        }

        public void PonNumero(int num)
        {
            if (num < 0 || num > 9) throw new IndexOutOfRangeException("Número introducido debe ser 0-9");
            if (mat[cursor.Fil, cursor.Col].fija) throw new Exception("Escribe en una celda no fija");
            if (!Comprueba(num)) throw new Exception("Ese número no vale en esta posición");
            mat[cursor.Fil, cursor.Col].num = num;
        }

        public bool FinJuego()
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    if (mat[i, j].num == 0) return false;

            return true;
        }

        public Lista DamePosibles()
        {
            Lista lista = new Lista();
            // Casilla fija
            if (mat[cursor.Fil, cursor.Col].fija) lista.InsertaPpio(mat[cursor.Fil, cursor.Col].num);
            // Casilla NO fija
            else
            {
                bool[] aux = Posibles();
                for (int i = 1; i <= N; i++)
                {
                    if (aux[i]) lista.InsertaFin(i);
                }
            }
            return lista;
        }
    }
}