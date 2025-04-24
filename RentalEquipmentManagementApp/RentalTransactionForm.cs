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
    public partial class RentalTransactionForm : Form
    {
        private readonly UserDto _currentUser;
        private readonly EquipmentRentalDBContext _context;
        public RentalTransactionForm(UserDto currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _context = new EquipmentRentalDBContext();
        }

        private void RentalTransactionForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new MainDashboardForm(_currentUser).Show();
            this.Hide();
        }
    }
}
