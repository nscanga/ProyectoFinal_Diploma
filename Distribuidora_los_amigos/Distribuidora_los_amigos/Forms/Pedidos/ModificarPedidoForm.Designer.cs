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
            this.buttonModificarProducto = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxEstadoPedido = new System.Windows.Forms.ComboBox();
            this.dateTimePickerCrearPedido = new System.Windows.Forms.DateTimePicker();
            this.comboBoxSeleccionCliente = new System.Windows.Forms.ComboBox();
            this.textBoxIdPedido = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonEliminarProducto = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonGuardarPedido
            // 
            this.buttonGuardarPedido.Location = new System.Drawing.Point(268, 164);
            this.buttonGuardarPedido.Name = "buttonGuardarPedido";
            this.buttonGuardarPedido.Size = new System.Drawing.Size(107, 23);
            this.buttonGuardarPedido.TabIndex = 16;
            this.buttonGuardarPedido.Tag = "Guardar pedido";
            this.buttonGuardarPedido.Text = "Guardar pedido";
            this.buttonGuardarPedido.UseVisualStyleBackColor = true;
            this.buttonGuardarPedido.Click += new System.EventHandler(this.buttonGuardarPedido_Click);
            // 
            // buttonModificarProducto
            // 
            this.buttonModificarProducto.Location = new System.Drawing.Point(42, 164);
            this.buttonModificarProducto.Name = "buttonModificarProducto";
            this.buttonModificarProducto.Size = new System.Drawing.Size(107, 23);
            this.buttonModificarProducto.TabIndex = 15;
            this.buttonModificarProducto.Tag = "Modificar Pedido";
            this.buttonModificarProducto.Text = "Modificar Pedido";
            this.buttonModificarProducto.UseVisualStyleBackColor = true;
            this.buttonModificarProducto.Click += new System.EventHandler(this.buttonModificarProducto_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(63, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 14;
            this.label3.Tag = "Estado";
            this.label3.Text = "Estado";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 13;
            this.label2.Tag = "Fecha del pedido";
            this.label2.Text = "Fecha del pedido";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 12;
            this.label1.Tag = "Cliente";
            this.label1.Text = "Cliente";
            // 
            // comboBoxEstadoPedido
            // 
            this.comboBoxEstadoPedido.FormattingEnabled = true;
            this.comboBoxEstadoPedido.Location = new System.Drawing.Point(167, 120);
            this.comboBoxEstadoPedido.Name = "comboBoxEstadoPedido";
            this.comboBoxEstadoPedido.Size = new System.Drawing.Size(196, 21);
            this.comboBoxEstadoPedido.TabIndex = 11;
            // 
            // dateTimePickerCrearPedido
            // 
            this.dateTimePickerCrearPedido.Location = new System.Drawing.Point(167, 94);
            this.dateTimePickerCrearPedido.Name = "dateTimePickerCrearPedido";
            this.dateTimePickerCrearPedido.Size = new System.Drawing.Size(196, 20);
            this.dateTimePickerCrearPedido.TabIndex = 10;
            // 
            // comboBoxSeleccionCliente
            // 
            this.comboBoxSeleccionCliente.FormattingEnabled = true;
            this.comboBoxSeleccionCliente.Location = new System.Drawing.Point(167, 67);
            this.comboBoxSeleccionCliente.Name = "comboBoxSeleccionCliente";
            this.comboBoxSeleccionCliente.Size = new System.Drawing.Size(196, 21);
            this.comboBoxSeleccionCliente.TabIndex = 9;
            // 
            // textBoxIdPedido
            // 
            this.textBoxIdPedido.Location = new System.Drawing.Point(167, 41);
            this.textBoxIdPedido.Name = "textBoxIdPedido";
            this.textBoxIdPedido.Size = new System.Drawing.Size(196, 20);
            this.textBoxIdPedido.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(64, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 19;
            this.label4.Tag = "Id Pedido";
            this.label4.Text = "Id Pedido";
            // 
            // buttonEliminarProducto
            // 
            this.buttonEliminarProducto.Location = new System.Drawing.Point(155, 164);
            this.buttonEliminarProducto.Name = "buttonEliminarProducto";
            this.buttonEliminarProducto.Size = new System.Drawing.Size(107, 23);
            this.buttonEliminarProducto.TabIndex = 20;
            this.buttonEliminarProducto.Tag = "Eliminar producto";
            this.buttonEliminarProducto.Text = "Eliminar producto";
            this.buttonEliminarProducto.UseVisualStyleBackColor = true;
            this.buttonEliminarProducto.Click += new System.EventHandler(this.buttonEliminarProducto_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(167, 15);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(196, 20);
            this.numericUpDown1.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(64, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 22;
            this.label5.Tag = "Cantidad";
            this.label5.Text = "Cantidad";
            // 
            // ModificarPedidoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 212);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.buttonEliminarProducto);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxIdPedido);
            this.Controls.Add(this.buttonGuardarPedido);
            this.Controls.Add(this.buttonModificarProducto);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxEstadoPedido);
            this.Controls.Add(this.dateTimePickerCrearPedido);
            this.Controls.Add(this.comboBoxSeleccionCliente);
            this.Name = "ModificarPedidoForm";
            this.Tag = "ModificarPedidoForm";
            this.Text = "ModificarPedidoForm";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonGuardarPedido;
        private System.Windows.Forms.Button buttonModificarProducto;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxEstadoPedido;
        private System.Windows.Forms.DateTimePicker dateTimePickerCrearPedido;
        private System.Windows.Forms.ComboBox comboBoxSeleccionCliente;
        private System.Windows.Forms.TextBox textBoxIdPedido;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonEliminarProducto;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label5;
    }
}