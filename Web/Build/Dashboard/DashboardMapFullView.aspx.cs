using System;
using System.Web.UI;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;

public partial class Dashboard_DashboardMapFullView : PhoenixBasePage
{
    public string vesselposition;

    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {
        }
        GetVesselPositions();

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
            }
            GetVesselPositions();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void GetVesselPositions()
    {
        DataSet ds = PhoenixCommonDashboard.DashboardVesselSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, null);

        String Locations = "[";

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {

            DataRow dr = ds.Tables[0].Rows[i];
            //bypass empty rows 
            if (dr["FLDDECIMALLAT"].ToString().Trim().Length == 0)
                continue;
            if (dr["FLDDECIMALLONG"].ToString().Trim().Length == 0)
                continue;
            Locations += "['" + dr["FLDVESSELNAME"].ToString() + "','" + dr["FLDIMONUMBER"].ToString() + "'," + dr["FLDDECIMALLAT"].ToString() + "," + dr["FLDDECIMALLONG"].ToString() + ",'" + dr["FLDDETAILS"] + "'],";

        }
        vesselposition = Locations.TrimEnd(',') + "]";

        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", "InitializePopUpMap();", true);
    }
}
