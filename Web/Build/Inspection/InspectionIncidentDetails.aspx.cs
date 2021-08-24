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

public partial class InspectionIncidentDetails : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {
        }        
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();

                toolbar.AddButton("Incident Details", "INCIDENTDETAILS");
                toolbar.AddButton("Consequence", "CONSEQUENCE");
                toolbar.AddButton("Cause Analysis", "CAUSE");
                toolbar.AddButton("Investigation", "INVESTIGATION");
                toolbar.AddButton("Attachments", "ATTACHMENTS");

                MenuIncidentGeneral.AccessRights = this.ViewState;
                MenuIncidentGeneral.MenuList = toolbar.Show();
                MenuIncidentGeneral.SelectedMenuIndex = 1;

                toolbar = new PhoenixToolbar();

                toolbar.AddButton("Incident Details", "INCIDENTDETAILS");
                toolbar.AddButton("Component Damage", "DAMAGE");
                toolbar.AddButton("Property Damage", "PROPERTYDAMAGE");
                toolbar.AddButton("Injury", "INJURY");
                toolbar.AddButton("Process Loss", "PROCESSLOSS");
                toolbar.AddButton("Security", "SECURITY");
                toolbar.AddButton("Environment Release", "POLLUTION");
                toolbar.AddButton("Drug and Alcohol Test", "DRUGALCOHOLTEST");

                MenuIncidentReportGeneral.AccessRights = this.ViewState;
                MenuIncidentReportGeneral.MenuList = toolbar.Show();
                MenuIncidentReportGeneral.SelectedMenuIndex = 0;

                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE");
                MenuGeneral.AccessRights = this.ViewState;
                if (Filter.CurrentSelectedIncidentMenu == null)
                    MenuGeneral.MenuList = toolbar.Show();
                BindIncidentDetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BindIncidentDetails()
    {
        DataSet ds;

        ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(Filter.CurrentIncidentID));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtComprehenciveDescription.Text = dr["FLDINCIDENTCOMPREHENSIVEDESCRIPTION"].ToString();
            ViewState["DTKEY"] = dr["FLDDTKEY"].ToString(); 
        }
    }

    protected void IncidentGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("INCIDENTDETAILS"))
        {
            Response.Redirect("../Inspection/InspectionIncidentList.aspx?", true);
        }
        else if (dce.CommandName.ToUpper().Equals("INVESTIGATION"))
        {
            Response.Redirect("../Inspection/InspectionIncidentList.aspx?selectTab=INVESTIGATION", true);
        }
        else if (dce.CommandName.ToUpper().Equals("CAUSE"))
        {
            Response.Redirect("../Inspection/InspectionIncidentList.aspx?selectTab=CAUSE", true);
        }
        else if (dce.CommandName.ToUpper().Equals("ATTACHMENTS"))
        {
            Response.Redirect("../Inspection/InspectionIncidentAttachment.aspx", true);
            //Response.Redirect("../Common/CommonFileAttachment.aspx?selectTab=ATTACHMENTS&dtkey=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.QUALITY + "&type=INCIDENT", true);
        }
        else if (dce.CommandName.ToUpper().Equals("CONSEQUENCE"))
        {
            Response.Redirect("../Inspection/InspectionIncidentDetailsList.aspx", true);
        }
    }

    protected void IncidentReportGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("DAMAGE"))
        {
            Response.Redirect("../Inspection/InspectionIncidentDamageList.aspx", true);
        }
        else if (dce.CommandName.ToUpper().Equals("PROPERTYDAMAGE"))
        {
            Response.Redirect("../Inspection/InspectionIncidentPropertyDamageList.aspx", true);
        }
        else if (dce.CommandName.ToUpper().Equals("INJURY"))
        {
            Response.Redirect("../Inspection/InspectionIncidentInjuryList.aspx", true);
        }
        else if (dce.CommandName.ToUpper().Equals("PROCESSLOSS"))
        {
            Response.Redirect("../Inspection/InspectionIncidentProcessLossList.aspx", true);
        }
        else if (dce.CommandName.ToUpper().Equals("POLLUTION"))
        {
            Response.Redirect("../Inspection/InspectionIncidentPollutionList.aspx", true);
        }
        else if (dce.CommandName.ToUpper().Equals("SECURITY"))
        {
            Response.Redirect("../Inspection/InspectionIncidentSecurityList.aspx", true);
        }
        else if (dce.CommandName.ToUpper().Equals("DRUGALCOHOLTEST"))
        {
            Response.Redirect("../Inspection/InspectionIncidentDrugAndAlcoholTest.aspx", true);
        } 

    }       

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["DAMAGEID"] == null)
                {
                    PhoenixInspectionIncident.IncidentDetailsUpdate(
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , new Guid(Filter.CurrentIncidentID)
                                                                    , txtComprehenciveDescription.Text
                                                                    , null
                                                                    , null
                                                                    );

                    ucStatus.Text = "Incident details updated.";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.Visible = true;
            ucError.Text = ex.Message;
            return;
        }
    }       
}
