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
            btnResetFilter = new Button();
            btnFilter = new Button();
            ddFilterEquipment = new ComboBox();
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
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(27, 625);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(177, 56);
            btnUpdate.TabIndex = 4;
            btnUpdate.Text = "Update";
            btnUpdate.UseVisualStyleBackColor = true;
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
            groupBox1.Controls.Add(btnResetFilter);
            groupBox1.Controls.Add(btnFilter);
            groupBox1.Controls.Add(ddFilterEquipment);
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
            // btnResetFilter
            // 
            btnResetFilter.Location = new Point(736, 27);
            btnResetFilter.Name = "btnResetFilter";
            btnResetFilter.Size = new Size(123, 31);
            btnResetFilter.TabIndex = 5;
            btnResetFilter.Text = "ResetFilter";
            btnResetFilter.UseVisualStyleBackColor = true;
            // 
            // btnFilter
            // 
            btnFilter.Location = new Point(644, 27);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(86, 31);
            btnFilter.TabIndex = 4;
            btnFilter.Text = "Filter";
            btnFilter.UseVisualStyleBackColor = true;
            // 
            // ddFilterEquipment
            // 
            ddFilterEquipment.FormattingEnabled = true;
            ddFilterEquipment.Location = new Point(392, 29);
            ddFilterEquipment.Name = "ddFilterEquipment";
            ddFilterEquipment.Size = new Size(201, 28);
            ddFilterEquipment.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(241, 31);
            label2.Name = "label2";
            label2.Size = new Size(145, 20);
            label2.TabIndex = 2;
            label2.Text = "Filter by Equipment :";
            // 
            // txtFilterID
            // 
            txtFilterID.Location = new Point(147, 29);
            txtFilterID.Name = "txtFilterID";
            txtFilterID.Size = new Size(75, 27);
            txtFilterID.TabIndex = 1;
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
            // btnAdd
            // 
            btnAdd.Location = new Point(210, 625);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(177, 56);
            btnAdd.TabIndex = 7;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(393, 625);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(177, 56);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
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
        private ComboBox ddFilterEquipment;
        private Label label2;
        private TextBox txtFilterID;
        private Label label1;
        private Button btnAdd;
        private Button btnDelete;
    }
}