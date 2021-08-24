using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreEmployeeDocumentsAuthentication : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Authenticate", "SAVE",ToolBarDirection.Right);
            AuthenticationMainMenu.AccessRights = this.ViewState;
            AuthenticationMainMenu.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
               
                ViewState["DocType"] = Request.QueryString["DocType"] != null ? Request.QueryString["DocType"] : "";
                ViewState["EmployeeId"] = Request.QueryString["EmployeeId"] != null ? Request.QueryString["EmployeeId"] : "";
                ViewState["DocumentId"] = Request.QueryString["DocumentId"] != null ? Request.QueryString["DocumentId"] : "";
                ViewState["DtKey"] = Request.QueryString["DtKey"] != null ? Request.QueryString["DtKey"] : "";
                ViewState["LOCKYN"] = Request.QueryString["LOCKYN"] != null ? Request.QueryString["LOCKYN"] : "";
                BindPMUDoctor(ddlDoctorM);
                BindPMUDoctor(ddlDoctor);                
                PopulateDetails(ViewState["DocType"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void PopulateDetails(string DocType)
    {
        if (DocType.Trim().ToUpper() == "TRAVEL")
        {
            string mimetype = ".pdf";
            if (ViewState["LOCKYN"].ToString().Equals("1"))
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?cmdname=TRAVELDOCUPLOAD&DTKEY="
                                               + ViewState["DtKey"].ToString() + "&MOD=" + PhoenixModule.CREW
                                               + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS
                                               + "&mimetype=" + mimetype + "&maxnoofattachments=2&U=1";
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?cmdname=TRAVELDOCUPLOAD&DTKEY="
                                + ViewState["DtKey"].ToString() + "&MOD=" + PhoenixModule.CREW
                                + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS
                                + "&mimetype=" + mimetype + "&maxnoofattachments=2";
            }
            dvTravel.Visible = true;
            dvLicence.Visible = dvMedicalTest.Visible = dvCourse.Visible = dvOther.Visible = false;
            GetEmployeeTravelDetails();
            AuthenticationMainMenu.Title = "Travel Authentication";
        }
        else if (DocType.Trim().ToUpper() == "LICENCE")
        {
            string mimetype = ".pdf";
            if (ViewState["LOCKYN"].ToString().Equals("1"))
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?cmdname=LICENCEUPLOAD&DTKEY="
                                + ViewState["DtKey"].ToString() + "&MOD=" + PhoenixModule.CREW
                                + "&type=" + PhoenixCrewAttachmentType.LICENCE
                                + "&mimetype=" + mimetype + "&maxnoofattachments=2&U=1";
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?cmdname=LICENCEUPLOAD&DTKEY="
                                + ViewState["DtKey"].ToString() + "&MOD=" + PhoenixModule.CREW
                                + "&type=" + PhoenixCrewAttachmentType.LICENCE
                                + "&mimetype=" + mimetype + "&maxnoofattachments=2";
            }
            dvLicence.Visible = true;
            dvTravel.Visible = dvMedicalTest.Visible = dvCourse.Visible = dvOther.Visible = false;
            GetEmployeeLicenceDetails();
            AuthenticationMainMenu.Title = "Licence Authentication";
        }
        else if (DocType.Trim().ToUpper() == "MEDICAL")
        {
            string mimetype = ".pdf";
            if (ViewState["LOCKYN"].ToString().Equals("1"))
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?cmdname=MEDICALUPLOAD&DTKEY="
                                + ViewState["DtKey"].ToString() + "&MOD=" + PhoenixModule.CREW
                                + "&type=" + PhoenixCrewAttachmentType.MEDICAL
                                + "&mimetype=" + mimetype + "&maxnoofattachments=2&U=1";
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?cmdname=MEDICALUPLOAD&DTKEY="
                                + ViewState["DtKey"].ToString() + "&MOD=" + PhoenixModule.CREW
                                + "&type=" + PhoenixCrewAttachmentType.MEDICAL
                                + "&mimetype=" + mimetype + "&maxnoofattachments=2";
            }
            dvMedical.Visible = true;
            dvTravel.Visible = dvLicence.Visible = dvCourse.Visible = dvOther.Visible = dvMedicalTest.Visible = false;
            GetEmployeeMedicalDetails();
            AuthenticationMainMenu.Title = "Medical Authentication";
        }
        else if (DocType.Trim().ToUpper() == "MEDICALTEST")
        {
            string mimetype = ".pdf";
            if (ViewState["LOCKYN"].ToString().Equals("1"))
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?cmdname=MEDICALTESTUPLOAD&DTKEY="
                                + ViewState["DtKey"].ToString() + "&MOD=" + PhoenixModule.CREW
                                + "&type=" + PhoenixCrewAttachmentType.MEDICAL
                                + "&mimetype=" + mimetype + "&maxnoofattachments=2&U=1";
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?cmdname=MEDICALTESTUPLOAD&DTKEY="
                                + ViewState["DtKey"].ToString() + "&MOD=" + PhoenixModule.CREW
                                + "&type=" + PhoenixCrewAttachmentType.MEDICAL
                                + "&mimetype=" + mimetype + "&maxnoofattachments=2";
            }
            dvMedicalTest.Visible = true;
            dvTravel.Visible = dvLicence.Visible = dvCourse.Visible = dvOther.Visible = false;
            GetEmployeeMedicalTestDetails();
            AuthenticationMainMenu.Title = "Medical Test Authentication";
        }
        else if (DocType.Trim().ToUpper() == "COURSE")
        {
            string mimetype = ".pdf";
            if (ViewState["LOCKYN"].ToString().Equals("1"))
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?cmdname=COURSEUPLOAD&DTKEY="
                                + ViewState["DtKey"].ToString() + "&MOD=" + PhoenixModule.CREW
                                + "&type=" + PhoenixCrewAttachmentType.COURSE
                                + "&mimetype=" + mimetype + "&maxnoofattachments=2&U=1";
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?cmdname=COURSEUPLOAD&DTKEY="
                                + ViewState["DtKey"].ToString() + "&MOD=" + PhoenixModule.CREW
                                + "&type=" + PhoenixCrewAttachmentType.COURSE
                                + "&mimetype=" + mimetype + "&maxnoofattachments=2";
            }
            dvCourse.Visible = true;
            dvTravel.Visible = dvLicence.Visible = dvMedicalTest.Visible = dvOther.Visible = false;
            GetEmployeeCourseDetails();
            AuthenticationMainMenu.Title = "Course Authentication";
        }
        else if (DocType.Trim().ToUpper() == "OTHER")
        {
            string mimetype = ".pdf";
            if (ViewState["LOCKYN"].ToString().Equals("1"))
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?cmdname=OTHERDOCUPLOAD&DTKEY="
                                + ViewState["DtKey"].ToString() + "&MOD=" + PhoenixModule.CREW
                                + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS
                                + "&mimetype=" + mimetype + "&maxnoofattachments=2&U=1";
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?cmdname=OTHERDOCUPLOAD&DTKEY="
                                + ViewState["DtKey"].ToString() + "&MOD=" + PhoenixModule.CREW
                                + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS
                                + "&mimetype=" + mimetype + "&maxnoofattachments=2";
            }
            dvOther.Visible = true;
            dvTravel.Visible = dvLicence.Visible = dvMedicalTest.Visible = dvCourse.Visible = false;
            GetEmployeeOtherDocumentDetails();
            AuthenticationMainMenu.Title = "Other Document Authentication";
        }
    }

    private void GetEmployeeTravelDetails()
    {
        DataTable dt = PhoenixCrewDocumentsAuthenticationandCrossCheck.EmployeeTravelDocumentEdit(General.GetNullableInteger(ViewState["EmployeeId"].ToString())
                                                                                              , General.GetNullableInteger(ViewState["DocumentId"].ToString())
                                                                                              );
        if (dt.Rows.Count > 0)
        {
            DataRow dr=dt.Rows[0];
            txtDocumentName.Text = dr["FLDDOCUMENTNAME"].ToString();
            txtTravelDocNumber.Text = dr["FLDDOCUMENTNUMBER"].ToString();
            ucDateOfIssue.Text = dr["FLDDATEOFISSUE"].ToString();
            txtNationality.Text = dr["FLDCOUNTRYNAME"].ToString();
            ucValidFromDate.Text = dr["FLDVALIDFROM"].ToString();
            ucDateofExpiry.Text = dr["FLDDATEOFEXPIRY"].ToString();
            txtPlaceofIsue.Text = dr["FLDPLACEOFISSUE"].ToString();
            txtPassportNo.Text = dr["FLDPASSPORTNO"].ToString();
            ucAuthenticationDate.Text = dr["FLDAUTHENTICATEDON"].ToString();
            txtCategory.Text = dr["FLDCATEGORYNAME"].ToString();
            txtNoofEntries.Text = dr["FLDNOOFENTRYNAME"].ToString();
            txtContactVessel.Text = dr["FLDISCONNECTEDTOVESSEL"].ToString();
            txtDocStatus.Text = dr["FLDDOCSTATUS"].ToString();
            txtAuthenticateBy.Text = dr["FLDAUTHENTICATEDBYNAME"].ToString();
            ViewState["DtKey"] = dr["FLDDTKEY"].ToString();
        }

    }

    private void GetEmployeeLicenceDetails()
    {
        DataTable dt = PhoenixCrewDocumentsAuthenticationandCrossCheck.EmployeeLicenceEdit(General.GetNullableInteger(ViewState["EmployeeId"].ToString())
                                                                                              , General.GetNullableInteger(ViewState["DocumentId"].ToString())
                                                                                              );
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtLicence.Text = dr["FLDLICENCE"].ToString();
            txtLicenceNumber.Text = dr["FLDLICENCENUMBER"].ToString();
            ucLicenceDateofIssue.Text = dr["FLDDATEOFISSUE"].ToString();
            txtLicenceNationality.Text = dr["FLDFLAGNAME"].ToString();
            txtLicenceCategory.Text = dr["FLDCATEGORYNAME"].ToString();
            ucLicenceExpiryDate.Text = dr["FLDDATEOFEXPIRY"].ToString();
            txtLicencePlaceofIssue.Text = dr["FLDPLACEOFISSUE"].ToString();
            txtLicenceIssueAuthority.Text = dr["FLDISSUEDBY"].ToString();
            ucLicenceAuthenticationDate.Text = dr["FLDAUTHENTICATEDON"].ToString();
            txtLicenceType.Text = dr["FLDDOCUMENTTYPENAME"].ToString();
            txtLicenceAuthenticateBy.Text = dr["FLDAUTHENTICATEDBYNMAE"].ToString();
            ViewState["DtKey"] = dr["FLDDTKEY"].ToString();
        }
    }

    private void GetEmployeeMedicalTestDetails()
    {
        DataTable dt = PhoenixCrewDocumentsAuthenticationandCrossCheck.EmployeeMedicalTestEdit(General.GetNullableInteger(ViewState["EmployeeId"].ToString())
                                                                                              , General.GetNullableInteger(ViewState["DocumentId"].ToString())
                                                                                              );
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtTestName.Text = dr["FLDNAMEOFMEDICAL"].ToString();
            txtTestStatus.Text = dr["FLDSTATUSNAME"].ToString();
            ucTestIssueDate.Text = dr["FLDISSUEDDATE"].ToString();
            txtTestPlaceofIssue.Text = dr["FLDPLACEOFISSUE"].ToString();
            ucTestExpiryDate.Text = dr["FLDEXPIRYDATE"].ToString();
            ucTestAuthenticationDate.Text = dr["FLDAUTHENTICATEDON"].ToString();
            ddlDoctor.SelectedValue = dr["FLDPMUDOCTORID"].ToString();
            txtTestAuthenticateBy.Text = dr["FLDAUTHENTICATEDBYNAME"].ToString();
            ViewState["DtKey"] = dr["FLDDTKEY"].ToString();
        }
    }

    private void GetEmployeeMedicalDetails()
    {
        DataTable dt = PhoenixCrewDocumentsAuthenticationandCrossCheck.EmployeeMedicalEdit(General.GetNullableInteger(ViewState["EmployeeId"].ToString())
                                                                                              , General.GetNullableInteger(ViewState["DocumentId"].ToString())
                                                                                              );
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtMedical.Text = dr["FLDMEDICALNAME"].ToString();
            txtFlag.Text = dr["FLDFLAGNAME"].ToString();
            txtIssueDateM.Text = dr["FLDISSUEDDATE"].ToString();
            txtPlaceofIssueM.Text = dr["FLDPLACEOFISSUE"].ToString();
            txtExpiryDateM.Text = dr["FLDEXPIRYDATE"].ToString();
            txtStatusM.Text = dr["FLDSTATUSNAME"].ToString();
            txtAuthenticatedDateM.Text = dr["FLDAUTHENTICATEDON"].ToString();
            ddlDoctorM.SelectedValue = dr["FLDPMUDOCTORID"].ToString();
            txtAuthenticatedByM.Text = dr["FLDAUTHENTICATEDBY"].ToString();
            ViewState["DtKey"] = dr["FLDDTKEY"].ToString();
        }
    }

    private void GetEmployeeCourseDetails()
    {
        DataTable dt = PhoenixCrewDocumentsAuthenticationandCrossCheck.EmployeeCourseDocumentEdit(General.GetNullableInteger(ViewState["EmployeeId"].ToString())
                                                                                              , General.GetNullableInteger(ViewState["DocumentId"].ToString())
                                                                                              );
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtCourse.Text = dr["FLDCOURSENAME"].ToString();
            txtCertificateNumber.Text = dr["FLDCOURSENUMBER"].ToString();
            ucCourseIssueDate.Text = dr["FLDDATEOFISSUE"].ToString();
            txtCourseNationality.Text = dr["FLDNATIONALITY"].ToString();
            txtCourseCategory.Text = dr["FLDCATEGORYNAME"].ToString();
            ucCourseExpiryDate.Text = dr["FLDDATEOFEXPIRY"].ToString();
            txtCoursePlaceofIssue.Text = dr["FLDPLACEOFISSUE"].ToString();
            txtCourseIssueAuthority.Text = dr["FLDAUTHORITY"].ToString();
            txtCourseType.Text = dr["FLDHARDNAME"].ToString();
            txtCourseInstitute.Text = dr["FLDNAME"].ToString();
            ucCourseAuthenticationDate.Text = dr["FLDAUTHENTICATEDON"].ToString();
            txtCourseAuthenticateBy.Text = dr["FLDAUTHENTICATEDBYNAME"].ToString();
            ViewState["DtKey"] = dr["FLDDTKEY"].ToString();
        }
    }

    private void GetEmployeeOtherDocumentDetails()
    {
        DataTable dt = PhoenixCrewDocumentsAuthenticationandCrossCheck.EmployeeOtherDocumentEdit(General.GetNullableInteger(ViewState["EmployeeId"].ToString())
                                                                                              , General.GetNullableInteger(ViewState["DocumentId"].ToString())
                                                                                              );
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtOtherDocName.Text = dr["FLDDOCUMENTNAME"].ToString();
            txtOtherDocNumber.Text = dr["FLDDOCUMENTNUMBER"].ToString();
            ucOtherDocDateofIssue.Text = dr["FLDDATEOFISSUE"].ToString();
            txtOtherDocPlaceofIssue.Text = dr["FLDPLACEOFISSUE"].ToString();
            txtOtherDocIssuAuthority.Text = dr["FLDISSUINGAUTHORITY"].ToString();
            ucOtherDocExpiryDate.Text = dr["FLDDATEOFEXPIRY"].ToString();
            txtOtherDocCategory.Text = dr["FLDCATEGORYNAME"].ToString();
            ucOtherDocAuthenticationDate.Text = dr["FLDAUTHENTICATEDON"].ToString();
            txtOtherAuthenticateBy.Text = dr["FLDAUTHENTICATEDBYNAME"].ToString();
            ViewState["DtKey"] = dr["FLDDTKEY"].ToString();
        }
    }

    protected void AuthenticationMainMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.Trim().ToUpper().Equals("SAVE"))
            {
                if (ViewState["DocType"].ToString().Trim().Equals("TRAVEL"))
                {
                    PhoenixCrewDocumentsAuthenticationandCrossCheck.UpdateEmployeeTravelDocumentAuthentication(int.Parse(ViewState["EmployeeId"].ToString())
                        , General.GetNullableInteger(ViewState["DocumentId"].ToString())
                        , General.GetNullableDateTime(ucAuthenticationDate.Text)
                        , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        );
                    
                }
                else if (ViewState["DocType"].ToString().Trim().Equals("LICENCE"))
                {
                    PhoenixCrewDocumentsAuthenticationandCrossCheck.UpdateEmployeeLicenceAuthentication(int.Parse(ViewState["EmployeeId"].ToString())
                       , General.GetNullableInteger(ViewState["DocumentId"].ToString())
                       , General.GetNullableDateTime(ucLicenceAuthenticationDate.Text)
                       , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                       );
                }
                else if (ViewState["DocType"].ToString().Trim().Equals("MEDICAL"))
                {
                    PhoenixCrewDocumentsAuthenticationandCrossCheck.UpdateEmployeeMedicalAuthentication(int.Parse(ViewState["DocumentId"].ToString())
                       , General.GetNullableDateTime(txtAuthenticatedDateM.Text)
                       , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                       , General.GetNullableInteger(ddlDoctorM.SelectedValue)
                       );
                }
                else if (ViewState["DocType"].ToString().Trim().Equals("MEDICALTEST"))
                {
                    PhoenixCrewDocumentsAuthenticationandCrossCheck.UpdateEmployeeMedicalTestAuthentication(int.Parse(ViewState["EmployeeId"].ToString())
                       , General.GetNullableInteger(ViewState["DocumentId"].ToString())
                       , General.GetNullableDateTime(ucTestAuthenticationDate.Text)
                       , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                       ,General.GetNullableInteger(ddlDoctor.SelectedValue)
                       );
                }
                else if (ViewState["DocType"].ToString().Trim().Equals("COURSE"))
                {
                    PhoenixCrewDocumentsAuthenticationandCrossCheck.UpdateEmployeeCourseAuthentication(int.Parse(ViewState["EmployeeId"].ToString())
                      , General.GetNullableInteger(ViewState["DocumentId"].ToString())
                      , General.GetNullableDateTime(ucCourseAuthenticationDate.Text)
                      , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                      );
                }
                else if (ViewState["DocType"].ToString().Trim().Equals("OTHER"))
                {
                    PhoenixCrewDocumentsAuthenticationandCrossCheck.UpdateEmployeeOtherDocumentAuthentication(int.Parse(ViewState["EmployeeId"].ToString())
                     , General.GetNullableInteger(ViewState["DocumentId"].ToString())
                     , General.GetNullableDateTime(ucOtherDocAuthenticationDate.Text)
                     , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     );
                }
                ucStatus.Text = "Updated Sucessfully";
                ucStatus.Visible = true;
                PopulateDetails(ViewState["DocType"].ToString());
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                                             "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', 'keepopen');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                                            "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', 'keepopen');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsvalidDate(string AuthenticationDate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrEmpty(AuthenticationDate))
            ucError.ErrorMessage = "Authentication Date is Required";
        else if (General.GetNullableDateTime(AuthenticationDate) > DateTime.Today)
        {
            ucError.ErrorMessage = "Authentication Date should be earlier than current Date";
        }
        return (!ucError.IsError);
    }

    private void BindPMUDoctor(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.DataSource = PhoenixRegistersPMUDoctor.ListPMUDoctor(1);
        ddl.DataTextField = "FLDDOCTORNAME";
        ddl.DataValueField = "FLDDOCTORID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--Select--", ""));
    }
}
