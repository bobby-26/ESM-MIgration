using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionAuditDirectNonConformityMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
	{
        try
		{			
			SessionUtil.PageAccessRights(this.ViewState);            

            if (!IsPostBack)
			{
                VesselConfiguration();

				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;

                ViewState["currenttab"] = null;
                ViewState["Vesselid"] = "";

                if (Request.QueryString["DashboardYN"] != null && Request.QueryString["DashboardYN"].ToString() != "")
                {
                    ViewState["DashboardYN"] = Request.QueryString["DashboardYN"].ToString();
                    ViewState["currenttab"] = "DET";
                }
                else
                    ViewState["DashboardYN"] = "";

                if (Request.QueryString["REVIEWDNC"] != null)
				{
                    Filter.CurrentSelectedDeficiency = Request.QueryString["REVIEWDNC"].ToString();

                    DataSet ds = PhoenixInspectionAuditNonConformity.EditAuditNC(new Guid(Filter.CurrentSelectedDeficiency));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        ViewState["Vesselid"] = dr["FLDVESSELID"].ToString();
                    }
				}
                if (Request.QueryString["currenttab"] != null && Request.QueryString["currenttab"].ToString() != "")
                {
                    ViewState["currenttab"] = Request.QueryString["currenttab"].ToString();
                    SetSelectedTab(ViewState["currenttab"].ToString());
                }
                else
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 1;

                if (ViewState["URL"] == null || ViewState["URL"].ToString() == string.Empty)
                    ViewState["URL"] = "../Inspection/InspectionAuditDirectNonConformity.aspx?REVIEWDNC=" + Filter.CurrentSelectedDeficiency;
				
			}

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            if (ViewState["DashboardYN"].ToString() == "")
            {
                toolbarmain.AddButton("List", "LIST");
            }
            toolbarmain.AddButton("Details", "DETAILS");
            toolbarmain.AddButton("RCA", "MSCAT");
            toolbarmain.AddButton("CAR", "CAR");
            toolbarmain.AddButton("Defect Job", "WORKREQUEST");
            toolbarmain.AddButton("Requisition", "REQUISITION");
            //toolbarmain.AddButton("Defect", "DEFECT");
            MenuDirectNonConformityGeneral.AccessRights = this.ViewState;
            MenuDirectNonConformityGeneral.MenuList = toolbarmain.Show();
        }
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}

    protected void SetSelectedTab(string currenttab)
    {
        if (currenttab != null && currenttab.ToString() != string.Empty)
        {
            if (currenttab == "causeanaysis")
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionDirectNonConformityCauseAnalysis.aspx?REVIEWDNC=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency;
                if (ViewState["DashboardYN"].ToString() != "")
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 2;
                else
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 3;
            }
            else if (currenttab == "workrequest")
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionDirectNCDefectWorkRequest.aspx?REVIEWDNC=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency;
                if (ViewState["DashboardYN"].ToString() != "")
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 3;
                else
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 4;
            }
            else if (currenttab == "requisition")
            {
                ifMoreInfo.Attributes["src"] = null;
                Filter.CurrentAuditMenu = "directnc";
                ViewState["URL"] = "../Inspection/InspectionAuditPurchaseForm.aspx?callfrom=" + Request.QueryString["currenttab"] + "&REVIEWDNC=";
                Response.Redirect(ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency, false);
            }
            else if (currenttab == "list")
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionDeficiencyList.aspx";
                Response.Redirect(ViewState["URL"].ToString(), false);
            }
            else if (currenttab == "rca")
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionDirectNonConformityMSCAT.aspx?REVIEWDNC=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency;
                if (ViewState["DashboardYN"].ToString() != "")
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 1;
                else
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 2;
            }
            else if (currenttab == "DET")
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionDeficiencyEdit.aspx?DEFICIENCYID=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency;
                if (ViewState["DashboardYN"].ToString() != "")
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 0;
                else
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 1;
            }
            else if (currenttab == "Defect")
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionDirectNCDefectJobAdd.aspx?REVIEWDNC=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency;
                if (ViewState["DashboardYN"].ToString() != "")
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 5;
                else
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 6;
            }
        }
    }

	protected void DirectNonConformityGeneral_TabStripCommand(object sender, EventArgs e)
	{
        try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("DETAILS"))
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionDeficiencyEdit.aspx?DEFICIENCYID=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency;
                if (ViewState["DashboardYN"].ToString() != "")
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 0;
                else
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 1;
                ViewState["currenttab"] = "DET";
            }
			if (CommandName.ToUpper().Equals("CAR"))
			{
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionDirectNonConformityCauseAnalysis.aspx?REVIEWDNC=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency;
                if (ViewState["DashboardYN"].ToString() != "")
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 2;
                else
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 3;
                ViewState["currenttab"] = "causeanaysis";
			}			
			if (CommandName.ToUpper().Equals("WORKREQUEST"))
			{
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionDirectNCDefectWorkRequest.aspx?REVIEWDNC=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency;
                if (ViewState["DashboardYN"].ToString() != "")
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 3;
                else
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 4;
                ViewState["currenttab"] = "workrequest";
			}
			if (CommandName.ToUpper().Equals("REQUISITION"))
			{
                ifMoreInfo.Attributes["src"] = null;
                Filter.CurrentAuditMenu = "directnc";
                ViewState["URL"] = "../Inspection/InspectionAuditPurchaseForm.aspx?callfrom=" + ViewState["currenttab"].ToString() + "&REVIEWDNC=";
                Response.Redirect(ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency, false);
			}
            if (CommandName.ToUpper().Equals("LIST"))
            {
                ifMoreInfo.Attributes["src"] = null;
                if ((ViewState["Vesselid"].ToString() == "0") && (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE")))
                {
                    Response.Redirect("../Inspection/InspectionDeficiencyOfficeList.aspx", false);
                }
                else
                {
                    ViewState["URL"] = "../Inspection/InspectionDeficiencyList.aspx";
                    Response.Redirect(ViewState["URL"].ToString(), false);
                }
            }
            if (CommandName.ToUpper().Equals("MSCAT"))
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionDirectNonConformityMSCAT.aspx?REVIEWDNC=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency;
                ViewState["currenttab"] = "rca";
                if (ViewState["DashboardYN"].ToString() != "")
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 1;
                else
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 2;
            }
            if (CommandName.ToUpper().Equals("DEFECT"))
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionDirectNCDefectJobAdd.aspx?REVIEWDNC=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency;
                if (ViewState["DashboardYN"].ToString() != "")
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 5;
                else
                    MenuDirectNonConformityGeneral.SelectedMenuIndex = 6;
                ViewState["currenttab"] = "Defect";
            }
            String script = String.Format("javascript:resize();");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }
}
