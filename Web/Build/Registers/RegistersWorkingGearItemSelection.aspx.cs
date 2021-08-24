using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;


public partial class RegistersWorkingGearItemSelection : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvWorkingGearItem.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvWorkingGearItem.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

	protected void Page_Load(object sender, EventArgs e)
	{
        SessionUtil.PageAccessRights(this.ViewState);
		PhoenixToolbar toolbarmain = new PhoenixToolbar();
		toolbarmain.AddButton("Search", "SEARCH");
        MenuStockItem.AccessRights = this.ViewState;
		MenuStockItem.MenuList = toolbarmain.Show();
		MenuStockItem.SetTrigger(pnlStockItem);      

		if (!IsPostBack)
		{           
			ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
            
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
                ViewState["PAGENUMBER"] = 1;
				BindData();
				SetPageNavigator();
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
			int iTotalPageCount = 10;
			
			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;
			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            int? setid = General.GetNullableInteger(Filter.CurrentWorkingGearSetSelection);
            DataSet ds = PhoenixRegistersWorkingGearSetDetails.WorkingGearSetItemAddSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,setid,txtItemSearch.Text,
                                         General.GetNullableInteger(ucWorkingGearType.SelectedGearType), General.GetNullableDecimal(txtPricefrom.Text), General.GetNullableDecimal(txtPriceto.Text),
                                         sortexpression,
                                         sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                         General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
			{
                gvWorkingGearItem.DataSource = ds.Tables[0];
				gvWorkingGearItem.DataBind();               
			}
			else
			{
                ShowNoRecordsFound(ds.Tables[0], gvWorkingGearItem);
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

    protected void gvWorkingGearItem_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            UserControlMaskNumber qty = ((UserControlMaskNumber)e.Row.FindControl("txtQuantity"));
            if (qty == null && SessionUtil.CanAccess(this.ViewState, ed.CommandName))
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvWorkingGearItem, "Edit$" + e.Row.RowIndex.ToString(), false);
            SetKeyDownScroll(sender, e);
        }
	}

    protected void gvWorkingGearItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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

    protected void gvWorkingGearItem_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
            ((TextBox)((UserControlMaskNumber)_gridView.Rows[de.NewEditIndex].FindControl("txtQuantity")).FindControl("txtNumber")).Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkingGearItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string quantity = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuantity")).Text;
            string WorkingGearItem = _gridView.DataKeys[nCurrentRow].Value.ToString();
            if (IsValidEntry(quantity) && Filter.CurrentWorkingGearSetSelection != null)
            {

                int setid = int.Parse(Filter.CurrentWorkingGearSetSelection);
                PhoenixRegistersWorkingGearSetDetails.InsertWorkingGearSetItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode, setid, new Guid(WorkingGearItem), int.Parse(quantity));

                string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo','keepopen');";
                Script += "</script>" + "\n";
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
		gvWorkingGearItem.SelectedIndex = -1;
		gvWorkingGearItem.EditIndex = -1;
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

    protected void gvWorkingGearItem_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
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

    private bool IsValidEntry(string quantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableDecimal(quantity).HasValue)
        {
            ucError.ErrorMessage = "Quantity is required.";
        }
        else if (decimal.Parse(quantity) <= 0)
        {
            ucError.ErrorMessage = "Quantity should not be zero or negative";
        }
        return (!ucError.IsError);
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
