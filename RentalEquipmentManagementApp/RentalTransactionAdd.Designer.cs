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
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(106, 51);
            label1.Name = "label1";
            label1.Size = new Size(116, 20);
            label1.TabIndex = 0;
            label1.Text = "Customer name:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(106, 195);
            label2.Name = "label2";
            label2.Size = new Size(84, 20);
            label2.TabIndex = 1;
            label2.Text = "Equipment:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(342, 51);
            label3.Name = "label3";
            label3.Size = new Size(125, 20);
            label3.TabIndex = 2;
            label3.Text = "Rental Start Date:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(342, 195);
            label4.Name = "label4";
            label4.Size = new Size(91, 20);
            label4.TabIndex = 3;
            label4.Text = "Return Date:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(342, 351);
            label5.Name = "label5";
            label5.Size = new Size(100, 20);
            label5.TabIndex = 4;
            label5.Text = "Rental Period:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(568, 51);
            label6.Name = "label6";
            label6.Size = new Size(81, 20);
            label6.TabIndex = 5;
            label6.Text = "Rental Fee:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(568, 194);
            label7.Name = "label7";
            label7.Size = new Size(64, 20);
            label7.TabIndex = 6;
            label7.Text = "Deposit:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(106, 351);
            label8.Name = "label8";
            label8.Size = new Size(112, 20);
            label8.TabIndex = 7;
            label8.Text = "Payment Status:";
            // 
            // cbCustomer
            // 
            cbCustomer.FormattingEnabled = true;
            cbCustomer.Location = new Point(106, 74);
            cbCustomer.Name = "cbCustomer";
            cbCustomer.Size = new Size(116, 28);
            cbCustomer.TabIndex = 8;
            // 
            // cbEquipment
            // 
            cbEquipment.FormattingEnabled = true;
            cbEquipment.Location = new Point(106, 218);
            cbEquipment.Name = "cbEquipment";
            cbEquipment.Size = new Size(116, 28);
            cbEquipment.TabIndex = 9;
            // 
            // cbPaymentStatus
            // 
            cbPaymentStatus.FormattingEnabled = true;
            cbPaymentStatus.Location = new Point(106, 374);
            cbPaymentStatus.Name = "cbPaymentStatus";
            cbPaymentStatus.Size = new Size(116, 28);
            cbPaymentStatus.TabIndex = 10;
            // 
            // dtpStartDate
            // 
            dtpStartDate.Location = new Point(342, 72);
            dtpStartDate.Name = "dtpStartDate";
            dtpStartDate.Size = new Size(125, 27);
            dtpStartDate.TabIndex = 11;
            // 
            // dtpReturnDate
            // 
            dtpReturnDate.Location = new Point(342, 215);
            dtpReturnDate.Name = "dtpReturnDate";
            dtpReturnDate.Size = new Size(125, 27);
            dtpReturnDate.TabIndex = 12;
            // 
            // txtRentalPeriod
            // 
            txtRentalPeriod.Location = new Point(342, 374);
            txtRentalPeriod.Name = "txtRentalPeriod";
            txtRentalPeriod.ReadOnly = true;
            txtRentalPeriod.Size = new Size(125, 27);
            txtRentalPeriod.TabIndex = 13;
            // 
            // txtRentalFee
            // 
            txtRentalFee.Location = new Point(568, 74);
            txtRentalFee.Name = "txtRentalFee";
            txtRentalFee.Size = new Size(125, 27);
            txtRentalFee.TabIndex = 14;
            // 
            // txtDeposit
            // 
            txtDeposit.Location = new Point(568, 217);
            txtDeposit.Name = "txtDeposit";
            txtDeposit.Size = new Size(125, 27);
            txtDeposit.TabIndex = 15;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(568, 342);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(94, 29);
            btnSave.TabIndex = 16;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(568, 373);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 17;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // RentalTransactionAdd
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(txtDeposit);
            Controls.Add(txtRentalFee);
            Controls.Add(txtRentalPeriod);
            Controls.Add(dtpReturnDate);
            Controls.Add(dtpStartDate);
            Controls.Add(cbPaymentStatus);
            Controls.Add(cbEquipment);
            Controls.Add(cbCustomer);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "RentalTransactionAdd";
            Text = "RentalTransactionAdd";
            Load += RentalTransactionAdd_Load;
            ResumeLayout(false);
            PerformLayout();
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
    }
}