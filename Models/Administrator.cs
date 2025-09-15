using System;
using System.Collections.Generic;
using System.Linq;
using HospitalManagementSystem.Services;

namespace HospitalManagementSystem.Models
{
    // Administrator class inherits from BaseUser
    public class Administrator : BaseUser
    {
        // Constructor overloading examples

        // Default constructor
        public Administrator() : base() { }

        // Constructor with basic info
        public Administrator(string id, string password, string firstName, string lastName)
            : base(id, password, firstName, lastName)
        {
        }

        // Full constructor
        public Administrator(string id, string password, string firstName, string lastName,
                           string email, string phone)
            : base(id, password, firstName, lastName, email, phone)
        {
        }

        // Method overriding - customize how administrator name is displayed
        public override string GetFullName()
        {
            return $"Admin: {FirstName} {LastName}";
        }

        // Abstract method implementation - administrator's menu system
        public override void ShowMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("╔═══════════════════════════════════════╗");
                Console.WriteLine("║         ADMINISTRATOR MENU            ║");
                Console.WriteLine("╠═══════════════════════════════════════╣");
                Console.WriteLine("║ 1. List All Doctors                   ║");
                Console.WriteLine("║ 2. List All Patients                  ║");
                Console.WriteLine("║ 3. Search Doctor                      ║");
                Console.WriteLine("║ 4. Search Patient                     ║");
                Console.WriteLine("║ 5. Add New Doctor                     ║");
                Console.WriteLine("║ 6. Add New Patient                    ║");
                Console.WriteLine("║ 7. Logout                             ║");
                Console.WriteLine("║ 8. Exit                               ║");
                Console.WriteLine("╚═══════════════════════════════════════╝");
                Console.Write("Please select an option (1-8): ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ListAllDoctors();
                        break;
                    case "2":
                        ListAllPatients();
                        break;
                    case "3":
                        SearchDoctor();
                        break;
                    case "4":
                        SearchPatient();
                        break;
                    case "5":
                        AddNewDoctor();
                        break;
                    case "6":
                        AddNewPatient();
                        break;
                    case "7":
                        return; // Logout
                    case "8":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Private method to list all doctors
        private void ListAllDoctors()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("            ALL DOCTORS                ");
            Console.WriteLine("═══════════════════════════════════════");

            try
            {
                var doctors = FileManager.LoadDoctors();
                if (doctors.Any())
                {
                    foreach (var doctor in doctors)
                    {
                        Console.WriteLine(doctor.GetDisplayInfo(true));
                        Console.WriteLine($"Specialization: {doctor.Specialization}");
                        Console.WriteLine("─────────────────────────────────────");
                    }
                }
                else
                {
                    Console.WriteLine("No doctors found in the system.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading doctors: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        // Private method to list all patients
        private void ListAllPatients()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("            ALL PATIENTS               ");
            Console.WriteLine("═══════════════════════════════════════");

            try
            {
                var patients = FileManager.LoadPatients();
                if (patients.Any())
                {
                    foreach (var patient in patients)
                    {
                        Console.WriteLine(patient.GetDisplayInfo(true));
                        Console.WriteLine($"Address: {patient.Address}");
                        Console.WriteLine($"Date of Birth: {patient.DateOfBirth:yyyy-MM-dd}");
                        Console.WriteLine("─────────────────────────────────────");
                    }
                }
                else
                {
                    Console.WriteLine("No patients found in the system.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading patients: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        // Private method to search for a doctor
        private void SearchDoctor()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("          SEARCH DOCTOR                ");
            Console.WriteLine("═══════════════════════════════════════");

            Console.Write("Enter Doctor ID: ");
            string searchId = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(searchId))
            {
                Console.WriteLine("Please enter a valid Doctor ID.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                var doctors = FileManager.LoadDoctors();
                var doctor = doctors.FirstOrDefault(d => d.ID.Equals(searchId, StringComparison.OrdinalIgnoreCase));

                if (doctor != null)
                {
                    Console.WriteLine("\nDoctor Found:");
                    Console.WriteLine("─────────────────────────────────────");
                    Console.WriteLine(doctor.GetDisplayInfo(true));
                    Console.WriteLine($"Specialization: {doctor.Specialization}");
                }
                else
                {
                    Console.WriteLine("\nDoctor not found!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching for doctor: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        // Private method to search for a patient
        private void SearchPatient()
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

        // Private method to add new doctor
        private void AddNewDoctor()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("          ADD NEW DOCTOR               ");
            Console.WriteLine("═══════════════════════════════════════");

            try
            {
                Console.Write("Enter Doctor ID: ");
                string id = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(id))
                {
                    Console.WriteLine("Doctor ID cannot be empty!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                // Check if doctor ID already exists
                var doctors = FileManager.LoadDoctors();
                if (doctors.Any(d => d.ID.Equals(id, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Doctor ID already exists!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Enter Password: ");
                string password = GetMaskedInput();

                Console.Write("\nEnter First Name: ");
                string firstName = Console.ReadLine();

                Console.Write("Enter Last Name: ");
                string lastName = Console.ReadLine();

                Console.Write("Enter Email: ");
                string email = Console.ReadLine();

                Console.Write("Enter Phone: ");
                string phone = Console.ReadLine();

                Console.Write("Enter Specialization: ");
                string specialization = Console.ReadLine();

                var newDoctor = new Doctor(id, password, firstName, lastName, email, phone, specialization);
                doctors.Add(newDoctor);
                FileManager.SaveDoctors(doctors);

                Console.WriteLine("\nDoctor added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding doctor: {ex.Message}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // Private method to add new patient
        private void AddNewPatient()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("         ADD NEW PATIENT               ");
            Console.WriteLine("═══════════════════════════════════════");

            try
            {
                Console.Write("Enter Patient ID: ");
                string id = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(id))
                {
                    Console.WriteLine("Patient ID cannot be empty!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                // Check if patient ID already exists
                var patients = FileManager.LoadPatients();
                if (patients.Any(p => p.ID.Equals(id, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Patient ID already exists!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Enter Password: ");
                string password = GetMaskedInput();

                Console.Write("\nEnter First Name: ");
                string firstName = Console.ReadLine();

                Console.Write("Enter Last Name: ");
                string lastName = Console.ReadLine();

                Console.Write("Enter Email: ");
                string email = Console.ReadLine();

                Console.Write("Enter Phone: ");
                string phone = Console.ReadLine();

                Console.Write("Enter Address: ");
                string address = Console.ReadLine();

                Console.Write("Enter Date of Birth (yyyy-mm-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime dateOfBirth))
                {
                    var newPatient = new Patient(id, password, firstName, lastName, email, phone, address, dateOfBirth);
                    patients.Add(newPatient);
                    FileManager.SavePatients(patients);

                    Console.WriteLine("\nPatient added successfully!");
                }
                else
                {
                    Console.WriteLine("Invalid date format! Please use yyyy-mm-dd format.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding patient: {ex.Message}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // Private method to get masked password input
        private string GetMaskedInput()
        {
            string password = "";
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
                else if (keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Backspace &&
                         keyInfo.KeyChar != '\0')
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
            }
            while (keyInfo.Key != ConsoleKey.Enter);

            return password;
        }
    }
}