namespace Distribuidora_los_amigos.Forms.GestionUsuarios
{
    partial class CrearPatenteForm
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
            this.labelNombre = new System.Windows.Forms.Label();
            this.labelTipoAcceso = new System.Windows.Forms.Label();
            this.textBoxNombre = new System.Windows.Forms.TextBox();
            this.comboBoxTipoAcceso = new System.Windows.Forms.ComboBox();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(12, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(165, 20);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Tag = "CrearNuevaPatente";
            this.labelTitle.Text = "Crear nueva patente";
            // 
            // labelNombre
            // 
            this.labelNombre.AutoSize = true;
            this.labelNombre.Location = new System.Drawing.Point(13, 40);
            this.labelNombre.Name = "labelNombre";
            this.labelNombre.Size = new System.Drawing.Size(47, 13);
            this.labelNombre.TabIndex = 1;
            this.labelNombre.Tag = "Nombre";
            this.labelNombre.Text = "Nombre:";
            // 
            // labelTipoAcceso
            // 
            this.labelTipoAcceso.AutoSize = true;
            this.labelTipoAcceso.Location = new System.Drawing.Point(13, 80);
            this.labelTipoAcceso.Name = "labelTipoAcceso";
            this.labelTipoAcceso.Size = new System.Drawing.Size(84, 13);
            this.labelTipoAcceso.TabIndex = 2;
            this.labelTipoAcceso.Tag = "TipoDeAcceso";
            this.labelTipoAcceso.Text = "Tipo de acceso:";
            // 
            // textBoxNombre
            // 
            this.textBoxNombre.Location = new System.Drawing.Point(16, 56);
            this.textBoxNombre.Name = "textBoxNombre";
            this.textBoxNombre.Size = new System.Drawing.Size(250, 20);
            this.textBoxNombre.TabIndex = 3;
            // 
            // comboBoxTipoAcceso
            // 
            this.comboBoxTipoAcceso.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTipoAcceso.FormattingEnabled = true;
            this.comboBoxTipoAcceso.Location = new System.Drawing.Point(16, 96);
            this.comboBoxTipoAcceso.Name = "comboBoxTipoAcceso";
            this.comboBoxTipoAcceso.Size = new System.Drawing.Size(250, 21);
            this.comboBoxTipoAcceso.TabIndex = 4;
            // 
            // buttonCreate
            // 
            this.buttonCreate.Location = new System.Drawing.Point(16, 136);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(120, 30);
            this.buttonCreate.TabIndex = 5;
            this.buttonCreate.Tag = "CrearPatente";
            this.buttonCreate.Text = "Crear Patente";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // CrearPatenteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.comboBoxTipoAcceso);
            this.Controls.Add(this.textBoxNombre);
            this.Controls.Add(this.labelTipoAcceso);
            this.Controls.Add(this.labelNombre);
            this.Controls.Add(this.labelTitle);
            this.Name = "CrearPatenteForm";
            this.Tag = "CrearPatente";
            this.Text = "Crear Patente";
            this.Load += new System.EventHandler(this.CrearPatenteForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelNombre;
        private System.Windows.Forms.Label labelTipoAcceso;
        private System.Windows.Forms.TextBox textBoxNombre;
        private System.Windows.Forms.ComboBox comboBoxTipoAcceso;
        private System.Windows.Forms.Button buttonCreate;
    }
}