namespace Ordinaria2023_fp1
{
    class SolitarioParejas
    {
        static Random rnd = new Random(); // generador de aleatorios (se usará al final)
        const int DESCUBIERTAS = 8; // número de cartas descubiertas en la mesa
        static void Main()
        {
            int[] mazo = new int[40], // array de cartas del mazo
            mesa = new int[DESCUBIERTAS]; // array de cartas de la mesa
            int prim; // posición de la primera carta aún no extraída del mazo
            InicializaMazo(mazo, out prim); // método ya implementado que genera el mazo
        }

	// Nota: este método como dice en el main lo daba Jaime pero yo no
	// lo tengo, por lo que recomiendo hacer el IniMazoAleatorio de primeras
        static void InicializaMazo(int[] mazo, out int prim)
        {
            prim = 0;
            for (int i = 0; i < 40; i++)
                mazo[i] = i;
        }

    }
}