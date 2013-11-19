using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeFirst.Model;
using PrivateDistributor.Services.Attributes;
using PrivateDistributor.Services.Data;
using PrivateDistributor.Services.Models;
using PrivateDistributor.Services.Utilities;

namespace PrivateDistributor.Services.Controllers
{
    public class NewUserAuthCodeController : BaseApiController
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();


        [HttpPost]
        [ActionName("NewUserAuthCodeCheck")]
        public HttpResponseMessage IsNewUserAuthCodeАuthentic([FromBody] NewUserAuthCodeRequestModel codeModel)
        {
            var responseMessage = this.TryExecuteOperation(() =>
            {
                if (codeModel.AuthCode.Contains('@'))
                {
                    UserValidator.ValidateEmail(codeModel.AuthCode);

                    var userRepositoryCount =
                        this.unitOfWork.userRepository.All().Count();

                    if (userRepositoryCount == 0)
                    {
                        var firstUser = this.unitOfWork.newUserAuthCodeRepository.All()
                                            .FirstOrDefault();
                        if (firstUser == null)
                        {
                            firstUser = new NewUserAuthCode()
                            {
                                AuthCode = SessionGenerator.GenerateSessionKey(0),
                                Email = codeModel.AuthCode,
                                Company = new Company() { DisplayName = "Administrator", Name = "Administrator" },
                                Type = UserType.Administrator
                            };
                            var asd = firstUser.AuthCode.Length;
                            this.unitOfWork.newUserAuthCodeRepository.Add(firstUser);
                            this.unitOfWork.Save();
                        }

                        return this.Request.CreateResponse(HttpStatusCode.Created,
                            NewUserAuthCodeResponseModel.FromEntity(firstUser));
                    }
                    else
                    {
                        throw new InvalidOperationException("The program has been already initiated!");
                    }
                }

                var doesCodeExist =
                    this.unitOfWork.newUserAuthCodeRepository.All()
                          .FirstOrDefault(
                                          x =>
                                          x.AuthCode.ToLower() == codeModel.AuthCode.ToLower());
                if (doesCodeExist == null)
                {
                    throw new InvalidOperationException("The Authification code is wrong!");
                }
                else if (doesCodeExist.IsUsed == true)
                {
                    throw new InvalidOperationException("The Authification code is already used!");
                }

                return this.Request.CreateResponse(HttpStatusCode.Created,
                    NewUserAuthCodeResponseModel.FromEntity(doesCodeExist));
            });

            return responseMessage;
        }

