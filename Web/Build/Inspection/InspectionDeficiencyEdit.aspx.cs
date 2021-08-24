using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class Inspection_InspectionDeficiencyEdit : PhoenixBasePage
{
    int days = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["COMPANYID"] = "";
            ViewState["SELECTEDCOMPANYID"] = "";
            ViewState["DEFICIENCYTYPE"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
            }

            if (Request.QueryString["DEFICIENCYID"] != null && Request.QueryString["DEFICIENCYID"].ToString() != string.Empty)
                ViewState["DEFICIENCYID"] = Request.QueryString["DEFICIENCYID"].ToString();

            BindSource();
            BindVIRItem();
            BindData();
            SetRights();
        }
        BindMenu();
        BindDays();
    }
    private void BindMenu()
    {
        if (Request.QueryString["viewonly"] == null)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                if (ViewState["DEFICIENCYTYPE"].ToString() == "1" || ViewState["DEFICIENCYTYPE"].ToString() == "2")
                {
                    toolbarmain.AddButton("Review", "REVIEW", ToolBarDirection.Right);
                }
                toolbarmain.AddButton("Close", "CLOSE", ToolBarDirection.Right);
            }
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuInspectionDeficiency.AccessRights = this.ViewState;
            MenuInspectionDeficiency.MenuList = toolbarmain.Show();
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

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

    protected void InspectionDeficiency_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? deficiencyid;
            if (ViewState["DEFICIENCYID"] == null)
                deficiencyid = null;
            else
                deficiencyid = new Guid(ViewState["DEFICIENCYID"].ToString());
            int raisedfrom;
            int raisedformref = 0;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["DEFICIENCYID"] != null && ViewState["DEFICIENCYID"].ToString() != string.Empty)
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
                            if (ViewState["RAISEDFROM"].ToString() == "4")
                            {
                                raisedfrom = 4;
                            }

                            PhoenixInspectionDeficiency.InsertObsDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                    , General.GetNullableGuid(ddlSchedule.SelectedValue)
                                    , General.GetNullableInteger(ucVessel.SelectedVessel)
                                    , General.GetNullableDateTime(ucDate.Text)
                                    , General.GetNullableInteger(ddlStatus.SelectedHard)
                                    , General.GetNullableInteger(ucNonConformanceCategory.SelectedQuick)
                                    , General.GetNullableString(txtChecklistRef.Text)
                                    , txtDesc.Text
                                    , txtInspectorComments.Text
                                    , txtMasterComments.Text
                                    , null
                                    , null
                                    , raisedfrom
                                    , ref deficiencyid
                                    , raisedfrom
                                    , General.GetNullableDateTime(ucCompletionDate.Text)
                                    , General.GetNullableString(txtCancelReason.Text)
                                    , General.GetNullableString(txtCloseOutRemarks.Text)
                                    , (chkRCANotrequired.Checked == true ? 0 : 1)
                                    , (chkRCAcompleted.Checked == true ? 1 : 0)
                                    , General.GetNullableDateTime(ucRcaTargetDate.Text)
                                    , null
                                    , General.GetNullableString(txtKey.Text)
                                    , General.GetNullableString(txtItem.Text)
                                    , General.GetNullableInteger(rblDeficiencyType.SelectedValue)
                                    , (chkCopyCAR.Checked == true ? 1 : 0)
                                    , General.GetNullableInteger(chkCARNotRequiredYN.Checked == true ? "0" : "1")
                                    , General.GetNullableInteger(ViewState["SELECTEDCOMPANYID"].ToString())
                                    , General.GetNullableString(txtAuditor.Text)
                                    , General.GetNullableString(txtAuditPlace.Text)
                                    , General.GetNullableDateTime(ucNCDuedate.Text)
                                    , General.GetNullableInteger(ucAuditDept.SelectedDepartment)
                                    , General.GetNullableString(txtCorrectiveAction.Text)
                                    , General.GetNullableDateTime(ucCompletionDate.Text)
                                    , General.GetNullableDateTime(ucReviewDate.Text)
                                    , General.GetNullableDateTime(ucCloseoutDate.Text)
                                    , General.GetNullableDateTime(ucCancelDate.Text)
                                    , General.GetNullableString(txtReviewRemarks.Text.Trim())
                                    );
                            ucStatus.Text = "Observation Updated.";
                        }
                        else
                        {
                            if (General.GetNullableGuid(ddlSchedule.SelectedValue) == null)
                            {
                                raisedfrom = 2;
                            }
                            else
                            {
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

                                //raisedfrom = 1;
                            }
                            if (ViewState["RAISEDFROM"].ToString() == "3")
                            {
                                raisedfrom = 3;
                            }

                            PhoenixInspectionDeficiency.InsertNCDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , General.GetNullableGuid(ddlSchedule.SelectedValue)
                                , General.GetNullableInteger(rblDeficiencyType.SelectedValue)
                                , General.GetNullableInteger(ucVessel.SelectedVessel)
                                , General.GetNullableDateTime(ucDate.Text)
                                , General.GetNullableInteger(ddlStatus.SelectedHard)
                                , General.GetNullableInteger(ucNonConformanceCategory.SelectedQuick)
                                , General.GetNullableString(txtChecklistRef.Text)
                                , txtDesc.Text
                                , txtInspectorComments.Text
                                , txtMasterComments.Text
                                , null
                                , null
                                , raisedfrom
                                , ref deficiencyid
                                , General.GetNullableDateTime(ucCompletionDate.Text)
                                , General.GetNullableString(txtCancelReason.Text)
                                , General.GetNullableString(txtCloseOutRemarks.Text)
                                , (chkRCANotrequired.Checked == true ? 0 : 1)
                                , (chkRCAcompleted.Checked == true ? 1 : 0)
                                , General.GetNullableDateTime(ucRcaTargetDate.Text)
                                , null
                                , General.GetNullableString(txtKey.Text)
                                , General.GetNullableString(txtItem.Text)
                                , (chkCopyCAR.Checked == true ? 1 : 0)
                                , General.GetNullableString(txtAuditor.Text)
                                , General.GetNullableString(txtAuditPlace.Text)
                                , General.GetNullableDateTime(ucNCDuedate.Text)
                                , General.GetNullableInteger(ucAuditDept.SelectedDepartment)
                                , General.GetNullableString(txtReviewRemarks.Text)
                                , General.GetNullableString(txtCorrectiveAction.Text)
                                , General.GetNullableDateTime(ucCompletionDate.Text)
                                , General.GetNullableDateTime(ucReviewDate.Text)
                                , General.GetNullableDateTime(ucCloseoutDate.Text)
                                , General.GetNullableDateTime(ucCancelDate.Text)
                                );
                            ucStatus.Text = "NC Updated.";
                        }
                    }
                    else
                    {
                        ucError.Visible = true;
                        return;
                    }
                }
                BindData();
            }
            if (CommandName.ToUpper().Equals("CLOSE"))
            {
                if (ViewState["DEFICIENCYID"] != null && ViewState["DEFICIENCYID"].ToString() != string.Empty)
                {
                    if (IsValidDeficiency())
                    {
                        RadWindowManager1.RadConfirm("Are you sure you want to close this Deficiency?", "ConfirmClose", 320, 150, null, "ConfirmClose");
                        return;
                    }
                    else
                    {
                        ucError.Visible = true;
                        return;
                    }
                }

            }
            if (CommandName.ToUpper().Equals("CANCEL"))
            {
                if (ViewState["DEFICIENCYID"] != null && ViewState["DEFICIENCYID"].ToString() != string.Empty)
                {
                    if (IsValidDeficiency())
                    {
                        RadWindowManager1.RadConfirm("Are you sure you want to cancel this Deficiency?", "ConfirmCancel", 320, 150, null, "ConfirmCancel");
                        return;
                    }
                    else
                    {
                        ucError.Visible = true;
                        return;
                    }
                }
            }
            if (CommandName.ToUpper().Equals("REVIEW"))
            {
                if (ViewState["DEFICIENCYID"] != null && ViewState["DEFICIENCYID"].ToString() != string.Empty)
                {
                    if (ViewState["DEFICIENCYTYPE"].ToString() == "3" || ViewState["DEFICIENCYTYPE"].ToString() == "4")
                    {
                        //PhoenixInspectionDeficiency.InspectionObservationDeficiencyClose(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        //    , General.GetNullableGuid(ViewState["DEFICIENCYID"].ToString())
                        //    , General.GetNullableString(txtCloseOutRemarks.Text));
                        //ucStatus.Text = "Observation is Closed.";
                    }
                    if (ViewState["DEFICIENCYTYPE"].ToString() == "1" || ViewState["DEFICIENCYTYPE"].ToString() == "2")
                    {
                        PhoenixInspectionDeficiency.UpdateInspectionNCDeficiencyReviewStatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , General.GetNullableGuid(ViewState["DEFICIENCYID"].ToString())
                     , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 146, "REV"))
                     , General.GetNullableString(txtReviewRemarks.Text));

                        ucStatus.Text = "NC is Reviewed.";
                    }
                    BindData();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {
        if (ViewState["DEFICIENCYID"] != null && ViewState["DEFICIENCYID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionAuditNonConformity.EditAuditNC(new Guid(ViewState["DEFICIENCYID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                rblDeficiencyType.Enabled = true;
                if (dr["FLDRAISEDFROM"].ToString() == "3") //open reports
                {
                    rblDeficiencyType.Items.Clear();
                    rblDeficiencyType.DataSource = null;
                    rblDeficiencyType.Items.Insert(0, new ButtonListItem("NC", "2"));
                    rblDeficiencyType.Items.Insert(1, new ButtonListItem("Major NC", "1"));
                    rblDeficiencyType.Enabled = true;
                    ddlSchedule.Enabled = false;
                    txtChecklistRef.Enabled = false;
                }
                if (dr["FLDNONCONFORMITYTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 173, "MAJ"))
                {
                    rblDeficiencyType.SelectedValue = "1";
                    ViewState["DEFICIENCYTYPE"] = "1";
                }
                else if (dr["FLDNONCONFORMITYTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 173, "MIN"))
                {
                    rblDeficiencyType.SelectedValue = "2";
                    ViewState["DEFICIENCYTYPE"] = "2";
                }

                ViewState["RAISEDFROM"] = dr["FLDRAISEDFROM"].ToString();
                ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                ucVessel.bind();
                ucVessel.Enabled = false;
                ucCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();
                ViewState["SELECTEDCOMPANYID"] = dr["FLDCOMPANYID"].ToString();
                ucDate.Text = dr["FLDDATE"].ToString();
                BindSource();
                if (dr["FLDREVIEWSCHEDULEID"] != null && dr["FLDREVIEWSCHEDULEID"].ToString() != string.Empty)
                    ddlSchedule.SelectedValue = dr["FLDREVIEWSCHEDULEID"].ToString();
                else
                    ddlSchedule.SelectedIndex = 0;
                ucNonConformanceCategory.SelectedQuick = dr["FLDNCCATEGORYID"].ToString();
                txtChecklistRef.Text = dr["FLDCHECKLISTREFERENCENUMBER"].ToString();
                txtDesc.Text = dr["FLDCOMPREHENSIVEDESCRIPTION"].ToString();
                txtInspectorComments.Text = dr["FLDINSPECTORCOMMENTS"].ToString();
                txtMasterComments.Text = dr["FLDFOLLOWUPREMARKS"].ToString();
                txtCorrectiveAction.Text = dr["FLDTASKCORRECTIVEACTION"].ToString();
                txtCorrectiveAction.Text = txtCorrectiveAction.Text.Replace("</BR>", Environment.NewLine);
                ddlStatus.SelectedHard = dr["FLDSTATUS"].ToString();
                ddlStatus.bind();
                //setEnabledDisabled();
                ucCompletionDate.Text = dr["FLDNCCOMPLETIONDATE"].ToString();
                ucCloseoutDate.Text = dr["FLDCLOSEDATE"].ToString();
                txtCloseOutByName.Text = dr["FLDCLOSEOUTBYNAME"].ToString();
                txtCloseOutByDesignation.Text = dr["FLDCLOSEOUTBYDESIGNATION"].ToString();
                txtCloseOutRemarks.Text = dr["FLDCLOSEOUTREMARKS"].ToString();
                txtCancelReason.Text = dr["FLDNCCANCELREASON"].ToString();
                ucCancelDate.Text = dr["FLDCANCELDATE"].ToString();
                txtCancelledByName.Text = dr["FLDCANCELLEDBYNAME"].ToString();
                txtCancelledByDesignation.Text = dr["FLDCANCELLEDBYDESIGNATION"].ToString();
                if (dr["FLDNCISRCACOMPLETED"].ToString() == "1")
                    chkRCAcompleted.Checked = true;
                else
                    chkRCAcompleted.Checked = false;
                if (dr["FLDNCISRCAREQUIRED"].ToString() == "0")
                {
                    chkRCANotrequired.Checked = true;
                    chkRCAcompleted.Enabled = false;
                    chkRCAcompleted.Checked = false;
                    ucRcaTargetDate.Enabled = false;
                }
                else
                {
                    //chkRCAcompleted.Enabled = true;
                    SetRights();
                    ucRcaTargetDate.Text = dr["FLDTARGETDATE"].ToString();

                }
                txtChapterNo.Text = dr["FLDCHAPTERNUMBER"].ToString();
                //ddlItem.SelectedValue = dr["FLDNCITEM"].ToString();
                txtKey.Text = dr["FLDNCKEY"].ToString();
                txtKeyName.Text = dr["FLDKEYNAME"].ToString();
                txtItem.Text = dr["FLDNCITEMTEXT"].ToString();

                if (dr["FLDCOPYTOCAR"].ToString() == "1")
                    chkCopyCAR.Checked = true;
                else
                    chkCopyCAR.Checked = false;
                ltDefDetails.Text = dr["FLDDEFICIENCYDETAILS"].ToString();
                txtStatus.Text = dr["FLDSTATUSNAME"].ToString();
                txtAuditor.Text = dr["FLDAUDITORNAME"].ToString();
                txtAuditPlace.Text = dr["FLDPLACEOFAUDIT"].ToString();
                txtReviewRemarks.Text = dr["FLDREVIEWEDREMARKS"].ToString();
                txtReviewedByName.Text = dr["FLDREVIEWEDBY"].ToString();
                ucReviewDate.Text = dr["FLDREVIEWEDDATE"].ToString();
                ucAuditDept.SelectedDepartment = dr["FLDAUDITORDEPARTMENTID"].ToString();
                ucNCDuedate.Text = dr["FLDNCDUEDATE"].ToString();
                txtReviewedByDesignation.Text = dr["FLDREVIEWEDBYDESIGNATION"].ToString();
                txtCompCode.Text = dr["FLDCOMPCODE"].ToString();

                txtAuditor.Text = dr["FLDAUDITORNAME1"].ToString();
                txtAuditPlace.Text = dr["FLDPLACEOFAUDIT1"].ToString();

                lblCARNotRequired.Visible = false;
                chkCARNotRequiredYN.Visible = false;
            }

            DataSet ds1 = PhoenixInspectionObservation.EditDirectObservation(new Guid(ViewState["DEFICIENCYID"].ToString()));
            if (ds1.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds1.Tables[0].Rows[0];
                if (dr["FLDOBSERVATIONTYPE"].ToString().Equals("1"))
                {
                    rblDeficiencyType.SelectedValue = "3";
                    ViewState["DEFICIENCYTYPE"] = "3";
                }
                else if (dr["FLDOBSERVATIONTYPE"].ToString().Equals("2"))
                {
                    ViewState["DEFICIENCYTYPE"] = "4";
                    rblDeficiencyType.SelectedValue = "4";
                }

                if (dr["FLDRAISEDFROM"].ToString() == "4")
                {
                    ddlSchedule.Enabled = false;
                    txtChecklistRef.Enabled = false;
                }

                ViewState["RAISEDFROM"] = dr["FLDRAISEDFROM"].ToString();
                rblDeficiencyType.Enabled = true;

                ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                ucVessel.bind();
                ucVessel.Enabled = false;
                ucCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();
                ViewState["SELECTEDCOMPANYID"] = dr["FLDCOMPANYID"].ToString();
                ucDate.Text = dr["FLDDATE"].ToString();
                BindSource();
                if (dr["FLDINSPECTIONSCHEDULEID"] != null && dr["FLDINSPECTIONSCHEDULEID"].ToString() != string.Empty)
                    ddlSchedule.SelectedValue = dr["FLDINSPECTIONSCHEDULEID"].ToString();
                else
                    ddlSchedule.SelectedIndex = 0;
                ucNonConformanceCategory.SelectedQuick = dr["FLDRISKCATEGORYID"].ToString();
                txtChecklistRef.Text = dr["FLDCHECKLISTREFERENCENUMBER"].ToString();
                txtDesc.Text = dr["FLDOBSERVATION"].ToString();
                txtInspectorComments.Text = dr["FLDCOMMENTS"].ToString();
                txtMasterComments.Text = dr["FLDOWNERCOMMENTS"].ToString();
                txtCorrectiveAction.Text = dr["FLDTASKCORRECTIVEACTION"].ToString();
                txtCorrectiveAction.Text = txtCorrectiveAction.Text.Replace("</BR>", Environment.NewLine);
                ddlStatus.SelectedHard = dr["FLDSTATUS"].ToString();
                ddlStatus.bind();
                //setEnabledDisabled();
                ucCompletionDate.Text = dr["FLDCOMPLETIONDATE"].ToString();
                ucCloseoutDate.Text = dr["FLDCLOSEDATE"].ToString();
                txtCloseOutByName.Text = dr["FLDCLOSEOUTBYNAME"].ToString();
                txtCloseOutByDesignation.Text = dr["FLDCLOSEOUTBYDESIGNATION"].ToString();
                txtCloseOutRemarks.Text = dr["FLDCLOSEOUTREMARKS"].ToString();
                txtCancelReason.Text = dr["FLDOBSCANCELREASON"].ToString();
                ucCancelDate.Text = dr["FLDCANCELDATE"].ToString();
                txtCancelledByName.Text = dr["FLDCANCELLEDBYNAME"].ToString();
                txtCancelledByDesignation.Text = dr["FLDCANCELLEDBYDESIGNATION"].ToString();
                if (dr["FLDOBSISRCACOMPLETED"].ToString() == "1")
                {
                    chkRCAcompleted.Checked = true;
                }
                else
                {
                    chkRCAcompleted.Checked = false;
                }
                if (dr["FLDOBSISRCAREQUIRED"].ToString() == "0")
                {
                    chkRCANotrequired.Checked = true;
                    chkRCAcompleted.Enabled = false;
                    chkRCAcompleted.Checked = false;
                    ucRcaTargetDate.Enabled = false;
                }
                else
                {
                    //chkRCAcompleted.Enabled = true;
                    SetRights();
                    ucRcaTargetDate.Text = dr["FLDTARGETDATE"].ToString();
                }

                txtChapterNo.Text = dr["FLDCHAPTERNUMBER"].ToString();
                //ddlItem.SelectedValue = dr["FLDOBSITEM"].ToString();
                txtKey.Text = dr["FLDOBSKEY"].ToString();
                txtKeyName.Text = dr["FLDKEYNAME"].ToString();
                txtItem.Text = dr["FLDOBSITEMTEXT"].ToString();

                if (dr["FLDCOPYTOCAR"].ToString() == "1")
                    chkCopyCAR.Checked = true;
                else
                    chkCopyCAR.Checked = false;
                ltDefDetails.Text = dr["FLDDEFICIENCYDETAILS"].ToString();
                txtStatus.Text = dr["FLDSTATUSNAME"].ToString();
                txtAuditor.Text = dr["FLDAUDITORNAME"].ToString();
                txtAuditPlace.Text = dr["FLDPLACEOFAUDIT"].ToString();
                txtReviewRemarks.Text = dr["FLDREVIEWEDREMARKS"].ToString();
                txtReviewedByName.Text = dr["FLDREVIEWEDBY"].ToString();
                ucReviewDate.Text = dr["FLDREVIEWEDDATE"].ToString();
                ucAuditDept.SelectedDepartment = dr["FLDAUDITORDEPARTMENT"].ToString();
                ucNCDuedate.Text = dr["FLDOBSDUEDATE"].ToString();
                txtReviewedByDesignation.Text = dr["FLDREVIEWEDBYDESIGNATION"].ToString();
                txtCompCode.Text = dr["FLDCOMPCODE"].ToString();

                txtAuditor.Text = dr["FLDAUDITORNAME1"].ToString();
                txtAuditPlace.Text = dr["FLDPLACEOFAUDIT1"].ToString();



                if (dr["FLDOBSISCARNOTREQUIRED"].ToString() == "1")
                {
                    chkCARNotRequiredYN.Checked = true;
                }
                else
                {
                    chkCARNotRequiredYN.Checked = false;
                }

                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    lblCARNotRequired.Visible = false;
                    chkCARNotRequiredYN.Visible = false;
                }
                else
                {
                    lblCARNotRequired.Visible = true;
                    chkCARNotRequiredYN.Visible = true;
                }
            }
        }
    }

    protected void ddlStatus_Changed(object sender, EventArgs e)
    {
        //setEnabledDisabled();
    }

    protected void setEnabledDisabled()
    {
        if (ddlStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 146, "CLD"))
            txtCloseOutRemarks.Enabled = true;
        else
            txtCloseOutRemarks.Enabled = false;

        if (ddlStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 146, "CAD"))
            txtCancelReason.Enabled = true;
        else
            txtCancelReason.Enabled = false;
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
            , General.GetNullableInteger(ViewState["SELECTEDCOMPANYID"].ToString())
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
    }

    protected void company_TextChanged(object sender, EventArgs e)
    {
        BindSource();
    }
    protected void DeficiencyType_TextChanged(object sender, EventArgs e)
    {
        //BindSource();
    }

    private bool IsValidDeficiency()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableInteger(rblDeficiencyType.SelectedValue) == null)
            ucError.ErrorMessage = "Deficiency Type is required.";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableInteger(ucNonConformanceCategory.SelectedQuick) == null)
            ucError.ErrorMessage = "Deficiency Category is required.";

        if (General.GetNullableDateTime(ucDate.Text).Equals("") || General.GetNullableDateTime(ucDate.Text).Equals(null))
            ucError.ErrorMessage = "Issue date is required.";

        //if (General.GetNullableInteger(ddlStatus.SelectedHard) == null)
        //    ucError.ErrorMessage = "Status is required.";
        //else
        //{
        //    if (ddlStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 146, "CMP"))
        //    {
        //        if (General.GetNullableDateTime(ucCompletionDate.Text) == null)
        //            ucError.ErrorMessage = "Completion Date is required.";
        //        else if (General.GetNullableDateTime(ucCompletionDate.Text) > DateTime.Today)
        //            ucError.ErrorMessage = "Completion Date cannot be the future date.";
        //    }
        //    else if (ddlStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 146, "CAD"))
        //    {
        //        if (General.GetNullableString(txtCancelReason.Text) == null)
        //            ucError.ErrorMessage = "Cancel Reason is required.";
        //    }
        //}

        if (General.GetNullableDateTime(ucDate.Text) > DateTime.Today)
            ucError.ErrorMessage = "Issue date should not be greater than current date.";

        if (General.GetNullableString(txtDesc.Text) == null)
            ucError.ErrorMessage = "Description is required.";

        if (General.GetNullableDateTime(ucNCDuedate.Text) == null)
            ucError.ErrorMessage = "NCN/Obs Due Date is required";

        return (!ucError.IsError);
    }
    private bool IsValidDeficiencyCancel()
    {
        ucError.HeaderMessage = "Please provide the following required information.";
        if (General.GetNullableString(txtCancelReason.Text) == null)
            ucError.ErrorMessage = "Cancel Reason is required.";
        return (!ucError.IsError);
    }

    protected void chkRCANotrequired_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRCANotrequired.Checked.Equals(true))
        {
            chkRCAcompleted.Enabled = false;
            chkRCAcompleted.Checked = false;
            ucRcaTargetDate.Text = "";
            ucRcaTargetDate.Enabled = false;

        }
        else
        {
            //chkRCAcompleted.Enabled = true;
            SetRights();
            if (ucDate.Text != null)
            {
                DateTime ucIssueDate = DateTime.Parse(ucDate.Text);
                DateTime ucRCATargetdate = ucIssueDate.AddDays(days);
                ucRcaTargetDate.Text = ucRCATargetdate.ToString();
            }
        }
    }

    protected void ucIssueDateEdit_TextChanged(object sender, EventArgs e)
    {
        if (ucDate.Text != null)
        {
            DateTime ucIssueDate = DateTime.Parse(ucDate.Text);
            DateTime ucRCATargetdate = ucIssueDate.AddDays(days);
            ucRcaTargetDate.Text = ucRCATargetdate.ToString();
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

    protected void ucConfirmClose_Click(object sender, EventArgs e)
    {
        try
        {
            if (rblDeficiencyType.SelectedValue == "3" || rblDeficiencyType.SelectedValue == "4")
                {
                    PhoenixInspectionDeficiency.InspectionObservationDeficiencyClose(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid(ViewState["DEFICIENCYID"].ToString())
                        , General.GetNullableString(txtCloseOutRemarks.Text)
                        , General.GetNullableDateTime(ucCloseoutDate.Text));
                    ucStatus.Text = "Observation is Closed.";
                }
                else
                {
                    PhoenixInspectionDeficiency.InspectionDeficiencyClose(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid(ViewState["DEFICIENCYID"].ToString())
                        , General.GetNullableString(txtCloseOutRemarks.Text)
                        , General.GetNullableDateTime(ucCloseoutDate.Text));

                    ucStatus.Text = "NC is Closed.";
                }
                BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmCancel_Click(object sender, EventArgs e)
    {
        try
        {
            int type = 0;
            if (IsValidDeficiencyCancel())
            {
                if (rblDeficiencyType.SelectedValue == "3" || rblDeficiencyType.SelectedValue == "4")
                    type = 4;
                else
                    type = 3;
                if (General.GetNullableString(txtCancelReason.Text) == null)
                {
                    ucError.Visible = true;
                    ucError.ErrorMessage = "Cancel reason required";
                }
                else
                {
                    PhoenixInspectionIncident.CancelReasonUpdate(
                                                                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                     , type
                                                                     , new Guid(ViewState["DEFICIENCYID"].ToString())
                                                                     , txtCancelReason.Text);

                    ucStatus.Text = "Status Updated.";
                }
                BindData();
            }
            else
            {
                ucError.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
