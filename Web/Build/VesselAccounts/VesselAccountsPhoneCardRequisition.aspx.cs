using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;


public partial class VesselAccountsPhoneCardRequisition : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsPhoneCardRequisition.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvPhonReq')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsPhoneCardRequisitionFilter.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsPhoneCardRequisition.aspx", "Clear", "clear-filter.png", "CLEAR");
            MenuBondReq.AccessRights = this.ViewState;
            MenuBondReq.MenuList = toolbargrid.Show();
            MenuBondReq.SetTrigger(pnlPhonReq);

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
                ViewState["REQUESTID"] = null;
                ViewState["ALLOWEDIT"] = "true";
                ViewState["CURRENTTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

            }
            BindData();

            ResetMenu();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDREFERENCENO", "FLDREQUESTDATE", "FLDREQUESTSTATUS" };
            string[] alCaptions = { "Request No", "Request Date", "Request Status" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentVesselPhoneCardRequestFilter;
            DataSet ds = PhoenixVesselAccountsPhoneCardRequisition.SearchPhoneCradRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , nvc != null ? nvc.Get("txtRefNo") : null
                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Phone Card Requisition", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuBondReq_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["REQUESTID"] = null;
                Filter.CurrentVesselPhoneCardRequestFilter = null;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void OrderForm_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsPhoneCardRequisitionGeneral.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("LINEITEM"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsPhoneCardRequisitionLineItem.aspx";
            }
            ifMoreInfo.Attributes["src"] = ViewState["CURRENTTAB"].ToString() + "?requestid=" + ViewState["REQUESTID"].ToString() + "&allowedit=" + ViewState["ALLOWEDIT"].ToString();
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
            string[] alColumns = { "FLDREFERENCENO", "FLDREQUESTDATE", "FLDREQUESTSTATUS" };
            string[] alCaptions = { "Request No", "Request Date", "Request Status" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = Filter.CurrentVesselPhoneCardRequestFilter;
            DataSet ds = PhoenixVesselAccountsPhoneCardRequisition.SearchPhoneCradRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , nvc != null ? nvc.Get("txtRefNo") : null
                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvPhonReq", "PhoneCard Requisition", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvPhonReq.DataSource = ds;
                gvPhonReq.DataBind();
                if (ViewState["REQUESTID"] == null)
                {
                    ViewState["REQUESTID"] = ds.Tables[0].Rows[0]["FLDREQUESTID"].ToString();
                    gvPhonReq.SelectedIndex = 0;
                    if (!ds.Tables[0].Rows[0]["FLDREQUESTSTATUS"].ToString().Contains("Req")) ViewState["ALLOWEDIT"] = "false";
                    ResetMenu();
                }

                if (ViewState["CURRENTTAB"] == null)
                {
                    ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsPhoneCardRequisitionGeneral.aspx";
                }

                ifMoreInfo.Attributes["src"] = ViewState["CURRENTTAB"].ToString() + "?requestid=" + ViewState["REQUESTID"] + "&allowedit=" + ViewState["ALLOWEDIT"];

            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvPhonReq);
                ifMoreInfo.Attributes["src"] = "../VesselAccounts/VesselAccountsPhoneCardRequisitionGeneral.aspx";
            }
            SetTabHighlight();
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

    protected void gvPhonReq_Sorting(object sender, GridViewSortEventArgs se)
    {
        try
        {
            ViewState["STORETYPEID"] = null;
            ViewState["SORTEXPRESSION"] = se.SortExpression;

            if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
                ViewState["SORTDIRECTION"] = 1;
            else
                ViewState["SORTDIRECTION"] = 0;
            ViewState["REQUESTID"] = null;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPhonReq_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        try
        {
            GridView _gridView = sender as GridView;
            gvPhonReq.SelectedIndex = se.NewSelectedIndex;
            string requestid = ((Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblRequestId")).Text;
            string reqstatus = ((Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblStatus")).Text;
            ViewState["REQUESTID"] = requestid;
            if (!reqstatus.Contains("Req")) ViewState["ALLOWEDIT"] = "false";
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPhonReq_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["SORTEXPRESSION"] != null)
                {
                    HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                    if (img != null)
                    {
                        if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                            img.Src = Session["images"] + "/arrowUp.png";
                        else
                            img.Src = Session["images"] + "/arrowDown.png";

                        img.Visible = true;
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ab = (ImageButton)e.Row.FindControl("cmdApprove");
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                if (db != null)
                {

                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    if (lblStatus.Text == "Requested")
                    {
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to cancel ?')");
                    }
                    if (!lblStatus.Text.Contains("Req")) db.Visible = false;
                }
                if (ab != null)
                {

                    ab.Visible = SessionUtil.CanAccess(this.ViewState, ab.CommandName);
                    ab.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to confirm ?')");
                    if (drv["FLDACTIVEYN"].ToString() == "0") ab.Visible = false;
                    if (!lblStatus.Text.Contains("Req")) ab.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvPhonReq_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = sender as GridView;
            int nCurrentRow = e.RowIndex;
            string requestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRequestId")).Text;
            string reqstatus = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStatus")).Text;
            ViewState["REQUESTID"] = requestid;
            if (!reqstatus.Contains("Req")) ViewState["ALLOWEDIT"] = "false";
            PhoenixVesselAccountsPhoneCardRequisition.ConfirmPhoneCradRequest(new Guid(requestid));
            ViewState["REQUESTID"] = null;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
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
            ViewState["REQUESTID"] = null;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvPhonReq.SelectedIndex = -1;
            gvPhonReq.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            ViewState["REQUESTID"] = null;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPageNavigator()
    {
        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

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
            return true;

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
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["REQUESTID"] = null;
            BindData();
            for (int i = 0; i < gvPhonReq.DataKeyNames.Length; i++)
            {
                if (gvPhonReq.DataKeyNames[i] == ViewState["REQUESTID"].ToString())
                {
                    gvPhonReq.SelectedIndex = i;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ResetMenu()
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("General", "GENERAL");
            if (ViewState["REQUESTID"] != null)
                toolbar.AddButton("Line Item", "LINEITEM");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbar.Show();
            MenuOrderForm.SelectedMenuIndex = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SetTabHighlight()
    {
        try
        {
            DataList dl = (DataList)MenuOrderForm.FindControl("dlstTabs");
            if (dl.Items.Count > 0)
            {
                if (ViewState["CURRENTTAB"].ToString().Trim().Contains("VesselAccountsPhoneCardRequisitionGeneral.aspx"))
                {
                    MenuOrderForm.SelectedMenuIndex = 0;
                }
                else if (ViewState["CURRENTTAB"].ToString().Trim().Contains("VesselAccountsPhoneCardRequisitionLineItem.aspx"))
                {
                    MenuOrderForm.SelectedMenuIndex = 1;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPhonReq_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "APPROVE")
            {

                GridView _gridView = sender as GridView;
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
                string requestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRequestId")).Text;
                string reqstatus = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStatus")).Text;
                ViewState["REQUESTID"] = requestid;
                if (!reqstatus.Contains("Req")) ViewState["ALLOWEDIT"] = "false";
                PhoenixVesselAccountsPhoneCardRequisition.ConfirmPhoneCradRequest(new Guid(requestid));
                ViewState["REQUESTID"] = null;
                BindData();
            }
            if (e.CommandName == "ORDERCANCEL")
            {

                GridView _gridView = sender as GridView;
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
                string requestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRequestId")).Text;
                string reqstatus = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStatus")).Text;
                ViewState["REQUESTID"] = requestid;
                if (reqstatus.Contains("Req"))
                {
                    ViewState["REQUESTID"] = null;
                    PhoenixVesselAccountsPhoneCardRequisition.CancelPhoneCradRequest(new Guid(requestid));
                }
                BindData();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
