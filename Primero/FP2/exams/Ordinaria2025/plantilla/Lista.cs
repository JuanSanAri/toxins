using System;

namespace Listas{ // listas enlazadas de ENTEROS 
	public class Lista{

		// CLASE NODO (clase privada para los nodos de la lista)
		private class Nodo{
			public int dato;   // información del nodo (podría ser de cualquier tipo)
			public Nodo sig;   // referencia al siguiente

			public Nodo(int _dato=0, Nodo _sig=null) {  // valores por defecto dato=0; y sig=null
				dato = _dato;
				sig = _sig;
			}
		}
		// FIN CLASE NODO

		// campo pri: referencia al primer nodo de la lista
		Nodo pri;  


		// constructora de la clase Lista
		public Lista(){  
			pri = null;   //  lista vacia
		}


		// insertar elto e al ppio de la lista
		public void InsertaPpio(int e){  
			pri = new Nodo(e,pri);			
		}


		public void InsertaFin(int e){
			if (pri==null) pri = new Nodo(e,pri);			
			else {
				Nodo aux=pri;
				while (aux.sig!=null) aux=aux.sig;
				aux.sig = new Nodo(e);			
			}
		}


		public bool EsVacia() {return pri==null;}

		
		// buscar elto e
		public bool BuscaDato(int e){
			Nodo aux = pri; // referencia al primero para buscar de ppio a fin
			  // búsqueda de nodo con elto e
			while (aux!=null && aux.dato!=e) aux = aux.sig;

			// termina con aux==null (elto no encontrado)
			// o bien con aux apuntando al primer nodo con elto e
			return aux!=null;
		}
	

		// Conversion a string
        // método ToString que se invoca implícitamente cuando se hace Console.Write
        public override string ToString() { 
			string salida = "";						
			Nodo aux = pri;
			while (aux!=null) {
				salida += aux.dato + " ";
				aux = aux.sig;
			}			
			return salida;
        }


		// Devuelve el num de eltos de la lista.		
		// public int Cuenta(){ }

	}

}

