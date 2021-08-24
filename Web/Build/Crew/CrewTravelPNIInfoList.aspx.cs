using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using System.Collections.Specialized;
using Telerik.Web.UI;


public partial class Crew_CrewTravelPNIInfoList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["agentinvoiceid"] = "";
            if (Request.QueryString["agentinvoiceid"] != null && Request.QueryString["agentinvoiceid"] != null)
                ViewState["agentinvoiceid"] = Request.QueryString["agentinvoiceid"].ToString();

            BindData();
           
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    protected void BindData()
    {
        DataTable dt = PhoenixCrewTravelInvoice.CrewTravelInvoiceList(General.GetNullableGuid(ViewState["agentinvoiceid"].ToString()));
        if(dt.Rows.Count>0)
        {
            lblmeicalcaseid.Text = dt.Rows[0]["FLDREFERENCENO"].ToString();
            lblempname.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
        }
      
    }
}