# ? Checklist - Crear Contenido de Manuales CHM

## ?? Objetivo

Crear contenido HTML para las **24 páginas faltantes** del manual de ayuda.

---

## ?? Estado Actual

- ? Sistema F1 funcionando al 100% (27 formularios)
- ? Archivo .hhp con secciones [MAP] y [ALIAS]
- ? CHM compilado correctamente
- ?? Solo 3 páginas tienen contenido completo

---

## ?? Páginas Completadas (3/27)

- ? `html/general/topic_20_main.html` - Página principal
- ? `html/login/topic_31_login.html` - Login
- ? `html/clientes/topic_40_crear_cliente.html` - Crear cliente

---

## ?? Páginas Pendientes de Crear (24/27)

### **?? Login y Seguridad (2)**
- [ ] `html/login/topic_32_recuperar.html` - Recuperar contraseña
- [ ] `html/login/topic_33_cambiar_pass.html` - Cambiar contraseña

### **?? Gestión de Usuarios (4)**
- [ ] `html/usuarios/topic_23_crear_usuario.html` - Crear usuario
- [ ] `html/usuarios/topic_24_asignar_rol.html` - Asignar rol
- [ ] `html/usuarios/topic_28_modificar_usuario.html` - Modificar usuario
- [ ] `html/usuarios/topic_29_mostrar_usuarios.html` - Mostrar usuarios

### **?? Roles y Permisos (2)**
- [ ] `html/roles/topic_26_crear_rol.html` - Crear rol
- [ ] `html/roles/topic_27_crear_patente.html` - Crear patente

### **?? Backup y Restore (3)**
- [ ] `html/backup/topic_22_backup.html` - Backup
- [ ] `html/backup/topic_25_bitacora.html` - Bitácora
- [ ] `html/backup/topic_30_restore.html` - Restore

### **?? Clientes (2)**
- [ ] `html/clientes/topic_41_modificar_cliente.html` - Modificar cliente
- [ ] `html/clientes/topic_42_mostrar_clientes.html` - Mostrar clientes

### **?? Productos (4)**
- [ ] `html/productos/topic_50_crear_producto.html` - Crear producto
- [ ] `html/productos/topic_51_modificar_producto.html` - Modificar producto
- [ ] `html/productos/topic_52_mostrar_productos.html` - Mostrar productos
- [ ] `html/productos/topic_53_eliminar_producto.html` - Eliminar producto

### **?? Proveedores (3)**
- [ ] `html/proveedores/topic_60_crear_proveedor.html` - Crear proveedor
- [ ] `html/proveedores/topic_61_modificar_proveedor.html` - Modificar proveedor
- [ ] `html/proveedores/topic_62_mostrar_proveedores.html` - Mostrar proveedores

### **?? Stock (2)**
- [ ] `html/stock/topic_70_mostrar_stock.html` - Mostrar stock
- [ ] `html/stock/topic_71_modificar_stock.html` - Modificar stock

### **?? Pedidos (4)**
- [ ] `html/pedidos/topic_80_crear_pedido.html` - Crear pedido
- [ ] `html/pedidos/topic_81_modificar_pedido.html` - Modificar pedido
- [ ] `html/pedidos/topic_82_mostrar_pedidos.html` - Mostrar pedidos
- [ ] `html/pedidos/topic_83_detalle_pedido.html` - Detalle pedido

---

## ?? Workflow para Cada Página

### **1. Preparación**
```bash
# Ir a la carpeta correspondiente
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Distribuidora_los_amigos\Docs\Manual_CHM\source_es\html\[carpeta]"

# Copiar la plantilla
copy ..\..\template.html topic_XX_nombre.html
```

### **2. Editar Contenido**
Abrir el archivo y modificar:
- [ ] Título de la página
- [ ] Breadcrumb navigation
- [ ] Descripción general
- [ ] Pasos de uso
- [ ] Notas importantes
- [ ] Problemas comunes
- [ ] Capturas de pantalla (opcional)

### **3. Compilar**
```powershell
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Distribuidora_los_amigos\Docs\Manual_CHM\source_es"
& "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" "help_es.hhp"
```

### **4. Copiar CHM**
```powershell
Copy-Item "..\help_es.chm" -Destination "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual\help_es.chm" -Force
```

