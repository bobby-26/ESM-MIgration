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

public partial class CrewOtherMembership : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../Crew/CrewOtherMembership.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvOtherDocument')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Crew/CrewTravelDocumentArchived.aspx?empid=" + Filter.CurrentCrewSelection + "&type=2&t=p');return false", "Show Archived", "<i class=\"fas fa-archive\"></i>", "ARCHIVED");
            MenuCrewNewApplicantTravelDocument.AccessRights = this.ViewState;
            MenuCrewNewApplicantTravelDocument.MenuList = toolbargrid.Show();
          
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Other Documents", "ODOC");
            toolbar.AddButton("CBA/Other Membership", "CBA");          
            MenuOtherDocuments.AccessRights = this.ViewState;
            MenuOtherDocuments.MenuList = toolbar.Show();
            MenuOtherDocuments.SelectedMenuIndex = 1;
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void MenuOtherDocuments_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("ODOC"))
        {
            Response.Redirect("../Crew/CrewOtherDocument.aspx");
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
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
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
        string[] alCaptions = { "Document Name", "Number", "Date of Issue", "Date of Expiry", "Place of Issue", "Issuing Authority", "Remarks" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewTravelDocument.SearchEmployeeOtherDocument(Convert.ToInt32(Filter.CurrentCrewSelection), null, 1, 1,
                                                                              sortexpression, sortdirection,
                                                                            int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                           gvOtherDocument.PageSize,
                                                                            ref iRowCount,
                                                                            ref iTotalPageCount);
        General.ShowExcel("CBA/Other Membership", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDDOCUMENTNAME", "FLDDOCUMENTNUMBER", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDPLACEOFISSUE", "FLDISSUINGAUTHORITY", "FLDREMARKS" };
            string[] alCaptions = { "Document Name", "Number", "Date of Issue", "Date of Expiry", "Place of Issue", "Issuing Authority", "Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixCrewTravelDocument.SearchEmployeeOtherDocument(Convert.ToInt32(Filter.CurrentCrewSelection), null, 1, 1,
                                                                               sortexpression, sortdirection,
                                                                               int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                                gvOtherDocument.PageSize,
                                                                               ref iRowCount,
                                                                               ref iTotalPageCount);

            General.SetPrintOptions("gvOtherDocument", "CBA/Other Membership", alCaptions, alColumns, ds);

            gvOtherDocument.DataSource = ds;
            gvOtherDocument.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    public void InsertTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, string dateofexpiry, string issuingauthority)
    {
        PhoenixCrewTravelDocument.InsertEmployeeOtherDocument(Convert.ToInt32(Filter.CurrentCrewSelection)
                                                            , Convert.ToInt32(documenttype)
                                                            , documentnumber
                                                            , Convert.ToDateTime(dateofissue)
                                                            , placeofissue
                                                            , General.GetNullableDateTime(dateofexpiry)
                                                            , issuingauthority
                                                            , 1
                                                            );
    }
    public void UpdateTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, string dateofexpiry, string issuingauthority, string traveldocumentid)
    {
        PhoenixCrewTravelDocument.UpdateEmployeeOtherDocument(Convert.ToInt32(Filter.CurrentCrewSelection)
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



    protected void ddlDocumentTypeEdit_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            RadComboBox dc = (RadComboBox)sender;
            DataSet ds = new DataSet();
            UserControlDate date;
            if (dc.SelectedValue != "")
            {
                ds = PhoenixRegistersDocumentOther.EditDocumentOther(General.GetNullableInteger(dc.SelectedValue).Value);
            }
            if (dc.ID == "ddlDocumentTypeAdd" && ds.Tables.Count > 0)
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
                date.CssClass = "input_mandatory";
            else
                date.CssClass = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    protected void ddlDocumentTypeAdd_TextChanged(object sender, EventArgs e)
    {
        try
        {
            RadComboBox dc = (RadComboBox)sender;
            DataSet ds = new DataSet();
            UserControlDate date;
            if (dc.SelectedValue != "")
            {
                ds = PhoenixRegistersDocumentOther.EditDocumentOther(General.GetNullableInteger(dc.SelectedValue).Value);
            }
            if (dc.ID == "ddlDocumentTypeAdd" && ds.Tables.Count > 0)
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
                date.CssClass = "input_mandatory";
            else
                date.CssClass = "";
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
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string traveldocumentid = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;

                PhoenixNewApplicantTravelDocument.DeleteEmployeeTravelDocument(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , Convert.ToInt32(traveldocumentid)
                                                                    );
                BindData();
                gvOtherDocument.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string documenttype = ((RadComboBox)e.Item.FindControl("ddlDocumentTypeAdd")).SelectedValue;
                string documentnumber = ((RadTextBox)e.Item.FindControl("txtNumberAdd")).Text;
                string dateofissue = ((UserControlDate)e.Item.FindControl("ucDateOfIssueAdd")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceIssueAdd")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("ucDateExpiryAdd"));
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuingAuthorityAdd")).Text;
                if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry.Text, issuingauthority);
                BindData();
                gvOtherDocument.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string documenttype = ((RadComboBox)e.Item.FindControl("ddlDocumentTypeEdit")).SelectedValue;
                string documentnumber = ((RadTextBox)e.Item.FindControl("txtNumberEdit")).Text;
                string dateofissue = ((UserControlDate)e.Item.FindControl("ucDateOfIssueEdit")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssue")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("ucDateExpiryEdit"));
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuingAuthorityEdit")).Text;
                string traveldocumentid = ((RadLabel)e.Item.FindControl("lbltraveldocumentidEdit")).Text;
                if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry.Text, issuingauthority, traveldocumentid);
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

    protected void gvOtherDocument_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOtherDocument.CurrentPageIndex + 1;
            BindData();
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

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            RadLabel lnkNumber = (RadLabel)e.Item.FindControl("lnkNumber");
            if (ed != null)
            {
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
                lnkNumber.Enabled = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            }
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
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text.Trim() + "&mod=" + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + " &cmdname=OTHERDOCUPLOAD'); return false;");
            }
            LinkButton ad = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);

            RadLabel lbl = (RadLabel)e.Item.FindControl("lbltraveldocumentid");
            LinkButton img = (LinkButton)e.Item.FindControl("imgRemarks");
            RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
            if (img != null)
            {
                if (string.IsNullOrEmpty(lblR.Text.Trim()))
                {
                    HtmlGenericControl html = new HtmlGenericControl();                    
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses-nr\"></i></span>";
                    img.Controls.Add(html);
                }
                img.Attributes.Add("onclick", "javascript:openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "','xlarge')");
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
                        //e.Row.CssClass = "rowred";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }
            }
        }

        if (e.Item.IsInEditMode)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadComboBox ddlDocumentTypeEdit = e.Item.FindControl("ddlDocumentTypeEdit") as RadComboBox;
            if (ddlDocumentTypeEdit != null) ddlDocumentTypeEdit.SelectedValue = drv["FLDDOCUMENTTYPE"].ToString();

        }

        if (e.Item is GridFooterItem)
        {
            RadComboBox ddlDocumentTypeAdd = e.Item.FindControl("ddlDocumentTypeAdd") as RadComboBox;
            if (ddlDocumentTypeAdd != null)
            {
                ddlDocumentTypeAdd.Items.Insert(0, "--select--");
            }
        }
    }

    
}
