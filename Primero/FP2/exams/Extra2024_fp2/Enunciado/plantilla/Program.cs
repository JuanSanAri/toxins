
using System;

namespace LightsOut{
    class MainClass  {
        public static void Main(){
  			string [] lineas = {
			    "***..", // fila 0
			    "*..*.", // fila 1
			    "..***", // ...
			    "*.*..",
			    "..*.." };
			Tablero t = new Tablero(lineas); //llamada a la constructora

			// bucle ppal	
            
        }

        static char LeeInput(){
		    char d = ' ';			
			if (Console.KeyAvailable) {	
				string tecla = Console.ReadKey (true).Key.ToString ();
				switch (tecla) {
                // movimiento del cursor 	
				case "LeftArrow":  d = 'l'; break;
				case "UpArrow":    d = 'u'; break;
				case "RightArrow": d = 'r'; break;
				case "DownArrow":  d = 'd'; break;				
				// pulsacion de casilla (click)
				case "Spacebar": d = 'c'; break;				
				// terminar juego
				case "Escape": case "q": d = 'q'; break;
				// deshacer jugada
				case "z": case "Z": d='z'; break;
				// guardar partida
				case "s": case "S": d='s'; break;
				// recuperar partida guardada
				case "o": case "O":	d='o'; break;				
                // reiniciar estado original del juego
				case "i": case "I": d='i'; break;
				default: d=' '; break;
				}
			}  			
			return d;
                     
		}

    }   

}