#region Librerias usadas por el formulario

using System;               
using System.Data;          
using System.Windows.Forms; 

#endregion

namespace MBCode.Framework.DataBase.Utils
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha última modificación: 05/03/2012.
    /// Descripción: formulario encargado de mostrar los datos de manera gráfica de los diferentes tipos de objeto de Bases 
    ///              de Datos con los que trabaja la clase de BD
    /// </summary>
    public partial class frmMostrarDatosTabla : Form
    {
        #region Propiedades

        /// <summary>
        /// Establece el origen de datos del DataGrid.
        /// </summary>
        public DataTable TablaOrigenDatos
        {
            set
            {
                this.dgDatosElemento.DataSource = value;
            }
        }
        
        /// <summary>
        /// Establece la leyenda del formulario.
        /// </summary>
        public string LeyendaFormulario
        {
            set 
            {
                this.Text = value;
            }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por defecto de la clase.
        /// </summary>
        public frmMostrarDatosTabla()
        {
            InitializeComponent();
        }

        #endregion

        #region Métodos

        #region Métodos relacionados con la gestión de los eventos de los controles del formulario

        /// <summary>
        /// Método que gestiona el evento que se produce cuando el usuario hace click sobre 
        /// el botón de cerrar del formulario.
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento</param>
        /// <param name="e">Parámetros con los que se lanza el evento</param>
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #endregion
    }
}