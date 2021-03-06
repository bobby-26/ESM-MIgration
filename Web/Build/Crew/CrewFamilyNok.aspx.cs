using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web;
using Telerik.Web.UI;
public partial class CrewFamilyNok : PhoenixBasePage
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
                if (Request.QueryString["familyid"] != "" && Request.QueryString["familyid"] != null)
                {
                    ViewState["EMPLOYEEFAMILYID"] = Request.QueryString["familyid"];
                }
                if (Request.QueryString["empid"] != "" && Request.QueryString["empid"] != null)
                {
                    Filter.CurrentCrewSelection = Request.QueryString["empid"];
                }

                SetEmployeePrimaryDetails();
                ucAddress.Country = "97";
                SetEmployeeFamilyDetails();
                
                Page.ClientScript.RegisterStartupScript(
                typeof(CrewFamilyNok),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);
            }

            RadTextBox txtBankAddress1 = (RadTextBox)(ucBankAddress.FindControl("txtAddressLine1"));
            UserControlCountry ddlCountry = (UserControlCountry)(ucBankAddress.FindControl("ddlCountry"));
            UserControlCity ddlcity = (UserControlCity)(ucBankAddress.FindControl("ddlCity"));
           
            txtBankAddress1.CssClass = "";
            ddlCountry.CssClass = "";
            ddlcity.CssClass = "";

            CreateTabs();

            CrewFamilyTabs.SelectedMenuIndex = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void CreateTabs()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        if (ViewState["EMPLOYEEFAMILYID"] != null)
        {
            toolbarmain.AddButton("Delete", "DELETE", ToolBarDirection.Right);
            toolbarmain.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.FAMILY + "&cmdname=FAMILYNOKUPLOAD'); return false;", "Attachment", "", "ATTACHMENT", ToolBarDirection.Right);

        }      
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
        CrewFamily.AccessRights = this.ViewState;
        CrewFamily.MenuList = toolbarmain.Show();

        toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("Family NOK", "FAMILYNOK");
        toolbarmain.AddButton("Sign On/Off", "SIGNON");
        toolbarmain.AddButton("Documents", "DOCUMENTS");
        toolbarmain.AddButton("Travel", "TRAVEL");
        CrewFamilyTabs.AccessRights = this.ViewState;
        CrewFamilyTabs.MenuList = toolbarmain.Show();
        CrewFamilyTabs.SelectedMenuIndex = 0;
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

    protected void CrewFamilyTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (ViewState["EMPLOYEEFAMILYID"] == null)
            {
                ucError.ErrorMessage = "Please select the Family Nok";
                ucError.Visible = true;
                return;
            }

            if (CommandName.ToUpper().Equals("SIGNON"))
            {
                Response.Redirect("CrewFamilySignon.aspx?familyid=" + ViewState["EMPLOYEEFAMILYID"], false);
            }
            else if (CommandName.ToUpper().Equals("MEDICAL"))
            {
                Response.Redirect("CrewFamilyMedicalDocument.aspx?familyid=" + ViewState["EMPLOYEEFAMILYID"], false);
            }
            else if (CommandName.ToUpper().Equals("DOCUMENTS"))
            {
                Response.Redirect("CrewFamilyTravelDocument.aspx?familyid=" + ViewState["EMPLOYEEFAMILYID"], false);
            }
            else if (CommandName.ToUpper().Equals("OTHERDOCUMENT"))
            {
                Response.Redirect("CrewFamilyOtherDocument.aspx?familyid=" + ViewState["EMPLOYEEFAMILYID"], false);
            }
            else if (CommandName.ToUpper().Equals("TRAVEL"))
            {
                Response.Redirect("CrewFamilyTravel.aspx?familyid=" + ViewState["EMPLOYEEFAMILYID"] + "&from=familynok", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
                }
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                ResetFormControlValues(this);
                SetEmployeePrimaryDetails();
                ViewState["EMPLOYEEFAMILYID"] = null;
                imgPhoto.ImageUrl = Session["images"] + "/Blank.png";
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
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
            }
            dt = PhoenixCrewFamilyNok.ListEmployeeFamily(Convert.ToInt32(Filter.CurrentCrewSelection), null);
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
            string strchkNOK = "0";

            if (chkNOK.Checked == true)
                strchkNOK = "1";

            DataTable family = PhoenixCrewFamilyNok.InsertEmployeeFamily(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , Convert.ToInt32(Filter.CurrentCrewSelection)
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
                                                                    , byte.Parse(strchkNOK)
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

            ViewState["EMPLOYEEFAMILYID"] = family.Rows[0][0].ToString();
            ViewState["attachmentcode"] = family.Rows[0][1].ToString();

            if (Request.Files.Count > 0)
            {
                foreach (UploadedFile postedFile in txtFileUpload.UploadedFiles)
                {
                    if (!Object.Equals(postedFile, null))
                    {
                        if (postedFile.ContentLength > 0)
                        {

                            if (ViewState["dtkey"] == null || ViewState["dtkey"].ToString() == string.Empty)
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

            DataTable dt = PhoenixCrewFamilyNok.ListEmployeeFamily(Convert.ToInt32(Filter.CurrentCrewSelection)
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

            DataTable dt = PhoenixCrewFamilyNok.ListEmployeeFamily(Convert.ToInt32(Filter.CurrentCrewSelection), General.GetNullableInteger(familyid));

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
                if (ViewState["EMPLOYEEFAMILYID"] != null)
                    lstFamily.SelectedValue = ViewState["EMPLOYEEFAMILYID"].ToString();

                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
                DataTable dta = PhoenixCommonFileAttachment.AttachmentList(new Guid(dt.Rows[0]["FLDDTKEY"].ToString()), "CREWIMAGE");
                ViewState["dtkey"] = string.Empty;
                if (dta.Rows.Count > 0)
                {
                    ViewState["dtkey"] = dta.Rows[0]["FLDDTKEY"].ToString();
                    imgPhoto.ImageUrl =  "../Common/Download.aspx?dtkey=" + dta.Rows[0]["FLDDTKEY"].ToString();
                    aCrewFamilyImg.HRef = "#";
                    aCrewFamilyImg.Attributes["onclick"] = "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/Download.aspx?dtkey=" + dta.Rows[0]["FLDDTKEY"].ToString() + "');";
                }
                else
                {
                    imgPhoto.ImageUrl = Session["images"] + "/Blank.png";
                }
                CreateTabs();
                if (ViewState["EMPLOYEEFAMILYID"] != null && Filter.CurrentCrewSelection != null)
                {
                    imgIDCard.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=CREWFAMILYIDCARD&familyid=" + ViewState["EMPLOYEEFAMILYID"].ToString() + "&employeeid=" + Filter.CurrentCrewSelection + "');return false;");
                    
                }
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
        try
        {
            if (Request.Files.Count > 0)
            //if (txtFileUpload.UploadedFiles.Count > 0)
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

            string strchkNOK = "0";

            if (chkNOK.Checked == true)
                strchkNOK = "1";

            PhoenixCrewFamilyNok.UpdateEmployeeFamily(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , Convert.ToInt32(Filter.CurrentCrewSelection)
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
                                                                    , byte.Parse(strchkNOK)
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

            ucStatus.Text = "Family Member information Updated.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        SetEmployeeFamilyDetails();
    }

    private bool IsValidate(string firstname, string sex, string relation, string address1,
                            string country, string email, string mobilenumber, string dob)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        Int16 resultint;
        DateTime resultdate;

        if (firstname.Trim() == "")
            ucError.ErrorMessage = "First Name is required";

        if (sex.Equals("") || !Int16.TryParse(sex, out resultint))
            ucError.ErrorMessage = "Sex  is required";

        if (relation.Equals("") || !Int16.TryParse(relation, out resultint))
            ucError.ErrorMessage = "Relationship  is required";

        if (address1.Trim() == "")
            ucError.ErrorMessage = "Address1 is required";

        if (country.Equals("") || !Int16.TryParse(country, out resultint))
            ucError.ErrorMessage = "Country  is required";
        if (ucAddress.City.Equals("") || !Int16.TryParse(ucAddress.City, out resultint))
            ucError.ErrorMessage = "City  is required";
        if (txtEmail.Text != string.Empty && !General.IsvalidEmail(txtEmail.Text))
        {
            ucError.ErrorMessage = "E-Mail is not valid";
        }

        if (!string.IsNullOrEmpty(ucDateOfBirth.Text) && DateTime.TryParse(ucDateOfBirth.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Date of Birth should be earlier than current date";
        }

        if (!string.IsNullOrEmpty(ucAnniversaryDate.Text) && DateTime.TryParse(ucAnniversaryDate.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Anniversary Date should be earlier than current date";
        }

        if (chkNOK.Checked == true)
        {
            if (mobilenumber == null || mobilenumber == "~")
            {
                ucError.ErrorMessage = "Mobile Number is required";
            }
            if (dob == null)
            {
                ucError.ErrorMessage = "Date of Birth is required";
            }

        }

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

        SetEmployeePrimaryDetails();

        SetEmployeeFamilyDetails();

        CreateTabs();
    }

    protected void AnniversaryDetails(object sender, EventArgs e)
    {
        if (ucRelation.SelectedQuick == "237")
        {
            ucAnniversaryDate.Enabled = true;
        }
        else
        {
            ucAnniversaryDate.Enabled = false;
        }
    }

    protected void CopyAddress_Click(object sender, EventArgs e)
    {
        RadPushButton lnk = sender as RadPushButton;
        int i = lnk.ID == "lnkCPA" ? 0 : 1;
        DataTable dt = PhoenixCrewAddress.ListEmployeeAddress(Convert.ToInt32(Filter.CurrentCrewSelection));
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
}
