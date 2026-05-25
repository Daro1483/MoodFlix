using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace DAL
{
    public class BackupRepository
    {
        Access access = new Access();

        public int CreateBackup(string ruta)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@ruta", ruta)
            };

            access.Open();
            int resultado = access.Write("GENERAR_BACKUP", parameters);
            access.Close();

            return resultado;
        }

        public void RestoresBackup(string rutaBak)
        {
            // Obtener la conexión DanDesktop y cambiar BD -> master
            string baseConn = WebConfigurationManager.ConnectionStrings["DanDesktop"].ConnectionString;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(baseConn);
            builder.InitialCatalog = "master";   // 🔥 Conectar a master

            using (SqlConnection conexion = new SqlConnection(builder.ConnectionString))
            {
                conexion.Open();

                // 1️⃣ Matar sesiones activas en MOODFLIX
                string killSessions = @"
                    DECLARE @kill varchar(8000) = '';
                    SELECT @kill = @kill + 'KILL ' + CONVERT(varchar(5), session_id) + ';'
                    FROM sys.dm_exec_sessions
                    WHERE database_id = DB_ID('MOODFLIX');

                    EXEC(@kill);
                ";

                SqlCommand cmdKill = new SqlCommand(killSessions, conexion);
                cmdKill.ExecuteNonQuery();

                // 2️⃣ Poner la base en SINGLE_USER
                SqlCommand cmdSingle = new SqlCommand(
                    "ALTER DATABASE MOODFLIX SET SINGLE_USER WITH ROLLBACK IMMEDIATE;",
                    conexion
                );
                cmdSingle.ExecuteNonQuery();

                // 3️⃣ Restaurar desde archivo .bak
                SqlCommand cmdRestore = new SqlCommand(
                @"RESTORE DATABASE MOODFLIX 
                  FROM DISK = @ruta 
                  WITH REPLACE, STATS = 10;",
                  conexion);

                cmdRestore.Parameters.AddWithValue("@ruta", rutaBak);
                cmdRestore.ExecuteNonQuery();

                // 4️⃣ Volver a MULTI_USER
                SqlCommand cmdMulti = new SqlCommand(
                    "ALTER DATABASE MOODFLIX SET MULTI_USER;",
                    conexion
                );
                cmdMulti.ExecuteNonQuery();

                conexion.Close();
            }
        }
    }
}
