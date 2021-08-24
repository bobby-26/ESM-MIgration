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

public partial class CrewFamilyOtherDocument : PhoenixBasePage
{
    string strFamilyId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            strFamilyId = Request.QueryString["familyid"];
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmenu = new PhoenixToolbar();
            toolbarmenu.AddButton("Family NOK", "FAMILYNOK");
            toolbarmenu.AddButton("Sign On/Off", "SIGNON");
            toolbarmenu.AddButton("Documents", "DOCUMENTS");
            toolbarmenu.AddButton("Travel", "TRAVEL");
            CrewFamilyTabs.AccessRights = this.ViewState;
            CrewFamilyTabs.MenuList = toolbarmenu.Show();
            CrewFamilyTabs.SelectedMenuIndex = 2;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Travel", "DOCUMENTS");
            toolbar.AddButton("Medical", "MEDICAL");
            toolbar.AddButton("Other", "OTHERDOCUMENT");
            CrewFamilyotherDoc.AccessRights = this.ViewState;
            CrewFamilyotherDoc.MenuList = toolbar.Show();
            CrewFamilyotherDoc.SelectedMenuIndex = 2;

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewFamilyOtherDocument.aspx?familyid=" + strFamilyId, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvOtherDocument')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewTravelDocumentArchived.aspx?empid=" + Filter.CurrentCrewSelection + "&type=2&t=p&familyid=" + strFamilyId + "'); return false;", "Show Archived", "<i class=\"fas fa-archive\"></i>", "ARCHIVED");
            MenuCrewNewApplicantTravelDocument.AccessRights = this.ViewState;
            MenuCrewNewApplicantTravelDocument.MenuList = toolbargrid.Show();


            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                SetEmployeePrimaryDetails();

                gvOtherDocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

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
            else if (CommandName.ToUpper().Equals("DOCUMENTS"))
            {
                Response.Redirect("CrewFamilyTravelDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("MEDICAL"))
            {
                Response.Redirect("CrewFamilyMedicalDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
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
    protected void CrewFamilyotherDoc_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("MEDICAL"))
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }


    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewFamilyNok.ListEmployeeFamily(Convert.ToInt32(Filter.CurrentCrewSelection), General.GetNullableInteger(strFamilyId));
            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

        string[] alColumns = { "FLDDOCUMENTNAME", "FLDDOCUMENTNUMBER", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDPLACEOFISSUE", "FLDISSUINGAUTHORITY", "FLDREMARKS" };
        string[] alCaptions = { "Document Name", "Number", "Issued", "Expiry", "Place of Issue", "Issuing Authority", "Remarks" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewTravelDocument.SearchEmployeeOtherDocument(Convert.ToInt32(Filter.CurrentCrewSelection), General.GetNullableInteger(strFamilyId), 1, 0,
                                                                              sortexpression, sortdirection,
                                                                            (int)ViewState["PAGENUMBER"],
                                                                             gvOtherDocument.PageSize,
                                                                            ref iRowCount,
                                                                            ref iTotalPageCount);
        General.ShowExcel("Other Document", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }


    protected void gvOtherDocument_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOtherDocument.CurrentPageIndex + 1;

        BindData();
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDDOCUMENTNAME", "FLDDOCUMENTNUMBER", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDPLACEOFISSUE", "FLDISSUINGAUTHORITY", "FLDREMARKS" };
            string[] alCaptions = { "Document Name", "Number", "Issued", "Expiry", "Place of Issue", "Issuing Authority", "Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewTravelDocument.SearchEmployeeOtherDocument(Convert.ToInt32(Filter.CurrentCrewSelection), General.GetNullableInteger(strFamilyId), 1, 0,
                                                                               sortexpression, sortdirection,
                                                                               Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                               gvOtherDocument.PageSize,
                                                                               ref iRowCount,
                                                                               ref iTotalPageCount);

            General.SetPrintOptions("gvOtherDocument", "Other Document", alCaptions, alColumns, ds);

            gvOtherDocument.DataSource = ds;
            gvOtherDocument.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    protected void gvOtherDocument_ItemDataBound(object sender, GridItemEventArgs e)
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
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&cmdname=FODOCUUPLOAD'); return false;");
            }
            
            RadLabel lbl = (RadLabel)e.Item.FindControl("lbltraveldocumentid");
            LinkButton img = (LinkButton)e.Item.FindControl("imgRemarks");

            if (lbl != null)
            {
                if (img != null)
                    img.Attributes.Add("onclick", "parent.openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "','xlarge')");
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
            DataRowView drv = (DataRowView)e.Item.DataItem;
         
            UserControlDocumentType ucDocumentType = (UserControlDocumentType)e.Item.FindControl("ucDocumentTypeEdit");
            if (ucDocumentType != null) ucDocumentType.SelectedDocumentType = drv["FLDDOCUMENTTYPE"].ToString();

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
                string lblHavingExpiryAdd = ((RadLabel)e.Item.FindControl("lblHavingExpiryAdd")).Text;

                if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry, lblHavingExpiryAdd))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry.Text, issuingauthority);
                BindData();
                gvOtherDocument.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "ARCHIVE")
            {
                string id = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;
                PhoenixNewApplicantTravelDocument.ArchiveEmployeeTravelDocument(int.Parse(id), 0);
                BindData();
                gvOtherDocument.Rebind();
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

    protected void gvOtherDocument_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string documenttype = ((UserControlDocumentType)e.Item.FindControl("ucDocumentTypeEdit")).SelectedDocumentType;
            string documentnumber = ((RadTextBox)e.Item.FindControl("txtNumberEdit")).Text;
            string dateofissue = ((UserControlDate)e.Item.FindControl("ucDateOfIssueEdit")).Text;
            string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssue")).Text;
            UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("ucDateExpiryEdit"));
            string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuingAuthorityEdit")).Text;
            string traveldocumentid = ((RadLabel)e.Item.FindControl("lbltraveldocumentidEdit")).Text;
            string lblHavingExpiryEdit = ((RadLabel)e.Item.FindControl("lblHavingExpiryEdit")).Text;

            if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry, lblHavingExpiryEdit))
            {
                ucError.Visible = true;
                return;
            }

            UpdateTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry.Text, issuingauthority, traveldocumentid);

            BindData();
            gvOtherDocument.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }

    protected void gvOtherDocument_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string traveldocumentid = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;

            PhoenixNewApplicantTravelDocument.DeleteEmployeeTravelDocument(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , Convert.ToInt32(traveldocumentid)
                                                                );
            BindData();
            gvOtherDocument.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvOtherDocument_SortCommand(object sender, GridSortCommandEventArgs e)
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

    
    public void InsertTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, string dateofexpiry, string issuingauthority)
    {
        PhoenixCrewTravelDocument.InsertEmployeeOtherDocumentFamily(Convert.ToInt32(Filter.CurrentCrewSelection), int.Parse(strFamilyId)
                                                            , Convert.ToInt32(documenttype)
                                                            , documentnumber
                                                            , Convert.ToDateTime(dateofissue)
                                                            , placeofissue
                                                            , General.GetNullableDateTime(dateofexpiry)
                                                            , issuingauthority
                                                            );
    }
    public void UpdateTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, string dateofexpiry, string issuingauthority, string traveldocumentid)
    {
        PhoenixCrewTravelDocument.UpdateEmployeeOtherDocumentFamily(Convert.ToInt32(Filter.CurrentCrewSelection)
                                                            , Convert.ToInt32(traveldocumentid)
                                                            , Convert.ToInt32(documenttype)
                                                            , documentnumber
                                                            , Convert.ToDateTime(dateofissue)
                                                            , placeofissue
                                                            , General.GetNullableDateTime(dateofexpiry)
                                                            , issuingauthority
                                                            );
    }

    private bool IsValidTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, UserControlDate dateofexpiry, string havingexpiry)
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

        if (!DateTime.TryParse(dateofexpiry.Text, out resultdate) && havingexpiry == "1")
            ucError.ErrorMessage = "Expiry Date is required";
        else if (DateTime.TryParse(dateofissue, out resultdate)
            && DateTime.TryParse(dateofexpiry.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(dateofissue)) < 0)
        {
            ucError.ErrorMessage = "Date of Expiry should be later than 'Date of Issue'";
        }

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
            GridFooterItem row = (GridFooterItem)dc.NamingContainer;

            date = (UserControlDate)row.FindControl("ucDateExpiryAdd");
        }
        else
        {
            GridDataItem row = (GridDataItem)dc.NamingContainer;

            date = (UserControlDate)row.FindControl("ucDateExpiryEdit");
        }
        if (ds.Tables[0].Rows[0]["FLDHAVINGEXPIRY"].ToString() == "1")
            date.CssClass = "input_mandatory";
        else
            date.CssClass = "";
    }


}
