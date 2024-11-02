using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace PracticalWork_16
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Valera\source\repos\PracticalWork_16\PracticalWork_16\db\MSAccessDB.accdb

        public SqlConnection sqlConnection;
        SqlDataAdapter sqlDataAdapter;

        public OleDbConnection oleDbConnection;
        string oleDbStrCon;
        OleDbDataAdapter oleDbDataAdapter;

        DataTable dataTable;
        DataRowView dataRowView;

        public MainWindow()
        {
            InitializeComponent();

            Preparing();
        }

       public void Preparing()
        {
            dataTable = new DataTable();

            SqlDBConnection();
            AccessBDConnection();

            sqlDataAdapter.Fill(dataTable);
            GridView.DataContext = dataTable.DefaultView;
        }

        /// <summary>
        /// Начало редактирования 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GVCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            dataRowView = (DataRowView)GridView.SelectedItem;
            dataRowView.BeginEdit();
        }

        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GVCurrentCellChanged(object sender, EventArgs e)
        {
            if (dataRowView == null) return;
            dataRowView.EndEdit();
            sqlDataAdapter.Update(dataTable);
        }

        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemDeleteClick(object sender, RoutedEventArgs e)
        {
            if(GridView.SelectedItem != null)
            {
                dataRowView = (DataRowView)GridView.SelectedItem;
                dataRowView.Row.Delete();
                sqlDataAdapter.Update(dataTable);
            }
        }

        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemAddClick(object sender, RoutedEventArgs e)
        {
            DataRow r = dataTable.NewRow();
            AddWindow add = new AddWindow(r);
            add.ShowDialog();


            if (add.DialogResult.Value)
            {
                dataTable.Rows.Add(r);
                sqlDataAdapter.Update(dataTable);
            }
        }

        private void MenuItemShowClick(object sender, RoutedEventArgs e)
        {
            if (GridView.SelectedItem != null) 
            {
                dataRowView = (DataRowView)GridView.SelectedItem;
            }
            
            new AllOrders(dataRowView[5].ToString() , oleDbDataAdapter, oleDbConnection).ShowDialog();
        }

        #region Подключение баз данных

        /// <summary>
        /// Подключение базы данных SQL
        /// </summary>
        private void SqlDBConnection()
        {
            var sqlStrCon = new SqlConnectionStringBuilder()
            {
                DataSource = @"(LocalDB)\MSSQLLocalDB",
                AttachDBFilename = @"C:\Users\Valera\source\repos\PracticalWork_16\PracticalWork_16\db\MSSQLLocalDemo.mdf",
                InitialCatalog = "MSSQLLocalDemo",
                IntegratedSecurity = true,
                Pooling = false
            };

            sqlConnection = new SqlConnection() { ConnectionString = sqlStrCon.ConnectionString };
            sqlDataAdapter = new SqlDataAdapter();

            #region Select

            var sql = @"SELECT * FROM Customers Order by Customers.Id";

            sqlDataAdapter.SelectCommand = new SqlCommand(sql, sqlConnection);

            #endregion

            #region Insert

            sql = @"INSERT INTO Customers (Surname,  Name,  Patronymic, PhoneNumber, Mail) 
                                 VALUES (@Surname,  @Name,  @Patronymic, @PhoneNumber, @Mail); 
                     SET @Id = @@IDENTITY;";

            sqlDataAdapter.InsertCommand = new SqlCommand(sql, sqlConnection);

            sqlDataAdapter.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id").Direction = ParameterDirection.Output;
            sqlDataAdapter.InsertCommand.Parameters.Add("@Surname", SqlDbType.NVarChar, 50, "Surname");
            sqlDataAdapter.InsertCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 50, "Name");
            sqlDataAdapter.InsertCommand.Parameters.Add("@Patronymic", SqlDbType.NVarChar, 50, "Patronymic");
            sqlDataAdapter.InsertCommand.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, 50, "PhoneNumber");
            sqlDataAdapter.InsertCommand.Parameters.Add("@Mail", SqlDbType.NVarChar, 50, "Mail");

            #endregion

            #region Update

            sql = @"UPDATE Customers SET 
                           Surname = @Surname,
                           Name = @Name,
                           Patronymic = @Patronymic,
                           PhoneNumber = @PhoneNumber,
                           Mail = @Mail
                    WHERE Id = @Id";

            sqlDataAdapter.UpdateCommand = new SqlCommand(sql, sqlConnection);
            sqlDataAdapter.UpdateCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id").SourceVersion = DataRowVersion.Original;
            sqlDataAdapter.UpdateCommand.Parameters.Add("@Surname", SqlDbType.NVarChar, 50, "Surname");
            sqlDataAdapter.UpdateCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 50, "Name");
            sqlDataAdapter.UpdateCommand.Parameters.Add("@Patronymic", SqlDbType.NVarChar, 50, "Patronymic");
            sqlDataAdapter.UpdateCommand.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, 50, "PhoneNumber");
            sqlDataAdapter.UpdateCommand.Parameters.Add("@Mail", SqlDbType.NVarChar, 50, "Mail");

            #endregion

            #region Delete

            sql = "DELETE FROM Customers WHERE Id = @Id";

            sqlDataAdapter.DeleteCommand = new SqlCommand(sql, sqlConnection);
            sqlDataAdapter.DeleteCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id");

            #endregion
        }

        /// <summary>
        /// Подключение базы данных Access
        /// </summary>
        private void AccessBDConnection()
        {
            oleDbStrCon = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=C:\Users\Valera\source\repos\PracticalWork_16\PracticalWork_16\db\MSAccessDB.accdb; User ID=admin;"
            + "Jet OLEDB:Encrypt Database=True;Jet OLEDB:Database Password=test123;Persist Security Info=True;";

            oleDbConnection = new OleDbConnection(oleDbStrCon);
            oleDbDataAdapter = new OleDbDataAdapter();

            #region Insert

            var query = @"INSERT INTO Table_1 (ProductName,  ProductCode, Mail) 
                                 VALUES (@ProductName, @ProductCode, @Mail); 
                     SET @Id = @@IDENTITY;";

            oleDbDataAdapter.InsertCommand = new OleDbCommand(query, oleDbConnection);
            oleDbDataAdapter.InsertCommand.Parameters.Add("@Id", OleDbType.Integer, 4, "Id").Direction = ParameterDirection.Output;
            oleDbDataAdapter.InsertCommand.Parameters.Add("@ProductName", OleDbType.VarWChar, 50, "ProductName");
            oleDbDataAdapter.InsertCommand.Parameters.Add("@ProductCode", OleDbType.VarWChar, 50, "ProductCode");
            oleDbDataAdapter.InsertCommand.Parameters.Add("@Mail", OleDbType.VarWChar, 50, "Mail");

            #endregion

            #region Update

            query = @"UPDATE Table_1 SET 
                           ProductName = @ProductName,
                           ProductCode = @ProductCode,
                           Mail = @Mail
                    WHERE Id = @Id";

            oleDbDataAdapter.UpdateCommand = new OleDbCommand(query, oleDbConnection);
            oleDbDataAdapter.UpdateCommand.Parameters.Add("@Id", OleDbType.Integer, 0, "Id").SourceVersion = DataRowVersion.Original;
            oleDbDataAdapter.UpdateCommand.Parameters.Add("@ProductName", OleDbType.VarWChar, 50, "ProductName");
            oleDbDataAdapter.UpdateCommand.Parameters.Add("@ProductCode", OleDbType.VarWChar, 50, "ProductCode");
            oleDbDataAdapter.UpdateCommand.Parameters.Add("@Mail", OleDbType.VarWChar, 50, "Mail");
            #endregion

            #region Delete

            query = "DELETE FROM Table_1 WHERE Id = @Id";

            oleDbDataAdapter.DeleteCommand = new OleDbCommand(query, oleDbConnection);
            oleDbDataAdapter.DeleteCommand.Parameters.Add("@Id", OleDbType.Integer, 4, "Id");

            #endregion
        }

        #endregion
    }
}
