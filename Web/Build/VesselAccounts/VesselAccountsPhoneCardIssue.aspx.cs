using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Generic;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;

public partial class VesselAccountsPhoneCardIssue : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {
            CreateMenu();
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ddlHard.HardTypeCode = "97";
                ddlHard.HardList = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 97, 0, "PHC");
                ddlHard.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 97, "PHC");

                ViewState["ROB"] = string.Empty;
                ViewState["UNITPRICE"] = string.Empty;
                ViewState["UNITNAME"] = string.Empty;
                ViewState["AMOUNT"] = string.Empty;
                ViewState["TOTALAMOUNT"] = string.Empty;
                ViewState["STOREITEMID"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
            CreateMenu();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuBondIssue_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {

                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDSTORENAME", "FLDUNITNAME", "FLDQUANTITY", "FLDUNITPRICE", "FLDAMOUNT", "FLDISSUEDATE", "FLDREMARKS" };
                string[] alCaptions = { "Item Name", "Unit", "Sold Quantity", "Unit Price", "Amount", "Issue Date", "Remarks" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixVesselAccountsStoreIssue.SearchStoreIssue(General.GetNullableInteger(ddlEmployee.SelectedEmployee), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                        , General.GetNullableInteger(ddlHard.SelectedHard)
                                                                        , General.GetNullableDateTime(txtFromDate.Text)
                                                                        , General.GetNullableDateTime(txtToDate.Text)
                                                                        , sortexpression, sortdirection
                                                                        , 1, iRowCount
                                                                        , ref iRowCount, ref iTotalPageCount);
                General.ShowExcel("Issue of Bonded Stores", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;
            string[] alColumns = { "FLDSTORENAME", "FLDUNITNAME", "FLDQUANTITY", "FLDUNITPRICE", "FLDAMOUNT", "FLDISSUEDATE", "FLDREMARKS" };
            string[] alCaptions = { "Item Name", "Unit", "Sold Quantity", "Unit Price", "Amount", "Issue Date", "Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixVesselAccountsStoreIssue.SearchStoreIssue(General.GetNullableInteger(ddlEmployee.SelectedEmployee), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , General.GetNullableInteger(ddlHard.SelectedHard)
                            , General.GetNullableDateTime(txtFromDate.Text)
                            , General.GetNullableDateTime(txtToDate.Text)
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvCrewSearch", "Issue of Bonded Stores", alCaptions, alColumns, ds);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["TOTALAMOUNT"] = ds.Tables[0].Rows[0]["FLDTOTALAMOUNT"].ToString();
                gvCrewSearch.DataSource = ds;
                gvCrewSearch.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCrewSearch);
                ViewState["TOTALAMOUNT"] = "";
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvCrewSearch_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
            PhoenixVesselAccountsCorrections.DeleteAdminStoreIssue(id);
            ViewState["ROB"] = string.Empty;
            ViewState["UNITPRICE"] = string.Empty;
            ViewState["UNITNAME"] = string.Empty;
            ViewState["AMOUNT"] = string.Empty;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvCrewSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.HorizontalAlign = HorizontalAlign.Right;
            e.Row.Font.Bold = true;
            ((Label)e.Row.FindControl("lblAmount")).Text = ViewState["TOTALAMOUNT"].ToString();
        }
    }
    protected void gvCrewSearch_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvCrewSearch_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = e.NewEditIndex;
        BindData();
    }
    protected void gvCrewSearch_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            GridViewRow gr = _gridView.Rows[nCurrentRow];
            Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
            string rob = ((Label)gr.FindControl("lblRobEdit")).Text;
            string quantity = ((UserControlMaskNumber)gr.FindControl("txtQuantityEdit")).Text;
            string date = ((UserControlDate)gr.FindControl("txtDateEdit")).Text;
            string remarks = ((TextBox)gr.FindControl("txtRemarksEdit")).Text;
            string oldqty = ((Label)gr.FindControl("lblOldQty")).Text;
            if (!IsValidIssue(ddlEmployee.SelectedEmployee, id.ToString(), date, quantity, rob, oldqty))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixVesselAccountsStoreIssue.UpdateStoreIssue(id, DateTime.Parse(date), decimal.Parse(quantity), remarks, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
    }

    private bool IsValidIssue(string employeeid, string storeitem, string date, string quantity, string rob, string oldqty)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        //decimal resultDecimal;
        decimal varience = decimal.Parse(String.IsNullOrEmpty(oldqty) ? "0" : oldqty) - decimal.Parse(String.IsNullOrEmpty(quantity) ? "0" : quantity);

        if (!General.GetNullableInteger(employeeid).HasValue)
        {
            ucError.ErrorMessage = "Staff Name is required.";
        }
        if (string.IsNullOrEmpty(storeitem))
            ucError.ErrorMessage = "Item is required.";
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Issue Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be earlier than current date";
        }
        if (!General.GetNullableDecimal(quantity).HasValue)
        {
            ucError.ErrorMessage = "Sold Quantity is required.";
        }
        else if (decimal.Parse(quantity) <= 0)
        {
            ucError.ErrorMessage = "Sold Quantity should not be zero or negative";
        }
        if (varience < 0 && (varience + decimal.Parse(rob == string.Empty ? "0" : rob)) < 0)
            ucError.ErrorMessage = "Please check your stock. Sold Quantity cannot be greater than ROB.";
        //if (decimal.TryParse(quantity, out resultDecimal) && resultDecimal > decimal.Parse(rob == string.Empty ? "0" : rob))
        //    ucError.ErrorMessage = "Please check your stock. Sold Quantity cannot be greater than ROB.";
        return (!ucError.IsError);
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvCrewSearch.SelectedIndex = -1;
        gvCrewSearch.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            string stockitemid = nvc.Get("StoreItemId").ToString();
            string qty = nvc.Get("Quantity");
            if (General.GetNullableInteger(ddlEmployee.SelectedEmployee).HasValue && stockitemid.Trim() != string.Empty)
            {
                PhoenixVesselAccountsStoreIssue.InsertStoreIssue(PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(ddlEmployee.SelectedEmployee)
                        , stockitemid.Replace('¿', ','), General.GetNullableDateTime(nvc.Get("IssueDate")), qty.Replace('¿', ','), nvc.Get("Remarks").ToString());
                gvCrewSearch.EditIndex = -1;
                gvCrewSearch.SelectedIndex = -1;
                BindData();
            }
            else
            {
                string message = "Select a Staff to add Items";
                if (stockitemid.Trim() == string.Empty)
                    message = "No Items selected.";
                ucError.ErrorMessage = message;
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetPrice(Guid StoreItemId)
    {
        DataTable dt = PhoenixVesselAccountsEmployee.ListVesselStoreItemROB(StoreItemId);
        if (dt.Rows.Count > 0)
        {
            ViewState["ROB"] = dt.Rows[0]["FLDROB"].ToString();
            ViewState["UNITPRICE"] = dt.Rows[0]["FLDUNITPRICE"].ToString();
            ViewState["UNITNAME"] = dt.Rows[0]["FLDUNITNAME"].ToString();
        }
    }
    private void CreateMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../VesselAccounts/VesselAccountsPhoneCardIssue.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvCrewSearch')", "Print Grid", "icon_print.png", "PRINT");
        //toolbar.AddImageLink("javascript:showPickList('spnPickListStore', 'codehelp1', '', 'VesselAccountsStoreIssueStoreItemSelection.aspx?storeclass=" + ddlHard.SelectedHard + "&accountfor=" + ddlEmployee.SelectedEmployee + "', true); return false;", "Store Item", "add.png", "ADDSTORE");
        toolbar.AddImageButton("../VesselAccounts/VesselAccountsPhoneCardIssue.aspx", "Find", "search.png", "FIND");
        MenuBondIssue.AccessRights = this.ViewState;
        MenuBondIssue.MenuList = toolbar.Show();
    }
    protected void ddlEmployee_TextChangedEvent(object sender, EventArgs e)
    {
        CreateMenu();
        BindData();
    }
}
