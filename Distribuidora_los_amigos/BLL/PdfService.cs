using System;
using System.IO;
using System.Collections.Generic;
using DOMAIN;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Linq;
//using System.Windows.Forms;

namespace BLL
{
    public class PdfService
    {
        private readonly PedidoService _pedidoService;
        private readonly ProductoService _productoService;

        public PdfService()
        {
            _pedidoService = new PedidoService();
            _productoService = new ProductoService();
        }

        public void GenerarPdfPedido(Pedido pedido, string rutaArchivo)
        {
            try
            {
                // Crear el documento PDF
                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(rutaArchivo, FileMode.Create));
                
                document.Open();

                // 🎨 Configurar fuentes
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font titleFont = new Font(baseFont, 18, Font.BOLD);
                Font headerFont = new Font(baseFont, 12, Font.BOLD);
                Font normalFont = new Font(baseFont, 10, Font.NORMAL);
                Font boldFont = new Font(baseFont, 10, Font.BOLD);

                // 📋 Título del documento
                Paragraph title = new Paragraph("COMPROBANTE DE PEDIDO ENTREGADO", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 20f;
                document.Add(title);

                // 📅 Información del pedido
                AddPedidoInfo(document, pedido, headerFont, normalFont);

                // 👤 Información del cliente
                AddClienteInfo(document, pedido, headerFont, normalFont);

                // 📦 Detalle de productos
                AddProductosTable(document, pedido, headerFont, normalFont, boldFont);

                // 💰 Total final
                AddTotalFinal(document, pedido, headerFont, boldFont);

                // 📝 Pie de página
                AddFooter(document, normalFont);

                document.Close();
                writer.Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar el PDF: {ex.Message}", ex);
            }
        }

