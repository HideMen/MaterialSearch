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
            if (!File.Exists(dbName))
                SQLiteConnection.CreateFile(dbName);

            try
            {
                BDconnect = new SQLiteConnection("Data Source=" + dbName + ";Version=3;");
                BDconnect.Open();
                CommandBD.Connection = BDconnect;

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
            if (!File.Exists(dbName))
                CreateBD();

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
            if (BDconnect.State != ConnectionState.Open)
            {
                MessageBox.Show("Open connection with database");
                return;
            }

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

            if (BDconnect.State != ConnectionState.Open)
            {
                MessageBox.Show("Open connection with database");
                return null;
            }

            try
            {
                sqlQuery = "SELECT * FROM Materials WHERE Materials.width LIKE '%" + width + "'";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, BDconnect);
                adapter.Fill(dTable);

                if (dTable.Rows.Count > 0)
                {
                    return dTable;
                }
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