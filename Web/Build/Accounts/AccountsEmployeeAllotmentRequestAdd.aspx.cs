using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
public partial class AccountsEmployeeAllotmentRequestAdd : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvCrewSearch.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvCrewSearch.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Search", "SEARCH");
            //MenuStockItem.AccessRights = this.ViewState;
            //MenuStockItem.MenuList = toolbarmain.Show();

            ucTitle.Text = "Add Request";

            ViewState["EMPLOYEEID"] = null;
            ViewState["VESSELID"] = null;
            ViewState["MONTH"] = null;
            ViewState["YEAR"] = null;

            ViewState["EMPLOYEEID"] = Request.QueryString["EMPLOYEEID"].ToString();
            ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            ViewState["MONTH"] = Request.QueryString["MONTH"].ToString();
            ViewState["YEAR"] = Request.QueryString["YEAR"].ToString();

            txtDate.Text = DateTime.Now.ToString();
            ViewState["UID"] = Guid.NewGuid();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            BindAllotmentType();

        }
        BindData();

    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixAccountsEmployeeAllotmentRequest.EmployeeAllotmentRequestAddSearch(int.Parse(ViewState["VESSELID"].ToString())
                   , (short?)General.GetNullableInteger(ViewState["MONTH"].ToString())
                   , General.GetNullableInteger(ViewState["YEAR"].ToString())
                   , General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString())
                   , sortexpression, sortdirection
                   , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                   , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            if (dt.Rows.Count > 0)
            {
                gvCrewSearch.DataSource = dt;
                gvCrewSearch.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvCrewSearch);
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
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
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

    protected void gvCrewSearch_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
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
            UserControlMaskNumber amt = (UserControlMaskNumber)e.Row.FindControl("txtAmount");
            if (amt == null)
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCrewSearch, "Edit$" + e.Row.RowIndex.ToString(), false);
            SetKeyDownScroll(sender, e);
        }
    }
    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataTable)_gridView.DataSource).Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }
    }

    protected void gvCrewSearch_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {

            GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
            ((TextBox)((UserControlMaskNumber)_gridView.Rows[de.NewEditIndex].FindControl("txtAmount")).FindControl("txtNumber")).Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvCrewSearch_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string Script = "";
            decimal d;
            decimal balance;
            NameValueCollection nvc = new NameValueCollection();
            string id = string.Empty;
            string amount = string.Empty;
            string date = string.Empty;
            string strbankacc = string.Empty;

            string signoffdate = "";
            string signonoffid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblsignonoffid")).Text;
            Label lbsignofdate = (Label)_gridView.Rows[nCurrentRow].FindControl("lblsignoffDate");
            if (lbsignofdate != null && !String.IsNullOrEmpty(lbsignofdate.Text))
            {
                signoffdate = lbsignofdate.Text;
            }

            UserControlDate ucdate = null;
            UserControlEmployeeBankAccount bankacc = null;

            ucdate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDate"));
            bankacc = ((UserControlEmployeeBankAccount)_gridView.Rows[nCurrentRow].FindControl("ddlBankAccount"));
            string tempamt = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtAmount")).Text;
            decimal.TryParse(tempamt, out d);
            if (d > 0)
            {
                id += ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeId")).Text + ",";
                amount += d.ToString() + ",";

                date += ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDate")).Text + ",";

                strbankacc += bankacc.SelectedBankAccount + ",";
            }
            nvc.Add("lblEmployeeId", id.TrimEnd(','));
            nvc.Add("lblAmount", amount.TrimEnd(','));
            nvc.Add("lblDate", date.TrimEnd(','));
            nvc.Add("lblBankAccountId", strbankacc.TrimEnd(','));
            nvc.Add("lblsignonoffid", signonoffid.TrimEnd(','));


            if (!IsValidEarningDeduction(ucdate.Text, bankacc.SelectedBankAccount, General.GetNullableDateTime(signoffdate), tempamt))
            {
                ucError.Visible = true;
                return;
            }

            ViewState["NVC"] = nvc;
            decimal.TryParse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBalance")).Text, out balance);
            if (d != 0 && d > balance)
            {
                ucConfirm.HeaderMessage = "Please Confirm";
                ucConfirm.Text = "Insufficient Balance of Wage <br/> Do you want to Proceed ?";
                ucConfirm.Visible = true;
                ucConfirm.CancelText = "No";
                ucConfirm.OKText = "Yes";
                ((Button)ucConfirm.FindControl("cmdNo")).Focus();
                return;
            }

            string temp = ddlAllotmentType.SelectedItem.Text;
            string type = string.Empty;
            type = ddlAllotmentType.SelectedValue;
            if (nvc["lblAmount"] != string.Empty)
            {

                PhoenixAccountsEmployeeAllotmentRequest.VesselEarningDeductionInsert(PhoenixSecurityContext.CurrentSecurityContext.VesselID, byte.Parse(type), int.Parse(nvc["lblEmployeeId"])
                            , General.GetNullableGuid(nvc["lblBankAccountId"]), null, byte.Parse(Request.QueryString["MONTH"]), int.Parse(Request.QueryString["YEAR"]), int.Parse("-1")
                            , General.GetNullableDateTime(nvc["lblDate"]), decimal.Parse(nvc["lblAmount"]), temp, General.GetNullableGuid(ViewState["UID"].ToString()), int.Parse(nvc["lblsignonoffid"]));

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo','keepopen');";
                Script += "</script>" + "\n";
                Filter.CurrentPickListSelection = nvc;
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidEarningDeduction(string date, string bankaccountid, DateTime? signofdate, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        DateTime resultsignofDate;
        string month = ViewState["MONTH"].ToString(), year = ViewState["YEAR"].ToString();
        if (signofdate != null)
        {
            DateTime.TryParse(signofdate.ToString(), out resultsignofDate);
            DateTime.TryParse(date, out resultDate);
            if (resultDate > resultsignofDate)
            {
                ucError.ErrorMessage = "Employee signoff date is updated and Entered date should be less than signoff";
            }
        }
        if (!General.GetNullableInteger(amount).HasValue && General.GetNullableGuid(bankaccountid).HasValue)
        {
            ucError.ErrorMessage = "Amount is required.";
        }
        else if (decimal.Parse(amount) < 0)
        {
            ucError.ErrorMessage = "Amount is positive integer field";
        }
        if (!General.GetNullableGuid(bankaccountid).HasValue && General.GetNullableInteger(amount).HasValue)
        {
            ucError.ErrorMessage = "Bank Account is required.";
        }
        return (!ucError.IsError);
    }
    protected void gvCrewSearch_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
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
    protected void CloseWindow(object sender, EventArgs e)
    {
        try
        {
            if (sender != null && ((UserControlConfirmMessage)sender).confirmboxvalue == 1)
            {
                NameValueCollection nvc = (NameValueCollection)ViewState["NVC"];
                if (nvc["lblAmount"] != string.Empty)
                {
                    string temp = ddlAllotmentType.SelectedItem.Text;
                    string type = string.Empty;
                    type = ddlAllotmentType.SelectedValue;

                    PhoenixAccountsEmployeeAllotmentRequest.VesselEarningDeductionInsert(int.Parse(ViewState["VESSELID"].ToString()), byte.Parse(type), int.Parse(ViewState["EMPLOYEEID"].ToString())
                                , General.GetNullableGuid(nvc["lblBankAccountId"]), null, byte.Parse(Request.QueryString["MONTH"]), int.Parse(Request.QueryString["YEAR"]), int.Parse("-1")
                                                                , General.GetNullableDateTime(nvc["lblDate"]), decimal.Parse(nvc["lblAmount"]), temp, General.GetNullableGuid(ViewState["UID"].ToString()), int.Parse(nvc["lblsignonoffid"]));

                    string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList('codehelp1','ifMoreInfo','keepopen');";
                    Script += "</script>" + "\n";
                    Filter.CurrentPickListSelection = nvc;
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                }
                gvCrewSearch.EditIndex = -1;
                BindData();
            }
            if (sender != null && gvCrewSearch.EditIndex != -1)
            {
                ((TextBox)((UserControlMaskNumber)gvCrewSearch.Rows[gvCrewSearch.EditIndex].FindControl("txtAmount")).FindControl("txtNumber")).Focus();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    private void BindAllotmentType()
    {
        ddlAllotmentType.Items.Clear();
        ListItem li = new ListItem("--Select--", "DUMMY");
        ddlAllotmentType.Items.Add(li);
        ddlAllotmentType.AppendDataBoundItems = true;
        DataTable dt = PhoenixAccountsEmployeeAllotmentRequest.AllotmentTypeList();
        ddlAllotmentType.DataSource = dt;
        ddlAllotmentType.DataBind();
    }
}
