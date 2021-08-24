using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class InspectionUnsafeActsConditions : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["directincidentid"] != null && Request.QueryString["directincidentid"].ToString() != "")
                    ViewState["directincidentid"] = Request.QueryString["directincidentid"].ToString();
                else
                    ViewState["directincidentid"] = "";

                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();

                if (Request.QueryString["DashboardYN"] != null && Request.QueryString["DashboardYN"].ToString() != "")
                    ViewState["DashboardYN"] = Request.QueryString["DashboardYN"].ToString();
                else
                    ViewState["DashboardYN"] = "";

                ViewState["OfficeDashboard"] = "";

                if (!string.IsNullOrEmpty(Request.QueryString["OfficeDashboard"]))
                {
                    ViewState["OfficeDashboard"] = Request.QueryString["OfficeDashboard"];
                }

                BindInspectionIncident();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (ViewState["DashboardYN"].ToString() == "")
            {
                toolbar.AddButton("List", "LIST");
            }
            toolbar.AddButton("Report", "REPORT");
            toolbar.AddButton("Master's Investigation", "INVESTIGATE");
            MenuInspectionGeneral.AccessRights = this.ViewState;
            MenuInspectionGeneral.MenuList = toolbar.Show();
            MenuInspectionGeneral.SelectedMenuIndex = 1;
            if (ViewState["DashboardYN"].ToString() != "")
            {
                MenuInspectionGeneral.SelectedMenuIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void BindInspectionIncident()
    {
        DataSet ds;

        if (ViewState["directincidentid"] != null && !string.IsNullOrEmpty(ViewState["directincidentid"].ToString()))
        {
            ds = PhoenixInspectionUnsafeActsConditions.DirectIncidentEdit(new Guid(ViewState["directincidentid"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtInvestigationAndEvidence.Text = dr["FLDSUMMARY"].ToString();
                txtVessel.Text = dr["FLDVESSELNAME"].ToString();
                txtLocation.Text = dr["FLDLOCATION"].ToString();
                ucDate.Text = dr["FLDINCIDENTDATE"].ToString();
                txtTimeOfIncident.SelectedDate = Convert.ToDateTime(dr["FLDINCIDENTDATETIME"].ToString());
                txtCategory.Text = dr["FLDICCATEGORYNAME"].ToString();
                txtSubcategory.Text = dr["FLDICSUBCATEGORYNAME"].ToString();
                txtRank.Text = dr["FLDRANK"].ToString();
                txtName.Text = dr["FLDNAME"].ToString();
            }
        }
    }

    protected void MenuInspectionGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("LIST"))
            {
               Response.Redirect("../Inspection/InspectionUnsafeActsConditionsList.aspx", true);
            }
            else if (CommandName.ToUpper().Equals("INVESTIGATE"))
            {
                Response.Redirect("../Inspection/InspectionUnsafeActsConditionsInvestigation.aspx?DashboardYN="+ViewState["DashboardYN"].ToString()+"&directincidentid=" + ViewState["directincidentid"]+ "&OfficeDashboard=" + ViewState["OfficeDashboard"], false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}
