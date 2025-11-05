namespace Distribuidora_los_amigos.Forms.StockForm
{
    partial class ModificarStockForm
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
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxTipoStock = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownStock = new System.Windows.Forms.NumericUpDown();
            this.btnCancelarProducto = new System.Windows.Forms.Button();
            this.btnGuardarProducto = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStock)).BeginInit();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(38, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 27;
            this.label7.Tag = "TipoStock";
            this.label7.Text = "Tipo Stock";
            // 
            // comboBoxTipoStock
            // 
            this.comboBoxTipoStock.FormattingEnabled = true;
            this.comboBoxTipoStock.Location = new System.Drawing.Point(125, 57);
            this.comboBoxTipoStock.Name = "comboBoxTipoStock";
            this.comboBoxTipoStock.Size = new System.Drawing.Size(172, 21);
            this.comboBoxTipoStock.TabIndex = 26;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(38, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 25;
            this.label6.Tag = "Cantidad";
            this.label6.Text = "Cantidad";
            // 
            // numericUpDownStock
            // 
            this.numericUpDownStock.Location = new System.Drawing.Point(125, 31);
            this.numericUpDownStock.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numericUpDownStock.Name = "numericUpDownStock";
            this.numericUpDownStock.Size = new System.Drawing.Size(172, 20);
            this.numericUpDownStock.TabIndex = 24;
            // 
            // btnCancelarProducto
            // 
            this.btnCancelarProducto.Location = new System.Drawing.Point(72, 100);
            this.btnCancelarProducto.Name = "btnCancelarProducto";
            this.btnCancelarProducto.Size = new System.Drawing.Size(75, 23);
            this.btnCancelarProducto.TabIndex = 23;
            this.btnCancelarProducto.Tag = "Cancelar";
            this.btnCancelarProducto.Text = "Cancelar";
            this.btnCancelarProducto.UseVisualStyleBackColor = true;
            // 
            // btnGuardarProducto
            // 
            this.btnGuardarProducto.Location = new System.Drawing.Point(183, 100);
            this.btnGuardarProducto.Name = "btnGuardarProducto";
            this.btnGuardarProducto.Size = new System.Drawing.Size(75, 23);
            this.btnGuardarProducto.TabIndex = 22;
            this.btnGuardarProducto.Tag = "Guardar";
            this.btnGuardarProducto.Text = "Guardar";
            this.btnGuardarProducto.UseVisualStyleBackColor = true;
            this.btnGuardarProducto.Click += new System.EventHandler(this.btnGuardarProducto_Click);
            // 
            // ModificarStockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 149);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBoxTipoStock);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numericUpDownStock);
            this.Controls.Add(this.btnCancelarProducto);
            this.Controls.Add(this.btnGuardarProducto);
            this.Name = "ModificarStockForm";
            this.Tag = "ModificarStock";
            this.Text = "ModificarStockForm";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStock)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxTipoStock;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownStock;
        private System.Windows.Forms.Button btnCancelarProducto;
        private System.Windows.Forms.Button btnGuardarProducto;
    }
}