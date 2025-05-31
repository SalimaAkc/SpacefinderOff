using System;
using System.ComponentModel;

namespace SpacefinderOff.Models
{
    public class User : INotifyPropertyChanged
    {
        private int _userID;
        private string _fullName;
        private string _email;
        private string _phoneNumber;
        private string _password;
        public string Role { get; set; }
        private DateTime _registrationDate;
        private int _totalBookings;
        private DateTime _lastLogin;

        public int UserID
        {
            get => _userID;
            set
            {
                _userID = value;
                OnPropertyChanged(nameof(UserID));
            }
        }

        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
                OnPropertyChanged(nameof(PhoneDisplay));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public DateTime RegistrationDate
        {
            get => _registrationDate;
            set
            {
                _registrationDate = value;
                OnPropertyChanged(nameof(RegistrationDate));
            }
        }

        public int TotalBookings
        {
            get => _totalBookings;
            set
            {
                _totalBookings = value;
                OnPropertyChanged(nameof(TotalBookings));
            }
        }

        public DateTime LastLogin
        {
            get => _lastLogin;
            set
            {
                _lastLogin = value;
                OnPropertyChanged(nameof(LastLogin));
            }
        }

        // Computed properties for backward compatibility and display
        public string DisplayName => FullName ?? "Unknown User";

        public string Status => Role; // For backward compatibility

        public string StatusDisplay => Role ?? "User";

        public string PhoneDisplay => string.IsNullOrWhiteSpace(PhoneNumber) ? "Not provided" : PhoneNumber;

        // For compatibility with some older code that might reference UserName
        public string UserName => Email?.Split('@')[0] ?? DisplayName;

        public bool IsAdmin => string.Equals(Role, "Admin", StringComparison.OrdinalIgnoreCase);

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"{DisplayName} ({Email})";
        }

        public override bool Equals(object obj)
        {
            if (obj is User otherUser)
            {
                return UserID == otherUser.UserID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return UserID.GetHashCode();
        }


    }
}