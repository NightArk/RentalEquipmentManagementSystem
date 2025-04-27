using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RentalEquipmentManagementLogic;
using RentalEquipmentManagementLogic.Models;

namespace RentalEquipmentManagementApp
{
    public partial class RentalTransactionAdd : Form
    {
        private readonly UserDto _currentUser;
        private readonly EquipmentRentalDBContext _context;
        private readonly AuthService _authService;
        public RentalTransactionAdd(UserDto currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _context = new EquipmentRentalDBContext();
            _authService = new AuthService(_context);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void RentalTransactionAdd_Load(object sender, EventArgs e)
        {
            cbCustomer.DataSource = _context.Users
                .Where(u => u.Role == "Customer")
                .Select(u => new { u.Id, u.Name })
                .ToList();
            cbCustomer.DisplayMember = "Name";
            cbCustomer.ValueMember = "Id";
            cbCustomer.SelectedIndex = -1;

            cbEquipment.DataSource = _context.Equipment
                .Select(e => new { e.Id, e.Name })
                .ToList();
            cbEquipment.DisplayMember = "Name";
            cbEquipment.ValueMember = "Id";
            cbEquipment.SelectedIndex = -1;

            cbPaymentStatus.Items.AddRange(new[] { "Pending", "Paid", "Overdue" });
            cbPaymentStatus.SelectedIndex = -1;

            dtpStartDate.Value = DateTime.Today;
            dtpReturnDate.Value = DateTime.Today;

            dtpStartDate.ValueChanged += DtpDates_ValueChanged;
            dtpReturnDate.ValueChanged += DtpDates_ValueChanged;
        }

        private void DtpDates_ValueChanged(object sender, EventArgs e)
        {
            int period = (dtpReturnDate.Value.Date - dtpStartDate.Value.Date).Days;

            if (period >= 0)
                txtRentalPeriod.Text = period.ToString();
            else
                txtRentalPeriod.Text = "0";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbCustomer.SelectedIndex == null || cbEquipment.SelectedIndex == null || cbPaymentStatus.SelectedIndex == null)
                {
                    MessageBox.Show("Please select a customer, equipment, and payment status.");
                    return;
                }
                if (!decimal.TryParse(txtRentalFee.Text, out decimal rentalFee) || rentalFee < 0)
                {
                    MessageBox.Show("Please enter a valid rental fee.");
                    return;
                }
                if (!decimal.TryParse(txtDeposit.Text, out decimal deposit) || deposit < 0)
                {
                    MessageBox.Show("Please enter a valid deposit amount.");
                    return;
                }

                int customerId = (int)((dynamic)cbCustomer.SelectedItem).Id;
                int equipmentId = (int)((dynamic)cbEquipment.SelectedItem).Id;
                string paymentStatus = cbPaymentStatus.SelectedItem.ToString();

                var transaction = new RentalTransaction
                {
                    CustomerId = customerId,
                    AssignedEquipmentId = equipmentId,
                    ActualRentalStartDate = dtpStartDate.Value.Date,
                    ReturnDate = dtpReturnDate.Value.Date,
                    RentalPeriod = (dtpReturnDate.Value.Date - dtpStartDate.Value.Date).Days,
                    RentalFee = rentalFee,
                    Deposit = deposit,
                    PaymentStatus = paymentStatus,
                    CreatedAt = DateTime.Now
                };

                _context.RentalTransactions.Add(transaction);
                _context.SaveChanges();

                _authService.LogAccess(
                    _currentUser.Id,
                    "Added Rental Transaction",
                    $"Customer ID: {customerId}, Equipment ID: {equipmentId}"
                );

                MessageBox.Show("Rental transaction added successfully.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding transaction: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
