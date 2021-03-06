﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Analizador : MonoBehaviour
{

    public static Analizador single { get; private set; }
    public int[] cantFilCol=new int[2]; //filas luego columnas
    public List<int[]> matFil = new List<int[]>();
    public List<int[]> matCol = new List<int[]>();
    public List<List<GameObject>> cuadricula = new List<List<GameObject>>();
    public int[][] matUpdate;
    public GameObject cuadroSpawn;
    public Camera cam;
    [SerializeField]
    public float tamannoCuadro;

    public void Awake()
    {
        if (single == null)
        {
            single = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        string path = BuscadorArchivos.singelton.direccion;
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
                matUpdate = new int[cantFilCol[0]][];
                if (cantFilCol[0] >= cantFilCol[1])
                    cam.orthographicSize = cantFilCol[0]+2;
                else
                    cam.orthographicSize = cantFilCol[1]+2;
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
                    matFil.Add(tempo.ToArray());
                else
                    matCol.Add(tempo.ToArray());
            }

            cont++;
        }
        arch.Close();

        generarGrid();
    }

    public void generarGrid()
    {

        for (int fila = 0; fila < cantFilCol[0]; fila++)
        {
            int[] temp = new int[cantFilCol[1]];
            List<GameObject> filCuad = new List<GameObject>();
            for (int columna = 0; columna < cantFilCol[1]; columna++)
            {
                GameObject cuadroNuevo=Instantiate(cuadroSpawn,transform.position,Quaternion.identity);

                float posX = (columna *tamannoCuadro)- ((cantFilCol[0] * tamannoCuadro) / 2);
                float posY = (fila * -tamannoCuadro) + ((cantFilCol[1] * tamannoCuadro) / 2);

                cuadroNuevo.transform.position = new Vector2(posX, posY);
                if (fila == 0)
                {
                    GameObject pistAct = new GameObject();
                    pistAct.AddComponent<TextMeshPro>();
                    pistAct.GetComponent<TextMeshPro>().renderer.sortingOrder=20;
                    pistAct.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,1);
                    pistAct.GetComponent<TextMeshPro>().alignment = TMPro.TextAlignmentOptions.Bottom;
                    pistAct.GetComponent<TextMeshPro>().fontSize = 10;
                    string tx = "";
                    foreach (int t in matCol[columna])
                    {
                        tx = tx + t.ToString() + " ";
                    }
                    pistAct.GetComponent<TextMeshPro>().text=tx;
                    pistAct.GetComponent<TextMeshPro>().transform.position = new Vector2(posX, posY + 3);
                }
                if (columna == 0)
                {
                    GameObject pistAct = new GameObject();
                    pistAct.AddComponent<TextMeshPro>();
                    pistAct.GetComponent<TextMeshPro>().renderer.sortingOrder = 20;
                    pistAct.GetComponent<TextMeshPro>().alignment = TMPro.TextAlignmentOptions.Midline;
                    pistAct.GetComponent<TextMeshPro>().fontSize = 10;
                    string tx = "";
                    foreach (int t in matFil[fila])
                    {
                        tx = tx + t.ToString() + " ";
                    }
                    pistAct.GetComponent<TextMeshPro>().text=tx;
                    pistAct.GetComponent<TextMeshPro>().transform.position = new Vector2(posX - 3, posY);
                }
                filCuad.Add(cuadroNuevo);
                temp[columna] = 0;
            }
            cuadricula.Add(filCuad);
            matUpdate[fila] = temp;
        }
    }

}
