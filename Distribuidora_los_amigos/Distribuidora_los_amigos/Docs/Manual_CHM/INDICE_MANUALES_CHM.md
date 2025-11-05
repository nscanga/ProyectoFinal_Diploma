# ?? ÍNDICE DE MANUALES - Sistema de Ayuda CHM

## ?? Documentación Disponible

Este directorio contiene toda la documentación necesaria para compilar y mantener los manuales de ayuda (archivos .CHM) del sistema Distribuidora Los Amigos.

---

## ?? Guías Principales

### 1. **MANUAL_COMPILACION_CHM_COMPLETO.md** ? MÁS COMPLETO
?? **Descripción:** Manual completo y detallado con toda la información necesaria
- ? Requisitos previos
- ? Estructura de archivos detallada
- ? Proceso paso a paso
- ? Corrección de errores comunes
- ? Replicar para otros idiomas
- ? Lista completa de 32 archivos HTML
- ? Códigos de idioma para CHM

?? **Usar cuando:**
- Es la primera vez que compilas un CHM
- Necesitas entender toda la estructura
- Quieres crear un manual en un nuevo idioma
- Necesitas referencia completa

?? **Nivel:** Principiante a Avanzado  
?? **Tiempo de lectura:** 20-30 minutos

---

### 2. **GUIA_RAPIDA_INGLES.md** ? GUÍA RÁPIDA
?? **Descripción:** Guía específica para crear el manual en INGLÉS
- ? 5 pasos rápidos
- ? Script de verificación incluido
- ? Ejemplos de traducción
- ? Lista priorizada de archivos a traducir

?? **Usar cuando:**
- Ya compilaste el manual en español
- Quieres crear la versión en inglés rápidamente
- Necesitas una guía paso a paso específica

?? **Nivel:** Intermedio  
?? **Tiempo de lectura:** 10-15 minutos  
?? **Tiempo de ejecución:** 30-45 minutos + traducción

---

### 3. **crear_manual_ingles.ps1** ?? AUTOMATIZACIÓN
?? **Descripción:** Script PowerShell que automatiza la creación del manual en inglés
- ? Copia estructura automáticamente
- ? Renombra archivos
- ? Edita archivo .hhp
- ? Compila el CHM
- ? Verifica resultado
- ? Interfaz visual con colores

?? **Usar cuando:**
- Quieres automatizar todo el proceso
- Necesitas crear varios manuales en diferentes idiomas
- Prefieres no hacer pasos manuales

?? **Nivel:** Todos (script automático)  
?? **Tiempo de ejecución:** 2-5 minutos

**Cómo ejecutar:**
```powershell
cd "C:\Mac\Home\Documents\ProyectoFinal_Diploma\Distribuidora_los_amigos\Docs\Manual_CHM"
.\crear_manual_ingles.ps1
```

---

## ?? Documentación Adicional (Ya Existente)

### **GUIA_RAPIDA_COMPILACION.md**
- Guía rápida de compilación (española)
- Comandos esenciales
- Solución de errores básicos

### **CHECKLIST_CONTENIDO.md**
- Lista de las 24 páginas pendientes
- Estado de implementación
- Prioridades

### **ESTADO_IMPLEMENTACION_F1.md**
- Estado general del sistema F1
- Formularios implementados
- Roadmap

### **SOLUCION_FINAL_F1_RECUPERAR_PASSWORD.md**
- Solución específica para RecuperarPasswordForm
- Logs y debugging

### **RESUMEN_PROXIMO_CHAT.md**
- Resumen ejecutivo del estado actual
- Guía para continuar en futuros chats

---

## ??? Flujo de Trabajo Recomendado

### **Opción A: Manual Detallado (Primera Vez)**
```
1. Leer MANUAL_COMPILACION_CHM_COMPLETO.md (todo)
2. Compilar manual en español (verificar que funciona)
3. Leer GUIA_RAPIDA_INGLES.md
4. Crear manual en inglés manualmente (paso a paso)
5. Traducir archivos HTML
6. Recompilar y verificar
```

### **Opción B: Automatizado (Rápido)**
```
1. Leer GUIA_RAPIDA_INGLES.md (5 minutos)
2. Ejecutar crear_manual_ingles.ps1
3. Traducir archivos HTML
4. Recompilar
```

### **Opción C: Semi-Automatizado (Recomendado)**
```
1. Leer GUIA_RAPIDA_INGLES.md (entender el proceso)
2. Ejecutar crear_manual_ingles.ps1 (automatizar estructura)
3. Verificar archivos generados
4. Traducir archivos HTML por prioridad (usar lista en guía)
5. Recompilar cada 5-10 archivos traducidos
6. Verificar resultado final
```

---

## ?? Conceptos Clave a Entender

