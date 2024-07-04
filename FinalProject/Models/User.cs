using FinalProject.Enums;
using System.Security.Cryptography;
using System.Text;

namespace FinalProject.Model
{
    public class User:Entity<Guid>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public short? CityId { get; set; }
        public string ProfilePicture { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] Salt { get; set; }
       


        public byte UserRoleId { get; set; }
        public byte UserStatusId { get; set; }
        public DateTime? LastLogged { get; set; }
        public string IP { get; set; }
        public bool? Gender { get; set; }
        public bool EmailConfirmed { get; set; }



        public UserRole UserRole { get; set; }
        public ICollection<UserWishList> UserWishlists { get; set; }
        public City City { get; set; }


        public User(string name,
                    string surname,
                    string email)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Phone = " ";
            ProfilePicture = " ";
            Created = DateTime.Now;
            Updated = DateTime.Now;
            UserStatusId = (byte)UserStatusEnum.Active;
        }

        

        public void UpdatePassword(string newPassword)
        {
            var salt = Guid.NewGuid();
            newPassword += salt.ToString();
            using (SHA256 sha256 = SHA256.Create())
            {
                var buffer = Encoding.UTF8.GetBytes(newPassword);
                var hash = sha256.ComputeHash(buffer);
                Salt = Encoding.UTF8.GetBytes(salt.ToString());
                PasswordHash = hash;
            }

        }

        public void AddPassword(string password)
        {
            var salt = Guid.NewGuid();
            password += salt.ToString();
            using (SHA256 sha256 = SHA256.Create())
            {
                var buffer = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(buffer);
                Salt = Encoding.UTF8.GetBytes(salt.ToString());
                PasswordHash = hash;
            }
        }
        public bool CheckPassword(string password)
        {
            var salt = Encoding.UTF8.GetString(Salt);
            password += salt;
            using (SHA256 sha256 = SHA256.Create())
            {
                var buffer = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(buffer);

                if (hash.SequenceEqual(PasswordHash)) return true;
                else
                    return false;
            }

        }

        public void AddUserRole()
        {
            UserRoleId = (byte)UserRoleEnum.User;

        }
    }
}

