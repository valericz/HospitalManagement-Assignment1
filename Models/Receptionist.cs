using System;
using System.Collections.Generic;
using System.Linq;
using HospitalManagementSystem.Services;

namespace HospitalManagementSystem.Models
{
    // Receptionist class inherits from BaseUser
    public class Receptionist : BaseUser
    {
        // Constructor overloading examples
        public Receptionist() : base() { }

        public Receptionist(string id, string password, string firstName, string lastName)
            : base(id, password, firstName, lastName)
        {
        }

        public Receptionist(string id, string password, string firstName, string lastName,
                           string email, string phone)
            : base(id, password, firstName, lastName, email, phone)
        {
        }

        // Method overriding
        public override string GetFullName()
        {
            return $"Receptionist: {FirstName} {LastName}";
        }

        // Abstract method implementation
        public override void ShowMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("╔═══════════════════════════════════════╗");
                Console.WriteLine("║         RECEPTIONIST MENU             ║");
                Console.WriteLine("╠═══════════════════════════════════════╣");
                Console.WriteLine("║ 1. View Today's Appointments          ║");
                Console.WriteLine("║ 2. View All Doctors                   ║");
                Console.WriteLine("║ 3. View All Patients                  ║");
                Console.WriteLine("║ 4. Logout                             ║");
                Console.WriteLine("║ 5. Exit                               ║");
                Console.WriteLine("╚═══════════════════════════════════════╝");
                Console.Write("Please select an option (1-5): ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewTodaysAppointments();
                        break;
                    case "2":
                        ViewAllDoctors();
                        break;
                    case "3":
                        ViewAllPatients();
                        break;
                    case "4":
                        return; // Logout
                    case "5":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void ViewTodaysAppointments()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("        TODAY'S APPOINTMENTS           ");
            Console.WriteLine("═══════════════════════════════════════");

            try
            {
                var appointments = FileManager.LoadAppointments();
                var todaysAppointments = appointments.Where(a => a.DateTime.Date == DateTime.Today).ToList();

                if (todaysAppointments.Any())
                {
                    foreach (var appointment in todaysAppointments)
                    {
                        Console.WriteLine($"Time: {appointment.DateTime:HH:mm}");
                        Console.WriteLine($"Patient: {appointment.PatientID}");
                        Console.WriteLine($"Doctor: {appointment.DoctorID}");
                        Console.WriteLine($"Description: {appointment.Description}");
                        Console.WriteLine("─────────────────────────────────────");
                    }
                }
                else
                {
                    Console.WriteLine("No appointments scheduled for today.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading appointments: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void ViewAllDoctors()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("            ALL DOCTORS                ");
            Console.WriteLine("═══════════════════════════════════════");

            try
            {
                var doctors = FileManager.LoadDoctors();
                foreach (var doctor in doctors)
                {
                    Console.WriteLine($"ID: {doctor.ID}, Name: {doctor.GetFullName()}");
                    Console.WriteLine($"Specialization: {doctor.Specialization}");
                    Console.WriteLine($"Email: {doctor.Email}, Phone: {doctor.Phone}");
                    Console.WriteLine("─────────────────────────────────────");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading doctors: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void ViewAllPatients()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("            ALL PATIENTS               ");
            Console.WriteLine("═══════════════════════════════════════");

            try
            {
                var patients = FileManager.LoadPatients();
                foreach (var patient in patients)
                {
                    Console.WriteLine($"ID: {patient.ID}, Name: {patient.GetFullName()}");
                    Console.WriteLine($"Email: {patient.Email}, Phone: {patient.Phone}");
                    Console.WriteLine("─────────────────────────────────────");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading patients: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}