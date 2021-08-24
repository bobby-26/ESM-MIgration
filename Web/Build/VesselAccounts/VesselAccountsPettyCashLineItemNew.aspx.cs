using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;

public partial class VesselAccountsPettyCashLineItemNew :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                DateTime firstDayOfTheMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                txtFromDate.Text = firstDayOfTheMonth.ToString();
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddImageButton("../VesselAccounts/VesselAccountsPettyCashLineItemNew.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbarmain.AddImageLink("javascript:CallPrint('gvPettyCash')", "Print Grid", "icon_print.png", "PRINT");
            toolbarmain.AddImageButton("../VesselAccounts/VesselAccountsPettyCashLineItemNew.aspx", "Search", "search.png", "SEARCH");
            toolbarmain.AddImageButton("../VesselAccounts/VesselAccountsPettyCashLineItemNew.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbarmain.AddImageButton("../VesselAccounts/VesselAccountsPettyCashLineItemNew.aspx", "Add New", "add.png", "NEW");
            MenuCTM.MenuList = toolbarmain.Show();
            BindData();
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
            int iTotalPageCount = 10;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            string[] alColumns = { "FLDSEAPORTNAME", "FLDDATE", "FLDPURPOSE", "FLDPAYMENTRECEIPT", "FLDAMOUNT" };
            string[] alCaptions = { "Port", "Expenses On", "Purpose", "Type", "Amount" };
            DataSet ds = PhoenixVesselAccountsCTMNew.SearchCaptainPettyCash(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                            , General.GetNullableDateTime(txtFromDate.Text)
                                                            , General.GetNullableDateTime(txtToDate.Text)
                                                            , sortexpression, sortdirection,
                                                            Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null),
                                                            ref iRowCount,
                                                            ref iTotalPageCount);
            General.SetPrintOptions("gvPettyCash", "Expenses", alCaptions, alColumns, ds);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gvPettyCash.DataSource = ds;
                gvPettyCash.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvPettyCash);
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

    protected void MenuCTM_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                ViewState["PAGENUMBER"] = 1;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                string[] alColumns = { "FLDSEAPORTNAME", "FLDDATE", "FLDPURPOSE", "FLDPAYMENTRECEIPT", "FLDAMOUNT" };
                string[] alCaptions = { "Port", "Expenses On", "Purpose", "Type", "Amount" };

                DataSet ds = PhoenixVesselAccountsCTMNew.SearchCaptainPettyCash(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                           , General.GetNullableDateTime(txtFromDate.Text)
                                                           , General.GetNullableDateTime(txtToDate.Text)
                                                           , sortexpression, sortdirection,
                                                           Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), iRowCount,
                                                           ref iRowCount,
                                                           ref iTotalPageCount);

                General.ShowExcel("Expenses", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            if (dce.CommandName.ToUpper().Equals("SEARCH"))
            {
                BindData();
            }
            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                DateTime firstDayOfTheMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                txtFromDate.Text = firstDayOfTheMonth.ToString();
                txtToDate.Text = string.Empty;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                txtnopage.Text = "";
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsPettyCashLineItemAddNew.aspx", false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPettyCash_RowCommand(object sender, GridViewCommandEventArgs e)
    {
      
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
                Response.Redirect("../VesselAccounts/VesselAccountsPettyCashLineItemAddNew.aspx?id="+id.ToString(),true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPettyCash_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
            PhoenixVesselAccountsCTMNew.DeleteCaptainPettyCash(id);
            _gridView.EditIndex = -1;
            BindData();
            String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvPettyCash_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            ImageButton re = (ImageButton)e.Row.FindControl("cmdRecoverable");
            if (re != null)
            {
                re.Visible = SessionUtil.CanAccess(this.ViewState, re.CommandName);
                re.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','VesselAccountsCrewRecoverable.aspx?pcid=" + drv["FLDPETTYCASHID"].ToString() + "'); return false;");
            }

        }
    }
   
    private bool IsValidPettyCash(string seaport, string date, string purpose, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableInteger(seaport).HasValue)
        {
            ucError.ErrorMessage = "Port is required.";
        }

        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Expenses On is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Expenses On should be earlier than current date";
        }

        if (string.IsNullOrEmpty(purpose))
            ucError.ErrorMessage = "Purpose is required.";

        if (!General.GetNullableDecimal(amount).HasValue)
        {
            ucError.ErrorMessage = "Amount is required.";
        }

        return (!ucError.IsError);
    }
    decimal r, p;
    protected void gvCTM_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            r = 0;
            p = 0;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (drv["FLDCREDITDEBIT"].ToString().ToString() != string.Empty && ",1,".Contains("," + drv["FLDCREDITDEBIT"].ToString() + ",")) r = r + decimal.Parse(drv["FLDAMOUNT"].ToString());
            if (drv["FLDCREDITDEBIT"].ToString().ToString() != string.Empty && !",1,".Contains("," + drv["FLDCREDITDEBIT"].ToString() + ",")) p = p + decimal.Parse(drv["FLDAMOUNT"].ToString());
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = r.ToString();
            e.Row.Cells[3].Text = p.ToString();
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["DATE"] = null;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
        gvPettyCash.SelectedIndex = -1;
        gvPettyCash.EditIndex = -1;
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
        lblPagenumber.Text = "Page " + (ViewState["TOTALPAGECOUNT"].ToString() == "0" ? ViewState["TOTALPAGECOUNT"].ToString() : ViewState["PAGENUMBER"].ToString());
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


}