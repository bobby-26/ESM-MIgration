using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_UserControlAudit : System.Web.UI.UserControl
{
    string _dtkey = Guid.NewGuid().ToString();
    string _shortcode = "";

    NameValueCollection nvc = new NameValueCollection();

    protected void Page_Load(object sender, EventArgs e)
    {
        string js = "javascript:parent.Openpopup('AuditTrail','','../Common/CommonPhoenixAuditChanges.aspx?dtkey="+ _dtkey + "&shortcode=" + _shortcode + CreateQueryString() + "');return false;";
        cmdAuditTrail.Attributes.Clear();
        cmdAuditTrail.Attributes.Add("onclick", js);
    }

    public string dtkey
    {
        get { return _dtkey;  }
        set { _dtkey = value;  }
    }

    public string shortcode
    {
        get { return _shortcode; }
        set { _shortcode = value; }
    }

    public void AddQueryString(string name, string value)
    {
        nvc.Add(name, value);
    }

    public void ClearQueryString()
    {
        nvc.Clear();
    }

    private string CreateQueryString()
    {
        string _s = "";
        foreach (string s in nvc.AllKeys)
        {
            if (s.ToUpper().Equals("DTKEY") || s.ToUpper().Equals("SHORTCODE"))
                continue;
            _s = _s + "&" + s + "=" + nvc[s];
        }
        return _s;
    }
}
