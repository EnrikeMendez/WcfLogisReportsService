using System;
using System.Collections.Generic;
using System.Data;
//using System.Linq;
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

    public bool ftn_modifica_cambio_prioridad_dinamica()
    {
        SQL = "select reporte.id_rep,  chron.priorite, rep_det.id_cron, chron.id_chron \n";
        SQL = SQL + " from rep_detalle_reporte rep_det  \n";
        SQL = SQL + " join REP_CHRON chron on chron.id_rapport = rep_det.id_cron \n";
        SQL = SQL + " join rep_reporte reporte on reporte.id_rep = rep_det.id_rep  \n";
        SQL = SQL + " where chron.active = 1  \n";
        SQL = SQL + " and chron.MINUTES is null and chron.HEURES is null and chron.JOURS is null and chron.MOIS is null and chron.JOUR_SEMAINE is null and chron.LAST_EXECUTION is null  \n";
        SQL = SQL + " order by chron.priorite, id_cron desc \n";
        ejecuta_querye = conexion.EjecutarQuery(SQL);
        SQL = "";
        return ejecuta_querye;
    }

    public string ftn_Mail(string Id_Cron, string NumCli, string id_mail, string nombre, string correo, string Tercero, string status, string hdnURI)
    {
        string mail_ok = string.Empty;
        string msg = string.Empty;
        string idMail = string.Empty;
        int allOk = 0;
        if (Id_Cron != "undefined" && Id_Cron != "")
        {
            dt = ftn_obtener_idmail_idcrone(Id_Cron);
        }
        if (dt.Rows.Count > 0)
        {
            mail_ok = dt.Rows[0][0].ToString();
        }
        if (NumCli != "")
        {
            dt = ftn_obtener_cliente(NumCli);
            if (dt.Rows.Count == 0)
            {
                if (NumCli != "")
                {
                    return "Este numero de cliente '" + NumCli + "' no existe.";
                }
            }
        }
        if (id_mail != "")
        {
            if (NumCli != "")
            {
                ejecuta_querye = ftn_modifica_repMail(nombre, correo, NumCli, Tercero, status, id_mail);
                if (ejecuta_querye)
                {
                    msg = "Contacto Modificado";
                    allOk = 1;
                }
            }
        }
        else
        //'verificacion que el correo es unico en la base
        {
            if (NumCli != "")
            {
                dt = ftn_validar_correunico(correo, NumCli);
                if (dt.Rows.Count > 0)
                {
                    return "Este correo ya existe para este cliente.";
                }
            }
            if (NumCli != "")
            {
                msg = ftn_insertar_repMail(id_mail, nombre, correo, NumCli, Tercero, status);
                allOk = 1;
            }
        }
        dt = ftn_obtener_idmail_numCliente(correo, NumCli);
        if (dt.Rows.Count > 0)
        {
            idMail = dt.Rows[0][0].ToString();
        }
        if (mail_ok != "")
        {
            ejecuta_querye = ftn_insertar_rep_dest_mail(mail_ok, idMail);
            if (ejecuta_querye)
            {
                return hdnURI + msg + "El usuario " + correo + " fue agregado correctamente.";
            }
           
        }
        return msg;
    }

        //08-06-2023 JM
        //ver_lista
        public DataTable ftn_verlista_contactos(string usuario, string idCron)
    {
        List<string> p1 = new List<string>();
        List<string> p2 = new List<string>();
        p1 = obj_func_genericas.perfil1();
        p2 = obj_func_genericas.perfil1();




        SQL = " select repdet.ID_CRON, repdet.NAME, rep.ID_REP, rep.name, repdet.CLIENTE, InitCap(cli.clinom)" +
        ", repdet.CARPETA, repdet.FILE_NAME, repdet.LAST_CREATED, repdet.MAIL_OK, repdet.MAIL_ERROR" +
        ", repdet.FRECUENCIA, tipo.DESCRIPCION, cron.HEURES, cron.MINUTES, cron.JOURS" +
        ", cron.MOIS, cron.JOUR_SEMAINE, cron.PRIORITE, nvl(rep.subcarpeta,' ') SUBCARPETA, rep.TEMP_MENSAJE_FECHA" +
        ", rep.TEMP_MENSAJE, rep.NUM_OF_PARAM, decode(repdet.confirmacion, '1', 'checked')" +
        ", repdet.days_deleted from rep_detalle_reporte repdet  , rep_reporte rep  , rep_chron cron" +
        ", REP_TIPO_FRECUENCIA tipo, eclient cli where rep.ID_REP = repdet.id_rep and cron.ID_RAPPORT = repdet.id_cron " +
        "and tipo.ID_TIPO_FREC = repdet.FRECUENCIA and cli.cliclef = repdet.cliente and repdet.ID_CRON = " + idCron;
        for (int cont = 0; cont < p1.Count; cont++)
        {
            if (usuario.Equals(p1[cont]))
            {
                SQL = SQL + " and rep.id_rep in (14,173,24) ";
            }
        }
        for (int cont = 0; cont < p2.Count; cont++)
        {
            if (usuario.Equals(p2[cont]))
            {
                SQL = SQL + " and rep.id_rep = 174 ";
            }
        }
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }

    public DataTable ftn_validar_reprocesos(string idCron)
    {
        SQL = string.Format("select nombre_proceso from REP_REPROCESOS_REPORTE where id_cron = '{0}' and status = '{1}'", idCron, 1);
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }

    public DataTable ftn_muestra_tiporeporte(string usuario, string idreporte)
    {
        List<string> p1 = new List<string>();
        List<string> p2 = new List<string>();
        p1 = obj_func_genericas.perfil1();
        p2 = obj_func_genericas.perfil1();

        SQL = String.Format(" select id_rep, name, decode(id_rep,'{0}', 'selected') SELECCION " +
            " from rep_reporte WHERE 1=1", idreporte);
        for (int cont = 0; cont < p1.Count; cont++)
        {
            if (usuario.Equals(p1[cont]))
            {
                SQL = SQL + " and id_rep in (14,173,24) ";
            }
        }
        for (int cont = 0; cont < p2.Count; cont++)
        {
            if (usuario.Equals(p2[cont]))
            {
                SQL = SQL + " and id_rep = 174 ";
            }
        }
        SQL = SQL + "  order by 2";

        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }
    //case 1
    public DataTable ftn_lista_reporte(string usuario, string status)
    {
        List<string> p1 = new List<string>();
        List<string> p2 = new List<string>();
        p1 = obj_func_genericas.perfil1();
        p2 = obj_func_genericas.perfil1();

        SQL = "select repdet.ID_CRON, \n";
        SQL = SQL + "trim(nvl(repdet.NAME,''))NOMBRE, \n";
        SQL = SQL + "rep.id_rep, trim(nvl(rep.name,'')) REP_NAME\n";
        SQL = SQL + ", repdet.CLIENTE, \n";
        SQL = SQL + "InitCap(cli.clinom) INIT_CAP, \n";
        SQL = SQL + "repdet.FRECUENCIA, \n";
        SQL = SQL + "tipo.DESCRIPCION, \n";
        SQL = SQL + "repdet.mail_ok, nvl(cron.active, 0)NVL_ACTIVE , \n";
        SQL = SQL + "repdet.mail_error\n";
        SQL = SQL + ", cli.clirfc, case when  rep.id_rep in (80,98,106,108 \n";
        SQL = SQL + ",110,114,117,118,126,130,142,159,160, 169,171,174,179,175 \n";
        SQL = SQL + ",176,183,186,199,201,221,228,236,240,242,244,248,\n";
        SQL = SQL + "249,260,263,287,288,290) then 'DISTRIBUCION' else 'COEX' end Area_Negocio, \n";
        SQL = SQL + "cron.priorite as Prioridad, \n";
        SQL = SQL + "tipo.DESCRIPCION as frecuencia_desc, \n";
        SQL = SQL + "repdet.days_deleted as dias_servidor, \n";
        SQL = SQL + "nvl(cron.jours, ' ') AS DIA_MES, \n";
        SQL = SQL + "nvl(cron.jour_semaine,' ') AS DIA_SEMANA, \n";
        SQL = SQL + "nvl(cron.heures,'0') AS HORA, \n";
        SQL = SQL + "nvl(cron.minutes,'0') AS MINUTO,\n";
        SQL = SQL + "repdet.CLIENTE || ' ' || repdet.NAME AS Num_Nom, \n";
        SQL = SQL + "nvl(repdet.param_1,' ') PARAM_1, \n";
        SQL = SQL + "nvl(repdet.param_2,' ') PARAM_2, \n";
        SQL = SQL + "nvl(repdet.param_3,' ') PARAM_3, \n";
        SQL = SQL + "nvl(repdet.param_4,' ') PARAM_4, \n";
        SQL = SQL + "trim(nvl(repdet.created_by,' ')) USR_CREACION, \n";
        SQL = SQL + "TO_char(repdet.date_created,'DD/MON/YYYY HH24:MI') CREACION, \n";
        SQL = SQL + "trim(nvl(repdet.modified_by,' ')) USUARIO, \n";
        SQL = SQL + "nvl(TO_char(repdet.date_modified,'DD/MON/YYYY HH24:MI'), ' ') MODIFICACION, \n ";
        SQL = SQL + "rep.COMMAND AS COMMAND from rep_detalle_reporte repdet, \n";
        SQL = SQL + "rep_reporte rep, \n";
        SQL = SQL + "rep_chron cron, \n";
        SQL = SQL + "REP_TIPO_FRECUENCIA tipo,\n";
        SQL = SQL + "eclient cli where 1=1";
        if (status.Equals("Desactivar"))
        {
            SQL = SQL + " and nvl(cron.active, 0) = 1";
        }
        else
        {
            SQL = SQL + " and nvl(cron.active, 0) = 0";
        }

        SQL = SQL + " and rep.ID_REP = repdet.id_rep and cron.ID_RAPPORT(+) = repdet.id_cron \n";
        SQL = SQL + "and tipo.ID_TIPO_FREC = repdet.FRECUENCIA \n";
        SQL = SQL + "and cli.cliclef = repdet.cliente\n";
        for (int cont = 0; cont < p1.Count; cont++)
        {
            if (usuario.Equals(p1[cont]))
            {
                SQL = SQL + " and rep.id_rep in (14,173,24)\n";
            }
        }
        for (int cont = 0; cont < p2.Count; cont++)
        {
            if (usuario.Equals(p2[cont]))
            {
                SQL = SQL + " and rep.id_rep = 174\n";
            }
        }
        SQL = SQL + " order by 2";
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }

    public DataTable ftn_nombre_reporte(string idCron)
    {
        SQL = string.Format("select Name FROM rep_detalle_reporte repdet WHERE repdet.ID_CRON = '" + idCron + "'" );
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }

    public DataTable ftn_lista_idcontactos(string idLista, string tipoLista)
    {
        if (tipoLista.Equals("grupo"))
        {
            SQL = string.Format("select id_dest_lista from rep_lista_mail where id_lista = '{0}'" + idLista);
        }
        else
        {
            SQL = string.Format("select id_dest from rep_dest_mail where id_dest_mail = '{0}'" + idLista);
        }
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }

    public DataTable ftn_lista_contactos(string lista)
    {
        SQL = "select id_dest from rep_dest_mail where id_dest_mail = " + lista;
        dt = conexion.ObtieneDataTable(SQL);

        SQL = "";
        return dt;
    }

    public DataTable ftn_armar_contactos(string mail_id)
    {
        SQL = " select nombre, mail, decode(client_num, 9929,'Logis',client_num) as client_num " +
                " , decode(tercero, 1, 'Si', '') as tercero From rep_mail Where id_mail  in (" + mail_id + ")" +
                " and status = 1 order by client_num, tercero desc, nombre ";
        dt = conexion.ObtieneDataTable(SQL);

        SQL = "";
        return dt;
    }

    public DataTable ftn_obtener_idmail_idcrone(string idCrone)
    {
        SQL = string.Format("select MAIL_OK, ID_CRON from rep_detalle_reporte where ID_CRON = '{0}'" + idCrone);
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }

    public DataTable ftn_obtener_cliente(string numCli)
    {
        SQL = string.Format("select 1 from eclient where cliclef = '{0}'", numCli);
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }

    public bool ftn_modifica_repMail(string nombre, string correo, string numCli, string tercero,
        string status, string idMail)
    {
        SQL = string.Format(" update rep_mail set nombre= '{0}', mail = '{1}', " +
              " client_num = '{2}', tercero = '{3}', " +
              " status = '{4}' where id_mail= '{5}' ", nombre, correo, numCli, tercero, status, idMail);
        ejecuta_querye = conexion.EjecutarQuery(SQL);
        SQL = "";
        return ejecuta_querye;
    }

    public DataTable ftn_validar_correunico(string correo, string numCliente)
    {
        SQL = string.Format(" select 1 from rep_mail where mail = '{0}' and CLIENT_NUM = '{1}'", correo, numCliente);
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }

    public string ftn_insertar_repMail(string idMail, string nombre, string mail, string numCliente, string tercero, string status)
    {
        string res = "";
        try
        {
            SQL = string.Format("insert into rep_mail (ID_MAIL, NOMBRE, MAIL, CLIENT_NUM, TERCERO, STATUS) values" +
                "(seq_mail.nextval,'{0}','{1}','{2}','{3}','{4}')", nombre, mail, numCliente, tercero, status);
            ejecuta_querye = conexion.EjecutarQuery(SQL);
            SQL = "";

            res = "Contacto incluido";

        }
        catch (Exception ex)
        {
            res = "Error";
        }
        return res;
    }

    public DataTable ftn_obtener_idmail_numCliente(string correo, string numCliente)
    {
        SQL = string.Format(" select ID_MAIL,CLIENT_NUM from rep_mail where mail = '{0}' and CLIENT_NUM = '{1}'",
            correo, numCliente);
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }

    public bool ftn_insertar_rep_dest_mail(string idMail, string idDestino)
    {
        SQL = string.Format("insert into rep_dest_mail (id_dest_mail, id_dest) values ('{0}','{1}')", idMail, idDestino);
        ejecuta_querye = conexion.EjecutarQuery(SQL);
        SQL = "";
        return ejecuta_querye;
    }

    public DataTable ftn_obtener_busca_contactos()
    {
        SQL = string.Format("select distinct client_num from rep_mail order by client_num");
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }

    public string ftn_borrar_repMail(string mail)
    {
        string res = "";
        try
        {
            SQL = string.Format("delete from rep_mail where id_mail= '{0}'", mail);
            ejecuta_querye = conexion.EjecutarQuery(SQL);
            SQL = "";
            res = "Contacto borrado";
        }
        catch (Exception ex)
        {
            res = "Error";
        }
        return res;
    }

    //Inicia reporocesos Case 2 reprocesos
    public DataTable ftn_siguiente_registro()
    {
        SQL = "select SEQ_CHRON.nextval from dual";
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }

    public DataTable ftn_insert_rep_detalle_reporte(string num_reporte, string last_conf_date_1,
        string last_conf_date_2, string reqnum_reporte)
    {
        SQL = string.Format("insert into REP_DETALLE_REPORTE " +
        "(id_cron, id_rep, dest_mail, mail_ok, mail_error, name, cliente, frecuencia, file_name, carpeta, param_1, " +
        "param_2, days_deleted, last_created, last_conf_date_1, last_conf_date_2, test, param_3, created_BY, " +
        "date_modified select '{0}', id_rep, 'desarrollo_web@logis.com.mx', 6381, 6381, name, cliente," +
        " frecuencia, file_name, carpeta, param_1, param_2, days_deleted, last_created, " +
        "to_char(to_date('{1}','mm/dd/yyyy')) last_conf_date_1, " +
        "to_date('{2}', 'mm/dd/yyyy') last_conf_date_2, test, " +
        "param_3, created_BY, date_modified from REP_DETALLE_REPORTE where ID_CRON = '{3}'", num_reporte
        , last_conf_date_1, last_conf_date_2, reqnum_reporte);
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }

    public DataTable ftn_insert_repchron(string num_reporte)
    {
        SQL = string.Format("insert into rep_chron (id_chron, id_rapport, priorite, test, active) " +
              "values (SEQ_CHRON.nextval, '{0}', 1,0, 1)", num_reporte);
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }
    //Actualiza tablas de reproceso
    public DataTable ftn_update_rep_detalle_reporte(string tipo_reporte, string report_name, string file_name,
        string carpeta, string frecuencia, string con_conf, string diasServidor, string param_1, string param_2,
        string param_3, string param_4, string usuario, string numreporte)
    {

        SQL = string.Format(" update REP_DETALLE_REPORTE " +
        " set ID_REP = '{0}' " +
        ", NAME ='{1}' " +
        ", FILE_NAME = '{2}' " +
        ", CARPETA = '{3}' " +
        ", FRECUENCIA = '{4}' " +
        ", CONFIRMACION = '{5}' " +
        ", DAYS_DELETED = '{6}' " +
        ", param_1 = '{7}'  " +
        ", param_2 = '{8}'  " +
        ", param_3 = '{9}'  " +
        ", param_4 = '{10}'  " +
        ", MODIFIED_BY = '{11}' " +
        ", DATE_MODIFIED = sysdate " +
        " where ID_CRON = '{12}' ",
        tipo_reporte, report_name, file_name, carpeta,
        frecuencia, con_conf, diasServidor, param_1,
        param_2, param_3, param_4, usuario, numreporte);

        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }

    public DataTable ftn_update_rep_chron(string hora, string minutos, string dia, string mes,
        string dia_semana, string prioridad, string num_reporte)
    {
        SQL = string.Format("update REP_CHRON " +
        "set HEURES = '{0}'" +
        ", MINUTES = '{1}'" +
        ", JOURS = '{2}'" +
        ", MOIS = '{3}'" +
        ", JOUR_SEMAINE = '{4}'" +
        ", PRIORITE = '{5}'" +
        ", LAST_EXECUTION = null " +
        "where ID_RAPPORT = '{6}'", hora, minutos, dia, mes, dia_semana,
        prioridad, num_reporte);
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }

    public DataTable ftn_update_rep_reporte(string tempMsg, string tmpMensajeFecha, string tipoReporte)
    {
        SQL = string.Format("update REP_REPORTE " +
        " set TEMP_MENSAJE = '{0}' " +
        " , TEMP_MENSAJE_FECHA = to_date('{1}', 'mm/dd/yyyy') " +
        " where ID_REP = '{2}'", tempMsg, tmpMensajeFecha, tipoReporte);
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }
    //case 3 desactivar / activar reportes
    public string ftn_act_desa_rep_chron(string idreporte, string accion)
    {
        string res = "";
        try
        {
            SQL = string.Format("update rep_chron set active = '{0}', last_execution = null where id_rapport = '{1}'", accion, idreporte);
            ejecuta_querye = conexion.EjecutarQuery(SQL);
            SQL = "";
            if (accion.Equals("1"))
            {
                return "Reporte reactivado.";
            }
            else
            {
                return "Reporte desactivado.";
            }
        }
        catch (Exception ex)
        {
            res = "Error";
        }
        return res;
    }

    public DataTable ftn_modicacion_lista_correos(string mail_list, string id_client)
    {
        SQL = string.Format("select distinct id_mail, nombre, mail, decode(client_num, 9929,'Logis',client_num) as client_num" +
                ", decode(tercero, 1, 'Si', '') as TERCERO, decode(id_dest_mail,'{0}', 'checked') checked" +
                " From rep_mail, rep_dest_mail" +
                " Where id_dest(+) = id_mail" +
                " and client_num in ('{1}', 9929)" +
                " and status = 1" +
                " order by client_num, tercero desc, nombre, checked", mail_list, id_client);
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }

    public DataTable ftn_Valida_Correos(string mail_list)
    {
        SQL = string.Format("SELECT ID_CRON, NAME FROM REP_DETALLE_REPORTE WHERE MAIL_OK = '{0}'", mail_list);
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }

    public DataTable ftn_Valida_Correos_Originales(string mail_list)
    {
        SQL = string.Format("SELECT LISTAGG(ID_DEST,',') WITHIN GROUP(ORDER BY  ID_DEST) ORIGINALES FROM rep_dest_mail WHERE ID_DEST_MAIL = '{0}'", mail_list);
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }

    public string ftn_Elimina_Originales(string mail_list)
    {
        string res = "";
        try
        {
            SQL = string.Format("delete from rep_dest_mail where id_dest_mail = '{0}'", mail_list);
            ejecuta_querye = conexion.EjecutarQuery(SQL);
            SQL = "";
            res = "";
        }
        catch (Exception ex)
        {
            res = "Error";
        }
        return res;
    }

    public string ftn_Inserta_Registro(string nameobjeto, string contenareglo)
    {
        string res = "";
        if (contenareglo.Equals(""))
        {
            res = "- escoge al menos un contacto.";
        }
        else
        {
            contenareglo = contenareglo.Remove(contenareglo.Length - 1);
            char caracter = ',';
            string[] referencias = contenareglo.Split(caracter);
            try
            {
                for (int i = 0; i < referencias.Length; i++)
                {
                    SQL = string.Format(" insert into rep_dest_mail (id_dest_mail, id_des) " +
                    " values ('{0}','{1}')", nameobjeto, referencias[i]);
                    ejecuta_querye = conexion.EjecutarQuery(SQL);
                    SQL = "";
                }
                res = "";
            }
            catch (Exception ex)
            {
                res = "Error";
            }
        }
        return res;
    }


    public DataTable ftn_muestra_tipofrecuencia(string idfrecuencia)
    {        
        SQL = String.Format(" select tipo.ID_TIPO_FREC, tipo.DESCRIPCION, decode(tipo.ID_TIPO_FREC, '{0}', 'selected') SELECCION from REP_TIPO_FRECUENCIA tipo order by 2", idfrecuencia);        
        dt = conexion.ObtieneDataTable(SQL);
        SQL = "";
        return dt;
    }



    
}