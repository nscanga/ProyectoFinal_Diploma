using System;
using System.IO;
using System.Resources;
using System.Globalization;
using System.Collections.Generic;

namespace Service.DAL.Implementations
{
    /// <summary>
    /// Proporciona traducciones básicas y persistencia de idioma seleccionado para el usuario.
    /// </summary>
    public static class LenguajeRepository // ✅ Nombre correcto
    {
        private static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "language.config");
        
        private static readonly Dictionary<string, Dictionary<string, string>> _translations = new Dictionary<string, Dictionary<string, string>>
        {
            ["es-ES"] = new Dictionary<string, string>
            {
                ["Usuario"] = "Usuario",
                ["Roles"] = "Roles",
                ["CerraSesion"] = "Cerrar Sesión",
                // ✅ Agregar traducciones para menús
                ["CLIENTE"] = "CLIENTE",
                ["PEDIDOS"] = "PEDIDOS", 
                ["PRODUCTOS"] = "PRODUCTOS",
                ["STOCK"] = "STOCK",
                ["BUSQUEDA"] = "BÚSQUEDA",
                ["REPORTES"] = "REPORTES",
                ["GESTION_USUARIOS"] = "GESTIÓN DE USUARIOS",
                ["PROVEEDORES"] = "PROVEEDORES",
                ["Crear_cliente"] = "Crear cliente",
                ["Mostrar_clientes"] = "Mostrar clientes",
                ["CREAR_PEDIDO"] = "CREAR PEDIDO",
                ["MOSTRAR_PEDIDOS"] = "MOSTRAR PEDIDOS",
                ["AGREGAR"] = "AGREGAR",
                ["MODIFICAR"] = "MODIFICAR",
                ["ELIMINAR"] = "ELIMINAR",
                ["VER_PRODUCTOS"] = "VER PRODUCTOS",
                // 🆕 Traducciones para Reportes
                ["ReporteStockBajo"] = "Reporte de Stock Bajo",
                ["ReporteProductosMasVendidos"] = "Productos Más Vendidos",
                ["Producto"] = "Producto",
                ["Categoría"] = "Categoría",
                ["Cantidad Actual"] = "Cantidad Actual",
                ["Tipo"] = "Tipo",
                ["Estado"] = "Estado",
                ["Total productos"] = "Total productos",
                ["Críticos"] = "Críticos",
                ["Muy Bajos"] = "Muy Bajos",
                ["Bajos"] = "Bajos",
                ["Ingrese un valor válido mayor a 0"] = "Ingrese un valor válido mayor a 0",
                ["Reporte exportado exitosamente"] = "Reporte exportado exitosamente",
                ["Cantidad Vendida"] = "Cantidad Vendida",
                ["Monto Total"] = "Monto Total",
                ["Precio Promedio"] = "Precio Promedio",
                ["Veces Vendido"] = "Veces Vendido",
                ["Total Ventas"] = "Total Ventas",
                ["Total Unidades"] = "Total Unidades",
                ["Productos Únicos"] = "Productos Únicos",
                ["Período"] = "Período",
                ["La fecha desde debe ser menor a la fecha hasta"] = "La fecha desde debe ser menor a la fecha hasta"
            },
            ["en-US"] = new Dictionary<string, string>
            {
                ["Usuario"] = "User",
                ["Roles"] = "Roles", 
                ["CerraSesion"] = "Logout",
                // ✅ Traducciones en inglés
                ["CLIENTE"] = "CUSTOMER",
                ["PEDIDOS"] = "ORDERS", 
                ["PRODUCTOS"] = "PRODUCTS",
                ["STOCK"] = "STOCK",
                ["BUSQUEDA"] = "SEARCH",
                ["REPORTES"] = "REPORTS",
                ["GESTION_USUARIOS"] = "USER MANAGEMENT",
                ["PROVEEDORES"] = "SUPPLIERS",
                ["Crear_cliente"] = "Create customer",
                ["Mostrar_clientes"] = "Show customers",
                ["CREAR_PEDIDO"] = "CREATE ORDER",
                ["MOSTRAR_PEDIDOS"] = "SHOW ORDERS",
                ["AGREGAR"] = "ADD",
                ["MODIFICAR"] = "MODIFY",
                ["ELIMINAR"] = "DELETE",
                ["VER_PRODUCTOS"] = "VIEW PRODUCTS",
                // 🆕 Traducciones para Reportes
                ["ReporteStockBajo"] = "Low Stock Report",
                ["ReporteProductosMasVendidos"] = "Best Selling Products",
                ["Producto"] = "Product",
                ["Categoría"] = "Category",
                ["Cantidad Actual"] = "Current Quantity",
                ["Tipo"] = "Type",
                ["Estado"] = "Status",
                ["Total productos"] = "Total products",
                ["Críticos"] = "Critical",
                ["Muy Bajos"] = "Very Low",
                ["Bajos"] = "Low",
                ["Ingrese un valor válido mayor a 0"] = "Enter a valid value greater than 0",
                ["Reporte exportado exitosamente"] = "Report exported successfully",
                ["Cantidad Vendida"] = "Quantity Sold",
                ["Monto Total"] = "Total Amount",
                ["Precio Promedio"] = "Average Price",
                ["Veces Vendido"] = "Times Sold",
                ["Total Ventas"] = "Total Sales",
                ["Total Unidades"] = "Total Units",
                ["Productos Únicos"] = "Unique Products",
                ["Período"] = "Period",
                ["La fecha desde debe ser menor a la fecha hasta"] = "Start date must be before end date"
            },
            ["pt-PT"] = new Dictionary<string, string>
            {
                ["Usuario"] = "Usuário",
                ["Roles"] = "Funções",
                ["CerraSesion"] = "Sair",
                // ✅ Traducciones en portugués
                ["CLIENTE"] = "CLIENTE",
                ["PEDIDOS"] = "PEDIDOS", 
                ["PRODUCTOS"] = "PRODUTOS",
                ["STOCK"] = "ESTOQUE",
                ["BUSQUEDA"] = "PESQUISA",
                ["REPORTES"] = "RELATÓRIOS",
                ["GESTION_USUARIOS"] = "GESTÃO DE USUÁRIOS",
                ["PROVEEDORES"] = "FORNECEDORES",
                ["Crear_cliente"] = "Criar cliente",
                ["Mostrar_clientes"] = "Mostrar clientes",
                ["CREAR_PEDIDO"] = "CRIAR PEDIDO",
                ["MOSTRAR_PEDIDOS"] = "MOSTRAR PEDIDOS",
                ["AGREGAR"] = "ADICIONAR",
                ["MODIFICAR"] = "MODIFICAR",
                ["ELIMINAR"] = "EXCLUIR",
                ["VER_PRODUCTOS"] = "VER PRODUTOS",
                // 🆕 Traducciones para Reportes
                ["ReporteStockBajo"] = "Relatório de Estoque Baixo",
                ["ReporteProductosMasVendidos"] = "Produtos Mais Vendidos",
                ["Producto"] = "Produto",
                ["Categoría"] = "Categoria",
                ["Cantidad Actual"] = "Quantidade Atual",
                ["Tipo"] = "Tipo",
                ["Estado"] = "Status",
                ["Total productos"] = "Total de produtos",
                ["Críticos"] = "Críticos",
                ["Muy Bajos"] = "Muito Baixos",
                ["Bajos"] = "Baixos",
                ["Ingrese un valor válido mayor a 0"] = "Digite um valor válido maior que 0",
                ["Reporte exportado exitosamente"] = "Relatório exportado com sucesso",
                ["Cantidad Vendida"] = "Quantidade Vendida",
                ["Monto Total"] = "Montante Total",
                ["Precio Promedio"] = "Preço Médio",
                ["Veces Vendido"] = "Vezes Vendido",
                ["Total Ventas"] = "Total de Vendas",
                ["Total Unidades"] = "Total de Unidades",
                ["Productos Únicos"] = "Produtos Únicos",
                ["Período"] = "Período",
                ["La fecha desde debe ser menor a la fecha hasta"] = "A data inicial deve ser anterior à data final"
            }
        };

