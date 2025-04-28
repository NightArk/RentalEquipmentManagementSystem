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
    public partial class LoginForm : Form
    {
        private readonly AuthService _authService;

        public LoginForm()
        {
            InitializeComponent();
            _authService = new AuthService(new EquipmentRentalDBContext());
            this.StartPosition = FormStartPosition.CenterScreen; // Center the form on the screen
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var user = _authService.Authenticate(txtEmail.Text, txtPassword.Text);

            if (user == null)
            {
                MessageBox.Show("Invalid credentials");
                return;
            }

            if (user.Role != "Admin" && user.Role != "Manager" && user.Role != "RentalManager")
            {
                MessageBox.Show("Access denied. This application is for administrators and managers only.");
                return;
            }

           
            new MainDashboardForm(user).Show();
            _authService.LogAccess(user.Id,"Login","none");
            this.Hide();
        }
    }
}
