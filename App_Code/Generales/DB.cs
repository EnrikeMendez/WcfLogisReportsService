using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;

public class DB
{
    private OracleConnection cnn = null;
    //private OracleDataReader dr = null;
    private OracleCommand cmd = null;
    private OracleDataAdapter da = null;
    private DataTable dtRes = null;
    private string msj = string.Empty;
    //private Business negocio = new Business();

    private string GetConnectionString()
    {
        AppSettingsReader reader = new AppSettingsReader();
        return reader.GetValue("cnn", typeof(string)).ToString();
    }
    public bool EjecutarQuery(string sql)
    {
        bool res = true;

        try
        {
            cnn = new OracleConnection();
            cnn.ConnectionString = GetConnectionString();

            cmd = new OracleCommand();
            cmd.CommandText = sql;
            cmd.Connection = cnn;

            cnn.Open();
            cmd.ExecuteNonQuery();
            cnn.Close();
        }
        catch (Exception ex)
        {
            ex.Source += sql;
            //LOG.RegistraExcepcion(ex);
            res = false;
        }
        finally
        {
            if (cnn != null)
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
                cnn.Dispose();
                GC.SuppressFinalize(cnn);
            }
            if (cmd != null)
            {
                cmd.Dispose();
                GC.SuppressFinalize(cmd);
            }
        }

        return res;
    }
    public string ObtieneCadena(string sql)
    {
        string res = string.Empty;

        try
        {
            cnn = new OracleConnection();
            cnn.ConnectionString = GetConnectionString();

            cmd = new OracleCommand();
            cmd.CommandText = sql;
            cmd.Connection = cnn;

            cnn.Open();
            res = cmd.ExecuteScalar().ToString();
            cnn.Close();
        }
        catch (Exception ex)
        {
            ex.Source += sql;
            //LOG.RegistraExcepcion(ex);
            res = string.Empty;
        }
        finally
        {
            if (cnn != null)
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
                cnn.Dispose();
                GC.SuppressFinalize(cnn);
            }
            if (cmd != null)
            {
                cmd.Dispose();
                GC.SuppressFinalize(cmd);
            }
        }

        return res;
    }
    public DataTable ObtieneDataTable(string sql)
    {
        DataTable dtTemp = new DataTable();

        try
        {
            dtRes = new DataTable();
            cnn = new OracleConnection();
            cnn.ConnectionString = GetConnectionString();

            cmd = new OracleCommand();
            cmd.CommandText = sql;
            cmd.Connection = cnn;

            cnn.Open();
            da = new OracleDataAdapter(cmd);
            da.Fill(dtTemp);
            cnn.Close();

            dtRes = dtTemp.Copy();
        }
        catch (Exception ex)
        {
            ex.Source += sql;
            //LOG.RegistraExcepcion(ex);
            dtRes = null;
        }
        finally
        {
            if (cnn != null)
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
                cnn.Dispose();
                GC.SuppressFinalize(cnn);
            }
            if (cmd != null)
            {
                cmd.Dispose();
                GC.SuppressFinalize(cmd);
            }
            if (da != null)
            {
                da.Dispose();
                GC.SuppressFinalize(da);
            }
            if (dtTemp != null)
            {
                dtTemp.Dispose();
                GC.SuppressFinalize(dtTemp);
            }
        }

        return dtRes;
    }
}