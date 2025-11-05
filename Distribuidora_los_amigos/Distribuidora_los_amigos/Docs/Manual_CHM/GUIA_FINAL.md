# ?? GUÍA DEFINITIVA - Compilar CHM desde Cero

## ?? Tu Situación Actual

? Tienes archivos HTML en: `C:\DistribuidoraLosAmigos\Manual\source_es\html\`  
? NO tienes archivos de proyecto (.hhp, .hhc, .hhk)  
? HTML Help Workshop instalado

## ? Solución en 2 Pasos

### **Paso 1: Generar Archivos del Proyecto**

Los archivos del proyecto se crean **automáticamente** desde los HTML existentes.

**Doble click en:**
```
generar_proyecto.bat
```

Este script:
- ? Escanea todos los archivos HTML en `C:\DistribuidoraLosAmigos\Manual\source_es\html\`
- ? Crea `help_es.hhp` (proyecto)
- ? **Agrega secciones [MAP] y [ALIAS] para ayuda F1** ? NUEVO
- ? Crea `help_es.hhc` (tabla de contenidos)
- ? Crea `help_es.hhk` (índice de búsqueda)
- ? Crea `styles.css` (estilos)
- ? Organiza el contenido por carpetas

### **Paso 2: Compilar el CHM**

**Doble click en:**
```
compilar_directo.bat
```

Este script:
- ? Verifica que todo esté listo
- ? Compila el archivo CHM
- ? Te muestra la ubicación del resultado

### **Paso 3: Copiar el CHM a la Aplicación**

**Opción A: Copiar manualmente**
```
Copiar de: C:\DistribuidoraLosAmigos\Manual\help_es.chm
Copiar a:   C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual\help_es.chm
```

**Opción B: Actualizar App.config**
```xml
<add key="HelpFilePath_es-ES" value="C:\DistribuidoraLosAmigos\Manual\help_es.chm"/>
```

---

## ?? Estructura que se Genera

```
C:\DistribuidoraLosAmigos\Manual\source_es\
??? help_es.hhp          ? Proyecto (con MAP y ALIAS para F1) ?
??? help_es.hhc          ? Tabla de contenidos
??? help_es.hhk          ? Índice
??? css\
?   ??? styles.css       ? Estilos
??? html\
    ??? general\
    ?   ??? *.html       ? Ya existen
    ??? login\
    ?   ??? *.html       ? Ya existen
    ??? clientes\
    ?   ??? *.html       ? Ya existen
    ??? ... (otras carpetas)

? COMPILAR ?

C:\DistribuidoraLosAmigos\Manual\
??? help_es.chm ?        ? Resultado final (con soporte F1)
```

---

## ?? Novedades - Soporte para Ayuda Contextual (F1)

### **Secciones Agregadas Automáticamente:**

El archivo `help_es.hhp` ahora incluye:

#### **[MAP] Section - Define IDs Numéricos**
```ini
[MAP]
#define TOPIC_MAIN 20
#define TOPIC_LOGIN 31
#define TOPIC_CREAR_CLIENTE 40
... (27 IDs más)
```

#### **[ALIAS] Section - Mapea IDs a Archivos HTML**
```ini
[ALIAS]
TOPIC_MAIN=html\general\topic_20_main.html
TOPIC_LOGIN=html\login\topic_31_login.html
TOPIC_CREAR_CLIENTE=html\clientes\topic_40_crear_cliente.html
... (27 mapeos más)
```

### **Cómo Funciona F1:**

```
Usuario presiona F1 en LoginForm
    ?
Código C#: Help.ShowHelp(null, "help_es.chm", HelpNavigator.TopicId, "31")
    ?
CHM busca ID 31 en [MAP]: TOPIC_LOGIN
    ?
CHM busca TOPIC_LOGIN en [ALIAS]: html\login\topic_31_login.html
    ?
