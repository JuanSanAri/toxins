using System;
using System.IO;
namespace Main
{
    class Nim
    {
        static Random rnd = new Random(); // generador de aleatorios
        const int NUM_MONTONES = 5, MAX_PALILLOS = 4;
        public static void Main()
        {
            string[] jugadores = { "Ana", "Berto", "Carla", "Humano" };
            int[] montones = new int[NUM_MONTONES];
            int turno;

            int num = 0;
            int mon = 0;
            int pals;

            Inicializa(montones, jugadores, out turno);
            Render(montones, jugadores, turno, num, mon);

            while (!FinJuego(montones))
            {
                if (turno == 3)
                {
                    JuegaHumano(montones, out mon, out pals);
                }
                else
                {
                    JuegaMaquina(montones, mon, pals);
                }

                Render(montones, jugadores, turno, num, mon);
            }
        }

        static void Inicializa(int[] montones, string[] jugadores, out int turno)
        {
            int cantJugadores = jugadores.Length;

            for (int i = 0; i < NUM_MONTONES; i++)
            {
                montones[i] = rnd.Next(1, MAX_PALILLOS + 1);
            }
            turno = rnd.Next(0, cantJugadores + 1);
        }

        static void Render(int[] montones, string[] jugadores, int turno, int num, int mon)
        {
            if (num == 0) { Console.Write("Empieza el juego!!"); }
            else if (num > 0)
            {
                Console.Write($"{jugadores[turno]} quita {num} del montón {mon}");
            }
            Console.WriteLine();

            for (int i = 0; i < montones.Length; i++)
            {
                Console.Write(i + ": ");
                for (int j = 0; j < montones[i]; j++)
                {
                    Console.Write("|");
                }
                Console.WriteLine();
            }
        }

        static void JuegaHumano(int[] montones, out int mon, out int pals)
        {
            do
            {
                Console.Write($"Humano, elige montón del 0 al {NUM_MONTONES - 1}(-1 para terminar): ");
                mon = int.Parse(Console.ReadLine());
            } while (mon < -1 || mon >= NUM_MONTONES);

            if (mon > -1)
            {
                do
                {
                    Console.Write($"Cuántos palillos quieres del montón {mon}: ");
                    pals = int.Parse(Console.ReadLine());
                } while (pals < 1 || pals > montones[mon]);

                montones[mon] = montones[mon] - pals;
            }
            else { pals = 0; }
        }

        static void JuegaMaquina(int[] montones, int mon, int pals)
        {

        }

        static bool FinJuego(int[] montones)
        {
            return false;
        }
    }
}