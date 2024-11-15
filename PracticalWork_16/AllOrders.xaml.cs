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
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Odbc;

namespace PracticalWork_16
{
    /// <summary>
    /// Логика взаимодействия для AllOrders.xaml
    /// </summary>
    public partial class AllOrders : Window
    {
        private ObservableCollection<Order> orders;
        private Order selectedOrder;

        private string connStr;
        private string mail;

        NewOrderWindow newOrderWindow;

        public AllOrders(string customerMail, string strConnection)
        {
            InitializeComponent();

            newOrderWindow = new NewOrderWindow(this);

            connStr = strConnection;
            mail = customerMail;

            #region Select
            var query = @"SELECT * FROM Table_1";
            #endregion

            orders = new ObservableCollection<Order>();
            
            var command = new OleDbCommand(query);

            using (var conn = new OleDbConnection(connStr))
            {
                command.Connection = conn;
                conn.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Order item = new Order(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));

                        if(item.Mail == customerMail)
                        { 
                            orders.Add(item); 
                        }
                    }
                };
                conn.Close();
            }

            OrdersView.ItemsSource = orders;
        }

        /// <summary>
        /// Открыть страницу добавления заказа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenNewOrder(object sender, RoutedEventArgs e)
        {
            newOrderWindow.ShowDialog();
        }

        /// <summary>
        /// Добавление нового заказа
        /// </summary>
        /// <param name="orderName"></param>
        /// <param name="orderCode"></param>
        public void NewOrder(string orderName, string orderCode)
        {
            #region Insert

            var query = @"INSERT INTO Table_1 (ProductName, ProductCode, CustomerMail) 
                                 VALUES (@ProductName, @ProductCode, @CustomerMail)";

            using (var conn = new OleDbConnection(connStr))
            {
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@ProductName", orderName);
                cmd.Parameters.AddWithValue("@ProductCode", orderCode);
                cmd.Parameters.AddWithValue("@CustomerMail", mail);

                conn.Open();

                cmd.ExecuteNonQuery();

                conn.Close();
            }

            #endregion

            #region AddToList

            query = @"SELECT TOP 1 * FROM Table_1 ORDER BY Id DESC";

            var command = new OleDbCommand(query);

            using (var conn = new OleDbConnection(connStr))
            {
                command.Connection = conn;
                conn.Open();

                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Order item = new Order(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));

                    orders.Add(item);
                }

                conn.Close();
            }

            #endregion
        }


        /// <summary>
        /// Удаление заказа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteOrder(object sender, RoutedEventArgs e)
        {
            Order delete = FindOrderByID(selectedOrder.Id);

            if (delete != null) 
            {
                orders.Remove(delete);

                var query = "DELETE FROM Table_1 WHERE Id = @Id";

                using (var conn = new OleDbConnection(connStr))
                {
                    OleDbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@Id", delete.Id);

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    conn.Close();
                }

            }
        }

        /// <summary>
        /// Поиск заказа по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Order FindOrderByID(int id)
        {
            for (int i = 0; i < orders.Count; i++)
            {
                if (orders[i].Id == id)
                { return orders[i]; }
            }

            return null;
        }

        /// <summary>
        /// Выбор заказа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedItem(object sender, SelectionChangedEventArgs e)
        {
            selectedOrder = OrdersView.SelectedItem as Order;
        }
    }
}