CHM abre la página específica ?
```

---

## ?? Resultado Esperado

### **Después del Paso 1 (generar_proyecto.bat):**

```
==========================================
  GENERADOR DE PROYECTO CHM
==========================================

Directorio base: C:\DistribuidoraLosAmigos\Manual
Directorio fuente: C:\DistribuidoraLosAmigos\Manual\source_es

[OK] Carpeta HTML encontrada

Escaneando archivos HTML...
Archivos HTML encontrados: 12
  - general\topic_20_main.html
  - login\topic_31_login.html
  - clientes\topic_40_crear_cliente.html
  ... (más archivos)

[1/3] Creando archivo help_es.hhp...
[OK] help_es.hhp creado con mapeo de IDs para F1

[2/3] Creando archivo help_es.hhc...
[OK] help_es.hhc creado

[3/3] Creando archivo help_es.hhk...
[OK] help_es.hhk creado

==========================================
  RESUMEN
==========================================

Archivos generados:
  [OK] C:\DistribuidoraLosAmigos\Manual\source_es\help_es.hhp (con seccion MAP y ALIAS para F1)
  [OK] C:\DistribuidoraLosAmigos\Manual\source_es\help_es.hhc
  [OK] C:\DistribuidoraLosAmigos\Manual\source_es\help_es.hhk

Archivos HTML incluidos: 12

NOTA IMPORTANTE:
El archivo .hhp incluye mapeo de IDs para ayuda contextual (F1)
IDs mapeados: 20, 22-33, 40-42, 50-53, 60-62, 70-71, 80-83, 90-91

==========================================

SIGUIENTE PASO:
Ejecutar: compilar_directo.bat
```

### **Después del Paso 2 (compilar_directo.bat):**

```
==========================================
 Compilador CHM - Distribuidora Los Amigos
==========================================

[OK] HTML Help Workshop encontrado
[OK] Proyecto encontrado
[OK] Carpeta html\ encontrada

==========================================
Compilando help_es.chm...
==========================================

Microsoft HTML Help Compiler 4.74.8702
Compiling c:\DistribuidoraLosAmigos\Manual\source_es\help_es.hhp
Created c:\DistribuidoraLosAmigos\Manual\help_es.chm

==========================================
VERIFICANDO RESULTADO
==========================================

[OK] help_es.chm compilado exitosamente

Archivo generado:
C:\DistribuidoraLosAmigos\Manual\help_es.chm

Tamano: 45678 bytes

