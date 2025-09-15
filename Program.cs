// See https://aka.ms/new-console-template for more information

using System;
using System.Linq;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;

namespace HospitalManagementSystem
{
    // Main Program class with authentication system
    public class Program
    {
        // Main method - entry point of the application
        public static void Main(string[] args)
        {
            try
            {
                // Initialize sample data on first run
                FileManager.InitializeSampleData();

                // Display welcome message
                DisplayWelcomeMessage();

                bool exitProgram = false;
                while (!exitProgram)
                {
                    try
                    {
                        // Authenticate user
                        var user = AuthenticateUser();

                        if (user != null)
                        {
                            // Successfully logged in - show user's menu
                            user.ShowMenu();
                        }
                        else
                        {
                            // Login failed - ask if user wants to try again
                            Console.WriteLine("\nWould you like to try logging in again? (y/n): ");
                            string response = Console.ReadLine();
                            if (response?.ToLower() != "y")
                            {
                                exitProgram = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                    finally
                    {
                        // Demonstrate garbage collection after each session
                        FileManager.ForceGarbageCollection();
                    }
                }

                DisplayExitMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical error: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        // Method to display welcome message
        private static void DisplayWelcomeMessage()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                           ║");
            Console.WriteLine("║             HOSPITAL MANAGEMENT SYSTEM                    ║");
            Console.WriteLine("║                                                           ║");
            Console.WriteLine("║                    Welcome!                               ║");
            Console.WriteLine("║                                                           ║");
            Console.WriteLine("║    This system demonstrates Object-Oriented Programming   ║");
            Console.WriteLine("║    concepts including Inheritance, Polymorphism,          ║");
            Console.WriteLine("║    Method Overloading, and Advanced C# Features.          ║");
            Console.WriteLine("║                                                           ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // Display sample login credentials
            DisplaySampleCredentials();

            Console.WriteLine("Press any key to continue to login...");
            Console.ReadKey();
        }

        // Method to display sample credentials for testing
        private static void DisplaySampleCredentials()
        {
            Console.WriteLine("📋 Sample Login Credentials for Testing:");
            Console.WriteLine("─────────────────────────────────────────────");
            Console.WriteLine("👑 Administrator:");
            Console.WriteLine("   ID: admin       Password: admin123");
            Console.WriteLine();
            Console.WriteLine("🩺 Doctors:");
            Console.WriteLine("   ID: doc001      Password: doc123    (Cardiology)");
            Console.WriteLine("   ID: doc002      Password: doc456    (Neurology)");
            Console.WriteLine("   ID: doc003      Password: doc789    (Pediatrics)");
            Console.WriteLine();
            Console.WriteLine("🏥 Patients:");
            Console.WriteLine("   ID: pat001      Password: pat123    (Alice Wilson)");
            Console.WriteLine("   ID: pat002      Password: pat456    (Bob Taylor)");
            Console.WriteLine("   ID: pat003      Password: pat789    (Carol Anderson)");
            Console.WriteLine();
        }

        // User authentication method
        private static BaseUser AuthenticateUser()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════════╗");
            Console.WriteLine("║             LOGIN SYSTEM              ║");
            Console.WriteLine("╠═══════════════════════════════════════╣");
            Console.WriteLine("║  Please enter your credentials        ║");
            Console.WriteLine("╚═══════════════════════════════════════╝");
            Console.WriteLine();

            // Get user ID
            Console.Write("👤 Enter User ID: ");
            string userId = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userId))
            {
                Console.WriteLine("❌ User ID cannot be empty!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return null;
            }

            // Get password with masking
            Console.Write("🔒 Enter Password: ");
            string password = GetMaskedPassword();

            Console.WriteLine(); // New line after masked input

            try
            {
                // Check administrators first
                var administrators = FileManager.LoadAdministrators();
                var admin = administrators.FirstOrDefault(a =>
                    a.ID.Equals(userId, StringComparison.OrdinalIgnoreCase) &&
                    a.Password == password);

                if (admin != null)
                {
                    DisplayLoginSuccess(admin);
                    return admin;
                }

                // Check doctors
                var doctors = FileManager.LoadDoctors();
                var doctor = doctors.FirstOrDefault(d =>
                    d.ID.Equals(userId, StringComparison.OrdinalIgnoreCase) &&
                    d.Password == password);

                if (doctor != null)
                {
                    DisplayLoginSuccess(doctor);
                    return doctor;
                }

                // Check patients
                var patients = FileManager.LoadPatients();
                var patient = patients.FirstOrDefault(p =>
                    p.ID.Equals(userId, StringComparison.OrdinalIgnoreCase) &&
                    p.Password == password);

                if (patient != null)
                {
                    DisplayLoginSuccess(patient);
                    return patient;
                }
                // Check receptionists
                if (File.Exists("receptionists.txt"))
                {
                    var receptionistLines = File.ReadAllLines("receptionists.txt");
                    foreach (var line in receptionistLines)
                    {
                        var parts = line.Split('|');
                        if (parts.Length >= 6 &&
                            parts[0].Equals(userId, StringComparison.OrdinalIgnoreCase) &&
                            parts[1] == password)
                        {
                            var receptionist = new Receptionist(parts[0], parts[1], parts[2], parts[3], parts[4], parts[5]);
                            DisplayLoginSuccess(receptionist);
                            return receptionist;
                        }
                    }
                }

                // No user found
                DisplayLoginFailure();
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Authentication error: {ex.Message}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return null;
            }
        }

        // Method to get masked password input (shows asterisks)
        private static string GetMaskedPassword()
        {
            string password = "";
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    // Handle backspace - remove last character
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
                else if (keyInfo.Key != ConsoleKey.Enter &&
                         keyInfo.Key != ConsoleKey.Backspace &&
                         keyInfo.KeyChar != '\0')
                {
                    // Add character to password and display asterisk
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
            }
            while (keyInfo.Key != ConsoleKey.Enter);

            return password;
        }

        // Display successful login message
        private static void DisplayLoginSuccess(BaseUser user)
        {
            Console.WriteLine();
            Console.WriteLine("✅ Login Successful!");
            Console.WriteLine($"🎉 Welcome, {user.GetFullName()}!");
            Console.WriteLine();

            // Show current system time
            Console.WriteLine($"📅 Login Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue to your dashboard...");
            Console.ReadKey();
        }

        // Display failed login message
        private static void DisplayLoginFailure()
        {
            Console.WriteLine();
            Console.WriteLine("❌ Login Failed!");
            Console.WriteLine("🚫 Invalid User ID or Password.");
            Console.WriteLine();
            Console.WriteLine("💡 Please check your credentials and try again.");
            Console.WriteLine("   Remember: User IDs are case-insensitive.");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // Display exit message
        private static void DisplayExitMessage()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                           ║");
            Console.WriteLine("║            Thank you for using                            ║");
            Console.WriteLine("║            HOSPITAL MANAGEMENT SYSTEM                     ║");
            Console.WriteLine("║                                                           ║");
            Console.WriteLine("║       This system successfully demonstrated:              ║");
            Console.WriteLine("║       Inheritance (BaseUser → Patient/Doctor/Admin)       ║");
            Console.WriteLine("║       Method Overriding (GetFullName, ShowMenu)           ║");
            Console.WriteLine("║       Method Overloading (Constructors, GetDisplayInfo)   ║");
            Console.WriteLine("║       Extension Methods (IsValidEmail)                    ║");
            Console.WriteLine("║       Generics & Delegates (ProcessFile<T>)               ║");
            Console.WriteLine("║       Exception Handling & File I/O                       ║");
            Console.WriteLine("║       Garbage Collection & Memory Management              ║");
            Console.WriteLine("║                                                           ║");
            Console.WriteLine("║                    Goodbye!                               ║");
            Console.WriteLine("║                                                           ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        // Bonus method: Display system statistics
        public static void DisplaySystemStats()
        {
            try
            {
                var doctors = FileManager.LoadDoctors();
                var patients = FileManager.LoadPatients();
                var appointments = FileManager.LoadAppointments();
                var admins = FileManager.LoadAdministrators();

                Console.WriteLine("System Statistics:");
                Console.WriteLine($"   Doctors: {doctors.Count}");
                Console.WriteLine($"   Patients: {patients.Count}");
                Console.WriteLine($"   Appointments: {appointments.Count}");
                Console.WriteLine($"   Administrators: {admins.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading statistics: {ex.Message}");
            }
        }
    }

    // Bonus: Extension method demonstration for BaseUser
    public static class UserExtensions
    {
        // Extension method to get user type as string
        public static string GetUserType(this BaseUser user)
        {
            return user switch
            {
                Patient => "Patient",
                Doctor => "Doctor",
                Administrator => "Administrator",
                _ => "Unknown"
            };
        }

        // Extension method to check if user has valid contact info
        public static bool HasCompleteContactInfo(this BaseUser user)
        {
            return !string.IsNullOrWhiteSpace(user.Email) &&
                   user.Email.IsValidEmail() &&
                   !string.IsNullOrWhiteSpace(user.Phone);
        }
    }
}