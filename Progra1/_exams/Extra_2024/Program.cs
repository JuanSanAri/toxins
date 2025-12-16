using System;
using System.IO;

class Hitori{
    static void Main(){
        int  [,] tab;      // números del tablero
        bool [,] tachadas; // casillas tachadas
        int  fil, col;     // posición del cursor

        fil = col = 0;

        tab = new int [,]{
            {4, 4, 1, 2},
            {3, 2, 3, 1},
            {1, 3, 2, 4},
            {2, 1, 4, 3}};

        tachadas = new bool [,]{ 
            {false, false, false, false}, 
            {false, false, false, false}, 
            {false, false, false, false},                 
            {false, false, false, false}}; 


    } // Main


    
    static char LeeInput(){
		char d = ' ';
        		
		string tecla = Console.ReadKey (true).Key.ToString ();
		switch (tecla) {
            /* INPUTS ELEMENTALES PARA EL JUEGO BÁSICO */ 

            // movimiento del cursor 	
		    case "LeftArrow":  d = 'l'; break;
		    case "UpArrow":    d = 'u'; break;
		    case "RightArrow": d = 'r'; break;
		    case "DownArrow":  d = 'd'; break;				

		    // pulsacion de casilla (click)
		    case "Spacebar": d = 'c'; break;				

		    // terminar juego
		    case "Escape": case "q": d = 'q'; break;		
            default: d=' '; break;
		}	
		return d;                     
	}


}