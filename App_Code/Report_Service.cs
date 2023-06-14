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
	Boolean ejecuto_querye;

    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

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
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else
        {
            return "Sin Información";
        }
    }
    [WebInvoke(Method = "GET",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    ResponseFormat = WebMessageFormat.Json)]
    public string GetMonitoreoRep()
    {
        obj_dt = obj_consultas_procesos.ftn_consulta_monitoreo_reportes();
        if (obj_dt != null)
        {
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else
        {
            return "Sin Información";
        }
    }

    
    [WebInvoke(Method = "GET",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    ResponseFormat = WebMessageFormat.Json)]
    public string GetSql(String sql)
    {
        obj_dt = obj_consultas_procesos.ftn_consulta_gen(sql);
        if (obj_dt != null)
        {
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else    
        {
            //return "Sin Información";
            return string.Format("{0}", " Sin Informacion");
        }
    }
	
	
	[WebInvoke(Method = "GET",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    ResponseFormat = WebMessageFormat.Json)]
    public string GetConsultaCambioPrioridad()
    {
        obj_dt = obj_consultas_procesos.ftn_consulta_cambio_prioridad();
        if (obj_dt != null)
        {
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else
        {
            return "Sin Información";
        }
    }


    [WebInvoke(Method = "GET",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    ResponseFormat = WebMessageFormat.Json)]
    public string GetModificaCambioPrioridad(string id_crons, string prioridad)
    {
        ejecuto_querye  = obj_consultas_procesos.ftn_modifica_cambio_prioridad(id_crons, prioridad);
        if (ejecuto_querye == true)
        {
            return "Los procesos: " + id_crons  + " se cambiaron a prioridad: " + prioridad;
        }
        else
        {
            return "Sin Información";
        }
    }
	
	[WebInvoke(Method = "GET",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    ResponseFormat = WebMessageFormat.Json)]
    public string GetModificaCambioPrioridadDinamica()
    {
        ejecuto_querye = obj_consultas_procesos.ftn_modifica_cambio_prioridad_dinamica();
        if (ejecuto_querye == true)
        {
            //return "Los procesos: " + id_crons + " se cambiaron a prioridad: " + prioridad;
            return "";
        }
        else
        {
            return "Sin Información";
        }
    }

   
    [WebInvoke(Method ="GET",
        BodyStyle =WebMessageBodyStyle.Wrapped,
        ResponseFormat =WebMessageFormat.Json)]
    public string GetMail(string Id_Cron, string NumCli, string id_mail, string nombre, string correo, string Tercero,string status)
    {
        //
      
        return "";
    }

    [WebInvoke(Method = "GET",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    ResponseFormat = WebMessageFormat.Json)]
    public string GetConsultaReportes(string usuario, string status)
    {
        obj_dt = obj_consultas_procesos.ftn_lista_reporte(usuario, status);
        if (obj_dt != null)
        {
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else
        {
            return "Sin Información";
        }
    }

    [WebInvoke(Method = "GET",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    ResponseFormat = WebMessageFormat.Json)]
    public string GetStatusReporte(string idreporte, string accion)
    {
        String res = obj_consultas_procesos.ftn_act_desa_rep_chron(idreporte, accion);
        if (obj_dt != null)
        {
            return res;
        }
        else
        {
            return "Sin Información";
        }
    }

    [WebInvoke(Method = "GET",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    ResponseFormat = WebMessageFormat.Json)]
    public string GetListaCorreos(string lista)
    {
        obj_dt = obj_consultas_procesos.ftn_lista_contactos(lista);
        if (obj_dt != null)
        {
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else
        {
            return "Sin Información";
        }
    }

    [WebInvoke(Method = "GET",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    ResponseFormat = WebMessageFormat.Json)]
    public string GetArmarContactos(string mail_id)
    {
        obj_dt = obj_consultas_procesos.ftn_armar_contactos(mail_id);
        if (obj_dt != null)
        {
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else
        {
            return "Sin Información";
        }
    }
}