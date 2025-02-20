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
            this.buttonSave = new System.Windows.Forms.Button();
            this.comboBoxAccessType = new System.Windows.Forms.ComboBox();
            this.labelAccessType = new System.Windows.Forms.Label();
            this.textBoxDataKey = new System.Windows.Forms.TextBox();
            this.labelDataKey = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(162, 174);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(100, 30);
            this.buttonSave.TabIndex = 13;
            this.buttonSave.Tag = "Guardar";
            this.buttonSave.Text = "Guardar";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // comboBoxAccessType
            // 
            this.comboBoxAccessType.FormattingEnabled = true;
            this.comboBoxAccessType.Items.AddRange(new object[] {
            "Lectura",
            "Escritura",
            "Admin"});
            this.comboBoxAccessType.Location = new System.Drawing.Point(12, 136);
            this.comboBoxAccessType.Name = "comboBoxAccessType";
            this.comboBoxAccessType.Size = new System.Drawing.Size(250, 21);
            this.comboBoxAccessType.TabIndex = 12;
            // 
            // labelAccessType
            // 
            this.labelAccessType.AutoSize = true;
            this.labelAccessType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAccessType.Location = new System.Drawing.Point(9, 116);
            this.labelAccessType.Name = "labelAccessType";
            this.labelAccessType.Size = new System.Drawing.Size(126, 17);
            this.labelAccessType.TabIndex = 11;
            this.labelAccessType.Tag = "ElegirTipoAcceso";
            this.labelAccessType.Text = "Elegir Tipo Acceso";
            // 
            // textBoxDataKey
            // 
            this.textBoxDataKey.Location = new System.Drawing.Point(12, 86);
            this.textBoxDataKey.Name = "textBoxDataKey";
            this.textBoxDataKey.Size = new System.Drawing.Size(250, 20);
            this.textBoxDataKey.TabIndex = 10;
            // 
            // labelDataKey
            // 
            this.labelDataKey.AutoSize = true;
            this.labelDataKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDataKey.Location = new System.Drawing.Point(9, 66);
            this.labelDataKey.Name = "labelDataKey";
            this.labelDataKey.Size = new System.Drawing.Size(62, 17);
            this.labelDataKey.TabIndex = 9;
            this.labelDataKey.Tag = "DataKey";
            this.labelDataKey.Text = "DataKey";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(12, 36);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(250, 20);
            this.textBoxName.TabIndex = 8;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelName.Location = new System.Drawing.Point(9, 16);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(58, 17);
            this.labelName.TabIndex = 7;
            this.labelName.Tag = "Nombre";
            this.labelName.Text = "Nombre";
            // 
            // CrearPatenteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 216);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.comboBoxAccessType);
            this.Controls.Add(this.labelAccessType);
            this.Controls.Add(this.textBoxDataKey);
            this.Controls.Add(this.labelDataKey);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelName);
            this.Name = "CrearPatenteForm";
            this.Text = "CrearPatenteForm";
            this.Load += new System.EventHandler(this.CrearPatenteForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ComboBox comboBoxAccessType;
        private System.Windows.Forms.Label labelAccessType;
        private System.Windows.Forms.TextBox textBoxDataKey;
        private System.Windows.Forms.Label labelDataKey;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelName;
    }
}