        /// <summary>
        /// Devuelve la traducción correspondiente a la clave indicada según el idioma activo.
        /// </summary>
        /// <param name="key">Identificador del texto a traducir.</param>
        /// <returns>Cadena traducida o la clave original si no existe traducción.</returns>
        public static string Translate(string key)
        {
            string currentLanguage = LoadUserLanguage();
            
            if (_translations.ContainsKey(currentLanguage) && _translations[currentLanguage].ContainsKey(key))
            {
                return _translations[currentLanguage][key];
            }
            
            return key; // Retornar la clave original si no se encuentra traducción
        }

        /// <summary>
        /// Persiste el código de idioma seleccionado por el usuario en el archivo de configuración.
        /// </summary>
        /// <param name="languageCode">Código de cultura a guardar.</param>
        public static void SaveUserLanguage(string languageCode)
        {
            try
            {
                File.WriteAllText(ConfigFilePath, languageCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving language: {ex.Message}");
            }
        }

        /// <summary>
        /// Recupera el idioma almacenado en disco o retorna el español por defecto.
        /// </summary>
        /// <returns>Código de cultura a utilizar.</returns>
        public static string LoadUserLanguage()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    return File.ReadAllText(ConfigFilePath).Trim();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading language: {ex.Message}");
            }
            
            return "es-ES"; // Idioma por defecto
        }
    }
}