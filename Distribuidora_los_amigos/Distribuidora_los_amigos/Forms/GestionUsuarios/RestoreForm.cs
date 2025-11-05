using Service.Facade;
using Services.Facade;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Service.DAL.Contracts;

namespace Distribuidora_los_amigos.Forms.GestionUsuarios
{
    /// <summary>
    /// Formulario para restaurar bases de datos desde archivos de backup.
    /// </summary>
    public partial class RestoreForm : Form, IIdiomaObserver
    {
        /// <summary>
        /// Inicializa el formulario de restauración y registra el evento de carga.
        /// </summary>
        public RestoreForm()
        {
            InitializeComponent();
            this.Load += RestoreForm_Load;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosed += RestoreForm_FormClosed;
            this.KeyPreview = true;
            this.KeyDown += RestoreForm_KeyDown;

            // Suscribirse al servicio de idiomas
            IdiomaService.Subscribe(this);

            // Traducir el formulario al cargarlo
            IdiomaService.TranslateForm(this);
        }

        /// <summary>
        /// Conecta el botón principal a la lógica de restauración.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void RestoreForm_Load(object sender, EventArgs e)
        {
            try
            {
                // ? Configurar el evento del botón
                btnRestore.Click += BtnRestore_Click;

                LoggerService.WriteLog("Formulario de restauración de base de datos abierto.", System.Diagnostics.TraceLevel.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el formulario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Ejecuta el flujo de selección de carpeta y restauración de bases de datos mostrando retroalimentación al usuario.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void BtnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                // ?? ADVERTENCIA CRÍTICA
                DialogResult warningResult = MessageBox.Show(
                    "?? ADVERTENCIA IMPORTANTE ??\n\n" +
                    "La restauración de la base de datos:\n" +
                    "• Reemplazará TODOS los datos actuales\n" +
                    "• Cerrará todas las conexiones activas\n" +
                    "• No se puede deshacer esta operación\n\n" +
                    "Se recomienda:\n" +
                    "• Crear un backup de la base de datos actual antes de continuar\n" +
                    "• Cerrar todas las aplicaciones conectadas a la base de datos\n" +
                    "• Asegurarse de seleccionar los archivos correctos\n\n" +
                    "¿Está ABSOLUTAMENTE SEGURO de que desea continuar?",
                    "?? Advertencia Crítica - Restaurar Base de Datos",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (warningResult != DialogResult.Yes)
                {
                    return;
                }

                // ? Mostrar diálogo para seleccionar carpeta con los backups
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
                            "¿Desea continuar con la restauración?",
                            "Confirmar Archivos de Backup",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (confirmFiles == DialogResult.Yes)
                        {
                            // ? Confirmación final
                            DialogResult finalConfirm = MessageBox.Show(
                                "ÚLTIMA ADVERTENCIA:\n\n" +
                                "Esta acción ELIMINARÁ todos los datos actuales y los reemplazará con los datos del backup.\n\n" +
                                "¿Confirma que desea proceder?",
                                "Confirmación Final",
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
                                    // ? Ejecutar la restauración
                                    BackupService.ExecuteRestore(selectedPath);

                                    // ? Mostrar mensaje de éxito
                                    MessageBox.Show(
                                        "? Restauración completada exitosamente.\n\n" +
                                        "Las bases de datos han sido restauradas correctamente.\n\n" +
                                        "?? IMPORTANTE: Se recomienda reiniciar la aplicación para evitar problemas de conexión.",
                                        "Restauración Exitosa",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);

                                    LoggerService.WriteLog($"Restauración ejecutada exitosamente desde: {selectedPath}", System.Diagnostics.TraceLevel.Info);

                                    // ? Preguntar si desea reiniciar la aplicación
                                    DialogResult restartResult = MessageBox.Show(
                                        "¿Desea reiniciar la aplicación ahora?",
                                        "Reiniciar Aplicación",
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
                    "Por favor, seleccione otra ubicación o ejecute la aplicación como administrador.",
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
                    "• Que los archivos de backup sean válidos\n" +
                    "• Que no haya otras aplicaciones usando la base de datos\n" +
                    "• Que tenga permisos suficientes\n" +
                    "• Los logs del sistema para más detalles",
                    "Error en la Restauración",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                LoggerService.WriteException(ex);
                LoggerService.WriteLog($"Error al restaurar base de datos: {ex.Message}", System.Diagnostics.TraceLevel.Error);
            }
        }

        /// <summary>
        /// Actualiza los textos del formulario cuando cambia el idioma.
        /// </summary>
        public void UpdateIdioma()
        {
            IdiomaService.TranslateForm(this);
            this.Refresh();
        }

        /// <summary>
        /// Registra el cierre del formulario.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento de cierre.</param>
        private void RestoreForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Desuscribirse del servicio de idiomas
            IdiomaService.Unsubscribe(this);

            // Registrar cuando el formulario se cierra
            LoggerService.WriteLog($"Formulario '{this.Text}' cerrado.", System.Diagnostics.TraceLevel.Info);
        }

        /// <summary>
        /// Muestra la ayuda del formulario cuando se presiona F1.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void RestoreForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ManualService manualService = new ManualService();
                    manualService.AbrirAyudaRestore();
                    e.Handled = true; // Prevenir propagación del evento
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ayuda: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }
    }
}
