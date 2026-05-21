using June2024Prep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PatientData db = new PatientData();
            try
            {
                using (db)
                {
                    Patient p1 = new Patient { FirstName = "John", Surname = "Doe", DOB = new DateTime(1990, 1, 1), ContactNumber = "1234567890" };
                    Patient p2 = new Patient { FirstName = "Jane", Surname = "Smith", DOB = new DateTime(1985, 5, 15), ContactNumber = "0987654321" };
                    Patient p3 = new Patient { FirstName = "Alice", Surname = "Johnson", DOB = new DateTime(1975, 10, 30), ContactNumber = "5555555555" };

                    Appointment a1 = new Appointment { AppointmentTime = new DateTime(2024, 7, 1, 9, 0, 0), AppointmentNotes = "General Checkup", Patient = p1 };
                    Appointment a2 = new Appointment { AppointmentTime = new DateTime(2024, 7, 2, 10, 0, 0), AppointmentNotes = "Dental Cleaning", Patient = p2 };
                    Appointment a3 = new Appointment { AppointmentTime = new DateTime(2024, 7, 3, 11, 0, 0), AppointmentNotes = "Eye Exam", Patient = p3 };
                    Appointment a4 = new Appointment { AppointmentTime = new DateTime(2024, 7, 4, 14, 0, 0), AppointmentNotes = "Physical Therapy", Patient = p1 };

                    Console.WriteLine("Adding patients and appointments to the database...");

                    db.Patients.Add(p1);
                    db.Patients.Add(p2);
                    db.Patients.Add(p3);
                    db.Appointments.Add(a1);
                    db.Appointments.Add(a2);
                    db.Appointments.Add(a3);
                    db.Appointments.Add(a4);

                    Console.WriteLine("Saving changes to the database...");

                    db.SaveChanges();   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
