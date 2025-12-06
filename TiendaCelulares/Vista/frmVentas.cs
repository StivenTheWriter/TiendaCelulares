using System;
using System.Collections.Generic; // Necesario para List<>
using System.Data;
using System.Windows.Forms;
using TiendaCelulares.Datos;
using TiendaCelulares.Modelo;

namespace TiendaCelulares.Vista
{
    public partial class frmVentas : Form
    {
        DataTable tablaCarrito = new DataTable();
        decimal totalVenta = 0;

        public frmVentas()
        {
            InitializeComponent();
        }

        private void frmVentas_Load(object sender, EventArgs e)
        {
            ConfigurarCarrito();
            CargarDatos();
            LimpiarInterfaz();
        }

        private void ConfigurarCarrito()
        {
            // estructura de la tabla visual
            tablaCarrito.Columns.Add("IdProducto", typeof(int));
            tablaCarrito.Columns.Add("Producto", typeof(string)); 
            tablaCarrito.Columns.Add("Precio", typeof(decimal));
            tablaCarrito.Columns.Add("Cantidad", typeof(int));
            tablaCarrito.Columns.Add("Subtotal", typeof(decimal));

            dgvDetalles.DataSource = tablaCarrito;

            // ocultar ID 
            if (dgvDetalles.Columns.Contains("IdProducto"))
                dgvDetalles.Columns["IdProducto"].Visible = false;

            dgvDetalles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarDatos()
        {
            try
            {
                // cargar productos
                ProductoDatos pDatos = new ProductoDatos();
                cmbProducto.DataSource = pDatos.ObtenerTodos();
                cmbProducto.DisplayMember = "Modelo";
                cmbProducto.ValueMember = "Id"; // asegúrate que tu modelo Producto tenga propiedad 'Id'
                cmbProducto.SelectedIndex = -1;

                // cargar clientes
                try
                {
                    ClienteDatos cDatos = new ClienteDatos();
                    cmbCliente.DataSource = cDatos.ObtenerTodos();
                    cmbCliente.DisplayMember = "Nombre";
                    cmbCliente.ValueMember = "IdCliente";
                    cmbCliente.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error cargando clientes: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }

        private void cmbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbProducto.SelectedIndex != -1 && cmbProducto.SelectedItem is DataRowView fila) // Si usas DataTable en ProductoDatos
                {
                    txtPrecio.Text = fila["Precio"].ToString();
                }
                
            }
            catch { }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //  validaciones basicas
            if (cmbProducto.SelectedIndex == -1)
            {
                MessageBox.Show("Selecciona un producto.");
                return;
            }
            if (numCantidad.Value < 1)
            {
                MessageBox.Show("La cantidad debe ser mayor a 0.");
                return;
            }

            // obtener datos del producto seleccionado 
            DataRowView fila = (DataRowView)cmbProducto.SelectedItem;

            int id = Convert.ToInt32(fila["Id"]);
            string modelo = fila["Modelo"].ToString();
            decimal precio = Convert.ToDecimal(fila["Precio"]);
            int cantidad = (int)numCantidad.Value;
            int stock = Convert.ToInt32(fila["Stock"]);

            //  validar stock
            if (cantidad > stock)
            {
                MessageBox.Show($"Stock insuficiente. Solo quedan {stock} unidades.");
                return;
            }

            //  evitar duplicados  si ya esta en la lista 
            foreach (DataRow row in tablaCarrito.Rows)
            {
                if (Convert.ToInt32(row["IdProducto"]) == id)
                {
                    MessageBox.Show("Este producto ya está en el carrito. Elimínalo si quieres corregir la cantidad.");
                    return;
                }
            }

            // calcular subtotal y agregar a la tabla visual
            decimal subtotal = precio * cantidad;
            tablaCarrito.Rows.Add(id, modelo, precio, cantidad, subtotal);

            //  actualiza el texto del total
            CalcularTotal();
        }

        private void CalcularTotal()
        {
            totalVenta = 0;
            foreach (DataRow fila in tablaCarrito.Rows)
            {
                totalVenta += Convert.ToDecimal(fila["Subtotal"]);
            }
            lblTotal.Text = "Total: RD$ " + totalVenta.ToString("N2");
        }

        // conecta con ventaDatos
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            // validaciones
            if (tablaCarrito.Rows.Count == 0)
            {
                MessageBox.Show("El carrito está vacío.");
                return;
            }
            if (cmbCliente.SelectedIndex == -1)
            {
                MessageBox.Show("Selecciona un cliente.");
                return;
            }

            //  preguntar confirmación
            if (MessageBox.Show("¿Procesar la venta?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    //crea el objeto Venta 
                    Venta nuevaVenta = new Venta();
                    nuevaVenta.IdCliente = Convert.ToInt32(cmbCliente.SelectedValue);
                    nuevaVenta.Fecha = DateTime.Now;
                    nuevaVenta.Total = totalVenta;

                    // nicializar la lista de detalles
                    nuevaVenta.Detalles = new List<DetalleVenta>();

                    //  llenar la lista con lo que hay en la tabla visual
                    foreach (DataRow row in tablaCarrito.Rows)
                    {
                        DetalleVenta detalle = new DetalleVenta();
                        detalle.IdProducto = Convert.ToInt32(row["IdProducto"]);
                        detalle.PrecioUnitario = Convert.ToDecimal(row["Precio"]);
                        detalle.Cantidad = Convert.ToInt32(row["Cantidad"]);

                        nuevaVenta.Detalles.Add(detalle);
                    }

                    // llamar a la Base de Datos
                    VentasDatos vDatos = new VentasDatos();
                    bool exito = vDatos.InsertarVenta(nuevaVenta);

                    if (exito)
                    {
                        MessageBox.Show("¡Venta registrada con éxito!");
                        LimpiarInterfaz();
                        CargarDatos(); // recargar para actualizar el stock en el combo
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar: " + ex.Message);
                }
            }
        }

        private void LimpiarInterfaz()
        {
            tablaCarrito.Rows.Clear();
            lblTotal.Text = "Total: RD$ 0.00";
            cmbProducto.SelectedIndex = -1;
            cmbCliente.SelectedIndex = -1;
            txtPrecio.Text = "0.00";
            numCantidad.Value = 1;
        }
    }
}