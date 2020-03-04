using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Analizador : MonoBehaviour
{

    public string path= BuscadorArchivos.singelton.direccion;
    public int[] cantFilColnew=new int[2];


    public void leerArch()
    {
        string[] temp;
        int cont = 0;
        string line; 
        System.IO.StreamReader arch=new System.IO.StreamReader(path);
        while ((line = arch.ReadLine()) != null)
        {
            if(cont == 0)
            {
                temp = line.Split(',');
                cantFilColnew[0] = System.Int16.Parse(temp[0]);
                cantFilColnew[1] = System.Int16.Parse(temp[1]);
            }
            cont++;
        }
        arch.Close();
    }

}
