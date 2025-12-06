using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TiendaCelulares.Datos;
using TiendaCelulares.Modelo;

namespace TiendaCelulares.Vista
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtClave_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string user = txtUsuario.Text;
            string pass = txtClave.Text;

            UsuarioDatos datos = new UsuarioDatos();
            Usuario usuarioLogueado = datos.Loguear(user, pass);

            if (usuarioLogueado != null)
            {
                MessageBox.Show("Bienvenido " + usuarioLogueado.NombreUsuario);
                this.Hide();
                
                frmMenu menuPrincipal = new frmMenu();

                // mostrar el menu
                menuPrincipal.Show();

                //ocultar el login actual para que no estorbe
                this.Hide();

                // OPCIONAL: Si quieres que al cerrar el Menu se cierre toda la app, 
                // puedes agregar esto en el evento FormClosed del Menu (lo veremos luego).
            }
            else
            {
                MessageBox.Show("Datos incorrectos");
            }
        }
    }
}
