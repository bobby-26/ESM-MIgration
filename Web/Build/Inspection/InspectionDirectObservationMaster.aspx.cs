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
public partial class InspectionDirectObservationMaster : PhoenixBasePage
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

                if (Request.QueryString["DIRECTOBSERVATIONID"] != null)
                {
                    Filter.CurrentSelectedDeficiency = Request.QueryString["DIRECTOBSERVATIONID"].ToString();

                    DataSet ds = PhoenixInspectionObservation.EditDirectObservation(new Guid(Filter.CurrentSelectedDeficiency));

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
                if (ViewState["URL"] == null || ViewState["URL"].ToString() == string.Empty)
                    ViewState["URL"] = "../Inspection/InspectionDirectObservation.aspx?DIRECTOBSERVATIONID=" + Filter.CurrentSelectedDeficiency;                
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
            MenuDirectObservationGeneral.AccessRights = this.ViewState;
            MenuDirectObservationGeneral.MenuList = toolbarmain.Show();
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
            if (currenttab == "car")
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionObservationCorrectiveAction.aspx?DIRECTOBSERVATIONID=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency + "&reffrom=directobs";

                if (ViewState["DashboardYN"].ToString() != "")
                    MenuDirectObservationGeneral.SelectedMenuIndex = 2;
                else
                    MenuDirectObservationGeneral.SelectedMenuIndex = 3;
            }
            else if (currenttab == "workrequest")
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionDefectWorkRequest.aspx?OBSID=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency + "&reffrom=directobs";

                if (ViewState["DashboardYN"].ToString() != "")
                    MenuDirectObservationGeneral.SelectedMenuIndex = 3;
                else
                    MenuDirectObservationGeneral.SelectedMenuIndex = 4;
            }
            else if (currenttab == "requisition")
            {
                ifMoreInfo.Attributes["src"] = null;
                Filter.CurrentAuditMenu = "directnc";
                ViewState["URL"] = "../Inspection/InspectionPurchaseForm.aspx?callfrom=" + Request.QueryString["currenttab"] + "&DIRECTOBJ=";
                Response.Redirect(ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency + "&reffrom=directobs", false);
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
                ViewState["URL"] = "../Inspection/InspectionDirectObservationMSCAT.aspx?OBSID=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency;

                if (ViewState["DashboardYN"].ToString() != "")
                    MenuDirectObservationGeneral.SelectedMenuIndex = 1;
                else
                    MenuDirectObservationGeneral.SelectedMenuIndex = 2;
            }
            else if (currenttab == "DET")
            {
                ifMoreInfo.Attributes["src"] = null;
                ViewState["URL"] = "../Inspection/InspectionDeficiencyEdit.aspx?DEFICIENCYID=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency;

                if (ViewState["DashboardYN"].ToString() != "")
                    MenuDirectObservationGeneral.SelectedMenuIndex = 0;
                else
                    MenuDirectObservationGeneral.SelectedMenuIndex = 1;
            }
        }
    }

    protected void MenuDirectObservationGeneral_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("DETAILS"))
            {
                Filter.CurrentInspectionMenu = "directobs";
                ViewState["URL"] = "../Inspection/InspectionDeficiencyEdit.aspx?DEFICIENCYID=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency ;
                if (ViewState["DashboardYN"].ToString() != "")
                    MenuDirectObservationGeneral.SelectedMenuIndex = 0;
                else
                    MenuDirectObservationGeneral.SelectedMenuIndex = 1;
                ViewState["currenttab"] = "DET";
            }

            if (CommandName.ToUpper().Equals("CAR"))
            {
                Filter.CurrentInspectionMenu = "directobs";
                ViewState["URL"] = "../Inspection/InspectionObservationCorrectiveAction.aspx?DIRECTOBSERVATIONID=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency + "&reffrom=directobs";
                if (ViewState["DashboardYN"].ToString() != "")
                    MenuDirectObservationGeneral.SelectedMenuIndex = 2;
                else
                    MenuDirectObservationGeneral.SelectedMenuIndex = 3;
                ViewState["currenttab"] = "car";
            }
            if (CommandName.ToUpper().Equals("WORKREQUEST"))
            {
                Filter.CurrentInspectionMenu = "directobs";
                ViewState["URL"] = "../Inspection/InspectionDefectWorkRequest.aspx?OBSID=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency + "&reffrom=directobs";
                if (ViewState["DashboardYN"].ToString() != "")
                    MenuDirectObservationGeneral.SelectedMenuIndex = 3;
                else
                    MenuDirectObservationGeneral.SelectedMenuIndex = 4;
                ViewState["currenttab"] = "workrequest";
            }
            if (CommandName.ToUpper().Equals("REQUISITION"))
            {
                Filter.CurrentInspectionMenu = "directobs";
                ViewState["URL"] = "../Inspection/InspectionPurchaseForm.aspx?DIRECTOBJ=";
                Response.Redirect(ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency + "&reffrom=directobs&currenttab=" + ViewState["currenttab"].ToString(), false);
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
                ViewState["URL"] = "../Inspection/InspectionDirectObservationMSCAT.aspx?OBSID=";
                ifMoreInfo.Attributes["src"] = ViewState["URL"].ToString() + Filter.CurrentSelectedDeficiency;
                ViewState["currenttab"] = "rca";
                if (ViewState["DashboardYN"].ToString() != "")
                    MenuDirectObservationGeneral.SelectedMenuIndex = 1;
                else
                    MenuDirectObservationGeneral.SelectedMenuIndex = 2;
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
