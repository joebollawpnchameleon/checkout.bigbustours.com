
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace bigbus.checkout.data.PlainQueries
{
    public interface IQueryFunctions
    {
        DataSet DataSetFromStoredProcedure(string storedProcedureName, IEnumerable<SqlParameter> parameters);

        DataTable DataTableFromStoredProcedure(string storedProcedureName, IEnumerable<SqlParameter> parameters);

        DataRow DataRowFromStoredProcedure(string storedProcedureName, IEnumerable<SqlParameter> parameters);

        DataRow DataRowFromStoredProcedure(string storedProcedureName, string paramName, string paramValue);
    }
}
