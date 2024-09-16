using System.Data;
using Microsoft.Data.SqlClient;

namespace Core
{
    public static class SqlHelper
    {
        public const int TIMEOUT = 30; // in seconds

        public static DataTable Read(string connectionString, string sqlStatement)
        {
            return Read(connectionString, sqlStatement, new List<SqlParameter>());
        }

        public static DataTable Read(string connectionString, string sqlStatement, List<ObjectParameter> parameters)
        {
            return Read(connectionString, sqlStatement, parameters.ToSqlParameterList());
        }

        public static DataTable Read(string connectionString, string sqlStatement, List<SqlParameter> parameters)
        {
            DataSet ds = new();

            SqlConnection sqlConnection = new(connectionString);
            using (SqlConnection cnn = sqlConnection)
            {
                SqlCommand cmd = GetSqlCommand(cnn, sqlStatement, parameters);
                SqlDataAdapter da = new(cmd);
                cnn.Open();
                da.Fill(ds);
                ds.Dispose();
                cmd.Parameters.Clear();
                cnn.Close();
            }

            return ds.Tables[0];
        }

        public static int Insert(string connectionString, string sqlStatement, List<SqlParameter> parameters)
        {
            SqlConnection cnn = new(connectionString);

            cnn.Open();

            sqlStatement += ";SELECT CAST(scope_identity() AS int)";

            SqlCommand cmd = GetSqlCommand(cnn, sqlStatement, parameters);

            int newId = (int)cmd.ExecuteScalar();

            cnn.Close();

            return newId;
        }

        public static int Execute(string connectionString, string sqlStatement)
        {
            return Execute(connectionString, sqlStatement, new List<SqlParameter>());
        }

        public static int Execute(string connectionString, string sqlStatement, List<ObjectParameter> parameters)
        {
            return Execute(connectionString, sqlStatement, parameters.ToSqlParameterList());
        }

        public static int Execute(string connectionString, string sqlStatement, List<SqlParameter> parameters)
        {
            using SqlConnection cnn = new(connectionString);
            cnn.Open();

            SqlCommand cmd = GetSqlCommand(cnn, sqlStatement, parameters);
            int rowsAffected = cmd.ExecuteNonQuery();

            cnn.Close();

            return rowsAffected;
        }

        public static SqlDataReader ExecuteReader(string connectionString, string sqlStatement, List<SqlParameter> parameters)
        {
            SqlConnection cnn = new(connectionString);
            cnn.Open();

            SqlCommand cmd = GetSqlCommand(cnn, sqlStatement, parameters);
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            return reader;
        }

        private static SqlCommand GetSqlCommand(SqlConnection cnn, string sql, List<SqlParameter> parameters)
        {
            SqlCommand cmd = new(sql, cnn)
            {
                CommandTimeout = TIMEOUT
            };

            foreach (SqlParameter param in parameters)
            {
                _ = new SqlParameter(param.ParameterName, param.Value);

                if (param.Value == null)
                {
                    cmd.Parameters.Add(new SqlParameter(param.ParameterName, DBNull.Value));
                }
                else
                {
                    cmd.Parameters.Add(param);
                }
            }

            return cmd;
        }

        public static SqlParameter[]? ToArray(this List<SqlParameter> parameters)
        {
            if (parameters == null)
                return null;

            if (parameters.Count == 0)
                return null;

            SqlParameter[] sp = new SqlParameter[parameters.Count];

            for (int i = 0; i < parameters.Count; i++)
            {
                sp[i] = parameters[i];
            }

            return sp;
        }

        public static List<SqlParameter> ToSqlParameterList(this List<ObjectParameter> parameters)
        {
            List<SqlParameter> p = [];

            foreach (ObjectParameter o in parameters)
                p.Add(new SqlParameter(o.Name, o.Value));

            return p;
        }

        public static List<string> ToSearchTerms(this string searchString)
        {
            List<string> lst = [];

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.Trim();

                string[] ssArray = searchString.Split(' ');

                foreach (string s in ssArray)
                {
                    if (!String.IsNullOrWhiteSpace(s.Trim()))
                        lst.Add(s.Trim());
                }
            }

            return lst;
        }

        public static DataTable GetPrimaryKeys(string connectionString, string tableName)
        {
            string sql = String.Format("sp_pkeys '{0}'", tableName);

            return Read(connectionString, sql);
        }
    }
}
