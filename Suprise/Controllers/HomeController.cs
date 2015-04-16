using Core;
using Core.Utilities;
using Suprise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return View();
        }

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
                            return View("Index", Model);
                        }
                    }
                    catch(Exception ex)
                    {
                        ex.Message.LogString();
                    }

                }
                else
                {
                    Model.CheckoutModelProperty.IsError = true;
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

        [Route("success/")]
        public ActionResult Success()
        {
            return View();
        }

        [Route("error/")]
        public ActionResult Error()
        {
            return View();
        }
	}
}