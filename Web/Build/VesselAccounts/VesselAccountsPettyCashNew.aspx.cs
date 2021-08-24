using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;

public partial class VesselAccountsPettyCashNew :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../VesselAccounts/VesselAccountsPettyCashNew.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCTM')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../VesselAccounts/VesselAccountsPettyCashNew.aspx", "New Captain Cash Report", "add.png", "NEW");
            MenuCTM.AccessRights = this.ViewState;
            MenuCTM.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }
            BindData();
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
                DateTime nextfromdate = new DateTime();
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

                string[] alColumns = { "FLDFROMDATE", "FLDDATE", "FLDOPENINGBALANCE", "FLDCLOSINGBALANCE" };
                string[] alCaptions = { "From", "To", "Opening Balance", "Closing Balance" };

                DataSet ds = PhoenixVesselAccountsCTMNew.SearchCaptainCashCTM(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                               , null, null
                                                                               , sortexpression, sortdirection
                                                                               , 1
                                                                               , iRowCount, ref iRowCount, ref iTotalPageCount, ref nextfromdate);
                General.ShowExcel("Captain Cash", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsPettyCashGeneralNew.aspx?type=n&fromdate=" + ViewState["FROMDATE"].ToString(), false);
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
            DateTime nextfromdate = new DateTime();
            string[] alColumns = { "FLDFROMDATE", "FLDDATE", "FLDOPENINGBALANCE", "FLDCLOSINGBALANCE" };
            string[] alCaptions = { "From", "To", "Opening Balance", "Closing Balance" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixVesselAccountsCTMNew.SearchCaptainCashCTM(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , null, null
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, ref nextfromdate);
            General.SetPrintOptions("gvCTM", "Captain Cash", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCTM.DataSource = ds;
                gvCTM.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCTM);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();
            ViewState["FROMDATE"] = nextfromdate.ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCTM_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                _gridView.SelectedIndex = nCurrentRow;
                string todate = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDate")).Text;
                string fromdate = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFromDate")).Text;
                Response.Redirect("../VesselAccounts/VesselAccountsPettyCashGeneralNew.aspx?fromdate=" + fromdate + "&todate=" + todate, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidCTM(string fromdate, string Todate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;

        if (!General.GetNullableDateTime(Todate).HasValue)
        {
            ucError.ErrorMessage = "Closing Date is required.";
        }
        else if (DateTime.TryParse(Todate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Closing Date should be earlier than current date";
        }
        if (General.GetNullableDateTime(fromdate).HasValue && General.GetNullableDateTime(Todate).HasValue
            && (General.GetNullableDateTime(fromdate).Value.Month != General.GetNullableDateTime(Todate).Value.Month
            || General.GetNullableDateTime(fromdate).Value.Year != General.GetNullableDateTime(Todate).Value.Year))
        {
            ucError.ErrorMessage = "Captain Cash can only be locked if the From Date and To Date are of the same month.";
        }
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
        gvCTM.SelectedIndex = -1;
        gvCTM.EditIndex = -1;
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
    //protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //} 
    //protected void UnLockCTM_Confirm(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
    //        if (uc.confirmboxvalue == 1)
    //        {
    //            PhoenixVesselAccountsCTMNew.DeleteCaptainCashBalance(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(ViewState["DATE"].ToString()));
    //            BindData();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void LockCTM_Confirm(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
    //        // if (uc.confirmboxvalue == 1)
    //        //{
    //        //    PhoenixVesselAccountsCTMNew.InsertCaptainCashBalance(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(txtClosingDate.Text), null, decimal.Parse(ViewState["OPENINGBALANCE"].ToString()), decimal.Parse(txtclosingbalance.Text));
    //        //    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
    //        //    Response.Redirect("../VesselAccounts/VesselAccountsPettyCashNew.aspx", false);
    //        //}
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
}
