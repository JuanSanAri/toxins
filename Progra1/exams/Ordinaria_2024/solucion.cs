using System;
using System.IO;

namespace Main {
	class MainClass	{
		static Random rnd = new Random();
		const int NUM_MONTONES = 5, MAX_PALILLOS = 4;
		

		public static void Main () {
			int [] montones = new int[NUM_MONTONES];
			string [] jugadores = {"Ana", "Berto", "Carla", "Humano"};	
			int turno;
			
			// preguntamos si se quiere recuperar una partida
			string r=" ";
			do {
				Console.Write("Recuperar partida [s/n]? ");
				r = Console.ReadLine();
			} while (r!="s" && r!="n");

			// iniciamos aleatoriamente o recuperamos partida de archivo
			if (r=="n") Inicializa(montones, jugadores, out turno);
			else LeeArchivo(montones,out turno);



			Render(montones,jugadores,turno,0,0);			

			int mon=0, num;
			bool finJuego = false;
			while (mon!=-1 && !finJuego) {
				// se hace jugada, de humano o de otro jugador (maquina)				
				if (jugadores[turno]=="Humano") 
					JuegaHumano(montones, out mon, out num);						
				else 
					JuegaMaquina(montones,out mon, out num);

				Render(montones,jugadores,turno,num,mon);

				finJuego = FinJuego(montones);

				// si no acaba el juego, avance de turno
				if (mon!=-1 && !finJuego) // juego no terminado
					if (Palindromo(montones)) // repite turno
						Console.WriteLine($"Palíndromo! {jugadores[turno]} repite turno");
					else // avanza turno
						turno = (turno+1)%jugadores.Length;
				
			} // fin while

			// si el juego ha termindo, informe del ganador
			if (mon!=-1) Console.WriteLine("El ganador es: "+jugadores[turno]);
			// si no ha terminado, preguntamos si se quiere guardar
			else {
				r = "";
				do {
					Console.Write("Guardar partida [s/n]? ");
					r = Console.ReadLine();
				} while (r!="s" && r!="n");
				// si se quiere guardar, se guarda en archivo 
				if (r=="s") { 
					GuardaPartida(montones,turno);				
					Console.WriteLine("Partida guardada en \"saved\"");
				}
			}
		}


		static void Inicializa(int [] montones, string [] jugadores, out int turno){
			// rellenamos cada monton con un num de palillos aleatorio entre 1 y MAX_PALILLOS
			for (int i=0; i<montones.Length; ++i) 
				montones[i]=rnd.Next(1,MAX_PALILLOS+1);			
			// escogemos un turno aleatorio
			turno = rnd.Next(0,jugadores.Length);
		}


		static void Render(int [] montones, string [] jugadores, int turno, int num, int mon){
			if (num==0) 
				Console.WriteLine("Empieza el juego");
			else
				Console.WriteLine($"{jugadores[turno]} quita {num} del montón {mon}"); 
			
			// mostramos los montones
			for (int i=0; i<montones.Length; ++i) {
				Console.Write(i + " ");
				for (int j=0; j<montones[i]; ++j) {
					Console.Write("|");
				}
				Console.WriteLine();
			}
			Console.WriteLine();
		}


		static void JuegaHumano(int [] montones, out int mon, out int num){
			Console.WriteLine("Humano, tu turno:");
			do { // pedimos monton entre que exista y tenga palillos (o -1 para terminar)
				Console.Write("Montón (-1 para terminar): ");
				mon = int.Parse(Console.ReadLine());
			} while (mon<-1 || mon >= NUM_MONTONES || (mon>0 && montones[mon]==0));

			// si no quiere terminar, pedimos numero de palillos entre 1 y montones[mon]
			if (mon>=0) {
				do { // numero de palillos en [1,montones[mon]]
					Console.Write("Palillos: ");
					num = int.Parse(Console.ReadLine());
				} while (num<=0 || num > montones[mon]);
				// se hace la jugada
				montones[mon] -= num;
			} else num=0;
		}

		static void JuegaMaquina(int [] montones, out int mon, out int num){
			// seleccionamos montón con palillos
			do { 
			 mon = rnd.Next(0,montones.Length);
			} while (montones[mon]==0);

			// quitamos un nun aleatorio, al menos uno
			num = rnd.Next(1,montones[mon]+1);
			montones[mon] -= num;
		}





		static bool FinJuego(int [] montones){
			int i=0;
			// buscamos montón con palillos
			while (i<montones.Length && montones[i]==0) i++;
			// si no hay montones con palillos (i llega al final), fin del juego
			return (i==montones.Length);
		}



		static void GuardaPartida(int [] montones, int turno){	
			StreamWriter file = new StreamWriter("saved");
			for (int i=0; i<montones.Length; i++) file.Write(montones[i]+" ");
			file.Write(turno);
			file.Close();
		}

		static void LeeArchivo(int [] montones, out int turno){
			StreamReader file = new StreamReader("saved");
			string [] s = file.ReadLine().Split(' ');
			// leemos montones en los primeros elementos
			for (int i=0; i<s.Length-1; i++) montones[i]=int.Parse(s[i]);
			// leemos turno en el último elemento
			turno = int.Parse(s[s.Length-1]);			
			file.Close();
		}


		static bool Palindromo(int [] montones){			
			int i=0;			
			// recorremos hasta la mitad o hasta encontrar una discrepancia
			while (i<montones.Length/2 && montones[i]==montones[montones.Length-i-1]) ++i;
			// si llegamos a la mitad sin discrepancia, es palindromo
			return i==montones.Length/2;
		}

	}
}