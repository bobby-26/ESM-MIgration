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
public partial class InspectionIncidentConsequence : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("List", "LIST");
            toolbar.AddButton("Incident Details", "INCIDENTDETAILS");
            toolbar.AddButton("Consequence", "CONSEQUENCE");
            toolbar.AddButton("Investigation", "INVESTIGATION");
            toolbar.AddButton("RCA", "MSCAT");
            toolbar.AddButton("CAR", "CAR");
            toolbar.AddButton("Defect Work Order", "WORKREQUEST");
            toolbar.AddButton("Requisition", "REQUISITION");
            toolbar.AddButton("Correspondence", "COMPANYRESPONSE");
            toolbar.AddButton("Attachments", "ATTACHMENTS");

            //MenuIncidentGeneral.AccessRights = this.ViewState;
            //MenuIncidentGeneral.MenuList = toolbar.Show();
            //MenuIncidentGeneral.SelectedMenuIndex = 2;

            Filter.CurrentIncidentTab = "CONSEQUENCE";

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuIncidentConsequence.AccessRights = this.ViewState;
            MenuIncidentConsequence.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                BindPropertyDamageList();
                BindProcessLossList();
                BindSecurityList();
                BindEnvironmental();
                BindData();
                imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponentTreeView.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true);");                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void BindPropertyDamageList()
    {
        string propertydamage = PhoenixCommonRegisters.GetHardCode(1, 204, "PDM");
        cblPropertyDamage.DataSource = PhoenixInspectionIncident.ListConsequence(4, General.GetNullableInteger(propertydamage));
        cblPropertyDamage.DataBindings.DataTextField = "FLDNAME";
        cblPropertyDamage.DataBindings.DataValueField = "FLDSUBHAZARDID";
        cblPropertyDamage.DataBind();
    }

    protected void BindProcessLossList()
    {
        string processloss = PhoenixCommonRegisters.GetHardCode(1,204,"PLS");
        cblProcessLoss.DataSource = PhoenixInspectionIncident.ListConsequence(4, General.GetNullableInteger(processloss));
        cblProcessLoss.DataBindings.DataTextField = "FLDNAME";
        cblProcessLoss.DataBindings.DataValueField = "FLDSUBHAZARDID";
        cblProcessLoss.DataBind();
    }

    protected void BindSecurityList()
    {
        cblSecurity.DataSource = PhoenixInspectionIncident.ListConsequence(3, null);
        cblSecurity.DataBindings.DataTextField = "FLDNAME";
        cblSecurity.DataBindings.DataValueField = "FLDSUBHAZARDID";
        cblSecurity.DataBind();
    }

    protected void BindEnvironmental()
    {
        cblEnvironmental.DataSource = PhoenixInspectionIncident.ListConsequence(2, null);
        cblEnvironmental.DataBindings.DataTextField = "FLDNAME";
        cblEnvironmental.DataBindings.DataValueField = "FLDSUBHAZARDID";
        cblEnvironmental.DataBind();
    }

    protected void BindData()
    {
        DataSet ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(Filter.CurrentIncidentID));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
            BindCheckBoxList(cblPropertyDamage, dr["FLDPROPERTYDAMAGELIST"].ToString());
            txtDetails.Text = dr["FLDDETAILS"].ToString();
            txtComponentId.Text = dr["FLDCOMPONENTID"].ToString();
            txtComponentCode.Text = dr["FLDCOMPONENTNUMBER"].ToString();
            txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
            BindCheckBoxList(cblProcessLoss, dr["FLDPROCESSLOSSLIST"].ToString());
            BindCheckBoxList(cblSecurity, dr["FLDSECURITYLIST"].ToString());
            BindCheckBoxList(cblEnvironmental, dr["FLDENVIRONMENTALLIST"].ToString());
            txtSubstance.Text = dr["FLDNAMEOFSUBSTANCE"].ToString();
            txtnumberofhourslost.Text = dr["FLDNUMBEROFHOURSLOST"].ToString();
            if (dr["FLDRELEASETYPE"].ToString().Equals("1"))
                chkCargo.Checked = true;
            else
                chkCargo.Checked = false;
        }
    }

    private void BindCheckBoxList(RadCheckBoxList cbl, string list)
    {        
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                //if (cbl.Items.FindByValue(item) != null)
                //cbl.Items.FindByValue(item).Selected = true;
                cbl.SelectedValue = item;
            }
        }
    }

    private string ReadCheckBoxList(RadCheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ButtonListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }

    
   
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindProcessLossList();
        BindData();
    }
    protected void IncidentGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

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
            Response.Redirect("../Inspection/InspectionIncidentPurchaseForm.aspx?", true);
        }
        else if (CommandName.ToUpper().Equals("COMPANYRESPONSE"))
        {
            Filter.CurrentIncidentTab = "COMPANYRESPONSE";
            Response.Redirect("../Inspection/InspectionIncidentCompanyResponse.aspx?", true);
        }
        else if (CommandName.ToUpper().Equals("LIST"))
        {
            if ((ViewState["VESSELID"].ToString() == "0") && (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.Equals("OFFSHORE")))
            {
                Response.Redirect("../Inspection/InspectionIncidentNearMissOfficeList.aspx", true);
            }
            else
            {
                Response.Redirect("../Inspection/InspectionIncidentNearMissList.aspx", true);
            }
        }
    }
    protected void MenuIncidentConsequence_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string properydamage = ReadCheckBoxList(cblPropertyDamage);
                string processloss = ReadCheckBoxList(cblProcessLoss);
                string security = ReadCheckBoxList(cblSecurity);
                string environmental = ReadCheckBoxList(cblEnvironmental);

                PhoenixInspectionIncidentDamage.InsertPropertyDamage(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(Filter.CurrentIncidentID), General.GetNullableString(properydamage), General.GetNullableGuid(txtComponentId.Text),
                    General.GetNullableString(txtDetails.Text));

                PhoenixInspectionIncidentProcessLoss.InsertProcessLoss(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(Filter.CurrentIncidentID), General.GetNullableString(processloss), General.GetNullableInteger(txtnumberofhourslost.Text));

                PhoenixInspectionIncidentSecurity.InsertSecurity(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(Filter.CurrentIncidentID), General.GetNullableString(security));

                PhoenixInspectionIncidentPollution.InsertPollution(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(Filter.CurrentIncidentID), General.GetNullableString(txtSubstance.Text), int.Parse(chkCargo.Checked == true ? "1" : "0"),
                    General.GetNullableString(environmental));

                ucStatus.Text = "Information updated.";

                String script = String.Format("javascript:fnReloadList('code1','ifMoreInfo','','');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ClearComponent(object sender, EventArgs e)
    {
        txtComponentCode.Text = "";
        txtComponentName.Text = "";
        txtComponentId.Text = "";
    }
}
