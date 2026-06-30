using System;

namespace Listas
{
	public class Lista{
		class Nodo{
			public int dato;
			public Nodo sig; // enlace al siguiente nodo

			// constructoras
			public Nodo(int e){ dato = e; sig = null;}
			public Nodo(int e, Nodo n){ 
				dato = e; sig = n;

			}

		}
		// referencia al primer nodo
		Nodo pri;

		// constructora 
		public Lista(){ pri =null; }

		public bool EsVacia(){ return pri==null; }

		// insertar al ppio
		public void InsertaPri(int x){
			Nodo aux = new Nodo(x);
			aux.sig = pri;
			pri = aux;
		}

		// borrar primero
		public void EliminaPri(){
			if (pri==null) throw new Exception("EliminaPri");
			else pri=pri.sig;
		}

		// obtener primero
		public int DamePri(){
			if (pri==null) throw new Exception("DamePri");
			else return pri.dato;
		}

		// ver lista (como string), para depuración
		public string SacaLista(){
			Nodo aux = pri;
			string sal="";
			while (aux!=null){
				sal += aux.dato.ToString()+" ";
				aux = aux.sig;
			}
			return sal;
		}	
	}
}

