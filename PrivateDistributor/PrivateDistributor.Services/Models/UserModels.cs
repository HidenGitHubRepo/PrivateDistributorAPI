using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CodeFirst.Model;

namespace PrivateDistributor.Services.Models
{
    public class MakeNewUserAuthCodeRequestModel
    {
        [DataMember(Name = "AuthCode")]
        public string AuthCode { get; set; }

        [DataMember(Name = "Email")]
        public string Email { get; set; }

        [DataMember(Name = "Type")]
        public string Type { get; set; }

        [DataMember(Name = "Company")]
        public string Company { get; set; }
    }

    [DataContract]
    public class MakeNewUserAuthCodeResponseModel
    {
        public static Func<NewUserAuthCode, MakeNewUserAuthCodeResponseModel> FromEntity { get; set; }

        [DataMember(Name = "Email")]
        public string Email { get; set; }

        [DataMember(Name = "Type")]
        public string Type { get; set; }

        [DataMember(Name = "Company")]
        public string Company { get; set; }

        [DataMember(Name = "AuthCodeCreator")]
        public string AuthCodeCreator { get; set; }

        [DataMember(Name = "AuthCode")]
        public string AuthCode { get; set; }

        static MakeNewUserAuthCodeResponseModel()
        {
            FromEntity = x => new MakeNewUserAuthCodeResponseModel
            {
                Email = x.Email,
                Company = x.Company.Name,
                Type = x.Type.ToString(),
                AuthCodeCreator = x.AuthCodeCreator.LastName
            };
        }
    }
    
    public class NewUserAuthCodeRequestModel
    {
        [DataMember(Name = "AuthCode")]
        public string AuthCode { get; set; }

        [DataMember(Name = "Email")]
        public string Email { get; set; }

        [DataMember(Name = "Type")]
        public string Type { get; set; }

        [DataMember(Name = "Company")]
        public string Company { get; set; }
    }

    [DataContract]
    public class NewUserAuthCodeResponseModel
    {
        public static Func<NewUserAuthCode, NewUserAuthCodeResponseModel> FromEntity { get; set; }

        [DataMember(Name = "Email")]
        public string Email { get; set; }

        [DataMember(Name = "Type")]
        public string Type { get; set; }

        [DataMember(Name = "Company")]
        public string Company { get; set; }

        [DataMember(Name = "AuthCode")]
        public string AuthCode { get; set; }

        static NewUserAuthCodeResponseModel()
        {
            FromEntity = x => new NewUserAuthCodeResponseModel 
            {
                Email = x.Email,
                Company = x.Company.Name,
                Type = x.Type.ToString(),
                AuthCode = x.AuthCode
            };
        }
    }
    
    public class UserRegisterRequestModel
    {
        public static Func<UserRegisterRequestModel, User> ToEntity { get; set; }

        public static Func<UserRegisterRequestModel, Company> ToEntityCompany { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }

        public string AuthCode { get; set; }

        public string RegistrationAuthCode { get; set; }

        public ICollection<string> Mails { get; set; }

        public ICollection<string> Phones { get; set; }

        public UserType UserType { get; set; }

        public string CompanyName { get; set; }

        public string CompanyDisplayName { get; set; }

        public string Fax { get; set; }

        public string CompanyMoreInformation { get; set; }

        public string CompanyLocation { get; set; }

        public ICollection<string> CompanyMails { get; set; }

        public ICollection<string> CompanyPhones { get; set; }

        static UserRegisterRequestModel()
        {
            ToEntity = x => new User
            {
                Username = x.Username,
                FirstName = x.FirstName,
                SecondName = x.SecondName,
                LastName = x.LastName, 
                AuthCode = x.AuthCode, 
                UserType = x.UserType
                //,
                //Location = x.Location
            };

            ToEntityCompany = x => new Company
            {
                Name = x.CompanyName,
                DisplayName = x.CompanyDisplayName,
                Fax = x.Fax,
                MoreInformation = x.CompanyMoreInformation, 
                Location = x.CompanyLocation, 
                //Mails = x.CompanyMails, 
                //Phones = x.CompanyPhones
            };
        }

        
        //static UserRegisterRequestModel()
        //{
        //    ToEntityCompany = x => new Company
        //    {
        //        Name = x.CompanyName,
        //        DisplayName = x.CompanyDisplayName,
        //        Fax = x.SecondName,
        //        MoreInformation = x.CompanyMoreInformation, 
        //        Location = x.CompanyLocation, 
        //        Mails = x.Mails, 
        //        Phones = x.Phones
        //    };
        //}
    }
    
