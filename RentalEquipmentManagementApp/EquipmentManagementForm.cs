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
        }

        private void EquipmentManagementForm_Load(object sender, EventArgs e)
        {

        }
    }
}
