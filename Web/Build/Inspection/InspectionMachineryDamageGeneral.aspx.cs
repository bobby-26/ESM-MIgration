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
using System.Web.UI.HtmlControls;

public partial class InspectionMachineryDamageGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (Request.QueryString["VIEWONLY"] != null && Request.QueryString["VIEWONLY"].ToString() != "")
                ViewState["VIEWONLY"] = Request.QueryString["VIEWONLY"].ToString();

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            txtComponentId.Attributes.Add("style", "display:none");

            if (!IsPostBack)
            {
                if (Request.QueryString["MACHINERYDAMAGEID"] != null && Request.QueryString["MACHINERYDAMAGEID"].ToString() != "")
                    ViewState["MACHINERYDAMAGEID"] = Request.QueryString["MACHINERYDAMAGEID"].ToString();
                else
                    ViewState["MACHINERYDAMAGEID"] = "";

                ViewState["COMPANYID"] = "";
                ViewState["DashboardYN"] = string.IsNullOrEmpty(Request.QueryString["DashboardYN"]) ? "" : Request.QueryString["DashboardYN"];

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;

                if (nvc != null && nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();
                }
                SetVessel();
                BindControls();
                BindNearMiss();
                MachineryDamageEdit();

                if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
                    imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx?vesselid=" + General.GetNullableInteger(ucVessel.SelectedVessel) + "', null); ");
                else
                    imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx?', null); ");

                if (Request.QueryString["PageNo"] != null)
                {
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["PageNo"].ToString());
                }
                else
                {
                    ViewState["PAGENUMBER"] = null;
                }
            }

            txtWorkOrderNumber.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','', '" + Session["sitepath"] + "/Inspection/InspectionMachineryDamageDefectWorkRequest.aspx?MACID=" + ViewState["MACHINERYDAMAGEID"].ToString() + "&VESSELID=" + ucVessel.SelectedVessel + "&DefectId=" + ViewState["DDEFECTJOBID"] + "');return true;");
            lnkWorkRequest.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','', '" + Session["sitepath"] + "/Inspection/InspectionMachineryDamageDefectWorkRequest.aspx?MACID=" + ViewState["MACHINERYDAMAGEID"].ToString() + "&VESSELID=" + ucVessel.SelectedVessel + "');return true;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (ViewState["DashboardYN"].ToString() == "")
            {
                toolbar.AddButton("List", "LIST");
            }
            toolbar.AddButton("Details", "DETAILS");
            toolbar.AddButton("CAR", "CAR");
            MenuMachineryDamage.AccessRights = this.ViewState;
            MenuMachineryDamage.MenuList = toolbar.Show();
            if (ViewState["DashboardYN"].ToString() == "")
            {
                MenuMachineryDamage.SelectedMenuIndex = 1;
            }

            if (ViewState["DashboardYN"].ToString() == "1")
            {
                MenuMachineryDamage.SelectedMenuIndex = 0;
            }

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                toolbarsub.AddButton("Close", "CLOSE", ToolBarDirection.Right);
                toolbarsub.AddButton("Review", "REVIEW", ToolBarDirection.Right);
            }
            toolbarsub.AddButton("Raise Incident", "RAISEINCIDENT", ToolBarDirection.Right);
            toolbarsub.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuMachineryDamageGeneral.AccessRights = this.ViewState;
            MenuMachineryDamageGeneral.MenuList = toolbarsub.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuMachineryDamage_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Inspection/InspectionMachineryDamageList.aspx?MACHINERYDAMAGEID=" + ViewState["MACHINERYDAMAGEID"].ToString() + "&PageNo=" + ViewState["PAGENUMBER"]);
            }
            else if (CommandName.ToUpper().Equals("CAR"))
            {
                Response.Redirect("../Inspection/InspectionMachineryDamageCAR.aspx?MACHINERYDAMAGEID=" + ViewState["MACHINERYDAMAGEID"].ToString() + "&VESSELID=" + General.GetNullableInteger(ucVessel.SelectedVessel) + "&REFERENCENUMBER=" + txtReferenceNumber.Text + "&DashboardYN=" + ViewState["DashboardYN"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuMachineryDamageGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDetails())
                {
                    ucError.Visible = true;
                    return;
                }

                string strTimeTaken;
                string strImmediateCause, strRootCause;
                strImmediateCause = strRootCause = "";
                strTimeTaken = "";
                Guid? MachineryDamageid = Guid.Empty;

                string dateOfIncident = ucDateOfIncident.Text + " " + txtTimeOfIncident.SelectedTime;

                foreach (ButtonListItem li in cblImmediateCause.Items)
                {
                    if (li.Selected == true)
                        strImmediateCause = strImmediateCause + ',' + li.Value;
                }
                foreach (ButtonListItem li in cblRootCause.Items)
                {
                    if (li.Selected == true)
                        strRootCause = strRootCause + ',' + li.Value;
                }

                if (ViewState["MACHINERYDAMAGEID"] == null || General.GetNullableGuid(ViewState["MACHINERYDAMAGEID"].ToString()) == null)
                {

                    PhoenixInspectionMachineryDamage.MachineryDamageInsert(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableDateTime(dateOfIncident),
                        General.GetNullableString(txtTitle.Text.Trim()),
                        General.GetNullableInteger(ucVessel.SelectedVessel),
                        General.GetNullableGuid(txtComponentId.Text),
                        General.GetNullableGuid(null),
                        General.GetNullableString(txtModeofDiscovery.Text.Trim()),
                        General.GetNullableGuid(ddlProcessLoss.SelectedValue),
                        General.GetNullableGuid(ddlCost.SelectedValue),
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        General.GetNullableInteger(ucQuickVesselActivity.SelectedQuick),
                        null,
                        General.GetNullableInteger(ViewState["COMPANYID"].ToString()),
                        General.GetNullableString(txtDetialsOfIncident.Text.Trim()),
                        General.GetNullableDecimal(strTimeTaken),
                        null,
                        null,
                        null,
                        null,
                        General.GetNullableString(txtCorrectiveAction.Text.Trim()),
                        General.GetNullableString(txtRecommendations.Text.Trim()),
                        General.GetNullableString(txtOfficeComments.Text.Trim()),
                        null,
                        ref MachineryDamageid,
                        null,
                        null,
                        null,
                        General.GetNullableInteger(ucModeofDiscovery.SelectedQuick),
                        null,
                        General.GetNullableGuid(ddlCategory.SelectedValue),
                        General.GetNullableGuid(ddlSubCategory.SelectedValue),
                        General.GetNullableString(strImmediateCause),
                        General.GetNullableString(strRootCause),
                        General.GetNullableString(txtImmediateCauseOthers.Text),
                        General.GetNullableString(txtRootCauseOthers.Text),
                        General.GetNullableDateTime(ucLastOverhaulDate.Text),
                        General.GetNullableDecimal(ucRunningHrs.Text),
                        General.GetNullableInteger(txtnumberofhourslost.Text.Trim()),
                        General.GetNullableGuid(rblNearmiss.SelectedValue));

                    ViewState["MACHINERYDAMAGEID"] = MachineryDamageid;
                    ucStatus.Text = "Machinery Damage details have been added successfully.";
                    MachineryDamageEdit();
                }
                else
                {
                    PhoenixInspectionMachineryDamage.MachineryDamageUpdate(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableGuid(ViewState["MACHINERYDAMAGEID"].ToString()),
                        General.GetNullableDateTime(dateOfIncident),
                        General.GetNullableString(txtTitle.Text),
                        General.GetNullableInteger(ucVessel.SelectedVessel),
                        General.GetNullableGuid(txtComponentId.Text),
                        General.GetNullableGuid(null),
                        General.GetNullableString(txtModeofDiscovery.Text.Trim()),
                        General.GetNullableGuid(ddlProcessLoss.SelectedValue),
                        General.GetNullableGuid(ddlCost.SelectedValue),
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        General.GetNullableInteger(ucQuickVesselActivity.SelectedQuick),
                        null,
                        int.Parse(ViewState["COMPANYID"].ToString()),
                        General.GetNullableString(txtDetialsOfIncident.Text.Trim()),
                        General.GetNullableDecimal(strTimeTaken),
                        null,
                        null,
                        null,
                        null,
                        General.GetNullableString(txtCorrectiveAction.Text.Trim()),
                        General.GetNullableString(txtRecommendations.Text.Trim()),
                        General.GetNullableString(txtOfficeComments.Text.Trim()),
                        null,
                        General.GetNullableString(txtCancelledRemarks.Text.Trim()),
                        null,
                        null,
                        null,
                        General.GetNullableInteger(ucModeofDiscovery.SelectedQuick),
                        null,
                        General.GetNullableGuid(ddlCategory.SelectedValue),
                        General.GetNullableGuid(ddlSubCategory.SelectedValue),
                        General.GetNullableString(strImmediateCause),
                        General.GetNullableString(strRootCause),
                        General.GetNullableString(txtImmediateCauseOthers.Text),
                        General.GetNullableString(txtRootCauseOthers.Text),
                        General.GetNullableDateTime(ucLastOverhaulDate.Text),
                        General.GetNullableDecimal(ucRunningHrs.Text),
                        General.GetNullableInteger(txtnumberofhourslost.Text.Trim()),
                        General.GetNullableGuid(rblNearmiss.SelectedValue),
                        General.GetNullableString(txtReviewRemarks.Text));

                    ucStatus.Text = "Machinery Damage details have been updated successfully.";
                    MachineryDamageEdit();
                }
            }
            else if (CommandName.ToUpper().Equals("RAISEINCIDENT"))
            {
                ViewState["TYPE"] = "2";
                RadWindowManager1.RadConfirm("Do you want to raise an Incident.?", "Confirm", 320, 150, null, "Confirm");
                return;
            }
            else if (CommandName.ToUpper().Equals("RAISENEARMISS"))
            {
                ViewState["TYPE"] = "3";
                RadWindowManager1.RadConfirm("Do you want to raise a Near Miss.?", "Confirm", 320, 150, null, "Confirm");
                return;
            }
            else if (CommandName.ToUpper().Equals("REVIEW"))
            {
                ViewState["TYPE"] = "7";
                RadWindowManager1.RadConfirm("Do you want to review this Machinery Damage / Failure.?", "Confirm", 320, 150, null, "Confirm");
                return;
            }
            else if (CommandName.ToUpper().Equals("CLOSE"))
            {
                ViewState["TYPE"] = "4";
                RadWindowManager1.RadConfirm("Do you want to close this Machinery Damage / Failure.?", "Confirm", 320, 150, null, "Confirm");
                return;
            }
            else if (CommandName.ToUpper().Equals("CANCEL"))
            {
                ViewState["TYPE"] = "5";
                RadWindowManager1.RadConfirm("Do you want to cancel this Machinery Damage / Failure.?", "Confirm", 320, 150, null, "Confirm");
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        MachineryDamageEdit();
    }

    protected void ClearComponent(object sender, EventArgs e)
    {
        txtComponentCode.Text = "";
        txtComponentName.Text = "";
        txtComponentId.Text = "";
    }

    protected void BindNearMiss()
    {
        try
        {
            rblNearmiss.DataSource = PhoenixInspectionIncidentNearMissCategory.MachineryDamageNearMissCategory();
            rblNearmiss.DataBindings.DataTextField = "FLDNAME";
            rblNearmiss.DataBindings.DataValueField = "FLDCATEGORYID";
            rblNearmiss.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void BindControls()
    {
        try
        {
            // Process Loss

            ddlProcessLoss.DataSource = PhoenixInspectionRiskAssessmentSubHazard.RASubHazardListForMachineryDamage(null, 1, null);
            ddlProcessLoss.DataTextField = "FLDNAME";
            ddlProcessLoss.DataValueField = "FLDSUBHAZARDID";
            ddlProcessLoss.DataBind();
            ddlProcessLoss.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

            // Cost

            ddlCost.DataSource = PhoenixInspectionRiskAssessmentSubHazard.RASubHazardListForMachineryDamage(null, null, 1);
            ddlCost.DataTextField = "FLDNAME";
            ddlCost.DataValueField = "FLDSUBHAZARDID";
            ddlCost.DataBind();
            ddlCost.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

            ddlCategory.DataSource = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(4);
            ddlCategory.DataTextField = "FLDNAME";
            ddlCategory.DataValueField = "FLDINCIDENTNEARMISSCATEGORYID";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlSubCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

            cblImmediateCause.DataSource = PhoenixInspectionMachineryDamageImmediateCause.ListImmediateCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
            cblImmediateCause.DataBindings.DataTextField = "FLDIMMEDIATECAUSENAME";
            cblImmediateCause.DataBindings.DataValueField = "FLDIMMEDIATECAUSEID";
            cblImmediateCause.DataBind();

            cblRootCause.DataSource = PhoenixInspectionMachineryDamageRootCause.ListRootCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
            cblRootCause.DataBindings.DataTextField = "FLDROOTCAUSENAME";
            cblRootCause.DataBindings.DataValueField = "FLDROOTCAUSEID";
            cblRootCause.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void Reset()
    {
        try
        {
            txtReferenceNumber.Text = "";
            txtTitle.Text = "";
            txtStatus.Text = "";
            ucDateOfIncident.Text = "";
            txtTimeOfIncident.SelectedTime = null;
            txtReportedByName.Text = "";
            txtReportedByRank.Text = "";
            txtReportedOfIncidentUTC.Text = "";
            txtReportedTimeOfIncidentUTC.SelectedTime = null;
            txtComponentCode.Text = "";
            txtComponentName.Text = "";
            txtComponentId.Text = "";
            txtModeofDiscovery.Text = "";
            txtConsequenceCategory.Text = "";
            ddlProcessLoss.SelectedValue = "";
            ddlCost.SelectedValue = "";
            cblImmediateCause.SelectedValue = "";
            cblRootCause.SelectedValue = "";
            ucQuickVesselActivity.SelectedQuick = "";
            txtDetialsOfIncident.Text = "";
            txtCorrectiveAction.Text = "";
            ucIncidentRaisedDate.Text = "";
            txtIncidentRaisedByName.Text = "";
            txtIncidentRaisedByRank.Text = "";
            txtRecommendations.Text = "";
            txtOfficeComments.Text = "";
            ucClosedDate.Text = "";
            txtClosedByName.Text = "";
            txtClosedByRank.Text = "";
            SetVessel();
            txtIncidentorNearMissRefNo.Text = "";
            chkIncidentRaisedYN.Checked = false;
            txtCancelledByName.Text = "";
            txtCancelledByRank.Text = "";
            ucCancelledDate.Text = "";
            txtCancelledRemarks.Text = "";
            txtCancelledRemarks.Enabled = false;
            ucModeofDiscovery.SelectedQuick = "";
            ddlCategory.SelectedValue = "";
            ddlSubCategory.SelectedValue = "";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MachineryDamageEdit()
    {
        try
        {
            string strList = "";
            if (ViewState["MACHINERYDAMAGEID"] != null && General.GetNullableGuid(ViewState["MACHINERYDAMAGEID"].ToString()) != null)
            {
                DataSet ds = PhoenixInspectionMachineryDamage.MachineryDamageEdit(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                            General.GetNullableGuid(ViewState["MACHINERYDAMAGEID"].ToString())
                                            );
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ViewState["COMPANYID"] = dr["FLDCOMPANYID"].ToString();
                    ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                    ucVessel.Enabled = false;
                    txtReferenceNumber.Text = dr["FLDREFERENCENUMBER"].ToString();
                    txtTitle.Text = dr["FLDTITLE"].ToString();
                    txtStatus.Text = dr["FLDSTATUSNAME"].ToString();
                    ucDateOfIncident.Text = "";
                    ucDateOfIncident.Text = General.GetNullableDateTime(dr["FLDINCIDENTDATE"].ToString()).ToString();
                    if (dr["FLDINCIDENTDATETIME"].ToString() != "")
                    {
                        txtTimeOfIncident.SelectedDate = Convert.ToDateTime(dr["FLDINCIDENTDATETIME"].ToString());
                    }
                    if (dr["FLDINCIDENTDATETIME"].ToString() == null)
                    {
                        txtTimeOfIncident.SelectedDate = null;
                    }
                    txtReportedByName.Text = dr["FLDREPORTEDBYNAME"].ToString();
                    txtReportedByRank.Text = dr["FLDREPORTEDBYRANK"].ToString();

                    txtReportedOfIncidentUTC.Text = General.GetNullableDateTime(dr["FLDREPORTEDDATE"].ToString()).ToString();
                    if (dr["FLDREPORTEDDATETIME"].ToString() != "")
                    {
                        txtReportedTimeOfIncidentUTC.SelectedDate = Convert.ToDateTime(dr["FLDREPORTEDDATETIME"].ToString());
                    }
                    if (dr["FLDREPORTEDDATETIME"].ToString() == null)
                    {
                        txtReportedTimeOfIncidentUTC.SelectedDate = null;
                    }
                    txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
                    txtComponentCode.Text = dr["FLDCOMPONENTNUMBER"].ToString();
                    txtComponentId.Text = dr["FLDCOMPONENTID"].ToString();
                    txtModeofDiscovery.Text = dr["FLDMODEOFDISCOVERY"].ToString();
                    ucQuickVesselActivity.SelectedQuick = dr["FLDVESSELACTIVITY"].ToString();
                    txtConsequenceCategory.Text = dr["FLDCONSEQUENCECATEGORYNAME"].ToString();

                    string strCategory = General.GetNullableString(txtConsequenceCategory.Text.ToString().ToUpper());
                    if (strCategory == "A" || strCategory == "B")
                        lblConsequenceGuidenceText.Visible = true;

                    if (ddlProcessLoss.Items.FindItemByValue(dr["FLDPROCESSSUBHAZARDID"].ToString()) != null)
                        ddlProcessLoss.SelectedValue = dr["FLDPROCESSSUBHAZARDID"].ToString();
                    if (ddlCost.Items.FindItemByValue(dr["FLDPROPERTYSUBHAZARDID"].ToString()) != null)
                        ddlCost.SelectedValue = dr["FLDPROPERTYSUBHAZARDID"].ToString();
                    txtDetialsOfIncident.Text = dr["FLDINCIDENTCOMPREHENSIVEDESCRIPTION"].ToString();

                    if (ddlCategory.Items.FindItemByValue(dr["FLDCATEGORYID"].ToString()) != null)
                        ddlCategory.SelectedValue = dr["FLDCATEGORYID"].ToString();

                    ddlCategory_SelectedIndexChanged(ddlCategory, new EventArgs());

                    if (ddlSubCategory.Items.FindItemByValue(dr["FLDSUBCATEGORYID"].ToString()) != null)
                        ddlSubCategory.SelectedValue = dr["FLDSUBCATEGORYID"].ToString();

                    txtCorrectiveAction.Text = dr["FLDCORRECTIVEACTIONTAKEN"].ToString();
                    ucIncidentRaisedDate.Text = General.GetNullableDateTime(dr["FLDINCIDENTRAISEDDATE"].ToString()).ToString();
                    txtIncidentRaisedByName.Text = dr["FLDINCIDENTRAISEDBYNAME"].ToString();
                    txtIncidentRaisedByRank.Text = dr["FLDINCIDENTRAISEDBYRANK"].ToString();
                    txtRecommendations.Text = dr["FLDSUGGESTIONS"].ToString();
                    txtOfficeComments.Text = dr["FLDOFFICECOMMENTS"].ToString();
                    txtClosedByName.Text = dr["FLDCOLSEDBYNAME"].ToString();
                    ucClosedDate.Text = General.GetNullableDateTime(dr["FLDCLOSEDDATE"].ToString()).ToString();
                    txtIncidentorNearMissRefNo.Text = dr["FLDINCIDENTRFNO"].ToString();

                    chkIncidentRaisedYN.Checked = General.GetNullableString(dr["FLDINCIDENTRFNO"].ToString()) != null ? true : false;
                    txtIncidentorNearMissRefNo.Visible = General.GetNullableString(dr["FLDINCIDENTRFNO"].ToString()) != null ? true : false;

                    txtCancelledByName.Text = dr["FLDCANCELLEDBYNAME"].ToString();
                    txtCancelledByRank.Text = dr["FLDCANCELLEDBYRANK"].ToString();
                    ucCancelledDate.Text = General.GetNullableDateTime(dr["FLDCANCELLEDDATE"].ToString()).ToString();
                    txtCancelledRemarks.Text = dr["FLDCANCELLEDCOMMENTS"].ToString();
                    txtCancelledRemarks.Enabled = true;
                    ucModeofDiscovery.SelectedQuick = dr["FLDDISCOVERYMODE"].ToString();
                    txtnumberofhourslost.Text = dr["FLDNUMBEROFHOURSLOST"].ToString();

                    txtReviewRemarks.Text = dr["FLDREVIEWEDREMARKS"].ToString();
                    txtReviewByName.Text = dr["FLDREVIEWEDBYNAME"].ToString();
                    ucReviewDate.Text = General.GetNullableDateTime(dr["FLDREVIEWEDDATE"].ToString()).ToString();

                    string dtKey = dr["FLDDTKEY"].ToString();

                    strList = dr["FLDIMMEDIATECAUSE"].ToString();

                    foreach (ButtonListItem li in cblImmediateCause.Items)
                    {
                        if (strList.Contains(li.Value))
                            li.Selected = true;
                    }

                    strList = dr["FLDROOTCAUSE"].ToString();
                    foreach (ButtonListItem li in cblRootCause.Items)
                    {
                        if (strList.Contains(li.Value))
                            li.Selected = true;
                    }


                    imgEvidence.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + dtKey + "&mod="
                        + PhoenixModule.QUALITY + "&type=MCHNRYDMGEVIDENCE" + "&cmdname=MACHINERYDAMAGEEVIDENCEUPLOAD&VESSELID=" + General.GetNullableInteger(dr["FLDVESSELID"].ToString()) + "'); return false;";

                    HtmlGenericControl html = new HtmlGenericControl();

                    if (dr["FLDATTACHMENTEXISTSYN"].ToString() == "0")
                    {
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                        imgEvidence.Controls.Add(html);
                    }
                    else
                    {
                        html.InnerHtml = "<span class=\"icon\" style=\"color:skyblue\"><i class=\"fas fa-paperclip\"></i></span>";
                        imgEvidence.Controls.Add(html);
                    }

                    cblImmediateCause_Changed(null, null);
                    cblRootCause_Changed(null, null);
                    txtImmediateCauseOthers.Text = dr["FLDIMMEDIATECAUSEOTHERS"].ToString();
                    txtRootCauseOthers.Text = dr["FLDROOTCAUSEOTHERS"].ToString();
                    txtWorkOrderNumber.Text = dr["FLDWORKNUMBER"].ToString();
                    ucLastOverhaulDate.Text = dr["FLDLASTOVERHAULDATE"].ToString();
                    ucRunningHrs.Text = dr["FLDRUNNINGHOURS"].ToString();
                    ViewState["DDEFECTJOBID"] = dr["FLDDEFECTJOBID"].ToString();

                    rblNearmiss.SelectedValue = dr["FLDNEARMISSCATEGORYID"].ToString();
                    //if (rblNearmiss.Items.FindByValue(dr["FLDNEARMISSCATEGORYID"].ToString()) != null)
                    if (rblNearmiss.SelectedValue != null)
                        rblNearmiss.SelectedValue = dr["FLDNEARMISSCATEGORYID"].ToString();
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

    protected void SetVessel()
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.bind();
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
            }
            else
            {
                ucVessel.SelectedVessel = "";
                ucVessel.Enabled = true;
            }
        }
        else
        {
            ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.InstallCode.ToString();
            ucVessel.Enabled = false;
        }
    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
            imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx?vesselid=" + General.GetNullableInteger(ucVessel.SelectedVessel) + "&showcritical=1', null); ");
        else
            imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx?showcritical=1', null); ");
    }

    private bool IsValidDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (txtTitle.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Title is required.";

        if (General.GetNullableDateTime(ucDateOfIncident.Text) == null)
            ucError.ErrorMessage = "Date of Incident is required.";

        if (txtTimeOfIncident.SelectedTime == null)
            ucError.ErrorMessage = "Time of Incident is required.";
        else
        {
            if (General.GetNullableDateTime(ucDateOfIncident.Text + " " + txtTimeOfIncident.SelectedTime) == null)
                ucError.ErrorMessage = "Time of Incident is invalid.";
        }

        if (General.GetNullableDateTime(ucDateOfIncident.Text) > System.DateTime.Now)
            ucError.ErrorMessage = "Date of Incident should not be the future date.";

        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            if (ViewState["COMPANYID"] == null || General.GetNullableInteger(ViewState["COMPANYID"].ToString()) == null)
                ucError.ErrorMessage = "Default company is not configured in user permissions for this login.";
        }
        return (!ucError.IsError);
    }

    private bool IsValidWorkRequest(bool view)
    {
        if (ViewState["MACHINERYDAMAGEID"] == null || !General.GetNullableGuid(ViewState["MACHINERYDAMAGEID"].ToString()).HasValue)
        {
            ucError.ErrorMessage = "Create Machinery Damage/Failure First.";
        }
        if (ViewState["MACHINERYDAMAGEID"] != null && txtWorkOrderNumber.Text.Trim().Equals("") && view)
        {
            ucError.ErrorMessage = "Create Work Request to view the Work Order.";
        }
        return (!ucError.IsError);
    }

    protected void MachineryDamageStatusUpdate(int status, string comments)
    {
        PhoenixInspectionMachineryDamage.MachineryDamageStatusUpdate(
                                                                   PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                   General.GetNullableGuid(ViewState["MACHINERYDAMAGEID"].ToString()),
                                                                   status,
                                                                   General.GetNullableString(comments),
                                                                   null,
                                                                   null
                                                                   );

    }
    protected void MachineryDamageStatusReview(string comments)
    {
        PhoenixInspectionMachineryDamage.MachineryDamageReview(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                  , new Guid((ViewState["MACHINERYDAMAGEID"]).ToString())
                                                                  , General.GetNullableString(comments));

    }

    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["MACHINERYDAMAGEID"] != null && ViewState["MACHINERYDAMAGEID"].ToString() != string.Empty)
            {
                if (ViewState["TYPE"] != null)
                {
                    if (General.GetNullableInteger(ViewState["TYPE"].ToString()) == 2)
                    {
                        string script = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMachineryDamageRaiseIncidentOrNearMiss.aspx?MACHINERYDAMAGEID=" + ViewState["MACHINERYDAMAGEID"].ToString() + "&INCIDENTORNEARMISS=2" + "','large')\r\n;";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
                    }
                    else if (General.GetNullableInteger(ViewState["TYPE"].ToString()) == 3)
                    {
                        string script = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMachineryDamageRaiseIncidentOrNearMiss.aspx?MACHINERYDAMAGEID=" + ViewState["MACHINERYDAMAGEID"].ToString() + "&INCIDENTORNEARMISS=3" + "','large')\r\n;";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
                    }
                    else if (General.GetNullableInteger(ViewState["TYPE"].ToString()) == 4)
                    {
                        MachineryDamageStatusUpdate(4, txtOfficeComments.Text.Trim());
                        ucStatus.Text = "Machinery Damage has been closed.";
                    }
                    else if (General.GetNullableInteger(ViewState["TYPE"].ToString()) == 5)
                    {
                        MachineryDamageStatusUpdate(5, txtCancelledRemarks.Text.Trim());
                        ucStatus.Text = "Machinery Damage has been cancelled.";
                    }
                    else if (General.GetNullableInteger(ViewState["TYPE"].ToString()) == 7)
                    {
                        MachineryDamageStatusReview(txtReviewRemarks.Text.Trim());
                        ucStatus.Text = "Machinery Damage has been Reviewed.";
                    }
                    MachineryDamageEdit();
                    ViewState["TYPE"] = null;
                }
            }
            else
            {
                ucError.ErrorMessage = "Create Machinery Damage/Failure First.";
                ucError.Visible = true;
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
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSubCategory.DataSource = PhoenixInspectionIncidentNearMissSubCategory.ListIncidentNearMissSubCategory(General.GetNullableGuid(ddlCategory.SelectedValue));
        ddlSubCategory.DataTextField = "FLDNAME";
        ddlSubCategory.DataValueField = "FLDINCIDENTNEARMISSSUBCATEGORYID";
        ddlSubCategory.DataBind();
        ddlSubCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void cblImmediateCause_Changed(object sender, EventArgs e)
    {
        bool flag = false;
        foreach (ButtonListItem item in cblImmediateCause.Items)
        {
            if (item.Selected == true)
            {
                DataSet ds = PhoenixInspectionMachineryDamageImmediateCause.ListImmediateCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , General.GetNullableGuid(item.Value));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (dr["FLDOTHERSYN"].ToString() == "1")
                    {
                        flag = true;
                        break;
                    }
                }
            }
        }
        txtEnableDisableControl(txtImmediateCauseOthers, flag);
    }
    protected void cblRootCause_Changed(object sender, EventArgs e)
    {
        bool flag = false;
        foreach (ButtonListItem item in cblRootCause.Items)
        {
            if (item.Selected == true)
            {
                DataSet ds = PhoenixInspectionMachineryDamageRootCause.ListRootCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , General.GetNullableGuid(item.Value));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (dr["FLDOTHERSYN"].ToString() == "1")
                    {
                        flag = true;
                        break;
                    }
                }
            }
        }
        txtEnableDisableControl(txtRootCauseOthers, flag);
    }

    public void txtEnableDisableControl(RadTextBox txt, bool flag)
    {
        if (flag == true)
        {
            txt.Enabled = true;
            txt.ReadOnly = false;
            txt.CssClass = "input";
        }
        else
        {
            txt.Enabled = false;
            txt.ReadOnly = true;
            txt.CssClass = "readonlytextbox";
        }
    }
    protected void lnkWorkRequest_Onlick(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Inspection/InspectionMachineryDamageDefectWorkRequest.aspx?MACID=" + ViewState["MACHINERYDAMAGEID"].ToString() + "&VESSELID="+ ucVessel.SelectedVessel, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void imgView_Onlick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (!IsValidWorkRequest(true))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        Response.Redirect("~/PlannedMaintenance/PlannedMaintenanceWorkOrder.aspx?from=machinery&wno=" + txtWorkOrderNumber.Text + "&MACHINERYDAMAGEID=" + ViewState["MACHINERYDAMAGEID"].ToString(), true);
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
}
