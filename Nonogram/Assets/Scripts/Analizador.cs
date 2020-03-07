using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Analizador : MonoBehaviour
{

    public string path;
    public int[] cantFilCol=new int[2]; //filas luego columnas
    public List<List<int>> matFil = new List<List<int>>();
    public List<List<int>> matCol = new List<List<int>>();
    public GameObject cuadroSpawn;
    public float tamannoCuadro;

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
                cantFilCol[0] = System.Int16.Parse(temp[0]);
                cantFilCol[1] = System.Int16.Parse(temp[1]);
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

        generarGrid();

        //print("Columnas");
        //foreach (List<int> i in matCol)//De aqui para abajo para visualizar las matrices con las pistas
        //{
        //    print("[");
        //    foreach (int j in i)
        //    {
        //        print(j);
        //    }
        //    print("]");
        //}
        //print("Filas");
        //foreach (List<int> i in matFil)
        //{
        //    print("[");
        //    foreach (int j in i)
        //    {
        //        print(j);
        //    }
        //    print("]");
        //}
    }

    public void generarGrid()
    {
        tamannoCuadro = 1;
        print("cantidades");
        print(cantFilCol[0]);
        print(cantFilCol[1]);
        print(tamannoCuadro);
        for (int fila = 0; fila < cantFilCol[0]; fila++)
        {
            for (int columna = 0; columna < cantFilCol[1]; columna++)
            {
                GameObject cuadroNuevo=Instantiate(cuadroSpawn,transform.position,Quaternion.identity);

                float posX = columna * tamannoCuadro;
                float posY = fila * (-tamannoCuadro);
                print("col");
                print(columna);
                print("tamaño");
                print(tamannoCuadro);


                cuadroNuevo.transform.position = new Vector2(posX, posY);
            }
        }
    }

}
