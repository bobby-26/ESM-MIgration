using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CrewFamilyTravelDocument : PhoenixBasePage
{
    string familyid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            familyid = Request.QueryString["familyid"];

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            Tabsub1.AccessRights = this.ViewState;
            Tabsub1.MenuList = toolbar.Show();
            
            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Family NOK", "FAMILYNOK");
            toolbar.AddButton("Sign On/Off", "SIGNON");
            toolbar.AddButton("Documents", "DOCUMENTS");        
            toolbar.AddButton("Travel", "TRAVEL");
            CrewFamilyTabs.AccessRights = this.ViewState;
            CrewFamilyTabs.MenuList = toolbar.Show();
            CrewFamilyTabs.SelectedMenuIndex = 2;

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Travel", "DOCUMENTS");
            toolbar.AddButton("Medical", "MEDICAL");
            toolbar.AddButton("Other", "OTHERDOCUMENT");
            CrewFamilyTravelDoc.AccessRights = this.ViewState;
            CrewFamilyTravelDoc.MenuList = toolbar.Show();
            CrewFamilyTravelDoc.SelectedMenuIndex = 0;

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewFamilyTravelDocument.aspx?familyid=" + familyid, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvTravelDocument')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewTravelDocumentArchived.aspx?empid=" + Filter.CurrentCrewSelection + "&type=1&t=p&familyid=" + familyid + "'); return false;", "Show Archived", "<i class=\"fas fa-archive\"></i>", "ARCHIVED");
            MenuCrewTravelDocument.AccessRights = this.ViewState;
            MenuCrewTravelDocument.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["EMPLOYEEFAMILYID"] = Request.QueryString["familyid"];
                ucECNR.HardTypeCode = ((int)PhoenixHardTypeCode.PASSPORTECNR).ToString();
                ucBlankPages.HardTypeCode = ((int)PhoenixHardTypeCode.PASSPORTECNR).ToString();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                SetEmployeePrimaryDetails();
                SetPassportDetails();
                imgPPClip.Attributes["onclick"] = "javascript:parent.openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                  + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.PASSPORT + "&cmdname=FPASSPORTUPLOAD'); return false;";

                gvTravelDocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewFamilyTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FAMILYNOK"))
            {
                Response.Redirect("CrewFamilyNok.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("SIGNON"))
            {
                Response.Redirect("CrewFamilySignOn.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("OTHERDOCUMENT"))
            {
                Response.Redirect("CrewFamilyOtherDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("TRAVEL"))
            {
                Response.Redirect("CrewFamilyTravel.aspx?familyid=" + Request.QueryString["familyid"] + "&from=familynok", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewFamilyTravelDoc_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidPassport())
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateFamilyPassportDetails();
                ucStatus.Text = "Family Member Information Updated.";
            }
            else if (CommandName.ToUpper().Equals("MEDICAL"))
            {
                Response.Redirect("CrewFamilyMedicalDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("DOCUMENTS"))
            {
                Response.Redirect("CrewFamilyTravelDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("OTHERDOCUMENT"))
            {
                Response.Redirect("CrewFamilyOtherDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewFamilyTravelSave_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
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
            ucError.ErrorMessage = "Passport Date of Issue should be earlier than current date";
        }
        //string A = ucDateOfExpiry.Text;
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
    private void UpdateFamilyPassportDetails()
    {


        PhoenixCrewFamilyNok.UpdateEmployeePassortFamily(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , Convert.ToInt32(Filter.CurrentCrewSelection)
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
    private void SetPassportDetails()
    {
        try
        {
            string familyid = null;
            if (ViewState["EMPLOYEEFAMILYID"] != null)
            {
                familyid = ViewState["EMPLOYEEFAMILYID"].ToString();
            }
            HtmlGenericControl html = new HtmlGenericControl();
            DataTable dt = PhoenixCrewFamilyNok.ListEmployeeFamily(Convert.ToInt32(Filter.CurrentCrewSelection), General.GetNullableInteger(familyid));

            if (dt.Rows.Count > 0)
            {

                txtPassportnumber.Text = dt.Rows[0]["FLDPASSPORTNUMBER"].ToString();
                ucDateOfIssue.Text = dt.Rows[0]["FLDDATEOFISSUE"].ToString();
                txtPlaceOfIssue.Text = dt.Rows[0]["FLDPLACEOSISSUE"].ToString();
                ucDateOfExpiry.Text = dt.Rows[0]["FLDDATEOFEXPIRY"].ToString();
                ucECNR.SelectedHard = dt.Rows[0]["FLDECNR"].ToString();
                ucBlankPages.SelectedHard = dt.Rows[0]["FLDMINIMUNPAGES"].ToString();
                if (dt.Rows[0]["FLDISATTACHMENT"].ToString() == string.Empty)
                { 
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    imgPPClip.Controls.Add(html);
                }
                else { 
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fas fa-paperclip\"></i></span>";
                    imgPPClip.Controls.Add(html);                
                }
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
        gvTravelDocument.Rebind();
        SetPassportDetails();
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewFamilyNok.ListEmployeeFamily(Convert.ToInt32(Filter.CurrentCrewSelection), General.GetNullableInteger(familyid));
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

        string[] alColumns = { "FLDDOCUMENTNAME", "FLDDOCUMENTNUMBER", "FLDDATEOFISSUE", "FLDVALIDFROM", "FLDDATEOFEXPIRY", "FLDNOOFENTRYNAME", "FLDPLACEOFISSUE", "FLDCOUNTRYNAME", "FLDREMARKS" };
        string[] alCaptions = { "Document Name", "Number", "Date of Issue", "Valid From", "Date of Expiry", "No of entry", "Place of Issue", "Nationality/Flag", "Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewTravelDocument.SearchEmployeeTravelDocument(Convert.ToInt32(Filter.CurrentCrewSelection), General.GetNullableInteger(familyid), 1,
                                                                              sortexpression, sortdirection,
                                                                            (int)ViewState["PAGENUMBER"],
                                                                            gvTravelDocument.PageSize,
                                                                            ref iRowCount,
                                                                            ref iTotalPageCount);
        General.ShowExcel("Crew Travel Document", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
            string[] alColumns = { "FLDDOCUMENTNAME", "FLDDOCUMENTNUMBER", "FLDDATEOFISSUE", "FLDVALIDFROM", "FLDDATEOFEXPIRY", "FLDNOOFENTRYNAME", "FLDPLACEOFISSUE", "FLDCOUNTRYNAME", "FLDREMARKS" };
            string[] alCaptions = { "Document Name", "Number", "Date of Issue", "Valid From", "Date of Expiry", "No of entry", "Place of Issue", "Nationality/Flag", "Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            //DataTable dst;
            DataSet ds = PhoenixCrewTravelDocument.SearchEmployeeTravelDocument(Convert.ToInt32(Filter.CurrentCrewSelection), General.GetNullableInteger(familyid), 1,
                                                                               sortexpression, sortdirection,
                                                                               Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                               gvTravelDocument.PageSize,
                                                                               ref iRowCount,
                                                                               ref iTotalPageCount);

            General.SetPrintOptions("gvTravelDocument", "Travel Documnet", alCaptions, alColumns, ds);

            //if (Filter.CurrentCrewSelection != null && Filter.CurrentCrewSelection != "")
            //{
            //    dst = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            //    if (dst.Rows.Count > 0)
            //    {

            //        TravelDocument.Text =  	"Travel Document" + " (" + dst.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dst.Rows[0]["FLDMIDDLENAME"].ToString() + " " + dst.Rows[0]["FLDLASTNAME"].ToString()
            //            + " [" + dst.Rows[0]["FLDRANKNAME"].ToString() + "] " + ")";
            //    }

            //}

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

                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('OpenHelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&cmdname=FTRAVELUPLOAD'); return false;");
            }

            RadLabel lbl = (RadLabel)e.Item.FindControl("lbltraveldocumentid");
            LinkButton img = (LinkButton)e.Item.FindControl("imgRemarks");
          
            if (lbl != null)
            {
                if (img != null)
                    img.Attributes.Add("onclick", "javascript:parent.openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "'); return true;");
                //img.Attributes.Add("onclick", "parent.openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "','xlarge')");
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
                
            }

            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            RadLabel expdate = e.Item.FindControl("lblDateofExpiry") as RadLabel;

            if (lbl != null)
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

        }

        if (e.Item.IsInEditMode)
        {
            UserControlCountry ucCountry = (UserControlCountry)e.Item.FindControl("ddlCountryEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucCountry != null) ucCountry.SelectedCountry = drv["FLDCOUNTRYCODE"].ToString();

            UserControlDocumentType ucDocumentType = (UserControlDocumentType)e.Item.FindControl("ucDocumentTypeEdit");
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

                if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry, country, noofentry))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry.Text, int.Parse(country), validfrom, noofentry);
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


    protected void gvTravelDocument_UpdateCommand(object sender, GridCommandEventArgs e)
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

            if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry, counry, noofentry))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }
            UpdateTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry.Text, int.Parse(counry), traveldocumentid, validfrom, noofentry);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

        BindData();
        gvTravelDocument.Rebind();
    }

    protected void gvTravelDocument_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void InsertTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, string dateofexpiry, int country, string validfrom, string noofentry)
    {
        PhoenixCrewFamilyNok.InsertEmployeeFamilyTravelDocument(Convert.ToInt32(Filter.CurrentCrewSelection), int.Parse(familyid)
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
    protected void UpdateTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, string dateofexpiry, int country, string traveldocumentid, string validfrom, string noofentry)
    {
        PhoenixCrewFamilyNok.UpdateEmployeeFamilyTravelDocument(Convert.ToInt32(Filter.CurrentCrewSelection)
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

        if (documenttype != "Dummy")
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
            GridFooterItem row = (GridFooterItem)dc.NamingContainer;

            date = (UserControlDate)row.FindControl("ucDateExpiryAdd");           
            ddlnoofentry = (RadComboBox)row.FindControl("ddlNoofentryAdd");
        }
        else
        {
            GridDataItem row = (GridDataItem)dc.NamingContainer;

            date = (UserControlDate)row.FindControl("ucDateExpiryEdit");
            ddlnoofentry = (RadComboBox)row.FindControl("ddlNoofentryEdit");
        }
        if (ds.Tables[0].Rows[0]["FLDHAVINGEXPIRY"].ToString() == "1")
        {
            date.CssClass = "input_mandatory";
        }
        else
        {
            date.CssClass = "";
        }
        if (ds.Tables[0].Rows[0]["FLDDOCUMENTNAME"].ToString() == "VISA")
        {
            ddlnoofentry.CssClass = "input_mandatory";
        }
        else
        {
            ddlnoofentry.CssClass = "";
        }
    }


    protected void gvTravelDocument_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }
}
