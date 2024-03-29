﻿using System;
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
    string msg = string.Empty;
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
            return string.Format("{0}", "Sin Información");
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
            return string.Format("{0}", "Sin Información");
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
            // return string.Format("{0}", "Sin Información");
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
            return string.Format("{0}", "Sin Información");
        }
    }


    [WebInvoke(Method = "GET",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    ResponseFormat = WebMessageFormat.Json)]
    public string GetModificaCambioPrioridad(string id_crons, string prioridad)
    {
        ejecuto_querye = obj_consultas_procesos.ftn_modifica_cambio_prioridad(id_crons, prioridad);
        if (ejecuto_querye == true)
        {
            return "Los procesos: " + id_crons + " se cambiaron a prioridad: " + prioridad;
        }
        else
        {
            return string.Format("{0}", "Sin Información");
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
            return string.Format("{0}", "Sin Información");
        }
    }


    [WebInvoke(Method = "GET",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
    public string GetMail(string Id_Cron, string NumCli, string id_mail, string nombre, string correo, string Tercero, string status, string hdnURI)
    {
        msg = obj_consultas_procesos.ftn_Mail(Id_Cron, NumCli, id_mail, nombre, correo, Tercero, status, hdnURI);
        if (msg != "")
        {
            return string.Format("{0}", msg);
        }
        else
        {
            return string.Format("{0}", "Sin Información");
        }
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
            return string.Format("{0}", "Sin Información");
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
            return string.Format("{0}", "Sin Información");
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
            return string.Format("{0}", "Sin Información");
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
            return string.Format("{0}", "Sin Información");
        }
    }

    [WebInvoke(Method = "GET",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    ResponseFormat = WebMessageFormat.Json)]
    public string GetArmarCorreos(string mail_list, string id_client)
    {
        obj_dt = obj_consultas_procesos.ftn_modicacion_lista_correos(mail_list, id_client);
        if (obj_dt != null)
        {
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else
        {
            return string.Format("{0}", "Sin Información");
        }
    }

    [WebInvoke(Method = "GET",
   BodyStyle = WebMessageBodyStyle.Wrapped,
   ResponseFormat = WebMessageFormat.Json)]
    public string GetValidaCorreos(string mail_list)
    {
        obj_dt = obj_consultas_procesos.ftn_Valida_Correos(mail_list);
        if (obj_dt != null)
        {
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else
        {
            return string.Format("{0}", "Sin Información");
        }
    }

    [WebInvoke(Method = "GET",
   BodyStyle = WebMessageBodyStyle.Wrapped,
   ResponseFormat = WebMessageFormat.Json)]
    public string GetValidaCorreosOriginales(string mail_list)
    {
        obj_dt = obj_consultas_procesos.ftn_Valida_Correos_Originales(mail_list);
        if (obj_dt != null)
        {
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else
        {
            return string.Format("{0}", "Sin Información");
        }
    }

    [WebInvoke(Method = "GET",
      BodyStyle = WebMessageBodyStyle.Wrapped,
      ResponseFormat = WebMessageFormat.Json)]
    public string GetEliminaOriginales(string mail_list)
    {
        string res = obj_consultas_procesos.ftn_Elimina_Originales(mail_list);
        if (obj_dt != null)
        {
            return res;
        }
        else
        {
            return string.Format("{0}", "Sin Información");
        }
    }

    [WebInvoke(Method = "GET",
     BodyStyle = WebMessageBodyStyle.Wrapped,
     ResponseFormat = WebMessageFormat.Json)]
    public string GetInsertaRegistro(string nameobjeto, string contenareglo)
    {
        string res = obj_consultas_procesos.ftn_Inserta_Registro(nameobjeto, contenareglo);
        if (obj_dt != null)
        {
            return res;
        }
        else
        {
            return string.Format("{0}", "Sin Información");
        }
    }

    [WebInvoke(Method = "GET",
   BodyStyle = WebMessageBodyStyle.Wrapped,
   ResponseFormat = WebMessageFormat.Json)]
    public string GetModificaReporte(string usuario, string idCron)
    {
        obj_dt = obj_consultas_procesos.ftn_verlista_contactos(usuario, idCron);
        if (obj_dt != null)
        {
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else
        {
            return string.Format("{0}", "Sin Información");
        }
    }

    [WebInvoke(Method = "GET",
   BodyStyle = WebMessageBodyStyle.Wrapped,
   ResponseFormat = WebMessageFormat.Json)]
    public string GetReporte(string usuario, string idreporte)
    {
        obj_dt = obj_consultas_procesos.ftn_muestra_tiporeporte(usuario, idreporte);
        if (obj_dt != null)
        {
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else
        {
            return string.Format("{0}", "Sin Información");
        }
    }

    [WebInvoke(Method = "GET",
   BodyStyle = WebMessageBodyStyle.Wrapped,
   ResponseFormat = WebMessageFormat.Json)]
    public string GetReferencia(string idfrecuencia)
    {
        obj_dt = obj_consultas_procesos.ftn_muestra_tipofrecuencia(idfrecuencia);
        if (obj_dt != null)
        {
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else
        {
            return string.Format("{0}", "Sin Información");
        }
    }

    [WebInvoke(Method = "GET",
  BodyStyle = WebMessageBodyStyle.Wrapped,
  ResponseFormat = WebMessageFormat.Json)]
    public string GetNombre_Reporte(string idCron)
    {
        obj_dt = obj_consultas_procesos.ftn_nombre_reporte(idCron);
        if (obj_dt != null)
        {
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else
        {
            return string.Format("{0}", "Sin Información");
        }
    }


    [WebInvoke(Method = "GET",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    ResponseFormat = WebMessageFormat.Json)]
    public string GetConsultaTiposProcesosAduana()
    {
        obj_dt = obj_consultas_procesos.ftn_consulta_tipos_procesos_aduana();
        if (obj_dt != null)
        {
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else
        {
            return string.Format("{0}", "Sin Información");
        }
    }



    [WebInvoke(Method = "GET",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    ResponseFormat = WebMessageFormat.Json)]
    public string GetConsultaTiposProcesosTrading()
    {
        obj_dt = obj_consultas_procesos.ftn_consulta_tipos_procesos_trading();
        if (obj_dt != null)
        {
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else
        {
            return string.Format("{0}", "Sin Información");
        }
    }


    [WebInvoke(Method = "GET",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    ResponseFormat = WebMessageFormat.Json)]
    public string GetConsultaTiposProcesosAduanaTrading()
    {
        obj_dt = obj_consultas_procesos.ftn_consulta_tipos_procesos_aduana_trading();
        if (obj_dt != null)
        {
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else
        {
            return string.Format("{0}", "Sin Información");
        }
    }


    [WebInvoke(Method = "GET",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    ResponseFormat = WebMessageFormat.Json)]
    public string GetConsultaNumeroParametrosReporteAnomalia(string id_rep)
    {
        obj_dt = obj_consultas_procesos.ftn_consulta_numero_parametros_reporte_anomalia(id_rep);
        if (obj_dt != null)
        {
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else
        {
            return string.Format("{0}", "Sin Información");
        }
    }


    [WebInvoke(Method = "GET",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    ResponseFormat = WebMessageFormat.Json)]
    public string GetConsultaParametrosReporteAnomalia(string sql)
    {
        obj_dt = obj_consultas_procesos.ftn_consulta_parametros_reporte_anomalia(sql);
        if (obj_dt != null)
        {
            return obj_func_genericas.ftn_retorna_serializable(obj_dt).ToString();
        }
        else
        {
            return string.Format("{0}", "Sin Información");
        }
    }

}

