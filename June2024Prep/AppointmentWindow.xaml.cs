using System;
using System.Windows;

namespace June2024Prep
{
    /// <summary>
    /// Appointment dialog window – used for both adding and editing appointments.
    /// </summary>
    public partial class AppointmentWindow : Window
    {
        private readonly PatientData _db = new PatientData();
        private readonly Appointment _appointmentToEdit;
        private readonly int _patientId;
        private readonly bool _isEditMode;

        // Constructor for adding a new appointment to the given patient
        public AppointmentWindow(int patientId)
        {
            InitializeComponent();
            _patientId = patientId;
            _isEditMode = false;

            // In add mode the Update button is disabled
            btnUpdate.IsEnabled = false;
        }

        // Constructor for editing an existing appointment
        public AppointmentWindow(Appointment appointment)
        {
            InitializeComponent();
            _appointmentToEdit = appointment;
            _isEditMode = true;

            // Pre-populate fields with the existing appointment data
            dpAppointmentDate.SelectedDate = appointment.AppointmentTime.Date;
            tpAppointmentTime.SelectedTime = appointment.AppointmentTime;
            tbxNotes.Text = appointment.AppointmentNotes;

            // In edit mode the Add button is disabled
            btnAdd.IsEnabled = false;
        }

        // Q1(k) – Save a new appointment to the database
        private void AddAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs()) return;

            var appointment = new Appointment
            {
                PatientID = _patientId,
                AppointmentTime = BuildDateTime(),
                AppointmentNotes = tbxNotes.Text.Trim()
            };

            _db.Appointments.Add(appointment);
            _db.SaveChanges();

            DialogResult = true;
            Close();
        }

        // Q1(m) – Update an existing appointment in the database
        private void UpdateAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs()) return;

            var appt = _db.Appointments.Find(_appointmentToEdit.AppointmentID);
            if (appt == null) return;

            appt.AppointmentTime = BuildDateTime();
            appt.AppointmentNotes = tbxNotes.Text.Trim();
            _db.SaveChanges();

            DialogResult = true;
            Close();
        }

        // Combine the selected date and time into a single DateTime value
        private DateTime BuildDateTime()
        {
            var date = dpAppointmentDate.SelectedDate.Value.Date;
            var time = tpAppointmentTime.SelectedTime ?? DateTime.Today;
            return date.Add(time.TimeOfDay);
        }

        private bool ValidateInputs()
        {
            if (dpAppointmentDate.SelectedDate == null)
            {
                MessageBox.Show("Please select an appointment date.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
    }
}
