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
    public partial class EjercicioDetallesPedidos : Window
    {
        private string connectionString =
            "Data Source=LAB1502-005\\SQLEXPRESS;" +
            "Initial Catalog=Neptuno;" +
            "User ID=userTecsup;" +
            "Password=123456;" +
            "TrustServerCertificate=True";

        public EjercicioDetallesPedidos()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (dpInicio.SelectedDate == null || dpFin.SelectedDate == null)
            {
                MessageBox.Show("Debes seleccionar ambas fechas.");
                return;
            }

            List<DetallePedido> detalles = new List<DetallePedido>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("USP_ListarDetallesPedidosPorFechas", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FechaInicio", dpInicio.SelectedDate.Value);
                        cmd.Parameters.AddWithValue("@FechaFin", dpFin.SelectedDate.Value);

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            detalles.Add(new DetallePedido
                            {
                                IdPedido = Convert.ToInt32(reader["idpedido"]),
                                IdProducto = Convert.ToInt32(reader["idproducto"]),
                                PrecioUnidad = Convert.ToDecimal(reader["preciounidad"]),
                                Cantidad = Convert.ToInt16(reader["cantidad"]),
                                Descuento = Convert.ToSingle(reader["descuento"]),
                                IdCliente = reader["idcliente"].ToString(),
                                IdEmpleado = Convert.ToInt32(reader["idempleado"]),
                                FechaPedido = Convert.ToDateTime(reader["fechapedido"]),
                                FechaEntrega = reader["fechaentrega"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["fechaentrega"]),
                                FechaEnvio = reader["fechaenvio"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["fechaenvio"]),
                                IdTransportista = Convert.ToInt32(reader["idtransportista"])
                            });
                        }
                    }
                }

                dgDetallesPedidos.ItemsSource = detalles;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar detalles: " + ex.Message);
            }
        }
    }
}