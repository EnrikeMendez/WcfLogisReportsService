using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Newtonsoft.Json;

/// <summary>
/// Descripción breve de funciones_genericas
/// </summary>
public class funciones_genericas
{
    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
    


    public string ftn_retorna_serializable(DataTable obj_dt)
    {

        Dictionary<string, object> row;
        foreach (DataRow dr in obj_dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn col in obj_dt.Columns)
            {
                row.Add(col.ColumnName, dr[col]);
            }
            rows.Add(row);
        }


        return serializer.Serialize(rows);
    }


    public int ftn_Weekday(DateTime dt, DayOfWeek startOfWeek)
    {
        return (dt.DayOfWeek - startOfWeek + 7) % 7;
    }


    //Funcion que debuelve un catalogo de reglas para la prioridad de los reportes.	
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