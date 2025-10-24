namespace Distribuidora_los_amigos.Forms.Reportes
{
    partial class ReporteStockBajoForm
    {
        private System.ComponentModel.IContainer components = null;
        private Syncfusion.WinForms.DataGrid.SfDataGrid sfDataGrid1;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelControles;
        private System.Windows.Forms.Label lblUmbral;
        private System.Windows.Forms.TextBox txtUmbral;
        private System.Windows.Forms.Button btnActualizarUmbral;
        private System.Windows.Forms.Button btnExportarExcel;
        private System.Windows.Forms.Button btnRefrescar;
        private System.Windows.Forms.Panel panelEstadisticas;
        private System.Windows.Forms.Label lblTotalProductos;
        private System.Windows.Forms.Label lblCriticos;
        private System.Windows.Forms.Label lblMuyBajos;
        private System.Windows.Forms.Label lblBajos;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.sfDataGrid1 = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelControles = new System.Windows.Forms.Panel();
            this.lblUmbral = new System.Windows.Forms.Label();
            this.txtUmbral = new System.Windows.Forms.TextBox();
            this.btnActualizarUmbral = new System.Windows.Forms.Button();
            this.btnExportarExcel = new System.Windows.Forms.Button();
            this.btnRefrescar = new System.Windows.Forms.Button();
            this.panelEstadisticas = new System.Windows.Forms.Panel();
            this.lblTotalProductos = new System.Windows.Forms.Label();
            this.lblCriticos = new System.Windows.Forms.Label();
            this.lblMuyBajos = new System.Windows.Forms.Label();
            this.lblBajos = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.panelControles.SuspendLayout();
            this.panelEstadisticas.SuspendLayout();
            this.SuspendLayout();
            
            // panelTop
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.panelTop.Controls.Add(this.lblTitulo);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1200, 60);
            this.panelTop.TabIndex = 0;
            
            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(250, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Tag = "ReporteStockBajo";
            this.lblTitulo.Text = "Reporte de Stock Bajo";
            
            // panelControles
            this.panelControles.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelControles.Controls.Add(this.lblUmbral);
            this.panelControles.Controls.Add(this.txtUmbral);
            this.panelControles.Controls.Add(this.btnActualizarUmbral);
            this.panelControles.Controls.Add(this.btnExportarExcel);
            this.panelControles.Controls.Add(this.btnRefrescar);
            this.panelControles.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControles.Location = new System.Drawing.Point(0, 60);
            this.panelControles.Name = "panelControles";
            this.panelControles.Size = new System.Drawing.Size(1200, 60);
            this.panelControles.TabIndex = 1;
            
            // lblUmbral
            this.lblUmbral.AutoSize = true;
            this.lblUmbral.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUmbral.Location = new System.Drawing.Point(20, 20);
            this.lblUmbral.Name = "lblUmbral";
            this.lblUmbral.Size = new System.Drawing.Size(120, 19);
            this.lblUmbral.TabIndex = 0;
            this.lblUmbral.Tag = "UmbralStockBajo";
            this.lblUmbral.Text = "Umbral Stock Bajo:";
            
            // txtUmbral
            this.txtUmbral.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtUmbral.Location = new System.Drawing.Point(150, 17);
            this.txtUmbral.Name = "txtUmbral";
            this.txtUmbral.Size = new System.Drawing.Size(80, 25);
            this.txtUmbral.TabIndex = 1;
            this.txtUmbral.Text = "10";
            
            // btnActualizarUmbral
            this.btnActualizarUmbral.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnActualizarUmbral.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActualizarUmbral.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnActualizarUmbral.ForeColor = System.Drawing.Color.White;
            this.btnActualizarUmbral.Location = new System.Drawing.Point(240, 15);
            this.btnActualizarUmbral.Name = "btnActualizarUmbral";
            this.btnActualizarUmbral.Size = new System.Drawing.Size(100, 30);
            this.btnActualizarUmbral.TabIndex = 2;
            this.btnActualizarUmbral.Tag = "Actualizar";
            this.btnActualizarUmbral.Text = "Actualizar";
            this.btnActualizarUmbral.UseVisualStyleBackColor = false;
            this.btnActualizarUmbral.Click += new System.EventHandler(this.btnActualizarUmbral_Click);
            
            // btnRefrescar
            this.btnRefrescar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnRefrescar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefrescar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRefrescar.ForeColor = System.Drawing.Color.White;
            this.btnRefrescar.Location = new System.Drawing.Point(360, 15);
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Size = new System.Drawing.Size(100, 30);
            this.btnRefrescar.TabIndex = 3;
            this.btnRefrescar.Tag = "Refrescar";
            this.btnRefrescar.Text = "Refrescar";
            this.btnRefrescar.UseVisualStyleBackColor = false;
            this.btnRefrescar.Click += new System.EventHandler((s, e) => CargarReporte());
            
            // btnExportarExcel
            this.btnExportarExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(83)))));
            this.btnExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarExcel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnExportarExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportarExcel.Location = new System.Drawing.Point(480, 15);
            this.btnExportarExcel.Name = "btnExportarExcel";
            this.btnExportarExcel.Size = new System.Drawing.Size(120, 30);
            this.btnExportarExcel.TabIndex = 4;
            this.btnExportarExcel.Tag = "ExportarExcel";
            this.btnExportarExcel.Text = "Exportar a Excel";
            this.btnExportarExcel.UseVisualStyleBackColor = false;
            this.btnExportarExcel.Click += new System.EventHandler(this.btnExportarExcel_Click);
            
            // panelEstadisticas
            this.panelEstadisticas.BackColor = System.Drawing.Color.White;
            this.panelEstadisticas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelEstadisticas.Controls.Add(this.lblTotalProductos);
            this.panelEstadisticas.Controls.Add(this.lblCriticos);
            this.panelEstadisticas.Controls.Add(this.lblMuyBajos);
            this.panelEstadisticas.Controls.Add(this.lblBajos);
            this.panelEstadisticas.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEstadisticas.Location = new System.Drawing.Point(0, 120);
            this.panelEstadisticas.Name = "panelEstadisticas";
            this.panelEstadisticas.Padding = new System.Windows.Forms.Padding(10);
            this.panelEstadisticas.Size = new System.Drawing.Size(1200, 50);
            this.panelEstadisticas.TabIndex = 2;
            
            // lblTotalProductos
            this.lblTotalProductos.AutoSize = true;
            this.lblTotalProductos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalProductos.Location = new System.Drawing.Point(20, 15);
            this.lblTotalProductos.Name = "lblTotalProductos";
            this.lblTotalProductos.Size = new System.Drawing.Size(150, 19);
            this.lblTotalProductos.TabIndex = 0;
            this.lblTotalProductos.Text = "Total productos: 0";
            
            // lblCriticos
            this.lblCriticos.AutoSize = true;
            this.lblCriticos.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCriticos.ForeColor = System.Drawing.Color.Red;
            this.lblCriticos.Location = new System.Drawing.Point(250, 15);
            this.lblCriticos.Name = "lblCriticos";
            this.lblCriticos.Size = new System.Drawing.Size(80, 19);
            this.lblCriticos.TabIndex = 1;
            this.lblCriticos.Text = "Críticos: 0";
            
            // lblMuyBajos
            this.lblMuyBajos.AutoSize = true;
            this.lblMuyBajos.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblMuyBajos.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblMuyBajos.Location = new System.Drawing.Point(400, 15);
            this.lblMuyBajos.Name = "lblMuyBajos";
            this.lblMuyBajos.Size = new System.Drawing.Size(100, 19);
            this.lblMuyBajos.TabIndex = 2;
            this.lblMuyBajos.Text = "Muy Bajos: 0";
            
            // lblBajos
            this.lblBajos.AutoSize = true;
            this.lblBajos.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBajos.ForeColor = System.Drawing.Color.Orange;
            this.lblBajos.Location = new System.Drawing.Point(570, 15);
            this.lblBajos.Name = "lblBajos";
            this.lblBajos.Size = new System.Drawing.Size(70, 19);
            this.lblBajos.TabIndex = 3;
            this.lblBajos.Text = "Bajos: 0";
            
            // sfDataGrid1
            this.sfDataGrid1.AccessibleName = "Table";
            this.sfDataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sfDataGrid1.Location = new System.Drawing.Point(0, 170);
            this.sfDataGrid1.Name = "sfDataGrid1";
            this.sfDataGrid1.Size = new System.Drawing.Size(1200, 530);
            this.sfDataGrid1.TabIndex = 3;
            
            // ReporteStockBajoForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.sfDataGrid1);
            this.Controls.Add(this.panelEstadisticas);
            this.Controls.Add(this.panelControles);
            this.Controls.Add(this.panelTop);
            this.Name = "ReporteStockBajoForm";
            this.Text = "Reporte de Stock Bajo";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelControles.ResumeLayout(false);
            this.panelControles.PerformLayout();
            this.panelEstadisticas.ResumeLayout(false);
            this.panelEstadisticas.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
