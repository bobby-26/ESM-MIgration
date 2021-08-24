using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class Accounts_AccountsProjectLineItemConfirmation : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            PhoenixToolbar toolmain = new PhoenixToolbar();

            toolmain.AddButton("Fund Request", "FUNDREQUEST", ToolBarDirection.Right);
            toolmain.AddButton("Line Item", "LINEITEM", ToolBarDirection.Right);
            toolmain.AddButton("Project", "PROJECTCODE", ToolBarDirection.Right);

            Menu.AccessRights = this.ViewState;
            Menu.MenuList = toolmain.Show();
            Menu.SelectedMenuIndex = 1;

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Vouchers", "VOUCHERREGISTER", ToolBarDirection.Right);
            toolbarmain.AddButton("Purchase Order", "PO", ToolBarDirection.Right);
            toolbarmain.AddButton("Awaiting Confirmation", "AWAITINGORDER", ToolBarDirection.Right);

            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.MenuList = toolbarmain.Show();
            MenuLineItem.SelectedMenuIndex = 2;

            if (!IsPostBack)
            {
                ViewState["PROJECTID"] = null;
                ViewState["ACCOUNTID"] = "";

                if (Request.QueryString["id"] != null)
                {
                    ViewState["PROJECTID"] = Request.QueryString["id"].ToString();
                }
                if (Request.QueryString["accountid"] != null)
                {
                    ViewState["ACCOUNTID"] = Request.QueryString["accountid"].ToString();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsProjectLineItemConfirmation.aspx", "Export to Excel", "icon_xls.png", "Excel"); ;
            toolbargrid.AddImageLink("javascript:CallPrint('gvLineItem')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageLink("../Accounts/AccountsProjectLineItemConfirmation.aspx", "Filter", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsProjectLineItemConfirmation.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbargrid.AddImageButton("../Accounts/AccountsProjectLineItemConfirmation.aspx", "Add", "Add.png", "ADD");

            MenuOrderLineItem.AccessRights = this.ViewState;
            MenuOrderLineItem.MenuList = toolbargrid.Show();

            //BindData();
            ProjectCodeEdit();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Menu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (ViewState["PROJECTID"] != null)
            {
                if (CommandName.ToUpper().Equals("PROJECTCODE"))
                {
                    Response.Redirect("../Accounts/AccountsProjectList.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString());
                }

                if (CommandName.ToUpper().Equals("FUNDREQUEST"))
                {
                    Response.Redirect("../Accounts/AccountsProjectCodeFundRequest.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString()); 
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (ViewState["PROJECTID"] != null)
            {
                if (CommandName.ToUpper().Equals("AWAITINGORDER"))
                {
                    Response.Redirect("../Accounts/AccountsProjectLineItemConfirmation.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString()); 
                }
                else if (CommandName.ToUpper().Equals("PO"))
                {
                    Response.Redirect("../Accounts/AccountsProjectLineItemPurchaseOrder.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString());
                }
                else if (CommandName.ToUpper().Equals("VOUCHERREGISTER"))
                {
                    Response.Redirect("../Accounts/AccountsProjectLineItemVoucherRegister.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString()); 
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("ADD"))
            {
                String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsProjectLineItemConfirmationAdd.aspx?id=" + ViewState["PROJECTID"].ToString() + " ');");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.ProjectCodeConfirmationListFilter = null;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                if (ViewState["PROJECTID"] != null)
                {
                    Response.Redirect("../Accounts/AccountsProjectLineItemConfirmationFilter.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString());
                }
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDREQUISITIONNUMBER", "FLDSUPPLIERCODE", "FLDSUPPLIERNAME", "FLDREQUESTDATE", "FLDREQUISITIONSTATUS", "FLDREQUISITIONAMOUNT", "FLDESTIMATEDTOTAL", "FLDBUDGETCODE", "FLDOWNERBUDGETCODE" };
                string[] alCaptions = { "Requisition Number", "Supplier Code", "Supplier Name", "Request Date", "Requisition Status", " 	Requisition Amount", "Estimated Amount", "Budget Code", "Owner Budget Code" };

                string sortexpression;
                int? sortdirection = 1;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                NameValueCollection nvc = Filter.ProjectCodeConfirmationListFilter;

                DataTable dt = PhoenixAccountProjectConfirmationLineItem.ConfirmationLineItemSearch(General.GetNullableInteger(ViewState["PROJECTID"].ToString())
                                                                         , nvc != null ? General.GetNullableString(nvc.Get("txtRequisitionNo")) : null
                                                                         , nvc != null ? General.GetNullableInteger(nvc.Get("ddlType")) : null
                                                                         , nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId")) : null
                                                                         , sortexpression, sortdirection
                                                                         , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                                         , General.ShowRecords(null)
                                                                         , ref iRowCount, ref iTotalPageCount
                                                                         );

                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Awaiting Confirmation", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDREQUISITIONNUMBER", "FLDSUPPLIERCODE", "FLDSUPPLIERNAME", "FLDREQUESTDATE", "FLDREQUISITIONSTATUS", "FLDREQUISITIONAMOUNT", "FLDESTIMATEDTOTAL", "FLDBUDGETCODE", "FLDOWNERBUDGETCODE" };
            string[] alCaptions = { "Requisition Number", "Supplier Code", "Supplier Name", "Request Date", "Requisition Status", "Requisition Amount", "Estimated Amount", "Budget Code", "Owner Budget Code" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            if (ViewState["PROJECTID"] != null)
            {
                NameValueCollection nvc = Filter.ProjectCodeConfirmationListFilter;

                DataTable dt = PhoenixAccountProjectConfirmationLineItem.ConfirmationLineItemSearch(General.GetNullableInteger(ViewState["PROJECTID"].ToString())
                                                                        , nvc != null ? General.GetNullableString(nvc.Get("txtRequisitionNo")) : null
                                                                        , nvc != null ? General.GetNullableInteger(nvc.Get("ddlType")) : null
                                                                        , nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId")) : null
                                                                        , sortexpression, sortdirection
                                                                        , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                                        , General.ShowRecords(null)
                                                                        , ref iRowCount, ref iTotalPageCount
                );

                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                General.SetPrintOptions("gvLineItem", "Awaiting Confirmation", alCaptions, alColumns, ds);


                gvLineItem.DataSource = dt;
                gvLineItem.VirtualItemCount = iRowCount;

            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ProjectCodeEdit()
    {
        if (ViewState["PROJECTID"] != null)
        {

            DataTable dt = PhoenixAccountProject.ProjectEdit(int.Parse(ViewState["PROJECTID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                lbltotal.Text = dt.Rows[0]["FLDAWAITINGAMOUNT"].ToString();
                Title1.Text = "Project Code (" + dt.Rows[0]["FLDPROJECTCODE"].ToString() + ")";
            }
        }
    }


    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.ItemIndex;
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string txtEstimatedAmountEdit = ((TextBox)_gridView.Items[nCurrentRow].FindControl("txtEstimatedAmountEdit")).Text;
                string lblConfirmationLineItemId = ((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblConfirmationLineItemIdEdit")).Text;
                if (General.GetNullableGuid(lblConfirmationLineItemId) != null)
                {
                    PhoenixAccountProjectConfirmationLineItem.ConfirmationLineItemUpdate(General.GetNullableDecimal(txtEstimatedAmountEdit), new Guid(lblConfirmationLineItemId));
                }
                Rebind();
                ProjectCodeEdit();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string lblConfirmationLineItemId = ((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblConfirmationLineItemId")).Text;
                string lblConfirmationType = ((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblConfirmationType")).Text;
                if (General.GetNullableGuid(lblConfirmationLineItemId) != null && General.GetNullableInteger(lblConfirmationType) != null)
                {
                    PhoenixAccountProjectConfirmationLineItem.DeleteConfirmationLineiTem(new Guid(lblConfirmationLineItemId), General.GetNullableInteger(lblConfirmationType));
                }

                Rebind();
                ProjectCodeEdit();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvLineItem_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = de.RowIndex;
    //        string lblConfirmationLineItemId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblConfirmationLineItemId")).Text;
    //        string lblConfirmationType = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblConfirmationType")).Text;
    //        if (General.GetNullableGuid(lblConfirmationLineItemId) != null && General.GetNullableInteger(lblConfirmationType) != null)
    //        {
    //            PhoenixAccountProjectConfirmationLineItem.DeleteConfirmationLineiTem(new Guid(lblConfirmationLineItemId), General.GetNullableInteger(lblConfirmationType));
    //        }

    //        Rebind();
    //        ProjectCodeEdit();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvLineItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    try
    //    {
    //        int nCurrentRow = e.RowIndex;

    //        string txtEstimatedAmountEdit = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtEstimatedAmountEdit")).Text;
    //        string lblConfirmationLineItemId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblConfirmationLineItemIdEdit")).Text;
    //        if (General.GetNullableGuid(lblConfirmationLineItemId) != null)
    //        {
    //            PhoenixAccountProjectConfirmationLineItem.ConfirmationLineItemUpdate(General.GetNullableDecimal(txtEstimatedAmountEdit), new Guid(lblConfirmationLineItemId));
    //        }
    //        _gridView.EditIndex = -1;
    //        Rebind();
    //        ProjectCodeEdit();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvLineItem_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        Rebind();
    }

    //protected void gvLineItem_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        _gridView.EditIndex = de.NewEditIndex;
    //        _gridView.SelectedIndex = de.NewEditIndex;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvLineItem_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
    //    {
    //        e.Row.TabIndex = -1;
    //        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvLineItem, "Edit$" + e.Row.RowIndex.ToString(), false);
    //    }
    //}

    protected void gvLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        //   if (e.Item.RowType == DataControlRowType.DataRow)
        {
            // if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
            //   && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                ImageButton ed = (ImageButton)e.Item.FindControl("cmdEdit");
                if (ed != null)
                    ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvLineItem.SelectedIndexes.Clear();
        gvLineItem.EditIndexes.Clear();
        gvLineItem.DataSource = null;
        gvLineItem.Rebind();
    }

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //    gv.Rows[0].Attributes["onclick"] = "";
    //}

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int result;
    //        if (Int32.TryParse(txtnopage.Text, out result))
    //        {
    //            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


    //            if (0 >= Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = 1;

    //            if ((int)ViewState["PAGENUMBER"] == 0)
    //                ViewState["PAGENUMBER"] = 1;

    //            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //        }
    //        BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvLineItem.SelectedIndex = -1;
    //    gvLineItem.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;
    //    BindData();
    //    SetPageNavigator();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}
    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}
    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}



    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLineItem.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}