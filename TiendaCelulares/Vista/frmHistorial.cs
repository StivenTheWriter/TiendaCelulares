using System;
using System.Data; // Necesario para DataTable
using System.Drawing; // Necesario para tamaños visuales
using System.Windows.Forms;
using TiendaCelulares.Datos;

namespace TiendaCelulares.Vista
{
    public partial class frmHistorial : Form
    {
        VentasDatos ventasDatos = new VentasDatos();

        public frmHistorial()
        {
            InitializeComponent();
        }

       
        private void frmHistorial_Load(object sender, EventArgs e)
        {
            // en lugar de cargar todo el historial le configure las fechas para hoy
            
            // inicio del dia
            dateTimePicker1.Value = DateTime.Today;

            //  hasta ahora mismo
            dateTimePicker2.Value = DateTime.Now;

            // busca automaticamente lo de esas fechas
            btnBuscar_Click(sender, e);
        }

       
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime desde = dateTimePicker1.Value;
                DateTime hasta = dateTimePicker2.Value;

                DataTable resultados = ventasDatos.ListarVentas(desde, hasta);

                dgvHistorial.DataSource = resultados;

                // ajustes visuales 
                if (dgvHistorial.Columns.Contains("Total"))
                {
                    dgvHistorial.Columns["Total"].DefaultCellStyle.Format = "N2"; // formato moneda
                }
                dgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar: " + ex.Message);
            }
        }

       
        private void btnDetalles_Click(object sender, EventArgs e)
        {
            // valida que haya una fila seleccionada
            if (dgvHistorial.SelectedRows.Count > 0)
            {
                // obtinee el ID de la venta de la fila seleccionada
                int idVenta = Convert.ToInt32(dgvHistorial.SelectedRows[0].Cells["Id"].Value);

                MostrarDetalleVenta(idVenta);
            }
            else
            {
                MessageBox.Show("Selecciona una venta de la lista para ver sus detalles.");
            }
        }

        
        // Eesto crea una ventanita flotante con los productos sin que yo la diseñe
        private void MostrarDetalleVenta(int idVenta)
        {
            try
            {
                DataTable detalles = ventasDatos.ObtenerDetalleVenta(idVenta);

                // creo un formulario temporal en codigo
                Form formDetalle = new Form();
                formDetalle.Text = "Detalles de la Venta #" + idVenta;
                formDetalle.Size = new Size(500, 300);
                formDetalle.StartPosition = FormStartPosition.CenterParent;

                // creo una tabla para mostrar los datos
                DataGridView gridDetalle = new DataGridView();
                gridDetalle.Dock = DockStyle.Fill;
                gridDetalle.DataSource = detalles;
                gridDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                gridDetalle.ReadOnly = true;
                gridDetalle.AllowUserToAddRows = false;

                formDetalle.Controls.Add(gridDetalle);
                formDetalle.ShowDialog(); // muestra la ventanita
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los detalles: " + ex.Message);
            }
        }

        
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // eventos vacíos que no se usan 
        private void dgvHistorial_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e) { }
    }
}