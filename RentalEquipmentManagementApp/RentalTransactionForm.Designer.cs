namespace RentalEquipmentManagementApp
{
    partial class RentalTransactionForm
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
            btnClose = new Button();
            btnUpdate = new Button();
            groupBox1 = new GroupBox();
            dgvTransaction = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            txtFilterID = new TextBox();
            ddFilterStatus = new ComboBox();
            btnResetFilter = new Button();
            btnFilter = new Button();
            btnDelete = new Button();
            btnAdd = new Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTransaction).BeginInit();
            SuspendLayout();
            // 
            // btnClose
            // 
            btnClose.Location = new Point(993, 625);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(103, 56);
            btnClose.TabIndex = 0;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += button1_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(27, 625);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(177, 56);
            btnUpdate.TabIndex = 1;
            btnUpdate.Text = "Update";
            btnUpdate.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnFilter);
            groupBox1.Controls.Add(ddFilterStatus);
            groupBox1.Controls.Add(btnResetFilter);
            groupBox1.Controls.Add(txtFilterID);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(27, 17);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(839, 81);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // dgvTransaction
            // 
            dgvTransaction.BackgroundColor = Color.White;
            dgvTransaction.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTransaction.Location = new Point(27, 107);
            dgvTransaction.Margin = new Padding(3, 4, 3, 4);
            dgvTransaction.Name = "dgvTransaction";
            dgvTransaction.RowHeadersWidth = 51;
            dgvTransaction.Size = new Size(1069, 501);
            dgvTransaction.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(34, 33);
            label1.Name = "label1";
            label1.Size = new Size(84, 20);
            label1.TabIndex = 0;
            label1.Text = "Filter by ID:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(241, 31);
            label2.Name = "label2";
            label2.Size = new Size(132, 20);
            label2.TabIndex = 1;
            label2.Text = "Filter by Customer:";
            // 
            // txtFilterID
            // 
            txtFilterID.Location = new Point(147, 29);
            txtFilterID.Name = "txtFilterID";
            txtFilterID.Size = new Size(75, 27);
            txtFilterID.TabIndex = 2;
            // 
            // ddFilterStatus
            // 
            ddFilterStatus.FormattingEnabled = true;
            ddFilterStatus.Location = new Point(377, 29);
            ddFilterStatus.Name = "ddFilterStatus";
            ddFilterStatus.Size = new Size(201, 28);
            ddFilterStatus.TabIndex = 3;
            // 
            // btnResetFilter
            // 
            btnResetFilter.Location = new Point(690, 27);
            btnResetFilter.Name = "btnResetFilter";
            btnResetFilter.Size = new Size(123, 31);
            btnResetFilter.TabIndex = 4;
            btnResetFilter.Text = "Reset Filter";
            btnResetFilter.UseVisualStyleBackColor = true;
            // 
            // btnFilter
            // 
            btnFilter.Location = new Point(598, 27);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(86, 31);
            btnFilter.TabIndex = 5;
            btnFilter.Text = "Filter";
            btnFilter.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(393, 625);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(177, 56);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(210, 625);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(177, 56);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            // 
            // RentalTransactionForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1108, 703);
            Controls.Add(btnAdd);
            Controls.Add(btnDelete);
            Controls.Add(dgvTransaction);
            Controls.Add(groupBox1);
            Controls.Add(btnUpdate);
            Controls.Add(btnClose);
            Name = "RentalTransactionForm";
            Text = "RentalTransactionForm";
            Load += RentalTransactionForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTransaction).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnClose;
        private Button btnUpdate;
        private GroupBox groupBox1;
        private DataGridView dgvTransaction;
        private Label label2;
        private Label label1;
        private TextBox txtFilterID;
        private ComboBox ddFilterStatus;
        private Button btnResetFilter;
        private Button btnFilter;
        private Button btnDelete;
        private Button btnAdd;
    }
}