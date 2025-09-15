using System;
using System.Collections.Generic;
using System.Linq;
using HospitalManagementSystem.Services;

namespace HospitalManagementSystem.Models
{
    // Doctor class inherits from BaseUser
    public class Doctor : BaseUser
    {
        // Doctor-specific property
        public string Specialization { get; set; }

        // Constructor overloading examples

        // Default constructor
        public Doctor() : base()
        {
            Specialization = "General Practice";
        }

        // Constructor with basic info
        public Doctor(string id, string password, string firstName, string lastName)
            : base(id, password, firstName, lastName)
        {
            Specialization = "General Practice";
        }

        // Full constructor
        public Doctor(string id, string password, string firstName, string lastName,
                     string email, string phone, string specialization)
            : base(id, password, firstName, lastName, email, phone)
        {
            Specialization = specialization;
        }

        // Method overriding - customize how doctor name is displayed
        public override string GetFullName()
        {
            return $"Dr. {FirstName} {LastName}";
        }

        // Abstract method implementation - doctor's menu system
        public override void ShowMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("╔═══════════════════════════════════════╗");
                Console.WriteLine("║            DOCTOR MENU                ║");
                Console.WriteLine("╠═══════════════════════════════════════╣");
                Console.WriteLine("║ 1. View Doctor Details                ║");
                Console.WriteLine("║ 2. View My Patients                   ║");
                Console.WriteLine("║ 3. View My Appointments               ║");
                Console.WriteLine("║ 4. Search Specific Patient            ║");
                Console.WriteLine("║ 5. View Patient Appointments          ║");
                Console.WriteLine("║ 6. Logout                             ║");
                Console.WriteLine("║ 7. Exit                               ║");
                Console.WriteLine("╚═══════════════════════════════════════╝");
                Console.Write("Please select an option (1-7): ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewDoctorDetails();
                        break;
                    case "2":
                        ViewMyPatients();
                        break;
                    case "3":
                        ViewMyAppointments();
                        break;
                    case "4":
                        SearchSpecificPatient();
                        break;
                    case "5":
                        ViewPatientAppointments();
                        break;
                    case "6":
                        return; // Logout
                    case "7":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Private method to display doctor details
        private void ViewDoctorDetails()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("          DOCTOR DETAILS               ");
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine($"Doctor ID: {ID}");
            Console.WriteLine($"Name: {GetFullName()}");
            Console.WriteLine($"Specialization: {Specialization}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Phone: {Phone}");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        // Private method to view doctor's patients
        private void ViewMyPatients()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("            MY PATIENTS                ");
            Console.WriteLine("═══════════════════════════════════════");

            try
            {
                var appointments = FileManager.LoadAppointments();
                // Find unique patient IDs from my appointments
                var myPatientIds = appointments.Where(a => a.DoctorID == ID)
                                             .Select(a => a.PatientID)
                                             .Distinct()
                                             .ToList();

                if (myPatientIds.Any())
                {
                    var patients = FileManager.LoadPatients();
                    foreach (var patientId in myPatientIds)
                    {
                        var patient = patients.FirstOrDefault(p => p.ID == patientId);
                        if (patient != null)
                        {
                            Console.WriteLine(patient.GetDisplayInfo(true));
                            Console.WriteLine($"Address: {patient.Address}");
                            Console.WriteLine($"Date of Birth: {patient.DateOfBirth:yyyy-MM-dd}");
                            Console.WriteLine("─────────────────────────────────────");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("You don't have any patients yet.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading patients: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        // Private method to view doctor's appointments
        private void ViewMyAppointments()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("           MY APPOINTMENTS             ");
            Console.WriteLine("═══════════════════════════════════════");

            try
            {
                var appointments = FileManager.LoadAppointments();
                var myAppointments = appointments.Where(a => a.DoctorID == ID)
                                               .OrderBy(a => a.DateTime)
                                               .ToList();

                if (myAppointments.Any())
                {
                    var patients = FileManager.LoadPatients();
                    foreach (var appointment in myAppointments)
                    {
                        var patient = patients.FirstOrDefault(p => p.ID == appointment.PatientID);
                        Console.WriteLine($"Appointment ID: {appointment.ID}");
                        Console.WriteLine($"Patient: {patient?.GetFullName() ?? "Unknown"}");
                        Console.WriteLine($"Date & Time: {appointment.DateTime:yyyy-MM-dd HH:mm}");
                        Console.WriteLine($"Description: {appointment.Description}");
                        Console.WriteLine("─────────────────────────────────────");
                    }
                }
                else
                {
                    Console.WriteLine("You don't have any appointments scheduled.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading appointments: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        // Private method to search for specific patient
        private void SearchSpecificPatient()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("         SEARCH PATIENT                ");
            Console.WriteLine("═══════════════════════════════════════");

            Console.Write("Enter Patient ID: ");
            string searchId = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(searchId))
            {
                Console.WriteLine("Please enter a valid Patient ID.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                var patients = FileManager.LoadPatients();
                var patient = patients.FirstOrDefault(p => p.ID.Equals(searchId, StringComparison.OrdinalIgnoreCase));

                if (patient != null)
                {
                    Console.WriteLine("\nPatient Found:");
                    Console.WriteLine("─────────────────────────────────────");
                    Console.WriteLine(patient.GetDisplayInfo(true));
                    Console.WriteLine($"Address: {patient.Address}");
                    Console.WriteLine($"Date of Birth: {patient.DateOfBirth:yyyy-MM-dd}");
                }
                else
                {
                    Console.WriteLine("\nPatient not found!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching for patient: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        // Private method to view appointments with specific patient
        private void ViewPatientAppointments()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("      PATIENT APPOINTMENTS            ");
            Console.WriteLine("═══════════════════════════════════════");

            Console.Write("Enter Patient ID: ");
            string patientId = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(patientId))
            {
                Console.WriteLine("Please enter a valid Patient ID.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                var appointments = FileManager.LoadAppointments();
                var patientAppointments = appointments.Where(a => a.PatientID.Equals(patientId, StringComparison.OrdinalIgnoreCase)
                                                                && a.DoctorID == ID)
                                                     .OrderBy(a => a.DateTime)
                                                     .ToList();

                if (patientAppointments.Any())
                {
                    var patients = FileManager.LoadPatients();
                    var patient = patients.FirstOrDefault(p => p.ID.Equals(patientId, StringComparison.OrdinalIgnoreCase));

                    Console.WriteLine($"\nAppointments with {patient?.GetFullName() ?? patientId}:");
                    Console.WriteLine("─────────────────────────────────────");

                    foreach (var appointment in patientAppointments)
                    {
                        Console.WriteLine($"Appointment ID: {appointment.ID}");
                        Console.WriteLine($"Date & Time: {appointment.DateTime:yyyy-MM-dd HH:mm}");
                        Console.WriteLine($"Description: {appointment.Description}");
                        Console.WriteLine("─────────────────────────────────────");
                    }
                }
                else
                {
                    Console.WriteLine("\nNo appointments found with this patient.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading patient appointments: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}