# ?? MANUAL COMPLETO - Compilación de Archivos CHM

## ?? Tabla de Contenidos
1. [Requisitos Previos](#requisitos-previos)
2. [Estructura de Archivos](#estructura-de-archivos)
3. [Proceso Paso a Paso](#proceso-paso-a-paso)
4. [Corrección de Errores Comunes](#corrección-de-errores-comunes)
5. [Replicar para Otros Idiomas](#replicar-para-otros-idiomas)
6. [Verificación Final](#verificación-final)

---

## ?? Requisitos Previos

### Software Necesario:
- ? **HTML Help Workshop** instalado en: `C:\Program Files (x86)\HTML Help Workshop\hhc.exe`
- ? **PowerShell** (incluido en Windows)
- ? Editor de texto (Visual Studio, VS Code, Notepad++)

### Verificar Instalación:
```powershell
Test-Path "C:\Program Files (x86)\HTML Help Workshop\hhc.exe"
# Debe retornar: True
```

---

## ?? Estructura de Archivos

### Ubicación de Archivos para ESPAÑOL:
```
C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\
??? Manual\
?   ??? source_es\                          ? Fuentes en español
?   ?   ??? help_es.hhp                     ? Proyecto CHM (ARCHIVO CLAVE)
?   ?   ??? help_es.hhc                     ? Tabla de contenidos
?   ?   ??? help_es.hhk                     ? Índice
?   ?   ??? css\
?   ?   ?   ??? styles.css                  ? Estilos CSS
?   ?   ??? html\                           ? Archivos HTML
?   ?       ??? general\
?   ?       ?   ??? topic_20_main.html
?   ?       ?   ??? topic_21_intro.html
?   ?       ??? login\
?   ?       ?   ??? topic_31_login.html
?   ?       ?   ??? topic_32_recuperar.html
?   ?       ?   ??? topic_33_cambiar_pass.html
?   ?       ??? usuarios\
?   ?       ?   ??? topic_23_crear_usuario.html
?   ?       ?   ??? topic_24_asignar_rol.html
?   ?       ?   ??? topic_28_modificar_usuario.html
?   ?       ?   ??? topic_29_mostrar_usuarios.html
?   ?       ??? roles\
?   ?       ?   ??? topic_26_crear_rol.html
?   ?       ?   ??? topic_27_crear_patente.html
?   ?       ??? backup\
?   ?       ?   ??? topic_22_backup.html
?   ?       ?   ??? topic_25_bitacora.html
?   ?       ?   ??? topic_30_restore.html
?   ?       ??? clientes\
?   ?       ?   ??? topic_40_crear_cliente.html
?   ?       ?   ??? topic_41_modificar_cliente.html
?   ?       ?   ??? topic_42_mostrar_clientes.html
?   ?       ??? productos\
?   ?       ?   ??? topic_50_crear_producto.html
?   ?       ?   ??? topic_51_modificar_producto.html
?   ?       ?   ??? topic_52_mostrar_productos.html
?   ?       ?   ??? topic_53_eliminar_producto.html
?   ?       ??? proveedores\
?   ?       ?   ??? topic_60_crear_proveedor.html
?   ?       ?   ??? topic_61_modificar_proveedor.html
?   ?       ?   ??? topic_62_mostrar_proveedores.html
?   ?       ??? stock\
?   ?       ?   ??? topic_70_mostrar_stock.html
?   ?       ?   ??? topic_71_modificar_stock.html
?   ?       ??? pedidos\
?   ?       ?   ??? topic_80_crear_pedido.html
?   ?       ?   ??? topic_81_modificar_pedido.html
?   ?       ?   ??? topic_82_mostrar_pedidos.html
?   ?       ?   ??? topic_83_detalle_pedido.html
?   ?       ??? reportes\
?   ?           ??? topic_90_reporte_stock_bajo.html
?   ?           ??? topic_91_reporte_mas_vendidos.html
?   ??? help_es.chm                         ? CHM compilado (RESULTADO)
```

### Para INGLÉS (estructura similar):
```
Manual\
??? source_en\                              ? Fuentes en inglés
?   ??? help_en.hhp                         ? Proyecto CHM en inglés
?   ??? help_en.hhc
?   ??? help_en.hhk
?   ??? css\
?   ?   ??? styles.css
?   ??? html\                               ? Archivos HTML en inglés
?       ??? [misma estructura que español]
??? help_en.chm                             ? CHM en inglés
```

---

## ?? Proceso Paso a Paso

### **PASO 1: Verificar Archivos HTML Existentes**

```powershell
# Ir a la carpeta de archivos HTML
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual\source_es\html"

# Contar archivos HTML
$count = (Get-ChildItem -Recurse -Filter "*.html").Count
Write-Host "Total de archivos HTML: $count" -ForegroundColor Green

# Listar todos los archivos
Get-ChildItem -Recurse -Filter "*.html" | Select-Object Directory, Name
```

**Resultado esperado:** 32 archivos HTML (todas las páginas del manual)

---

### **PASO 2: Revisar el Archivo .hhp (Proyecto CHM)**

El archivo `.hhp` es el **más importante**. Contiene:
- Configuración del proyecto
- Mapeo de IDs (sección [MAP])
- Alias a archivos HTML (sección [ALIAS])
- Lista de archivos a incluir (sección [FILES])

**Ubicación:** `Manual\source_es\help_es.hhp`

#### Estructura del archivo .hhp:

```ini
[OPTIONS]
Compatibility=1.1 or later
Compiled file=..\help_es.chm                    ? Ubicación del CHM compilado
Contents file=help_es.hhc
Default topic=html\general\topic_20_main.html   ? Página inicial
Display compile progress=Yes
Full-text search=Yes
Index file=help_es.hhk
Language=0x0c0a Espanol (Espana - alfabetizacion internacional)
Title=Manual de Usuario - Distribuidora Los Amigos
Default Window=main

[WINDOWS]
main="Manual de Usuario - Distribuidora Los Amigos","help_es.hhc","help_es.hhk","html\general\topic_20_main.html","html\general\topic_20_main.html",,,,,0x63520,,0x387e,,,,,,,,0

[MAP]
#define TOPIC_MAIN 20
#define TOPIC_LOGIN 31
#define TOPIC_RECUPERAR_PASSWORD 32
#define TOPIC_CAMBIAR_PASSWORD 33
#define TOPIC_CREAR_USUARIO 23
#define TOPIC_MODIFICAR_USUARIO 28
#define TOPIC_MOSTRAR_USUARIOS 29
#define TOPIC_ASIGNAR_ROL 24
#define TOPIC_CREAR_ROL 26
#define TOPIC_CREAR_PATENTE 27
#define TOPIC_BACKUP 22
#define TOPIC_RESTORE 30
#define TOPIC_BITACORA 25
#define TOPIC_CREAR_CLIENTE 40
#define TOPIC_MODIFICAR_CLIENTE 41
#define TOPIC_MOSTRAR_CLIENTES 42
#define TOPIC_CREAR_PRODUCTO 50
#define TOPIC_MODIFICAR_PRODUCTO 51
#define TOPIC_MOSTRAR_PRODUCTOS 52
#define TOPIC_ELIMINAR_PRODUCTO 53
#define TOPIC_CREAR_PROVEEDOR 60
#define TOPIC_MODIFICAR_PROVEEDOR 61
#define TOPIC_MOSTRAR_PROVEEDORES 62
#define TOPIC_MOSTRAR_STOCK 70
#define TOPIC_MODIFICAR_STOCK 71
#define TOPIC_CREAR_PEDIDO 80
#define TOPIC_MODIFICAR_PEDIDO 81
#define TOPIC_MOSTRAR_PEDIDOS 82
#define TOPIC_DETALLE_PEDIDO 83
#define TOPIC_REPORTE_STOCK_BAJO 90
#define TOPIC_REPORTE_PRODUCTOS_MAS_VENDIDOS 91

[ALIAS]
TOPIC_MAIN=html\general\topic_20_main.html
TOPIC_LOGIN=html\login\topic_31_login.html
TOPIC_RECUPERAR_PASSWORD=html\login\topic_32_recuperar.html
TOPIC_CAMBIAR_PASSWORD=html\login\topic_33_cambiar_pass.html
TOPIC_CREAR_USUARIO=html\usuarios\topic_23_crear_usuario.html
TOPIC_MODIFICAR_USUARIO=html\usuarios\topic_28_modificar_usuario.html
TOPIC_MOSTRAR_USUARIOS=html\usuarios\topic_29_mostrar_usuarios.html
TOPIC_ASIGNAR_ROL=html\usuarios\topic_24_asignar_rol.html
TOPIC_CREAR_ROL=html\roles\topic_26_crear_rol.html
TOPIC_CREAR_PATENTE=html\roles\topic_27_crear_patente.html
TOPIC_BACKUP=html\backup\topic_22_backup.html
TOPIC_RESTORE=html\backup\topic_30_restore.html
TOPIC_BITACORA=html\backup\topic_25_bitacora.html
TOPIC_CREAR_CLIENTE=html\clientes\topic_40_crear_cliente.html
TOPIC_MODIFICAR_CLIENTE=html\clientes\topic_41_modificar_cliente.html
TOPIC_MOSTRAR_CLIENTES=html\clientes\topic_42_mostrar_clientes.html
TOPIC_CREAR_PRODUCTO=html\productos\topic_50_crear_producto.html
TOPIC_MODIFICAR_PRODUCTO=html\productos\topic_51_modificar_producto.html
TOPIC_MOSTRAR_PRODUCTOS=html\productos\topic_52_mostrar_productos.html
TOPIC_ELIMINAR_PRODUCTO=html\productos\topic_53_eliminar_producto.html
TOPIC_CREAR_PROVEEDOR=html\proveedores\topic_60_crear_proveedor.html
TOPIC_MODIFICAR_PROVEEDOR=html\proveedores\topic_61_modificar_proveedor.html
TOPIC_MOSTRAR_PROVEEDORES=html\proveedores\topic_62_mostrar_proveedores.html
TOPIC_MOSTRAR_STOCK=html\stock\topic_70_mostrar_stock.html
TOPIC_MODIFICAR_STOCK=html\stock\topic_71_modificar_stock.html
TOPIC_CREAR_PEDIDO=html\pedidos\topic_80_crear_pedido.html
TOPIC_MODIFICAR_PEDIDO=html\pedidos\topic_81_modificar_pedido.html
TOPIC_MOSTRAR_PEDIDOS=html\pedidos\topic_82_mostrar_pedidos.html
TOPIC_DETALLE_PEDIDO=html\pedidos\topic_83_detalle_pedido.html
TOPIC_REPORTE_STOCK_BAJO=html\reportes\topic_90_reporte_stock_bajo.html
TOPIC_REPORTE_PRODUCTOS_MAS_VENDIDOS=html\reportes\topic_91_reporte_mas_vendidos.html

[FILES]
css\styles.css
html\backup\topic_22_backup.html
html\backup\topic_25_bitacora.html
html\backup\topic_30_restore.html
html\clientes\topic_40_crear_cliente.html
html\clientes\topic_41_modificar_cliente.html
html\clientes\topic_42_mostrar_clientes.html
html\general\topic_20_main.html
html\general\topic_21_intro.html
html\login\topic_31_login.html
html\login\topic_32_recuperar.html
html\login\topic_33_cambiar_pass.html
html\pedidos\topic_80_crear_pedido.html
html\pedidos\topic_81_modificar_pedido.html
html\pedidos\topic_82_mostrar_pedidos.html
html\pedidos\topic_83_detalle_pedido.html
html\productos\topic_50_crear_producto.html
html\productos\topic_51_modificar_producto.html
html\productos\topic_52_mostrar_productos.html
html\productos\topic_53_eliminar_producto.html
html\proveedores\topic_60_crear_proveedor.html
html\proveedores\topic_61_modificar_proveedor.html
html\proveedores\topic_62_mostrar_proveedores.html
html\reportes\topic_90_reporte_stock_bajo.html
html\reportes\topic_91_reporte_mas_vendidos.html
html\roles\topic_26_crear_rol.html
html\roles\topic_27_crear_patente.html
html\stock\topic_70_mostrar_stock.html
html\stock\topic_71_modificar_stock.html
html\usuarios\topic_23_crear_usuario.html
html\usuarios\topic_24_asignar_rol.html
html\usuarios\topic_28_modificar_usuario.html
html\usuarios\topic_29_mostrar_usuarios.html
```

---

### **PASO 3: Identificar Errores en Alias**

Los errores más comunes ocurren cuando:
- El alias apunta a un archivo con nombre diferente
- El alias apunta a una carpeta incorrecta

**Ejemplo de errores encontrados en español:**

| Alias | Ruta Incorrecta | Ruta Correcta |
|-------|----------------|---------------|
| `TOPIC_RECUPERAR_PASSWORD` | `html\login\topic_32_recuperar_password.html` | `html\login\topic_32_recuperar.html` |
| `TOPIC_CAMBIAR_PASSWORD` | `html\login\topic_33_cambiar_password.html` | `html\login\topic_33_cambiar_pass.html` |
| `TOPIC_ASIGNAR_ROL` | `html\roles\topic_24_asignar_rol.html` | `html\usuarios\topic_24_asignar_rol.html` |
| `TOPIC_BITACORA` | `html\reportes\topic_25_bitacora.html` | `html\backup\topic_25_bitacora.html` |
| `TOPIC_REPORTE_PRODUCTOS_MAS_VENDIDOS` | `topic_91_reporte_productos_mas_vendidos.html` | `topic_91_reporte_mas_vendidos.html` |

**Cómo detectar estos errores:**
El compilador mostrará warnings como:
```
HHC3015: Warning: An alias has been created to "html\login\topic_32_recuperar_password.html" but the file does not exist
```

---

### **PASO 4: Corregir el Archivo .hhp**

Editar el archivo `help_es.hhp` y corregir las rutas en la sección `[ALIAS]`:

```ini
[ALIAS]
# ...otras líneas...
TOPIC_RECUPERAR_PASSWORD=html\login\topic_32_recuperar.html
TOPIC_CAMBIAR_PASSWORD=html\login\topic_33_cambiar_pass.html
TOPIC_ASIGNAR_ROL=html\usuarios\topic_24_asignar_rol.html
TOPIC_BITACORA=html\backup\topic_25_bitacora.html
TOPIC_REPORTE_PRODUCTOS_MAS_VENDIDOS=html\reportes\topic_91_reporte_mas_vendidos.html
# ...otras líneas...
```

**?? IMPORTANTE:** Las rutas usan **backslash** `\` no forward slash `/`

---

### **PASO 5: Compilar el CHM**

```powershell
# Ir a la carpeta del proyecto
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual\source_es"

# Compilar usando HTML Help Workshop
& "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" "help_es.hhp"
```

**Salida esperada:**
```
Microsoft HTML Help Compiler 4.74.8702

Compiling c:\Mac\Home\Documents\...\Manual\help_es.chm

css\styles.css
html\backup\topic_22_backup.html
html\backup\topic_25_bitacora.html
...
[lista de todos los archivos]
...

Compile time: 0 minutes, 0 seconds
34	Topics
154	Local links
0	Internet links
0	Graphics

Created c:\Mac\Home\Documents\...\Manual\help_es.chm, 26,950 bytes
Compression decreased file by 133,819 bytes.
```

**Notas:**
- ? Los errores `HHC5003` son normales si faltan archivos
- ? Los warnings `HHC3015` indican alias incorrectos (deben corregirse)
- ? El warning `HHC4003` sobre BOM UTF-8 no afecta la funcionalidad

---

### **PASO 6: Verificar el CHM Generado**

```powershell
# Verificar que el CHM se generó
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual"

Get-Item "help_es.chm" | Select-Object Name, Length, LastWriteTime
```

**Salida esperada:**
```
Name        Length LastWriteTime     
----        ------ -------------     
help_es.chm  26950 4/11/2025 23:31:05
```

**Probar el CHM:**
1. Hacer doble clic en `help_es.chm`
2. Debe abrirse el visor de ayuda de Windows
3. Navegar por las páginas
4. Verificar que todos los links funcionan

---

## ?? Corrección de Errores Comunes

### Error 1: "File not found" al compilar

**Causa:** El archivo `.hhp` no existe o la ruta es incorrecta

**Solución:**
```powershell
# Verificar que el archivo existe
Test-Path "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual\source_es\help_es.hhp"
# Debe retornar: True
```

---

### Error 2: "HHC3015: Warning: An alias has been created to X but the file does not exist"

**Causa:** El alias apunta a un archivo con nombre incorrecto

**Solución:**
1. Verificar el nombre real del archivo HTML:
   ```powershell
   Get-ChildItem "Manual\source_es\html\login" -Filter "*.html"
   ```
2. Corregir el alias en el archivo `.hhp`

---

### Error 3: El CHM se compila pero las páginas están en blanco

**Causa:** Los archivos HTML existen pero están vacíos

**Solución:**
1. Abrir el archivo HTML y verificar que tiene contenido
2. Si está vacío, copiar la plantilla base y agregar contenido

---

### Error 4: "Access Denied" al compilar

**Causa:** El archivo CHM anterior está bloqueado

**Solución:**
```powershell
# Cerrar todas las ventanas de ayuda
# Eliminar el CHM anterior
Remove-Item "Manual\help_es.chm" -Force

# Compilar nuevamente
& "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" "Manual\source_es\help_es.hhp"
```

---

## ?? Replicar para Otros Idiomas

### **Crear Manual en INGLÉS**

#### PASO 1: Copiar Estructura de Carpetas

```powershell
# Ubicación base
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual"

# Copiar toda la carpeta source_es a source_en
Copy-Item "source_es" -Destination "source_en" -Recurse -Force
```

#### PASO 2: Renombrar Archivos del Proyecto

```powershell
cd "source_en"

# Renombrar archivos del proyecto
Rename-Item "help_es.hhp" "help_en.hhp"
Rename-Item "help_es.hhc" "help_en.hhc"
Rename-Item "help_es.hhk" "help_en.hhk"
```

#### PASO 3: Editar help_en.hhp

Abrir `help_en.hhp` y cambiar:

```ini
[OPTIONS]
Compatibility=1.1 or later
Compiled file=..\help_en.chm                    ? Cambiar a help_en.chm
Contents file=help_en.hhc                       ? Cambiar a help_en.hhc
Default topic=html\general\topic_20_main.html
Display compile progress=Yes
Full-text search=Yes
Index file=help_en.hhk                          ? Cambiar a help_en.hhk
Language=0x0409 English (United States)         ? Cambiar idioma
Title=User Manual - Distribuidora Los Amigos   ? Traducir título
Default Window=main

[WINDOWS]
main="User Manual - Distribuidora Los Amigos","help_en.hhc","help_en.hhk","html\general\topic_20_main.html","html\general\topic_20_main.html",,,,,0x63520,,0x387e,,,,,,,,0

[MAP]
# NO CAMBIAR - Los IDs deben ser los mismos que en español

[ALIAS]
# NO CAMBIAR - Los nombres de archivos HTML son los mismos

[FILES]
# NO CAMBIAR - Los nombres de archivos son los mismos
```

**?? CRÍTICO:** 
- Los IDs en `[MAP]` deben ser **exactamente iguales** en todos los idiomas
- Los nombres de archivos HTML deben ser **iguales** (solo cambia el contenido interno)
- Solo cambia: `Compiled file`, `Language`, `Title`, y los archivos `.hhc` y `.hhk`

#### PASO 4: Traducir Contenido HTML

Traducir el contenido **dentro** de cada archivo HTML, pero **NO** cambiar:
- Nombres de archivos
- Rutas de archivos
- IDs de tópicos

**Ejemplo - topic_31_login.html:**

**ESPAÑOL:**
```html
<h1>Iniciar Sesión</h1>
<p>Este formulario permite iniciar sesión en el sistema...</p>
```

**INGLÉS:**
```html
<h1>Login</h1>
<p>This form allows you to log in to the system...</p>
```

#### PASO 5: Compilar el Manual en Inglés

```powershell
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual\source_en"

& "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" "help_en.hhp"
```

#### PASO 6: Verificar el CHM en Inglés

```powershell
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Manual"

Get-Item "help_en.chm" | Select-Object Name, Length, LastWriteTime
```

---

### **Tabla de Idiomas Soportados**

Para otros idiomas, cambiar el código en `Language=`:

| Idioma | Código | Valor en .hhp |
|--------|--------|---------------|
| Español (España) | 0x0c0a | `Language=0x0c0a Espanol (Espana - alfabetizacion internacional)` |
| Inglés (Estados Unidos) | 0x0409 | `Language=0x0409 English (United States)` |
| Portugués (Portugal) | 0x0816 | `Language=0x0816 Portuguese (Portugal)` |
| Portugués (Brasil) | 0x0416 | `Language=0x0416 Portuguese (Brazil)` |
| Francés | 0x040c | `Language=0x040c French (France)` |
| Alemán | 0x0407 | `Language=0x0407 German (Germany)` |
| Italiano | 0x0410 | `Language=0x0410 Italian (Italy)` |

---

## ? Verificación Final

### Checklist de Verificación:

#### Para ESPAÑOL:
- [ ] Existe `Manual\source_es\help_es.hhp`
- [ ] Existen 32 archivos HTML en `Manual\source_es\html\`
- [ ] La sección `[MAP]` tiene 30 definiciones
- [ ] La sección `[ALIAS]` tiene 30 alias
- [ ] La sección `[FILES]` tiene 32 archivos HTML + 1 CSS
- [ ] El archivo `help_es.chm` se generó correctamente
- [ ] Al abrir `help_es.chm` se ve el contenido en español
- [ ] Todos los links internos funcionan

#### Para INGLÉS (o cualquier otro idioma):
- [ ] Existe `Manual\source_en\help_en.hhp`
- [ ] Existen 32 archivos HTML en `Manual\source_en\html\`
- [ ] Los IDs en `[MAP]` son **idénticos** a los de español
- [ ] Los nombres de archivos en `[ALIAS]` son **idénticos** a los de español
- [ ] El contenido HTML está traducido al inglés
- [ ] `Compiled file=..\help_en.chm`
- [ ] `Language=0x0409` (inglés)
- [ ] El archivo `help_en.chm` se generó correctamente
- [ ] Al abrir `help_en.chm` se ve el contenido en inglés
- [ ] Todos los links internos funcionan

---

## ?? Resumen de Comandos Importantes

### Verificar archivos HTML:
```powershell
cd "Manual\source_es\html"
(Get-ChildItem -Recurse -Filter "*.html").Count
```

### Compilar CHM (español):
```powershell
cd "Manual\source_es"
& "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" "help_es.hhp"
```

### Compilar CHM (inglés):
```powershell
cd "Manual\source_en"
& "C:\Program Files (x86)\HTML Help Workshop\hhc.exe" "help_en.hhp"
```

### Verificar CHM generado:
```powershell
cd "Manual"
Get-Item "help_*.chm" | Select-Object Name, Length, LastWriteTime
```

---

## ?? Lista de Archivos HTML Requeridos

### Total: 32 archivos

#### General (2 archivos):
1. `html\general\topic_20_main.html` - Página principal
2. `html\general\topic_21_intro.html` - Introducción

#### Login (3 archivos):
3. `html\login\topic_31_login.html` - Login
4. `html\login\topic_32_recuperar.html` - Recuperar contraseña
5. `html\login\topic_33_cambiar_pass.html` - Cambiar contraseña

#### Usuarios (4 archivos):
6. `html\usuarios\topic_23_crear_usuario.html` - Crear usuario
7. `html\usuarios\topic_24_asignar_rol.html` - Asignar rol
8. `html\usuarios\topic_28_modificar_usuario.html` - Modificar usuario
9. `html\usuarios\topic_29_mostrar_usuarios.html` - Mostrar usuarios

#### Roles (2 archivos):
10. `html\roles\topic_26_crear_rol.html` - Crear rol
11. `html\roles\topic_27_crear_patente.html` - Crear patente

#### Backup (3 archivos):
12. `html\backup\topic_22_backup.html` - Backup
13. `html\backup\topic_25_bitacora.html` - Bitácora
14. `html\backup\topic_30_restore.html` - Restore

#### Clientes (3 archivos):
15. `html\clientes\topic_40_crear_cliente.html` - Crear cliente
16. `html\clientes\topic_41_modificar_cliente.html` - Modificar cliente
17. `html\clientes\topic_42_mostrar_clientes.html` - Mostrar clientes

#### Productos (4 archivos):
18. `html\productos\topic_50_crear_producto.html` - Crear producto
19. `html\productos\topic_51_modificar_producto.html` - Modificar producto
20. `html\productos\topic_52_mostrar_productos.html` - Mostrar productos
21. `html\productos\topic_53_eliminar_producto.html` - Eliminar producto

#### Proveedores (3 archivos):
22. `html\proveedores\topic_60_crear_proveedor.html` - Crear proveedor
23. `html\proveedores\topic_61_modificar_proveedor.html` - Modificar proveedor
24. `html\proveedores\topic_62_mostrar_proveedores.html` - Mostrar proveedores

#### Stock (2 archivos):
25. `html\stock\topic_70_mostrar_stock.html` - Mostrar stock
26. `html\stock\topic_71_modificar_stock.html` - Modificar stock

#### Pedidos (4 archivos):
27. `html\pedidos\topic_80_crear_pedido.html` - Crear pedido
28. `html\pedidos\topic_81_modificar_pedido.html` - Modificar pedido
29. `html\pedidos\topic_82_mostrar_pedidos.html` - Mostrar pedidos
30. `html\pedidos\topic_83_detalle_pedido.html` - Detalle pedido

#### Reportes (2 archivos):
31. `html\reportes\topic_90_reporte_stock_bajo.html` - Reporte stock bajo
32. `html\reportes\topic_91_reporte_mas_vendidos.html` - Productos más vendidos

---

## ?? Notas Finales

### Puntos Clave:
1. **Los IDs nunca cambian** entre idiomas (20, 31, 32, etc.)
2. **Los nombres de archivos HTML nunca cambian** entre idiomas
3. **Solo el contenido interno** de los HTML se traduce
4. **El archivo .hhp** debe corregirse antes de compilar
5. **Los alias deben apuntar a archivos existentes**

### Errores Comunes a Evitar:
- ? Cambiar los IDs en diferentes idiomas
- ? Renombrar archivos HTML por idioma
- ? Usar forward slash `/` en lugar de backslash `\`
- ? No verificar que todos los archivos HTML existan antes de compilar
- ? Olvidar cambiar `Compiled file=` al crear un nuevo idioma

### Buenas Prácticas:
- ? Siempre compilar después de cada cambio
- ? Verificar warnings del compilador
- ? Probar el CHM abriendo varias páginas
- ? Mantener backup de archivos funcionales
- ? Documentar cualquier cambio en la estructura

---

## ?? Referencias

- **HTML Help Workshop:** Herramienta oficial de Microsoft
- **Ubicación oficial:** C:\Program Files (x86)\HTML Help Workshop\
- **Documentación:** Incluida con HTML Help Workshop

---

**Creado:** 11 de Enero 2025  
**Versión:** 1.0  
**Autor:** Sistema de Ayuda F1 - Distribuidora Los Amigos  
**Estado:** ? Completado y Verificado
