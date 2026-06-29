using System;
using System.IO;
using Listas;

namespace Kakurasu;

public class Tablero {
    private enum Casilla { NoDef, Negra, Blanca }; // estado de la casilla    
    private int N;  // lado de la matriz de juego
    private Casilla[,] matriz; // matriz de juego
    private int[] objetivosFila,    // sumas objetivo por fila
                  objetivosColumna; // sumas objetivo por columna
    private int fil, col; // posición del cursor

    public Tablero(int[,] datos) { }

    public void Render() { }


}
