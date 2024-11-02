using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Collections;

namespace PracticalWork_16
{
    /// <summary>
    /// Логика взаимодействия для AllOrders.xaml
    /// </summary>
    public partial class AllOrders : Window
    {
        DataTable dataTable;

        public AllOrders(string customerMail, OleDbDataAdapter oleDbDataAdapter, OleDbConnection oleDbConnection)
        {
            InitializeComponent();

            dataTable = new DataTable();

            #region Select

            var query = $@"SELECT * FROM Table_1 WHERE Table_1.Mail = {customerMail} ";

            oleDbDataAdapter.SelectCommand = new OleDbCommand(query, oleDbConnection);

            #endregion

            oleDbDataAdapter.Fill(dataTable);
            GridView.DataContext = dataTable.DefaultView;
        }

        private void OpenNewOrder(object sender, RoutedEventArgs e)
        {

        }
    }
}
