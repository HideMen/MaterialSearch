using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaterialSearch
{
    public partial class MainWindow : Window
    {
        MaterialsDB md;
        public MainWindow()
        {
            InitializeComponent();
            //Инициализируем Базу данных
            md = new MaterialsDB();
            md.initBD();
            md.bdConnect();
        }
        //Поиск в БД
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            
            DataTable dTable = md.bdSearch(widthText.Text, lengthText.Text);
            if (dTable == null)
                return;
            ListBDview.Items.Clear();

            for (int i = 0; i < dTable.Rows.Count; i++)
            {
                //Добавляем первый и второй эллемент из БД в наш ListView в цикле пока данные не закончатся
                ListViewItem item = new ListViewItem
                {
                    Width = dTable.Rows[i].ItemArray[1].ToString(),
                    Length = Int32.Parse(dTable.Rows[i].ItemArray[2].ToString()),
                };
                ListBDview.Items.Add(item);

            }
        }
        //добавление в БД
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            md.bdAddMaterials(widthAddText.Text, lengthAddText.Text);
        }
    }
    
    public class ListViewItem
    {
        public string Width { get; set; }
        public int Length { get; set; }
    }
}
