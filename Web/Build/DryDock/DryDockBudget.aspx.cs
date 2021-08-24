using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DryDock;

public partial class DryDockBudget : PhoenixBasePage
{
	protected override void Render(HtmlTextWriter writer)
	{
		foreach (GridViewRow r in gvBudget.Rows)
		{
			if (r.RowType == DataControlRowType.DataRow)
			{
				Page.ClientScript.RegisterForEventValidation(gvBudget.UniqueID, "Select$" + r.RowIndex.ToString());
			}
		}
		base.Render(writer);
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);

		PhoenixToolbar toolbar = new PhoenixToolbar();
		toolbar.AddImageButton("../DryDock/DryDockBudget.aspx", "Export to Excel", "icon_xls.png", "Excel");
		toolbar.AddImageLink("javascript:CallPrint('gvBudget')", "Print Grid", "icon_print.png", "PRINT");
		toolbar.AddImageButton("../DryDock/DryDockBudget.aspx", "Find", "search.png", "FIND");
		MenuDryDockBudget.AccessRights = this.ViewState;
		MenuDryDockBudget.MenuList = toolbar.Show();
		MenuDryDockBudget.SetTrigger(pnlDryDockBudget);

		if (!IsPostBack)
		{
			
			ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
			ViewState["CURRENTINDEX"] = 1;
		}
		BindData();
		SetPageNavigator();
	}
	
	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		DataSet ds = new DataSet();
		string[] alColumns = { "FLDCERTIFICATENAME", "FLDCERTIFICATENO", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDISSUINGAUTHORITYNAME" };
		string[] alCaptions = { "Certificate", "Number", "Issue Date", "Expiry Date", "Authority" };
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

		 ds = PhoenixDryDockBudget.DryDockBudgetSearch(null,
			General.GetNullableString(txtBudgetCode.Text),
			General.GetNullableString(txtBudgetGroup.Text),
			sortexpression, sortdirection,
			Int32.Parse(ViewState["PAGENUMBER"].ToString()),
			iRowCount,
			ref iRowCount,
			ref iTotalPageCount);

		Response.AddHeader("Content-Disposition", "attachment; filename=DryDockBudget.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3>Budget</h3></td>");
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

	protected void DryDockBudget_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		if (dce.CommandName.ToUpper().Equals("EXCEL"))
		{
			ShowExcel();
		}
	}

	private void BindData()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		string[] alColumns = { "FLDNAME", "FLDBUDGETCODE", "FLDBUDGETGROUP" };
		string[] alCaptions = { "Principal", "Budget Code", "Budget Group" };

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


		DataSet ds = PhoenixDryDockBudget.DryDockBudgetSearch(null,
			General.GetNullableString(txtBudgetCode.Text),
			General.GetNullableString(txtBudgetGroup.Text),
	        sortexpression, sortdirection,
			Int32.Parse(ViewState["PAGENUMBER"].ToString()),
			General.ShowRecords(null),
			ref iRowCount,
			ref iTotalPageCount);

		General.SetPrintOptions("gvBudget", "Budget", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvBudget.DataSource = ds;
			gvBudget.DataBind();
		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvBudget);
		}

		ViewState["ROWCOUNT"] = iRowCount;
		ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

	}

	protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;

		ViewState["SORTEXPRESSION"] = ib.CommandName;
		ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

	}

	protected void gvBudget_RowCreated(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
		{
			e.Row.TabIndex = -1;
			e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvBudget, "Select$" + e.Row.RowIndex.ToString(), false);
		}
	}

	protected void gvBudget_Sorting(object sender, GridViewSortEventArgs se)
	{
		gvBudget.SelectedIndex = -1;
		gvBudget.EditIndex = -1;

		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
	}

	protected void gvBudget_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;
			if (!IsValidBudget(((UserControlAddressType)_gridView.Rows[nCurrentRow].FindControl("ucPrinicipalEdit")).SelectedAddress,
				   ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBudgetCodeEdit")).Text,
				   ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBudgetGroupEdit")).Text))
			{
				ucError.Visible = true;
				return;
			}
			PhoenixDryDockBudget.UpdateDryDockBudget(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBudgetidEdit")).Text),
				Convert.ToInt64(((UserControlAddressType)_gridView.Rows[nCurrentRow].FindControl("ucPrinicipalEdit")).SelectedAddress),
				((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBudgetCodeEdit")).Text,
				((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBudgetGroupEdit")).Text);

			
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

	protected void gvBudget_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		gvBudget.SelectedIndex = e.NewSelectedIndex;
	}

	protected void gvBudget_RowEditing(object sender, GridViewEditEventArgs de)
	{
		try
		{
			GridView _gridView = (GridView)sender;

			_gridView.EditIndex = de.NewEditIndex;
			_gridView.SelectedIndex = de.NewEditIndex;

			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvBudget_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;

			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

			if (e.CommandName.ToUpper().Equals("ADD"))
			{
				if (!IsValidBudget(((UserControlAddressType)_gridView.FooterRow.FindControl("ucPrinicipalAdd")).SelectedAddress,
				   ((TextBox)_gridView.FooterRow.FindControl("txtBudgetCodeAdd")).Text,
				   ((TextBox)_gridView.FooterRow.FindControl("txtBudgetGroupAdd")).Text))
				{
					ucError.Visible = true;
					return;
				}
				PhoenixDryDockBudget.InsertDryDockBudget(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Convert.ToInt64(((UserControlAddressType)_gridView.FooterRow.FindControl("ucPrinicipalAdd")).SelectedAddress),
				((TextBox)_gridView.FooterRow.FindControl("txtBudgetCodeAdd")).Text,
				((TextBox)_gridView.FooterRow.FindControl("txtBudgetGroupAdd")).Text);

				BindData();
		
			}
		
			else if (e.CommandName.ToUpper().Equals("DELETE"))
			{
				PhoenixDryDockBudget.DeleteDryDockBudget(PhoenixSecurityContext.CurrentSecurityContext.UserCode
					, new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBudgetid")).Text));
				BindData();
			}

			
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvBudget_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

		if (e.Row.RowType == DataControlRowType.Footer)
		{
			ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
			if (db != null)
			{
				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
					db.Visible = false;
			}
		}

		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
			if (del != null)
			{
				del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
			}

			ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
			if (edit != null)
			{
				edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
			}

			ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
			if (save != null)
			{
				save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
			}

			ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
			if (cancel != null)
			{
				cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
			}


			//if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
			//    && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
			//{
				ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
				if (db != null)
				{
					db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
				}

				LinkButton lb = (LinkButton)e.Row.FindControl("lnkCertificateName");
				if (lb != null)
				{
					if (!SessionUtil.CanAccess(this.ViewState, lb.CommandName))
						lb.CommandName = "";
				}

				
			//}
			//else
			//    e.Row.Attributes["onclick"] = "";

			UserControlAddressType ucAddressType = (UserControlAddressType)e.Row.FindControl("ucPrinicipalEdit");
			DataRowView drview = (DataRowView)e.Row.DataItem;
			if (ucAddressType != null) ucAddressType.SelectedAddress = drview["FLDPRINCIPALID"].ToString();
		}
	}

	protected void gvBudget_RowDeleting(object sender, GridViewDeleteEventArgs de)
	{
		BindData();
		SetPageNavigator();
	}

	protected void gvBudget_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindData();
	}

	protected void cmdSearch_Click(object sender, EventArgs e)
	{
		gvBudget.SelectedIndex = -1;
		gvBudget.EditIndex = -1;
		BindData();
		SetPageNavigator();
	}

	protected void cmdGo_Click(object sender, EventArgs e)
	{
		int result;

		gvBudget.SelectedIndex = -1;
		gvBudget.EditIndex = -1;

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

	public void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		BindData();
		SetPageNavigator();
	}

	protected void PagerButtonClick(object sender, CommandEventArgs ce)
	{
		gvBudget.SelectedIndex = -1;
		gvBudget.EditIndex = -1;

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
			return true;

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

	

	private bool IsValidBudget(
		  string principal
		, string budgetcode
		, string budgetgroup
		)
	{
		
		ucError.HeaderMessage = "Please provide the following required information";

		if (General.GetNullableInteger(principal)==null)
			ucError.ErrorMessage = "Principal is required.";

		if (budgetcode.Trim().Equals(""))
			ucError.ErrorMessage = "Budget Code is required.";

		if (budgetgroup.Trim().Equals(""))
			ucError.ErrorMessage = "Budget Group is required.";


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
