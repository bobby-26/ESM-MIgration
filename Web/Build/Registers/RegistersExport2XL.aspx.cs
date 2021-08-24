using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Export2XL;

public partial class RegistersExport2XL : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["visaid"] != null && Request.QueryString["visaid"].ToString() != "")
        {
            string visaid = Request.QueryString["visaid"].ToString();
            PhoenixCrew2XL.Export2XLCountryVisaOpen(int.Parse(visaid));
         
        }
        if (Request.QueryString["examid"] != null && Request.QueryString["examid"].ToString() != "")
        {
            string examid = Request.QueryString["examid"].ToString();
            if (examid != null && examid != "")
            {
                PhoenixCrewOffshore2XL.Export2XLTrainingQuestion(General.GetNullableGuid(examid));
            }
        }

        
        //ClientScript.RegisterStartupScript(GetType(), "SetFocusScript", "<Script>self.close();</Script>");
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", "<Script>self.close();</Script>");
    }
}
