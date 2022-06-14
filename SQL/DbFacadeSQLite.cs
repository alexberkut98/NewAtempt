using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Новая_попытка.SQL
{
    public class DbFacadeSQLite
    {
        private string lastError = string.Empty; //сообщение последней ошибки
        private string lastQuery = string.Empty; //последний выполненный запрос    
        #region Настройки
        /// <summary>
        /// Путь к файлу базы данных.
        /// </summary>
        string ConnectionString = string.Empty;
        public SQLiteConnection connect;
        private SQLiteTransaction transaction;
        private SQLiteConnectionStringBuilder csb = new SQLiteConnectionStringBuilder();
        public SQLiteCommand command = new SQLiteCommand();
        #endregion

        /// <summary>
        /// Инициализация подключения.
        /// </summary>
        /// <param name="fileName">Путь к файлу базы данных</param>
        public DbFacadeSQLite(string fileName)
        {
            this.csb.DataSource = fileName;
            this.csb.Version = 3;
            this.csb.UseUTF16Encoding = true;
            //this.csb.Add("New", true);    
            connect = new SQLiteConnection(this.csb.ConnectionString);
        }

        /// <summary>
        /// Получение данных по выбранным полям (колонкам).
        /// </summary>
        /// <param name="tablename">Имя таблицы</param>
        /// <param name="columns">Строка колонок через запятую, которые необходимо получить</param>
        /// <param name="where">Строка условий, начинающихся с WHERE</param>
        /// <param name="etc">Остальные параметры: сортировка, группировка и т.д.</param>
        /// <returns>Таблица с данными</returns>
    }
}
