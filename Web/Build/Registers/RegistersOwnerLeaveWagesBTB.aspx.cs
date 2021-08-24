using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersOwnerLeaveWagesBTB : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);            
            if (!IsPostBack)
            {
                
                ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
                
            }
            BindData();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Owner/Mapping", "OWNERMAPPING");
            toolbar.AddButton("LeaveWages and BTB", "LEAVEWAGESBTB");
            MenuOwnerMapping.AccessRights = this.ViewState;
            MenuOwnerMapping.MenuList = toolbar.Show();
            MenuOwnerMapping.SelectedMenuIndex = 1;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void OwnerMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("OWNERMAPPING"))
            {
                Response.Redirect("RegistersOwnerMapping.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"].ToString());
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
        DataTable dt = PhoenixRegistersOwnerLeaveWagesBTB.ListOwnerLeaveWagesBTB(int.Parse(ViewState["ADDRESSCODE"].ToString()));
        if (dt.Rows.Count > 0)
        {
           
            gvLVB.DataSource = dt;
            gvLVB.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvLVB);
        }
    }


    protected void gvLVB_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }


    protected void gvLVB_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();       
    }
    protected void gvLVB_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            Guid dtkey = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());
            string description = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescription")).Text;
            CheckBoxList cblList = (CheckBoxList)_gridView.Rows[nCurrentRow].FindControl("cblRank");
            string roundoff = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtRoundOff")).Text;
            string ranklist = string.Empty;
            foreach (ListItem item in cblList.Items)
            {
                if (item.Selected) ranklist = ranklist + item.Value.ToString() + ",";
            }
            ranklist = ranklist.TrimEnd(',');
            if (!IsValidRankMapping(description, ranklist, roundoff))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersOwnerLeaveWagesBTB.UpdateOwnerLeaveWagesBTB(description, ranklist, dtkey, byte.Parse(roundoff));
            _gridView.EditIndex = -1;
            BindData();            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
    protected void gvLVB_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            CheckBoxList cblList = (CheckBoxList)e.Row.FindControl("cblRank");
            if (cblList != null)
            {
                foreach (ListItem item in cblList.Items)
                {
                    item.Selected = false;
                    if (!string.IsNullOrEmpty(drv["FLDRANKLIST"].ToString()) && ("," + drv["FLDRANKLIST"].ToString() + ",").Contains("," + item.Value.ToString() + ","))
                        item.Selected = true;
                }
            }
        }      
    }
    private bool IsValidRankMapping(string description, string ranklist, string roundoff)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (description.Equals(""))
            ucError.ErrorMessage = "Description is required.";
        if (ranklist.Equals(""))
            ucError.ErrorMessage = "Group Rank is required.";
        if (!General.GetNullableInteger(roundoff).HasValue)
            ucError.ErrorMessage = "Round Off is required.";

        return (!ucError.IsError);
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
