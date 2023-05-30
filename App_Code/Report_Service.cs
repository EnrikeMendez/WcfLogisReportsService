using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Newtonsoft.Json;

// NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
[AspNetCompatibilityRequirements(RequirementsMode
    = AspNetCompatibilityRequirementsMode.Allowed)]
public class Report_Service : IReport_Service
{
    consultas_procesos obj_consultas_procesos = new consultas_procesos();
    funciones_genericas obj_func_genericas = new funciones_genericas();
    DataTable obj_dt = new DataTable();



    [WebInvoke(Method = "GET",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    ResponseFormat = WebMessageFormat.Json)]
    public string GetData(int number1, int number2)
    {
        System.Diagnostics.Debug.Print(string.Format("*** El resultado es: {0} ***", number1 + number2));

        return string.Format("{0}", number1 + number2);
    }

    public CompositeType GetDataUsingDataContract(CompositeType composite)
    {
        if (composite == null)
        {
            throw new ArgumentNullException("composite");
        }
        if (composite.BoolValue)
        {
            composite.StringValue += "Suffix";
        }
        return composite;
    }


    [WebInvoke(Method = "GET",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    ResponseFormat = WebMessageFormat.Json)]
    public string GetConsultaErrores()
    {
        obj_dt = obj_consultas_procesos.ftn_consulta_errores_reportes();
        if (obj_dt != null)
        {
            //if (obj_func_genericas.ftn_retorna_serializable(obj_dt) != "")
            //{
                return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
            //}
            //else
            //{
            //    return "Sin Información";
            //}
        }
        else
        {
            return "Sin Información";
        }
    }

    public string GetMonitoreoRep()
    {
        throw new NotImplementedException();
    }
}