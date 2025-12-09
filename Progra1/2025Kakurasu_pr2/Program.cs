// Juan Sánchez
using System;

namespace Kakurasu;

public class Program
{
    static void Main()
    {
        Console.CursorVisible = false;

        int[,] ex1 = new int[,] {
            { 0, 0, 0, 4}, // última col: suma por filas
            { 0, 0, 0, 5},
            { 0, 0, 0, 0},
            { 1, 2, 3, 0}  // ultima fil: suma por cols; el último 0 no cuenta
        };
        int[,] ex2 = new int[,] {
            { 0, 0, 0, 0, 7 },
            { 0, 0, 0, 0, 8 },
            { 0, 0, 0, 0, 5 },
            { 0, 0, 0, 0, 6 },
            { 3, 8, 5, 7, 0 }
        };

        // seleccion de ejemplo        
        int[,] mat = ex1;

        int N = mat.GetLength(0) - 1; // con ex1, N = 4 - 1 = 3

        // variables nos da el enunciado
        char[,] tab = new char[N, N];

        int[] obFil = new int[N]; // última columna
        int[] obCol = new int[N]; // última fila

        // inicialización y renderizado inicial
        int fil = 0;
        int col = 0;
        Inicializa(mat, tab, obFil, obCol, fil, col);
        Render(tab, obFil, obCol, fil, col);

        // creadas por mí
        bool terminado = Terminado(tab, obFil, obCol); // Obligatorio el bool aquí dsps de asignar val.
        bool salir = false; // auxiliar para el botón esc

        // bucle ppal
        while (!terminado && !salir)
        {
            char tecla = LeeInput();
            ProcesaInput(tecla, ref fil, ref col, tab);

            bool renderInc = (tecla == 'c'); // aux, true = renderIncorrectas, false = renderNormal

            if (renderInc)
            {
                (bool[] filsInc, bool[] colsInc) = Incorrectas(tab, obFil, obCol);
                RenderIncorrectas(tab, obFil, obCol, fil, col, filsInc, colsInc);
                terminado = Terminado(tab, obFil, obCol);
            }
            else
            {
                Render(tab, obFil, obCol, fil, col);
            }

            if (tecla == 'q') { salir = true; }

        }

        // informe final
        Console.WriteLine();
        if (terminado)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("FELICIDADES has ganao");
            Console.ResetColor();
        }
        else if (salir)
        {
            Console.ForegroundColor= ConsoleColor.Yellow;
            Console.Write("Has salido pulsando ESC");
            Console.ResetColor();
        }
    }

    static void Inicializa(int[,] mat, char[,] tab, int[] obFil, int[] obCol, int fil, int col)
    {
        fil = col = 0; // Estos valores se usan únicamente para la casilla activa del jugador,
        // no tienen nada que ver con obCol ni con obFil

        int N = mat.GetLength(0) - 1;

        // Inicializar matriz tab
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (mat[i, j] == 0) tab[i, j] = ' ';
                else if (mat[i, j] == 1) tab[i, j] = '·';
                else if (mat[i, j] == 2) tab[i, j] = 'X';
            }
        }

        // Sacar objetivosFila y objetivosColumna
        for (int i = 0; i < obFil.Length; i++)
        {
            obFil[i] = mat[i, N];
            obCol[i] = mat[N, i];
        } // Al ser una matriz cuadrada puedo sacar los 2 en el mismo for
    }

    static void Render(char[,] tab, int[] obFil, int[] obCol, int fil, int col)
    {
        int LenFila = tab.GetLength(0); // Variable auxiliar ya que se va a usar unas cuantas vces

        Console.SetCursorPosition(0, 0);
        // Números arriba y primer separador horizontal
        Console.Write("   ");
        for (int i = 0; i < LenFila; i++)
        {
            Console.Write(i + 1 + " ");
        }
        Console.WriteLine();
        Console.Write("   ");
        for (int i = 0; i < LenFila; i++) // SOLO para q cuando se cambie tam. de mat, quede cuki
        {
            Console.Write("--");
        }
        Console.WriteLine();

        // Tabla y números derecha e izquierda
        for (int i = 0; i < LenFila; i++)
        {
            Console.Write(i + 1 + " |"); //  Números izquierda y primer separador vertical
            for (int j = 0; j < LenFila; j++)
            {
                if (i == fil && j == col) // Cursor, como está inicializado en = 0, se pintará
                                          // la primera casilla, ya que tab[i, j] en la primera vuelta de los for es [0,0]
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.Write(tab[i, j]);
                    Console.ResetColor();
                    Console.Write(" ");

                }
                else
                {
                    Console.Write(tab[i, j] + " "); // Valores de las casillas dados por tab
                }
            }
            Console.WriteLine("| " + obFil[i]); // Segundo separador vertical y objetivos de fila
        }

        // Segundo separador horizontal y números abajo
        Console.Write("   ");
        for (int i = 0; i < LenFila; i++)
        {
            Console.Write("--");
        }
        Console.WriteLine();
        Console.Write("   ");
        for (int i = 0; i < LenFila; i++)
        {
            Console.Write(obCol[i] + " ");
        }
    }


    static char LeeInput()
    {
        char d = ' ';
        string tecla = Console.ReadKey(true).Key.ToString();
        switch (tecla)
        {
            case "LeftArrow": d = 'l'; break;
            case "UpArrow": d = 'u'; break;
            case "RightArrow": d = 'r'; break;
            case "DownArrow": d = 'd'; break;
            case "X": d = 'x'; break;  // marcar casilla
            case "V": d = 'v'; break;  // marcar casilla vacia
            case "C": d = 'c'; break;  // comprobar incorrectas
            case "Spacebar": d = 's'; break;  // limpiar casilla
            case "Escape": d = 'q'; break;  // terminar
            default: d = ' '; break;
        }
        return d;
    }

    static void ProcesaInput(char tecla, ref int fil, ref int col, char[,] tab)
    {
        if (tecla == 'u') // Arriba (tecla arriba)
        {
            if (fil <= 0) { fil = tab.GetLength(0) - 1; } // Si fil es 0 y se teclea, se wrapea a 2
            else { fil--; }
        }
        else if (tecla == 'l') // Izquierda (tecla izquierda)
        {
            if (col == 0) { col = tab.GetLength(1) - 1; } // Lo mismo pero por la izquierda
            else { col--; }
        }
        else if (tecla == 'd') // Abajo (tecla abajo)
        {
            if (fil == tab.GetLength(0) - 1) { fil = 0; } // Lo mismo pero por abajo
            else { fil++; }
        }
        else if (tecla == 'r') // Derecha (tecla derecha)
        {
            if (col == tab.GetLength(1) - 1) { col = 0; } // Lo mismo pero por la derecha
            else { col++; }
        }

        else if (tecla == 'x') { tab[fil, col] = 'X'; } // Tachar casilla (tab[x,y] = 2) (tecla X)
        else if (tecla == 'v') { tab[fil, col] = '·'; } // Marcar casilla (tab[x,y] = 1) (tecla V)
        else if (tecla == 's') { tab[fil, col] = ' '; } // Limpiar casilla (tab[x,y] = 0) (tecla Space)
    }

    static int SumaFil(char[,] tab, int fil)
    {
        int n = 0;
        for (int i = 0; i < tab.GetLength(1); i++)
        {
            if (tab[fil, i] == 'X') { n = n + (i + 1); }
        }
        return n;
    }

    static int SumaCol(char[,] tab, int col)
    {
        int n = 0;
        for (int i = 0; i < tab.GetLength(0); i++)
        {
            if (tab[i, col] == 'X') { n = n + (i + 1); }
        }
        return n;
    }

    static bool Terminado(char[,] tab, int[] obFil, int[] obCol)
    {
        int[] resultadosFil = new int[tab.GetLength(0)];
        int[] resultadosCol = new int[tab.GetLength(0)];

        for (int i = 0; i < tab.GetLength(0); i++)
        {
            resultadosFil[i] = SumaFil(tab, i);
            resultadosCol[i] = SumaCol(tab, i);
        }
        /*// debug tonto
        Console.SetCursorPosition(0, 10);
        for (int i = 0; i < resultadosCol.Length; i++)
        {
            Console.WriteLine("resCol: " + resultadosCol[i] + "   objetivoCol: " + obCol[i]);
            Console.WriteLine("resFil: " + resultadosFil[i] + "   objetivoFil: " + obFil[i]);
        }*/

        for (int i = 0; i < obFil.Length; i++)
        {
            if (resultadosCol[i] != obCol[i]) return false;
            if (resultadosFil[i] != obFil[i]) return false;
        }
        return true;
    }

    static (bool[], bool[]) Incorrectas(char[,] tab, int[] obFil, int[] obCol)
    {
        int LFila = tab.GetLength(0);
        bool[] filsInc = new bool[LFila];
        bool[] colsInc = new bool[LFila];

        for (int i = 0; i < LFila; i++)
        {
            int sumaFila = SumaFil(tab, i); // necesitamos estos int para el if de dsps
            int sumaCol = SumaCol(tab, i); // ya que queda más limpito

            if (sumaFila != obFil[i]) { filsInc[i] = true; }
            else { filsInc[i] = false; } // Si el resultado es distinto al objetivo, true

            if (sumaCol != obCol[i]) { colsInc[i] = true; }
            else { colsInc[i] = false; } // Really es lo  mismo que la última parte del mét. Terminado

        }
        return (filsInc, colsInc);
    }

    static void RenderIncorrectas(char[,] tab, int[] obFil, int[] obCol, int fil, int col, bool[] filsInc, bool[] colsInc)
    {
        Render(tab, obFil, obCol, fil, col);

        int LFila = tab.GetLength(0);
        Console.SetCursorPosition(0, 12);

        Console.WriteLine("  WARNING: que una fila/col. esté bien, no implica necesariamente que sus casillas estén bien colocadas.");
        Console.WriteLine();
        Console.WriteLine(" Filas:");
        for (int i = 0; i < LFila; i++)
        {
            if (filsInc[i])
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" Fila {i + 1} mala");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor= ConsoleColor.Green;
                Console.WriteLine($" Fila {i + 1} bien");
                Console.ResetColor();
            }
        }
        Console.WriteLine();

        Console.WriteLine(" Columnas:");
        for (int i = 0; i < LFila; i++)
        {
            if (colsInc[i])
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" Col. {i + 1} mala");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($" Col. {i + 1} bien");
                Console.ResetColor();
            }
        }
    }
}
