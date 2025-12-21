/**
 * QUÉ SE DEBE SABER PARA APROBAR ESTE EXAMEN
 * Una lista de prioridades para desenvolverse bien (realmente en cualquier examen de Jaime en FP1)
 * 
 * 1. CONTROL ABSOLUTO DE MATRICES Y SU LÓGICA EN BUCLES FOR (VELOCIDAD, si te ves 
 *    lento programando practica y practica, NO vas a aprobar si no controlas lógica)
 * 2. JUNTO A ELLO, ARRAYS, SABER CÓMO SE COMPENETRAN (cómo sacar una fila de una matriz p.e.)
 * 3. PLAYER (muy extenso, pero básicamente tener control de fil y col)
 * 4. MANEJO DE CONDICIONES DE FINALIZACIÓN (IMPORTANTÍSIMO!!!!)
 * 5. ENTENDIMIENTO DE MÉTODOS Y ARGUMENTOS (argumentos con out, ref, métodos bool, tuplas...)
 *    ^^^HASTA AQUÍ^^^ Tendrías alrededor de 7-8 pts en el examen
 *    
 *    Como alumno, recomiendo entrenar y practicar todos los exámenes que encuentres,
 *    todas las prácticas, etc. PARECE QUE TIENES COLCHÓN, PERO NO, los métodos
 *    iniciales te dan como mucho hasta 4pts, LO COMPLICADO es conseguir los siguientes
 *    3-4pts y normalmente van de la mano, es decir, que esos pts. dependen unos de otros,
 *    mi más sincero consejo es que os centréis en sacar esto, el examen SE HACE MUY CORTO,
 *    si sientes que vas sobrado estudiando puedes sacar el Guarda o el Lee de sobra,
 *    pero incluso los mejores hacen estos métodos en los últimos minutos del examen.
 *    
 *    
 *    SIMULACRO EXAMEN EN CASA (2 hrs. y 30 mins.):
 *       Reglas:
 *       - Papel y boli, NADA de google, chatGPT, apuntes
 *       - Activaos un crono
 *       - NO música (bueno si pero muy bajita) lo suficiente como para q tengas la presión del examen
 *    
 *    1. Parte inicial: normalmente se compone de inicializa, render, inputs, salida, etc.
 *       HACEDLA LENTO, sé que es contradictorio porque es lo más fácil del examen, pero insisto,
 *       unos cimientos mal colocados os van a dar mucho dolor de cabeza.
 *       TIEMPO RECOMENDADO: MAX. 45 MINS (si se hace lento y bien, cae en 20-30 min.)
 *       
 *    2. LO TOCHO: se compone básicamente de la lógica que le vamos a dar al juego, en esta
 *       zona entran complejidades de bucles y alguna cosilla que si no has estudiado lo
 *       suficiente te pilla, recomiendo altamente LEER muy bien lo que te está pidiendo,
 *       y de verdad, papel y boli, para escribiros, dibujar, lo que sea que os abra el coco.
 *       TIEMPO RECOMENDADO: MAX 1H 30M (en 1 hora debes estar acabando)
 *       
 *    3. REPASO: tiempo comodín para arreglar cosas, o por si no has terminado algo terminarlo,
 *       insisto en que es obligatorio que os pongáis un temporizador mental en el cual
 *       os OBLIGÁIS a acabar esas dos partes que os he mencionado antes alrededor de 2h dentro
 *       del examen, para que luego esta ventana de tiempo os salve la vida.
 *       
 *    4. DE XULO: esto es por si eres una bestia, memoriza métodos Guarda/Lee, se hacen en 5 mins,
 *       y luego completarlos en el Main puede ser un poco lío, así que dale un buen repaso, pero 
 *       se tarda 5 mins también, y digo que se tarda muy poco ya que si te sobra tiempo para esto
 *       probablemente esto lo sacas en 3 segundos y vás más que sobrado al examen.
 *       
 *       
 *       Mensaje final: Es una asignatura jodida, sobretodo si no has programado en tu vida,
 *       aprende a ver las cosas de otro modo y no pierdas la esperanza, TODOS estuvimos alguna
 *       vez en tu sitio, mucho ánimo y no te rindas de verdad, programa todos los días y verás que
 *       le empiezas a pillar cariño. Mucha suerte!!
 *       
 *       NOTA: no tengo el archivo que requerimos para el método LeeArchivo, por lo que hay
 *       aquí hecho es solo hasta los 7,5 puntos.
 */

namespace Ordinaria_2025
{
    internal class Program
    {
        const int N = 4; // tamaño del tablero de ejemplo
        // IMPORTANTE: se debe cambir el tamaño de N para que corresponda
        // con el tamaño de nuestra matriz cuadrada de tamaño NxN

