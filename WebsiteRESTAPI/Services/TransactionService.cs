using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebsiteRESTAPI.Helper;
using WebsiteRESTAPI.Interface;
using WebsiteRESTAPI.Models;
using WebsiteRESTAPI.ResponseHelper;
using Stripe;
using WebsiteRESTAPI.Entity;
using Transaction = WebsiteRESTAPI.Entity.Transaction;
using TransactionModel = WebsiteRESTAPI.Models.Transaction;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace WebsiteRESTAPI.Services
{
    public class TransactionService : ITransaction
    {
        private readonly IEmailService _emailService;
        public TransactionService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<APIResponseResult<object>> AddPayment(ApplicationDbContext dbContext, Payment payment)
        {
            try
            {

                var options = new PaymentIntentCreateOptions()
                {
                    Amount = payment.Amount,
                    Currency = "EUR",
                    Description = "Pierre Gassin",
                    ReceiptEmail = payment.User.Email,
                    PaymentMethod = payment.Id,
                    Confirm = true,
                    Metadata= payment.Metadata


                };
                // create payment
                var service = new PaymentIntentService();
                var intent = service.Create(options);
                if (intent.Status == "succeeded")
                {

                   List< string> ImageLinks= new List<string>();
                    foreach (var item in payment.Images)
                    {
                        //save transaction in database
                        string SeizesString = "";
                        StringBuilder builder = new StringBuilder();

                        foreach (var item1 in item.Value)
                        {
                            builder.Append(item1);
                            builder.Append(',');

                        }
                        SeizesString = builder.ToString();
                        TransactionModel transaction = new TransactionModel()
                        {
                            ImageId = item.Key.ToString(),
                            Email = payment.User.Email,
                            ImageSeizes = SeizesString,
                            Devis = new JObject(intent)
                        };
                        dbContext.Transactions.Add(transaction);
                        //Get the images links,
                        var myimage = dbContext.Images.Find(item.Key);
                        if (item.Value.Contains("Taille1"))
                        {
                            ImageLinks.Add(myimage.Taille1);
                        }
                        if (item.Value.Contains("Taille2"))
                        {
                            ImageLinks.Add(myimage.Taille2);
                        }
                        if (item.Value.Contains("Taille3"))
                        {
                            ImageLinks.Add(myimage.Taille3);
                        }
                        if (item.Value.Contains("Main"))
                        {
                            ImageLinks.Add(myimage.Main);
                        }

                    }
                    dbContext.SaveChanges();


                    
                    // send email
                    EmailAddress toadress = new EmailAddress()
                    {
                        Address = payment.User.Email,
                        Name = payment.User.FirstName
                    };
                    List<EmailAddress> to = new List<EmailAddress>() { toadress };
                    EmailAddress fromadress = new EmailAddress()
                    {
                        Address = "nammouchidali2@gmail.com",
                        Name = "Dali"
                    };
                    List<EmailAddress> from = new List<EmailAddress>() { fromadress };

                    string seizes = StringExtensions.AppendSeizesList(ImageLinks);
                    string info = JsonConvert.SerializeObject(payment.Metadata);
                    string firstName = payment.User.FirstName;
                    EmailMessage myEmail = new EmailMessage()
                    {
                        Content = StringExtensions.CreateEmailBody(firstName,info,seizes),
                        FromAddresses = from,
                        ToAddresses = to,
                        Subject = "Photos Charges"

                    };
                    _emailService.Send(myEmail);
                    //return response
                    List<Image> images = new List<Image>();
                    foreach (var item in payment.Images)
                    {
                        images.Add(dbContext.Images.Find(item.Key));
                    }
                    Transaction mytransaction = new Transaction()
                    {

                        Images = images,
                        Payment = intent,
                        Status = "confirmed",
                        User = payment.User

                    };
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = true,
                        message = "Payment Succeeded",
                        data = JsonConvert.SerializeObject(mytransaction)
                    };

                    return new APIResponseResult<object>(apiResonse, HttpStatusCode.OK);
                }
                else
                {
                    ApiResonse apiResonse = new ApiResonse()
                    {
                        success = false,
                        message = "Payment Failed",

                    };

                    return new APIResponseResult<object>(apiResonse, HttpStatusCode.BadRequest);
                }




            }
            catch (Exception Ex)
            {
                ApiResonse apiResonse = new ApiResonse()
                {
                    success = false,
                    message = "Payment Failed",

                };

                return new APIResponseResult<object>(apiResonse, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<APIResponseResult<object>> GetTransactionsByAccount(ApplicationDbContext dbContext, string Email)
        {
            try
            {
                if (string.IsNullOrEmpty(Email))
                {
                    ApiResonse resonse = new ApiResonse()
                    {
                        success = false,
                        message = "Email is invalide"
                    };
                    return new APIResponseResult<object>(resonse, HttpStatusCode.BadRequest);
                }
                else
                {
                    var transactions = dbContext.Transactions.Where(x => x.Email == Email);
                    if (transactions.Count() > 0)
                    {
                        List<Transaction> transactions1 = new List<Transaction>();
                        foreach (var item in transactions)
                        {
                            List<Image> images = new List<Image>();
                            List<long> myIds;
                            var stringId = item.ImageId.ToString();
                            images.Add(dbContext.Images.FirstOrDefault(x => x.ImageId.ToString() == item.ImageId));
                            Transaction transaction = new Transaction()
                            {
                                Status = "confirmed",
                                Payment = item.Devis,
                                User = dbContext.Users.FirstOrDefault(x => x.Email == Email),
                                Images = images

                            };
                            transactions1.Add(transaction);
                        }
                        ApiResonse apiResponse = new ApiResonse()
                        {
                            success = true,
                            data = JsonConvert.SerializeObject(transactions1)
                        };
                        return new APIResponseResult<object>(apiResponse, HttpStatusCode.OK);



                    }
                    else
                    {
                        ApiResonse resonse = new ApiResonse()
                        {
                            success = false,
                            message = "There is no transaction found"
                        };
                        return new APIResponseResult<object>(resonse, HttpStatusCode.NotFound);

                    }
                }
            }
            catch (Exception Ex)
            {
                ApiResonse resonse = new ApiResonse()
                {
                    success = false,
                    message = "Something went wrong"
                };
                return new APIResponseResult<object>(resonse, HttpStatusCode.InternalServerError);

            }
        }



    }
}
