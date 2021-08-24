using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;


public partial class AccountsAllotmentRequesPendingReimbRecoveries : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvRem.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvRem.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
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
                ViewState["ALLOTMENTID"] = "";
                ViewState["VESSELID"] = "";
                ViewState["CHECKTYPE"] = "3";
                ViewState["EMPLOYEEID"] = "";
                ViewState["REMARKSID"] = "";

                if (Request.QueryString["ALLOTMENTID"] != null)
                    ViewState["ALLOTMENTID"] = Request.QueryString["ALLOTMENTID"].ToString();

                if (Request.QueryString["CHECKTYPE"] != null)
                    ViewState["CHECKTYPE"] = Request.QueryString["CHECKTYPE"].ToString();

                if (Request.QueryString["fileNo"] != null)
                    ViewState["fileNo"] = Request.QueryString["fileNo"].ToString();

                PhoenixToolbar toolBar = new PhoenixToolbar();
                toolBar.AddButton("Allotment Request", "REQUEST");
                toolBar.AddButton("Details", "DETAILS");                
                MenuAllotment.AccessRights = this.ViewState;
                MenuAllotment.MenuList = toolBar.Show();
                MenuAllotment.SelectedMenuIndex = 1;

                if (Request.QueryString["fileNo"] != null)
                    MenuAllotment.Visible = true;
                else
                    MenuAllotment.Visible = false;
            }
            BindData();
            SetRowSelection();
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
        dt = PhoenixAccountsAllotmentRequestSystemChecking.CrewPendingReimRecAndRemarksSearch(General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();
            ViewState["EMPLOYEEID"] = dt.Rows[0]["FLDEMPLOYEEID"].ToString();

            gvRem.DataSource = dt;
            gvRem.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvRem);
        }
    }
    protected void gvRem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = int.Parse(e.CommandArgument.ToString());
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {

                string dtkey = "";
                string remarksid = "";
                string remarks = "";
                string allotmenttype = "";

                dtkey = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text;
                remarksid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRemarksId")).Text;
                allotmenttype = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAllotmentType")).Text;
                remarks = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarks")).Text;

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
            if (e.CommandName.ToUpper().Equals("PAYMENT"))
            {
                Label lblEarningDeduction = (Label)_gridView.Rows[nCurrentRow].FindControl("lblEarningDeduction");
                Label lblReimbursementId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblReimbursementId");

                //Response.Redirect("../Crew/" + (",1,-1,3,-3,".Contains("," + lblEarningDeduction.Text + ",") ? "CrewReimbursementPayment" : "CrewReimbursementDeduction") + ".aspx?rembid=" + lblReimbursementId.Text +
                //    "&showBack=1&ALLOTMENTID=" + ViewState["ALLOTMENTID"].ToString() + "&CHECKTYPE" + ViewState["CHECKTYPE"].ToString() + "&fileNo=" + ViewState["fileNo"].ToString());

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            ImageButton pb = (ImageButton)e.Row.FindControl("cmdPayment");
            if (eb != null)
            {
                if (General.GetNullableGuid(drv["FLDPAYMENTID"].ToString()) == null)
                    eb.Visible = false;

            }
            if (pb != null)
            {
                if (drv["FLDAPPROVEDYN"].ToString() == "1")
                {
                    pb.Visible = true;
                    pb.Attributes.Add("onclick", "parent.Openpopup('payment', '', '../Crew/" + (",1,-1,3,-3,".Contains("," + drv["FLDEARNINGDEDUCTION"].ToString() + ",") ? "CrewReimbursementPayment" : "CrewReimbursementDeduction") + ".aspx?rembid=" + drv["FLDREIMBURSEMENTID"] + "');return false;");
                }
                else
                    pb.Visible = false;
            }

        }
    }
    protected void gvRem_RowCancellingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvRem_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.NewEditIndex;
            _gridView.EditIndex = nCurrentRow;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuAllotment_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("REQUEST"))
            {
                Response.Redirect("../Accounts/AccountsAllotmentRequestAllDetails.aspx?allotmentid=" + ViewState["ALLOTMENTID"].ToString() + "&fileNo=" + ViewState["fileNo"].ToString());
            }
            if (dce.CommandName.ToUpper().Equals("PAYMENT"))
            {
                Response.Redirect("../Crew/" + (",1,-1,3,-3,".Contains("," + ViewState["EarningDeduction"].ToString() + ",") ? "CrewReimbursementPayment" : "CrewReimbursementDeduction") + ".aspx?rembid=" + ViewState["ReimbursementId"].ToString() +
                            "&showBack=1&ALLOTMENTID=" + ViewState["ALLOTMENTID"].ToString() + "&CHECKTYPE" + ViewState["CHECKTYPE"].ToString() + "&fileNo=" + ViewState["fileNo"].ToString());

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected string GetName(string val)
    {
        string result = string.Empty;
        if (val == "1")
            result = "Reimbursement(B.O.C)";
        else if (val == "2")
            result = "Reimbursement(Monthly / OneTime)";
        else if (val == "3")
            result = "Reimbursement(E.O.C)";
        else if (val == "-1")
            result = "Recovery(B.O.C)";
        else if (val == "-2")
            result = "Recovery(Monthly / OneTime)";
        else if (val == "-3")
            result = "Recovery(E.O.C)";
        return result;
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

    protected void gvRem_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        GridView _gridView = sender as GridView;
        _gridView.SelectedIndex = se.NewSelectedIndex;

        Label lblEarningDeduction = (Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblEarningDeduction");
        Label lblReimbursementId = (Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblReimbursementId");

        Response.Redirect("../Crew/" + (",1,-1,3,-3,".Contains("," + lblEarningDeduction.Text + ",") ? "CrewReimbursementPayment" : "CrewReimbursementDeduction") + ".aspx?rembid=" + lblReimbursementId.Text +
                    "&showBack=1&ALLOTMENTID=" + ViewState["ALLOTMENTID"].ToString() + "&CHECKTYPE" + ViewState["CHECKTYPE"].ToString() + "&fileNo=" + ViewState["fileNo"].ToString());

    }
    private void SetRowSelection()
    {
        gvRem.SelectedIndex = 0;
        Label lblEarningDeduction = (Label)gvRem.Rows[gvRem.SelectedIndex].FindControl("lblEarningDeduction");
        Label lblReimbursementId = (Label)gvRem.Rows[gvRem.SelectedIndex].FindControl("lblReimbursementId");
        if (lblEarningDeduction != null)
            ViewState["EarningDeduction"] = lblEarningDeduction.Text.ToString();
        if (lblReimbursementId != null)
            ViewState["ReimbursementId"] = lblReimbursementId.Text.ToString();
    }
}
