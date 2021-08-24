using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using System.Text;
using Telerik.Web.UI;

public partial class CrewCourseEmployeeList : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);
		PhoenixToolbar toolbar = new PhoenixToolbar();
		toolbar.AddButton("Search", "SEARCH");
	//	toolbar.AddButton("Save", "SAVE");
		CrewList.AccessRights = this.ViewState;
		
		CrewList.MenuList = toolbar.Show();
		if (!IsPostBack)
		{
			ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
			ViewState["CURRENTINDEX"] = 1;
			if (Request.QueryString["typelist"] != "1")
			{
				ucBatch.CssClass = "dropdown_mandatory";
				ucBatch.Enabled = true;
				if (Session["COURSEID"] != null)
				{
					ucBatch.CourseId = Session["COURSEID"].ToString();
				}
				else
				{
					ucBatch.CourseId = Filter.CurrentCourseSelection;
				}
			}
			else
			{
				ucBatch.CssClass = "input";
				ucBatch.Enabled = false;
			}
			if (Request.QueryString["batchid"] != null)
			{
				ucBatch.SelectedBatch =  Request.QueryString["batchid"];
				ucBatch.Enabled = false;
			}

		}
		BindData();
		SetPageNavigator();
	}
	protected void CrewList_TabStripCommand(object sender, EventArgs e)
	{
		try
		{

			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("SEARCH"))
			{
				ViewState["PAGENUMBER"] = 1;
				BindData();
			}
			if (dce.CommandName.ToUpper().Equals("SAVE"))
			{
				if (Request.QueryString["typelist"] != "1")
				{
					if (!IsValidBatch())
					{
						ucError.Visible = true;
						return;
					}
 
				}
				//SaveEmployee();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private bool IsValidBatch()
	{
		ucError.HeaderMessage = "Please provide the following required information";

		int result;

		if (int.TryParse(ucBatch.SelectedBatch, out result) == false)
			ucError.ErrorMessage = "Batch No  is required.";

		return (!ucError.IsError);

	}
	public void BindData()
	{

		int iRowCount = 0;
		int iTotalPageCount = 0;
		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		try
		{

			DataSet ds = PhoenixCrewCourseEnrollment.CrewEnrollmentEmployeeSearch(
															Session["COURSEID"]!=null?
																General.GetNullableInteger(Session["COURSEID"].ToString()):Convert.ToInt32(Filter.CurrentCourseSelection)
															 , chkOnBoardSeafarer.Checked==true? 1:0 
															 , sortexpression, sortdirection
															 , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
															 , ref iRowCount, ref iTotalPageCount
															 , General.GetNullableString(txtName.Text)
															 , General.GetNullableInteger(ddlRank.SelectedRank)
															 , General.GetNullableString(txtFileNo.Text)
															 , General.GetNullableInteger(Request.QueryString["batchid"]));

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvCrewList.DataSource = ds;
				gvCrewList.DataBind();
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvCrewList);
			}
			ViewState["ROWCOUNT"] = iRowCount;
			ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
			
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	protected void gvCrewList_RowDataBound(object sender, GridViewRowEventArgs e)
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
			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
			{
				Label lb = (Label)e.Row.FindControl("lblPLStatus");
				ImageButton imgPL = (ImageButton)e.Row.FindControl("cmdPL");
				if (lb.Text == "Participant List" || lb.Text == "Wait List" || lb.Text == "Nomination List")
				{
					imgPL.Enabled = false;
					imgPL.Visible = false;
				}
			
			}
		}
		
	}
	protected void gvCrewList_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;

			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
			if (e.CommandName.ToUpper().Equals("ADDLIST"))
			{
				
				SaveEmployee(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeid")).Text));
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
	protected void SaveEmployee(int employeeid)
	{
	
		string Script = "";
		Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
		Script += "fnReloadList('codehelp1','ifMoreInfo','');";
		Script += "</script>" + "\n";

		try
		{
			int iMessageCode = 0;
			string iMessageText = "";

			PhoenixCrewCourseEnrollment.InsertCrewCourseEnrollment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Convert.ToInt32(Session["COURSEID"] != null ?
				General.GetNullableInteger(Session["COURSEID"].ToString()) : Convert.ToInt32(Filter.CurrentCourseSelection)),
				employeeid.ToString(),
				General.GetNullableInteger(Request.QueryString["type"].ToString()), 1,
				ucConfirm.confirmboxvalue, ref iMessageCode, ref iMessageText,
				General.GetNullableInteger(Request.QueryString["typelist"].ToString()),
				General.GetNullableInteger(ucBatch.SelectedBatch));
			BindData();
			if (iMessageCode == 1)
				throw new ApplicationException(iMessageText);
			Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
		}
		catch (ApplicationException aex)
		{
			ucConfirm.HeaderMessage = "Please Confirm";
			ucConfirm.ErrorMessage = aex.Message;
			ucConfirm.Visible = true;
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	



	}


	protected void ucConfirm_OnClick(object sender, EventArgs e)
	{
		//SaveEmployee();
		BindData();
	}
	protected void gvCrewList_Sorting(object sender, GridViewSortEventArgs se)
	{
		GridView _gridView = (GridView)sender;
		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
	}
	private void SetPageNavigator()
	{
		try
		{
			cmdPrevious.Enabled = IsPreviousEnabled();
			cmdNext.Enabled = IsNextEnabled();
			lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
			lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
			lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
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

	protected void cmdGo_Click(object sender, EventArgs e)
	{
		try
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
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void PagerButtonClick(object sender, CommandEventArgs ce)
	{
		try
		{
			gvCrewList.SelectedIndex = -1;
			gvCrewList.EditIndex = -1;
			if (ce.CommandName == "prev")
				ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
			else
				ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

			BindData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
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
