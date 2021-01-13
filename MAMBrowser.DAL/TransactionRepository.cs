using Dapper;
using MAMBrowser;
using MAMBrowser.DAL;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace MAMBrowser.DAL
{
    public class TransactionRepository
    {
        public static string ConnectionString { get; set; }
        private OracleConnection _con;
        private OracleTransaction _transaction;
        public TransactionRepository()
        {
        }
        public void BeginTransaction()
        {
            _con = new OracleConnection(ConnectionString);
            _con.Open();
            _transaction = _con.BeginTransaction();
        }
        public void CommitTransaction()
        {
            _transaction.Commit();
            _con.Close();
        }
        public void RollbackTransaction()
        {
            _transaction.Rollback();
            _con.Close();
        }
        public P Insert<P>(string query, object param) 
        {
            var dparam = new DynamicParameters(param);
            dparam.Add(name: "KEY", dbType: DbType.Int64, direction: ParameterDirection.Output);
            _con.Execute(query, dparam);
            return dparam.Get<P>("KEY");
        }
        public void Insert(string query, object param) 
        {

            _con.Execute(query, param);
        }

        public int Update(string query, object param) 
        {
            var result = _con.Execute(query, param);
            return result;
        }

        public int Delete(string deleteQuery, object param)
        {
            var result = _con.Execute(deleteQuery, param);
            return result;
        }

    }
}
