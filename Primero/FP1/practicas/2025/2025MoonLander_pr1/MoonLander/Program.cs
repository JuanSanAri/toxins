// Juan Sánchez Arias

using System;
using System.Threading;
using System.Diagnostics;
class MoonLander
{
    static Random rnd = new Random();  // generador de aleatorios para colocar la plataforma aleatoriamente
    const int ANCHO = 100, ALTO = 30,  // ancho y alto del área de  juego
              ANCHO_PLAT = 6; // ancho de la plataforma
    const bool DEBUG = true, // mostrar datos para depuración por debajo del HUD
               SON = true;

    static void Main()
    {
        Console.SetWindowSize(ANCHO, ALTO);
        Console.CursorVisible = false;
        Console.OutputEncoding = System.Text.Encoding.UTF8;


        // Estado del módulo de aterrizaje
        double posX = ANCHO / 2, posY = 2, // posición inicial
                velX = 0, velY = 0,       // velocidad inicial
                gravedad = 0.04,    // gravedad lunar
                empuje = -0.3,      // aceleración del propulsor vertical
                lateral = 0.05;     // aceleración de los propulsores horizontales
        int combustible = 200,      // unidades de combustible iniciales
            plataformaX,
            plataformaAncho = 6;        // ancho de la plataforma   

        bool landed = false,
             destroyed = false;


        // situar plataforma en una posición aleatoria dejando al menos 5 posiciones libres a cada lado 
        plataformaX = rnd.Next(ANCHO / 2 - 5, ANCHO / 2 + 6);


        while (!destroyed && !landed)
        { // bucle ppal 
            // RECOGIDA DE INPUT (lectura no bloqueante)
            ConsoleKey tecla = ConsoleKey.NoName;
            if (Console.KeyAvailable) tecla = Console.ReadKey(true).Key;

            // PROCESAMIENTO DE INPUT (LÓGICA)
            if (tecla == ConsoleKey.W && combustible > 0)
            {
                velY += empuje;
                combustible--;
            }
            else
            {
                velY += gravedad;
            }

            if (tecla == ConsoleKey.A && combustible > 0)
            {
                velX -= lateral;
                combustible--;
            }
            else if (tecla == ConsoleKey.D && combustible > 0)
            {
                velX += lateral;
                combustible--;
            }

            // MOTOR DE FÍSICA
            posX = velX + posX;
            posY = velY + posY;


            // Double a int para dibujar en consola
            int posXconsola = (int)Math.Round(posX);
            int posYconsola = (int)Math.Round(posY);
            // RENDERIZADO GRAFICO
            Console.Clear();
            // Módulo lunar, plataforma, HUD (debug)
            if (destroyed == false)
            {
                Console.SetCursorPosition(plataformaX, ALTO - 2);
                Console.ForegroundColor = ConsoleColor.Yellow;
                for (int i = 0; i < plataformaAncho; i++)
                {
                    Console.Write("=");
                }
            }
            if (destroyed == false)
            {
                Console.SetCursorPosition(posXconsola, posYconsola);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("███");
                Console.SetCursorPosition(posXconsola, posYconsola - 1);
                Console.Write(" ▲ ");
                Console.SetCursorPosition(posXconsola, posYconsola + 1);
                Console.Write("╱ ╲");
            }
            if (DEBUG == true)
            {
                Console.SetCursorPosition(0, ALTO - 1);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"VelY={velY.ToString("F2")}  VelX={velX.ToString("F2")}  Fuel:{combustible}  posX = {posX.ToString("F2")}   posY = {posY.ToString("F2")}   plataformX = {plataformaX}");
            }


            // MOTOR DE COLISIONES 
            double nextY = posY + velY;
            double nextX = posX + velX;

            // TECHO
            if (nextY <= 2)
            {
                velY = 0;
                posY = 2;
            }

            // LADOS
            if (nextX <= 2 || nextX >= ANCHO - 3)
            {
                velX = 0;
                nextX = posX;
            }

            // SUELO
            if (nextY >= ALTO - 3)
            {
                if (posX >= plataformaX && posX <= plataformaX + plataformaAncho)
                {
                    if (velY <= 0.5) landed = true;
                    else destroyed = true;
                }
                else
                {
                    destroyed = true;
                }
            }

            // RETARDO PARA CONTROLAR LA VELOCIDAD DE SIMULACIÓN                
            Thread.Sleep(80);
        }

        // INFORME DE RESULTADO DE ATERRIZAJE
        if (landed)
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Has ganado!");
        }

        if (destroyed || combustible == 0)
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Has perdido!");
        }
    }
}