### **1. Estructura de Archivos**
```
Manual\
??? source_es\              ? Español
?   ??? help_es.hhp         ? Proyecto CHM (CLAVE)
?   ??? help_es.hhc         ? Tabla de contenidos
?   ??? help_es.hhk         ? Índice
?   ??? css\
?   ?   ??? styles.css
?   ??? html\               ? 32 archivos HTML
?       ??? [carpetas por módulo]
??? source_en\              ? Inglés
?   ??? [misma estructura]
??? help_es.chm             ? CHM compilado español
??? help_en.chm             ? CHM compilado inglés
```

### **2. Archivo .hhp (Proyecto CHM)**
- **[OPTIONS]:** Configuración general
- **[WINDOWS]:** Configuración de ventana
- **[MAP]:** IDs numéricos (NUNCA cambiar entre idiomas)
- **[ALIAS]:** Mapeo ID ? archivo HTML
- **[FILES]:** Lista de archivos a incluir

### **3. Archivos HTML**
- **Nombres:** Iguales en todos los idiomas (ej: topic_31_login.html)
- **Contenido:** Traducido según el idioma
- **IDs:** Consistentes entre idiomas

### **4. Proceso de Compilación**
```
1. HTML Help Workshop lee el .hhp
2. Procesa todos los archivos en [FILES]
3. Genera el .chm comprimido
4. El .chm se ubica según "Compiled file="
```

---

## ?? Errores Comunes y Dónde Buscar Solución

| Error | Documento a Consultar | Sección |
|-------|----------------------|---------|
| "File not found" al compilar | MANUAL_COMPILACION_CHM_COMPLETO.md | Corrección de Errores > Error 1 |
| Warnings HHC3015 (alias incorrectos) | MANUAL_COMPILACION_CHM_COMPLETO.md | Paso 3: Identificar Errores en Alias |
| CHM compilado pero páginas en blanco | MANUAL_COMPILACION_CHM_COMPLETO.md | Corrección de Errores > Error 3 |
| Script no se ejecuta | GUIA_RAPIDA_INGLES.md | Errores Comunes > Error 1 |
| Contenido en español en manual inglés | GUIA_RAPIDA_INGLES.md | Traducir Contenido HTML |

---

## ?? Estadísticas del Sistema

### Manual Completo (cualquier idioma):
- **Archivos HTML:** 32
- **Topics en CHM:** 34 (32 HTML + 2 archivos especiales)
- **Links internos:** ~154
- **Tamaño CHM:** ~27 KB (comprimido)
- **Formularios con F1:** 27

### Módulos del Sistema:
| Módulo | Archivos HTML |
|--------|---------------|
| General | 2 |
| Login | 3 |
| Usuarios | 4 |
| Roles | 2 |
| Backup | 3 |
| Clientes | 3 |
| Productos | 4 |
| Proveedores | 3 |
| Stock | 2 |
| Pedidos | 4 |
| Reportes | 2 |

---

## ?? Inicio Rápido

### **¿Nunca compilaste un CHM?**
?? Empieza con: **MANUAL_COMPILACION_CHM_COMPLETO.md**

### **¿Ya compilaste el español y quieres crear inglés?**
?? Usa: **crear_manual_ingles.ps1** + **GUIA_RAPIDA_INGLES.md**

### **¿Necesitas crear otro idioma (ej: portugués)?**
?? Lee: **MANUAL_COMPILACION_CHM_COMPLETO.md** > Sección "Replicar para Otros Idiomas"

### **¿Tienes un error específico?**
?? Busca en: **MANUAL_COMPILACION_CHM_COMPLETO.md** > Sección "Corrección de Errores Comunes"

---

## ?? Recursos Externos

- **HTML Help Workshop:** C:\Program Files (x86)\HTML Help Workshop\
- **Documentación oficial:** Incluida con HTML Help Workshop
- **Códigos de idioma:** Ver tabla en MANUAL_COMPILACION_CHM_COMPLETO.md

---

## ?? Notas Importantes

### ? LO QUE ES IGUAL EN TODOS LOS IDIOMAS:
- IDs en sección [MAP]
- Nombres de archivos HTML
- Estructura de carpetas
- Archivos CSS

### ?? LO QUE CAMBIA POR IDIOMA:
- Contenido dentro de los HTML
- Título del manual
- Código de idioma en .hhp
- Nombres de archivos del proyecto (.hhp, .hhc, .hhk)

### ? LO QUE NUNCA DEBES CAMBIAR:
- IDs numéricos (20, 31, 32, etc.)
- Nombres de archivos HTML entre idiomas
- Estructura de la sección [MAP]

---

## ?? Historial de Versiones

### Versión 1.0 - 11 de Enero 2025
- ? Manual completo de compilación
- ? Guía rápida para inglés
- ? Script automatizado PowerShell
- ? Índice general

---

## ?? Contacto y Soporte

Para preguntas o problemas:
1. Consulta primero el manual correspondiente
2. Revisa la sección de errores comunes
3. Verifica los archivos de ejemplo (español)

---

**Última actualización:** 11 de Enero 2025  
**Mantenido por:** Equipo de Desarrollo - Distribuidora Los Amigos
