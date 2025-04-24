namespace RentalEquipmentManagementApp
{
    partial class LoginForm
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
            txtEmail = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            groupBox1 = new GroupBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(90, 90);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(215, 31);
            txtEmail.TabIndex = 0;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(90, 149);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(215, 31);
            txtPassword.TabIndex = 1;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = SystemColors.HotTrack;
            btnLogin.ForeColor = SystemColors.ControlLightLight;
            btnLogin.Location = new Point(90, 214);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(215, 57);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnLogin);
            groupBox1.Controls.Add(txtEmail);
            groupBox1.Controls.Add(txtPassword);
            groupBox1.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(199, 57);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(389, 314);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "User Login";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(groupBox1);
            Name = "LoginForm";
            Text = "LoginForm";
            Load += LoginForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnLogin;
        private GroupBox groupBox1;
    }
}