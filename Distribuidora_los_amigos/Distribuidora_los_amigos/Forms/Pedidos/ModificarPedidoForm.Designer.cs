namespace Distribuidora_los_amigos.Forms.Pedidos
{
    partial class ModificarPedidoForm
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
            this.buttonGuardarPedido = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxEstadoPedido = new System.Windows.Forms.ComboBox();
            this.dateTimePickerCrearPedido = new System.Windows.Forms.DateTimePicker();
            this.comboBoxSeleccionCliente = new System.Windows.Forms.ComboBox();
            this.textBoxIdPedido = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonEliminarPedido = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridViewProductos = new System.Windows.Forms.DataGridView();
            this.dataGridViewDetallePedido = new System.Windows.Forms.DataGridView();
            this.buttonAgregarProducto = new System.Windows.Forms.Button();
            this.buttonQuitarProducto = new System.Windows.Forms.Button();
            this.buttonModificarCantidad = new System.Windows.Forms.Button();
            this.labelTotal = new System.Windows.Forms.Label();
            this.labelTotalValor = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetallePedido)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonGuardarPedido
            // 
            this.buttonGuardarPedido.Location = new System.Drawing.Point(245, 411);
            this.buttonGuardarPedido.Name = "buttonGuardarPedido";
            this.buttonGuardarPedido.Size = new System.Drawing.Size(107, 30);
            this.buttonGuardarPedido.TabIndex = 16;
            this.buttonGuardarPedido.Tag = "Guardar cambios";
            this.buttonGuardarPedido.Text = "Guardar cambios";
            this.buttonGuardarPedido.UseVisualStyleBackColor = true;
            this.buttonGuardarPedido.Click += new System.EventHandler(this.buttonGuardarPedido_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 14;
            this.label3.Tag = "Estado";
            this.label3.Text = "Estado";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 13;
            this.label2.Tag = "Fecha del pedido";
            this.label2.Text = "Fecha del pedido";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 12;
            this.label1.Tag = "Cliente";
            this.label1.Text = "Cliente";
            // 
            // comboBoxEstadoPedido
            // 
            this.comboBoxEstadoPedido.FormattingEnabled = true;
            this.comboBoxEstadoPedido.Location = new System.Drawing.Point(146, 120);
            this.comboBoxEstadoPedido.Name = "comboBoxEstadoPedido";
            this.comboBoxEstadoPedido.Size = new System.Drawing.Size(196, 21);
            this.comboBoxEstadoPedido.TabIndex = 11;
            // 
            // dateTimePickerCrearPedido
            // 
            this.dateTimePickerCrearPedido.Enabled = false;
            this.dateTimePickerCrearPedido.Location = new System.Drawing.Point(146, 94);
            this.dateTimePickerCrearPedido.Name = "dateTimePickerCrearPedido";
            this.dateTimePickerCrearPedido.Size = new System.Drawing.Size(196, 20);
            this.dateTimePickerCrearPedido.TabIndex = 10;
            // 
            // comboBoxSeleccionCliente
            // 
            this.comboBoxSeleccionCliente.FormattingEnabled = true;
            this.comboBoxSeleccionCliente.Location = new System.Drawing.Point(146, 67);
            this.comboBoxSeleccionCliente.Name = "comboBoxSeleccionCliente";
            this.comboBoxSeleccionCliente.Size = new System.Drawing.Size(196, 21);
            this.comboBoxSeleccionCliente.TabIndex = 9;
            // 
            // textBoxIdPedido
            // 
            this.textBoxIdPedido.Enabled = false;
            this.textBoxIdPedido.Location = new System.Drawing.Point(146, 41);
            this.textBoxIdPedido.Name = "textBoxIdPedido";
            this.textBoxIdPedido.Size = new System.Drawing.Size(196, 20);
            this.textBoxIdPedido.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 19;
            this.label4.Tag = "Id Pedido";
            this.label4.Text = "Id Pedido";
            // 
            // buttonEliminarPedido
            // 
            this.buttonEliminarPedido.Location = new System.Drawing.Point(132, 411);
            this.buttonEliminarPedido.Name = "buttonEliminarPedido";
            this.buttonEliminarPedido.Size = new System.Drawing.Size(107, 30);
            this.buttonEliminarPedido.TabIndex = 20;
            this.buttonEliminarPedido.Tag = "Eliminar pedido";
            this.buttonEliminarPedido.Text = "Eliminar pedido";
            this.buttonEliminarPedido.UseVisualStyleBackColor = true;
            this.buttonEliminarPedido.Click += new System.EventHandler(this.buttonEliminarPedido_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(146, 15);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(196, 20);
            this.numericUpDown1.TabIndex = 21;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 22;
            this.label5.Tag = "Cantidad";
            this.label5.Text = "Cantidad";
            // 
            // dataGridViewProductos
            // 
            this.dataGridViewProductos.AllowUserToAddRows = false;
            this.dataGridViewProductos.AllowUserToDeleteRows = false;
            this.dataGridViewProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProductos.Location = new System.Drawing.Point(379, 41);
            this.dataGridViewProductos.MultiSelect = false;
            this.dataGridViewProductos.Name = "dataGridViewProductos";
            this.dataGridViewProductos.ReadOnly = true;
            this.dataGridViewProductos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewProductos.Size = new System.Drawing.Size(500, 150);
            this.dataGridViewProductos.TabIndex = 23;
            // 
            // dataGridViewDetallePedido
            // 
            this.dataGridViewDetallePedido.AllowUserToAddRows = false;
            this.dataGridViewDetallePedido.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDetallePedido.Location = new System.Drawing.Point(379, 222);
            this.dataGridViewDetallePedido.MultiSelect = false;
            this.dataGridViewDetallePedido.Name = "dataGridViewDetallePedido";
            this.dataGridViewDetallePedido.ReadOnly = true;
            this.dataGridViewDetallePedido.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewDetallePedido.Size = new System.Drawing.Size(500, 183);
            this.dataGridViewDetallePedido.TabIndex = 24;
            // 
            // buttonAgregarProducto
            // 
            this.buttonAgregarProducto.Location = new System.Drawing.Point(30, 160);
            this.buttonAgregarProducto.Name = "buttonAgregarProducto";
            this.buttonAgregarProducto.Size = new System.Drawing.Size(145, 30);
            this.buttonAgregarProducto.TabIndex = 25;
            this.buttonAgregarProducto.Tag = "Agregar producto";
            this.buttonAgregarProducto.Text = "Agregar producto";
            this.buttonAgregarProducto.UseVisualStyleBackColor = true;
            this.buttonAgregarProducto.Click += new System.EventHandler(this.buttonAgregarProducto_Click);
            // 
            // buttonQuitarProducto
            // 
            this.buttonQuitarProducto.Location = new System.Drawing.Point(181, 160);
            this.buttonQuitarProducto.Name = "buttonQuitarProducto";
            this.buttonQuitarProducto.Size = new System.Drawing.Size(145, 30);
            this.buttonQuitarProducto.TabIndex = 26;
            this.buttonQuitarProducto.Tag = "Quitar producto";
            this.buttonQuitarProducto.Text = "Quitar producto";
            this.buttonQuitarProducto.UseVisualStyleBackColor = true;
            this.buttonQuitarProducto.Click += new System.EventHandler(this.buttonQuitarProducto_Click);
            // 
            // buttonModificarCantidad
            // 
            this.buttonModificarCantidad.Location = new System.Drawing.Point(30, 196);
            this.buttonModificarCantidad.Name = "buttonModificarCantidad";
            this.buttonModificarCantidad.Size = new System.Drawing.Size(296, 30);
            this.buttonModificarCantidad.TabIndex = 27;
            this.buttonModificarCantidad.Tag = "Modificar cantidad";
            this.buttonModificarCantidad.Text = "Modificar cantidad";
            this.buttonModificarCantidad.UseVisualStyleBackColor = true;
            this.buttonModificarCantidad.Click += new System.EventHandler(this.buttonModificarCantidad_Click);
            // 
            // labelTotal
            // 
            this.labelTotal.AutoSize = true;
            this.labelTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotal.Location = new System.Drawing.Point(27, 240);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(109, 17);
            this.labelTotal.TabIndex = 28;
            this.labelTotal.Tag = "Total del pedido:";
            this.labelTotal.Text = "Total pedido:";
            // 
            // labelTotalValor
            // 
            this.labelTotalValor.AutoSize = true;
            this.labelTotalValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalValor.ForeColor = System.Drawing.Color.Green;
            this.labelTotalValor.Location = new System.Drawing.Point(143, 240);
            this.labelTotalValor.Name = "labelTotalValor";
            this.labelTotalValor.Size = new System.Drawing.Size(49, 17);
            this.labelTotalValor.TabIndex = 29;
            this.labelTotalValor.Text = "$0.00";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(376, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(155, 15);
            this.label6.TabIndex = 30;
            this.label6.Tag = "Productos disponibles";
            this.label6.Text = "Productos disponibles";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(376, 201);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 15);
            this.label7.TabIndex = 31;
            this.label7.Tag = "Detalle del pedido";
            this.label7.Text = "Detalle del pedido";
            // 
            // ModificarPedidoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 461);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.labelTotalValor);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.buttonModificarCantidad);
            this.Controls.Add(this.buttonQuitarProducto);
            this.Controls.Add(this.buttonAgregarProducto);
            this.Controls.Add(this.dataGridViewDetallePedido);
            this.Controls.Add(this.dataGridViewProductos);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.buttonEliminarPedido);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxIdPedido);
            this.Controls.Add(this.buttonGuardarPedido);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxEstadoPedido);
            this.Controls.Add(this.dateTimePickerCrearPedido);
            this.Controls.Add(this.comboBoxSeleccionCliente);
            this.MinimumSize = new System.Drawing.Size(920, 500);
            this.Name = "ModificarPedidoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "Modificar Pedido";
            this.Text = "Modificar Pedido";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetallePedido)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonGuardarPedido;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxEstadoPedido;
        private System.Windows.Forms.DateTimePicker dateTimePickerCrearPedido;
        private System.Windows.Forms.ComboBox comboBoxSeleccionCliente;
        private System.Windows.Forms.TextBox textBoxIdPedido;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonEliminarPedido;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dataGridViewProductos;
        private System.Windows.Forms.DataGridView dataGridViewDetallePedido;
        private System.Windows.Forms.Button buttonAgregarProducto;
        private System.Windows.Forms.Button buttonQuitarProducto;
        private System.Windows.Forms.Button buttonModificarCantidad;
        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.Label labelTotalValor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}