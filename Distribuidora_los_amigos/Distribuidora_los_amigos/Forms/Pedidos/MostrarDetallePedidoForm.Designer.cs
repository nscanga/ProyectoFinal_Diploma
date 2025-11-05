namespace Distribuidora_los_amigos.Forms.Pedidos
{
    partial class MostrarDetallePedidoForm
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
            this.dataGridViewDetallePedido = new System.Windows.Forms.DataGridView();
            this.textBoxEstado = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxCliente = new System.Windows.Forms.TextBox();
            this.textBoxFecha = new System.Windows.Forms.TextBox();
            this.buttonCerrar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetallePedido)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewDetallePedido
            // 
            this.dataGridViewDetallePedido.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDetallePedido.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewDetallePedido.Name = "dataGridViewDetallePedido";
            this.dataGridViewDetallePedido.Size = new System.Drawing.Size(776, 300);
            this.dataGridViewDetallePedido.TabIndex = 0;
            // 
            // textBoxEstado
            // 
            this.textBoxEstado.Location = new System.Drawing.Point(189, 395);
            this.textBoxEstado.Name = "textBoxEstado";
            this.textBoxEstado.Size = new System.Drawing.Size(120, 20);
            this.textBoxEstado.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(114, 321);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 4;
            this.label1.Tag = "Cliente";
            this.label1.Text = "Cliente";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(114, 360);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 5;
            this.label2.Tag = "Fecha";
            this.label2.Text = "Fecha";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(115, 398);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 6;
            this.label3.Tag = "Estado";
            this.label3.Text = "Estado";
            // 
            // textBoxCliente
            // 
            this.textBoxCliente.Location = new System.Drawing.Point(190, 321);
            this.textBoxCliente.Name = "textBoxCliente";
            this.textBoxCliente.Size = new System.Drawing.Size(120, 20);
            this.textBoxCliente.TabIndex = 25;
            // 
            // textBoxFecha
            // 
            this.textBoxFecha.Location = new System.Drawing.Point(189, 357);
            this.textBoxFecha.Name = "textBoxFecha";
            this.textBoxFecha.Size = new System.Drawing.Size(120, 20);
            this.textBoxFecha.TabIndex = 26;
            // 
            // buttonCerrar
            // 
            this.buttonCerrar.Location = new System.Drawing.Point(713, 395);
            this.buttonCerrar.Name = "buttonCerrar";
            this.buttonCerrar.Size = new System.Drawing.Size(75, 23);
            this.buttonCerrar.TabIndex = 27;
            this.buttonCerrar.Tag = "Cerrar";
            this.buttonCerrar.Text = "Cerrar";
            this.buttonCerrar.UseVisualStyleBackColor = true;
            this.buttonCerrar.Click += new System.EventHandler(this.buttonCerrar_Click);
            // 
            // MostrarDetallePedidoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonCerrar);
            this.Controls.Add(this.textBoxFecha);
            this.Controls.Add(this.textBoxCliente);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxEstado);
            this.Controls.Add(this.dataGridViewDetallePedido);
            this.Name = "MostrarDetallePedidoForm";
            this.Tag = "MostrarDetallePedidoForm";
            this.Text = "MostrarDetallePedidoForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetallePedido)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewDetallePedido;
        private System.Windows.Forms.TextBox textBoxEstado;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxCliente;
        private System.Windows.Forms.TextBox textBoxFecha;
        private System.Windows.Forms.Button buttonCerrar;
    }
}