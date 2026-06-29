namespace Coords{
    public class Coor{
        int fil, col; // componentes x (fila) e y (columna) de la coordenada

        public Coor(int fil=0, int col=0){ 
            this.fil = fil;
            this.col = col;
        }
        public int Fil{   
            get => fil;  
            set => fil = value; 
        }
      
        public int Col{   
            get => col;  
            set => col = value; 
        }
        
        public override string ToString(){
            return $"({fil},{col})";
        }

        public static Coor operator+(Coor c1, Coor c2){
            return new Coor(c1.fil+c2.fil, c1.col+c2.col);
        }   

        public static Coor Parse(string s){
            int ini,fin; // buscamos índices de "(" y de ")"
            (ini,fin) = (s.IndexOf("("),s.IndexOf(")"));
            // nos quedamos con la subcadena entre medias de ambos
            s = s.Substring(ini+1,fin-ini-1);
            // dividimos con la ","
            string [] nums = s.Split(",",StringSplitOptions.RemoveEmptyEntries);
            // parseamos los dos enteros y construimos coordenada
            return new Coor(int.Parse(nums[0]), int.Parse(nums[1]));
        }

        // igualdad y desigualdad de coordenadas
        public static bool operator==(Coor c1, Coor c2){
            return c1.fil==c2.fil && c1.col==c2.col;
        }

        public static bool operator!=(Coor c1, Coor c2){
            return !(c1==c2); //c1.fil!=c2.fil || c1.col!=c2.col;
        }
    }
}