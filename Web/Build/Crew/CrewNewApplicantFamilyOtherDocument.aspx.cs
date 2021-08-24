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

public partial class CrewNewApplicantFamilyOtherDocument : PhoenixBasePage
{
    string strFamilyId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            strFamilyId = Request.QueryString["familyid"];
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar maintoolbar = new PhoenixToolbar();
            maintoolbar.AddButton("Trvl Document", "DOCUMENTS", ToolBarDirection.Right);
            maintoolbar.AddButton("Medical", "MEDICAL", ToolBarDirection.Right);
            maintoolbar.AddButton("Back", "NOK", ToolBarDirection.Right);

            CrewFamilyMedical.AccessRights = this.ViewState;
            CrewFamilyMedical.MenuList = maintoolbar.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewNewApplicantFamilyOtherDocument.aspx?familyid=" + strFamilyId, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvOtherDocument')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewTravelDocumentArchived.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&type=2&t=n&familyid=" + strFamilyId + "'); return false;", "Show Archived", "<i class=\"fas fa-archive\"></i>", "ARCHIVED");
            MenuCrewNewApplicantTravelDocument.AccessRights = this.ViewState;
            MenuCrewNewApplicantTravelDocument.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

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
    protected void CrewFamilyDocument_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("NOK"))
            {
                Response.Redirect("CrewNewApplicantFamilyNok.aspx", false);
            }
            else if (CommandName.ToUpper().Equals("MEDICAL"))
            {
                Response.Redirect("CrewNewApplicantFamilyMedicalDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
            }
            else if (CommandName.ToUpper().Equals("DOCUMENTS"))
            {
                Response.Redirect("CrewNewApplicantFamilyTravelDocument.aspx?familyid=" + Request.QueryString["familyid"], false);
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

            DataTable dt = PhoenixCrewFamilyNok.ListEmployeeFamily(Convert.ToInt32(Filter.CurrentNewApplicantSelection), General.GetNullableInteger(strFamilyId));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtEmployeeMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtEmployeeLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
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
        string[] alCaptions = { "Document", "Number", "Issued on", "Expiry", "Place of Issue", "Issuing Authority", "Remarks" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewTravelDocument.SearchEmployeeOtherDocument(Convert.ToInt32(Filter.CurrentNewApplicantSelection), General.GetNullableInteger(strFamilyId), 1, 0,
                                                                              sortexpression, sortdirection,
                                                                            (int)ViewState["PAGENUMBER"],
                                                                            General.ShowRecords(null),
                                                                            ref iRowCount,
                                                                            ref iTotalPageCount);
        General.ShowExcel("Other Document", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDDOCUMENTNAME", "FLDDOCUMENTNUMBER", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDPLACEOFISSUE", "FLDISSUINGAUTHORITY", "FLDREMARKS" };
            string[] alCaptions = { "Document", "Number", "Issued on", "Expiry", "Place of Issue", "Issuing Authority", "Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixCrewTravelDocument.SearchEmployeeOtherDocument(Convert.ToInt32(Filter.CurrentNewApplicantSelection), General.GetNullableInteger(strFamilyId), 1, 0,
                                                                               sortexpression, sortdirection,
                                                                               (int)ViewState["PAGENUMBER"],
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
    public void InsertTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, string dateofexpiry, string issuingauthority)
    {
        PhoenixCrewTravelDocument.InsertEmployeeOtherDocumentFamily(Convert.ToInt32(Filter.CurrentNewApplicantSelection), int.Parse(strFamilyId)
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
        PhoenixCrewTravelDocument.UpdateEmployeeOtherDocumentFamily(Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                                                            , Convert.ToInt32(traveldocumentid)
                                                            , Convert.ToInt32(documenttype)
                                                            , documentnumber
                                                            , Convert.ToDateTime(dateofissue)
                                                            , placeofissue
                                                            , General.GetNullableDateTime(dateofexpiry)
                                                            , issuingauthority
                                                            );
    }

    private bool IsValidTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, UserControlDate dateofexpiry)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        Int16 result;
        DateTime resultdate;

        if (documenttype.Equals("") || !Int16.TryParse(documenttype, out result))
            ucError.ErrorMessage = "Document   is required";

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
            ucError.ErrorMessage = "Expiry  is required";
        else if (DateTime.TryParse(dateofissue, out resultdate)
            && DateTime.TryParse(dateofexpiry.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(dateofissue)) < 0)
        {
            ucError.ErrorMessage = "Expiry should be later than 'Issued'";
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
            GridFooterItem footer = (GridFooterItem)dc.NamingContainer;
            date = (UserControlDate)footer.FindControl("ucDateExpiryAdd");

        }
        else
        {
            GridDataItem row = (GridDataItem)dc.NamingContainer;
            date = (UserControlDate)row.FindControl("ucDateExpiryEdit");
        }
        if (ds.Tables[0].Rows[0]["FLDHAVINGEXPIRY"].ToString() == "1")
            date.CssClass = "input_mandatory";
        else
            date.CssClass = "input";
    }

    protected void gvOtherDocument_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOtherDocument.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvOtherDocument_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
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
                att.Attributes.Add("onclick", "javascript:openNewWindow('NATD','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&cmdname=NFODOCUPLOAD'); return false;");
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
        }
        if (e.Item.IsInEditMode)
        {
            UserControlDocumentType ucDocumentType = (UserControlDocumentType)e.Item.FindControl("ucDocumentTypeEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucDocumentType != null)
            {
                ucDocumentType.DocumentTypeList = PhoenixRegistersDocumentOther.ListDocumentOther(((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString(), 1);
                if (ucDocumentType.SelectedDocumentType == "")
                    ucDocumentType.SelectedDocumentType = drv["FLDDOCUMENTTYPE"].ToString();
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ad = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
        }
    }

    protected void gvOtherDocument_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "SORT") return;
            if (e.CommandName.ToString().ToUpper() == "ADD")
            {
                gvOtherDocument.EditIndexes.Clear();
                gvOtherDocument.SelectedIndexes.Clear();

                string documenttype = ((UserControlDocumentType)e.Item.FindControl("ucDocumentTypeAdd")).SelectedDocumentType;
                string documentnumber = ((RadTextBox)e.Item.FindControl("txtNumberAdd")).Text;
                string dateofissue = ((UserControlDate)e.Item.FindControl("ucDateOfIssueAdd")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceIssueAdd")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("ucDateExpiryAdd"));
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuingAuthorityAdd")).Text;
                if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry))
                {
                    ucError.Visible = true;
                    e.Canceled = true;
                    if (gvOtherDocument.EditIndexes != null)
                    {
                        gvOtherDocument.EditIndexes.Clear();
                        BindData();

                    }
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
                gvOtherDocument.EditIndexes.Clear();
                gvOtherDocument.SelectedIndexes.Clear();
                BindData();
                gvOtherDocument.Rebind();
            }
            else if(e.CommandName.ToString().ToUpper()=="DELETE")
            {
                string traveldocumentid = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;

                PhoenixNewApplicantTravelDocument.DeleteEmployeeTravelDocument(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , Convert.ToInt32(traveldocumentid)
                                                                    );
                gvOtherDocument.EditIndexes.Clear();
                gvOtherDocument.SelectedIndexes.Clear();
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
}
