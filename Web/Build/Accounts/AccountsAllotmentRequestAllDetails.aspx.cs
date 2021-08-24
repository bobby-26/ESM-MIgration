using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using SouthNests.Phoenix.Accounts;

public partial class AccountsAllotmentRequestAllDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("All", "ALL");
            toolbar.AddButton("Details", "DETAILS");
            MenuRequest.AccessRights = this.ViewState;
            MenuRequest.MenuList = toolbar.Show();
            MenuRequest.SelectedMenuIndex = 0;
            MenuRequest.Visible = true;

            if (!IsPostBack)
            {
                if (Request.QueryString["fileNo"] != null)
                    ViewState["FileNo"] = Request.QueryString["fileNo"].ToString();
                if (Request.QueryString["ALLOTMENTID"] != null)
                    ViewState["ALLOTMENTID"] = Request.QueryString["ALLOTMENTID"].ToString();

                BindData();
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
            if (dce.CommandName.ToUpper().Equals("DETAILS"))
            {
                Response.Redirect("../Accounts/AccountsAllotmentRequesPendingReimbRecoveries.aspx?allotmentid=" + ViewState["ALLOTMENTID"].ToString()+"&fileNo="+ViewState["FileNo"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //BindData();
        //SetPageNavigator();
    }

    protected void gvAllotment_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
    }

    private void BindData()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixAccountsAllotmentRequest.AllotmentRequestSearchAll(PhoenixSecurityContext.CurrentSecurityContext.UserCode
               , General.GetNullableString(ViewState["FileNo"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                gvAllotment.DataSource = ds;
                gvAllotment.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvAllotment);
            }
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
        gv.Rows[0].Attributes["onclick"] = "";
    }

}