        private void AddPedidoInfo(Document document, Pedido pedido, Font headerFont, Font normalFont)
        {
            // Información del pedido
            Paragraph pedidoInfo = new Paragraph("INFORMACIÓN DEL PEDIDO", headerFont);
            pedidoInfo.SpacingBefore = 10f;
            pedidoInfo.SpacingAfter = 10f;
            document.Add(pedidoInfo);

            PdfPTable infoTable = new PdfPTable(2);
            infoTable.WidthPercentage = 100f;
            infoTable.SetWidths(new float[] { 30f, 70f });

            // ID del Pedido (solo los primeros 8 caracteres para legibilidad)
            string idPedidoCorto = pedido.IdPedido.ToString().Substring(0, 8).ToUpper();
            infoTable.AddCell(new PdfPCell(new Phrase("ID Pedido:", normalFont)) { Border = Rectangle.NO_BORDER });
            infoTable.AddCell(new PdfPCell(new Phrase(idPedidoCorto, normalFont)) { Border = Rectangle.NO_BORDER });

            infoTable.AddCell(new PdfPCell(new Phrase("Fecha Pedido:", normalFont)) { Border = Rectangle.NO_BORDER });
            infoTable.AddCell(new PdfPCell(new Phrase(pedido.FechaPedido.ToString("dd/MM/yyyy"), normalFont)) { Border = Rectangle.NO_BORDER });

            string estadoPedido = _pedidoService.ObtenerNombreEstadoPorId(pedido.IdEstadoPedido);
            infoTable.AddCell(new PdfPCell(new Phrase("Estado:", normalFont)) { Border = Rectangle.NO_BORDER });
            infoTable.AddCell(new PdfPCell(new Phrase(estadoPedido, normalFont)) { Border = Rectangle.NO_BORDER });

            infoTable.AddCell(new PdfPCell(new Phrase("Fecha Generación:", normalFont)) { Border = Rectangle.NO_BORDER });
            infoTable.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToString("dd/MM/yyyy HH:mm"), normalFont)) { Border = Rectangle.NO_BORDER });

            infoTable.SpacingAfter = 15f;
            document.Add(infoTable);
        }

        private void AddClienteInfo(Document document, Pedido pedido, Font headerFont, Font normalFont)
        {
            // Información del cliente
            Paragraph clienteInfo = new Paragraph("INFORMACIÓN DEL CLIENTE", headerFont);
            clienteInfo.SpacingBefore = 10f;
            clienteInfo.SpacingAfter = 10f;
            document.Add(clienteInfo);

            string nombreCliente = _pedidoService.ObtenerNombreClientePorId(pedido.IdCliente);

            PdfPTable clienteTable = new PdfPTable(2);
            clienteTable.WidthPercentage = 100f;
            clienteTable.SetWidths(new float[] { 30f, 70f });

            clienteTable.AddCell(new PdfPCell(new Phrase("Cliente:", normalFont)) { Border = Rectangle.NO_BORDER });
            clienteTable.AddCell(new PdfPCell(new Phrase(nombreCliente, normalFont)) { Border = Rectangle.NO_BORDER });

            clienteTable.SpacingAfter = 15f;
            document.Add(clienteTable);
        }

        private void AddProductosTable(Document document, Pedido pedido, Font headerFont, Font normalFont, Font boldFont)
        {
            // Título de productos
            Paragraph productosTitle = new Paragraph("DETALLE DE PRODUCTOS", headerFont);
            productosTitle.SpacingBefore = 10f;
            productosTitle.SpacingAfter = 10f;
            document.Add(productosTitle);

            // Tabla de productos
            PdfPTable productosTable = new PdfPTable(4);
            productosTable.WidthPercentage = 100f;
            productosTable.SetWidths(new float[] { 50f, 15f, 20f, 15f });

            // Headers
            productosTable.AddCell(new PdfPCell(new Phrase("Producto", boldFont)) { 
                BackgroundColor = BaseColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5f
            });
            productosTable.AddCell(new PdfPCell(new Phrase("Cantidad", boldFont)) { 
                BackgroundColor = BaseColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5f
            });
            productosTable.AddCell(new PdfPCell(new Phrase("Precio Unit.", boldFont)) { 
                BackgroundColor = BaseColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5f
            });
            productosTable.AddCell(new PdfPCell(new Phrase("Subtotal", boldFont)) { 
                BackgroundColor = BaseColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5f
            });

            // Obtener detalles del pedido
            List<DetallePedido> detalles = _pedidoService.ObtenerDetallesPorPedido(pedido.IdPedido);

            foreach (var detalle in detalles)
            {
                // Obtener nombre del producto
                Producto producto = _productoService.ObtenerProductoPorId(detalle.IdProducto);
                string nombreProducto = producto != null ? producto.Nombre : "Producto no encontrado";

                productosTable.AddCell(new PdfPCell(new Phrase(nombreProducto, normalFont)) { Padding = 5f });
                productosTable.AddCell(new PdfPCell(new Phrase(detalle.Cantidad.ToString(), normalFont)) { 
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5f
                });
                productosTable.AddCell(new PdfPCell(new Phrase(detalle.PrecioUnitario.ToString("C2"), normalFont)) { 
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    Padding = 5f
                });
                productosTable.AddCell(new PdfPCell(new Phrase(detalle.Subtotal.ToString("C2"), normalFont)) { 
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    Padding = 5f
                });
            }

            productosTable.SpacingAfter = 15f;
            document.Add(productosTable);
        }

        private void AddTotalFinal(Document document, Pedido pedido, Font headerFont, Font boldFont)
        {
            // Total final
            PdfPTable totalTable = new PdfPTable(2);
            totalTable.WidthPercentage = 50f;
            totalTable.HorizontalAlignment = Element.ALIGN_RIGHT;

            totalTable.AddCell(new PdfPCell(new Phrase("TOTAL FINAL:", boldFont)) { 
                Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 8f
            });
            totalTable.AddCell(new PdfPCell(new Phrase(pedido.Total.ToString("C2"), boldFont)) { 
                Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 8f
            });

            totalTable.SpacingAfter = 20f;
            document.Add(totalTable);
        }

        private void AddFooter(Document document, Font normalFont)
        {
            // Pie de página
            Paragraph footer = new Paragraph("\n\nEste documento certifica la entrega del pedido.", normalFont);
            footer.Alignment = Element.ALIGN_CENTER;
            footer.SpacingBefore = 20f;
            document.Add(footer);

            Paragraph empresa = new Paragraph("Distribuidora Los Amigos", normalFont);
            empresa.Alignment = Element.ALIGN_CENTER;
            document.Add(empresa);
        }

        public string GenerarNombreArchivoPdf(Pedido pedido)
        {
            // Generar nombre de archivo único
            string nombreCliente = _pedidoService.ObtenerNombreClientePorId(pedido.IdCliente);
            string fechaFormateada = pedido.FechaPedido.ToString("yyyyMMdd");
            string idCorto = pedido.IdPedido.ToString().Substring(0, 8).ToUpper();
            
            // Limpiar caracteres no válidos para nombres de archivo
            nombreCliente = string.Join("_", nombreCliente.Split(Path.GetInvalidFileNameChars()));
            
            return $"Pedido_{nombreCliente}_{fechaFormateada}_{idCorto}.pdf";
        }
    }
}