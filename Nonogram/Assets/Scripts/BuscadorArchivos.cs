using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BuscadorArchivos : MonoBehaviour
{
    public static BuscadorArchivos singelton { get; private set; }
    public string dirExp;
    public TMP_InputField txDir;
    public Button boton;
    public string direccion;


    public void Awake()
    {
        if (singelton == null)
        {
            singelton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (txDir.text != "")
            boton.interactable = true;
        else
            boton.interactable = false;
    }

    public void AbrirExplorador()
    {
        dirExp = EditorUtility.OpenFilePanel("Nonogram a Resolver","","txt");
        txDir.text = dirExp;
    }

    public void Confirmar()
    {
        direccion = txDir.text;
        SceneManager.LoadScene("Matriz");
    }

}
