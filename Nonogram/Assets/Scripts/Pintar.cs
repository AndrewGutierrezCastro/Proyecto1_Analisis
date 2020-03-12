using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pintar : MonoBehaviour
{
    List<GameObject> cuadObj = new List<GameObject>();
    bool[][] matBol = new bool[4][];

    public void PintarCuadros()//entrada es la matriz bool
    {
        matBol[0]=new bool[]{true,true,false,true};
        matBol[1]=new bool[]{false,true,false,true};
        matBol[1]=new bool[]{false,true,false,true};
        matBol[2]=new bool[]{false,false,false,true};
        matBol[3]=new bool[]{true,true,false,false};// creacion de matriz booleana de prueba a ser reemplazada

       List<List<GameObject>> cuad=Analizador.single.cuadricula;

        for (int i=0; i<matBol.Length; i++)
        {
            for (int j=0; j<matBol[i].Length;j++)
            {
                print(cuad.Count);
                //if (matBol[i][j])
                //{
                //    Renderer rend = cuad[i][j].GetComponent<Renderer>();
                //}
            }
        }
    }
}
