namespace MBCode.Framework.DataBase.Utils
{
    partial class frmMostrarDatosTabla
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMostrarDatosTabla));
            this.btnCerrar = new System.Windows.Forms.Button();
            this.dgDatosElemento = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgDatosElemento)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCerrar
            // 
            this.btnCerrar.AccessibleDescription = null;
            this.btnCerrar.AccessibleName = null;
            resources.ApplyResources(this.btnCerrar, "btnCerrar");
            this.btnCerrar.BackgroundImage = null;
            this.btnCerrar.Font = null;
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // dgDatosElemento
            // 
            this.dgDatosElemento.AccessibleDescription = null;
            this.dgDatosElemento.AccessibleName = null;
            this.dgDatosElemento.AllowUserToAddRows = false;
            this.dgDatosElemento.AllowUserToDeleteRows = false;
            this.dgDatosElemento.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.dgDatosElemento, "dgDatosElemento");
            this.dgDatosElemento.BackgroundColor = System.Drawing.Color.SteelBlue;
            this.dgDatosElemento.BackgroundImage = null;
            this.dgDatosElemento.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDatosElemento.Font = null;
            this.dgDatosElemento.Name = "dgDatosElemento";
            this.dgDatosElemento.ReadOnly = true;
            // 
            // frmMostrarDatosTabla
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.BackgroundImage = null;
            this.Controls.Add(this.dgDatosElemento);
            this.Controls.Add(this.btnCerrar);
            this.Font = null;
            this.Icon = null;
            this.Name = "frmMostrarDatosTabla";
            ((System.ComponentModel.ISupportInitialize)(this.dgDatosElemento)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.DataGridView dgDatosElemento;
    }
}