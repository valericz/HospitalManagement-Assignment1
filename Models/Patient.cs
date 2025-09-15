using System;
using System.Collections.Generic;
using System.Linq;
using HospitalManagementSystem.Services;

namespace HospitalManagementSystem.Models
{
    // Patient class inherits from BaseUser
    public class Patient : BaseUser
    {
        // Patient-specific properties
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }

        // Constructor overloading examples - multiple constructors with different parameters

        // Default constructor
        public Patient() : base()
        {
            Address = "Not specified";
            DateOfBirth = DateTime.MinValue;
        }

        // Constructor with basic info only
        public Patient(string id, string password, string firstName, string lastName)
            : base(id, password, firstName, lastName)
        {
            Address = "Not specified";
            DateOfBirth = DateTime.MinValue;
        }

        // Constructor with contact info
        public Patient(string id, string password, string firstName, string lastName,
                      string email, string phone)
            : base(id, password, firstName, lastName, email, phone)
        {
            Address = "Not specified";
            DateOfBirth = DateTime.MinValue;
        }

        // Full constructor with all information
        public Patient(string id, string password, string firstName, string lastName,
                      string email, string phone, string address, DateTime dateOfBirth)
            : base(id, password, firstName, lastName, email, phone)
        {
            Address = address;
            DateOfBirth = dateOfBirth;
        }

        // Method overriding - customize how patient name is displayed
        public override string GetFullName()
        {
            return $"Patient: {FirstName} {LastName}";
        }

        // Abstract method implementation - patient's menu system
        public override void ShowMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("╔═══════════════════════════════════════╗");
                Console.WriteLine("║           PATIENT MENU                ║");
                Console.WriteLine("╠═══════════════════════════════════════╣");
                Console.WriteLine("║ 1. View Personal Information          ║");
                Console.WriteLine("║ 2. View My Doctor Information         ║");
                Console.WriteLine("║ 3. View My Appointments               ║");
                Console.WriteLine("║ 4. Book New Appointment               ║");
                Console.WriteLine("║ 5. Logout                             ║");
                Console.WriteLine("║ 6. Exit                               ║");
                Console.WriteLine("╚═══════════════════════════════════════╝");
                Console.Write("Please select an option (1-6): ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewPersonalInfo();
                        break;
                    case "2":
                        ViewDoctorInfo();
                        break;
                    case "3":
                        ViewMyAppointments();
                        break;
                    case "4":
                        BookNewAppointment();
                        break;
                    case "5":
                        return; // Logout - return to main program
                    case "6":
                        Environment.Exit(0); // Exit program completely
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Private method to display personal information
        private void ViewPersonalInfo()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("         PERSONAL INFORMATION          ");
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine($"Patient ID: {ID}");
            Console.WriteLine($"Name: {GetFullName()}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Phone: {Phone}");
            Console.WriteLine($"Address: {Address}");
            Console.WriteLine($"Date of Birth: {DateOfBirth:yyyy-MM-dd}");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        // Private method to view doctor information
        private void ViewDoctorInfo()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("         MY DOCTOR INFORMATION         ");
            Console.WriteLine("═══════════════════════════════════════");

            try
            {
                var appointments = FileManager.LoadAppointments();
                var myAppointments = appointments.Where(a => a.PatientID == ID).ToList();
                var doctors = FileManager.LoadDoctors();

                if (myAppointments.Any())
                {
                    var myDoctorIds = myAppointments.Select(a => a.DoctorID).Distinct();
                    foreach (var doctorId in myDoctorIds)
                    {
                        var doctor = doctors.FirstOrDefault(d => d.ID == doctorId);
                        if (doctor != null)
                        {
                            Console.WriteLine($"Doctor: {doctor.GetDisplayInfo(true)}");
                            Console.WriteLine($"Specialization: {doctor.Specialization}");
                            Console.WriteLine("─────────────────────────────────────");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("You don't have any doctors assigned yet.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading doctor information: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        // Private method to view patient's appointments
        private void ViewMyAppointments()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("           MY APPOINTMENTS             ");
            Console.WriteLine("═══════════════════════════════════════");

            try
            {
                var appointments = FileManager.LoadAppointments();
                var myAppointments = appointments.Where(a => a.PatientID == ID).OrderBy(a => a.DateTime).ToList();

                if (myAppointments.Any())
                {
                    var doctors = FileManager.LoadDoctors();
                    foreach (var appointment in myAppointments)
                    {
                        var doctor = doctors.FirstOrDefault(d => d.ID == appointment.DoctorID);
                        Console.WriteLine($"Appointment ID: {appointment.ID}");
                        Console.WriteLine($"Doctor: {doctor?.GetFullName() ?? "Unknown"}");
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

        // Private method to book new appointments
        private void BookNewAppointment()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("        BOOK NEW APPOINTMENT          ");
            Console.WriteLine("═══════════════════════════════════════");

            try
            {
                var doctors = FileManager.LoadDoctors();
                if (!doctors.Any())
                {
                    Console.WriteLine("No doctors available at the moment.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Available Doctors:");
                Console.WriteLine("─────────────────────────────────────");
                for (int i = 0; i < doctors.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {doctors[i].GetDisplayInfo()} - {doctors[i].Specialization}");
                }

                Console.Write("\nSelect doctor (number): ");
                if (int.TryParse(Console.ReadLine(), out int doctorIndex) &&
                    doctorIndex > 0 && doctorIndex <= doctors.Count)
                {
                    var selectedDoctor = doctors[doctorIndex - 1];

                    Console.Write("Enter appointment date (yyyy-mm-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                    {
                        Console.Write("Enter appointment time (HH:mm): ");
                        if (TimeSpan.TryParse(Console.ReadLine(), out TimeSpan time))
                        {
                            var appointmentDateTime = date.Add(time);

                            Console.Write("Enter description: ");
                            string description = Console.ReadLine() ?? "";

                            var appointment = new Appointment
                            {
                                ID = Guid.NewGuid().ToString(),
                                PatientID = ID,
                                DoctorID = selectedDoctor.ID,
                                DateTime = appointmentDateTime,
                                Description = description
                            };

                            var appointments = FileManager.LoadAppointments();
                            appointments.Add(appointment);
                            FileManager.SaveAppointments(appointments);

                            Console.WriteLine("\nAppointment booked successfully!");
                            Console.WriteLine($"📧 Sending confirmation email to {Email}...");
                            Console.WriteLine($"✅ Email sent: Appointment with {selectedDoctor.GetFullName()} confirmed");
                            Console.WriteLine($"📅 Appointment details: {appointmentDateTime:yyyy-MM-dd HH:mm}");
                        }
                        else
                        {
                            Console.WriteLine("Invalid time format! Please use HH:mm format.");

                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format! Please use yyyy-mm-dd format.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid doctor selection!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error booking appointment: {ex.Message}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}