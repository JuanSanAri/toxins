using System;
using System.IO;
using Listas;

namespace Sudoku{
	class MainClass{ 
		public static void Main (){
			int[,] mat = {
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




		// static void ProcesaInput(char c, Tablero t){


        static char leeInput(){
		    char d = ' ';
			
			if (Console.KeyAvailable) {					
				string tecla = Console.ReadKey (true).Key.ToString ();				
				switch (tecla) {

                /* INPUTS ELEMENTALES PARA EL JUEGO BÁSICO */ 
                
                // movimiento del cursor 	
				case "LeftArrow":  d = 'l'; break;
				case "UpArrow":    d = 'u'; break;
				case "RightArrow": d = 'r'; break;
				case "DownArrow":  d = 'd'; break;				
				
				// terminar juego
				case "Escape": case "q": case "Q": d = 'q'; break;
				
				// ver posibles valores en posicion	
				case "p": case "P": d='p'; break;				

				// lectura de dígito
				default:			
					if (tecla.Length==2 && tecla[0]=='D' && tecla[1]>='0' && tecla[1]<='9') d=tecla[1];
					else d = ' ';
					break;
				}
			}  			
			return d;                     
		} 

	} 

}




