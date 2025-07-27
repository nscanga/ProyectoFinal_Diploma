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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPedidos)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewPedidos
            // 
            this.dataGridViewPedidos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPedidos.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewPedidos.Name = "dataGridViewPedidos";
            this.dataGridViewPedidos.Size = new System.Drawing.Size(776, 383);
            this.dataGridViewPedidos.TabIndex = 0;
            // 
            // buttonCrearPedido
            // 
            this.buttonCrearPedido.Location = new System.Drawing.Point(470, 401);
            this.buttonCrearPedido.Name = "buttonCrearPedido";
            this.buttonCrearPedido.Size = new System.Drawing.Size(75, 23);
            this.buttonCrearPedido.TabIndex = 1;
            this.buttonCrearPedido.Text = "Crear";
            this.buttonCrearPedido.UseVisualStyleBackColor = true;
            this.buttonCrearPedido.Click += new System.EventHandler(this.buttonCrearPedido_Click_1);
            // 
            // buttonModificarPedido
            // 
            this.buttonModificarPedido.Location = new System.Drawing.Point(551, 401);
            this.buttonModificarPedido.Name = "buttonModificarPedido";
            this.buttonModificarPedido.Size = new System.Drawing.Size(75, 23);
            this.buttonModificarPedido.TabIndex = 2;
            this.buttonModificarPedido.Text = "Modificar";
            this.buttonModificarPedido.UseVisualStyleBackColor = true;
            this.buttonModificarPedido.Click += new System.EventHandler(this.buttonModificarPedido_Click);
            // 
            // buttonEliminarPedido
            // 
            this.buttonEliminarPedido.Location = new System.Drawing.Point(632, 401);
            this.buttonEliminarPedido.Name = "buttonEliminarPedido";
            this.buttonEliminarPedido.Size = new System.Drawing.Size(75, 23);
            this.buttonEliminarPedido.TabIndex = 3;
            this.buttonEliminarPedido.Text = "ELiminar";
            this.buttonEliminarPedido.UseVisualStyleBackColor = true;
            this.buttonEliminarPedido.Click += new System.EventHandler(this.buttonEliminarPedido_Click_1);
            // 
            // buttonVerDetalle
            // 
            this.buttonVerDetalle.Location = new System.Drawing.Point(713, 401);
            this.buttonVerDetalle.Name = "buttonVerDetalle";
            this.buttonVerDetalle.Size = new System.Drawing.Size(75, 23);
            this.buttonVerDetalle.TabIndex = 4;
            this.buttonVerDetalle.Text = "Ver Detalle";
            this.buttonVerDetalle.UseVisualStyleBackColor = true;
            this.buttonVerDetalle.Click += new System.EventHandler(this.buttonVerDetalle_Click);
            // 
            // buttonActualizarPedido
            // 
            this.buttonActualizarPedido.Location = new System.Drawing.Point(389, 401);
            this.buttonActualizarPedido.Name = "buttonActualizarPedido";
            this.buttonActualizarPedido.Size = new System.Drawing.Size(75, 23);
            this.buttonActualizarPedido.TabIndex = 5;
            this.buttonActualizarPedido.Text = "Actualizar";
            this.buttonActualizarPedido.UseVisualStyleBackColor = true;
            // 
            // MostrarPedidosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2162, 667);
            this.Controls.Add(this.buttonActualizarPedido);
            this.Controls.Add(this.buttonVerDetalle);
            this.Controls.Add(this.buttonEliminarPedido);
            this.Controls.Add(this.buttonModificarPedido);
            this.Controls.Add(this.buttonCrearPedido);
            this.Controls.Add(this.dataGridViewPedidos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MostrarPedidosForm";
            this.Text = "MostrarPedidosForm";
            this.Load += new System.EventHandler(this.MostrarPedidosForm_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPedidos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewPedidos;
        private System.Windows.Forms.Button buttonCrearPedido;
        private System.Windows.Forms.Button buttonModificarPedido;
        private System.Windows.Forms.Button buttonEliminarPedido;
        private System.Windows.Forms.Button buttonVerDetalle;
        private System.Windows.Forms.Button buttonActualizarPedido;
    }
}