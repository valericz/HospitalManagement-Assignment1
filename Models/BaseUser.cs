using System;

namespace HospitalManagementSystem.Models
{
    // Abstract base class - cannot be instantiated directly, only inherited
    public abstract class BaseUser
    {
        // Properties - common attributes for all users
        public string ID { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // Constructor overloading examples - different parameter combinations

        // Default constructor (no parameters)
        public BaseUser()
        {
            ID = "";
            Password = "";
            FirstName = "";
            LastName = "";
            Email = "";
            Phone = "";
        }

        // Basic info constructor
        public BaseUser(string id, string password, string firstName, string lastName)
        {
            ID = id;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = "";
            Phone = "";
        }

        // Full constructor with all information
        public BaseUser(string id, string password, string firstName, string lastName, string email, string phone)
        {
            ID = id;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
        }

        // Virtual method - can be overridden by child classes
        public virtual string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        // Method overloading example - same method name, different parameters
        public virtual string GetDisplayInfo()
        {
            return $"ID: {ID}, Name: {GetFullName()}";
        }

        public virtual string GetDisplayInfo(bool includeContact)
        {
            if (includeContact)
                return $"ID: {ID}, Name: {GetFullName()}, Email: {Email}, Phone: {Phone}";
            return GetDisplayInfo();
        }

        // Abstract method - child classes MUST implement this method
        public abstract void ShowMenu();

        // Override ToString method from Object class
        public override string ToString()
        {
            return GetDisplayInfo(true);
        }
    }
}