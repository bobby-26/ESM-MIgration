﻿using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Collections;
using Telerik.Web.UI;


public partial class DocumentManagementFMSOfficeFormList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("E Mails", "ESMA", ToolBarDirection.Left);
            toolbarmain.AddButton("ESM Filing", "ESMF", ToolBarDirection.Left);

            toolbarmain.AddButton("Shipboard Forms", "SPFF", ToolBarDirection.Left);
            toolbarmain.AddButton("Office Forms", "OFFF", ToolBarDirection.Left);
            toolbarmain.AddButton("Maintenance Forms", "MCFS", ToolBarDirection.Left);
            toolbarmain.AddButton("Plans and Drawings", "DRWS", ToolBarDirection.Left);
            toolbarmain.AddButton("Manuals", "MNSF", ToolBarDirection.Left);
            toolbarmain.AddButton("Equipment Manuals", "EQSF", ToolBarDirection.Left);
            MenuFMS.AccessRights = this.ViewState;
            MenuFMS.MenuList = toolbarmain.Show();           
            MenuFMS.SelectedMenuIndex = 3;

            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementFMSOfficeFormList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvForm')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementFMSOfficeFormList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementFMSOfficeFormList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuRegistersCountry.AccessRights = this.ViewState;
            MenuRegistersCountry.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["FORMID"] = "";
                ViewState["FORMREVISIONID"] = "";
                ViewState["FORMTYPE"] = "";

                Filter.CurrentSelectedForms = null;

                ViewState["CATEGORYID"] = "";

                if (Request.QueryString["FORMID"] != null && Request.QueryString["FORMID"].ToString() != "")
                    ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();
                else
                    ViewState["FORMID"] = "";

                if (Request.QueryString["FORMNO"] != null && Request.QueryString["FORMNO"].ToString() != "")
                {
                    ViewState["FORMNO"] = Request.QueryString["FORMNO"].ToString();
                }
                else
                    ViewState["FORMNO"] = "";

                if (Request.QueryString["CATEGORYNO"] != null && Request.QueryString["CATEGORYNO"].ToString() != "")
                {
                    ViewState["CATEGORYNO"] = Request.QueryString["CATEGORYNO"].ToString();
                }

                btnShowCategory.Attributes.Add("onclick", "return showPickList('spnPickListCategory', 'codehelp1', '', 'Common/CommonPickListFMSDocumentCategory.aspx?CATEGORYID=16', true); ");
                gvForm.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (Request.QueryString["Callfrom"] != null)
                {
                    Filter.FMSOfficeFormFilterCriteria = null;
                }

                if (Filter.FMSOfficeFormFilterCriteria != null)
                {
                    NameValueCollection nvc = Filter.FMSOfficeFormFilterCriteria;

                    txtFormNo.Text = nvc.Get("txtformno").ToString();
                    txtFormName.Text = nvc.Get("txtformname").ToString();
                    txtCategory.Text = nvc.Get("txtCategory").ToString();
                    ddlvessel.SelectedValue = int.Parse(nvc.Get("ddlvessel").ToString());

                }
                BindParentCategory();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuFMS_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("ESMA"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSMailList.aspx", false);
            }
            if (CommandName.ToUpper().Equals("ESMF"))
            {
                Response.Redirect("../DocumentManagement/DocumentFMSFileNoList.aspx", false);
            }
            if (CommandName.ToUpper().Equals("SPFF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSShipboardFormList.aspx?CATEGORYNO=2", false);
            }
            if (CommandName.ToUpper().Equals("OFFF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSOfficeFormList.aspx?CATEGORYNO=16&Callfrom=1", false);
            }
            if (CommandName.ToUpper().Equals("MCFS"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSMaintenanceHistoryTemplate.aspx?", false);
            }
            if (CommandName.ToUpper().Equals("DRWS"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSDrawingList.aspx?", false);
            }
            if (CommandName.ToUpper().Equals("MNSF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSVesselSurveyScheduleList.aspx?", false);
            }
            if (CommandName.ToUpper().Equals("EQSF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSEquipmentManuals.aspx?", false);
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

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDFORMNO", "FLDCAPTION", "FLDTYPENAME", "FLDCATEGORYNAME", "FLDACTIVESTATUS", "FLDPURPOSE", "FLDCOMPANYSHORTCODE", "FLDDATE", "FLDAUTHORNAME", "FLDREVISIONDETAILS", "FLDDRAFTREVISION" };
        string[] alCaptions = { "Form No.", "Name", "Type", "Category", "Active", "Remarks", "Company", "Added", "Added By", "Revision", "Draft" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        string formno = "";
        if (ViewState["FORMNO"].ToString() != "")
        {
            formno = ViewState["FORMNO"].ToString();
        }
        else
        {
            formno = txtFormNo.Text;
        }

        ds = PhoenixDocumentManagementForm.FMSFormSearch(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , General.GetNullableString(txtFormName.Text)
                                                    , null
                                                    , General.GetNullableGuid(txtCategoryid.Text)
                                                    , null
                                                    , sortexpression
                                                    , sortdirection
                                                    , (int)ViewState["PAGENUMBER"]
                                                    , General.ShowRecords(null)
                                                    , ref iRowCount
                                                    , ref iTotalPageCount
                                                    , General.GetNullableString(formno)
                                                    , companyid
                                                    , General.GetNullableInteger(ddlvessel.SelectedVessel)
                                                    );

        Response.AddHeader("Content-Disposition", "attachment; filename=FormList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Form List</h3></td>");
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

    protected void RegistersCountry_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Filter.FMSOfficeFormFilterCriteria = null;

                gvForm.CurrentPageIndex = 0;

                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("txtformno", txtFormNo.Text);
                nvc.Add("txtformname", txtFormName.Text);
                nvc.Add("txtCategoryid", txtCategoryid.Text);
                nvc.Add("txtCategory", txtCategory.Text);
                nvc.Add("ddlvessel", ddlvessel.SelectedVessel);
                Filter.FMSOfficeFormFilterCriteria = nvc;
                gvForm.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentDMSDocumentFilter = null;
                Filter.FMSOfficeFormFilterCriteria = null;
                ViewState["FORMID"] = "";
                txtFormNo.Text = "";
                txtFormName.Text = "";
                txtCategory.Text = "";                
                BindParentCategory();
                gvForm.CurrentPageIndex = 0;
                gvForm.Rebind();
            }
            else if (CommandName.ToUpper().Equals("RESET"))
            {

            }
            else if (CommandName.ToUpper().Equals("UPDATECATEGORY"))
            {
                //SaveCheckedValues(sender, e);
            }
            else if (CommandName.ToUpper().Equals("ADDFIELDS"))
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void BindParentCategory()
    {
        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        DataSet ds = PhoenixCommonDocumentManagement.FMSShipboardFormCategory(companyid, int.Parse(ViewState["CATEGORYNO"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtCategoryid.Text = dr["FLDCATEGORYID"].ToString();
            ViewState["CATEGORYID"] = dr["FLDCATEGORYID"].ToString();
        }
    }

    protected void MenuDocument_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FORMDEFINITION"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFormFieldList.aspx?FORMID=" + ViewState["FORMID"] + "&CATEGORYID=" + ViewState["CATEGORYID"].ToString());
            }
            else if (CommandName.ToUpper().Equals("CATEGORY"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFormCategoryList.aspx?CATEGORYID=" + ViewState["CATEGORYID"].ToString());
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvForm_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFORMNO", "FLDCAPTION", "FLDTYPENAME", "FLDCATEGORYNAME", "FLDACTIVESTATUS", "FLDPURPOSE", "FLDCOMPANYSHORTCODE", "FLDDATE", "FLDAUTHORNAME", "FLDREVISIONDETAILS", "FLDDRAFTREVISION" };
        string[] alCaptions = { "Form No.", "Name", "Type", "Category", "Active", "Remarks", "Company", "Added", "Added By", "Revision", "Draft" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        string formno = "";
        if (ViewState["FORMNO"].ToString() != "")
        {
            formno = ViewState["FORMNO"].ToString();
        }
        else
        {
            formno = txtFormNo.Text;
        }

        if (Filter.FMSOfficeFormFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.FMSOfficeFormFilterCriteria;

            ds = PhoenixDocumentManagementForm.FMSFormSearch(
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableString(nvc.Get("txtformname").ToString())
                                                            , null
                                                            , General.GetNullableGuid(nvc.Get("txtCategoryid").ToString())
                                                            , null
                                                            , sortexpression
                                                            , sortdirection
                                                            , gvForm.CurrentPageIndex + 1
                                                            , gvForm.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount
                                                            , General.GetNullableString(nvc.Get("txtformno").ToString())
                                                            , companyid
                                                            , General.GetNullableInteger(nvc.Get("ddlvessel").ToString())
                                                            );
        }
        else
        {
            ds = PhoenixDocumentManagementForm.FMSFormSearch(
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableString(txtFormName.Text)
                                                            , null
                                                            , General.GetNullableGuid(txtCategoryid.Text)
                                                            , null
                                                            , sortexpression
                                                            , sortdirection
                                                            , gvForm.CurrentPageIndex + 1
                                                            , gvForm.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount
                                                            , General.GetNullableString(formno)
                                                            , companyid
                                                            , General.GetNullableInteger(ddlvessel.SelectedVessel)
                                                            );

        }

        General.SetPrintOptions("gvForm", "Form List", alCaptions, alColumns, ds);
        gvForm.DataSource = ds;
        gvForm.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvForm_ItemCommand(object sender, GridCommandEventArgs e)
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

            if (e.CommandName == "RowClick" || (e.CommandName == RadGrid.ExpandCollapseCommandName) && (!e.Item.Expanded))
            {
                bool lastState = e.Item.Expanded;

                if (e.CommandName == RadGrid.ExpandCollapseCommandName || e.CommandName == "RowClick")
                {
                    lastState = !lastState;
                }

                //CollapseAllRows(lastState);
                e.Item.Expanded = !lastState;
            }


            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(nCurrentRow);
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                BindPageURL(nCurrentRow);
                //Response.Redirect("../DocumentManagement/DocumentManagementFormFieldList.aspx?FORMID=" + ViewState["FORMID"].ToString() + "&FORMREVISIONID=" + ViewState["FORMREVISIONID"].ToString() + "&CATEGORYID=" + ViewState["CATEGORYID"]);
            }
            else if (e.CommandName.ToUpper().Equals("EDITDOCUMENT"))
            {
                BindPageURL(nCurrentRow);
                //Response.Redirect("../DocumentManagement/DocumentManagementFormFieldList.aspx?FORMID=" + ViewState["FORMID"].ToString() + "&FORMREVISIONID=" + ViewState["FORMREVISIONID"].ToString() + "&CATEGORYID=" + ViewState["CATEGORYID"]);
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem eFooteritem = (GridFooterItem)e.Item;
                if (!IsValidField(
                     ((RadTextBox)eFooteritem.FindControl("txtFormNoAdd")).Text,
                    ((RadTextBox)eFooteritem.FindControl("txtFormNameAdd")).Text,
                     ((RadTextBox)eFooteritem.FindControl("txtCategoryidAdd")).Text,
                     ((UserControlCompany)eFooteritem.FindControl("ucCompanyAdd")).SelectedCompany
                     ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDocumentManagementForm.FormInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , ((RadTextBox)eFooteritem.FindControl("txtFormNoAdd")).Text
                    , ((RadTextBox)eFooteritem.FindControl("txtFormNameAdd")).Text
                    , General.GetNullableGuid(((RadTextBox)eFooteritem.FindControl("txtCategoryidAdd")).Text)
                    , (((CheckBox)eFooteritem.FindControl("chkActiveYNAdd")).Checked == true) ? 1 : 0
                    , ((RadTextBox)eFooteritem.FindControl("txtPurposeAdd")).Text
                    , General.GetNullableInteger(((RadioButtonList)eFooteritem.FindControl("rListAdd")).SelectedItem.Value)
                    , General.GetNullableInteger(((UserControlCompany)eFooteritem.FindControl("ucCompanyAdd")).SelectedCompany)
                    );

                ucStatus.Text = "Form is added.";
                ViewState["FORMID"] = "";
                gvForm.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixDocumentManagementForm.FormDelete(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((RadLabel)eeditedItem.FindControl("lblFormId")).Text));

                ViewState["FORMID"] = "";
                ucStatus.Text = "Form is deleted.";
                gvForm.Rebind();
                //Filter.CurrentDMSFormRevision = "";   
            }
            else if (e.CommandName.ToUpper().Equals("VIEWREVISION"))
            {
                BindPageURL(nCurrentRow);
                Response.Redirect("../DocumentManagement/DocumentManagementFormRevisionList.aspx?FORMID=" + ViewState["FORMID"].ToString() + "&CATEGORYID=" + ViewState["CATEGORYID"] + "&FORMTYPE=" + ViewState["FORMTYPE"]);
            }
            else if (e.CommandName.ToUpper().Equals("DESIGNEDFORM"))
            {
                BindPageURL(nCurrentRow);
                RadLabel lblDraftRevisionId = (RadLabel)eeditedItem.FindControl("lblDraftRevisionId");
                //Response.Redirect("../StandardForm/StandardFormFBFormCreate.aspx?FormId=" + ViewState["FORMID"].ToString() + "&FORMTYPE=DMSForm&formrevisionid=" + ViewState["FLDDRAFTREVISIONID"].ToString());
                //Response.Redirect("../DocumentManagement/DocumentManagementFormDesignUpload.aspx?FORMID=" + ViewState["FORMID"].ToString() + "&formrevisionid=" + ViewState["FLDDRAFTREVISIONID"].ToString());
            }
            if (e.CommandName.ToUpper().Equals("EXPAND"))
            {
            }
            if (e.CommandName.ToUpper().Equals("EDITDETAILS"))
            {
                BindPageURL(nCurrentRow);
                LinkButton cmdedit = (LinkButton)e.Item.FindControl("cmdEditDetails");
                //cmdedit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFormListDetailsUpdate.aspx?FORMID=" + ViewState["FORMID"].ToString() + "'); return false;");
                Response.Redirect("../DocumentManagement/DocumentManagementFormListDetailsUpdate.aspx?FORMID=" + ViewState["FORMID"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private bool IsValidDate(string publishdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(publishdate))
            ucError.ErrorMessage = "Enter Publish date";
        return (!ucError.IsError);
    }

    private void AddNewRow(RadGrid gv, int row, string id, int headercount)
    {
        GridViewRow newRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
        TableCell cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.ColumnSpan = gv.Columns.Count;
        cell.Text = "<div id='div" + id + "'></div>";
        newRow.Cells.Add(cell);
        gv.Controls[0].Controls.AddAt(row + headercount + 1, newRow);
    }

    protected void gvForm_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        gvForm.Rebind();
    }

    protected void gvForm_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem item = (GridEditableItem)e.Item;
            int nCurrentRow = e.Item.ItemIndex;

            if (!IsValidField(
                    ((RadTextBox)item.FindControl("txtFormNoEdit")).Text,
                    ((RadTextBox)item.FindControl("txtFormNameEdit")).Text,
                    ((RadTextBox)item.FindControl("txtCategoryidEdit")).Text,
                    ((UserControlCompany)item.FindControl("ucCompanyEdit")).SelectedCompany
                    ))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixDocumentManagementForm.FormUpdate(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , ((RadTextBox)item.FindControl("txtFormNoEdit")).Text
                    , ((RadTextBox)item.FindControl("txtFormNameEdit")).Text
                    , General.GetNullableGuid(((RadTextBox)item.FindControl("txtCategoryidEdit")).Text)
                    , (((CheckBox)item.FindControl("chkActiveYNEdit")).Checked == true) ? 1 : 0
                    , ((RadTextBox)item.FindControl("txtPurposeedit")).Text
                    , new Guid(((RadLabel)item.FindControl("lblFormIdEdit")).Text)
                    , General.GetNullableInteger(((RadioButtonList)item.FindControl("rListEdit")).SelectedItem.Value)
                    , General.GetNullableInteger(((UserControlCompany)item.FindControl("ucCompanyEdit")).SelectedCompany)
                    );

            ucStatus.Text = "Form details updated.";
            gvForm.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvForm_ItemDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
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

            if (!e.Item.IsInEditMode)
            {
                LinkButton db = (LinkButton)item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmTelerik(event); return false;");
                }
            }

            if (ViewState["Formid"] != null)
            {
                RadLabel lblFormid = (RadLabel)item.FindControl("lblFormid");
                if (lblFormid != null)
                {
                    if (ViewState["Formid"].ToString() == lblFormid.Text)
                    {
                        AddNewRow(gvForm, e.Item.ItemIndex, lblFormid.Text, 1);
                        ImageButton cmdBDetails = (ImageButton)e.Item.FindControl("cmdBDetails");
                        if (cmdBDetails != null)
                            cmdBDetails.ImageUrl = Session["images"] + "/downarrow.png";
                    }
                }
            }

            DataRowView dr = (DataRowView)e.Item.DataItem;

            LinkButton cmgViewContent = (LinkButton)e.Item.FindControl("cmgViewContent");
            LinkButton cmdApprove = (LinkButton)e.Item.FindControl("cmdApprove");
            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            LinkButton cmdRevisionReset = (LinkButton)e.Item.FindControl("cmdRevisionReset");
            LinkButton cmdRevision = (LinkButton)e.Item.FindControl("cmdRevision");

            if (cmdApprove != null)
            {
                //cmdApprove.Attributes.Add("onclick", "return fnConfirmTelerik(event,'Are you sure you want to Approve this Form? Y/N'); return false;");
                //cmdApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagenetTimePicker.aspx); return false;");
                cmdApprove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagenetTimePicker.aspx?formid=" + dr["FLDFORMID"].ToString() + "&formrevisionid=" + dr["FLDDRAFTREVISIONID"].ToString() + "'); return false;");
            }

            if (cmdRevisionReset != null)
            {
                cmdRevisionReset.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFormRevisionReset.aspx?formid=" + dr["FLDFORMID"].ToString() + "'); return false;");
            }

            if (cmgViewContent != null && !SessionUtil.CanAccess(this.ViewState, cmgViewContent.CommandName))
                cmgViewContent.Visible = false;
            if (cmdApprove != null && !SessionUtil.CanAccess(this.ViewState, cmdApprove.CommandName))
                cmdApprove.Visible = false;
            if (cmdEdit != null && !SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName))
                cmdEdit.Visible = false;
            if (cmdDelete != null && !SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName))
                cmdDelete.Visible = false;
            if (cmdRevisionReset != null && !SessionUtil.CanAccess(this.ViewState, cmdRevisionReset.CommandName))
                cmdRevisionReset.Visible = false;
            if (cmdRevision != null && !SessionUtil.CanAccess(this.ViewState, cmdRevision.CommandName))
                cmdRevision.Visible = false;

            RadioButtonList rListEdit = (RadioButtonList)e.Item.FindControl("rListEdit");

            if (rListEdit != null)
            {
                if (dr != null && dr["FLDTYPE"].ToString() == "0")
                    rListEdit.Items[0].Selected = true;
                if (dr != null && dr["FLDTYPE"].ToString() == "1")
                    rListEdit.Items[1].Selected = true;
            }

            HyperLink hlnkfilename = (HyperLink)e.Item.FindControl("lnkfilename");
            HyperLink hlnkDraftName = (HyperLink)e.Item.FindControl("hlnkDraftName");
            //LinkButton cmdUpload = (LinkButton)e.Item.FindControl("cmdUpload");
            LinkButton cmdDesignFormUpload = (LinkButton)e.Item.FindControl("cmdDesignFormUpload");
            RadLabel lblType = (RadLabel)e.Item.FindControl("lblType");
            //if (cmdUpload != null)
            //{
            //    if (dr["FLDTYPE"] != null && dr["FLDTYPE"].ToString() == "1")
            //        cmdUpload.Visible = true;
            //    cmdUpload.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFormUpload.aspx?formid=" + dr["FLDFORMID"].ToString() + "&formrevisionid=" + dr["FLDDRAFTREVISIONID"].ToString() + "&formrevisiondtkey=" + dr["FLDFORMREVISIONDTKEY"].ToString() + "','medium'); return false;");
            //
            //    if (!SessionUtil.CanAccess(this.ViewState, cmdUpload.CommandName))
            //        cmdUpload.Visible = false;
            //}

            if (cmdDesignFormUpload != null)
            {
                if (dr["FLDTYPE"] != null && dr["FLDTYPE"].ToString() == "0")
                    cmdDesignFormUpload.Visible = true;

                if (!SessionUtil.CanAccess(this.ViewState, cmdDesignFormUpload.CommandName))
                    cmdDesignFormUpload.Visible = false;
            }

            //if (dr["FLDTYPE"] != null && dr["FLDTYPE"].ToString() == "1")
            //{
            //    if (dr["FLDAPPROVEDREVISIONDTKEY"] != null && General.GetNullableGuid(dr["FLDAPPROVEDREVISIONDTKEY"].ToString()) != null)
            //    {
            //        DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(dr["FLDAPPROVEDREVISIONDTKEY"].ToString()));
            //        if (dt.Rows.Count > 0)
            //        {
            //            DataRow drRow = dt.Rows[0];
            //            if (hlnkfilename != null)
            //            {
            //                hlnkfilename.Attributes.Add("onclick", "javascript:return openNewWindow('codehelp', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFMSSubCategoryReports.aspx?FORMID=" + dr["FLDFORMID"].ToString() + "&type=Form&FormName=" + dr["FLDCAPTION"] + "'); return false;");
            //                //hlnkfilename.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();
            //                //hlnkfilename.NavigateUrl = Session["sitepath"] + "/attachments/" + drRow["FLDFILEPATH"].ToString();
            //            }
            //        }
            //    }
            //    if (dr["FLDDRAFTREVISIONDTKEY"] != null && General.GetNullableGuid(dr["FLDDRAFTREVISIONDTKEY"].ToString()) != null)
            //    {
            //        DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(dr["FLDDRAFTREVISIONDTKEY"].ToString()));
            //        if (dt.Rows.Count > 0)
            //        {
            //            DataRow drRow = dt.Rows[0];
            //            if (hlnkDraftName != null)
            //            {
            //                hlnkfilename.Attributes.Add("onclick", "javascript:return openNewWindow('codehelp', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFMSSubCategoryReports.aspx?FORMID=" + dr["FLDFORMID"].ToString() + "&type=Form&FormName=" + dr["FLDCAPTION"] + "'); return false;");
            //                //hlnkDraftName.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();
            //                //hlnkDraftName.NavigateUrl = Session["sitepath"] + "/attachments/" + drRow["FLDFILEPATH"].ToString();
            //            }


            //        }
            //    }
            //}
            //else
            //{
            //if (dr["FLDAPPROVEDREVISIONDTKEY"] != null && General.GetNullableGuid(dr["FLDAPPROVEDREVISIONDTKEY"].ToString()) != null)
            //{
            if (hlnkfilename != null && dr["FLDFORMBUILDERID"].ToString() != null)
                //hlnkfilename.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FormId=" + dr["FLDFORMID"].ToString() + "&FORMTYPE=DMSForm&FormName=" + dr["FLDFORMDESIGNNAME"].ToString() + "&FORMREVISIONID=" + dr["FLDFORMREVISIONID"].ToString() + "'); return false;");
                //hlnkfilename.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentMangementFormDesignView.aspx?FormId=" + dr["FLDFORMBUILDERID"].ToString() + "&FormName=" + dr["FLDFORMDESIGNNAME"].ToString() + "'); return false;");
                hlnkfilename.Attributes.Add("onclick", "javascript:return openNewWindow('formList', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFMSSubCategoryReports.aspx?typeid=" + lblType.Text + "&FORMID=" + dr["FLDFORMID"].ToString() + "&type=Form&FormName=" + dr["FLDCAPTION"] + "'); return false;");

            //}
            //    if (dr["FLDDRAFTREVISIONDTKEY"] != null && General.GetNullableGuid(dr["FLDDRAFTREVISIONDTKEY"].ToString()) != null)
            //    {
            //        if (hlnkDraftName != null && dr["FLDFORMBUILDERID"].ToString() != null)
            //            //hlnkDraftName.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FormId=" + dr["FLDFORMID"].ToString() + "&FORMTYPE=DMSForm" + "&FORMREVISIONID=" + dr["FLDDRAFTREVISIONID"].ToString() + "'); return false;");
            //            //hlnkDraftName.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentMangementFormDesignView.aspx?FORMID=" + dr["FLDFORMID"].ToString() + "&FORMREVISIONID=" + dr["FLDDRAFTREVISIONID"].ToString() + "'); return false;");
            //            hlnkfilename.Attributes.Add("onclick", "javascript:return openNewWindow('codehelp', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFMSSubCategoryReports.aspx?FORMID=" + dr["FLDFORMID"].ToString() + "&type=Form&FormName=" + dr["FLDCAPTION"] + "'); return false;");
            //    }
            //}

            RadDropDownList ddlcategoryEdit = (RadDropDownList)e.Item.FindControl("ddlCategoryIdEdit");
            DataRowView drvCategory = (DataRowView)e.Item.DataItem;

            ImageButton btnShowCategoryEdit = (ImageButton)e.Item.FindControl("btnShowCategoryEdit");
            if (btnShowCategoryEdit != null)
            {
                btnShowCategoryEdit.Attributes.Add("onclick", "return showPickList('spnPickListCategoryedit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDocumentCategory.aspx', true); ");
            }
            RadTextBox txtCategoryEdit = (RadTextBox)e.Item.FindControl("txtCategoryEdit");
            if (txtCategoryEdit != null)
                txtCategoryEdit.Text = drvCategory["FLDCATEGORYNAME"].ToString();
            RadTextBox txtCategoryidEdit = (RadTextBox)e.Item.FindControl("txtCategoryidEdit");
            if (txtCategoryidEdit != null)
                txtCategoryidEdit.Text = drvCategory["FLDCATEGORYID"].ToString();

            if (ddlcategoryEdit != null)
            {
                ddlcategoryEdit.DataSource = PhoenixDocumentManagementCategory.ListDocumentCategory();
                ddlcategoryEdit.DataTextField = "FLDCATEGORYNAME";
                ddlcategoryEdit.DataValueField = "FLDCATEGORYID";
                ddlcategoryEdit.DataBind();
                ddlcategoryEdit.SelectedValue = drvCategory["FLDCATEGORYID"].ToString();

            }

            RadLabel lblAddedByName = (RadLabel)e.Item.FindControl("lblAddedByName");
            UserControlToolTip ucAddedByNameTT = (UserControlToolTip)e.Item.FindControl("ucAddedByNameTT");
            if (lblAddedByName != null)
            {
                ucAddedByNameTT.Position = ToolTipPosition.BottomCenter;
                ucAddedByNameTT.TargetControlId = lblAddedByName.ClientID;
            }
            UserControlToolTip ucFilenameTT = (UserControlToolTip)e.Item.FindControl("ucFilenameTT");
            if (hlnkfilename != null)
            {
                ucFilenameTT.Position = ToolTipPosition.BottomCenter;
                ucFilenameTT.TargetControlId = hlnkfilename.ClientID;
            }

            UserControlToolTip ucPurpose = (UserControlToolTip)e.Item.FindControl("ucPurpose");
            RadLabel lblPurpose = (RadLabel)e.Item.FindControl("lblPurpose");
            if (lblPurpose != null)
            {
                ucPurpose.Position = ToolTipPosition.BottomCenter;
                ucPurpose.TargetControlId = lblPurpose.ClientID;
                //lblPurpose.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucPurpose.ToolTip + "', 'visible');");
                //lblPurpose.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucPurpose.ToolTip + "', 'hidden');");
            }
            DataRowView dv = (DataRowView)e.Item.DataItem;
            UserControlCompany ucCompanyEdit = (UserControlCompany)e.Item.FindControl("ucCompanyEdit");
            if (ucCompanyEdit != null)
            {
                ucCompanyEdit.CompanyList = PhoenixRegistersCompany.ListCompany();
                ucCompanyEdit.SelectedCompany = dv["FLDCOMPANYID"].ToString();
            }

            LinkButton cmdVesselList = (LinkButton)e.Item.FindControl("cmdVesselList");
            if (cmdVesselList != null)
            {
                cmdVesselList.Attributes.Add("onclick", "javascript:return openNewWindow('formList', '', 'DocumentManagement/DocumentManagementFMSSubCategoryReports.aspx?FORMID=" + dv["FLDFORMID"].ToString() + "&type=Form&FormName=" + dv["FLDCAPTION"] + "'); return false;");
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            ImageButton btnShowCategoryAdd = (ImageButton)e.Item.FindControl("btnShowCategoryAdd");
            if (btnShowCategoryAdd != null)
            {
                btnShowCategoryAdd.Attributes.Add("onclick", "return showPickList('spnPickListCategoryAdd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDocumentCategory.aspx', true); ");
            }

            RadDropDownList ddlcategoryAdd = (RadDropDownList)e.Item.FindControl("ddlCategoryIdAdd");
            if (ddlcategoryAdd != null)
            {
                ddlcategoryAdd.DataSource = PhoenixDocumentManagementCategory.ListDocumentCategory();
                ddlcategoryAdd.DataTextField = "FLDCATEGORYNAME";
                ddlcategoryAdd.DataValueField = "FLDCATEGORYID";
                ddlcategoryAdd.DataBind();
            }
            UserControlCompany ucCompanyAdd = (UserControlCompany)e.Item.FindControl("ucCompanyAdd");
            if (ucCompanyAdd != null)
            {
                ucCompanyAdd.CompanyList = PhoenixRegistersCompany.ListCompany();
                ucCompanyAdd.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
            }
        }
    }

    protected void gvForm_SelectedIndexChanging(object sender, GridSelectCommandEventArgs se)
    {
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            //string previousformid = ViewState["FORMID"].ToString();
            ViewState["FORMID"] = ((RadLabel)gvForm.Items[rowindex].FindControl("lblFormId")).Text;
            ViewState["FORMREVISIONID"] = ((RadLabel)gvForm.Items[rowindex].FindControl("lblFormRevisionId")).Text;
            ViewState["FLDDRAFTREVISIONID"] = ((RadLabel)gvForm.Items[rowindex].FindControl("lblDraftRevisionId")).Text;
            ViewState["FORMTYPE"] = ((RadLabel)gvForm.Items[rowindex].FindControl("lblType")).Text;
            //if (previousformid != ViewState["FORMID"].ToString())
            //    Filter.CurrentDMSFormRevision = "";           
            SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidField(string formno, string formname, string categoryid, string companyid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(formno) == null)
            ucError.ErrorMessage = "Form Number is required.";

        if (General.GetNullableString(formname) == null)
            ucError.ErrorMessage = "Form Name is required.";

        if (General.GetNullableGuid(categoryid) == null)
            ucError.ErrorMessage = "Form Category is required.";

        if (General.GetNullableInteger(companyid) == null)
            ucError.ErrorMessage = "Company is required.";

        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void SetRowSelection()
    {

        foreach (GridDataItem item in gvForm.Items)
        {

            if (item.GetDataKeyValue("FLDFORMID").ToString().Equals(ViewState["FORMID"].ToString()))
            {
                gvForm.SelectedIndexes.Add(item.ItemIndex);
                //ViewState["DOCUMENTID"] = item.GetDataKeyValue("FLDDOCUMENTID").ToString();
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //gvForm.CurrentPageIndex = 0;
        gvForm.Rebind();
    }

    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedForms = new ArrayList();

        Guid index = new Guid();
        foreach (GridDataItem gvrow in gvForm.Items)
        {
            bool result = false;

            if (gvrow.GetDataKeyValue("FLDFORMID").ToString() != "")
            {
                index = new Guid(gvrow.GetDataKeyValue("FLDFORMID").ToString());

                if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
                {
                    result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
                }

                // Check in the Session
                if (Filter.CurrentSelectedForms != null)
                    SelectedForms = (ArrayList)Filter.CurrentSelectedForms;
                if (result)
                {
                    if (!SelectedForms.Contains(index))
                        SelectedForms.Add(index);
                }
                else
                    SelectedForms.Remove(index);
            }
        }
        if (SelectedForms != null && SelectedForms.Count > 0)
            Filter.CurrentSelectedForms = SelectedForms;
    }

    private void GetSelectedForms()
    {
        if (Filter.CurrentSelectedForms != null)
        {
            ArrayList SelectedForms = (ArrayList)Filter.CurrentSelectedForms;
            Guid index = new Guid();
            if (SelectedForms != null && SelectedForms.Count > 0)
            {
                foreach (GridDataItem row in gvForm.Items)
                {
                    RadCheckBox chk = (RadCheckBox)(row.Cells[0].FindControl("chkSelect"));
                    index = new Guid(row.GetDataKeyValue("FLDFORMID").ToString());
                    if (SelectedForms.Contains(index))
                    {
                        RadCheckBox myCheckBox = (RadCheckBox)row.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }
    }

    protected void CheckAll(object sender, EventArgs e)
    {
        RadCheckBox headerCheckBox = (sender as RadCheckBox);
        ArrayList SelectedForms = new ArrayList();
        Guid index = new Guid();
        foreach (GridDataItem dataItem in gvForm.MasterTableView.Items)
        {
            bool result = false;
            if (headerCheckBox.Checked == true)
            {
                if (dataItem.GetDataKeyValue("FLDFORMID").ToString() != "")
                {
                    index = new Guid(dataItem.GetDataKeyValue("FLDFORMID").ToString());
                    (dataItem.FindControl("chkSelect") as RadCheckBox).Checked = true;
                    result = true;
                }
                //dataItem.Selected = true;
            }
            else
            {
                (dataItem.FindControl("chkSelect") as RadCheckBox).Checked = false;
                Filter.CurrentSelectedForms = null;
            }

            // Check in the Session
            if (Filter.CurrentSelectedForms != null)
                SelectedForms = (ArrayList)Filter.CurrentSelectedForms;
            if (result)
            {
                if (!SelectedForms.Contains(index))
                    SelectedForms.Add(index);
            }
            else
                SelectedForms.Remove(index);
        }
        if (SelectedForms != null && SelectedForms.Count > 0)
            Filter.CurrentSelectedForms = SelectedForms;
    }
}
