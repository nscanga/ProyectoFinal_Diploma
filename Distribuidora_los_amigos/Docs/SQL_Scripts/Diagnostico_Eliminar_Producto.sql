-- Script para diagnosticar por qué no se puede eliminar un producto

-- 1. Ver todos los productos y su stock
SELECT p.IdProducto, p.Nombre, p.Categoria, p.Precio, 
       ISNULL(s.Cantidad, 0) AS Stock
FROM Producto p
LEFT JOIN Stock s ON p.IdProducto = s.IdProducto
ORDER BY p.Nombre;

-- 2. Verificar si hay detalles de pedido para un producto específico
-- (Reemplaza el GUID con el IdProducto que intentas eliminar)
DECLARE @IdProducto UNIQUEIDENTIFIER = 'XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX'; 

SELECT 
    dp.IdDetallePedido,
    dp.IdPedido,
    p.IdProducto,
    pr.Nombre AS NombreProducto,
    dp.Cantidad,
    dp.PrecioUnitario,
    ped.FechaPedido,
    ep.NombreEstado
FROM DetallePedido dp
INNER JOIN Producto pr ON dp.IdProducto = pr.IdProducto
INNER JOIN Pedido ped ON dp.IdPedido = ped.IdPedido
LEFT JOIN EstadoPedido ep ON ped.IdEstadoPedido = ep.IdEstadoPedido
WHERE dp.IdProducto = @IdProducto;

-- 3. Ver todas las restricciones de clave foránea de la tabla Producto
SELECT 
    fk.name AS ForeignKeyName,
    OBJECT_NAME(fk.parent_object_id) AS TableName,
    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName,
    OBJECT_NAME (fk.referenced_object_id) AS ReferencedTableName,
    COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferencedColumnName
FROM sys.foreign_keys AS fk
INNER JOIN sys.foreign_key_columns AS fc 
    ON fk.OBJECT_ID = fc.constraint_object_id
WHERE OBJECT_NAME(fk.referenced_object_id) = 'Producto';

-- 4. Contar cuántos pedidos tiene cada producto
SELECT 
    p.Nombre,
    COUNT(dp.IdDetallePedido) AS CantidadPedidos
FROM Producto p
LEFT JOIN DetallePedido dp ON p.IdProducto = dp.IdProducto
GROUP BY p.IdProducto, p.Nombre
HAVING COUNT(dp.IdDetallePedido) > 0
ORDER BY COUNT(dp.IdDetallePedido) DESC;
