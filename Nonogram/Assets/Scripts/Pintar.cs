using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pintar : MonoBehaviour
{
    void Update()
    {
        Renderer rend;
        List<List<GameObject>> cuad = Analizador.single.cuadricula;
        for (int i = 0; i < Analizador.single.matUpdate.Length; i++)
        {
            for (int j = 0; j < Analizador.single.matUpdate[i].Length; j++)
            {
                rend = cuad[i][j].GetComponent<Renderer>();
                switch (Analizador.single.matUpdate[i][j])
                {
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