### **5. Probar**
- [ ] Ejecutar la aplicación
- [ ] Abrir el formulario correspondiente
- [ ] Presionar F1
- [ ] Verificar que se abre la página correcta
- [ ] Verificar que el contenido se ve bien

---

## ?? Recursos Disponibles

### **Plantilla:**
- `source_es/template.html` - Base para todas las páginas

### **Ejemplos:**
- `topic_20_main.html` - Página principal con navegación
- `topic_31_login.html` - Formulario simple
- `topic_40_crear_cliente.html` - Formulario de creación

### **Estilos CSS:**
- `.container` - Contenedor principal
- `.breadcrumb` - Navegación
- `.note` - Cuadro de información (azul)
- `.warning` - Cuadro de advertencia (amarillo)
- `code` - Bloques de código

---

## ?? Estructura Recomendada por Página

```html
<section class="content">
    <h2>?? Descripción</h2>
    <p>Breve descripción de la funcionalidad</p>

    <h2>?? Cómo Usar</h2>
    <ol>
        <li>Paso 1</li>
        <li>Paso 2</li>
        <li>...</li>
    </ol>

    <div class="note">
        <strong>?? Nota:</strong> Información importante
    </div>

    <h2>?? Campos del Formulario</h2>
    <ul>
        <li><strong>Campo 1:</strong> Descripción</li>
        <li><strong>Campo 2:</strong> Descripción</li>
    </ul>

    <h2>?? Problemas Comunes</h2>
    <ul>
        <li><strong>Error X:</strong> Solución</li>
    </ul>

    <h2>?? Ver También</h2>
    <ul>
        <li><a href="../otro/topic_XX.html">Página relacionada</a></li>
    </ul>
</section>
```

---

## ?? Progreso Estimado

| Módulo | Páginas | Estimado por Página | Total |
|--------|---------|---------------------|-------|
| Login/Seguridad | 2 | 15 min | 30 min |
| Usuarios | 4 | 20 min | 80 min |
| Roles | 2 | 15 min | 30 min |
| Backup | 3 | 15 min | 45 min |
| Clientes | 2 | 20 min | 40 min |
| Productos | 4 | 20 min | 80 min |
| Proveedores | 3 | 20 min | 60 min |
| Stock | 2 | 15 min | 30 min |
| Pedidos | 4 | 25 min | 100 min |

**Total Estimado:** ~8 horas

---

## ? Checklist de Verificación Final

### **Por Cada Página:**
- [ ] El HTML es válido
- [ ] Los enlaces internos funcionan
- [ ] Las rutas a CSS son correctas
- [ ] El contenido es claro y útil
- [ ] Hay ejemplos cuando es necesario
- [ ] Se mencionan problemas comunes

### **Compilación:**
- [ ] El CHM se compila sin errores críticos
- [ ] El archivo se genera correctamente
- [ ] Se copia a la ubicación correcta

### **Pruebas:**
- [ ] F1 abre la página correcta
- [ ] El contenido se visualiza bien
- [ ] Los estilos se aplican correctamente
- [ ] La navegación funciona

---

## ?? Prioridad Sugerida

### **Alta Prioridad (Usar primero):**
1. Login y recuperación de contraseña
2. Crear usuario
3. Crear cliente
4. Crear producto
5. Crear pedido

### **Media Prioridad:**
6. Mostrar/Modificar usuarios
7. Gestión de roles
8. Mostrar clientes/productos
9. Stock

### **Baja Prioridad:**
10. Backup/Restore
11. Reportes
12. Bitácora

---

## ?? Notas Importantes

- ? El sistema F1 ya está funcionando al 100%
- ? No necesitas modificar código, solo crear HTML
- ? Usa la plantilla para mantener consistencia
- ? Compila después de cada 3-4 páginas para verificar
- ? Los errores de compilación "HHC5003" son normales

---

## ?? Documentación de Referencia

- **GUIA_RAPIDA_COMPILACION.md** - Cómo compilar
- **INDICE.md** - Lista completa de documentación
- **template.html** - Plantilla base
- **ESTADO_IMPLEMENTACION_F1.md** - Estado del proyecto

---

**Creado:** 4 de Enero 2025 - 23:30  
**Para:** Próximo chat de creación de contenido  
**Estado:** ? Lista para empezar
