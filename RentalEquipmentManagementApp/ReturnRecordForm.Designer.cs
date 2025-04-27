namespace RentalEquipmentManagementApp
{
    partial class ReturnRecordForm
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
            dgvReturnRecord = new DataGridView();
            btnUpdate = new Button();
            btnClose = new Button();
            groupBox1 = new GroupBox();
            chkEnableDateFilter = new CheckBox();
            dtpFilterTo = new DateTimePicker();
            dtpFilterFrom = new DateTimePicker();
            cbFilterCondition = new ComboBox();
            cbFilterEquipment = new ComboBox();
            cbFilterCustomer = new ComboBox();
            label3 = new Label();
            label4 = new Label();
            btnResetFilter = new Button();
            btnFilter = new Button();
            label2 = new Label();
            txtFilterID = new TextBox();
            label1 = new Label();
            btnAdd = new Button();
            btnDelete = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvReturnRecord).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvReturnRecord
            // 
            dgvReturnRecord.BackgroundColor = Color.White;
            dgvReturnRecord.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReturnRecord.Location = new Point(27, 107);
            dgvReturnRecord.Margin = new Padding(3, 4, 3, 4);
            dgvReturnRecord.Name = "dgvReturnRecord";
            dgvReturnRecord.RowHeadersWidth = 51;
            dgvReturnRecord.Size = new Size(1069, 501);
            dgvReturnRecord.TabIndex = 0;
            dgvReturnRecord.CellClick += dgvReturnRecord_CellClick;
            dgvReturnRecord.DataBindingComplete += dgvReturnRecord_DataBindingComplete;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(27, 625);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(177, 56);
            btnUpdate.TabIndex = 4;
            btnUpdate.Text = "Update";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(993, 625);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(103, 56);
            btnClose.TabIndex = 5;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(chkEnableDateFilter);
            groupBox1.Controls.Add(dtpFilterTo);
            groupBox1.Controls.Add(dtpFilterFrom);
            groupBox1.Controls.Add(cbFilterCondition);
            groupBox1.Controls.Add(cbFilterEquipment);
            groupBox1.Controls.Add(cbFilterCustomer);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(btnResetFilter);
            groupBox1.Controls.Add(btnFilter);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtFilterID);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(27, 17);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1069, 81);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // chkEnableDateFilter
            // 
            chkEnableDateFilter.AutoSize = true;
            chkEnableDateFilter.Location = new Point(619, 21);
            chkEnableDateFilter.Name = "chkEnableDateFilter";
            chkEnableDateFilter.Size = new Size(179, 24);
            chkEnableDateFilter.TabIndex = 16;
            chkEnableDateFilter.Text = "Return Date (from/to):";
            chkEnableDateFilter.UseVisualStyleBackColor = true;
            // 
            // dtpFilterTo
            // 
            dtpFilterTo.Location = new Point(762, 50);
            dtpFilterTo.Name = "dtpFilterTo";
            dtpFilterTo.Size = new Size(137, 27);
            dtpFilterTo.TabIndex = 15;
            // 
            // dtpFilterFrom
            // 
            dtpFilterFrom.Location = new Point(619, 50);
            dtpFilterFrom.Name = "dtpFilterFrom";
            dtpFilterFrom.Size = new Size(137, 27);
            dtpFilterFrom.TabIndex = 14;
            // 
            // cbFilterCondition
            // 
            cbFilterCondition.FormattingEnabled = true;
            cbFilterCondition.Location = new Point(462, 52);
            cbFilterCondition.Name = "cbFilterCondition";
            cbFilterCondition.Size = new Size(151, 28);
            cbFilterCondition.TabIndex = 11;
            // 
            // cbFilterEquipment
            // 
            cbFilterEquipment.FormattingEnabled = true;
            cbFilterEquipment.Location = new Point(462, 19);
            cbFilterEquipment.Name = "cbFilterEquipment";
            cbFilterEquipment.Size = new Size(151, 28);
            cbFilterEquipment.TabIndex = 10;
            // 
            // cbFilterCustomer
            // 
            cbFilterCustomer.FormattingEnabled = true;
            cbFilterCustomer.Location = new Point(147, 47);
            cbFilterCustomer.Name = "cbFilterCustomer";
            cbFilterCustomer.Size = new Size(151, 28);
            cbFilterCustomer.TabIndex = 9;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(311, 55);
            label3.Name = "label3";
            label3.Size = new Size(124, 20);
            label3.TabIndex = 8;
            label3.Text = "Return Condition:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(34, 55);
            label4.Name = "label4";
            label4.Size = new Size(75, 20);
            label4.TabIndex = 6;
            label4.Text = "Customer:";
            // 
            // btnResetFilter
            // 
            btnResetFilter.Location = new Point(940, 47);
            btnResetFilter.Name = "btnResetFilter";
            btnResetFilter.Size = new Size(123, 31);
            btnResetFilter.TabIndex = 5;
            btnResetFilter.Text = "ResetFilter";
            btnResetFilter.UseVisualStyleBackColor = true;
            btnResetFilter.Click += btnResetFilter_Click;
            // 
            // btnFilter
            // 
            btnFilter.Location = new Point(940, 15);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(123, 31);
            btnFilter.TabIndex = 4;
            btnFilter.Text = "Filter";
            btnFilter.UseVisualStyleBackColor = true;
            btnFilter.Click += btnFilter_Click_2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(311, 21);
            label2.Name = "label2";
            label2.Size = new Size(84, 20);
            label2.TabIndex = 2;
            label2.Text = "Equipment:";
            // 
            // txtFilterID
            // 
            txtFilterID.Location = new Point(147, 18);
            txtFilterID.Name = "txtFilterID";
            txtFilterID.Size = new Size(75, 27);
            txtFilterID.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(34, 22);
            label1.Name = "label1";
            label1.Size = new Size(84, 20);
            label1.TabIndex = 0;
            label1.Text = "Filter by ID:";
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(210, 625);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(177, 56);
            btnAdd.TabIndex = 7;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(393, 625);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(177, 56);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // ReturnRecordForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1108, 703);
            Controls.Add(btnDelete);
            Controls.Add(btnAdd);
            Controls.Add(groupBox1);
            Controls.Add(btnClose);
            Controls.Add(btnUpdate);
            Controls.Add(dgvReturnRecord);
            Name = "ReturnRecordForm";
            Text = "ReturnRecordForm";
            Load += ReturnRecordForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvReturnRecord).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvReturnRecord;
        private Button btnUpdate;
        private Button btnClose;
        private GroupBox groupBox1;
        private Button btnResetFilter;
        private Button btnFilter;
        private Label label2;
        private TextBox txtFilterID;
        private Label label1;
        private Button btnAdd;
        private Button btnDelete;
        private DateTimePicker dtpFilterTo;
        private DateTimePicker dtpFilterFrom;
        private ComboBox cbFilterCondition;
        private ComboBox cbFilterEquipment;
        private ComboBox cbFilterCustomer;
        private Label label3;
        private Label label4;
        private CheckBox chkEnableDateFilter;
    }
}