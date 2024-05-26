using System;
using System.Diagnostics;
using System.Windows;

namespace PasswordBruteForce
{
    public partial class MainWindow : Window
    {
        private string _encryptedPassword;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
             string Salt = "abc";
        string password = PasswordTextBox.Text;
            if (!string.IsNullOrWhiteSpace(password))
            {
                _encryptedPassword = PasswordManager.EncryptPassword(password, Salt);
                ResultTextBox.Text = "Password encrypted and saved.";
            }
            else
            {
                MessageBox.Show("Please enter a password.");
            }
        }

        private void BruteForceButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_encryptedPassword))
            {
                MessageBox.Show("Please encrypt a password first.");
                return;
            }

            if (!int.TryParse(MaxThreadsTextBox.Text, out int maxThreads))
            {
                MessageBox.Show("Invalid number of threads.");
                return;
            }

            var bruteForceEngine = new BruteForceEngine(_encryptedPassword, PasswordTextBox.Text, maxThreads, OnPasswordFound);
            Stopwatch stopwatch = Stopwatch.StartNew();
            bruteForceEngine.Start();
            stopwatch.Stop();
            
            ElapsedTimeTextBlock.Text = $"Found Salt: {bruteForceEngine.GetSalt()} Time elapsed: {stopwatch.Elapsed}";
        }

        private void OnPasswordFound(string password)
        {
            Dispatcher.Invoke(() => ResultTextBox.Text = $"Password found: {password}");
        }
    }
}
