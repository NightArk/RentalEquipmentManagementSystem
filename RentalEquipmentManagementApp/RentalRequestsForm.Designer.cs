namespace RentalEquipmentManagementApp
{
    partial class RentalRequestsForm
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
            groupBox1 = new GroupBox();
            btnResetFilter = new Button();
            btnFilter = new Button();
            ddFilterStatus = new ComboBox();
            label2 = new Label();
            txtFilterID = new TextBox();
            label1 = new Label();
            btnClose = new Button();
            dgvRequests = new DataGridView();
            btnUpdate = new Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRequests).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnResetFilter);
            groupBox1.Controls.Add(btnFilter);
            groupBox1.Controls.Add(ddFilterStatus);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtFilterID);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(27, 17);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(839, 81);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // btnResetFilter
            // 
            btnResetFilter.Location = new Point(690, 29);
            btnResetFilter.Name = "btnResetFilter";
            btnResetFilter.Size = new Size(123, 31);
            btnResetFilter.TabIndex = 5;
            btnResetFilter.Text = "ResetFilter";
            btnResetFilter.UseVisualStyleBackColor = true;
            btnResetFilter.Click += btnResetFilter_Click;
            // 
            // btnFilter
            // 
            btnFilter.Location = new Point(598, 29);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(86, 31);
            btnFilter.TabIndex = 4;
            btnFilter.Text = "Filter";
            btnFilter.UseVisualStyleBackColor = true;
            btnFilter.Click += btnFilter_Click;
            // 
            // ddFilterStatus
            // 
            ddFilterStatus.FormattingEnabled = true;
            ddFilterStatus.Location = new Point(377, 29);
            ddFilterStatus.Name = "ddFilterStatus";
            ddFilterStatus.Size = new Size(201, 28);
            ddFilterStatus.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(248, 33);
            label2.Name = "label2";
            label2.Size = new Size(132, 20);
            label2.TabIndex = 2;
            label2.Text = "Filter by Customer:";
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
            label1.Location = new Point(30, 35);
            label1.Name = "label1";
            label1.Size = new Size(84, 20);
            label1.TabIndex = 0;
            label1.Text = "Filter by ID:";
            // 
            // btnClose
            // 
            btnClose.Location = new Point(763, 625);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(103, 56);
            btnClose.TabIndex = 1;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // dgvRequests
            // 
            dgvRequests.BackgroundColor = Color.White;
            dgvRequests.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRequests.Location = new Point(27, 107);
            dgvRequests.Margin = new Padding(3, 4, 3, 4);
            dgvRequests.Name = "dgvRequests";
            dgvRequests.RowHeadersWidth = 51;
            dgvRequests.Size = new Size(1069, 501);
            dgvRequests.TabIndex = 2;
            dgvRequests.CellClick += dgvRequests_CellClick;
            dgvRequests.CellEndEdit += dgvRequests_CellEndEdit;
            dgvRequests.DataBindingComplete += dgvRequests_DataBindingComplete;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(27, 625);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(177, 56);
            btnUpdate.TabIndex = 3;
            btnUpdate.Text = "Update";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click_1;
            // 
            // RentalRequestsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1108, 703);
            Controls.Add(btnUpdate);
            Controls.Add(dgvRequests);
            Controls.Add(btnClose);
            Controls.Add(groupBox1);
            Name = "RentalRequestsForm";
            Text = "RentalRequestsForm";
            Load += RentalRequestsForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRequests).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Button btnClose;
        private ComboBox ddFilterStatus;
        private Label label2;
        private TextBox txtFilterID;
        private Label label1;
        private DataGridView dgvRequests;
        private Button btnUpdate;
        private Button btnResetFilter;
        private Button btnFilter;
    }
}