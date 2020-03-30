using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pintar : MonoBehaviour
{
    List<GameObject> cuadObj = new List<GameObject>();
    int[][] matBol = new int[4][];

    public void PintarCuadros()//entrada es la matriz bool
    {
        matBol[0]=new int[]{0,2,2,1};
        matBol[1]=new int[]{0,1,1,2};
        matBol[2]=new int[]{2,2,2,2};
        matBol[3]=new int[]{2,0,0,1};// creacion de matriz entera de prueba a ser reemplazada

       List<List<GameObject>> cuad=Analizador.single.cuadricula;

        for (int i=0; i<matBol.Length; i++)
        {
            for (int j=0; j<matBol[i].Length;j++)
            {
                if (matBol[i][j]==2)
                {
                    Renderer rend = cuad[i][j].GetComponent<Renderer>();
                    rend.material.color = Color.black;
                }
            }
        }
    }
    public void paintMatrix(int[][] pMatriz){
        List<List<GameObject>> cuad=Analizador.single.cuadricula;
        Renderer rend; 
        for (int i=0; i<pMatriz.Length; i++)
        {
            for (int j=0; j<pMatriz[i].Length;j++)
            {
                rend = cuad[i][j].GetComponent<Renderer>();
                switch(pMatriz[i][j]){
                    case 0:
                        rend.material.color = Color.white;
                    break;
                    case 1:
                        rend.material.color = Color.black;
                    break;
                    case 2:
                        rend.material.color = Color.red;
                    break;

                }
            }
        }
    }
}
