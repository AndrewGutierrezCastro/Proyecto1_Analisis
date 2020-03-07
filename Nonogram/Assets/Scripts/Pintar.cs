using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pintar : MonoBehaviour
{
    List<GameObject> cuadObj = new List<GameObject>();
    bool[][] matBol = new bool[4][];

    public void PintarCuadros()
    {
        matBol[0]=new bool[]{true,true,false,true};
        matBol[1]=new bool[]{false,true,false,true};
        matBol[2]=new bool[]{false,false,false,true};
        matBol[3]=new bool[]{true,true,false,false};// creacion de matriz booleana de prueba a ser reemplazada
        
        foreach(bool[] fila in matBol)
        {
            foreach(bool estado in fila)
            {

            }
        }
    }
}
