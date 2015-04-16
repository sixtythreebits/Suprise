using Core;
using Core.Properties;
using Core.Utilities;
using Suprise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Suprise.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            if (Session["paypal-success"] != null)
            {
                Session.Remove("paypal-success");
                ViewBag.PaypalSuccess = true;
            }

            if (Session["email-success"] != null)
            {
                Session.Remove("email-success");
                ViewBag.EmailSuccess = true;
            }
            return View();
        }
        #region Paypal
        [Route("checkout/")]
        [HttpPost]
        public ActionResult PaypalInitPayment(CheckoutModel CheckoutModelProperty)
        {
            var Model = new IndexModel() { CheckoutModelProperty = CheckoutModelProperty };
            if(
                string.IsNullOrWhiteSpace(Model.CheckoutModelProperty.Product) ||
                string.IsNullOrWhiteSpace(Model.CheckoutModelProperty.Recipient) ||
                string.IsNullOrWhiteSpace(Model.CheckoutModelProperty.Address) ||
                string.IsNullOrWhiteSpace(Model.CheckoutModelProperty.Zip) ||
                string.IsNullOrWhiteSpace(Model.CheckoutModelProperty.Note)
            )
            {
                Model.CheckoutModelProperty.IsError = true;
                Model.CheckoutModelProperty.ErrorMessage = Resources.RequiredAllFields;
                return View("Index", Model);
            }
            else
            {
                decimal Price = (decimal)8.70;
                var R = new OrdersRepository();
                var OrderID = R.TSP_Orders(
                    iud: 0,
                    ID: null,
                    IsLovePack: Model.CheckoutModelProperty.Product == "angel" ? true : false,
                    Price:Price,
                    Recipient: Model.CheckoutModelProperty.Recipient,
                    Address: Model.CheckoutModelProperty.Address,
                    ZipCode: Model.CheckoutModelProperty.Zip,
                    Note: Model.CheckoutModelProperty.Note,
                    IsPaid: false,
                    IsDelivered: false
                );

                if (OrderID > 0)
                {
                    try
                    {
                        Session["OrderID"] = OrderID;
                        var Result = PayPalService.CreatePayment(string.Format("SupriseLa Order ID: {0}", OrderID), string.Format("{0:n2}", Price), AppSettings.WebsiteHttpFullPath + "paypal-success/", AppSettings.WebsiteHttpFullPath, OrderID);
                        if (Result != null && !string.IsNullOrWhiteSpace(Result.CustomerAuthenticationRedirecUrl))
                        {
                            return Redirect(Result.CustomerAuthenticationRedirecUrl);
                        }
                        else
                        {
                            R.TSP_Orders(iud: 2, ID: OrderID);
                            Model.CheckoutModelProperty.IsError = true;
                            Model.CheckoutModelProperty.ErrorMessage = Resources.Abort;
                            return View("Index", Model);
                        }
                    }
                    catch(Exception ex)
                    {
                        Model.CheckoutModelProperty.IsError = true;
                        Model.CheckoutModelProperty.ErrorMessage = Resources.Abort;
                        ex.Message.LogString();
                    }

                }
                else
                {
                    Model.CheckoutModelProperty.IsError = true;
                    Model.CheckoutModelProperty.ErrorMessage = Resources.Abort;
                    return View("Index", Model);
                }
                return Redirect("/success/");
            }                                
        }

        [Route("paypal-success/")]
        public ActionResult PaypalPaymentSuccess()
        {
            var OrderID = (int?)Session["OrderID"];
            var Result = PayPalService.ExecutePayment(Request.QueryString["paymentId"], Request.QueryString["PayerID"], OrderID);
            if (Result == null)
            {
                return Redirect("/error/");
            }
            else
            {
                var R = new OrdersRepository();
                R.TSP_Orders(
                    iud: 1,
                    ID: OrderID,
                    IsPaid: true
                );
                Session["paypal-success"] = true;
                return Redirect("/");
            }
        }
        #endregion Paypal

        #region MailChamp
        [Route("email-subscription/")]
        [HttpPost]
        public ActionResult EmailSubscription(EmailSubscriptionModel EmailSubscriptionModelProperty)
        {
            var Model = new IndexModel() { EmailSubscriptionModelProperty = EmailSubscriptionModelProperty };

            if (!string.IsNullOrWhiteSpace(Model.EmailSubscriptionModelProperty.Email) && Regex.IsMatch(Model.EmailSubscriptionModelProperty.Email, Resources.RegexEmail))
            {

                var R = new MailChimpRepository();
                R.AddEmailToTheList(Model.EmailSubscriptionModelProperty.Email);
                if (R.IsError)
                {
                    Model.EmailSubscriptionModelProperty.IsError = true;
                    Model.EmailSubscriptionModelProperty.ErrorMessage = Resources.Abort;                    
                }
                else
                {
                    Session["email-success"] = true;
                    return Redirect("/");
                }                
            }
            else
            {
                Model.EmailSubscriptionModelProperty.IsError = true;
                Model.EmailSubscriptionModelProperty.ErrorMessage = Resources.RequiredEmail;                
            }
            return View("Index", Model);
        }
        #endregion MailChamp
    }
}