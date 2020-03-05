using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Analizador : MonoBehaviour
{

    public string path;
    public int[] cantFilColnew=new int[2];
    public List<List<int>> matFil = new List<List<int>>();
    public List<List<int>> matCol = new List<List<int>>();

    public void leerArch()
    {
        path = BuscadorArchivos.singelton.direccion;
        List<int> tempo;
        string[] temp;
        int cont = 0;
        string line;
        bool filCol=true;
        System.IO.StreamReader arch=new System.IO.StreamReader(path);
        while ((line = arch.ReadLine()) != null)
        {
            tempo= new List<int>();
            if (cont == 0)
            {
                temp = line.Split(',');
                cantFilColnew[0] = System.Int16.Parse(temp[0]);
                cantFilColnew[1] = System.Int16.Parse(temp[1]);
            }
            if (line == "FILAS")
                filCol = true;
            else if (line == "COLUMNAS")
                filCol = false;
            if (cont !=0 && line!="FILAS" && line!="COLUMNAS")
            {
                temp = line.Split(',');
                foreach (string i in temp)
                    tempo.Add(System.Int16.Parse(i));
                if (filCol)
                    matFil.Add(tempo);
                else
                    matCol.Add(tempo);
            }

            cont++;
        }
        arch.Close();

        print("Columnas");
        foreach (List<int> i in matCol)//De aqui para abajo para visualizar las matrices con las pistas
        {
            print("[");
            foreach (int j in i)
            {
                print(j);
            }
            print("]");
        }
        print("Filas");
        foreach (List<int> i in matFil)
        {
            print("[");
            foreach (int j in i)
            {
                print(j);
            }
            print("]");
        }
    }

}
