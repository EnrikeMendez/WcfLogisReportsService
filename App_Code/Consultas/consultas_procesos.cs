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

    public DataTable ftn_consulta_errores_reportes()
    {
        SQL = " SELECT reporte.name AS name ,error.id_reporte AS id_cron ,error.id_chron_error AS id_chron_error ,\n";
        SQL = SQL + " error.tipo_error AS tipo_error ,error.log AS log ,TO_CHAR(error.date_created,'DD/MON/YYYY HH24:MI') AS hora \n";
        SQL = SQL + " FROM rep_chron_error error INNER JOIN rep_detalle_reporte reporte ON error.id_reporte = reporte.id_cron \n";
        SQL = SQL + " WHERE error.date_created >= ((sysdate - 3) + (8/24)) \n";
        SQL = SQL + " UNION SELECT CONCAT(REPLACE(REPLACE(SUBSTR(error.log,INSTR(error.log,'|',-1)+1 ),' Reporte : ',''),'.xls',''), ' \n";
        SQL = SQL + " (Reporte generado bajo demanda)') AS name ,error.id_reporte AS id_cron ,error.id_chron_error AS id_chron_error \n";
        SQL = SQL + " ,error.tipo_error AS tipo_error ,error.log AS log ,TO_CHAR(error.date_created,'DD/MON/YYYY HH24:MI') AS hora \n";
        SQL = SQL + " FROM rep_chron_error error \n";
        SQL = SQL + " WHERE error.date_created >= ((sysdate - 3) + (8/24)) AND error.id_reporte NOT IN (SELECT DISTINCT reporte.id_cron FROM rep_detalle_reporte reporte) \n";
        SQL = SQL + " ORDER BY hora DESC ,id_cron DESC \n";
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";

        return dt;
    }

}