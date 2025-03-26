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
            InitializeUI();
        }

        private void MainDashboardForm_Load(object sender, EventArgs e)
        {

        }
        private void InitializeUI()
        {
            lblWelcome.Text = $"Welcome, {_currentUser.Name}!";
        }
    }
}
