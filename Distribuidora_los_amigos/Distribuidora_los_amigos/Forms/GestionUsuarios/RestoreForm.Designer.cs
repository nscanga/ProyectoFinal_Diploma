namespace Distribuidora_los_amigos.Forms.GestionUsuarios
{
    partial class RestoreForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnRestore = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Tag = "RestaurarBD";
            this.label1.Text = "Restaurar B.D.";
            // 
            // btnRestore
            // 
            this.btnRestore.Location = new System.Drawing.Point(62, 51);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(75, 23);
            this.btnRestore.TabIndex = 1;
            this.btnRestore.Tag = "Restaurar";
            this.btnRestore.Text = "Restaurar";
            this.btnRestore.UseVisualStyleBackColor = true;
            // 
            // RestoreForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(215, 103);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.label1);
            this.Name = "RestoreForm";
            this.Tag = "Restaurar";
            this.Text = "Restaurar";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Label label1;
    }
}
