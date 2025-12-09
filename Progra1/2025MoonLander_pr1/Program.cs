using System;
using System.Threading;
using System.Diagnostics;

class MoonLander {
    static Random rnd = new Random();  // generador de aleatorios para colocar la plataforma aleatoriamente
    const int ANCHO = 100, ALTO = 30,  // ancho y alto del área de  juego
              ANCHO_PLAT = 6; // ancho de la plataforma
    const bool DEBUG = false, // mostrar datos para depuración por debajo del HUD
               SON=true;

    static void Main() {
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


        // situar plataforma en una posición aleatoria dejando al menos 5 posiciones libres a cada lado 
        //...

        while (...){ // bucle ppal 
            // RECOGIDA DE INPUT (lectura no bloqueante)
            ConsoleKey tecla = ConsoleKey.NoName;
            if (Console.KeyAvailable) tecla = Console.ReadKey(true).Key;

            // PROCESAMIENTO DE INPUT (LÓGICA)
            //...

            // MOTOR DE FÍSICA 
            //...

            // RENDERIZADO GRAFICO
            Console.Clear();
            // Módulo lunar, plataforma, HUD (debug)
            // " ▲ "       
            // "███"
            // "╱ ╲"
            // ...
            

            // MOTOR DE COLISIONES 
            // ...

            // RETARDO PARA CONTROLAR LA VELOCIDAD DE SIMULACIÓN                
            Thread.Sleep(80);
        }

        // informe de resultado del aterrizaje
        //...
    }
}
