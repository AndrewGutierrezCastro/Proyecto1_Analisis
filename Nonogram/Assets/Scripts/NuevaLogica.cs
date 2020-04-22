using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class NuevaLogica : MonoBehaviour
{
    public int[][] matriz;
    public Boolean pintar = false;
    public int filasTamano;
    public int colTamano;
    private List<int> ordenPistas = new List<int>();
    private int[][] pistFilMaList; //cambiar a array de listas
    private int[][] pistColMaList; //cambiar a array de listas

    //public void imprimir(int[][] matImp)
    //{
    //    Console.Write("\n\n");
    //    for (int k = 0; k < matImp.Length; k++)
    //    {
    //        Console.Write(k + " ");
    //        int n = 0;
    //        while (n < matImp[k].Length)
    //        {
    //            Console.Write(matImp[k][n]);
    //            n++;
    //        }
    //        Console.Write("\n");
    //    }
    //}
    public void setPistasFilas(int[][] pPistFilMaList) //cambiar a array de listas
    {
        pistFilMaList = pPistFilMaList;
    }
    public void setPistasColumnas(int[][] pPistColMaList) //cambiar a array de listas
    {
        pistColMaList = pPistColMaList;
    }
    public void setMatrizSize(int pFila, int pColumna)
    {
        filasTamano = pFila;
        colTamano = pColumna;
        matriz = new int[pFila][];
        for (int i = 0; i < pFila; i++)
        {
            matriz[i] = new int[pColumna];
        }
        //imprimir(matriz);

    }

    public void Inicial()
    {
        setPistasFilas(Analizador.single.matFil.ToArray());
        setPistasColumnas(Analizador.single.matCol.ToArray());
        setMatrizSize(Analizador.single.cantFilCol[0], Analizador.single.cantFilCol[1]);

        pintar = true;
        Thread hilo = new Thread(inicioThread);
        hilo.Start();
        //imprimir(matriz);
        //Console.Write("Fin de la resolvacion");
    }

    public void inicioThread()
    {
        resolver(0, matriz);
    }
    public Boolean resolver(int indice, int[][] pMatriz)
    {
        //Console.Write("matriz");
        if (indice >= matriz.Length)
        {
            matriz = pMatriz;
            return true;
        }
        else
        {
            int[] filaTemp = new int[matriz[indice].Length];
            Array.Copy(matriz[indice], filaTemp, matriz[indice].Length);
            int[] posicionesIni = new int[pistFilMaList[indice].Length];
            int PistAct = 0;
            int PosAct = 0;
            bool posible = false;
            bool proximo = false;
            bool ultimo = false;
            while (!posible)
            {
                //Console.Write("\n\n");
                if (PosAct + pistFilMaList[indice][PistAct] <= filaTemp.Length)
                {
                    for (int posi = 0; posi <= pistFilMaList[indice][PistAct]; posi++)
                    {
                        if (posi < pistFilMaList[indice][PistAct])
                            filaTemp[PosAct + posi] = 1;
                        else
                        {
                            if (PistAct + 1 == posicionesIni.Length)
                                ultimo = true;
                            else
                                ultimo = false;
                            List<int> colEditadas = new List<int>() { PosAct, PosAct + posi - 1 };
                            int[][] matTempo = new int[matriz.Length][];
                            Array.Copy(pMatriz, matTempo, matriz.Length);
                            matTempo[indice] = filaTemp;
                            if (revisarColumnas(colEditadas, matTempo, indice, ultimo))
                            {
                                Analizador.single.matUpdate = matTempo;
                                posicionesIni[PistAct] = PosAct;
                                PosAct = PosAct + posi + 1;
                                if (PistAct + 1 != posicionesIni.Length)
                                    PistAct++;
                                else
                                    proximo = true;
                            }
                            else
                            {
                                for (int i = PosAct; i < filaTemp.Length; i++)
                                {
                                    filaTemp[i] = 0;
                                }
                                PosAct = PosAct + 1;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    if (PistAct > 0)
                    {
                        PistAct--;
                        PosAct = posicionesIni[PistAct] + 1;
                        for (int i = PosAct - 1; i < filaTemp.Length; i++)
                        {
                            filaTemp[i] = 0;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                if (proximo == true)
                {
                    int[][] matTemp = new int[matriz.Length][];
                    Array.Copy(pMatriz, matTemp, matriz.Length);
                    matTemp[indice] = filaTemp;
                    if (resolver(indice + 1, matTemp))
                    {
                        posible = true;
                    }
                    else
                    {
                        PosAct = posicionesIni[PistAct] + 1;
                        for (int j = PosAct - 1; j < filaTemp.Length; j++)
                        {
                            filaTemp[j] = 0;
                        }
                        proximo = false;
                    }
                }
            }
            return true;
        }
    }

    private Boolean revisarColumnas(List<int> pColumnasEditadas, int[][] pMatrizTmp, int numFila, Boolean esUltPist)
    {
        /*Revisar columnas
        Se recibe la lista con las columnas modificadas por ende a revisar, ademas de la
        matris
        */
        int[][] nuevaMatriz = new int[pMatrizTmp[0].GetLength(0)][];
        for (int i = 0; i < pMatrizTmp.GetLength(0); i++)
        {
            nuevaMatriz[i] = new int[pMatrizTmp.GetLength(0)];
        }
        for (int i = 0; i < pMatrizTmp.GetLength(0); i++)
        {
            for (int j = 0; j < pMatrizTmp[0].GetLength(0); j++)
            {
                nuevaMatriz[j][i] = pMatrizTmp[i][j];
            }
        }
        List<int> copyPista;
        Revisadores.Revisador[] revisadores = new Revisadores.Revisador[filasTamano];
        int inicio = pColumnasEditadas.First(), final = pColumnasEditadas.Last();
        int iniRes=0, finRes=0;
        Boolean unaPista = false;
        if(esUltPist){
                if(pistFilMaList[numFila].GetLength(0) > 1){
                    inicio = 0;
                    final = nuevaMatriz.GetLength(0)-1;
                }else{
                    for(int i = inicio; i <= final; i++){
                        copyPista = pistColMaList[i].ToList();
                        revisadores[i] = new Revisadores.Revisador(0, nuevaMatriz[i], 0, copyPista, numFila);
                        revisadores[i].revisarColumnas();
                        if(revisadores[i].errorColumna){
                            return false;
                        }                  
                    }
                    unaPista = true;
                    iniRes = inicio;
                    finRes = final;
                    inicio = 0;
                    final = nuevaMatriz.GetLength(0)-1;
                } 
            }
            for(int i = inicio; i <= final; i++){
                if(unaPista){
                    if(i>=iniRes & i<=finRes){
                        continue;
                    }
                }
                copyPista = pistColMaList[i].ToList();
                revisadores[i] = new Revisadores.Revisador(0, nuevaMatriz[i], 0, copyPista, numFila);
                revisadores[i].revisarColumnas();
                if(revisadores[i].errorColumna){
                    return false;
                }                   
            }
            if(esUltPist & numFila<pMatrizTmp.GetLength(0)-1){
                int pistasSigFila = 0;
                for (int i = inicio; i <= final; i++)
                {   
                    if(revisadores[i].faltaPunto>0){
                        pistasSigFila++;
                    }
                }
                if(pistasSigFila > pistFilMaList[numFila+1].Sum()){
                    return false;
                }
            }
        return true;
    }
}


