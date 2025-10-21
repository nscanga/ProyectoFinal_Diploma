using Service.Facade;
using Services.Facade;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Distribuidora_los_amigos.Forms.GestionUsuarios
{
    public partial class BackUpForm : Form
    {
        /// <summary>
        /// Inicializa el formulario de backups y registra el evento de carga.
        /// </summary>
        public BackUpForm()
        {
            InitializeComponent();
            this.Load += BackUpForm_Load;
        }

        /// <summary>
        /// Conecta el botón principal a la lógica de creación de backups.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void BackUpForm_Load(object sender, EventArgs e)
        {
            try
            {
                // ✅ Configurar el evento del botón
                button1.Click += Button1_Click;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el formulario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerService.WriteException(ex);
            }
        }

        /// <summary>
        /// Ejecuta el flujo de selección de carpeta y generación de backup mostrando retroalimentación al usuario.
        /// </summary>
        /// <param name="sender">Origen del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ Mostrar diálogo para seleccionar carpeta
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Seleccione la carpeta donde desea guardar el backup";
                    folderDialog.ShowNewFolderButton = true;

                    // ✅ Intentar establecer una ruta por defecto
                    List<string> availablePaths = BackupService.GetAvailableBackupPaths();
                    if (availablePaths.Count > 0 && Directory.Exists(availablePaths[0]))
                    {
                        folderDialog.SelectedPath = availablePaths[0];
                    }

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedPath = folderDialog.SelectedPath;

                        // ✅ Confirmar acción
                        DialogResult confirm = MessageBox.Show(
                            $"¿Está seguro de que desea crear un backup en la siguiente ubicación?\n\n{selectedPath}",
                            "Confirmar Backup",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (confirm == DialogResult.Yes)
                        {
                            // ✅ Mostrar mensaje de progreso
                            Cursor = Cursors.WaitCursor;
                            button1.Enabled = false;
                            button1.Text = "Creando backup...";

                            try
                            {
                                // ✅ Ejecutar el backup
                                BackupService.ExecuteBackup(selectedPath);

                                // ✅ Mostrar mensaje de éxito
                                MessageBox.Show(
                                    $"Backup creado exitosamente en:\n{selectedPath}",
                                    "Éxito",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                                LoggerService.WriteLog($"Backup ejecutado manualmente por el usuario en: {selectedPath}", System.Diagnostics.TraceLevel.Info);
                            }
                            finally
                            {
                                // ✅ Restaurar UI
                                Cursor = Cursors.Default;
                                button1.Enabled = true;
                                button1.Text = "Crear BackUp";
                            }
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(
                    "No tiene permisos para acceder a la carpeta seleccionada. Por favor, seleccione otra ubicación.",
                    "Error de Permisos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al crear el backup:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                LoggerService.WriteException(ex);
            }
        }
    }
}
