namespace RentalEquipmentManagementApp
{
    partial class RentalTransactionAdd
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            cbCustomer = new ComboBox();
            cbEquipment = new ComboBox();
            cbPaymentStatus = new ComboBox();
            dtpStartDate = new DateTimePicker();
            dtpReturnDate = new DateTimePicker();
            txtRentalPeriod = new TextBox();
            txtRentalFee = new TextBox();
            txtDeposit = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            groupBox1 = new GroupBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 77);
            label1.Name = "label1";
            label1.Size = new Size(116, 20);
            label1.TabIndex = 0;
            label1.Text = "Customer name:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 113);
            label2.Name = "label2";
            label2.Size = new Size(84, 20);
            label2.TabIndex = 1;
            label2.Text = "Equipment:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 168);
            label3.Name = "label3";
            label3.Size = new Size(125, 20);
            label3.TabIndex = 2;
            label3.Text = "Rental Start Date:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 233);
            label4.Name = "label4";
            label4.Size = new Size(91, 20);
            label4.TabIndex = 3;
            label4.Text = "Return Date:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(165, 204);
            label5.Name = "label5";
            label5.Size = new Size(100, 20);
            label5.TabIndex = 4;
            label5.Text = "Rental Period:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 309);
            label6.Name = "label6";
            label6.Size = new Size(81, 20);
            label6.TabIndex = 5;
            label6.Text = "Rental Fee:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(6, 381);
            label7.Name = "label7";
            label7.Size = new Size(64, 20);
            label7.TabIndex = 6;
            label7.Text = "Deposit:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(0, 449);
            label8.Name = "label8";
            label8.Size = new Size(112, 20);
            label8.TabIndex = 7;
            label8.Text = "Payment Status:";
            // 
            // cbCustomer
            // 
            cbCustomer.FormattingEnabled = true;
            cbCustomer.Location = new Point(128, 74);
            cbCustomer.Name = "cbCustomer";
            cbCustomer.Size = new Size(116, 28);
            cbCustomer.TabIndex = 8;
            // 
            // cbEquipment
            // 
            cbEquipment.FormattingEnabled = true;
            cbEquipment.Location = new Point(128, 113);
            cbEquipment.Name = "cbEquipment";
            cbEquipment.Size = new Size(116, 28);
            cbEquipment.TabIndex = 9;
            // 
            // cbPaymentStatus
            // 
            cbPaymentStatus.FormattingEnabled = true;
            cbPaymentStatus.Location = new Point(0, 472);
            cbPaymentStatus.Name = "cbPaymentStatus";
            cbPaymentStatus.Size = new Size(116, 28);
            cbPaymentStatus.TabIndex = 10;
            // 
            // dtpStartDate
            // 
            dtpStartDate.Location = new Point(6, 197);
            dtpStartDate.Name = "dtpStartDate";
            dtpStartDate.Size = new Size(125, 27);
            dtpStartDate.TabIndex = 11;
            // 
            // dtpReturnDate
            // 
            dtpReturnDate.Location = new Point(6, 253);
            dtpReturnDate.Name = "dtpReturnDate";
            dtpReturnDate.Size = new Size(125, 27);
            dtpReturnDate.TabIndex = 12;
            // 
            // txtRentalPeriod
            // 
            txtRentalPeriod.Location = new Point(165, 227);
            txtRentalPeriod.Name = "txtRentalPeriod";
            txtRentalPeriod.ReadOnly = true;
            txtRentalPeriod.Size = new Size(125, 27);
            txtRentalPeriod.TabIndex = 13;
            // 
            // txtRentalFee
            // 
            txtRentalFee.Location = new Point(6, 332);
            txtRentalFee.Name = "txtRentalFee";
            txtRentalFee.Size = new Size(125, 27);
            txtRentalFee.TabIndex = 14;
            // 
            // txtDeposit
            // 
            txtDeposit.Location = new Point(6, 404);
            txtDeposit.Name = "txtDeposit";
            txtDeposit.Size = new Size(125, 27);
            txtDeposit.TabIndex = 15;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(49, 523);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(195, 43);
            btnSave.TabIndex = 16;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(49, 572);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(195, 43);
            btnCancel.TabIndex = 17;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.White;
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(btnCancel);
            groupBox1.Controls.Add(cbCustomer);
            groupBox1.Controls.Add(btnSave);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(cbPaymentStatus);
            groupBox1.Controls.Add(txtDeposit);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(cbEquipment);
            groupBox1.Controls.Add(txtRentalFee);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(txtRentalPeriod);
            groupBox1.Controls.Add(dtpStartDate);
            groupBox1.Controls.Add(dtpReturnDate);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label5);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(311, 625);
            groupBox1.TabIndex = 18;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // RentalTransactionAdd
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(348, 673);
            Controls.Add(groupBox1);
            Name = "RentalTransactionAdd";
            Text = "RentalTransactionAdd";
            Load += RentalTransactionAdd_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private ComboBox cbCustomer;
        private ComboBox cbEquipment;
        private ComboBox cbPaymentStatus;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpReturnDate;
        private TextBox txtRentalPeriod;
        private TextBox txtRentalFee;
        private TextBox txtDeposit;
        private Button btnSave;
        private Button btnCancel;
        private GroupBox groupBox1;
    }
}