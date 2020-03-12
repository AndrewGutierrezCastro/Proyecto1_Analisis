using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Analizador : MonoBehaviour
{

    public static Analizador single { get; private set; }
    public int[] cantFilCol=new int[2]; //filas luego columnas
    public List<List<int>> matFil = new List<List<int>>();
    public List<List<int>> matCol = new List<List<int>>();
    public List<List<GameObject>> cuadricula = new List<List<GameObject>>();
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
    }

    public void generarGrid()
    {

        for (int fila = 0; fila < cantFilCol[0]; fila++)
        {
            List<GameObject> filCuad = new List<GameObject>();
            for (int columna = 0; columna < cantFilCol[1]; columna++)
            {
               GameObject cuadroNuevo=Instantiate(cuadroSpawn,transform.position,Quaternion.identity);

               float posX = (columna *tamannoCuadro)- ((cantFilCol[0] * tamannoCuadro) / 2);
               float posY = (fila * -tamannoCuadro) + ((cantFilCol[1] * tamannoCuadro) / 2);

               cuadroNuevo.transform.position = new Vector2(posX, posY);
               filCuad.Add(cuadroNuevo);
            }
            cuadricula.Add(filCuad);
        }
        print(cuadricula.Count);
    }

}
