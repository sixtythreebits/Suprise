using System;
using System.Collections.Generic;
using System.Linq;
using Core.DB;
using Newtonsoft.Json;
using RestSharp;
using Core.Utilities;
using MailChimp;
using MailChimp.Helper;
using System.Configuration;

namespace Core
{
    public class MailChimpRepository
    {
        public bool IsError { set; get; }

        public void AddEmailToTheList(string Email)
        {
            try
            {
                var mc = new MailChimpManager(ConfigurationManager.AppSettings["MailChampApiKey"]);

                //  Create the email parameter
                var email = new EmailParameter()
                {
                    Email = Email
                };

                EmailParameter results = mc.Subscribe(ConfigurationManager.AppSettings["MailChampListID"], email);
            }
            catch (Exception ex)
            {
                IsError = true;
                string.Format("AddEmailToTheList(Email = {0}) - {1}", Email, ex.Message).LogString();
            }
        }
    }

    public class OrdersRepository
    {
        public static List<Order> ListOrders()
        {
            try
            {
                using (var db = ConnectionFactory.GetDBCoreDataContext())
                {
                    return db.List_Orders()
                    .OrderByDescending(o => o.CRTime)
                    .Select(o => new Order
                    {
                        ID = o.OrderID,
                        IsLovePack = o.IsLovePack,
                        Price = o.Price,
                        Recipient = o.Recipient,
                        Address = o.Address,
                        ZipCode = o.ZipCode,
                        Note = o.Note,
                        IsPaid = o.IsPaid,
                        IsDelivered = o.IsDelivered,
                        CRTime = o.CRTime

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                string.Format("ListOrders() - {0}", ex.Message).LogString();
                return null;
            }
        }

        public int? TSP_Orders(byte? iud = null, int? ID = null, bool? IsLovePack = null, decimal? Price = null, string Recipient = null, string Address = null, string ZipCode = null, string Note = null, bool? IsPaid = null, bool? IsDelivered = null)
        {
            try
            {
                using (var db = ConnectionFactory.GetDBCoreDataContext())
                {
                    db.tsp_Orders(iud, ref ID, IsLovePack, Price, Recipient, Address, ZipCode, Note, IsPaid, IsDelivered);
                    return ID;
                }
            }
            catch (Exception ex)
            {
                string.Format("TSP_Orders(iud = {0}, ID = {1}, IsLovePack = {2}, Price = {3}, Recipient = {4}, Address = {5}, ZipCode = {6}, Note = {7}, IsPaid = {8}, IsDelivered = {9}) - {10}", iud, ID, IsLovePack, Price, Recipient, Address, ZipCode, Note, IsPaid, IsDelivered, ex.Message).LogString();
                return null;
            }
        }
    }

    public class Order
    {
        public int? ID{ set; get; } 
        public bool IsLovePack{ set; get; }         
        public decimal? Price{ set; get; } 
        public string Recipient{ set; get; } 
        public string Address{ set; get; } 
        public string ZipCode{ set; get; } 
        public string Note{ set; get; } 
        public bool IsPaid{ set; get; } 
        public bool IsDelivered{ set; get; } 
        public DateTime? CRTime{ set; get; }
    }


    public class PayPalService
    {
        #region Properties
        static string ClientID = ConfigurationManager.AppSettings["PaypalClientID"];
        static string Secret = ConfigurationManager.AppSettings["PaypalSecret"];
        static string Url = ConfigurationManager.AppSettings["PaypalExpressUrl"];
        
        static string AccessToken = null;
        static DateTime AccessTokenExpiration = DateTime.Now;
        #endregion Properties

        #region Methods
        public static CreatePaymentReturnResult CreatePayment(string Caption, string Price, string ReturnUrl, string CancelUrl, int? OrderID)
        {
            // Access Token initialization
            InitAccessToken();

            // Request url initialization
            var client = new RestClient();
            client.BaseUrl = new Uri(Url + "payments/payment");
            var request = new RestRequest();

            // Request header initialization
            request.AddHeader("Authorization", "Bearer " + AccessToken);
            request.AddHeader("content-type", "application/json");

            // Request post parameters initiazation
            request.Method = Method.POST;
            var Json = new
            {
                intent = "sale",
                redirect_urls = new
                {
                    return_url = ReturnUrl,
                    cancel_url = CancelUrl
                },
                payer = new
                {
                    payment_method = "paypal"
                },
                transactions = new object[] 
                { 
                    new
                    {
                        amount = new 
                        {
                            total = Price,
                            currency = ConfigurationManager.AppSettings["PaypalCurrency"],
                        },
                        description = string.Format("{0} - ${1}",Caption,Price)
                    }
                }
            };
            var JsonString = JsonConvert.SerializeObject(Json, Formatting.None);
            request.AddParameter("application/json;", JsonString, ParameterType.RequestBody);

            // Sending request to paypal
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                //Returning paypal response
                var R = JsonConvert.DeserializeObject<CreatePaymentResponse>(response.Content);
                return new CreatePaymentReturnResult
                {
                    ID = R.ID,
                    CustomerAuthenticationRedirecUrl = R.Links.Where(l => l.rel == "approval_url").Select(l => l.Href).FirstOrDefault()
                };
            }
            else
            {
                return null;
            }
        }

        public static ExecutePaymentResponse ExecutePayment(string PaymentID, string PayerID, int? OrderID)
        {
            // Access Token initialization
            InitAccessToken();

            // Request url initialization
            var client = new RestClient();
            client.BaseUrl = new Uri(string.Format("{0}payments/payment/{1}/execute", Url, PaymentID));
            var request = new RestRequest();

            // Request header initialization
            request.AddHeader("Authorization", "Bearer " + AccessToken);
            request.AddHeader("content-type", "application/json");

            // Request post parameters initiazation
            request.Method = Method.POST;
            var Json = new
            {
                payer_id = PayerID
            };
            var JsonString = JsonConvert.SerializeObject(Json, Formatting.None);
            request.AddParameter("application/json;", JsonString, ParameterType.RequestBody);

            // Sending request to paypal
            var response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
                (response.StatusDescription + Environment.NewLine + response.ErrorMessage + Environment.NewLine + response.Content).LogString();
            }
            else
            {
                //Returning paypal response
                return JsonConvert.DeserializeObject<ExecutePaymentResponse>(response.Content);
            }
        }

        static void InitAccessToken()
        {
            // If previosly grabbed Access Token is not expired there is no need to request it, it's better to use existing one.
            if (string.IsNullOrWhiteSpace(AccessToken) || AccessTokenExpiration <= DateTime.Now)
            {
                // Request url initialization
                var client = new RestClient();
                client.BaseUrl = new Uri(Url + "oauth2/token");
                client.Authenticator = new HttpBasicAuthenticator(ClientID, Secret);
                var request = new RestRequest();

                // Request header initialization
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Accept-Language", "en_US");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("grant_type", "client_credentials");

                // Request post parameters initiazation
                request.Method = Method.POST;

                // Sending request to paypal
                var response = client.Execute(request);

                //Getting paypal response
                var R = JsonConvert.DeserializeObject<PaypalAuthenticationResponse>(response.Content);
                AccessToken = R.access_token;
                AccessTokenExpiration = DateTime.Now.AddSeconds(R.expires_in);
            }
        }
        #endregion Methods
    }

