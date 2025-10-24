using Service.Facade;
using Services.Facade;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Distribuidora_los_amigos.Forms.GestionUsuarios
{
    /// <summary>
    /// Formulario para restaurar bases de datos desde archivos de backup.
    /// </summary>
    public partial class RestoreForm : Form
    {
        /// <summary>
        /// Inicializa el formulario de restauraci�n y registra el evento de carga.
        /// </summary>
        public RestoreForm()
        {
            InitializeComponent();
            this.Load += RestoreForm_Load;
        }

        /// <summary>
        /// Conecta el bot�n principal a la l�gica de restauraci�n.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void RestoreForm_Load(object sender, EventArgs e)
        {
            try
            {
                // ? Configurar el evento del bot�n
                btnRestore.Click += BtnRestore_Click;
                
                LoggerService.WriteLog("Formulario de restauraci�n de base de datos abierto.", System.Diagnostics.TraceLevel.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el formulario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Ejecuta el flujo de selecci�n de carpeta y restauraci�n de bases de datos mostrando retroalimentaci�n al usuario.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void BtnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                // ?? ADVERTENCIA CR�TICA
                DialogResult warningResult = MessageBox.Show(
                    "?? ADVERTENCIA IMPORTANTE ??\n\n" +
                    "La restauraci�n de la base de datos:\n" +
                    "� Reemplazar� TODOS los datos actuales\n" +
                    "� Cerrar� todas las conexiones activas\n" +
                    "� No se puede deshacer esta operaci�n\n\n" +
                    "Se recomienda:\n" +
                    "� Crear un backup de la base de datos actual antes de continuar\n" +
                    "� Cerrar todas las aplicaciones conectadas a la base de datos\n" +
                    "� Asegurarse de seleccionar los archivos correctos\n\n" +
                    "�Est� ABSOLUTAMENTE SEGURO de que desea continuar?",
                    "?? Advertencia Cr�tica - Restaurar Base de Datos",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (warningResult != DialogResult.Yes)
                {
                    return;
                }

                // ? Mostrar di�logo para seleccionar carpeta con los backups
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Seleccione la carpeta que contiene los archivos de backup (.bak)";
                    folderDialog.ShowNewFolderButton = false;

                    // ? Intentar establecer una ruta por defecto
                    List<string> availablePaths = BackupService.GetAvailableBackupPaths();
                    if (availablePaths.Count > 0 && Directory.Exists(availablePaths[0]))
                    {
                        folderDialog.SelectedPath = availablePaths[0];
                    }

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedPath = folderDialog.SelectedPath;

                        // ? Validar que la carpeta contenga archivos .bak
                        string[] bakFiles = Directory.GetFiles(selectedPath, "*.bak");
                        if (bakFiles.Length == 0)
                        {
                            MessageBox.Show(
                                "La carpeta seleccionada no contiene archivos de backup (.bak).\n\n" +
                                "Por favor, seleccione una carpeta que contenga los archivos de backup.",
                                "No se encontraron archivos de backup",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }

                        // ? Mostrar archivos encontrados
                        string fileList = string.Join("\n", Array.ConvertAll(bakFiles, Path.GetFileName));
                        DialogResult confirmFiles = MessageBox.Show(
                            $"Se encontraron {bakFiles.Length} archivo(s) de backup:\n\n{fileList}\n\n" +
                            "�Desea continuar con la restauraci�n?",
                            "Confirmar Archivos de Backup",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (confirmFiles == DialogResult.Yes)
                        {
                            // ? Confirmaci�n final
                            DialogResult finalConfirm = MessageBox.Show(
                                "�LTIMA ADVERTENCIA:\n\n" +
                                "Esta acci�n ELIMINAR� todos los datos actuales y los reemplazar� con los datos del backup.\n\n" +
                                "�Confirma que desea proceder?",
                                "Confirmaci�n Final",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Exclamation);

                            if (finalConfirm == DialogResult.Yes)
                            {
                                // ? Mostrar mensaje de progreso
                                Cursor = Cursors.WaitCursor;
                                btnRestore.Enabled = false;
                                btnRestore.Text = "Restaurando...";

                                try
                                {
                                    // ? Ejecutar la restauraci�n
                                    BackupService.ExecuteRestore(selectedPath);

                                    // ? Mostrar mensaje de �xito
                                    MessageBox.Show(
                                        "? Restauraci�n completada exitosamente.\n\n" +
                                        "Las bases de datos han sido restauradas correctamente.\n\n" +
                                        "?? IMPORTANTE: Se recomienda reiniciar la aplicaci�n para evitar problemas de conexi�n.",
                                        "Restauraci�n Exitosa",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);

                                    LoggerService.WriteLog($"Restauraci�n ejecutada exitosamente desde: {selectedPath}", System.Diagnostics.TraceLevel.Info);

                                    // ? Preguntar si desea reiniciar la aplicaci�n
                                    DialogResult restartResult = MessageBox.Show(
                                        "�Desea reiniciar la aplicaci�n ahora?",
                                        "Reiniciar Aplicaci�n",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question);

                                    if (restartResult == DialogResult.Yes)
                                    {
                                        Application.Restart();
                                    }
                                }
                                finally
                                {
                                    // ? Restaurar UI
                                    Cursor = Cursors.Default;
                                    btnRestore.Enabled = true;
                                    btnRestore.Text = "Restaurar Base de Datos";
                                }
                            }
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(
                    "No tiene permisos para acceder a la carpeta seleccionada.\n\n" +
                    "Por favor, seleccione otra ubicaci�n o ejecute la aplicaci�n como administrador.",
                    "Error de Permisos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                
                LoggerService.WriteLog("Error de permisos al intentar restaurar base de datos.", System.Diagnostics.TraceLevel.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"? Error al restaurar la base de datos:\n\n{ex.Message}\n\n" +
                    "Por favor, verifique:\n" +
                    "� Que los archivos de backup sean v�lidos\n" +
                    "� Que no haya otras aplicaciones usando la base de datos\n" +
                    "� Que tenga permisos suficientes\n" +
                    "� Los logs del sistema para m�s detalles",
                    "Error en la Restauraci�n",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                LoggerService.WriteException(ex);
                LoggerService.WriteLog($"Error al restaurar base de datos: {ex.Message}", System.Diagnostics.TraceLevel.Error);
            }
        }
    }
}
