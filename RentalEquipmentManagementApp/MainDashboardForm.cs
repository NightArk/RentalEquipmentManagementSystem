using Microsoft.VisualBasic.ApplicationServices;
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
using User = Microsoft.VisualBasic.ApplicationServices.User;

namespace RentalEquipmentManagementApp
{
    public partial class MainDashboardForm : Form
    {
        private readonly UserDto _currentUser;
        private readonly EquipmentRentalDBContext _context;

        public MainDashboardForm(UserDto currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _context = new EquipmentRentalDBContext();
           
            this.StartPosition = FormStartPosition.CenterScreen; // Center the form on the screen
        }


        private void MainDashboardForm_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Welcome, {_currentUser.Name}!";
        }
        

        private void btnLogout_Click(object sender, EventArgs e)
        {
            new LoginForm().Show();
            this.Hide();
        }

        private void btnRentalRequests_Click(object sender, EventArgs e)
        {
            new RentalRequestsForm(_currentUser).Show();
            this.Hide();
        }

        private void btnRentalTransaction_Click(object sender, EventArgs e)
        {
            new RentalTransactionForm(_currentUser).Show();
            this.Hide();
        }

        private void btnReturnRecord_Click(object sender, EventArgs e)
        {
            new ReturnRecordForm(_currentUser).Show();
            this.Hide();
        }

        private void btnEquipmentManagement_Click(object sender, EventArgs e)
        {
            new EquipmentManagementForm(_currentUser).Show();
            this.Hide();
        }
    }
}
