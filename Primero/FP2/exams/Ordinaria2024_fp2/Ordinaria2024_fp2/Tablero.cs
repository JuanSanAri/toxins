using Listas;

namespace Kakurasu;

public class Tablero
{
    private enum Casilla { NoDef, Negra, Blanca }; // estado de la casilla    
    private int N;  // lado de la matriz de juego
    private Casilla[,] mat; // matriz de juego
    private int[] objetivosFila,    // sumas objetivo por fila
                  objetivosColumna; // sumas objetivo por columna
    private int fil, col; // posición del cursor

    public Tablero(int[,] datos)
    {
        N = datos.GetLength(0) - 1;
        mat = new Casilla[N, N];

        // Datos de la casilla
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (datos[i, j] == -1) mat[i, j] = Casilla.NoDef;
                else if (datos[i, j] == 0) mat[i, j] = Casilla.Negra;
                else mat[i, j] = Casilla.Blanca;
            }
        }
        // Fila y columna objetivo
        objetivosFila = new int[N];
        objetivosColumna = new int[N];
        for (int i = 0; i < N; i++)
        {
            objetivosFila[i] = datos[i, N];
            objetivosColumna[i] = datos[N, i];
        }
        // Cursor
        fil = col = 0;
    }

    public void Render()
    {
        Console.Clear();
        // Fila enumerada de arriba y barra separadora
        Console.Write("   "); // 3 espacios hasta que empezamos a fila numerada
        for (int i = 0; i < N; i++)
        {
            Console.Write(" " + (i + 1));
        }
        Console.WriteLine();
        Console.Write("   ");
        for (int i = 0; i < N; i++)
        {
            Console.Write("--");
        }
        Console.WriteLine();
        // Lo del medio
        for (int i = 0; i < N; i++)
        {
            Console.Write(i + 1 + " |");
            for (int j = 0; j < N; j++)
            {
                if (i == fil && j == col)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Gray;
                }

                else Console.BackgroundColor = ConsoleColor.Black;
                // Lo que se escribe
                if (mat[i, j] == Casilla.NoDef) Console.Write("  ");
                else if (mat[i, j] == Casilla.Negra) Console.Write(" 0");
                else Console.Write(" ·");

                Console.ResetColor();
            }
            Console.Write(" | " + objetivosFila[i]);
            Console.WriteLine();
        }
        // Barra separadora de abajo y objetivosColumna
        Console.Write("   "); // 4 espacios hasta la barra
        for (int i = 0; i < N; i++)
        {
            Console.Write("--");
        }
        Console.WriteLine();
        Console.Write("   "); // 5 espacios hasta que empezamos a fila numerada
        for (int i = 0; i < N; i++)
        {
            Console.Write(" " + objetivosColumna[i]);
        }
        Console.SetCursorPosition(2 * col, fil);
    }

    public void ProcesaInput(char c)
    {
        switch (c)
        {
            case 'u': if (fil > 0) fil--; break;
            case 'd': if (fil < N - 1) fil++; break;
            case 'r': if (col < N - 1) col++; break;
            case 'l': if (col > 0) col--; break;
        }
        if (c == 'n') mat[fil, col] = Casilla.Negra;
        if (c == 'b') mat[fil, col] = Casilla.Blanca;
        if (c == 's') mat[fil, col] = Casilla.NoDef;
    }

    int SumaFila(int fil)
    {
        int sumaT = 0;
        for (int i = 0; i < N; i++)
        {
            if (mat[fil, i] == Casilla.Negra) sumaT += (i + 1);
        }
        return sumaT;
    }

    int SumaCol(int col)
    {
        int sumaT = 0;
        for (int i = 0; i < N; i++)
        {
            if (mat[i, col] == Casilla.Negra) sumaT += (i + 1);
        }
        return sumaT;
    }

    void Incorrectas(out Lista fils, out Lista cols)
    {
        Lista filsAux = new Lista();
        Lista colsAux = new Lista();
        for (int i = 0; i < N; i++)
        {
            if (SumaFila(i) != objetivosFila[i]) filsAux.InsertaUlt(i);
            if (SumaCol(i) != objetivosColumna[i]) colsAux.InsertaUlt(i);
        }
        fils = filsAux;
        cols = colsAux;
    }

    public bool Terminado()
    {
        Lista filsF;
        Lista colsF;
        Incorrectas(out filsF, out colsF);

        return filsF.EsVacia() && colsF.EsVacia();
    }

    public void Pista()
    {
        Incorrectas(out Lista filsI, out Lista colsI);

        Console.WriteLine();
        if (!filsI.EsVacia()) Console.WriteLine("Filas incorrectas: " + filsI.AString());
        if (!colsI.EsVacia()) Console.WriteLine("Columnas incorrectas: " + colsI.AString());
    }
}