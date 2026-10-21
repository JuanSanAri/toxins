using System;
using System.Diagnostics;

class Program {
    const int ANCHO = 71, ALTO = 20,  // ancho y alto del área de juego (sin contar la parte de puntuación)              
              LONG_PALA = 4;          // longitud de las palas

    static Random rnd = new Random(); // para aleatorizar la dirección de la pelota

    static void Main() {
        Console.CursorVisible = false; // para no dibujar el cursor en pantalla

        int pala1Y, pala2Y,    // posición vertical de las palas
            pelotaX, pelotaY,  // posición de la pelota
            dirX, dirY1,       // dirección de la pelota
            puntos1, puntos2;  // puntos de cada jugador


        // Inicialización de las variables de estado del juego       

        bool salir = false;
        while (!salir) {

            // LEER TECLADO
            if (Console.KeyAvailable) { // ver si se ha pulsado alguna tecla
                ConsoleKeyInfo tecla = Console.ReadKey(true); // recoger pulsación de tecla
                // tecla puede ser: 
                // ConsoleKey.W, ConsoleKey.S, ConsoleKey.I, ConsoleKey.K: movimiento palas
                // ConsoleKey.Q, ConsoleKey.P:  salir, pausar

            }
            
            // MOVIMIENTO PELOTA
 
            // COLISIÓNES: MUROS SUPERIOR E INFERIOR, SALIDA DEL ÁREA DE JUEGO (PUNTO JUGADOR), PALAS 
        
            // RENDERIZADO
            Console.Clear(); // borrado de pantalla
            // dibujo red central, pelota, palas, puntos jugadores

            // Retardo
            System.Threading.Thread.Sleep(90);

            // borrado de la pelota y palas para el siguiente frame
        }
    }
}