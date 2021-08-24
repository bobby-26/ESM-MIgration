using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections.Specialized;
using System.Collections;
using Telerik.Web.UI;

public partial class DocumentManagementAdminDocumentSectionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            confirm.Attributes.Add("style", "display:none;");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            //toolbar.AddImageButton("../DocumentManagement/DocumentManagementAdminDocumentSectionList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementAdminDocumentSectionList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDocument')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementAdminDocumentSectionList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementAdminDocumentSectionList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");


            MenuRegistersCountry.AccessRights = this.ViewState;
            MenuRegistersCountry.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Section", "SECTION", ToolBarDirection.Right);
            toolbar.AddButton("Document", "DOCUMENT", ToolBarDirection.Right);


            if (!IsPostBack)
            {
                Filter.CurrentSelectedDocumentSections = null;
                //ucConfirm.Visible = false;
                //ucConfirmApprove.Visible = false;
                //ucConfirmStatus.Visible = false;

                if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != "")
                {
                    ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();                    
                }
                else
                    ViewState["DOCUMENTID"] = "";

                if (Request.QueryString["SECTIONID"] != null && Request.QueryString["SECTIONID"].ToString() != "")
                {
                    ViewState["SECTIONID"] = Request.QueryString["SECTIONID"].ToString();
                    hiddenkey.Value = Request.QueryString["SECTIONID"].ToString();
                }
                else
                    ViewState["SECTIONID"] = "";

                if (Request.QueryString["DOCUMENTTYPE"] != null && Request.QueryString["DOCUMENTTYPE"].ToString() != "")
                {
                    ViewState["DOCUMENTTYPE"] = Request.QueryString["DOCUMENTTYPE"].ToString();
                }
                else
                    ViewState["DOCUMENTTYPE"] = "";

                if (Request.QueryString["SECTIONNO"] != null && Request.QueryString["SECTIONNO"].ToString() != "")
                {
                    ViewState["SECTIONNO"] = Request.QueryString["SECTIONNO"].ToString();
                }
                else
                    ViewState["SECTIONNO"] = "";

                if (Filter.CurrentDMSDocumentFilter != null)
                {
                    NameValueCollection nvc = Filter.CurrentDMSDocumentSectionFilter;

                    if (nvc != null)
                    {
                        if (nvc.Get("txtSectionNo") != null) txtSectionNo.Text = nvc.Get("txtSectionNo").ToString();
                        if (nvc.Get("txtName") != null) txtName.Text = nvc.Get("txtName").ToString();
                        if (nvc.Get("ddlStatus") != null) ddlStatus.SelectedValue = nvc.Get("ddlStatus").ToString();
                    }
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["REVISIONSTATUS"] = "0";

                gvDocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
            GetDocumentName();

            MenuDocument.AccessRights = this.ViewState;
            MenuDocument.MenuList = toolbar.Show();
            MenuDocument.SelectedMenuIndex = 0;

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Approve", "BULKAPPROVE", ToolBarDirection.Right);
            MenuDocumentGeneral.AccessRights = this.ViewState;
            MenuDocumentGeneral.MenuList = toolbar.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void RegistersCountry_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {

                Filter.CurrentDMSDocumentSectionFilter = null;

                gvDocument.CurrentPageIndex = 0; 

                NameValueCollection criteria = new NameValueCollection();

                criteria.Add("txtSectionNo", txtSectionNo.Text.Trim());
                criteria.Add("txtName", txtName.Text.Trim());
                criteria.Add("ddlStatus", ddlStatus.SelectedValue);
                Filter.CurrentDMSDocumentSectionFilter = criteria;
                gvDocument.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtSectionNo.Text = "";
                txtName.Text = "";
                ddlStatus.SelectedValue = "";
                Filter.CurrentDMSDocumentSectionFilter = null;
                gvDocument.CurrentPageIndex = 0; 

                gvDocument.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuDocumentGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("BULKAPPROVE"))
            {
                RadWindowManager1.RadConfirm("Are you sure you want to approve all the selected sections.? Y/N", "confirm", 320, 150, null, "Confirm");
                //gvDocument.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDocument_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("DOCUMENT"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentList.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"]);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private void GetDocumentName()
    {
        if (ViewState["DOCUMENTID"] != null && ViewState["DOCUMENTID"].ToString() != "")
        {
            DataSet ds = PhoenixDocumentManagementDocument.DocumentEdit(new Guid(ViewState["DOCUMENTID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                MenuDocument.Title = dr["FLDDOCUMENTNAME"].ToString();
            }
        }
    }

    protected void gvDocument_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSECTIONNUMBER", "FLDSECTIONNAME", "FLDACTIVESTATUS", "FLDREVISIONDETAILS", "FLDDRAFTREVISION", "FLDSTATUSNAME", "FLDOFFICEREMARKS" };
        string[] alCaptions = { "Section", "Name", "Active", "Revision", "Draft", "Status", "Office Remarks" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentDMSDocumentFilter;

        string documentid = (nvc != null && nvc.Get("ddlDocument") != null) ? nvc.Get("ddlDocument").ToString() : "";
        string sectionno = "";
        if (General.GetNullableGuid(documentid) == null)
            documentid = ViewState["DOCUMENTID"].ToString();
        if (ViewState["SECTIONNO"].ToString() != "")
        {
            sectionno = ViewState["SECTIONNO"].ToString();
        }
        else
        {
            sectionno = txtSectionNo.Text.Trim();
        }

        ds = PhoenixDocumentManagementDocumentSection.DocumentSectionSearch(
                                                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(documentid)
                                                                , General.GetNullableString(txtName.Text.Trim())
                                                                , General.GetNullableString(sectionno)
                                                                , null
                                                                , null
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvDocument.CurrentPageIndex + 1
                                                                , gvDocument.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , General.GetNullableInteger(ddlStatus.SelectedValue)
                                                                , General.GetNullableGuid(hiddenkey.Value)
                                                                );

        hiddenkey.Value = "";

        General.SetPrintOptions("gvDocument", MenuDocument.Title + " - Sections", alCaptions, alColumns, ds);
        gvDocument.DataSource = ds;
        gvDocument.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (General.GetNullableGuid(ViewState["SECTIONID"].ToString()) == null)
            {
                ViewState["SECTIONID"] = ds.Tables[0].Rows[0]["FLDSECTIONID"].ToString();
                ViewState["REVISIONSTATUS"] = ds.Tables[0].Rows[0]["FLDSTATUSID"].ToString();
            }
        }
        // SetRowSelection();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDSECTIONNUMBER", "FLDSECTIONNAME", "FLDACTIVESTATUS", "FLDREVISIONDETAILS", "FLDDRAFTREVISION", "FLDSTATUSNAME", "FLDOFFICEREMARKS" };
        string[] alCaptions = { "Section", "Name", "Active", "Revision", "Draft", "Status", "Office Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentDMSDocumentFilter;

        string sectionno = "";
        string documentid = (nvc != null && nvc.Get("ddlDocument") != null) ? nvc.Get("ddlDocument").ToString() : "";

        if (General.GetNullableGuid(documentid) == null)
            documentid = ViewState["DOCUMENTID"].ToString();

        if (ViewState["SECTIONNO"].ToString() != "")
        {
            sectionno = ViewState["SECTIONNO"].ToString();
        }
        else
        {
            sectionno = txtSectionNo.Text.Trim();
        }

        ds = PhoenixDocumentManagementDocumentSection.DocumentSectionSearch(
                                                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(documentid)
                                                                , General.GetNullableString(txtName.Text.Trim())
                                                                , General.GetNullableString(sectionno)
                                                                , null
                                                                , null
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , General.GetNullableInteger(ddlStatus.SelectedValue)
                                                                , General.GetNullableGuid(hiddenkey.Value)
                                                                );

        Response.AddHeader("Content-Disposition", "attachment; filename=DocumentSection.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td colspan='" + (alColumns.Length).ToString() + "'><h3>" + MenuDocument.Title + " - Sections" + "</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void gvDocument_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;

            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("ROWCLICK"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                Guid documentid = new Guid(item.GetDataKeyValue("FLDSECTIONID").ToString());
                ////gvDocument.SelectedIndexes.Add(e.Item.ItemIndex);
                //BindPageURL(e.Item.ItemIndex);
            }

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(nCurrentRow);
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                BindPageURL(nCurrentRow);
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                if (!IsValidDocument(
                    ((RadTextBox)item.FindControl("txtSectionNameAdd")).Text,
                    ((RadTextBox)item.FindControl("txtSectionNumberAdd")).Text,
                     ViewState["DOCUMENTID"].ToString())
                    )
                {
                    ucError.Visible = true;
                    return;
                }
                InsertDocumentSection(
                    ((RadTextBox)item.FindControl("txtSectionNameAdd")).Text
                    , ViewState["DOCUMENTID"].ToString()
                    , (((RadCheckBox)item.FindControl("chkActiveYNAdd")).Checked == true) ? 1 : 0
                    , ((RadTextBox)item.FindControl("txtSectionNumberAdd")).Text
                    , ((RadTextBox)item.FindControl("txtVersionNumberAdd")).Text
                );

                ucStatus.Text = "Section is added.";
                ViewState["SECTIONID"] = "";
                gvDocument.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDocumentSection((((RadLabel)eeditedItem.FindControl("lblSectionId")).Text));
                ucStatus.Text = "Section is deleted.";
                ViewState["SECTIONID"] = "";
                gvDocument.Rebind();
            }
            //else if (e.CommandName.ToUpper().Equals("EDITDOCUMENT"))
            //{
            //    BindPageURL(nCurrentRow);
            //    Response.Redirect("../DocumentManagement/DocumentManagementDocumentSectionContentGeneral.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + ViewState["SECTIONID"].ToString());
            //}
            //else if (e.CommandName.ToUpper().Equals("VIEWDOCUMENT"))
            //{
            //    BindPageURL(nCurrentRow);
            //    Response.Redirect("../DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + ViewState["SECTIONID"].ToString());
            //}
            else if (e.CommandName.ToUpper().Equals("VIEWREVISON"))
            {
                BindPageURL(nCurrentRow);
                Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentSectionRevisionList.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + ViewState["SECTIONID"].ToString());
            }
            else if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                BindPageURL(nCurrentRow);

                RadLabel lblDraftRevisionId = ((RadLabel)eeditedItem.FindControl("lblDraftRevisionId"));

                PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonApprove(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["SECTIONID"].ToString())
                        , new Guid(ViewState["DOCUMENTID"].ToString())
                        , General.GetNullableGuid(lblDraftRevisionId != null ? lblDraftRevisionId.Text : "")
                        );

                ucStatus.Text = "Revision is approved.";
                BindData();
                gvDocument.Rebind();

                //RadLabel lblRevisionStatus = (RadLabel)eeditedItem.FindControl("lblRevisionStatus");

                //if (lblRevisionStatus.Text != "3" && lblRevisionStatus.Text != "4")
                //{
                //    ucConfirm.Visible = true;
                //    ucConfirm.Text = "Are you sure you want to Approve this draft.? Y/N";
                //    return;
                //}
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvDocument_RowDeleting(object sender, GridDeletedEventArgs de)
    {
        gvDocument.Rebind();
    }

    protected void gvDocument_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem item = (GridEditableItem)e.Item;

            if (!IsValidDocument(
                     ((RadTextBox)item.FindControl("txtSectionNameEdit")).Text,
                     ((RadTextBox)item.FindControl("txtSectionNumberEdit")).Text,
                     ViewState["DOCUMENTID"].ToString())
                )
            {
                ucError.Visible = true;
                return;
            }

            int? oldrevisionstatus = General.GetNullableInteger(ViewState["REVISIONSTATUS"].ToString());
            int? newstatus = General.GetNullableInteger(((RadComboBox)item.FindControl("ddlRevisionStatusEdit")).SelectedValue);

            UpdateDocumentSection(
                      ((RadTextBox)item.FindControl("txtSectionNameEdit")).Text
                    , ViewState["DOCUMENTID"].ToString()
                    , ((RadLabel)item.FindControl("lblSectionIdEdit")).Text
                    , (((RadCheckBox)item.FindControl("chkActiveYNEdit")).Checked == true) ? 1 : 0
                    , ((RadTextBox)item.FindControl("txtSectionNumberEdit")).Text
                    , General.GetNullableInteger(((RadComboBox)item.FindControl("ddlRevisionStatusEdit")).SelectedValue)
                 );
            ucStatus.Text = "Section details updated.";
            gvDocument.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDocument_ItemDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;

            LinkButton del = (LinkButton)item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmTelerik(event); return false;");
            }

            if (sb != null)
            {
                int? oldrevisionstatus = General.GetNullableInteger(ViewState["REVISIONSTATUS"].ToString());
                int? newstatus = General.GetNullableInteger(((RadComboBox)item.FindControl("ddlRevisionStatusEdit")).SelectedValue);
                if (oldrevisionstatus == 3 && (newstatus == 1 || newstatus == 2))
                {
                    sb.Attributes.Add("onclick", "return fnConfirmTelerik(event,'Are you sure you want to revoke the approval.? Y/N'); return false;");
                }
            }



            RadLabel lblParentSectionYN = (RadLabel)item.FindControl("lblParentSectionYN");
            if (lblParentSectionYN != null && lblParentSectionYN.Text.ToString() == "1")
            {
                HtmlImage imgSection = (HtmlImage)item.FindControl("imgSection");
                if (imgSection != null)
                    imgSection.Visible = true;
            }

            RadLabel lblAccepted = (RadLabel)item.FindControl("lblAccepted");
            LinkButton cmdSubSection = (LinkButton)item.FindControl("cmdSubSection");
            LinkButton cmgEditContent = (LinkButton)item.FindControl("cmgEditContent");
            LinkButton cmdRevison = (LinkButton)item.FindControl("cmdRevison");
            LinkButton cmdApprove = (LinkButton)item.FindControl("cmdApprove");
            LinkButton cmdParentSection = (LinkButton)item.FindControl("cmdParentSection");
            LinkButton imbPriorityInvoice = (LinkButton)item.FindControl("imbPriorityInvoice");

            if (lblAccepted != null)
            {
                if (!string.IsNullOrEmpty(lblAccepted.Text) && int.Parse(lblAccepted.Text) == 1 && lblAccepted != null)
                    imbPriorityInvoice.Visible = true;
            }
            if (imbPriorityInvoice != null)
                if (!SessionUtil.CanAccess(this.ViewState, imbPriorityInvoice.CommandName)) imbPriorityInvoice.Visible = false;

            RadLabel lblRevisionStatus = (RadLabel)item.FindControl("lblRevisionStatus");

            if (cmdApprove != null)
            {
                if (lblRevisionStatus.Text != "3" && lblRevisionStatus.Text != "4")
                {
                    cmdApprove.Attributes.Add("onclick", "return fnConfirmTelerik(event,'Are you sure you want to Approve this draft.? Y/N'); return false;");
                }
            }

            DataRowView dr = (DataRowView)item.DataItem;

            RadLabel lblSectionId = (RadLabel)item.FindControl("lblSectionId");
            RadLabel lblOfficeRemarks = (RadLabel)item.FindControl("lblOfficeRemarks");
            RadLabel lblDraftRevisionId = (RadLabel)item.FindControl("lblDraftRevisionId");
            RadLabel lblRevisionNumber = (RadLabel)item.FindControl("lblRevisionNumber");
            RadLabel lblRevisionId = (RadLabel)item.FindControl("lblRevisionId");

            if (cmdSubSection != null && lblSectionId != null)
                cmdSubSection.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filter','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSubSectionAdd.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + lblSectionId.Text + "',null, 400,250,null,null); return false;");

            if (cmgEditContent != null && lblSectionId != null && lblRevisionStatus.Text != "3")
            {
                cmgEditContent.Attributes.Add("onclick", "openNewWindow('Filter','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentGeneral.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + lblSectionId.Text + "'); return false;");
            }
            if (cmgEditContent != null && lblSectionId != null && lblRevisionNumber.Text != "" && lblRevisionStatus.Text == "4")
            {
                cmgEditContent.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentEditReason.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + lblSectionId.Text + "&REVISIONID=" + lblRevisionId.Text + "'); return false;");
            }

            if (cmgEditContent != null && lblSectionId != null && lblRevisionNumber.Text == "" && lblRevisionStatus.Text != "3")
            {
                cmgEditContent.Attributes.Add("onclick", "openNewWindow('Filter','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentGeneral.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + lblSectionId.Text + "'); return false;");
            }


            if (cmdParentSection != null && lblSectionId != null)
                cmdParentSection.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filter','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentParentSectionAdd.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + lblSectionId.Text + "',null, 400,250,null,null); return false;");

            if (cmdSubSection != null && !SessionUtil.CanAccess(this.ViewState, cmdSubSection.CommandName))
                cmdSubSection.Visible = false;
            if (cmgEditContent != null && !SessionUtil.CanAccess(this.ViewState, cmgEditContent.CommandName))
                cmgEditContent.Visible = false;
            if (cmdRevison != null && !SessionUtil.CanAccess(this.ViewState, cmdRevison.CommandName))
                cmdRevison.Visible = false;
            if (cmdApprove != null && !SessionUtil.CanAccess(this.ViewState, cmdApprove.CommandName))
                cmdApprove.Visible = false;

            LinkButton lnkSectionName = (LinkButton)item.FindControl("lnkSectionName");

            if (lnkSectionName != null)
                lnkSectionName.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp','','DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + lblSectionId.Text + "'); return false;");

            LinkButton lnkDraftRevisionNo = (LinkButton)item.FindControl("lnkDraftRevisionNo");


            if (lnkDraftRevisionNo != null && lblDraftRevisionId != null)
                lnkDraftRevisionNo.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp','','DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"].ToString() + "&SECTIONID=" + dr["FLDSECTIONID"].ToString() + "&REVISONID=" + lblDraftRevisionId.Text + "'); return false;");

            RadDropDownList ddlDocumentCategoryAdd = (RadDropDownList)item.FindControl("ddlDocumentCategoryAdd");
            if (ddlDocumentCategoryAdd != null)
            {
                ddlDocumentCategoryAdd.DataSource = PhoenixDocumentManagementCategory.ListDocumentCategory();
                ddlDocumentCategoryAdd.DataTextField = "FLDCATEGORYNAME";
                ddlDocumentCategoryAdd.DataValueField = "FLDCATEGORYID";
                ddlDocumentCategoryAdd.DataBind();
            }

            UserControlToolTip ucOfficeRemarks = (UserControlToolTip)item.FindControl("ucOfficeRemarks");
            if (lblOfficeRemarks != null)
            {
                ucOfficeRemarks.Position = ToolTipPosition.BottomCenter;
                ucOfficeRemarks.TargetControlId = lblOfficeRemarks.ClientID;
                //lblOfficeRemarks.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucOfficeRemarks.ToolTip + "', 'visible');");
                //lblOfficeRemarks.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucOfficeRemarks.ToolTip + "', 'hidden');");
            }

            RadComboBox ddlRevisionStatusEdit = (RadComboBox)item.FindControl("ddlRevisionStatusEdit");
            DataRowView dv = (DataRowView)item.DataItem;

            if (ddlRevisionStatusEdit != null)
            {
                ddlRevisionStatusEdit.SelectedValue = dv["FLDSTATUSID"].ToString();
            }
        }

        //hide dropdown in edit based on section status


        if (e.Item.IsInEditMode)
        {
            RadComboBox dpstatus = (RadComboBox)e.Item.FindControl("ddlRevisionStatusEdit");
            RadLabel lblrevstatus = (RadLabel)e.Item.FindControl("lblRevStatusedit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            string strsectionid = drv["FLDSECTIONID"].ToString();
            Guid? sectionid = General.GetNullableGuid(strsectionid);
            DataSet ds = new DataSet();

            ds = PhoenixDocumentManagementDocumentSection.SectionStatus(sectionid);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow drr = ds.Tables[0].Rows[0];
                string STATUS = drr["FLDREVISIONSTATUS"].ToString();

                if (STATUS == "4")
                {
                    dpstatus.Visible = false;
                    lblrevstatus.Visible = true;
                }
                else if (STATUS == "")
                {
                    dpstatus.Visible = false;
                }
                else
                {
                    dpstatus.Visible = true;

                }
            }
            //if (dpstatus != null)
            //{
            //    dpstatus.SelectedValue = drv["FLDSTATUSID"].ToString();
            //}

        }

        if (e.Item is GridFooterItem)
        {
            GridFooterItem item = (GridFooterItem)e.Item;
            LinkButton db = (LinkButton)item.FindControl("cmdAdd");

            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            if (ViewState["DOCUMENTTYPE"] != null && ViewState["DOCUMENTTYPE"].ToString() == "1")
            {
                item.Visible = false;
            }
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblSectionId = ((RadLabel)gvDocument.Items[rowindex].FindControl("lblSectionId"));
            if (lblSectionId != null)
                ViewState["SECTIONID"] = lblSectionId.Text;

            RadLabel lblRevisionStatus = (RadLabel)gvDocument.Items[rowindex].FindControl("lblRevisionStatus");
            if (lblRevisionStatus != null)
                ViewState["REVISIONSTATUS"] = lblRevisionStatus.Text;

            SetRowSelection();

            //Filter.CurrentSelectedBulkOrderId = ((Label)gvDocument.Rows[rowindex].FindControl("lblOrderId")).Text;            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void InsertDocumentSection(string sectionname, string documentid, int? activyn, string sectionnumber, string newrevisionnumber)
    {
        PhoenixDocumentManagementDocumentSection.DocumentSectionInsert(
              PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , sectionname
            , null
            , null
            , new Guid(documentid)
            , activyn
            , sectionnumber
            , newrevisionnumber);
    }

    private void UpdateDocumentSection(string sectionname, string documentid, string sectionid, int? activyn, string sectionnumber, int? revisionstatus)
    {
        PhoenixDocumentManagementDocumentSection.DocumentSectionUpdate(
              PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , sectionname
            , new Guid(documentid)
            , new Guid(sectionid)
            , activyn
            , sectionnumber
            , revisionstatus
            );

    }

    private void DeleteDocumentSection(string sectionid)
    {
        PhoenixDocumentManagementDocumentSection.DocumentSectionDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(sectionid));
    }

    private bool IsValidDocument(string sectionname, string sectionnumber, string documentid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(documentid) == null)
            ucError.ErrorMessage = "Document is not selected.";

        if (General.GetNullableString(sectionnumber) == null)
            ucError.ErrorMessage = "Section is required.";

        if (General.GetNullableString(sectionname) == null)
            ucError.ErrorMessage = "Name is required.";

        return (!ucError.IsError);
    }


    private void SetRowSelection()
    {
        foreach (GridDataItem item in gvDocument.Items)
        {

            if (item.GetDataKeyValue("FLDSECTIONID").ToString().Equals(ViewState["SECTIONID"].ToString()))
            {
                gvDocument.SelectedIndexes.Add(item.ItemIndex);
                //ViewState["DOCUMENTID"] = item.GetDataKeyValue("FLDDOCUMENTID").ToString();
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (hiddenkey != null && General.GetNullableGuid(hiddenkey.Value) != null)
            {
                ViewState["SECTIONID"] = General.GetNullableGuid(hiddenkey.Value);
            }
            gvDocument.Rebind();
            hiddenkey.Value = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    //protected void ucConfirm_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;
    //        int nCurrentRow = gvDocument.SelectedIndex;

    //        if (ucCM.confirmboxvalue == 1)
    //        {
    //            Label lblDraftRevisionId = ((Label)gvDocument.Rows[nCurrentRow].FindControl("lblDraftRevisionId"));

    //            PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonApprove(
    //                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                    , new Guid(ViewState["SECTIONID"].ToString())
    //                    , new Guid(ViewState["DOCUMENTID"].ToString())
    //                    , General.GetNullableGuid(lblDraftRevisionId != null ? lblDraftRevisionId.Text : "")
    //                    );

    //            ucStatus.Text = "Revision is approved.";
    //            BindData();
    //            SetPageNavigator();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //        return;
    //    }
    //}

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {            
            if (Filter.CurrentSelectedDocumentSections != null)
            {
                ArrayList selectedsections = (ArrayList)Filter.CurrentSelectedDocumentSections;
                if (selectedsections != null && selectedsections.Count > 0)
                {
                    foreach (Guid sectionid in selectedsections)
                    {
                        string draftrevisionid = null;
                        DataSet ds = PhoenixDocumentManagementDocumentSection.DocumentSectionEdit(sectionid);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            draftrevisionid = ds.Tables[0].Rows[0]["FLDDRAFTREVISIONID"].ToString();
                        }

                        PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonApprove(
                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                    , sectionid
                                    , new Guid(ViewState["DOCUMENTID"].ToString())
                                    , General.GetNullableGuid(draftrevisionid != null ? draftrevisionid : "")
                                    );
                    }
                }
            }
            Filter.CurrentSelectedDocumentSections = null;
            ucStatus.Text = "Revisions are approved.";
            gvDocument.Rebind();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    //protected void ucConfirmStatus_Click(object sender, GridCommandEventArgs e)
    //{
    //    try
    //    {

    //        GridEditableItem eeditedItem = e.Item as GridEditableItem;
    //        RadGrid _gridView = (RadGrid)sender;

    //        UpdateDocumentSection(
    //           ((RadTextBox)eeditedItem.FindControl("txtSectionNameEdit")).Text
    //            , ViewState["DOCUMENTID"].ToString()
    //            , ((RadLabel)eeditedItem.FindControl("lblSectionIdEdit")).Text
    //            , (((CheckBox)eeditedItem.FindControl("chkActiveYNEdit")).Checked) ? 1 : 0
    //            , ((RadTextBox)eeditedItem.FindControl("txtSectionNumberEdit")).Text
    //            , General.GetNullableInteger(((RadDropDownList)eeditedItem.FindControl("ddlRevisionStatusEdit")).SelectedValue)
    //        );

    //        ucStatus.Text = "Section details updated.";
    //        gvDocument.Rebind();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //        return;
    //    }
    //}

    protected void CheckAll(object sender, EventArgs e)
    {
        RadCheckBox headerCheckBox = (sender as RadCheckBox);
        ArrayList SelectedForms = new ArrayList();
        Guid index = new Guid();
        foreach (GridDataItem dataItem in gvDocument.MasterTableView.Items)
        {
            bool result = false;
            if (headerCheckBox.Checked == true)
            {
                if (dataItem.GetDataKeyValue("FLDSECTIONID").ToString() != "")
                {
                    index = new Guid(dataItem.GetDataKeyValue("FLDSECTIONID").ToString());
                    (dataItem.FindControl("chkSelect") as RadCheckBox).Checked = true;
                    result = true;
                }
                //dataItem.Selected = true;
            }
            else
            {
                (dataItem.FindControl("chkSelect") as RadCheckBox).Checked = false;
                Filter.CurrentSelectedDocumentSections = null;
            }

            // Check in the Session
            if (Filter.CurrentSelectedDocumentSections != null)
                SelectedForms = (ArrayList)Filter.CurrentSelectedDocumentSections;
            if (result)
            {
                if (!SelectedForms.Contains(index))
                    SelectedForms.Add(index);
            }
            else
                SelectedForms.Remove(index);
        }
        if (SelectedForms != null && SelectedForms.Count > 0)
            Filter.CurrentSelectedDocumentSections = SelectedForms;
    }


    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedSections = new ArrayList();
        Guid index = new Guid();


        foreach (GridDataItem item in gvDocument.Items)
        {
            bool result = false;

            if (item.GetDataKeyValue("FLDSECTIONID").ToString() != "")
            {
                index = new Guid(item.GetDataKeyValue("FLDSECTIONID").ToString());                

                if (((RadCheckBox)(item.FindControl("chkSelect"))).Checked == true)
                {
                    result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
                }

                // Check in the Session
                if (Filter.CurrentSelectedDocumentSections != null)
                    SelectedSections = (ArrayList)Filter.CurrentSelectedDocumentSections;
                if (result)
                {
                    if (!SelectedSections.Contains(index))
                        SelectedSections.Add(index);
                }
                else
                    SelectedSections.Remove(index);
            }
        }
        if (SelectedSections != null && SelectedSections.Count > 0)
            Filter.CurrentSelectedDocumentSections = SelectedSections;
    }

    private void GetSelectedSections()
    {
        if (Filter.CurrentSelectedDocumentSections != null)
        {
            ArrayList SelectedSections = (ArrayList)Filter.CurrentSelectedDocumentSections;
            Guid index = new Guid();
            if (SelectedSections != null && SelectedSections.Count > 0)
            {
                foreach (GridDataItem row in gvDocument.Items)
                {
                    //CheckBox ChkPlan = (CheckBox)row["ClientSelectColumn"].Controls[0];
                    //CheckBox chk = (CheckBox)(row.Cells[0].FindControl("chkSelect"));
                    RadCheckBox chk = (RadCheckBox)(row.Cells[0].FindControl("chkSelect"));
                    index = new Guid(row.GetDataKeyValue("FLDSECTIONID").ToString());
                    if (SelectedSections.Contains(index))
                    {
                        RadCheckBox myCheckBox = (RadCheckBox)row.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }
    }
}
