using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewCommon;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using System.Web;
using Telerik.Web.UI;
public partial class CrewNewApplicantFamilyNok : PhoenixBasePage
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
            if (!IsPostBack)
            {
                confirm.Attributes.Add("style", "display:none");
                HookOnFocus(this.Page as Control);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ucSex.HardTypeCode = ((int)PhoenixHardTypeCode.SEX).ToString();
                ucRelation.QuickTypeCode = ((int)PhoenixQuickTypeCode.MISCELLANEOUSRELATION).ToString();
                SetEmployeePrimaryDetails();
                ucAddress.Country = "97";
                SetEmployeeFamilyDetails();
                RadTextBox txtBankAddress1 = (RadTextBox)(ucBankAddress.FindControl("txtAddressLine1"));
                UserControlCountry ddlCountry = (UserControlCountry)(ucBankAddress.FindControl("ddlCountry"));
                UserControlCity ddlcity = (UserControlCity)(ucBankAddress.FindControl("ddlCity"));
                txtBankAddress1.CssClass = "input";
                //ddlCountry.CssClass = "input";
                //ddlcity.CssClass = "input";
                Page.ClientScript.RegisterStartupScript(
                typeof(CrewNewApplicantFamilyNok),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);
            }
            CreateTabs();
            //SetAttachmentMarking();
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
    protected void CrewFamily_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidate(txtFamilyFirstName.Text, ucSex.SelectedHard, ucRelation.SelectedQuick
                                    , ucAddress.Address1, ucAddress.Country, txtEmail.Text, ucMobileNumber.Text, ucDateOfBirth.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if (!ucPhoneNumber.IsValidPhoneNumber())
                {
                    ucError.ErrorMessage = "Enter area code for phone number";
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["EMPLOYEEFAMILYID"] == null)
                {
                    SaveEmployeeFamilyDetails();
                    CreateTabs();
                }
                else
                {
                    UpdateEmployeeFamilyDetails();
                    ucStatus.Text = "Family Member Information Updated.";
                }
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                ResetFormControlValues(this);
                SetEmployeePrimaryDetails();
                ViewState["EMPLOYEEFAMILYID"] = null;
                imgPhoto.ImageUrl = Session["images"] + "/Blank.png";
                //lnkPassport.Text = "No Attachment";
                //lnkPassport.NavigateUrl = string.Empty;               
                CreateTabs();
            }
            else if (CommandName.ToUpper().Equals("DELETE"))
            {
                if (ViewState["EMPLOYEEFAMILYID"] == null)
                {
                    ucError.ErrorMessage = "Select Family Memember";
                    ucError.Visible = true;
                    return;
                }
                RadWindowManager1.RadConfirm("Are you sure want to delete?", "confirm", 320, 150, null, "Confirm");

            }
            else if (CommandName.ToUpper().Equals("MEDICAL"))
            {
                Response.Redirect("CrewNewApplicantFamilyMedicalDocument.aspx?familyid=" + ViewState["EMPLOYEEFAMILYID"], false);
            }
            else if (CommandName.ToUpper().Equals("DOCUMENTS"))
            {
                Response.Redirect("CrewNewApplicantFamilyTravelDocument.aspx?familyid=" + ViewState["EMPLOYEEFAMILYID"], false);
            }
            else if (CommandName.ToUpper().Equals("OTHERDOCUMENT"))
            {
                Response.Redirect("CrewNewApplicantFamilyOtherDocument.aspx?familyid=" + ViewState["EMPLOYEEFAMILYID"], false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixNewApplicantFamilyNok.DeleteEmployeeFamily(General.GetNullableInteger(ViewState["EMPLOYEEFAMILYID"].ToString()).Value);
            ViewState["EMPLOYEEFAMILYID"] = null;
            imgPhoto.ImageUrl = Session["images"] + "/Blank.png";
            ResetFormControlValues(this);
            SetEmployeePrimaryDetails();
            SetEmployeeFamilyDetails();
            CreateTabs();

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
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {

                txtEmployeeFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtEmployeeMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtEmployeeLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
            }
            dt = PhoenixNewApplicantFamilyNok.ListEmployeeFamily(Convert.ToInt32(Filter.CurrentNewApplicantSelection), null);
            lstFamily.Items.Clear();
            ListItem items = new ListItem();
            lstFamily.DataSource = dt;
            lstFamily.DataTextField = "FLDFIRSTNAME";
            lstFamily.DataValueField = "FLDFAMILYID";
            lstFamily.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SaveEmployeeFamilyDetails()
    {
        try
        {

            DataTable df = PhoenixNewApplicantFamilyNok.InsertEmployeeFamily(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                                                                    , txtFamilyFirstName.Text
                                                                    , txtFamilyMiddleName.Text
                                                                    , txtFamilyLastName.Text
                                                                    , Convert.ToInt32(ucSex.SelectedHard)
                                                                    , Convert.ToInt32(ucRelation.SelectedQuick)
                                                                    , General.GetNullableDateTime(ucDateOfBirth.Text)
                                                                    , ucAddress.Address1
                                                                    , ucAddress.Address2
                                                                    , ucAddress.Address3
                                                                    , ucAddress.Address4
                                                                    , ucAddress.City
                                                                    , General.GetNullableInteger(ucAddress.State)
                                                                    , Convert.ToInt32(ucAddress.Country)
                                                                    , ucAddress.PostalCode
                                                                    , General.GetNullableString(string.Empty)
                                                                    , ucPhoneNumber.Text
                                                                    , ucMobileNumber.Text
                                                                    , txtEmail.Text
                                                                    , General.GetNullableDateTime(ucAnniversaryDate.Text)
                                                                    , chkNOK.Checked ? byte.Parse("1") : byte.Parse("0")
                                                                    , txtBankName.Text
                                                                    , txtAccountNumber.Text
                                                                    , txtBranch.Text
                                                                    , ucBankAddress.Address1
                                                                    , ucBankAddress.Address2
                                                                    , ucBankAddress.Address3
                                                                    , ucBankAddress.Address4
                                                                    , General.GetNullableInteger(ucBankAddress.City)
                                                                    , General.GetNullableInteger(ucBankAddress.State)
                                                                    , General.GetNullableInteger(ucBankAddress.Country)
                                                                    , ucBankAddress.PostalCode
                                                                    , General.GetNullableInteger(ucNatioanlity.SelectedNationality)
                                                                    );

            ViewState["EMPLOYEEFAMILYID"] = df.Rows[0][0].ToString();
            ViewState["attachmentcode"] = df.Rows[0][1].ToString();
            if (Request.Files.Count > 0)
            {
                foreach (UploadedFile postedFile in txtFileUpload.UploadedFiles)
                {
                    if (!Object.Equals(postedFile, null))
                    {
                        if (postedFile.ContentLength > 0)
                        {
                            PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["attachmentcode"].ToString()), PhoenixModule.CREW, null, ".jpg,.png,.gif", string.Empty, "CREWIMAGE");
                        }

                    }
                }
            }
            DataTable dt = PhoenixNewApplicantFamilyNok.ListEmployeeFamily(Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                                                                    , General.GetNullableInteger(ViewState["EMPLOYEEFAMILYID"].ToString()));
            lstFamily.DataSource = dt;
            lstFamily.DataTextField = "FLDFIRSTNAME";
            lstFamily.DataValueField = "FLDFAMILYID";
            lstFamily.DataBind();
            SetEmployeeFamilyDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private void SetEmployeeFamilyDetails()
    {
        try
        {
            string familyid = null;
            if (ViewState["EMPLOYEEFAMILYID"] != null)
            {
                familyid = ViewState["EMPLOYEEFAMILYID"].ToString();
            }

            DataTable dt = PhoenixNewApplicantFamilyNok.ListEmployeeFamily(Convert.ToInt32(Filter.CurrentNewApplicantSelection), General.GetNullableInteger(familyid));

            if (dt.Rows.Count > 0)
            {
                txtFamilyFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtFamilyMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtFamilyLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                ucSex.SelectedHard = dt.Rows[0]["FLDSEX"].ToString();
                ucNatioanlity.SelectedNationality = dt.Rows[0]["FLDNATIONALITY"].ToString();
                ucRelation.SelectedQuick = dt.Rows[0]["FLDRELATIONSHIP"].ToString();
                ucDateOfBirth.Text = dt.Rows[0]["FLDDATEOFBIRTH"].ToString();
                ucAddress.Address1 = dt.Rows[0]["FLDADDRESS1"].ToString();
                ucAddress.Address2 = dt.Rows[0]["FLDADDRESS2"].ToString();
                ucAddress.Address3 = dt.Rows[0]["FLDADDRESS3"].ToString();
                ucAddress.Address4 = dt.Rows[0]["FLDADDRESS4"].ToString();
                ucAddress.Country = dt.Rows[0]["FLDCOUNTRY"].ToString();
                ucAddress.State = dt.Rows[0]["FLDSTATE"].ToString();
                ucAddress.City = dt.Rows[0]["FLDCITY"].ToString();
                ucAddress.PostalCode = dt.Rows[0]["FLDPOSTALCODE"].ToString();
                //txtSTDCode.Text = dt.Rows[0]["FLDSTDCODE"].ToString();
                ucPhoneNumber.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                ucPhoneNumber.Text = dt.Rows[0]["FLDPHONENUMBER"].ToString();
                txtEmail.Text = dt.Rows[0]["FLDEMAIL"].ToString();
                //ucMobileNumber.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                ucMobileNumber.Text = dt.Rows[0]["FLDMOBILENUMBER"].ToString();
                ucAnniversaryDate.Text = dt.Rows[0]["FLDANNIVERSARYDATE"].ToString();
                ViewState["EMPLOYEEFAMILYID"] = dt.Rows[0]["FLDFAMILYID"].ToString();
                chkNOK.Checked = dt.Rows[0]["FLDNOK"].ToString() == "1" ? true : false;
                txtBankName.Text = dt.Rows[0]["FLDBANKNAME"].ToString();
                txtAccountNumber.Text = dt.Rows[0]["FLDACCOUNTNUMBER"].ToString();
                txtBranch.Text = dt.Rows[0]["FLDBRANCH"].ToString();
                ucBankAddress.Address1 = dt.Rows[0]["FLDBANKADDRESS1"].ToString();
                ucBankAddress.Address2 = dt.Rows[0]["FLDBANKADDRESS2"].ToString();
                ucBankAddress.Address3 = dt.Rows[0]["FLDBANKADDRESS3"].ToString();
                ucBankAddress.Address4 = dt.Rows[0]["FLDBANKADDRESS4"].ToString();
                ucBankAddress.Country = dt.Rows[0]["FLDBANKCOUNTRY"].ToString();
                ucBankAddress.State = dt.Rows[0]["FLDBANKSTATE"].ToString();
                ucBankAddress.City = dt.Rows[0]["FLDBANKCITY"].ToString();
                ucBankAddress.PostalCode = dt.Rows[0]["FLDBANKPOSTALCODE"].ToString();

                lstFamily.SelectedValue = ViewState["EMPLOYEEFAMILYID"].ToString();

                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
                DataTable dta = PhoenixCommonFileAttachment.AttachmentList(new Guid(dt.Rows[0]["FLDDTKEY"].ToString()), "CREWIMAGE");
                ViewState["dtkey"] = string.Empty;
                if (dta.Rows.Count > 0)
                {
                    ViewState["dtkey"] = dta.Rows[0]["FLDDTKEY"].ToString();
                    imgPhoto.ImageUrl = "../Common/Download.aspx?dtkey=" + dta.Rows[0]["FLDDTKEY"].ToString();
                    aCrewFamilyImg.HRef = "#";
                    aCrewFamilyImg.Attributes["onclick"] = "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/Download.aspx?dtkey=" + dta.Rows[0]["FLDDTKEY"].ToString() + "');";
                }
                else
                {
                    imgPhoto.ImageUrl = Session["images"] + "/Blank.png";
                }
                CreateTabs();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UpdateEmployeeFamilyDetails()
    {

        if (Request.Files.Count > 0)
        {
            foreach (UploadedFile postedFile in txtFileUpload.UploadedFiles)
            {
                if (!Object.Equals(postedFile, null))
                {
                    if (postedFile.ContentLength > 0)
                    {
                        if (string.IsNullOrEmpty(ViewState["dtkey"].ToString()))
                        {
                            PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["attachmentcode"].ToString()), PhoenixModule.CREW, null, ".jpg,.png,.gif", string.Empty, "CREWIMAGE");
                        }
                        else
                        {
                            PhoenixCommonFileAttachment.UpdateAttachment(Request.Files, new Guid(ViewState["dtkey"].ToString()), PhoenixModule.CREW, ".jpg,.png,.gif");
                        }
                    }
                }
            }
        }
        PhoenixNewApplicantFamilyNok.UpdateEmployeeFamily(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                                                                , Convert.ToInt32(ViewState["EMPLOYEEFAMILYID"].ToString())
                                                                , txtFamilyFirstName.Text
                                                                , txtFamilyMiddleName.Text
                                                                , txtFamilyLastName.Text
                                                                , Convert.ToInt32(ucSex.SelectedHard)
                                                                , Convert.ToInt32(ucRelation.SelectedQuick)
                                                                , General.GetNullableDateTime(ucDateOfBirth.Text)
                                                                , ucAddress.Address1
                                                                , ucAddress.Address2
                                                                , ucAddress.Address3
                                                                , ucAddress.Address4
                                                                , ucAddress.City
                                                                , General.GetNullableInteger(ucAddress.State)
                                                                , Convert.ToInt32(ucAddress.Country)
                                                                , ucAddress.PostalCode
                                                                , General.GetNullableString(string.Empty)
                                                                , ucPhoneNumber.Text
                                                                , ucMobileNumber.Text
                                                                , txtEmail.Text
                                                                , General.GetNullableDateTime(ucAnniversaryDate.Text)
                                                                , chkNOK.Checked ? byte.Parse("1") : byte.Parse("0")
                                                                , txtBankName.Text
                                                                , txtAccountNumber.Text
                                                                , txtBranch.Text
                                                                , ucBankAddress.Address1
                                                                , ucBankAddress.Address2
                                                                , ucBankAddress.Address3
                                                                , ucBankAddress.Address4
                                                                , General.GetNullableInteger(ucBankAddress.City)
                                                                , General.GetNullableInteger(ucBankAddress.State)
                                                                , General.GetNullableInteger(ucBankAddress.Country)
                                                                , ucBankAddress.PostalCode
                                                                , General.GetNullableInteger(ucNatioanlity.SelectedNationality)
                                                                );
        SetEmployeeFamilyDetails();
    }

    private bool IsValidate(string firstname, string sex, string relation, string address1,
                            string country, string email, string mobilenumber, string dob)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        Int16 resultint;
        DateTime resultdate;
        //if (ecnr.Equals("Dummy")) ecnr = string.Empty;
        //if (minimumoage.Equals("Dummy")) minimumoage = string.Empty;
        if (firstname.Trim() == "")
            ucError.ErrorMessage = "First Name is required";

        if (sex.Equals("") || !Int16.TryParse(sex, out resultint))
            ucError.ErrorMessage = "Gender  is required";

        if (relation.Equals("") || !Int16.TryParse(relation, out resultint))
            ucError.ErrorMessage = "Relationship  is required";
        if (ucAddress.City.Equals("") || !Int16.TryParse(ucAddress.City, out resultint))
            ucError.ErrorMessage = "City  is required";
        if (address1.Trim() == "")
            ucError.ErrorMessage = "Address1 is required";

        if (country.Equals("") || !Int16.TryParse(country, out resultint))
            ucError.ErrorMessage = "Country  is required";

        if (txtEmail.Text != string.Empty && !General.IsvalidEmail(txtEmail.Text))
        {
            ucError.ErrorMessage = "E-Mail is not valid";
        }

        if (!string.IsNullOrEmpty(ucDateOfBirth.Text) && DateTime.TryParse(ucDateOfBirth.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Date of Birth should be earlier than current date";
        }
        if (!string.IsNullOrEmpty(ucAnniversaryDate.Text) && !DateTime.TryParse(ucAnniversaryDate.Text, out resultdate))
            ucError.ErrorMessage = "Anniversary  is required.";
        if (DateTime.TryParse(ucAnniversaryDate.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Anniversary should be earlier than current date";
        }
        if (chkNOK.Checked == true)
        {
            if (mobilenumber == null || mobilenumber == "~")
            {
                ucError.ErrorMessage = "Mobile Number is required";
            }
            if (dob == null)
            {
                ucError.ErrorMessage = "D.O.B. is required";
            }
        }

        //if (passportnumber.Trim() != string.Empty)
        //{
        //    //ucError.ErrorMessage = "Passport Number is required";

        //    if (dateofissue == null || !DateTime.TryParse(dateofissue, out resultdate))
        //        ucError.ErrorMessage = "Date of Issue is required";
        //    else if (DateTime.TryParse(dateofissue, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        //    {
        //        ucError.ErrorMessage = "Date of Issue should be earlier than current date";
        //    }

        //    if (dateofexpiry == null || !DateTime.TryParse(dateofexpiry, out resultdate))
        //        ucError.ErrorMessage = "Date of Expiry is required";
        //    else if (!string.IsNullOrEmpty(dateofissue)
        //        && DateTime.TryParse(dateofexpiry, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(dateofissue)) < 0)
        //    {
        //        ucError.ErrorMessage = "Date of Expiry should be later than 'Date of Issue'";
        //    }

        //    if (placeofissue.Trim() == "")
        //        ucError.ErrorMessage = "Place of Issue is required";

        //    if (minimumoage.Equals("") || !Int16.TryParse(minimumoage, out resultint))
        //        ucError.ErrorMessage = "Minimum 4 Blank pages  is required";

        //    if (ecnr.Equals("") || !Int16.TryParse(ecnr, out resultint))
        //        ucError.ErrorMessage = "ECNR  is required";
        //}
        //else if ((DateTime.TryParse(dateofissue, out resultdate) || DateTime.TryParse(dateofexpiry, out resultdate) || !string.IsNullOrEmpty(placeofissue) || !minimumoage.Equals("") || !ecnr.Equals(""))
        //    && passportnumber.Trim() == string.Empty)
        //{
        //    ucError.ErrorMessage = "Passport Number is required";
        //}
        return (!ucError.IsError);
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
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
                    if (c is RadTextBox)
                    {
                        ((RadTextBox)c).Text = "";
                    }
                    else if (c is RadCheckBox)
                    {
                        ((RadCheckBox)c).Checked = false;
                    }
                    else if (c is RadRadioButton)
                    {
                        ((RadRadioButton)c).Checked = false;
                    }
                    else if (c is RadDropDownList)
                    {
                        ((RadDropDownList)c).SelectedIndex = -1;
                        ((RadDropDownList)c).ClearSelection();
                        ((RadDropDownList)c).SelectedValue = "";
                        ((RadDropDownList)c).SelectedText = "";
                    }
                    else if (c is RadComboBox)
                    {
                        ((RadComboBox)c).SelectedIndex = -1;
                        ((RadComboBox)c).ClearSelection();
                        ((RadComboBox)c).Text = string.Empty;
                        ((RadComboBox)c).SelectedValue = "";

                    }
                    else if (c is RadListBox)
                    {
                        ((RadListBox)c).SelectedIndex = -1;
                    }
                }


            }
            ucDateOfBirth.Text = "";
            ucNatioanlity.SelectedNationality = "";
            ucRelation.SelectedQuick = "";
            ucSex.SelectedHard = "";
            ucAddress.State = "";
            ucAddress.City = "";
            ucAddress.Country = "97";
            ucBankAddress.State = "";
            ucBankAddress.City = "";
            ucBankAddress.Country = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void lstFamily_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["EMPLOYEEFAMILYID"] = lstFamily.SelectedValue.ToString();
        ResetFormControlValues(this);
        SetEmployeeFamilyDetails();
    }

    //private void SetAttachmentMarking()
    //{
    //    if (ViewState["attachmentcode"] != null && ViewState["attachmentcode"].ToString() != string.Empty)
    //    {
    //        DataTable dt1 = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["attachmentcode"].ToString()), PhoenixCrewAttachmentType.FAMILY.ToString());
    //        if (dt1.Rows.Count > 0)
    //        {
    //            imgClip.Visible = true;
    //            imgClip.Attributes["onclick"] = "javascript:parent.Openpopup('NAA','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
    //                   + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.FAMILY + "'); return false;";
    //        }
    //        else
    //            imgClip.Visible = false;
    //    }
    //    else
    //        imgClip.Visible = false;
    //}
    private void CreateTabs()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();


        if (ViewState["EMPLOYEEFAMILYID"] != null)
        {
            toolbarmain.AddButton("Othr.Document", "OTHERDOCUMENT", ToolBarDirection.Right);
            toolbarmain.AddButton("Trvl Document", "DOCUMENTS", ToolBarDirection.Right);
            toolbarmain.AddButton("Medical", "MEDICAL", ToolBarDirection.Right);
            toolbarmain.AddButton("Delete", "DELETE", ToolBarDirection.Right);

            toolbarmain.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.FAMILY + "&cmdname=FAMILYNOKUPLOAD'); return false;", "Attachment", "", "ATTACHMENT", ToolBarDirection.Right);
        }
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
        CrewFamily.AccessRights = this.ViewState;
        CrewFamily.MenuList = toolbarmain.Show();
    }

    protected void CopyAddress_Click(object sender, EventArgs e)
    {
        RadPushButton lnk = sender as RadPushButton;
        int i = lnk.ID == "lnkCPA" ? 0 : 1;
        DataTable dt = PhoenixNewApplicantManagement.ListEmployeeAddress(Convert.ToInt32(Filter.CurrentNewApplicantSelection));
        if (dt.Rows.Count > 0)
        {
            ucAddress.Address1 = dt.Rows[i]["FLDADDRESS1"].ToString();
            ucAddress.Address2 = dt.Rows[i]["FLDADDRESS2"].ToString();
            ucAddress.Address3 = dt.Rows[i]["FLDADDRESS3"].ToString();
            ucAddress.Address4 = dt.Rows[i]["FLDADDRESS4"].ToString();
            ucAddress.Country = dt.Rows[i]["FLDCOUNTRY"].ToString();
            ucAddress.State = dt.Rows[i]["FLDSTATE"].ToString();
            ucAddress.City = dt.Rows[i]["FLDCITY"].ToString();
            ucAddress.PostalCode = dt.Rows[i]["FLDPOSTALCODE"].ToString();
        }

    }
    protected void AnniversaryDetails(object sender, EventArgs e)
    {
        if (ucRelation.SelectedQuick == "237")
        {
            ucAnniversaryDate.CssClass = "input";
            ucAnniversaryDate.Enabled = true;
        }
        else
        {
            ucAnniversaryDate.CssClass = "readonlytextbox";
            ucAnniversaryDate.Enabled = false;

        }
    }

}
