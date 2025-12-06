using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TiendaCelulares.Modelo;
using TiendaCelulares.Datos;

namespace TiendaCelulares.Vista
{
    public partial class frmAltaProducto : Form
    {
        public frmAltaProducto()
        {
            InitializeComponent();

            // inicializa el ComboBox si esta vacio 
            if (cmbTipo.Items.Count > 0 && cmbTipo.SelectedIndex == -1)
            {
                cmbTipo.SelectedIndex = 0; // Selecciona el primero por defecto
            }

          
            cmbTipo_SelectedIndexChanged(null, null);
        }

        private void frmAltaProducto_Load(object sender, EventArgs e)
        {
            this.Text = "Alta de Nuevo Producto";
        }

        
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // validaciones basicas
            if (string.IsNullOrWhiteSpace(txtMarca.Text) ||
                string.IsNullOrWhiteSpace(txtModelo.Text) ||
                string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("Por favor complete Marca, Modelo y Precio.");
                return;
            }

            Producto nuevoProducto = null;

            try
            {
                //  logica de polimorfismo (celular vs accesorio)
                if (cmbTipo.Text == "Celular")
                {
                    Celular cel = new Celular();

                    // Validación y conversión segura de RAM y Almacenamiento
                    // si el campo esta vacio, guardamos 0 , si no convertimos el texto.
                    cel.Ram = string.IsNullOrWhiteSpace(txtRam.Text) ? 0 : int.Parse(txtRam.Text.Trim());
                    cel.Almacenamiento = string.IsNullOrWhiteSpace(txtAlmacenamiento.Text) ? 0 : int.Parse(txtAlmacenamiento.Text.Trim());

                    nuevoProducto = cel;
                }
                else
                {
                    // si no es celular, asumimos que es un sccesorio generico
                    nuevoProducto = new Accesorio();
                }

                //  llenar datos comunes (clase padre)
                nuevoProducto.Marca = txtMarca.Text.Trim();
                nuevoProducto.Modelo = txtModelo.Text.Trim();
                nuevoProducto.Precio = Convert.ToDecimal(txtPrecio.Text.Trim());

                // us el numStock  en lugar de textbox
                nuevoProducto.Stock = Convert.ToInt32(numStock.Value);

                //  guardo en bd
                ProductoDatos datos = new ProductoDatos();

                if (datos.Insertar(nuevoProducto))
                {
                    MessageBox.Show("¡Producto registrado con éxito!");
                    this.Close(); // cierra el formulario y vuelve al inventario
                }
                else
                {
                    MessageBox.Show("No se pudo registrar el producto en la base de datos.");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Error de formato: Verifique que Precio, RAM y Almacenamiento sean números válidos.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message);
            }
        }

        // visibilidad de los campos
        private void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // verifico si se selecciono un celular
            bool esCelular = (cmbTipo.Text == "Celular");

            // mostrar u ocultar campos específicos 
            if (txtRam != null) txtRam.Visible = esCelular;
            if (txtAlmacenamiento != null) txtAlmacenamiento.Visible = esCelular;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}