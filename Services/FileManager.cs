using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Services
{
    // Static class for file management operations
    // Static = only one copy exists, no need to create objects
    public static class FileManager
    {
        // File names as constants
        private const string DOCTORS_FILE = "doctors.txt";
        private const string PATIENTS_FILE = "patients.txt";
        private const string ADMINS_FILE = "administrators.txt";
        private const string APPOINTMENTS_FILE = "appointments.txt";

        // Generic delegate example - can work with any type T
        public delegate T DataProcessor<T>(string line);

        // Extension method example - adds functionality to string type
        public static bool IsValidEmail(this string email)
        {
            return !string.IsNullOrEmpty(email) && email.Contains("@") && email.Contains(".");
        }

        // Generic method with delegate - processes any type of file
        public static List<T> ProcessFile<T>(string fileName, DataProcessor<T> processor)
        {
            var items = new List<T>();

            try
            {
                if (File.Exists(fileName))
                {
                    var lines = File.ReadAllLines(fileName);

                    // Using anonymous method with LINQ
                    var validLines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();

                    foreach (var line in validLines)
                    {
                        try
                        {
                            var item = processor(line);  // Call the delegate
                            if (item != null)
                                items.Add(item);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing line '{line}': {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file {fileName}: {ex.Message}");
            }

            return items;
        }

        // Load doctors using generic method and delegate
        public static List<Doctor> LoadDoctors()
        {
            return ProcessFile(DOCTORS_FILE, line =>
            {
                var parts = line.Split('|');
                if (parts.Length >= 7)
                {
                    return new Doctor
                    {
                        ID = parts[0],
                        Password = parts[1],
                        FirstName = parts[2],
                        LastName = parts[3],
                        Email = parts[4],
                        Phone = parts[5],
                        Specialization = parts[6]
                    };
                }
                return null;
            });
        }

        // Save doctors to file
        public static void SaveDoctors(List<Doctor> doctors)
        {
            try
            {
                // Using LINQ to convert objects to strings
                var lines = doctors.Select(d => $"{d.ID}|{d.Password}|{d.FirstName}|{d.LastName}|{d.Email}|{d.Phone}|{d.Specialization}");
                File.WriteAllLines(DOCTORS_FILE, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving doctors: {ex.Message}");
                throw; // Re-throw to let caller handle it
            }
        }

        // Load patients using generic method and delegate
        public static List<Patient> LoadPatients()
        {
            return ProcessFile(PATIENTS_FILE, line =>
            {
                var parts = line.Split('|');
                if (parts.Length >= 8)
                {
                    if (DateTime.TryParse(parts[7], out DateTime dateOfBirth))
                    {
                        return new Patient
                        {
                            ID = parts[0],
                            Password = parts[1],
                            FirstName = parts[2],
                            LastName = parts[3],
                            Email = parts[4],
                            Phone = parts[5],
                            Address = parts[6],
                            DateOfBirth = dateOfBirth
                        };
                    }
                }
                return null;
            });
        }

        // Save patients to file
        public static void SavePatients(List<Patient> patients)
        {
            try
            {
                var lines = patients.Select(p => $"{p.ID}|{p.Password}|{p.FirstName}|{p.LastName}|{p.Email}|{p.Phone}|{p.Address}|{p.DateOfBirth:yyyy-MM-dd}");
                File.WriteAllLines(PATIENTS_FILE, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving patients: {ex.Message}");
                throw;
            }
        }

        // Load administrators using generic method and delegate
        public static List<Administrator> LoadAdministrators()
        {
            return ProcessFile(ADMINS_FILE, line =>
            {
                var parts = line.Split('|');
                if (parts.Length >= 6)
                {
                    return new Administrator
                    {
                        ID = parts[0],
                        Password = parts[1],
                        FirstName = parts[2],
                        LastName = parts[3],
                        Email = parts[4],
                        Phone = parts[5]
                    };
                }
                return null;
            });
        }

        // Save administrators to file
        public static void SaveAdministrators(List<Administrator> administrators)
        {
            try
            {
                var lines = administrators.Select(a => $"{a.ID}|{a.Password}|{a.FirstName}|{a.LastName}|{a.Email}|{a.Phone}");
                File.WriteAllLines(ADMINS_FILE, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving administrators: {ex.Message}");
                throw;
            }
        }

        // Load appointments using generic method and delegate
        public static List<Appointment> LoadAppointments()
        {
            return ProcessFile(APPOINTMENTS_FILE, line =>
            {
                var parts = line.Split('|');
                if (parts.Length >= 5)
                {
                    if (DateTime.TryParse(parts[3], out DateTime dateTime))
                    {
                        return new Appointment
                        {
                            ID = parts[0],
                            PatientID = parts[1],
                            DoctorID = parts[2],
                            DateTime = dateTime,
                            Description = parts[4]
                        };
                    }
                }
                return null;
            });
        }

        // Save appointments to file
        public static void SaveAppointments(List<Appointment> appointments)
        {
            try
            {
                var lines = appointments.Select(a => $"{a.ID}|{a.PatientID}|{a.DoctorID}|{a.DateTime:yyyy-MM-dd HH:mm}|{a.Description}");
                File.WriteAllLines(APPOINTMENTS_FILE, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving appointments: {ex.Message}");
                throw;
            }
        }

        // Initialize sample data if files don't exist
        public static void InitializeSampleData()
        {
            // Create sample administrators if file doesn't exist
            if (!File.Exists(ADMINS_FILE))
            {
                var admins = new List<Administrator>
                {
                    new Administrator("admin", "admin123", "John", "Smith", "admin@hospital.com", "555-0001")
                };
                SaveAdministrators(admins);
                Console.WriteLine("Sample administrator data created.");
            }

            // Create sample doctors if file doesn't exist
            if (!File.Exists(DOCTORS_FILE))
            {
                var doctors = new List<Doctor>
                {
                    new Doctor("doc001", "doc123", "Sarah", "Johnson", "sarah.johnson@hospital.com", "555-0101", "Cardiology"),
                    new Doctor("doc002", "doc456", "Michael", "Brown", "michael.brown@hospital.com", "555-0102", "Neurology"),
                    new Doctor("doc003", "doc789", "Emily", "Davis", "emily.davis@hospital.com", "555-0103", "Pediatrics")
                };
                SaveDoctors(doctors);
                Console.WriteLine("Sample doctor data created.");
            }

            // Create sample patients if file doesn't exist
            if (!File.Exists(PATIENTS_FILE))
            {
                var patients = new List<Patient>
                {
                    new Patient("pat001", "pat123", "Alice", "Wilson", "alice.wilson@email.com", "555-0201", "123 Main St", new DateTime(1985, 3, 15)),
                    new Patient("pat002", "pat456", "Bob", "Taylor", "bob.taylor@email.com", "555-0202", "456 Oak Ave", new DateTime(1990, 7, 22)),
                    new Patient("pat003", "pat789", "Carol", "Anderson", "carol.anderson@email.com", "555-0203", "789 Pine Rd", new DateTime(1975, 11, 8))
                };
                SavePatients(patients);
                Console.WriteLine("Sample patient data created.");
            }
            // Create sample receptionist if file doesn't exist
            if (!File.Exists("receptionists.txt"))
            {
                var receptionists = new List<string> 
                { 
                    "rec001|rec123|Jane|Doe|jane.doe@hospital.com|555-0301" 
                };
                File.WriteAllLines("receptionists.txt", receptionists);
                Console.WriteLine("Sample receptionist data created.");
            }
    
            // Create empty appointments file if it doesn't exist
            if (!File.Exists(APPOINTMENTS_FILE))
            {
                File.WriteAllText(APPOINTMENTS_FILE, "");
                Console.WriteLine("Appointments file created.");
            }
        }

        // Method overloading example - different ways to search users
        public static List<BaseUser> SearchUsers(string searchTerm)
        {
            var results = new List<BaseUser>();

            // Search in doctors
            var doctors = LoadDoctors().Where(d =>
                d.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                d.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                d.ID.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            results.AddRange(doctors);

            // Search in patients  
            var patients = LoadPatients().Where(p =>
                p.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.ID.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            results.AddRange(patients);

            return results;
        }

        // Method overloading - search with user type filter
        public static List<BaseUser> SearchUsers(string searchTerm, Type userType)
        {
            var allResults = SearchUsers(searchTerm);
            return allResults.Where(u => u.GetType() == userType).ToList();
        }

        // Garbage collection demonstration
        public static void ForceGarbageCollection()
        {
            // Force garbage collection - demonstration purpose only
            // In real applications, let .NET handle garbage collection automatically
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Console.WriteLine("Garbage collection performed.");
        }

        // Utility method to check if all required files exist
        public static bool AllFilesExist()
        {
            return File.Exists(DOCTORS_FILE) &&
                   File.Exists(PATIENTS_FILE) &&
                   File.Exists(ADMINS_FILE) &&
                   File.Exists(APPOINTMENTS_FILE);
        }

        // Utility method to get file info
        public static void DisplayFileInfo()
        {
            string[] files = { DOCTORS_FILE, PATIENTS_FILE, ADMINS_FILE, APPOINTMENTS_FILE };

            Console.WriteLine("File Information:");
            Console.WriteLine("─────────────────────────────────────");

            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    Console.WriteLine($"{file}: {fileInfo.Length} bytes, modified {fileInfo.LastWriteTime}");
                }
                else
                {
                    Console.WriteLine($"{file}: Not found");
                }
            }
        }
    }
}