using System.Diagnostics;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;


namespace RationalNums{
    class Rational{
        // atributos
        int num, den; // numerador y denominador
        // invs representacion
        // - den>0 y fracción irreducible


        public Rational(int num=0, int den=1){ 
            if (den==0) {
                Console.WriteLine("Error Rational: denominador 0");
                // qué hacemos a partir de aquí?
                // una solución es "inventarse" un valor, informar y continuar
                this.num = 0;
                this.den = 1;
                Console.WriteLine("Se ha creado el racional 0/1");
                // mas adelante trataremos este caso como EXCEPCIÓN
            } else {
                this.num = num;
                this.den = den;                
                Normalize();
            }
        }        

        // Auxiliares privadas para garantizar invariantes
        private void Normalize(){
            int m = Mcd(Math.Abs(num),Math.Abs(den));
            if (den<0) (num,den) = (-num,-den);
            num = num/m;
            den = den/m;
        }

        private int Mcd(int n, int m){
            while (m != 0) (n,m) = (m,n%m);          
            return n;
        }


        // operador + unario
        public static Rational operator+(Rational r){
            return r;
        }

        // operador - unario (respeta invariantes de representación)
        public static Rational operator-(Rational r){
            return new Rational(-r.num,r.den);
        }

        public static Rational operator+(Rational r1, Rational r2){
            return new Rational(r1.num*r2.den+r2.num*r1.den, r1.den*r2.den);
        }

        public static Rational operator-(Rational r1, Rational r2){
            return r1 + (-r2);
        }

        // public static Rational operator*(Rational r1, Rational r2) ...
        // public static Rational operator/(Rational r1, Rational r2) ... ojo con la división entre 0


        // "sobreescritura" del método ToString para poder escribir racionales directamente con Console.Write(c);
        // este método construye la cadena que queremos imprimir, que será el valor de retorno
        public override string ToString(){
            return $"{num}/{den}";
        }


        // para poder leer definimos el método Parse
        public static Rational Parse(string s){
            string [] nums = s.Split("/",StringSplitOptions.RemoveEmptyEntries);
            // parseamos los dos enteros y construimos rational, que se normalizará
            return new Rational(int.Parse(nums[0]), int.Parse(nums[1]));

            // mejoras: si leemos un solo entero a, podemos interpretar  a/1
        }
        




    }


}