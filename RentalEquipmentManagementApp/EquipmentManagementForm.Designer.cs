namespace RentalEquipmentManagementApp
{
    partial class EquipmentManagementForm
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
            dgvTransaction = new DataGridView();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnClose = new Button();
            groupBox1 = new GroupBox();
            btnFilter = new Button();
            ddFilterStatus = new ComboBox();
            btnResetFilter = new Button();
            txtFilterID = new TextBox();
            label2 = new Label();
            label1 = new Label();
            ddFilterEquipment = new ComboBox();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvTransaction).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
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
            dgvTransaction.TabIndex = 4;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(27, 625);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(177, 56);
            btnAdd.TabIndex = 9;
            btnAdd.Text = "Add ( optional )";
            btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(210, 625);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(177, 56);
            btnUpdate.TabIndex = 7;
            btnUpdate.Text = "Update ( optional )";
            btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(986, 625);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(103, 56);
            btnClose.TabIndex = 6;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(ddFilterEquipment);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(btnFilter);
            groupBox1.Controls.Add(ddFilterStatus);
            groupBox1.Controls.Add(btnResetFilter);
            groupBox1.Controls.Add(txtFilterID);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(27, 17);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1062, 81);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // btnFilter
            // 
            btnFilter.Location = new Point(830, 28);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(86, 31);
            btnFilter.TabIndex = 5;
            btnFilter.Text = "Filter";
            btnFilter.UseVisualStyleBackColor = true;
            // 
            // ddFilterStatus
            // 
            ddFilterStatus.FormattingEnabled = true;
            ddFilterStatus.Location = new Point(377, 29);
            ddFilterStatus.Name = "ddFilterStatus";
            ddFilterStatus.Size = new Size(118, 28);
            ddFilterStatus.TabIndex = 3;
            // 
            // btnResetFilter
            // 
            btnResetFilter.Location = new Point(922, 28);
            btnResetFilter.Name = "btnResetFilter";
            btnResetFilter.Size = new Size(123, 31);
            btnResetFilter.TabIndex = 4;
            btnResetFilter.Text = "Reset Filter";
            btnResetFilter.UseVisualStyleBackColor = true;
            // 
            // txtFilterID
            // 
            txtFilterID.Location = new Point(147, 29);
            txtFilterID.Name = "txtFilterID";
            txtFilterID.Size = new Size(75, 27);
            txtFilterID.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(241, 31);
            label2.Name = "label2";
            label2.Size = new Size(109, 20);
            label2.TabIndex = 1;
            label2.Text = "Filter by Status:";
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
            // ddFilterEquipment
            // 
            ddFilterEquipment.FormattingEnabled = true;
            ddFilterEquipment.Location = new Point(677, 30);
            ddFilterEquipment.Name = "ddFilterEquipment";
            ddFilterEquipment.Size = new Size(118, 28);
            ddFilterEquipment.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(516, 33);
            label3.Name = "label3";
            label3.Size = new Size(145, 20);
            label3.TabIndex = 6;
            label3.Text = "Filter by Equipment :";
            // 
            // EquipmentManagementForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1108, 703);
            Controls.Add(groupBox1);
            Controls.Add(btnAdd);
            Controls.Add(btnUpdate);
            Controls.Add(btnClose);
            Controls.Add(dgvTransaction);
            Name = "EquipmentManagementForm";
            Text = "EquipmentManagementForm";
            Load += EquipmentManagementForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTransaction).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvTransaction;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnClose;
        private GroupBox groupBox1;
        private Button btnFilter;
        private ComboBox ddFilterStatus;
        private Button btnResetFilter;
        private TextBox txtFilterID;
        private Label label2;
        private Label label1;
        private ComboBox ddFilterEquipment;
        private Label label3;
    }
}