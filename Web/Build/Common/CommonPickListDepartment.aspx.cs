using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;

public partial class CommonPickListDepartment : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{

		PhoenixToolbar toolbarmain = new PhoenixToolbar();
		toolbarmain.AddButton("Search", "SEARCH");
		MenuDepartment.MenuList = toolbarmain.Show();
		MenuDepartment.SetTrigger(pnlDepartmentEntry);

		if (!IsPostBack)
		{
			ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
			if (Filter.CurrentPickListSelection != null)
			{
				NameValueCollection nvc = Filter.CurrentPickListSelection;
				ViewState["DEPARTMENTID"] = nvc.Get(2);
			}
		}

		BindData();
		SetPageNavigator();
	}

	private void BindData()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		string[] alColumns = { "FLDDEPARTMENTCODE", "FLDDEPARTMENTNAME", "FLDDEPARTMENTTYPENAME" };
		string[] alCaptions = { "Code", "Name", "Type " };

		DataSet ds = PhoenixRegistersDepartment.DepartmentSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtDepartmentCode.Text, txtSearch.Text, sortexpression, sortdirection,
			(int)ViewState["PAGENUMBER"],
			General.ShowRecords(null),
			ref iRowCount,
			ref iTotalPageCount);

		General.SetPrintOptions("gvDepartment", "Department", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvDepartment.DataSource = ds;
			gvDepartment.DataBind();
		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvDepartment);
		}

		ViewState["ROWCOUNT"] = iRowCount;
		ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
	}
	protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;

		ViewState["SORTEXPRESSION"] = ib.CommandName;
		ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

		BindData();
		SetPageNavigator();
	}

	protected void gvDepartment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindData();
	}

	protected void gvDepartment_RowCreated(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
		{
			e.Row.TabIndex = -1;
			e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvDepartment, "Edit$" + e.Row.RowIndex.ToString(), false);
		}
	}

	protected void gvDepartment_Sorting(object sender, GridViewSortEventArgs se)
	{
		gvDepartment.SelectedIndex = -1;
		gvDepartment.EditIndex = -1;

		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
	}


	protected void cmdSearch_Click(object sender, EventArgs e)
	{
		gvDepartment.SelectedIndex = -1;
		gvDepartment.EditIndex = -1;

		BindData();
		SetPageNavigator();
	}

	protected void cmdGo_Click(object sender, EventArgs e)
	{
		int result;
		gvDepartment.SelectedIndex = -1;
		gvDepartment.EditIndex = -1;

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
		gvDepartment.SelectedIndex = -1;
		gvDepartment.EditIndex = -1;
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
		gv.Rows[0].Attributes["onclick"] = "";
	}
	protected void MenuDepartment_TabStripCommand(object sender, EventArgs e)
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

	protected void gvDepartment_RowEditing(object sender, GridViewEditEventArgs e)
	{
	}

	protected void gvDepartment_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{

			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
			{
				//CheckBox chk = (CheckBox)e.Row.FindControl("chkSelect");
				//Label lblDepartmentId = (Label)e.Row.FindControl("lblDepartmentId");
				//if (ViewState["DepartmentS"] != null && lblDepartmentId != null && chk != null)
				//{
				//    string Departments = "," + ViewState["DepartmentS"].ToString() + ",";
				//    string Departmentid = "," + lblDepartmentId.Text.Trim() + ",";
				//    chk.Checked = Departments.Contains(Departmentid);
				//}
			}
		}
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
	}


	protected void gvDepartment_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		if (e.CommandName.ToUpper().Equals("SORT"))
			return;

		GridView _gridView = (GridView)sender;
		int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
		string Script = "";
		NameValueCollection nvc;

		if (Request.QueryString["mode"] == "custom")
		{
			Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
			if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
				Script += "fnReloadList('codehelp1','ifMoreInfo',true);";
			else
				Script += "fnReloadList('codehelp1','ifMoreInfo');";
			Script += "</script>" + "\n";

			nvc = new NameValueCollection();

			//Label lb = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDepartmentCode");
			//nvc.Set(nvc.GetKey(1), lb.Text.ToString());

			LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkDepartmentName");
			nvc.Add(lb.ID, lb.Text.ToString());
			Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDepartmentId");
			nvc.Add(lbl.ID, lbl.Text);
		}
		else
		{

			nvc = Filter.CurrentPickListSelection;

			Label lb = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDepartmentCode");
			nvc.Set(nvc.GetKey(1), lb.Text.ToString());
			Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDepartmentName");
			nvc.Set(nvc.GetKey(2), lbl.Text);
			Label lbl1 = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDepartmentId");
			nvc.Set(nvc.GetKey(3), lbl1.Text);


			Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
			if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
				Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
			else
				Script += "fnClosePickList('codehelp1','ifMoreInfo');";
			Script += "</script>" + "\n";
		}

		//Filter.CurrentPickListSelection = nvc;
		Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
	}

	//protected void CloseWindow(object sender, EventArgs e)
	//{
	//    if (sender != null && ((UserControlConfirmMessage)sender).confirmboxvalue == 1)
	//    {
	//        string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
	//        if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
	//            Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
	//        else
	//            Script += "fnClosePickList('codehelp1','ifMoreInfo');";
	//        Script += "</script>" + "\n";
	//        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
	//    }
	//}

}
