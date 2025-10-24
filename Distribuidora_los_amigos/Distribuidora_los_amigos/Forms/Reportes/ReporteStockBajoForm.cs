using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DOMAIN;
using Service.DAL.Contracts;
using Service.Facade;
using Services.Facade;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;

namespace Distribuidora_los_amigos.Forms.Reportes
{
    /// <summary>
    /// Formulario para mostrar los productos con stock bajo o cr�tico que requieren reabastecimiento.
    /// </summary>
    public partial class ReporteStockBajoForm : Form, IIdiomaObserver
    {
        private readonly StockService _stockService;
        private int _umbralStockBajo = 10; // Umbral configurable

        /// <summary>
        /// Inicializa el formulario de reporte de stock bajo.
        /// </summary>
        public ReporteStockBajoForm()
        {
            InitializeComponent();
            _stockService = new StockService();
            this.Load += ReporteStockBajoForm_Load;
            IdiomaService.Subscribe(this);
        }

        /// <summary>
        /// Carga los datos del reporte al iniciar el formulario.
        /// </summary>
        private void ReporteStockBajoForm_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigurarDataGrid();
                CargarReporte();
                IdiomaService.TranslateForm(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el reporte: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Configura el DataGrid con las columnas necesarias.
        /// </summary>
        private void ConfigurarDataGrid()
        {
            sfDataGrid1.AutoGenerateColumns = false;
            sfDataGrid1.AllowEditing = false;
            sfDataGrid1.AllowFiltering = true;
            sfDataGrid1.AllowSorting = true;
            sfDataGrid1.SelectionMode = GridSelectionMode.Single;

            // Limpiar columnas existentes
            sfDataGrid1.Columns.Clear();

            // Configurar columnas
            sfDataGrid1.Columns.Add(new GridTextColumn
            {
                MappingName = "NombreProducto",
                HeaderText = IdiomaService.Translate("Producto"),
                Width = 200
            });

            sfDataGrid1.Columns.Add(new GridTextColumn
            {
                MappingName = "Categoria",
                HeaderText = IdiomaService.Translate("Categor�a"),
                Width = 150
            });

            sfDataGrid1.Columns.Add(new GridNumericColumn
            {
                MappingName = "Cantidad",
                HeaderText = IdiomaService.Translate("Cantidad Actual"),
                Width = 120,
                FormatMode = Syncfusion.WinForms.Input.Enums.FormatMode.Numeric
            });

            sfDataGrid1.Columns.Add(new GridTextColumn
            {
                MappingName = "Tipo",
                HeaderText = IdiomaService.Translate("Tipo"),
                Width = 100
            });

            sfDataGrid1.Columns.Add(new GridTextColumn
            {
                MappingName = "Estado",
                HeaderText = IdiomaService.Translate("Estado"),
                Width = 120
            });
        }

        /// <summary>
        /// Carga el reporte de productos con stock bajo.
        /// </summary>
        private void CargarReporte()
        {
            try
            {
                var stockCompleto = _stockService.ObtenerStockConDetalles();
                
                // Filtrar productos con stock bajo
                var stockBajo = stockCompleto
                    .Where(s => s.Cantidad <= _umbralStockBajo && s.Activo)
                    .Select(s => new
                    {
                        s.NombreProducto,
                        s.Categoria,
                        s.Cantidad,
                        s.Tipo,
                        Estado = s.Cantidad == 0 ? "Cr�tico" : 
                                 s.Cantidad <= 5 ? "Muy Bajo" : "Bajo"
                    })
                    .OrderBy(s => s.Cantidad)
                    .ToList();

                sfDataGrid1.DataSource = stockBajo;

                // Actualizar estad�sticas
                lblTotalProductos.Text = $"{IdiomaService.Translate("Total productos")}: {stockBajo.Count}";
                lblCriticos.Text = $"{IdiomaService.Translate("Cr�ticos")}: {stockBajo.Count(s => s.Cantidad == 0)}";
                lblMuyBajos.Text = $"{IdiomaService.Translate("Muy Bajos")}: {stockBajo.Count(s => s.Cantidad > 0 && s.Cantidad <= 5)}";
                lblBajos.Text = $"{IdiomaService.Translate("Bajos")}: {stockBajo.Count(s => s.Cantidad > 5 && s.Cantidad <= _umbralStockBajo)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Actualiza el umbral de stock bajo y recarga el reporte.
        /// </summary>
        private void btnActualizarUmbral_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtUmbral.Text, out int nuevoUmbral) && nuevoUmbral > 0)
            {
                _umbralStockBajo = nuevoUmbral;
                CargarReporte();
            }
            else
            {
                MessageBox.Show(IdiomaService.Translate("Ingrese un valor v�lido mayor a 0"), 
                    "Validaci�n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Exporta el reporte a un archivo CSV.
        /// </summary>
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV Files|*.csv|Text Files|*.txt",
                    FileName = $"ReporteStockBajo_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportarACSV(saveFileDialog.FileName);
                    MessageBox.Show(IdiomaService.Translate("Reporte exportado exitosamente"), 
                        "�xito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Exporta los datos del grid a un archivo CSV.
        /// </summary>
        private void ExportarACSV(string filePath)
        {
            var data = sfDataGrid1.DataSource as List<dynamic>;
            if (data == null || data.Count == 0)
            {
                throw new Exception("No hay datos para exportar");
            }

            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                // Escribir encabezados
                writer.WriteLine("Producto,Categor�a,Cantidad Actual,Tipo,Estado");

                // Escribir datos
                foreach (var item in data)
                {
                    var producto = item.NombreProducto?.ToString() ?? "";
                    var categoria = item.Categoria?.ToString() ?? "";
                    var cantidad = item.Cantidad?.ToString() ?? "0";
                    var tipo = item.Tipo?.ToString() ?? "";
                    var estado = item.Estado?.ToString() ?? "";

                    writer.WriteLine($"{producto},{categoria},{cantidad},{tipo},{estado}");
                }
            }
        }

        /// <summary>
        /// Actualiza el idioma del formulario.
        /// </summary>
        public void UpdateIdioma()
        {
            IdiomaService.TranslateForm(this);
            ConfigurarDataGrid();
            CargarReporte();
        }

        /// <summary>
        /// Limpia recursos al cerrar el formulario.
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            IdiomaService.Unsubscribe(this);
            base.OnFormClosing(e);
        }
    }
}
