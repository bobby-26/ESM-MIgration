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
public partial class InspectionUnsafeActsConditionsInvestigation : PhoenixBasePage
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
                BindGFTBasicFactor();
                BindGFTRootCause();
                lnkQ5Report.Visible = true;
                lnkQ5Report.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=Q5UNSAFEOCCURRENCE&showexcel=no&showword=no&showmenu=no&DIRECTINCIDENTID=" + ViewState["directincidentid"].ToString() + "');return true;");
            }
            setAccessibilityOfControls();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (ViewState["DashboardYN"].ToString() == "")
            {
                toolbar.AddButton("List", "LIST");
            }
            toolbar.AddButton("Report", "REPORT");
            toolbar.AddButton("Master's Investigation", "INVESTIGATE");
            MenuInspectionGeneral.AccessRights = this.ViewState;
            MenuInspectionGeneral.MenuList = toolbar.Show();

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Raise Near Miss", "RAISENEARMISS", ToolBarDirection.Right);
            toolbarsub.AddButton("Raise Incident", "RAISEINCIDENT", ToolBarDirection.Right);
            toolbarsub.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuInspectionIncident.AccessRights = this.ViewState;
            MenuInspectionIncident.MenuList = toolbarsub.Show();

            MenuInspectionGeneral.SelectedMenuIndex = 2;
            if (ViewState["DashboardYN"].ToString() != "")
            {
                MenuInspectionGeneral.SelectedMenuIndex = 1;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void setAccessibilityOfControls()
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
            ucActStatus.Enabled = false;
        else
            ucActStatus.Enabled = true;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0) //office
        {
            ucCategory.Enabled = false;
            ddlSubcategory.Enabled = false;
            cblRootCause.Enabled = false;
            ddlGFTBasicFactor.Enabled = false;
            ddlGFTRootCause.Enabled = false;
            txtMasterComments.Enabled = false;
            txtOfficeComments.Enabled = true;
            txtCorrectiveActionTaken.Enabled = false;
            ucCompletionDate.Enabled = false;
        }
        else //vessel
        {
            ucCategory.Enabled = true;
            ddlSubcategory.Enabled = true;
            cblRootCause.Enabled = true;
            ddlGFTBasicFactor.Enabled = true;
            ddlGFTRootCause.Enabled = true;
            txtMasterComments.Enabled = true;
            txtOfficeComments.Enabled = false;
            txtCorrectiveActionTaken.Enabled = true;
            ucCompletionDate.Enabled = true;
            lnkQ5Report.Visible = false;
            txtCloseOutRemarks.Enabled = false;
        }

        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
        {
            lblGFTBasicFactor.Visible = false;
            ddlGFTBasicFactor.Visible = false;
            lblGFTRootCause.Visible = false;
            ddlGFTRootCause.Visible = false;
        }
        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
        {
            lblHeader1.Text = "MSCAT Root Cause (To be filled in by the Safety Officer in agreement with Master)";
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
                ucCategory.SelectedHard = dr["FLDICCATEGORYMASTER"].ToString();
                ucCategory.bind();
                BindSubCategory();
                ddlSubcategory.SelectedValue = dr["FLDICSUBCATEGORYMASTER"].ToString();
                BindRootCauses();
                BindCheckBoxList(cblRootCause, dr["FLDROOTCAUSELIST"].ToString());
                txtRank.Text = dr["FLDRANK"].ToString();
                txtName.Text = dr["FLDNAME"].ToString();
                txtMasterComments.Text = dr["FLDMASTERCOMMENTS"].ToString();
                txtOfficeComments.Text = dr["FLDOFFICECOMMENTS"].ToString();
                ViewState["INCIDENTDATE"] = dr["FLDINCIDENTDATE"].ToString();
                txtMasterName.Text = dr["FLDMASTERNAME"].ToString();
                txtOfficeUserName.Text = dr["FLDOFFICEUSERNAME"].ToString();
                txtOfficeUserDesignation.Text = dr["FLDOFFICEUSERDESIGNATION"].ToString();
                ucActStatus.SelectedHard = dr["FLDSTATUS"].ToString();
                ucActStatus.bind();
                //setEnabledDisabled();
                ucCompletionDate.Text = dr["FLDCOMPLETIONDATE"].ToString();
                ucCloseoutDate.Text = dr["FLDCLOSEOUTDATE"].ToString();
                txtCloseOutByName.Text = dr["FLDCLOSEOUTBYNAME"].ToString();
                txtCloseOutByDesignation.Text = dr["FLDCLOSEOUTBYDESIGNATION"].ToString();
                txtCloseOutRemarks.Text = dr["FLDUNSAFECLOSEOUTREMARKS"].ToString();
                txtCancelReason.Text = dr["FLDCANCELREASON"].ToString();
                ucCancelDate.Text = dr["FLDCANCELDATE"].ToString();
                txtCancelledByName.Text = dr["FLDCANCELLEDBYNAME"].ToString();
                txtCancelledByDesignation.Text = dr["FLDCANCELLEDBYDESIGNATION"].ToString();
                txtCorrectiveActionTaken.Text = dr["FLDACTIONTOBETAKEN"].ToString();
                BindGFTBasicFactor();
                if (General.GetNullableGuid(dr["FLDGFTBASICFACTOR"].ToString()) != null)
                {
                    ddlGFTBasicFactor.SelectedValue = dr["FLDGFTBASICFACTOR"].ToString();
                }
                BindGFTRootCause();
                if (General.GetNullableGuid(dr["FLDGFTROOTCAUSE"].ToString()) != null)
                {
                    ddlGFTRootCause.SelectedValue = dr["FLDGFTROOTCAUSE"].ToString();
                }

                ucIncidentRaisedDate.Text = General.GetNullableDateTime(dr["FLDINCIDENTRAISEDDATE"].ToString()).ToString();
                txtIncidentRaisedByName.Text = dr["FLDINCIDENTRAISEDBYNAME"].ToString();
                txtIncidentRaisedByRank.Text = dr["FLDINCIDENTRAISEDBYRANK"].ToString();
                txtIncidentorNearMissRefNo.Text = dr["FLDINCIDENTRFNO"].ToString();

                chkIncidentRaisedYN.Checked = General.GetNullableString(dr["FLDINCIDENTRFNO"].ToString()) != null ? true : false;
                txtIncidentorNearMissRefNo.Visible = General.GetNullableString(dr["FLDINCIDENTRFNO"].ToString()) != null ? true : false;
            }
        }
    }

    protected void Status_Changed(object sender, EventArgs e)
    {
        //setEnabledDisabled();
    }

    protected void setEnabledDisabled()
    {
        if (ucActStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 146, "CLD"))
            txtCloseOutRemarks.Enabled = true;
        else
            txtCloseOutRemarks.Enabled = false;

        if (ucActStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 146, "CAD"))
            txtCancelReason.Enabled = true;
        else
            txtCancelReason.Enabled = false;
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
            else if (CommandName.ToUpper().Equals("REPORT"))
            {
                Response.Redirect("../Inspection/InspectionUnsafeActsConditions.aspx?DashboardYN=" + ViewState["DashboardYN"].ToString() + "&directincidentid=" + ViewState["directincidentid"] + "&OfficeDashboard=" + ViewState["OfficeDashboard"], false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void InspectionIncident_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (General.GetNullableGuid(ViewState["directincidentid"].ToString()) != null)
                {
                    if (!IsValidInvestigation())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    string rootcause = ReadCheckBoxList(cblRootCause);
                    PhoenixInspectionUnsafeActsConditions.DirectIncidentNearmissInvestigate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                new Guid(ViewState["directincidentid"].ToString()),
                                                int.Parse(ucCategory.SelectedHard),
                                                new Guid(ddlSubcategory.SelectedValue),
                                                General.GetNullableString(rootcause),
                                                General.GetNullableString(txtMasterComments.Text),
                                                General.GetNullableString(txtOfficeComments.Text),
                                                General.GetNullableInteger(ucActStatus.SelectedHard),
                                                General.GetNullableDateTime(ucCompletionDate.Text),
                                                General.GetNullableString(txtCancelReason.Text),
                                                General.GetNullableString(txtCloseOutRemarks.Text),
                                                General.GetNullableString(txtCorrectiveActionTaken.Text),
                                                General.GetNullableGuid(ddlGFTBasicFactor.SelectedValue),
                                                General.GetNullableGuid(ddlGFTRootCause.SelectedValue)
                                                );

                    ucStatus.Text = "Investigation updated successfully.";
                    BindInspectionIncident();
                }
            }
            else if (CommandName.ToUpper().Equals("RAISEINCIDENT"))
            {
                ViewState["TYPE"] = "1";
                RadWindowManager1.RadConfirm("Do you want to raise an Incident.?", "Confirm", 320, 150, null, "Confirm");
                return;
            }
            else if (CommandName.ToUpper().Equals("RAISENEARMISS"))
            {
                ViewState["TYPE"] = "2";
                RadWindowManager1.RadConfirm("Do you want to raise a Near Miss.?", "Confirm", 320, 150, null, "Confirm");
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidInvestigation()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableInteger(ucCategory.SelectedHard) == null)
            ucError.ErrorMessage = "Category is required.";

        if (General.GetNullableGuid(ddlSubcategory.SelectedValue) == null)
            ucError.ErrorMessage = "Sub-category is required.";

        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
        {
            if (General.GetNullableDateTime(ucCompletionDate.Text) > DateTime.Today)
                ucError.ErrorMessage = "Completion Date cannot be the future date.";
        }
        else
        {
            if (General.GetNullableInteger(ucActStatus.SelectedHard) == null)
                ucError.ErrorMessage = "Status is required.";
            else
            {
                if (ucActStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 146, "CMP"))
                {
                    if (General.GetNullableDateTime(ucCompletionDate.Text) == null)
                        ucError.ErrorMessage = "Completion Date is required.";
                    else if (General.GetNullableDateTime(ucCompletionDate.Text) > DateTime.Today)
                        ucError.ErrorMessage = "Completion Date cannot be the future date.";
                }
                else if (ucActStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 146, "CAD"))
                {
                    if (General.GetNullableString(txtCancelReason.Text) == null)
                        ucError.ErrorMessage = "Cancel Reason is required.";
                }
            }
        }
        return (!ucError.IsError);
    }

    protected void ucCategory_TextChanged(object sender, EventArgs e)
    {
        BindSubCategory();
    }

    protected void ddlGFTBasicFactor_Changed(object sender, EventArgs e)
    {
        BindGFTRootCause();
    }
    private void BindSubCategory()
    {
        DataTable dt = PhoenixInspectionUnsafeActsConditions.OpenReportSubcategoryList(General.GetNullableInteger(ucCategory.SelectedHard));
        ddlSubcategory.DataSource = dt;
        ddlSubcategory.DataTextField = "FLDIMMEDIATECAUSE";
        ddlSubcategory.DataValueField = "FLDIMMEDIATECAUSEID";
        ddlSubcategory.DataBind();
        ddlSubcategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        BindRootCauses();
    }

    protected void ddlSubcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindRootCauses();
    }

    protected void BindRootCauses()
    {
        DataSet ds = PhoenixInspectionBasicCause.ListRootCause(General.GetNullableGuid(ddlSubcategory.SelectedValue));
        cblRootCause.DataSource = ds;
        cblRootCause.DataBindings.DataTextField = "FLDBASICCAUSE";
        cblRootCause.DataBindings.DataValueField = "FLDBASICCAUSEID";
        cblRootCause.DataBind();
    }

    protected void BindGFTBasicFactor()
    {
        DataSet ds = PhoenixInspectionGFTBasicFactor.ListBasicFactor(1, null);
        ddlGFTBasicFactor.DataSource = ds;
        ddlGFTBasicFactor.DataTextField = "FLDBASICFACTORNAME";
        ddlGFTBasicFactor.DataValueField = "FLDBASICFACTORID";
        ddlGFTBasicFactor.DataBind();
        ddlGFTBasicFactor.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void BindGFTRootCause()
    {
        DataSet ds = PhoenixInspectionGFTRootCause.ListRootCause(1, General.GetNullableGuid(ddlGFTBasicFactor.SelectedValue), null);
        ddlGFTRootCause.DataSource = ds;
        ddlGFTRootCause.DataTextField = "FLDROOTCAUSENAME";
        ddlGFTRootCause.DataValueField = "FLDROOTCAUSEID";
        ddlGFTRootCause.DataBind();
        ddlGFTRootCause.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private void BindCheckBoxList(RadCheckBoxList cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
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

    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["TYPE"] != null)
            {
                if (General.GetNullableInteger(ViewState["TYPE"].ToString()) == 1)
                {
                    string script = "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionUnsafeActsRaiseIncidentOrNearMiss.aspx?DIRECTINCIDENTID=" + ViewState["directincidentid"].ToString() + "&INCIDENTORNEARMISS=1" + "','large')\r\n;";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
                }
                else if (General.GetNullableInteger(ViewState["TYPE"].ToString()) == 2)
                {
                    string script = "javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionUnsafeActsRaiseIncidentOrNearMiss.aspx?DIRECTINCIDENTID=" + ViewState["directincidentid"].ToString() + "&INCIDENTORNEARMISS=2" + "','large')\r\n;";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
                }
                BindInspectionIncident();
                ViewState["TYPE"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindInspectionIncident();
    }
}

