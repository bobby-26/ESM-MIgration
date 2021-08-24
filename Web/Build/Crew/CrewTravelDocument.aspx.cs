using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
public partial class CrewTravelDocument : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();            
            toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelDocument.aspx?e=1", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvTravelDocument')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewTravelDocumentArchived.aspx?empid=" + Filter.CurrentCrewSelection + "&type=1&t=p'); return false;", "Show Archived", "<i class=\"fas fa-archive\"></i>", "ARCHIVED");          
            MenuCrewTravelDocument.AccessRights = this.ViewState;
            MenuCrewTravelDocument.MenuList = toolbargrid.Show();
           
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                imgPassportArchive.Attributes.Add("onclick", "return fnConfirmDelete(event,'You are adding a new passport,please confirm to proceed?')");
                imgSeamanBook.Attributes.Add("onclick", "return fnConfirmDelete(event,'You are adding a new CDC,please confirm to proceed ?')");
                imgUSVisaArchive.Attributes.Add("onclick", "return fnConfirmDelete(event,'You are adding a US Visa,please confirm to proceed ?')");
                imgMCVArchive.Attributes.Add("onclick", "return fnConfirmDelete(event,'You are adding a new MCV,please confirm to proceed ?')");
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
                imgPPClip.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                  + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.PASSPORT + "&cmdname=PASSPORTUPLOAD'); return false;";
                imgCCClip.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                 + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.SEAMANBOOK + "&cmdname=SEAMANBOOKUPLOAD'); return false;";
                imgUSVisaClip.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                 + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.USVISA + "&cmdname=VISAUPLOAD'); return false;";
                imgMCVClip.Attributes["onclick"] = "javascript:openNewWindow('MCV','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                 + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MCVAUSTRALIA + "&cmdname=MCVUPLOAD'); return false;";
                imgPPDF.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileExport.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
              + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.PASSPORT + "&cmdname=PASSPORTUPLOAD'); return false;";
                imgCPDF.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileExport.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                 + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.SEAMANBOOK + "&cmdname=SEAMANBOOKUPLOAD'); return false;";
                imgPassportArchive.Visible = SessionUtil.CanAccess(this.ViewState, imgPassportArchive.CommandName);
                imgSeamanBook.Visible = SessionUtil.CanAccess(this.ViewState, imgSeamanBook.CommandName);
                imgUSVisaArchive.Visible = SessionUtil.CanAccess(this.ViewState, imgUSVisaArchive.CommandName);
                imgMCVArchive.Visible = SessionUtil.CanAccess(this.ViewState, imgMCVArchive.CommandName);

                gvTravelDocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
            toolbarmain.AddButton("Send Mail", "SENDMAIL",ToolBarDirection.Right);
            CrewPassPort.AccessRights = this.ViewState;
            CrewPassPort.MenuList = toolbarmain.Show();


            DateTime? d = General.GetNullableDateTime(ucDateOfExpiry.Text);
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        SetEmployeePassportDetails();
        SetEmployeeSeamanBookDetails();
        SetEmployeeUSVisaDetails();
        SetEmployeeMCVDetails();
    }
    public void OnClickPassportArchive(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewTravelDocument.EmployeePassportArchive(int.Parse(Filter.CurrentCrewSelection));
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
            PhoenixCrewTravelDocument.EmployeeSeamanBookArchive(int.Parse(Filter.CurrentCrewSelection));
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
            PhoenixCrewTravelDocument.EmployeeUSVisaArchive(int.Parse(Filter.CurrentCrewSelection));
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
            PhoenixCrewTravelDocument.EmployeeMCVAustraliaArchive(int.Parse(Filter.CurrentCrewSelection));
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
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
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

    protected void CrewTravelDocumentMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
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

        string[] alColumns = { "FLDDOCUMENTNAME", "FLDDOCUMENTNUMBER", "FLDDATEOFISSUE", "FLDVALIDFROM", "FLDDATEOFEXPIRY", "FLDNOOFENTRYNAME", "FLDPLACEOFISSUE", "FLDCOUNTRYNAME", "FLDREMARKS", "FLDFULLTERM" };
        string[] alCaptions = { "Document Name", "Number", "Date of Issue", "Valid From", "Date of Expiry", "No of entry", "Place of Issue", "Nationality/Flag", "Remarks", "Full Term YN" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewTravelDocument.SearchEmployeeTravelDocument(Convert.ToInt32(Filter.CurrentCrewSelection), null, 1,
                                                                              sortexpression, sortdirection,
                                                                            (int)ViewState["PAGENUMBER"],
                                                                            iRowCount,
                                                                            ref iRowCount,
                                                                            ref iTotalPageCount);
        General.ShowExcel("Crew Travel Document", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void CrewPassPort_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidPassport() || !IsValidSeamanBook() || !IsValidUSVisa())
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateEmployeePassport();
                UpdateEmployeeSeamanBook();
                UpdateEmployeeUSVisa();
                UpdateEmployeeMCV();
            }
            if (CommandName.ToUpper() == "SENDMAIL")
            {
                try
                {
                    PhoenixCrewTravelDocument.EmployeeTravelDocsSendMail(null, General.GetNullableInteger(Filter.CurrentCrewSelection.ToString()));
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
    protected void gvTravelDocument_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTravelDocument.CurrentPageIndex + 1;
        BindData();
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDDOCUMENTNAME", "FLDDOCUMENTNUMBER", "FLDDATEOFISSUE", "FLDVALIDFROM", "FLDDATEOFEXPIRY", "FLDNOOFENTRYNAME", "FLDPLACEOFISSUE", "FLDCOUNTRYNAME", "FLDREMARKS", "FLDFULLTERM" };
            string[] alCaptions = { "Document Name", "Number", "Date of Issue", "Valid From", "Date of Expiry", "No of entry", "Place of Issue", "Nationality/Flag", "Remarks", "Full Term YN" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixCrewTravelDocument.SearchEmployeeTravelDocument(Convert.ToInt32(Filter.CurrentCrewSelection), null, 1,
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
                string connectedtovessel = ((RadCheckBox)e.Item.FindControl("chkConnectedToVesselAdd")).Checked == true  ? "1" : "0";
                string validfrom = ((UserControlDate)e.Item.FindControl("ucValidFromAdd")).Text;
                string noofentry = ((RadComboBox)e.Item.FindControl("ddlNoofentryAdd")).SelectedValue;
                string passportno = ((RadTextBox)e.Item.FindControl("txtpassportnoAdd")).Text;

                if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry, country))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewTravelDocument.InsertEmployeeTravelDocument(Convert.ToInt32(Filter.CurrentCrewSelection)
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
                                                            );

                BindData();
                gvTravelDocument.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "ARCHIVE")
            {
                string id = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;
                PhoenixCrewTravelDocument.ArchiveEmployeeTravelDocument(int.Parse(Filter.CurrentCrewSelection), int.Parse(id), 0);

                BindData();
                gvTravelDocument.Rebind();
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
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }


            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton lnkNumber = (LinkButton)e.Item.FindControl("lnkNumber");
            if (cme != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
                lnkNumber.Enabled = SessionUtil.CanAccess(this.ViewState, cme.CommandName);
            }


            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
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
            LinkButton img = (LinkButton)e.Item.FindControl("imgRemarks");
            RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
            if (img != null)
            {
                img.Attributes.Add("onclick", "openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "','xlarge')");
                if (string.IsNullOrEmpty(lblR.Text.Trim()))
                {
                    if (img != null)
                    {
                        HtmlGenericControl html = new HtmlGenericControl();                        
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses-nr\"></i></span>";
                        img.Controls.Add(html);
                    }

                }
            }

            
            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            RadLabel expdate = e.Item.FindControl("lblDateofExpiry") as RadLabel;
            if (expdate != null)
            {
                DateTime? d = General.GetNullableDateTime(expdate.Text);
                if (d.HasValue)
                {
                    TimeSpan t = d.Value - DateTime.Now;
                    if (t.Days >= 0 && t.Days < 120)
                    {
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days < 0)
                    {
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }
            }

            gvTravelDocument.MasterTableView.GetColumn("FlagIconHeader").Display = true;
            gvTravelDocument.MasterTableView.GetColumn("RemarksHeader").Display = true;
            gvTravelDocument.MasterTableView.GetColumn("StatusHeader").Display = true;
        }

        if (e.Item.IsInEditMode)
        {
            gvTravelDocument.MasterTableView.GetColumn("FlagIconHeader").Display = false;
            gvTravelDocument.MasterTableView.GetColumn("RemarksHeader").Display = false;
            gvTravelDocument.MasterTableView.GetColumn("StatusHeader").Display = false;

            UserControlCountry ucCountry = (UserControlCountry)e.Item.FindControl("ddlCountryEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucCountry != null) ucCountry.SelectedCountry = drv["FLDCOUNTRYCODE"].ToString();

            UserControlDocumentType ucDocumentType = (UserControlDocumentType)e.Item.FindControl("ucDocumentTypeEdit");
            DataRowView drvDocumentType = (DataRowView)e.Item.DataItem;
            if (ucDocumentType != null) ucDocumentType.SelectedDocumentType = drv["FLDDOCUMENTTYPE"].ToString();

            RadComboBox ddlNoofentryEdit = (RadComboBox)e.Item.FindControl("ddlNoofentryEdit");
            DataRowView drvNoofentry = (DataRowView)e.Item.DataItem;
            if (ddlNoofentryEdit != null) ddlNoofentryEdit.SelectedValue = drvNoofentry["FLDNOOFENTRY"].ToString();

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
            string passportno = ((RadTextBox)e.Item.FindControl("txtpassportnoEdit")).Text;

            if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry, country))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewTravelDocument.UpdateEmployeeTravelDocument(Convert.ToInt32(Filter.CurrentCrewSelection)
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
                                                                );

            BindData();
            gvTravelDocument.Rebind();
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
            string traveldocumentid = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;

            PhoenixCrewTravelDocument.DeleteEmployeeTravelDocument(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , Convert.ToInt32(traveldocumentid)
                                                                );
            BindData();
            gvTravelDocument.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelDocument_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvTravelDocument_EditCommand(object sender, GridCommandEventArgs e)
    {

    }
    
    public void UpdateEmployeePassport()
    {

        string pptno = RemoveSpecialCharacters(txtPassportnumber.Text);
        PhoenixCrewTravelDocument.UpdateEmployeePassport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , Convert.ToInt32(Filter.CurrentCrewSelection)
                                                            , Convert.ToDateTime(ucDateOfIssue.Text)
                                                            , txtPlaceOfIssue.Text
                                                            , Convert.ToDateTime(ucDateOfExpiry.Text)
                                                            , General.GetNullableInteger(ucECNR.SelectedHard)
                                                            , General.GetNullableInteger(ucBlankPages.SelectedHard)
                                                            , General.GetNullableString(pptno)
                                                            , Convert.ToInt16(chkpptVerifieddyn.Checked==true ? 1 : 0)
                                                            , Convert.ToInt16(chkcrosscheck.Checked == true ? 1 : 0)
                                                            );

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

    public void UpdateEmployeeSeamanBook()
    {

        try
        {
            PhoenixCrewTravelDocument.UpdateEmployeeSeamanBook(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , Convert.ToInt32(Filter.CurrentCrewSelection)
                                                                , Convert.ToInt32(ucSeamanCountry.SelectedFlag)
                                                                , Convert.ToDateTime(ucSeamanDateOfIssue.Text)
                                                                , txtSeamanPlaceOfIssue.Text
                                                                , Convert.ToDateTime(ucSeamanDateOfExpiry.Text)
                                                                , Convert.ToInt16(chkVerifiedYN.Checked==true ? 1 : 0)
                                                                , General.GetNullableString(txtSeamanBookNumber.Text)
                                                                 , Convert.ToInt16(chkcdccrosscheckedyn.Checked == true ? 1 : 0)
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

            PhoenixCrewTravelDocument.UpdateEmployeeUSVisa(Convert.ToInt32(Filter.CurrentCrewSelection)
                                                                , General.GetNullableString(txtUSVisaType.Text)
                                                                , General.GetNullableString(txtUSVisaNumber.Text)
                                                                , General.GetNullableDateTime(txtUSVisaIssuedOn.Text)
                                                                , General.GetNullableString(txtUSPlaceOfIssue.Text)
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
            PhoenixCrewTravelDocument.UpdateEmployeeMCVAustralia(Convert.ToInt32(Filter.CurrentCrewSelection)
                                                                , General.GetNullableString(txtMCVNumber.Text)
                                                                , General.GetNullableDateTime(txtMCVIssuedOn.Text)
                                                                , General.GetNullableDateTime(txtMCVDateofExpiry.Text)
                                                                , General.GetNullableString(txtMCVRemarks.Text)
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
        Int16 result;
        DateTime resultdate;

        if (txtSeamanBookNumber.Text == "")
            ucError.ErrorMessage = "Seaman's Book Number is required";

        else if (!IsValidSeamansBookTextBox(txtSeamanBookNumber.Text.Trim()))
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


    public static bool IsValidSeamansBookTextBox(string text)
    {

        string regex = "^[0-9a-zA-Z-]+$";
        Regex re = new Regex(regex);
        if (!re.IsMatch(text))
            return (false);

        return true;
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


    protected void SetEmployeePassportDetails()
    {
        DataTable dt = PhoenixCrewTravelDocument.ListEmployeePassport(Convert.ToInt32(Filter.CurrentCrewSelection));

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
            if (txtPassportnumber.Text != "")
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

    }

    protected void SetEmployeeSeamanBookDetails()
    {
        DataTable dt = PhoenixCrewTravelDocument.ListEmployeeSeamanBook(Convert.ToInt32(Filter.CurrentCrewSelection));

        string flag;

        if (dt.Rows.Count > 0)
        {
            txtSeamanBookNumber.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
            ucSeamanDateOfIssue.Text = dt.Rows[0]["FLDSDATEOFISSUE"].ToString();
            txtSeamanPlaceOfIssue.Text = dt.Rows[0]["FLDSPLACEOFISSUE"].ToString();
            ucSeamanDateOfExpiry.Text = dt.Rows[0]["FLDSDATEOFEXPIRY"].ToString();
            ucSeamanCountry.SelectedFlag = dt.Rows[0]["FLDSEAMANFLAG"].ToString();
            flag = dt.Rows[0]["FLDSEAMANFLAG"].ToString();
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

            if (txtSeamanBookNumber.Text != "")
            {
                imgSeamanBook.Visible = true;
            }
            else
            {
                imgSeamanBook.Visible = false;
            }
            txtUpdatedByCDC.Text = dt.Rows[0]["FLDCDCUPDATEDBY"].ToString();
            ucUpdatedDateCDC.Text = dt.Rows[0]["FLDCDCUPDATEDDATE"].ToString();

            if (flag != "")
            {
                DataSet ds = PhoenixRegistersThirdPartyLinks.ListThirdPartyLinks("CDC", General.GetNullableInteger(flag));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cdcChecker.InnerText = "\"" + ds.Tables[0].Rows[0]["FLDNATIONALITYNAME"].ToString() + " CDC\" Checker";
                    cdcChecker.HRef = ds.Tables[0].Rows[0]["FLDLINKNAME"].ToString().IndexOf("http://") > -1 ? ds.Tables[0].Rows[0]["FLDLINKNAME"].ToString() : "http://" + ds.Tables[0].Rows[0]["FLDLINKNAME"].ToString();
                }
                else
                {
                    cdcChecker.InnerText = "No CDC Checker";
                    cdcChecker.HRef = "";
                }
            }
            else
            {
                cdcChecker.InnerText = "No CDC Checker";
                cdcChecker.HRef = "";
            }

        }

    }
    public void SetEmployeeUSVisaDetails()
    {
        DataTable dt = PhoenixCrewTravelDocument.ListEmployeeUSVisa(Convert.ToInt32(Filter.CurrentCrewSelection));

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
    }
    public void SetEmployeeMCVDetails()
    {
        DataTable dt = PhoenixCrewTravelDocument.ListEmployeeMCVAustralia(Convert.ToInt32(Filter.CurrentCrewSelection));

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


    protected void ddlDocumentType_TextChanged(object sender, EventArgs e)
    {
        UserControlDocumentType dc = (UserControlDocumentType)sender;
        DataSet ds = new DataSet();
        UserControlDate date;
        if (dc.SelectedDocumentType != "Dummy")
        {
            ds = PhoenixRegistersDocumentOther.EditDocumentOther(General.GetNullableInteger(dc.SelectedDocumentType).Value);
        }
        if (ds.Tables.Count > 0)
        {
            if (dc.ID == "ucDocumentTypeAdd")
            {
                GridFooterItem Item = (GridFooterItem)dc.NamingContainer;
                date = (UserControlDate)Item.FindControl("ucDateExpiryAdd");
            }
            else
            {
                GridDataItem dataItem = (GridDataItem)dc.NamingContainer;
                date = (UserControlDate)dataItem.FindControl("ucDateExpiryEdit");
            }
            if (ds.Tables[0].Rows[0]["FLDHAVINGEXPIRY"].ToString() == "1")
            {
                date.CssClass = "input_mandatory";
            }
            else
            {
                date.CssClass = "";
            }
        }

    }

    public static string RemoveSpecialCharacters(string str)
    {
        // Strips all special characters and spaces from a string.
        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
    }


}
