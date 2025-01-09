using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace ProyectoPP3
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
            



            // configuraciones de materialskin
            
            var materialSkinManager = MaterialSkinManager.Instance;

            
            materialSkinManager.AddFormToManage(this);

            
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT; // O DARK para un tema oscuro
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue600, Primary.Blue700, Primary.Blue200, Accent.Orange700, TextShade.WHITE);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarDatos("Cliente", dataGridView1);
            CargarDatos("Proveedores", dataGridView2);
            CargarDatos("Empleados", dataGridView3);
            CargarDatos("Productos", dataGridView4);

            // configuraciones de materialskin
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT; 
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue600, Primary.Blue700, Primary.Blue200, Accent.Orange700, TextShade.WHITE);
            

        }

        private void CargarDatos(string tabla, DataGridView dataGridView)
        {
            // ruta absoluta de la base de datos creada con DB Browser SQLite
            string connectionString = @"Data Source=C:\DATABASE\Proyecto.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    // se abre la conexion
                    connection.Open();

                    // comando sql para seleccionar todos los registros de la tabla que pasamos como parametro
                    string query = $"SELECT * FROM {tabla}";
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);

                    // llena el datatable con los datos de la query
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // se asigna el datable como la fuente de datos del datagrid
                    dataGridView.DataSource = dataTable;

                    AjustarAnchoColumnas(dataGridView, tabla);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar datos de la tabla {tabla}: " + ex.Message);
                }
            }
        }
        private void AjustarAnchoColumnas(DataGridView dataGridView, string tabla)
        {
            // los 4 datagrid tienen aprox un ancho de 900px
            if (tabla == "Cliente")
            {
                dataGridView.Columns[0].Width = 80;  
                dataGridView.Columns[1].Width = 180;  
                dataGridView.Columns[2].Width = 200;
                dataGridView.Columns[3].Width = 130;
                dataGridView.Columns[4].Width = 130;
                dataGridView.Columns[5].Width = 135;
            }
            else if (tabla == "Proveedores")
            {
                dataGridView.Columns[0].Width = 100;
                dataGridView.Columns[1].Width = 260;
                dataGridView.Columns[2].Width = 250;
                dataGridView.Columns[3].Width = 250;
            }
            else if (tabla == "Empleados")
            {
                dataGridView.Columns[0].Width = 100;
                dataGridView.Columns[1].Width = 300;
                dataGridView.Columns[2].Width = 250;
                dataGridView.Columns[3].Width = 210;
            }
            else if (tabla == "Productos")
            {
                dataGridView.Columns[0].Width = 120;
                dataGridView.Columns[1].Width = 230;
                dataGridView.Columns[2].Width = 180;
                dataGridView.Columns[3].Width = 225;
            }
        }



        private void btnAgregar1_Click_1(object sender, EventArgs e)
        {
            // se obtienen los valores de los textboxes (en este caso de la mascara: MaterialSkin2)
            string nombre = materialTextBox1.Text;
            string direccion = materialTextBox2.Text;
            string contacto1 = materialTextBox3.Text;
            string contacto2 = materialTextBox4.Text;
            string cuit = materialTextBox5.Text;

            // ruta absoluta la base de datos creada con DB Browser SQLite
            string connectionString = @"Data Source=C:\DATABASE\Proyecto.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // comando sql para insertar datos
                    string query = "INSERT INTO Cliente (Nombre, Direccion, Contacto1, Contacto2, CUIT) VALUES (@Nombre, @Direccion, @Contacto1, @Contacto2, @CUIT)";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        // se asignan los valores a los parametros utilizados en la query
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        command.Parameters.AddWithValue("@Direccion", direccion);
                        command.Parameters.AddWithValue("@Contacto1", contacto1);
                        command.Parameters.AddWithValue("@Contacto2", contacto2);
                        command.Parameters.AddWithValue("@CUIT", cuit);

                        // se ejecuta el comando
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Registro agregado con éxito a la tabla Cliente.");

                    // se actualiza el DataGridView
                    CargarDatos("Cliente", dataGridView1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al agregar registro a la tabla Cliente: " + ex.Message);
                }
            }
        }

        private void btnModificar1_Click_1(object sender, EventArgs e)
        {
            // verifica que haya una fila seleccionada
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // se obtiene el valor de la clave primaria
                int idCliente = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ClienteId"].Value);

                // se obtienen los valores de los textboxes
                string nombre = materialTextBox1.Text;
                string direccion = materialTextBox2.Text;
                string contacto1 = materialTextBox3.Text;
                string contacto2 = materialTextBox4.Text;
                string cuit = materialTextBox5.Text;

                // string con la ruta absoluta de la base de datos creada con DB Browser SQLite
                string connectionString = @"Data Source=C:\DATABASE\Proyecto.db;Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // comando sql para actualizar Cliente, con las columnas especificas y valor especifico
                        string query = "UPDATE Cliente SET Nombre = @Nombre, Direccion = @Direccion, Contacto1 = @Contacto1, Contacto2 = @Contacto2, CUIT = @CUIT WHERE ClienteId = @Id";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            // se asignan los valores de las variables a los parametros que estan en la query
                            command.Parameters.AddWithValue("@Nombre", nombre);
                            command.Parameters.AddWithValue("@Direccion", direccion);
                            command.Parameters.AddWithValue("@Contacto1", contacto1);
                            command.Parameters.AddWithValue("@Contacto2", contacto2);
                            command.Parameters.AddWithValue("@CUIT", cuit);
                            command.Parameters.AddWithValue("@Id", idCliente);

                            // se ejecuta el comando
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Registro modificado con éxito en la tabla Cliente.");

                        // se actualiza el DataGridView
                        CargarDatos("Cliente", dataGridView1);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al modificar el registro: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para modificar.");
            }
        }

        private void btnEliminar1_Click_1(object sender, EventArgs e)
        {
            // verifica que haya una fila seleccionada
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // se obtiene el valor de la clave primaria (eb este caso autoincrement)
                int idCliente = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ClienteId"].Value);

                // string de conexion a la base de datos creada con DB Browser SQLite
                string connectionString = @"Data Source=C:\DATABASE\Proyecto.db;Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // query donde se especifica la accion sobre la tabla correspondiente y al registro exacto
                        string query = "DELETE FROM Cliente WHERE ClienteId = @Id";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            // se asigna la variable idCliente al parametro @Id utilizado para la query escrita
                            command.Parameters.AddWithValue("@Id", idCliente);

                            // se ejecuta el comando
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Registro eliminado con éxito de la tabla Cliente.");

                        // llama a la funcion que recarga el datagrid
                        CargarDatos("Cliente", dataGridView1);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el registro: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para eliminar.");
            }
        }

        private void btnBuscar1_Click_1(object sender, EventArgs e)
        {
            // se almacena el valor del text box donde se inserta el nombre a buscar
            string nombreBuscado = buscar1.Text;

            // string con la ruta a la base de datos creaada con DB Browser SQLite
            string connectionString = @"Data Source=C:\DATABASE\Proyecto.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // se construye la query para filtrar la tabla Cliente en base a la columna Nombre
                    string query = "SELECT * FROM Cliente WHERE Nombre LIKE @Nombre";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        // se asigna la variable nombreBuscado al parametro @Nombre para que sea insertado en la query de arriba
                        // los % % indica que puede o no haber caracteres antes o despues
                        command.Parameters.AddWithValue("@Nombre", "%" + nombreBuscado + "%");

                        // se llena el datatable con los resultados de la busqueda
                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // se pone al  DataTable como fuente de datos del dataGridview1
                        dataGridView1.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar en la tabla Cliente: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string direccion = materialTextBox22.Text;
            string nombre = materialTextBox21.Text;
            string contacto = materialTextBox23.Text;
            string connectionString = @"Data Source=C:\DATABASE\Proyecto.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    
                    string query = "INSERT INTO Proveedores (Direccion, Nombre, Contacto) VALUES (@Direccion, @Nombre, @Contacto1)";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@Direccion", direccion);
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        
                        command.Parameters.AddWithValue("@Contacto1", contacto);
                        

                        
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Registro agregado con éxito a la tabla Proveedores.");

                    
                    CargarDatos("Proveedores", dataGridView2);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al agregar registro a la tabla Proveedores: " + ex.Message);
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            
            if (dataGridView2.SelectedRows.Count > 0)
            {
                
                int idCliente = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["ProveedorID"].Value);

                
                string nombre = materialTextBox21.Text;
                string direccion = materialTextBox22.Text;
                string contacto1 = materialTextBox23.Text;
                

                
                string connectionString = @"Data Source=C:\DATABASE\Proyecto.db;Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        
                        string query = "UPDATE Proveedores SET Direccion = @Direccion, Nombre = @Nombre, Contacto = @Contacto1 WHERE ProveedorID = @Id";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            
                            command.Parameters.AddWithValue("@Direccion", direccion);
                            command.Parameters.AddWithValue("@Nombre", nombre);
                            
                            command.Parameters.AddWithValue("@Contacto1", contacto1);
                            
                            command.Parameters.AddWithValue("@Id", idCliente);

                            
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Registro modificado con éxito en la tabla Proveedores.");

                        
                        CargarDatos("Proveedores", dataGridView2);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al modificar el registro: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para modificar.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            if (dataGridView2.SelectedRows.Count > 0)
            {
                
                int idCliente = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["ProveedorID"].Value);

                
                string connectionString = @"Data Source=C:\DATABASE\Proyecto.db;Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        
                        string query = "DELETE FROM Proveedores WHERE ProveedorID = @Id";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            
                            command.Parameters.AddWithValue("@Id", idCliente);

                            
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Registro eliminado con éxito de la tabla Proveedores.");

                        
                        CargarDatos("Proveedores", dataGridView2);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el registro: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para eliminar.");
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            
            string nombreBuscado = materialTextBox6.Text;

            
            string connectionString = @"Data Source=C:\DATABASE\Proyecto.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    
                    string query = "SELECT * FROM Proveedores WHERE Nombre LIKE @Nombre";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@Nombre", "%" + nombreBuscado + "%");

                        
                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        
                        dataGridView2.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar en la tabla Proveedores: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            string nombre = materialTextBox24.Text;
            string fecha = materialTextBox25.Text;
            string cuit = materialTextBox26.Text;


            
            string connectionString = @"Data Source=C:\DATABASE\Proyecto.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    
                    string query = "INSERT INTO Empleados (Nombre, FechaNacimiento, CUIT) VALUES (@Nombre, @Fecha, @CUIT)";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        command.Parameters.AddWithValue("@Fecha", fecha);

                        command.Parameters.AddWithValue("@CUIT", cuit);


                        
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Registro agregado con éxito a la tabla Empleados.");

                    
                    CargarDatos("Empleados", dataGridView3);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al agregar registro a la tabla Empleados: " + ex.Message);
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            
            if (dataGridView3.SelectedRows.Count > 0)
            {
                
                int idCliente = Convert.ToInt32(dataGridView3.SelectedRows[0].Cells["EmpleadoID"].Value);

                
                string nombre = materialTextBox24.Text;
                string fecha = materialTextBox25.Text;
                string cuit = materialTextBox26.Text;


                
                string connectionString = @"Data Source=C:\DATABASE\Proyecto.db;Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        
                        string query = "UPDATE Empleados SET Nombre = @Nombre, FechaNacimiento = @Fecha, CUIT = @CUIT WHERE EmpleadoID = @Id";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            
                            command.Parameters.AddWithValue("@Nombre", nombre);
                            command.Parameters.AddWithValue("@Fecha", fecha);

                            command.Parameters.AddWithValue("@CUIT", cuit);
                            command.Parameters.AddWithValue("@Id", idCliente);



                            
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Registro modificado con éxito en la tabla Empleados.");

                        
                        CargarDatos("Empleados", dataGridView3);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al modificar el registro: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para modificar.");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            if (dataGridView3.SelectedRows.Count > 0)
            {
                
                int idCliente = Convert.ToInt32(dataGridView3.SelectedRows[0].Cells["EmpleadoID"].Value);

                
                string connectionString = @"Data Source=C:\DATABASE\Proyecto.db;Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        
                        string query = "DELETE FROM Empleados WHERE EmpleadoID = @Id";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            
                            command.Parameters.AddWithValue("@Id", idCliente);

                            
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Registro eliminado con éxito de la tabla Empleados.");

                        
                        CargarDatos("Empleados", dataGridView3);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el registro: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para eliminar.");
            }
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            
            string nombreBuscado = materialTextBox211.Text;

            
            string connectionString = @"Data Source=C:\DATABASE\Proyecto.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    
                    string query = "SELECT * FROM Empleados WHERE Nombre LIKE @Nombre";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@Nombre", "%" + nombreBuscado + "%");

                        
                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        
                        dataGridView3.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar en la tabla Proveedores: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            string productoname = materialTextBox27.Text;
            string proveedorid = materialTextBox28.Text;
            string cantidad = materialTextBox29.Text;
            string precio = materialTextBox210.Text;


            
            string connectionString = @"Data Source=C:\DATABASE\Proyecto.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    
                    string query = "INSERT INTO Productos (ProductoName, ProveedorID, Cantidad, Precio) VALUES (@Nombre, @ProveedorID, @Cantidad, @Precio)";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@Nombre", productoname);
                        command.Parameters.AddWithValue("@ProveedorID", proveedorid);

                        command.Parameters.AddWithValue("@Cantidad", cantidad);
                        command.Parameters.AddWithValue("@Precio", precio);


                        
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Registro agregado con éxito a la tabla Productos.");

                    
                    CargarDatos("Productos", dataGridView4);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al agregar registro a la tabla Productos: " + ex.Message);
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            
            if (dataGridView4.SelectedRows.Count > 0)
            {
                
                int idCliente = Convert.ToInt32(dataGridView4.SelectedRows[0].Cells["ProductoID"].Value);

                
                string productoname = materialTextBox27.Text;
                string proveedorid = materialTextBox28.Text;
                string cantidad = materialTextBox29.Text;
                string precio = materialTextBox210.Text;


                
                string connectionString = @"Data Source=C:\DATABASE\Proyecto.db;Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        
                        string query = "UPDATE Productos SET ProductoName = @Nombre, ProveedorID = @Proveedor, Cantidad = @Cantidad, Precio = @Precio WHERE ProductoID = @Id";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            
                            command.Parameters.AddWithValue("@Nombre", productoname);
                            command.Parameters.AddWithValue("@Proveedor", proveedorid);

                            command.Parameters.AddWithValue("@Cantidad", cantidad);
                            command.Parameters.AddWithValue("@Precio", precio);
                            command.Parameters.AddWithValue("@Id", idCliente);



                            
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Registro modificado con éxito en la tabla Productos.");

                        
                        CargarDatos("Productos", dataGridView4);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al modificar el registro: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para modificar.");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            
            if (dataGridView4.SelectedRows.Count > 0)
            {
                
                int idCliente = Convert.ToInt32(dataGridView4.SelectedRows[0].Cells["ProductoID"].Value);

                
                string connectionString = @"Data Source=C:\DATABASE\Proyecto.db;Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        
                        string query = "DELETE FROM Productos WHERE ProductoID = @Id";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            
                            command.Parameters.AddWithValue("@Id", idCliente);

                            
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Registro eliminado con éxito de la tabla Productos.");

                        
                        CargarDatos("Productos", dataGridView4);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el registro: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para eliminar.");
            }
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            
            string nombreBuscado = materialTextBox212.Text;

            
            string connectionString = @"Data Source=C:\DATABASE\Proyecto.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    
                    string query = "SELECT * FROM Productos WHERE ProductoName LIKE @Nombre";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@Nombre", "%" + nombreBuscado + "%");

                        
                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        
                        dataGridView4.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar en la tabla Productos: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                // Crea un diálogo para seleccionar la carpeta de destino
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    // Si el usuario selecciona una carpeta
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // ruta absoluta de la base de datos de origen
                            string sourceFilePath = @"C:\DATABASE\Proyecto.db";
                            // ruta de destino seleccionada por el usuario
                            string destinationFilePath = Path.Combine(folderDialog.SelectedPath, "Proyecto.db");

                            // copia el archivo
                            File.Copy(sourceFilePath, destinationFilePath, overwrite: true);
                            MessageBox.Show("Base de datos copiada con éxito.");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al copiar la base de datos: " + ex.Message);
                        }
                    }
                
            }
        }

        private void btnLimpiar1_Click(object sender, EventArgs e)
        {
            materialTextBox1.Text = string.Empty;
            materialTextBox2.Text = string.Empty;
            materialTextBox3.Text = string.Empty;
            materialTextBox4.Text = string.Empty;
            materialTextBox5.Text = string.Empty;
            buscar1.Text = string.Empty;
        }

        private void btnLimpiar2_Click(object sender, EventArgs e)
        {
            materialTextBox21.Text = string.Empty;
            materialTextBox22.Text = string.Empty;
            materialTextBox23.Text = string.Empty;
            materialTextBox6.Text = string.Empty;
        }

        private void btnLimpiar3_Click(object sender, EventArgs e)
        {
            materialTextBox24.Text = string.Empty;
            materialTextBox25.Text = string.Empty;
            materialTextBox26.Text = string.Empty;
            materialTextBox211.Text = string.Empty;
        }

        private void btnLimpiar4_Click(object sender, EventArgs e)
        {
            materialTextBox27.Text = string.Empty;
            materialTextBox28.Text = string.Empty;
            materialTextBox29.Text = string.Empty;
            materialTextBox210.Text = string.Empty;
            materialTextBox212.Text = string.Empty;
        }
    }
}
