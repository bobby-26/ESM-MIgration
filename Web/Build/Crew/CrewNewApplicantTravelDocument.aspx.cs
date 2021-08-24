using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Drawing;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
public partial class CrewNewApplicantTravelDocument : PhoenixBasePage
{
    // protected override void Render(HtmlTextWriter writer)
    // {
    //     foreach (GridViewRow r in gvTravelDocument.Rows)
    //     {
    //if (r.RowType == DataControlRowType.DataRow)
    //{
    //    Page.ClientScript.RegisterForEventValidation
    //            (r.UniqueID + "$ctl00");
    //    Page.ClientScript.RegisterForEventValidation
    //            (r.UniqueID + "$ctl01");
    //}
    //     }
    //     base.Render(writer);
    // }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewNewApplicantTravelDocument.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvTravelDocument')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewTravelDocumentArchived.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&type=1&t=n'); return false;", "Show Archived", "<i class=\"fas fa-archive\"></i>", "ARCHIVED");
            MenuCrewNewApplicantTravelDocument.AccessRights = this.ViewState;
            MenuCrewNewApplicantTravelDocument.MenuList = toolbargrid.Show();
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            if (!IsPostBack)
            {
                DataSet ds = PhoenixRegistersThirdPartyLinks.ListThirdPartyLinks("CDC", PhoenixGeneralSettings.CurrentGeneralSetting.Nationality);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cdcChecker.InnerText = "\"" + ds.Tables[0].Rows[0]["FLDNATIONALITYNAME"].ToString() + " CDC\" Checker";
                    cdcChecker.HRef = ds.Tables[0].Rows[0]["FLDLINKNAME"].ToString().IndexOf("http://") > -1 ? ds.Tables[0].Rows[0]["FLDLINKNAME"].ToString() : "http://" + ds.Tables[0].Rows[0]["FLDLINKNAME"].ToString();
                }
                imgPassportArchive.Attributes.Add("onclick", "return fnConfirmDelete(event,'You are adding a new Passport, Please confirm to proceed.')");
                imgSeamanBook.Attributes.Add("onclick", "return fnConfirmDelete(event,'You are adding a new CDC, Please confirm to proceed.')");
                imgUSVisaArchive.Attributes.Add("onclick", "return fnConfirmDelete(event,'You are adding a US Visa, Please confirm to proceed.')");
                imgMCVArchive.Attributes.Add("onclick", "return fnConfirmDelete(event,'You are adding a new MCV, Please confirm to proceed .')");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ucECNR.HardTypeCode = ((int)PhoenixHardTypeCode.PASSPORTECNR).ToString();
                ucBlankPages.HardTypeCode = ((int)PhoenixHardTypeCode.PASSPORTECNR).ToString();
                if (PhoenixGeneralSettings.CurrentGeneralSetting.CountryCode != "IND")
                    ucECNR.SelectedHard = "171";
                SetEmployeePrimaryDetails();
                SetEmployeePassportDetails();
                SetEmployeeSeamanBookDetails();
                SetEmployeeUSVisaDetails();
                SetEmployeeMCVDetails();

                imgPPClip.Attributes["onclick"] = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                 + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.PASSPORT + "&cmdname=NPASSPORTUPLOAD'); return false;";
                imgCCClip.Attributes["onclick"] = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                 + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.SEAMANBOOK + "&cmdname=NSEAMANBOOKUPLOAD'); return false;";
                imgUSVisaClip.Attributes["onclick"] = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                 + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.USVISA + "&cmdname=NVISAUPLOAD'); return false;";
                imgMCVClip.Attributes["onclick"] = "javascript:openNewWindow('MCV','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                 + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MCVAUSTRALIA + "&cmdname=MCVUPLOAD'); return false;";

                gvTravelDocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            CrewPassPort.AccessRights = this.ViewState;
            CrewPassPort.MenuList = toolbarmain.Show();

            //BindData();
            //SetPageNavigator();
            //SetAttachmentMarking();

