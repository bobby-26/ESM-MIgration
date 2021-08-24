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
using Telerik.Web.UI;

public partial class CrewNewApplicantTravelDocument : PhoenixBasePage
{
    string familyid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            familyid = Request.QueryString["familyid"];
            PhoenixToolbar maintoolbar = new PhoenixToolbar();
            maintoolbar.AddButton("Othr.Document", "OTHERDOCUMENT", ToolBarDirection.Right);
            maintoolbar.AddButton("Medical", "MEDICAL", ToolBarDirection.Right);
            maintoolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            maintoolbar.AddButton("Back", "NOK", ToolBarDirection.Right);
            CrewFamilyMedical.AccessRights = this.ViewState;
            CrewFamilyMedical.MenuList = maintoolbar.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewNewApplicantFamilyTravelDocument.aspx?familyid=" + familyid, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvTravelDocument')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewTravelDocumentArchived.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&type=1&t=n&familyid=" + familyid + "'); return false;", "Show Archived", "<i class=\"fas fa-archive\"></i>", "ARCHIVED");

            MenuCrewNewApplicantTravelDocument.AccessRights = this.ViewState;
            MenuCrewNewApplicantTravelDocument.MenuList = toolbargrid.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            if (!IsPostBack)
            {
                ViewState["EMPLOYEEFAMILYID"] = Request.QueryString["familyid"];
                ucECNR.HardTypeCode = ((int)PhoenixHardTypeCode.PASSPORTECNR).ToString();
                ucBlankPages.HardTypeCode = ((int)PhoenixHardTypeCode.PASSPORTECNR).ToString();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvTravelDocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                SetEmployeePrimaryDetails();
                SetPassportDetails();
                imgPPClip.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                  + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.PASSPORT + "&cmdname=NFPASSPORTUPLOAD'); return false;";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewFamilyMedical_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("NOK"))
            {
                Response.Redirect("CrewNewApplicantFamilyNok.aspx", false);
            }
            else if (CommandName.ToUpper().Equals("OTHERDOCUMENT"))
            {
                Response.Redirect("CrewNewApplicantFamilyOtherDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("MEDICAL"))
            {
                Response.Redirect("CrewNewApplicantFamilyMedicalDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidPassport())
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateFamilyPassportDetails();
                ucStatus.Text = "Family Member Information Updated.";
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

        if (ucDateOfIssue.Text == null)
            ucError.ErrorMessage = "Passport Date of Issue is required";
        else if (DateTime.TryParse(ucDateOfIssue.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Passport Issued should be earlier than current date";
        }

        if (ucDateOfExpiry.Text == null)
            ucError.ErrorMessage = "Passport Date of Expiry is required";
        else if (!string.IsNullOrEmpty(ucDateOfIssue.Text)
            && DateTime.TryParse(ucDateOfExpiry.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(ucDateOfIssue.Text)) < 0)
        {
            ucError.ErrorMessage = "Passport Expiry should be later than 'Issued'";
        }

        if (txtPlaceOfIssue.Text == "")
            ucError.ErrorMessage = "Passport Place of Issue is required";

        if (ucECNR.SelectedHard.Equals("") || !Int16.TryParse(ucECNR.SelectedHard, out result))
            ucError.ErrorMessage = "Passport ECNR  is required";

        if (ucBlankPages.SelectedHard.Equals("") || !Int16.TryParse(ucBlankPages.SelectedHard, out result))
            ucError.ErrorMessage = " Passport Minimum 4 Blank pages  is required";

        return (!ucError.IsError);

    }
    private void UpdateFamilyPassportDetails()
    {


        PhoenixNewApplicantFamilyNok.UpdateEmployeePassortFamily(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                                                                , Convert.ToInt32(ViewState["EMPLOYEEFAMILYID"].ToString())
                                                                , txtPassportnumber.Text
                                                                , General.GetNullableDateTime(ucDateOfIssue.Text)
                                                                , txtPlaceOfIssue.Text
                                                                , General.GetNullableDateTime(ucDateOfExpiry.Text)
                                                                , General.GetNullableInteger(ucECNR.SelectedHard)
                                                                , General.GetNullableInteger(ucBlankPages.SelectedHard)

                                                                );
        SetPassportDetails();
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantFamilyNok.ListEmployeeFamily(Convert.ToInt32(Filter.CurrentNewApplicantSelection), General.GetNullableInteger(familyid));
            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetPassportDetails()
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

                txtPassportnumber.Text = dt.Rows[0]["FLDPASSPORTNUMBER"].ToString();
                ucDateOfIssue.Text = dt.Rows[0]["FLDDATEOFISSUE"].ToString();
                txtPlaceOfIssue.Text = dt.Rows[0]["FLDPLACEOSISSUE"].ToString();
                ucDateOfExpiry.Text = dt.Rows[0]["FLDDATEOFEXPIRY"].ToString();
                ucECNR.SelectedHard = dt.Rows[0]["FLDECNR"].ToString();
                ucBlankPages.SelectedHard = dt.Rows[0]["FLDMINIMUNPAGES"].ToString();
                if (dt.Rows[0]["FLDISATTACHMENT"].ToString() == string.Empty)
                    imgPPClip.ImageUrl = Session["images"] + "/no-attachment.png";
                else
                    imgPPClip.ImageUrl = Session["images"] + "/attachment.png";
            }
            else
                imgPPClip.Visible = false;
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
        SetPassportDetails();
        gvTravelDocument.Rebind();
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

        string[] alColumns = { "FLDDOCUMENTNAME", "FLDDOCUMENTNUMBER", "FLDDATEOFISSUE", "FLDVALIDFROM", "FLDDATEOFEXPIRY", "FLDNOOFENTRYNAME", "FLDPLACEOFISSUE", "FLDCOUNTRYNAME", "FLDREMARKS" };
        string[] alCaptions = { "Document", "Number", "Issued", "Valid From", "Expiry", "No of entries", "Place of Issue", "Nationality/Flag", "Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewTravelDocument.SearchEmployeeTravelDocument(Convert.ToInt32(Filter.CurrentNewApplicantSelection), General.GetNullableInteger(familyid), 1,
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
            string[] alCaptions = { "Document", "Number", "Issued", "Valid From", "Expiry", "No of entries", "Place of Issue", "Nationality/Flag", "Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixCrewTravelDocument.SearchEmployeeTravelDocument(Convert.ToInt32(Filter.CurrentNewApplicantSelection), General.GetNullableInteger(familyid), 1,
                                                                               sortexpression, sortdirection,
                                                                               (int)ViewState["PAGENUMBER"],
                                                                               gvTravelDocument.PageSize,
                                                                               ref iRowCount,
                                                                               ref iTotalPageCount);

            General.SetPrintOptions("gvTravelDocument", "Travel Document", alCaptions, alColumns, ds);

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
    public void InsertTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, string dateofexpiry, int country, string validfrom, string noofentry)
    {
        PhoenixCrewFamilyNok.InsertEmployeeFamilyTravelDocument(Convert.ToInt32(Filter.CurrentNewApplicantSelection), int.Parse(familyid)
                                                            , Convert.ToInt32(documenttype)
                                                            , documentnumber
                                                            , Convert.ToDateTime(dateofissue)
                                                            , placeofissue
                                                            , General.GetNullableDateTime(dateofexpiry)
                                                            , country
                                                            , General.GetNullableDateTime(validfrom)
                                                            , General.GetNullableInteger(noofentry)
                                                            );
    }
    public void UpdateTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, string dateofexpiry, int country, string traveldocumentid, string validfrom, string noofentry)
    {
        PhoenixCrewFamilyNok.UpdateEmployeeFamilyTravelDocument(Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                                                            , Convert.ToInt32(traveldocumentid)
                                                            , Convert.ToInt32(documenttype)
                                                            , documentnumber
                                                            , Convert.ToDateTime(dateofissue)
                                                            , placeofissue
                                                            , General.GetNullableDateTime(dateofexpiry)
                                                            , country
                                                            , General.GetNullableDateTime(validfrom)
                                                            , General.GetNullableInteger(noofentry)
                                                            );
    }

    private bool IsValidTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, UserControlDate dateofexpiry, string country, string noofentry)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        Int16 result;
        DateTime resultdate;

        if (documenttype.Equals("") || !Int16.TryParse(documenttype, out result))
            ucError.ErrorMessage = "Document  is required";

        if (documentnumber.Trim() == "")
            ucError.ErrorMessage = "Number is required";

        if (!DateTime.TryParse(dateofissue, out resultdate))
            ucError.ErrorMessage = "Issued is required";
        else if (DateTime.TryParse(dateofissue, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issued should be earlier than current date";
        }

        if (placeofissue.Trim() == "")
            ucError.ErrorMessage = "Place of Issue is required";

        if (!DateTime.TryParse(dateofexpiry.Text, out resultdate) && dateofexpiry.CssClass == "input_mandatory")
            ucError.ErrorMessage = "Expiry is required";
        else if (DateTime.TryParse(dateofissue, out resultdate)
            && DateTime.TryParse(dateofexpiry.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(dateofissue)) < 0)
        {
            ucError.ErrorMessage = "Expiry should be later than 'Issued'";
        }
        if (country.Equals("") || !Int16.TryParse(country, out result))
            ucError.ErrorMessage = "Nationality/Flag  is required";

        if (documenttype != "Dummy" && documenttype != "")
        {
            DataSet ds = PhoenixRegistersDocumentOther.EditDocumentOther(General.GetNullableInteger(documenttype).Value);
            if (ds.Tables[0].Rows[0]["FLDDOCUMENTNAME"].ToString() == "VISA")
            {
                if (General.GetNullableInteger(noofentry) == null)
                {
                    ucError.ErrorMessage = "No of entries is required for Family Visa";
                }
            }
        }
        return (!ucError.IsError);
    }

    protected void ddlDocumentType_TextChanged(object sender, EventArgs e)
    {
        UserControlDocumentType dc = (UserControlDocumentType)sender;
        DataSet ds = new DataSet();
        UserControlDate date;
        RadComboBox ddlnoofentry;
        if (dc.SelectedDocumentType != "Dummy")
        {
            ds = PhoenixRegistersDocumentOther.EditDocumentOther(General.GetNullableInteger(dc.SelectedDocumentType).Value);
        }
        if (dc.ID == "ucDocumentTypeAdd" && ds.Tables.Count > 0)
        {
            GridFooterItem footer = (GridFooterItem)dc.NamingContainer;
            date = (UserControlDate)footer.FindControl("ucDateExpiryAdd");
            ddlnoofentry = (RadComboBox)footer.FindControl("ddlNoofentryAdd");
        }
        else
        {
            GridDataItem row = (GridDataItem)dc.NamingContainer;
            date = (UserControlDate)row.FindControl("ucDateExpiryEdit");
            ddlnoofentry = (RadComboBox)row.FindControl("ddlNoofentryEdit");
        }
        if (ds.Tables[0].Rows[0]["FLDHAVINGEXPIRY"].ToString() == "1")
            date.CssClass = "input_mandatory";
        else
            date.CssClass = "input";

        if (ds.Tables[0].Rows[0]["FLDDOCUMENTNAME"].ToString() == "VISA")
            ddlnoofentry.CssClass = "input_mandatory";
        else
            ddlnoofentry.CssClass = "input";
    }

    protected void gvTravelDocument_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTravelDocument.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvTravelDocument_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
                db1.Visible = SessionUtil.CanAccess(this.ViewState, db1.CommandName);
            }
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
                RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&cmdname=NFTRAVELDOCUPLOAD'); return false;");
            }
            RadLabel lbl = (RadLabel)e.Item.FindControl("lbltraveldocumentid");
            LinkButton img = (LinkButton)e.Item.FindControl("imgRemarks");
            if (lbl != null)
            {
                if (img != null)
                    img.Attributes.Add("onclick", "openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "','xlarge')");
            }
            RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
            if (string.IsNullOrEmpty(lblR.Text.Trim()))
            {
                if (img != null)
                {
                    HtmlGenericControl html = new HtmlGenericControl();                    
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses-nr\"></i></span>";
                    img.Controls.Add(html);
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
                }
            }
        }

        if (e.Item.IsInEditMode)
        {
            UserControlCountry ucCountry = (UserControlCountry)e.Item.FindControl("ddlCountryEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucCountry != null) ucCountry.SelectedCountry = drv["FLDCOUNTRYCODE"].ToString();

            UserControlDocumentType ucDocumentType = (UserControlDocumentType)e.Item.FindControl("ucDocumentTypeEdit");
            DataRowView drvDocumentType = (DataRowView)e.Item.DataItem;
            if (ucDocumentType != null)
            {
                ucDocumentType.DocumentTypeList = PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_VISA).ToString() + "," + ((int)PhoenixDocumentType.OTHER_DOC_CDC).ToString());
                if (ucDocumentType.SelectedDocumentType == "")
                    ucDocumentType.SelectedDocumentType = drvDocumentType["FLDDOCUMENTTYPE"].ToString();
            }

            RadComboBox ddlNoofentryEdit = (RadComboBox)e.Item.FindControl("ddlNoofentryEdit");
            DataRowView drvNoofentry = (DataRowView)e.Item.DataItem;
            if (ddlNoofentryEdit != null) ddlNoofentryEdit.SelectedValue = drvNoofentry["FLDNOOFENTRY"].ToString();
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ad = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
        }
    }

    protected void gvTravelDocument_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "SORT") return;

            if (e.CommandName.ToString().ToUpper() == "ADD")
            {
                GridFooterItem footer = (GridFooterItem)e.Item;

                gvTravelDocument.EditIndexes.Clear();
                gvTravelDocument.SelectedIndexes.Clear();
                string documenttype = ((UserControlDocumentType)footer.FindControl("ucDocumentTypeAdd")).SelectedDocumentType;
                string documentnumber = ((RadTextBox)footer.FindControl("txtNumberAdd")).Text;
                string dateofissue = ((UserControlDate)footer.FindControl("ucDateOfIssueAdd")).Text;
                string placeofissue = ((RadTextBox)footer.FindControl("txtPlaceIssueAdd")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)footer.FindControl("ucDateExpiryAdd"));
                string country = ((UserControlCountry)footer.FindControl("ddlCountryAdd")).SelectedCountry;
                string validfrom = ((UserControlDate)footer.FindControl("ucValidFromAdd")).Text;
                string noofentry = ((RadComboBox)footer.FindControl("ddlNoofentryAdd")).SelectedValue;
                if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry, country, noofentry))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    if (gvTravelDocument.EditIndexes != null)
                    {
                        gvTravelDocument.EditIndexes.Clear();
                        BindData();
                        gvTravelDocument.Rebind();
                    }
                    return;
                }
                InsertTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry.Text, int.Parse(country), validfrom, noofentry);
                BindData();
                gvTravelDocument.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "ARCHIVE")
            {
                string id = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;
                PhoenixNewApplicantTravelDocument.ArchiveEmployeeTravelDocument(int.Parse(id), 0);
                gvTravelDocument.EditIndexes.Clear();
                gvTravelDocument.SelectedIndexes.Clear();
                BindData();
                gvTravelDocument.Rebind();
            }
            else if(e.CommandName.ToString().ToUpper()=="DELETE")
            {
                string traveldocumentid = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;

                PhoenixNewApplicantTravelDocument.DeleteEmployeeTravelDocument(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , Convert.ToInt32(traveldocumentid)
                                                                    );
                gvTravelDocument.EditIndexes.Clear();
                gvTravelDocument.SelectedIndexes.Clear();
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
}
