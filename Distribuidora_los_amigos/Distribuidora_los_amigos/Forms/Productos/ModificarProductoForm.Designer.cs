namespace Distribuidora_los_amigos.Forms.Productos
{
    partial class ModificarProductoForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.textBoxNombreProducto = new System.Windows.Forms.TextBox();
            this.btnCancelarProducto = new System.Windows.Forms.Button();
            this.btnGuardarProducto = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownPrecioProducto = new System.Windows.Forms.NumericUpDown();
            this.Disponible = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPrecioProducto)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 1;
            this.label1.Tag = "Buscar Producto";
            this.label1.Text = "Buscar Producto";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(110, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(172, 21);
            this.comboBox1.TabIndex = 11;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(110, 65);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(172, 21);
            this.comboBox2.TabIndex = 23;
            // 
            // textBoxNombreProducto
            // 
            this.textBoxNombreProducto.Location = new System.Drawing.Point(110, 39);
            this.textBoxNombreProducto.Name = "textBoxNombreProducto";
            this.textBoxNombreProducto.Size = new System.Drawing.Size(172, 20);
            this.textBoxNombreProducto.TabIndex = 22;
            // 
            // btnCancelarProducto
            // 
            this.btnCancelarProducto.Location = new System.Drawing.Point(72, 196);
            this.btnCancelarProducto.Name = "btnCancelarProducto";
            this.btnCancelarProducto.Size = new System.Drawing.Size(75, 23);
            this.btnCancelarProducto.TabIndex = 21;
            this.btnCancelarProducto.Tag = "Cancelar";
            this.btnCancelarProducto.Text = "Cancelar";
            this.btnCancelarProducto.UseVisualStyleBackColor = true;
            // 
            // btnGuardarProducto
            // 
            this.btnGuardarProducto.Location = new System.Drawing.Point(183, 196);
            this.btnGuardarProducto.Name = "btnGuardarProducto";
            this.btnGuardarProducto.Size = new System.Drawing.Size(75, 23);
            this.btnGuardarProducto.TabIndex = 20;
            this.btnGuardarProducto.Tag = "Modificar";
            this.btnGuardarProducto.Text = "Modificar";
            this.btnGuardarProducto.UseVisualStyleBackColor = true;
            this.btnGuardarProducto.Click += new System.EventHandler(this.btnGuardarProducto_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 15;
            this.label2.Tag = "Categoria";
            this.label2.Text = "Categoria";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 14;
            this.label6.Tag = "Nombre";
            this.label6.Text = "Nombre";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(110, 147);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(195, 20);
            this.dateTimePicker2.TabIndex = 30;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(110, 121);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(195, 20);
            this.dateTimePicker1.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 28;
            this.label3.Tag = "Fecha Ingreso";
            this.label3.Text = "Fecha Ingreso";
            // 
            // numericUpDownPrecioProducto
            // 
            this.numericUpDownPrecioProducto.Location = new System.Drawing.Point(110, 92);
            this.numericUpDownPrecioProducto.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.numericUpDownPrecioProducto.Name = "numericUpDownPrecioProducto";
            this.numericUpDownPrecioProducto.Size = new System.Drawing.Size(172, 20);
            this.numericUpDownPrecioProducto.TabIndex = 27;
            // 
            // Disponible
            // 
            this.Disponible.AutoSize = true;
            this.Disponible.Location = new System.Drawing.Point(230, 173);
            this.Disponible.Name = "Disponible";
            this.Disponible.Size = new System.Drawing.Size(75, 17);
            this.Disponible.TabIndex = 26;
            this.Disponible.Tag = "Disponible";
            this.Disponible.Text = "Disponible";
            this.Disponible.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 25;
            this.label5.Tag = "Vencimiento";
            this.label5.Text = "Vencimiento";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 24;
            this.label4.Tag = "Precio";
            this.label4.Text = "Precio";
            // 
            // ModificarProductoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 239);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDownPrecioProducto);
            this.Controls.Add(this.Disponible);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.textBoxNombreProducto);
            this.Controls.Add(this.btnCancelarProducto);
            this.Controls.Add(this.btnGuardarProducto);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Name = "ModificarProductoForm";
            this.Tag = "ModificarProductoForm";
            this.Text = "ModificarProductoForm";
            this.Load += new System.EventHandler(this.ModificarProductoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPrecioProducto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.TextBox textBoxNombreProducto;
        private System.Windows.Forms.Button btnCancelarProducto;
        private System.Windows.Forms.Button btnGuardarProducto;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownPrecioProducto;
        private System.Windows.Forms.CheckBox Disponible;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
    }
}