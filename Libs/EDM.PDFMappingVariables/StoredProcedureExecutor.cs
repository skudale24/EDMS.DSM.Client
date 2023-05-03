using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDM.PDFMappingVariables
{
    using System.Data;
    using System.Data.SqlClient;

    namespace YourNamespace
    {
        public class StoredProcedureExecutor
        {
            public static void ExecuteStoredProcedure(string connectionString, string storedProcedureName, SqlParameter[] parameters)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parameters);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }

            public static DataSet ExecuteStoredProcedureAsDataSet(string connectionString, string storedProcedureName, SqlParameter[] parameters)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parameters);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataSet dataSet = new DataSet();
                            adapter.Fill(dataSet);
                            return dataSet;
                        }
                    }
                }
            }
        }
    }

}
