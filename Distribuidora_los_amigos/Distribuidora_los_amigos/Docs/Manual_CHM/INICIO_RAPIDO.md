# ?? GUÍA RÁPIDA - Compilar CHM

## ? Solución Rápida (3 clicks)

Ya tienes las carpetas creadas en `C:\DistribuidoraLosAmigos\Manual\source_es\`, ahora solo necesitas:

### **Paso 1: Copiar archivos (Opción A - Automática)**

**Doble click en:**
```
compilar_simple.bat
```

? Este script ahora:
- Copia automáticamente los archivos si no existen
- Compila el CHM
- Todo en un solo paso

### **Paso 1: Copiar archivos (Opción B - Manual)**

Si prefieres copiar manualmente:

**Doble click en:**
```
copiar_archivos.bat
```

Luego ejecutar:
```
compilar_simple.bat
```

---

## ?? Scripts Disponibles

| Script | Función | Cuándo Usar |
|--------|---------|-------------|
| **compilar_simple.bat** | ? Copia archivos y compila | Usar siempre |
| **copiar_archivos.bat** | Copia archivos solamente | Solo si quieres copiar sin compilar |
| **diagnostico.bat** | Verifica estado | Si hay problemas |

---

## ?? Qué Hace Cada Script

### **compilar_simple.bat (RECOMENDADO)**
```
1. Verifica HTML Help Workshop
2. Copia automáticamente los archivos desde tu proyecto a C:\DistribuidoraLosAmigos\Manual\
3. Compila help_es.chm
4. Compila help_us.chm (si existe)
5. Muestra resumen
```

### **copiar_archivos.bat**
```
Copia todos los archivos del proyecto:
- help_es.hhp
- help_es.hhc
- help_es.hhk
- template.html
- css/
- html/
- images/

Desde: Distribuidora_los_amigos\Docs\Manual_CHM\source_es\
Hacia: C:\DistribuidoraLosAmigos\Manual\source_es\
```

### **diagnostico.bat**
```
Verifica:
- HTML Help Workshop instalado
- Archivos fuente en tu proyecto
- Archivos copiados al destino
- Archivos HTML
- CHM compilados
```

---

## ? Resultado Esperado

Después de ejecutar `compilar_simple.bat`, deberías ver:

```
==========================================
RESUMEN
==========================================
Archivos CHM generados:
help_es.chm

[OK] Proceso completado

Archivos generados en: C:\DistribuidoraLosAmigos\Manual
==========================================
```

Y el archivo estará en:
```
C:\DistribuidoraLosAmigos\Manual\help_es.chm
```

---

## ?? Solución de Problemas

### **Problema: "HTML Help Workshop no encontrado"**

? **Solución:**
1. Descargar: https://www.microsoft.com/en-us/download/details.aspx?id=21138
2. Instalar
3. Ejecutar de nuevo

### **Problema: "No se generaron archivos CHM"**

? **Solución:**
```
1. Ejecutar: diagnostico.bat
2. Ver qué falta
3. Seguir las recomendaciones
```

### **Problema: "Faltan archivos HTML"**

? **Solución:**
```
Los archivos HTML ya existen como ejemplos (3 páginas).
Para generar las 32 páginas:

Desde PowerShell:
.\generar_paginas.ps1

Luego:
compilar_simple.bat
```

---

## ?? Estructura de Archivos

```
Tu proyecto:
Distribuidora_los_amigos\Docs\Manual_CHM\source_es\
??? help_es.hhp
??? help_es.hhc
??? help_es.hhk
??? template.html
??? css\
?   ??? styles.css
??? html\
    ??? general\
    ??? login\
    ??? clientes\
    ??? ...

? COPIAR ?

Carpeta de compilación:
C:\DistribuidoraLosAmigos\Manual\source_es\
??? help_es.hhp
??? help_es.hhc
??? help_es.hhk
??? template.html
??? css\
?   ??? styles.css
??? html\
    ??? ...

? COMPILAR ?

Resultado:
C:\DistribuidoraLosAmigos\Manual\
??? help_es.chm ?
```

---

## ?? Pasos Simples (Resumen)

1. **Doble click:** `compilar_simple.bat`
2. **Esperar** (1-2 minutos)
3. **Verificar:** `C:\DistribuidoraLosAmigos\Manual\help_es.chm`
4. **Probar:** Doble click en el archivo .chm

¡Listo! ??

---

## ?? Necesitas Más Ayuda

- **Guía completa:** Ver `GUIA_CREACION_CHM.md`
- **Guía visual:** Ver `GUIA_VISUAL_HTML_HELP_WORKSHOP.md`
- **Resumen:** Ver `RESUMEN_EJECUTIVO.md`

---

**Actualizado:** 2024
**Estado:** ? Listo para usar
