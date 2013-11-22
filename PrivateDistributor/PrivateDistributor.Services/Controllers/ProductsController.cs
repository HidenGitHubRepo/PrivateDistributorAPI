using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeFirst.Model;
using PrivateDistributor.Services.Data;
using PrivateDistributor.Services.Models;

namespace PrivateDistributor.Services.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        private string returnImgExtentionIfIsImage(string uriToImage)//, string mimeType)
        {
            uriToImage = uriToImage.ToLower();
            string imgExtention = "";
            if (uriToImage.EndsWith(".jpeg"))
            {
                imgExtention = ".jpeg";
            }
            else if (uriToImage.EndsWith(".jpg"))
            {
                imgExtention = ".jpg";
            }
            else if (uriToImage.EndsWith(".png"))
            {
                imgExtention = ".png";
            }
            else
            {
                throw new ArgumentException("Image must be with \".jpeg\",\".png/\" or \".jpg\" extention.");
            }

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriToImage);
            //request.Method = "HEAD";

            //try
            //{
            //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //    if (response.StatusCode != HttpStatusCode.OK)// && response.ContentType == mimeType)
            //    {
            //        throw new ArgumentException("Wrong URL.");
            //    }
            //}
            //catch
            //{
            //    throw new ArgumentException("Wrong URL.");
            //}
            return imgExtention;
        }

        [HttpPost]
        [ActionName("add")]
        public HttpResponseMessage DeleteUser([FromBody] ProductRequestModel productModel)
        {
            var messageResponse = this.TryExecuteOperation<HttpResponseMessage>(() =>
            {
                User user = unitOfWork.userRepository.All().Single(x => x.SessionKey == productModel.SessionKey);
                if (user == null)
                {
                    throw new InvalidOperationException("User has not logged in!");
                }
                if (user.UserType != UserType.Administrator)
                {
                    throw new InvalidOperationException("Only administrators can delete users!");
                }

                string extention = returnImgExtentionIfIsImage(productModel.ImageUrl);
                Product product = ProductRequestModel.FromEntity(productModel);
                product.ImageType = extention;

                try
                {
                    decimal dec = decimal.Parse(productModel.Price);
                    product.Price = dec;
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("The Price must be a number.");
                }
                unitOfWork.productRepository.Add(product);
                
                var response = ProductResponseModel.FromEntity(product);

                return this.Request.CreateResponse(HttpStatusCode.Created, response);
            });

            return messageResponse;
        }
    }
}
