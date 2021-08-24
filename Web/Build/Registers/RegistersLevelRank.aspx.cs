using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersLevelRank : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);            
            if (!IsPostBack)
            {
                
            }
            BindData();
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void OwnerMapping_TabStripCommand(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
    //        if (dce.CommandName.ToUpper().Equals("OWNERMAPPING"))
    //        {
    //            Response.Redirect("RegistersOwnerMapping.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"].ToString());
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    private void BindData()
    {
        DataTable dt = PhoenixRegistersLevelRankList.LevelRankList(General.GetNullableInteger(ucHard.SelectedHard));
        if (dt.Rows.Count > 0)
        {
            //ucTitle.Text = "LeaveWages and BTB [ " + dt.Rows[0]["FLDNAME"].ToString() + " ]";
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
            //Guid dtkey = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());
            string level = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblLevelId")).Text;
            string purpose = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblPurposeId")).Text;
            CheckBoxList cblList = (CheckBoxList)_gridView.Rows[nCurrentRow].FindControl("cblRank");
            string ranklist = string.Empty;
            foreach (ListItem item in cblList.Items)
            {
                if (item.Selected) ranklist = ranklist + item.Value.ToString() + ",";
            }
            ranklist = ranklist.TrimEnd(',');
            if (!IsValidRankMapping(level, ranklist,purpose))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersLevelRankList.UpdateLevelRankList(ranklist,int.Parse(purpose),int.Parse(level));
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
    private bool IsValidRankMapping(string level, string ranklist,string purpose )
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (ranklist.Equals(""))
            ucError.ErrorMessage = "Rank is required.";
        if (!General.GetNullableInteger(level).HasValue)
            ucError.ErrorMessage = "Level is required.";

        if (!General.GetNullableInteger(purpose).HasValue)
            ucError.ErrorMessage = "Purpose is required.";

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