SIGUIENTE PASO:
1. Abrir el archivo para probar
2. O copiar a la carpeta de su aplicacion
```

---

## ?? Scripts Disponibles

| Script | Función | Orden de Ejecución |
|--------|---------|-------------------|
| **generar_proyecto.bat** | Crea archivos .hhp, .hhc, .hhk con soporte F1 | ? **1º EJECUTAR** |
| **compilar_directo.bat** | Compila el CHM | ? **2º EJECUTAR** |
| diagnostico.bat | Verifica estado | Si hay problemas |

---

## ?? IDs de Ayuda Contextual Mapeados

| ID | Pantalla | Método C# |
|----|----------|-----------|
| 20 | Principal | `AbrirAyudaMain()` |
| 31 | Login | `AbrirAyudaLogin()` |
| 32 | Recuperar Password | `AbrirAyudaRecuperoPass()` |
| 33 | Cambiar Password | `AbrirAyudaCambiarPass()` |
| 22 | Backup | `AbrirAyudaBackUp()` |
| 30 | Restore | `AbrirAyudaRestore()` |
| 23 | Crear Usuario | `AbrirAyudaCrearUsuario()` |
| 28 | Modificar Usuario | `AbrirAyudaModUsuario()` |
| 29 | Mostrar Usuarios | `AbrirAyudaMostrarUsuario()` |
| 24 | Asignar Rol | `AbrirAyudaAsignarRol()` |
| 26 | Crear Rol | `AbrirAyudaCrearRol()` |
| 27 | Crear Patente | `AbrirAyudaCrearPatente()` |
| 25 | Bitácora | `AbrirAyudaBitacora()` |
| 40 | Crear Cliente | `AbrirAyudaCrearCliente()` |
| 41 | Modificar Cliente | `AbrirAyudaModificarCliente()` |
| 42 | Mostrar Clientes | `AbrirAyudaMostrarClientes()` |
| 50 | Crear Producto | `AbrirAyudaCrearProducto()` |
| 51 | Modificar Producto | `AbrirAyudaModificarProducto()` |
| 52 | Mostrar Productos | `AbrirAyudaMostrarProductos()` |
| 53 | Eliminar Producto | `AbrirAyudaEliminarProducto()` |
| 60 | Crear Proveedor | `AbrirAyudaCrearProveedor()` |
| 61 | Modificar Proveedor | `AbrirAyudaModificarProveedor()` |
| 62 | Mostrar Proveedores | `AbrirAyudaMostrarProveedores()` |
| 70 | Mostrar Stock | `AbrirAyudaMostrarStock()` |
| 71 | Modificar Stock | `AbrirAyudaModificarStock()` |
| 80 | Crear Pedido | `AbrirAyudaCrearPedido()` |
| 81 | Modificar Pedido | `AbrirAyudaModificarPedido()` |
| 82 | Mostrar Pedidos | `AbrirAyudaMostrarPedidos()` |
| 83 | Detalle Pedido | `AbrirAyudaDetallePedido()` |
| 90 | Reporte Stock Bajo | `AbrirAyudaReporteStockBajo()` |
| 91 | Productos Más Vendidos | `AbrirAyudaReporteProductosMasVendidos()` |

---

## ?? Solución de Problemas

### **Problema: "HH_HELP_CONTEXT called without a [MAP] section"**

? **Solución:**
```
1. Ejecutar: generar_proyecto.bat (regenera con MAP y ALIAS)
2. Ejecutar: compilar_directo.bat
3. Copiar el nuevo CHM a la carpeta de la aplicación
```

Ver: `SOLUCION_F1_MAP_SECTION.md`

### **Problema: "No se encontraron archivos HTML"**

? **Solución:**
```
Verifique que existan archivos .html en:
C:\DistribuidoraLosAmigos\Manual\source_es\html\

Si no existen, use los scripts PowerShell originales:
1. crear_estructura.ps1
2. generar_paginas.ps1
```

### **Problema: "Error de ejecución de scripts"**

? **Solución:**
```powershell
# Abrir PowerShell como Administrador
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser

# Luego ejecutar de nuevo generar_proyecto.bat
```

### **Problema: "Archivo CHM vacío o no se abre"**

? **Solución:**
```
1. Verificar que los archivos HTML tengan contenido
2. Abrir help_es.hhp con HTML Help Workshop
3. Compilar desde el menú (File ? Compile)
4. Ver errores en la ventana de compilación
```

---

## ?? Archivos Creados/Modificados

1. ? **`generar_proyecto_chm.ps1`** - Script PowerShell (actualizado con MAP/ALIAS)
2. ? **`generar_proyecto.bat`** - Ejecutador simple
3. ? **`SOLUCION_F1_MAP_SECTION.md`** - Documentación del problema y solución

---

## ?? Resumen Simple

```
1. Doble click: generar_proyecto.bat
   (Crea archivos .hhp, .hhc, .hhk con soporte F1)

2. Doble click: compilar_directo.bat
   (Compila el CHM con mapeo de IDs)

3. Copiar: help_es.chm a la carpeta de la aplicación
   (O actualizar App.config con la ruta correcta)

4. Probar: Presionar F1 en cualquier formulario
   (Debería abrir la ayuda específica sin errores)
```

¡Listo! ??

---

**Última actualización:** 2024  
**Estado:** ? Probado y funcionando con soporte F1  
**Novedades:** Agregado mapeo automático de IDs para ayuda contextual
