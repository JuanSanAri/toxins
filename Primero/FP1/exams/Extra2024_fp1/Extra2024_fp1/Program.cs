namespace letrasDeslizantes{
    class Program{
        static Random rnd = new Random(); // generador de números aleatorios

        struct Coor { // Coordenadas para representar posiciones
            public int fil, col; }

        static void Main() {
            char[,] tab; // tablero de juego
            Coor pos; // posicion actual del cursor


        }




        static char LeeInput()
        {
            char d = ' ';
            if (Console.KeyAvailable)
            {
                string tecla = Console.ReadKey(true).Key.ToString();
                switch (tecla)
                {
                    /* INPUTS ELEMENTALES PARA EL JUEGO BÁSICO */
                    // movimiento del cursor 	
                    case "LeftArrow": d = 'l'; break;
                    case "UpArrow": d = 'u'; break;
                    case "RightArrow": d = 'r'; break;
                    case "DownArrow": d = 'd'; break;
                    case "Spacebar": d = 's'; break;

                    // terminar juego
                    case "Escape": case "q": case "Q": d = 'q'; break;

                    default: d = ' '; break;
                }
            }

            return d;
        }

    }
}

















