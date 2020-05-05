//Autor: Andrew J Gutierrez Castro

using System;
using System.Collections.Generic;
using System.Linq;

namespace Revisadores{
class Revisador{
    private int indicePista;
    private int[] CRfila;
    private int indiceFila;
    private List<int> pListPistas;
    private int numFila;
    public int faltaPunto;
    public Boolean errorColumna = false;
    public Revisador(int pIndicePista, int[] pCRfila, int pIndiceFila, List<int> ppListPistas, int pNumFila){
        indicePista = pIndicePista;
        CRfila = pCRfila;
        indiceFila = pIndiceFila;
        pListPistas = ppListPistas;
        numFila = pNumFila;
    }
    public void revisarColumnas(){
        for(int m=0; m<pListPistas.Count; m++){
            int cantidadPtsSeguidos = pListPistas.ElementAt(indicePista);
            Boolean noSepuede, entroFor = false, encontrado = false;
            int hastaFORpaint;
            for(int i = indiceFila; i<=numFila; i++){ 
                entroFor = true;
                if(CRfila[i] == 1){
                    indiceFila = i;
                    encontrado = true;
                    break;
                }
            }
            List<int> newList = pListPistas.GetRange(indicePista,pListPistas.Count-indicePista);
            if(newList.Sum() + (newList.Count-1)+indiceFila > CRfila.GetLength(0)){
                errorColumna=true;
                return;
            }else if(indiceFila == 0 & entroFor & numFila == 0){
                return;
            }else if(!encontrado){
                    //Si para poder poder poner todos los puntos se sale del largo del arreglo esta malo
                    return;
                }
            
            noSepuede = false;
            hastaFORpaint = cantidadPtsSeguidos+indiceFila; 
            for(int i = indiceFila; i < hastaFORpaint; i++){ 
                    if(i > numFila){
                        faltaPunto = 1;
                        return;
                    }   
                    if(CRfila[i] != 1){
                        noSepuede = true;
                        break;                         
                    }
                                          
            }
            if(noSepuede){
                errorColumna = true;
                return;
            }else{
                if(hastaFORpaint < CRfila.GetLength(0)){
                    if(CRfila[hastaFORpaint] == 1){
                        noSepuede = true;
                    }
                }
            }
            if(!noSepuede ){            
                if(++indicePista < pListPistas.Count ){
                    //hay mas cuadros por revisar
                    if(!(hastaFORpaint+1 > numFila)){
                        indiceFila = hastaFORpaint+1;
                        continue;
                    }
                    return; 
                }else{
                    if(CRfila.Sum() > pListPistas.Sum()){
                        errorColumna = true;

                    }
                    return;
                }
            }           
            errorColumna = true;    
            return;
            //de ninguna forma logro cumplir entonces retornar falso
            }
    }
 
    public void Threads(){
            int inicio=0, final=0;
            int threadListos = 0;
            System.Threading.Thread[] arrThreads=null;
            Revisadores.Revisador[] revisadores=null;
            for (int i = inicio; i <= final; i++)
            {
                arrThreads[i].Start();
            }
            while (threadListos < arrThreads.GetLength(0)){

                for (int i = inicio; i <= final; i++)
                {   
                    if(arrThreads[i].ThreadState == System.Threading.ThreadState.Stopped){
                        if(revisadores[i].errorColumna){
                            //return false;
                            return;
                        }else{
                            threadListos++;
                        }
                    }

                }

            }
    }
}
} 