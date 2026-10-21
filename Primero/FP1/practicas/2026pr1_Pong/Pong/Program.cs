using System;
using System.Diagnostics;

class Program
{
    // Constantes
    const int ANCHO = 71, ALTO = 20,  // ancho y alto del área de juego (sin contar la parte de puntuación)              
              LONG_PALA = 4;          // longitud de las palas

    static Random rnd = new Random(); // para aleatorizar la dirección de la pelota

    static void Main()
    {
        Console.CursorVisible = false; // para no dibujar el cursor en pantalla

        // Variables e inicialización
        int pala1Y = ALTO / 2, pala2Y = ALTO / 2,    // posición vertical de las palas
            pelotaX = ANCHO / 2, pelotaY = ALTO / 2,  // posición de la pelota
            dirX = 1, dirY = 1,       // dirección de la pelota
            puntos1 = 0, puntos2 = 0;  // puntos de cada jugador

        bool salir = false;
        while (!salir)
        {

            // LEER TECLADO
            if (Console.KeyAvailable)
            { // ver si se ha pulsado alguna tecla
                ConsoleKeyInfo tecla = Console.ReadKey(true); // recoger pulsación de tecla
                // tecla puede ser: 
                // ConsoleKey.W, ConsoleKey.S, ConsoleKey.I, ConsoleKey.K: movimiento palas
                // ConsoleKey.Q, ConsoleKey.P:  salir, pausar

            }

            // MOVIMIENTO PELOTA

            // COLISIÓNES: MUROS SUPERIOR E INFERIOR, SALIDA DEL ÁREA DE JUEGO (PUNTO JUGADOR), PALAS 


            // RENDERIZADO
            Console.Clear(); // borrado de pantalla

            // Límites
            Console.SetCursorPosition(0, ALTO - 1);
            Console.BackgroundColor = ConsoleColor.Green;
            for (int i = 0; i < ANCHO; i++) Console.Write(" ");
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < ANCHO; i++) Console.Write(" ");
            Console.ResetColor();

            // Red central
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int i = 1; i <= ALTO - 2; i++)
            {
                Console.SetCursorPosition(ANCHO / 2, i);
                Console.Write("|");
            }

            // Pelota
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(pelotaX, pelotaY);
            Console.Write("o");

            // Palas
            for (int i = pala1Y; i < pala1Y + LONG_PALA; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(1, i);
                Console.Write("|");
            }
            for (int i = pala2Y; i < pala2Y + LONG_PALA; i++)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.SetCursorPosition(ANCHO - 2, i);
                Console.Write("|");
            }
            Console.ResetColor();

            // Puntos jugadores
            Console.SetCursorPosition(0, ALTO);
            Console.Write($"Jugador 1: {puntos1}    Jugador 2: {puntos2}");


            // Retardo
            System.Threading.Thread.Sleep(90);

            // borrado de la pelota y palas para el siguiente frame
        }
    }
}