using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CodeFirst.Model;

namespace PrivateDistributor.Services.Models
{
    public class NewUserAuthCodeRequestModel
    {
        public string AuthCode { get; set; }
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

        static NewUserAuthCodeResponseModel()
        {
            FromEntity = x => new NewUserAuthCodeResponseModel 
            {
                Email = x.Email,
                Company = x.Company.Name,
                Type = x.Type.ToString()
            };
        }
    }
    
    public class UserRegisterRequestModel
    {
        public static Func<UserRegisterRequestModel, User> ToEntity { get; set; }

        public string Username { get; set; }

        public string DisplayName { get; set; }

        public string AuthCode { get; set; }

        public ICollection<string> Mails { get; set; }

        public ICollection<string> Phones { get; set; }

        public string Location { get; set; }

        public UserType UserType { get; set; }

        static UserRegisterRequestModel()
        {
            ToEntity = x => new User
            {
                Username = x.Username, 
                DisplayName = x.DisplayName, 
                AuthCode = x.AuthCode, 
                UserType = x.UserType, 
                Mails = x.Mails, 
                Phones = x.Phones,
                Location = x.Location
            };
        }
    }
    
    [DataContract]
    public class UserRegisterResponseModel
    {
        public static Func<User, UserRegisterResponseModel> FromEntity { get; set; }

        [DataMember(Name = "DisplayName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "SessionKey")]
        public string SessionKey { get; set; }

        static UserRegisterResponseModel()
        {
            FromEntity = x => new UserRegisterResponseModel { DisplayName = x.DisplayName, SessionKey = x.SessionKey, };
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
            FromEntity = x => new UserLoginResponseModel { DisplayName = x.DisplayName, SessionKey = x.SessionKey, UserType = x.UserType };
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