namespace Distribuidora_los_amigos.Forms.Productos
{
    partial class MostrarProductosForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonActualizarProducto = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonCrearProducto2 = new System.Windows.Forms.Button();
            this.buttonModificarProducto2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 21);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(767, 325);
            this.dataGridView1.TabIndex = 0;
            // 
            // buttonActualizarProducto
            // 
            this.buttonActualizarProducto.Location = new System.Drawing.Point(659, 369);
            this.buttonActualizarProducto.Name = "buttonActualizarProducto";
            this.buttonActualizarProducto.Size = new System.Drawing.Size(120, 30);
            this.buttonActualizarProducto.TabIndex = 2;
            this.buttonActualizarProducto.Tag = "Actualizar";
            this.buttonActualizarProducto.Text = "Actualizar";
            this.buttonActualizarProducto.UseVisualStyleBackColor = true;
            this.buttonActualizarProducto.Click += new System.EventHandler(this.buttonActualizarProducto_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(533, 369);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 30);
            this.button1.TabIndex = 3;
            this.button1.Tag = "Eliminar Producto";
            this.button1.Text = "Eliminar Producto";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonCrearProducto2
            // 
            this.buttonCrearProducto2.Location = new System.Drawing.Point(281, 369);
            this.buttonCrearProducto2.Name = "buttonCrearProducto2";
            this.buttonCrearProducto2.Size = new System.Drawing.Size(120, 30);
            this.buttonCrearProducto2.TabIndex = 4;
            this.buttonCrearProducto2.Tag = "Crear Producto";
            this.buttonCrearProducto2.Text = "Crear Producto";
            this.buttonCrearProducto2.UseVisualStyleBackColor = true;
            this.buttonCrearProducto2.Click += new System.EventHandler(this.buttonCrearProducto2_Click);
            // 
            // buttonModificarProducto2
            // 
            this.buttonModificarProducto2.Location = new System.Drawing.Point(407, 369);
            this.buttonModificarProducto2.Name = "buttonModificarProducto2";
            this.buttonModificarProducto2.Size = new System.Drawing.Size(120, 30);
            this.buttonModificarProducto2.TabIndex = 5;
            this.buttonModificarProducto2.Tag = "Modificar Producto";
            this.buttonModificarProducto2.Text = "Modificar Producto";
            this.buttonModificarProducto2.UseVisualStyleBackColor = true;
            this.buttonModificarProducto2.Click += new System.EventHandler(this.buttonModificarProducto2_Click);
            // 
            // MostrarProductosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 424);
            this.Controls.Add(this.buttonModificarProducto2);
            this.Controls.Add(this.buttonCrearProducto2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonActualizarProducto);
            this.Controls.Add(this.dataGridView1);
            this.Name = "MostrarProductosForm";
            this.Tag = "MostrarProductosForm";
            this.Text = "MostrarProductosForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonActualizarProducto;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonCrearProducto2;
        private System.Windows.Forms.Button buttonModificarProducto2;
    }
}