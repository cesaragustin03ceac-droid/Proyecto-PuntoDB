using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Punto.conexion
{
    internal class Coneccion
    {
        private readonly string cadena;

        public Coneccion()
        {
            cadena = "Server=127.0.0.1; Database=puntodb; Uid=root; Pwd=; Port=3306";

        }


        public MySqlConnection obtenerconexion()
        {
            try
            {
                MySqlConnection conexion = new MySqlConnection(cadena);
                conexion.Open();
                MessageBox.Show("Conectado corrextamente...");
                return conexion;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al conectar la base de datos " + ex.ToString());
                return null;
            }
        }
    }
}
