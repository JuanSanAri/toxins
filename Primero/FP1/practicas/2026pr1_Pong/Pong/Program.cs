// Juan Sánchez Arias

namespace Pong
{
    internal class Program
    {
        // constantes (fuera del método Main)
        const int ANCHO = 71, ALTO = 20, // ancho y alto del área de juego
        LONG_PALA = 4; // longitud de las palas

        static void Main(string[] args)
        {
            static void Main()
            {
                // Variables
                int pala1Y, pala2Y, // posición vertical del extremo superior de las palpelotaX, pelotaY, // posición de la pelota
                dirX, dirY, // dirección de la pelota
                puntos1, puntos2; // puntos de cada jugador


                // INICIALIZACIÓN DEL ESTADO DEL JUEGO
                bool salir = false;
                while (!salir)
                { // bucle principal
                  // Lectura de teclado no bloqueante y movimiento de las palas
                    if (Console.KeyAvailable)
                    { // ver si se ha pulsado alguna tecla
                        ConsoleKeyInfo tecla = Console.ReadKey(true); // recoger pulsación de tecla
                                                                      // ConsoleKey.W, ConsoleKey.S: actualizar posición pala1
                                                                      // ConsoleKey.I, ConsoleKey.K: actualizar posición pala2
                                                                      // ConsoleKey.Q: salir de juego
                                                                      // ConsoleKey.P: pausar
                    }
                    // RENDERIZADO
                    // Retardo temporal para ralentizar la ejecución y que el juego sea jugable
                    System.Threading.Thread.Sleep(90); // 90 milisegundos
                }
            }
        }

    }
}
