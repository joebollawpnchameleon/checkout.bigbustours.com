
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace bigbus.checkout.data.PlainQueries
{
    public interface IBarcodeFunctions
    {
        DataSet DataSetFromStoredProcedure(string storedProcedureName, IEnumerable<SqlParameter> parameters);
    }
}
