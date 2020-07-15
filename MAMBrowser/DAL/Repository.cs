using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.DAL
{
    public class Repository<T> where T : class, new()
    {
        private string strCon =  "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=dev.adsoft.kr)(PORT=1523)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));User Id=MIROS;Password=MIROS;";
        public Repository()
        {

        }
        public long Insert(T entity)
        {
            return 0;
        }
        public long Insert(IList<T> entity)
        {
            return 0;
        }
        public int Update(T entity)
        {
            return 0;
        }
        public long Update(IList<T> entity)
        {
            return 0;
        }
        public int Delete(T entity)
        {
            return 0;
        }
        public IList<T> Select(string query, object param,Func<dynamic, T> resultMapping)
        {
            using (OracleConnection con = new OracleConnection(strCon))
            {
                con.Open();
                var queryData = con.Query(query, param).Select<dynamic,T>(resultMapping);
                return queryData.ToList();
            }
        }
    }
}
