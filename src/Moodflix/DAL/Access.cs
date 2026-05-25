using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;


namespace DAL
{
    public class Access
    {
        SqlConnection _conn;

        public void Open()
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["DanDesktop"].ConnectionString;
            _conn = new SqlConnection(connectionString);
            _conn.Open();
        }

        public void Close()
        {
            _conn.Close();
            _conn = null;
            GC.Collect();
        }

        public SqlParameter CreateParameter(string name, object value)
        {
            SqlParameter param = new SqlParameter(name, value ?? DBNull.Value);
            return param;
        }


        //public SqlParameter CreateParameter(string name, string value)
        //{
        //    SqlParameter param = new SqlParameter(name, value);
        //    param.DbType = DbType.String;

        //    return param;
        //}

        //public SqlParameter CreateParameter(string name, Guid value)
        //{
        //    SqlParameter param = new SqlParameter(name, value)
        //    {
        //        DbType = DbType.Guid
        //    };

        //    return param;
        //}

        //public SqlParameter CreateParameter(string name, int value)
        //{
        //    SqlParameter param = new SqlParameter(name, value);
        //    param.DbType = DbType.Int32;
        //    return param;
        //}

        //public SqlParameter CreateParameter(string name, bool value)
        //{
        //    SqlParameter param = new SqlParameter(name, value);
        //    param.DbType = DbType.Boolean;
        //    return param;
        //}

        //public SqlParameter CreateParameter(string name, DateTime value)
        //{
        //    SqlParameter param = new SqlParameter(name, value);
        //    param.DbType = DbType.DateTime;
        //    return param;
        //}

        //public SqlParameter CreateParameter(string name, float value)
        //{
        //    SqlParameter param = new SqlParameter(name, value);
        //    param.DbType = DbType.Decimal;
        //    return param;
        //}

        private SqlCommand CreateCommand(string sql, List<SqlParameter> parameters = null)
        {
            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                foreach (SqlParameter param in parameters)
                {
                    cmd.Parameters.Add(param);
                }
            }
            return cmd;

        }

        public int Write(string sql, List<SqlParameter> parameters = null)
        {
            int filasAfectadas;
            SqlCommand cmd = CreateCommand(sql, parameters);

            try
            {
                filasAfectadas = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                filasAfectadas = -1;
            }

            return filasAfectadas;

        }

        public int WriteScalar(string sql, List<SqlParameter> parameters = null)
        {
            int id;
            var cmd = CreateCommand(sql, parameters);
            try
            {
                id = int.Parse(cmd.ExecuteScalar().ToString());
            }
            catch
            {
                id = -1;
            }
            return id;
        }

        public DataTable Read(string sql, List<SqlParameter> parameters = null)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = CreateCommand(sql, parameters);
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                adapter.Fill(dt);
                adapter.Dispose();
            }
            return dt;
        }
    }
}
