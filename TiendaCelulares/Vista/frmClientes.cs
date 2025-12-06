using System;
using System.Windows.Forms;
using TiendaCelulares.Datos;
using TiendaCelulares.Modelo;

namespace TiendaCelulares.Vista
{
    public partial class frmClientes : Form
    {
        ClienteDatos datos = new ClienteDatos();

        public frmClientes()
        {
            InitializeComponent();
        }

        private void frmClientes_Load(object sender, EventArgs e)
        {
            CargarTabla();
        }

        private void CargarTabla()
        {
            try
            {
                dgvClientes.DataSource = datos.ObtenerTodos();
                dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // validacion basica
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Escribe un nombre.");
                return;
            }

            Cliente c = new Cliente();
            c.Nombre = txtNombre.Text;
            c.Telefono = txtTelefono.Text;
            c.Direccion = txtDireccion.Text;

            try
            {
                // si el label ID es 0 o vacio, es un cliente NUEVO
                if (lblId.Text == "0" || string.IsNullOrEmpty(lblId.Text))
                {
                    if (datos.Guardar(c))
                    {
                        MessageBox.Show("Cliente guardado.");
                        Limpiar();
                        CargarTabla();
                    }
                }
                else
                {
                    // si tiene ID, es edicion
                    c.IdCliente = Convert.ToInt32(lblId.Text);
                    if (datos.Editar(c))
                    {
                        MessageBox.Show("Cliente editado.");
                        Limpiar();
                        CargarTabla();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (lblId.Text == "0" || string.IsNullOrEmpty(lblId.Text))
            {
                MessageBox.Show("Selecciona un cliente de la tabla primero.");
                return;
            }

            if (MessageBox.Show("¿Eliminar cliente?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(lblId.Text);
                    if (datos.Eliminar(id))
                    {
                        Limpiar();
                        CargarTabla();
                        MessageBox.Show("Eliminado.");
                    }
                }
                catch (Exception ex) { MessageBox.Show("No se puede eliminar (quizás tiene ventas)." + ex.Message); }
            }
        }

        
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            txtNombre.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            lblId.Text = "0";
            btnGuardar.Text = "Guardar"; // Resetear texto del botón
        }

        // Para pasar datos a las cajas 
        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                lblId.Text = dgvClientes.CurrentRow.Cells["IdCliente"].Value.ToString();
                txtNombre.Text = dgvClientes.CurrentRow.Cells["Nombre"].Value.ToString();

                // verificaciones seguras
                var tel = dgvClientes.CurrentRow.Cells["Telefono"].Value;
                txtTelefono.Text = (tel != null) ? tel.ToString() : "";

                var dir = dgvClientes.CurrentRow.Cells["Direccion"].Value;
                txtDireccion.Text = (dir != null) ? dir.ToString() : "";

                btnGuardar.Text = "Actualizar"; // cambiar texto para que sepa que va a editar
            }
        }
    }
}