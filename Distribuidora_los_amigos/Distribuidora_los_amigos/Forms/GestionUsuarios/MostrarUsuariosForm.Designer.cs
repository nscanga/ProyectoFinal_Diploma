
namespace Distribuidora_los_amigos.Forms.GestionUsuarios
{
    partial class MostrarUsuariosForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelTitle = new System.Windows.Forms.Label();
            this.sfDataGrid = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(12, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(118, 20);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Tag = "VerUsuarios";
            this.labelTitle.Text = "Ver Usuarios:";
            // 
            // sfDataGrid
            // 
            this.sfDataGrid.AccessibleName = "Table";
            this.sfDataGrid.AllowEditing = false;
            this.sfDataGrid.AllowFiltering = true;
            this.sfDataGrid.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.AllCells;
            this.sfDataGrid.Location = new System.Drawing.Point(16, 42);
            this.sfDataGrid.Name = "sfDataGrid";
            this.sfDataGrid.Size = new System.Drawing.Size(760, 400);
            this.sfDataGrid.TabIndex = 1;
            this.sfDataGrid.Text = "sfDataGrid";
            // 
            // MostrarUsuariosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.sfDataGrid);
            this.Controls.Add(this.labelTitle);
            this.Name = "MostrarUsuariosForm";
            this.Text = "View Users";
            this.Load += new System.EventHandler(this.MostrarUsuariosForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Label labelTitle;
        private Syncfusion.WinForms.DataGrid.SfDataGrid sfDataGrid;
        #endregion


    }
}