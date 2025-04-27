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

namespace RentalEquipmentManagementApp
{
    public partial class EquipmentManagementForm : Form
    {
        private readonly UserDto _currentUser;
        private readonly EquipmentRentalDBContext _context;
        public EquipmentManagementForm(UserDto currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _context = new EquipmentRentalDBContext();

            this.StartPosition = FormStartPosition.CenterScreen; // Center the form on the screen
        }

        private void EquipmentManagementForm_Load(object sender, EventArgs e)
        {
            // Populate filter dropdowns
            ddFilterEquipment.DataSource = _context.Equipment
                .Select(e => new { e.Id, e.Name })
                .ToList();
            ddFilterEquipment.DisplayMember = "Name";
            ddFilterEquipment.ValueMember = "Id";
            ddFilterEquipment.SelectedIndex = -1;

            ddFilterStatus.Items.AddRange(new[] { "Available", "Rented", "Under Maintenance", "Unavailable", "Damaged", "Good", "New" });
            ddFilterStatus.SelectedIndex = -1;

            LoadEquipmentData();
        }
        private void LoadEquipmentData()
        {
            dgvEquipmentInformation.AutoGenerateColumns = true;
            dgvEquipmentInformation.Columns.Clear();

            var equipmentList = _context.Equipment
                .OrderBy(e => e.Name)
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.Description,
                    e.RentalPrice,
                    e.AvailabilityStatus,
                    e.ConditionStatus,
                    CreatedAt = e.CreatedAt.HasValue ? e.CreatedAt.Value.ToString("dd/MM/yyyy") : ""
                })
                .ToList();

            dgvEquipmentInformation.DataSource = equipmentList;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            new MainDashboardForm(_currentUser).Show();
            this.Hide();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            var equipmentQuery = _context.Equipment.AsQueryable();

            // Filter by ID
            if (!string.IsNullOrWhiteSpace(txtFilterID.Text) && int.TryParse(txtFilterID.Text, out int equipmentId))
            {
                equipmentQuery = equipmentQuery.Where(e => e.Id == equipmentId);
            }

            // Filter by Equipment
            if (ddFilterEquipment.SelectedItem != null)
            {
                var selectedEquipment = (dynamic)ddFilterEquipment.SelectedItem;
                int selectedEquipmentId = selectedEquipment.Id;
                equipmentQuery = equipmentQuery.Where(e => e.Id == selectedEquipmentId);
            }

            // Filter by Status
            if (ddFilterStatus.SelectedItem != null)
            {
                string selectedStatus = ddFilterStatus.SelectedItem.ToString();
                equipmentQuery = equipmentQuery.Where(e => e.AvailabilityStatus == selectedStatus);
            }

            var filteredEquipment = equipmentQuery
                .OrderBy(e => e.Name)
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.Description,
                    e.RentalPrice,
                    e.AvailabilityStatus,
                    e.ConditionStatus,
                    CreatedAt = e.CreatedAt.HasValue ? e.CreatedAt.Value.ToString("dd/MM/yyyy") : ""
                })
                .ToList();

            dgvEquipmentInformation.DataSource = filteredEquipment;
        }

        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            txtFilterID.Clear();
            ddFilterEquipment.SelectedIndex = -1;
            ddFilterStatus.SelectedIndex = -1;

            LoadEquipmentData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
          
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
          
        }
    }
}
