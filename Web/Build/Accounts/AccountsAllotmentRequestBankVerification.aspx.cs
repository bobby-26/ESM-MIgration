using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;

public partial class AccountsAllotmentRequestBankVerification : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["ALLOTMENTID"] = "";
                ViewState["VESSELID"] = "";
                ViewState["CHECKTYPE"] = "2";
                ViewState["EMPLOYEEID"] = "";
                ViewState["REMARKSID"] = "";

                if (Request.QueryString["ALLOTMENTID"] != null)
                    ViewState["ALLOTMENTID"] = Request.QueryString["ALLOTMENTID"].ToString();
                if (Request.QueryString["CHECKTYPE"] != null)
                    ViewState["CHECKTYPE"] = Request.QueryString["CHECKTYPE"].ToString();
            }
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
        DataTable dt = new DataTable();
        dt = PhoenixAccountsAllotmentRequestSystemChecking.CrewBankDetailsAndRemarksSearch(General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();
            ViewState["EMPLOYEEID"] = dt.Rows[0]["FLDEMPLOYEEID"].ToString();

            gvCrewBankAccount.DataSource = dt;
            gvCrewBankAccount.DataBind();
        }
        else
            ShowNoRecordsFound(dt, gvCrewBankAccount);

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void gvCrewBankAccount_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string dtkey = "";
                string remarksid = "";
                string remarks = "";
                string allotmenttype = "";

                dtkey = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text;
                remarksid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRemarksId")).Text;
                allotmenttype = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAllotmentType")).Text;
                remarks = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBankVerificationRemarks")).Text;

                if (General.GetNullableString(remarks) == null)
                {
                    ucError.ErrorMessage = "Remarks Required";
                    ucError.Visible = true;
                    return;
                }
                if (General.GetNullableGuid(remarksid) == null)
                {
                    PhoenixAccountsAllotmentRequestSystemChecking.AllotmentCheckingRemarksInsert(
                        General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString())
                        , new Guid(dtkey)
                        , General.GetNullableInteger(ViewState["CHECKTYPE"].ToString())
                        , General.GetNullableString(remarks));

                }
                else
                {
                    PhoenixAccountsAllotmentRequestSystemChecking.AllotmentCheckingRemarksUpdate(
                         General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString())
                         , General.GetNullableGuid(remarksid)
                         , General.GetNullableInteger(ViewState["CHECKTYPE"].ToString())
                         , General.GetNullableString(remarks));
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
                _gridView.EditIndex = -1;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuAllotment_TabStripCommand(object sender, EventArgs e)
    {
    }
    protected void gvCrewBankAccount_Sorting(object sender, GridViewSortEventArgs se)
    {
    }
    protected void gvCrewBankAccount_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
    }
    protected void gvCrewBankAccount_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = de.NewEditIndex;
            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewBankAccount_RowCancellingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvCrewBankAccount_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv=(DataRowView)e.Row.DataItem;
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
            Label lblActiveYN = (Label)e.Row.FindControl("lblActiveYN");

            if (lblActiveYN != null && ed != null)
            {
                if (lblActiveYN.Text == "0")
                    ed.Visible = false;
                else
                    ed.Visible = true;
            }
            CheckBox chkBankId = (CheckBox)e.Row.FindControl("chkBankId");

            if (chkBankId != null)
            {
                if (drv["FLDCHKBANK"].ToString() == "1")
                    chkBankId.Checked = true;
                else
                    chkBankId.Checked = false;
            }
        }
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

    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
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
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvCrewBankAccount.SelectedIndex = -1;
        gvCrewBankAccount.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void chkBankId_OnCheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow gvrow = (GridViewRow)chk.Parent.Parent;

        string lblCrewBankAccountId = ((Label)gvrow.FindControl("lblCrewBankAccountId")).Text;

        if (chk.Checked == true)
        {
            PhoenixAccountsAllotmentRequestSystemChecking.CrewBankDetailsUpdate(new Guid(ViewState["ALLOTMENTID"].ToString()),
                                                                                new Guid(lblCrewBankAccountId),
                                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
        }
        BindData();
    }
}
