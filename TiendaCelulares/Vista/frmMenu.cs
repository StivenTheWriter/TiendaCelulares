using System;
using System.Windows.Forms;

namespace TiendaCelulares.Vista
{
    public partial class frmMenu : Form
    {
        public frmMenu()
        {
            InitializeComponent();
        }

        //ir a ventas
        private void btnVentas_Click_1(object sender, EventArgs e)
        {
            // abre el formulario de ventas
            frmVentas formulario = new frmVentas();
            formulario.ShowDialog();
        }

        //ir a ventas
        private void btnHistorial_Click(object sender, EventArgs e)
        {
            // abre el historial
            frmHistorial formulario = new frmHistorial();
            formulario.ShowDialog();
        }

        private void btnProductos_Click_1(object sender, EventArgs e)
        {
            FrmInventario formulario = new FrmInventario();
            formulario.ShowDialog();
        }
        
        private void btnClientes_Click_1(object sender, EventArgs e)
        {
          
            new frmClientes().ShowDialog();
        }

        private void btnSalir_Click_1(object sender, EventArgs e)
        {
            Application.Exit(); // cierra toda la aplicación
        }

        //carga el formulario
        private void frmMenu_Load(object sender, EventArgs e)
        {
            this.Text = "Sistema Tienda de Celulares - Menú Principal";
        }

       
        private void frmMenu_Load_1(object sender, EventArgs e)
        {
        }

       
        private void frmMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); // asegura que se cierre el login 
        }
    }
}