using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
using SouthNests.Phoenix.CrewOffshore;

public partial class CrewPersonal : PhoenixBasePage
{
    private const string SCRIPT_DOFOCUS = @"window.setTimeout('DoFocus()', 1);
            function DoFocus()
            {
                try {
                    document.getElementById('REQUEST_LASTFOCUS').focus();
                } catch (ex) {}
            }";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            string offerletter = PhoenixCommonRegisters.GetHardCode(1, 266, "OFO");
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            if (offerletter != null && offerletter != "")
                toolbarmain.AddButton("OC - 9A", "OC9A", ToolBarDirection.Right);


            if (Request.QueryString["portal"] != null && Request.QueryString["portal"].ToString() == "1")
            {
                toolbarmain.AddButton("Edit Details", "EMPSAVE", ToolBarDirection.Right);
                toolbarmain.AddButton("Edit Address", "ADDRESS", ToolBarDirection.Right);

                cmdeditpool.Visible = false;
                imguser.Visible = false;
                compact.Visible = false;
            }
            else
            {
                toolbarmain.AddButton("OC - 9", "EXPORTOC9", ToolBarDirection.Right);
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
            DataTable dt = PhoenixCrewPortalDetailsConfirmation.EmployeeDetailConfirmationList(General.GetNullableInteger(Filter.CurrentCrewSelection));
            if (dt.Rows.Count > 0 && Request.QueryString["portal"] == null)
                toolbarmain.AddButton("Approve Details", "APPROVE", ToolBarDirection.Right);

            CrewMainPersonal.AccessRights = this.ViewState;
            CrewMainPersonal.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                imguser.Attributes.Add("onclick", "javascript:return showPickList('spnPickListFleetManager', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListUser.aspx', true);");
                HookOnFocus(this.Page as Control);
                divBMI.InnerHtml = BMIChart();
                txtBMI.Attributes["onclick"] = "javascript:showBMI();";
                ucSex.HardTypeCode = ((int)PhoenixHardTypeCode.SEX).ToString();
                ucConfirm.Visible = false;
                if (Filter.CurrentCrewSelection != null)
                {
                    lnkIncompatibility.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewIncompatibilityList.aspx');return false;");
                    lnkIncompatibility.Visible = true;
                    lnkIncompatibility.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewIncompatibilityList.aspx?empid=" + Filter.CurrentCrewSelection + "');return false;");
                    imgIDCard.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/SsrsReports/SsrsReportsView.aspx?applicationcode=4&reportcode=CREWIDCARD&employeeid=" + Filter.CurrentCrewSelection + "');return false;");
                    ViewState["WARNLIST"] = string.Empty;
                    ListCrewInformation();

                    imgPPClipPanNo.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.PANNO + "&cmdname=FPANNOUPLOAD'); return false;";

                    imgPPClipUidNo.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.UIDNO + "&cmdname=FUIDUPLOAD'); return false;";
                    CalculateBMI(null, null);
                    if (IsImportantRemarks())
                        lnkImportantRemarks.Visible = true;
                    else
                        lnkImportantRemarks.Visible = false;
                    lnkIncompatibility.Visible = SessionUtil.CanAccess(this.ViewState, "INCOMPATIBILITY");
                }
                else
                {
                    lnkIncompatibility.Visible = false;
                    lnkImportantRemarks.Visible = false;
                }
                lnkResume.Visible = SessionUtil.CanAccess(this.ViewState, "RESUME");
                if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom != "")
                {
                    lblRecrOff.Text = "Manning Office";
                    imgPDForm.Visible = true;
                    imgPDForm.Attributes.Add("onclick", "openNewWindow('PDForm', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + Filter.CurrentCrewSelection + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");
                    cmdEmail.Visible = true;
                    cmdEmail.Attributes.Add("onclick", "openNewWindow('codehelp2','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEmployeeDocumentList.aspx?empid=" + Filter.CurrentCrewSelection + "&pdform=1" + "');return false;");

                }
                Page.ClientScript.RegisterStartupScript(
                typeof(CrewPersonal),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);
                ucPool.Enabled = false;
                ucZone.Enabled = false;
                txtEmployeeCode.Enabled = SessionUtil.CanAccess(this.ViewState, "EMPCODE");
                chkDirectSignon.Enabled = SessionUtil.CanAccess(this.ViewState, "EMPCODE");
                ddlRankPosted.Enabled = SessionUtil.CanAccess(this.ViewState, "EMPCODE");
                ucRank.Enabled = SessionUtil.CanAccess(this.ViewState, "EMPCODE");

