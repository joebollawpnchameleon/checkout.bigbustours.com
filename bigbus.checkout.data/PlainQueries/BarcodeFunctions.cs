using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bigbus.checkout.data.PlainQueries
{
    public class BarcodeFunctions : IBarcodeFunctions
    {
        private readonly string _connString;

        public BarcodeFunctions(string connString)
        {
            _connString = connString;
        }

        public DataSet DataSetFromStoredProcedure(string storedProcedureName, IEnumerable<SqlParameter> parameters)
        {           
            var ds = new DataSet();

            using (var conn = new SqlConnection(_connString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(ds);
                    }
                }
            }

            return ds;
        }
    }
}
