using Dapper;
using MAMBrowser.Helpers;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DAL
{
    public class Repository<T> where T : class, new()
    {
        private string _strCon =  "";
        public Repository()
        {
            _strCon = SystemConfig.AppSettings.ConnectionString;
        }
        public long Insert(string insertQuery, object entities)
        {
            return 0;
        }
        public int Update(string updateQuery, object entities)
        {
            using (OracleConnection con = new OracleConnection(_strCon))
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
     
        public int Delete(T entity)
        {
            return 0;
        }
        public T Get(string query, object param, Func<dynamic, T> resultMapping)
        {
            using (OracleConnection con = new OracleConnection(_strCon))
            {
                con.Open();
                var queryData = con.Query(query, param).Select<dynamic, T>(resultMapping).FirstOrDefault();
                return queryData;
            }
        }
        public IList<T> Select(string query, object param,Func<dynamic, T> resultMapping)
        {
            using (OracleConnection con = new OracleConnection(_strCon))
            {
                con.Open();
                var queryData = con.Query(query, param).Select<dynamic,T>(resultMapping);
                return queryData.ToList();
            }
        }
    }
}
