using Azure.Core;
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
    public partial class RentalRequestsForm : Form
    {
        private readonly AuthService _authService;
        private readonly UserDto _currentUser;
        private readonly EquipmentRentalDBContext _context;
        private DateTimePicker dtp = new DateTimePicker();
        private Rectangle _dtpRectangle;
        public RentalRequestsForm(UserDto currentUser)
        {
            InitializeComponent();

            _currentUser = currentUser;
            _context = new EquipmentRentalDBContext();
            _authService = new AuthService(_context);

            this.StartPosition = FormStartPosition.CenterScreen; // Center the form on the screen
        }

        private void ReftereshData()
        {
            dgvRequests.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.Black
            };
            dgvRequests.DataSource = _context.RentalRequests.ToList();

            dgvRequests.ReadOnly = false;
            var requests = _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .AsQueryable();


            if (ddFilterStatus.SelectedItem != null)
            {
                var selectedCustomer = (dynamic)ddFilterStatus.SelectedItem;
                int selectedCustomerId = selectedCustomer.Id;

                if (selectedCustomerId != 0)
                {
                    requests = requests.Where(r => r.CustomerId == selectedCustomerId);
                }
            }



            if (txtFilterID.Text != "")
                requests = requests.Where(r => r.Id == Convert.ToInt32(txtFilterID.Text));


#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var requestItems = requests.OrderByDescending(m => m.ReturnDate)
                .Select(r => new RentalRequestGridItem
                {
                    Id = r.Id,
                    Customer = r.Customer.Name,
                    Equipment = r.Equipment.Name,
                    RentalStartDate = r.RentalStartDate,
                    ReturnDate = r.ReturnDate,
                    TotalCost = r.TotalCost,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt.HasValue ? r.CreatedAt.Value.ToString("dd/MM/yyyy") : ""
                }).ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8601 // Possible null reference assignment.

            dgvRequests.DataSource = requestItems;
        }

        private void RentalRequestsForm_Load(object sender, EventArgs e)
        {
            dtp.Visible = false;
            dtp.Format = DateTimePickerFormat.Short;
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            dtp.TextChanged += new EventHandler(dtp_OnTextChange);
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            dgvRequests.Controls.Add(dtp);
            var customers = _context.Users
                .Where(u => u.Role == "Customer")
                .Select(u => new { u.Id, u.Name })
                .ToList();

            customers.Insert(0, new { Id = 0, Name = "None" });

            ddFilterStatus.DataSource = customers;
            ddFilterStatus.DisplayMember = "Name";
            ddFilterStatus.ValueMember = "Id";
            ddFilterStatus.SelectedIndex = 0;

            ReftereshData();
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            dgvRequests.CellClick += dgvRequests_CellClick;
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            _authService.LogAccess(
                _currentUser.Id,
                "Viewed Rental Requests",
                "Opened RentalRequestsForm"
            );
        }
        private void dtp_OnTextChange(object sender, EventArgs e)
        {
            dgvRequests.CurrentCell.Value = dtp.Text;
        }

        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            txtFilterID.Clear();
            ddFilterStatus.SelectedIndex = 0;
            ReftereshData();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            ReftereshData();
            _authService.LogAccess(
                _currentUser.Id,
                "Filtered Rental Requests",
                $"ID Filter: {txtFilterID.Text}, Customer Filter: {ddFilterStatus.Text}"
            );
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            new MainDashboardForm(_currentUser).Show();
            this.Hide();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void dgvRequests_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvRequests_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvRequests_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvRequests.Columns["RentalStartDate"].ReadOnly = false;
            dgvRequests.Columns["ReturnDate"].ReadOnly = false;
            if (dgvRequests.Columns.Contains("Status"))
            {
                var statusColumn = new DataGridViewComboBoxColumn
                {
                    DataPropertyName = "Status",
                    HeaderText = "Status",
                    Name = "Status", // Must match the DTO property
                    DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                    FlatStyle = FlatStyle.Flat,
                    Items = { "Pending", "Approved", "Rejected" }
                };

                // Replace the existing "Status" column with the dropdown
                int colIndex = dgvRequests.Columns["Status"].Index;
                dgvRequests.Columns.Remove("Status");
                dgvRequests.Columns.Insert(colIndex, statusColumn);
            }
            dgvRequests.Columns["TotalCost"].ReadOnly = false;
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgvRequests.Rows)
                {
                    if (row.Cells["Id"].Value == null)
                        continue;

                    int id = Convert.ToInt32(row.Cells["Id"].Value);
                    var request = _context.RentalRequests.FirstOrDefault(r => r.Id == id);

                    if (request != null)
                    {
                        // Parse new values
                        DateTime newStartDate = Convert.ToDateTime(row.Cells["RentalStartDate"].Value);
                        DateTime newReturnDate = Convert.ToDateTime(row.Cells["ReturnDate"].Value);
                        decimal newTotalCost = Convert.ToDecimal(row.Cells["TotalCost"].Value);
                        string newStatus = row.Cells["Status"].Value?.ToString();

                        // VALIDATION
                        if (newReturnDate < newStartDate)
                        {
                            MessageBox.Show($"Return date cannot be before rental start date for Request ID {id}.");
                            continue;
                        }

                        if (newTotalCost < 0)
                        {
                            MessageBox.Show($"Total cost must be a positive number for Request ID {id}.");
                            continue;
                        }

                        // Track changes
                        bool changed = false;
                        StringBuilder changes = new StringBuilder();

                        if (request.RentalStartDate != newStartDate)
                        {
                            changes.AppendLine($"RentalStartDate: {request.RentalStartDate} → {newStartDate}");
                            request.RentalStartDate = newStartDate;
                            changed = true;
                        }

                        if (request.ReturnDate != newReturnDate)
                        {
                            changes.AppendLine($"ReturnDate: {request.ReturnDate} → {newReturnDate}");
                            request.ReturnDate = newReturnDate;
                            changed = true;
                        }

                        if (request.TotalCost != newTotalCost)
                        {
                            changes.AppendLine($"TotalCost: {request.TotalCost} → {newTotalCost}");
                            request.TotalCost = newTotalCost;
                            changed = true;
                        }

                        if (request.Status != newStatus)
                        {
                            changes.AppendLine($"Status: {request.Status} → {newStatus}");
                            request.Status = newStatus;
                            changed = true;
                        }

                        // Only log if something actually changed
                        if (changed)
                        {
                            _authService.LogAccess(
                                _currentUser.Id,
                                "Updated Rental Request",
                                $"Request ID {id}\n{changes}"
                            );
                        }
                    }
                }

                _context.SaveChanges();
                MessageBox.Show("All valid changes saved successfully.");
                ReftereshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving changes: " + ex.Message);
            }
        }


        private void dgvRequests_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((dgvRequests.Columns[e.ColumnIndex].Name == "RentalStartDate" ||
         dgvRequests.Columns[e.ColumnIndex].Name == "ReturnDate") && e.RowIndex >= 0)
            {
                _dtpRectangle = dgvRequests.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                dtp.Size = new Size(_dtpRectangle.Width, _dtpRectangle.Height);
                dtp.Location = new Point(_dtpRectangle.X, _dtpRectangle.Y);
                dtp.Visible = true;

                if (dgvRequests.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    dtp.Value = Convert.ToDateTime(dgvRequests.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                }
                else
                {
                    dtp.Value = DateTime.Today;
                }

                dtp.Focus();
            }
            else
            {
                dtp.Visible = false;
            }
        }
    }
}
