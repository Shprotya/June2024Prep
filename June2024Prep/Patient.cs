using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace June2024Prep
{
    public class Patient
    {
        public int PatientID { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTime DOB { get; set; }
        public string ContactNumber { get; set; }

        // Navigation property to represent the one-to-many relationship with Appointments
        public virtual List<Appointment> Appointments { get; set; }

    }

    public class Appointment
    {
        public int AppointmentID { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string AppointmentNotes { get; set; }

        // Foreign key to link to the Patient
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }
    }

    public class PatientData : DbContext
    {
        //the name of the database
        public PatientData() : base("OODExam_YelyzavetaKareieva") { }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
