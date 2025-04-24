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
            btnEquipmentManagement = new Button();
            btnReturnRecord = new Button();
            btnRentalTransaction = new Button();
            btnRentalRequests = new Button();
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
            btnLogout.Click += btnLogout_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnEquipmentManagement);
            groupBox1.Controls.Add(btnLogout);
            groupBox1.Controls.Add(btnReturnRecord);
            groupBox1.Controls.Add(btnRentalTransaction);
            groupBox1.Controls.Add(btnRentalRequests);
            groupBox1.Location = new Point(12, 46);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(236, 443);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // btnEquipmentManagement
            // 
            btnEquipmentManagement.Location = new Point(16, 292);
            btnEquipmentManagement.Name = "btnEquipmentManagement";
            btnEquipmentManagement.Size = new Size(200, 62);
            btnEquipmentManagement.TabIndex = 3;
            btnEquipmentManagement.Text = "Equipment Information Management";
            btnEquipmentManagement.UseVisualStyleBackColor = true;
            btnEquipmentManagement.Click += btnEquipmentManagement_Click;
            // 
            // btnReturnRecord
            // 
            btnReturnRecord.Location = new Point(16, 212);
            btnReturnRecord.Name = "btnReturnRecord";
            btnReturnRecord.Size = new Size(200, 62);
            btnReturnRecord.TabIndex = 2;
            btnReturnRecord.Text = "Equipment Check-in (Return Record)";
            btnReturnRecord.UseVisualStyleBackColor = true;
            btnReturnRecord.Click += btnReturnRecord_Click;
            // 
            // btnRentalTransaction
            // 
            btnRentalTransaction.Location = new Point(16, 132);
            btnRentalTransaction.Name = "btnRentalTransaction";
            btnRentalTransaction.Size = new Size(200, 62);
            btnRentalTransaction.TabIndex = 1;
            btnRentalTransaction.Text = "Equipment Checkout (Rental Transaction)";
            btnRentalTransaction.UseVisualStyleBackColor = true;
            btnRentalTransaction.Click += btnRentalTransaction_Click;
            // 
            // btnRentalRequests
            // 
            btnRentalRequests.Location = new Point(16, 52);
            btnRentalRequests.Name = "btnRentalRequests";
            btnRentalRequests.Size = new Size(200, 62);
            btnRentalRequests.TabIndex = 0;
            btnRentalRequests.Text = "Rental Requests Management";
            btnRentalRequests.UseVisualStyleBackColor = true;
            btnRentalRequests.Click += btnRentalRequests_Click;
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
        private Button btnRentalTransaction;
        private Button btnRentalRequests;
        private Button btnEquipmentManagement;
        private Button btnReturnRecord;
    }
}