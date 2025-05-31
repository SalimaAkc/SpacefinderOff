using MySql.Data.MySqlClient;
using SpacefinderOff.Models;
using SpacefinderOff.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SpacefinderOff.Views
{
    public partial class SignUpPage : Page
    {
        private bool isSyncingPassword = false;
        private bool isSyncingConfirmPassword = false;

        public SignUpPage()
        {
            InitializeComponent();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string phone = PhoneNumberTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (!Regex.IsMatch(phone, @"^\d+$"))
            {
                MessageBox.Show(
                    "Phone number must contain only numbers (0-9). Please remove any letters or symbols.",
                    "Invalid Phone Number",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            if (!EmailValidator.IsValidThomasMoreEmail(email))
            {
                MessageBox.Show("Please use your school email address ending with '@student.thomasmore.be' or '@thomasmore.be'.",
                                "Invalid Email",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(fullName) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(phone) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string connectionString = "server=localhost;user=root;password=;database=SpaceFinderAppDB;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE email = @Email";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@Email", email);
                    int existingCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (existingCount > 0)
                    {
                        MessageBox.Show("An account with this email already exists.", "Duplicate Account", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

     
                    string insertQuery = "INSERT INTO Users (fullname, email, phone_number, password, role_id, created_at) " +
                                         "VALUES (@FullName, @Email, @PhoneNumber, @Password, @RoleId, @CreatedAt)";
                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@FullName", fullName);
                    insertCmd.Parameters.AddWithValue("@Email", email);
                    insertCmd.Parameters.AddWithValue("@PhoneNumber", phone);
                    insertCmd.Parameters.AddWithValue("@Password", password);
                    insertCmd.Parameters.AddWithValue("@RoleId", "user"); 
                    insertCmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                    int rowsAffected = insertCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        AppState.RequestRefresh();

                        AppState.CurrentUser = new Models.User
                        {
                            FullName = fullName,
                            Email = email,
                            PhoneNumber = phone,
                            Password = password,
                            Role = "user"
                        };

                        this.NavigationService?.Navigate(new BookingPage());
                    }
                    else
                    {
                        MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new LoginPage());
        }

        private void TogglePasswordVisibilityButton_Checked(object sender, RoutedEventArgs e)
        {
            PasswordBox.Visibility = Visibility.Collapsed;
            PasswordVisible.Visibility = Visibility.Visible;
            EyeOutline.Visibility = Visibility.Collapsed;
            EyeFilled.Visibility = Visibility.Visible;
            PasswordVisible.Text = PasswordBox.Password;
        }

        private void TogglePasswordVisibilityButton_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordBox.Visibility = Visibility.Visible;
            PasswordVisible.Visibility = Visibility.Collapsed;
            EyeOutline.Visibility = Visibility.Visible;
            EyeFilled.Visibility = Visibility.Collapsed;
            PasswordBox.Password = PasswordVisible.Text;
        }

        private void ToggleConfirmPasswordVisibilityButton_Checked(object sender, RoutedEventArgs e)
        {
            ConfirmPasswordBox.Visibility = Visibility.Collapsed;
            ConfirmPasswordVisible.Visibility = Visibility.Visible;
            EyeOutline2.Visibility = Visibility.Collapsed;
            EyeFilled2.Visibility = Visibility.Visible;
            ConfirmPasswordVisible.Text = ConfirmPasswordBox.Password;
        }

        private void ToggleConfirmPasswordVisibilityButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ConfirmPasswordBox.Visibility = Visibility.Visible;
            ConfirmPasswordVisible.Visibility = Visibility.Collapsed;
            EyeOutline2.Visibility = Visibility.Visible;
            EyeFilled2.Visibility = Visibility.Collapsed;
            ConfirmPasswordBox.Password = ConfirmPasswordVisible.Text;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (isSyncingPassword) return;
            isSyncingPassword = true;
            PasswordVisible.Text = PasswordBox.Password;
            isSyncingPassword = false;
        }

        private void PasswordVisible_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isSyncingPassword) return;
            isSyncingPassword = true;
            PasswordBox.Password = PasswordVisible.Text;
            isSyncingPassword = false;
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (isSyncingConfirmPassword) return;
            isSyncingConfirmPassword = true;
            ConfirmPasswordVisible.Text = ConfirmPasswordBox.Password;
            isSyncingConfirmPassword = false;
        }

        private void ConfirmPasswordVisible_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isSyncingConfirmPassword) return;
            isSyncingConfirmPassword = true;
            ConfirmPasswordBox.Password = ConfirmPasswordVisible.Text;
            isSyncingConfirmPassword = false;
        }
    }
}