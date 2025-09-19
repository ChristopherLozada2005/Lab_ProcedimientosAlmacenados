using Microsoft.Data.SqlClient;
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

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para Ejercicio9.xaml
    /// </summary>
    public partial class Ejercicio9 : Window
    {
        private string connectionString =
            "Data Source=LAB1502-005\\SQLEXPRESS;" +
            "Initial Catalog=Neptuno;" +
            "TrustServerCertificate=True";

        public Ejercicio9()
        {
            InitializeComponent();
        }
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            List<Proveedor> proveedores = new List<Proveedor>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand("USP_BuscarProveedores", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@nombreContacto",
                            string.IsNullOrWhiteSpace(txtNombreContacto.Text) ? (object)DBNull.Value : txtNombreContacto.Text);
                        sqlCommand.Parameters.AddWithValue("@ciudad",
                            string.IsNullOrWhiteSpace(txtCiudad.Text) ? (object)DBNull.Value : txtCiudad.Text);

                        SqlDataReader reader = sqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            proveedores.Add(new Proveedor
                            {
                                IdProveedor = Convert.ToInt32(reader["idProveedor"]),
                                NombreCompañia = reader["nombreCompañia"].ToString(),
                                NombreContacto = reader["nombreContacto"].ToString(),
                                CargoContacto = reader["cargoContacto"].ToString(),
                                Direccion = reader["direccion"].ToString(),
                                Ciudad = reader["ciudad"].ToString(),
                                Region = reader["region"].ToString(),
                                CodPostal = reader["codPostal"].ToString(),
                                Pais = reader["pais"].ToString(),
                                Telefono = reader["telefono"].ToString(),
                                Fax = reader["fax"].ToString(),
                                PaginaPrincipal = reader["paginaprincipal"].ToString()
                            });
                        }
                    }

                    sqlConnection.Close();
                }

                dgProveedores.ItemsSource = proveedores;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar proveedores: " + ex.Message);
            }
        }
    }
}