    #region Response Classes
    public class CreatePaymentResponse
    {
        #region Properties
        [JsonProperty("id")]
        public string ID { set; get; }

        [JsonProperty("links")]
        public Link[] Links { set; get; }
        #endregion Properties
    }

    public class CreatePaymentReturnResult
    {
        public string ID { set; get; }
        public string CustomerAuthenticationRedirecUrl { set; get; }
    }

    public class ExecutePaymentResponse
    {
        #region Properties
        public int? LocalTransactionID { set; get; }
        [JsonProperty("id")]
        public string ID { set; get; }

        [JsonProperty("state")]
        public string State { set; get; }

        [JsonProperty("create_time")]
        public DateTime CreateTime { set; get; }

        [JsonProperty("update_time")]
        public DateTime UpdateTime { set; get; }

        [JsonProperty("intent")]
        public string Intent { set; get; }

        [JsonProperty("payer")]
        public Payer Payer { set; get; }

        [JsonProperty("transactions")]
        public Transaction[] Transactions { set; get; }

        [JsonProperty("links")]
        public Link[] Links { set; get; }
        #endregion Properties
    }

    public class Link
    {
        #region Properties
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("rel")]
        public string rel { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }
        #endregion Properties
    }

    public class Payer
    {
        #region Properties
        [JsonProperty("payment_method")]
        public string PaymentMethod { set; get; }

        [JsonProperty("payer_info")]
        public PayerInfo PayerInfo { set; get; }
        #endregion Properties
    }

    public class PayerInfo
    {
        #region Properties
        [JsonProperty("email")]
        public string Email { set; get; }

        [JsonProperty("first_name")]
        public string FirstName { set; get; }

        [JsonProperty("last_name")]
        public string LastName { set; get; }

        [JsonProperty("payer_id")]
        public string PayerID { set; get; }

        [JsonProperty("shipping_address")]
        public ShippingAddress ShippingAddress { set; get; }
        #endregion Properties
    }

    public class PaypalAuthenticationResponse
    {
        #region Properties
        public string scope { set; get; }
        public string access_token { set; get; }
        public string token_type { set; get; }
        public string app_id { set; get; }
        public int expires_in { set; get; }
        #endregion Properties
    }

    public class ShippingAddress
    {
        #region Properties
        [JsonProperty("line1")]
        public string Line1 { set; get; }

        [JsonProperty("line2")]
        public string Line2 { set; get; }

        [JsonProperty("city")]
        public string City { set; get; }

        [JsonProperty("state")]
        public string State { set; get; }

        [JsonProperty("postal_code")]
        public string PostalCode { set; get; }

        [JsonProperty("country_code")]
        public string CountryCode { set; get; }

        [JsonProperty("recipient_name")]
        public string RecipientName { set; get; }
        #endregion Properties
    }

    public class Transaction
    {
        [JsonProperty("amount")]
        public TransactionAmount Amount { set; get; }

        [JsonProperty("description")]
        public string Description { set; get; }

        [JsonProperty("related_resources")]
        public dynamic[] RelatedResources { set; get; }
    }

    public class TransactionAmount
    {
        #region Properties
        [JsonProperty("total")]
        public decimal Total { set; get; }

        [JsonProperty("currency")]
        public string Currency { set; get; }

        [JsonProperty("details")]
        public TransactionAmountDetails Details { set; get; }
        #endregion Properties
    }

    public class TransactionAmountDetails
    {
        #region Properties
        [JsonProperty("subtotal")]
        public decimal Subtotal { set; get; }
        #endregion Properties
    }
    #endregion Response Classes
}
