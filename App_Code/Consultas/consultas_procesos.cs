﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de consultas_procesos
/// </summary>
public class consultas_procesos
{
    DataTable dt = new DataTable();
    DB conexion = new DB();
    string SQL;
	Boolean ejecuta_querye;

    //Variables procesos:
    int backDays = 1;
    DateTime obj_date_time = DateTime.Now;
    funciones_genericas obj_func_genericas = new funciones_genericas();

    public DataTable ftn_consulta_errores_reportes()
    {
        if (obj_func_genericas.ftn_Weekday(obj_date_time, DayOfWeek.Friday) == 2)
        {
            backDays = 3;
        }

        SQL = " SELECT reporte.name AS name ,error.id_reporte AS id_cron ,error.id_chron_error AS id_chron_error ,\n";
        SQL = SQL + " error.tipo_error AS tipo_error ,error.log AS log ,TO_CHAR(error.date_created,'DD/MON/YYYY HH24:MI') AS hora \n";
        SQL = SQL + " FROM rep_chron_error error INNER JOIN rep_detalle_reporte reporte ON error.id_reporte = reporte.id_cron \n";
        SQL = SQL + " WHERE error.date_created >= ((sysdate - " + backDays + ") + (8/24)) \n";
        SQL = SQL + " UNION SELECT CONCAT(REPLACE(REPLACE(SUBSTR(error.log,INSTR(error.log,'|',-1)+1 ),' Reporte : ',''),'.xls',''), ' \n";
        SQL = SQL + " (<i>Reporte generado bajo demanda</i>)') AS name ,error.id_reporte AS id_cron ,error.id_chron_error AS id_chron_error \n";
        SQL = SQL + " ,error.tipo_error AS tipo_error ,error.log AS log ,TO_CHAR(error.date_created,'DD/MON/YYYY HH24:MI') AS hora \n";
        SQL = SQL + " FROM rep_chron_error error \n";
        SQL = SQL + " WHERE error.date_created >= ((sysdate - " + backDays + ") + (8/24)) AND error.id_reporte NOT IN (SELECT DISTINCT reporte.id_cron FROM rep_detalle_reporte reporte) \n";
        SQL = SQL + " ORDER BY hora DESC ,id_cron DESC \n";
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";

        return dt;
    }
    public DataTable ftn_consulta_monitoreo_reportes()
    {
        if (obj_func_genericas.ftn_Weekday(obj_date_time, DayOfWeek.Friday) == 2)
        {
            backDays = 3;
        }
        SQL = "select  cron.id_rapport as id_cron, TO_char(cron.last_execution,'DD/MON/YYYY HH24:MI') as hora_creacion, rep_detalle.name,  \n";
        SQL = SQL + " rep.id_rep || ' - ' ||rep.name as tipo_reporte, \n";
        SQL = SQL + " NVL((cron.MOIS || cron.JOUR_SEMAINE || cron.HEURES || cron.MINUTES || cron.JOURS),' ') as programacion, \n";
        SQL = SQL + "  cron.priorite, cron.test, cron.in_progress, rep_detalle.id_cron id_cron_det, \n";
        SQL = SQL + "  (select error.log from rep_chron_error error, rep_detalle_reporte reporte where trunc(error.date_created) = trunc(sysdate) and error.id_reporte = reporte.id_cron and error.id_reporte = rep_detalle.id_cron and rownum = 1)as errores \n";
        SQL = SQL + "  , cron.id_chron, reprocesos.nombre_proceso ,reprocesos.status\n";
        SQL = SQL + "  from REP_CHRON cron \n";
        SQL = SQL + "  JOIN rep_detalle_reporte rep_detalle on cron.id_rapport = rep_detalle.id_cron   \n";
        SQL = SQL + "  JOIN rep_reporte rep on rep.ID_REP = rep_detalle.id_rep  \n";
        SQL = SQL + "  LEFT OUTER JOIN rep_reprocesos_reporte reprocesos on reprocesos.id_cron = cron.id_rapport  \n";
        SQL = SQL + "  where cron.active <> 0 \n";
        SQL = SQL + "  AND cron.last_execution	between sysdate - " + backDays + " and sysdate \n";
        SQL = SQL + "  order by  cron.in_progress desc, hora_creacion desc ";
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";

        return dt;
    }
    public DataTable ftn_consulta_gen(string SQL)
    {
        dt = conexion.ObtieneDataTable(SQL);
        return dt;
    }
	
	
	public DataTable ftn_consulta_cambio_prioridad()
    {
        SQL = " select reporte.id_rep, reporte.name as nombre_reporte, chron.priorite, cliente, rep_det.id_cron, rep_det.name as nombre_detalle, TO_char(date_created,'DD/MON/YYYY HH24:MI') as hora_creacion, rep_det.dest_mail, id_chron \n";
        SQL = SQL + " from rep_detalle_reporte \n";
        SQL = SQL + " rep_det join REP_CHRON chron on chron.id_rapport = rep_det.id_cron \n";
        SQL = SQL + " join rep_reporte reporte on reporte.id_rep = rep_det.id_rep \n";
        SQL = SQL + " where chron.active = 1 and chron.MINUTES is null and chron.HEURES is null and chron.JOURS is null and chron.MOIS is null and chron.JOUR_SEMAINE is null and chron.LAST_EXECUTION is null \n";
        SQL = SQL + " order by chron.priorite, id_cron desc \n";

        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";

        return dt;

    }


