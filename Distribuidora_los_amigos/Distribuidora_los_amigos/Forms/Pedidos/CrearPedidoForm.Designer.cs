namespace Distribuidora_los_amigos.Forms.Pedidos
{
    partial class CrearPedidoForm
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
            this.comboBoxSeleccionCliente = new System.Windows.Forms.ComboBox();
            this.dateTimePickerCrearPedido = new System.Windows.Forms.DateTimePicker();
            this.comboBoxEstadoPedido = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonAgregarProducto = new System.Windows.Forms.Button();
            this.buttonGuardarPedido = new System.Windows.Forms.Button();
            this.dataGridViewProductos = new System.Windows.Forms.DataGridView();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridViewDetallePedido = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetallePedido)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxSeleccionCliente
            // 
            this.comboBoxSeleccionCliente.FormattingEnabled = true;
            this.comboBoxSeleccionCliente.Location = new System.Drawing.Point(158, 22);
            this.comboBoxSeleccionCliente.Name = "comboBoxSeleccionCliente";
            this.comboBoxSeleccionCliente.Size = new System.Drawing.Size(196, 21);
            this.comboBoxSeleccionCliente.TabIndex = 0;
            // 
            // dateTimePickerCrearPedido
            // 
            this.dateTimePickerCrearPedido.Location = new System.Drawing.Point(158, 68);
            this.dateTimePickerCrearPedido.Name = "dateTimePickerCrearPedido";
            this.dateTimePickerCrearPedido.Size = new System.Drawing.Size(196, 20);
            this.dateTimePickerCrearPedido.TabIndex = 1;
            // 
            // comboBoxEstadoPedido
            // 
            this.comboBoxEstadoPedido.FormattingEnabled = true;
            this.comboBoxEstadoPedido.Location = new System.Drawing.Point(158, 119);
            this.comboBoxEstadoPedido.Name = "comboBoxEstadoPedido";
            this.comboBoxEstadoPedido.Size = new System.Drawing.Size(196, 21);
            this.comboBoxEstadoPedido.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Cliente";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Fecha del pedido";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Estado";
            // 
            // buttonAgregarProducto
            // 
            this.buttonAgregarProducto.Location = new System.Drawing.Point(89, 305);
            this.buttonAgregarProducto.Name = "buttonAgregarProducto";
            this.buttonAgregarProducto.Size = new System.Drawing.Size(121, 23);
            this.buttonAgregarProducto.TabIndex = 6;
            this.buttonAgregarProducto.Text = "Agregar producto";
            this.buttonAgregarProducto.UseVisualStyleBackColor = true;
            this.buttonAgregarProducto.Click += new System.EventHandler(this.buttonAgregarProducto_Click_1);
            // 
            // buttonGuardarPedido
            // 
            this.buttonGuardarPedido.Location = new System.Drawing.Point(216, 305);
            this.buttonGuardarPedido.Name = "buttonGuardarPedido";
            this.buttonGuardarPedido.Size = new System.Drawing.Size(121, 23);
            this.buttonGuardarPedido.TabIndex = 7;
            this.buttonGuardarPedido.Text = "Guardar pedido";
            this.buttonGuardarPedido.UseVisualStyleBackColor = true;
            this.buttonGuardarPedido.Click += new System.EventHandler(this.buttonGuardarPedido_Click_1);
            // 
            // dataGridViewProductos
            // 
            this.dataGridViewProductos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridViewProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProductos.Location = new System.Drawing.Point(406, 22);
            this.dataGridViewProductos.Name = "dataGridViewProductos";
            this.dataGridViewProductos.Size = new System.Drawing.Size(305, 150);
            this.dataGridViewProductos.TabIndex = 8;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(158, 169);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            -727379968,
            232,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(196, 20);
            this.numericUpDown1.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Cantidad";
            // 
            // dataGridViewDetallePedido
            // 
            this.dataGridViewDetallePedido.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDetallePedido.Location = new System.Drawing.Point(406, 178);
            this.dataGridViewDetallePedido.Name = "dataGridViewDetallePedido";
            this.dataGridViewDetallePedido.Size = new System.Drawing.Size(305, 150);
            this.dataGridViewDetallePedido.TabIndex = 11;
            // 
            // CrearPedidoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 381);
            this.Controls.Add(this.dataGridViewDetallePedido);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.dataGridViewProductos);
            this.Controls.Add(this.buttonGuardarPedido);
            this.Controls.Add(this.buttonAgregarProducto);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxEstadoPedido);
            this.Controls.Add(this.dateTimePickerCrearPedido);
            this.Controls.Add(this.comboBoxSeleccionCliente);
            this.Name = "CrearPedidoForm";
            this.Text = "CrearPedidoForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetallePedido)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxSeleccionCliente;
        private System.Windows.Forms.DateTimePicker dateTimePickerCrearPedido;
        private System.Windows.Forms.ComboBox comboBoxEstadoPedido;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonAgregarProducto;
        private System.Windows.Forms.Button buttonGuardarPedido;
        private System.Windows.Forms.DataGridView dataGridViewProductos;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridViewDetallePedido;
    }
}