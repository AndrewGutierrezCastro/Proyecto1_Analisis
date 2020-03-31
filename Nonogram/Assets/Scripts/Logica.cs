using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        int [][] matRes;
        if(transpuesta){
            matRes = matTranSINBOOL(pMatriz);
        }
        if(pintar){
           // pintarScrp.paintMatrix(matRes);
        }

        /*for(int f = 0; f < matRes.GetLength(0); f++)
        {
            for(int c = 0; c < matRes[0].GetLength(0); c++)
            {
                Console.Write(matRes[f][c]+" ");
            }
            Console.WriteLine()
        }*/
        

    }
    public void Inicial()
    {
        setPistasFilas(Analizador.single.matFil.ToArray());
        setPistasColumnas(Analizador.single.matCol.ToArray());
        setMatrizSize(Analizador.single.cantFilCol[0], Analizador.single.cantFilCol[1]);
        pintar = true;
        resolver();
    }
    private void generarOrdenDeSolucion(){
        /*
        Este metodo genera dos listas el cual es el orden a resolver
        del nonograma y si estan en las colunmas o filas*/
        int tope = columnas;
        if(filas >= columnas){
            tope = filas;
        }
        for (int i = tope; i >= 0; i--)
        {
            for (int j = 0; j < filas; j++)
            {   
                if(pistColMaList[j].Sum() == i){
                    ordenPistas.Add(j);
                    ordenPistasFC.Add(true);
                }
                if(pistFilMaList[j].Sum() == i){
                    ordenPistas.Add(j);
                    ordenPistasFC.Add(false);
                }
                
            }

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
            return true;
        }
        int rAfila = ordenPistas.ElementAt(indice); //Indice de la fila a resolver
        if(ordenPistasFC.ElementAt(indice) ^ transpuesta){ 
            // XOR de, ¿Se debe transponer? xor ¿Esta transpuesta?
            pMatriz = matTranspues(pMatriz);
        }
        //Tomar la fila correspondiente con el indice calculado anteriormente
        int[] Lfila = new int[pMatriz[rAfila].GetLength(0)];
        Array.Copy(pMatriz[rAfila], Lfila, pMatriz[rAfila].GetLength(0));
        //Hacer un respaldo con el estado actual para poder intentar mas combianciones o bien
        // devolverse sin cambios

        setArrayPistas(rAfila);//arreglo que contiene las pistas de dicha fila
        int[][] filasRespaldo = new int[arrayPista.GetLength(0)+1][];
        for (int i = 0; i < arrayPista.GetLength(0)+1; i++)
        {
            filasRespaldo[i] = new int[Lfila.GetLength(0)];
        }     
        Array.Copy(pMatriz[rAfila], filasRespaldo[0], pMatriz[rAfila].GetLength(0));
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
                Array.Copy(this.fila, pMatriz[rAfila], pMatriz[rAfila].GetLength(0));
                /*Console.WriteLine();
                Console.Write(indice+" ------------------------------ "+(ordenPistasFC.ElementAt(indice) ^ transpuesta));
                Console.WriteLine();*/
                imprimir(pMatriz);
                Analizador.single.matUpdate = pMatriz;
                for(int atrasador=0; atrasador<1000;atrasador++)
                {
                    print(atrasador);
                }
                /*Console.WriteLine();
                Console.Write(indice+" ------------------------------ "+ordenPistasFC.ElementAt(indice) +"-"+transpuesta);
                Console.WriteLine();*/

                

                if(resolAux(++indice, pMatriz)){
                    //llamarse nuevamente; incrementar el indice de la lista ordenPistas 
                    //para resolver la siguiente fila
                    return true;
                }else{
                    //Si no pudo pintar las pistas en la fila, borrar cambios e intentar 
                    //nuevamente 
                    //Aca seria donde se hace el "Backtracking" en teoria
                    if(pist > 0 ){
                        pist--;
                        Array.Copy(filasRespaldo[pist], Lfila, filasRespaldo[pist].GetLength(0));
                        final = lisVar[pist] + 1; 
                    }else{
                        return false;
                    }                 
                    indice--;//decrementar el indice de ordenPistas por el if anterior  
                    /*devolver el valor booleano de la transpuesta al de la actual fila
                        en las llamadas recursivas pudo cambiarse
                    */
                    transpuesta = ordenPistasFC.ElementAt(indice);
                    setArrayPistas(rAfila);
                    //Borrar cambios en la matriz local
                    Array.Copy(filasRespaldo[0], pMatriz[rAfila], filasRespaldo[0].GetLength(0)); 
                                        
                }
            }
            if(indice == 4 & pist == 0){

            }                 
            if (combinacionesRecursiva(pist, Lfila, final, rAfila, pMatriz))
            {
                lisVar[pist] = inicio;
                pist++;
                Array.Copy(this.fila, filasRespaldo[pist], this.fila.GetLength(0)); 
            }
            else{
                if (pist > 0){   
                    pist--;
                    Array.Copy(filasRespaldo[pist], Lfila, filasRespaldo[pist].GetLength(0));
                    final = lisVar[pist] + 1;
                }
                else
                {
                    return false;
                } 
            }

        }      
        return true;
    }
        //Si intento pintar en todos los inicios posibles y no se logro retornar falso
        //es imposible esta combinacion, hacer Backtracking  
    private Boolean revisarFC(int[] pFila, int inicio, int final, int numFila, int[][] pMatriz1){
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
        
        Boolean revisarFC_Aux( int indicePista, int[] CRfila, int indiceFila,int[][] pMatriz, List<int> pListPistas){
        int cantidadPtsSeguidos = pListPistas.ElementAt(indicePista);
        int corredorPosiciones = 0;
        Boolean noSepuede = false;
        
            while(cantidadPtsSeguidos+corredorPosiciones+indiceFila <= CRfila.GetLength(0)){
                
                noSepuede = false;
                int hastaFORpaint = cantidadPtsSeguidos+corredorPosiciones+indiceFila;
                int[] a = Array.FindAll(CRfila,(int b) =>{ return b == 1; } );
                if( a.GetLength(0) > pListPistas.Sum()){
                    break;
                }
                for(int i = (corredorPosiciones+indiceFila); i < hastaFORpaint; i++){  
                        
                        if(CRfila[i] == 2 ){
                            // fila = 0 es vacia, fila = 1 es pintada, fila = 2 es X
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

                if(!noSepuede){             
                    if(++indicePista < pListPistas.Count ){
                        //hay mas cuadros por revisar
                        
                        if(revisarFC_Aux( indicePista, CRfila,  indiceFila, pMatriz,  pListPistas)){
                            //si la combinacion de las pistas funciono
                            //entonces no incumple ningun vector vertical
                            return true;
                        }else{
                            //si no funciono la revision entonces hay que chequear otra,
                            //para ver si puede cumplir de alguna forma
                            indicePista--;
                            //se debe decrementar porque arriba se incremento para resolver la siguiente pista
                            //entones debo volver para revisar de otra forma
                        }
                    }else{
                    //no hay mas cuadros por revisar
                    //no hay mas pistas
                    //y la combinancion que se hizo no incumple nada
                    return true; 
                    }
                }
                //sino funciono la revision entonces
                //correr posicion y seguir con la siguiente posible combinacion             
                corredorPosiciones++;
                if(cantidadPtsSeguidos+corredorPosiciones+indiceFila > CRfila.GetLength(0)){
                    return false;
                }
            }
        return false;
        //de ninguna forma logro cumplir entonces retornar falso
    }
        
        //Respaldar la matriz donde no esta incluida la posible solucion
        //y al respaldo se le agrega la posible solucion para revisar
        int[][] matrizRespaldo = new int[matriz.GetLength(0)][];          
        for (int k = 0; k < matriz.GetLength(0); k++)
        {   
            matrizRespaldo[k] = new int[pMatriz1[k].GetLength(0)];
            Array.Copy(pMatriz1[k], matrizRespaldo[k], matrizRespaldo.GetLength(0));
        }
        matrizRespaldo[numFila] = pFila;
        matrizRespaldo = matTranSINBOOL(matrizRespaldo);
        List<int> copyPista;
        for(int i = inicio; i <= final; i++){
            if(transpuesta){
                copyPista = pistFilMaList[i];
            }else{
                copyPista = pistColMaList[i];
            }
            if( !(revisarFC_Aux(0, matrizRespaldo[i], 0, matrizRespaldo, copyPista) ) ) {
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
            int[] a;

            for(int i = (corredorPosiciones+indiceFila); i < hastaFORpaint; i++){
    
                    if(CRfila[i] == 0 || CRfila[i] == 1 ){
                        // fila = 0 es vacia, fila = 1 es pintada, fila = 2 es X
                        CRfila[i] = 1;
                        a = Array.FindAll(CRfila,(int b) =>{ return b == 1; } );
                        //Encontrar todo los 1 en la fila, si el mismo excede la
                        //cantidad maxima de la pista, no se puede pintar de 
                        //esta forma
                        if(a.GetLength(0) > arrayPista.Sum()){
                            noSepuede = true;
                            break;
                        }
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
            , hastaFORpaint-1, numFila, pMatriz)){ 
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
