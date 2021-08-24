using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;

public partial class CommonPickListStockBudget : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{

		PhoenixToolbar toolbarmain = new PhoenixToolbar();
		toolbarmain.AddButton("Search", "SEARCH");
		MenuStockBudget.MenuList = toolbarmain.Show();
		MenuStockBudget.SetTrigger(pnlStockBudget);

		if (!IsPostBack)
		{
			ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
		}
		BindData();
	}


	protected void MenuStockBudget_TabStripCommand(object sender, EventArgs e)
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
			int iTotalPageCount = 0;

			DataSet ds;

			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;
			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ds = PhoenixCommonRegisters.BudgetSearch(
				1 
				, txtBudgetCodeSearch.Text
				, txtDescriptionNameSearch.Text
				, null
				, 30
				, sortexpression, sortdirection,
				Int32.Parse(ViewState["PAGENUMBER"].ToString()),
				General.ShowRecords(null),
				ref iRowCount,
				ref iTotalPageCount);

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvStockBudget.DataSource = ds;
				gvStockBudget.DataBind();
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvStockBudget);
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

	protected void gvStockBudget_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
		string Script = "";
		NameValueCollection nvc;

		if (Request.QueryString["mode"] == "custom")
		{

			Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
			Script += "fnReloadList('codehelp1','ifMoreInfo');";
			Script += "</script>" + "\n";

			nvc = new NameValueCollection();

			Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblBudgetCodeId");
			nvc.Add(lbl.ID, lbl.Text);
			LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkBudgetCode");
			nvc.Add(lb.ID, lb.Text.ToString());
		}
		else
		{

			Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
			Script += "fnClosePickList('codehelp1','ifMoreInfo');";
			Script += "</script>" + "\n";

			nvc = Filter.CurrentPickListSelection;

			Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblBudgetCodeId");
			nvc.Set(nvc.GetKey(1), lbl.Text);
			LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkBudgetCode");
			nvc.Set(nvc.GetKey(2), lb.Text.ToString());
		}

		Filter.CurrentPickListSelection = nvc;
		Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

	}

	protected void gvStockBudget_RowEditing(object sender, GridViewEditEventArgs e)
	{
	}

	protected void gvStockBudget_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
			{
			}
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
		SetPageNavigator();
	}

	protected void PagerButtonClick(object sender, CommandEventArgs ce)
	{
		gvStockBudget.SelectedIndex = -1;
		gvStockBudget.EditIndex = -1;
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

	protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;

		ViewState["SORTEXPRESSION"] = ib.CommandName;
		ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

		BindData();
		SetPageNavigator();
	}
}
