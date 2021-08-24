using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
public partial class CrewOffshore_CrewOffshorePortalDocumentsList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            ViewState["empid"] = "";
            if (Request.QueryString["empid"] != null)
                ViewState["empid"] = Request.QueryString["empid"].ToString();
        }

        /* Travel Document */
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        toolbargrid.AddImageButton("../CrewOffShore/CrewOffshoreDocumentsList.aspx?empid=" + ViewState["empid"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('gvTravelDocument')", "Print Grid", "icon_print.png", "PRINT");
        toolbargrid.AddImageLink("javascript:parent.Openpopup('NAFA','','../Crew/CrewTravelDocumentArchived.aspx?empid=" + ViewState["empid"].ToString() + "&type=1&t=p'); return false;", "Show Archived", "show-archive.png", "ARCHIVED");
        MenuCrewTravelDocument.AccessRights = this.ViewState;

        /* Licence */
        PhoenixToolbar toolbarLIC = new PhoenixToolbar();
        // toolbarLIC.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDocumentsList.aspx?e=1&empid=" + ViewState["empid"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        toolbarLIC.AddFontAwesomeButton("javascript:CallPrint('gvCrewLicence')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");


        MenuCrewLicence.AccessRights = this.ViewState;
        MenuCrewLicence.MenuList = toolbarLIC.Show();

        /* Medical */
        int? vesselid = null;
        AutoArchive();
        AutoCorrectMedicalStatus();


        PhoenixToolbar toolbarMDI = new PhoenixToolbar();
        toolbarMDI.AddFontAwesomeButton("../CrewOffShore/CrewOffshoreDocumentsList.aspx?empid=" + ViewState["empid"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "MedicalExcel");
        toolbarMDI.AddFontAwesomeButton("javascript:CallPrint('gvCrewMedical')", "Print Grid", "<i class=\"fas fa-print\"></i>", "MedicalPRINT");
        toolbarMDI.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewMedicalAdd.aspx?empid=" + ViewState["empid"].ToString() + "&PORTAL=1')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDMEDICAL");

        SetCourseRequest(ref vesselid, null);


        MenuCrewMedical.AccessRights = this.ViewState;
        MenuCrewMedical.MenuList = toolbarMDI.Show();

        toolbarMDI = new PhoenixToolbar();
        toolbarMDI.AddFontAwesomeButton("../CrewOffShore/CrewOffshoreDocumentsList.aspx?empid=" + ViewState["empid"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "MedicalTestExcel");
        toolbarMDI.AddFontAwesomeButton("javascript:CallPrint('gvMedicalTest')", "Print Grid", "<i class=\"fas fa-print\"></i>", "MedicalTestPRINT");
        CrewMedicalTest.AccessRights = this.ViewState;
        CrewMedicalTest.MenuList = toolbarMDI.Show();

        /* Course */
        PhoenixToolbar toolbarCOU = new PhoenixToolbar();
        toolbarCOU.AddFontAwesomeButton("../CrewOffShore/CrewOffshoreDocumentsList.aspx?empid=" + ViewState["empid"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbarCOU.AddFontAwesomeButton("javascript:CallPrint('gvCrewCourseCertificate')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");


        MenuCrewCourseCertificate.AccessRights = this.ViewState;
        MenuCrewCourseCertificate.MenuList = toolbarCOU.Show();

        /* Other Documnet*/


        PhoenixToolbar toolbargridOTH = new PhoenixToolbar();
        toolbargridOTH.AddFontAwesomeButton("../CrewOffShore/CrewOffshoreDocumentsList.aspx?empid=" + ViewState["empid"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargridOTH.AddFontAwesomeButton("javascript:CallPrint('gvOtherDocument')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");


        MenuCrewOtherDocument.AccessRights = this.ViewState;
        MenuCrewOtherDocument.MenuList = toolbargridOTH.Show();

        if (!IsPostBack)
        {

            //imgPassportArchive.Attributes.Add("onclick", "return fnConfirmDelete(event,'You are adding a new passport,please confirm to proceed?')");
            //imgSeamanBook.Attributes.Add("onclick", "return fnConfirmDelete(event,'You are adding a new CDC,please confirm to proceed ?')");

            ViewState["PAGENUMBERTRV"] = 1;
            ViewState["SORTEXPRESSIONTRV"] = null;
            ViewState["SORTDIRECTIONTRV"] = null;
            gvTravelDocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            ucECNR.HardTypeCode = ((int)PhoenixHardTypeCode.PASSPORTECNR).ToString();
            ucBlankPages.HardTypeCode = ((int)PhoenixHardTypeCode.PASSPORTECNR).ToString();
            ucECNRemp.HardTypeCode = ((int)PhoenixHardTypeCode.PASSPORTECNR).ToString();
            ucBlankPagesemp.HardTypeCode = ((int)PhoenixHardTypeCode.PASSPORTECNR).ToString();
            SetEmployeePrimaryDetails();
            SetEmployeePassportDetails();
            SetEmployeePassportDetailsBySeafarer();
            SetEmployeeSeamanBookDetails();
            SetEmployeeSeamanBookDetailsBySeafarer();


            imgPPClip.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                  + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.PASSPORT + "&cmdname=PASSPORTUPLOAD&u=n'); return false;";

            imgCCClip.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
             + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.SEAMANBOOK + "&cmdname=SEAMANBOOKUPLOAD&u=n'); return false;";

            imgPPClipemp.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcodeemp"] + "&mod="
                 + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.PASSPORT + "&cmdname=PASSPORTUPLOAD'); return false;";

            imgCCClipemp.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcodeemp"] + "&mod="
             + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.SEAMANBOOK + "&cmdname=SEAMANBOOKUPLOAD'); return false;";


            ViewState["PAGENUMBERCOU"] = 1;

            /* Licence */
            ViewState["PAGENUMBERLIC"] = 1;
            ViewState["SORTEXPRESSIONLIC"] = null;
            ViewState["SORTDIRECTIONLIC"] = null;
            gvCrewLicence.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);



            ViewState["FEPAGENUMBER"] = 1;
            ViewState["FESORTEXPRESSION"] = null;
            ViewState["FESORTDIRECTION"] = null;
            ViewState["FECURRENTINDEX"] = 1;
            gvCrewCourseCertificate.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);


            ViewState["PAGENUMBEROTH"] = 1;
            ViewState["SORTEXPRESSIONOTH"] = null;
            ViewState["SORTDIRECTIONOTH"] = null;
            gvOtherDocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);


        }
        //    BindDataTRV();
        //  SetPageNavigatorTRV();

        //  BindMedicalData();
        //   BindMedicalTestData();

        //  BindCourse();
        //SetPageNavigatorCOU();
        // SetAttachmentMarking();

        // BindDataLIC();
        //   SetPageNavigatorLIC();

        //   BindDataOTH();
        //  SetPageNavigatorOTH();
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        //toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

        CrewPassPort.MenuList = toolbarmain.Show();


    }
    /* Travel */

    protected void CrewTravelDocumentMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindDataTRV();
                //   SetPageNavigatorTRV();
            }

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelTRV();
            }

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
                if (!IsValidPassport() || !IsValidSeamanBook())
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateEmployeePassport();
                UpdateEmployeeSeamanBook();
                //ucStatus.Text = "Passport, Seaman's Book & Visa information updated";
            }
            if (CommandName.ToUpper() == "SENDMAIL")
            {
                try
                {
                    PhoenixCrewTravelDocument.EmployeeTravelDocsSendMail(null, General.GetNullableInteger(ViewState["empid"].ToString()));
                    ucStatus.Text = "Mail sent successfully";
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

            imgPPClipemp.Visible = true;
            imgPPClipemp.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcodeemp"] + "&mod="
               + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.PASSPORT + "&cmdname=PASSPORTUPLOAD'); return false;";


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
        else
        {
            imgPPClipemp.ImageUrl = Session["images"] + "/no-attachment.png";
            imgPPClipemp.Visible = false;
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

            imgCCClipemp.Visible = true;
            imgCCClipemp.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcodeemp"] + "&mod="
          + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.SEAMANBOOK + "&cmdname=SEAMANBOOKUPLOAD'); return false;";

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
        else
        {
            imgCCClipemp.Visible = false;
        }
    }
    private void BindDataTRV()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDDOCUMENTNAME", "FLDDOCUMENTNUMBER", "FLDCATEGORYNAME", "FLDDATEOFISSUE", "FLDVALIDFROM", "FLDDATEOFEXPIRY", "FLDNOOFENTRYNAME", "FLDPLACEOFISSUE", "FLDCOUNTRYNAME", "FLDREMARKS",
                                 "FLDPASSPORTNO", "FLDDOCSTATUS", "FLDFULLTERM", "FLDAUTHENTICATEDYN", "FLDAUTHENTICATEDON","FLDAUTHENTICATEDBYNAME", "FLDCROSSCHECKEDYN", "FLDVERIFIEDDATE", "FLDVERIFIEDBY" };
            string[] alCaptions = { "Document Name", "Number", "Category", "Date of Issue", "Valid From", "Date of Expiry", "No of entry", "Place of Issue", "Nationality/Flag", "Remarks",
                                  "Passport No", "Doc Status", "Full Term YN", " Authenticated Y/N", "Authenticated Date", "Authenticated By","Cross checked Y/N","Cross checked Date","Cross checked by" };

            string sortexpression = (ViewState["SORTEXPRESSIONTRV"] == null) ? null : (ViewState["SORTEXPRESSIONTRV"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONTRV"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONTRV"].ToString());


            DataSet ds = PhoenixCrewTravelDocument.SearchEmployeeTravelDocument(Convert.ToInt32(ViewState["empid"].ToString()), null, 1,
                                                                               sortexpression, sortdirection,
                                                                                //(int)ViewState["PAGENUMBERTRV"],
                                                                                int.Parse(ViewState["PAGENUMBERTRV"].ToString()),
                                                                               gvTravelDocument.PageSize,
                                                                               ref iRowCount,
                                                                               ref iTotalPageCount,
                                                                               General.GetNullableInteger(chkPendingAuthentication.Checked == true ? "1" : "0"),
                                                                               General.GetNullableInteger(chkPendingCrosscheck.Checked == true ? "1" : "0"));

            General.SetPrintOptions("gvTravelDocument", "Travel Documnet", alCaptions, alColumns, ds);

            //if (ds.Tables[0].Rows.Count > 0)
            //{

            gvTravelDocument.DataSource = ds;
            gvTravelDocument.VirtualItemCount = iRowCount;
            //  gvTravelDocument.DataBind();
            //}
            //else
            //{

            //    DataTable dt = ds.Tables[0];
            //    ShowNoRecordsFound(dt, gvTravelDocument);
            //}

            ViewState["ROWCOUNTTRV"] = iRowCount;
            // ViewState["TOTALPAGECOUNTTRV"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvTravelDocument_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //RadGrid _gridView = (RadGrid)sender;
        try
        {
            if (e.CommandName.ToString().ToUpper() == "SORT") return;

            //   int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToString().ToUpper() == "ADD")
            {


                string documenttype = ((UserControlDocumentType)e.Item.FindControl("ucDocumentTypeAdd")).SelectedDocumentType;
                string documentnumber = ((RadTextBox)e.Item.FindControl("txtNumberAdd")).Text;
                string dateofissue = ((UserControlDate)e.Item.FindControl("ucDateOfIssueAdd")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceIssueAdd")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("ucDateExpiryAdd"));
                string country = ((UserControlCountry)e.Item.FindControl("ddlCountryAdd")).SelectedCountry;
                string connectedtovessel = ((RadCheckBox)e.Item.FindControl("chkConnectedToVesselAdd")).Checked == true ? "1" : "0";
                string validfrom = ((UserControlDate)e.Item.FindControl("ucValidFromAdd")).Text;
                string noofentry = ((RadComboBox)e.Item.FindControl("ddlNoofentryAdd")).SelectedValue;
                RadComboBox ddlFullterm = ((RadComboBox)e.Item.FindControl("ddlFullTermAdd"));
                string passportno = ((RadTextBox)e.Item.FindControl("txtpassportnoAdd")).Text;
                //string verifieddate = ((UserControlDate)_gridView.FooterRow.FindControl("ucVerifiedDateAdd")).Text;
                string verificationmethod = ((UserControlQuick)e.Item.FindControl("ucVerificationMethodAdd")).SelectedQuick;
                int declaration = (((RadCheckBox)e.Item.FindControl("chkTravelYN")).Checked.Equals(true)) ? 1 : 0;

                //string authenticateddate = ((UserControlDate)_gridView.FooterRow.FindControl("ucAuthenticatedDateAdd")).Text;
                //string authenticationyn = (((CheckBox)_gridView.FooterRow.FindControl("chkAuthenticatedYNAdd")).Checked) ? "1" : "0";
                //string crosscheckedyn = (((CheckBox)_gridView.FooterRow.FindControl("chkCrossCheckedYNAdd")).Checked) ? "1" : "0";

                if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry, country))
                {
                    ucError.Visible = true;
                    //   if (_gridView.EditIndex > -1)
                    {
                        //      _gridView.EditIndex = -1;
                        BindDataTRV();

                    }
                    return;
                }
                if (declaration==0)
                {
                    ucError.ErrorMessage = "Please check the Declaration Checkbox before you save.";
                    ucError.Visible = true;
                }
                else
                {
                    PhoenixCrewTravelDocument.InsertEmployeeTravelDocument(Convert.ToInt32(ViewState["empid"].ToString())
                                                            , Convert.ToInt32(documenttype)
                                                            , documentnumber
                                                            , Convert.ToDateTime(dateofissue)
                                                            , placeofissue
                                                            , General.GetNullableDateTime(dateofexpiry.Text)
                                                            , int.Parse(country)
                                                            , int.Parse(connectedtovessel)
                                                            , General.GetNullableDateTime(validfrom)
                                                            , General.GetNullableInteger(noofentry)
                                                            , null
                                                            , General.GetNullableString(passportno)
                                                            , 1
                                                            , General.GetNullableInteger(verificationmethod)
                                                            , 1
                                                            );
                    //    _gridView.SelectedIndex = -1;
                    BindDataTRV();
                    gvTravelDocument.Rebind();
                }
                // SetPageNavigatorTRV();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERTRV"] = null;
            }
            else if (e.CommandName.ToString().ToUpper() == "ARCHIVE")
            {
                string id = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;
                PhoenixCrewTravelDocument.ArchiveEmployeeTravelDocument(int.Parse(ViewState["empid"].ToString()), int.Parse(id), 0);
                //  _gridView.EditIndex = -1;
                //  _gridView.SelectedIndex = -1;
                BindDataTRV();
                // SetPageNavigatorTRV();
            }
            else if (e.CommandName.ToString().ToUpper() == "UNLOCKTRAVELDOC")
            {
                string id = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;
                PhoenixCrewTravelDocument.UnlockTravelDocument(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(id));
                BindDataTRV();
                // SetPageNavigatorTRV();
            }
            else if (e.CommandName.ToString().ToUpper() == "AUTHENTICATIONTRAVELDOC")
            {
                string documentid = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblTravelLockYN = (RadLabel)e.Item.FindControl("lblTravelLockYN");
                string surl = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEmployeeDocumentsAuthentication.aspx?DocType=TRAVEL&EmployeeId="
                    + ViewState["empid"].ToString()
                    + "&DocumentId=" + documentid.Trim()
                    + "&DtKey=" + (lblDTKey != null ? lblDTKey.Text.Trim() : "")
                    + "&LOCKYN=" + (lblTravelLockYN != null ? lblTravelLockYN.Text.Trim() : "")
                    + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Authenticate", surl, true);
            }
            else if (e.CommandName.ToString().ToUpper() == "CROSSCHECKTRAVELDOC")
            {
                string documentid = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblTravelLockYN = (RadLabel)e.Item.FindControl("lblTravelLockYN");
                string surl = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEmployeeDocumentsCrossCheck.aspx?DocType=TRAVEL&EmployeeId="
                    + ViewState["empid"].ToString()
                    + "&DocumentId=" + documentid.Trim()
                    + "&DtKey=" + (lblDTKey != null ? lblDTKey.Text.Trim() : "")
                    + "&LOCKYN=" + (lblTravelLockYN != null ? lblTravelLockYN.Text.Trim() : "")
                    + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CrossCheck", surl, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTravelDocument_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = e.RowIndex;
            string traveldocumentid = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;

            PhoenixCrewTravelDocument.DeleteEmployeeTravelDocument(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , Convert.ToInt32(traveldocumentid)
                                                                );
            BindDataTRV();
            //  SetPageNavigatorTRV();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelDocument_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            string documenttype = ((UserControlDocumentType)e.Item.FindControl("ucDocumentTypeEdit")).SelectedDocumentType;
            string documentnumber = ((RadTextBox)e.Item.FindControl("txtNumberEdit")).Text;
            string dateofissue = ((UserControlDate)e.Item.FindControl("ucDateOfIssueEdit")).Text;
            string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssue")).Text;
            UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("ucDateExpiryEdit"));
            string country = ((UserControlCountry)e.Item.FindControl("ddlCountryEdit")).SelectedCountry;
            string traveldocumentid = ((RadLabel)e.Item.FindControl("lbltraveldocumentidEdit")).Text;
            string connectedtovessel = ((RadCheckBox)e.Item.FindControl("chkConnectedToVesselEdit")).Checked == true ? "1" : "0";
            string validfrom = ((UserControlDate)e.Item.FindControl("ucValidFromEdit")).Text;
            string noofentry = ((RadComboBox)e.Item.FindControl("ddlNoofentryEdit")).SelectedValue;
            RadComboBox ddlFullterm = ((RadComboBox)e.Item.FindControl("ddlFullTermEdit"));
            string passportno = ((RadTextBox)e.Item.FindControl("txtpassportnoEdit")).Text;
            string verificationmethod = ((UserControlQuick)e.Item.FindControl("ucVerificationMethodEdit")).SelectedQuick;
            //string verifieddate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucVerifiedDateEdit")).Text;
            //string authenticatedate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucAuthenticatedDateEdit")).Text;
            //string authenticatedyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkAuthenticatedYNEdit")).Checked) ? "1" : "0";
            //string crosschekcedyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkCrossCheckedYNEdit")).Checked) ? "1" : "0";

            if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry, country))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewTravelDocument.UpdateEmployeeTravelDocument(Convert.ToInt32(ViewState["empid"].ToString())
                                                                , Convert.ToInt32(traveldocumentid)
                                                                , Convert.ToInt32(documenttype)
                                                                , documentnumber
                                                                , Convert.ToDateTime(dateofissue)
                                                                , placeofissue
                                                                , General.GetNullableDateTime(dateofexpiry.Text)
                                                                , int.Parse(country)
                                                                , int.Parse(connectedtovessel)
                                                                , General.GetNullableDateTime(validfrom)
                                                                , General.GetNullableInteger(noofentry)
                                                                , null
                                                                , General.GetNullableString(passportno)
                                                                , 1
                                                                , General.GetNullableInteger(verificationmethod)
                                                                );

            //  _gridView.EditIndex = -1;
            BindDataTRV();
            gvTravelDocument.Rebind();
            //  SetPageNavigatorTRV();
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
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            ImageButton cmdUnlock = (ImageButton)e.Item.FindControl("cmdUnlock");
            ImageButton cmdAuthenticate = (ImageButton)e.Item.FindControl("cmdAuthenticate");
            ImageButton cmdCrossCheck = (ImageButton)e.Item.FindControl("cmdCrossCheck");
            RadLabel lblAuthenticationRequYN = (RadLabel)e.Item.FindControl("lblAuthenticationRequYN");
            GridDataItem dataItem = (GridDataItem)e.Item;
          


            RadLabel lblisbyseafarer = (RadLabel)e.Item.FindControl("lblisbyseafarer");
            if (lblisbyseafarer.Text == "1")
            {
                RadLabel lblDocumentTypename = (RadLabel)e.Item.FindControl("lblDocumentTypename");
                lblDocumentTypename.Attributes.Add("style", "color:red !important;");

            }
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblNumber = (RadLabel)e.Item.FindControl("lblNumber");
            RadLabel lnkNumber = (RadLabel)e.Item.FindControl("lnkNumber");

            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
              && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {

                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
              

                if (att != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                    if (lblIsAtt.Text == string.Empty)
                    {
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                        att.Controls.Add(html);
                    }

                    att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&cmdname=TRAVELDOCUPLOAD'); return false;");
                }

                RadLabel lbl = (RadLabel)e.Item.FindControl("lbltraveldocumentid");
                HtmlImage img = (HtmlImage)e.Item.FindControl("imgRemarks");

                if (img != null)
                    img.Attributes.Add("onclick", "parent.openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "','xlarge')");
                RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
                if (lblR != null && string.IsNullOrEmpty(lblR.Text.Trim())) img.Src = Session["images"] + "/no-remarks.png";

                System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
                RadLabel expdate = e.Item.FindControl("lblDateofExpiry") as RadLabel;
                //DateTime? d = General.GetNullableDateTime(expdate.Text);
                DateTime? d = null;
                if (d.HasValue)
                {
                    TimeSpan t = d.Value - DateTime.Now;
                    if (t.Days >= 0 && t.Days < 120)
                    {
                        //e.Row.CssClass = "rowyellow";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days < 0)
                    {
                        //e.Row.CssClass = "rowred";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }

                //}
                else
                {
                    e.Item.Attributes["onclick"] = "";
                }
                UserControlCountry ucCountry = (UserControlCountry)e.Item.FindControl("ddlCountryEdit");
                if (ucCountry != null) ucCountry.SelectedCountry = drv["FLDCOUNTRYCODE"].ToString();

                UserControlDocumentType ucDocumentType = (UserControlDocumentType)e.Item.FindControl("ucDocumentTypeEdit");
                DataRowView drvDocumentType = (DataRowView)e.Item.DataItem;
                if (ucDocumentType != null) ucDocumentType.SelectedDocumentType = drv["FLDDOCUMENTTYPE"].ToString();

                RadComboBox ddlNoofentryEdit = (RadComboBox)e.Item.FindControl("ddlNoofentryEdit");
                DataRowView drvNoofentry = (DataRowView)e.Item.DataItem;
                if (ddlNoofentryEdit != null) ddlNoofentryEdit.SelectedValue = drvNoofentry["FLDNOOFENTRY"].ToString();

                RadComboBox ddlFullterm = (RadComboBox)e.Item.FindControl("ddlFullTermEdit");
                if (ddlFullterm != null) ddlFullterm.SelectedValue = drv["FLDFULLTERMYN"].ToString();

                UserControlQuick ucVerificationMethodEdit = (UserControlQuick)e.Item.FindControl("ucVerificationMethodEdit");
                if (ucVerificationMethodEdit != null)
                {
                    ucVerificationMethodEdit.QuickList = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , 131);
                    ucVerificationMethodEdit.DataBind();

                    ucVerificationMethodEdit.SelectedQuick = drv["FLDVERIFICATIONMETHOD"].ToString();
                }

                if (cmdAuthenticate != null && lblAuthenticationRequYN != null)
                {
                    cmdAuthenticate.Visible = lblAuthenticationRequYN.Text.Trim().Equals("1");
                }

                if (drv["FLDLOCKYN"].ToString().Equals("1"))
                {
                    if (cmdUnlock != null) cmdUnlock.Visible = true;
                    if (lblNumber != null) lblNumber.Visible = true;
                    if (lnkNumber != null) lnkNumber.Visible = false;
                    if (ed != null) ed.Visible = false;
                    if (db != null) db.Visible = false;
                    if (db1 != null) db1.Visible = false;
                    if (cmdAuthenticate != null) cmdAuthenticate.Visible = false;
                    if (cmdCrossCheck != null) cmdCrossCheck.Visible = false;
                }

                if (ed != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, ed.CommandName)) ed.Visible = false;
                }

                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }

                if (db1 != null)
                {
                    db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
                    if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
                }
                if (att != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
                }

                if (cmdUnlock != null)
                {
                    cmdUnlock.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure to unlock the document?')");
                    if (!SessionUtil.CanAccess(this.ViewState, cmdUnlock.CommandName)) cmdUnlock.Visible = false;
                }

                if (cmdCrossCheck != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdCrossCheck.CommandName)) cmdCrossCheck.Visible = false;
                if (cmdAuthenticate != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdAuthenticate.CommandName)) cmdAuthenticate.Visible = false;
            }

            if (e.Item is GridHeaderItem)
            {
                if (ViewState["SORTEXPRESSION"] != null)
                {
                    HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
                    if (img != null)
                    {
                        if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                            img.Src = Session["images"] + "/arrowUp.png";
                        else
                            img.Src = Session["images"] + "/arrowDown.png";

                        img.Visible = true;
                    }
                }
            }
            if (e.Item is GridFooterItem)
            {
                LinkButton ad = (LinkButton)e.Item.FindControl("cmdAdd");
                if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);

                UserControlQuick ucVerificationMethodAdd = (UserControlQuick)e.Item.FindControl("ucVerificationMethodAdd");
                if (ucVerificationMethodAdd != null)
                {
                    ucVerificationMethodAdd.QuickList = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , 131);
                    ucVerificationMethodAdd.DataBind();
                }
            }
        }
    }
    protected void gvTravelDocument_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBERTRV"] = ViewState["PAGENUMBERTRV"] != null ? ViewState["PAGENUMBERTRV"] : gvTravelDocument.CurrentPageIndex + 1;
            BindDataTRV();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvTravelDocument_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
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


    public void UpdateEmployeePassport()
    {
        if (ChkPassport.Checked == false)
        {
            ucError.ErrorMessage = "Please check the Declaration Checkbox before you save.";
            ucError.Visible = true;          
        }
        else
        {
            string pptno = RemoveSpecialCharacters(txtPassportnumberemp.Text);
            PhoenixCrewTravelDocument.UpdateEmployeeDetailConfirmationPassport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , Convert.ToInt32(ViewState["empid"].ToString())
                                                                , Convert.ToDateTime(ucDateOfIssueemp.Text)
                                                                , txtPlaceOfIssueemp.Text
                                                                , Convert.ToDateTime(ucDateOfExpiryemp.Text)
                                                                , General.GetNullableInteger(ucECNRemp.SelectedHard)
                                                                , General.GetNullableInteger(ucBlankPagesemp.SelectedHard)
                                                                , General.GetNullableString(pptno)

                                                                );
            SetEmployeePassportDetails();
            SetEmployeePassportDetailsBySeafarer();
            ucStatus.Text = "Passport, Seaman's Book & Visa information send to office";
        }
    }
    public void UpdateEmployeeSeamanBook()
    {

        try
        {
            if (ChkSeamandetails.Checked == false)
            {
                ucError.ErrorMessage = "Please check the Declaration Checkbox before you save.";
                ucError.Visible = true;
            }
            else
            {
                string cdcno = RemoveSpecialCharacters(txtSeamanBookNumberemp.Text);
                PhoenixCrewTravelDocument.UpdateEmployeeDetailConfirmSeamanBook(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , Convert.ToInt32(ViewState["empid"].ToString())
                                                                    , General.GetNullableInteger(ucSeamanCountryemp.SelectedFlag)
                                                                    , General.GetNullableDateTime(ucSeamanDateOfIssueemp.Text)
                                                                    , txtSeamanPlaceOfIssueemp.Text
                                                                    , General.GetNullableDateTime(ucSeamanDateOfExpiryemp.Text)
                                                                    , General.GetNullableString(cdcno)
                                                                                                                                   );
                SetEmployeeSeamanBookDetails();
                SetEmployeeSeamanBookDetailsBySeafarer();
                ucStatus.Text = "Passport, Seaman's Book & Visa information send to office";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    public bool IsValidPassport()
    {

        ucError.HeaderMessage = "Please provide the following required information";
        Int16 result;
        DateTime resultdate;

        if (txtPassportnumber.Text == "")
            ucError.ErrorMessage = "Passport Number is required";

        else if (!IsValidTextBox(txtPassportnumber.Text.Trim()))
            ucError.ErrorMessage = "Invalid Passport Number";

        if (ucDateOfIssue.Text == null)
            ucError.ErrorMessage = "Passport Date of Issue is required";
        else if (DateTime.TryParse(ucDateOfIssue.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Passport Date of Issue should be earlier than current date";
        }

        if (ucDateOfExpiry.Text == null)
            ucError.ErrorMessage = "Passport Date of Expiry is required";
        else if (!string.IsNullOrEmpty(ucDateOfIssue.Text)
            && DateTime.TryParse(ucDateOfExpiry.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(ucDateOfIssue.Text)) < 0)
        {
            ucError.ErrorMessage = "Passport Date of Expiry should be later than 'Date of Issue'";
        }

        if (txtPlaceOfIssue.Text == "")
            ucError.ErrorMessage = "Passport Place of Issue is required";

        if (ucECNR.SelectedHard.Equals("") || !Int16.TryParse(ucECNR.SelectedHard, out result))
            ucError.ErrorMessage = "Passport ECNR  is required";

        if (ucBlankPages.SelectedHard.Equals("") || !Int16.TryParse(ucBlankPages.SelectedHard, out result))
            ucError.ErrorMessage = " Passport Minimum 4 Blank pages  is required";

        return (!ucError.IsError);

    }
    public bool IsValidSeamanBook()
    {

        ucError.HeaderMessage = "Please provide the following required information";
        Int16 result;
        DateTime resultdate;

        if (txtSeamanBookNumber.Text == "")
            ucError.ErrorMessage = "Seaman's Book Number is required";

        else if (!IsValidTextBox(txtSeamanBookNumber.Text.Trim()))
            ucError.ErrorMessage = "Invalid Seaman Book Number";

        if (ucSeamanDateOfIssue.Text == null)
            ucError.ErrorMessage = "Seaman's Book Date of Issue is required";
        else if (DateTime.TryParse(ucSeamanDateOfIssue.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Seaman's Book Date of Issue should be earlier than current date";
        }

        if (ucSeamanDateOfExpiry.Text == null)
            ucError.ErrorMessage = "Seaman's Book Date of Expiry is required";
        else if (!string.IsNullOrEmpty(ucSeamanDateOfIssue.Text)
            && DateTime.TryParse(ucSeamanDateOfExpiry.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(ucSeamanDateOfIssue.Text)) < 0)
        {
            ucError.ErrorMessage = "Seaman's Book Date of Expiry should be later than 'Date of Issue'";
        }

        if (txtSeamanPlaceOfIssue.Text == "")
            ucError.ErrorMessage = "Seaman's Book Place of Issue is required";

        if (ucSeamanCountry.SelectedFlag.Equals("") || !Int16.TryParse(ucSeamanCountry.SelectedFlag, out result))
            ucError.ErrorMessage = "Seaman's Book Flag  is required";

        return (!ucError.IsError);

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

        if (dateofissue == null)
            ucError.ErrorMessage = "Date of Issue is required";
        else if (DateTime.TryParse(dateofissue, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Date of Issue should be earlier than current date";
        }

        if (placeofissue.Trim() == "")
            ucError.ErrorMessage = "Place of Issue is required";

        if (string.IsNullOrEmpty(dateofexpiry.Text) && dateofexpiry.CssClass == "input_mandatory")
            ucError.ErrorMessage = "Expiry Date is required";
        else if (!string.IsNullOrEmpty(dateofissue)
            && DateTime.TryParse(dateofexpiry.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(dateofissue)) < 0)
        {
            ucError.ErrorMessage = "Date of Expiry should be later than 'Date of Issue'";
        }
        if (country.Equals("") || !Int16.TryParse(country, out result))
            ucError.ErrorMessage = "Nationality/Flag  is required";

        return (!ucError.IsError);
    }

    public void OnClickPassportArchive(object sender, EventArgs e)
    {
        try
        {
            UpdateEmployeePassport();
            //PhoenixCrewTravelDocument.EmployeePassportArchive(int.Parse(ViewState["empid"].ToString()));
            //SetEmployeePassportDetails();
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
            UpdateEmployeeSeamanBook();
            //PhoenixCrewTravelDocument.EmployeeSeamanBookArchive(int.Parse(ViewState["empid"].ToString()));
            //SetEmployeeSeamanBookDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
            RadTextBox txtNumberAdd = (RadTextBox)footerItem.FindControl("txtNumberAdd");
            //txtNumberAdd.Focus();

        }
        else
        {
            GridDataItem row = ((GridDataItem)dc.Parent.Parent);
            date = (UserControlDate)row.FindControl("ucDateExpiryEdit");
            RadTextBox txtNumberEdit = (RadTextBox)row.FindControl("txtNumberEdit");
            txtNumberEdit.Focus();
        }
        if (ds.Tables[0].Rows[0]["FLDHAVINGEXPIRY"].ToString() == "1")
            date.CssClass = "input_mandatory";
        else
            date.CssClass = "input";
    }

    protected void ddlDocumentType1_TextChanged(object sender, EventArgs e)
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
            GridFooterItem footerItem = (GridFooterItem)gvOtherDocument.MasterTableView.GetItems(GridItemType.Footer)[0];

            date = (UserControlDate)footerItem.FindControl("ucDateExpiryAdd");
            RadTextBox txtNumberAdd = (RadTextBox)footerItem.FindControl("txtNumberAdd");
            txtNumberAdd.Focus();

        }
        else
        {
            GridDataItem row = ((GridDataItem)dc.Parent.Parent);
            date = (UserControlDate)row.FindControl("ucDateExpiryEdit");
            RadTextBox txtNumberEdit = (RadTextBox)row.FindControl("txtNumberEdit");
            txtNumberEdit.Focus();
        }
        if (ds.Tables[0].Rows[0]["FLDHAVINGEXPIRY"].ToString() == "1")
            date.CssClass = "input_mandatory";
        else
            date.CssClass = "input";
    }

    protected void ShowExcelTRV()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDOCUMENTNAME", "FLDDOCUMENTNUMBER", "FLDCATEGORYNAME", "FLDDATEOFISSUE", "FLDVALIDFROM", "FLDDATEOFEXPIRY", "FLDNOOFENTRYNAME", "FLDPLACEOFISSUE", "FLDCOUNTRYNAME", "FLDREMARKS",
                                 "FLDPASSPORTNO", "FLDDOCSTATUS", "FLDFULLTERM", "FLDAUTHENTICATEDYN", "FLDAUTHENTICATEDON","FLDAUTHENTICATEDBYNAME", "FLDCROSSCHECKEDYN", "FLDVERIFIEDDATE", "FLDVERIFIEDBY" };
        string[] alCaptions = { "Document Name", "Number", "Category", "Date of Issue", "Valid From", "Date of Expiry", "No of entry", "Place of Issue", "Nationality/Flag", "Remarks",
                                  "Passport No", "Doc Status", "Full Term YN", " Authenticated Y/N", "Authenticated Date", "Authenticated By","Cross checked Y/N","Cross checked Date","Cross checked by" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewTravelDocument.SearchEmployeeTravelDocument(Convert.ToInt32(ViewState["empid"].ToString()), null, 1,
                                                                              sortexpression, sortdirection,
                                                                            1,
                                                                           iRowCount,
                                                                            ref iRowCount,
                                                                            ref iTotalPageCount);
        General.ShowExcel("Crew Travel Document", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    public static bool IsValidTextBox(string text)
    {

        string regex = "^[0-9a-zA-Z ]+$";
        Regex re = new Regex(regex);
        if (!re.IsMatch(text))
            return (false);

        return true;
    }
    public static string RemoveSpecialCharacters(string str)
    {
        // Strips all special characters and spaces from a string.
        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton ib = (ImageButton)sender;

            ViewState["SORTEXPRESSIONTRV"] = ib.CommandName;
            ViewState["SORTDIRECTIONTRV"] = int.Parse(ib.CommandArgument);
            BindDataTRV();
            //SetPageNavigatorTRV();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    /* Licence */

    private void BindDataLIC()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDLICENCENAME", "FLDCATEGORYNAME", "FLDDOCUMENTTYPENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDAUTHENTICATEDYN", "FLDAUTHENTICATEDON",
                                     "FLDAUTHENTICATEDBYNAME", "FLDVERIFIEDYN", "FLDVERIFIEDON", "FLDVERIFIEDBYNAME", "FLDISSUEDBY", "FLDREMARKS" };
            string[] alCaptions = { "Licence", "Category", "Type", "Licence Number ", "Place Of Issue", "Issue Date", "Expiry Date", "Nationality", "Authenticated Y/N", "Authenticated Date",
                                      "Authenticated By", "Cross checked Y/N", "Cross checked date", "Cross checked by", "Issuing Authority", "Limitation Remarks" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTIONLIC"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONLIC"].ToString());

            DataSet ds = PhoenixCrewLicence.CrewLicenceSearch(
                        Int32.Parse(ViewState["empid"].ToString()), 0, 1
                        , sortexpression, sortdirection
                        , int.Parse(ViewState["PAGENUMBERLIC"].ToString())
                        , gvCrewLicence.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount
                        , General.GetNullableInteger(chkPendingAuthentication.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(chkPendingCrosscheck.Checked == true ? "1" : "0"));
            General.SetPrintOptions("gvCrewLicence", "Crew Licence", alCaptions, alColumns, ds);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            gvCrewLicence.DataSource = ds;
            gvCrewLicence.VirtualItemCount = iRowCount;
            //    gvCrewLicence.DataBind();
            //}
            //else
            //{
            //    DataTable dt = ds.Tables[0];
            //    ShowNoRecordsFound(dt, gvCrewLicence);
            //}

            ViewState["ROWCOUNTLIC"] = iRowCount;
            ViewState["TOTALPAGECOUNTLIC"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewLicence_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL") && Request.QueryString["e"] == "1")
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDLICENCENAME", "FLDCATEGORYNAME", "FLDDOCUMENTTYPENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDAUTHENTICATEDYN", "FLDAUTHENTICATEDON",
                                     "FLDAUTHENTICATEDBYNAME", "FLDVERIFIEDYN", "FLDVERIFIEDON", "FLDVERIFIEDBYNAME", "FLDISSUEDBY", "FLDREMARKS" };
                string[] alCaptions = { "Licence", "Category", "Type", "Licence Number ", "Place Of Issue", "Issue Date", "Expiry Date", "Nationality", "Authenticated Y/N", "Authenticated Date",
                                      "Authenticated By", "Cross checked Y/N", "Cross checked date", "Cross checked by", "Issuing Authority", "Limitation Remarks" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSIONLIC"] == null) ? null : (ViewState["SORTEXPRESSIONLIC"].ToString());

                if (ViewState["SORTDIRECTIONLIC"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTIONLIC"].ToString());
                DataSet ds = PhoenixCrewLicence.CrewLicenceSearch(
                       Int32.Parse(ViewState["empid"].ToString()), 0, 1
                       , sortexpression, sortdirection
                       , 1
                       , iRowCount
                       , ref iRowCount
                       , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Licence", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCrewLicence_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBERLIC"] = ViewState["PAGENUMBERLIC"] != null ? ViewState["PAGENUMBERLIC"] : gvCrewLicence.CurrentPageIndex + 1;
            BindDataLIC();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewLicence_ItemCommand(object sender, GridCommandEventArgs e)
    {


        try
        {

            if (e.CommandName.ToString().ToUpper() == "ADD")
            {


                string licence = ((UserControlDocuments)e.Item.FindControl("ddlLicenceAdd")).SelectedDocument;
                string licencenumber = ((RadTextBox)e.Item.FindControl("txtLicenceNumberAdd")).Text;
                UserControlDate dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateAdd"));
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueAdd")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateAdd"));
                //CheckBox nationallicence = _gridView.FooterRow.FindControl("chkNationalLicenceAdd") as CheckBox;
                UserControlCountry ddlFlag = ((UserControlCountry)e.Item.FindControl("ddlFlagAdd"));

                string remarks = ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text;
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuedByAdd")).Text;

                //string verifieddate = ((UserControlDate)_gridView.FooterRow.FindControl("ucVerifiedDateAdd")).Text;
                string verificationmethod = ((UserControlQuick)e.Item.FindControl("ucVerificationMethodAdd")).SelectedQuick;
                int declaration = (((RadCheckBox)e.Item.FindControl("chkLicenceYN")).Checked.Equals(true)) ? 1 : 0;

                //string authenticateddate = ((UserControlDate)_gridView.FooterRow.FindControl("ucAuthenticatedDateAdd")).Text;
                //string authenticationyn = (((CheckBox)_gridView.FooterRow.FindControl("chkAuthenticatedYNAdd")).Checked) ? "1" : "0";
                //string verifiedyn = (((CheckBox)_gridView.FooterRow.FindControl("chkCrossCheckedYNAdd")).Checked) ? "1" : "0";

                if (!IsValidLicence(licence, licencenumber, dateofissue, dateofexpiry, placeofissue, ddlFlag.SelectedCountry))
                {
                    ucError.Visible = true;
                    return;
                }

                if (declaration == 0)
                {
                    ucError.ErrorMessage = "Please check the Declaration Checkbox before you save.";
                    ucError.Visible = true;
                }
                else
                {
                    PhoenixCrewLicence.InsertOffshoreCrewLicence(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(ViewState["empid"].ToString())
                    , Convert.ToInt32(licence)
                    , licencenumber
                    , General.GetNullableDateTime(dateofissue.Text)
                    , placeofissue
                    , General.GetNullableDateTime(dateofexpiry.Text)
                    //, nationallicence.Checked ? byte.Parse("1") : byte.Parse("0")
                    , General.GetNullableInteger(ddlFlag.SelectedCountry)
                    , issuingauthority
                    , remarks
                    , 0
                    , null
                    , General.GetNullableInteger(verificationmethod)
                    , 1
                    );

                    BindDataLIC();
                    gvCrewLicence.Rebind();
                }
            }
            else if (e.CommandName.ToString().ToUpper() == "ARCHIVE")
            {
                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceId")).Text;
                PhoenixCrewLicence.ArchiveCrewLicence(int.Parse(licenceid), 0);

                BindDataLIC();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERLIC"] = null;
            }
            else if (e.CommandName.ToString().ToUpper() == "UNLOCKLICENCE")
            {
                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceId")).Text;
                PhoenixCrewLicence.UnlockLicence(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(licenceid));
                BindDataLIC();
                //  SetPageNavigatorLIC();
            }
            else if (e.CommandName.ToString().ToUpper() == "AUTHENTICATIONLICENCE")
            {
                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceId")).Text;
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblLicencelLockYN = (RadLabel)e.Item.FindControl("lblLicencelLockYN");
                string surl = "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEmployeeDocumentsAuthentication.aspx?DocType=LICENCE&EmployeeId="
                    + ViewState["empid"].ToString()
                    + "&DocumentId=" + licenceid.Trim()
                    + "&DtKey=" + (lblDTKey != null ? lblDTKey.Text.Trim() : "")
                    + "&LOCKYN=" + (lblLicencelLockYN != null ? lblLicencelLockYN.Text.Trim() : "")
                    + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Authenticate", surl, true);
            }
            else if (e.CommandName.ToString().ToUpper() == "CROSSCHECKLICENCE")
            {
                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceId")).Text;
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblLicenceLockYN = (RadLabel)e.Item.FindControl("lblLicenceLockYN");
                string surl = "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEmployeeDocumentsCrossCheck.aspx?DocType=LICENCE&EmployeeId="
                    + ViewState["empid"].ToString()
                    + "&DocumentId=" + licenceid.Trim()
                    + "&DtKey=" + (lblDTKey != null ? lblDTKey.Text.Trim() : "")
                    + "&LOCKYN=" + (lblLicenceLockYN != null ? lblLicenceLockYN.Text.Trim() : "")
                    + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CrossCheck", surl, true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvCrewLicence_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceId")).Text;

            PhoenixCrewLicence.DeleteCrewLicence(
               Convert.ToInt32(licenceid)
                , int.Parse(ViewState["empid"].ToString()));

            BindDataLIC();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    //protected void gvCrewLicence_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.SelectedIndex = e.NewSelectedIndex;
    //    _gridView.EditIndex = -1;
    //    BindDataLIC();
    //  //  SetPageNavigatorLIC();
    //}
    protected void gvCrewLicence_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            ImageButton cme = (ImageButton)e.Item.FindControl("cmdEdit");
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            ImageButton db1 = (ImageButton)e.Item.FindControl("cmdArchive");
            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            ImageButton cmdUnlock = (ImageButton)e.Item.FindControl("cmdUnlock");
            RadLabel lblNumber = (RadLabel)e.Item.FindControl("lblNumber");
            RadLabel lnkLicenceName = (RadLabel)e.Item.FindControl("lnkLicenceName");
            ImageButton cmdAuthenticate = (ImageButton)e.Item.FindControl("cmdAuthenticate");
            ImageButton cmdCrossCheck = (ImageButton)e.Item.FindControl("cmdCrossCheck");
            RadLabel lblAuthenticationRequYN = (RadLabel)e.Item.FindControl("lblAuthenticationRequYN");


            RadLabel lblisbyseafarer = (RadLabel)e.Item.FindControl("lblisbyseafarer");
            if (lblisbyseafarer.Text == "1")
            {
                RadLabel lblLicenceName = (RadLabel)e.Item.FindControl("lblLicenceName");
                lblLicenceName.Attributes.Add("style", "color:red !important;");

            }
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
               && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
                string mimetype = ".pdf";
                if (att != null)
                {
                    if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
                    if (drv["FLDLOCKYN"].ToString().Equals("1"))
                    {
                        att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&cmdname=LICENCEUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2&U=1'); return false;");
                    }
                    else
                    {
                        att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&cmdname=LICENCEUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2'); return false;");
                    }
                }
                RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
                System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
                //DateTime? d = General.GetNullableDateTime(expdate.Text);
                //if (d.HasValue)
                DateTime? d = null;
                if (expdate != null) d = General.GetNullableDateTime(expdate.Text);
                if (d.HasValue && imgFlag != null)
                {
                    TimeSpan t = d.Value - DateTime.Now;
                    if (t.Days >= 0 && t.Days < 120)
                    {
                        //e.Row.CssClass = "rowyellow";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days < 0)
                    {
                        //e.Row.CssClass = "rowred";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }

                RadLabel lbl = (RadLabel)e.Item.FindControl("lblLicenceId");
                HtmlImage img = (HtmlImage)e.Item.FindControl("imgRemarks");
                if (img != null)
                {
                    img.Attributes.Add("onclick", "parent.openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.LICENCE + "','xlarge')");
                }
                RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
                if (lblR != null && string.IsNullOrEmpty(lblR.Text.Trim())) img.Src = Session["images"] + "/no-remarks.png";
            }
            else
            {
                e.Item.Attributes["onclick"] = "";
            }
            UserControlDocuments ucDocuments = (UserControlDocuments)e.Item.FindControl("ddlLicenceEdit");
            if (ucDocuments != null) ucDocuments.SelectedDocument = drv["FLDLICENCE"].ToString();

            UserControlCountry ucFlag = (UserControlCountry)e.Item.FindControl("ddlFlagEdit");
            if (ucFlag != null) ucFlag.SelectedCountry = drv["FLDFLAGID"].ToString();

            UserControlQuick ucVerificationMethodEdit = (UserControlQuick)e.Item.FindControl("ucVerificationMethodEdit");
            if (ucVerificationMethodEdit != null)
            {
                ucVerificationMethodEdit.QuickList = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , 131);
                ucVerificationMethodEdit.DataBind();

                ucVerificationMethodEdit.SelectedQuick = drv["FLDVERIFICATIONMETHOD"].ToString();
            }

            if (cmdAuthenticate != null && lblAuthenticationRequYN != null)
            {
                cmdAuthenticate.Visible = lblAuthenticationRequYN.Text.Trim().Equals("1");
            }

            if (drv["FLDLOCKYN"].ToString().Equals("1"))
            {
                if (cmdUnlock != null) cmdUnlock.Visible = true;
                if (lblNumber != null) lblNumber.Visible = true;
                if (lnkLicenceName != null) lnkLicenceName.Visible = false;
                if (cme != null) cme.Visible = false;
                if (db != null) db.Visible = false;
                if (db1 != null) db1.Visible = false;
                if (cmdAuthenticate != null) cmdAuthenticate.Visible = false;
                if (cmdCrossCheck != null) cmdCrossCheck.Visible = false;
            }

            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
            }

            if (cme != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
            }

            if (cmdUnlock != null)
            {
                cmdUnlock.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure to unlock?')");
                if (!SessionUtil.CanAccess(this.ViewState, cmdUnlock.CommandName)) cmdUnlock.Visible = false;
            }

            if (cmdCrossCheck != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdCrossCheck.CommandName)) cmdCrossCheck.Visible = false;
            if (cmdAuthenticate != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdAuthenticate.CommandName)) cmdAuthenticate.Visible = false;

        }
        if (e.Item is GridFooterItem)
        {
            LinkButton imgbt = (LinkButton)e.Item.FindControl("cmdAdd");
            if (!SessionUtil.CanAccess(this.ViewState, imgbt.CommandName)) imgbt.Visible = false;

            UserControlQuick ucVerificationMethodAdd = (UserControlQuick)e.Item.FindControl("ucVerificationMethodAdd");
            if (ucVerificationMethodAdd != null)
            {
                ucVerificationMethodAdd.QuickList = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , 131);
                ucVerificationMethodAdd.DataBind();
            }

        }
    }
    protected void gvCrewLicence_RowUpdating(object sender, GridCommandEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            try
            {
                string licence = ((UserControlDocuments)e.Item.FindControl("ddlLicenceEdit")).SelectedDocument;
                string licencenumber = ((RadTextBox)e.Item.FindControl("txtLicenceNumberEdit")).Text;
                UserControlDate dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit"));
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit"));
                string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceIdEdit")).Text;
                //CheckBox nationallicence = _gridView.Rows[nCurrentRow].FindControl("chkNationalLicenceEdit") as CheckBox;
                UserControlCountry ddlFlag = ((UserControlCountry)e.Item.FindControl("ddlFlagEdit"));

                //string verifieddate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucVerifiedDateEdit")).Text;

                string verificationmethod = ((UserControlQuick)e.Item.FindControl("ucVerificationMethodEdit")).SelectedQuick;
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuedByEdit")).Text;
                //string authenticatedate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucAuthenticatedDateEdit")).Text;
                //string authenticatedyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkAuthenticatedYNEdit")).Checked)? "1":"0";
                //string verifiedyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkCrossCheckedYNEdit")).Checked) ? "1" : "0";

                if (!IsValidLicence(licence, licencenumber, dateofissue, dateofexpiry, placeofissue, ddlFlag.SelectedCountry))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewLicence.UpdateCrewLicence(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(ViewState["empid"].ToString())
                    , Convert.ToInt32(licenceid)
                    , Convert.ToInt32(licence)
                    , licencenumber
                    , General.GetNullableDateTime(dateofissue.Text)
                    , placeofissue
                    , General.GetNullableDateTime(dateofexpiry.Text)
                    //, nationallicence.Checked ? byte.Parse("1") : byte.Parse("0")
                    , General.GetNullableInteger(ddlFlag.SelectedCountry)
                    , issuingauthority
                    , 0
                    , null
                    , General.GetNullableInteger(verificationmethod)
                   );
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }

            BindDataLIC();
            gvCrewLicence.Rebind();
            //  SetPageNavigatorLIC();
        }
    }


    private bool IsValidLicence(string licence, string licencenumber, UserControlDate dateofissue, UserControlDate dateofexpiry, string placeofissue, string country)
    {
        int resultInt;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!int.TryParse(licence, out resultInt))
            ucError.ErrorMessage = "Licence is required";

        if (licencenumber.Trim() == "")
            ucError.ErrorMessage = "Licence Number is required";

        if (placeofissue.Trim() == "")
            ucError.ErrorMessage = "Place of Issue is required";

        if (string.IsNullOrEmpty(dateofissue.Text) && dateofissue.CssClass == "input_mandatory")
            ucError.ErrorMessage = "Issue Date is required.";
        else if (DateTime.TryParse(dateofissue.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(dateofexpiry.Text) && dateofexpiry.CssClass == "input_mandatory")
            ucError.ErrorMessage = "Expiry Date is required.";

        if (dateofissue.Text != null && dateofexpiry.Text != null)
        {
            if ((DateTime.TryParse(dateofissue.Text, out resultDate)) && (DateTime.TryParse(dateofexpiry.Text, out resultDate)))
                if ((DateTime.Parse(dateofissue.Text)) >= (DateTime.Parse(dateofexpiry.Text)))
                    ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'";
        }
        if (!int.TryParse(country, out resultInt))
            ucError.ErrorMessage = "Nationality is required";

        return (!ucError.IsError);
    }


    protected void ddlDocumentLIC_TextChanged(object sender, EventArgs e)
    {
        UserControlDocuments dc = (UserControlDocuments)sender;
        DataSet ds = new DataSet();
        UserControlDate issuedate;
        UserControlDate expirydate;
        if (dc.SelectedDocument != "Dummy")
        {
            ds = PhoenixRegistersDocumentLicence.EditDocumentLicence(General.GetNullableInteger(dc.SelectedDocument).Value);
        }
        if (ds.Tables.Count > 0)
        {
            if (dc.ID == "ddlLicenceAdd")
            {
                GridFooterItem footerItem = (GridFooterItem)gvCrewLicence.MasterTableView.GetItems(GridItemType.Footer)[0];

                issuedate = (UserControlDate)footerItem.FindControl("txtIssueDateAdd");
                expirydate = (UserControlDate)footerItem.FindControl("txtExpiryDateAdd");

            }
            else
            {

                GridDataItem row = ((GridDataItem)dc.Parent.Parent);

                issuedate = (UserControlDate)row.FindControl("txtIssueDateEdit");
                expirydate = (UserControlDate)row.FindControl("txtExpiryDateEdit");
            }
            if (ds.Tables[0].Rows[0]["FLDEXPIRY"].ToString() == "1")
            {
                //issuedate = (UserControlDate)gvCrewLicence.FindControl("txtIssueDateEdit");
                //expirydate = (UserControlDate)gvCrewLicence.FindControl("txtExpiryDateEdit");
                issuedate.CssClass = "input_mandatory";
                expirydate.CssClass = "input_mandatory";
            }
            else
            {
                //issuedate = (UserControlDate)gvCrewLicence.FindControl("txtIssueDateEdit");
                //expirydate = (UserControlDate)gvCrewLicence.FindControl("txtExpiryDateEdit");
                issuedate.CssClass = "input";
                expirydate.CssClass = "input";
            }
        }
    }

    /* Medical */

    protected void CrewMedical_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("MEDICALEXCEL"))
            {
                string[] alColumns = { "FLDMEDICALNAME", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE", "FLDCOUNTRYNAME", "FLDSTATUSNAME", "FLDAUTHENTICATEDON", "FLDAUTHENTICATEDBYNAME",
                                     "FLDDOCTORNAME", "FLDVERIFIEDDATE", "FLDVERIFIEDBY", "FLDVRIMETHODNAME" };
                string[] alCaptions = { "Medical", "Place Of Issue", "Issue Date", "Expiry Date", "Flag", "Status", "Authenticated Date", "Authenticated by", "Doctor Name",
                                      "Cross checked Date", "Cross checked by", "Verification Method" };

                DataSet ds = PhoenixCrewMedicalDocuments.ListCrewFlagMedical(
                        Int32.Parse(ViewState["empid"].ToString()), 1, null);

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Crew Medical", ds.Tables[0], alColumns, alCaptions, null, null);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewMedicalTest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("MEDICALTESTEXCEL"))
            {
                string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE", "FLDSTATUSNAME", "FLDREMARKS", "FLDAUTHENTICATEDYN", "FLDDOCTORNAME", "FLDAUTHENTICATEDON", "FLDAUTHENTICATEDBYNAME",
                                 "FLDVERIFIEDYN", "FLDVERIFIEDDATE", "FLDVERIFIEDBY" };
                string[] alCaptions = { "Medical Test", "Place Of Issue", "Issue Date", "Expiry Date", "Status", "Remarks", "Authenticated Y/N", "Doctor Name", "Authenticated Date", "Authenticated By",
                                  "Cross checked Y/N", "Cross checked Date", "Cross checked by"};

                DataSet ds = PhoenixCrewMedicalDocuments.ListCrewMedicalTest(
                        Int32.Parse(ViewState["empid"].ToString()), 1, null);

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Crew Medical Test", ds.Tables[0], alColumns, alCaptions, null, null);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindMedicalData()
    {
        try
        {
            string[] alColumns = { "FLDMEDICALNAME", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE", "FLDCOUNTRYNAME", "FLDSTATUSNAME", "FLDAUTHENTICATEDON", "FLDAUTHENTICATEDBYNAME",
                                     "FLDDOCTORNAME", "FLDVERIFIEDDATE", "FLDVERIFIEDBY", "FLDVRIMETHODNAME" };
            string[] alCaptions = { "Medical", "Place Of Issue", "Issue Date", "Expiry Date", "Flag", "Status", "Authenticated Date", "Authenticated by", "Doctor Name",
                                      "Cross checked Date", "Cross checked by", "Verification Method" };

            DataSet ds = PhoenixCrewMedicalDocuments.ListCrewFlagMedical(
                        Int32.Parse(ViewState["empid"].ToString()), 1, null
                        , General.GetNullableInteger(chkPendingAuthentication.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(chkPendingCrosscheck.Checked == true ? "1" : "0"));

            General.SetPrintOptions("gvCrewMedical", "Crew Medical", alCaptions, alColumns, ds);


            gvCrewMedical.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindMedicalTestData()
    {
        try
        {
            string[] alColumns = { "FLDNAMEOFMEDICAL", "FLDPLACEOFISSUE", "FLDISSUEDDATE", "FLDEXPIRYDATE", "FLDSTATUSNAME", "FLDREMARKS", "FLDAUTHENTICATEDYN", "FLDDOCTORNAME", "FLDAUTHENTICATEDON", "FLDAUTHENTICATEDBYNAME",
                                 "FLDVERIFIEDYN", "FLDVERIFIEDDATE", "FLDVERIFIEDBY" };
            string[] alCaptions = { "Medical Test", "Place Of Issue", "Issue Date", "Expiry Date", "Status", "Remarks", "Authenticated Y/N", "Doctor Name", "Authenticated Date", "Authenticated By",
                                  "Cross checked Y/N", "Cross checked Date", "Cross checked by"};

            DataSet ds = PhoenixCrewMedicalDocuments.ListCrewMedicalTest(
                        Int32.Parse(ViewState["empid"].ToString()), 1, null
                        , General.GetNullableInteger(chkPendingAuthentication.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(chkPendingCrosscheck.Checked == true ? "1" : "0"));

            General.SetPrintOptions("gvMedicalTest", "Crew Medical Test", alCaptions, alColumns, ds);

            gvMedicalTest.DataSource = ds;


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void AutoArchive()
    {
        PhoenixCrewMedicalDocuments.AutoArchiveCewMedical(Int32.Parse(ViewState["empid"].ToString()));
    }
    private void AutoCorrectMedicalStatus()
    {
        PhoenixCrewMedicalDocuments.AutoCorrectCrewMedicalStatus(Int32.Parse(ViewState["empid"].ToString()), null);
    }
    protected void SetCourseRequest(ref int? vesselid, int? lftreqyn)
    {
        DataSet ds = PhoenixCrewMedicalDocuments.ListCrewCourseRequest(Int32.Parse(ViewState["empid"].ToString()), lftreqyn);

        if (ds.Tables[0].Rows[0]["FLDVESSELID"].ToString() != "")
        {
            vesselid = Convert.ToInt32(ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());

        }
    }
    protected void gvCrewMedical_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            BindMedicalData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvCrewMedical_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //GridView _gridView = (GridView)sender;
        //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        try
        {
            if (e.CommandName.ToString().ToUpper() == "MEDICALARCHIVE")
            {
                string crewflagmedicalid = ((RadLabel)e.Item.FindControl("lblMedicalId")).Text;
                PhoenixCrewMedicalDocuments.ArchiveCrewMedical(int.Parse(crewflagmedicalid), 0);
                //  _gridView.EditIndex = -1;
                //   _gridView.SelectedIndex = -1;
                BindMedicalData();

            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToString().ToUpper() == "EDIT")
            {
                // _gridView.SelectedIndex = nCurrentRow;
                // _gridView.EditIndex = nCurrentRow;
                BindMedicalData();
                DisableFlag((UserControlHard)e.Item.FindControl("ucMedicalEdit"), null);
            }
            else if (e.CommandName.ToString().ToUpper() == "MEDICALDELETE")
            {
                string medicalid = ((RadLabel)e.Item.FindControl("lblMedicalId")).Text;

                PhoenixCrewMedicalDocuments.DeleteCrewFlagMedical(
                    Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                    , Convert.ToInt32(medicalid));
                BindMedicalData();
            }
            else if (e.CommandName.ToString().ToUpper() == "CANCEL")
            {
                //  _gridView.EditIndex = -1;
                BindMedicalData();
            }
            else if (e.CommandName.ToString().ToUpper() == "UNLOCKMEDICAL")
            {
                string flagmedicalid = ((RadLabel)e.Item.FindControl("lblMedicalId")).Text;
                PhoenixCrewMedicalDocuments.UnlockMedical(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(flagmedicalid));
                BindMedicalData();
            }
            else if (e.CommandName.ToString().ToUpper() == "AUTHENTICATIONMEDICAL")
            {
                string flagmedicalid = ((RadLabel)e.Item.FindControl("lblMedicalId")).Text;
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblMedicalLockYN = (RadLabel)e.Item.FindControl("lblMedicalLockYN");
                string surl = "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEmployeeDocumentsAuthentication.aspx?DocType=MEDICAL&EmployeeId="
                    + ViewState["empid"].ToString()
                    + "&DocumentId=" + flagmedicalid.Trim()
                    + "&DtKey=" + (lblDTKey != null ? lblDTKey.Text.Trim() : "")
                    + "&LOCKYN=" + (lblMedicalLockYN != null ? lblMedicalLockYN.Text.Trim() : "")
                    + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Authenticate", surl, true);
            }
            else if (e.CommandName.ToString().ToUpper() == "CROSSCHECKMEDICAL")
            {
                string flagmedicalid = ((RadLabel)e.Item.FindControl("lblMedicalId")).Text;
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblMedicalLockYN = (RadLabel)e.Item.FindControl("lblMedicalLockYN");
                string surl = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEmployeeDocumentsCrossCheck.aspx?DocType=MEDICAL&EmployeeId="
                    + ViewState["empid"].ToString()
                    + "&DocumentId=" + flagmedicalid.Trim()
                    + "&DtKey=" + (lblDTKey != null ? lblDTKey.Text.Trim() : "")
                    + "&LOCKYN=" + (lblMedicalLockYN != null ? lblMedicalLockYN.Text.Trim() : "")
                    + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CrossCheck", surl, true);
            }
        }

        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }
    protected void gvCrewMedical_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = e.RowIndex;
            string medicalid = ((RadLabel)e.Item.FindControl("lblMedicalId")).Text;

            PhoenixCrewMedicalDocuments.DeleteCrewFlagMedical(
                Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                , Convert.ToInt32(medicalid)
            );
            BindMedicalData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMedicalTest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            BindMedicalTestData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewMedical_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            ImageButton cmdAuthenticate = (ImageButton)e.Item.FindControl("cmdAuthenticate");
            ImageButton cmdCrossCheck = (ImageButton)e.Item.FindControl("cmdCrossCheck");
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            ImageButton db1 = (ImageButton)e.Item.FindControl("cmdArchive");
            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            ImageButton dbedit = (ImageButton)e.Item.FindControl("cmdEdit");
            ImageButton cmdUnlock = (ImageButton)e.Item.FindControl("cmdUnlock");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            GridDataItem dataItem = (GridDataItem)e.Item;
           
            RadLabel lblisbyseafarer = (RadLabel)e.Item.FindControl("lblisbyseafarer");
            if (lblisbyseafarer.Text == "1")
            {
                RadLabel lblMedicalName = (RadLabel)e.Item.FindControl("lblMedicalName");
                lblMedicalName.Attributes.Add("style", "color:red !important;");

            }
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
                && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
                string mimetype = ".pdf";
                if (att != null)
                {
                    if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
                    if (drv["FLDLOCKYN"].ToString().Equals("1"))
                    {
                        att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=MEDICALUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2&U=1'); return false;");
                    }
                    else
                    {
                        att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=MEDICALUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2'); return false;");
                    }
                }

                RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
                System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
                DateTime? d = General.GetNullableDateTime(expdate.Text);
                if (d.HasValue)
                {
                    TimeSpan t = d.Value - DateTime.Now;
                    if (t.Days >= 0 && t.Days < 120)
                    {
                        //e.Row.CssClass = "rowyellow";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days < 0)
                    {
                        //e.Row.CssClass = "rowred";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }
                RadLabel l = (RadLabel)e.Item.FindControl("lblMedicalId");
                if (dbedit != null)
                    dbedit.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Crew/CrewMedicalAdd.aspx?empid=" + ViewState["empid"].ToString() + "&CREWFLAGMEDICALID=" + l.Text + "');return false;");
                //  }
                else
                {
                    e.Item.Attributes["onclick"] = "";
                }

                UserControlFlag ucFlag = (UserControlFlag)e.Item.FindControl("ucFlagEdit");
                if (ucFlag != null) ucFlag.SelectedFlag = drv["FLDFLAGID"].ToString();

                UserControlHard ucHard = (UserControlHard)e.Item.FindControl("ucMedicalEdit");
                if (ucHard != null) ucHard.SelectedHard = drv["FLDMEDICALID"].ToString();

                UserControlHard ucStatus = (UserControlHard)e.Item.FindControl("ddlStatus");
                if (ucStatus != null)
                {
                    ucStatus.SelectedHard = drv["FLDSTATUS"].ToString();
                    DataSet ds = PhoenixRegistersHard.EditHardCode(1, 95, "FLM");
                    if (drv["FLDMEDICALID"].ToString() == ds.Tables[0].Rows[0]["FLDHARDCODE"].ToString())
                    {
                        ucStatus.CssClass = "input";
                    }
                    else
                    {
                        ucStatus.CssClass = "input_mandatory";
                    }

                }

                if (drv["FLDLOCKYN"].ToString().Equals("1"))
                {
                    if (cmdUnlock != null) cmdUnlock.Visible = true;
                    if (dbedit != null) dbedit.Visible = false;
                    if (db != null) db.Visible = false;
                    if (db1 != null) db1.Visible = false;
                    if (cmdAuthenticate != null) cmdAuthenticate.Visible = false;
                    if (cmdCrossCheck != null) cmdCrossCheck.Visible = false;
                }

                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }

                if (db1 != null)
                {
                    db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
                    if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
                }

                if (att != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
                }

                if (dbedit != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, dbedit.CommandName)) dbedit.Visible = false;
                }

                if (cmdUnlock != null)
                {
                    cmdUnlock.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure to unlock?')");
                    if (!SessionUtil.CanAccess(this.ViewState, cmdUnlock.CommandName)) cmdUnlock.Visible = false;
                }

                if (cmdCrossCheck != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdCrossCheck.CommandName)) cmdCrossCheck.Visible = false;
                if (cmdAuthenticate != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdAuthenticate.CommandName)) cmdAuthenticate.Visible = false;
            }
            if (e.Item is GridFooterItem)
            {
                LinkButton imgbt = (LinkButton)e.Item.FindControl("cmdAdd");
                if (!SessionUtil.CanAccess(this.ViewState, imgbt.CommandName)) imgbt.Visible = false;

            }
        }
    }
    //protected void gvCrewMedical_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.SelectedIndex = e.NewSelectedIndex;
    //    _gridView.EditIndex = -1;
    //    BindMedicalData();
    //}


    protected void gvMedicalTest_ItemCommand(object sender, GridCommandEventArgs e)
    {

        try
        {

            if (e.CommandName.ToString().ToUpper() == "MEDICALTESTADD")
            {

                //  _gridView.EditIndex = -1;

                string medicaltestid = ((UserControlDocuments)e.Item.FindControl("ucMedicalTestAdd")).SelectedDocument;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateAdd")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueAdd")).Text;
                string dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateAdd")).Text;
                string status = ((UserControlHard)e.Item.FindControl("ddlStatusAdd")).SelectedHard;
                string remarks = ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text;
                //string verifieddate = ((UserControlDate)_gridView.FooterRow.FindControl("ucVerifiedDateAdd")).Text;

                string verificationmethod = ((UserControlQuick)e.Item.FindControl("ucVerificationMethodAdd")).SelectedQuick;
                int declaration = (((RadCheckBox)e.Item.FindControl("chkMedicalTestYN")).Checked.Equals(true)) ? 1 : 0;
                //string authenticateddate = ((UserControlDate)_gridView.FooterRow.FindControl("ucAuthenticatedDateAdd")).Text;
                //string authenticationyn = (((CheckBox)_gridView.FooterRow.FindControl("chkAuthenticatedYNAdd")).Checked) ? "1" : "0";
                //string verifiedyn = (((CheckBox)_gridView.FooterRow.FindControl("chkCrossCheckedYNAdd")).Checked) ? "1" : "0";
                //string doctorid = ((DropDownList)_gridView.FooterRow.FindControl("ddlDoctorAdd")).SelectedValue;

                if (!IsValidMedicalTest(medicaltestid, dateofissue, dateofexpiry, status, remarks))
                {
                    ucError.Visible = true;
                    return;
                }
                if (declaration == 0)
                {
                    ucError.ErrorMessage = "Please check the Declaration Checkbox before you save.";
                    ucError.Visible = true;
                }
                else
                {
                    PhoenixCrewMedicalDocuments.InsertOffshoreCrewMedicalTest(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(ViewState["empid"].ToString())
                    , Convert.ToInt32(medicaltestid)
                    , placeofissue
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry)
                    , General.GetNullableInteger(status)
                    , General.GetNullableString(remarks)
                    , General.GetNullableInteger(verificationmethod)
                    , 1
                    );

                    BindMedicalTestData();
                    gvMedicalTest.Rebind();
                }
            }

            else if (e.CommandName.ToString().ToUpper() == "MEDICALTESTARCHIVE")
            {
                string medicaltestid = ((RadLabel)e.Item.FindControl("lblMedicalTestId")).Text;
                PhoenixCrewMedicalDocuments.ArchiveCrewMedicalTest(int.Parse(medicaltestid), 0);

                BindMedicalTestData();
                gvMedicalTest.Rebind();
            }

            else if (e.CommandName.ToString().ToUpper() == "MEDICALTESTDELETE")
            {
                string medicaltestid = ((RadLabel)e.Item.FindControl("lblMedicalTestId")).Text;

                PhoenixCrewMedicalDocuments.DeleteCrewMedicalTest(
                    Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                    , Convert.ToInt32(medicaltestid)
                                                                );
                BindMedicalTestData();
                gvMedicalTest.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {
                try
                {
                    string medicaltestid = ((UserControlDocuments)e.Item.FindControl("ucMedicalTestEdit")).SelectedDocument;
                    string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit")).Text;
                    string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
                    string dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit")).Text;
                    string crewmedicaltestid = ((RadLabel)e.Item.FindControl("lblMedicalTestIdEdit")).Text;
                    string status = ((UserControlHard)e.Item.FindControl("ddlStatusEdit")).SelectedHard;
                    string remarks = ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text;
                    //string verifieddate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucVerifiedDateEdit")).Text;
                    string verificationmethod = ((UserControlQuick)e.Item.FindControl("ucVerificationMethodEdit")).SelectedQuick;
                    //string authenticatedate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucAuthenticatedDateEdit")).Text;
                    //string authenticatedyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkAuthenticatedYNEdit")).Checked) ? "1" : "0";
                    //string verifiedyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkCrossCheckedYNEdit")).Checked) ? "1" : "0";
                    //string doctorid = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlDoctorEdit")).SelectedValue;
                    if (!IsValidMedicalTest(medicaltestid, dateofissue, dateofexpiry, status, remarks))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixCrewMedicalDocuments.UpdateCrewMedicalTest(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , Convert.ToInt32(crewmedicaltestid)
                        , Convert.ToInt32(ViewState["empid"].ToString())
                        , Convert.ToInt32(medicaltestid)
                        , placeofissue
                        , General.GetNullableDateTime(dateofissue)
                        , General.GetNullableDateTime(dateofexpiry)
                        , General.GetNullableInteger(status)
                        , General.GetNullableString(remarks)
                        , General.GetNullableInteger(verificationmethod)
                        );

                    // _gridView.EditIndex = -1;
                    BindMedicalTestData();
                    gvMedicalTest.Rebind();
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "Please make the required correction";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            else if (e.CommandName.ToString().ToUpper() == "CANCEL")
            {
                //  _gridView.EditIndex = -1;
                BindMedicalTestData();
            }
            else if (e.CommandName.ToString().ToUpper() == "UNLOCKMEDICALTEST")
            {
                string medicaltestid = ((RadLabel)e.Item.FindControl("lblMedicalTestId")).Text;
                PhoenixCrewMedicalDocuments.UnlockMedicalTest(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(medicaltestid));
                BindMedicalTestData();
            }
            else if (e.CommandName.ToString().ToUpper() == "AUTHENTICATIONMEDICALTEST")
            {
                string medicaltestid = ((RadLabel)e.Item.FindControl("lblMedicalTestId")).Text;
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblMedicalTestLockYN = (RadLabel)e.Item.FindControl("lblMedicalTestLockYN");
                string surl = "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEmployeeDocumentsAuthentication.aspx?DocType=MEDICALTEST&EmployeeId="
                    + ViewState["empid"].ToString()
                    + "&DocumentId=" + medicaltestid.Trim()
                    + "&DtKey=" + (lblDTKey != null ? lblDTKey.Text.Trim() : "")
                    + "&LOCKYN=" + (lblMedicalTestLockYN != null ? lblMedicalTestLockYN.Text.Trim() : "")
                    + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Authenticate", surl, true);
            }
            else if (e.CommandName.ToString().ToUpper() == "CROSSCHECKMEDICALTEST")
            {
                string medicaltestid = ((RadLabel)e.Item.FindControl("lblMedicalTestId")).Text;
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblMedicalTestLockYN = (RadLabel)e.Item.FindControl("lblMedicalTestLockYN");
                string surl = "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEmployeeDocumentsCrossCheck.aspx?DocType=MEDICALTEST&EmployeeId="
                    + ViewState["empid"].ToString()
                    + "&DocumentId=" + medicaltestid.Trim()
                    + "&DtKey=" + (lblDTKey != null ? lblDTKey.Text.Trim() : "")
                    + "&LOCKYN=" + (lblMedicalTestLockYN != null ? lblMedicalTestLockYN.Text.Trim() : "")
                    + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CrossCheck", surl, true);
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvMedicalTest_DeleteCommand(object sender, GridCommandEventArgs e)
    //{
    //    try
    //    {
    //        //GridView _gridView = (GridView)sender;
    //        //int nCurrentRow = e.RowIndex;
    //        string medicaltestid = ((RadLabel)e.Item.FindControl("lblMedicalTestId")).Text;

    //        PhoenixCrewMedicalDocuments.DeleteCrewMedicalTest(
    //            Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
    //            , Convert.ToInt32(medicaltestid)
    //                                                        );
    //        BindMedicalTestData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;

    //    }
    //}

    protected void gvMedicalTest_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            ImageButton cme = (ImageButton)e.Item.FindControl("cmdEdit");
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            ImageButton db1 = (ImageButton)e.Item.FindControl("cmdArchive");
            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            ImageButton cmdUnlock = (ImageButton)e.Item.FindControl("cmdUnlock");
            ImageButton cmdAuthenticate = (ImageButton)e.Item.FindControl("cmdAuthenticate");
            ImageButton cmdCrossCheck = (ImageButton)e.Item.FindControl("cmdCrossCheck");
            RadLabel lblAuthenticationRequYN = (RadLabel)e.Item.FindControl("lblAuthenticationRequYN");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            GridDataItem dataItem = (GridDataItem)e.Item;

            RadLabel lblisbyseafarer = (RadLabel)e.Item.FindControl("lblisbyseafarer");
            if (lblisbyseafarer.Text == "1")
            {
                RadLabel lblMedicalTestName = (RadLabel)e.Item.FindControl("lblMedicalTestName");
                lblMedicalTestName.Attributes.Add("style", "color:red !important;");

            }

            if (e.Item is GridEditableItem)
            {
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
                string mimetype = ".pdf";
                if (att != null)
                {
                    if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
                    if (drv["FLDLOCKYN"].ToString().Equals("1"))
                    {
                        att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=MEDICALTESTUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2&U=1'); return false;");
                    }
                    else
                    {
                        att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=MEDICALTESTUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2'); return false;");
                    }
                }

                RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
                System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
                DateTime? d = null;
                if (expdate != null) d = General.GetNullableDateTime(expdate.Text);
                if (d.HasValue && imgFlag != null)
                {
                    TimeSpan t = d.Value - DateTime.Now;
                    if (t.Days >= 0 && t.Days < 120)
                    {
                        //e.Row.CssClass = "rowyellow";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days < 0)
                    {
                        //e.Row.CssClass = "rowred";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }
                // }
                else
                {
                    e.Item.Attributes["onclick"] = "";
                }
                //UserControlDocuments ucDocuments = (UserControlDocuments)e.Item.FindControl("ucMedicalTestEdit");
                //if (ucDocuments != null) ucDocuments.SelectedDocument = drv["FLDMEDICALTESTID"].ToString();


                UserControlQuick ucVerificationMethodEdit = (UserControlQuick)e.Item.FindControl("ucVerificationMethodEdit");
                if (ucVerificationMethodEdit != null)
                {
                    ucVerificationMethodEdit.QuickList = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , 131);
                    ucVerificationMethodEdit.DataBind();

                    ucVerificationMethodEdit.SelectedQuick = drv["FLDVERIFICATIONMETHOD"].ToString();
                }

                //UserControlHard ddlStatusEdit = (UserControlHard)e.Item.FindControl("ddlStatusEdit");
                //if (ddlStatusEdit != null)
                //{
                //    ddlStatusEdit.HardList = PhoenixRegistersHard.ListHard(SouthNests.Phoenix.Framework.PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105);
                //    ddlStatusEdit.SelectedHard = drv["FLDSTATUS"].ToString();
                //}

                DropDownList ddlDoctorEdit = (DropDownList)e.Item.FindControl("ddlDoctorEdit");
                if (ddlDoctorEdit != null)
                {
                    BindPMUDoctor(ddlDoctorEdit);
                    ddlDoctorEdit.SelectedValue = drv["FLDPMUDOCTORID"].ToString();
                }

                if (cmdAuthenticate != null && lblAuthenticationRequYN != null)
                {
                    cmdAuthenticate.Visible = lblAuthenticationRequYN.Text.Trim().Equals("1");
                }

                if (drv["FLDLOCKYN"].ToString().Equals("1"))
                {
                    if (cmdUnlock != null) cmdUnlock.Visible = true;
                    if (cme != null) cme.Visible = false;
                    if (db != null) db.Visible = false;
                    if (db1 != null) db1.Visible = false;
                    if (cmdAuthenticate != null) cmdAuthenticate.Visible = false;
                    if (cmdCrossCheck != null) cmdCrossCheck.Visible = false;
                }

                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }

                if (db1 != null)
                {
                    db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
                    if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
                }

                if (att != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
                }

                if (cme != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
                }

                if (cmdUnlock != null)
                {
                    cmdUnlock.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure to unlock?')");
                    if (!SessionUtil.CanAccess(this.ViewState, cmdUnlock.CommandName)) cmdUnlock.Visible = false;
                }

                if (cmdCrossCheck != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdCrossCheck.CommandName)) cmdCrossCheck.Visible = false;
                if (cmdAuthenticate != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdAuthenticate.CommandName)) cmdAuthenticate.Visible = false;

            }

            if (e.Item.IsInEditMode)
            {



                UserControlDocuments ucMedicaltest = (UserControlDocuments)e.Item.FindControl("ucMedicalTestEdit");
                if (ucMedicaltest != null) ucMedicaltest.SelectedDocument = drv["FLDMEDICALTESTID"].ToString();


                UserControlHard ucStatus = (UserControlHard)e.Item.FindControl("ddlStatusEdit");
                if (ucStatus != null)
                {
                    ucStatus.HardTypeCode = "105";
                    ucStatus.HardList = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105);
                    if (ucStatus.SelectedHard == "")
                        ucStatus.SelectedHard = drv["FLDSTATUS"].ToString();
                }

            }
            if (e.Item is GridFooterItem)
            {
                ImageButton imgbt = (ImageButton)e.Item.FindControl("cmdAdd");
                if (!SessionUtil.CanAccess(this.ViewState, imgbt.CommandName)) imgbt.Visible = false;


            }
        }
    }


    private bool IsValidMedical(string medicalid, string dateofissue, string dateofexpiry, string flagid, string placeofissue, string status)
    {
        Int16 resultInt;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        DataSet ds = PhoenixRegistersHard.EditHardCode(1, 95, "FLM");

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (medicalid == ds.Tables[0].Rows[0]["FLDHARDCODE"].ToString())
                if (General.GetNullableInteger(flagid) == null)
                    ucError.ErrorMessage = "Flag is required for Flag Medical.";

            if (medicalid != ds.Tables[0].Rows[0]["FLDHARDCODE"].ToString())
                if (!Int16.TryParse(status, out resultInt))
                    ucError.ErrorMessage = "Status is required";
        }
        if (!Int16.TryParse(medicalid, out resultInt))
            ucError.ErrorMessage = "Medical is required";

        if (string.IsNullOrEmpty(dateofissue))
            ucError.ErrorMessage = "Issue Date is required.";

        else if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be earlier than current date";
        }

        if (ViewState["LASTSIGNOFF"].ToString() != string.Empty && (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(ViewState["LASTSIGNOFF"].ToString())) < 0))
        {
            ucError.ErrorMessage = "Issue Date should be greater than Last SignOff Date";
        }
        if (string.IsNullOrEmpty(placeofissue))
        {
            ucError.ErrorMessage = "Place of Issue is required";
        }
        if (string.IsNullOrEmpty(dateofexpiry))
            ucError.ErrorMessage = "Expiry Date is required.";

        if (dateofissue != null && dateofexpiry != null)
        {
            if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry, out resultDate)))
                if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry)))
                    ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'";
        }

        return (!ucError.IsError);
    }
    private bool IsValidMedicalTest(string medicaltestid, string dateofissue, string dateofexpiry, string status, string remarks)
    {
        Int16 resultInt;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!Int16.TryParse(medicaltestid, out resultInt))
            ucError.ErrorMessage = "Medical Test is required";

        if (string.IsNullOrEmpty(dateofissue))
            ucError.ErrorMessage = "Issue Date is required.";

        else if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be earlier than current date";
        }

        if (status != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105, "UFT")
            && status != PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105, "TUF"))
        {
            if (string.IsNullOrEmpty(dateofexpiry))
                ucError.ErrorMessage = "Expiry Date is required.";
            if (dateofissue != null && dateofexpiry != null)
            {
                if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry, out resultDate)))
                    if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry)))
                        ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'";
            }
        }
        if (General.GetNullableInteger(status) == null)
        {
            ucError.ErrorMessage = "Status is required.";
        }
        if (status == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105, "UFT")
            || status == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105, "TUF"))
        {

            if (remarks == "")
                ucError.ErrorMessage = "Remarks is required.";

        }
        return (!ucError.IsError);
    }

    protected void DisableFlag(object sender, EventArgs e)
    {
        UserControlHard h = sender as UserControlHard;
        GridViewRow row = (GridViewRow)h.Parent.Parent;

        UserControlFlag uc = row.FindControl(row.RowType == DataControlRowType.Footer ? "ucFlagAdd" : "ucFlagEdit") as UserControlFlag;
        UserControlHard status = row.FindControl(row.RowType == DataControlRowType.Footer ? "ddlStatusAdd" : "ddlStatus") as UserControlHard;

        if (h.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 95, "FLM"))
        {
            uc.Enabled = true;
            status.CssClass = "input";
        }
        else
        {
            uc.SelectedFlag = string.Empty;
            uc.Enabled = false;
            status.CssClass = "input_mandatory";
        }
    }

    protected void Status_OnTextChangedEvent(object sender, EventArgs e)
    {
        UserControlHard h = sender as UserControlHard;

        RadTextBox txtremarks;
        UserControlHard status;
        UserControlDate txtdateofexpiry;
        if (h != null && (h.ID == "ddlStatusAdd"))
        {
            GridFooterItem row = (GridFooterItem)h.NamingContainer;
            txtremarks = (RadTextBox)row.FindControl("txtRemarksAdd");
            txtdateofexpiry = (UserControlDate)row.FindControl("txtExpiryDateAdd");
            status = (UserControlHard)row.FindControl("ddlStatusAdd");
        }
        else
        {
            GridDataItem row = (GridDataItem)h.NamingContainer;
            txtremarks = (RadTextBox)row.FindControl("txtRemarksEdit");
            txtdateofexpiry = (UserControlDate)row.FindControl("txtExpiryDateEdit");
            status = (UserControlHard)row.FindControl("ddlStatus");
        }

        if (h.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105, "UFT")
            || h.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105, "TUF"))
        {
            txtdateofexpiry.CssClass = "gridinput";
            txtremarks.CssClass = "gridinput_mandatory";
        }
        else
        {
            txtdateofexpiry.CssClass = "gridinput_mandatory";
            txtremarks.CssClass = "gridinput";

        }
    }
    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        //int nextRow = 0;
        //GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            // int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
                                                //    nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            ////  script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
                                                //  nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            ////   script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }


    /* Course   */

    protected void CrewCourseCertificate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDCOURSENAME", "FLDCATEGORYNAME", "FLDHARDNAME", "FLDCOURSENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDNAME", "FLDNATIONALITY",
                                     "FLDAUTHORITY", "FLDREMARKS", "FLDAUTHENTICATEDYN", "FLDAUTHENTICATEDON", "FLDAUTHENTICATEDBYNAME", "FLDVERIFIEDYN", "FLDVERIFIEDDATE", "FLDVERIFIEDBY"};
                string[] alCaptions = { "Course", "Category", "Type", "Certificate Number", "Place Of Issue", "Issue Date", "Expiry Date", "Institution", "Nationality", "Issuing Authority", "Remarks",
                                  "Authenticated Y/N", "Authenticated Date", "Authenticated By", "Cross checked Y/N", "Cross checked Date", "Cross checked by"};
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewCourseCertificate.CrewCourseCertificateSearch(
                                Int32.Parse(ViewState["empid"].ToString()), 1
                                , 0  // list courses other than cbt's
                                , 1
                                , iRowCount
                                , ref iRowCount
                                , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Crew Course And Certificate", ds.Tables[0], alColumns, alCaptions, null, "");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindCourse()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDCOURSENAME", "FLDCATEGORYNAME", "FLDHARDNAME", "FLDCOURSENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDNAME", "FLDNATIONALITY",
                                     "FLDAUTHORITY", "FLDREMARKS", "FLDAUTHENTICATEDYN", "FLDAUTHENTICATEDON", "FLDAUTHENTICATEDBYNAME", "FLDVERIFIEDYN", "FLDVERIFIEDDATE", "FLDVERIFIEDBY"};
            string[] alCaptions = { "Course", "Category", "Type", "Certificate Number", "Place Of Issue", "Issue Date", "Expiry Date", "Institution", "Nationality", "Issuing Authority", "Remarks",
                                  "Authenticated Y/N", "Authenticated Date", "Authenticated By", "Cross checked Y/N", "Cross checked Date", "Cross checked by"};
            DataSet ds = PhoenixCrewCourseCertificate.CrewCourseCertificateSearch(
                        Int32.Parse(ViewState["empid"].ToString()), 1
                        , 0  // list courses other than cbt's
                        , int.Parse(ViewState["PAGENUMBERCOU"].ToString())
                        , gvCrewCourseCertificate.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount
                        , General.GetNullableInteger(chkPendingAuthentication.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(chkPendingCrosscheck.Checked == true ? "1" : "0"));

            General.SetPrintOptions("gvCrewCourseCertificate", "Crew Course And Certificate", alCaptions, alColumns, ds);

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            gvCrewCourseCertificate.DataSource = ds;
            gvCrewCourseCertificate.VirtualItemCount = iRowCount;
            // gvCrewCourseCertificate.DataBind();
            //}
            //else
            //{
            //    DataTable dt = ds.Tables[0];
            //    ShowNoRecordsFound(dt, gvCrewCourseCertificate);
            //}

            ViewState["ROWCOUNTCOU"] = iRowCount;
            ViewState["TOTALPAGECOUNTCOU"] = iTotalPageCount;
            // RadComboBox institution = (RadComboBox)gvCrewCourseCertificate.FindControl("ucInstitutionAdd").FindControl("ddlAddressType");
            //institution.Attributes["style"] = "width:250px";
            // RadComboBox course = (RadComboBox)gvCrewCourseCertificate.FindControl("ddlCourseAdd").FindControl("ddlCourse");
            //course.Attributes["style"] = "width:250px";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCrewCourseCertificate_ItemCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            if (e.CommandName.ToString().ToUpper() == "CADD")
            {
                string courseid = ((UserControlCourse)e.Item.FindControl("ddlCourseAdd")).SelectedCourse;
                string institutionid = ((UserControlAddressType)e.Item.FindControl("ucInstitutionAdd")).SelectedAddress;
                string certificatenumber = ((RadTextBox)e.Item.FindControl("txtCourseNumberAdd")).Text;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateAdd")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueAdd")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateAdd"));
                string remarks = ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text;
                string flagid = ((UserControlNationality)e.Item.FindControl("ddlFlagAdd")).SelectedNationality;
                string authority = ((RadTextBox)e.Item.FindControl("txtAuthorityAdd")).Text;
                //string verifieddate = ((UserControlDate)_gridView.FooterRow.FindControl("ucVerifiedDateAdd")).Text;
                string verificationmethod = ((UserControlQuick)e.Item.FindControl("ucVerificationMethodAdd")).SelectedQuick;
                int declaration = (((RadCheckBox)e.Item.FindControl("chkCourseYN")).Checked.Equals(true)) ? 1 : 0;
                //string authenticateddate = ((UserControlDate)_gridView.FooterRow.FindControl("ucAuthenticatedDateAdd")).Text;
                //string authenticationyn = (((CheckBox)_gridView.FooterRow.FindControl("chkAuthenticatedYNAdd")).Checked) ? "1" : "0";
                //string verifiedyn = (((CheckBox)_gridView.FooterRow.FindControl("chkCrossCheckedYNAdd")).Checked) ? "1" : "0";

                if (!IsValidCourseCertificate(courseid, certificatenumber, dateofissue, dateofexpiry, placeofissue, institutionid, flagid))
                {
                    ucError.Visible = true;
                    return;
                }
                if (declaration == 0)
                {
                    ucError.ErrorMessage = "Please check the Declaration Checkbox before you save.";
                    ucError.Visible = true;
                }
                else
                {
                    PhoenixCrewCourseCertificate.InsertOffshoreCrewCourseCertificate(Convert.ToInt32(ViewState["empid"].ToString())
                    , Convert.ToInt32(courseid)
                    , certificatenumber
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableDateTime(dateofexpiry.Text)
                    , placeofissue
                    , General.GetNullableInteger(institutionid)
                    , General.GetNullableString(remarks)
                    , General.GetNullableInteger(flagid)
                    , General.GetNullableString(authority)
                    , null
                    , General.GetNullableInteger(verificationmethod)
                    , 1
                    );

                    BindCourse();
                    gvCrewCourseCertificate.Rebind();
                }
                // SetPageNavigatorCOU();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERCOU"] = null;
            }
            else if (e.CommandName.ToString().ToUpper() == "CARCHIVE")
            {
                string id = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;
                PhoenixCrewCourseCertificate.ArchiveCrewCourseCertificate(int.Parse(ViewState["empid"].ToString()), int.Parse(id), 0);
                //  _gridView.EditIndex = -1;
                //  _gridView.SelectedIndex = -1;
                BindCourse();
                gvCrewCourseCertificate.Rebind();
                // SetPageNavigatorCOU();
            }
            else if (e.CommandName.ToString().ToUpper() == "CDELETE")
            {
                string coursecertificateid = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;

                PhoenixCrewCourseCertificate.DeleteCrewCourseCertificate(Convert.ToInt32(coursecertificateid)
                                                                );
                BindCourse();
                gvCrewCourseCertificate.Rebind();
                //  SetPageNavigatorCOU();
            }

            else if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {
                try
                {
                    string courseid = ((UserControlCourse)e.Item.FindControl("ddlCourseEdit")).SelectedCourse;
                    string institutionid = ((UserControlAddressType)e.Item.FindControl("ucInstitutionEdit")).SelectedAddress;
                    string certificatenumber = ((RadTextBox)e.Item.FindControl("txtCourseNumberEdit")).Text;
                    string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit")).Text;
                    string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
                    UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit"));
                    string coursecertificateid = ((RadLabel)e.Item.FindControl("lblCourseIdEdit")).Text;
                    string flagid = ((UserControlNationality)e.Item.FindControl("ddlFlagEdit")).SelectedNationality;
                    string authority = ((RadTextBox)e.Item.FindControl("txtAuthorityEdit")).Text;
                    //string verifieddate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucVerifiedDateEdit")).Text;
                    string verificationmethod = ((UserControlQuick)e.Item.FindControl("ucVerificationMethodEdit")).SelectedQuick;
                    //string authenticatedate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucAuthenticatedDateEdit")).Text;
                    //string authenticatedyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkAuthenticatedYNEdit")).Checked) ? "1" : "0";
                    //string verifiedyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkCrossCheckedYNEdit")).Checked) ? "1" : "0";
                    //if (!IsValidateGrid(_gridView, nCurrentRow))
                    //{
                    //    ucError.Visible = true;
                    //    return;
                    //}

                    PhoenixCrewCourseCertificate.UpdateCrewCourseCertificate(Convert.ToInt32(coursecertificateid)
                        , Convert.ToInt32(ViewState["empid"].ToString())
                        , Convert.ToInt32(courseid)
                        , certificatenumber
                        , General.GetNullableDateTime(dateofissue)
                        , General.GetNullableDateTime(dateofexpiry.Text)
                        , placeofissue
                        , General.GetNullableInteger(institutionid)
                        , General.GetNullableInteger(flagid)
                        , General.GetNullableString(authority)
                        , null
                        , General.GetNullableInteger(verificationmethod)
                       );

                    //   _gridView.EditIndex = -1;
                    BindCourse();
                    gvCrewCourseCertificate.Rebind();
                    //  SetPageNavigatorCOU();
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "Please make the required correction";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }

            else if (e.CommandName.ToString().ToUpper() == "UNLOCKCOURSE")
            {
                string coursecertificateid = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;
                PhoenixCrewCourseCertificate.UnlockCourse(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(coursecertificateid));
                BindCourse();
                gvCrewCourseCertificate.Rebind();
                //  SetPageNavigatorCOU();
            }
            else if (e.CommandName.ToString().ToUpper() == "AUTHENTICATIONCOURSE")
            {
                string coursecertificateid = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblCourseLockYN = (RadLabel)e.Item.FindControl("lblCourseLockYN");
                string surl = "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEmployeeDocumentsAuthentication.aspx?DocType=COURSE&EmployeeId="
                    + ViewState["empid"].ToString()
                    + "&DocumentId=" + coursecertificateid.Trim()
                    + "&DtKey=" + (lblDTKey != null ? lblDTKey.Text.Trim() : "")
                    + "&LOCKYN=" + (lblCourseLockYN != null ? lblCourseLockYN.Text.Trim() : "")
                    + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Authenticate", surl, true);
            }
            else if (e.CommandName.ToString().ToUpper() == "CROSSCHECKCOURSE")
            {
                string coursecertificateid = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblCourseLockYN = (RadLabel)e.Item.FindControl("lblCourseLockYN");
                string surl = "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEmployeeDocumentsCrossCheck.aspx?DocType=COURSE&EmployeeId="
                    + ViewState["empid"].ToString()
                    + "&DocumentId=" + coursecertificateid.Trim()
                    + "&DtKey=" + (lblDTKey != null ? lblDTKey.Text.Trim() : "")
                    + "&LOCKYN=" + (lblCourseLockYN != null ? lblCourseLockYN.Text.Trim() : "")
                    + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CrossCheck", surl, true);
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    //protected void gvCrewCourseCertificate_DeleteCommand(object sender, GridCommandEventArgs e)
    //{
    //    try
    //    {

    //        string coursecertificateid = ((RadLabel)e.Item.FindControl("lblCourseId")).Text;

    //        PhoenixCrewCourseCertificate.DeleteCrewCourseCertificate(Convert.ToInt32(coursecertificateid)
    //                                                        );
    //        BindCourse();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;

    //    }
    //}

    protected void gvCrewCourseCertificate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBERCOU"] = ViewState["PAGENUMBERCOU"] != null ? ViewState["PAGENUMBERCOU"] : gvCrewCourseCertificate.CurrentPageIndex + 1;
            BindCourse();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewCourseCertificate_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;
            TableCell myCell1 = dataItem["ISADDEDBYSEAFARER"];
            if (((RadLabel)myCell1.FindControl("lblisbyseafarer")).Text == "1")
                myCell1.BackColor = System.Drawing.Color.Red;
            RadLabel lblisbyseafarer = (RadLabel)e.Item.FindControl("lblisbyseafarer"); 
            if (lblisbyseafarer.Text == "1")
            {
                RadLabel lblCourseName = (RadLabel)e.Item.FindControl("lblCourseName");              
                lblCourseName.Attributes.Add("style", "color:red !important;");           
             
            }

            ImageButton cme = (ImageButton)e.Item.FindControl("cmdEdit");
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            ImageButton db1 = (ImageButton)e.Item.FindControl("cmdArchive");
            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            ImageButton cmdUnlock = (ImageButton)e.Item.FindControl("cmdUnlock");
            RadLabel lblNumber = (RadLabel)e.Item.FindControl("lblNumber");
            RadLabel lnkCourseNumber = (RadLabel)e.Item.FindControl("lnkCourseNumber");
            ImageButton cmdAuthenticate = (ImageButton)e.Item.FindControl("cmdAuthenticate");
            ImageButton cmdCrossCheck = (ImageButton)e.Item.FindControl("cmdCrossCheck");
            RadLabel lblAuthenticationRequYN = (RadLabel)e.Item.FindControl("lblAuthenticationRequYN");
            DataRowView drv = (DataRowView)e.Item.DataItem;
          

            //if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
            //  && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            //{
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
                string mimetype = ".pdf";
                if (att != null)
                {
                    if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
                    if (drv["FLDLOCKYN"].ToString().Equals("1"))
                    {
                        att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.COURSE + "&cmdname=COURSEUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2&U=1'); return false;");
                    }
                    else
                    {
                        att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.COURSE + "&cmdname=COURSEUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2'); return false;");
                    }
                }
                RadLabel expdate = e.Item.FindControl("lblExpiryDate") as RadLabel;
                System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
                DateTime? d = null;
                if (expdate != null) d= General.GetNullableDateTime(expdate.Text);
                if (d.HasValue)
                {
                    TimeSpan t = d.Value - DateTime.Now;
                    if (t.Days >= 0 && t.Days < 120)
                    {
                        //e.Row.CssClass = "rowyellow";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days < 0)
                    {
                        //e.Row.CssClass = "rowred";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblCourseId");
                HtmlImage img = (HtmlImage)e.Item.FindControl("imgRemarks");
                if (img != null) img.Attributes.Add("onclick", "parent.openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.COURSE + "','xlarge')");
                RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
                if (lblR != null && string.IsNullOrEmpty(lblR.Text.Trim())) img.Src = Session["images"] + "/no-remarks.png";

                // }
                else
                    e.Item.Attributes["onclick"] = "";

                UserControlAddressType ucAddressType = (UserControlAddressType)e.Item.FindControl("ucInstitutionEdit");
                if (ucAddressType != null) ucAddressType.SelectedAddress = drv["FLDINSTITUTIONID"].ToString();

                UserControlCourse ucCourse = (UserControlCourse)e.Item.FindControl("ddlCourseEdit");
                DataRowView drvCourse = (DataRowView)e.Item.DataItem;
                if (ucCourse != null) ucCourse.SelectedCourse = drvCourse["FLDCOURSE"].ToString();

                UserControlNationality ucFlag = (UserControlNationality)e.Item.FindControl("ddlFlagEdit");
                DataRowView drvFlag = (DataRowView)e.Item.DataItem;
                if (ucFlag != null) ucFlag.SelectedNationality = drvFlag["FLDFLAGID"].ToString();

                UserControlQuick ucVerificationMethodEdit = (UserControlQuick)e.Item.FindControl("ucVerificationMethodEdit");
                if (ucVerificationMethodEdit != null)
                {
                    ucVerificationMethodEdit.QuickList = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , 131);
                    ucVerificationMethodEdit.DataBind();

                    ucVerificationMethodEdit.SelectedQuick = drv["FLDVERIFICATIONMETHOD"].ToString();
                }

                if (cmdAuthenticate != null && lblAuthenticationRequYN != null)
                {
                    cmdAuthenticate.Visible = lblAuthenticationRequYN.Text.Trim().Equals("1");
                }

                if (drv["FLDLOCKYN"].ToString().Equals("1"))
                {
                    if (cmdUnlock != null) cmdUnlock.Visible = true;
                    if (lblNumber != null) lblNumber.Visible = true;
                    if (lnkCourseNumber != null) lnkCourseNumber.Visible = false;
                    if (cme != null) cme.Visible = false;
                    if (db != null) db.Visible = false;
                    if (db1 != null) db1.Visible = false;
                    if (cmdAuthenticate != null) cmdAuthenticate.Visible = false;
                    if (cmdCrossCheck != null) cmdCrossCheck.Visible = false;
                }

                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }

                if (db1 != null)
                {
                    db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
                    if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
                }

                if (att != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
                }

                if (cme != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
                }

                if (cmdUnlock != null)
                {
                    cmdUnlock.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure to unlock?')");
                    if (!SessionUtil.CanAccess(this.ViewState, cmdUnlock.CommandName)) cmdUnlock.Visible = false;
                }

                if (cmdCrossCheck != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdCrossCheck.CommandName)) cmdCrossCheck.Visible = false;
                if (cmdAuthenticate != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdAuthenticate.CommandName)) cmdAuthenticate.Visible = false;
            //}
            if (e.Item is GridFooterItem)
            {
                LinkButton imgbt = (LinkButton)e.Item.FindControl("cmdAdd");
                if (!SessionUtil.CanAccess(this.ViewState, imgbt.CommandName)) imgbt.Visible = false;

                UserControlQuick ucVerificationMethodAdd = (UserControlQuick)e.Item.FindControl("ucVerificationMethodAdd");
                if (ucVerificationMethodAdd != null)
                {
                    ucVerificationMethodAdd.QuickList = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , 131);
                    ucVerificationMethodAdd.DataBind();
                }
            }
        }

    }
    //protected void gvCrewCourseCertificate_UpdateCommand(object sender, GridCommandEventArgs e)
    //{

    //    try
    //    {
    //        string courseid = ((UserControlCourse)e.Item.FindControl("ddlCourseEdit")).SelectedCourse;
    //        string institutionid = ((UserControlAddressType)e.Item.FindControl("ucInstitutionEdit")).SelectedAddress;
    //        string certificatenumber = ((RadTextBox)e.Item.FindControl("txtCourseNumberEdit")).Text;
    //        string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit")).Text;
    //        string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
    //        UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit"));
    //        string coursecertificateid = ((RadLabel)e.Item.FindControl("lblCourseIdEdit")).Text;
    //        string flagid = ((UserControlNationality)e.Item.FindControl("ddlFlagEdit")).SelectedNationality;
    //        string authority = ((RadTextBox)e.Item.FindControl("txtAuthorityEdit")).Text;
    //        //string verifieddate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucVerifiedDateEdit")).Text;
    //        string verificationmethod = ((UserControlQuick)e.Item.FindControl("ucVerificationMethodEdit")).SelectedQuick;
    //        //string authenticatedate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucAuthenticatedDateEdit")).Text;
    //        //string authenticatedyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkAuthenticatedYNEdit")).Checked) ? "1" : "0";
    //        //string verifiedyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkCrossCheckedYNEdit")).Checked) ? "1" : "0";

    //        //if (!IsValidateGrid(_gridView, nCurrentRow))
    //        //{
    //        //    ucError.Visible = true;
    //        //    return;
    //        //}

    //        PhoenixCrewCourseCertificate.UpdateCrewCourseCertificate(Convert.ToInt32(coursecertificateid)
    //            , Convert.ToInt32(ViewState["empid"].ToString())
    //            , Convert.ToInt32(courseid)
    //            , certificatenumber
    //            , General.GetNullableDateTime(dateofissue)
    //            , General.GetNullableDateTime(dateofexpiry.Text)
    //            , placeofissue
    //            , General.GetNullableInteger(institutionid)
    //            , General.GetNullableInteger(flagid)
    //            , General.GetNullableString(authority)
    //            , null
    //            , General.GetNullableInteger(verificationmethod)
    //           );

    //        //_gridView.EditIndex = -1;
    //        BindCourse();
    //      //  SetPageNavigatorCOU();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.HeaderMessage = "Please make the required correction";
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;

    //    }
    //}
    //protected void gvCrewCourseCertificate_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.SelectedIndex = e.NewSelectedIndex;
    //    _gridView.EditIndex = -1;

    //}

    protected void ddlDocument_TextChanged(object sender, EventArgs e)
    {
        UserControlCourse dc = (UserControlCourse)sender;
        DataSet ds = new DataSet();
        UserControlDate expirydate;
        UserControlDate issuedate;
        GridFooterItem gv = (GridFooterItem)dc.NamingContainer;
        RadTextBox txtCourseNumberAdd = (RadTextBox)gv.FindControl("txtCourseNumberAdd");
        if (dc.SelectedCourse != "Dummy")
        {
            ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(General.GetNullableInteger(dc.SelectedCourse).Value);
        }
        if (ds.Tables.Count > 0)
        {
            if (dc.ID == "ddlCourseAdd")
            {
                GridFooterItem footerItem = (GridFooterItem)gvCrewCourseCertificate.MasterTableView.GetItems(GridItemType.Footer)[0];

                expirydate = (UserControlDate)footerItem.FindControl("txtExpiryDateAdd");
                issuedate = (UserControlDate)footerItem.FindControl("txtIssueDateAdd");
            }
            else
            {
                GridDataItem row = ((GridDataItem)dc.Parent.Parent);
                expirydate = (UserControlDate)row.FindControl("txtExpiryDateEdit");
                issuedate = (UserControlDate)row.FindControl("txtIssueDateEdit");
            }
            if (ds.Tables[0].Rows[0]["FLDEXPIRY"].ToString() == "1")
            {
                expirydate.CssClass = "input_mandatory";
                issuedate.CssClass = "input_mandatory";
            }
            else
            {
                expirydate.CssClass = "input";
                issuedate.CssClass = "input";
            }
            txtCourseNumberAdd.Focus();
        }
    }
    //private void SetAttachmentMarking()
    //{
    //    DataTable dt1 = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["attachmentcode"].ToString()), PhoenixCrewAttachmentType.COURSE.ToString());
    //    if (dt1.Rows.Count > 0)
    //    {
    //        //imgClip.Visible = true;
    //       // imgClip.Attributes["onclick"] = "javascript:parent.Openpopup('NAA','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
    //       //        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.COURSE + "&cmdname=COURSEUPLOAD'); return false;";
    //    }
    //    else
    //     //   imgClip.Visible = false;
    //}


    private bool IsValidCourseCertificate(string courseid, string certificatenumber, string dateofissue, UserControlDate dateofexpiry, string placeofissue, string institutionid, string flagid)
    {
        Int16 resultInt;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!Int16.TryParse(courseid, out resultInt))
            ucError.ErrorMessage = "Course is required";


        if (!Int16.TryParse(institutionid, out resultInt))
            ucError.ErrorMessage = "Institution is required";

        if (!Int16.TryParse(flagid, out resultInt))
            ucError.ErrorMessage = "Nationality is required";

        if (certificatenumber.Trim() == "")
            ucError.ErrorMessage = "Certificate Number is required";

        if (placeofissue.Trim() == "")
            ucError.ErrorMessage = "Place of issue is required";

        if (string.IsNullOrEmpty(dateofissue))
            ucError.ErrorMessage = "Issue Date is required.";
        else if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(dateofexpiry.Text) && dateofexpiry.CssClass == "input_mandatory")
            ucError.ErrorMessage = "Expiry Date is required.";

        if (dateofissue != null && dateofexpiry.Text != null)
        {
            if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry.Text, out resultDate)))
                if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry.Text)))
                    ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'";
        }

        return (!ucError.IsError);
    }
    private bool IsValidateGrid(GridView _gridView, int nCurrentRow)
    {
        string courseid = ((UserControlCourse)_gridView.Rows[nCurrentRow].FindControl("ddlCourseEdit")).SelectedCourse;
        string certificatenumber = ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtCourseNumberEdit")).Text;
        string dateofissue = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtIssueDateEdit")).Text;
        string placeofissue = ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtPlaceOfIssueEdit")).Text;
        UserControlDate dateofexpiry = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtExpiryDateEdit"));
        string coursecertificateid = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblCourseIdEdit")).Text;
        string institutionid = ((UserControlAddressType)_gridView.Rows[nCurrentRow].FindControl("ucInstitutionEdit")).SelectedAddress;
        string flagid = ((UserControlNationality)_gridView.Rows[nCurrentRow].FindControl("ddlFlagEdit")).SelectedNationality;
        return IsValidCourseCertificate(courseid, certificatenumber, dateofissue, dateofexpiry, placeofissue, institutionid, flagid);
    }

    /* Other Documents */

    protected void CrewOtherDocumentMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBEROTH"] = 1;
                BindDataOTH();
                // SetPageNavigatorOTH();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDDOCUMENTNAME", "FLDCATEGORYNAME", "FLDDOCUMENTNUMBER", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDPLACEOFISSUE", "FLDISSUINGAUTHORITY", "FLDREMARKS",
                                 "FLDAUTHENTICATEDYN", "FLDAUTHENTICATEDON", "FLDAUTHENTICATEDBYNAME", "FLDVERIFIEDYN", "FLDVERIFIEDON", "FLDVERIFIEDBY" };
                string[] alCaptions = { "Document Name", "Category", "Number", "Date of Issue", "Date of Expiry", "Place of Issue", "Issuing Authority", "Remarks",
                                  "Authenticated Y/N","Authenticated Date","Authenticated By","Cross checked Y/N","Cross checked Date","Cross checked by"};
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSIONOTH"] == null) ? null : (ViewState["SORTEXPRESSIONOTH"].ToString());
                if (ViewState["SORTDIRECTIONOTH"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTIONOTH"].ToString());

                DataSet ds = PhoenixCrewTravelDocument.SearchEmployeeOtherDocument(Convert.ToInt32(ViewState["empid"].ToString()), null, 1, 0,
                                                                                      sortexpression, sortdirection,
                                                                                    1,
                                                                                    iRowCount,
                                                                                    ref iRowCount,
                                                                                    ref iTotalPageCount);
                General.ShowExcel("Other Document", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void BindDataOTH()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDDOCUMENTNAME", "FLDCATEGORYNAME", "FLDDOCUMENTNUMBER", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDPLACEOFISSUE", "FLDISSUINGAUTHORITY", "FLDREMARKS",
                                 "FLDAUTHENTICATEDYN", "FLDAUTHENTICATEDON", "FLDAUTHENTICATEDBYNAME", "FLDVERIFIEDYN", "FLDVERIFIEDON", "FLDVERIFIEDBY" };
            string[] alCaptions = { "Document Name", "Category", "Number", "Date of Issue", "Date of Expiry", "Place of Issue", "Issuing Authority", "Remarks",
                                  "Authenticated Y/N","Authenticated Date","Authenticated By","Cross checked Y/N","Cross checked Date","Cross checked by"};

            string sortexpression = (ViewState["SORTEXPRESSIONOTH"] == null) ? null : (ViewState["SORTEXPRESSIONOTH"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONOTH"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONOTH"].ToString());


            DataSet ds = PhoenixCrewTravelDocument.SearchEmployeeOtherDocument(Convert.ToInt32(ViewState["empid"].ToString()), null, 1, 0,
                                                                               sortexpression, sortdirection,
                                                                               int.Parse(ViewState["PAGENUMBEROTH"].ToString()),
                                                                               gvOtherDocument.PageSize,
                                                                               ref iRowCount,
                                                                               ref iTotalPageCount,
                                                                               General.GetNullableInteger(chkPendingAuthentication.Checked == true ? "1" : "0"),
                                                                               General.GetNullableInteger(chkPendingCrosscheck.Checked == true ? "1" : "0"));

            General.SetPrintOptions("gvOtherDocument", "Other Documnet", alCaptions, alColumns, ds);


            //if (ds.Tables[0].Rows.Count > 0)
            //{

            gvOtherDocument.DataSource = ds;
            gvOtherDocument.VirtualItemCount = iRowCount;
            // gvOtherDocument.DataBind();
            //}
            //else
            //{

            //    DataTable dt = ds.Tables[0];
            //    ShowNoRecordsFound(dt, gvOtherDocument);
            //}

            ViewState["ROWCOUNTOTH"] = iRowCount;
            ViewState["TOTALPAGECOUNTOTH"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvOtherDocument_ItemCommand(object sender, GridCommandEventArgs e)
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
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuingAuthorityAdd")).Text;
                //string verifieddate = ((UserControlDate)_gridView.FooterRow.FindControl("ucVerifiedDateAdd")).Text;
                string verificationmethod = ((UserControlQuick)e.Item.FindControl("ucVerificationMethodAdd")).SelectedQuick;
                int declaration = (((RadCheckBox)e.Item.FindControl("chkOtherDocYN")).Checked.Equals(true)) ? 1 : 0;
                //string authenticateddate = ((UserControlDate)_gridView.FooterRow.FindControl("ucAuthenticatedDateAdd")).Text;
                //string authenticationyn = (((CheckBox)_gridView.FooterRow.FindControl("chkAuthenticatedYNAdd")).Checked) ? "1" : "0";
                //string crosscheckedyn = (((CheckBox)_gridView.FooterRow.FindControl("chkCrossCheckedYNAdd")).Checked) ? "1" : "0";

                if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry))
                {
                    ucError.Visible = true;
                    BindDataOTH();

                    return;
                }
                if (declaration == 0)
                {
                    ucError.ErrorMessage = "Please check the Declaration Checkbox before you save.";
                    ucError.Visible = true;
                }
                else
                {
                    InsertTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry.Text, issuingauthority, verificationmethod, declaration);
                    BindDataOTH();
                    gvOtherDocument.Rebind();
                }
            }
            if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {

                string documenttype = ((UserControlDocumentType)e.Item.FindControl("ucDocumentTypeEdit")).SelectedDocumentType;
                string documentnumber = ((RadLabel)e.Item.FindControl("txtNumberEdit")).Text;
                string dateofissue = ((UserControlDate)e.Item.FindControl("ucDateOfIssueEdit")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssue")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("ucDateExpiryEdit"));
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuingAuthorityEdit")).Text;
                //string verifieddate = ((UserControlDate)_gridView.FooterRow.FindControl("ucVerifiedDateAdd")).Text;
                string verificationmethod = ((UserControlQuick)e.Item.FindControl("ucVerificationMethodEdit")).SelectedQuick;
                int declaration = (((RadCheckBox)e.Item.FindControl("chkOtherDocYN")).Checked.Equals(true)) ? 1 : 0;
                //string authenticateddate = ((UserControlDate)_gridView.FooterRow.FindControl("ucAuthenticatedDateAdd")).Text;
                //string authenticationyn = (((CheckBox)_gridView.FooterRow.FindControl("chkAuthenticatedYNAdd")).Checked) ? "1" : "0";
                //string crosscheckedyn = (((CheckBox)_gridView.FooterRow.FindControl("chkCrossCheckedYNAdd")).Checked) ? "1" : "0";

                if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry))
                {
                    ucError.Visible = true;
                    // if (_gridView.EditIndex > -1)
                    {
                        //    _gridView.EditIndex = -1;
                        BindDataOTH();
                    }
                    return;
                }

                InsertTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry.Text, issuingauthority, verificationmethod,declaration);
                BindDataOTH();
                gvOtherDocument.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBEROTH"] = null;
            }
            else if (e.CommandName.ToString().ToUpper() == "ARCHIVE")
            {
                string id = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;
                PhoenixNewApplicantTravelDocument.ArchiveEmployeeTravelDocument(int.Parse(id), 0);

                BindDataOTH();
            }
            else if (e.CommandName.ToString().ToUpper() == "UNLOCKOTHERDOC")
            {
                string id = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;
                PhoenixCrewTravelDocument.UnlockTravelDocument(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(id));
                BindDataOTH();
            }
            else if (e.CommandName.ToString().ToUpper() == "AUTHENTICATIONOTHERDOC")
            {
                string OtherDocId = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblOtherLockYN = (RadLabel)e.Item.FindControl("lblOtherLockYN");
                string surl = "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEmployeeDocumentsAuthentication.aspx?DocType=OTHER&EmployeeId="
                    + ViewState["empid"].ToString()
                    + "&DocumentId=" + OtherDocId.Trim()
                    + "&DtKey=" + (lblDTKey != null ? lblDTKey.Text.Trim() : "")
                    + "&LOCKYN=" + (lblOtherLockYN != null ? lblOtherLockYN.Text.Trim() : "")
                    + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Authenticate", surl, true);
            }
            else if (e.CommandName.ToString().ToUpper() == "CROSSCHECKOTHERDOC")
            {
                string OtherDocId = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblOtherLockYN = (RadLabel)e.Item.FindControl("lblOtherLockYN");
                string surl = "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEmployeeDocumentsCrossCheck.aspx?DocType=OTHER&EmployeeId="
                    + ViewState["empid"].ToString()
                    + "&DocumentId=" + OtherDocId.Trim()
                    + "&DtKey=" + (lblDTKey != null ? lblDTKey.Text.Trim() : "")
                    + "&LOCKYN=" + (lblOtherLockYN != null ? lblOtherLockYN.Text.Trim() : "")
                    + "');";
                RadScriptManager.RegisterStartupScript(this, this.GetType(), "CrossCheck", surl, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    //protected void gvOtherDocument_DeleteCommand(object sender, GridCommandEventArgs e)
    //{
    //    try
    //    {


    //        string traveldocumentid = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;

    //        PhoenixNewApplicantTravelDocument.DeleteEmployeeTravelDocument(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                                                            , Convert.ToInt32(traveldocumentid)
    //                                                            );

    //        BindDataOTH();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;

    //    }
    //}

    protected void gvOtherDocument_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridDataItem)
        //{
        //    Get the LinkButton control in the first cell
        //   LinkButton _doubleClickButton = (LinkButton)e.Item.Cells[0].Controls[0];
        //    Get the javascript which is assigned to this LinkButton
        //    string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
        //    Add this javascript to the onclick Attribute of the row
        //    e.Item.Attributes["onclick"] = _jsDouble;
        //}
        if (e.Item is GridDataItem)
        {
            ImageButton ed = (ImageButton)e.Item.FindControl("cmdEdit");
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            ImageButton db1 = (ImageButton)e.Item.FindControl("cmdArchive");
            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            ImageButton cmdUnlock = (ImageButton)e.Item.FindControl("cmdUnlock");
            RadLabel lblNumber = (RadLabel)e.Item.FindControl("lblNumber");
            RadLabel lnkNumber = (RadLabel)e.Item.FindControl("lnkNumber");
            ImageButton cmdAuthenticate = (ImageButton)e.Item.FindControl("cmdAuthenticate");
            ImageButton cmdCrossCheck = (ImageButton)e.Item.FindControl("cmdCrossCheck");
            RadLabel lblAuthenticationRequYN = (RadLabel)e.Item.FindControl("lblAuthenticationRequYN");
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lblisbyseafarer = (RadLabel)e.Item.FindControl("lblisbyseafarer");
            if (lblisbyseafarer.Text == "1")
            {
                RadLabel lblDocumentTypename = (RadLabel)e.Item.FindControl("lblDocumentTypename");
                lblDocumentTypename.Attributes.Add("style", "color:red !important;");

            }

            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
              && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                string mimetype = ".pdf";
                if (att != null)
                {
                    RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
                    if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
                    if (drv["FLDLOCKYN"].ToString().Equals("1"))
                    {
                        att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&cmdname=OTHERDOCUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2&U=1'); return false;");
                    }
                    else
                    {
                        att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&cmdname=OTHERDOCUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2'); return false;");
                    }
                }
                RadLabel lbl = (RadLabel)e.Item.FindControl("lbltraveldocumentid");
                HtmlImage img = (HtmlImage)e.Item.FindControl("imgRemarks");
                if (img != null)
                {
                    img.Attributes.Add("onclick", "javascript:openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "','xlarge')");
                }
                RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
                if (lblR != null && string.IsNullOrEmpty(lblR.Text.Trim())) img.Src = Session["images"] + "/no-remarks.png";

                System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
                RadLabel expdate = e.Item.FindControl("lblDateofExpiry") as RadLabel;
                //DateTime? d = General.GetNullableDateTime(expdate.Text);
                //if (d.HasValue)
                DateTime? d = null;
                if (expdate != null) d = General.GetNullableDateTime(expdate.Text);
                if (d.HasValue && imgFlag != null)
                {
                    TimeSpan t = d.Value - DateTime.Now;
                    if (t.Days >= 0 && t.Days < 120)
                    {
                        //e.Row.CssClass = "rowyellow";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days < 0)
                    {
                        //e.Row.CssClass = "rowred";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }
                // }
                else
                {
                    e.Item.Attributes["onclick"] = "";
                }

                UserControlDocumentType ucDocumentType = (UserControlDocumentType)e.Item.FindControl("ucDocumentTypeEdit");
                if (ucDocumentType != null) ucDocumentType.SelectedDocumentType = drv["FLDDOCUMENTTYPE"].ToString();

                UserControlQuick ucVerificationMethodEdit = (UserControlQuick)e.Item.FindControl("ucVerificationMethodEdit");
                if (ucVerificationMethodEdit != null)
                {
                    ucVerificationMethodEdit.QuickList = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , 131);
                    ucVerificationMethodEdit.DataBind();

                    ucVerificationMethodEdit.SelectedQuick = drv["FLDVERIFICATIONMETHOD"].ToString();
                }

                if (cmdAuthenticate != null && lblAuthenticationRequYN != null)
                {
                    cmdAuthenticate.Visible = lblAuthenticationRequYN.Text.Trim().Equals("1");
                }

                if (drv["FLDLOCKYN"].ToString().Equals("1"))
                {
                    if (cmdUnlock != null) cmdUnlock.Visible = true;
                    if (lblNumber != null) lblNumber.Visible = true;
                    if (lnkNumber != null) lnkNumber.Visible = false;
                    if (ed != null) ed.Visible = false;
                    if (db != null) db.Visible = false;
                    if (db1 != null) db1.Visible = false;
                    if (cmdAuthenticate != null) cmdAuthenticate.Visible = false;
                    if (cmdCrossCheck != null) cmdCrossCheck.Visible = false;
                }

                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }

                if (db1 != null)
                {
                    db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
                    if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
                }

                if (att != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
                }

                if (ed != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, ed.CommandName)) ed.Visible = false;
                }

                if (cmdUnlock != null)
                {
                    cmdUnlock.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure to unlock?')");
                    if (!SessionUtil.CanAccess(this.ViewState, cmdUnlock.CommandName)) cmdUnlock.Visible = false;
                }

                if (cmdCrossCheck != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdCrossCheck.CommandName)) cmdCrossCheck.Visible = false;
                if (cmdAuthenticate != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdAuthenticate.CommandName)) cmdAuthenticate.Visible = false;
            }

            if (e.Item is GridHeaderItem)
            {
                if (ViewState["SORTEXPRESSION"] != null)
                {
                    HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
                    if (img != null)
                    {
                        if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                            img.Src = Session["images"] + "/arrowUp.png";
                        else
                            img.Src = Session["images"] + "/arrowDown.png";

                        img.Visible = true;
                    }
                }
            }
            if (e.Item is GridFooterItem)
            {
                LinkButton ad = (LinkButton)e.Item.FindControl("cmdAdd");
                if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);


                UserControlQuick ucVerificationMethodAdd = (UserControlQuick)e.Item.FindControl("ucVerificationMethodAdd");
                if (ucVerificationMethodAdd != null)
                {
                    ucVerificationMethodAdd.QuickList = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , 131);
                    ucVerificationMethodAdd.DataBind();
                }
            }

        }
    }
    //protected void gvOtherDocument_UpdateCommand(object sender, GridCommandEventArgs e)
    //{

    //    try
    //    {
    //        string documenttype = ((UserControlDocumentType)e.Item.FindControl("ucDocumentTypeEdit")).SelectedDocumentType;
    //        string documentnumber = ((RadLabel)e.Item.FindControl("txtNumberEdit")).Text;
    //        string dateofissue = ((UserControlDate)e.Item.FindControl("ucDateOfIssueEdit")).Text;
    //        string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssue")).Text;
    //        UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("ucDateExpiryEdit"));
    //        string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuingAuthorityEdit")).Text;
    //        string traveldocumentid = ((RadLabel)e.Item.FindControl("lbltraveldocumentidEdit")).Text;

    //        //string verifieddate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucVerifiedDateEdit")).Text;
    //        string verificationmethod = ((UserControlQuick)e.Item.FindControl("ucVerificationMethodEdit")).SelectedQuick;
    //        //string authenticatedate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucAuthenticatedDateEdit")).Text;
    //        //string authenticatedyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkAuthenticatedYNEdit")).Checked) ? "1" : "0";
    //        //string crosschekcedyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkCrossCheckedYNEdit")).Checked) ? "1" : "0";

    //        if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        UpdateTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry.Text, issuingauthority, traveldocumentid, verificationmethod);
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }

    //    BindDataOTH();
    //}
    protected void gvOtherDocument_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
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
    //protected void gvOtherDocument_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.SelectedIndex = e.NewSelectedIndex;
    //    _gridView.EditIndex = -1;
    //    BindDataOTH();
    // //   SetPageNavigatorOTH();
    //}emp

    public void InsertTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, string dateofexpiry, string issuingauthority, string verificationmethod,int declaration)
    {
        PhoenixCrewTravelDocument.InsertOffshoreEmployeeOtherDocument(Convert.ToInt32(ViewState["empid"].ToString())
                                                            , Convert.ToInt32(documenttype)
                                                            , documentnumber
                                                            , Convert.ToDateTime(dateofissue)
                                                            , placeofissue
                                                            , General.GetNullableDateTime(dateofexpiry)
                                                            , issuingauthority
                                                            , 0
                                                            , General.GetNullableInteger(verificationmethod)
                                                            ,1
                                                           
                                                            );
    }
    public void UpdateTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, string dateofexpiry, string issuingauthority, string traveldocumentid, string verificationmethod)
    {
        PhoenixCrewTravelDocument.UpdateEmployeeOtherDocument(Convert.ToInt32(ViewState["empid"].ToString())
                                                            , Convert.ToInt32(traveldocumentid)
                                                            , Convert.ToInt32(documenttype)
                                                            , documentnumber
                                                            , Convert.ToDateTime(dateofissue)
                                                            , placeofissue
                                                            , General.GetNullableDateTime(dateofexpiry)
                                                            , issuingauthority
                                                            , General.GetNullableInteger(verificationmethod)
                                                            );
    }

    private bool IsValidTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, UserControlDate dateofexpiry)
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

        return (!ucError.IsError);
    }




    protected void ddlDocumentTypeOTH_TextChanged(object sender, EventArgs e)
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
            date = (UserControlDate)gvOtherDocument.FindControl("ucDateExpiryAdd");

        }
        else
        {
            GridViewRow row = ((GridViewRow)dc.Parent.Parent);
            date = (UserControlDate)gvOtherDocument.Items[row.RowIndex].FindControl("ucDateExpiryEdit");
        }
        if (ds.Tables[0].Rows[0]["FLDHAVINGEXPIRY"].ToString() == "1")
            date.CssClass = "input_mandatory";
        else
            date.CssClass = "input";
    }
    /*  Common  */

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(ViewState["empid"].ToString()));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
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
        SetEmployeePassportDetails();
        SetEmployeeSeamanBookDetails();

        BindMedicalData();
        // BindMedicalTestData();

        BindCourse();
        BindDataOTH();
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void BindPMUDoctor(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.DataSource = PhoenixRegistersPMUDoctor.ListPMUDoctor(1);
        ddl.DataTextField = "FLDDOCTORNAME";
        ddl.DataValueField = "FLDDOCTORID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }







    protected void gvOtherDocument_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOtherDocument.CurrentPageIndex + 1;
            BindDataOTH();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


}