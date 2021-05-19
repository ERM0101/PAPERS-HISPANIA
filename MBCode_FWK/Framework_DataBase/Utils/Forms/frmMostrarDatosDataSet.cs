#region Librerias usadas por el formulario

using MBCode.Framework.Managers.Messages;
using System;
using System.Data;
using System.Windows.Forms;

#endregion

namespace MBCode.Framework.DataBase.Utils
{
    /// <summary>
    /// Autor: Alejandro Molt� Bou.
    /// Fecha �ltimo cambio: 05/03/2012.
    /// Descripci�n: Formulario encargado de mostrar los datos de manera gr�fica de los diferentes tipos de objeto de Bases 
    ///              de Datos con los que trabaja la clase de BD
    /// </summary>
    public partial class frmMostrarDatosDataSet : Form
    {
        #region Atributos

        /// <summary>
        /// DataSet del que se quiere mostrar informaci�n.
        /// </summary>
        private DataSet dsDataSetDatos;

        #endregion

        #region Propiedades

        /// <summary>
        /// Establece el origen de datos del DataGrid.
        /// </summary>
        public DataSet DataSetOrigenDatos
        {
            set
            {
                if ((dsDataSetDatos = value) != null) InicializarComponentesDataSet();
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
        public frmMostrarDatosDataSet()
        {
            InitializeComponent();
            this.dsDataSetDatos = null;
            this.tabDataTables.Visible = false;
        }

        #endregion

        #region M�todos

        #region M�todos relacionados con la presentaci�n de las diferentes Tablas del DataSet

        /// <summary>
        /// M�todo encargado de crear los controles gr�ficos necesarios para mostrar la 
        /// informaci�n de las tablas del DataSet.
        /// </summary>
        private void InicializarComponentesDataSet()
        {
            try
            {
                int iAncho = tabDataTables.Size.Width;
                int iAlto = tabDataTables.Size.Height;
                DataGridView dgDatosTabla;
                TabPage tabTabla;

                foreach (DataTable dtTabla in this.dsDataSetDatos.Tables)
                {
                    //  Crea los Controles asociados a la tabla.
                        tabTabla = new TabPage(dtTabla.TableName);
                        dgDatosTabla = new DataGridView();
                        tabTabla.SuspendLayout();
                        tabDataTables.Controls.Add(tabTabla);
                    // 
                    //  tabTabla -> Incializa ala pesta�a que se ascociara con la Tabla.
                    // 
                        tabTabla.Controls.Add(dgDatosTabla);
                        tabTabla.Location = new System.Drawing.Point(4, 22);
                        tabTabla.Name = "tabTabla_" + dtTabla.TableName;
                        tabTabla.Padding = new System.Windows.Forms.Padding(3);
                        tabTabla.Size = new System.Drawing.Size(iAncho - 15, iAlto - 35);
                        tabTabla.TabIndex = 0;
                        tabTabla.Text = dtTabla.TableName;
                        tabTabla.UseVisualStyleBackColor = true;
                    // 
                    //  dgDatosTabla -> Inicializa el control que contendr� los Datos de la Tabla.
                    // 
                        dgDatosTabla.Location = new System.Drawing.Point(3, 3);
                        dgDatosTabla.Size = new System.Drawing.Size(iAncho - 15, iAlto - 35);
                        dgDatosTabla.AllowUserToAddRows = false;
                        dgDatosTabla.AllowUserToDeleteRows = false;
                        dgDatosTabla.AllowUserToOrderColumns = true;
                        dgDatosTabla.BackgroundColor = System.Drawing.Color.SteelBlue;
                        dgDatosTabla.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                        dgDatosTabla.Name = "dgDatosTabla_" + dtTabla.TableName;
                        dgDatosTabla.Dock = System.Windows.Forms.DockStyle.Fill;
                        dgDatosTabla.ReadOnly = true;
                        dgDatosTabla.TabIndex = 2;
                    //  Asignamos el DataSource del DataGridView que acabamos de configurar.
                        dgDatosTabla.DataSource = dtTabla;
                }
                if (dsDataSetDatos.Tables.Count > 0) this.tabDataTables.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(MsgManager.ExcepMsg(ex));
            }
        }

        #endregion

        #region M�todos relacionados con la gesti�n de los eventos de los controles del formulario

        /// <summary>
        /// M�todo que gestiona el evento que se produce cuando el usuario hace click sobre 
        /// el bot�n de cerrar del formulario.
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento</param>
        /// <param name="e">Par�metros con los que se lanza el evento</param>
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #endregion
    }
}