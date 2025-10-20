using System.Windows.Forms;

namespace Distribuidora_los_amigos.Forms.Pedidos
{
    partial class MostrarPedidosForm
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
            this.dataGridViewPedidos = new System.Windows.Forms.DataGridView();
            this.buttonCrearPedido = new System.Windows.Forms.Button();
            this.buttonModificarPedido = new System.Windows.Forms.Button();
            this.buttonEliminarPedido = new System.Windows.Forms.Button();
            this.buttonVerDetalle = new System.Windows.Forms.Button();
            this.buttonActualizarPedido = new System.Windows.Forms.Button();
            this.lblContadorPedidos = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPedidos)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewPedidos
            // 
            this.dataGridViewPedidos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPedidos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPedidos.Location = new System.Drawing.Point(12, 50);
            this.dataGridViewPedidos.Name = "dataGridViewPedidos";
            this.dataGridViewPedidos.Size = new System.Drawing.Size(960, 350);
            this.dataGridViewPedidos.TabIndex = 0;
            // 
            // lblContadorPedidos
            // 
            this.lblContadorPedidos.AutoSize = true;
            this.lblContadorPedidos.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblContadorPedidos.Location = new System.Drawing.Point(12, 20);
            this.lblContadorPedidos.Name = "lblContadorPedidos";
            this.lblContadorPedidos.Size = new System.Drawing.Size(150, 17);
            this.lblContadorPedidos.TabIndex = 6;
            this.lblContadorPedidos.Text = "Total de pedidos: 0";
            // 
            // buttonActualizarPedido
            // 
            this.buttonActualizarPedido.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonActualizarPedido.Location = new System.Drawing.Point(572, 420);
            this.buttonActualizarPedido.Name = "buttonActualizarPedido";
            this.buttonActualizarPedido.Size = new System.Drawing.Size(90, 30);
            this.buttonActualizarPedido.TabIndex = 5;
            this.buttonActualizarPedido.Text = "🔄 Actualizar";
            this.buttonActualizarPedido.UseVisualStyleBackColor = true;
            this.buttonActualizarPedido.Click += new System.EventHandler(this.buttonActualizarPedido_Click);
            // 
            // buttonCrearPedido
            // 
            this.buttonCrearPedido.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCrearPedido.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.buttonCrearPedido.ForeColor = System.Drawing.Color.White;
            this.buttonCrearPedido.Location = new System.Drawing.Point(668, 420);
            this.buttonCrearPedido.Name = "buttonCrearPedido";
            this.buttonCrearPedido.Size = new System.Drawing.Size(90, 30);
            this.buttonCrearPedido.TabIndex = 1;
            this.buttonCrearPedido.Text = "➕ Crear";
            this.buttonCrearPedido.UseVisualStyleBackColor = false;
            this.buttonCrearPedido.Click += new System.EventHandler(this.buttonCrearPedido_Click_1);
            // 
            // buttonModificarPedido
            // 
            this.buttonModificarPedido.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonModificarPedido.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.buttonModificarPedido.ForeColor = System.Drawing.Color.Black;
            this.buttonModificarPedido.Location = new System.Drawing.Point(764, 420);
            this.buttonModificarPedido.Name = "buttonModificarPedido";
            this.buttonModificarPedido.Size = new System.Drawing.Size(90, 30);
            this.buttonModificarPedido.TabIndex = 2;
            this.buttonModificarPedido.Text = "✏️ Modificar";
            this.buttonModificarPedido.UseVisualStyleBackColor = false;
            this.buttonModificarPedido.Click += new System.EventHandler(this.buttonModificarPedido_Click);
            // 
            // buttonEliminarPedido
            // 
            this.buttonEliminarPedido.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEliminarPedido.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.buttonEliminarPedido.ForeColor = System.Drawing.Color.White;
            this.buttonEliminarPedido.Location = new System.Drawing.Point(860, 420);
            this.buttonEliminarPedido.Name = "buttonEliminarPedido";
            this.buttonEliminarPedido.Size = new System.Drawing.Size(90, 30);
            this.buttonEliminarPedido.TabIndex = 3;
            this.buttonEliminarPedido.Text = "🗑️ Eliminar";
            this.buttonEliminarPedido.UseVisualStyleBackColor = false;
            this.buttonEliminarPedido.Click += new System.EventHandler(this.buttonEliminarPedido_Click_1);
            // 
            // buttonVerDetalle
            // 
            this.buttonVerDetalle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonVerDetalle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(184)))));
            this.buttonVerDetalle.ForeColor = System.Drawing.Color.White;
            this.buttonVerDetalle.Location = new System.Drawing.Point(12, 420);
            this.buttonVerDetalle.Name = "buttonVerDetalle";
            this.buttonVerDetalle.Size = new System.Drawing.Size(100, 30);
            this.buttonVerDetalle.TabIndex = 4;
            this.buttonVerDetalle.Text = "👁️ Ver Detalle";
            this.buttonVerDetalle.UseVisualStyleBackColor = false;
            this.buttonVerDetalle.Click += new System.EventHandler(this.buttonVerDetalle_Click);
            // 
            // MostrarPedidosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(984, 471);
            this.Controls.Add(this.lblContadorPedidos);
            this.Controls.Add(this.buttonActualizarPedido);
            this.Controls.Add(this.buttonVerDetalle);
            this.Controls.Add(this.buttonEliminarPedido);
            this.Controls.Add(this.buttonModificarPedido);
            this.Controls.Add(this.buttonCrearPedido);
            this.Controls.Add(this.dataGridViewPedidos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(800, 400);
            this.Name = "MostrarPedidosForm";
            this.Text = "Gestión de Pedidos";
            this.Load += new System.EventHandler(this.MostrarPedidosForm_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPedidos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewPedidos;
        private System.Windows.Forms.Button buttonCrearPedido;
        private System.Windows.Forms.Button buttonModificarPedido;
        private System.Windows.Forms.Button buttonEliminarPedido;
        private System.Windows.Forms.Button buttonVerDetalle;
        private System.Windows.Forms.Button buttonActualizarPedido;
        private System.Windows.Forms.Label lblContadorPedidos;
    }
}