using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class CommonBudgetAllocationHistory : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucFinancialYear.SelectedQuick = Request.QueryString["FinYear"].ToString();
            ucOwner.SelectedAddress = Request.QueryString["OwnerId"].ToString();
        }

        BindVesselAllocation();
    }

    private void BindVesselAllocation()
    {
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCommonBudgetGroupAllocation.BudgetVesselAllocationHistorySearch(
              General.GetNullableInteger(Request.QueryString["FinYear"].ToString())
             , General.GetNullableInteger(Request.QueryString["VesselId"].ToString())
            );

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVesselAllocation.DataSource = ds;
            gvVesselAllocation.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvVesselAllocation);

            gvVesselAllocation.SelectedIndex = -1;
            gvVesselAllocation.EditIndex = -1;
        }
    }

    protected void gvVesselAllocation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselAllocation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindVesselAllocation();
    }

    protected void gvVesselAllocation_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblOverwritten = (Label)e.Row.FindControl("lblOverwritten");

            if (lblOverwritten != null && lblOverwritten.Text.Trim().ToUpper().Equals("YES"))
                e.Row.ForeColor = System.Drawing.Color.Red;

        }
    }

    protected void gvVesselAllocation_RowEditing(object sender, GridViewEditEventArgs de)
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
