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
    /// Formulario para mostrar los productos más vendidos por cantidad y monto.
    /// </summary>
    public partial class ReporteProductosMasVendidosForm : Form, IIdiomaObserver
    {
        private readonly DetallePedidoService _detallePedidoService;
        private readonly PedidoService _pedidoService;
        private readonly ProductoService _productoService;
        private int _topProductos = 10;

        /// <summary>
        /// Inicializa el formulario de reporte de productos más vendidos.
        /// </summary>
        public ReporteProductosMasVendidosForm()
        {
            InitializeComponent();
            _detallePedidoService = new DetallePedidoService();
            _pedidoService = new PedidoService();
            _productoService = new ProductoService();
            this.Load += ReporteProductosMasVendidosForm_Load;
            IdiomaService.Subscribe(this);
        }

        /// <summary>
        /// Carga los datos del reporte al iniciar el formulario.
        /// </summary>
        private void ReporteProductosMasVendidosForm_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigurarDataGrid();
                dtpDesde.Value = DateTime.Now.AddMonths(-1);
                dtpHasta.Value = DateTime.Now;
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
            sfDataGrid1.Columns.Add(new GridNumericColumn
            {
                MappingName = "Posicion",
                HeaderText = "#",
                Width = 50,
                FormatMode = Syncfusion.WinForms.Input.Enums.FormatMode.Numeric
            });

            sfDataGrid1.Columns.Add(new GridTextColumn
            {
                MappingName = "NombreProducto",
                HeaderText = IdiomaService.Translate("Producto"),
                Width = 250
            });

            sfDataGrid1.Columns.Add(new GridTextColumn
            {
                MappingName = "Categoria",
                HeaderText = IdiomaService.Translate("Categoría"),
                Width = 150
            });

            sfDataGrid1.Columns.Add(new GridNumericColumn
            {
                MappingName = "CantidadVendida",
                HeaderText = IdiomaService.Translate("Cantidad Vendida"),
                Width = 150,
                FormatMode = Syncfusion.WinForms.Input.Enums.FormatMode.Numeric
            });

            sfDataGrid1.Columns.Add(new GridNumericColumn
            {
                MappingName = "MontoTotal",
                HeaderText = IdiomaService.Translate("Monto Total"),
                Width = 150,
                FormatMode = Syncfusion.WinForms.Input.Enums.FormatMode.Currency
            });

            sfDataGrid1.Columns.Add(new GridNumericColumn
            {
                MappingName = "PrecioPromedio",
                HeaderText = IdiomaService.Translate("Precio Promedio"),
                Width = 150,
                FormatMode = Syncfusion.WinForms.Input.Enums.FormatMode.Currency
            });

            sfDataGrid1.Columns.Add(new GridNumericColumn
            {
                MappingName = "VecesVendido",
                HeaderText = IdiomaService.Translate("Veces Vendido"),
                Width = 120,
                FormatMode = Syncfusion.WinForms.Input.Enums.FormatMode.Numeric
            });
        }

        /// <summary>
        /// Carga el reporte de productos más vendidos.
        /// </summary>
        private void CargarReporte()
        {
            try
            {
                DateTime fechaDesde = dtpDesde.Value.Date;
                DateTime fechaHasta = dtpHasta.Value.Date.AddDays(1).AddSeconds(-1);

                // Obtener todos los pedidos en el rango de fechas
                var pedidos = _pedidoService.ObtenerTodosLosPedidos()
                    .Where(p => p.FechaPedido >= fechaDesde && p.FechaPedido <= fechaHasta)
                    .ToList();

                // Obtener todos los productos para hacer el join
                var productos = _productoService.ObtenerTodosProductos();

                // Obtener todos los detalles de pedidos y hacer join con productos
                var detalles = new List<DetallePedidoDTO>();
                foreach (var pedido in pedidos)
                {
                    var detallesPedido = _detallePedidoService.ObtenerDetallesPorPedido(pedido.IdPedido);
                    
                    // Mapear a DTOs con información del producto
                    foreach (var detalle in detallesPedido)
                    {
                        var producto = productos.FirstOrDefault(p => p.IdProducto == detalle.IdProducto);
                        if (producto != null)
                        {
                            detalles.Add(new DetallePedidoDTO
                            {
                                IdDetallePedido = detalle.IdDetallePedido,
                                IdPedido = detalle.IdPedido,
                                IdProducto = detalle.IdProducto,
                                NombreProducto = producto.Nombre,
                                Categoria = producto.Categoria,
                                Cantidad = detalle.Cantidad,
                                PrecioUnitario = detalle.PrecioUnitario,
                                Subtotal = detalle.Subtotal
                            });
                        }
                    }
                }

                // Agrupar por producto
                var productosMasVendidos = detalles
                    .GroupBy(d => new { d.IdProducto, d.NombreProducto, d.Categoria })
                    .Select(g => new
                    {
                        IdProducto = g.Key.IdProducto,
                        NombreProducto = g.Key.NombreProducto,
                        Categoria = g.Key.Categoria,
                        CantidadVendida = g.Sum(d => d.Cantidad),
                        MontoTotal = g.Sum(d => d.Subtotal),
                        PrecioPromedio = g.Average(d => d.PrecioUnitario),
                        VecesVendido = g.Count()
                    })
                    .OrderByDescending(p => p.CantidadVendida)
                    .Take(_topProductos)
                    .Select((p, index) => new
                    {
                        Posicion = index + 1,
                        p.NombreProducto,
                        p.Categoria,
                        p.CantidadVendida,
                        p.MontoTotal,
                        p.PrecioPromedio,
                        p.VecesVendido
                    })
                    .ToList();

                sfDataGrid1.DataSource = productosMasVendidos;

                // Actualizar estadísticas
                lblTotalVentas.Text = $"{IdiomaService.Translate("Total Ventas")}: {productosMasVendidos.Sum(p => p.MontoTotal):C2}";
                lblTotalUnidades.Text = $"{IdiomaService.Translate("Total Unidades")}: {productosMasVendidos.Sum(p => p.CantidadVendida)}";
                lblProductosUnicos.Text = $"{IdiomaService.Translate("Productos Únicos")}: {productosMasVendidos.Count}";
                lblPeriodo.Text = $"{IdiomaService.Translate("Período")}: {fechaDesde:dd/MM/yyyy} - {fechaHasta:dd/MM/yyyy}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Actualiza el top de productos y recarga el reporte.
        /// </summary>
        private void btnActualizarTop_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtTopProductos.Text, out int nuevoTop) && nuevoTop > 0)
            {
                _topProductos = nuevoTop;
                CargarReporte();
            }
            else
            {
                MessageBox.Show(IdiomaService.Translate("Ingrese un valor válido mayor a 0"), 
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Filtra el reporte por el rango de fechas seleccionado.
        /// </summary>
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (dtpDesde.Value > dtpHasta.Value)
            {
                MessageBox.Show(IdiomaService.Translate("La fecha desde debe ser menor a la fecha hasta"), 
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            CargarReporte();
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
                    FileName = $"ReporteProductosMasVendidos_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportarACSV(saveFileDialog.FileName);
                    MessageBox.Show(IdiomaService.Translate("Reporte exportado exitosamente"), 
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            var data = sfDataGrid1.DataSource as IEnumerable<dynamic>;
            if (data == null || !data.Any())
            {
                throw new Exception("No hay datos para exportar");
            }

            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                // Escribir encabezados
                writer.WriteLine("#,Producto,Categoría,Cantidad Vendida,Monto Total,Precio Promedio,Veces Vendido");

                // Escribir datos
                foreach (var item in data)
                {
                    var posicion = item.Posicion?.ToString() ?? "0";
                    var producto = item.NombreProducto?.ToString() ?? "";
                    var categoria = item.Categoria?.ToString() ?? "";
                    var cantidad = item.CantidadVendida?.ToString() ?? "0";
                    var monto = item.MontoTotal?.ToString("F2") ?? "0.00";
                    var promedio = item.PrecioPromedio?.ToString("F2") ?? "0.00";
                    var veces = item.VecesVendido?.ToString() ?? "0";

                    writer.WriteLine($"{posicion},{producto},{categoria},{cantidad},{monto},{promedio},{veces}");
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
