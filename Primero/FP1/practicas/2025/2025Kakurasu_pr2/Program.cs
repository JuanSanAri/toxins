// ESTO ES EL ENUNCIADO
namespace Kakurasu;

public class Program {    
    static void Main() {    
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

        int N = mat.GetLength(0) - 1;
        
        // inicialización y renderizado inicial

        // bucle ppal

        // informee final

    }



    static char LeeInput() {
        char d = ' ';
        string tecla = Console.ReadKey(true).Key.ToString ();
		switch (tecla) {
		    case "LeftArrow":  d = 'l'; break;
		    case "UpArrow":    d = 'u'; break;
		    case "RightArrow": d = 'r'; break;
		    case "DownArrow":  d = 'd';	break;
            case "X":          d = 'x'; break;  // marcar casilla
            case "V":          d = 'v'; break;  // marcar casilla vacia
            case "C":          d = 'c'; break;  // comprobar incorrectas
            case "Spacebar":   d = 's'; break;  // limpiar casilla
            case "Escape":     d = 'q'; break;  // terminar
            default:           d = ' '; break;
		}   
		return d;
	}


}
