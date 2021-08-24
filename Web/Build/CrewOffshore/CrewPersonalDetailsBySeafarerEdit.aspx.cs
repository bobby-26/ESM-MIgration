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
public partial class CrewOffshore_CrewPersonalDetailsBySeafarerEdit : PhoenixBasePage
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
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            if (Request.QueryString["portal"] != null && Request.QueryString["portal"].ToString() == "1")
            {
                toolbarmain.AddButton("Send to Office", "EMPSAVE", ToolBarDirection.Right);
                tablepassport.Visible = false;
                tableseamanbook.Visible = false;
            }
            else
            {
                tablepassport.Visible = true;
                tableseamanbook.Visible = true;
                toolbarmain.AddButton("Approve", "APPROVE", ToolBarDirection.Right);
                toolbarmain.AddButton("Reject", "REJECT", ToolBarDirection.Right);
            }

            CrewPersonaltab.AccessRights = this.ViewState;
            CrewPersonaltab.MenuList = toolbarmain.Show();
            if (!Page.IsPostBack)
            {
                HookOnFocus(this.Page as Control);
                //divBMI.InnerHtml = BMIChart();
                // txtBMI.Attributes["onclick"] = "javascript:showBMI();";

                ddlSex.HardTypeCode = ((int)PhoenixHardTypeCode.SEX).ToString();
                ddlSexemp.HardTypeCode = ((int)PhoenixHardTypeCode.SEX).ToString();
                if (Filter.CurrentNewApplicantSelection != null || Filter.CurrentCrewSelection != null)
                {
                    if (Filter.CurrentNewApplicantSelection != null) ViewState["empid"] = Filter.CurrentNewApplicantSelection;
                    if (Filter.CurrentCrewSelection != null) ViewState["empid"] = Filter.CurrentCrewSelection;
                    ListNewApplicantInformation();
                    ListEmployeeDetailConformation();

                    SetEmployeePassportDetails();
                    SetEmployeePassportDetailsBySeafarer();
                    SetEmployeeSeamanBookDetails();
                    SetEmployeeSeamanBookDetailsBySeafarer();



                    imgPDForm.Attributes.Add("onclick", "openNewWindow('PDForm', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + Filter.CurrentNewApplicantSelection + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");

                    imgPPClipPanNo.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                   + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.PANNO + "&cmdname=FPANNOUPLOAD&u=n'); return false;";

                    imgPPClipUidNo.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.UIDNO + "&cmdname=FUIDUPLOAD&u=n'); return false;";
                    imgPPClipPanNoemp.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcodeemp"] + "&mod="
                 + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.PANNO + "&cmdname=FPANNOUPLOAD'); return false;";

                    imgPPClipUidNoemp.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcodeemp"] + "&mod="
                                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.UIDNO + "&cmdname=FUIDUPLOAD'); return false;";
                    CalculateBMI(null, null);

                    imgPPClipemp.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcodeemp"] + "&mod="
               + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.PASSPORT + "&cmdname=PASSPORTUPLOAD'); return false;";

                    imgCCClipemp.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcodeemp"] + "&mod="
                     + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.SEAMANBOOK + "&cmdname=SEAMANBOOKUPLOAD'); return false;";

                }
                if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom != "")
                {
                    //lblZone.Text = "Manning Office";
                    imgPDForm.Visible = true;
                    imgPDForm.Attributes.Add("onclick", "openNewWindow('PDForm', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + Filter.CurrentNewApplicantSelection + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");
                }


                if (Request.QueryString["t"] != null)
                    Response.Redirect("CrewNewApplicantRegister.aspx", false);
                Page.ClientScript.RegisterStartupScript(
                typeof(CrewOffshore_CrewPersonalDetailsBySeafarerEdit),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);
                if (PhoenixGeneralSettings.CurrentGeneralSetting.CountryCode != "IND")
                {
                    txtPanNo.ReadOnly = true;
                    txtPanNo.CssClass = "readonlytextbox";
                    txtUidNo.ReadOnly = true;
                    txtUidNo.CssClass = "readonlytextbox";
                    //ddlZone.Enabled = false;
                    //txtMentorName.ReadOnly = true;
                    //txtMentorName.CssClass = "readonlytextbox";
                    //imguser.Visible = false;
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

            if (CommandName.ToUpper().Equals("EMPSAVE"))
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
                ListEmployeeDetailConformation();
            }


            if (CommandName.ToUpper().Equals("EXPORTOC9"))
            {
                PhoenixCrew2XL.Export2XLOC9(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                Response.Redirect("CrewNewApplicantRegister.aspx?r=n", false);
            }
            if (CommandName.ToUpper().Equals("APPROVE"))
            {
                string pptno = RemoveSpecialCharacters(txtPassportnumberemp.Text);
                string cdcno = RemoveSpecialCharacters(txtSeamanBookNumberemp.Text);
                PhoenixCrewPortalDetailsConfirmation.EmployeeDetailsConfirmationApprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                         , General.GetNullableInteger(Filter.CurrentNewApplicantSelection)
                                         , txtFirstnameemp.Text == string.Empty ? txtFirstname.Text.Trim().ToUpper() : txtFirstnameemp.Text.Trim().ToUpper()
                                         , General.GetNullableString(txtLastNameemp.Text.Trim().ToUpper())
                                         , General.GetNullableString(txtMiddleNameemp.Text.Trim().ToUpper())
                                         , General.GetNullableInteger(ddlMaritialStatusemp.SelectedMaritalStatus.ToString())
                                         , General.GetNullableInteger(ddlRankemp.SelectedRank)
                                         , General.GetNullableDateTime(txtDateofBirthemp.Text)
                                         , General.GetNullableString(txtPlaceofBirthemp.Text.Trim().ToUpper())
                                         , General.GetNullableInteger(ddlNationalityemp.SelectedNationality)
                                         , General.GetNullableString(txtPanNoemp.Text.Trim().ToUpper())
                                         , General.GetNullableString(txtUidNoemp.Text.Trim().ToUpper())
                                         , null//txtINDOsNumberemp.Text.Trim().ToUpper()
                                         , General.GetNullableInteger(ucBatchemp.SelectedBatch.ToString())
                                         , General.GetNullableInteger(ddlSexemp.SelectedHard)
                                         , General.GetNullableDecimal(txtHeightemp.Text.Trim())
                                         , General.GetNullableDecimal(txtWeightemp.Text.Trim())
                                         , General.GetNullableDecimal(ddlShoesSizeemp.SelectedValue)
                                         , General.GetNullableString(txtHairColoremp.Text.Trim().ToUpper())
                                         , General.GetNullableString(txtEyeColoremp.Text.Trim().ToUpper())
                                         , General.GetNullableString(txtDistinguishMarkemp.Text.Trim().ToUpper())
                                         , General.GetNullableString(ddlBoilerSuitSizeemp.SelectedValue)

                                         , General.GetNullableDateTime(ucDateOfIssueemp.Text)
                                         , txtPlaceOfIssueemp.Text
                                         , General.GetNullableDateTime(ucDateOfExpiryemp.Text)
                                         , General.GetNullableInteger(ucECNRemp.SelectedHard)
                                         , General.GetNullableInteger(ucBlankPagesemp.SelectedHard)
                                         , General.GetNullableString(pptno)

                                          , General.GetNullableInteger(ucSeamanCountryemp.SelectedFlag)
                                          , General.GetNullableDateTime(ucSeamanDateOfIssueemp.Text)
                                          , txtSeamanPlaceOfIssueemp.Text
                                          , General.GetNullableDateTime(ucSeamanDateOfExpiryemp.Text)
                                          , General.GetNullableString(cdcno)

                                          , General.GetNullableGuid(ViewState["attachmentcodeemp"].ToString())

                                         );

                ucStatus.Text = "Details approved.";
                ListEmployeeDetailConformation();
                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            if (CommandName.ToUpper().Equals("REJECT"))
            {
                PhoenixCrewPortalDetailsConfirmation.EmployeeDetailReject(General.GetNullableInteger(Request.QueryString["empid"].ToString()));
                ucStatus.Text = "Details rejected.";
                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                ListEmployeeDetailConformation();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    public static string RemoveSpecialCharacters(string str)
    {
        // Strips all special characters and spaces from a string.
        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
    }
    public void SaveCrewPersonalInformation()
    {
        DataTable dt = PhoenixCrewPortalDetailsConfirmation.InsertEmployee(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                         , General.GetNullableInteger(Filter.CurrentNewApplicantSelection)
                                         , txtFirstnameemp.Text == string.Empty ? txtFirstname.Text.Trim().ToUpper() : txtFirstnameemp.Text.Trim().ToUpper()
                                         , General.GetNullableString(txtLastNameemp.Text.Trim().ToUpper())
                                         , General.GetNullableString(txtMiddleNameemp.Text.Trim().ToUpper())
                                         , General.GetNullableInteger(ddlMaritialStatusemp.SelectedMaritalStatus.ToString())
                                         , General.GetNullableInteger(ddlRankemp.SelectedRank)
                                         , General.GetNullableDateTime(txtDateofBirthemp.Text.Trim())
                                         , General.GetNullableString(txtPlaceofBirthemp.Text.Trim().ToUpper())
                                         , General.GetNullableInteger(ddlNationalityemp.SelectedNationality)
                                         , General.GetNullableString(txtPanNoemp.Text.Trim().ToUpper())
                                         , General.GetNullableString(txtUidNoemp.Text.Trim().ToUpper())
                                         , null//txtINDOsNumberemp.Text.Trim().ToUpper()
                                         , General.GetNullableInteger(ucBatchemp.SelectedBatch.ToString())
                                         , General.GetNullableInteger(ddlSexemp.SelectedHard)
                                         , General.GetNullableDecimal(txtHeightemp.Text.Trim())
                                         , General.GetNullableDecimal(txtWeightemp.Text.Trim())
                                         , General.GetNullableDecimal(ddlShoesSizeemp.SelectedValue)
                                         , General.GetNullableString(txtHairColoremp.Text.Trim().ToUpper())
                                         , General.GetNullableString(txtEyeColoremp.Text.Trim().ToUpper())
                                         , General.GetNullableString(txtDistinguishMarkemp.Text.Trim().ToUpper())
                                         , General.GetNullableString(ddlBoilerSuitSizeemp.SelectedValue));

        if (dt.Rows.Count > 0)
        {
            if (Request.Files.Count > 0)
            {
                foreach (UploadedFile postedFile in txtFileUploademp.UploadedFiles)
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
            DataTable dt;
            if (Request.QueryString["portal"] != null && Request.QueryString["portal"].ToString() == "1")
            {
                dt = PhoenixCrewManagement.PortalEmployee(PhoenixSecurityContext.CurrentSecurityContext.UserCode);

                if (dt.Rows.Count > 0)
                {
                    Filter.CurrentCrewSelection = dt.Rows[0]["FLDEMPLOYEEID"].ToString();

                    if (dt.Rows[0]["FLDNEWAPP"].ToString() == "1")
                    {
                        dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
                        txtRepFileNo.Text = dt.Rows[0]["FLDREPFILENO"].ToString();
                    }
                    else
                    {
                        dt = PhoenixCrewPersonal.EmployeeList(Convert.ToInt32(Filter.CurrentCrewSelection));
                        txtRepFileNo.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                    }
                }
            }
            else
            {
                dt = PhoenixCrewPersonal.EmployeeList(Convert.ToInt32(Request.QueryString["empid"].ToString()));
            }

            txtFirstname.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
            txtEmployeeCode.Text = dt.Rows[0]["FLDFILENO"].ToString();
            txtFirstname.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
            txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
            txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
            ddlSex.SelectedHard = dt.Rows[0]["FLDSEX"].ToString();
            //txtPassport.Text = dt.Rows[0]["FLDPASSPORTNO"].ToString();
            //txtSeamenBookNumber.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
            //ucPool.SelectedPool = dt.Rows[0]["FLDPOOL"].ToString();
            txtDateofBirth.Text = dt.Rows[0]["FLDDATEOFBIRTH"].ToString();
            txtPlaceofBirth.Text = dt.Rows[0]["FLDPLACEOFBIRTH"].ToString();
            ddlMaritialStatus.SelectedMaritalStatus = dt.Rows[0]["FLDMARITALSTATUS"].ToString();
            ddlNationality.SelectedNationality = dt.Rows[0]["FLDNATIONALITY"].ToString();
            //ddlZone.SelectedZone = dt.Rows[0]["FLDZONE"].ToString();
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
            //txtCreatedBy.Text = dt.Rows[0]["FLDCREATEDBY"].ToString();
            //txtAppliedOn.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDAPPLIEDON"].ToString()));
            //txtINDOsNumber.Text = dt.Rows[0]["FLDINDOSNO"].ToString();
            ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            txtHairColor.Text = dt.Rows[0]["FLDHAIRCOLOR"].ToString();
            txtEyeColor.Text = dt.Rows[0]["FLDEYECOLOR"].ToString();
            txtDistinguishMark.Text = dt.Rows[0]["FLDDISTINGUISHINGMARK"].ToString();

            DataTable dta = PhoenixCommonFileAttachment.AttachmentList(new Guid(dt.Rows[0]["FLDDTKEY"].ToString()), "CREWIMAGE");
            ViewState["dtkey"] = string.Empty;
            //ucPool.Enabled = SessionUtil.CanAccess(this.ViewState, "POOL");
            //txtAge.Text = dt.Rows[0]["FLDEMPLOYEEAGE"].ToString();
            //if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
            //    txtEmployeeStatus.Text = dt.Rows[0]["FLDOFFSHORESTATUS"].ToString();
            //else
            //    txtEmployeeStatus.Text = dt.Rows[0]["FLDSTATUS"].ToString() + "/" + dt.Rows[0]["FLDSTATUSNAME"].ToString();
            ucBatch.SelectedBatch = dt.Rows[0]["FLDTRAININGBATCH"].ToString();
            //txtuserid.Text = dt.Rows[0]["FLDMENTORID"].ToString();
            //txtMentorName.Text = dt.Rows[0]["FLDMENTORNAME"].ToString();
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
            //txtuserid.Text = dt.Rows[0]["FLDMENTORID"].ToString();
            //txtMentorName.Text = dt.Rows[0]["FLDMENTORNAME"].ToString();
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
            //txtPassportName.Text = dt.Rows[0]["FLDNAMEASPERPASSPORT"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void ListEmployeeDetailConformation()
    {
        try
        {
            DataTable dt = PhoenixCrewPortalDetailsConfirmation.EmployeeDetailConfirmationList(General.GetNullableInteger(ViewState["empid"].ToString()));
            if (dt.Rows.Count > 0)
            {
                txtFirstnameemp.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleNameemp.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastNameemp.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                ddlSexemp.SelectedHard = dt.Rows[0]["FLDSEX"].ToString();
                txtDateofBirthemp.Text = dt.Rows[0]["FLDDATEOFBIRTH"].ToString();
                txtPlaceofBirthemp.Text = dt.Rows[0]["FLDPLACEOFBIRTH"].ToString();
                ddlMaritialStatusemp.SelectedMaritalStatus = dt.Rows[0]["FLDMARITALSTATUS"].ToString();
                ddlNationalityemp.SelectedNationality = dt.Rows[0]["FLDNATIONALITY"].ToString();

                if (dt.Rows[0]["FLDHEIGHT"].ToString() != "" && dt.Rows[0]["FLDHEIGHT"].ToString() != "0.00")
                {
                    txtHeightemp.Text = dt.Rows[0]["FLDHEIGHT"].ToString();
                    //txtHeightemp.Text = txtHeightemp.Text.Substring(0, txtHeightemp.Text.IndexOf('.'));

                }
                if (dt.Rows[0]["FLDWEIGHT"].ToString() != "" && dt.Rows[0]["FLDWEIGHT"].ToString() != "0.00")
                {
                    txtWeightemp.Text = dt.Rows[0]["FLDWEIGHT"].ToString();
                    // txtWeightemp.Text = txtWeightemp.Text.Substring(0, txtWeightemp.Text.IndexOf('.'));
                }
                ddlShoesSizeemp.SelectedValue = dt.Rows[0]["FLDSHOESCMS"].ToString();
                ddlRankemp.SelectedRank = dt.Rows[0]["FLDRANK"].ToString();

                //  txtINDOsNumber.Text = dt.Rows[0]["FLDINDOSNO"].ToString();
                ViewState["attachmentcodeemp"] = dt.Rows[0]["FLDDTKEY"].ToString();
                txtHairColoremp.Text = dt.Rows[0]["FLDHAIRCOLOR"].ToString();
                txtEyeColoremp.Text = dt.Rows[0]["FLDEYECOLOR"].ToString();
                txtDistinguishMarkemp.Text = dt.Rows[0]["FLDDISTINGUISHINGMARK"].ToString();

                DataTable dta = PhoenixCommonFileAttachment.AttachmentList(new Guid(dt.Rows[0]["FLDDTKEY"].ToString()), "CREWIMAGE");
                ViewState["dtkeyemp"] = string.Empty;

                ucBatchemp.SelectedBatch = dt.Rows[0]["FLDTRAININGBATCH"].ToString();

                txtPanNoemp.Text = dt.Rows[0]["FLDPANNO"].ToString();
                txtUidNoemp.Text = dt.Rows[0]["FLDUIDNO"].ToString();
                DataTable dts = PhoenixCommonFileAttachment.AttachmentList(new Guid(dt.Rows[0]["FLDDTKEY"].ToString()), "CREWRESUME");
                ViewState["dtresumekeyemp"] = "";

                ViewState["attachmentcodeemp"] = dt.Rows[0]["FLDDTKEY"].ToString();

                if (dt.Rows[0]["FLDISATTACHMENTPANNO"].ToString() == "0")
                    imgPPClipPanNoemp.ImageUrl = Session["images"] + "/no-attachment.png";
                else
                    imgPPClipPanNoemp.ImageUrl = Session["images"] + "/attachment.png";

                if (dt.Rows[0]["FLDISATTACHMENTUIDNO"].ToString() == "0")
                    imgPPClipUidNoemp.ImageUrl = Session["images"] + "/no-attachment.png";
                else
                    imgPPClipUidNoemp.ImageUrl = Session["images"] + "/attachment.png";

                imgPPClipPanNoemp.Visible = true;
                imgPPClipUidNoemp.Visible = true;
                ViewState["dtkeyemp"] = string.Empty;

                if (dta.Rows.Count > 0)
                {
                    ViewState["dtkeyemp"] = dta.Rows[0]["FLDDTKEY"].ToString();
                    imgPhotoemp.ImageUrl = ".." + "/Common/Download.aspx?dtkey=" + dta.Rows[0]["FLDDTKEY"].ToString();
                    aCrewImgemp.HRef = "#";
                    aCrewImgemp.Attributes["onclick"] = "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/Download.aspx?dtkey=" + dta.Rows[0]["FLDDTKEY"].ToString() + "');";
                }

                if (dts.Rows.Count > 0)
                {
                    ViewState["dtresumekeyemp"] = dts.Rows[0]["FLDDTKEY"].ToString();
                    imgResumeemp.Attributes["onclick"] = "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/Download.aspx?dtkey=" + dts.Rows[0]["FLDDTKEY"].ToString() + "');";
                }
                ddlBoilerSuitSizeemp.SelectedValue = dt.Rows[0]["FLDBOILERSUITSIZE"].ToString();
            }
            else
            {
                imgPPClipPanNoemp.ImageUrl = Session["images"] + "/no-attachment.png";
                imgPPClipPanNoemp.Visible = false;
                imgPPClipUidNoemp.ImageUrl = Session["images"] + "/no-attachment.png";
                imgPPClipUidNoemp.Visible = false;


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void UpdateCrewPersonalInformation()
    {

        DataTable dt = PhoenixCrewPortalDetailsConfirmation.InsertEmployee(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                     , General.GetNullableInteger(Filter.CurrentNewApplicantSelection)
                                     , txtFirstnameemp.Text == string.Empty ? txtFirstname.Text.Trim().ToUpper() : txtFirstnameemp.Text.Trim().ToUpper()
                                     , General.GetNullableString(txtLastNameemp.Text.Trim().ToUpper())
                                     , General.GetNullableString(txtMiddleNameemp.Text.Trim().ToUpper())
                                     , General.GetNullableInteger(ddlMaritialStatusemp.SelectedMaritalStatus.ToString())
                                     , General.GetNullableInteger(ddlRankemp.SelectedRank)
                                     , General.GetNullableDateTime(txtDateofBirthemp.Text)
                                     , General.GetNullableString(txtPlaceofBirthemp.Text.Trim().ToUpper())
                                     , General.GetNullableInteger(ddlNationalityemp.SelectedNationality)
                                     , General.GetNullableString(txtPanNoemp.Text.Trim().ToUpper())
                                     , General.GetNullableString(txtUidNoemp.Text.Trim().ToUpper())
                                     , null//txtINDOsNumberemp.Text.Trim().ToUpper()
                                     , General.GetNullableInteger(ucBatchemp.SelectedBatch.ToString())
                                     , General.GetNullableInteger(ddlSexemp.SelectedHard)
                                     , General.GetNullableDecimal(txtHeightemp.Text.Trim())
                                     , General.GetNullableDecimal(txtWeightemp.Text.Trim())
                                     , General.GetNullableDecimal(ddlShoesSizeemp.SelectedValue)
                                     , General.GetNullableString(txtHairColoremp.Text.Trim().ToUpper())
                                     , General.GetNullableString(txtEyeColoremp.Text.Trim().ToUpper())
                                     , General.GetNullableString(txtDistinguishMarkemp.Text.Trim().ToUpper())
                                     , General.GetNullableString(ddlBoilerSuitSizeemp.SelectedValue));

        dt = PhoenixCrewPortalDetailsConfirmation.EmployeeDetailConfirmationList(General.GetNullableInteger(ViewState["empid"].ToString()));
        if (dt.Rows.Count > 0)
        {
            ViewState["dtkeyemp"] = dt.Rows[0]["FLDDTKEY"].ToString();
            ViewState["dtresumekeyemp"] = dt.Rows[0]["FLDDTKEY"].ToString();
        }
        if (Request.Files.Count > 0)
        {
            foreach (UploadedFile postedFile in txtFileUploademp.UploadedFiles)
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

                        if (string.IsNullOrEmpty(ViewState["dtkeyemp"].ToString()))
                        {

                            PhoenixCommonFileAttachment.InsertAttachment(postedFile, new Guid(ViewState["attachmentcodeemp"].ToString()), PhoenixModule.CREW, null, ".jpg,.png,.gif", string.Empty, "CREWIMAGE");
                        }
                        else
                        {
                            PhoenixCommonFileAttachment.InsertAttachment(postedFile, new Guid(ViewState["dtkeyemp"].ToString()), PhoenixModule.CREW, null, ".jpg,.png,.gif", string.Empty, "CREWIMAGE");
                            //PhoenixCommonFileAttachment.UpdateAttachment(postedFile, new Guid(ViewState["dtkeyemp"].ToString()), PhoenixModule.CREW, ".jpg,.png,.gif");
                        }
                    }
                }
            }
            if (Request.Files["UploadResumeemp"].ContentLength <= 0 && Request.Files["UploadResumeemp"].FileName != "")
            {
                ucError.Text = "Cannot be a blank Resume";
                ucError.Visible = true;
                return;
            }
            else
            {
                if (Request.Files["UploadResumeemp"].ContentLength > 0)
                {
                    if (string.IsNullOrEmpty(ViewState["dtresumekeyemp"].ToString()))
                    {
                        PhoenixCommonFileAttachment.InsertAttachment(Request.Files["UploadResumeemp"], new Guid(ViewState["attachmentcodeemp"].ToString()), PhoenixModule.CREW, null, null, string.Empty, "CREWRESUME");

                    }
                    else
                    {
                        PhoenixCommonFileAttachment.InsertAttachment(Request.Files["UploadResumeemp"], new Guid(ViewState["dtresumekeyemp"].ToString()), PhoenixModule.CREW, null, null, string.Empty, "CREWRESUME");
                        //PhoenixCommonFileAttachment.UpdateAttachment(Request.Files["UploadResume"], new Guid(ViewState["dtresumekeyemp"].ToString()), PhoenixModule.CREW, null);
                    }


                }
            }

            ucStatus.Text = "Details sent to office for approval.";
            ListEmployeeDetailConformation();
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
                //txtBMI.Text = bmi.ToString();
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
                //txtBMI.ToolTip = desc;
                //txtBMI.BackColor = BMIColor(bmi);
            }
            else
            {
                //txtBMI.BackColor = Color.Empty;
                //txtBMI.CssClass = "readonlytextbox";
                //txtBMI.Text = "";
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

    protected void SetEmployeePassportDetails()
    {
        DataTable dt = PhoenixCrewTravelDocument.ListEmployeePassport(Convert.ToInt32(ViewState["empid"].ToString()));

        if (dt.Rows.Count > 0)
        {
            txtPassportnumber.Text = dt.Rows[0]["FLDPASSPORTNO"].ToString();
            ucDateOfIssue.Text = dt.Rows[0]["FLDDATEOFISSUE"].ToString();
            txtPlaceOfIssue.Text = dt.Rows[0]["FLDPLACEOFISSUE"].ToString();
            ucDateOfExpiry.Text = dt.Rows[0]["FLDDATEOFEXPIRY"].ToString();
            ucECNR.SelectedHard = dt.Rows[0]["FLDECNRYESNO"].ToString();
            ucBlankPages.SelectedHard = dt.Rows[0]["FLDMINIMUMPAGE"].ToString();

            //chkpptVerifieddyn.Checked = dt.Rows[0]["FLDPPTVERIFIEDYN"].ToString() == "1" ? true : false;
            //chkpptVerifieddyn.Enabled = dt.Rows[0]["FLDPPTVERIFIEDYN"].ToString() == "1" ? false : true;
            //txtpptVerifiedby.Text = dt.Rows[0]["FLDPPTVERIFIEDBY"].ToString();
            //ucpptVerifieddate.Text = dt.Rows[0]["FLDPPTVERIFIEDDAE"].ToString();

            //chkcrosscheck.Checked = dt.Rows[0]["FLDPPTCROSSCHECKEDYN"].ToString() == "1" ? true : false;
            //chkcrosscheck.Enabled = dt.Rows[0]["FLDPPTCROSSCHECKEDYN"].ToString() == "1" ? false : true;
            //txtpptcrosscheckby.Text = dt.Rows[0]["FLDPPTCROSSCHECKEDBY"].ToString();
            //ucpptcrosscheckdate.Text = dt.Rows[0]["FLDPPTCROSSCHECKEDDAE"].ToString();

            if (dt.Rows[0]["FLDISATTACHMENT"].ToString() == string.Empty)
                imgPPClip.ImageUrl = Session["images"] + "/no-attachment.png";
            else
                imgPPClip.ImageUrl = Session["images"] + "/attachment.png";
            //if (txtPassportnumber.Text != "")
            //{
            //    imgPassportArchive.Visible = true;
            //}
            //else
            //{
            //    imgPassportArchive.Visible = false;
            //}

            if (dt.Rows[0]["FLDPASSPORTNO"].ToString() != string.Empty)
            {
                //txtUpdatedByPPT.Text = dt.Rows[0]["FLDPPTUPDATEDBY"].ToString();
                //ucUpdatedDatePPT.Text = dt.Rows[0]["FLDPPTUPDATEDDATE"].ToString();
            }
            else
            {
                //txtUpdatedByPPT.Text = "";
                //ucUpdatedDatePPT.Text = "";
            }
        }

    }
    protected void SetEmployeeSeamanBookDetails()
    {
        DataTable dt = PhoenixCrewTravelDocument.ListEmployeeSeamanBook(Convert.ToInt32(ViewState["empid"].ToString()));

        if (dt.Rows.Count > 0)
        {
            txtSeamanBookNumber.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
            ucSeamanDateOfIssue.Text = dt.Rows[0]["FLDSDATEOFISSUE"].ToString();
            txtSeamanPlaceOfIssue.Text = dt.Rows[0]["FLDSPLACEOFISSUE"].ToString();
            ucSeamanDateOfExpiry.Text = dt.Rows[0]["FLDSDATEOFEXPIRY"].ToString();
            ucSeamanCountry.SelectedFlag = dt.Rows[0]["FLDSEAMANFLAG"].ToString();

            //chkVerifiedYN.Checked = dt.Rows[0]["FLDCDCVERIFIEDYN"].ToString() == "1" ? true : false;
            //chkVerifiedYN.Enabled = dt.Rows[0]["FLDCDCVERIFIEDYN"].ToString() == "1" ? false : true;
            //txtVerifiedBy.Text = dt.Rows[0]["FLDVERIFIEDBYNAME"].ToString();
            //txtVerifiedOn.Text = dt.Rows[0]["FLDVERIFIEDON"].ToString();

            //chkcdccrosscheckedyn.Checked = dt.Rows[0]["FLDCDCCROSSCHECKEDYN"].ToString() == "1" ? true : false;
            //chkcdccrosscheckedyn.Enabled = dt.Rows[0]["FLDCDCCROSSCHECKEDYN"].ToString() == "1" ? false : true;
            //txtcdccrosscheckedby.Text = dt.Rows[0]["FLDCDCCROSSCHECKEDBY"].ToString();
            //uccdccrosscheckeddate.Text = dt.Rows[0]["FLDCDCCROSSCHECKEDDAE"].ToString();

            if (dt.Rows[0]["FLDISATTACHMENT"].ToString() == string.Empty)
                imgCCClip.ImageUrl = Session["images"] + "/no-attachment.png";
            else
                imgCCClip.ImageUrl = Session["images"] + "/attachment.png";

            //if (txtSeamanBookNumber.Text != "")
            //{
            //    imgSeamanBook.Visible = true;
            //}
            //else
            //{
            //    imgSeamanBook.Visible = false;
            //}
            if (dt.Rows[0]["FLDSEAMANBOOKNO"].ToString() != string.Empty)
            {
                //txtUpdatedByCDC.Text = dt.Rows[0]["FLDCDCUPDATEDBY"].ToString();
                //ucUpdatedDateCDC.Text = dt.Rows[0]["FLDCDCUPDATEDDATE"].ToString();
            }
            else
            {
                //txtUpdatedByCDC.Text = "";
                //ucUpdatedDateCDC.Text = "";
            }
        }
    }

    protected void SetEmployeePassportDetailsBySeafarer()
    {
        DataTable dt = PhoenixCrewTravelDocument.ListEmployeeDetailConfirmPassport(Convert.ToInt32(ViewState["empid"].ToString()));

        if (dt.Rows.Count > 0)
        {
            txtPassportnumberemp.Text = dt.Rows[0]["FLDPASSPORTNO"].ToString();
            ucDateOfIssueemp.Text = dt.Rows[0]["FLDDATEOFISSUE"].ToString();
            txtPlaceOfIssueemp.Text = dt.Rows[0]["FLDPLACEOFISSUE"].ToString();
            ucDateOfExpiryemp.Text = dt.Rows[0]["FLDDATEOFEXPIRY"].ToString();
            ucECNRemp.SelectedHard = dt.Rows[0]["FLDECNRYESNO"].ToString();
            ucBlankPagesemp.SelectedHard = dt.Rows[0]["FLDMINIMUMPAGE"].ToString();
            ViewState["attachmentcodeemp"] = dt.Rows[0]["FLDDTKEY"].ToString();

            //chkpptVerifieddyn.Checked = dt.Rows[0]["FLDPPTVERIFIEDYN"].ToString() == "1" ? true : false;
            //chkpptVerifieddyn.Enabled = dt.Rows[0]["FLDPPTVERIFIEDYN"].ToString() == "1" ? false : true;
            //txtpptVerifiedby.Text = dt.Rows[0]["FLDPPTVERIFIEDBY"].ToString();
            //ucpptVerifieddate.Text = dt.Rows[0]["FLDPPTVERIFIEDDAE"].ToString();

            //chkcrosscheck.Checked = dt.Rows[0]["FLDPPTCROSSCHECKEDYN"].ToString() == "1" ? true : false;
            //chkcrosscheck.Enabled = dt.Rows[0]["FLDPPTCROSSCHECKEDYN"].ToString() == "1" ? false : true;
            //txtpptcrosscheckby.Text = dt.Rows[0]["FLDPPTCROSSCHECKEDBY"].ToString();
            //ucpptcrosscheckdate.Text = dt.Rows[0]["FLDPPTCROSSCHECKEDDAE"].ToString();

            if (dt.Rows[0]["FLDISATTACHMENT"].ToString() == string.Empty)
                imgPPClipemp.ImageUrl = Session["images"] + "/no-attachment.png";
            else
                imgPPClipemp.ImageUrl = Session["images"] + "/attachment.png";
            if (txtPassportnumberemp.Text != "")
            {
                //imgPassportArchiveemp.Visible = true;
            }
            else
            {
                //imgPassportArchiveemp.Visible = false;
            }

            if (dt.Rows[0]["FLDPASSPORTNO"].ToString() != string.Empty)
            {
                //txtUpdatedByPPT.Text = dt.Rows[0]["FLDPPTUPDATEDBY"].ToString();
                //ucUpdatedDatePPT.Text = dt.Rows[0]["FLDPPTUPDATEDDATE"].ToString();
            }
            else
            {
                //txtUpdatedByPPT.Text = "";
                //ucUpdatedDatePPT.Text = "";
            }
        }

    }

    protected void SetEmployeeSeamanBookDetailsBySeafarer()
    {
        DataTable dt = PhoenixCrewTravelDocument.ListEmployeeDetailConfirmSeamanBook(Convert.ToInt32(ViewState["empid"].ToString()));

        if (dt.Rows.Count > 0)
        {
            txtSeamanBookNumberemp.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
            ucSeamanDateOfIssueemp.Text = dt.Rows[0]["FLDSDATEOFISSUE"].ToString();
            txtSeamanPlaceOfIssueemp.Text = dt.Rows[0]["FLDSPLACEOFISSUE"].ToString();
            ucSeamanDateOfExpiryemp.Text = dt.Rows[0]["FLDSDATEOFEXPIRY"].ToString();
            ucSeamanCountryemp.SelectedFlag = dt.Rows[0]["FLDSEAMANFLAG"].ToString();
            ViewState["attachmentcodeemp"] = dt.Rows[0]["FLDDTKEY"].ToString();


            if (dt.Rows[0]["FLDISATTACHMENT"].ToString() == string.Empty)
                imgCCClipemp.ImageUrl = Session["images"] + "/no-attachment.png";
            else
                imgCCClipemp.ImageUrl = Session["images"] + "/attachment.png";

            //if (txtSeamanBookNumberemp.Text != "")
            //{
            //    imgSeamanBookemp.Visible = true;
            //}
            //else
            //{
            //    imgSeamanBookemp.Visible = false;
            // }
            if (dt.Rows[0]["FLDSEAMANBOOKNO"].ToString() != string.Empty)
            {
                //txtUpdatedByCDC.Text = dt.Rows[0]["FLDCDCUPDATEDBY"].ToString();
                //ucUpdatedDateCDC.Text = dt.Rows[0]["FLDCDCUPDATEDDATE"].ToString();
            }
            else
            {
                //txtUpdatedByCDC.Text = "";
                //ucUpdatedDateCDC.Text = "";
            }
        }
    }
}
