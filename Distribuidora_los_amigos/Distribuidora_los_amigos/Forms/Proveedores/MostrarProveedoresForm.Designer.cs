namespace Distribuidora_los_amigos.Forms.Proveedores
{
    partial class MostrarProveedoresForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonModificarProveedor = new System.Windows.Forms.Button();
            this.buttonCrearProveedor = new System.Windows.Forms.Button();
            this.buttonEliminarProveedor = new System.Windows.Forms.Button();
            this.buttonActualizarProveedor = new System.Windows.Forms.Button();
            this.dataGridViewProveedores = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProveedores)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonModificarProveedor
            // 
            this.buttonModificarProveedor.Location = new System.Drawing.Point(407, 374);
            this.buttonModificarProveedor.Name = "buttonModificarProveedor";
            this.buttonModificarProveedor.Size = new System.Drawing.Size(120, 30);
            this.buttonModificarProveedor.TabIndex = 10;
            this.buttonModificarProveedor.Tag = "Modificar";
            this.buttonModificarProveedor.Text = "Modificar Proveedor";
            this.buttonModificarProveedor.UseVisualStyleBackColor = true;
            this.buttonModificarProveedor.Click += new System.EventHandler(this.buttonModificarProveedor_Click);
            // 
            // buttonCrearProveedor
            // 
            this.buttonCrearProveedor.Location = new System.Drawing.Point(281, 374);
            this.buttonCrearProveedor.Name = "buttonCrearProveedor";
            this.buttonCrearProveedor.Size = new System.Drawing.Size(120, 30);
            this.buttonCrearProveedor.TabIndex = 9;
            this.buttonCrearProveedor.Tag = "Crear";
            this.buttonCrearProveedor.Text = "Crear Proveedor";
            this.buttonCrearProveedor.UseVisualStyleBackColor = true;
            this.buttonCrearProveedor.Click += new System.EventHandler(this.buttonCrearProveedor_Click);
            // 
            // buttonEliminarProveedor
            // 
            this.buttonEliminarProveedor.Location = new System.Drawing.Point(533, 374);
            this.buttonEliminarProveedor.Name = "buttonEliminarProveedor";
            this.buttonEliminarProveedor.Size = new System.Drawing.Size(120, 30);
            this.buttonEliminarProveedor.TabIndex = 8;
            this.buttonEliminarProveedor.Tag = "Eliminar";
            this.buttonEliminarProveedor.Text = "Eliminar Proveedor";
            this.buttonEliminarProveedor.UseVisualStyleBackColor = true;
            this.buttonEliminarProveedor.Click += new System.EventHandler(this.buttonEliminarProveedor_Click_1);
            // 
            // buttonActualizarProveedor
            // 
            this.buttonActualizarProveedor.Location = new System.Drawing.Point(659, 374);
            this.buttonActualizarProveedor.Name = "buttonActualizarProveedor";
            this.buttonActualizarProveedor.Size = new System.Drawing.Size(120, 30);
            this.buttonActualizarProveedor.TabIndex = 7;
            this.buttonActualizarProveedor.Tag = "Actualizar";
            this.buttonActualizarProveedor.Text = "Actualizar";
            this.buttonActualizarProveedor.UseVisualStyleBackColor = true;
            // 
            // dataGridViewProveedores
            // 
            this.dataGridViewProveedores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProveedores.Location = new System.Drawing.Point(12, 26);
            this.dataGridViewProveedores.Name = "dataGridViewProveedores";
            this.dataGridViewProveedores.Size = new System.Drawing.Size(767, 325);
            this.dataGridViewProveedores.TabIndex = 6;
            // 
            // MostrarProveedoresForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonModificarProveedor);
            this.Controls.Add(this.buttonCrearProveedor);
            this.Controls.Add(this.buttonEliminarProveedor);
            this.Controls.Add(this.buttonActualizarProveedor);
            this.Controls.Add(this.dataGridViewProveedores);
            this.Name = "MostrarProveedoresForm";
            this.Text = "MostrarProveedoresForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProveedores)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonModificarProveedor;
        private System.Windows.Forms.Button buttonCrearProveedor;
        private System.Windows.Forms.Button buttonEliminarProveedor;
        private System.Windows.Forms.Button buttonActualizarProveedor;
        private System.Windows.Forms.DataGridView dataGridViewProveedores;
    }
}