                if (SessionUtil.CanAccess(this.ViewState, "EMPCODE"))
                    txtEmployeeCode.CssClass = "input_mandatory upperCase";

                if (PhoenixGeneralSettings.CurrentGeneralSetting.CountryCode != "IND")
                {
                    txtPanNo.ReadOnly = true;
                    txtPanNo.CssClass = "readonlytextbox";
                    txtUidNo.ReadOnly = true;
                    txtUidNo.CssClass = "readonlytextbox";
                    ucZone.Enabled = false;
                    txtMentorName.ReadOnly = true;
                    txtMentorName.CssClass = "readonlytextbox";
                    imguser.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void HookOnFocus(Control CurrentControl)
    {
        if ((CurrentControl is TextBox) ||
            (CurrentControl is DropDownList))

            (CurrentControl as WebControl).Attributes.Add(
               "onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");
        if (CurrentControl.HasControls())

            foreach (Control CurrentChildControl in CurrentControl.Controls)
                HookOnFocus(CurrentChildControl);
    }

    private bool IsImportantRemarks()
    {
        DataTable dt = PhoenixCrewManagement.CrewImportantRemarksEdit(new Guid(ViewState["attachmentcode"].ToString()));

        if (dt.Rows.Count > 0)
            return true;
        else
            return false;
    }

    protected void lnkImportantRemarks_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Crew/CrewImportantRemarks.aspx", false);
    }


