using RentalEquipmentManagementLogic;
using RentalEquipmentManagementLogic.Models;
using System;
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
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var user = await _authService.AuthenticateAsync(txtEmail.Text, txtPassword.Text);

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
                await _authService.LogAccessAsync(user.Id, "Login", "none");
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}");
            }
        }
    }
}