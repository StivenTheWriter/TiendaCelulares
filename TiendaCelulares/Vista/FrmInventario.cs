using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// Referencias a tus capas de Datos y Modelo
using TiendaCelulares.Datos;
using TiendaCelulares.Modelo;

namespace TiendaCelulares.Vista
{
    public partial class FrmInventario : Form
    {
        public FrmInventario()
        {
            InitializeComponent();
        }

        // carga el frm
        private void FrmInventario_Load(object sender, EventArgs e)
        {
            this.Text = "Gestión de Inventario - Stock de Celulares";
             
        }

        // metodo para cargar datos
        private void RefrescarLista()
        {
            try
            {
               
                ProductoDatos datos = new ProductoDatos();

                //  metodo que devuelve la tabla SQL
                DataTable tabla = datos.ObtenerTodos();

                // se le asigna al datagriview
                dgvProductos.DataSource = tabla;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar: " + ex.Message);
            }
        }

        
        private void btnCargar_Click(object sender, EventArgs e)
        {
            RefrescarLista();
        }

       
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            // abre la  ventana de alta
            frmAltaProducto ventana = new frmAltaProducto();

            // ShowDialog congela esta ventana hasta que la cierres
            ventana.ShowDialog();
            // Al regresar, recargamos la lista automáticamente
            RefrescarLista();
        }

        
        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close(); // cierra el inventario y regresa al menu principal
        }

        private void FrmInventario_Load_1(object sender, EventArgs e)
        {

        }
    }
}