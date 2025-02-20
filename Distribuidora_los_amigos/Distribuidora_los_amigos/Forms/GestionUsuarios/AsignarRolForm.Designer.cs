namespace Distribuidora_los_amigos.Forms.GestionUsuarios
{
    partial class AsignarRolForm
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
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelUser = new System.Windows.Forms.Label();
            this.labelRole = new System.Windows.Forms.Label();
            this.comboBoxUsers = new System.Windows.Forms.ComboBox();
            this.comboBoxRoles = new System.Windows.Forms.ComboBox();
            this.buttonAssignRole = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(12, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(102, 20);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Tag = "AsignarRol";
            this.labelTitle.Text = "Asignar Rol";
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(12, 40);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(105, 13);
            this.labelUser.TabIndex = 1;
            this.labelUser.Tag = "SeleccionarUsuario";
            this.labelUser.Text = "Seleccionar Usuario:";
            // 
            // labelRole
            // 
            this.labelRole.AutoSize = true;
            this.labelRole.Location = new System.Drawing.Point(12, 80);
            this.labelRole.Name = "labelRole";
            this.labelRole.Size = new System.Drawing.Size(85, 13);
            this.labelRole.TabIndex = 2;
            this.labelRole.Tag = "SeleccionarRol";
            this.labelRole.Text = "Seleccionar Rol:";
            // 
            // comboBoxUsers
            // 
            this.comboBoxUsers.FormattingEnabled = true;
            this.comboBoxUsers.Location = new System.Drawing.Point(15, 56);
            this.comboBoxUsers.Name = "comboBoxUsers";
            this.comboBoxUsers.Size = new System.Drawing.Size(250, 21);
            this.comboBoxUsers.TabIndex = 3;
            // 
            // comboBoxRoles
            // 
            this.comboBoxRoles.FormattingEnabled = true;
            this.comboBoxRoles.Location = new System.Drawing.Point(15, 96);
            this.comboBoxRoles.Name = "comboBoxRoles";
            this.comboBoxRoles.Size = new System.Drawing.Size(250, 21);
            this.comboBoxRoles.TabIndex = 4;
            // 
            // buttonAssignRole
            // 
            this.buttonAssignRole.Location = new System.Drawing.Point(15, 136);
            this.buttonAssignRole.Name = "buttonAssignRole";
            this.buttonAssignRole.Size = new System.Drawing.Size(120, 30);
            this.buttonAssignRole.TabIndex = 5;
            this.buttonAssignRole.Tag = "AsignarRol";
            this.buttonAssignRole.Text = "Asignar Rol";
            this.buttonAssignRole.UseVisualStyleBackColor = true;
            this.buttonAssignRole.Click += new System.EventHandler(this.buttonAssignRole_Click);
            // 
            // AsignarRolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.Controls.Add(this.buttonAssignRole);
            this.Controls.Add(this.comboBoxRoles);
            this.Controls.Add(this.comboBoxUsers);
            this.Controls.Add(this.labelRole);
            this.Controls.Add(this.labelUser);
            this.Controls.Add(this.labelTitle);
            this.Name = "AsignarRolForm";
            this.Text = "Asignar Rol";
            this.Load += new System.EventHandler(this.AsignarRolForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        

        #endregion

        private System.Windows.Forms.Button buttonAssignRole;
        private System.Windows.Forms.ComboBox comboBoxRoles;
        private System.Windows.Forms.ComboBox comboBoxUsers;
        private System.Windows.Forms.Label labelRole;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelTitle;
    }
}