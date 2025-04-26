using RentalEquipmentManagementLogic.Models;
using RentalEquipmentManagementLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace RentalEquipmentManagementApp
{
    public partial class RentalTransactionForm : Form
    {
        private readonly AuthService _authService;
        private readonly UserDto _currentUser;
        private readonly EquipmentRentalDBContext _context;

        private DateTimePicker dtpStart = new DateTimePicker();
        private DateTimePicker dtpReturn = new DateTimePicker();
        private Rectangle _dtpRectangle;
        public RentalTransactionForm(UserDto currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _context = new EquipmentRentalDBContext();
            _authService = new AuthService(_context);
            this.StartPosition = FormStartPosition.CenterScreen; // Center the form on the screen
        }

        private void RentalTransactionForm_Load(object sender, EventArgs e)
        {
            ddFilterCustomer.DataSource = _context.Users
                .Where(u => u.Role == "Customer")
                .Select(u => new { u.Id, u.Name })
                .ToList();
            ddFilterCustomer.DisplayMember = "Name";
            ddFilterCustomer.ValueMember = "Id";
            ddFilterCustomer.SelectedIndex = -1;

            ddFilterEquipment.DataSource = _context.Equipment
                .Select(e => new { e.Id, e.Name })
                .ToList();
            ddFilterEquipment.DisplayMember = "Name";
            ddFilterEquipment.ValueMember = "Id";
            ddFilterEquipment.SelectedIndex = -1;

            ddFilterStatus.Items.AddRange(new[] { "Pending", "Paid", "Overdue" });
            ddFilterStatus.SelectedIndex = -1;
            LoadTransactionData();

            dtpStart.Visible = false;
            dtpStart.Format = DateTimePickerFormat.Short;
            dtpStart.TextChanged += DtpStart_OnTextChange;
            dgvTransaction.Controls.Add(dtpStart);

            dtpReturn.Visible = false;
            dtpReturn.Format = DateTimePickerFormat.Short;
            dtpReturn.TextChanged += DtpReturn_OnTextChange;
            dgvTransaction.Controls.Add(dtpReturn);

            dgvTransaction.CellClick += DgvTransaction_CellClick;
        }

        private void DgvTransaction_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex < dgvTransaction.Columns.Count)
            {
                var cell = dgvTransaction.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string column = dgvTransaction.Columns[e.ColumnIndex].Name;

                _dtpRectangle = dgvTransaction.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);

                if (column == "ActualRentalStartDate")
                {
                    dtpStart.Size = new Size(_dtpRectangle.Width, _dtpRectangle.Height);
                    dtpStart.Location = new Point(_dtpRectangle.X, _dtpRectangle.Y);
                    dtpStart.Visible = true;

                    if (cell.Value != null)
                        dtpStart.Value = Convert.ToDateTime(cell.Value);
                    else
                        dtpStart.Value = DateTime.Today;

                    dtpStart.Focus();
                }
                else
                {
                    dtpStart.Visible = false;
                }

                if (column == "ReturnDate")
                {
                    dtpReturn.Size = new Size(_dtpRectangle.Width, _dtpRectangle.Height);
                    dtpReturn.Location = new Point(_dtpRectangle.X, _dtpRectangle.Y);
                    dtpReturn.Visible = true;

                    if (cell.Value != null)
                        dtpReturn.Value = Convert.ToDateTime(cell.Value);
                    else
                        dtpReturn.Value = DateTime.Today;

                    dtpReturn.Focus();
                }
                else
                {
                    dtpReturn.Visible = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new MainDashboardForm(_currentUser).Show();
            this.Hide();
        }

        private void dgvTransaction_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadTransactionData()
        {
            dgvTransaction.AutoGenerateColumns = true;
            dgvTransaction.Columns.Clear();

            var transactions = _context.RentalTransactions
                .Include(t => t.Customer)
                .Include(t => t.AssignedEquipment)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new RentalTransactionRow
                {
                    Id = t.Id,
                    Customer = t.Customer.Name,
                    Equipment = t.AssignedEquipment.Name,
                    ActualRentalStartDate = t.ActualRentalStartDate,
                    ReturnDate = t.ReturnDate,
                    RentalPeriod = t.RentalPeriod,
                    RentalFee = t.RentalFee,
                    Deposit = t.Deposit,
                    PaymentStatus = t.PaymentStatus,
                    CreatedAt = t.CreatedAt.HasValue ? t.CreatedAt.Value.ToString("dd/MM/yyyy") : ""
                })
                .ToList();

            dgvTransaction.DataSource = transactions;
        }

        private void txtFilterID_TextChanged(object sender, EventArgs e)
        {

        }

        private void ddFilterEquipment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ddFilterCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ddFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            var transactions = _context.RentalTransactions
                .Include(t => t.Customer)
                .Include(t => t.AssignedEquipment)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(txtFilterID.Text) && int.TryParse(txtFilterID.Text, out int transactionId))
            {
                transactions = transactions.Where(t => t.Id == transactionId);
            }

            if (ddFilterCustomer.SelectedItem != null)
            {
                var selectedCustomer = (dynamic)ddFilterCustomer.SelectedItem;
                int customerId = selectedCustomer.Id;
                transactions = transactions.Where(t => t.CustomerId == customerId);
            }

            if (ddFilterEquipment.SelectedItem != null)
            {
                var selectedEquipment = (dynamic)ddFilterEquipment.SelectedItem;
                int equipmentId = selectedEquipment.Id;
                transactions = transactions.Where(t => t.AssignedEquipmentId == equipmentId);
            }

            if (ddFilterStatus.SelectedItem != null)
            {
                string selectedStatus = ddFilterStatus.SelectedItem.ToString();
                transactions = transactions.Where(t => t.PaymentStatus == selectedStatus);
            }

            if (chkEnableStartDateFilter.Checked)
            {
                var startFrom = dtpFrom.Value.Date;
                var startTo = dtpTo.Value.Date;
                transactions = transactions.Where(t => t.ActualRentalStartDate >= startFrom && t.ActualRentalStartDate <= startTo);
            }

            var result = transactions
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new RentalTransactionRow
                {
                    Id = t.Id,
                    Customer = t.Customer.Name,
                    Equipment = t.AssignedEquipment.Name,
                    ActualRentalStartDate = t.ActualRentalStartDate,
                    ReturnDate = t.ReturnDate,
                    RentalPeriod = t.RentalPeriod,
                    RentalFee = t.RentalFee,
                    Deposit = t.Deposit,
                    PaymentStatus = t.PaymentStatus,
                    CreatedAt = t.CreatedAt.HasValue ? t.CreatedAt.Value.ToString("dd/MM/yyyy") : ""
                }).ToList();

            dgvTransaction.DataSource = result;
        }

        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            txtFilterID.Clear();
            ddFilterCustomer.SelectedIndex = -1;
            ddFilterEquipment.SelectedIndex = -1;
            ddFilterStatus.SelectedIndex = -1;
            chkEnableStartDateFilter.Checked = false;
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today;

            LoadTransactionData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgvTransaction.Rows)
                {
                    if (row.Cells["Id"].Value == null)
                        continue;

                    int id = Convert.ToInt32(row.Cells["Id"].Value);
                    var transaction = _context.RentalTransactions.FirstOrDefault(t => t.Id == id);
                    if (transaction != null)
                    {
                        DateTime newStart = Convert.ToDateTime(row.Cells["ActualRentalStartDate"].Value);
                        DateTime newReturn = Convert.ToDateTime(row.Cells["ReturnDate"].Value);
                        decimal newFee = Convert.ToDecimal(row.Cells["RentalFee"].Value);
                        decimal newDeposit = Convert.ToDecimal(row.Cells["Deposit"].Value);
                        string newStatus = row.Cells["PaymentStatus"].Value?.ToString();

                        if (newReturn < newStart)
                        {
                            MessageBox.Show($"Return date cannot be before rental start date.");
                            continue;
                        }

                        if (newFee < 0 || newDeposit < 0)
                        {
                            MessageBox.Show($"Fee and deposit must be positive.");
                            continue;
                        }

                        bool changed = false;
                        if (transaction.ActualRentalStartDate != newStart)
                        {
                            transaction.ActualRentalStartDate = newStart;
                            changed = true;
                        }

                        if (transaction.ReturnDate != newReturn)
                        {
                            transaction.ReturnDate = newReturn;
                            changed = true;
                        }

                        int period = (newReturn - newStart).Days;
                        transaction.RentalPeriod = period >= 0 ? period : 0;

                        if (transaction.RentalFee != newFee)
                        {
                            transaction.RentalFee = newFee;
                            changed = true;
                        }

                        if (transaction.Deposit != newDeposit)
                        {
                            transaction.Deposit = newDeposit;
                            changed = true;
                        }

                        if (transaction.PaymentStatus != newStatus)
                        {
                            transaction.PaymentStatus = newStatus;
                            changed = true;
                        }

                        if (changed)
                        {
                            _context.SaveChanges();

                            _authService.LogAccess(
                                _currentUser.Id,
                                "Updated Rental Transaction",
                                $"Transaction ID {id}"
                            );
                        }
                    }
                }
                MessageBox.Show("Updated successfully.");
                LoadTransactionData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during update: " + ex.Message);
            }
        }

        private void dgvTransaction_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgvTransaction.Columns.Contains("ActualRentalStartDate"))
                dgvTransaction.Columns["ActualRentalStartDate"].ReadOnly = false;

            if (dgvTransaction.Columns.Contains("ReturnDate"))
                dgvTransaction.Columns["ReturnDate"].ReadOnly = false;

            if (dgvTransaction.Columns.Contains("RentalFee"))
                dgvTransaction.Columns["RentalFee"].ReadOnly = false;

            if (dgvTransaction.Columns.Contains("Deposit"))
                dgvTransaction.Columns["Deposit"].ReadOnly = false;

            if (dgvTransaction.Columns.Contains("PaymentStatus"))
            {
                var comboColumn = new DataGridViewComboBoxColumn
                {
                    DataPropertyName = "PaymentStatus",
                    HeaderText = "Payment Status",
                    Name = "PaymentStatus",
                    DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                    FlatStyle = FlatStyle.Flat,
                    Items = { "Pending", "Paid", "Overdue" }
                };

                int index = dgvTransaction.Columns["PaymentStatus"].Index;
                dgvTransaction.Columns.Remove("PaymentStatus");
                dgvTransaction.Columns.Insert(index, comboColumn);
            }

            if (dgvTransaction.Columns.Contains("RentalPeriod"))
                dgvTransaction.Columns["RentalPeriod"].ReadOnly = true;


        }

        private void DtpStart_OnTextChange(object sender, EventArgs e)
        {
            if (dgvTransaction.CurrentCell != null)
            {
                dgvTransaction.CurrentCell.Value = dtpStart.Text;
                UpdateRentalPeriodFromGrid();
            }
            dtpStart.Visible = false;
        }

        private void DtpReturn_OnTextChange(object sender, EventArgs e)
        {
            if (dgvTransaction.CurrentCell != null)
            {
                dgvTransaction.CurrentCell.Value = dtpReturn.Text;
                UpdateRentalPeriodFromGrid();
            }
            dtpReturn.Visible = false;
        }

        private void UpdateRentalPeriodFromGrid()
        {
            if (dgvTransaction.CurrentRow != null)
            {
                var row = dgvTransaction.CurrentRow;

                if (row.Cells["ActualRentalStartDate"].Value != null && row.Cells["ReturnDate"].Value != null)
                {
                    DateTime start = Convert.ToDateTime(row.Cells["ActualRentalStartDate"].Value);
                    DateTime end = Convert.ToDateTime(row.Cells["ReturnDate"].Value);

                    int period = (end - start).Days;

                    if (period >= 0)
                        row.Cells["RentalPeriod"].Value = period;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTransaction.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a transaction to delete.");
                return;
            }

            var row = dgvTransaction.SelectedRows[0];
            int transactionId = Convert.ToInt32(row.Cells["Id"].Value);

            var confirm = MessageBox.Show($"Are you sure you want to delete transaction ID {transactionId}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                var transaction = _context.RentalTransactions.FirstOrDefault(t => t.Id == transactionId);

                if (transaction != null)
                {
                    var relatedDocs = _context.Documents.Where(d => d.RentalTransactionId == transactionId).ToList();
                    _context.Documents.RemoveRange(relatedDocs);

                    _context.RentalTransactions.Remove(transaction);
                    _context.SaveChanges();

                    _authService.LogAccess(
                        _currentUser.Id,
                        "Deleted Rental Transaction",
                        $"Transaction ID {transactionId}"
                    );

                    MessageBox.Show("Transaction deleted successfully.");
                    LoadTransactionData();
                }
                else
                {
                    MessageBox.Show("Transaction not found.");
                }
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addForm = new RentalTransactionAdd(_currentUser);
            addForm.FormClosed += (s, args) => LoadTransactionData();
            addForm.ShowDialog();
        }
    }
}
