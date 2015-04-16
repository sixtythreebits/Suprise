using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Suprise.Models
{
    public class IndexModel
    {
        public CheckoutModel CheckoutModelProperty { set; get; }
        public EmailSubscriptionModel EmailSubscriptionModelProperty { set; get; }
    }

    public class CheckoutModel : FormModelBase
    {
        public string Product { set; get; }
        public string Recipient { set; get; }
        public string Address { set; get; }
        public string Zip { set; get; }
        public string Note { set; get; }        
    }

    public class EmailSubscriptionModel : FormModelBase
    {
        public string Email { set; get; }
    }

    #region Base
    public class FormErrors
    {
        #region Properties
        public List<string> Messages = new List<string>();
        public List<string> Fields = new List<string>();
        public int Count
        {
            get { return Messages.Count > 0 ? Messages.Count : Fields.Count; }
        }
        #endregion Properties

        public void Add(string Field, string Message)
        {
            if (!string.IsNullOrWhiteSpace(Field))
            {
                Fields.Add(Field);
            }
            if (!string.IsNullOrWhiteSpace(Message))
            {
                Messages.Add(Message);
            }
        }

        public bool ContainsField(string SearchField)
        {
            return Fields.Contains(SearchField);
        }

        public void Clear()
        {
            Messages.Clear();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Messages.ForEach(m =>
            {
                sb.AppendFormat("&bull; {0} <br/>", m);
            });
            return sb.ToString();
        }
    }

    public class FormModelBase
    {
        #region Properties
        public bool? IsError { set; get; }
        public FormErrors Errors { set; get; }

        public string ErrorMessage { set; get; }
        #endregion Properties

        #region Constructors
        public FormModelBase()
        {
            Errors = new FormErrors();
        }
        #endregion Constructors
    }
    #endregion Base
}