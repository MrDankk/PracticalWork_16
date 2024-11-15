using System;
using System.Collections.Generic;
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

namespace PracticalWork_16
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class NewOrderWindow : Window
    {
        AllOrders allOrders;
        public NewOrderWindow(AllOrders allOrders) 
        { 
            InitializeComponent();

            this.allOrders = allOrders;
        }

        /// <summary>
        /// Передача информации о добавляемом товаре
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendOrderInformation(object sender, RoutedEventArgs e)
        {
            if (CheckInput())
            {
                allOrders.NewOrder(txtOrderName.Text, txtOrderCode.Text);

                this.Close();
            }
        }

        /// <summary>
        /// Проверка полей ввода
        /// </summary>
        /// <returns></returns>
        private bool CheckInput()
        {
            bool check = true;

            if (txtOrderName.Text.Trim() == "")
            {
                txtOrderName.Background = Brushes.Red;
                check = false;
            }
            else
            {
                txtOrderName.Background = Brushes.White;
            }

            if(txtOrderCode.Text.Trim() == "")
            {
                txtOrderCode.Background = Brushes.Red;
                check = false;
            }
            else
            {
                txtOrderCode.Background = Brushes.White;
            }

            return check;
        }

        /// <summary>
        /// Закрыть страницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosePage(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
