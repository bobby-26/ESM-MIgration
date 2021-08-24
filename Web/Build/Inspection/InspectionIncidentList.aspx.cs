using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class InspectionIncidentList : PhoenixBasePage
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
                ViewState["SECTIONID"] = "";
                ViewState["Vesselid"] = "";

                if (Request.QueryString["DashboardYN"] != null && Request.QueryString["DashboardYN"].ToString() != "")
                {
                    ViewState["DashboardYN"] = Request.QueryString["DashboardYN"].ToString();
                    Filter.CurrentIncidentTab = "INCIDENTDETAILS";
                }
                else
                    ViewState["DashboardYN"] = "";

                if (Request.QueryString["IncidentId"] != null && Request.QueryString["IncidentId"].ToString() != "")
                {                    
                    Filter.CurrentIncidentID = Request.QueryString["IncidentId"];
                }
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (ViewState["DashboardYN"].ToString() == "")
            {
                toolbar.AddButton("List", "LIST");
            }
            toolbar.AddButton("Incident Details", "INCIDENTDETAILS");
            toolbar.AddButton("Consequence", "CONSEQUENCE", CreateConsequenceSubTab(string.Empty, 2, string.Empty));
            toolbar.AddButton("Investigation", "INVESTIGATION", CreateInvestigationSubTab(string.Empty, 3, string.Empty));
            toolbar.AddButton("RCA", "MSCAT");
            toolbar.AddButton("CAR", "CAR");
            toolbar.AddButton("Defect Job", "WORKREQUEST");
            toolbar.AddButton("Requisition", "REQUISITION");
            toolbar.AddButton("Correspondence", "COMPANYRESPONSE");
            toolbar.AddButton("Attachments", "ATTACHMENTS");
            IncidentGeneral.AccessRights = this.ViewState;
            IncidentGeneral.MenuList = toolbar.Show();

            if (Filter.CurrentIncidentID == null)
            {
                if (ViewState["DashboardYN"].ToString() != "")
                    IncidentGeneral.SelectedMenuIndex = 0;
                else
                    IncidentGeneral.SelectedMenuIndex = 1;

                Filter.CurrentIncidentTab = "INCIDENTDETAILS";
            }

            DataSet ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(Filter.CurrentIncidentID));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["Vesselid"] = dr["FLDVESSELID"].ToString();
                Filter.CurrentIncidentVesselID = dr["FLDVESSELID"].ToString();
            }

            if (Filter.CurrentIncidentTab != null && Filter.CurrentIncidentTab == "INCIDENTDETAILS")
            {
                if (ViewState["DashboardYN"].ToString() != "")
                    IncidentGeneral.SelectedMenuIndex = 0;
                else
                    IncidentGeneral.SelectedMenuIndex = 1;

                ViewState["PAGEURL"] = "../Inspection/InspectionIncidentGeneral.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString();
            }
            else
            {
                if (Filter.CurrentIncidentTab.ToUpper().Equals("CONSEQUENCE"))
                {
                    Filter.CurrentIncidentTab = "CONSEQUENCE";
                    ViewState["PAGEURL"] = "../Inspection/InspectionIncidentInjuryList.aspx";
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString();

                    if (ViewState["DashboardYN"].ToString() != "")
                        IncidentGeneral.SelectedMenuIndex = 2;
                    else
                        IncidentGeneral.SelectedMenuIndex = 3;
                }
                if (Filter.CurrentIncidentTab.ToUpper().Equals("MSCAT"))
                {
                    Filter.CurrentIncidentTab = "MSCAT";
                    ViewState["PAGEURL"] = "../Inspection/InspectionIncidentMSCAT.aspx";
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString();
                    if (ViewState["DashboardYN"].ToString() != "")
                        IncidentGeneral.SelectedMenuIndex = 3;
                    else
                        IncidentGeneral.SelectedMenuIndex = 4;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void IncidentGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (Filter.CurrentIncidentID == null)
        {
            ucError.ErrorMessage = "You should select an Incident from the list to edit the details";
            ucError.Visible = true;
            IncidentGeneral.SelectedMenuIndex = 0;
            return;
        }
        else
        {
            if (CommandName.ToUpper().Equals("INCIDENTDETAILS"))
            {
                Filter.CurrentIncidentTab = "INCIDENTDETAILS";
                ViewState["PAGEURL"] = "../Inspection/InspectionIncidentGeneral.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString();

                if (ViewState["DashboardYN"].ToString() != "")
                    IncidentGeneral.SelectedMenuIndex = 0;
                else
                    IncidentGeneral.SelectedMenuIndex = 1;
            }
            else if (CommandName.ToUpper().Equals("CAR"))
            {
                Filter.CurrentIncidentTab = "CAR";
                ViewState["PAGEURL"] = "../Inspection/InspectionIncidentCriticalFactor.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString();
                if (ViewState["DashboardYN"].ToString() != "")
                    IncidentGeneral.SelectedMenuIndex = 4;
                else
                    IncidentGeneral.SelectedMenuIndex = 5;
            }
            else if (CommandName.ToUpper().Equals("INVESTIGATION"))
            {
                Filter.CurrentIncidentTab = "INVESTIGATION";
                ViewState["PAGEURL"] = "../Inspection/InspectionIncidentCAR.aspx";
            }
            else if (CommandName.ToUpper().Equals("WORKREQUEST"))
            {
                Filter.CurrentIncidentTab = "WORKREQUEST";
                ViewState["PAGEURL"] = "../Inspection/InspectionIncidentDefectWorkRequest.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString();
                if (ViewState["DashboardYN"].ToString() != "")
                    IncidentGeneral.SelectedMenuIndex = 5;
                else
                    IncidentGeneral.SelectedMenuIndex = 6;
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENTS"))
            {
                Filter.CurrentIncidentTab = "ATTACHMENTS";
                ViewState["PAGEURL"] = "../Inspection/InspectionIncidentAttachment.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString();
                if (ViewState["DashboardYN"].ToString() != "")
                    IncidentGeneral.SelectedMenuIndex = 8;
                else
                    IncidentGeneral.SelectedMenuIndex = 9;
            }
            else if (CommandName.ToUpper().Equals("CONSEQUENCE"))
            {
                Filter.CurrentIncidentTab = "CONSEQUENCE";
                ViewState["PAGEURL"] = "../Inspection/InspectionIncidentInjuryList.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString();
                if (ViewState["DashboardYN"].ToString() != "")
                    IncidentGeneral.SelectedMenuIndex = 2;
                else
                    IncidentGeneral.SelectedMenuIndex = 3;
            }
            else if (CommandName.ToUpper().Equals("REQUISITION"))
            {
                Response.Redirect("../Inspection/InspectionIncidentPurchaseForm.aspx?redirectfrom=INCIDENTDETAILS", true);
                if (ViewState["DashboardYN"].ToString() != "")
                    IncidentGeneral.SelectedMenuIndex = 6;
                else
                    IncidentGeneral.SelectedMenuIndex = 7;
            }
            else if (CommandName.ToUpper().Equals("COMPANYRESPONSE"))
            {
                Filter.CurrentIncidentTab = "COMPANYRESPONSE";
                ViewState["PAGEURL"] = "../Inspection/InspectionIncidentCompanyResponse.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString();
                if (ViewState["DashboardYN"].ToString() != "")
                    IncidentGeneral.SelectedMenuIndex = 7;
                else
                    IncidentGeneral.SelectedMenuIndex = 8;
            }
            else if (CommandName.ToUpper().Equals("LIST"))
            {
                if ((ViewState["Vesselid"].ToString() == "0") && (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.Equals("OFFSHORE")))
                {
                    Response.Redirect("../Inspection/InspectionIncidentNearMissOfficeList.aspx", true);
                }
                else
                {
                    Response.Redirect("../Inspection/InspectionIncidentNearMissList.aspx", true);
                }
            }
            else if (CommandName.ToUpper().Equals("MSCAT"))
            {
                Filter.CurrentIncidentTab = "MSCAT";
                ViewState["PAGEURL"] = "../Inspection/InspectionIncidentMSCAT.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString();
                if (ViewState["DashboardYN"].ToString() != "")
                    IncidentGeneral.SelectedMenuIndex = 3;
                else
                    IncidentGeneral.SelectedMenuIndex = 4;
            }
            else
            {
                QualitySubMenu_TabStripCommand(sender, e);
            }
        }
    }
    private PhoenixToolbar CreateConsequenceSubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();

        toolbarsub.AddButton("Health and Safety", "HEALTH");
        toolbarsub.AddButton("Other Consequence", "OTHERCONSEQUENCE");

        ifMoreInfo.Attributes["src"] = Page;
        return toolbarsub;
    }
    protected void QualitySubMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("HEALTH"))
        {
            CreateConsequenceSubTab("../Inspection/InspectionIncidentInjuryList.aspx", 0, "HEALTH");
            if (ViewState["DashboardYN"].ToString() != "")
                IncidentGeneral.SelectedMenuIndex = 1;
            else
                IncidentGeneral.SelectedMenuIndex = 2;
        }
        else if (CommandName.ToUpper().Equals("OTHERCONSEQUENCE"))
        {
            CreateConsequenceSubTab("../Inspection/InspectionIncidentConsequence.aspx", 1, "OTHERCONSEQUENCE");
            if (ViewState["DashboardYN"].ToString() != "")
                IncidentGeneral.SelectedMenuIndex = 1;
            else
                IncidentGeneral.SelectedMenuIndex = 2;
        }
        else if (CommandName.ToUpper().Equals("DETAILS"))
        {
            CreateInvestigationSubTab("../Inspection/InspectionIncidentCAR.aspx", 0, "DETAILS");
            if (ViewState["DashboardYN"].ToString() != "")
                IncidentGeneral.SelectedMenuIndex = 2;
            else
                IncidentGeneral.SelectedMenuIndex = 3;
        }
        else if (CommandName.ToUpper().Equals("DRUGANDALCOHOLTEST"))
        {
            CreateInvestigationSubTab("../Inspection/InspectionIncidentDrugAndAlcoholTest.aspx", 1, "DRUGANDALCOHOLTEST");
            if (ViewState["DashboardYN"].ToString() != "")
                IncidentGeneral.SelectedMenuIndex = 2;
            else
                IncidentGeneral.SelectedMenuIndex = 3;
        }
    }
    private PhoenixToolbar CreateInvestigationSubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();

        toolbarsub.AddButton("Details", "DETAILS");
        toolbarsub.AddButton("Drug And Alcohol Test", "DRUGANDALCOHOLTEST");

        ifMoreInfo.Attributes["src"] = Page;
        return toolbarsub;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }
}
