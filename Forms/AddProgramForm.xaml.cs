using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SaitProjectCSVDesktop.Classes;

namespace SaitProjectCSVDesktop.Forms
{
    /// <summary>
    /// Form for adding new data to CSV
    /// </summary>
    public partial class AddProgramForm : Window
    {
        public AddProgramForm()
        {
            InitializeComponent();
        }

        // Add user input of program to MainWindow data source
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            // Basic user input checking
            if (string.IsNullOrEmpty(TextBoxTitle.Text))
            {
                MessageBox.Show("Please add title for Program.");
                return;
            }
            if (string.IsNullOrEmpty(ComboBoxPriority.Text))
            {
                MessageBox.Show("Please add priority for Program.");
                return;
            }

            if (string.IsNullOrEmpty(ComboBoxStatus.Text))
            {
                MessageBox.Show("Please add status for Program.");
                return;
            }

            // Get the current MainWindow instance
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            // Create a new program Object
            Program program = new Program(TextBoxTitle.Text.Trim(), Convert.ToInt32(ComboBoxPriority.Text.Trim()), ComboBoxStatus.Text.Trim());
            mainWindow.AddProgram(program);
            // Close this window
            this.Close();
        }
    }
}
