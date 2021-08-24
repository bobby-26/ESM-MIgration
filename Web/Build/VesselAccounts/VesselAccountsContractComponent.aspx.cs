using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;


public partial class VesselAccountsContractComponent :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
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

    private void BindData()
    {
        try
        {
            DataTable dt = PhoenixVesselAccountsEmployee.ListCrewContractComponent(new Guid(Request.QueryString["cid"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dt.Rows.Count > 0)
            {
                gvCC.DataSource = dt;
                gvCC.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvCC);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCC_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                //if (drv["FLDCOMPONENTTYPE"].ToString() == "1") eb.Visible = false;
            }
        }
    }
    protected void gvCC_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvCC_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = de.NewEditIndex;        
        BindData();        
    }

    protected void gvCC_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string name = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtComponentName")).Text;
            string amount = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtAmount")).Text;
            Guid id = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());
            if (!IsValidComponent(name,amount))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixVesselAccountsEmployee.UpdateCrewContractComponentAmount(new Guid(Request.QueryString["cid"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID, id
                , name, decimal.Parse(amount));
            _gridView.EditIndex = -1;
            BindData();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidComponent(string name, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Component Name is required.";

        if (!General.GetNullableDecimal(amount).HasValue)
            ucError.ErrorMessage = "Amount is required.";      

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
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
}
