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
        }

        private void ReturnRecordForm_Load(object sender, EventArgs e)
        {

        }
    }
}