    [DataContract]
    public class UserRegisterResponseModel
    {
        public static Func<User, string[], string[],CompanyModel, UserRegisterResponseModel> FromEntity { get; set; }

        [DataMember(Name = "SessionKey")]
        public string SessionKey { get; set; }

        [DataMember(Name = "Company")]
        public CompanyModel Company { get; set; }

        //[DataMember(Name = "Username")]
        //public string Username { get; set; }

        [DataMember(Name = "FirstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "SecondName")]
        public string SecondName { get; set; }

        [DataMember(Name = "LastName")]
        public string LastName { get; set; }

        [DataMember(Name = "Mails")]
        public ICollection<string> Mails { get; set; }

        [DataMember(Name = "Phones")]
        public ICollection<string> Phones { get; set; }

        [DataMember(Name = "UserType")]
        public UserType UserType { get; set; }

        static UserRegisterResponseModel()
        {
            FromEntity = (u,m,p,c) => new UserRegisterResponseModel 
            {
                FirstName = u.FirstName, 
                SecondName = u.SecondName, 
                LastName = u.LastName,
                Mails = m, 
                Phones = p,  
                SessionKey = u.SessionKey,
                UserType = u.UserType,
                Company = c
            };
        }
    }
    
    [DataContract]
    public class CompanyModel
    {
        public static Func<Company, string[], string[], CompanyModel> FromEntity { get; set; }

        [DataMember(Name = "Name")]
        public string CompanyName { get; set; }

        [DataMember(Name = "DisplayName")]
        public string CompanyDisplayName { get; set; }

        [DataMember(Name = "Fax")]
        public string Fax { get; set; }

        [DataMember(Name = "MoreInformation")]
        public string CompanyMoreInformation { get; set; }

        [DataMember(Name = "Location")]
        public string CompanyLocation { get; set; }

        [DataMember(Name = "Mails")]
        public ICollection<string> CompanyMails { get; set; }

        [DataMember(Name = "Phones")]
        public ICollection<string> CompanyPhones { get; set; }

        [DataMember(Name = "Type")]
        public CompanyType CompanyType { get; set; }


        static CompanyModel()
        {
            FromEntity = (x,m,p) => new CompanyModel 
            {
                CompanyName = x.Name, 
                CompanyDisplayName = x.DisplayName, 
                Fax = x.Fax, 
                CompanyMoreInformation = x.MoreInformation, 
                CompanyLocation = x.Location, 
                CompanyMails = m, 
                CompanyPhones = p, 
                CompanyType = x.CompanyType, 
            };
        }
    }

    public class UserLoginRequestModel
    {
        public static Func<UserLoginRequestModel, User> ToEntity { get; set; }

        public string Username { get; set; }

        public string AuthCode { get; set; }

        public UserType UserType { get; set; }

        static UserLoginRequestModel()
        {
            ToEntity = x => new User { Username = x.Username, AuthCode = x.AuthCode, UserType = x.UserType };
        }
    }

    [DataContract]
    public class UserLoginResponseModel
    {
        public static Func<User, UserLoginResponseModel> FromEntity { get; set; }

        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "sessionKey")]
        public string SessionKey { get; set; }

        [DataMember(Name = "userType")]
        public UserType UserType { get; set; }

        static UserLoginResponseModel()
        {
            FromEntity = x => new UserLoginResponseModel { DisplayName = x.LastName, SessionKey = x.SessionKey, UserType = x.UserType };
        }
    }

    public class UserLogoutRequestModel
    {
        public string SessionKey { get; set; }
    }

    public class UserModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string DisplayName { get; set; }

        public UserType UserType { get; set; }

        public ICollection<string> Mails { get; set; }

        public ICollection<string> Phones { get; set; }

        public string Location { get; set; }
    }

    public class UserDetailedModel : UserModel
    {
        public ICollection<CarModel> Cars { get; set; }
    }
}