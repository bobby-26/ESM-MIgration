using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class CrewOffshoreWageTrackerTotal : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["vesselid"] != null)
                {
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
                }
                if (Request.QueryString["date"] != null)
                {
                    ViewState["FROMDATE"] = Request.QueryString["date"].Split('~')[0];
                    ViewState["TODATE"] = Request.QueryString["date"].Split('~')[1];
                }
            }
          
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
    public void BindData()
    {
        DataSet ds;
        if (ViewState["VESSELID"] != null && ViewState["FROMDATE"] != null && ViewState["TODATE"] != null)
        {
            ds = PhoenixCrewOffshoreWageTracker.wagetrackertotaldetail(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                    , General.GetNullableDateTime(ViewState["FROMDATE"].ToString())
                    , General.GetNullableDateTime(ViewState["TODATE"].ToString()));

            gvWageTracker.DataSource = ds.Tables[0];

        }

    }
  
    protected void gvWageTracker_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