        static void Main(string[] args)
        {
            char[,] tab;
            tab = new char[N, N] { // tablero del ejemplo
                {'.','1','.','0'}, // fila 0
                {'.','.','0','.'}, // fila 1
                {'.','0','.','.'}, // etc
                {'1','1','.','0'}
            };

            bool[,] fijas = new bool[N, N]; // matriz de posiciones fijas
            int fil, col; // fila y columna de la casilla activa

            Inicializa(tab, fijas, out fil, out col);
            Renderiza(tab, fijas, fil, col);

            while (!TabLleno(tab))
            {
                char c = LeeInput();
                ProcesaInput(c, tab, fijas, ref fil, ref col);

                Renderiza(tab, fijas, fil, col);
            }

            // Condiciones de finalización
            if (TabLleno(tab) && !MuestraResultado(tab))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Enhorabuena, lo has conseguido!!");
                Console.ResetColor();
            }
            else
            {
                Renderiza(tab, fijas, fil, col);
                Console.SetCursorPosition(0, 5);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Casi casi!!");
                MuestraResultado(tab);
                Console.ResetColor();
            }
        }

        static void Inicializa(char[,] tab, bool[,] fijas, out int fil, out int col)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (tab[i, j] == '.') { fijas[i, j] = false; }
                    else { fijas[i, j] = true; }
                }
            }
            fil = col = 0;
            Console.SetCursorPosition(col, fil);
        }

        static void Renderiza(char[,] tab, bool[,] fijas, int fil, int col)
        {
            Console.Clear();
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    // Color del texto
                    if (fijas[i, j] == true) { Console.ForegroundColor = ConsoleColor.Blue; }
                    else { Console.ForegroundColor = ConsoleColor.Yellow; }

                    // Color del fondo dependiendo de la posición del jugador
                    if (col == j && fil == i)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(tab[i, j]);
                        Console.ResetColor();
                        Console.Write(" ");
                    }
                    else { Console.Write(tab[i, j] + " "); }
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        static char LeeInput()
        {
            char d = ' ';
            string tecla = Console.ReadKey(true).Key.ToString();

            switch (tecla)
            {
                // Movimiento del cursor 	
                case "LeftArrow": d = 'l'; break;
                case "UpArrow": d = 'u'; break;
                case "RightArrow": d = 'r'; break;
                case "DownArrow": d = 'd'; break;

                // Casillas
                case "Spacebar": d = 'c'; break;
                case "D0": d = '0'; break;
                case "D1": d = '1'; break;

                // Terminar juego
                case "Escape": case "q": d = 'q'; break;

                default: d = ' '; break;
            }
            return d;
        }

        static void ProcesaInput(char c, char[,] tab, bool[,] fijas, ref int fil, ref int col)
        {
            // eje x
            if (c == 'l' && col > 0) { col--; }
            else if (c == 'r' && col < N - 1) { col++; }
            // eje y
            if (c == 'd' && fil < N - 1) { fil++; }
            else if (c == 'u' && fil > 0) { fil--; }

            // casillas
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (fijas[i, j] == false && col == j && fil == i)
                    {
                        if (c == 'c') { tab[i, j] = '.'; }
                        else if (c == '0') { tab[i, j] = '0'; }
                        else if (c == '1') { tab[i, j] = '1'; }
                    }
                }
            }

            // escape
            if (c == 'q') return;
        }


        static bool TabLleno(char[,] tab)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (tab[i, j] == '.') { return false; }
                }
            }
            return true;
        }

        static void SacaFilCol(int k, char[,] tab, char[] filk, char[] colk)
        {
            for (int i = 0; i < N; i++)
            {
                colk[i] = tab[i, k];
                filk[i] = tab[k, i];
            }
        }

        static bool TresSeguidos(char[] v)
        {
            int ceroCount = 0;
            int unoCount = 0;

            for (int i = 0; i < N; i++)
            {
                if (v[i] == '0')
                {
                    ceroCount++;
                    unoCount = 0;
                }
                else if (v[i] == '1')
                {
                    unoCount++;
                    ceroCount = 0;
                }
                else
                {
                    ceroCount = 0;
                    unoCount = 0;
                }

                if (ceroCount == 3 || unoCount == 3) { return true; }
                // CONDICIÓN DE CONTADOR DENTRO DEL FOR (recordatorio xq soy un inútil)
            }
            // SI LO PUSIESE AQUÍ, no sirve para nada, se quedaría con las cuentas de la última vuelta de i
            return false;
        }

        static bool IgCerosUnos(char[] v)
        {
            int ceroCount = 0;
            int unoCount = 0;

            for (int i = 0; i < N; i++)
            {
                if (v[i] == '0') { ceroCount++; }
                else if (v[i] == '1') { unoCount++; }
            }
            if (ceroCount == unoCount) { return true; }
            return false;
        }

        static bool MuestraResultado(char[,] tab)
        {
            char[] colk = new char[N];
            char[] filk = new char[N];

            bool fallo = false;

            for (int i = 0; i < N; i++)
            {
                SacaFilCol(i, tab, filk, colk);

                // tres seguidos
                if (TresSeguidos(filk))
                {
                    Console.WriteLine("Tres iguales seguidos en la fila " + i);
                    fallo = true;
                }
                if (TresSeguidos(colk))
                {
                    Console.WriteLine("Tres iguales seguidos en la columna " + i);
                    fallo = true;
                }

                // ceros y unos
                if (!IgCerosUnos(filk))
                {
                    Console.WriteLine("No mismo número de 0s y 1s en la fil. " + i);
                    fallo = true;
                }
                if (!IgCerosUnos(colk))
                {
                    Console.WriteLine("No mismo número de 0s y 1s en la col. " + i);
                    fallo = true;
                }

                Console.WriteLine();
            }
            return fallo;
        }
        //end
    }
}