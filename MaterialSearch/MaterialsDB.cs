using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Windows;
using System.Data;

namespace MaterialSearch
{
    class MaterialsDB
    {
        private String dbName;
        private SQLiteConnection BDconnect;
        private SQLiteCommand CommandBD;

        public void initBD()
        {
            BDconnect = new SQLiteConnection();
            CommandBD = new SQLiteCommand();
            dbName = "sample.sqlite";
        }
        public void CreateBD()
        {
            //проверяем наличие БД и если не находим создаем
            if (!File.Exists(dbName))
                SQLiteConnection.CreateFile(dbName);

            try
            {
                BDconnect = new SQLiteConnection("Data Source=" + dbName + ";Version=3;");
                BDconnect.Open();
                CommandBD.Connection = BDconnect;
                //создаем Таблицу Materials из двух полей width и length
                CommandBD.CommandText = "CREATE TABLE IF NOT EXISTS Materials (id INTEGER PRIMARY KEY AUTOINCREMENT, width TEXT, length TEXT)";
                CommandBD.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public void bdConnect()
        {
            //проверяем наличие БД и если нет вызываем функцию создания.
            if (!File.Exists(dbName))
                CreateBD();
            //подключаемся к БД
            try
            {
                BDconnect = new SQLiteConnection("Data Source=" + dbName + ";Version=3;");
                BDconnect.Open();
                CommandBD.Connection = BDconnect;

            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        public void bdAddMaterials(string width, string length)
        {
            //проверяем соединение с БД
            if (BDconnect.State != ConnectionState.Open)
            {
                MessageBox.Show("База данных не подключена");
                return;
            }
            //отправляем команду на добавление новой записи
            try
            {
                CommandBD.CommandText = "INSERT INTO Materials ('width', 'length') values ('" +
                        width + "' , '" +
                        length + "')";

                CommandBD.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public DataTable bdSearch(string width, string length)
        {
           
            DataTable dTable = new DataTable();
            String sqlQuery;
            //проверяем соединение с БД
            if (BDconnect.State != ConnectionState.Open)
            {
                MessageBox.Show("База данных не подключена");
                return null;
            }

            try
            {
                //отправляем запрос в БД для поиска строки с указанной шириной.
                sqlQuery = "SELECT * FROM Materials WHERE Materials.width LIKE '%" + width + "'";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, BDconnect);
                adapter.Fill(dTable);

                if (dTable.Rows.Count > 0)
                    return dTable; //Если все успешно возвращаем обьект с данными
                else
                    MessageBox.Show("Не найдено"); return null;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return null;
        }

    }
}