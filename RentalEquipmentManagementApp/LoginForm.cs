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
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var user = _authService.Authenticate(txtEmail.Text, txtPassword.Text);

            if (user == null)
            {
                MessageBox.Show("Invalid credentials");
                return;
            }

            if (user.Role != "Admin" && user.Role != "Manager")
            {
                MessageBox.Show("Access denied. This application is for administrators and managers only.");
                return;
            }

            _authService.LogAccess(user.Id, "Login");
            this.Hide();
            MainDashboardForm mainDashboardForm = new MainDashboardForm();
            mainDashboardForm.Show();
        }
    }
}
