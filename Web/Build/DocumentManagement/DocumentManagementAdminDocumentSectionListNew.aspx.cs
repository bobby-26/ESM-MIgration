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
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Integration;
using SouthNests.Phoenix.Common;
using System.Text.RegularExpressions;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;

public partial class DocumentManagement_DocumentManagementAdminDocumentSectionListNew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            confirm.Attributes.Add("style", "display:none;");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();

            //toolbar.AddImageButton("../DocumentManagement/DocumentManagementAdminDocumentSectionList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar1.AddFontAwesomeButton("../DocumentManagement/DocumentManagementAdminDocumentSectionListNew.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvDocument')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../DocumentManagement/DocumentManagementAdminDocumentSectionListNew.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar1.AddFontAwesomeButton("../DocumentManagement/DocumentManagementAdminDocumentSectionListNew.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");


            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Section", "SECTION", ToolBarDirection.Right);
            toolbar.AddButton("Document", "DOCUMENT", ToolBarDirection.Right);

            if (!IsPostBack)
            {
                Filter.CurrentSelectedDocumentSections = null;

                ucConfirmApprove.Visible = false;
                lblMessage.Text = string.Empty;
                //ucConfirm.Visible = false;
                //ucConfirmApprove.Visible = false;
                //ucConfirmStatus.Visible = false;

                if (Request.QueryString["pageno"] != null && Request.QueryString["pageno"].ToString() != "")
                    ViewState["pageno"] = Request.QueryString["pageno"].ToString();
                else
                    ViewState["pageno"] = "";

                if (Request.QueryString["pagesize"] != null && Request.QueryString["pagesize"].ToString() != "")
                    ViewState["pagesize"] = Request.QueryString["pagesize"].ToString();
                else
                    ViewState["pagesize"] = "";

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

                if (Request.QueryString["Wholemanualread"] != null && Request.QueryString["Wholemanualread"].ToString() != "")
                {
                    ViewState["Wholemanualread"] = Request.QueryString["Wholemanualread"].ToString();
                }
                else
                    ViewState["Wholemanualread"] = 0;

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

                ViewState["REVISIONID"] = "";
                ViewState["DRAFTREVISIONID"] = "";

                int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
                btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnShowDocuments', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + companyid + "'); ");

                gvDocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
            GetDocumentName();
            BindHSEQA();

            MenuDocument.AccessRights = this.ViewState;
            MenuDocument.MenuList = toolbar.Show();
            MenuDocument.SelectedMenuIndex = 0;

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Approve", "BULKAPPROVE", ToolBarDirection.Right);
            MenuDocumentGeneral.AccessRights = this.ViewState;
            MenuDocumentGeneral.MenuList = toolbar.Show();

            toolbar1.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1','Add Section','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionAdd.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"].ToString() + "')", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuRegistersCountry.AccessRights = this.ViewState;
            MenuRegistersCountry.MenuList = toolbar1.Show();

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
                RadWindowManager1.RadConfirm("Are you sure you want to approve all the selected sections.? Y/N</br>(Note:</br> * Are you donot want to revice the Question/Reading Required? </br> * Once the Section is Approved Questions/Reading Required cannot be change.", "confirm", 400, 200, null, "Confirm");
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
                Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentListNew.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&pageno=" + ViewState["pageno"].ToString() + "&pagesize=" + ViewState["pagesize"].ToString());
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

        string[] alColumns = { "FLDSECTIONNUMBER", "FLDSECTIONNAME", "FLDACTIVESTATUS", "FLDREVISIONDETAILS", "FLDDRAFTREVISION", "FLDSTATUSNAME", "FLDOFFICEREMARKS", "FLDQUESAPPROVEDYN", "FLDREADINGUSER" };
        string[] alCaptions = { "Section", "Name", "Active", "Revision", "Draft", "Status", "Office Remarks", "Questions Approved YN", "Reading Required" };

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
        string[] alColumns = { "FLDSECTIONNUMBER", "FLDSECTIONNAME", "FLDACTIVESTATUS", "FLDREVISIONDETAILS", "FLDDRAFTREVISION", "FLDSTATUSNAME", "FLDOFFICEREMARKS", "FLDQUESAPPROVEDYN", "FLDREADINGUSER" };
        string[] alCaptions = { "Section", "Name", "Active", "Revision", "Draft", "Status", "Office Remarks", "Questions Approved YN", "Reading Required" };

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

            if (e.CommandName.ToUpper().Equals("FORMS"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                lblMessage.Text = string.Empty;
                ViewState["SECTIONID"] = new Guid(item.GetDataKeyValue("FLDSECTIONID").ToString());
                ViewState["REVISIONID"] = ((RadLabel)eeditedItem.FindControl("lblRevisionId")).Text;
                ViewState["DRAFTREVISIONID"] = ((RadLabel)item.FindControl("lblDraftRevisionId")).Text;

                string script = "function sd(){showDialog('Forms and Checklist Mapping'); $find('" + RadAjaxPanel2.ClientID + "').ajaxRequest(',HEDIT');   Sys.Application.remove_load(sd);}Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

            }


            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                BindPageURL(nCurrentRow);
            }

            else if (e.CommandName.ToUpper().Equals("CONFIGURATION"))
            {
                //Response.Redirect("/DocumentManagement/DocumentManagementConfiguration.aspx?SECTIONID="+ ViewState["SECTIONID"].ToString());
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
                Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentSectionRevisionListNew.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + ViewState["SECTIONID"].ToString());
            }
            else if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                BindPageURL(nCurrentRow);
                ViewState["DRAFTREVISIONID"] = ((RadLabel)eeditedItem.FindControl("lblDraftRevisionId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to approve all the selected sections.? Y/N.", "ConfirmApprove", 400, 200, null, "Approve");

            }
            if (e.CommandName.ToUpper().Equals("BASE64"))
            {
                BindPageURL(nCurrentRow);
                UbdateBase64Data(ViewState["DOCUMENTID"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void RadAjaxPanel2_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    {
        var args = e.Argument;
        var array = args.Split(',');
        var id = array[0];
        var cmd = array[1];

        if (cmd.ToUpper() == "HEDIT")
        {
            BindHSEQA();
        }
    }

    protected void BindHSEQA()
    {
        DataSet dss = PhoenixDocumentManagementQuestion.FormsList(General.GetNullableGuid(ViewState["SECTIONID"].ToString()), General.GetNullableGuid(ViewState["DRAFTREVISIONID"].ToString()));
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0)
        {
            tblDocuments.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                RadCheckBox cb = new RadCheckBox();
                cb.ID = dr["FLDFORMPOSTERID"].ToString();
                cb.Text = "";
                cb.Checked = true;
                cb.AutoPostBack = true;
                cb.CheckedChanged += new EventHandler(doc_CheckedChanged);
                HyperLink hl = new HyperLink();
                hl.Text = dr["FLDNAME"].ToString();
                hl.ID = "hlink" + number.ToString();
                hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");

                int type = 0;
                PhoenixIntegrationQuality.GetSelectedeTreeNodeType(General.GetNullableGuid(dr["FLDFORMPOSTERID"].ToString()), ref type);

                if (type == 5)
                {
                    DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , new Guid(hl.ID.ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drr = ds.Tables[0].Rows[0];
                        hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + dr["FLDFORMPOSTERID"].ToString() + "&FORMREVISIONID=" + drr["FLDFORMREVISIONID"].ToString() + "');return false;");
                    }
                }
                else if (type == 6)
                {
                    DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , new Guid(dr["FLDFORMPOSTERID"].ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drr = ds.Tables[0].Rows[0];
                        if (drr["FLDFORMREVISIONDTKEY"] != null && General.GetNullableGuid(drr["FLDFORMREVISIONDTKEY"].ToString()) != null)
                        {
                            DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(drr["FLDFORMREVISIONDTKEY"].ToString()));
                            if (dt.Rows.Count > 0)
                            {
                                DataRow drRow = dt.Rows[0];
                                hl.Target = "_blank";
                                hl.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();
                            }
                        }
                    }
                }

                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                tc.Controls.Add(cb);
                tr.Cells.Add(tc);
                tc = new HtmlTableCell();
                tc.Controls.Add(hl);
                tr.Cells.Add(tc);
                tblDocuments.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tblDocuments.Rows.Add(tr);
                number = number + 1;
            }
        }
    }

    protected void lnkDocumentAdd_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixDocumentManagementQuestion.FormsUpdate(new Guid(ViewState["SECTIONID"].ToString()), General.GetNullableGuid(ViewState["DRAFTREVISIONID"].ToString()), new Guid(txtDocumentId.Text));

            txtDocumentId.Text = "";
            txtDocumentname.Text = "";
            BindHSEQA();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
            return;
        }
    }

    void doc_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            RadCheckBox c = (RadCheckBox)sender;
            if (c.Checked == false)
            {
                PhoenixDocumentManagementQuestion.FormsDelete(new Guid(ViewState["SECTIONID"].ToString()), General.GetNullableGuid(ViewState["DRAFTREVISIONID"].ToString()), new Guid(c.ID));

                BindHSEQA();
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
            RadLabel lblSectionId = (RadLabel)item.FindControl("lblSectionId");

            LinkButton cmdbase64 = (LinkButton)item.FindControl("cmdbase64");
            if (cmdbase64 != null && !SessionUtil.CanAccess(this.ViewState, cmdbase64.CommandName))
                cmdbase64.Visible = false;

            LinkButton del = (LinkButton)item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)item.FindControl("cmdEdit");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                eb.Attributes.Add("onclick", "openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionAdd.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + lblSectionId.Text + "'); return false;");
                //eb.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','Edit Section'" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionAdd.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + lblSectionId.Text + "'); return false;");
            }

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


            LinkButton cmdConfig = (LinkButton)e.Item.FindControl("cmdConfig");
            RadLabel lblParentSectionYN = (RadLabel)item.FindControl("lblParentSectionYN");
            if (lblParentSectionYN != null && lblParentSectionYN.Text.ToString() == "1")
            {
                if (ViewState["Wholemanualread"] != null && ViewState["Wholemanualread"].ToString().Equals("0"))
                {
                    if (cmdConfig != null)
                        cmdConfig.Visible = true;
                }
                else
                {
                    if (cmdConfig != null)
                        cmdConfig.Visible = false;
                }
                HtmlImage imgSection = (HtmlImage)item.FindControl("imgSection");
                if (imgSection != null)
                    imgSection.Visible = false;


            }
            else
            {
                cmdConfig.Visible = false;
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

            //if (cmdApprove != null)
            //{
            //    if (lblRevisionStatus.Text != "3" && lblRevisionStatus.Text != "4")
            //    {
            //        //RadWindowManager1.RadConfirm("Are you sure you want to approve all the selected sections.? Y/N</br>(Note:</br> * Are you don't want to revice the Question/Reading Required? </br> * Once the Section is Approved Questions/Reading Required cannot be change.", "ConfirmApprove", 320, 150, null, "Confirm");
            //        //RadWindowManager1.RadConfirm("Are you sure you want to approve all the selected sections.? Y/N</br>(Note:</br> * Are you don't want to revice the Question/Reading Required? </br> * Once the Section is Approved Questions/Reading Required cannot be change.", "confirm", 400, 200, null, "Confirm");
            //        //cmdApprove.Attributes.Add("onclick", "return fnConfirmTelerik(event,'Are you sure you want to Approve this draft.? Y/N</br>(Note:</br> * Are you don't want to revice the Question/Reading Required? </br> * One the Section is Approved Questions/Reading Required cannot be change.)'); return false;");
            //    }
            //}

            DataRowView dr = (DataRowView)item.DataItem;

            //RadLabel lblSectionId = (RadLabel)item.FindControl("lblSectionId");
            RadLabel lblOfficeRemarks = (RadLabel)item.FindControl("lblOfficeRemarks");
            RadLabel lblDraftRevisionId = (RadLabel)item.FindControl("lblDraftRevisionId");
            RadLabel lblRevisionNumber = (RadLabel)item.FindControl("lblRevisionNumber");
            RadLabel lblRevisionId = (RadLabel)item.FindControl("lblRevisionId");

            if (cmdSubSection != null && lblSectionId != null)
                cmdSubSection.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filter','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSubSectionAdd.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + lblSectionId.Text + "','small'); return false;");

            if (cmgEditContent != null && lblSectionId != null && lblRevisionStatus.Text != "3")
            {
                cmgEditContent.Attributes.Add("onclick", "openNewWindow('Filter','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentGeneralnNew.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + lblSectionId.Text + "'); return false;");
            }
            if (cmgEditContent != null && lblSectionId != null && lblRevisionNumber.Text != "" && lblRevisionStatus.Text == "4")
            {
                cmgEditContent.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentEditReasonNew.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + lblSectionId.Text + "&REVISIONID=" + lblRevisionId.Text + "'); return false;");
            }

            if (cmgEditContent != null && lblSectionId != null && lblRevisionNumber.Text == "" && lblRevisionStatus.Text != "3")
            {
                cmgEditContent.Attributes.Add("onclick", "openNewWindow('Filter','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentGeneralnNew.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + lblSectionId.Text + "'); return false;");
            }


            if (cmdParentSection != null && lblSectionId != null)
                cmdParentSection.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filter','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentParentSectionAdd.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + lblSectionId.Text + "','medium'); return false;");

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
                lnkSectionName.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp','','DocumentManagement/DocumentManagementDocumentSectionNewContentView.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + lblSectionId.Text + "'); return false;");

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

            LinkButton cmdQuestion = (LinkButton)e.Item.FindControl("cmdQuestion");
            if (cmdQuestion != null)
            {
                SessionUtil.CanAccess(this.ViewState, cmdQuestion.CommandName);
                cmdQuestion.Attributes.Add("onclick", "openNewWindow('Questions', 'Questions (" + lnkSectionName.Text + ") Rev.No: " + dv["FLDQUESTIONREVISIONNUMBER"].ToString() + "', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementQuestions.aspx?SECTIONID=" + lblSectionId.Text + "&REVISIONID=" + lblDraftRevisionId.Text + "&SECTION=" + lnkSectionName.Text + "');return false;");
            }


            if (cmdConfig != null)
            {
                SessionUtil.CanAccess(this.ViewState, cmdConfig.CommandName);
                cmdConfig.Attributes.Add("onclick", "openNewWindow('Configuration', 'Configuration (" + lnkSectionName.Text + ")', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementConfiguration.aspx?SECTIONID=" + lblSectionId.Text + "&REVISIONID=" + lblDraftRevisionId.Text + "&SECTION=" + lnkSectionName.Text + "');return false;");
            }
            LinkButton lnkReadUnread = (LinkButton)e.Item.FindControl("lnkReadUnread");
            if (lnkReadUnread != null)
            {
                SessionUtil.CanAccess(this.ViewState, lnkReadUnread.CommandName);
                lnkReadUnread.Attributes.Add("onclick", "openNewWindow('ReadUnread', '" + lnkSectionName.Text + " - Read/Unread User List', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionReadUnreadList.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + lblSectionId.Text + "');return false;");
            }

            RadLabel lblVersionNumber = (RadLabel)e.Item.FindControl("lblVersionNumber");
            RadLabel lblLastRevision = (RadLabel)e.Item.FindControl("lblLastRevision");

            RadLabel lblForms = (RadLabel)item.FindControl("lblForms");
            LinkButton cmdForms = (LinkButton)item.FindControl("cmdForms");
            //if (lblForms.Text == null || lblForms.Text == string.Empty)
            //{
            //    if (cmdForms != null)
            //        cmdForms.Visible = false;
            //}

        }

        //hide dropdown in edit based on section status



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

                        PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonApproveNew(
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

    protected void ucConfirmApprove_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["DRAFTREVISIONID"] != null && ViewState["DRAFTREVISIONID"].ToString() != string.Empty)
            {
                PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonApproveNew(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["SECTIONID"].ToString())
                        , new Guid(ViewState["DOCUMENTID"].ToString())
                        , General.GetNullableGuid(ViewState["DRAFTREVISIONID"] != null ? ViewState["DRAFTREVISIONID"].ToString() : "")
                        );

                ucStatus.Text = "Revision is approved.";
                gvDocument.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void UbdateBase64Data(String Documentid)
    {
        if (Documentid != null && Documentid.ToString() != "")
        {
            DataSet ds = PhoenixDocumentManagementDocumentSection.NewDocumentSectionData(General.GetNullableGuid(Documentid.ToString())
                , General.GetNullableGuid(ViewState["SECTIONID"].ToString())
                , General.GetNullableInteger(null)
                 , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                 , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                );

            if (ds.Tables[1].Rows[0]["FLDSECTIONID"].ToString().ToLower() != ViewState["DOCUMENTID"].ToString().ToLower())
            {
                string orignaldata = HttpUtility.HtmlDecode(ds.Tables[1].Rows[0]["FLDNEWTEXT"].ToString());
                Regex imgRegex = new Regex("<img[^>]*>");

                MatchEvaluator imgEvaluator = new MatchEvaluator(ProcessMatch);
                string newContent = imgRegex.Replace(orignaldata, imgEvaluator);

                MatchEvaluator imgSize = new MatchEvaluator(ProcessImage);
                string newImageContent = imgRegex.Replace(newContent, imgSize);

                Regex tblRegex = new Regex("<table[^>]*>");
                MatchEvaluator tblSize = new MatchEvaluator(ProcessTable);
                string newtableContent = tblRegex.Replace(newImageContent, tblSize);
                newtableContent = HttpUtility.HtmlEncode(newtableContent);

                try
                {
                    PhoenixDocumentManagementDocument.DocumentSectionImagebase64Update(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , new Guid(Documentid.ToString())
                                                                                        , new Guid(ViewState["SECTIONID"].ToString())
                                                                                        , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                                        , General.GetNullableString(newtableContent));
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }

            }
        }
    }
    private string ProcessMatch(Match match)
    {
        Regex srcRegex = new Regex("src=(\"|')(?!data:)[^\"']*");

        if (!srcRegex.IsMatch(match.Value))
        {
            // if it is already a base64 value, return it. 
            return match.Value;
        }
        //MatchEvaluator imgSize = new MatchEvaluator(ProcessImage);

        Match srcMatch = srcRegex.Match(match.Value);
        string result = String.Empty;

        try
        {

            string urlValue = srcMatch.Value.Substring(5);
            urlValue = urlValue.Replace("../", FullyQualifiedApplicationPath());
            bool isAbsolute = urlValue.StartsWith("http", StringComparison.CurrentCultureIgnoreCase);
            Uri myUri = new Uri(urlValue);

            //Create the base64 string from the file's data.

            using (MemoryStream stream = new MemoryStream())
            {
                Uri Uri = new Uri(urlValue);
                WebClient webClient = new WebClient();
                string fileExtension = Path.GetExtension(urlValue).TrimStart('.');
                var openRead = webClient.OpenRead(urlValue);
                if (openRead != null)
                {
                    openRead.CopyTo(stream);
                    FileStream fs = GetImage(Uri);
                    byte[] bites = new byte[fs.Length];
                    fs.Read(bites, 0, bites.Length);

                    string base64ImageRepresentation = Convert.ToBase64String(bites);

                    string queryString = Uri.Query;
                    var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
                    string dtkey = queryDictionary["dtkey"];
                    DataTable dt = PhoenixCommonFileAttachment.EditAttachment(new Guid(dtkey));
                    if (dt.Rows.Count > 0)
                    {
                        fileExtension = Path.GetExtension(dt.Rows[0]["FLDFILENAME"].ToString()).TrimStart('.');
                    }
                    result = String.Format("data:image/{0};base64,{1}", fileExtension, base64ImageRepresentation);
                }
            }

            // Build the new src value.
            string base64SrcAttr = "src=\"" + result;

            // Replace the the old src value with the new one and return the new img tag. 
            return match.Value.Replace(srcMatch.Value, base64SrcAttr);
        }
        catch (Exception)
        {
            string fileExtension = "png";
            FileStream file = new FileStream(Server.MapPath("~/css/Theme1/images/xf_close_icon.png"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(file);
            byte[] bites = new byte[file.Length];
            file.Read(bites, 0, bites.Length);
            string base64ImageRepresentation = Convert.ToBase64String(bites);
            result = String.Format("data:image/{0};base64,{1}", fileExtension, base64ImageRepresentation);
            string base64SrcAttr = "src=\"" + result;
            return match.Value.Replace(srcMatch.Value, base64SrcAttr);
        }

    }
    private FileStream GetImage(Uri url)
    {
        FileStream file = null;
        try
        {

            if (url == null)
            {
                return file;
            }
            string queryString = url.Query;
            var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);

            string dtkey = queryDictionary["dtkey"];
            DataTable dt = PhoenixCommonFileAttachment.EditAttachment(new Guid(dtkey));
            if (dt.Rows.Count > 0)
            {
                string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
                string archivedpath = PhoenixGeneralSettings.CurrentGeneralSetting.ArchivedAttachmentPath;
                if (!path.EndsWith("/"))
                    path = path + "/";
                if (!archivedpath.EndsWith("/"))
                    archivedpath = archivedpath + "/";

                string _fullPath = path + dt.Rows[0]["FLDFILEPATH"].ToString();
                string _targetPath = HttpContext.Current.Server.MapPath("..\\Attachments\\TEMP\\") + dt.Rows[0]["FLDDTKEY"].ToString() + '.' + Path.GetExtension(dt.Rows[0]["FLDFILENAME"].ToString()).TrimStart('.');

                ReduceImageSize(_fullPath, _targetPath);

                file = new FileStream(_targetPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(file);

                return file;

            }
            return file;
        }

        catch (Exception)
        {

            file = new FileStream(Server.MapPath("~/css/Theme1/images/xf_close_icon.png"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(file);
            return file;
        }
    }

    private static void ReduceImageSize(string sourcePath, string targetPath)
    {
        using (var image = System.Drawing.Image.FromFile(sourcePath))
        {
            var Width = (int)(image.Width);
            var Height = (int)(image.Height);

            var newWidth = 560;
            var newHeight = 600;

            if (Width > newWidth)
                Width = newWidth;

            if (Height > newHeight)
                Height = newHeight;

            var thumbnailImg = new Bitmap(Width, Height);
            var thumbGraph = Graphics.FromImage(thumbnailImg);
            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbGraph.DrawImage(image, imageRectangle);
            thumbnailImg.Save(targetPath, image.RawFormat);
            image.Dispose();


        }
    }

    public string FullyQualifiedApplicationPath()
    {

        //Return variable declaration
        var appPath = string.Empty;

        //Getting the current context of HTTP request
        var context = HttpContext.Current;

        //Checking the current context content
        if (context != null)
        {
            //Formatting the fully qualified website url/name
            appPath = string.Format("{0}://{1}{2}{3}",
            context.Request.Url.Scheme,
            context.Request.Url.Host,
            context.Request.Url.Port == 80
            ? string.Empty
            : ":" + context.Request.Url.Port,
            context.Request.ApplicationPath);
        }

        if (!appPath.EndsWith("/"))
            appPath += "/";

        return appPath;
    }
    private string ProcessImage(Match match)
    {
        Regex srcRegex = new Regex("src=*");
        Regex srcBase64Regex = new Regex("src=(\"|')(?!data:)[^\"']*");
        Match srcMatch = srcRegex.Match(match.Value);
        Regex styleRegex = new Regex("style=*");
        Match styleMatch = styleRegex.Match(match.Value);
        string oldwidthstring = string.Empty;
        string newwidthstring = string.Empty;
        string oldheightstring = string.Empty;
        string newheightstring = string.Empty;
        string Finalresult = string.Empty;

        string newstyle = string.Empty;
        int width, height;

        if (!styleRegex.IsMatch(match.Value))
        {
            string res = Regex.Replace(match.Value, "data:image\\/\\w+\\;base64\\,", "");
            //Match the img tag and get the base64  string value
            string matchString = Regex.Match(res, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;

            byte[] image = Convert.FromBase64String(matchString);
            using (var ms = new MemoryStream(image))
            {
                System.Drawing.Image imgs = System.Drawing.Image.FromStream(ms);
                width = imgs.Width - 1;
                height = imgs.Height - 1;
            }

            if (width > 560)
                width = 560;

            if (height > 600)
                height = 600;

            newstyle = @"style='width:" + width + "px;height:" + height + "px;' " + srcMatch.Value;
            Finalresult = match.Value.Replace(srcMatch.Value, newstyle);
        }
        if (!srcBase64Regex.IsMatch(match.Value) && styleRegex.IsMatch(match.Value))
        {
            string res = Regex.Replace(match.Value, "data:image\\/\\w+\\;base64\\,", "");
            //Match the img tag and get the base64  string value
            string matchString = Regex.Match(res, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;

            byte[] image = Convert.FromBase64String(matchString);
            using (var ms = new MemoryStream(image))
            {
                System.Drawing.Image imgs = System.Drawing.Image.FromStream(ms);
                width = imgs.Width - 1;
                height = imgs.Height - 1;
            }

            oldwidthstring = width.ToString() + "px";
            oldheightstring = height.ToString() + "px";

            if (width > 560)
                width = 560;

            if (height > 600)
                height = 600;

            newwidthstring = width.ToString() + "px";
            newheightstring = height.ToString() + "px";

            //newstyle = @"style='width:" + width + "px;height:" + height + "px;'";
            string newimage = match.Value.Replace(oldwidthstring, newwidthstring);
            newimage = match.Value.Replace(oldheightstring, newheightstring);
            // return match.Value.Replace(styleMatch.Value, newstyle);
            Finalresult = newimage;

        }
        return Finalresult;
    }
    private string ProcessTable(Match match)
    {
        Regex styleRegex = new Regex("style=*");
        Match styleMatch = styleRegex.Match(match.Value);

        if (!styleRegex.IsMatch(match.Value))
        {
            // if it is already a base64 value, return it. 
            return match.Value;
        }
        if (match.Value == @"<table style=""width:700px;"">")
        {
            // if it is already a base64 value, return it. 
            return match.Value;
        }
        string newTable = @"style='border: 1px solid #000000; width: 610px; border-collapse: collapse;' ";

        // Replace the the old src value with the new one and return the new img tag. 
        return match.Value.Replace(styleMatch.Value, newTable);

    }
}
