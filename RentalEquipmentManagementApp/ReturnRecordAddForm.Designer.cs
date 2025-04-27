namespace RentalEquipmentManagementApp
{
    partial class ReturnRecordAddForm
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
            txtLateFee = new TextBox();
            cbRentalTransaction = new ComboBox();
            dtpReturnDate = new DateTimePicker();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            btnAdd = new Button();
            btnCancel = new Button();
            cbReturnCondition = new ComboBox();
            txtAdditionalCharges = new TextBox();
            txtEquipmentName = new TextBox();
            label6 = new Label();
            label7 = new Label();
            txtCustomerName = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(74, 132);
            label1.Name = "label1";
            label1.Size = new Size(133, 20);
            label1.TabIndex = 0;
            label1.Text = "Rental Transaction:";
            // 
            // txtLateFee
            // 
            txtLateFee.Location = new Point(74, 260);
            txtLateFee.Name = "txtLateFee";
            txtLateFee.Size = new Size(125, 27);
            txtLateFee.TabIndex = 1;
            // 
            // cbRentalTransaction
            // 
            cbRentalTransaction.FormattingEnabled = true;
            cbRentalTransaction.Location = new Point(74, 155);
            cbRentalTransaction.Name = "cbRentalTransaction";
            cbRentalTransaction.Size = new Size(151, 28);
            cbRentalTransaction.TabIndex = 2;
            // 
            // dtpReturnDate
            // 
            dtpReturnDate.Location = new Point(581, 155);
            dtpReturnDate.Name = "dtpReturnDate";
            dtpReturnDate.Size = new Size(151, 27);
            dtpReturnDate.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(581, 132);
            label2.Name = "label2";
            label2.Size = new Size(137, 20);
            label2.TabIndex = 4;
            label2.Text = "Actual Return Date:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(334, 132);
            label3.Name = "label3";
            label3.Size = new Size(124, 20);
            label3.TabIndex = 5;
            label3.Text = "Return Condition:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(334, 237);
            label4.Name = "label4";
            label4.Size = new Size(139, 20);
            label4.TabIndex = 7;
            label4.Text = "Additional Charges:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(74, 237);
            label5.Name = "label5";
            label5.Size = new Size(114, 20);
            label5.TabIndex = 6;
            label5.Text = "Late Return Fee:";
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(624, 228);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(94, 29);
            btnAdd.TabIndex = 8;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(624, 263);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // cbReturnCondition
            // 
            cbReturnCondition.FormattingEnabled = true;
            cbReturnCondition.Location = new Point(334, 154);
            cbReturnCondition.Name = "cbReturnCondition";
            cbReturnCondition.Size = new Size(151, 28);
            cbReturnCondition.TabIndex = 10;
            // 
            // txtAdditionalCharges
            // 
            txtAdditionalCharges.Location = new Point(334, 260);
            txtAdditionalCharges.Name = "txtAdditionalCharges";
            txtAdditionalCharges.Size = new Size(125, 27);
            txtAdditionalCharges.TabIndex = 11;
            // 
            // txtEquipmentName
            // 
            txtEquipmentName.Location = new Point(334, 60);
            txtEquipmentName.Name = "txtEquipmentName";
            txtEquipmentName.ReadOnly = true;
            txtEquipmentName.Size = new Size(125, 27);
            txtEquipmentName.TabIndex = 15;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(334, 37);
            label6.Name = "label6";
            label6.Size = new Size(128, 20);
            label6.TabIndex = 14;
            label6.Text = "Equipment Name:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(74, 37);
            label7.Name = "label7";
            label7.Size = new Size(119, 20);
            label7.TabIndex = 13;
            label7.Text = "Customer Name:";
            // 
            // txtCustomerName
            // 
            txtCustomerName.Location = new Point(74, 60);
            txtCustomerName.Name = "txtCustomerName";
            txtCustomerName.ReadOnly = true;
            txtCustomerName.Size = new Size(125, 27);
            txtCustomerName.TabIndex = 12;
            // 
            // ReturnRecordAddForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtEquipmentName);
            Controls.Add(label6);
            Controls.Add(label7);
            Controls.Add(txtCustomerName);
            Controls.Add(txtAdditionalCharges);
            Controls.Add(cbReturnCondition);
            Controls.Add(btnCancel);
            Controls.Add(btnAdd);
            Controls.Add(label4);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(dtpReturnDate);
            Controls.Add(cbRentalTransaction);
            Controls.Add(txtLateFee);
            Controls.Add(label1);
            Name = "ReturnRecordAddForm";
            Text = "ReturnRecordAddForm";
            Load += ReturnRecordAddForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtLateFee;
        private ComboBox cbRentalTransaction;
        private DateTimePicker dtpReturnDate;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button btnAdd;
        private Button btnCancel;
        private ComboBox cbReturnCondition;
        private TextBox txtAdditionalCharges;
        private TextBox txtEquipmentName;
        private Label label6;
        private Label label7;
        private TextBox txtCustomerName;
    }
}