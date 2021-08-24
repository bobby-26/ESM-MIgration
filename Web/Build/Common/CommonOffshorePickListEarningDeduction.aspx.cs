using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;

public partial class CommonOffshorePickListEarningDeduction: PhoenixBasePage
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
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Search", "SEARCH");
            MenuStockItem.AccessRights = this.ViewState;
            MenuStockItem.MenuList = toolbarmain.Show();

            ucTitle.Text = Request.QueryString["t"];
            txtDate.Text = DateTime.Now.ToString();
            ViewState["UID"] = Guid.NewGuid();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToString().ToUpper().Equals("OFFSHORE"))
            {
                lblBOWCalculationDate.Visible = false;
                txtDate.Visible = false;
            }
        }
        BindData();
    }
    protected void MenuStockItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SEARCH"))
            {
                gvCrewSearch.EditIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
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

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixCommonVesselAccounts.SearchoffshoreTempVesselEarningDeduction(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(ViewState["UID"].ToString())
                   , (short?)General.GetNullableInteger(Request.QueryString["m"]), General.GetNullableInteger(Request.QueryString["y"]), null
                   , sortexpression, sortdirection
                   , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                   , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount
                   , General.GetNullableDateTime(txtDate.Text));

            if (dt.Rows.Count > 0)
            {
                txtCashOnboard.Text = dt.Rows[0]["FLDCASHONBOARD"].ToString();
                gvCrewSearch.DataSource = dt;
                gvCrewSearch.DataBind();
                if (Request.QueryString["t"].ToLower().Contains("all"))
                {
                    gvCrewSearch.Columns[5].Visible = false;
                }
                else
                {
                    gvCrewSearch.Columns[6].Visible = false;
                }
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToString().ToUpper().Equals("OFFSHORE"))
                {
                    gvCrewSearch.Columns[7].Visible = false;
                }
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

    private bool IsValidEarningDeduction(string date, string bankaccountid, DateTime? signofdate, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        DateTime resultsignofDate;
        string month = Request.QueryString["m"], year = Request.QueryString["y"];
        if (!General.GetNullableInteger(month).HasValue)
        {
            ucError.ErrorMessage = "Month is required.";
        }

        if (!General.GetNullableInteger(year).HasValue)
        {
            ucError.ErrorMessage = "Year is required.";
        }

        if (General.GetNullableInteger(month).HasValue && General.GetNullableInteger(year).HasValue
            && DateTime.Compare(General.GetNullableDateTime(year + "/" + month + "/01").Value, General.GetNullableDateTime(DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/01").Value) > 0)
        {
            ucError.ErrorMessage = "Month and Year should be earlier than current Month and Year";
        }

        if (!Request.QueryString["t"].ToLower().Contains("all") && !string.IsNullOrEmpty(month) && !string.IsNullOrEmpty(year))
        {
            if (!General.GetNullableDateTime(date).HasValue)
            {
                if (General.GetNullableInteger(amount).HasValue &&
                    !General.GetNullableInteger(amount).Value.Equals(0))
                    ucError.ErrorMessage = "Date is required.";
            }
            else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
            {
                ucError.ErrorMessage = "Date should be earlier than current date";
            }
            else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(Request.QueryString["y"] + "/" + Request.QueryString["m"] + "/1")) < 0)
            {
                ucError.ErrorMessage = "Date should be later than 1/" + Request.QueryString["m"] + "/" + Request.QueryString["y"];
            }
            else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(Request.QueryString["y"] + "/" + Request.QueryString["m"] + "/1").AddMonths(1).AddSeconds(-1)) > 0)
            {
                ucError.ErrorMessage = "Date should be earlier than " + string.Format("{0:dd/MM/yyyy}", DateTime.Parse(Request.QueryString["y"] + "/" + Request.QueryString["m"] + "/1").AddMonths(1).AddSeconds(-1));
            }
            else if (signofdate != null)
            {
                DateTime.TryParse(signofdate.ToString(), out resultsignofDate);
                DateTime.TryParse(date, out resultDate);
                if (resultDate > resultsignofDate)
                {
                    ucError.ErrorMessage = "Employee signoff date is updated and Entered date should be less than signoff";
                }
            }
            if (Request.QueryString["t"].ToLower().Contains("cash"))
            {
                if (!General.GetNullableDecimal(amount).HasValue)
                {
                    if (General.GetNullableDateTime(date).HasValue)
                        ucError.ErrorMessage = "Amount is required.";
                }
                else if (decimal.Parse(amount) < 0)
                {
                    ucError.ErrorMessage = "Amount is positive integer field";
                }
                else if (General.GetNullableDecimal(amount).HasValue && General.GetNullableDecimal(txtCashOnboard.Text).HasValue
                        && General.GetNullableDecimal(amount).Value > decimal.Parse(txtCashOnboard.Text) && !General.GetNullableDecimal(amount).Value.Equals(0))
                    ucError.ErrorMessage = "You dont have enough cash";

            }
        }
        else if (Request.QueryString["t"].ToLower().Contains("all"))
        {
            if (!General.GetNullableDecimal(amount).HasValue && General.GetNullableGuid(bankaccountid).HasValue)
            {
                ucError.ErrorMessage = "Amount is required.";
            }
            else if (decimal.Parse(amount) < 0)
            {
                ucError.ErrorMessage = "Amount is positive integer field";
            }
            if (!General.GetNullableGuid(bankaccountid).HasValue && General.GetNullableDecimal(amount).HasValue)
            {
                ucError.ErrorMessage = "Bank Account is required.";
            }
        }
        return (!ucError.IsError);
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
    protected void gvCrewSearch_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            if (_gridView.EditIndex > -1)
            {
                string amount = ((TextBox)((UserControlMaskNumber)_gridView.Rows[_gridView.EditIndex].FindControl("txtAmount")).FindControl("txtNumber")).Text;
               
                if (General.GetNullableDecimal(amount).HasValue && General.GetNullableDecimal(amount) > 0)
                {
                    string strbankacc = string.Empty;
                    strbankacc = ((UserControlEmployeeBankAccount)_gridView.Rows[_gridView.EditIndex].FindControl("ddlBankAccount")).SelectedBankAccount;
                    if(!General.GetNullableGuid(strbankacc).HasValue)
                    {
                        ucError.ErrorMessage = "Bank Account is required.";
                        ucError.Visible = true;
                        ((TextBox)((UserControlMaskNumber)_gridView.Rows[_gridView.EditIndex].FindControl("txtAmount")).FindControl("txtNumber")).Focus();
                        return;
                    }
                    _gridView.UpdateRow(_gridView.EditIndex, false);
                }
            }
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
            if (!PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToString().ToUpper().Equals("OFFSHORE"))
            {
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
            }
            string temp = Request.QueryString["t"].ToLower();
            string type = string.Empty;
            if (temp.Contains("cash")) type = "3";
            else if (temp.Contains("special")) type = "7";
            else if (temp.Contains("radio")) type = "5";
            else if (temp.Contains("allotment")) type = "4";
            if (nvc["lblAmount"] != string.Empty)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
                {
                    PhoenixCommonVesselAccounts.OffshoreVesselEarningDeductionInsert(PhoenixSecurityContext.CurrentSecurityContext.VesselID, byte.Parse(type), int.Parse(nvc["lblEmployeeId"])
                            , General.GetNullableGuid(nvc["lblBankAccountId"]), null, byte.Parse(Request.QueryString["m"]), int.Parse(Request.QueryString["y"]), int.Parse("-1")
                            , General.GetNullableDateTime(nvc["lblDate"]), decimal.Parse(nvc["lblAmount"]), Request.QueryString["t"], General.GetNullableGuid(ViewState["UID"].ToString()), int.Parse(nvc["lblsignonoffid"]));
                }
                else
                {
                    PhoenixCommonVesselAccounts.VesselEarningDeductionInsert(PhoenixSecurityContext.CurrentSecurityContext.VesselID, byte.Parse(type), int.Parse(nvc["lblEmployeeId"])
                            , General.GetNullableGuid(nvc["lblBankAccountId"]), null, byte.Parse(Request.QueryString["m"]), int.Parse(Request.QueryString["y"]), int.Parse("-1")
                            , General.GetNullableDateTime(nvc["lblDate"]), decimal.Parse(nvc["lblAmount"]), Request.QueryString["t"], General.GetNullableGuid(ViewState["UID"].ToString()), int.Parse(nvc["lblsignonoffid"]));
                }
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
    protected void CloseWindow(object sender, EventArgs e)
    {
        try
        {
            if (sender != null && ((UserControlConfirmMessage)sender).confirmboxvalue == 1)
            {
                NameValueCollection nvc = (NameValueCollection)ViewState["NVC"];
                if (nvc["lblAmount"] != string.Empty)
                {
                    string temp = Request.QueryString["t"].ToLower();
                    string type = string.Empty;
                    if (temp.Contains("cash")) type = "3";
                    else if (temp.Contains("special")) type = "7";
                    else if (temp.Contains("radio")) type = "5";
                    else if (temp.Contains("allotment")) type = "4";

                    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
                    {
                        PhoenixCommonVesselAccounts.OffshoreVesselEarningDeductionInsert(PhoenixSecurityContext.CurrentSecurityContext.VesselID, byte.Parse(type), int.Parse(nvc["lblEmployeeId"])
                                , General.GetNullableGuid(nvc["lblBankAccountId"]), null, byte.Parse(Request.QueryString["m"]), int.Parse(Request.QueryString["y"]), int.Parse("-1")
                                , General.GetNullableDateTime(nvc["lblDate"]), decimal.Parse(nvc["lblAmount"]), Request.QueryString["t"], General.GetNullableGuid(ViewState["UID"].ToString()), int.Parse(nvc["lblsignonoffid"]));
                    }
                    else
                    {
                        PhoenixCommonVesselAccounts.VesselEarningDeductionInsert(PhoenixSecurityContext.CurrentSecurityContext.VesselID, byte.Parse(type), int.Parse(nvc["lblEmployeeId"])
                                , General.GetNullableGuid(nvc["lblBankAccountId"]), null, byte.Parse(Request.QueryString["m"]), int.Parse(Request.QueryString["y"]), int.Parse("-1")
                                , General.GetNullableDateTime(nvc["lblDate"]), decimal.Parse(nvc["lblAmount"]), Request.QueryString["t"], General.GetNullableGuid(ViewState["UID"].ToString()), int.Parse(nvc["lblsignonoffid"]));
                    }
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
    protected void gvCrewSearch_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
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

}
