namespace Kakurasu;

public class Program
{
    static void Main()
    {
        // ejemplo para desarrollo
        int[,] ex1 = {
            {-1, -1, -1, 4}, // última col: suma por filas
            {-1,  0, -1, 5},
            {-1,  1, -1, 0},
            { 1,  2,  3, 0}  // ultima fil: suma por cols; el último 0 no cuenta
        };

        // Variables y cosas
        Console.CursorVisible = false;
        bool terminado = false;
        bool exit = false;
        char c = ' ';
        int[,] juego;

        // Empieza carga
        Console.Write("¿Ejemplo o archivo? [e/a]: ");
        string res = Console.ReadLine();
        if (res == "a")
        {
            Console.Write("Nombre del archivo (ex2.txt): ");
            string ruta = Console.ReadLine();
            juego = LeerArchivo(ruta);
        }
        else juego = ex1;
        // Ini
        Tablero t = new Tablero(juego);
        t.Render();
        // Bucle
        while (!terminado && !exit)
        {
            c = LeeInput();
            
            if (c == 'q') exit = true;

            t.ProcesaInput(c);
            t.Render();
            if (c == 'p') t.Pista();
            terminado = t.Terminado();
        }
        Console.Clear();
        if (terminado) Console.WriteLine("¡Has ganado!");
        else Console.WriteLine("Has salido.");
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
            case "E": d = 'e'; break;  // utilizar ejemplo
            case "A": d = 'a'; break;  // leer de archivo   
            case "N": d = 'n'; break;  // casilla negra
            case "B": d = 'b'; break;  // casilla blanca 
            case "P": d = 'p'; break;  // pista  
            case "Spacebar": d = 's'; break;  // limpiar casilla
            case "Escape": d = 'q'; break;  // terminar
            default: d = ' '; break;
        }
        return d;
    }

    static int[,] LeerArchivo(string ruta)
    {
        StreamReader sr = null;
        try
        {
            sr = new StreamReader(ruta);
            int tam = int.Parse(sr.ReadLine());
            int[,] mat = new int[tam + 1, tam + 1];

            for (int i = 0; i < tam; i++)
            {
                string[] aux = sr.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (aux.Length != tam + 1) throw new Exception("Número de columnas incorrecto");
                for (int j = 0; j <= tam; j++)
                    mat[i, j] = int.Parse(aux[j]);
            }

            string[] auxi = sr.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (auxi.Length != tam) throw new Exception("Fila de objetivos incorrecta");
            for (int i = 0; i < tam; i++)
                mat[tam, i] = int.Parse(auxi[i]);

            sr.Close();
            return mat;
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException("No se encuentra el archivo");
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
}
