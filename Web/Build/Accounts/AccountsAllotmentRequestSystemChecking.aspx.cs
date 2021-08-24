using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using SouthNests.Phoenix.Accounts;

public partial class AccountsAllotmentRequestSystemChecking : PhoenixBasePage
{
    NameValueCollection nvc = new NameValueCollection();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Request", "REQUEST");
            toolbar.AddButton("System Checking", "SYSTEMCHECKING");
            MenuRequest.AccessRights = this.ViewState;
            MenuRequest.MenuList = toolbar.Show();
            MenuRequest.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                ViewState["CURRENTINDEX"] = 1;
                if (Filter.CurrentAllotmentRequestFilter != null)
                nvc = Filter.CurrentAllotmentRequestFilter;
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
        try
        {
            DataTable dt = new DataTable();

            dt = PhoenixAccountsAllotmentRequestSystemChecking.AllotmentRequestSystemCheck(PhoenixSecurityContext.CurrentSecurityContext.UserCode
              , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDateFrom") : string.Empty)
              , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDateTo") : string.Empty)
              , General.GetNullableString(nvc != null ? nvc.Get("txtFileNo") : string.Empty)
              , General.GetNullableInteger(nvc != null ? nvc.Get("ucRank") : string.Empty)
              , General.GetNullableInteger(nvc != null ? nvc.Get("ddlVessel") : string.Empty)
              , General.GetNullableInteger(nvc != null ? nvc.Get("ddlMonth") : string.Empty)
              , General.GetNullableInteger(nvc != null ? nvc.Get("ddlYear") : string.Empty)
              , General.GetNullableInteger(nvc != null ? nvc.Get("ucAllotmentType") : string.Empty)
              , General.GetNullableInteger(nvc != null ? nvc.Get("ucRequestStatus") : string.Empty));

            if (dt.Rows.Count > 0)
            {
                gvAllotment.DataSource = dt;
                gvAllotment.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvAllotment);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAllotment_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAllotment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        try
        {
            if (e.CommandName.ToUpper() == "SELECT")
            {
                LinkButton lnkRequestNo = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkRequestNo");
                Label lblAllotmentId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAllotmentId");
                Label lblAllotmentType = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAllotmentType");

                Response.Redirect("AccountsAllotmentRequestDetails.aspx?ALLOTMENTID=" + lblAllotmentId.Text + "&ALLOTMENTTYPE=" + lblAllotmentType.Text, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRequest_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("REQUEST"))
            {
                Response.Redirect("AccountsAllotmentRequest.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAllotment_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {

    }
    protected void gvAllotment_RowCreated(object sender, GridViewRowEventArgs e)
    {
       
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
        gv.Rows[0].Attributes["onclick"] = "";
    }

}
