using System;
using System.Threading;

namespace macianos
{
    internal class MainClass
    {
        const int DELTA = 400; // Velocidad de juego
        const int FILS = 14, COLS = 22;

        static Random rnd = new Random(); // Aleatorios para el mvimiento del enemigo

        public static void Main(string[] args)
        {
            Console.SetWindowSize(COLS, FILS); // Tamaño de la consola (anchura x, altura y)
            Console.CursorVisible = false; // Ocultamos cursor en pantalla

            while ()
            {
                // Lógica del jugador
                // recogida no bloqueante de INPUT DE USUARIO
                string dir = "";
                if (Console.KeyAvailable)
                { // si se detecta pulsación de tecla
                  // leemos input y transformamos a string
                    dir = (Console.ReadKey(true)).KeyChar.ToString();
                    // limpiamos buffer para no acumular pulsaciones entre frames
                    while (Console.KeyAvailable) Console.ReadKey(true);

                    switch (dir)
                    {
                        // movimiento
                        case "w":
                            break;
                        case "s":
                            break;
                        case "a":
                            break;
                        case "d":
                            break;

                        case "k": // disparar
                            break;

                        case "q": // salir
                            break;
                    }
                }
                System.Threading.Thread.Sleep(DELTA);
            }
        }
    }
}