    protected void CrewMainPersonal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidate())
                {
                    ucError.Visible = true;
                    return;
                }

                if (Filter.CurrentCrewSelection == null)
                {
                    SaveCrewMainPersonalInformation();
                }
                else
                {
                    if (!string.IsNullOrEmpty(ViewState["WARNLIST"].ToString()))
                    {
                        ucConfirm.HeaderMessage = "Please Confirm";
                        ucConfirm.Text = "* Passport Number in Warnlist. Continue ?";
                        ucConfirm.Visible = true;
                        ucConfirm.CancelText = "No";
                        ucConfirm.OKText = "Yes";
                    }
                    else
                    {
                        UpdateCrewMainPersonalInformation(null, null);
                    }

                }
            }
            if (CommandName.ToUpper().Equals("EMPSAVE"))
            {

                String script = String.Format("javascript:openNewWindow('spnPickListVendor', 'codehelp1', '" + Session["sitepath"] + "/CrewOffshore/CrewPersonalDetailsBySeafarerEdit.aspx?empid=" + General.GetNullableInteger(Filter.CurrentNewApplicantSelection) + "&portal=1');");

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }

            if (CommandName.ToUpper().Equals("ADDRESS"))
            {

                String script = String.Format("javascript:openNewWindow('spnPickListVendor', 'codehelp1', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEmployeeAddressConfirmationEdit.aspx?empid=" + General.GetNullableInteger(Filter.CurrentCrewSelection) + "&portal=1');");

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }



            if (CommandName.ToUpper().Equals("HOME"))
            {

                Response.Redirect("../Dashboard/DashboardHome.aspx?Type=p&empid=" + Filter.CurrentCrewSelection);
            }
            if (CommandName.ToUpper().Equals("EXPORTOC9"))
            {
                PhoenixCrew2XL.Export2XLOC9(General.GetNullableInteger(Filter.CurrentCrewSelection));
            }
            if (CommandName.ToUpper().Equals("OC9A"))
            {
                Response.Redirect("../Crew/CrewOfferLetterList.aspx?Type=p&empid=" + Filter.CurrentCrewSelection);
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                ResetFormControlValues();
                Filter.CurrentCrewSelection = null;
                imgPhoto.ImageUrl = Session["images"] + "/Blank.png";
                lnkIncompatibility.Visible = false;
                lnkImportantRemarks.Visible = false;
            }
            if (CommandName.ToUpper().Equals("APPROVE"))
            {

                String script = String.Format("javascript:openNewWindow('spnPickListVendor', 'codehelp1', '" + Session["sitepath"] + "/CrewOffshore/CrewPersonalDetailsBySeafarerEdit.aspx?empid=" + General.GetNullableInteger(Filter.CurrentCrewSelection) + "');");

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SaveCrewMainPersonalInformation()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.InsertEmployee(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , General.GetNullableString(txtPassport.Text.Trim().ToUpper())
                                                 , General.GetNullableString(txtSeamenBookNumber.Text.Trim().ToUpper())
                                                 , txtFirstname.Text.Trim().ToUpper()
                                                 , txtLastname.Text.Trim().ToUpper(), txtMiddlename.Text.Trim().ToUpper()
                                                 , null, txtMiddlename.Text.Trim().ToUpper(), txtMiddlename.Text.Trim().ToUpper()
                                                 , Convert.ToInt32(ucNationality.SelectedNationality)
                                                 , Convert.ToDateTime(txtDateofBirth.Text), txtPlaceofBirth.Text.Trim().ToUpper()
                                                 , General.GetNullableInteger(ucRank.SelectedRank)

                                                 , General.GetNullableInteger(ucZone.SelectedZone.ToString())
                                                 , Convert.ToInt32(ucSex.SelectedHard)
                                                 , General.GetNullableInteger(ucMaritialStatus.SelectedMaritalStatus.ToString())
                                                 , General.GetNullableDecimal(txtHeight.Text.Trim())
                                                 , General.GetNullableDecimal(txtWeight.Text.Trim())
                                                 , General.GetNullableDecimal(ddlShoesSize.SelectedValue), txtINDOsNumber.Text.Trim().ToUpper()
                                                 , null //txtRegnumber.Text
                                                 , null //txtSSNumber.Text
                                                 , null  //General.GetNullableInteger(ucOwnerPool.SelectedAddress)
                                                 , null// General.GetNullableInteger(ucPool.SelectedPool)
                                                 , null
                                                 , General.GetNullableDateTime(txtDateofJoin.Text)
                                                 , null//txtFileNumber.Text
                                                 , txtHairColor.Text.Trim().ToUpper()
                                                 , txtEyeColor.Text.Trim().ToUpper()
                                                 , txtDistinguishMark.Text.Trim().ToUpper()
                                                 , General.GetNullableString(txtPanNo.Text.Trim().ToUpper())
                                                 , General.GetNullableString(txtUidNo.Text.Trim().ToUpper())
                                                 , General.GetNullableString(ddlShirtSize.SelectedValue)
                                                 , General.GetNullableString(ddlBoilerSuitSize.SelectedValue)
                                                 , General.GetNullableString(txtPassportName.Text.ToUpper())
                                                 );
            if (dt.Rows.Count > 0)
            {
                if (Request.Files["txtFileUpload"].ContentLength > (60 * 1024))
                {
                    ucError.ErrorMessage = "Photo size cannot exceed 60kb. Upload Photo size should be less than 60kb.";
                    ucError.Visible = true;
                    return;
                }
                Filter.CurrentCrewSelection = dt.Rows[0][0].ToString();
                PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(dt.Rows[0][1].ToString()), PhoenixModule.CREW, null, ".jpg,.png,.gif", string.Empty, "CREWIMAGE");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        ListCrewInformation();
    }


    protected void UpdateCrewMainPersonalInformation(object sender, EventArgs e)
    {
        try
        {
            if ((sender != null && ((UserControlConfirmMessage)sender).confirmboxvalue == 1) || sender == null)
            {
                if (Request.Files.Count > 0)
                {
                    foreach (UploadedFile postedFile in RadUpload1.UploadedFiles)
                    {
                        if (!Object.Equals(postedFile, null))
                        {
                            if (postedFile.ContentLength > 0)
                            {

                                if (postedFile.ContentLength > (60 * 1024))
                                {
                                    ucError.ErrorMessage = "Uploaded Photo size cannot exceed 60kb.";
                                    ucError.Visible = true;
                                    return;
                                }
                                if (string.IsNullOrEmpty(ViewState["dtkey"].ToString()))
                                {
                                    PhoenixCommonFileAttachment.InsertAttachment(postedFile, new Guid(ViewState["attachmentcode"].ToString()), PhoenixModule.CREW, null, ".jpg,.png,.gif", string.Empty, "CREWIMAGE");
                                }
                                else
                                {
                                    PhoenixCommonFileAttachment.UpdateAttachment(postedFile, new Guid(ViewState["dtkey"].ToString()), PhoenixModule.CREW, ".jpg,.png,.gif");
                                }

                            }
                        }
                    }
                }

                string directsignon = "0";

                if (chkDirectSignon.Checked == true)
                    directsignon = "1";

                PhoenixCrewPersonal.UpdateEmployee(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , Convert.ToInt32(Filter.CurrentCrewSelection)
                                                        , General.GetNullableString(txtPassport.Text.Trim().ToUpper().Replace(" ", string.Empty))
                                                        , General.GetNullableString(txtSeamenBookNumber.Text.Trim().ToUpper().Replace(" ", string.Empty))
                                                        , txtFirstname.Text.Trim().ToUpper()
                                                        , txtLastname.Text.Trim().ToUpper(), txtMiddlename.Text.Trim().ToUpper()
                                                        , null, null, null
                                                        , Convert.ToInt32(ucNationality.SelectedNationality)
                                                        , Convert.ToDateTime(txtDateofBirth.Text), txtPlaceofBirth.Text.Trim().ToUpper()
                                                        , General.GetNullableInteger(ucRank.SelectedRank)
                                                        , General.GetNullableInteger(ddlRankPosted.SelectedRank)
                                                        , General.GetNullableInteger(ucZone.SelectedZone.ToString())
                                                        , Convert.ToInt32(ucSex.SelectedHard)
                                                        , General.GetNullableInteger(ucMaritialStatus.SelectedMaritalStatus.ToString())
                                                        , General.GetNullableDecimal(txtHeight.Text.Trim())
                                                        , General.GetNullableDecimal(txtWeight.Text.Trim())
                                                        , General.GetNullableDecimal(ddlShoesSize.SelectedValue), txtINDOsNumber.Text.Trim().ToUpper().Replace(" ", string.Empty)
                                                        , null //txtRegnumber.Text
                                                        , null //txtSSNumber.Text
                                                        , null //General.GetNullableInteger(ucOwnerPool.SelectedAddress)
                                                        , null //General.GetNullableInteger(ucPool.SelectedPool)
                                                        , null
                                                        , General.GetNullableDateTime(txtDateofJoin.Text)
                                                        , General.GetNullableString(txtEmployeeCode.Text.Trim().ToUpper())//txtFileNumber.Text
                                                        , txtHairColor.Text.Trim().ToUpper()
                                                        , txtEyeColor.Text.Trim().ToUpper()
                                                        , txtDistinguishMark.Text.Trim().ToUpper()
                                                        , General.GetNullableInteger(ucBatch.SelectedBatch)
                                                        , General.GetNullableInteger(txtuserid.Text.Trim())
                                                        , General.GetNullableString(txtPanNo.Text.Trim().ToUpper())
                                                        , General.GetNullableString(txtUidNo.Text.Trim().ToUpper())
                                                        , byte.Parse(directsignon)
                                                        , General.GetNullableString(ddlShirtSize.SelectedValue)
                                                        , General.GetNullableString(ddlBoilerSuitSize.SelectedValue)
                                                        , General.GetNullableString(txtPassportName.Text.ToUpper())
                                                        );

                ucStatus.Text = "Crew Information Updated.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        ListCrewInformation();
        CalculateBMI(null, null);
    }

    public void ListCrewInformation()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(Convert.ToInt32(Filter.CurrentCrewSelection));
            if (dt.Rows.Count > 0)
            {

                txtEmployeeCode.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstname.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddlename.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
                {
                    if (dt.Rows[0]["FLDSTATUSNAME"].ToString() == "ONB")
                    {
                        lblLastOnboardVessel.Text = "Onboard Vessel";
                    }
                    else
                    {
                        lblLastOnboardVessel.Text = "Last Vessel";
                    }
                }
                txtLastname.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                ucSex.SelectedHard = dt.Rows[0]["FLDSEX"].ToString();
                txtPassport.Text = dt.Rows[0]["FLDPASSPORTNO"].ToString();
                txtSeamenBookNumber.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
                //txtRegnumber.Text = dt.Rows[0]["FLDREGISTRATIONNUMBER"].ToString();
                //txtSSNumber.Text = dt.Rows[0]["FLDSSNUMBER"].ToString();
                ucPool.SelectedPool = dt.Rows[0]["FLDPOOL"].ToString();
                cmdeditpool.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Pool History', '" + Session["sitepath"] + "/Crew/CrewPoolHistory.aspx?empid=" + Filter.CurrentCrewSelection + "&pool=" + dt.Rows[0]["FLDPOOL"].ToString() + "');return false;");
                txtDateofBirth.Text = dt.Rows[0]["FLDDATEOFBIRTH"].ToString();
                txtPlaceofBirth.Text = dt.Rows[0]["FLDPLACEOFBIRTH"].ToString();
                ucMaritialStatus.SelectedMaritalStatus = dt.Rows[0]["FLDMARITALSTATUS"].ToString();
                ucNationality.SelectedNationality = dt.Rows[0]["FLDNATIONALITY"].ToString();
                ucZone.SelectedZone = dt.Rows[0]["FLDZONE"].ToString();

                if (dt.Rows[0]["FLDHEIGHT"].ToString() != "0.00" && dt.Rows[0]["FLDHEIGHT"].ToString() != "")
                {
                    txtHeight.Text = dt.Rows[0]["FLDHEIGHT"].ToString();
                }
                if (dt.Rows[0]["FLDWEIGHT"].ToString() != "0.00" && dt.Rows[0]["FLDWEIGHT"].ToString() != "")
                {
                    txtWeight.Text = dt.Rows[0]["FLDWEIGHT"].ToString();
                }

                ddlShoesSize.SelectedValue = dt.Rows[0]["FLDSHOESCMS"].ToString();
                ddlShirtSize.SelectedValue = dt.Rows[0]["FLDSHIRTSIZE"].ToString();

                ucRank.SelectedRank = dt.Rows[0]["FLDRANK"].ToString();
                txtAppliedOn.Text = dt.Rows[0]["FLDAPPLIEDON"].ToString();

                txtLastVessel.Text = dt.Rows[0]["FLDLASTVESSELNAME"].ToString();
                ddlRankPosted.SelectedRank = dt.Rows[0]["FLDRANKPOSTED"].ToString();
                txtDateofJoin.Text = dt.Rows[0]["FLDDATEOFJOINING"].ToString();
                txtHairColor.Text = dt.Rows[0]["FLDHAIRCOLOR"].ToString();
                txtEyeColor.Text = dt.Rows[0]["FLDEYECOLOR"].ToString();
                txtDistinguishMark.Text = dt.Rows[0]["FLDDISTINGUISHINGMARK"].ToString();
                txtINDOsNumber.Text = dt.Rows[0]["FLDINDOSNO"].ToString();
                txtBMI.Text = dt.Rows[0]["FLDBMI"].ToString();
                txtCreatedBy.Text = dt.Rows[0]["FLDCREATEDBY"].ToString();
                txtPanNo.Text = dt.Rows[0]["FLDPANNO"].ToString();
                txtUidNo.Text = dt.Rows[0]["FLDUIDNO"].ToString();

                txtAge.Text = dt.Rows[0]["FLDEMPLOYEEAGE"].ToString();
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
                    txtEmployeeStatus.Text = dt.Rows[0]["FLDOFFSHORESTATUS"].ToString();
                else
                    txtEmployeeStatus.Text = dt.Rows[0]["FLDSTATUS"].ToString() + "/" + dt.Rows[0]["FLDSTATUSNAME"].ToString();
                ucBatch.SelectedBatch = dt.Rows[0]["FLDTRAININGBATCH"].ToString();
                if (dt.Rows[0]["FLDNTBRMGRSTATUS"].ToString() == "1" || dt.Rows[0]["FLDNTBRCORPSTATUS"].ToString() == "1" || dt.Rows[0]["FLDNTBRMGRSTATUS"].ToString() == "2")
                {
                    string strStatus = string.Empty;
                    if (dt.Rows[0]["FLDNTBRMGRSTATUS"].ToString() == "1")
                    {
                        strStatus += "Manager";
                    }
                    else if (dt.Rows[0]["FLDNTBRMGRSTATUS"].ToString() == "2")
                    {
                        strStatus += "Principle";
                    }
                    if (dt.Rows[0]["FLDNTBRCORPSTATUS"].ToString() == "1")
                    {
                        strStatus += "Corporate";
                    }

                    txtEmployeeCode.BackColor = System.Drawing.ColorTranslator.FromHtml("#FAD3BE");
                }

                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();

                if (dt.Rows[0]["FLDISATTACHMENTPANNO"].ToString() == string.Empty)
                    imgPPClipPanNo.ImageUrl = Session["images"] + "/no-attachment.png";
                else
                    imgPPClipPanNo.ImageUrl = Session["images"] + "/attachment.png";

                if (dt.Rows[0]["FLDISATTACHMENTUIDNO"].ToString() == string.Empty)
                    imgPPClipUidNo.ImageUrl = Session["images"] + "/no-attachment.png";
                else
                    imgPPClipUidNo.ImageUrl = Session["images"] + "/attachment.png";

                DataTable dta = PhoenixCommonFileAttachment.AttachmentList(new Guid(dt.Rows[0]["FLDDTKEY"].ToString()), "CREWIMAGE");
                ViewState["dtkey"] = string.Empty;
                txtuserid.Text = dt.Rows[0]["FLDMENTORID"].ToString();
                txtMentorName.Text = dt.Rows[0]["FLDMENTORNAME"].ToString();
                if (dta.Rows.Count > 0)
                {
                    ViewState["dtkey"] = dta.Rows[0]["FLDDTKEY"].ToString();
                    imgPhoto.ImageUrl = "../Common/Download.aspx?dtkey=" + dta.Rows[0]["FLDDTKEY"].ToString(); // "?time=" + DateTime.Now.TimeOfDay;
                    //PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "\\" + dta.Rows[0]["FLDFILEPATH"].ToString();
                    aCrewImg.HRef = "#";
                    aCrewImg.Attributes["onclick"] = "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/Download.aspx?dtkey=" + dta.Rows[0]["FLDDTKEY"].ToString() + "');";
                }
                DataTable dts = PhoenixCommonFileAttachment.AttachmentList(new Guid(dt.Rows[0]["FLDDTKEY"].ToString()), "CREWRESUME");
                if (dts.Rows.Count > 0)
                {
                    lnkResume.Enabled = true;
                    lnkResume.Attributes["onclick"] = "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/Download.aspx?dtkey=" + dts.Rows[0]["FLDDTKEY"].ToString() + "');";
                }
                else
                {
                    lnkResume.Enabled = false;
                    lnkResume.Attributes.Add("style", "color:darkgray;font-style:italic;font-weight:bold");
                }

                if (dt.Rows[0]["FLDDIRECTSIGNON"].ToString() == "1")
                {
                    chkDirectSignon.Checked = true;
                    txtWeight.CssClass = "input";
                    txtWeight.ReadOnly = false;
                }
                else
                {
                    txtWeight.CssClass = "readonlytextbox";
                    txtWeight.ReadOnly = true;
                    chkDirectSignon.Checked = false;
                }
                ddlBoilerSuitSize.SelectedValue = dt.Rows[0]["FLDBOILERSUITSIZE"].ToString();
                txtPassportName.Text = dt.Rows[0]["FLDNAMEASPERPASSPORT"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public static bool IsValidTextBox(string text)
    {

        string regex = "^[0-9a-zA-Z ]+$";
        Regex re = new Regex(regex);
        if (!re.IsMatch(text))
            return (false);

        return true;
    }

    public static bool IsValidName(string text)
    {

        string regex = "^[a-zA-Z. ]+$";
        Regex re = new Regex(regex);
        if (!re.IsMatch(text))
            return (false);

        return true;
    }

    public static bool IsValidFileNo(string text)
    {

        string regex = "^[0-9a-zA-Z]+$";
        Regex re = new Regex(regex);
        if (!re.IsMatch(text))
            return (false);

        return true;
    }
    private bool IsValidate()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        Int32 resultInt;
        decimal resultDec;

        if (!IsValidFileNo(txtEmployeeCode.Text.Trim()))
            ucError.ErrorMessage = "File Number should not contain special characters/spaces";
        if (txtFirstname.Text.Trim() == "")
            ucError.ErrorMessage = "First Name is required";
        if (Int32.TryParse(ucSex.SelectedHard.Trim(), out resultInt) == false)
            ucError.ErrorMessage = "Gender is required";
        if (!DateTime.TryParse(txtDateofBirth.Text, out resultDate))
        {
            ucError.ErrorMessage = "Date Of Birth is required";
        }
        else if (DateTime.TryParse(txtDateofBirth.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now.AddYears(-16)) > 0)
        {
            ucError.ErrorMessage = "Must be above a minimum age of 16 years and above.";
        }
        if (Int32.TryParse(ucNationality.SelectedNationality, out resultInt) == false)
            ucError.ErrorMessage = "Nationality is required";
        if (ucRank.SelectedRank.Trim() == "" || ucRank.SelectedRank.Trim() == "Dummy")
            ucError.ErrorMessage = "Rank Applied is required";
        if (ucMaritialStatus.SelectedMaritalStatus.Trim() == "" || ucMaritialStatus.SelectedMaritalStatus.Trim() == "Dummy")
            ucError.ErrorMessage = "Civil Status is required";
        if (ucPool.SelectedPool.Trim() == "" || ucPool.SelectedPool.Trim() == "Dummy")
            ucError.ErrorMessage = "Pool is required";
        if (!decimal.TryParse(txtHeight.Text, out resultDec))
        {
            ucError.ErrorMessage = "Height cannot be blank.";
        }
        if (txtHairColor.Text.Trim() == "")
            ucError.ErrorMessage = "Hair Colour is required";

        if (txtEyeColor.Text.Trim() == "")
            ucError.ErrorMessage = "Eye Colour is required";

        if (txtDistinguishMark.Text.Trim() == "")
            ucError.ErrorMessage = "Distinguish Mark is required";

        if (int.TryParse(ucBatch.SelectedBatch, out resultInt) == false)
            ucError.ErrorMessage = "Batch is required";

        if (txtEmployeeCode.Text.Length == 0)
        {
            ucError.ErrorMessage = "File No is required";
        }
        if (txtEmployeeCode.Text.Length != 0)
        {
            string substring = txtEmployeeCode.Text.Trim().ToUpper().Substring(1, txtEmployeeCode.Text.Trim().Length - 1);
            if ((txtEmployeeCode.Text.Trim().ToUpper().Substring(0, 1) != "E"))
            {
                ucError.ErrorMessage = "Enter Valid File No";
            }

            else if (!int.TryParse(substring, out resultInt))
            {
                ucError.ErrorMessage = "Enter Valid File No";
            }
        }

        if (!IsValidName(txtFirstname.Text.Trim()) && !String.IsNullOrEmpty(txtFirstname.Text.Trim()))
            ucError.ErrorMessage = "Firstname should contain alphabets only";

        if (!IsValidName(txtMiddlename.Text.Trim()) && !String.IsNullOrEmpty(txtMiddlename.Text.Trim()))
            ucError.ErrorMessage = "Middlename should contain alphabets only";

        if (!IsValidName(txtLastname.Text.Trim()) && !String.IsNullOrEmpty(txtLastname.Text.Trim()))
            ucError.ErrorMessage = "Lastname should contain alphabets only";


        return (!ucError.IsError);
    }

    private void ResetFormControlValues()
    {
        try
        {
            Filter.CurrentCrewSelection = null;
            txtEmployeeCode.Text = "";
            txtFirstname.Text = "";
            txtMiddlename.Text = "";
            txtLastname.Text = "";
            ucSex.SelectedHard = null;
            txtPassport.Text = "";
            txtSeamenBookNumber.Text = "";
            txtDateofBirth.Text = "";
            txtPlaceofBirth.Text = "";
            ucMaritialStatus.SelectedMaritalStatus = null;
            ucNationality.SelectedNationality = null;
            ucZone.SelectedZone = null;
            ucPool.SelectedPool = null;
            txtWeight.Text = "";
            ddlShoesSize.SelectedValue = null;
            ucRank.SelectedRank = null;
            txtAppliedOn.Text = "";
            txtLastVessel.Text = "";
            ddlRankPosted.SelectedRank = null;
            txtDateofJoin.Text = "";
            txtINDOsNumber.Text = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CalculateBMI(object sender, EventArgs e)
    {

        string desc = string.Empty;
        decimal wt, ht, bmi;
        if (decimal.TryParse(txtWeight.Text, out wt) && decimal.TryParse(txtHeight.Text, out ht))
        {
            if (txtWeight.Text != "0.00" && txtHeight.Text != "0.00")
            {
                bmi = Math.Round(wt / ((ht / 100) * (ht / 100)), 0);
                txtBMI.Text = bmi.ToString();
                if (bmi < 20)
                    desc = "Underweight";
                else if (bmi >= 20 && bmi <= 24)
                    desc = "Normal Weight";
                else if (bmi >= 25 && bmi <= 29)
                    desc = "Light to Moderate Overweight";
                else if (bmi >= 30 && bmi <= 39)
                    desc = "Strong Overweight";
                else if (bmi >= 40)
                    desc = "Extreme Overweight";
                if (!string.IsNullOrEmpty(txtDateofBirth.Text))
                {
                    TimeSpan s = DateTime.Now.Subtract(DateTime.Parse(txtDateofBirth.Text));
                    int age = s.Days / 365;
                    if (age >= 19 && age <= 24)
                        desc += ", Desireable BMI : 19 - 24";
                    if (age >= 25 && age <= 34)
                        desc += ", Desireable BMI : 20 - 24";
                    if (age >= 35 && age <= 44)
                        desc += ", Desireable BMI : 22 - 27";
                    if (age >= 45 && age <= 54)
                        desc += ", Desireable BMI : 23 - 28";
                    if (age >= 65)
                        desc += ", Desireable BMI : 24 - 29";
                }
                txtBMI.ToolTip = desc;
                txtBMI.BackColor = BMIColor(bmi);

            }
            else
            {
                txtBMI.BackColor = Color.Empty;
                txtBMI.CssClass = "readonlytextbox";
                txtBMI.Text = "";
            }
        }
    }

    private string BMIChart()
    {
        string strBMI = "<table style=\"padding: 1px; margin: 1px; border-style: solid; border-width: 1px;\"><tr><td>&nbsp;</td>{header}</tr>";
        string header = string.Empty;
        decimal bmi;
        for (double i = 1.50; i <= 2.00; i = i + .02)
        {
            i = Math.Round(i, 2);
            strBMI += "<tr>";
            for (int j = 40; j <= 118; j = j + 2)
            {
                if (i == 1.50)
                    header += "<th>" + j + "</th>";
                if (j == 40)
                    strBMI += "<th>" + i + "</th>";
                bmi = (decimal)Math.Round(j / (i * i));
                strBMI += "<td style=\"background-color: " + ColorTranslator.ToHtml(BMIColor(bmi)) + "\">" + bmi + "</td>";
            }
            strBMI += "</tr>";
        }
        strBMI += "</table>";
        strBMI += @" <table width='100%' style='padding: 1px; margin: 1px; border-style: solid; border-width: 1px;'>
        <tr>
            <th colspan='2'>
                General Definition
            </th>
             <th colspan='2'>
                Desireable BMI
            </th>
        </tr>
        <tr>
            <td>
                BMI < 20    Underweight
            </td>
            <td>
                BMI 20  -   24    Normal Weight
            </td>
            <td>
                19 - 24 Years   19 - 24
            </td>
            <td>
                25 - 34 Years   20 - 24
            </td>
        </tr>
         <tr>
            <td>
                 BMI 25  -   29    Light to Moderate Overweight
            </td>
            <td>
                 BMI 30  -   39    Strong Overweight
            </td>
            <td>
                35 - 44 Years   21 - 26
            </td>
            <td>
                45 - 54 Years   22 - 27
            </td>
        </tr>
        <tr>
            <td colspan='2'>
                 BMI > 40    Extreme Overweight
            </td>           
            <td>
                55 - 64 Years   23 - 28
            </td>
            <td>
                > 65 Years   24 - 29
            </td>
        </tr>
    </table>";
        return strBMI.Replace("{header}", header); ;
    }

    private Color BMIColor(decimal iBMI)
    {
        Color c = new Color();
        if (iBMI >= 20 && iBMI <= 24)
            c = System.Drawing.Color.FromArgb(255, 255, 255);
        else if ((iBMI >= 19 && iBMI < 20) || (iBMI > 24 && iBMI <= 28))
            c = System.Drawing.Color.FromArgb(255, 255, 153);
        else if ((iBMI > 16 && iBMI <= 18) || (iBMI > 28 && iBMI <= 32))
            c = System.Drawing.Color.FromArgb(255, 204, 153);
        else if (iBMI <= 16 || iBMI > 32)
            c = System.Drawing.Color.FromArgb(255, 0, 0);
        return c;
    }

    protected void DirectSignOnfn(object sender, EventArgs e)
    {
        if (chkDirectSignon.Checked == true)
        {
            txtWeight.CssClass = "input";
            txtWeight.ReadOnly = false;
        }
        else
        {
            txtWeight.CssClass = "readonlytextbox";
            txtWeight.ReadOnly = true;
        }
    }


}
