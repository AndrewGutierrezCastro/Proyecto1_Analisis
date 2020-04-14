using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Logica : MonoBehaviour
{
    private GameObject EmptyPintar;
    private Component pintarScrp;
    public Boolean pintar = false;
    private int[] fila;
    private int[] arrayPista;
    public int filas = 10;
    public int columnas = 10;
    private int inicio, final = 0;
    private List<int> ordenPistas = new List<int>();
    private List<Boolean> ordenPistasFC = new List<Boolean>();
    private List<int>[] pistFilMaList;
    private List<int>[] pistColMaList;

    private Boolean transpuesta;
    public int[][] matriz;
    
    // Start is called before the first frame update
    void Start()
    {   
        //EmptyPintar = GameObject.Find("EmptyPinta");
        //pintarScrp = GetComponent<Pintar>();

    }
    private void imprimir(int[][] pMatriz){
        if(pintar){
            int[][] matRes;
            if (transpuesta)
            {
                matRes = matTranSINBOOL(pMatriz);
            }
            else
            {
                matRes = pMatriz;
            }
            Analizador.single.matUpdate = matRes;
        }
        
    }
    public void Inicial()
    {
        setPistasFilas(Analizador.single.matFil.ToArray());
        setPistasColumnas(Analizador.single.matCol.ToArray());
        setMatrizSize(Analizador.single.cantFilCol[0], Analizador.single.cantFilCol[1]);
        pintar = true;
        Thread hilo=new Thread(resolver);
        hilo.Start();


    }
    private void generarOrdenDeSolucion(){
        /*
        Este metodo genera dos listas el cual es el orden a resolver
        del nonograma y si estan en las colunmas o filas*/
        int tope = filas;
            for (int j = 0; j < filas; j++)
            {   
                ordenPistas.Add(j);
                ordenPistasFC.Add(false); 
            }



            


        
    }

    public void resolver(){
        transpuesta = false;
        matriz = new int[filas][];
        for (int i = 0; i < filas; i++)
        {
            matriz[i] = new int[columnas];
        }
        generarOrdenDeSolucion();

        if(resolAux(0, matriz)){
            imprimir(matriz);

        }else{
            Console.WriteLine("NelxD");
        }
    }
    private Boolean resolAux(int indice, int[][] pMatriz){
        matriz = pMatriz;
        if(indice >= ordenPistas.Count){
            /*
            1.Condicion de parada, la lista de orden se ha completado por ende se ha pintado 
                todo correctamente, Nonogram resuelto
            */
            if(transpuesta){
                //Si la matriz esta transpuesta, se debe transponer nuevamente para dar la respuesta
                //Si no, ya esta lista
                pMatriz = matTranspues(pMatriz);
            }
            matriz = pMatriz;
            pintar = true;
            imprimir(matriz);
            return true;
        }
        int indexFila = ordenPistas.ElementAt(indice); //Indice de la fila a resolver
        if(ordenPistasFC.ElementAt(indice) ^ transpuesta){ 
            // XOR de, ¿Se debe transponer? xor ¿Esta transpuesta?
            pMatriz = matTranspues(pMatriz);
        }
        //Tomar la fila correspondiente con el indice calculado anteriormente
        int[] Lfila = new int[pMatriz[indexFila].GetLength(0)];
        Array.Copy(pMatriz[indexFila], Lfila, pMatriz[indexFila].GetLength(0));
        //Hacer un respaldo con el estado actual para poder intentar mas combianciones o bien
        // devolverse sin cambios

        setArrayPistas(indexFila);//arreglo que contiene las pistas de dicha fila

        int[][] filasRespaldo = new int[arrayPista.GetLength(0)+1][];
        for (int i = 0; i < arrayPista.GetLength(0)+1; i++)
        {
            filasRespaldo[i] = new int[Lfila.GetLength(0)];
        }     
        Array.Copy(pMatriz[indexFila], filasRespaldo[0], pMatriz[indexFila].GetLength(0));
        /*  2 pistas a pintar por tanto 2 respaldos
            [0,0,0,0,0],
            [1,2,0,0,0]
        */
        inicio = 0;
        final = 0;
        int pist = 0;
        int[] lisVar = new int[arrayPista.GetLength(0)];

        while(pist <= arrayPista.GetLength(0))
        {   
            if(pist == arrayPista.GetLength(0)){
                Array.Copy(this.fila, pMatriz[indexFila], pMatriz[indexFila].GetLength(0));
                imprimir(pMatriz);               

                if(resolAux(++indice, pMatriz)){
                    //llamarse nuevamente; incrementar el indice de la lista ordenPistas 
                    //para resolver la siguiente fila
                    return true;
                }else{
                    //Si no pudo pintar las pistas en la fila, borrar cambios e intentar 
                    //nuevamente 
                    //Aca seria donde se hace el "Backtracking" en teoria
                    indice--;//decrementar el indice de ordenPistas por el if anterior  
                    /*devolver el valor booleano de la transpuesta al de la actual fila
                        en las llamadas recursivas pudo cambiarse
                    */
                    transpuesta = ordenPistasFC.ElementAt(indice);
                    setArrayPistas(indexFila);
                    if(pist > 0 ){
                        pist--;
                        Array.Copy(filasRespaldo[pist], Lfila, filasRespaldo[pist].GetLength(0));
                        final = lisVar[pist] + 1; 
                    }else{
                        return false;
                    }                 
                    
                    //Borrar cambios en la matriz local
                    Array.Copy(filasRespaldo[0], pMatriz[indexFila], filasRespaldo[0].GetLength(0)); 
                                        
                }
            }               
            
            if (combinacionesRecursiva(pist, Lfila, final, indexFila, pMatriz))
            {
                lisVar[pist] = inicio;
                pist++;
                Array.Copy(this.fila, filasRespaldo[pist], this.fila.GetLength(0)); 
            }else{
                if (pist > 0){   
                    pist--;
                    Array.Copy(filasRespaldo[pist], Lfila, filasRespaldo[pist].GetLength(0));
                    final = lisVar[pist] + 1;
                    if(final+arrayPista[pist] >= Lfila.GetLength(0)){
                        if (pist > 0){   
                            pist--;
                            Array.Copy(filasRespaldo[pist], Lfila, filasRespaldo[pist].GetLength(0));
                            final = lisVar[pist] + 1;
                        }else{
                            return false;
                        }
                            
                    }
                }else{
                    return false;
                } 
            }

        }      
        return true;
    }
        //Si intento pintar en todos los inicios posibles y no se logro retornar falso
        //es imposible esta combinacion, hacer Backtracking  
    private Boolean revisarFC(int[] pFila, int inicio, int final, int numFila, int[][] pMatriz, Boolean esUltPist){
        /*Revisar filas columnas
        Se recibe la fila con la posible combinacion correcta, de donde se comenzo a pintar hasta
        en donde se termino. El numero de la fila para poder agregarla a la matriz de respaldo
        Y la matriz sin la fila agregada pues aun no se ha confirmado que cumpla.
        */
        
        /*Metodo auxiliar
            Este metodo es igual al de pintar, lo que hace es intentar de pintar las columnas que se 
            ven afectadas por la posible combinacion de la fila, de forma que cumplan sus propias pistas
            Si esto es posible entonces la combinacion de la fila no imposibilita el pintado de las columnas
        */
        
        int[][] matrizRespaldo = new int[matriz.GetLength(0)][];          
            for (int k = 0; k < matriz.GetLength(0); k++)
            {   
                matrizRespaldo[k] = new int[pMatriz[k].GetLength(0)];
                Array.Copy(pMatriz[k], matrizRespaldo[k], matrizRespaldo.GetLength(0));
            }
            matrizRespaldo[numFila] = pFila;
            matrizRespaldo = matTranSINBOOL(matrizRespaldo);
            List<int> copyPista;
            Revisadores.Revisador[] revisadores = new Revisadores.Revisador[filas];
            if(esUltPist){
                inicio = 0;
                final = columnas-1;
            }
            for(int i = inicio; i <= final; i++){
                copyPista = pistColMaList[i];
                revisadores[i] = new Revisadores.Revisador(0, matrizRespaldo[i], 0, copyPista, numFila);
                revisadores[i].revisarColumnas();
                if(revisadores[i].errorColumna){
                    return false;
                }                   
            }

            return true;

    }
    
    private Boolean combinacionesRecursiva( int indicePista, int[] CRfila, int indiceFila, int numFila,
        int[][] pMatriz){
        /*
        Este metodo pinta la pista que esta en el array de pistas global, se accede a el por 
        medio del indice dePista, si hay mas pistas se llama asi mismo hasta terminar de pintar
        todas las pistas en la fila. 

        La forma de pintar es la siguiente, primero tomar la cantidad de puntos seguidos que se
        deben pintar, se debe empezar en donde dice el indiceFila, y se tiene un corredor de posiciones
        que se incrementa si y solo si no se pudo pintar 
        EJ: 
            ptsSeguidos = 2, indiceFila = 0, corredorPosiciones = 0
            fila = 20100
            intenta pintar: pero se encuentra con el dos por ende pone a noSepuede en verdadero
            se incrementa el corredorPosiciones
            ptsSeguidos = 2, indiceFila = 0, corredorPosiciones = 1
            fila = 20100
            ->21100 pinta el primero
            ->21100 se repinta el 2do
        
        Luego debe revisar si antes y despues de los puntos pintados 
        */

        int cantidadPtsSeguidos = arrayPista[indicePista];
        if(indiceFila >= CRfila.GetLength(0)){
            return false;
        }
        int corredorPosiciones = 0;
        int[] filaRespaldo = new int[CRfila.GetLength(0)];
        Array.Copy(CRfila, filaRespaldo, CRfila.GetLength(0));
        Boolean noSepuede = false;
        int indiceFilaRespaldo;

        
        while(cantidadPtsSeguidos+corredorPosiciones+indiceFila <= CRfila.GetLength(0)){
    
            noSepuede = false;
            int hastaFORpaint = cantidadPtsSeguidos+corredorPosiciones+indiceFila;
            if(hastaFORpaint > CRfila.GetLength(0)){
                break;
            }
            indiceFilaRespaldo = indiceFila;
            for(int i = (corredorPosiciones+indiceFila); i < hastaFORpaint; i++){
    
                    if(CRfila[i] == 0 || CRfila[i] == 1 ){
                        // fila = 0 es vacia, fila = 1 es pintada, fila = 2 es X
                        CRfila[i] = 1;                       
                    }else{
                        noSepuede = true;
                        break;
                    }
            }
            
            int index = (indiceFila + corredorPosiciones)-1;
            if( index >= 0){
                if(CRfila[index] == 1){
                    noSepuede = true;
                }                  
            }
            if(hastaFORpaint < CRfila.GetLength(0)){
                if(CRfila[hastaFORpaint] == 1){
                    noSepuede = true;
                }
            }

            if(!noSepuede && revisarFC(CRfila, (indiceFila+corredorPosiciones)
            , hastaFORpaint-1, numFila, pMatriz, indicePista>= arrayPista.GetLength(0)-1)){ 
                // enviar la fila, de donde empece a pintar
                // revisar si la combinacion funciono
                //incrementar el indice de la pista para pasar
                //a la siguiente cantidad de cuadros a pintar
                

                if(hastaFORpaint < CRfila.GetLength(0)){
                    CRfila[hastaFORpaint] = 2; // poner una X para separar e incrementar (orden importa)             
                //indiceFila++;
                
                }
                final = hastaFORpaint + 1;
                inicio = indiceFila + corredorPosiciones;
                if(index >= 0){
                        CRfila[index] = 2; // la x de antes
                    }                    
                setFila(CRfila);
                return true; 

            }else{
                //no se pudo o se pinto y no funciono
                //Devolver el respaldo de la fila                   
                Array.Copy(filaRespaldo, CRfila, filaRespaldo.GetLength(0));

            }
            //sino funciono la combinacion entonces
            // correr posicion y seguir con la siguiente posible combinacion
            
            corredorPosiciones++;
            if(cantidadPtsSeguidos+corredorPosiciones > CRfila.GetLength(0)){
                return false;
            }

        }
        return false;
        //Salio del while, no pudo pintar de ninguna forma, retornar falso
        

    }
    private void setFila(int[] pFila){
        fila = new int[matriz.GetLength(0)];
        Array.Copy( pFila, fila, matriz.GetLength(0));

    }
    private void setArrayPistas(int x){
        /*Este metodo copia el contenido de la lista de pistas en el indice dado al 
        arreglo de pistas global, usa el de filas o columnas segun la orientacion 
        de la matriz, si esta transpuesta entonces esta resolviendo las columnas
        Sino entonces esta resolviendo las filas.
        */
        if(transpuesta){
            arrayPista = new int[pistColMaList[x].Count];
            pistColMaList[x].CopyTo(arrayPista);

        }else{
            arrayPista = new int[pistFilMaList[x].Count];
            pistFilMaList[x].CopyTo(arrayPista);
        }
        

    }
    private void matTranspuesta(int[][] pMatriz){
        //este metodo transpone la matriz recibida, SI afecta a la variable transpuesta
        //no retorna nada
        transpuesta = !transpuesta;
        int[][] nuevaMatriz = new int[columnas][];
        for (int i = 0; i < pMatriz.GetLength(0); i++)
        {
            nuevaMatriz[i] = new int[pMatriz.GetLength(0)];
        }

        for (int i = 0; i < pMatriz.GetLength(0); i++)
        {
            for (int j = 0; j < pMatriz[0].GetLength(0); j++)
            {
                nuevaMatriz[j][i] = matriz[i][j];
            }
        }
        int filasOld = filas;
        filas = columnas;
        columnas = filasOld;
        matriz = nuevaMatriz;
        
    }
    private int[][] matTranSINBOOL(int[][] pMatriz){
        //este metodo transpone la matriz recibida, no afecta a la variable transpuesta
        //y retorna la matriz transpuesta
        int[][] nuevaMatriz = new int[pMatriz[0].GetLength(0)][];
        for (int i = 0; i < pMatriz.GetLength(0); i++)
        {
            nuevaMatriz[i] = new int[pMatriz.GetLength(0)];
        }

        for (int i = 0; i < pMatriz.GetLength(0); i++)
        {
            for (int j = 0; j < pMatriz[0].GetLength(0); j++)
            {
                nuevaMatriz[j][i] = pMatriz[i][j];
            }
        }
        return nuevaMatriz;
        
    }
    private int[][] matTranspues(int[][] pMatriz){
        //este metodo transpone la matriz recibida, SI afecta a la variable transpuesta
        //y retorna la matriz transpuesta
        transpuesta = !transpuesta;
        int[][] nuevaMatriz = new int[pMatriz[0].GetLength(0)][];
        for (int i = 0; i < pMatriz.GetLength(0); i++)
        {
            nuevaMatriz[i] = new int[pMatriz.GetLength(0)];
        }

        for (int i = 0; i < pMatriz.GetLength(0); i++)
        {
            for (int j = 0; j < pMatriz[0].GetLength(0); j++)
            {
                nuevaMatriz[j][i] = pMatriz[i][j];
            }
        }
        return nuevaMatriz;
        
    }
    public void setPistasFilas(List<int>[] pPistFilMaList){
        pistFilMaList = pPistFilMaList;
    }
    public void setPistasColumnas(List<int>[] pPistColMaList){
        pistColMaList = pPistColMaList;
    }
    public void setPintar(Boolean pPintar){
        pintar = pPintar;
    }
    public void setMatrizSize(int pFila, int pColumna){
        filas = pFila;
        columnas = pColumna;
    }

}