    public bool ftn_modifica_cambio_prioridad(string id_crons, string prioridad)
    {
        SQL = "update rep_chron set priorite = " + prioridad + " where id_chron in (" + id_crons + ")";
        ejecuta_querye = conexion.EjecutarQuery(SQL);
        SQL = "";
        return ejecuta_querye;
    }

    public string ftn_Mail(string Id_Cron, string NumCli, string id_mail, string nombre, string correo, string Tercero,string status)
    {
        string mail_ok = string.Empty;
        string msg = string.Empty;
        int allOk = string.Empty;
        SQL = " select MAIL_OK,ID_CRON from rep_detalle_reporte where ID_CRON = '" + Id_Cron + "' ";
        dt = conexion.ObtieneDataTable(SQL);

       if(dt.Rows.Count > 0)
        {
            mail_ok = dt.Rows[0][0].ToString();
        }
        if (NumCli != "") {
            SQL = "select 1 from eclient where cliclef='" + NumCli + "'";
            dt = conexion.ObtieneDataTable(SQL);
            if (dt.Rows.Count == 0)
            {
                if (NumCli != "")
                {
                    return "window.location.href='mail.asp?msg= Este numero de cliente '" + NumCli + "' no existe.";
                }
            }
        }
        if (id_mail != "")
        {
            if (NumCli != "")
            {
                SQL = " update rep_mail set nombre= '" + nombre + "', ";
                SQL = SQL + " mail = '" + correo + "', ";
                SQL = SQL + " client_num = '" + NumCli + "', ";
                SQL = SQL + " tercero = '" + Tercero + "', ";
                SQL = SQL + " status = '" + status + "' ";
                SQL = SQL + " where id_mail= '" + id_mail + "' ";
                dt = conexion.ObtieneDataTable(SQL);
            }

            msg = "Contacto Modificado";

            allOk = 1;
        }
        else
        //'verificacion que el correo es unico en la base
        {
            if (NumCli != "")
            {
                SQL = " select 1 from rep_mail where mail = '" + correo + "' ";
                SQL = SQL + " and CLIENT_NUM = '" + NumCli + "'";
                dt = conexion.ObtieneDataTable(SQL);
                if (dt.Rows.Count > 0)
                {
                    return "window.location.href='mail.asp?msg= Este correo ya existe para este cliente.";
                }
            }

            //            'verificar que no se capturen correos de Logis para otro numero de cliente que el 9929

            if (NumCli != ""){
                //'<JEMV
                //if (InStr(LCase(Request.Form("correo")), "@logis.com.mx") > 0 and NumCli <> "9929" then
                //'					Response.Redirect asp_self & "?msg=" & Server.URLEncode ("Favor de crear los correos de Logis con el numero de cliente 9929.")
                //'				end if
                //'JEMV>


                //                SQL = "insert into rep_mail (ID_MAIL, NOMBRE, MAIL, CLIENT_NUM, TERCERO, STATUS) " & _

                //                        " values  (seq_mail.nextval, '" & _

                //                        SQLEscape(Request.Form("nombre")) & "', '" + correo + _

                //                        "', '" + NumCli + "', null,  1" & _

                //                        " )"
            }
            else {
                //            '<JEMV
                //'				if InStr(LCase(Request.Form("correo")), "@logis.com.mx") > 0 and Request.Form("cli_num") <> "9929" then
                //'					Response.Redirect asp_self & "?msg=" & Server.URLEncode ("Favor de crear los correos de Logis con el numero de cliente 9929.")
                //'				end if
                //'JEMV>


                //                SQL = "insert into rep_mail (ID_MAIL, NOMBRE, MAIL, CLIENT_NUM, TERCERO, STATUS) " & _

                //                        " values  (seq_mail.nextval, '" & _

                //                        SQLEscape(Request.Form("nombre")) & "', '" + correo + _

                //                        "', '" & SQLescape(Request.Form("cli_num")) & "', null,  1" & _

                //                        " )"

            }


            //            msg = "Contacto incluido"

            //            allOk = 1

        }
                return "";
    }
}