using MySql.Data.MySqlClient;
using Punto.conexion;
using System;
using System.Data;
using System.Windows.Forms;

namespace Punto.Forms
{
    public partial class frmProductos : Form
    {
        public frmProductos()
        {
            InitializeComponent();
        }

        private int idProductoSeleccionado = 0;



        private void frmProductos_Load(object sender, System.EventArgs e)
        {
            CargarProductos();

        }

        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProductos.Rows[e.RowIndex];

                idProductoSeleccionado = Convert.ToInt32(row.Cells["producto_id"].Value);
                txtCodigo.Text = row.Cells["Código"].Value.ToString();
                txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                txtPrecio.Text = row.Cells["Precio"].Value.ToString();
                txtStock.Text = row.Cells["Stock"].Value.ToString();
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtPrecio.Text, out decimal precioValidado))
            {
                MessageBox.Show("El precio debe ser un número válido.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stockValidado))
            {
                MessageBox.Show("El stock debe ser un número entero válido.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new Coneccion().obtenerconexion())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string query = "INSERT INTO productos (codigo, descripcion, precio, stock) VALUES (@cod, @desc, @pre, @sto)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cod", txtCodigo.Text);
                        cmd.Parameters.AddWithValue("@desc", txtNombre.Text);
                        cmd.Parameters.AddWithValue("@pre", precioValidado);
                        cmd.Parameters.AddWithValue("@sto", stockValidado);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Producto registrado con éxito.", "Éxito");
                CargarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar: " + ex.Message, "Error");
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idProductoSeleccionado == 0)
            {
                MessageBox.Show("Por favor, selecciona un producto de la tabla primero.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precioValidado))
            {
                MessageBox.Show("El precio debe ser un número válido.", "Error de Formato");
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stockValidado))
            {
                MessageBox.Show("El stock debe ser un número entero válido.", "Error de Formato");
                return;
            }

            try
            {
                using (MySqlConnection conn = new Coneccion().obtenerconexion())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string query = "UPDATE productos SET codigo=@cod, descripcion=@desc, precio=@pre, stock=@sto WHERE producto_id=@id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cod", txtCodigo.Text);
                        cmd.Parameters.AddWithValue("@desc", txtNombre.Text);
                        cmd.Parameters.AddWithValue("@pre", precioValidado);
                        cmd.Parameters.AddWithValue("@sto", stockValidado);
                        cmd.Parameters.AddWithValue("@id", idProductoSeleccionado);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Producto actualizado con éxito.", "Éxito");
                CargarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar: " + ex.Message, "Error");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idProductoSeleccionado == 0)
            {
                MessageBox.Show("Por favor, selecciona un producto de la tabla primero.", "Atención");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("¿Estás seguro de que deseas eliminar este producto?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new Coneccion().obtenerconexion())
                    {
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }

                        string query = "DELETE FROM productos WHERE producto_id=@id";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", idProductoSeleccionado);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Producto eliminado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProductos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void CargarProductos()
        {
            try
            {
                using (MySqlConnection conn = new Coneccion().obtenerconexion())
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    string query = "SELECT producto_id, codigo AS 'Código', descripcion AS 'Nombre', precio AS 'Precio', stock AS 'Stock' FROM productos";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvProductos.DataSource = dt;

                    if (dgvProductos.Columns.Contains("producto_id"))
                    {
                        dgvProductos.Columns["producto_id"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la tabla: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
