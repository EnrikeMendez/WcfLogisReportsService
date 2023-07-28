using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Funciones genericas propias de este sistema
/// </summary>
public class funciones_genericas_reportes
{

    public Array CatalogoPrioridadDinamica()
    {
        //Al agregar una regla, se debera incrementar la dimencion de filas del arreglo:
        string[,] ArrayPrioridadDinamica = new string[1, 1];

        //regla de prioridad para pedimentos Instantáneos (173):
        ArrayPrioridadDinamica[0, 0] = "173"; //id_rep
        ArrayPrioridadDinamica[1, 0] = "5"; //Prioridad

        //regla de prioridad para regla pedimentos Expediente Aduanal Antolin (311):
        ArrayPrioridadDinamica[0, 1] = "311"; //id_rep
        ArrayPrioridadDinamica[1, 1] = "6"; //Prioridad


        return ArrayPrioridadDinamica;
    }

    public List<string> perfil1()
    {
        List<string> per1 = new List<string>() {
            "ALEJANDROLE",
            "JAVIERD",
            "ALMALFS"};
        return per1;
    }

    public List<string> perfil2()
    {
        List<string> per2 = new List<string>() {
            "YAZMINCC",
            "EVELINGB",
            "ELIZABETHBM",
            "LUISFR",
            "DULCELO",
            "ALOURDESC",
            "ALEXANDRAMM",
            "LGABRIELAM",
            "MLOURDESB"
        };
        return per2;
    }


}