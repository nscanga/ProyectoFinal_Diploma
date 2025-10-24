namespace Distribuidora_los_amigos.Forms.Reportes
{
    partial class ReporteProductosMasVendidosForm
    {
        private System.ComponentModel.IContainer components = null;
        private Syncfusion.WinForms.DataGrid.SfDataGrid sfDataGrid1;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelControles;
        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Label lblTop;
        private System.Windows.Forms.TextBox txtTopProductos;
        private System.Windows.Forms.Button btnActualizarTop;
        private System.Windows.Forms.Button btnExportarExcel;
        private System.Windows.Forms.Panel panelEstadisticas;
        private System.Windows.Forms.Label lblTotalVentas;
        private System.Windows.Forms.Label lblTotalUnidades;
        private System.Windows.Forms.Label lblProductosUnicos;
        private System.Windows.Forms.Label lblPeriodo;

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
            this.lblDesde = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.lblTop = new System.Windows.Forms.Label();
            this.txtTopProductos = new System.Windows.Forms.TextBox();
            this.btnActualizarTop = new System.Windows.Forms.Button();
            this.btnExportarExcel = new System.Windows.Forms.Button();
            this.panelEstadisticas = new System.Windows.Forms.Panel();
            this.lblTotalVentas = new System.Windows.Forms.Label();
            this.lblTotalUnidades = new System.Windows.Forms.Label();
            this.lblProductosUnicos = new System.Windows.Forms.Label();
            this.lblPeriodo = new System.Windows.Forms.Label();
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
            this.lblTitulo.Size = new System.Drawing.Size(350, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Tag = "ReporteProductosMasVendidos";
            this.lblTitulo.Text = "Productos Más Vendidos";
            
            // panelControles
            this.panelControles.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelControles.Controls.Add(this.lblDesde);
            this.panelControles.Controls.Add(this.dtpDesde);
            this.panelControles.Controls.Add(this.lblHasta);
            this.panelControles.Controls.Add(this.dtpHasta);
            this.panelControles.Controls.Add(this.btnFiltrar);
            this.panelControles.Controls.Add(this.lblTop);
            this.panelControles.Controls.Add(this.txtTopProductos);
            this.panelControles.Controls.Add(this.btnActualizarTop);
            this.panelControles.Controls.Add(this.btnExportarExcel);
            this.panelControles.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControles.Location = new System.Drawing.Point(0, 60);
            this.panelControles.Name = "panelControles";
            this.panelControles.Size = new System.Drawing.Size(1200, 70);
            this.panelControles.TabIndex = 1;
            
            // lblDesde
            this.lblDesde.AutoSize = true;
            this.lblDesde.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDesde.Location = new System.Drawing.Point(20, 15);
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.Size = new System.Drawing.Size(50, 19);
            this.lblDesde.TabIndex = 0;
            this.lblDesde.Tag = "Desde";
            this.lblDesde.Text = "Desde:";
            
            // dtpDesde
            this.dtpDesde.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Location = new System.Drawing.Point(80, 12);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(120, 25);
            this.dtpDesde.TabIndex = 1;
            
            // lblHasta
            this.lblHasta.AutoSize = true;
            this.lblHasta.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblHasta.Location = new System.Drawing.Point(220, 15);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.Size = new System.Drawing.Size(50, 19);
            this.lblHasta.TabIndex = 2;
            this.lblHasta.Tag = "Hasta";
            this.lblHasta.Text = "Hasta:";
            
            // dtpHasta
            this.dtpHasta.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Location = new System.Drawing.Point(280, 12);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(120, 25);
            this.dtpHasta.TabIndex = 3;
            
            // btnFiltrar
            this.btnFiltrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnFiltrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFiltrar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnFiltrar.ForeColor = System.Drawing.Color.White;
            this.btnFiltrar.Location = new System.Drawing.Point(420, 10);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(90, 30);
            this.btnFiltrar.TabIndex = 4;
            this.btnFiltrar.Tag = "Filtrar";
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = false;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            
            // lblTop
            this.lblTop.AutoSize = true;
            this.lblTop.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTop.Location = new System.Drawing.Point(20, 45);
            this.lblTop.Name = "lblTop";
            this.lblTop.Size = new System.Drawing.Size(100, 19);
            this.lblTop.TabIndex = 5;
            this.lblTop.Tag = "TopProductos";
            this.lblTop.Text = "Top Productos:";
            
            // txtTopProductos
            this.txtTopProductos.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTopProductos.Location = new System.Drawing.Point(130, 42);
            this.txtTopProductos.Name = "txtTopProductos";
            this.txtTopProductos.Size = new System.Drawing.Size(70, 25);
            this.txtTopProductos.TabIndex = 6;
            this.txtTopProductos.Text = "10";
            
            // btnActualizarTop
            this.btnActualizarTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnActualizarTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActualizarTop.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnActualizarTop.ForeColor = System.Drawing.Color.White;
            this.btnActualizarTop.Location = new System.Drawing.Point(210, 40);
            this.btnActualizarTop.Name = "btnActualizarTop";
            this.btnActualizarTop.Size = new System.Drawing.Size(100, 30);
            this.btnActualizarTop.TabIndex = 7;
            this.btnActualizarTop.Tag = "Actualizar";
            this.btnActualizarTop.Text = "Actualizar";
            this.btnActualizarTop.UseVisualStyleBackColor = false;
            this.btnActualizarTop.Click += new System.EventHandler(this.btnActualizarTop_Click);
            
            // btnExportarExcel
            this.btnExportarExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(83)))));
            this.btnExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarExcel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnExportarExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportarExcel.Location = new System.Drawing.Point(330, 40);
            this.btnExportarExcel.Name = "btnExportarExcel";
            this.btnExportarExcel.Size = new System.Drawing.Size(120, 30);
            this.btnExportarExcel.TabIndex = 8;
            this.btnExportarExcel.Tag = "ExportarExcel";
            this.btnExportarExcel.Text = "Exportar a Excel";
            this.btnExportarExcel.UseVisualStyleBackColor = false;
            this.btnExportarExcel.Click += new System.EventHandler(this.btnExportarExcel_Click);
            
            // panelEstadisticas
            this.panelEstadisticas.BackColor = System.Drawing.Color.White;
            this.panelEstadisticas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelEstadisticas.Controls.Add(this.lblTotalVentas);
            this.panelEstadisticas.Controls.Add(this.lblTotalUnidades);
            this.panelEstadisticas.Controls.Add(this.lblProductosUnicos);
            this.panelEstadisticas.Controls.Add(this.lblPeriodo);
            this.panelEstadisticas.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEstadisticas.Location = new System.Drawing.Point(0, 130);
            this.panelEstadisticas.Name = "panelEstadisticas";
            this.panelEstadisticas.Padding = new System.Windows.Forms.Padding(10);
            this.panelEstadisticas.Size = new System.Drawing.Size(1200, 50);
            this.panelEstadisticas.TabIndex = 2;
            
            // lblTotalVentas
            this.lblTotalVentas.AutoSize = true;
            this.lblTotalVentas.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalVentas.ForeColor = System.Drawing.Color.Green;
            this.lblTotalVentas.Location = new System.Drawing.Point(20, 15);
            this.lblTotalVentas.Name = "lblTotalVentas";
            this.lblTotalVentas.Size = new System.Drawing.Size(120, 19);
            this.lblTotalVentas.TabIndex = 0;
            this.lblTotalVentas.Text = "Total Ventas: $0";
            
            // lblTotalUnidades
            this.lblTotalUnidades.AutoSize = true;
            this.lblTotalUnidades.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTotalUnidades.Location = new System.Drawing.Point(230, 15);
            this.lblTotalUnidades.Name = "lblTotalUnidades";
            this.lblTotalUnidades.Size = new System.Drawing.Size(130, 19);
            this.lblTotalUnidades.TabIndex = 1;
            this.lblTotalUnidades.Text = "Total Unidades: 0";
            
            // lblProductosUnicos
            this.lblProductosUnicos.AutoSize = true;
            this.lblProductosUnicos.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblProductosUnicos.Location = new System.Drawing.Point(440, 15);
            this.lblProductosUnicos.Name = "lblProductosUnicos";
            this.lblProductosUnicos.Size = new System.Drawing.Size(140, 19);
            this.lblProductosUnicos.TabIndex = 2;
            this.lblProductosUnicos.Text = "Productos Únicos: 0";
            
            // lblPeriodo
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPeriodo.Location = new System.Drawing.Point(650, 15);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(80, 19);
            this.lblPeriodo.TabIndex = 3;
            this.lblPeriodo.Text = "Período: -";
            
            // sfDataGrid1
            this.sfDataGrid1.AccessibleName = "Table";
            this.sfDataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sfDataGrid1.Location = new System.Drawing.Point(0, 180);
            this.sfDataGrid1.Name = "sfDataGrid1";
            this.sfDataGrid1.Size = new System.Drawing.Size(1200, 520);
            this.sfDataGrid1.TabIndex = 3;
            
            // ReporteProductosMasVendidosForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.sfDataGrid1);
            this.Controls.Add(this.panelEstadisticas);
            this.Controls.Add(this.panelControles);
            this.Controls.Add(this.panelTop);
            this.Name = "ReporteProductosMasVendidosForm";
            this.Text = "Productos Más Vendidos";
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
