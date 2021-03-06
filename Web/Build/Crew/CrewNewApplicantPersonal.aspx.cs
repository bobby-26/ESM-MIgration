using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Drawing;
using System.Web;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
using SouthNests.Phoenix.CrewOffshore;
public partial class CrewNewApplicantPersonal : PhoenixBasePage
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

            toolbarmain.AddButton("OC - 9", "EXPORTOC9", ToolBarDirection.Right);
            if (Request.QueryString["portal"] != null && Request.QueryString["portal"].ToString() == "1")
                toolbarmain.AddButton("Edit Details", "EMPSAVE", ToolBarDirection.Right);
            else
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            if (Request.QueryString["r"] == null && (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "NEWAPPLICANT"))
                toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
            DataTable dt = PhoenixCrewPortalDetailsConfirmation.EmployeeDetailConfirmationList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            if (dt.Rows.Count > 0 && Request.QueryString["portal"] == null)
                toolbarmain.AddButton("Approve Details", "APPROVE", ToolBarDirection.Right);
            CrewPersonaltab.AccessRights = this.ViewState;
            CrewPersonaltab.MenuList = toolbarmain.Show();
            if (!Page.IsPostBack)
            {
                HookOnFocus(this.Page as Control);
                divBMI.InnerHtml = BMIChart();
                txtBMI.Attributes["onclick"] = "javascript:showBMI();";

                ddlSex.HardTypeCode = ((int)PhoenixHardTypeCode.SEX).ToString();
                if (Filter.CurrentNewApplicantSelection != null)
                {
                    ListNewApplicantInformation();

                    imgPDForm.Attributes.Add("onclick", "openNewWindow('PDForm', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + Filter.CurrentNewApplicantSelection + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");

                    imgPPClipPanNo.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                   + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.PANNO + "&cmdname=FPANNOUPLOAD'); return false;";

                    imgPPClipUidNo.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.UIDNO + "&cmdname=FUIDUPLOAD'); return false;";
                    CalculateBMI(null, null);
                }
                if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom != "")
                {
                    lblZone.Text = "Manning Office";
                    imgPDForm.Visible = true;
                    imgPDForm.Attributes.Add("onclick", "openNewWindow('PDForm', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + Filter.CurrentNewApplicantSelection + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");

                    cmdEmail.Visible = true;
                    cmdEmail.Attributes.Add("onclick", "openNewWindow('codehelp2','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEmployeeDocumentList.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&pdform=1" + "');return false;");
                }


                if (Request.QueryString["t"] != null)
                    Response.Redirect("CrewNewApplicantRegister.aspx", false);
                Page.ClientScript.RegisterStartupScript(
                typeof(CrewNewApplicantPersonal),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);
                if (PhoenixGeneralSettings.CurrentGeneralSetting.CountryCode != "IND")
                {
                    txtPanNo.ReadOnly = true;
                    txtPanNo.CssClass = "readonlytextbox";
                    txtUidNo.ReadOnly = true;
                    txtUidNo.CssClass = "readonlytextbox";
                    ddlZone.Enabled = false;
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
    protected void CrewPersonaltab_TabStripCommand(object sender, EventArgs e)
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
                if (Filter.CurrentNewApplicantSelection == null)
                {
                    SaveCrewPersonalInformation();
                }
                else
                {
                    UpdateCrewPersonalInformation();
                }
            }
            if (CommandName.ToUpper().Equals("EMPSAVE"))
            {

                String script = String.Format("javascript:openNewWindow('spnPickListVendor', 'codehelp1', '" + Session["sitepath"] + "/CrewOffshore/CrewPersonalDetailsBySeafarerEdit.aspx?empid=" + General.GetNullableInteger(Filter.CurrentNewApplicantSelection) + "&portal=1');");

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }

            if (CommandName.ToUpper().Equals("EXPORTOC9"))
            {
                PhoenixCrew2XL.Export2XLOC9(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            }
            if (CommandName.ToUpper().Equals("OC9A"))
            {
                Response.Redirect("../Crew/CrewOfferLetterList.aspx?Type=n&empid=" + Filter.CurrentNewApplicantSelection);
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                Response.Redirect("CrewNewApplicantRegister.aspx?r=n", false);
            }
            if (CommandName.ToUpper().Equals("APPROVE"))
            {

                String script = String.Format("javascript:openNewWindow('spnPickListVendor', 'codehelp1', '" + Session["sitepath"] + "/CrewOffshore/CrewPersonalDetailsBySeafarerEdit.aspx?empid=" + General.GetNullableInteger(Filter.CurrentNewApplicantSelection) + "');");

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    public void SaveCrewPersonalInformation()
    {
        DataTable dt = PhoenixNewApplicantManagement.InsertEmployee(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                             , General.GetNullableString(txtPassport.Text.Trim().ToUpper().Replace(" ", string.Empty))
                                             , General.GetNullableString(txtSeamenBookNumber.Text.ToUpper().Replace(" ", "").Trim())
                                             , txtFirstname.Text.Trim().ToUpper()
                                             , General.GetNullableString(txtLastName.Text.Trim().ToUpper())
                                             , General.GetNullableString(txtMiddleName.Text.Trim().ToUpper())
                                             , null, null, null
                                             , General.GetNullableInteger(ddlNationality.SelectedNationality)
                                             , General.GetNullableDateTime(txtDateofBirth.Text.Trim())
                                             , General.GetNullableString(txtPlaceofBirth.Text.Trim().ToUpper())
                                             , General.GetNullableInteger(ddlRank.SelectedRank)
                                             , General.GetNullableInteger(ddlZone.SelectedZone.ToString())
                                             , General.GetNullableInteger(ddlSex.SelectedHard)
                                             , General.GetNullableInteger(ddlMaritialStatus.SelectedMaritalStatus.ToString())
                                             , General.GetNullableDecimal(txtHeight.Text.Trim())
                                             , General.GetNullableDecimal(txtWeight.Text.Trim())
                                             , General.GetNullableDecimal(ddlShoesSize.SelectedValue)
                                             , txtINDOsNumber.Text.Trim().ToUpper()
                                             , null
                                             , null
                                             , null
                                             , null
                                             , General.GetNullableString(txtHairColor.Text.Trim().ToUpper())
                                             , General.GetNullableString(txtEyeColor.Text.Trim().ToUpper())
                                             , General.GetNullableString(txtDistinguishMark.Text.Trim().ToUpper())
                                             , General.GetNullableString(txtPanNo.Text.Trim().ToUpper())
                                             , General.GetNullableString(txtUidNo.Text.Trim().ToUpper())
                                             , General.GetNullableString(ddlBoilerSuitSize.SelectedValue)
                                             , General.GetNullableString(txtPassportName.Text.ToUpper())
                                             );
        if (dt.Rows.Count > 0)
        {
            if (Request.Files.Count > 0)
            {
                foreach (UploadedFile postedFile in txtFileUpload.UploadedFiles)
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

                            Filter.CurrentNewApplicantSelection = dt.Rows[0][0].ToString();
                            PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(dt.Rows[0][1].ToString()), PhoenixModule.CREW, null, ".jpg,.png,.gif", string.Empty, "CREWIMAGE");

                        }
                    }
                }
            }
        }
        Response.Redirect("CrewNewApplicantAddress.aspx", false);
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

    private bool IsValidate()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        int resultInt;
        if (txtFirstname.Text == string.Empty)
        {
            ucError.ErrorMessage = "First Name cannot be blank";
        }
        if (txtRepFileNo.Text.Trim() == string.Empty)
            ucError.ErrorMessage = "File No cannot be blank";

        if (txtRepFileNo.Text.Trim() != string.Empty && !IsValidFileNo(txtRepFileNo.Text.Trim()))
            ucError.ErrorMessage = "File No should not contain special characters/spaces";

        if (!string.IsNullOrEmpty(txtDateofBirth.Text))
        {
            if (!DateTime.TryParse(txtDateofBirth.Text, out resultDate))
            {
                ucError.ErrorMessage = "Date of Birth not in correct format";
            }
            else if (DateTime.TryParse(txtDateofBirth.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now.AddYears(-16)) > 0)
            {
                ucError.ErrorMessage = "Must be above a minimum age of 16 years and above.";
            }
        }

        if (!string.IsNullOrEmpty(txtRepFileNo.Text.Trim()))
        {

            if (txtRepFileNo.Text.Length != 0)
            {
                string substring = txtRepFileNo.Text.Trim().ToUpper().Substring(1, txtRepFileNo.Text.Trim().Length - 1);
                if ((txtRepFileNo.Text.ToUpper() != "NA") && (txtRepFileNo.Text.Trim().ToUpper().Substring(0, 1) != "E"))
                {
                    ucError.ErrorMessage = "Enter NA or valid File No";
                }

                else if (txtRepFileNo.Text.ToUpper() != "NA" && !int.TryParse(substring, out resultInt))
                {
                    ucError.ErrorMessage = "Enter Valid File No";
                }
            }
        }

        if (General.GetNullableInteger(ddlRank.SelectedRank) == null)
            ucError.ErrorMessage = "Applied rank is required";

        if (General.GetNullableInteger(ddlMaritialStatus.SelectedMaritalStatus) == null)
            ucError.ErrorMessage = "Civil Status is required";

        if (!IsValidName(txtFirstname.Text.Trim()) && !String.IsNullOrEmpty(txtFirstname.Text.Trim()))
            ucError.ErrorMessage = "Firstname should contain alphabets only";

        if (!IsValidName(txtMiddleName.Text.Trim()) && !String.IsNullOrEmpty(txtMiddleName.Text.Trim()))
            ucError.ErrorMessage = "Middlename should contain alphabets only";

        if (!IsValidName(txtLastName.Text.Trim()) && !String.IsNullOrEmpty(txtLastName.Text.Trim()))
            ucError.ErrorMessage = "Lastname should contain alphabets only";

        return (!ucError.IsError);
    }
    public static bool IsValidFileNo(string text)
    {

        string regex = "^[0-9a-zA-Z]+$";
        Regex re = new Regex(regex);
        if (!re.IsMatch(text))
            return (false);

        return true;
    }
    public void ListNewApplicantInformation()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            txtFirstname.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
            txtEmployeeCode.Text = dt.Rows[0]["FLDFILENO"].ToString();
            txtFirstname.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
            txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
            txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
            ddlSex.SelectedHard = dt.Rows[0]["FLDSEX"].ToString();
            txtPassport.Text = dt.Rows[0]["FLDPASSPORTNO"].ToString();
            txtSeamenBookNumber.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
            ucPool.SelectedPool = dt.Rows[0]["FLDPOOL"].ToString();
            txtDateofBirth.Text = dt.Rows[0]["FLDDATEOFBIRTH"].ToString();
            txtPlaceofBirth.Text = dt.Rows[0]["FLDPLACEOFBIRTH"].ToString();
            ddlMaritialStatus.SelectedMaritalStatus = dt.Rows[0]["FLDMARITALSTATUS"].ToString();
            ddlNationality.SelectedNationality = dt.Rows[0]["FLDNATIONALITY"].ToString();
            ddlZone.SelectedZone = dt.Rows[0]["FLDZONE"].ToString();
            if (dt.Rows[0]["FLDHEIGHT"].ToString() != "" && dt.Rows[0]["FLDHEIGHT"].ToString() != "0.00")
            {
                txtHeight.Text = dt.Rows[0]["FLDHEIGHT"].ToString();
                txtHeight.Text = txtHeight.Text.Substring(0, txtHeight.Text.IndexOf('.'));

            }
            if (dt.Rows[0]["FLDWEIGHT"].ToString() != "" && dt.Rows[0]["FLDWEIGHT"].ToString() != "0.00")
            {
                txtWeight.Text = dt.Rows[0]["FLDWEIGHT"].ToString();
                txtWeight.Text = txtWeight.Text.Substring(0, txtWeight.Text.IndexOf('.'));
            }
            ddlShoesSize.SelectedValue = dt.Rows[0]["FLDSHOESCMS"].ToString();
            ddlRank.SelectedRank = dt.Rows[0]["FLDRANK"].ToString();
            txtCreatedBy.Text = dt.Rows[0]["FLDCREATEDBY"].ToString();
            txtAppliedOn.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDAPPLIEDON"].ToString()));
            txtINDOsNumber.Text = dt.Rows[0]["FLDINDOSNO"].ToString();
            ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            txtHairColor.Text = dt.Rows[0]["FLDHAIRCOLOR"].ToString();
            txtEyeColor.Text = dt.Rows[0]["FLDEYECOLOR"].ToString();
            txtDistinguishMark.Text = dt.Rows[0]["FLDDISTINGUISHINGMARK"].ToString();
            txtRepFileNo.Text = dt.Rows[0]["FLDREPFILENO"].ToString();
            DataTable dta = PhoenixCommonFileAttachment.AttachmentList(new Guid(dt.Rows[0]["FLDDTKEY"].ToString()), "CREWIMAGE");
            ViewState["dtkey"] = string.Empty;
            ucPool.Enabled = SessionUtil.CanAccess(this.ViewState, "POOL");
            txtAge.Text = dt.Rows[0]["FLDEMPLOYEEAGE"].ToString();
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
                txtEmployeeStatus.Text = dt.Rows[0]["FLDOFFSHORESTATUS"].ToString();
            else
                txtEmployeeStatus.Text = dt.Rows[0]["FLDSTATUS"].ToString() + "/" + dt.Rows[0]["FLDSTATUSNAME"].ToString();
            ucBatch.SelectedBatch = dt.Rows[0]["FLDTRAININGBATCH"].ToString();
            txtuserid.Text = dt.Rows[0]["FLDMENTORID"].ToString();
            txtMentorName.Text = dt.Rows[0]["FLDMENTORNAME"].ToString();
            txtPanNo.Text = dt.Rows[0]["FLDPANNO"].ToString();
            txtUidNo.Text = dt.Rows[0]["FLDUIDNO"].ToString();
            DataTable dts = PhoenixCommonFileAttachment.AttachmentList(new Guid(dt.Rows[0]["FLDDTKEY"].ToString()), "CREWRESUME");
            ViewState["dtresumekey"] = "";

            ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();

            if (dt.Rows[0]["FLDISATTACHMENTPANNO"].ToString() == "0")
                imgPPClipPanNo.ImageUrl = Session["images"] + "/no-attachment.png";
            else
                imgPPClipPanNo.ImageUrl = Session["images"] + "/attachment.png";

            if (dt.Rows[0]["FLDISATTACHMENTUIDNO"].ToString() == "0")
                imgPPClipUidNo.ImageUrl = Session["images"] + "/no-attachment.png";
            else
                imgPPClipUidNo.ImageUrl = Session["images"] + "/attachment.png";

            ViewState["dtkey"] = string.Empty;
            txtuserid.Text = dt.Rows[0]["FLDMENTORID"].ToString();
            txtMentorName.Text = dt.Rows[0]["FLDMENTORNAME"].ToString();
            if (dta.Rows.Count > 0)
            {
                ViewState["dtkey"] = dta.Rows[0]["FLDDTKEY"].ToString();
                imgPhoto.ImageUrl = ".." + "/Common/Download.aspx?dtkey=" + dta.Rows[0]["FLDDTKEY"].ToString();
                aCrewImg.HRef = "#";
                aCrewImg.Attributes["onclick"] = "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/Download.aspx?dtkey=" + dta.Rows[0]["FLDDTKEY"].ToString() + "');";
            }

            if (dts.Rows.Count > 0)
            {
                ViewState["dtresumekey"] = dts.Rows[0]["FLDDTKEY"].ToString();
                imgResume.Attributes["onclick"] = "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/Download.aspx?dtkey=" + dts.Rows[0]["FLDDTKEY"].ToString() + "');";
            }
            ddlBoilerSuitSize.SelectedValue = dt.Rows[0]["FLDBOILERSUITSIZE"].ToString();
            txtPassportName.Text = dt.Rows[0]["FLDNAMEASPERPASSPORT"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void UpdateCrewPersonalInformation()
    {
        if (Request.Files.Count > 0)
        {
            foreach (UploadedFile postedFile in txtFileUpload.UploadedFiles)
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
            if (Request.Files["UploadResume"].ContentLength <= 0 && Request.Files["UploadResume"].FileName != "")
            {
                ucError.Text = "Cannot be a blank Resume";
                ucError.Visible = true;
                return;
            }
            else
            {
                if (Request.Files["UploadResume"].ContentLength > 0)
                {
                    if (string.IsNullOrEmpty(ViewState["dtresumekey"].ToString()))
                    {
                        PhoenixCommonFileAttachment.InsertAttachment(Request.Files["UploadResume"], new Guid(ViewState["attachmentcode"].ToString()), PhoenixModule.CREW, null, null, string.Empty, "CREWRESUME");

                    }
                    else
                    {
                        PhoenixCommonFileAttachment.UpdateAttachment(Request.Files["UploadResume"], new Guid(ViewState["dtresumekey"].ToString()), PhoenixModule.CREW, null);
                    }


                }
            }
            PhoenixNewApplicantManagement.UpdateEmployee(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , General.GetNullableString(txtPassport.Text.Trim().ToUpper().Replace(" ", string.Empty))
                                                , General.GetNullableString(txtSeamenBookNumber.Text.ToUpper().Trim().Replace(" ", string.Empty))
                                                , txtFirstname.Text.Trim().ToUpper()
                                                , General.GetNullableString(txtLastName.Text.Trim().ToUpper())
                                                , General.GetNullableString(txtMiddleName.Text.Trim().ToUpper())
                                                , null
                                                , null
                                                , null
                                                , General.GetNullableInteger(ddlNationality.SelectedNationality)
                                                , General.GetNullableDateTime(txtDateofBirth.Text)
                                                , General.GetNullableString(txtPlaceofBirth.Text.Trim().ToUpper())
                                                , General.GetNullableInteger(ddlRank.SelectedRank)
                                                , General.GetNullableInteger(ddlZone.SelectedZone.ToString())
                                                , General.GetNullableInteger(ddlSex.SelectedHard)
                                                , General.GetNullableInteger(ddlMaritialStatus.SelectedMaritalStatus.ToString())
                                                , General.GetNullableDecimal(txtHeight.Text.Trim())
                                                , General.GetNullableDecimal(txtWeight.Text.Trim())
                                                , General.GetNullableDecimal(ddlShoesSize.SelectedValue)
                                                , General.GetNullableString(txtINDOsNumber.Text.Trim().ToUpper().Replace(" ", string.Empty))
                                                , null
                                                , null
                                                , null
                                                , null
                                                , General.GetNullableInteger(Filter.CurrentNewApplicantSelection)
                                                , General.GetNullableString(txtHairColor.Text.Trim().ToUpper())
                                                , General.GetNullableString(txtEyeColor.Text.Trim().ToUpper())
                                                , General.GetNullableString(txtDistinguishMark.Text.Trim().ToUpper())
                                                , General.GetNullableString(txtRepFileNo.Text.ToUpper().Trim())
                                                , General.GetNullableInteger(ucPool.SelectedPool)
                                                , General.GetNullableInteger(ucBatch.SelectedBatch)
                                                , General.GetNullableInteger(txtuserid.Text.Trim())
                                                , General.GetNullableString(txtPanNo.Text.Trim().ToUpper())
                                                , General.GetNullableString(txtUidNo.Text.Trim().ToUpper())
                                                , General.GetNullableString(ddlBoilerSuitSize.SelectedValue)
                                                , General.GetNullableString(txtPassportName.Text.ToUpper()));

            ucStatus.Text = "New Applicant Information Updated.";
            ListNewApplicantInformation();
            CalculateBMI(null, null);
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
}
