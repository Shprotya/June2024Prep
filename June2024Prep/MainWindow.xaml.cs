using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace June2024Prep
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Shared DbContext for the lifetime of the window
        private readonly PatientData _db = new PatientData();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Load patients from DB when the window opens
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPatients();
        }

        // Q1(i) – LINQ query: all patients ordered by surname then first name
        private void LoadPatients()
        {
            var patients = _db.Patients
                .OrderBy(p => p.Surname)
                .ThenBy(p => p.FirstName)
                .ToList();

            lbxPatients.ItemsSource = patients;
        }

        // Q1(l) – Show appointments for the selected patient, sorted by date
        private void PatientSelected(object sender, SelectionChangedEventArgs e)
        {
            RefreshAppointments();
        }

        // Reload and display appointments for the currently selected patient
        private void RefreshAppointments()
        {
            lbxAppointments.ItemsSource = null;
            lbxAppointments.Items.Clear();

            var patient = lbxPatients.SelectedItem as Patient;
            if (patient == null) return;

            // Eagerly load appointments for the selected patient
            var loaded = _db.Patients
                .Include(p => p.Appointments)
                .FirstOrDefault(p => p.PatientID == patient.PatientID);

            if (loaded == null) return;

            var appointments = loaded.Appointments
                .OrderBy(a => a.AppointmentTime)
                .ToList();

            if (appointments.Count == 0)
            {
                lbxAppointments.Items.Add("No appointments");
            }
            else
            {
                lbxAppointments.ItemsSource = appointments;
            }
        }

        // Q1(j) – Create a new patient from the form fields and save to DB
        private void AddPatient(object sender, RoutedEventArgs e)
        {
            string firstName = tbxFirstName.Text.Trim();
            string surname = tbxSurname.Text.Trim();
            string phone = tbxPhone.Text.Trim();
            var dob = dpDOB.SelectedDate;

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(surname) ||
                string.IsNullOrEmpty(phone) || dob == null)
            {
                MessageBox.Show("Please fill in all patient fields.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var patient = new Patient
            {
                FirstName = firstName,
                Surname = surname,
                ContactNumber = phone,
                DOB = dob.Value
            };

            _db.Patients.Add(patient);
            _db.SaveChanges();

            // Clear the form fields after saving
            tbxFirstName.Text = string.Empty;
            tbxSurname.Text = string.Empty;
            tbxPhone.Text = string.Empty;
            dpDOB.SelectedDate = null;

            LoadPatients();
        }

        // Q1(k) – Open the appointment window to add a new appointment for the selected patient
        private void AddAppointment(object sender, RoutedEventArgs e)
        {
            var patient = lbxPatients.SelectedItem as Patient;
            if (patient == null)
            {
                MessageBox.Show("Please select a patient first.", "No Patient Selected",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var window = new AppointmentWindow(patient.PatientID) { Owner = this };
            if (window.ShowDialog() == true)
            {
                RefreshAppointments();
            }
        }

        // Q1(m) – Open the appointment window to edit the selected appointment
        private void EditAppointment(object sender, RoutedEventArgs e)
        {
            var appointment = lbxAppointments.SelectedItem as Appointment;
            if (appointment == null)
            {
                MessageBox.Show("Please select an appointment to edit.", "No Appointment Selected",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var window = new AppointmentWindow(appointment) { Owner = this };
            if (window.ShowDialog() == true)
            {
                RefreshAppointments();
            }
        }
    }
}