            DateTime? d = General.GetNullableDateTime(ucDateOfExpiry.Text);
            HtmlGenericControl html = new HtmlGenericControl();
            if (d.HasValue)
            {
                TimeSpan t = d.Value - DateTime.Now;
                if (t.Days >= 0 && t.Days < 120)
                {
                    imgPPFlag.Visible = true;

                    imgPPFlag.ImageUrl = Session["images"] + "/yellow.png";
                }
                else if (t.Days < 0)
                {
                    imgPPFlag.Visible = true;
                    imgPPFlag.ImageUrl = Session["images"] + "/red.png";

                }
                else
                    imgPPFlag.Visible = false;
            }
            d = General.GetNullableDateTime(ucSeamanDateOfExpiry.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - DateTime.Now;
                html = new HtmlGenericControl();
                if (t.Days >= 0 && t.Days < 120)
                {
                    imgCCFlag.Visible = true;
                    imgCCFlag.ImageUrl = Session["images"] + "/yellow.png";
                }
                else if (t.Days < 0)
                {
                    imgCCFlag.Visible = true;

                    imgCCFlag.ImageUrl = Session["images"] + "/red.png";
                }
                else
                    imgCCFlag.Visible = false;
            }
            d = General.GetNullableDateTime(txtUSDateofExpiry.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - DateTime.Now;
                html = new HtmlGenericControl();


                if (t.Days >= 0 && t.Days < 120)
                {
                    imgUSVisa.Visible = true;
                    imgUSVisa.ImageUrl = Session["images"] + "/yellow.png";

                }
                else if (t.Days < 0)
                {
                    imgUSVisa.Visible = true;
                    imgUSVisa.ImageUrl = Session["images"] + "/red.png";

                }
                else
                    imgUSVisa.Visible = false;
            }
            d = General.GetNullableDateTime(txtMCVDateofExpiry.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - DateTime.Now;
                html = new HtmlGenericControl();
                if (t.Days >= 0 && t.Days < 120)
                {
                    imgMCV.Visible = true;
                    imgMCV.ImageUrl = Session["images"] + "/yellow.png";
                }
                else if (t.Days < 0)
                {
                    imgMCV.Visible = true;
                    imgMCV.ImageUrl = Session["images"] + "/red.png";
                }
                else
                    imgMCV.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void OnClickPassportArchive(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewTravelDocument.EmployeePassportArchive(int.Parse(Filter.CurrentNewApplicantSelection));
            SetEmployeePassportDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void OnClickSeamanBookArchive(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewTravelDocument.EmployeeSeamanBookArchive(int.Parse(Filter.CurrentNewApplicantSelection));
            SetEmployeeSeamanBookDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void OnClickUSVisaArchive(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewTravelDocument.EmployeeUSVisaArchive(int.Parse(Filter.CurrentNewApplicantSelection));
            SetEmployeeUSVisaDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void OnClickMCVAustraliaArchive(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewTravelDocument.EmployeeMCVAustraliaArchive(int.Parse(Filter.CurrentNewApplicantSelection));
            SetEmployeeMCVDetails();
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
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
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
        gvTravelDocument.Rebind();
        SetEmployeePassportDetails();
        SetEmployeeSeamanBookDetails();
        SetEmployeeUSVisaDetails();
        SetEmployeeMCVDetails();
    }
    protected void CrewNewApplicantTravelDocumentMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvTravelDocument.Rebind();

            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDOCUMENTNAME", "FLDDOCUMENTNUMBER", "FLDDATEOFISSUE", "FLDVALIDFROM", "FLDDATEOFEXPIRY", "FLDNOOFENTRYNAME", "FLDPLACEOFISSUE", "FLDCOUNTRYNAME", "FLDREMARKS" };
        string[] alCaptions = { "Document Name", "Number", "Date of Issue", "Valid From", "Date of Expiry", "No of entry", "Place of Issue", "Nationality/Flag", "Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixNewApplicantTravelDocument.SearchEmployeeTravelDocument(Convert.ToInt32(Filter.CurrentNewApplicantSelection), 1,
                                                                              sortexpression, sortdirection,
                                                                            (int)ViewState["PAGENUMBER"],
                                                                            General.ShowRecords(null),
                                                                            ref iRowCount,
                                                                            ref iTotalPageCount);
        General.ShowExcel("Travel Document", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDDOCUMENTNAME", "FLDDOCUMENTNUMBER", "FLDDATEOFISSUE", "FLDVALIDFROM", "FLDDATEOFEXPIRY", "FLDNOOFENTRYNAME", "FLDPLACEOFISSUE", "FLDCOUNTRYNAME", "FLDREMARKS" };
            string[] alCaptions = { "Document Name", "Number", "Date of Issue", "Valid From", "Date of Expiry", "No of entry", "Place of Issue", "Nationality/Flag", "Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixNewApplicantTravelDocument.SearchEmployeeTravelDocument(Convert.ToInt32(Filter.CurrentNewApplicantSelection), 1,
                                                                               sortexpression, sortdirection,
                                                                               (int)ViewState["PAGENUMBER"],
                                                                               gvTravelDocument.PageSize,
                                                                               ref iRowCount,
                                                                               ref iTotalPageCount);

            General.SetPrintOptions("gvTravelDocument", "Travel Documnet", alCaptions, alColumns, ds);

            gvTravelDocument.DataSource = ds;
            gvTravelDocument.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewPassPort_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidPassport() || !IsValidPassport() || !IsValidSeamanBook())
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateEmployeePassport();
                UpdateEmployeeSeamanBook();
                UpdateEmployeeUSVisa();
                UpdateEmployeeMCV();
                //ucStatus.Text = "Passport, Seaman's Book & Visa information updated";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvTravelDocument_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();

    }

    public void UpdateEmployeePassport()
    {


        string pptno = RemoveSpecialCharacters(txtPassportnumber.Text);
        PhoenixNewApplicantTravelDocument.UpdateEmployeePassport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                                                            , General.GetNullableDateTime(ucDateOfIssue.Text)
                                                            , General.GetNullableString(txtPlaceOfIssue.Text)
                                                            , General.GetNullableDateTime(ucDateOfExpiry.Text)
                                                            , General.GetNullableInteger(ucECNR.SelectedHard)
                                                            , General.GetNullableInteger(ucBlankPages.SelectedHard)
                                                            , General.GetNullableString(pptno)
                                                            , Convert.ToInt16(chkpptVerifieddyn.Checked ? 1 : 0)
                                                            , Convert.ToInt16(chkcrosscheck.Checked ? 1 : 0)
                                                            );
        //if (string.IsNullOrEmpty(ViewState["dtkey"].ToString()))
        //{
        //    PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["attachmentcode"].ToString()), PhoenixModule.CREW, null, string.Empty, string.Empty, "Passport");
        //}
        //else
        //{
        //    PhoenixCommonFileAttachment.UpdateAttachment(Request.Files, new Guid(ViewState["dtkey"].ToString()), PhoenixModule.CREW);
        //}
        SetEmployeePassportDetails();
    }

    public static bool IsValidTextBox(string text)
    {

        string regex = "^[0-9a-zA-Z ]+$";
        Regex re = new Regex(regex);
        if (!re.IsMatch(text))
            return (false);

        return true;
    }

    public bool IsValidPassport()
    {

        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultdate;

        if (General.GetNullableDateTime(ucDateOfIssue.Text) != null)
        {
            if (DateTime.TryParse(ucDateOfIssue.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
            {
                ucError.ErrorMessage = "Passport Date of Issue should be earlier than current date";
            }
            if (DateTime.TryParse(ucDateOfIssue.Text, out resultdate)
                && DateTime.TryParse(ucDateOfExpiry.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(ucDateOfIssue.Text)) < 0)
            {
                ucError.ErrorMessage = "Passport Date of Expiry should be later than 'Date of Issue'";
            }
        }


        return (!ucError.IsError);

    }
    public bool IsValidUSVisa()
    {

        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultdate;
        if (!string.IsNullOrEmpty(txtUSVisaType.Text) || !string.IsNullOrEmpty(txtUSVisaNumber.Text) || !string.IsNullOrEmpty(txtUSVisaIssuedOn.Text)
            || !string.IsNullOrEmpty(txtUSDateofExpiry.Text) || !string.IsNullOrEmpty(txtUSPlaceOfIssue.Text))
        {
            if (txtUSVisaType.Text == "")
                ucError.ErrorMessage = "US Visa Type is required";

            if (txtUSVisaNumber.Text == "")
                ucError.ErrorMessage = "US Visa Number is required";

            if (!DateTime.TryParse(txtUSVisaIssuedOn.Text, out resultdate))
                ucError.ErrorMessage = "US Visa Issued On is required";
            else if (DateTime.TryParse(txtUSVisaIssuedOn.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
            {
                ucError.ErrorMessage = "US Visa Issued On should be earlier than current date";
            }

            if (!DateTime.TryParse(txtUSDateofExpiry.Text, out resultdate))
                ucError.ErrorMessage = "US Visa Date of Expiry is required";
            else if (DateTime.TryParse(txtUSVisaIssuedOn.Text, out resultdate)
                && DateTime.TryParse(txtUSDateofExpiry.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(txtUSVisaIssuedOn.Text)) < 0)
            {
                ucError.ErrorMessage = "US Visa Date of Expiry should be later than 'Date of Issue'";
            }

            if (txtUSPlaceOfIssue.Text == "")
                ucError.ErrorMessage = "US Visa Place of Issue is required";
        }
        return (!ucError.IsError);

    }
    public void UpdateEmployeeSeamanBook()
    {

        try
        {
            //string cdcno = RemoveSpecialCharacters(txtSeamanBookNumber.Text);
            PhoenixNewApplicantTravelDocument.UpdateEmployeeSeamanBook(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                                                                , General.GetNullableInteger(ucSeamanCountry.SelectedFlag)
                                                                , General.GetNullableDateTime(ucSeamanDateOfIssue.Text)
                                                                , General.GetNullableString(txtSeamanPlaceOfIssue.Text.Trim())
                                                                , General.GetNullableDateTime(ucSeamanDateOfExpiry.Text)
                                                                , Convert.ToInt16(chkVerifiedYN.Checked ? 1 : 0)
                                                                , General.GetNullableString(txtSeamanBookNumber.Text)
                                                                , Convert.ToInt16(chkcdccrosscheckedyn.Checked ? 1 : 0)
                                                                );

            SetEmployeeSeamanBookDetails();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    public void UpdateEmployeeUSVisa()
    {

        try
        {


            PhoenixCrewTravelDocument.UpdateEmployeeUSVisa(Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                                                                , General.GetNullableString(txtUSVisaType.Text.Trim())
                                                                , General.GetNullableString(txtUSVisaNumber.Text.Trim())
                                                                , General.GetNullableDateTime(txtUSVisaIssuedOn.Text)
                                                                , General.GetNullableString(txtUSPlaceOfIssue.Text.Trim())
                                                                , General.GetNullableDateTime(txtUSDateofExpiry.Text)
                                                                );

            SetEmployeeUSVisaDetails();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    public void UpdateEmployeeMCV()
    {

        try
        {
            PhoenixCrewTravelDocument.UpdateEmployeeMCVAustralia(Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                                                                , General.GetNullableString(txtMCVNumber.Text.Trim())
                                                                , General.GetNullableDateTime(txtMCVIssuedOn.Text)
                                                                , General.GetNullableDateTime(txtMCVDateofExpiry.Text)
                                                                , General.GetNullableString(txtMCVRemarks.Text.Trim())
                                                                );

            SetEmployeeMCVDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    public bool IsValidSeamanBook()
    {

        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;

        if (!IsValidSeamansBookTextBox(txtSeamanBookNumber.Text.Trim()))
            ucError.ErrorMessage = "Invalid Seaman Book Number";

        if (General.GetNullableDateTime(ucDateOfIssue.Text) != null)
        {
            if (DateTime.TryParse(ucSeamanDateOfIssue.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
            {
                ucError.ErrorMessage = "Seaman's Book Date of Issue should be earlier than current date";
            }
            if (DateTime.TryParse(ucSeamanDateOfIssue.Text, out resultdate)
            && DateTime.TryParse(ucSeamanDateOfExpiry.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(ucSeamanDateOfIssue.Text)) < 0)
            {
                ucError.ErrorMessage = "Seaman's Book Date of Expiry should be later than 'Date of Issue'";
            }
        }

        return (!ucError.IsError);

    }

    public static bool IsValidSeamansBookTextBox(string text)
    {

        string regex = "^[0-9a-zA-Z-]+$";
        Regex re = new Regex(regex);
        if (!re.IsMatch(text))
            return (false);

        return true;
    }


    public void SetEmployeePassportDetails()
    {
        DataTable dt = PhoenixNewApplicantTravelDocument.ListEmployeePassport(Convert.ToInt32(Filter.CurrentNewApplicantSelection));

        if (dt.Rows.Count > 0)
        {
            txtPassportnumber.Text = dt.Rows[0]["FLDPASSPORTNO"].ToString();
            ucDateOfIssue.Text = dt.Rows[0]["FLDDATEOFISSUE"].ToString();
            txtPlaceOfIssue.Text = dt.Rows[0]["FLDPLACEOFISSUE"].ToString();
            ucDateOfExpiry.Text = dt.Rows[0]["FLDDATEOFEXPIRY"].ToString();
            ucECNR.SelectedHard = dt.Rows[0]["FLDECNRYESNO"].ToString();
            ucBlankPages.SelectedHard = dt.Rows[0]["FLDMINIMUMPAGE"].ToString();

            chkpptVerifieddyn.Checked = dt.Rows[0]["FLDPPTVERIFIEDYN"].ToString() == "1" ? true : false;
            chkpptVerifieddyn.Enabled = dt.Rows[0]["FLDPPTVERIFIEDYN"].ToString() == "1" ? false : true;
            txtpptVerifiedby.Text = dt.Rows[0]["FLDPPTVERIFIEDBY"].ToString();
            ucpptVerifieddate.Text = dt.Rows[0]["FLDPPTVERIFIEDDAE"].ToString();

            chkcrosscheck.Checked = dt.Rows[0]["FLDPPTCROSSCHECKEDYN"].ToString() == "1" ? true : false;
            chkcrosscheck.Enabled = dt.Rows[0]["FLDPPTCROSSCHECKEDYN"].ToString() == "1" ? false : true;
            txtpptcrosscheckby.Text = dt.Rows[0]["FLDPPTCROSSCHECKEDBY"].ToString();
            ucpptcrosscheckdate.Text = dt.Rows[0]["FLDPPTCROSSCHECKEDDAE"].ToString();


            if (dt.Rows[0]["FLDISATTACHMENT"].ToString() == string.Empty)
                imgPPClip.ImageUrl = Session["images"] + "/no-attachment.png";
            else
                imgPPClip.ImageUrl = Session["images"] + "/attachment.png";
            if (txtPassportnumber.Text != "" && General.GetNullableDateTime(ucDateOfIssue.Text) != null)
            {
                imgPassportArchive.Visible = true;
            }
            else
            {
                imgPassportArchive.Visible = false;
            }
            txtUpdatedByPPT.Text = dt.Rows[0]["FLDPPTUPDATEDBY"].ToString();
            ucUpdatedDatePPT.Text = dt.Rows[0]["FLDPPTUPDATEDDATE"].ToString();
        }
        else
            imgPPClip.Visible = false;
    }

    public void SetEmployeeUSVisaDetails()
    {
        DataTable dt = PhoenixCrewTravelDocument.ListEmployeeUSVisa(Convert.ToInt32(Filter.CurrentNewApplicantSelection));

        if (dt.Rows.Count > 0)
        {
            txtUSVisaType.Text = dt.Rows[0]["FLDUSVISATYPE"].ToString();
            txtUSVisaNumber.Text = dt.Rows[0]["FLDUSVISANUMBER"].ToString();
            txtUSVisaIssuedOn.Text = dt.Rows[0]["FLDUSVISADATEOFISSUE"].ToString();
            txtUSPlaceOfIssue.Text = dt.Rows[0]["FLDUSVISAPLACEOFISSUE"].ToString();
            txtUSDateofExpiry.Text = dt.Rows[0]["FLDUSVISADATEOFEXPIRY"].ToString();

            if (dt.Rows[0]["FLDISATTACHMENT"].ToString() == string.Empty)
                imgUSVisaClip.ImageUrl = Session["images"] + "/no-attachment.png";
            else
                imgUSVisaClip.ImageUrl = Session["images"] + "/attachment.png";
            if (txtUSVisaNumber.Text != "")
            {
                imgUSVisaArchive.Visible = true;
            }
            else
            {
                imgUSVisaArchive.Visible = false;
            }
            txtUpdatedByUS.Text = dt.Rows[0]["FLDUSVISAUPDATEDBY"].ToString();
            ucUpdatedDateUS.Text = dt.Rows[0]["FLDUSVISAUPDATEDDATE"].ToString();
        }
        else
        {
            imgUSVisaClip.Visible = false;
        }
    }

    public void SetEmployeeMCVDetails()
    {
        DataTable dt = PhoenixCrewTravelDocument.ListEmployeeMCVAustralia(Convert.ToInt32(Filter.CurrentNewApplicantSelection));

        if (dt.Rows.Count > 0)
        {
            txtMCVNumber.Text = dt.Rows[0]["FLDMCVAUSTRALIATXNUMBER"].ToString();
            txtMCVIssuedOn.Text = dt.Rows[0]["FLDMCVAUSTRALIAISSUEDATE"].ToString();
            txtMCVDateofExpiry.Text = dt.Rows[0]["FLDMCVAUSTRALIAEXPIRYDATE"].ToString();
            txtMCVRemarks.Text = dt.Rows[0]["FLDMCVAUSTRALIAREMARKS"].ToString();
            if (dt.Rows[0]["FLDISATTACHMENT"].ToString() == string.Empty)
                imgMCVClip.ImageUrl = Session["images"] + "/no-attachment.png";
            else
                imgMCVClip.ImageUrl = Session["images"] + "/attachment.png";
            if (txtMCVNumber.Text != "")
            {
                imgMCVArchive.Visible = true;
            }
            else
            {
                imgMCVArchive.Visible = false;
            }
            txtUpdatedByMCV.Text = dt.Rows[0]["FLDMCVUPDATEDBY"].ToString();
            ucUpdatedDateMCV.Text = dt.Rows[0]["FLDMCVUPDATEDDATE"].ToString();
        }
        else
        {
            imgMCVClip.Visible = false;
        }
    }
    public void SetEmployeeSeamanBookDetails()
    {
        DataTable dt = PhoenixNewApplicantTravelDocument.ListEmployeeSeamanBook(Convert.ToInt32(Filter.CurrentNewApplicantSelection));

        if (dt.Rows.Count > 0)
        {
            txtSeamanBookNumber.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
            ucSeamanDateOfIssue.Text = dt.Rows[0]["FLDSDATEOFISSUE"].ToString();
            txtSeamanPlaceOfIssue.Text = dt.Rows[0]["FLDSPLACEOFISSUE"].ToString();
            ucSeamanDateOfExpiry.Text = dt.Rows[0]["FLDSDATEOFEXPIRY"].ToString();
            ucSeamanCountry.SelectedFlag = dt.Rows[0]["FLDSEAMANFLAG"].ToString();
            chkVerifiedYN.Checked = dt.Rows[0]["FLDCDCVERIFIEDYN"].ToString() == "1" ? true : false;
            chkVerifiedYN.Enabled = dt.Rows[0]["FLDCDCVERIFIEDYN"].ToString() == "1" ? false : true;
            txtVerifiedBy.Text = dt.Rows[0]["FLDVERIFIEDBYNAME"].ToString();
            txtVerifiedOn.Text = dt.Rows[0]["FLDVERIFIEDON"].ToString();

            chkcdccrosscheckedyn.Checked = dt.Rows[0]["FLDCDCCROSSCHECKEDYN"].ToString() == "1" ? true : false;
            chkcdccrosscheckedyn.Enabled = dt.Rows[0]["FLDCDCCROSSCHECKEDYN"].ToString() == "1" ? false : true;
            txtcdccrosscheckedby.Text = dt.Rows[0]["FLDCDCCROSSCHECKEDBY"].ToString();
            uccdccrosscheckeddate.Text = dt.Rows[0]["FLDCDCCROSSCHECKEDDAE"].ToString();

            if (dt.Rows[0]["FLDISATTACHMENT"].ToString() == string.Empty)
                imgCCClip.ImageUrl = Session["images"] + "/no-attachment.png";
            else
                imgCCClip.ImageUrl = Session["images"] + "/attachment.png";
            if (txtSeamanBookNumber.Text != "" && General.GetNullableDateTime(ucSeamanDateOfIssue.Text) != null)
            {
                imgSeamanBook.Visible = true;
            }
            else
            {
                imgSeamanBook.Visible = false;
            }
            txtUpdatedByCDC.Text = dt.Rows[0]["FLDCDCUPDATEDBY"].ToString();
            ucUpdatedDateCDC.Text = dt.Rows[0]["FLDCDCUPDATEDDATE"].ToString();

        }
        else
            imgCCClip.Visible = false;
    }

    private bool IsValidTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, UserControlDate dateofexpiry, string country)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        Int16 result;
        DateTime resultdate;

        if (documenttype.Equals("") || !Int16.TryParse(documenttype, out result))
            ucError.ErrorMessage = "Document Type  is required";

        if (documentnumber.Trim() == "")
            ucError.ErrorMessage = "Document Number is required";

        if (!DateTime.TryParse(dateofissue, out resultdate))
            ucError.ErrorMessage = "Date of Issue is required";
        else if (DateTime.TryParse(dateofissue, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Date of Issue should be earlier than current date";
        }

        if (placeofissue.Trim() == "")
            ucError.ErrorMessage = "Place of Issue is required";

        if (!DateTime.TryParse(dateofexpiry.Text, out resultdate) && dateofexpiry.CssClass == "input_mandatory")
            ucError.ErrorMessage = "Expiry Date is required";
        else if (DateTime.TryParse(dateofissue, out resultdate)
            && DateTime.TryParse(dateofexpiry.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(dateofissue)) < 0)
        {
            ucError.ErrorMessage = "Date of Expiry should be later than 'Date of Issue'";
        }
        if (country.Equals("") || !Int16.TryParse(country, out result))
            ucError.ErrorMessage = "Nationality/Flag  is required";

        return (!ucError.IsError);
    }


    protected void ddlDocumentType_TextChanged(object sender, EventArgs e)
    {
        UserControlDocumentType dc = (UserControlDocumentType)sender;
        DataSet ds = new DataSet();
        UserControlDate date;
        if (dc.SelectedDocumentType != "Dummy")
        {
            ds = PhoenixRegistersDocumentOther.EditDocumentOther(General.GetNullableInteger(dc.SelectedDocumentType).Value);
        }
        if (dc.ID == "ucDocumentTypeAdd" && ds.Tables.Count > 0)
        {
            GridFooterItem footerItem = (GridFooterItem)gvTravelDocument.MasterTableView.GetItems(GridItemType.Footer)[0];
            date = (UserControlDate)footerItem.FindControl("ucDateExpiryAdd");


        }
        else
        {
            GridDataItem row = ((GridDataItem)dc.Parent.Parent);
            date = (UserControlDate)gvTravelDocument.Items[row.RowIndex].FindControl("ucDateExpiryEdit");
        }
        if (ds.Tables[0].Rows[0]["FLDHAVINGEXPIRY"].ToString() == "1")
            date.CssClass = "input_mandatory";
        else
            date.CssClass = "input";
    }


    public static string RemoveSpecialCharacters(string str)
    {
        // Strips all special characters and spaces from a string.
        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
    }

    protected void gvTravelDocument_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTravelDocument.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelDocument_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "SORT") return;


            if (e.CommandName.ToString().ToUpper() == "ADD")
            {


                string documenttype = ((UserControlDocumentType)e.Item.FindControl("ucDocumentTypeAdd")).SelectedDocumentType;
                string documentnumber = ((RadTextBox)e.Item.FindControl("txtNumberAdd")).Text;
                string dateofissue = ((UserControlDate)e.Item.FindControl("ucDateOfIssueAdd")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceIssueAdd")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("ucDateExpiryAdd"));
                string country = ((UserControlCountry)e.Item.FindControl("ddlCountryAdd")).SelectedCountry;
                string validfrom = ((UserControlDate)e.Item.FindControl("ucValidFromAdd")).Text;
                string noofentry = ((RadComboBox)e.Item.FindControl("ddlNoofentryAdd")).SelectedValue;
                RadComboBox ddlFullterm = ((RadComboBox)e.Item.FindControl("ddlFullTermAdd"));
                string passportno = ((RadTextBox)e.Item.FindControl("txtpassportnoAdd")).Text;

                if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry, country))
                {
                    ucError.Visible = true;

                    return;
                }
                PhoenixNewApplicantTravelDocument.InsertEmployeeTravelDocument(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                          , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                                                          , Convert.ToInt32(documenttype)
                                                          , documentnumber
                                                          , Convert.ToDateTime(dateofissue)
                                                          , placeofissue
                                                          , General.GetNullableDateTime(dateofexpiry.Text)
                                                          , int.Parse(country)
                                                          , General.GetNullableDateTime(validfrom)
                                                          , General.GetNullableInteger(noofentry)
                                                          , null
                                                          , General.GetNullableString(passportno)
                                                          , 1
                                                          );

                gvTravelDocument.Rebind();

            }
            else if (e.CommandName.ToString().ToUpper() == "ARCHIVE")
            {
                string id = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;
                PhoenixNewApplicantTravelDocument.ArchiveEmployeeTravelDocument(int.Parse(id), 0);

                gvTravelDocument.Rebind();

            }
            else if (e.CommandName.ToUpper() == "UPDATE")
            {

                try
                {
                    string documenttype = ((UserControlDocumentType)e.Item.FindControl("ucDocumentTypeEdit")).SelectedDocumentType;
                    string documentnumber = ((RadTextBox)e.Item.FindControl("txtNumberEdit")).Text;
                    string dateofissue = ((UserControlDate)e.Item.FindControl("ucDateOfIssueEdit")).Text;
                    string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssue")).Text;
                    UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("ucDateExpiryEdit"));
                    string counry = ((UserControlCountry)e.Item.FindControl("ddlCountryEdit")).SelectedCountry;
                    string traveldocumentid = ((RadLabel)e.Item.FindControl("lbltraveldocumentidEdit")).Text;
                    string validfrom = ((UserControlDate)e.Item.FindControl("ucValidFromEdit")).Text;
                    string noofentry = ((RadComboBox)e.Item.FindControl("ddlNoofentryEdit")).SelectedValue;
                    RadComboBox ddlFullterm = ((RadComboBox)e.Item.FindControl("ddlFullTermEdit"));
                    string passportno = ((RadTextBox)e.Item.FindControl("txtpassportnoEdit")).Text;

                    if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry, counry))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixNewApplicantTravelDocument.UpdateEmployeeTravelDocument(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                   , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                                                                   , Convert.ToInt32(traveldocumentid)
                                                                   , Convert.ToInt32(documenttype)
                                                                   , documentnumber
                                                                   , Convert.ToDateTime(dateofissue)
                                                                   , placeofissue
                                                                   , General.GetNullableDateTime(dateofexpiry.Text)
                                                                   , int.Parse(counry)
                                                                   , General.GetNullableDateTime(validfrom)
                                                                   , General.GetNullableInteger(noofentry)
                                                                   , null
                                                                   , General.GetNullableString(passportno)
                                                                   , 1
                                                                   );
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }


                gvTravelDocument.Rebind();
            }
            else if (e.CommandName.ToUpper() == "DELETE")
            {
                try
                {


                    string traveldocumentid = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;

                    PhoenixNewApplicantTravelDocument.DeleteEmployeeTravelDocument(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , Convert.ToInt32(traveldocumentid)
                                                                        );

                    gvTravelDocument.Rebind();

                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;

                }
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

    protected void gvTravelDocument_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null) db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
            if (db1 != null) db1.Visible = SessionUtil.CanAccess(this.ViewState, db1.CommandName);
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null) att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            HtmlGenericControl html2 = new HtmlGenericControl();

            if (lblIsAtt != null && lblIsAtt.Text == string.Empty)
            {
                html2 = new HtmlGenericControl();
                html2.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                att.Controls.Add(html2);
            }

            if (att != null)
            {
                att.Attributes.Add("onclick", "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
+ PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&cmdname=NTRAVELDOCUPLOAD'); return false;");
            }

            //att.ImageUrl = Session["images"] + "/no-attachment.png";


            RadLabel lbl = (RadLabel)e.Item.FindControl("lbltraveldocumentid");
            HtmlImage img = (HtmlImage)e.Item.FindControl("imgRemarks");
            RadLabel lblremarktext = (RadLabel)e.Item.FindControl("lblremarktext");
            LinkButton lblR = (LinkButton)e.Item.FindControl("lblRemarks");

            HtmlGenericControl html1 = new HtmlGenericControl();

            if (lblR != null && string.IsNullOrEmpty(lblremarktext.Text.Trim()))
            {
                html1 = new HtmlGenericControl();
                lblR.Visible = true;
                html1.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses-nr\"></i></span>";//  " <span class=\"icon\"><i class=\"fas fa-binoculars\" style=\"color:gray\"></i></span>";
                lblR.Controls.Add(html1);
                // img.Src = Session["images"] + "/no-remarks.png";
            }

            if (lblR != null) lblR.Attributes.Add("onclick", "javascript:openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "','xlarge'); return true;");
            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            RadLabel expdate = e.Item.FindControl("lblDateofExpiry") as RadLabel;
            DateTime? d = null;
            UserControlToolTip RadToolTipimgFlag = (UserControlToolTip)e.Item.FindControl("RadToolTipimgFlag");
            if (expdate != null) d = General.GetNullableDateTime(expdate.Text);
            HtmlGenericControl html = new HtmlGenericControl();
            if (d.HasValue && imgFlag != null)
            {
                TimeSpan t = d.Value - DateTime.Now;
                if (t.Days >= 0 && t.Days < 120)
                {
                    //e.Item.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                }
                else if (t.Days < 0)
                {
                    //e.Item.CssClass = "rowred";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red.png";
                }
            }

            UserControlCountry ucCountry = (UserControlCountry)e.Item.FindControl("ddlCountryEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucCountry != null) ucCountry.SelectedCountry = drv["FLDCOUNTRYCODE"].ToString();


            UserControlDocumentType ucDocumentType = (UserControlDocumentType)e.Item.FindControl("ucDocumentTypeEdit");
            DataRowView drvDocumentType = (DataRowView)e.Item.DataItem;
            if (ucDocumentType != null) ucDocumentType.SelectedDocumentType = drvDocumentType["FLDDOCUMENTTYPE"].ToString();

            RadComboBox ddlNoofentryEdit = (RadComboBox)e.Item.FindControl("ddlNoofentryEdit");
            DataRowView drvNoofentry = (DataRowView)e.Item.DataItem;
            if (ddlNoofentryEdit != null) ddlNoofentryEdit.SelectedValue = drvNoofentry["FLDNOOFENTRY"].ToString();

            RadComboBox ddlFullterm = (RadComboBox)e.Item.FindControl("ddlFullTermEdit");
            if (ddlFullterm != null) ddlFullterm.SelectedValue = drv["FLDFULLTERMYN"].ToString();


        }


        if (e.Item is GridFooterItem)
        {
            LinkButton ad = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
        }
    }

    protected void gvTravelDocument_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
