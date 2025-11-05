namespace Distribuidora_los_amigos.Forms.RecuperarPassword
{
    partial class RecuperarPasswordForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxUsuario = new System.Windows.Forms.TextBox();
            this.btnEnviarToken = new System.Windows.Forms.Button();
            this.panelToken = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxToken = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxNuevaPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxConfirmarPassword = new System.Windows.Forms.TextBox();
            this.btnCambiarPassword = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelToken.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(195)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(450, 60);
            this.panel1.TabIndex = 11;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(50, 50);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(175, 75);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 80);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(135, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(180, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Recuperar Contraseña";
            this.label3.Tag = "Recuperar_Contraseña";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(30, 170);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 16);
            this.label1.TabIndex = 18;
            this.label1.Text = "Nombre de usuario:";
            this.label1.Tag = "Nombre_de_usuario";
            // 
            // textBoxUsuario
            // 
            this.textBoxUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.textBoxUsuario.Location = new System.Drawing.Point(33, 195);
            this.textBoxUsuario.Name = "textBoxUsuario";
            this.textBoxUsuario.Size = new System.Drawing.Size(380, 23);
            this.textBoxUsuario.TabIndex = 19;
            // 
            // btnEnviarToken
            // 
            this.btnEnviarToken.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(195)))), ((int)(((byte)(192)))));
            this.btnEnviarToken.FlatAppearance.BorderSize = 0;
            this.btnEnviarToken.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnviarToken.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnEnviarToken.ForeColor = System.Drawing.Color.White;
            this.btnEnviarToken.Location = new System.Drawing.Point(33, 235);
            this.btnEnviarToken.Name = "btnEnviarToken";
            this.btnEnviarToken.Size = new System.Drawing.Size(380, 35);
            this.btnEnviarToken.TabIndex = 20;
            this.btnEnviarToken.Text = "Enviar código de recuperación";
            this.btnEnviarToken.Tag = "Enviar_código_de_recuperación";
            this.btnEnviarToken.UseVisualStyleBackColor = false;
            this.btnEnviarToken.Click += new System.EventHandler(this.btnEnviarToken_Click);
            // 
            // panelToken
            // 
            this.panelToken.Controls.Add(this.label4);
            this.panelToken.Controls.Add(this.textBoxToken);
            this.panelToken.Controls.Add(this.label2);
            this.panelToken.Controls.Add(this.textBoxNuevaPassword);
            this.panelToken.Controls.Add(this.label5);
            this.panelToken.Controls.Add(this.textBoxConfirmarPassword);
            this.panelToken.Controls.Add(this.btnCambiarPassword);
            this.panelToken.Location = new System.Drawing.Point(33, 310);
            this.panelToken.Name = "panelToken";
            this.panelToken.Size = new System.Drawing.Size(380, 200);
            this.panelToken.TabIndex = 21;
            this.panelToken.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label4.Location = new System.Drawing.Point(3, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 16);
            this.label4.TabIndex = 22;
            this.label4.Text = "Código de recuperación:";
            this.label4.Tag = "Código_de_recuperación";
            // 
            // textBoxToken
            // 
            this.textBoxToken.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.textBoxToken.Location = new System.Drawing.Point(3, 30);
            this.textBoxToken.MaxLength = 6;
            this.textBoxToken.Name = "textBoxToken";
            this.textBoxToken.Size = new System.Drawing.Size(374, 23);
            this.textBoxToken.TabIndex = 23;
            this.textBoxToken.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label2.Location = new System.Drawing.Point(3, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 16);
            this.label2.TabIndex = 24;
            this.label2.Text = "Nueva contraseña:";
            this.label2.Tag = "Nueva_contraseña";
            // 
            // textBoxNuevaPassword
            // 
            this.textBoxNuevaPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.textBoxNuevaPassword.Location = new System.Drawing.Point(3, 85);
            this.textBoxNuevaPassword.Name = "textBoxNuevaPassword";
            this.textBoxNuevaPassword.Size = new System.Drawing.Size(374, 23);
            this.textBoxNuevaPassword.TabIndex = 25;
            this.textBoxNuevaPassword.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label5.Location = new System.Drawing.Point(3, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(138, 16);
            this.label5.TabIndex = 26;
            this.label5.Text = "Confirmar contraseña:";
            this.label5.Tag = "Confirmar_contraseña";
            // 
            // textBoxConfirmarPassword
            // 
            this.textBoxConfirmarPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.textBoxConfirmarPassword.Location = new System.Drawing.Point(3, 140);
            this.textBoxConfirmarPassword.Name = "textBoxConfirmarPassword";
            this.textBoxConfirmarPassword.Size = new System.Drawing.Size(374, 23);
            this.textBoxConfirmarPassword.TabIndex = 27;
            this.textBoxConfirmarPassword.UseSystemPasswordChar = true;
            // 
            // btnCambiarPassword
            // 
            this.btnCambiarPassword.BackColor = System.Drawing.Color.Green;
            this.btnCambiarPassword.FlatAppearance.BorderSize = 0;
            this.btnCambiarPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCambiarPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnCambiarPassword.ForeColor = System.Drawing.Color.White;
            this.btnCambiarPassword.Location = new System.Drawing.Point(3, 170);
            this.btnCambiarPassword.Name = "btnCambiarPassword";
            this.btnCambiarPassword.Size = new System.Drawing.Size(374, 35);
            this.btnCambiarPassword.TabIndex = 28;
            this.btnCambiarPassword.Text = "Cambiar contraseña";
            this.btnCambiarPassword.Tag = "Cambiar_contraseña";
            this.btnCambiarPassword.UseVisualStyleBackColor = false;
            this.btnCambiarPassword.Click += new System.EventHandler(this.btnCambiarPassword_Click);
            // 
            // btnVolver
            // 
            this.btnVolver.BackColor = System.Drawing.Color.Gray;
            this.btnVolver.FlatAppearance.BorderSize = 0;
            this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolver.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnVolver.ForeColor = System.Drawing.Color.White;
            this.btnVolver.Location = new System.Drawing.Point(33, 530);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(380, 35);
            this.btnVolver.TabIndex = 22;
            this.btnVolver.Text = "Volver al Login";
            this.btnVolver.Tag = "Volver_al_Login";
            this.btnVolver.UseVisualStyleBackColor = false;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            // 
            // lblMensaje
            // 
            this.lblMensaje.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblMensaje.ForeColor = System.Drawing.Color.Green;
            this.lblMensaje.Location = new System.Drawing.Point(33, 280);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(380, 20);
            this.lblMensaje.TabIndex = 23;
            this.lblMensaje.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMensaje.Visible = false;
            // 
            // RecuperarPasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(450, 580);
            this.Controls.Add(this.lblMensaje);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.panelToken);
            this.Controls.Add(this.btnEnviarToken);
            this.Controls.Add(this.textBoxUsuario);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RecuperarPasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "RecuperarPassword";
            this.Text = "Recuperar Contraseña";
            this.Load += new System.EventHandler(this.RecuperarPasswordForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelToken.ResumeLayout(false);
            this.panelToken.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxUsuario;
        private System.Windows.Forms.Button btnEnviarToken;
        private System.Windows.Forms.Panel panelToken;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxToken;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxNuevaPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxConfirmarPassword;
        private System.Windows.Forms.Button btnCambiarPassword;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.Label lblMensaje;
    }
}