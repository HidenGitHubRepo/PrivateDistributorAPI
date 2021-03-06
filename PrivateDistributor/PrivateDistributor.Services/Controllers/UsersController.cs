﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ValueProviders;
using CodeFirst.Model;
using PrivateDistributor.Services.Attributes;
using PrivateDistributor.Services.Data;
using PrivateDistributor.Services.Models;
using PrivateDistributor.Services.Utilities;

namespace PrivateDistributor.Services.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        [HttpGet]
        [ActionName("all")]
        public IEnumerable<UserModel> GetAll(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var messageResponse = this.TryExecuteOperation<IEnumerable<UserModel>>(() =>
            {
                var user = unitOfWork.userRepository.All().Single(x => x.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new InvalidOperationException("User has not logged in!");
                }

                var users = unitOfWork.userRepository.All();
                var userModels = users.Select(x => new UserModel()
                {
                    Id = x.Id,
                    Username = x.Username,
                    DisplayName = x.LastName,
                    //Location = x.Location,
                    //Mails = x.Mails,
                    //Phones = x.Phones,
                    UserType = x.UserType,
                });

                return userModels;
            });

            return messageResponse;
        }

        [HttpGet]
        [ActionName("get-user")]
        public UserDetailedModel GetCarById(int id,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var messageResponse = this.TryExecuteOperation<UserDetailedModel>(() =>
            {
                var user = unitOfWork.userRepository.All().Single(x => x.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new InvalidOperationException("User has not logged in!");
                }

                var selectedUser = unitOfWork.userRepository.All().Single(x => x.Id == id);
                var userModel = new UserDetailedModel()
                {
                    Id = selectedUser.Id,
                    Username = selectedUser.Username,
                    DisplayName = selectedUser.LastName,
                    //Location = selectedUser.Location,
                    //Mails = selectedUser.Mails,
                    //Phones = selectedUser.Phones,
                    UserType = selectedUser.UserType,
                    Cars = (from car in selectedUser.Cars
                            select new CarModel()
                            {
                                Id = car.Id,
                                Engine = car.Engine,
                                Gear = car.Gear,
                                HP = car.HP,
                                ImageUrl = car.ImageUrl,
                                Maker = car.Maker,
                                Model = car.Model,
                                Price = car.Price,
                                ProductionYear = car.ProductionYear
                            }).ToList()
                };

                return userModel;
            });

            return messageResponse;
        }

        [HttpPost]
        [ActionName("login")]
        public HttpResponseMessage Login([FromBody] UserLoginRequestModel userModel)
        {
            var responseMessage = this.TryExecuteOperation(() =>
            {
                var user = this.unitOfWork.userRepository.All()
                    .SingleOrDefault(x => 
                        x.Username == userModel.Username && 
                        x.AuthCode == userModel.AuthCode);
                if (user == null)
                {
                    throw new ArgumentException("User is not registered!");
                }

                if (user.SessionKey == null)
                {
                    user.SessionKey = SessionGenerator.GenerateSessionKey(user.Id);
                    this.unitOfWork.userRepository.Update(user.Id, user);
                }

                var asd = unitOfWork.companyRepository.All().SingleOrDefault(x => x.Id == user.Company.Id);

                var response = this.Request.CreateResponse(HttpStatusCode.OK,
                    UserRegisterResponseModel.FromEntity(   user,
                                                            user.Mails.Select(x => x.Name).ToArray(), 
                                                            user.Phones.Select(x => x.Number).ToArray(),
                                                            CompanyModel.FromEntity(user.Company,
                                                                                    user.Company.Mails.Select(x => x.Name).ToArray(), 
                                                                                    user.Company.Phones.Select(x => x.Number).ToArray())));
                return response;
                //return UserLoginResponseModel.FromEntity(user);
            });

            return responseMessage;
        }

        [HttpPost]
        [ActionName("register")]
        public HttpResponseMessage Register([FromBody] UserRegisterRequestModel userModel)
        {
            var responseMessage = this.TryExecuteOperation(() =>
            {
                UserValidator.ValidateAuthCode(userModel.RegistrationAuthCode);

                var doesCodeExist =
                        this.unitOfWork.newUserAuthCodeRepository.All()
                        .FirstOrDefault(
                                        x =>
                                        x.AuthCode == userModel.RegistrationAuthCode);

                if (doesCodeExist == null)
                {
                    throw new InvalidOperationException("The Authification code is wrong!");
                }
                else if (doesCodeExist.IsUsed == true)
                {
                    throw new InvalidOperationException("The Authification code is already used!");
                }

                User user = UserRegisterRequestModel.ToEntity(userModel);
                user.Username = doesCodeExist.Email;
                user.UserType = doesCodeExist.Type;

                UserValidator.ValidateEmail(user.Username);
                UserValidator.ValidateName(user.FirstName);
                UserValidator.ValidateName(user.SecondName);
                UserValidator.ValidateName(user.LastName);

                //validate all emails (here and in company)

                if (user.UserType == UserType.Administrator ||
                    user.UserType == UserType.Dealer)
                {
                    var isFirstAdminUser = this.unitOfWork.userRepository.All()
                              .FirstOrDefault(
                                              x =>
                                              x.UserType == UserType.Administrator);
                    if (isFirstAdminUser == null &&
                        user.UserType == UserType.Dealer)
                    {
                        throw new InvalidOperationException("First user can not be a Dealer!");
                    }
                    else if (isFirstAdminUser == null)
                    {
                        Company newCompany = UserRegisterRequestModel.ToEntityCompany(userModel);

                        foreach (string phone in userModel.CompanyData.CompanyPhones)
                        {
                            var newPhone = new Phone()
                            {
                                Number = phone
                            };
                            unitOfWork.phoneRepository.Add(newPhone);

                            newCompany.Phones.Add(newPhone);
                        }

                        foreach (string mail in userModel.CompanyData.CompanyMails)
                        {
                            var newEmail = new Email()
                            {
                                Name = mail
                            };
                            unitOfWork.emailRepository.Add(newEmail);

                            newCompany.Mails.Add(newEmail);
                        }
                        //Company newCompany = new Company()
                        //{
                        //    DisplayName = userModel.CompanyDisplayName,
                        //    Name = userModel.CompanyName,
                        //    CompanyType = CompanyType.Owner,
                        //    Fax = userModel.Fax
                        //};
                        unitOfWork.companyRepository.Add(newCompany);
                        user.Company = newCompany;
                    }
                    else
                    {
                        user.Company = isFirstAdminUser.Company;
                    }
                }
                else
                {
                    user.Company = doesCodeExist.Company;
                }

                var doesUserExist =
                    this.unitOfWork.userRepository.All()
                              .FirstOrDefault(
                                              x =>
                                              x.Username.ToLower() == user.Username.ToLower());
                //||
                //x.DisplayName.ToLower() == user.DisplayName.ToLower());
                if (doesUserExist != null)
                {
                    throw new InvalidOperationException("User already exist in the database!");
                }


                foreach (string phone in userModel.Phones)
                {
                    var newPhone = new Phone()
                    {
                        Number = phone
                    };
                    unitOfWork.phoneRepository.Add(newPhone);

                    user.Phones.Add(newPhone);
                }

                foreach (string mail in userModel.Mails)
                {
                    var newEmail = new Email()
                    {
                        Name = mail
                    };
                    unitOfWork.emailRepository.Add(newEmail);

                    user.Mails.Add(newEmail);
                }

                //register all new users as clients
                //user.UserType = UserType.Client;

                this.unitOfWork.userRepository.Add(user);
                user.SessionKey = SessionGenerator.GenerateSessionKey(user.Id);
                this.unitOfWork.userRepository.Update(user.Id, user);

                doesCodeExist.IsUsed = true;
                if (doesCodeExist.NewUser == null)
                {
                    doesCodeExist.NewUser = user;
                }
                //this.unitOfWork.newUserAuthCodeRepository.Update(doesCodeExist.Id, doesCodeExist);


                var response = this.Request.CreateResponse(HttpStatusCode.Created,
                    UserRegisterResponseModel.FromEntity(   user,
                                                            user.Mails.Select(x => x.Name).ToArray(), 
                                                            user.Phones.Select(x => x.Number).ToArray(),
                                                            CompanyModel.FromEntity(user.Company,
                                                                                    user.Company.Mails.Select(x => x.Name).ToArray(), 
                                                                                    user.Company.Phones.Select(x => x.Number).ToArray())));

                return response;
            });

            return responseMessage;
        }

        [HttpPut]
        [ActionName("logout")]
        public HttpResponseMessage Logout([FromBody] UserLogoutRequestModel userModel)
        {
            var messageResponse = this.TryExecuteOperation(() =>
            {
                User user =
                    this.unitOfWork.userRepository.All().SingleOrDefault(x => x.SessionKey == userModel.SessionKey);
                if (user == null)
                {
                    throw new ArgumentException("User is missing or not logged in!");
                }

                user.SessionKey = null;
                this.unitOfWork.userRepository.Update(user.Id, user);

                return this.Request.CreateResponse(HttpStatusCode.OK, user);
            });

            return messageResponse;
        }

        [HttpDelete]
        [ActionName("delete")]
        public HttpResponseMessage DeleteUser(int id,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            var messageResponse = this.TryExecuteOperation<HttpResponseMessage>(() =>
            {
                User user = unitOfWork.userRepository.All().Single(x => x.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new InvalidOperationException("User has not logged in!");
                }
                if (user.UserType != UserType.Administrator)
                {
                    throw new InvalidOperationException("Only administrators can delete users!");
                }

                var userCars = this.unitOfWork.carRepository.All().Where(x => x.Owner.Id == id).ToList();

                for (int i = 0; i < userCars.Count(); i++)
                {
                    unitOfWork.carRepository.Delete(userCars[i].Id);
                }

                unitOfWork.userRepository.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            });

            return messageResponse;
        }
    }
}
