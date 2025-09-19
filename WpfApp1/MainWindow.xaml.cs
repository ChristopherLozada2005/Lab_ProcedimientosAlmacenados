using Microsoft.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        // Conexión con SQL Server Authentication
        private string connectionString =
            "Data Source=LAB1502-005\\SQLEXPRESS;" +
            "Initial Catalog=Neptuno;" +
            
            "TrustServerCertificate=True";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            List<Producto> productos = new List<Producto>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand("USP_ListarProductos", sqlConnection))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlDataReader reader = sqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            productos.Add(new Producto
                            {
                                IdProducto = Convert.ToInt32(reader["idproducto"]),
                                NombreProducto = reader["nombreProducto"].ToString(),
                                IdProveedor = Convert.ToInt32(reader["idProveedor"]),
                                IdCategoria = Convert.ToInt32(reader["idCategoria"]),
                                CantidadPorUnidad = reader["cantidadPorUnidad"].ToString(),
                                PrecioUnidad = Convert.ToDecimal(reader["precioUnidad"]),
                                UnidadesEnExistencia = Convert.ToInt16(reader["unidadesEnExistencia"]),
                                UnidadesEnPedido = Convert.ToInt16(reader["unidadesEnPedido"]),
                                NivelNuevoPedido = Convert.ToInt16(reader["nivelNuevoPedido"]),
                                Suspendido = Convert.ToBoolean(reader["suspendido"]),
                                CategoriaProducto = reader["categoriaProducto"].ToString()
                            });
                        }
                    }

                    sqlConnection.Close();
                }

                dgProductos.ItemsSource = productos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
            }
        }
    }
}