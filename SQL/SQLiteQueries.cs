using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Новая_попытка.SQL
{
    class SQLiteQueries
    {
        public DbFacadeSQLite _sqlt;

        //Подключение к EventLog
        public SQLiteQueries(string dbName)
        {
            _sqlt = new DbFacadeSQLite(dbName);
        }

    }
}
