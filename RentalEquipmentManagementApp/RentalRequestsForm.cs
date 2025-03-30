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
        private readonly UserDto _currentUser;
        private readonly EquipmentRentalDBContext _context;
        public RentalRequestsForm(UserDto currentUser)
        {
            InitializeComponent();

            _currentUser = currentUser;
            _context = new EquipmentRentalDBContext();

        }

        private void ReftereshData()
        {
            dgvRequests.DataSource = _context.RentalRequests.ToList();
            var requests = _context.RentalRequests.AsQueryable();

            if (ddFilterStatus.SelectedItem != null)
                requests = requests.Where(r => r.Status == ddFilterStatus.SelectedItem.ToString());


            if (txtFilterID.Text != "")
                requests = requests.Where(r => r.Id == Convert.ToInt32(txtFilterID.Text));


#pragma warning disable CS8629 // Nullable value type may be null.
            dgvRequests.DataSource = requests.OrderByDescending(m => m.ReturnDate)
                .Select(r => new
                {
                    ID = r.Id,
                    CustomerID = r.CustomerId,
                    EquipmentID = r.EquipmentId,
                    RentalStartDate = r.RentalStartDate,
                    RentalEndDate = r.ReturnDate,
                    Cost = r.TotalCost,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt.Value.ToString("dd/MM/yyyy"),
                }).ToList();
#pragma warning restore CS8629 // Nullable value type may be null.
        }

        private void RentalRequestsForm_Load(object sender, EventArgs e)
        {
            ddFilterStatus.DataSource = _context.RentalRequests.ToList();
            ddFilterStatus.DisplayMember = "Status";
            ddFilterStatus.ValueMember = "Id";
            ddFilterStatus.SelectedItem = null;
            ReftereshData();
        }

        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            ReftereshData();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            ReftereshData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            new MainDashboardForm(_currentUser).Show();
            this.Hide();
        }
    }
}
