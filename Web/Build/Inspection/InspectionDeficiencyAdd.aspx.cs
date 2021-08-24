using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class InspectionDeficiencyAdd : PhoenixBasePage
{
    int days = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuInspectionDeficiency.AccessRights = this.ViewState;
        MenuInspectionDeficiency.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ucVessel.Enabled = true;

            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
                ucVessel.Company = nvc.Get("QMS");
                ucVessel.bind();
            }

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
                ucVessel.bind();
            }
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            BindVIRItem();
            BindSource();
            SetRights();
        }
        BindDays();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void InspectionDeficiency_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? deficiencyid = null;
            int raisedformref = 0;
            int raisedfrom;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidDeficiency())
                {
                    if (rblDeficiencyType.SelectedValue == "3" || rblDeficiencyType.SelectedValue == "4")
                    {
                        if (General.GetNullableGuid(ddlSchedule.SelectedValue) == null)
                        {
                            raisedfrom = 3;
                        }
                        else
                        {
                            PhoenixInspectionDeficiency.ListRaisedfrom(General.GetNullableGuid(ddlSchedule.SelectedValue)
                                , ref raisedformref);
                            raisedfrom = raisedformref;
                        }

                        PhoenixInspectionDeficiency.InsertObsDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , General.GetNullableGuid(ddlSchedule.SelectedValue)
                                , General.GetNullableInteger(ucVessel.SelectedVessel)
                                , General.GetNullableDateTime(ucDate.Text)
                                , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 146, "OPN"))
                                , General.GetNullableInteger(ucNonConformanceCategory.SelectedQuick)
                                , General.GetNullableString(txtChecklistRef.Text)
                                , txtDesc.Text
                                , txtInspectorComments.Text
                                , txtMasterComments.Text
                                , txtOfficeRemarks.Text
                                , null
                                , raisedfrom
                                , ref deficiencyid
                                , raisedfrom
                                , null
                                , null
                                , null
                                , (chkRCANotrequired.Checked == true ? 0 : 1)
                                , null
                                , General.GetNullableDateTime(ucRcaTargetDate.Text)
                                , null
                                , General.GetNullableString(txtKey.Text)
                                , General.GetNullableString(txtItem.Text)
                                , General.GetNullableInteger(rblDeficiencyType.SelectedValue)
                                , (chkCopyCAR.Checked == true ? 1 : 0)
                                , General.GetNullableInteger(ucCompany.SelectedCompany)
                                );
                        ucStatus.Text = "Observation Inserted";
                    }
                    if (rblDeficiencyType.SelectedValue == "1" || rblDeficiencyType.SelectedValue == "2")
                    {
                        if (General.GetNullableGuid(ddlSchedule.SelectedValue) == null)
                        {
                            raisedfrom = 2;
                        }
                        else
                        {
                            //raisedfrom = 1;
                            PhoenixInspectionDeficiency.ListRaisedfrom(General.GetNullableGuid(ddlSchedule.SelectedValue)
                                , ref raisedformref);

                            if (raisedformref == 1)
                            {
                                raisedfrom = raisedformref;
                            }
                            else
                            {
                                raisedfrom = 4;
                            }
                        }

                        PhoenixInspectionDeficiency.InsertNCDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , General.GetNullableGuid(ddlSchedule.SelectedValue)
                            , General.GetNullableInteger(rblDeficiencyType.SelectedValue)
                            , General.GetNullableInteger(ucVessel.SelectedVessel)
                            , General.GetNullableDateTime(ucDate.Text)
                            , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 146, "OPN"))
                            , General.GetNullableInteger(ucNonConformanceCategory.SelectedQuick)
                            , General.GetNullableString(txtChecklistRef.Text)
                            , txtDesc.Text
                            , txtInspectorComments.Text
                            , txtMasterComments.Text
                            , txtOfficeRemarks.Text
                            , null
                            , raisedfrom
                            , ref deficiencyid
                            , null
                            , null
                            , null
                            , (chkRCANotrequired.Checked == true ? 0 : 1)
                            , null
                            , General.GetNullableDateTime(ucRcaTargetDate.Text)
                            , null
                            , General.GetNullableString(txtKey.Text)
                            , General.GetNullableString(txtItem.Text)
                            , (chkCopyCAR.Checked == true ? 1 : 0)
                            , General.GetNullableInteger(ucCompany.SelectedCompany)
                            );
                        ucStatus.Text = "NC Inserted";
                    }
                    Filter.CurrentSelectedDeficiency = deficiencyid.ToString();
                    String script = String.Format("javascript:fnReloadList('codehelp1',null,null);");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindDays()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCommonRegisters.HardSearch(1, "224", null, null, sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            days = int.Parse(ds.Tables[0].Rows[0]["FLDHARDNAME"].ToString());
        }
    }

    protected void BindSource()
    {
        int? deftype = null;
        if (rblDeficiencyType.SelectedValue == "1" || rblDeficiencyType.SelectedValue == "2")
            deftype = 1;
        else
            deftype = 2;

        DataSet ds = PhoenixInspectionDeficiency.ListDeficiencySource(
              null
            , General.GetNullableInteger(ucVessel.SelectedVessel)
            , null
            , deftype
            , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
            , General.GetNullableInteger(ucCompany.SelectedCompany)
            );
        ddlSchedule.DataSource = ds;
        ddlSchedule.DataTextField = "FLDINSPECTIONSCHEDULENAME";
        ddlSchedule.DataValueField = "FLDINSPECTIONSCHEDULEID";
        ddlSchedule.DataBind();
        ddlSchedule.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void vessel_TextChanged(object sender, EventArgs e)
    {
        BindSource();
        if (ucVessel.SelectedVesselName.ToUpper().Equals("-- OFFICE --"))
        {
            ucCompany.Enabled = true;
            ucCompany.CssClass = "input_mandatory";
        }
        else
        {
            ucCompany.Enabled = false;
            ucCompany.CssClass = "input";
        }
    }

    protected void company_TextChanged(object sender, EventArgs e)
    {
        BindSource();
    }

    protected void DeficiencyType_TextChanged(object sender, EventArgs e)
    {
        BindSource();
        BindRCADate();
    }

    protected void ddlSchedule_Changed(object sender, EventArgs e)
    {
        int pscinspectionynout = 0;

        PhoenixInspectionDeficiency.ScheduleInspectionPSCCheck(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableGuid(ddlSchedule.SelectedValue)
            , ref pscinspectionynout);

        if (pscinspectionynout == 1 && rblDeficiencyType.SelectedValue != null && rblDeficiencyType.SelectedValue != "2")
            rblDeficiencyType.SelectedValue = "2";
    }
    protected void BindRCADate()
    {
        if (rblDeficiencyType.SelectedValue == "3" || rblDeficiencyType.SelectedValue == "4")
        {
            chkRCANotrequired.Checked = true;
            ucRcaTargetDate.Text = "";
            ucRcaTargetDate.Enabled = false;
        }
        else
        {
            chkRCANotrequired.Checked = false;
            ucRcaTargetDate.Enabled = true;
            SetRights();
            if (ucDate.Text != null)
            {
                DateTime ucIssueDate = DateTime.Parse(ucDate.Text);
                DateTime ucRCATargetdate = ucIssueDate.AddDays(days);
                ucRcaTargetDate.Text = ucRCATargetdate.ToString();
            }
        }
    }

    private bool IsValidDeficiency()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableInteger(rblDeficiencyType.SelectedValue) == null)
            ucError.ErrorMessage = "Deficiency Type is required.";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == 0 && General.GetNullableInteger(ucCompany.SelectedCompany) == null)
        {
            ucError.ErrorMessage = "Company is required";
        }

        if (General.GetNullableInteger(ucNonConformanceCategory.SelectedQuick) == null)
            ucError.ErrorMessage = "Deficiency Category is required.";

        if (General.GetNullableDateTime(ucDate.Text).Equals("") || General.GetNullableDateTime(ucDate.Text).Equals(null))
            ucError.ErrorMessage = "Issue date is required.";

        if (General.GetNullableDateTime(ucDate.Text) > DateTime.Today)
            ucError.ErrorMessage = "Issue date should not be greater than current date.";

        if (General.GetNullableString(txtDesc.Text) == null)
            ucError.ErrorMessage = "Description is required.";

        return (!ucError.IsError);
    }
    protected void ucIssueDateEdit_TextChanged(object sender, EventArgs e)
    {
        if (ucDate.Text != null)
        {
            BindRCADate();
        }
    }

    protected void chkRCANotrequired_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRCANotrequired.Checked.Equals(true))
        {
            ucRcaTargetDate.Text = "";
            ucRcaTargetDate.Enabled = false;
        }
        else
        {
            ucRcaTargetDate.Enabled = true;
            SetRights();
            if (ucDate.Text != null)
            {
                DateTime ucIssueDate = DateTime.Parse(ucDate.Text);
                DateTime ucRCATargetdate = ucIssueDate.AddDays(days);
                ucRcaTargetDate.Text = ucRCATargetdate.ToString();
            }
        }
    }

    protected void BindVIRItem()
    {
        //ddlItem.DataSource = PhoenixInspectionVIRItem.InspectionVIRItemTreeList();
        //ddlItem.DataTextField = "FLDITEMNAME";
        //ddlItem.DataValueField = "FLDITEMID";
        //ddlItem.DataBind();
        //ddlItem.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
    public void SetRights()
    {
        ucRcaTargetDate.Enabled = SessionUtil.CanAccess(this.ViewState, "RCATARGETDATE");
    }
}
