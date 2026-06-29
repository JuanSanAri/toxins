// Disclaimer: Hecha completamente por IA, solo quería revisar conceptos en esta hoja.
namespace Listas
{
    public class Lista
    {
        private class Nodo
        {
            public int dato;
            public Nodo sig;
            public Nodo(int _dato = 0, Nodo _sig = null)
            {
                dato = _dato; sig = _sig;
            }
        }

        Nodo pri;

        public Lista() { pri = null; }

        public void InsertaPpio(int e) { pri = new Nodo(e, pri); }

        public bool BuscaDato(int e)
        {
            Nodo aux = pri;
            while (aux != null && aux.dato != e) aux = aux.sig;
            return aux != null;
        }

        // Devuelve número de elementos recorriendo la lista
        public int NumElems()
        {
            int n = 0;
            Nodo aux = pri;
            while (aux != null) { n++; aux = aux.sig; }
            return n;
        }

        // Suma todos los datos recorriendo la lista
        public int Suma()
        {
            int suma = 0;
            Nodo aux = pri;
            while (aux != null) { suma += aux.dato; aux = aux.sig; }
            return suma;
        }

        // Cuenta cuántas veces aparece e
        public int CuentaOcs(int e)
        {
            int n = 0;
            Nodo aux = pri;
            while (aux != null)
            {
                if (aux.dato == e) n++;
                aux = aux.sig;
            }
            return n;
        }

        // Inserta e en la posición pos (0-based)
        // Si pos no existe devuelve false
        public bool InsertaPos(int e, int pos)
        {
            if (pos == 0) { InsertaPpio(e); return true; }
            Nodo aux = pri;
            int i = 0;
            // avanzamos hasta la posición anterior a pos
            while (aux != null && i < pos - 1) { aux = aux.sig; i++; }
            if (aux == null) return false; // pos no existe
            aux.sig = new Nodo(e, aux.sig);
            return true;
        }

        // Inserta al final con un solo recorrido
        public void InsertaUlt(int e)
        {
            if (pri == null) { pri = new Nodo(e); return; }
            Nodo aux = pri;
            while (aux.sig != null) aux = aux.sig; // llegamos al último
            aux.sig = new Nodo(e);
        }

        // Borra la primera aparición de e
        // Necesita nodo anterior (ant) para reenlazar
        public bool BorraElto(int e)
        {
            Nodo ant = null, aux = pri;
            while (aux != null && aux.dato != e) { ant = aux; aux = aux.sig; }
            if (aux == null) return false; // no encontrado
            if (ant == null) pri = pri.sig; // era el primero
            else ant.sig = aux.sig;         // reenlazamos saltando aux
            return true;
        }

        // Devuelve el n-ésimo nodo (0-based), null si no existe
        private Nodo NesimoNodo(int n)
        {
            Nodo aux = pri;
            int i = 0;
            while (aux != null && i < n) { aux = aux.sig; i++; }
            return aux;
        }

        // Devuelve el n-ésimo elemento, lanza excepción si no existe
        public int Nesimo(int n)
        {
            Nodo nodo = NesimoNodo(n);
            if (nodo == null) throw new Exception("Posición " + n + " no existe");
            return nodo.dato;
        }

        // Inserta e en la posición n-ésima
        public void InsertaNesimo(int n, int e)
        {
            if (n == 0) { InsertaPpio(e); return; }
            Nodo ant = NesimoNodo(n - 1); // nodo anterior a la posición n
            if (ant == null) throw new Exception("Posición " + n + " no existe");
            ant.sig = new Nodo(e, ant.sig);
        }

        // Borra todas las apariciones de e
        public void BorraTodos(int e)
        {
            while (BorraElto(e)) ; // reutilizamos BorraElto hasta que devuelva false
        }

        // Borra el n-ésimo elemento
        public void BorraNesimo(int n)
        {
            if (n == 0)
            {
                if (pri == null) throw new Exception("Lista vacía");
                pri = pri.sig; return;
            }
            Nodo ant = NesimoNodo(n - 1);
            if (ant == null || ant.sig == null) throw new Exception("Posición " + n + " no existe");
            ant.sig = ant.sig.sig; // saltamos el n-ésimo
        }

        // Invierte el orden de la lista
        // Idea: ir insertando al principio de una nueva lista
        public void Invierte()
        {
            Nodo nueva = null;
            Nodo aux = pri;
            while (aux != null)
            {
                nueva = new Nodo(aux.dato, nueva); // insertar al principio
                aux = aux.sig;
            }
            pri = nueva;
        }

        // Iguales: mismos elementos en el mismo orden
        public bool Iguales(Lista l)
        {
            Nodo a = pri, b = l.pri;
            while (a != null && b != null)
            {
                if (a.dato != b.dato) return false;
                a = a.sig; b = b.sig;
            }
            return a == null && b == null; // ambas terminan a la vez
        }

        // Iguales2: mismos elementos en cualquier orden
        // Para cada elemento de this, comprobamos que aparece en l
        // y que el número de ocurrencias coincide
        public bool Iguales2(Lista l)
        {
            if (NumElems() != l.NumElems()) return false;
            Nodo aux = pri;
            while (aux != null)
            {
                if (CuentaOcs(aux.dato) != l.CuentaOcs(aux.dato)) return false;
                aux = aux.sig;
            }
            return true;
        }

        public override string ToString()
        {
            string s = "";
            Nodo aux = pri;
            while (aux != null) { s += aux.dato + " "; aux = aux.sig; }
            return s;
        }
    }
}