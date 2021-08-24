using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaHoliday : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../PreSea/PreSeaHoliday.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbar.AddImageLink("javascript:CallPrint('gvHoliday')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../PreSea/PreSeaHoliday.aspx", "Copy Holiday to Next Year", "copy.png", "COPY");
            toolbar.AddImageButton("../PreSea/PreSeaHoliday.aspx", "Find", "search.png", "FIND");
			MenuRegistersHoliday.AccessRights = this.ViewState;
			MenuRegistersHoliday.MenuList = toolbar.Show();

			if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
                BindHolidayYear();
			}
			BindData();
			SetPageNavigator();
            
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
    protected void CopyHolidayList()
    {
        if (ddlYear.SelectedValue != "Dummy")
        {
            PhoenixPreSeaHoliday.InsertHolidayToNextYear(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ddlYear.SelectedValue.ToString()));
            ucStatus.Text = "Holiday List Copied";
        }
        else
        {
            ucError.ErrorMessage = "Year is not selected.Please Select the Year from which holiday to be copied";
            ucError.Visible = true;
        }
       
    }


	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		DataSet ds = new DataSet();
		string[] alColumns = { "FLDDATE", "FLDREASON" };
		string[] alCaptions = { "Date", "Reason" };
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

		ds = PhoenixPreSeaHoliday.HolidaySearch(General.GetNullableInteger(ddlYear.SelectedValue.ToString()),General.GetNullableDateTime(txtDate.Text),General.GetNullableString(txtReason.Text), 
			sortexpression, sortdirection,
			Int32.Parse(ViewState["PAGENUMBER"].ToString()),
			iRowCount,
			ref iRowCount,
			ref iTotalPageCount);

		Response.AddHeader("Content-Disposition", "attachment; filename=Holiday.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3> Holiday Register</h3></td>");
		Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
		Response.Write("</tr>");
		Response.Write("</TABLE>");
		Response.Write("<br />");
		Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
		Response.Write("<tr>");
		for (int i = 0; i < alCaptions.Length; i++)
		{
			Response.Write("<td width='20%'>");
			Response.Write("<b>" + alCaptions[i] + "</b>");
			Response.Write("</td>");
		}
		Response.Write("</tr>");
		foreach (DataRow dr in ds.Tables[0].Rows)
		{
			Response.Write("<tr>");
			for (int i = 0; i < alColumns.Length; i++)
			{
				Response.Write("<td>");
				Response.Write(dr[alColumns[i]]);
				Response.Write("</td>");
			}
			Response.Write("</tr>");
		}
		Response.Write("</TABLE>");
		Response.End();

	}

	protected void RegistersHoliday_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("FIND"))
			{
				gvHoliday.EditIndex = -1;
				gvHoliday.SelectedIndex = -1;
				ViewState["PAGENUMBER"] = 1;
				BindData();
				SetPageNavigator();
			}
			if (dce.CommandName.ToUpper().Equals("EXCEL"))
			{
				ShowExcel();
			}
            if (dce.CommandName.ToUpper().Equals("COPY"))
            {
                CopyHolidayList();
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


		int iRowCount = 0;
		int iTotalPageCount = 0;
		string[] alColumns = { "FLDDATE", "FLDREASON" };
		string[] alCaptions = { "Date", "Reason" };
		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		DataSet ds = new DataSet();
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		
		ds = PhoenixPreSeaHoliday.HolidaySearch(General.GetNullableInteger(ddlYear.SelectedValue),General.GetNullableDateTime(txtDate.Text),General.GetNullableString(txtReason.Text), 
			sortexpression, sortdirection,
			Int32.Parse(ViewState["PAGENUMBER"].ToString()),
			General.ShowRecords(null),
			ref iRowCount,
			ref iTotalPageCount);

		General.SetPrintOptions("gvHoliday", "Registers", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{

			gvHoliday.DataSource = ds;
			gvHoliday.DataBind();
		}
		else
		{

			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvHoliday);
		}

		ViewState["ROWCOUNT"] = iRowCount;
		ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
		SetPageNavigator();
	}


	protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;
		BindData();
		SetPageNavigator();
	}

	protected void gvHoliday_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindData();
	}

	protected void gvHoliday_Sorting(object sender, GridViewSortEventArgs se)
	{
		gvHoliday.EditIndex = -1;
		gvHoliday.SelectedIndex = -1;
		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
	}
	protected void gvHoliday_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;
			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
			if (e.CommandName.ToUpper().Equals("ADD"))
			{
				if (!IsValidHoliday(((UserControlDate)_gridView.FooterRow.FindControl("txtDateAdd")).Text))
				{
					ucError.Visible = true;
					return;
				}
				InsertHoliday(
					((UserControlDate)_gridView.FooterRow.FindControl("txtDateAdd")).Text,
					((TextBox)_gridView.FooterRow.FindControl("txtReasonAdd")).Text
				);
				BindData();
				((UserControlDate)_gridView.FooterRow.FindControl("txtDateAdd")).Focus();
			}
			else if (e.CommandName.ToUpper().Equals("SAVE"))
			{
				if (!IsValidHoliday(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDateEdit")).Text))
				{
					ucError.Visible = true;
					return;
				}
				InsertHoliday(
					 ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDateEdit")).Text,
					 ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtReasonEdit")).Text
				 );
				_gridView.EditIndex = -1;
				BindData();

			}

			else if (e.CommandName.ToUpper().Equals("DELETE"))
			{
				DeleteHoliday(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblHolidayId")).Text));
			}

			SetPageNavigator();
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

    protected void BindHolidayYear()
    {
        ListItem li = new ListItem("-select-", "Dummy");
        ddlYear.Items.Add(li);
        for (int i = (DateTime.Today.Year - 5); i <= (DateTime.Today.Year)+1; i++)
        {
            li = new ListItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.DataBind();
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
    }

	protected void gvHoliday_RowDeleting(object sender, GridViewDeleteEventArgs de)
	{
		BindData();
		SetPageNavigator();
	}
	protected void gvHoliday_RowCreated(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
		{
			e.Row.TabIndex = -1;
			e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvHoliday, "Edit$" + e.Row.RowIndex.ToString(), false);
		}
	}
	protected void gvHoliday_RowEditing(object sender, GridViewEditEventArgs de)
	{
		try
		{
			GridView _gridView = (GridView)sender;

			_gridView.EditIndex = de.NewEditIndex;
			_gridView.SelectedIndex = de.NewEditIndex;
			BindData();
			((UserControlDate)_gridView.Rows[de.NewEditIndex].FindControl("txtDateEdit")).Focus();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvHoliday_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;
			if (!IsValidHoliday(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDateEdit")).Text))
			{
				ucError.Visible = true;
				return;
			}
			InsertHoliday(
					 ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDateEdit")).Text,
					 ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtReasonEdit")).Text
				 );
			_gridView.EditIndex = -1;
			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}


	}
	protected void gvHoliday_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
			if (db != null)
			{
				db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
				db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
			}

			ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
			if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

			ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
			if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

			ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
			if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
		}
		if (e.Row.RowType == DataControlRowType.Footer)
		{
			ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
			if (db != null)
			{
				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
					db.Visible = false;
			}
		}
	}

	protected void cmdSearch_Click(object sender, EventArgs e)
	{
		gvHoliday.SelectedIndex = -1;
		gvHoliday.EditIndex = -1;
		ViewState["PAGENUMBER"] = 1;
		BindData();
		SetPageNavigator();
	}

	protected void cmdGo_Click(object sender, EventArgs e)
	{
		int result;
		gvHoliday.SelectedIndex = -1;
		gvHoliday.EditIndex = -1;
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
		gvHoliday.SelectedIndex = -1;
		gvHoliday.EditIndex = -1;
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

	private void InsertHoliday(string date, string reason)
	{

		PhoenixPreSeaHoliday.InsertHoliday(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
			Convert.ToDateTime(date),General.GetNullableString(reason));
		ucStatus.Text = "Information updated";
	}

	
	private bool IsValidHoliday(string date)
	{
		ucError.HeaderMessage = "Please provide the following required information";

		GridView _gridView = gvHoliday;

        if (String.IsNullOrEmpty(date))
            ucError.ErrorMessage = "Holiday date is required.";

		return (!ucError.IsError);
	}

	private void DeleteHoliday(int Holidaycode)
	{
		PhoenixPreSeaHoliday.DeleteHoliday(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Holidaycode);
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
    protected void ddlYear_TextChanged1(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }
}
