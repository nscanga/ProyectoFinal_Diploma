namespace Distribuidora_los_amigos.Forms.GestionUsuarios
{
    partial class CrearRolForm
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

        private void InitializeComponent()
        {
            this.labelRoleName = new System.Windows.Forms.Label();
            this.textBoxRoleName = new System.Windows.Forms.TextBox();
            this.checkedListBoxPatents = new System.Windows.Forms.CheckedListBox();
            this.buttonCreateRole = new System.Windows.Forms.Button();
            this.labelAssignToPatents = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelRoleName
            // 
            this.labelRoleName.AutoSize = true;
            this.labelRoleName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRoleName.Location = new System.Drawing.Point(12, 9);
            this.labelRoleName.Name = "labelRoleName";
            this.labelRoleName.Size = new System.Drawing.Size(132, 20);
            this.labelRoleName.TabIndex = 0;
            this.labelRoleName.Tag = "NombreDelRol";
            this.labelRoleName.Text = "Nombre del Rol";
            // 
            // textBoxRoleName
            // 
            this.textBoxRoleName.Location = new System.Drawing.Point(16, 32);
            this.textBoxRoleName.Name = "textBoxRoleName";
            this.textBoxRoleName.Size = new System.Drawing.Size(250, 20);
            this.textBoxRoleName.TabIndex = 1;
            // 
            // checkedListBoxPatents
            // 
            this.checkedListBoxPatents.FormattingEnabled = true;
            this.checkedListBoxPatents.Location = new System.Drawing.Point(16, 75);
            this.checkedListBoxPatents.Name = "checkedListBoxPatents";
            this.checkedListBoxPatents.Size = new System.Drawing.Size(250, 94);
            this.checkedListBoxPatents.TabIndex = 2;
            // 
            // buttonCreateRole
            // 
            this.buttonCreateRole.Location = new System.Drawing.Point(16, 180);
            this.buttonCreateRole.Name = "buttonCreateRole";
            this.buttonCreateRole.Size = new System.Drawing.Size(120, 30);
            this.buttonCreateRole.TabIndex = 3;
            this.buttonCreateRole.Tag = "CrearRol";
            this.buttonCreateRole.Text = "Crear Rol";
            this.buttonCreateRole.UseVisualStyleBackColor = true;
            this.buttonCreateRole.Click += new System.EventHandler(this.buttonCreateRole_Click);
            // 
            // labelAssignToPatents
            // 
            this.labelAssignToPatents.AutoSize = true;
            this.labelAssignToPatents.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAssignToPatents.Location = new System.Drawing.Point(13, 55);
            this.labelAssignToPatents.Name = "labelAssignToPatents";
            this.labelAssignToPatents.Size = new System.Drawing.Size(151, 17);
            this.labelAssignToPatents.TabIndex = 4;
            this.labelAssignToPatents.Tag = "AsignarRolAPatentes";
            this.labelAssignToPatents.Text = "Asignar rol a patentes:";
            // 
            // CrearRolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 216);
            this.Controls.Add(this.labelAssignToPatents);
            this.Controls.Add(this.buttonCreateRole);
            this.Controls.Add(this.checkedListBoxPatents);
            this.Controls.Add(this.textBoxRoleName);
            this.Controls.Add(this.labelRoleName);
            this.Name = "CrearRolForm";
            this.Tag = "CrearRol";
            this.Text = "Crear Rol";
            this.Load += new System.EventHandler(this.CrearRolForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        #endregion

        private System.Windows.Forms.Label labelAssignToPatents;
        private System.Windows.Forms.Button buttonCreateRole;
        private System.Windows.Forms.CheckedListBox checkedListBoxPatents;
        private System.Windows.Forms.TextBox textBoxRoleName;
        private System.Windows.Forms.Label labelRoleName;
    }
}