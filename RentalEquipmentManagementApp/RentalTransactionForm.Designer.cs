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
            chkEnableStartDateFilter = new CheckBox();
            dtpTo = new DateTimePicker();
            dtpFrom = new DateTimePicker();
            ddFilterStatus = new ComboBox();
            ddFilterCustomer = new ComboBox();
            label4 = new Label();
            label3 = new Label();
            btnFilter = new Button();
            ddFilterEquipment = new ComboBox();
            btnResetFilter = new Button();
            txtFilterID = new TextBox();
            label2 = new Label();
            label1 = new Label();
            dgvTransaction = new DataGridView();
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
            btnUpdate.Click += btnUpdate_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(chkEnableStartDateFilter);
            groupBox1.Controls.Add(dtpTo);
            groupBox1.Controls.Add(dtpFrom);
            groupBox1.Controls.Add(ddFilterStatus);
            groupBox1.Controls.Add(ddFilterCustomer);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(btnFilter);
            groupBox1.Controls.Add(ddFilterEquipment);
            groupBox1.Controls.Add(btnResetFilter);
            groupBox1.Controls.Add(txtFilterID);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(27, 17);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1069, 92);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // chkEnableStartDateFilter
            // 
            chkEnableStartDateFilter.AutoSize = true;
            chkEnableStartDateFilter.Location = new Point(646, 26);
            chkEnableStartDateFilter.Name = "chkEnableStartDateFilter";
            chkEnableStartDateFilter.Size = new Size(165, 24);
            chkEnableStartDateFilter.TabIndex = 13;
            chkEnableStartDateFilter.Text = "Start date (from/to):";
            chkEnableStartDateFilter.UseVisualStyleBackColor = true;
            // 
            // dtpTo
            // 
            dtpTo.Location = new Point(781, 56);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(129, 27);
            dtpTo.TabIndex = 12;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // dtpFrom
            // 
            dtpFrom.Location = new Point(646, 56);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(129, 27);
            dtpFrom.TabIndex = 11;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // ddFilterStatus
            // 
            ddFilterStatus.FormattingEnabled = true;
            ddFilterStatus.Location = new Point(478, 55);
            ddFilterStatus.Name = "ddFilterStatus";
            ddFilterStatus.Size = new Size(151, 28);
            ddFilterStatus.TabIndex = 10;
            ddFilterStatus.SelectedIndexChanged += ddFilterStatus_SelectedIndexChanged;
            // 
            // ddFilterCustomer
            // 
            ddFilterCustomer.FormattingEnabled = true;
            ddFilterCustomer.Location = new Point(478, 21);
            ddFilterCustomer.Name = "ddFilterCustomer";
            ddFilterCustomer.Size = new Size(151, 28);
            ddFilterCustomer.TabIndex = 9;
            ddFilterCustomer.SelectedIndexChanged += ddFilterCustomer_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(340, 55);
            label4.Name = "label4";
            label4.Size = new Size(110, 20);
            label4.TabIndex = 7;
            label4.Text = "Payment status:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 50);
            label3.Name = "label3";
            label3.Size = new Size(125, 20);
            label3.TabIndex = 6;
            label3.Text = "Equipment name:";
            // 
            // btnFilter
            // 
            btnFilter.Location = new Point(940, 18);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(86, 31);
            btnFilter.TabIndex = 5;
            btnFilter.Text = "Filter";
            btnFilter.UseVisualStyleBackColor = true;
            btnFilter.Click += btnFilter_Click;
            // 
            // ddFilterEquipment
            // 
            ddFilterEquipment.FormattingEnabled = true;
            ddFilterEquipment.Location = new Point(183, 52);
            ddFilterEquipment.Name = "ddFilterEquipment";
            ddFilterEquipment.Size = new Size(151, 28);
            ddFilterEquipment.TabIndex = 3;
            ddFilterEquipment.SelectedIndexChanged += ddFilterEquipment_SelectedIndexChanged;
            // 
            // btnResetFilter
            // 
            btnResetFilter.Location = new Point(940, 55);
            btnResetFilter.Name = "btnResetFilter";
            btnResetFilter.Size = new Size(123, 31);
            btnResetFilter.TabIndex = 4;
            btnResetFilter.Text = "Reset Filter";
            btnResetFilter.UseVisualStyleBackColor = true;
            btnResetFilter.Click += btnResetFilter_Click;
            // 
            // txtFilterID
            // 
            txtFilterID.Location = new Point(183, 17);
            txtFilterID.Name = "txtFilterID";
            txtFilterID.Size = new Size(75, 27);
            txtFilterID.TabIndex = 2;
            txtFilterID.TextChanged += txtFilterID_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(340, 24);
            label2.Name = "label2";
            label2.Size = new Size(132, 20);
            label2.TabIndex = 1;
            label2.Text = "Filter by Customer:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 24);
            label1.Name = "label1";
            label1.Size = new Size(84, 20);
            label1.TabIndex = 0;
            label1.Text = "Filter by ID:";
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
            dgvTransaction.CellContentClick += dgvTransaction_CellContentClick;
            dgvTransaction.DataBindingComplete += dgvTransaction_DataBindingComplete;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(393, 625);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(177, 56);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(210, 625);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(177, 56);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
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
        private ComboBox ddFilterEquipment;
        private Button btnResetFilter;
        private Button btnFilter;
        private Button btnDelete;
        private Button btnAdd;
        private DateTimePicker dtpTo;
        private DateTimePicker dtpFrom;
        private ComboBox ddFilterStatus;
        private ComboBox ddFilterCustomer;
        private Label label4;
        private Label label3;
        private CheckBox chkEnableStartDateFilter;
    }
}