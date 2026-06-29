using Coords;
namespace hoja5;

class SetCoor
{
    // atributos de la clase
    Coor[] coors; // array con coordenadas
    int oc; // núm de componentes ocupadas del array = primera pos libre

    public SetCoor(int tam = 10) // Bob
    {
        coors = new Coor[tam]; // Inicializamos
        oc = 0;
    }

    private int SearchElem(Coor c)
    {
        for (int i = 0; i < oc; i++) // Búsqueda de toda la vida
            if (coors[i] == c) { return i; }

        return -1; // c no está en coors[]
    }

    public bool Add(Coor c)
    {
        // 1. Está lleno el array? si está lleno, salimos
        if (oc == coors.Length) throw new Exception("ERROR: array lleno");

        // 2. Existe c en nuestro array? si existe, salimos
        if (SearchElem(c) != -1) return false;

        // 3. Si no ocurre nada de lo "malo", hacemos lo que nos dice el enunciado
        coors[oc] = c;
        oc++;
        return true;
    }

    public bool Belongs(Coor c)
    {
        if (SearchElem(c) != -1) return true; // Existe en coors, belongs = true
        return true;
    }

    public override string ToString() // Extendemos usando el ToString de Coor
    {
        string s = "";
        for (int i = 0; i < oc; i++)
            s += coors[i].ToString() + " ";
        return s;
    }

    public bool Remove(Coor c)
    {
        int pos = SearchElem(c);

        if (pos != -1)
        {
            for (int i = pos; i < oc - 1; i++) // Ojito al oc
            {
                coors[i] = coors[i + 1];
            }
            oc--;
            return true;
        }
        return false;
    }

    public Coor PopElem()
    {
        if (oc == 0) throw new Exception("Array vacío");

        // Menor coste computacional = menos operaciones posibles
        oc--; // oc-- PRIMERO: pasa a apuntar justo al último elemento válido
        return coors[oc]; // devuelve el último y lo "elimina" decrementando oc
    }

    public int Size()
    {
        return oc;
    }

    public static SetCoor operator +(SetCoor e1, SetCoor e2)
    {
        SetCoor result = new SetCoor(e1.oc + e2.oc); // Tamaño máx. posible

        // Añadir todos los de e1
        for (int i = 0; i < e1.oc; i++)
            result.Add(e1.coors[i]);

        // Añadir los de e2 (Add ignora los duplicados)
        for (int i = 0; i < e2.oc; i++)
            result.Add(e2.coors[i]);

        return result;
    }

    public static SetCoor operator -(SetCoor e1, SetCoor e2)
    {
        SetCoor result = new SetCoor(e1.oc);

        // Comparamos espacios (los arrays)
        // Añadimos solo los de e1 que NO están en e2
        for (int i = 0; i < e1.oc; i++)
        {
            for (int j = 0; j < e2.oc; j++)
                if (!e2.Belongs(e1.coors[i]))  // si NO está en e2
                    result.Add(e1.coors[i]);
        }

        return result;
    }

    public static SetCoor operator &(SetCoor e1, SetCoor e2)
    {
        SetCoor result = new SetCoor(e1.oc);

        // Añadimos solo los de e1 que SÍ están en e2
        for (int i = 0; i < e1.oc; i++)
        {
            for (int j = 0; j < e2.oc; j++)
                if (e2.Belongs(e1.coors[i]))  // si SÍ está en e2
                    result.Add(e1.coors[i]);
        }

        return result;
    }

    // Bool de subespacios primero que me he pispao de que va muy bien para ==, !=
    public bool IsSubset(SetCoor s)
    {
        for (int i = 0; i < oc; i++)
            if (!s.Belongs(coors[i])) return false;// si alguna coor no está, no es subconjunto
        return true; // todos los coor del conjunto están en el conjunto s, -> true
    }

    public static bool operator ==(SetCoor e1, SetCoor e2)
    {
        if (e1.oc != e2.oc) return false; // Tamaños distintos -> no son iguales
        return e1.IsSubset(e2); // Utilizamos el método de subespacios que ya
                                // compara y verifica la igualdad de ambas coor
    }

    // Negación de la igualdad
    public static bool operator !=(SetCoor e1, SetCoor e2) { return !(e1 == e2); }

    public SetCoor Copy()
    {
        SetCoor copia = new SetCoor(oc);
        for (int i = 0; i < oc; i++)
            copia.Add(coors[i]);
        return copia;
    }


    static void Main()
    {
        SetCoor s = new SetCoor();

        // Add
        Console.WriteLine(s.Add(new Coor(1, 2))); // true
        Console.WriteLine(s.Add(new Coor(3, 4))); // true
        Console.WriteLine(s.Add(new Coor(1, 2))); // false, ya está

        // Belongs
        Console.WriteLine(s.Belongs(new Coor(1, 2))); // true

        // Escribir
        Console.WriteLine(s.ToString());
    }
}