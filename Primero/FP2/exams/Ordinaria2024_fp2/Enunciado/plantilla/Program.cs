using System;
using System.Collections;
using System.IO;

namespace Kakurasu;

public class Program {
    static void Main(){ 
        // ejemplo para desarrollo
        int[,] ex1 = { 
            {-1, -1, -1, 4}, // última col: suma por filas
            {-1,  0, -1, 5},
            {-1,  1, -1, 0},
            { 1,  2,  3, 0}  // ultima fil: suma por cols; el último 0 no cuenta
        };
     
    }

        static char LeeInput() {
            char d = ' ';
            string tecla = Console.ReadKey(true).Key.ToString ();
			switch (tecla) {
			    case "LeftArrow":  d = 'l'; break;
			    case "UpArrow":    d = 'u'; break;
			    case "RightArrow": d = 'r'; break;
			    case "DownArrow":  d = 'd';	break;
                case "E":          d = 'e'; break;  // utilizar ejemplo
                case "A":          d = 'a'; break;  // leer de archivo   
                case "N":          d = 'n'; break;  // casilla negra
                case "B":          d = 'b'; break;  // casilla blanca 
                case "P":          d = 'p'; break;  // pista  
                case "Spacebar":   d = 's'; break;  // limpiar casilla
                case "Escape":     d = 'q'; break;  // terminar
                default:           d = ' '; break;
			}   
			return d;
		}

}
