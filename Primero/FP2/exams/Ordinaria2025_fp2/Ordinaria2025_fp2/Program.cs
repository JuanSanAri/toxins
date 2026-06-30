using System.IO;
using Listas;

namespace Sudoku
{
    class MainClass
    {
        static string message = "";
        public static void Main()
        {
            Console.CursorVisible = false;
            int[,] sud;

            Console.Write("¿Usar sudoku de ejemplo?(s/n): ");
            string resp = Console.ReadLine();

            if (resp == "s" || resp == "S")
            {
                sud = new int[,] {
                    {5, 0, 0, 6, 0, 0, 9, 0, 0},
                    {0, 0, 2, 0, 0, 5, 0, 0, 8},
                    {1, 9, 0, 0, 0, 2, 0, 0, 0},
                    {0, 0, 0, 7, 0, 0, 4, 0, 3},
                    {0, 0, 6, 8, 0, 0, 7, 0, 0},
                    {0, 1, 0, 0, 2, 0, 8, 5, 6},
                    {9, 0, 1, 0, 3, 7, 0, 8, 4},
                    {0, 8, 0, 0, 0, 9, 6, 0, 0},
                    {3, 0, 5, 0, 0, 0, 1, 0, 9}
                };
            }
            else
            {
                Console.Write("Nombre del archivo (sudoku): ");
                string file = Console.ReadLine();
                sud = Lee(file);
            }

            Tablero t = new Tablero(sud);
            char c = ' ';
            bool fin = false;

            t.Render();

            while (!fin && c != 'q')
            {
                c = leeInput();
                if (c != ' ')
                {
                    ProcesaInput(c, t);
                    t.Render();
                    Console.SetCursorPosition(0, 10);
                    Console.WriteLine(message);
                    fin = t.FinJuego();
                }
            }

            if (fin) Console.WriteLine("¡Sudoku completado!");
        }

        static int[,] Lee(string file)
        {
            int[,] sud = new int[9, 9];
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(file);
                for (int i = 0; i < 9; i++)
                {
                    string[] nums = sr.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < 9; j++)
                        sud[i, j] = int.Parse(nums[j]);
                }
                return sud;
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("Archivo no encontrado");
            }
            catch (Exception)
            {
                throw new Exception("Formato de archivo incorrecto");
            }
            finally
            {
                if (sr != null) sr.Close();
            }
        }

        static void ProcesaInput(char c, Tablero t)
        {
            message = "";
            // Movimiento del cursor
            if (c == 'l' || c == 'r' || c == 'u' || c == 'd')
            {
                t.MueveCursor(c);
            }
            // Mostrar posibles
            else if (c == 'p')
            {
                Lista posibles = t.DamePosibles();
                Console.SetCursorPosition(0, 10);
                message = "Números posibles: " + posibles.ToString();
            }
            // Poner número
            else if (c >= '0' && c <= '9')
            {
                int num = (int)(c - '0');
                try
                {
                    t.PonNumero(num);
                }
                catch (Exception e)
                {
                    message = "Error: " + e.Message;
                }
            }
        }

        static char leeInput()
        {
            char d = ' ';
            if (Console.KeyAvailable)
            {
                string tecla = Console.ReadKey(true).Key.ToString();
                switch (tecla)
                {
                    case "LeftArrow": d = 'l'; break;
                    case "UpArrow": d = 'u'; break;
                    case "RightArrow": d = 'r'; break;
                    case "DownArrow": d = 'd'; break;
                    case "Escape": case "q": case "Q": d = 'q'; break;
                    case "p": case "P": d = 'p'; break;
                    default:
                        if (tecla.Length == 2 && tecla[0] == 'D' && tecla[1] >= '0' && tecla[1] <= '9') d = tecla[1];
                        else d = ' ';
                        break;
                }
            }
            return d;
        }
    }
}