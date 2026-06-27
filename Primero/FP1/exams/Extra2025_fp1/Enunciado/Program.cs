// Nombre Apellido1 Apellido2
// Laboratorio, puesto

using System;
//using System.Threading;

namespace Main{	
	class MainClass	{	
		static Random rnd = new Random(); // generador de números aleatorios

		struct Coor { public int fil,col;}; // estructura para representar posiciones en la matriz
		
		static bool [,] patron = {{false,false,false,false},  // patrón cuadrado 2x2 con borde vacío
				           		  {false, true,true ,false},
						   		  {false, true,true ,false},
						   		  {false,false,false,false}};

		public static void Main ()
		{
			
		}


	}
}
 
