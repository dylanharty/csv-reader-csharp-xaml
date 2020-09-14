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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Path = System.IO.Path;
using System.Reflection;
using SaitProjectCSVDesktop.Classes;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.CodeDom.Compiler;

namespace SaitProjectCSVDesktop
{
    /// <summary>
    /// This program takes in a CSV file of SAIT programs by title, priority and status and then allows the user to add, remove and sort by priority and status.
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Program> programSourceFilter;
        List<Program> programsSource;
        ICollectionView ListViewDraw;
        string csvPath;
        public MainWindow()
        {
            InitializeComponent();
            // Get the sait programs CSV file from the assembly folder
            csvPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\sait-programs.csv");
            programsSource = LoadCSV(csvPath);
            // Bind data to ListView for Programs
            ListViewPrograms.ItemsSource = programsSource;
            ListViewDraw = CollectionViewSource.GetDefaultView(programsSource);
        }

        // Load the CSV file into memory and bind to class of Program
        public List<Program> LoadCSV(string fileName)
        {
            List<Program> programs = new List<Program>();

            // Read lines one by one for enhanced performance (big files would freeze if not line by line)
            foreach (var line in File.ReadLines(fileName))
            {
                string[] data = line.Split(',');
                programs.Add(new Program(data[0], Convert.ToInt32(data[1]), data[2]));
            }
            return programs;
        }

        //Just normalization 
        public void ResetAndRefresh()
        {
            // Reset Filter Text
            ComboBoxPriority.Text = "";
            ComboBoxStatus.Text = "";

            // Refresh ListViewPrograms, required to re-draw ListView
            ListViewDraw.Refresh();
        }

        private void MenuItem_Remove_Click(object sender, RoutedEventArgs e)
        {
            // Check that a menu item is selected then remove it base on class identifier
            if (ListViewPrograms.SelectedItem != null)
            {
                Program program = ListViewPrograms.SelectedItem as Program;
                ListViewPrograms.ItemsSource = programsSource;
                programsSource.Remove(program);
                ResetAndRefresh();
            }
        }
        private void MenuItem_Add_Click(object sender, RoutedEventArgs e)
        {
            // Opens a new window to add a new value
            Forms.AddProgramForm programForm = new Forms.AddProgramForm();
            programForm.Show();
        }

        public void AddProgram(Program program)
        {
            // Make sure ItemsSource is not on a filter
            ListViewPrograms.ItemsSource = programsSource;
            programsSource.Add(program);
            ResetAndRefresh();
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ComboBoxPriority.Text) && string.IsNullOrEmpty(ComboBoxStatus.Text))
            {
                // Do not filter if both ComboBoxStatus and ComboBoxPriority are not set
                ListViewPrograms.ItemsSource = programsSource;
                return;
            }
            else
            {
                // Filtering. Basic way to filter lists based on two values.

                // General filtered items
                List<Program> filterList = new List<Program>();

                // Filtered items by priority
                List<Program> priorityList = new List<Program>();
                if (!string.IsNullOrEmpty(ComboBoxPriority.Text))
                {
                    priorityList = programsSource.FindAll(x => x.Priority == Convert.ToInt32(ComboBoxPriority.Text));
                }

                // Filtered items by status
                List<Program> statusList = new List<Program>();
                if (!string.IsNullOrEmpty(ComboBoxStatus.Text))
                {
                    statusList = programsSource.FindAll(x => x.Status.Equals(ComboBoxStatus.Text));
                }

                // If priorityList has results and not statusList, display priority filter
                if (priorityList.Count() > 0 && statusList.Count() <= 0)
                {
                    filterList = priorityList;
                }
                else if (statusList.Count() > 0 && priorityList.Count() <= 0)
                {   // If statusList has results and not priorityList, display statusList filter
                    filterList = statusList;
                }
                else
                {
                    // Both have results so display both, if they intersect in the Lists
                    filterList = priorityList.Intersect(statusList).ToList();
                }
                programSourceFilter = filterList;
            }
            ListViewPrograms.ItemsSource = programSourceFilter;
            ListViewDraw.Refresh(); // refresh the listview, required to re-draw ListView
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Write CSV back to file
            using (var csvWritePath = File.CreateText(csvPath))
            {
                // Foreach program write a line of CSV
                foreach (var program in programsSource)
                {
                    csvWritePath.Write(program.Title);
                    csvWritePath.Write(',');
                    csvWritePath.Write(program.Priority);
                    csvWritePath.Write(',');
                    csvWritePath.Write(program.Status);
                    csvWritePath.WriteLine();
                }
            }
            MessageBox.Show("File Saved!");

        }
    }
}
