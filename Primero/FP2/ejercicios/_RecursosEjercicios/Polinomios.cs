using System;
using System.IO;


namespace polinomios
{

	class MainClass	{
		const double EPS = 1e-15;
		const bool DEBUG = true;
		const int N = 100;  // tamaño de los arrays de monomios

		struct Monomio {    // coeficiente y exponente
			public double coef;
			public int exp;
		}

		struct Polinomio {
			public Monomio [] mon; // array de monomios
			public int nMons; // num de monomios = primera pos libre en el array mon
		}


		public static void Main () {
			Polinomio p1, p2;

			LeePolinomio (out p1);
			LeePolinomio (out p2);

			Polinomio p3 = Suma(p1, p2);
			Console.WriteLine ();
			EscribePolinomio (p3);

		}

		// "Igualdad" de nums en coma flotante 
		static bool EqDouble(double c1, double c2){ return Math.Abs(c1-c2)<EPS;	}			


		static void LeeMonomio(out Monomio m){	
			m.exp = 0;	// para garantizar valor de salida	
			Console.WriteLine("Monomio: ");			
			Console.Write("  coef: ");			
			m.coef = double.Parse(Console.ReadLine());			
			// solo leemos exp si coef!=0
			if (!EqDouble(m.coef,0)) {
				Console.Write("  exp: ");			
				m.exp = int.Parse(Console.ReadLine());			
			}
		}


		// esta versión de LeePolinomio utiliza el método Inserta que garantiza que no se insertan monomios con coef=0
		// ni con exponentes repetidos (se suman en ese caso)
		static void LeePolinomio(out Polinomio p){
			p.mon = new Monomio[N];
			p.nMons = 0;

			Console.Write ("Introduce monomios (coef=0 para terminar):\n ");			
			Monomio m;
			LeeMonomio(out m);
			while (!EqDouble(m.coef,0)) {
				Inserta(m, ref p);				
				if (DEBUG) { EscribePolinomio(p); Console.WriteLine();}
				LeeMonomio(out m);				
			}
		}

		static void EscribePolinomio(Polinomio p){
			for (int i = 0; i < p.nMons; i++) {
				if (p.mon[i].coef>0) Console.Write (" + ");
				Console.Write(p.mon[i].coef + "x^" + p.mon [i].exp);
			}
		}

		static void Inserta(Monomio m, ref Polinomio p){
			if (!EqDouble(m.coef,0)) {  // solo insertamos si coef!=0
				int i = 0;
				// busqueda de mon del mismo grado
				while (i<p.nMons && m.exp != p.mon[i].exp) i++;

				if (i < p.nMons) { // monomio encontrado
					// sumo coefs
					double c = p.mon [i].coef + m.coef;

					// si la suma se anula eliminamos monomio: copiamos el último a esta pos y decrementamos 
					// nMons. Nota. si el pol tiene 1 solo mon y se anula, copia componente sobre sí misma
					// y la descarta con nMons-- (podría distinguirse ese caso especial)
					if (EqDouble(c,0)) { 
						p.mon [i] = p.mon [p.nMons - 1];  
						p.nMons--; 						  											
					} else p.mon [i].coef = c; // si no se anula reemplazamos coef

				} else { // añadimos m al final, si cabe
					if (p.nMons==N) Console.WriteLine ("error: polinomio lleno");					
					else {
						p.mon [p.nMons] = m;
						p.nMons++;	
					}
				}
			}
		}


		// lee un polinomio de un archivo		
		static void LeeArchivo(string a, out Polinomio p) {
			StreamReader file;
			file = new StreamReader(a);

			p.nMons=0; 
			p.mon = new Monomio[N];
			while (!file.EndOfStream) {
				string s = file.ReadLine();
				string [] ss = s.Split(" ", StringSplitOptions.RemoveEmptyEntries);
				if (ss.Length==2) {
					Monomio m;
					m.coef = double.Parse(ss[0]);
					m.exp = int.Parse(ss[1]);					
					Inserta(m,ref p);
				}
			}
			file.Close();
		}



		// hace una copia del polinomio p, copiando todos sus datos
		// (sin compartir referencias en el heap)
		static Polinomio Copia(Polinomio p){
			Polinomio copia;
			copia.mon = new Monomio[N];   // generamos el array de monomios
			copia.nMons = p.nMons;        // mismo num de monomios
			for (int i=0; i<p.nMons; i++) // copiamos monomomios uno a uno
			 	copia.mon[i] = p.mon[i];  // asignacion de structs!!
			return copia;
		}


