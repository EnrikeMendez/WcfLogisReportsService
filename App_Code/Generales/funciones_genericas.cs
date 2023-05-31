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


}