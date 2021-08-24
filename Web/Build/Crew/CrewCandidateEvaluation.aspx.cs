using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewCandidateEvaluation : PhoenixBasePage
{
    string intvid = string.Empty;
    string status = string.Empty;
    string proposalstatus = string.Empty;
    protected string m_TargetPageTxt = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        dlstEvaluation.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                         Convert.ToInt32(PhoenixHardTypeCode.EVALUATION));
        dlstEvaluation.DataKeyField = "FLDHARDCODE";
        dlstEvaluation.DataBind();

        intvid = Request.QueryString["intvid"];
        status = Request.QueryString["status"];
        proposalstatus = Request.QueryString["proposalstatus"];

        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            confirm.Attributes.Add("style", "display:none");
            SessionUtil.PageAccessRights(this.ViewState);

            ViewState["ISRATING"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ViewState["CVSL"] = -1;
            ViewState["CRNK"] = -1;
            ViewState["CREWINTERVIEWID"] = string.Empty;
            ViewState["ATTACHMENTCODE"] = string.Empty;
            ViewState["APPROVED"] = string.Empty;
            ViewState["VESSEL"] = string.Empty;
            ViewState["JOINDATE"] = string.Empty;
            ViewState["RANK"] = string.Empty;
            ViewState["RANKPOSTED"] = string.Empty;
            ucConfirmProposal.Visible = false;

            SetEmployeePrimaryDetails();
            if (!string.IsNullOrEmpty(intvid))
                SetCrewInterviewSummary();
            if (!string.IsNullOrEmpty(status))
            {
                if (status.ToUpper() != "TENTATIVE")
                {
                    if (status.ToUpper() != "APPROVED" || status.ToUpper() != "PROPOSED")
                        DisableForm();
                }
            }

            gvInterviewBy.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }


        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        if (ViewState["CREWINTERVIEWID"].ToString() != string.Empty)
        {
            toolbarsub.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=CHECKLIST&employeeid="
                + Filter.CurrentCrewSelection
                + "&vesselid=" + ViewState["VESSEL"]
                + "&joindate=" + ViewState["JOINDATE"]
                + "&rankid=" + ViewState["RANK"]
                + "&planid=" + ViewState["PLANID"] + "&showmenu=1'); return false;", "Checklist", "", "CHECKLIST", ToolBarDirection.Right);

            toolbarsub.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=OWNERPROPOSAL&employeeid="
                  + Filter.CurrentCrewSelection
                  + "&showmenu=1'); return false;", "Owner Proposal", "", "OWNERPROPOSAL", ToolBarDirection.Right);

            toolbarsub.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDOWNER&employeeid="
               + Filter.CurrentCrewSelection
               + "&vslid=" + ViewState["VESSEL"] + "&rankid=" + ViewState["RANK"] + "&showmenu=1'); return false;", "Owner PD", "", "PDOWNER", ToolBarDirection.Right);

            if (proposalstatus == "Open")
            {
                lblnote.Visible = false;
                toolbarsub.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=EMPLOYEECV&employeeid="
                    + Filter.CurrentCrewSelection
                    + "&vslid=" + ViewState["VESSEL"] + "&rankid=" + ViewState["RANK"] + "&showmenu=1'); return false;", "PDForm", "", "PDFORM", ToolBarDirection.Right);
            }

            toolbarsub.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["ATTACHMENTCODE"]
                + "&mod=" + PhoenixModule.CREW
                + "&type=" + PhoenixCrewAttachmentType.ASSESSMENT
                + (!string.IsNullOrEmpty(ViewState["APPROVED"].ToString()) ? "&u=n" : string.Empty)
                + "&ratingyn=" + ViewState["ISRATING"].ToString()
                + "'); return false;", "Follow Up Sheet", "", "ATTACHMENT", ToolBarDirection.Right);

            PhoenixToolbar toolbarCopy = new PhoenixToolbar();
            toolbarCopy.AddButton("Copy", "COPY", ToolBarDirection.Right);
            CopyProposal.MenuList = toolbarCopy.Show();
        }
        else
            rblStatus.Enabled = false;

        if (string.IsNullOrEmpty(ViewState["APPROVED"].ToString()))
        {
            toolbarsub.AddButton("Save", "SAVE", ToolBarDirection.Right);
            txtOffsigner.Visible = false;
            ddlOffSigner.Visible = true;
        }
        else
        {
            txtOffsigner.Visible = true;
            ddlOffSigner.Visible = false;
        }

        CrewMenuGeneral.MenuList = toolbarsub.Show();
        //BindData();
        //BindRecommendedCourses();
        //BindLastVesselDetails();
    }

    private void BindRecommendedCourses()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = PhoenixCrewRecommendedCourse.CrewRecommendedCourseSearch(General.GetNullableInteger(Filter.CurrentCrewSelection)
                        , null, null
                        , 1
                        , General.ShowRecords(null)
                        , ref iRowCount
                        , ref iTotalPageCount);

            gvCrewRecommendedCourses.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindLastVesselDetails()
    {
        try
        {

            DataTable dt = PhoenixNewApplicantInterviewSummary.EmployeeLastVesselList(int.Parse(Filter.CurrentCrewSelection.ToString()));

            gvLastVessel.DataSource = dt;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            int res = 0;

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (!IsValidate(ddlVessel.SelectedVessel))
            {
                ucError.Visible = true;
                return;
            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["CREWINTERVIEWID"].ToString() == string.Empty)
                {
                    SaveCrewInterviewSummary(null);
                    SetCrewInterviewSummary();
                    PhoenixToolbar toolbarsub = new PhoenixToolbar();

                    if (ViewState["CREWINTERVIEWID"].ToString() != string.Empty)
                    {
                        toolbarsub.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=CHECKLIST&employeeid="
                         + Filter.CurrentCrewSelection
                         + "&vesselid=" + ViewState["VESSEL"]
                         + "&joindate=" + ViewState["JOINDATE"]
                         + "&rankid=" + ViewState["RANK"] + "&showmenu=1'); return false;", "Checklist", "", "CHECKLIST", ToolBarDirection.Right);

                        DataSet ds = PhoenixNewApplicantInterviewSummary.GetTopOfficerRank(General.GetNullableInteger(ViewState["RANK"].ToString()), ref res);
                        if (res == 1)
                        {
                            toolbarsub.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDOWNER&employeeid="
                                + Filter.CurrentCrewSelection
                                + "&vslid=" + ViewState["VESSEL"] + "&rankid=" + ViewState["RANK"] + "&showmenu=1'); return false;", "Owner PD", "", "PDOWNER", ToolBarDirection.Right);
                        }
                        toolbarsub.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=EMPLOYEECV&employeeid="
                        + Filter.CurrentCrewSelection + "&vslid=" + ViewState["VESSEL"] + "&rankid=" + ViewState["RANK"] + "&showmenu=1'); return false;", "PDForm", "", "PDFORM", ToolBarDirection.Right);

                        toolbarsub.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["ATTACHMENTCODE"]
                            + "&mod=" + PhoenixModule.CREW
                            + "&type=" + PhoenixCrewAttachmentType.ASSESSMENT
                            + (!string.IsNullOrEmpty(ViewState["APPROVED"].ToString()) ? "&u=n" : string.Empty)
                            + "&ratingyn=" + ViewState["ISRATING"].ToString()
                            + "'); return false;", "Follow Up Sheet", "", "ATTACHMENT", ToolBarDirection.Right);

                        rblStatus.Enabled = true;

                        PhoenixToolbar toolbarCopy = new PhoenixToolbar();
                        toolbarCopy.AddButton("Copy", "COPY", ToolBarDirection.Right);
                        CopyProposal.MenuList = toolbarCopy.Show();
                    }
                    else
                    {
                        rblStatus.Enabled = false;
                    }

                    if (string.IsNullOrEmpty(ViewState["APPROVED"].ToString()))
                    {
                        toolbarsub.AddButton("Save", "SAVE", ToolBarDirection.Right);
                    }

                    CrewMenuGeneral.MenuList = toolbarsub.Show();
                    BindData();
                    gvInterviewBy.Rebind();
                }
                else
                {
                    if (rblStatus.SelectedItem != null)
                    {
                        rblStatus_SelectedIndexChanged(null, null);
                    }
                    else
                        UpdateCrewInterviewSummary(null, 1);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CopyProposal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("COPY"))
            {
                CopyCrewInterviewSummary();
            }

            SetCrewInterviewSummary();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void OnBackCheckedClick(object sender, EventArgs e)
    {
        if (chkBackedChecked.Checked == true)
        {
            txtBackChk.ReadOnly = false;
            txtBackChk.CssClass = "";
        }
        else
        {
            txtBackChk.ReadOnly = true;
            txtBackChk.CssClass = "readonlytextbox";
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        SetCrewInterviewSummary();
    }

    public void SaveCrewInterviewSummary(Guid? gCrewPlanId)
    {
        string Script = "";
        Script += "fnReloadList(null,'ifMoreInfo','keepopen');";

        string strIntialInterView = (chkOrgLicSig.Checked == true ? "1" : "0") + "," + (chkCertSig.Checked == true ? "1" : "0") + ","
            + (chkExpConf.Checked == true ? "1" : "0") + "," + (chkOtherDet.Checked == true ? "1" : "0") + "," + (chkAddressLatest.Checked == true ? "1" : "0");
        string strIntailCheckedBy = txtOrgLicSigCB.Text + "," + txtCertSigCB.Text + "," + txtExpConfCB.Text + "," + txtOtherDetCB.Text + "," + txtAddressLatest.Text;
        string strEvaluation = string.Empty;
        string strFollowupcheklist = chkFlagDoc.SelectedValue + "," + chkPassport.SelectedValue + "," + chkCOC.SelectedValue + "," + chkGOC.SelectedValue + "," + chkUSVisa.SelectedValue + "," + chkCDCRenewed.SelectedValue;
        string strFollowupCheckedBy = txtCheckFlagDoc.Text + "," + txtCheckPassport.Text + "," + txtCheckCOC.Text + "," + txtCheckGOC.Text + "," + txtCheckUSVisa.Text + "," + txtCDCRenewedCB.Text;
        foreach (DataListItem item in dlstEvaluation.Items)
        {
            RadRadioButtonList rbtn = (RadRadioButtonList)item.FindControl("rblEvaluation");
            strEvaluation += (rbtn.SelectedItem == null ? string.Empty : rbtn.SelectedItem.Value) + ",";
        }

        strEvaluation = strEvaluation.Substring(0, strEvaluation.Length - 1);

        byte? s = null;
        if (rblStatus.SelectedItem != null)
            s = byte.Parse(rblStatus.SelectedItem.Value);

        int interviewid = 0;

        interviewid = PhoenixNewApplicantInterviewSummary.InsertNewApplicantInterviewSummary(Convert.ToInt32(Filter.CurrentCrewSelection)
                                                                                , strIntialInterView
                                                                                , strIntailCheckedBy
                                                                                , txtChnageReason.Text
                                                                                , txtIncedents.Text
                                                                                , txtDOA.Text
                                                                                //, txtInterviewBy.Text
                                                                                //, Convert.ToDateTime(txtInterviewDate.Text)
                                                                                , strEvaluation
                                                                                , txtSuperintendentName.Text
                                                                                , General.GetNullableDateTime(txtDate.Text)
                                                                                , txtRemaks.Text
                                                                                , s
                                                                                , chkBackedChecked.Checked == true ? byte.Parse("1") : byte.Parse("0")
                                                                                , txtCompliedBy.Text
                                                                                , General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                                , txtDockAttended.Text
                                                                                , txtVettingDone.Text
                                                                                , txtCargoCarried.Text
                                                                                , txtSalAgreed.Text
                                                                                , int.Parse(ViewState["RANKPOSTED"].ToString())
                                                                                , General.GetNullableInteger(ddlOffSigner.SelectedCrew)
                                                                                , DateTime.Parse(txtJoinDate.Text)
                                                                                , gCrewPlanId
                                                                                , null
                                                                                , chkDocsinHand.Checked == true ? byte.Parse("1") : byte.Parse("0")
                                                                                , General.GetNullableString(txtBackChk.Text)
                                                                                , txtHandremarks1.Text
                                                                                , txtHandremarks2.Text
                                                                                , txtHandremarks3.Text
                                                                                , txtHandremarks4.Text
                                                                                , strFollowupcheklist
                                                                                , strFollowupCheckedBy
                                                                                , Convert.ToInt32(ucRatingExp.SelectedHard)
                                                                                , General.GetNullableInteger(txtJoinOfficerRankid.Text)
                                                                                , General.GetNullableDecimal(txtExperience.Text)
                                                                                , General.GetNullableString(txtRemarksOfficer.Text)
                                                                                , int.Parse(ddlRank.SelectedRank)
                                                                                , chkProposing.Checked == true ? byte.Parse("1") : byte.Parse("0")
                                                                                , txtGeneralRemarks.Content
                                                                                );
        ViewState["CREWINTERVIEWID"] = interviewid;
        ucStatus.Text = "Assessment Information Updated.";

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, true);
    }

    private void CopyCrewInterviewSummary()
    {
        try
        {
            string Script = "";
            Script += "fnReloadList(null,'ifMoreInfo','keepopen');";

            int interviewid = 0;
            int newid = 0;
            interviewid = PhoenixNewApplicantInterviewSummary.CopyNewApplicantInterviewSummary(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , Convert.ToInt32(Filter.CurrentCrewSelection)
                                                                                        , Convert.ToInt32(ViewState["CREWINTERVIEWID"].ToString())
                                                                                        , ref newid);
            ViewState["CREWINTERVIEWID"] = newid;
            ViewState["APPROVED"] = string.Empty;

            ucStatus.Text = "Assessment Information Copied.";

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, true);
            EnableForm();
            SetCrewInterviewSummary();

            rblStatus.Enabled = true;
            rblStatus.SelectedIndex = -1;

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            int res = 0;

            if (ViewState["CREWINTERVIEWID"].ToString() != string.Empty)
            {
                toolbarsub.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=CHECKLIST&employeeid="
               + Filter.CurrentCrewSelection
               + "&vesselid=" + ViewState["VESSEL"]
               + "&joindate=" + ViewState["JOINDATE"]
               + "&rankid=" + ViewState["RANK"]
               + "&planid=" + ViewState["PLANID"] + "&showmenu=1'); return false;", "Checklist", "", "CHECKLIST", ToolBarDirection.Right);

                DataSet ds = PhoenixNewApplicantInterviewSummary.GetTopOfficerRank(int.Parse(ViewState["RANK"].ToString()), ref res);
                if (res == 1)
                    toolbarsub.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDOWNER&employeeid="
                        + Filter.CurrentCrewSelection
                        + "&vslid=" + ViewState["VESSEL"] + "&rankid=" + ViewState["RANK"] + "&showmenu=1'); return false;", "Owner PD", "", "PDOWNER", ToolBarDirection.Right);

                toolbarsub.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=EMPLOYEECV&employeeid="
               + Filter.CurrentCrewSelection
               + "&vslid=" + ViewState["VESSEL"] + "&rankid=" + ViewState["RANK"] + "&showmenu=1'); return false;", "PDForm", "", "PDFORM", ToolBarDirection.Right);

                toolbarsub.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["ATTACHMENTCODE"]
                    + "&mod=" + PhoenixModule.CREW
                    + "&type=" + PhoenixCrewAttachmentType.ASSESSMENT
                    + (!string.IsNullOrEmpty(ViewState["APPROVED"].ToString()) ? "&u=n" : string.Empty)
                    + "'); return false;", "Follow Up Sheet", "", "ATTACHMENT", ToolBarDirection.Right);
           
                PhoenixToolbar toolbarCopy = new PhoenixToolbar();
                toolbarCopy.AddButton("Copy", "COPY", ToolBarDirection.Right);
                CopyProposal.MenuList = toolbarCopy.Show();
            }
            else
            {
                rblStatus.Enabled = false;
            }

            if (string.IsNullOrEmpty(ViewState["APPROVED"].ToString()))
            {
                toolbarsub.AddButton("Save", "SAVE", ToolBarDirection.Right);
                txtOffsigner.Visible = false;
                ddlOffSigner.Visible = true;
            }
            else
            {
                txtOffsigner.Visible = true;
                ddlOffSigner.Visible = false;
            }


            CrewMenuGeneral.MenuList = toolbarsub.Show();

            BindData();
            gvInterviewBy.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UpdateCrewInterviewSummary(Guid? gPlanId, int? type)
    {
        byte? s = null;
        if (rblStatus.SelectedItem != null)
            s = byte.Parse(rblStatus.SelectedItem.Value);
        UpdateCrewInterviewSummary(gPlanId, s, type);
    }

    private void UpdateCrewInterviewSummary(Guid? gPlanId, byte? s, int? type)
    {
        string Script = "";
        Script += "fnReloadList(null,'ifMoreInfo','keepopen');";
        string strIntialInterView = (chkOrgLicSig.Checked == true ? "1" : "0") + "," + (chkCertSig.Checked == true ? "1" : "0") + ","
            + (chkExpConf.Checked == true ? "1" : "0") + "," + (chkOtherDet.Checked == true ? "1" : "0") + (chkAddressLatest.Checked == true ? "1" : "0");
        string strIntailCheckedBy = txtOrgLicSigCB.Text + "," + txtCertSigCB.Text + "," + txtExpConfCB.Text + "," + txtOtherDetCB.Text + "," + txtAddressLatest.Text;
        string strEvaluation = string.Empty;
        string strFollowupcheklist = chkFlagDoc.SelectedValue + "," + chkPassport.SelectedValue + "," + chkCOC.SelectedValue + "," + chkGOC.SelectedValue + "," + chkUSVisa.SelectedValue + "," + chkCDCRenewed.SelectedValue;
        string strFollowupCheckedBy = txtCheckFlagDoc.Text + "," + txtCheckPassport.Text + "," + txtCheckCOC.Text + "," + txtCheckGOC.Text + "," + txtCheckUSVisa.Text + "," + txtCDCRenewedCB.Text;
        foreach (DataListItem item in dlstEvaluation.Items)
        {
            RadRadioButtonList rbtn = (RadRadioButtonList)item.FindControl("rblEvaluation");
            strEvaluation += (rbtn.SelectedItem == null ? string.Empty : rbtn.SelectedItem.Value) + ",";
        }
        try
        {
            int iMessageCode = 0;
            string iMessageText = "";
            PhoenixNewApplicantInterviewSummary.UpdateNewApplicantInterviewSummary(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , Convert.ToInt32(Filter.CurrentCrewSelection)
                                                                                    , Convert.ToInt32(ViewState["CREWINTERVIEWID"].ToString())
                                                                                    , strIntialInterView
                                                                                    , strIntailCheckedBy
                                                                                    , txtChnageReason.Text
                                                                                    , txtIncedents.Text
                                                                                    , txtDOA.Text
                                                                                    , strEvaluation
                                                                                    , txtSuperintendentName.Text
                                                                                    , General.GetNullableDateTime(txtDate.Text)
                                                                                    , txtRemaks.Text
                                                                                    , s
                                                                                    , chkBackedChecked.Checked == true ? byte.Parse("1") : byte.Parse("0")
                                                                                    , txtCompliedBy.Text
                                                                                    , General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                                    , txtDockAttended.Text
                                                                                    , txtVettingDone.Text
                                                                                    , txtCargoCarried.Text
                                                                                    , txtSalAgreed.Text
                                                                                    , General.GetNullableInteger(ddlOffSigner.SelectedCrew)
                                                                                    , DateTime.Parse(txtJoinDate.Text)
                                                                                    , null
                                                                                    , chkDocsinHand.Checked == true ? byte.Parse("1") : byte.Parse("0")
                                                                                    , General.GetNullableString(txtBackChk.Text)
                                                                                    , txtHandremarks1.Text
                                                                                    , txtHandremarks2.Text
                                                                                    , txtHandremarks3.Text
                                                                                    , txtHandremarks4.Text
                                                                                    , strFollowupcheklist
                                                                                    , strFollowupCheckedBy
                                                                                    , Convert.ToInt32(ucRatingExp.SelectedHard)
                                                                                    , General.GetNullableInteger(txtJoinOfficerRankid.Text)
                                                                                    , General.GetNullableDecimal(txtExperience.Text)
                                                                                    , General.GetNullableString(txtRemarksOfficer.Text)
                                                                                    , int.Parse(ddlRank.SelectedRank)
                                                                                    , chkProposing.Checked == true ? byte.Parse("1") : byte.Parse("0")
                                                                                    , ucConfirmProposal.confirmboxvalue, ref iMessageCode, ref iMessageText
                                                                                    , type
                                                                                    , General.GetNullableInteger(ddlPool.SelectedPool)
                                                                                    , txtGeneralRemarks.Content
                                                                                    );
            if (iMessageCode == 1)
                throw new ApplicationException(iMessageText);
            SetCrewInterviewSummary();
            ucStatus.Text = (rblStatus.SelectedItem != null && (rblStatus.SelectedValue == "1" || rblStatus.SelectedValue == "2")) ? "Seafarer Proposed." : "Assessment Information Updated.";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, true);
        }
        catch (ApplicationException aex)
        {
            ucConfirmProposal.HeaderMessage = "Please Confirm";
            ucConfirmProposal.ErrorMessage = aex.Message;
            ucConfirmProposal.Visible = true;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    public void SetCrewInterviewSummary()
    {
        DataTable dt = PhoenixNewApplicantInterviewSummary.ListNewApplicantInterviewSummary(Convert.ToInt32(Filter.CurrentCrewSelection)
            , General.GetNullableInteger(ViewState["CREWINTERVIEWID"].ToString() != string.Empty ? ViewState["CREWINTERVIEWID"].ToString() : intvid));

        if (dt.Rows.Count > 0)
        {
            string[] s = dt.Rows[0]["FLDINTIALINTERVIEW"].ToString().Split(',');
            chkOrgLicSig.Checked = s[0] == "0" ? false : true;
            chkCertSig.Checked = s[1] == "0" ? false : true;
            chkExpConf.Checked = s[2] == "0" ? false : true;
            chkOtherDet.Checked = s[3] == "0" ? false : true;
            if (s.Length > 4)
                chkAddressLatest.Checked = s[4] == "0" ? false : true;

            txtBackChk.Text = dt.Rows[0]["FLDBACKCHECKEDREMARKS"].ToString();
            s = dt.Rows[0]["FLDINTIALCHECKED"].ToString().Split(',');
            txtOrgLicSigCB.Text = s[0];
            txtCertSigCB.Text = s[1];
            txtExpConfCB.Text = s[2];
            txtOtherDetCB.Text = s[3];
            if (s.Length > 4)
                txtAddressLatest.Text = s[4];

            if (dt.Rows[0]["FLDFOLLOWUPCHECKLIST"].ToString() != "")
            {
                string[] followup = dt.Rows[0]["FLDFOLLOWUPCHECKLIST"].ToString().Split(',');

                if (!String.IsNullOrEmpty(followup[0].ToString()))
                    chkFlagDoc.SelectedValue = followup[0];
                if (!String.IsNullOrEmpty(followup[1].ToString()))
                    chkPassport.SelectedValue = followup[1];
                if (!String.IsNullOrEmpty(followup[2].ToString()))
                    chkCOC.SelectedValue = followup[2];
                if (!String.IsNullOrEmpty(followup[3].ToString()))
                    chkGOC.SelectedValue = followup[3];
                if (!String.IsNullOrEmpty(followup[4].ToString()))
                    chkUSVisa.SelectedValue = followup[4];
                if (followup.Length > 5)
                {
                    if (!String.IsNullOrEmpty(followup[5].ToString()))
                        chkCDCRenewed.SelectedValue = followup[5];
                }
                followup = dt.Rows[0]["FLDFOLLOWUPCHECKEDBY"].ToString().Split(',');
                txtCheckFlagDoc.Text = followup[0];
                txtCheckPassport.Text = followup[1];
                txtCheckCOC.Text = followup[2];
                txtCheckGOC.Text = followup[3];
                txtCheckUSVisa.Text = followup[4];
                if (followup.Length > 5)
                {
                    txtCDCRenewedCB.Text = followup[5];
                }
            }
            txtChnageReason.Text = dt.Rows[0]["FLDREASONFORCHANGE"].ToString();
            txtIncedents.Text = dt.Rows[0]["FLDINCIDENT"].ToString();
            txtDOA.Text = dt.Rows[0]["FLDDATEOFAVAILABILITY"].ToString();
            txtHandremarks1.Text = dt.Rows[0]["FLDHANDOVERREMARKS1"].ToString();
            txtHandremarks2.Text = dt.Rows[0]["FLDHANDOVERREMARKS2"].ToString();
            txtHandremarks3.Text = dt.Rows[0]["FLDHANDOVERREMARKS3"].ToString();
            txtHandremarks4.Text = dt.Rows[0]["FLDHANDOVERREMARKS4"].ToString();
            if (dt.Rows[0]["FLDINTERVIEWSTATUS"].ToString() != string.Empty)
            {
                rblStatus.SelectedValue = dt.Rows[0]["FLDINTERVIEWSTATUS"].ToString();
                ViewState["APPROVED"] = 1;
                rblStatus.Enabled = false;
            }
            else
                rblStatus.Enabled = true;

            s = dt.Rows[0]["FLDEVALUATION"].ToString().Split(',');
            int i = 0;

            foreach (DataListItem item in dlstEvaluation.Items)
            {
                RadRadioButtonList rbtn = (RadRadioButtonList)item.FindControl("rblEvaluation");
                if (s[i] == string.Empty)
                {
                    i++;
                    continue;
                }
                else
                {
                    rbtn.SelectedValue = s[i].ToString();
                    i++;
                }
            }


            txtJoinDate.Text = dt.Rows[0]["FLDEXPECTEDJOINDATE"].ToString();
            txtSuperintendentName.Text = dt.Rows[0]["FLDASSESSMENT"].ToString();
            txtDate.Text = dt.Rows[0]["FLDASSESSMENTDATE"].ToString();
            txtRemaks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
            chkBackedChecked.Checked = dt.Rows[0]["FLDBACKCHECKED"].ToString() == "1" ? true : false;
            txtCompliedBy.Text = dt.Rows[0]["FLDCOMPILEDBY"].ToString();
            ddlOffSigner.Vessel = General.GetNullableInteger(dt.Rows[0]["FLDPLANNEDVESSEL"].ToString());
            ddlOffSigner.Rank = General.GetNullableInteger(dt.Rows[0]["FLDOFFSIGNERRANKCOPY"].ToString());
            chkNoRelevee.Checked = String.IsNullOrEmpty(dt.Rows[0]["FLDOFFSIGNERID"].ToString());
            SetOffsignerMandatory();
            ddlVessel.SelectedVessel = dt.Rows[0]["FLDPLANNEDVESSEL"].ToString();
            ddlRank.SelectedRank = dt.Rows[0]["FLDOFFSIGNERRANKCOPY"].ToString();
            ddlOffSigner.SelectedCrew = dt.Rows[0]["FLDOFFSIGNERID"].ToString();
            txtOffsigner.Text = dt.Rows[0]["FLDOFFSIGNERNAME"].ToString();
            txtDockAttended.Text = dt.Rows[0]["FLDDOCKATTENDED"].ToString();
            txtVettingDone.Text = dt.Rows[0]["FLDVETTINGDONE"].ToString();
            txtCargoCarried.Text = dt.Rows[0]["FLDCARGOCARRIED"].ToString();
            txtSalAgreed.Text = dt.Rows[0]["FLDSALAGREED"].ToString();
            ddlZone.SelectedZone = dt.Rows[0]["FLDZONE"].ToString();
            ddlPool.SelectedPool = dt.Rows[0]["FLDPOOL"].ToString();
            chkBackedChecked.Checked = dt.Rows[0]["FLDDOCSINHAND"].ToString() == "1" ? true : false;
            ViewState["CREWINTERVIEWID"] = dt.Rows[0]["FLDINTERVIEWID"].ToString();
            ViewState["ATTACHMENTCODE"] = dt.Rows[0]["FLDDTKEY"].ToString();
            ViewState["VESSEL"] = dt.Rows[0]["FLDPLANNEDVESSEL"].ToString();
            ViewState["JOINDATE"] = dt.Rows[0]["FLDEXPECTEDJOINDATE"].ToString();
            ViewState["RANK"] = dt.Rows[0]["FLDOFFSIGNERRANK"].ToString();
            ViewState["PLANID"] = dt.Rows[0]["FLDCREWPLANID"].ToString();
            ucRatingExp.SelectedHard = dt.Rows[0]["FLDRATINGEXPERIENCE"].ToString();
            if (rblStatus.Enabled != false)
            {
                txtSuperintendentName.CssClass = "input_mandatory";
                txtDate.CssClass = "input_mandatory";
                txtRemaks.CssClass = "input_mandatory";
                rblStatus.CssClass = "input_mandatory";
            }
            if (dt.Rows[0]["FLDSTATUS"].ToString() == "1")
            {
                CrewMenuGeneral.Visible = false;
                CrewUnLock.Visible = true;
            }
            else
            {
                CrewMenuGeneral.Visible = true;
                CrewUnLock.Visible = false;
            }
            txtJoinOfficerRankid.Text = dt.Rows[0]["FLDCOMBINEDOFFICERRANKID"].ToString();
            chkJoinTogether.Checked = (!string.IsNullOrEmpty(txtJoinOfficerRankid.Text)) ? true : false;
            txtJoinOfficerRank.Text = (!string.IsNullOrEmpty(dt.Rows[0]["FLDRANKNAME"].ToString())) ? dt.Rows[0]["FLDRANKNAME"].ToString() : "";
            txtExperience.Text = (!string.IsNullOrEmpty(dt.Rows[0]["FLDCOMBINEDOFFICEREXP"].ToString())) ? dt.Rows[0]["FLDCOMBINEDOFFICEREXP"].ToString() : "";
            txtRemarksOfficer.Text = (!string.IsNullOrEmpty(dt.Rows[0]["FLDCOMBINEDOFFICERREMARKS"].ToString())) ? dt.Rows[0]["FLDCOMBINEDOFFICERREMARKS"].ToString() : "";
            txtGeneralRemarks.Content = (!string.IsNullOrEmpty(dt.Rows[0]["FLDGENERALREMARKS"].ToString())) ? dt.Rows[0]["FLDGENERALREMARKS"].ToString() : "";
            if (dt.Rows[0]["FLDPROPOSING"].ToString() == "1")
                chkProposing.Checked = true;
            else
                chkProposing.Checked = false;

            changeStatus();
        }
    }

    private bool IsValidate(string interviewby, string interviewdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int resultInt;
        DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
        if (dt.Rows.Count > 0)
        {
            if (General.GetNullableDateTime(dt.Rows[0]["FLDSIGNOFFDATE"].ToString()) != null)
            {
                if (DateTime.Parse(interviewdate) < DateTime.Parse(dt.Rows[0]["FLDSIGNOFFDATE"].ToString()))
                    ucError.ErrorMessage = "Interview Date should be greater than Last Signoff date";
            }
        }
        if (interviewby.Trim() == "")
        {
            ucError.ErrorMessage = "Interviewed By can not be blank";
        }
        if (General.GetNullableDateTime(interviewdate) == null)
        {
            ucError.ErrorMessage = "Interview Date can not be blank";
        }
        DateTime d;
        if (!string.IsNullOrEmpty(interviewdate) && DateTime.TryParse(interviewdate, out d))
        {
            if (d > DateTime.Now)
                ucError.ErrorMessage = "'Interview Date' should be less than 'Current Date'";
        }
        if (chkNoRelevee.Checked != true)
        {
            if (!int.TryParse(ddlOffSigner.SelectedCrew, out resultInt))
            {
                ucError.ErrorMessage = "OffSigner is required.";
            }
        }
        if (ViewState["status"].ToString() == "ONL")
        {
            if (chkPassport.SelectedIndex == -1)
                ucError.ErrorMessage = "Check for Passport renewal' field is mandatory.";
            if (chkCOC.SelectedIndex == -1)
                ucError.ErrorMessage = "Check for COC renewal' field is mandatory.";
            if (chkGOC.SelectedIndex == -1)
                ucError.ErrorMessage = "Check for GOC renewal' field is mandatory.";
            if (chkUSVisa.SelectedIndex == -1)
                ucError.ErrorMessage = "Check for 'US Visa renewal' field is mandatory.";
        }
        if (!int.TryParse(ucRatingExp.SelectedHard, out resultInt))
        {
            ucError.ErrorMessage = "Please select if seafarer has previous experience as rating.";
        }

        return (!ucError.IsError);
    }

    private bool IsValidate(string strVessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int resultInt;
        decimal resultDecimal;
        DateTime resultDate;

        if (!int.TryParse(strVessel, out resultInt))
        {
            ucError.ErrorMessage = "Expected Joining Vessel is required.";
        }
        else if (int.TryParse(strVessel, out resultInt))
        {
            DataSet ds = PhoenixRegistersVessel.EditVessel(Convert.ToInt32(strVessel));
            if (ds.Tables[0].Rows[0]["FLDENGINETYPE"].ToString() == "")
                ucError.ErrorMessage = "Please go to vessel master and map engine type for the vessel " + ds.Tables[0].Rows[0]["FLDVESSELNAME"];
        }
        if (!int.TryParse(ddlRank.SelectedRank, out resultInt))
        {
            ucError.ErrorMessage = "Rank of the Person to be Relieved.";
        }
        if (string.IsNullOrEmpty(txtRank.Text))
        {
            ucError.ErrorMessage = "Applied Rank is required.";
        }
        if (string.IsNullOrEmpty(txtSalAgreed.Text))
            ucError.ErrorMessage = "Salary Agreed is required.";
        else if (!decimal.TryParse(txtSalAgreed.Text, out resultDecimal))
        {
            txtSalAgreed.Text = "";
            ucError.ErrorMessage = "Please enter valid salary.";
        }
        if (!DateTime.TryParse(txtJoinDate.Text, out resultDate))
        {
            ucError.ErrorMessage = "Expected Joining Date is required.";
        }
        else if (DateTime.TryParse(txtJoinDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) < 0)
        {
            ucError.ErrorMessage = "Expected Joining Date should be later than current date";
        }
        if (rblStatus.SelectedItem != null)
        {
            if (string.IsNullOrEmpty(txtSuperintendentName.Text))
            {
                ucError.ErrorMessage = "Superintendent Name is required.";
            }
            if (!DateTime.TryParse(txtDate.Text, out resultDate))
            {
                ucError.ErrorMessage = "Assessment Date is required.";
            }
            else if (DateTime.TryParse(txtDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
            {
                ucError.ErrorMessage = "Assessment Date should be earlier than current date";
            }
            if (string.IsNullOrEmpty(txtRemaks.Text))
            {
                ucError.ErrorMessage = "Assessment Remarks is required.";
            }
        }
        if (chkNoRelevee.Checked != true)
        {
            if (!int.TryParse(ddlOffSigner.SelectedCrew, out resultInt))
            {
                ucError.ErrorMessage = "OffSigner is required.";
            }
        }
        if (ViewState["status"].ToString() == "ONL")
        {
            if (chkPassport.SelectedIndex == -1)
                ucError.ErrorMessage = "Check for Passport renewal' field is mandatory.";
            if (chkCOC.SelectedIndex == -1)
                ucError.ErrorMessage = "Check for COC renewal' field is mandatory.";
            if (chkGOC.SelectedIndex == -1)
                ucError.ErrorMessage = "Check for GOC renewal' field is mandatory.";
            if (chkUSVisa.SelectedIndex == -1)
                ucError.ErrorMessage = "Check for 'US Visa renewal' field is mandatory.";
        }
        if (!int.TryParse(ucRatingExp.SelectedHard, out resultInt))
        {
            ucError.ErrorMessage = "Please select if seafarer has previous experience as rating.";
        }
        if (chkJoinTogether.Checked == true)
        {
            if (string.IsNullOrEmpty(txtJoinOfficerRank.Text))
                ucError.ErrorMessage = "Joining Officer's Rank is required.";
            if (string.IsNullOrEmpty(txtExperience.Text))
                ucError.ErrorMessage = "Joining Officer's Experience is required.";
            if (string.IsNullOrEmpty(txtRemarksOfficer.Text))
                ucError.ErrorMessage = "Remarks is required.";
        }
        if ((chkFlagDoc.SelectedValue == "0" || chkPassport.SelectedValue == "0" || chkCOC.SelectedValue == "0" || chkGOC.SelectedValue == "0" || chkUSVisa.SelectedValue == "0")
            && txtHandremarks1.Text.Trim().Equals(""))
        {
            ucError.ErrorMessage = "if documents check and check for renewal of documents is applied then Hand over remarks to operation team Remarks1 is required.";
        }
        if ((chkCDCRenewed.SelectedValue == "0"))
        {
            if (txtHandremarks1.Text.Trim().Equals(""))
            {
                ucError.ErrorMessage = "if documents check and check for CDC renewal is applied then Hand over remarks to operation team Remarks1 is required.";
            }
            else if (txtHandremarks2.Text.Trim().Equals("") && chkFlagDoc.SelectedValue == "0")
            {
                ucError.ErrorMessage = "if check for CDC renewal is applied then Hand over Remarks2 to operation team is required.";
            }
        }
        if (General.GetNullableInteger(ddlPool.SelectedPool) == null)
        {
            ucError.ErrorMessage = "Pool is required.";
        }
        return (!ucError.IsError);
    }
    protected void OnNoRelieveeClick(object sender, EventArgs e)
    {
        BindOffSigner(sender, e);
        SetOffsignerMandatory();
    }

    private void SetOffsignerMandatory()
    {
        if (chkNoRelevee.Checked == true)
        {
            ddlOffSigner.SelectedCrew = String.Empty;
            ddlOffSigner.Enabled = false;
            ddlOffSigner.CssClass = "input";
        }
        else
        {
            ddlOffSigner.Enabled = true;
            ddlOffSigner.CssClass = "input_mandatory";
        }
    }
    private void ResetFormControlValues(Control parent)
    {

        try
        {
            foreach (Control c in parent.Controls)
            {
                if (c.Controls.Count > 0)
                {
                    ResetFormControlValues(c);
                }
                else
                {
                    switch (c.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.TextBox":
                            ((TextBox)c).Text = "";
                            break;
                        case "System.Web.UI.WebControls.CheckBox":
                            ((CheckBox)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.RadioButton":
                            ((RadioButton)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.DropDownList":
                            ((DropDownList)c).SelectedIndex = 0;
                            break;
                        case "System.Web.UI.WebControls.ListBox":
                            ((ListBox)c).SelectedIndex = 0;
                            break;

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

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                ViewState["status"] = dt.Rows[0]["FLDSTATUSNAME"].ToString();
                string r = dt.Rows[0]["FLDRANK"].ToString();
                ddlRank.SelectedRank = dt.Rows[0]["FLDRANK"].ToString();
                txtPostedRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                ddlZone.SelectedZone = dt.Rows[0]["FLDZONE"].ToString();
                if (!checkIsTop4Rank(General.GetNullableInteger(r.ToString())))
                {
                    chkJoinTogether.Enabled = false;
                    chkJoinTogether.Checked = false;
                    changeStatus();
                }
                else
                {
                    chkJoinTogether.Enabled = true;
                }
                if (dt.Rows[0]["FLDISOFFICER"].ToString() == "1")
                {
                    ucRatingExp.ShortNameFilter = "S,N";
                    ViewState["ISRATING"] = "0";
                }
                else
                {
                    ViewState["ISRATING"] = "1";
                    ucRatingExp.ShortNameFilter = "S,N,NA";
                    ucRatingExp.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 43, "NA");
                }
                ViewState["RANKPOSTED"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void DisableForm()
    {
        chkCDCRenewed.Enabled = false;
        txtCDCRenewedCB.CssClass = "readonlytextbox";
        chkOrgLicSig.Enabled = false;
        txtOrgLicSigCB.CssClass = "readonlytextbox";
        txtOrgLicSigCB.ReadOnly = true;
        chkCertSig.Enabled = false;
        txtCertSigCB.CssClass = "readonlytextbox";
        txtOrgLicSigCB.ReadOnly = true;
        chkExpConf.Enabled = false;
        txtExpConfCB.CssClass = "readonlytextbox";
        txtExpConfCB.ReadOnly = true;
        chkOtherDet.Enabled = false;
        txtOtherDetCB.CssClass = "readonlytextbox";
        txtOtherDetCB.ReadOnly = true;
        chkAddressLatest.Enabled = false;
        txtAddressLatest.CssClass = "readonlytextbox";
        txtAddressLatest.ReadOnly = true;
        txtChnageReason.CssClass = "readonlytextbox";
        txtChnageReason.ReadOnly = true;
        txtIncedents.CssClass = "readonlytextbox";
        txtIncedents.ReadOnly = true;
        txtDockAttended.CssClass = "readonlytextbox";
        txtDockAttended.ReadOnly = true;
        txtCargoCarried.CssClass = "readonlytextbox";
        txtCargoCarried.ReadOnly = true;
        txtVettingDone.CssClass = "readonlytextbox";
        txtVettingDone.ReadOnly = true;
        txtBackChk.CssClass = "readonlytextbox";
        txtBackChk.ReadOnly = true;
        txtDOA.CssClass = "readonlytextbox";
        txtDOA.ReadOnly = true;
        txtSalAgreed.CssClass = "readonlytextbox";
        txtSalAgreed.ReadOnly = true;
        txtJoinDate.CssClass = "readonlytextbox";
        txtJoinDate.ReadOnly = true;
        txtCompliedBy.CssClass = "readonlytextbox";
        txtCompliedBy.ReadOnly = true;
        chkBackedChecked.Enabled = false;
        ddlVessel.Enabled = false;
        ddlRank.Enabled = false;
        ddlOffSigner.Visible = false;
        txtOffsigner.Visible = true;
        chkNoRelevee.Enabled = false;
        chkDocsinHand.Enabled = false;
        gvInterviewBy.Enabled = false;
        dlstEvaluation.Enabled = false;
        txtHandremarks1.CssClass = "readonlytextbox";
        txtHandremarks1.ReadOnly = true;
        txtHandremarks2.CssClass = "readonlytextbox";
        txtHandremarks2.ReadOnly = true;
        txtHandremarks3.CssClass = "readonlytextbox";
        txtHandremarks3.ReadOnly = true;
        txtHandremarks4.CssClass = "readonlytextbox";
        txtHandremarks4.ReadOnly = true;
        txtSuperintendentName.CssClass = "readonlytextbox";
        txtSuperintendentName.ReadOnly = true;
        txtDate.CssClass = "readonlytextbox";
        txtDate.ReadOnly = true;
        txtRemaks.CssClass = "readonlytextbox";
        txtRemaks.ReadOnly = true;
        chkFlagDoc.Enabled = false;
        chkGOC.Enabled = false;
        chkCOC.Enabled = false;
        chkUSVisa.Enabled = false;
        chkPassport.Enabled = false;
        chkJoinTogether.Enabled = false;
        txtJoinOfficerRank.Enabled = false;
        txtJoinOfficerRank.ReadOnly = true;
        txtJoinOfficerRank.CssClass = "readonlytextbox";
        txtExperience.Enabled = false;
        txtExperience.ReadOnly = true;
        txtExperience.CssClass = "readonlytextbox";
        txtRemarksOfficer.Enabled = false;
        txtRemarksOfficer.ReadOnly = true;
        txtRemarksOfficer.CssClass = "readonlytextbox";
    }

    private void EnableForm()
    {
        chkOrgLicSig.Enabled = true;
        txtOrgLicSigCB.CssClass = "";
        txtOrgLicSigCB.ReadOnly = false;
        chkCertSig.Enabled = true;
        txtCertSigCB.CssClass = "";
        txtOrgLicSigCB.ReadOnly = false;
        chkExpConf.Enabled = true;
        txtExpConfCB.CssClass = "";
        txtExpConfCB.ReadOnly = false;
        chkOtherDet.Enabled = true;
        txtOtherDetCB.CssClass = "";
        txtOtherDetCB.ReadOnly = false;
        chkAddressLatest.Enabled = true;
        txtAddressLatest.CssClass = "";
        txtAddressLatest.ReadOnly = false;
        txtChnageReason.CssClass = "";
        txtChnageReason.ReadOnly = true;
        txtIncedents.CssClass = "";
        txtIncedents.ReadOnly = false;
        txtDockAttended.CssClass = "";
        txtDockAttended.ReadOnly = false;
        txtCargoCarried.CssClass = "";
        txtCargoCarried.ReadOnly = false;
        txtVettingDone.CssClass = "";
        txtVettingDone.ReadOnly = false;
        txtBackChk.CssClass = "";
        txtBackChk.ReadOnly = false;
        txtDOA.CssClass = "";
        txtDOA.ReadOnly = false;
        txtSalAgreed.CssClass = "";
        txtSalAgreed.ReadOnly = false;
        txtJoinDate.CssClass = "";
        txtJoinDate.ReadOnly = false;
        txtCompliedBy.CssClass = "";
        txtCompliedBy.ReadOnly = false;
        chkBackedChecked.Enabled = true;
        ddlVessel.Enabled = true;
        ddlRank.Enabled = true;
        chkNoRelevee.Enabled = true;
        chkDocsinHand.Enabled = true;
        gvInterviewBy.Enabled = true;
        dlstEvaluation.Enabled = true;
        txtHandremarks1.CssClass = "";
        txtHandremarks1.ReadOnly = false;
        txtHandremarks2.CssClass = "";
        txtHandremarks2.ReadOnly = false;
        txtHandremarks3.CssClass = "";
        txtHandremarks3.ReadOnly = false;
        txtHandremarks4.CssClass = "";
        txtHandremarks4.ReadOnly = false;
        txtSuperintendentName.CssClass = "";
        txtSuperintendentName.ReadOnly = false;
        txtDate.CssClass = "";
        txtDate.ReadOnly = false;
        txtRemaks.CssClass = "";
        txtRemaks.ReadOnly = false;
        chkFlagDoc.Enabled = true;
        chkGOC.Enabled = true;
        chkCOC.Enabled = true;
        chkUSVisa.Enabled = true;
        chkPassport.Enabled = true;
        chkJoinTogether.Enabled = true;
        txtJoinOfficerRank.Enabled = true;
        txtJoinOfficerRank.ReadOnly = false;
        txtJoinOfficerRank.CssClass = "";
        txtExperience.Enabled = true;
        txtExperience.ReadOnly = false;
        txtExperience.CssClass = "";
        txtRemarksOfficer.Enabled = true;
        txtRemarksOfficer.ReadOnly = false;
        txtRemarksOfficer.CssClass = "";
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            string Script = "";
            Script += "fnReloadList(null,'ifMoreInfo','keepopen');";
            int res = 0;
            Guid? g = null;
            UpdateCrewInterviewSummary(g, 0);
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey="
                + ViewState["ATTACHMENTCODE"]
                + "&mod=" + PhoenixModule.CREW
                + "&type=" + PhoenixCrewAttachmentType.ASSESSMENT
                + (string.IsNullOrEmpty(ViewState["APPROVED"].ToString()) ? "&u=n" : string.Empty)
                + "'); return false;", "Follow Up Sheet", "", "ATTACHMENT");
            toolbarsub.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=EMPLOYEECV&employeeid="
                + Filter.CurrentCrewSelection + "&vslid=" + ViewState["VESSEL"] + "&rankid=" + ViewState["RANK"] + "&showmenu=1'); return false;", "PDForm", "", "PDFORM");
            DataSet ds = PhoenixNewApplicantInterviewSummary.GetTopOfficerRank(General.GetNullableInteger(ViewState["RANK"].ToString()), ref res);
            if (res == 1)
                toolbarsub.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDOWNER&employeeid="
                    + Filter.CurrentCrewSelection
                    + "&vslid=" + ViewState["VESSEL"] + "&rankid=" + ViewState["RANK"] + "&showmenu=1'); return false;", "Owner PD", "", "PDOWNER");
            CrewMenuGeneral.MenuList = toolbarsub.Show();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void btnProposal_Click(object sender, EventArgs e)
    {
        try
        {
            string Script = "";
            Script += "fnReloadList(null,'ifMoreInfo','keepopen');";
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            if (ucCM.confirmboxvalue == 1)
            {
                Guid? g = null;
                UpdateCrewInterviewSummary(g, 1);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixNewApplicantInterviewSummary.SearchInterviewedBy(Convert.ToInt32(Filter.CurrentCrewSelection)
                                                                                    , General.GetNullableInteger(ViewState["CREWINTERVIEWID"].ToString())
                                                                                    , sortexpression, sortdirection
                                                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                    , gvInterviewBy.PageSize
                                                                                    , ref iRowCount
                                                                                    , ref iTotalPageCount);
            gvInterviewBy.DataSource = ds;
            gvInterviewBy.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInterviewBy_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInterviewBy.CurrentPageIndex + 1;

        BindData();
    }


    protected void gvInterviewBy_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "ADD")
            {

                if (ViewState["CREWINTERVIEWID"].ToString() == string.Empty)
                {
                    if (!IsValidate(ddlVessel.SelectedVessel))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    SaveCrewInterviewSummary(null);
                }

                string strInverviewBy = ((RadTextBox)e.Item.FindControl("txtInterviewedByAdd")).Text;
                string strInverviewDate = ((UserControlDate)e.Item.FindControl("txtInterviewDateAdd")).Text;
                string strRemarks = ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text;

                if (!IsValidate(strInverviewBy, strInverviewDate))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixNewApplicantInterviewSummary.InsertInterviewedBy(Convert.ToInt32(Filter.CurrentCrewSelection)
                                                                , int.Parse(ViewState["CREWINTERVIEWID"].ToString())
                                                                , strInverviewBy
                                                                , General.GetNullableDateTime(strInverviewDate).Value
                                                                , strRemarks);
                BindData();
                gvInterviewBy.Rebind();

            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInterviewBy_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string strInterviewBy = ((RadTextBox)e.Item.FindControl("txtInterviewedByEdit")).Text;
            string strInterviewDate = ((UserControlDate)e.Item.FindControl("txtInterviewDateEdit")).Text;
            string strInterviewById = ((RadLabel)e.Item.FindControl("lblInterviewedByIdEdit")).Text;
            string strRemarks = ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text;

            if (!IsValidate(strInterviewBy, strInterviewDate))
            {
                ucError.Visible = true;
                BindData();
                return;
            }

            PhoenixNewApplicantInterviewSummary.UpdateInterviewedBy(Convert.ToInt32(strInterviewById.ToString())
                                                            , strInterviewBy
                                                                , General.GetNullableDateTime(strInterviewDate).Value
                                                                , strRemarks);

            BindData();
            gvInterviewBy.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvInterviewBy_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string strInterviewById = ((RadLabel)e.Item.FindControl("lblInterviewedById")).Text;

            PhoenixNewApplicantInterviewSummary.DeleteInterviewedBy(Convert.ToInt32(strInterviewById.ToString()));
            BindData();
            gvInterviewBy.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInterviewBy_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
               // db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null) if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;

        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }


    protected void BindOffSigner(Object sender, EventArgs e)
    {
        UserControlVessel vsl = ddlVessel;
        UserControlRank rank = ddlRank;
        UserControlCrewOnboard cob = ddlOffSigner;

        int? VesselId = General.GetNullableInteger(vsl.SelectedVessel);
        int? RankId = General.GetNullableInteger(rank.SelectedRank);

        if (VesselId.HasValue)
        {
            bool bind = false;
            if (int.Parse(ViewState["CVSL"].ToString()) != VesselId.Value)
            {
                ViewState["CVSL"] = VesselId.Value;
                bind = true;
            }
            if (RankId.HasValue && int.Parse(ViewState["CRNK"].ToString()) != RankId.Value)
            {
                ViewState["CRNK"] = RankId.Value;
                bind = true;
            }
            if (bind)
                cob.OnboardList = PhoenixCrewManagement.ListCrewOnboard(VesselId.Value, RankId);
        }
        else
            cob.OnboardList = PhoenixCrewManagement.ListCrewOnboard(General.GetNullableInteger(vsl.SelectedVessel), General.GetNullableInteger(rank.SelectedRank));
    }

    protected void rblStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!IsValidate(ddlVessel.SelectedVessel))
        {
            ucError.Visible = true;
            return;
        }
        if (rblStatus.SelectedItem != null && (rblStatus.SelectedValue == "1" || rblStatus.SelectedValue == "2"))
        {
            DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["ATTACHMENTCODE"].ToString())
                , PhoenixCrewAttachmentType.ASSESSMENT.ToString());

            if (dt.Rows.Count == 0 && ViewState["ISRATING"].ToString() == "0")
            {
                ucError.ErrorMessage = "Please attach Interview sheet/Offer letter to proceed further";
                ucError.Visible = true;
                return;
            }

            RadWindowManager1.RadConfirm("You will not be able to make any changes. Are you sure you want to propose this seafarer ?", "confirm", 320, 150, null, "Confirm");
        }
        else
        {
            RadWindowManager1.RadConfirm("You will not be able to make any changes. Are you sure you want to reject this seafarer ?", "confirm", 320, 150, null, "Confirm");

        }
    }

    protected void OnClick(object sender, EventArgs e)
    {
        RadCheckBox chk = (RadCheckBox)(sender);
        string username = PhoenixSecurityContext.CurrentSecurityContext.FirstName + "" + PhoenixSecurityContext.CurrentSecurityContext.MiddleName
                                     + "" + PhoenixSecurityContext.CurrentSecurityContext.LastName;
        switch (chk.ID)
        {
            case "chkOrgLicSig":
                txtOrgLicSigCB.Text = chk.Checked == true ? username : "";
                break;
            case "chkCertSig":
                txtCertSigCB.Text = chk.Checked == true ? username : "";
                break;
            case "chkExpConf":
                txtExpConfCB.Text = chk.Checked == true ? username : "";
                break;
            case "chkOtherDet":
                txtOtherDetCB.Text = chk.Checked == true ? username : "";
                break;

            case "chkAddressLatest":
                txtAddressLatest.Text = chk.Checked == true ? username : "";
                break;

        }

    }

    protected void OnSelect(object sender, EventArgs e)
    {

        RadRadioButtonList chk = (RadRadioButtonList)(sender);

        string username = PhoenixSecurityContext.CurrentSecurityContext.FirstName + "" + PhoenixSecurityContext.CurrentSecurityContext.MiddleName
                                     + "" + PhoenixSecurityContext.CurrentSecurityContext.LastName;
        switch (chk.ID)
        {
            case "chkFlagDoc":
                txtCheckFlagDoc.Text = chk.SelectedIndex.ToString().Contains("-") ? "" : username;
                break;
            case "chkPassport":
                txtCheckPassport.Text = chk.SelectedIndex.ToString().Contains("-") ? "" : username;
                break;
            case "chkCOC":
                txtCheckCOC.Text = chk.SelectedIndex.ToString().Contains("-") ? "" : username;
                break;
            case "chkGOC":
                txtCheckGOC.Text = chk.SelectedIndex.ToString().Contains("-") ? "" : username;
                break;
            case "chkUSVisa":
                txtCheckUSVisa.Text = chk.SelectedIndex.ToString().Contains("-") ? "" : username;
                break;
            case "chkCDCRenewed":
                txtCDCRenewedCB.Text = chk.SelectedIndex.ToString().Contains("-") ? "" : username;
                break;
        }

    }

    protected void onChkJoinTogetherClick(object sender, EventArgs e)
    {
        changeStatus();
        setTopOfficerRank();
    }
    protected void changeStatus()
    {
        if (chkJoinTogether.Checked == true)
        {
            txtJoinOfficerRank.CssClass = "input_mandatory";
            txtJoinOfficerRank.Enabled = true;
            txtExperience.CssClass = "input_mandatory";
            txtExperience.Enabled = true;
            txtRemarksOfficer.CssClass = "input_mandatory";
            txtRemarksOfficer.Enabled = true;
            chkJoinTogether.Enabled = true;
        }
        else
        {
            txtJoinOfficerRank.CssClass = "readonlytextbox";
            txtJoinOfficerRank.ReadOnly = true;
            txtJoinOfficerRank.Enabled = false;
            txtExperience.CssClass = "readonlytextbox";
            txtExperience.Enabled = false;
            txtRemarksOfficer.CssClass = "readonlytextbox";
            txtRemarksOfficer.Enabled = false;
            txtJoinOfficerRankid.Text = "";
            txtJoinOfficerRank.Text = "";
            txtExperience.Text = "";
            txtRemarksOfficer.Text = "";
        }
    }
    protected void setTopOfficerRank()
    {
        if (chkJoinTogether.Checked == true)
        {
            int r = 0;
            DataSet dt = PhoenixNewApplicantInterviewSummary.GetTopOfficerRank(General.GetNullableInteger(ddlRank.SelectedRank.ToString()), ref r);
            if (dt.Tables[0].Rows.Count > 0)
            {
                txtJoinOfficerRankid.Text = dt.Tables[0].Rows[0]["FLDRANKID"].ToString();
                txtJoinOfficerRank.Text = dt.Tables[0].Rows[0]["FLDRANKNAME"].ToString();
            }
            else
            {
                txtJoinOfficerRankid.Text = "";
                txtJoinOfficerRank.Text = "";
            }
            txtExperience.Text = "";
            txtRemarksOfficer.Text = "";
        }
        else
        {
            txtJoinOfficerRankid.Text = "";
            txtJoinOfficerRank.Text = "";
            txtExperience.Text = "";
            txtRemarksOfficer.Text = "";
        }
    }
    protected void ddlRank_TextChanged(object sender, EventArgs e)
    {
        if (checkIsTop4Rank(General.GetNullableInteger(ddlRank.SelectedRank.ToString())))
        {
            chkJoinTogether.Enabled = true;
            setTopOfficerRank();
        }
        else
        {
            chkJoinTogether.Enabled = false;
            chkJoinTogether.Checked = false;
            changeStatus();
        }
        BindOffSigner(sender, e);
    }
    protected bool checkIsTop4Rank(int? rankid)
    {
        int res = 0;
        DataSet ds = PhoenixNewApplicantInterviewSummary.GetTopOfficerRank(General.GetNullableInteger(rankid.ToString()), ref res);
        if (res == 1)
            return true;
        else
            return false;
    }

    protected void gvCrewRecommendedCourses_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindRecommendedCourses();
    }

    protected void gvLastVessel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindLastVesselDetails();
    }


}
