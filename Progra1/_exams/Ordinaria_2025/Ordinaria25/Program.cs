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


            Inicializa(montones, jugadores, turno);
        }

        static void Inicializa(int[] montones, string[] jugadores, int turno)
        {
            int cantJugadores = jugadores.Length;


            turno = rnd.Next(0,cantJugadores + 1);
        }
    }
}
