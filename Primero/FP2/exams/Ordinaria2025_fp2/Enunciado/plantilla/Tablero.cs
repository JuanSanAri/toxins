using Listas;

namespace Sudoku{

	public class Tablero {
		// estructura para representar las casillas del tablero
		struct Casilla {
			public int num;    // numero en la casilla
			public bool fija;  // casilla fijada inicialmente (true) o vacia (false)
		}

		Casilla [,] mat;  // matriz de casillas

		Coor cursor; // posicion actual del cursor en el tablero

        public Tablero(){}


    }  

}
