using System;

namespace HospitalManagementSystem.Models
{
    // Appointment class - connects patients and doctors
    public class Appointment
    {
        // Properties to store appointment information
        public string ID { get; set; }           // Unique appointment ID
        public string PatientID { get; set; }    // Which patient
        public string DoctorID { get; set; }     // Which doctor  
        public DateTime DateTime { get; set; }   // When is the appointment
        public string Description { get; set; }  // What is it for

        // Constructor overloading examples

        // Default constructor - used when reading from files
        public Appointment()
        {
            ID = "";
            PatientID = "";
            DoctorID = "";
            DateTime = DateTime.MinValue;
            Description = "";
        }

        // Basic constructor with minimal info
        public Appointment(string patientId, string doctorId, DateTime dateTime)
        {
            ID = Guid.NewGuid().ToString();  // Generate unique ID automatically
            PatientID = patientId;
            DoctorID = doctorId;
            DateTime = dateTime;
            Description = "General consultation";
        }

        // Full constructor with all information
        public Appointment(string id, string patientId, string doctorId, DateTime dateTime, string description)
        {
            ID = id;
            PatientID = patientId;
            DoctorID = doctorId;
            DateTime = dateTime;
            Description = description ?? "No description provided";
        }

        // Method to display appointment information
        public string GetAppointmentInfo()
        {
            return $"Appointment {ID}: Patient {PatientID} with Doctor {DoctorID} on {DateTime:yyyy-MM-dd HH:mm}";
        }

        // Method overloading - different ways to display appointment info
        public string GetAppointmentInfo(bool includeDescription)
        {
            string basicInfo = GetAppointmentInfo();

            if (includeDescription)
            {
                return $"{basicInfo} - {Description}";
            }

            return basicInfo;
        }

        // Method to check if appointment is in the future
        public bool IsFutureAppointment()
        {
            return DateTime > DateTime.Now;
        }

        // Method to check if appointment is today
        public bool IsToday()
        {
            return DateTime.Date == DateTime.Now.Date;
        }

        // Method to get time until appointment
        public TimeSpan GetTimeUntilAppointment()
        {
            return DateTime - DateTime.Now;
        }

        // Override ToString method for easy display
        public override string ToString()
        {
            return GetAppointmentInfo(true);
        }

        // Method to validate appointment data
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(ID) &&
                   !string.IsNullOrWhiteSpace(PatientID) &&
                   !string.IsNullOrWhiteSpace(DoctorID) &&
                   DateTime != DateTime.MinValue &&
                   DateTime > DateTime.Now.AddMinutes(-30); // Allow 30 minutes grace period
        }

        // Static method to create appointment with validation
        public static Appointment CreateAppointment(string patientId, string doctorId, DateTime dateTime, string description = "")
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(patientId))
                throw new ArgumentException("Patient ID cannot be empty");

            if (string.IsNullOrWhiteSpace(doctorId))
                throw new ArgumentException("Doctor ID cannot be empty");

            if (dateTime <= DateTime.Now)
                throw new ArgumentException("Appointment must be in the future");

            return new Appointment(Guid.NewGuid().ToString(), patientId, doctorId, dateTime, description);
        }

        // Method to format appointment for file storage (pipe-delimited)
        public string ToFileFormat()
        {
            return $"{ID}|{PatientID}|{DoctorID}|{DateTime:yyyy-MM-dd HH:mm}|{Description}";
        }

        // Static method to create appointment from file format
        public static Appointment FromFileFormat(string fileData)
        {
            if (string.IsNullOrWhiteSpace(fileData))
                return null;

            var parts = fileData.Split('|');
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
        }
    }
}