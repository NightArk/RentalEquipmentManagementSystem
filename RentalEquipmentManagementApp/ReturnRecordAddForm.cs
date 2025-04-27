using System;
using System.Collections.Generic;
using System.ComponentModel;
using RentalEquipmentManagementLogic;
using RentalEquipmentManagementLogic.Models;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace RentalEquipmentManagementApp
{
    public partial class ReturnRecordAddForm : Form
    {
        private readonly EquipmentRentalDBContext _context;
        private readonly UserDto _currentUser;
        private readonly AuthService _authService;
        private List<dynamic> _rentalList;
        public ReturnRecordAddForm(UserDto currentUser)
        {
            InitializeComponent();
            _context = new EquipmentRentalDBContext();
            _authService = new AuthService(_context);
            _currentUser = currentUser;

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void ReturnRecordAddForm_Load(object sender, EventArgs e)
        {
            _rentalList = _context.RentalTransactions
                .Include(rt => rt.Customer)
                .Include(rt => rt.AssignedEquipment)
                .Select(rt => new
                {
                    Id = rt.Id,
                    CustomerName = rt.Customer.Name,
                    EquipmentName = rt.AssignedEquipment.Name
                }).ToList<dynamic>();

            cbRentalTransaction.DataSource = _rentalList;
            cbRentalTransaction.DisplayMember = "Id";
            cbRentalTransaction.ValueMember = "Id";
            cbRentalTransaction.SelectedIndex = -1;

            cbRentalTransaction.SelectedIndexChanged += cbRentalTransaction_SelectedIndexChanged;

            cbReturnCondition.Items.AddRange(new[] { "Good", "Damaged", "Lost" });
            cbReturnCondition.SelectedIndex = -1;

            dtpReturnDate.Value = DateTime.Today;
        }

        private void cbRentalTransaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRentalTransaction.SelectedItem != null)
            {
                var selected = (dynamic)cbRentalTransaction.SelectedItem;
                txtCustomerName.Text = selected.CustomerName;
                txtEquipmentName.Text = selected.EquipmentName;
            }
            else
            {
                txtCustomerName.Clear();
                txtEquipmentName.Clear();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbRentalTransaction.SelectedIndex == -1 ||
                    cbReturnCondition.SelectedIndex == -1 ||
                    string.IsNullOrWhiteSpace(txtLateFee.Text) ||
                    string.IsNullOrWhiteSpace(txtAdditionalCharges.Text))
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int rentalTransactionId = (int)cbRentalTransaction.SelectedValue;
                DateTime actualReturnDate = dtpReturnDate.Value;
                string returnCondition = cbReturnCondition.SelectedItem.ToString();
                decimal lateFee = decimal.Parse(txtLateFee.Text);
                decimal additionalCharges = decimal.Parse(txtAdditionalCharges.Text);

                if (lateFee < 0 || additionalCharges < 0)
                {
                    MessageBox.Show("Late return fee and additional charges must be positive.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var returnRecord = new ReturnRecord
                {
                    RentalTransactionId = rentalTransactionId,
                    ActualReturnDate = actualReturnDate,
                    ReturnCondition = returnCondition,
                    LateReturnFee = lateFee,
                    AdditionalCharges = additionalCharges,
                    CreatedAt = DateTime.Now
                };

                _context.ReturnRecords.Add(returnRecord);
                _context.SaveChanges();

                _authService.LogAccess(
                    _currentUser.Id,
                    "Added Return Record",
                    $"Return Record ID: {returnRecord.Id}"
                );

                MessageBox.Show("Return record added successfully!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while adding return record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