        [HttpPost]
        [ActionName("MakeNewUserAuthCode")]
        public HttpResponseMessage MakeNewUserAuthCodeАuthentic([FromBody] MakeNewUserAuthCodeRequestModel codeModel)
        {
            var responseMessage = this.TryExecuteOperation(() =>
            {

                User user =
                    this.unitOfWork.userRepository.All().SingleOrDefault(x => x.SessionKey == codeModel.AuthCode);
                if (user == null)
                {
                    throw new ArgumentException("User is missing or not logged in!");
                }

                UserType newUserType;

                switch (codeModel.Type)
                {
                    case "Administrator":
                        newUserType = UserType.Administrator;
                        break;
                    case "Dealer":
                        newUserType = UserType.Dealer;
                        break;
                    case "ClientAdministrator":
                        newUserType =UserType.ClientAdministrator;
                        break;
                    case "Client":
                        newUserType =UserType.Client;
                        break;
                    default:
                        newUserType = UserType.Undefined;
                        break;
                }

                UserValidator.ValidateEmail(codeModel.Email);
                var doesCodeExist =
                        this.unitOfWork.userRepository.All()
                        .FirstOrDefault(
                                        x =>
                                        x.LastName == codeModel.Email);

                if (doesCodeExist != null)
                {
                    throw new InvalidOperationException("User with the same email already exist!");
                }

                Company newUserCompany = new Company();

                if (user.UserType == UserType.Administrator)
                {
                    if (newUserType == UserType.Administrator ||
                        newUserType == UserType.Dealer)
                    {
                        newUserCompany = user.Company;
                    }
                    else if (newUserType != UserType.Undefined)
                    {
                        var isCompanyExist = this.unitOfWork.companyRepository.All()
                              .FirstOrDefault(
                                              x =>
                                              x.Name == codeModel.Company ||
                                              x.DisplayName == codeModel.Company);
                        if (isCompanyExist == null)
                        {
                            if (newUserType == UserType.Client)
                            {
                                throw new ArgumentException("First User of a new company must be Client-Administrator!");
                            }

                            isCompanyExist = new Company()
                            {
                                DisplayName = codeModel.Company,
                                Name = codeModel.Company,
                                Mails = codeModel.Email,
                                CompanyType = CompanyType.Client
                            };
                            this.unitOfWork.companyRepository.Add(isCompanyExist);
                        }
                        newUserCompany = isCompanyExist;
                    }
                    else
                    {
                        throw new ArgumentException("New User type is invalid!");
                    }
                }
                else if (user.UserType == UserType.Dealer)
                {
                    if (newUserType== UserType.Undefined )
                    {
                        throw new ArgumentException("New User type is invalid!");
                    }

                    if (newUserType == UserType.Administrator)
                    {
                        throw new ArgumentException("Only administrators can make new administrator accounts!");
                    }

                    if (codeModel.Company != null && codeModel.Company != "")
                    {
                        var isCompanyExist = this.unitOfWork.companyRepository.All()
                              .FirstOrDefault(
                                              x =>
                                              x.Name == codeModel.Company ||
                                              x.DisplayName == codeModel.Company);
                        if (isCompanyExist == null)
                        {
                            if (newUserType != UserType.ClientAdministrator)
                            {
                                throw new ArgumentException("First User of a new company must be Client-Administrator!");
                            }
                            isCompanyExist = new Company()
                            {
                                DisplayName = codeModel.Company,
                                Name = codeModel.Company,
                                Mails = codeModel.Email,
                                CompanyType = CompanyType.Client
                            };
                            this.unitOfWork.companyRepository.Add(isCompanyExist);
                        }
                        else if (isCompanyExist.CompanyType == CompanyType.Owner)
                        {
                            throw new InvalidOperationException("Only Administrators can make new Dealer accounts!");
                        }
                        else
                        {
                            newUserCompany = isCompanyExist;
                        }
                    }
                    else
                    {
                        throw new ArgumentException("The company is undefined!");
                    }
                }
                else if (user.UserType == UserType.ClientAdministrator &&
                    user.Company.CompanyType != CompanyType.Owner)
                {
                    if (codeModel.Type == "Dealer" || codeModel.Type == "Administrator" )
                    {
                        throw new InvalidOperationException("You can not make Administrator/dealer accounts");
                    }
                    newUserCompany = user.Company;
                }
                else if (user.UserType == UserType.Client)
                {
                    throw new InvalidOperationException("Only administrators, Dealers and Clien-administrators can make new accounts");
                }



                var newUserAuthCode = new NewUserAuthCode()
                {
                    AuthCode = SessionGenerator.GenerateSessionKey(0),
                    Email = codeModel.Email,
                    Company = newUserCompany,
                    Type = newUserType
                };
                this.unitOfWork.newUserAuthCodeRepository.Add(newUserAuthCode);
                this.unitOfWork.Save();


                var response = this.Request.CreateResponse(HttpStatusCode.Created,
                            MakeNewUserAuthCodeResponseModel.FromEntity(newUserAuthCode));

                return this.Request.CreateResponse(HttpStatusCode.Created,
                            MakeNewUserAuthCodeResponseModel.FromEntity(newUserAuthCode));
            });

            return responseMessage;
        }


    }
}
