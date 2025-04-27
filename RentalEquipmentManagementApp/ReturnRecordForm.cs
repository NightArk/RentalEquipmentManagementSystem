using Microsoft.EntityFrameworkCore;
using RentalEquipmentManagementLogic;
using RentalEquipmentManagementLogic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RentalEquipmentManagementApp
{
    public partial class ReturnRecordForm : Form
    {
        private readonly AuthService _authService;
        private readonly UserDto _currentUser;
        private readonly EquipmentRentalDBContext _context;
        private DateTimePicker dtpReturnDate = new DateTimePicker();
        private Rectangle _dtpRectangle;
        public ReturnRecordForm(UserDto currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _context = new EquipmentRentalDBContext();
            _authService = new AuthService(_context);

            this.StartPosition = FormStartPosition.CenterScreen; // Center the form on the screen
        }


        private void DtpReturnDate_OnTextChange(object sender, EventArgs e)
        {
            if (dgvReturnRecord.CurrentCell != null)
            {
                dgvReturnRecord.CurrentCell.Value = dtpReturnDate.Text;
            }
            dtpReturnDate.Visible = false;
        }

        private void ReturnRecordForm_Load(object sender, EventArgs e)
        {
            dtpReturnDate.Visible = false;
            dtpReturnDate.Format = DateTimePickerFormat.Short;
            dtpReturnDate.TextChanged += new EventHandler(DtpReturnDate_OnTextChange);
            dgvReturnRecord.Controls.Add(dtpReturnDate);
            LoadReturnRecordsData();
            cbFilterCustomer.DataSource = _context.Users.Where(u => u.Role == "Customer")
                .Select(u => new { u.Id, u.Name })
                .ToList();
            cbFilterCustomer.DisplayMember = "Name";
            cbFilterCustomer.ValueMember = "Id";
            cbFilterCustomer.SelectedIndex = -1;

            cbFilterEquipment.DataSource = _context.Equipment
                .Select(e => new { e.Id, e.Name })
                .ToList();
            cbFilterEquipment.DisplayMember = "Name";
            cbFilterEquipment.ValueMember = "Id";
            cbFilterEquipment.SelectedIndex = -1;

            cbFilterCondition.Items.AddRange(new[] { "Good", "Damaged", "Lost" });
            cbFilterCondition.SelectedIndex = -1;
        }
        private void LoadReturnRecordsData()
        {
            dgvReturnRecord.AutoGenerateColumns = true;
            dgvReturnRecord.Columns.Clear();

            var returnRecords = _context.ReturnRecords
                .Include(r => r.RentalTransaction)
                .ThenInclude(t => t.Customer)
                .Include(r => r.RentalTransaction)
                .ThenInclude(t => t.AssignedEquipment)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new RentalRecordGridItem
                {
                    Id = r.Id,
                    Customer = r.RentalTransaction.Customer.Name,
                    Equipment = r.RentalTransaction.AssignedEquipment.Name,
                    ActualReturnDate = r.ActualReturnDate,
                    ReturnCondition = r.ReturnCondition,
                    LateReturnFee = r.LateReturnFee.GetValueOrDefault(),
                    AdditionalCharges = r.AdditionalCharges.GetValueOrDefault(),
                    CreatedAt = r.CreatedAt.HasValue ? r.CreatedAt.Value.ToString("dd/MM/yyyy") : ""
                }).ToList();

            dgvReturnRecord.DataSource = returnRecords;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            new MainDashboardForm(_currentUser).Show();
            this.Hide();
        }



        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            txtFilterID.Clear();
            cbFilterCustomer.SelectedIndex = -1;
            cbFilterEquipment.SelectedIndex = -1;
            cbFilterCondition.SelectedIndex = -1;
            chkEnableDateFilter.Checked = false;
            dtpFilterFrom.Value = DateTime.Today;
            dtpFilterTo.Value = DateTime.Today;

            LoadReturnRecordsData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgvReturnRecord.Rows)
                {
                    if (row.Cells["Id"].Value == null)
                        continue;

                    int id = Convert.ToInt32(row.Cells["Id"].Value);
                    var record = _context.ReturnRecords.FirstOrDefault(r => r.Id == id);

                    if (record != null)
                    {
                        DateTime newReturnDate = Convert.ToDateTime(row.Cells["ActualReturnDate"].Value);
                        string newCondition = row.Cells["ReturnCondition"].Value?.ToString();
                        decimal newLateFee = row.Cells["LateReturnFee"].Value != null ? Convert.ToDecimal(row.Cells["LateReturnFee"].Value) : 0;
                        decimal newAdditionalCharges = row.Cells["AdditionalCharges"].Value != null ? Convert.ToDecimal(row.Cells["AdditionalCharges"].Value) : 0;


                        if (newLateFee < 0 || newAdditionalCharges < 0)
                        {
                            MessageBox.Show("Late fee and additional charges must be positive.");
                            continue;
                        }

                        bool changed = false;
                        StringBuilder changes = new StringBuilder();

                        if (record.ActualReturnDate != newReturnDate)
                        {
                            record.ActualReturnDate = newReturnDate;
                            changes.AppendLine($"Actual Return Date changed.");
                            changed = true;
                        }

                        if (record.ReturnCondition != newCondition)
                        {
                            record.ReturnCondition = newCondition;
                            changes.AppendLine($"Return Condition changed.");
                            changed = true;
                        }

                        if (record.LateReturnFee != newLateFee)
                        {
                            record.LateReturnFee = newLateFee;
                            changes.AppendLine($"Late Return Fee changed.");
                            changed = true;
                        }

                        if (record.AdditionalCharges != newAdditionalCharges)
                        {
                            record.AdditionalCharges = newAdditionalCharges;
                            changes.AppendLine($"Additional Charges changed.");
                            changed = true;
                        }

                        if (changed)
                        {
                            _authService.LogAccess(
                                _currentUser.Id,
                                "Update Return Record",
                                $"Return Record ID: {id}\n{changes}");
                        }
                    }
                }

                _context.SaveChanges();
                MessageBox.Show("All valid changes saved successfully.");
                LoadReturnRecordsData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving changes: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvReturnRecord.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a return record to delete.");
                return;
            }

            var row = dgvReturnRecord.SelectedRows[0];
            int returnRecordId = Convert.ToInt32(row.Cells["Id"].Value);

            var confirm = MessageBox.Show($"Are you sure you want to delete return record ID {returnRecordId}?",
                "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                var record = _context.ReturnRecords.FirstOrDefault(r => r.Id == returnRecordId);

                if (record != null)
                {
                    _context.ReturnRecords.Remove(record);
                    _context.SaveChanges();
                    _authService.LogAccess(
                        _currentUser.Id,
                        "Deleted Return Record",
                        $"Return Record ID: {returnRecordId}"
                        );
                    MessageBox.Show("Return record deleted successfully.");
                    LoadReturnRecordsData();
                }
                else
                {
                    MessageBox.Show("Return record not found.");
                }
            }
        }



        private void btnFilter_Click_2(object sender, EventArgs e)
        {
            var records = _context.ReturnRecords
                .Include(r => r.RentalTransaction)
                .ThenInclude(t => t.Customer)
                .Include(r => r.RentalTransaction)
                .ThenInclude(t => t.AssignedEquipment)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(txtFilterID.Text) && int.TryParse(txtFilterID.Text, out int returnRecordId))
            {
                records = records.Where(r => r.Id == returnRecordId);
            }

            if (cbFilterCustomer.SelectedItem != null)
            {
                var selectedCustomer = (dynamic)cbFilterCustomer.SelectedItem;
                int customerId = selectedCustomer.Id;
                records = records.Where(r => r.RentalTransaction.CustomerId == customerId);
            }

            if (cbFilterEquipment.SelectedItem != null)
            {
                var selectedEquipment = (dynamic)cbFilterEquipment.SelectedItem;
                int equipmentId = selectedEquipment.Id;
                records = records.Where(r => r.RentalTransaction.AssignedEquipmentId == equipmentId);
            }

            if (cbFilterCondition.SelectedItem != null)
            {
                string condition = cbFilterCondition.SelectedItem.ToString();
                records = records.Where(r => r.ReturnCondition == condition);
            }

            if (chkEnableDateFilter.Checked)
            {
                DateTime fromDate = dtpFilterFrom.Value.Date;
                DateTime toDate = dtpFilterTo.Value.Date;
                records = records.Where(r => r.ActualReturnDate.Date >= fromDate && r.ActualReturnDate.Date <= toDate);
            }

            var result = records
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new RentalRecordGridItem
                {
                    Id = r.Id,
                    Customer = r.RentalTransaction.Customer.Name,
                    Equipment = r.RentalTransaction.AssignedEquipment.Name,
                    ActualReturnDate = r.ActualReturnDate,
                    ReturnCondition = r.ReturnCondition,
                    LateReturnFee = r.LateReturnFee.GetValueOrDefault(),
                    AdditionalCharges = r.AdditionalCharges.GetValueOrDefault(),
                    CreatedAt = r.CreatedAt.HasValue ? r.CreatedAt.Value.ToString("dd/MM/yyyy") : ""
                }).ToList();

            dgvReturnRecord.DataSource = result;
        }

        private void dgvReturnRecord_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgvReturnRecord.Columns.Contains("ActualReturnDate"))
                dgvReturnRecord.Columns["ActualReturnDate"].ReadOnly = false;

            if (dgvReturnRecord.Columns.Contains("LateReturnFee"))
                dgvReturnRecord.Columns["LateReturnFee"].ReadOnly = false;

            if (dgvReturnRecord.Columns.Contains("AdditionalCharges"))
                dgvReturnRecord.Columns["AdditionalCharges"].ReadOnly = false;

            if (dgvReturnRecord.Columns.Contains("ReturnCondition"))
            {
                var comboColumn = new DataGridViewComboBoxColumn
                {
                    DataPropertyName = "ReturnCondition",
                    HeaderText = "Return Condition",
                    Name = "ReturnCondition",
                    DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                    FlatStyle = FlatStyle.Flat,
                    Items = { "Good", "Damaged", "Lost" }
                };

                int index = dgvReturnRecord.Columns["ReturnCondition"].Index;
                dgvReturnRecord.Columns.Remove("ReturnCondition");
                dgvReturnRecord.Columns.Insert(index, comboColumn);
            }
        }

        private void dgvReturnRecord_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string columnName = dgvReturnRecord.Columns[e.ColumnIndex].Name;

                _dtpRectangle = dgvReturnRecord.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);

                if (columnName == "ActualReturnDate")
                {
                    dtpReturnDate.Size = new Size(_dtpRectangle.Width, _dtpRectangle.Height);
                    dtpReturnDate.Location = new Point(_dtpRectangle.X, _dtpRectangle.Y);
                    dtpReturnDate.Visible = true;

                    if (dgvReturnRecord.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        dtpReturnDate.Value = Convert.ToDateTime(dgvReturnRecord.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    }
                    else
                    {
                        dtpReturnDate.Value = DateTime.Today;
                    }

                    dtpReturnDate.Focus();
                }
                else
                {
                    dtpReturnDate.Visible = false;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addForm = new ReturnRecordAddForm(_currentUser);

            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadReturnRecordsData();
            }
        }
    }
}