		static Polinomio Suma(Polinomio p1, Polinomio p2){
			Polinomio p3, p4;
			if (p1.nMons<p2.nMons) {
				p3 = Copia(p2); // pol de salida
				p4 = p1;
			} else {
				p3 = Copia(p1); // pol de salida
				p4 = p2;
			}
			// en p3 tengo copiado el grande
			// en p4 tengo referencia al pequeño

			// y ahora insertamos uno a uno los momios de p4 en p3
			for (int i=0; i<p4.nMons; i++) Inserta (p4.mon[i], ref p3);
			return p3;
		}


		static Polinomio Resta(Polinomio p1, Polinomio p2){
			Polinomio p3 = Copia(p1); // pol de salida

			// y ahora insertamos uno a uno los momios de p2 en p3
			for (int i=0; i<p2.nMons; i++){
				Monomio m = p2.mon[i];
				m.coef *= -1;
				Inserta(m, ref p3);
			}
			return p3;
		}


		static int Grado(Polinomio p){
			int gr = 0;
			// recorrido
			for (int i=0; i<p.nMons; i++)
				if (gr < p.mon [i].exp)
					gr = p.mon [i].exp;
			return gr;
		}


		static double Evalua(Polinomio p, double v){
			double res = 0;
			for (int i=0; i<p.nMons; i++)
				res += p.mon [i].coef * Math.Pow (v, p.mon [i].exp);
			return res;
		}


		static Polinomio Multiplica(Polinomio p1, Polinomio p2){
			Polinomio p3;
			p3.mon = new Monomio[N];
			p3.nMons = 0;
			// multiplicamos todos los monomios de p1 por todos los de p2
			// recorrido en p1
			for (int i = 0; i < p1.nMons; i++) {
				// recorrido en p2
				for (int j = 0; j < p2.nMons; j++) {
					Monomio m;
					m.coef = p1.mon [i].coef * p2.mon [j].coef;
					m.exp = p1.mon [i].exp + p2.mon [j].exp;
					Inserta (m, ref p3);
				}
			}
			return p3;
		}


		// busca el coef asosicado a un expnente exp
		// 0 si no existe ese exponente
		static double Coef(Polinomio p, int exp){
			double c = 0;
			// búsqueda
			int i=0;
			// mientras no llegemos al al final de la estructura
			// y no encontremos el monomio buscado, avanzar
			while (i < p.nMons && p.mon [i].exp != exp)	i++;
			// notese que no hay problema en utilizar exp como parámetro y nombre de campo

			// si no hemos llegado al final, hemos parado pq 
			// hemos encontrado el monomio
			if (i < p.nMons) c = p.mon [i].coef;
			return c;
		}




		
		static bool Iguales(Polinomio p1, Polinomio p2){			
			if (p1.nMons != p2.nMons) 
				return false;
			else {
				int i=0;
				while (i<p1.nMons && 
					EqDouble(p1.mon[i].coef,Coef(p2,p1.mon[i].exp))) i++;
				return i==p1.nMons;
			}

		}




		// static void Divide(Polinomio p1, Polinomio p2, out Polinomio coc, out Polinomio res){


		// insercion con redimensión de array 
		static void InsertaRedim(Monomio m, ref Polinomio p){
			if (!EqDouble(m.coef,0)) {  // solo insertamos si coef!=0
				int i = 0;
				while (i<p.nMons && m.exp != p.mon[i].exp) i++;
				if (i < p.nMons) { // monomio encontrado					
					double c = p.mon [i].coef + m.coef;
					if (EqDouble(c,0)) { 
						p.mon [i] = p.mon [p.nMons - 1];  
						p.nMons--; 						  											
					} else p.mon [i].coef = c; // si no se anula reemplazamos coef
				} else { // añadimos m al final
					if (p.nMons==p.mon.Length) {  // si no cabe redimensionamos
						Monomio [] nuevo = new Monomio[p.mon.Length*2]; // duplicamos tamaño
						for (int j=0; j<p.nMons; j++) nuevo[j]=p.mon[j];					
						// cambiamos referncia al array de monomios
						p.mon = nuevo;
						Console.WriteLine($"Pol redimensionado. Nueva dimension {p.mon.Length}");
					}
					// añadimos nuevo al final
					p.mon [p.nMons] = m;
					p.nMons++;	
				}
			}
		}	

	}
}








