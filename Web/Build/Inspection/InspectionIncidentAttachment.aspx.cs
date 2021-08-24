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
public partial class InspectionIncidentAttachment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbartitle = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbartitle.Show();

            //PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddButton("List", "LIST");
            //toolbar.AddButton("Incident Details", "INCIDENTDETAILS");
            //toolbar.AddButton("Consequence", "CONSEQUENCE");
            //toolbar.AddButton("Investigation", "INVESTIGATION");
            //toolbar.AddButton("RCA", "MSCAT");
            //toolbar.AddButton("CAR", "CAR");
            //toolbar.AddButton("PMS Task", "WORKREQUEST");
            //toolbar.AddButton("Requisition", "REQUISITION");
            //toolbar.AddButton("Correspondence", "COMPANYRESPONSE");
            //toolbar.AddButton("Attachments", "ATTACHMENTS");

            //MenuInspectionGeneral.AccessRights = this.ViewState;
            //MenuInspectionGeneral.MenuList = toolbar.Show();
            //MenuInspectionGeneral.SelectedMenuIndex = 9;
            Filter.CurrentIncidentTab = "ATTACHMENTS";

            if (!IsPostBack)
            {
                ViewState["Vesselid"] = "";
                ViewState["COMPANYID"] = "";

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                if (ViewState["COMPANYID"].ToString().Equals("20"))
                {
                    lblNote.Text = "*Note: Following to be added here as 'Attachments': <br/> &nbsp; Ship staff Training record (CR 25), Rest Hour record(CR 6B),Sickness report(CR 11), Safety Alerts / Moments if issued, Ships Medical Log, Crew Statements, Photographs or sketches";
                }               

                EditInspectionIncident();
                int lockforvesselyn = 0;
                PhoenixInspectionIncident.IncidentVesselUnlockCheck(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["INSPECTIONID"] != null ? ViewState["INSPECTIONID"].ToString() : "")
                    , ref lockforvesselyn);

                if (lockforvesselyn == 1)
                {
                    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?U=1&ratingyn=1&DTKEY=" + ViewState["DTKEY"].ToString() + "&MOD=" + PhoenixModule.QUALITY + "&VESSELID=" + ViewState["FLDVESSELID"].ToString();
                }
                else
                {
                    if (ViewState["DTKEY"] != null)
                    {
                        if (Filter.CurrentSelectedIncidentMenu == null)
                            ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"].ToString() + "&MOD=" + PhoenixModule.QUALITY + "&VESSELID=" + ViewState["FLDVESSELID"].ToString();
                        else
                            ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?U=1&DTKEY=" + ViewState["DTKEY"].ToString() + "&MOD=" + PhoenixModule.QUALITY + "&VESSELID=" + ViewState["FLDVESSELID"].ToString();
                    }
                    else
                    {
                        if (Filter.CurrentSelectedIncidentMenu == null)
                            ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.QUALITY + "&VESSELID=" + ViewState["FLDVESSELID"].ToString();
                        else
                            ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?U=1&DTKEY=" + ViewState["DTKEY"].ToString() + "&MOD=" + PhoenixModule.QUALITY + "&VESSELID=" + ViewState["FLDVESSELID"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void EditInspectionIncident()
    {
        if (Filter.CurrentIncidentID != null)
        {
            DataSet ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(Filter.CurrentIncidentID.ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
                ViewState["FLDVESSELID"] = dr["FLDVESSELID"].ToString();
                ViewState["INSPECTIONID"] = dr["FLDINSPECTIONINCIDENTID"].ToString();
            }
          
        }
    }
    protected void InspectionGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        DataSet ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(Filter.CurrentIncidentID));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["Vesselid"] = dr["FLDVESSELID"].ToString();
        }

        if (CommandName.ToUpper().Equals("INCIDENTDETAILS"))
        {
            Filter.CurrentIncidentTab = "INCIDENTDETAILS";
            Response.Redirect("../Inspection/InspectionIncidentList.aspx", true);
        }
        else if (CommandName.ToUpper().Equals("INVESTIGATION"))
        {
            Filter.CurrentIncidentTab = "INVESTIGATION";
            Response.Redirect("../Inspection/InspectionIncidentList.aspx?selectTab=INVESTIGATION", true);
        }
        else if (CommandName.ToUpper().Equals("MSCAT"))
        {
            Filter.CurrentIncidentTab = "MSCAT";
            Response.Redirect("../Inspection/InspectionIncidentMSCAT.aspx?selectTab=MSCAT", true);
        }
        else if (CommandName.ToUpper().Equals("CAR"))
        {
            Filter.CurrentIncidentTab = "CAR";
            Response.Redirect("../Inspection/InspectionIncidentList.aspx?selectTab=CAR", true);
        }
        else if (CommandName.ToUpper().Equals("WORKREQUEST"))
        {
            Filter.CurrentIncidentTab = "WORKREQUEST";
            Response.Redirect("../Inspection/InspectionIncidentList.aspx?selectTab=WORKREQUEST", true);
        }
        else if (CommandName.ToUpper().Equals("ATTACHMENTS"))
        {
            Filter.CurrentIncidentTab = "ATTACHMENTS";
            Response.Redirect("../Inspection/InspectionIncidentAttachment.aspx?selectTab=ATTACHMENTS", true);
        }
        else if (CommandName.ToUpper().Equals("REQUISITION"))
        {
            Filter.CurrentIncidentTab = "REQUISITION";
            Response.Redirect("../Inspection/InspectionIncidentPurchaseForm.aspx?redirectfrom=ATTACHMENTS", true);
        }
        else if (CommandName.ToUpper().Equals("COMPANYRESPONSE"))
        {           
            Response.Redirect("../Inspection/InspectionIncidentCompanyResponse.aspx?redirectfrom=INCIDENT", true);            
        }
        else if (CommandName.ToUpper().Equals("CONSEQUENCE"))
        {
            Response.Redirect("../Inspection/InspectionIncidentInjuryList.aspx", true);
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
    }
}
