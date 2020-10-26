using Dapper;
using MAMBrowser.Helpers;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DAL
{
    public class Repository 
    {
        public static string ConnectionString { get; set; }
        public Repository()
        {
        }
        public void Insert(string insertQuery, object entities)
        {
            
            using (OracleConnection con = new OracleConnection(ConnectionString))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        con.Execute(insertQuery, entities);
                        transaction.Commit();//고유키가 db에서 채번되고, 문자형식이 포함될 수 있음.
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public int Update(string updateQuery, object entities)
        {
            using (OracleConnection con = new OracleConnection(ConnectionString))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        var result = con.Execute(updateQuery, entities);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return -1;
        }
     
        public int Delete(string deleteQuery, object param)
        {
            using (OracleConnection con = new OracleConnection(ConnectionString))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        var result = con.Execute(deleteQuery, param);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return -1;
        }
        public T Get<T>(string query, object param, Func<dynamic, T> resultMapping)
        {
            using (OracleConnection con = new OracleConnection(ConnectionString))
            {
                con.Open();
                var queryData = con.Query(query, param).Select<dynamic, T>(resultMapping).FirstOrDefault();
                return queryData;
            }
        }
        public IList<T> Select<T>(string query, object param,Func<dynamic, T> resultMapping)
        {
            using (OracleConnection con = new OracleConnection(ConnectionString))
            {
                con.Open();
                var queryData = con.Query(query, param).Select<dynamic,T>(resultMapping);
                return queryData.ToList();
            }
        }
    }
}
