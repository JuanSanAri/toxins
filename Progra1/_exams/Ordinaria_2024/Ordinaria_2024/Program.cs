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

            int mon = 0;
            int pals = 0;

            Inicializa(montones, jugadores, out turno);
            Render(montones, jugadores, turno, pals, mon);

            while (!FinJuego(montones))
            {
                if (turno == 3)
                {
                    JuegaHumano(montones, out mon, out pals);
                }
                else
                {
                    JuegaMaquina(montones, out mon, out pals);
                }

                Render(montones, jugadores, turno, pals, mon);


                if (FinJuego(montones))
                {
                    Console.WriteLine($"Se acabó, {jugadores[turno]} gana");
                }
                else turno = (turno + 1) % jugadores.Length; // magnífica esta línea

                // DEBUG
            }
            
        }

        static void Inicializa(int[] montones, string[] jugadores, out int turno)
        {
            int cantJugadores = jugadores.Length;

            for (int i = 0; i < NUM_MONTONES; i++)
            {
                montones[i] = rnd.Next(1, MAX_PALILLOS + 1);
            }
            turno = rnd.Next(0, cantJugadores);
        }

        static void Render(int[] montones, string[] jugadores, int turno, int pals, int mon)
        {
            if (pals == 0) { Console.Write("Empieza el juego!!"); }
            else if (pals > 0)
            {
                Console.Write($"{jugadores[turno]} quita {pals} pal. del montón {mon}");
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
            Console.WriteLine();
        }

        static void JuegaHumano(int[] montones, out int mon, out int pals)
        {
            do
            {
                Console.Write($"Humano, elige montón del 0 al {NUM_MONTONES - 1} (-1 para terminar): ");
                mon = int.Parse(Console.ReadLine());
            } while (mon < -1 || mon >= NUM_MONTONES || montones[mon] < 1);

            if (mon > -1 && montones[mon] > 0)
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

        static void JuegaMaquina(int[] montones, out int mon, out int pals)
        {
            do
            {
                mon = rnd.Next(0, NUM_MONTONES);
            } while (montones[mon] == 0);

            pals = rnd.Next(1, montones[mon] + 1);
            montones[mon] = montones[mon] - pals;
        }

        static bool FinJuego(int[] montones)
        {
            int contVacios = 0;

            for (int i = 0; i < NUM_MONTONES; i++)
            {
                if (montones[i] == 0) contVacios++;
                if (contVacios == NUM_MONTONES) return true;
            }
            return false;
        }

        //enddd
    }
}