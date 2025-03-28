namespace RentalEquipmentManagementApp
{
    partial class MainDashboardForm
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
            lblWelcome = new Label();
            btnLogout = new Button();
            groupBox1 = new GroupBox();
            button5 = new Button();
            button4 = new Button();
            button3 = new Button();
            button2 = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblWelcome.Location = new Point(429, 19);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(59, 25);
            lblWelcome.TabIndex = 0;
            lblWelcome.Text = "label1";
            // 
            // btnLogout
            // 
            btnLogout.BackColor = SystemColors.Control;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI Light", 10F, FontStyle.Underline);
            btnLogout.ForeColor = Color.FromArgb(1, 90, 132);
            btnLogout.Location = new Point(57, 376);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(111, 45);
            btnLogout.TabIndex = 1;
            btnLogout.Text = "Log out";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += button1_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button5);
            groupBox1.Controls.Add(btnLogout);
            groupBox1.Controls.Add(button4);
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(button2);
            groupBox1.Location = new Point(12, 46);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(236, 443);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // button5
            // 
            button5.Location = new Point(16, 292);
            button5.Name = "button5";
            button5.Size = new Size(200, 62);
            button5.TabIndex = 3;
            button5.Text = "Equipment Information Management";
            button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(16, 212);
            button4.Name = "button4";
            button4.Size = new Size(200, 62);
            button4.TabIndex = 2;
            button4.Text = "Equipment Check-in (Return Record)";
            button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(16, 132);
            button3.Name = "button3";
            button3.Size = new Size(200, 62);
            button3.TabIndex = 1;
            button3.Text = "Equipment Checkout (Rental Transaction)";
            button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(16, 52);
            button2.Name = "button2";
            button2.Size = new Size(200, 62);
            button2.TabIndex = 0;
            button2.Text = "Rental Requests Management";
            button2.UseVisualStyleBackColor = true;
            // 
            // MainDashboardForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(903, 518);
            Controls.Add(groupBox1);
            Controls.Add(lblWelcome);
            Name = "MainDashboardForm";
            Text = "MainDashboardForm";
            Load += MainDashboardForm_Load;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblWelcome;
        private Button btnLogout;
        private GroupBox groupBox1;
        private Button button3;
        private Button button2;
        private Button button5;
        private Button button4;
    }
}