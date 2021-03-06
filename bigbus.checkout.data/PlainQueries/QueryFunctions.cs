﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using bigbus.checkout.data.Model;

namespace bigbus.checkout.data.PlainQueries
{
    public class QueryFunctions : IQueryFunctions
    {
        private readonly string _connString;

        public QueryFunctions(string connString)
        {
            _connString = connString;
        }

        public DataSet DataSetFromStoredProcedure(string storedProcedureName, IEnumerable<SqlParameter> parameters)
        {    
            try
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
             catch (Exception ex)
             {
                 return null;
             }
        }
        #region DataTable functions

        public DataTable DataTableFromStoredProcedure(string storedProcedureName, IEnumerable<SqlParameter> parameters)
        {
            SqlConnection conn = null;

            try
            {
                var dt = new DataTable();

                using (conn = new SqlConnection(_connString))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = storedProcedureName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (var parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                        conn.Open();
                        var reader = cmd.ExecuteReader();
                        dt.Load(reader);
                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if(conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public DataTable DataTableFromStoredProcedure(string storedProcedureName, SqlParameter param)
        {
            SqlConnection conn = null;

            try
            {
                var dt = new DataTable();

                using (conn = new SqlConnection(_connString))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = storedProcedureName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(param);

                        conn.Open();
                        var reader = cmd.ExecuteReader();
                        dt.Load(reader);
                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public DataTable DataTableFromStoredProcedure(string storedProcedureName, string paramName, string paramValue)
        {
            SqlConnection conn = null;

            try
            {
                var dt = new DataTable();

                using (conn = new SqlConnection(_connString))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = storedProcedureName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter(paramName, paramValue));

                        conn.Open();
                        var reader = cmd.ExecuteReader();
                        dt.Load(reader);
                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public DataTable DataTableFromStoredProcedure(string storedProcedureName)
        {
            SqlConnection conn = null;

            try
            {
                var dt = new DataTable();

                using (conn = new SqlConnection(_connString))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = storedProcedureName;
                        cmd.CommandType = CommandType.StoredProcedure;                       
                        conn.Open();
                        var reader = cmd.ExecuteReader();
                        dt.Load(reader);
                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        #endregion

        #region DataRowfunctions

        public DataRow DataRowFromStoredProcedure(string storedProcedureName, IEnumerable<SqlParameter> parameters)
        {
            SqlConnection conn = null;

            try
            {
                var dt = new DataTable();

                using (conn = new SqlConnection(_connString))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = storedProcedureName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (var parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                        conn.Open();
                        var reader = cmd.ExecuteReader();
                        dt.Load(reader);
                    }
                }

                return dt.Rows[0];
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public DataRow DataRowFromStoredProcedure(string storedProcedureName, string paramName, string paramValue)
        {
            SqlConnection conn = null;

            try
            {
                var dt = new DataTable();

                using (conn = new SqlConnection(_connString))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = storedProcedureName;
                        cmd.CommandType = CommandType.StoredProcedure;
                       
                        cmd.Parameters.Add(new SqlParameter(paramName, paramValue));
                        conn.Open();
                        var reader = cmd.ExecuteReader();
                        dt.Load(reader);
                    }
                }

                return dt.Rows[0];
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        #endregion
    }
}
