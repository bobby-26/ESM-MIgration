using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewTravelTicketCopySummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {                
                ViewState["TRAVELID"] = null;
                if (Request.QueryString["travelid"] != null)
                {
                    ViewState["TRAVELID"] = Request.QueryString["travelid"].ToString();
                }
                BindTicket();
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindTicket()
    {
        if (ViewState["TRAVELID"] != null)
        {
            DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelQuotationTicketcopySearch(null,General.GetNullableGuid(Request.QueryString["TRAVELID"]),0);
            //DataRow dr = ds.Tables[0].Rows[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                repTicketList.DataSource = ds;
                repTicketList.DataBind();
            }
        }
    }
}
