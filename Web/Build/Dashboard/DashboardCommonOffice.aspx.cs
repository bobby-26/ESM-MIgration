using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Dashboard;

public partial class DashboardCommonOffice : PhoenixBasePage
{
    public string vesselposition;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenPick.Attributes.Add("style", "visibility:hidden");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindVesselList();
                SelectedOption();
            }

            GetVesselPositions();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SelectedOption()
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Add("App", "QUAL");
        criteria.Add("Option", "GMP");

        PhoenixDashboardOption.DashboardLastSelected(criteria);
    }

    private void BindVesselList()
    {
        DataSet ds = PhoenixRegistersVessel.ListFleetWiseVessel(null, null, null, null, (ucFleet.SelectedFleet == "" || ucFleet.SelectedFleet == "Dummy") ? null : ucFleet.SelectedFleet);
        ddlVessel.DataSource = ds;
        ddlVessel.DataTextField = "FLDVESSELNAME";
        ddlVessel.DataValueField = "FLDVESSELID";
        ddlVessel.DataBind();
        ddlVessel.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    private void GetVesselPositions()
    {
        DataSet ds = PhoenixCommonDashboard.DashboardVesselSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ucFleet.SelectedFleet), General.GetNullableInteger(ddlVessel.SelectedValue));

        String Locations = "[";

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {

            DataRow dr = ds.Tables[0].Rows[i];
            //bypass empty rows 
            if (dr["FLDDECIMALLAT"].ToString().Trim().Length == 0)
                continue;
            if (dr["FLDDECIMALLONG"].ToString().Trim().Length == 0)
                continue;
            Locations += "['" + dr["FLDVESSELNAME"].ToString() + "','" + dr["FLDIMONUMBER"].ToString() + "'," + dr["FLDDECIMALLAT"].ToString() + "," + dr["FLDDECIMALLONG"].ToString() + ",'" + dr["FLDDETAILS"] + "','" + dr["FLDVESSELCODE"] + "'],";

        }
        vesselposition = Locations.TrimEnd(',') + "]";

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", "InitializeMap();", true);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }

    protected void ucFleet_Changed(object sender, EventArgs e)
    {
        BindVesselList();
        GetVesselPositions();
    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        GetVesselPositions();
    }

    protected void SetNavication(object sender, EventArgs e)
    {
        try
        {
            if (rblNavication.SelectedValue == "1")
                Response.Redirect("DashboardCommonOffice.aspx", false);
            else
                Response.Redirect("DashboardCommonOfficeVesselPosition.aspx", false);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
