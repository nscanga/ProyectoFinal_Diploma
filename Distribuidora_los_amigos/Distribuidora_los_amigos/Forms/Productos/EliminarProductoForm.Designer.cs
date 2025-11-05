namespace Distribuidora_los_amigos.Forms.Productos
{
    partial class EliminarProductoForm
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancelarProducto = new System.Windows.Forms.Button();
            this.btnGuardarProducto = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(118, 25);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(172, 21);
            this.comboBox1.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 12;
            this.label1.Tag = "Buscar Producto";
            this.label1.Text = "Buscar Producto";
            // 
            // btnCancelarProducto
            // 
            this.btnCancelarProducto.Location = new System.Drawing.Point(63, 70);
            this.btnCancelarProducto.Name = "btnCancelarProducto";
            this.btnCancelarProducto.Size = new System.Drawing.Size(75, 23);
            this.btnCancelarProducto.TabIndex = 23;
            this.btnCancelarProducto.Tag = "Cancelar";
            this.btnCancelarProducto.Text = "Cancelar";
            this.btnCancelarProducto.UseVisualStyleBackColor = true;
            // 
            // btnGuardarProducto
            // 
            this.btnGuardarProducto.Location = new System.Drawing.Point(174, 70);
            this.btnGuardarProducto.Name = "btnGuardarProducto";
            this.btnGuardarProducto.Size = new System.Drawing.Size(75, 23);
            this.btnGuardarProducto.TabIndex = 22;
            this.btnGuardarProducto.Tag = "Eliminar";
            this.btnGuardarProducto.Text = "Eliminar";
            this.btnGuardarProducto.UseVisualStyleBackColor = true;
            // 
            // EliminarProductoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 132);
            this.Controls.Add(this.btnCancelarProducto);
            this.Controls.Add(this.btnGuardarProducto);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Name = "EliminarProductoForm";
            this.Tag = "EliminarProductoForm";
            this.Text = "EliminarProductoForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancelarProducto;
        private System.Windows.Forms.Button btnGuardarProducto;
    }
}