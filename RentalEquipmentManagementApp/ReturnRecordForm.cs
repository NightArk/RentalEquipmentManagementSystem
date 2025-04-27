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
        private readonly UserDto _currentUser;
        private readonly EquipmentRentalDBContext _context;
        public ReturnRecordForm(UserDto currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _context = new EquipmentRentalDBContext();

            this.StartPosition = FormStartPosition.CenterScreen; // Center the form on the screen
        }

        private void ReturnRecordForm_Load(object sender, EventArgs e)
        {
            // Populate the Equipment dropdown for filtering
            ddFilterEquipment.DataSource = _context.Equipment
                .Select(e => new { e.Id, e.Name })
                .ToList();
            ddFilterEquipment.DisplayMember = "Name";
            ddFilterEquipment.ValueMember = "Id";
            ddFilterEquipment.SelectedIndex = -1;

            // Load the initial data into the DataGridView
            LoadReturnRecordData();
        }
        private void LoadReturnRecordData()
        {
            dgvReturnRecord.AutoGenerateColumns = true;
            dgvReturnRecord.Columns.Clear();

            var returnRecords = _context.ReturnRecords
                .Include(r => r.RentalTransaction)
                .ThenInclude(t => t.AssignedEquipment)
                .OrderByDescending(r => r.ActualReturnDate)
                .Select(r => new
                {
                    r.Id,
                    EquipmentName = r.RentalTransaction.AssignedEquipment.Name,
                    r.ActualReturnDate,
                    r.ReturnCondition,
                    r.LateReturnFee
                })
                .ToList();

            dgvReturnRecord.DataSource = returnRecords;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            new MainDashboardForm(_currentUser).Show();
            this.Hide();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            var returnRecordQuery = _context.ReturnRecords
                  .Include(r => r.RentalTransaction)
                  .ThenInclude(t => t.AssignedEquipment)
                  .AsQueryable();

            // Filter by ID
            if (!string.IsNullOrWhiteSpace(txtFilterID.Text) && int.TryParse(txtFilterID.Text, out int returnRecordId))
            {
                returnRecordQuery = returnRecordQuery.Where(r => r.Id == returnRecordId);
            }

            // Filter by Equipment
            if (ddFilterEquipment.SelectedItem != null)
            {
                var selectedEquipment = (dynamic)ddFilterEquipment.SelectedItem;
                int selectedEquipmentId = selectedEquipment.Id;
                returnRecordQuery = returnRecordQuery.Where(r => r.RentalTransaction.AssignedEquipment.Id == selectedEquipmentId);
            }

            var filteredReturnRecords = returnRecordQuery
                .OrderByDescending(r => r.ActualReturnDate)
                .Select(r => new
                {
                    r.Id,
                    EquipmentName = r.RentalTransaction.AssignedEquipment.Name,
                    r.ActualReturnDate,
                    r.ReturnCondition,
                    r.LateReturnFee
                })
                .ToList();

            dgvReturnRecord.DataSource = filteredReturnRecords;
        }

        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            txtFilterID.Clear();
            ddFilterEquipment.SelectedIndex = -1;

            LoadReturnRecordData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvReturnRecord.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a return record to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dgvReturnRecord.SelectedRows[0];
            int returnRecordId = Convert.ToInt32(selectedRow.Cells["Id"].Value);

            var confirmResult = MessageBox.Show(
                $"Are you sure you want to delete the return record with ID {returnRecordId}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    var returnRecord = _context.ReturnRecords.FirstOrDefault(r => r.Id == returnRecordId);

                    // Check if the return record exists
                    if (returnRecord == null)
                       MessageBox.Show("Return record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


                    // Remove the return record from the database
                    _context.ReturnRecords.Remove(returnRecord);
                    _context.SaveChanges();

                    MessageBox.Show("Return record deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadReturnRecordData(); // Refresh the DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while deleting the return record: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
