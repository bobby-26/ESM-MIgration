using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;

public partial class VesselAccountsCTMBOWNew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("General", "GENERAL");
            toolbarmain.AddButton("BOW", "BOW");
            toolbarmain.AddButton("CTM", "CTMCAL");
            toolbarmain.AddButton("Denomination", "DENOMINATOIN");
            toolbarmain.AddButton("List", "LIST");
            MenuCTMMain.AccessRights = this.ViewState;
            MenuCTMMain.MenuList = toolbarmain.Show();
            MenuCTMMain.SelectedMenuIndex = 1;
            if (!IsPostBack)
            {

                ViewState["CTMID"] = Request.QueryString["CTMID"];
                ViewState["ACTIVEYN"] = Request.QueryString["a"];
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
            DataTable dt = PhoenixVesselAccountsCTMNew.ListCaptainCashBOW(new Guid(ViewState["CTMID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                gvCTM.DataSource = dt;
                gvCTM.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvCTM);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    decimal r;
    protected void gvCTM_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            r = 0;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (drv["FLDBOWID"].ToString() == string.Empty) db.Visible = false;
            }
            r = r + decimal.Parse((drv["FLDAMOUNT"].ToString() != string.Empty ? drv["FLDAMOUNT"].ToString() : "0"));
            if (ViewState["ACTIVEYN"].ToString() != "1")
            {
                if (db != null) db.Visible = false;
                if (ed != null) ed.Visible = false;
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[5].Text = r.ToString();
        }
    }
    protected void MenuCTMMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("GENERAL") || ViewState["CTMID"] == null)
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMGeneralNew.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("BOW"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMBOWNew.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("DENOMINATOIN"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMDenominationNew.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("CTMCAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCTMCalculationNew.aspx";
            }
            if (dce.CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsCTMNew.aspx", false);
            }
            else
                Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?CTMID=" + ViewState["CTMID"] + "&a=" + ViewState["ACTIVEYN"], false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCTM_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvCTM_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = e.NewEditIndex;
        BindData();
    }
    protected void gvCTM_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            Guid? id = General.GetNullableGuid(_gridView.DataKeys[nCurrentRow].Value.ToString()); string signondate = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblsignondate")).Text;
            string employeeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeId")).Text;
            string date = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDate")).Text;
            if (!IsValidBOW(date, signondate))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixVesselAccountsCTMNew.InsertCaptainCashBOWNEW(new Guid(ViewState["CTMID"].ToString()), int.Parse(employeeid), DateTime.Parse(date), id);
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCTM_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            Guid? id = General.GetNullableGuid(_gridView.DataKeys[nCurrentRow].Value.ToString());
            PhoenixVesselAccountsCTMNew.DeleteCaptainCashBOWNew(id.Value);
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidBOW(string date, string signon)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Relief Due Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(signon)) < 0)
        {
            ucError.ErrorMessage = "Relief Due Date should be later than sign on date.";
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
}
