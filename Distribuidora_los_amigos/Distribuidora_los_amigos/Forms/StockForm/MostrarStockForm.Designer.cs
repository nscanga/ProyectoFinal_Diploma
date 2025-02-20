namespace Distribuidora_los_amigos.Forms.StockForm
{
    partial class MostrarStockForm
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
            this.Eliminar = new System.Windows.Forms.Button();
            this.buttonModificarStock = new System.Windows.Forms.Button();
            this.dataGridViewStock = new System.Windows.Forms.DataGridView();
            this.buttonActualizarProducto = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStock)).BeginInit();
            this.SuspendLayout();
            // 
            // Eliminar
            // 
            this.Eliminar.Location = new System.Drawing.Point(685, 385);
            this.Eliminar.Name = "Eliminar";
            this.Eliminar.Size = new System.Drawing.Size(75, 23);
            this.Eliminar.TabIndex = 5;
            this.Eliminar.Text = "buttonEliminarStock";
            this.Eliminar.UseVisualStyleBackColor = true;
            this.Eliminar.Click += new System.EventHandler(this.Eliminar_Click);
            // 
            // buttonModificarStock
            // 
            this.buttonModificarStock.Location = new System.Drawing.Point(604, 385);
            this.buttonModificarStock.Name = "buttonModificarStock";
            this.buttonModificarStock.Size = new System.Drawing.Size(75, 23);
            this.buttonModificarStock.TabIndex = 4;
            this.buttonModificarStock.Text = "Modificar";
            this.buttonModificarStock.UseVisualStyleBackColor = true;
            this.buttonModificarStock.Click += new System.EventHandler(this.buttonModificarStock_Click);
            // 
            // dataGridViewStock
            // 
            this.dataGridViewStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStock.Location = new System.Drawing.Point(42, 34);
            this.dataGridViewStock.Name = "dataGridViewStock";
            this.dataGridViewStock.Size = new System.Drawing.Size(718, 326);
            this.dataGridViewStock.TabIndex = 3;
            // 
            // buttonActualizarProducto
            // 
            this.buttonActualizarProducto.Location = new System.Drawing.Point(523, 385);
            this.buttonActualizarProducto.Name = "buttonActualizarProducto";
            this.buttonActualizarProducto.Size = new System.Drawing.Size(75, 23);
            this.buttonActualizarProducto.TabIndex = 6;
            this.buttonActualizarProducto.Tag = "Actualizar";
            this.buttonActualizarProducto.Text = "Actualizar";
            this.buttonActualizarProducto.UseVisualStyleBackColor = true;
            this.buttonActualizarProducto.Click += new System.EventHandler(this.buttonActualizarProducto_Click);
            // 
            // MostrarStockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 427);
            this.Controls.Add(this.buttonActualizarProducto);
            this.Controls.Add(this.Eliminar);
            this.Controls.Add(this.buttonModificarStock);
            this.Controls.Add(this.dataGridViewStock);
            this.Name = "MostrarStockForm";
            this.Text = "MostrarStockForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStock)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Eliminar;
        private System.Windows.Forms.Button buttonModificarStock;
        private System.Windows.Forms.DataGridView dataGridViewStock;
        private System.Windows.Forms.Button buttonActualizarProducto;
    }
}