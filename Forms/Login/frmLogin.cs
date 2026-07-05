using MySql.Data.MySqlClient;
using Punto.conexion;
using System;
using System.Windows.Forms;

namespace Punto.Forms
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, System.EventArgs e)
        {
            string usuario = txtUser.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("El usuario y la contraseña son obligatorios.", "Validación");
                return;
            }

            try
            {
                Coneccion conecion = new Coneccion();
                using (MySqlConnection conn = conecion.obtenerconexion())
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    string consulta = "SELECT nombre_completo FROM usuarios WHERE username = @user AND PASSWORD = @pass";

                    using (MySqlCommand command = new MySqlCommand(consulta, conn))
                    {
                        command.Parameters.AddWithValue("@user", usuario);
                        command.Parameters.AddWithValue("@pass", password);

                        object resultado = command.ExecuteScalar();

                        if (resultado != null)
                        {
                            this.Hide();
                            frmPrincipal principal = new frmPrincipal();
                            principal.Show();
                        }
                        else
                        {
                            MessageBox.Show("Usuario o contraseña incorrectos.", "Error de acceso");
                            txtUser.Clear();
                            txtPassword.Clear();
                            txtUser.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error");
            }
        }
        private void frmLogin_Load(object sender, System.EventArgs e)
        {

        }
    }
}
