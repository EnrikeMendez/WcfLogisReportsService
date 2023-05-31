using System;
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
	
	//Variables procesos:
    int backDays = 1;
    DateTime obj_date_time = DateTime.Now;

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
        SQL = SQL + " (Reporte generado bajo demanda)') AS name ,error.id_reporte AS id_cron ,error.id_chron_error AS id_chron_error \n";
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
        SQL = "select  cron.id_rapport as id_cron, TO_char(cron.last_execution,'DD/MON/YYYY HH24:MI') as hora_creacion, rep_detalle.name,  \n";
        SQL = SQL + " rep.id_rep || ' - ' ||rep.name as tipo_reporte, \n";
        SQL = SQL + " (cron.MOIS || cron.JOUR_SEMAINE || cron.HEURES || cron.MINUTES || cron.JOURS) as programacion, \n";
        SQL = SQL + "  cron.priorite, cron.test, cron.in_progress, rep_detalle.id_cron, \n";
        SQL = SQL + "  (select error.log from rep_chron_error error, rep_detalle_reporte reporte where trunc(error.date_created) = trunc(sysdate) and error.id_reporte = reporte.id_cron and error.id_reporte = rep_detalle.id_cron and rownum = 1)as errores \n";
        SQL = SQL + "  , cron.id_chron, reprocesos.nombre_proceso ,reprocesos.status\n";
        SQL = SQL + "  from REP_CHRON cron \n";
        SQL = SQL + "  JOIN rep_detalle_reporte rep_detalle on cron.id_rapport = rep_detalle.id_cron   \n";
        SQL = SQL + "  JOIN rep_reporte rep on rep.ID_REP = rep_detalle.id_rep  \n";
        SQL = SQL + "  LEFT OUTER JOIN rep_reprocesos_reporte reprocesos on reprocesos.id_cron = cron.id_rapport  \n";
        SQL = SQL + "  where cron.active <> 0 \n";
        SQL = SQL + "  and trunc(cron.last_execution) = trunc(sysdate) \n";
        SQL = SQL + "  order by  cron.in_progress desc, hora_creacion desc ";
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";

        return dt;
    }

}