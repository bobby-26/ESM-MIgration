using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewCourseBatchList : PhoenixBasePage
{


	protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);
		PhoenixToolbar toolbar = new PhoenixToolbar();
		toolbar.AddImageButton("../Crew/CrewCourseBatchList.aspx", "Export to Excel", "icon_xls.png", "Excel");
		toolbar.AddImageLink("javascript:CallPrint('gvBatchList')", "Print Grid", "icon_print.png", "PRINT");
		MenuCrewBatchList.AccessRights = this.ViewState;
		MenuCrewBatchList.MenuList = toolbar.Show();
		cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
		MenuCrewBatchList.SetTrigger(pnlBatch);
		if (!IsPostBack)
		{
			ViewState["fromdate"] = "";
			ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
			ViewState["CURRENTINDEX"] = 1;
			Session["COURSEID"] = null;
		
            BindCourse();
			if (Filter.CurrentCourseSelection != null)
			{
				ddlCourse.SelectedValue = Filter.CurrentCourseSelection;
			}
			
		}
		BindData();
		SetPageNavigator();
	}

    private void BindCourse()
    {
		DataSet ds = PhoenixRegistersDocumentCourse.ListPostSeaCourse(null);
        ddlCourse.DataSource = ds;
        ddlCourse.DataBind();
    }
	
	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		DataSet ds = new DataSet();
		string[] alColumns = { "FLDBATCHNO", "FLDFROMDATE", "FLDTODATE", "FLDBATCHLOCATION", "FLDSTATUS", "FLDPUBLISHEDYN" };
		string[] alCaptions = { "Batch No", "From Date", "To Date", "Batch Location", "Status", "Published Y/N" };
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


		ds = PhoenixRegistersBatch.BatchSearch(null,
			   General.GetNullableInteger(Session["COURSEID"].ToString()), null, sortexpression, sortdirection,
			   (int)ViewState["PAGENUMBER"],
			   iRowCount,
			   ref iRowCount,
			   ref iTotalPageCount,
			   null,null,null,null);

		Response.AddHeader("Content-Disposition", "attachment; filename=BatchList.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3>Batch List</h3></td>");
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

	protected void CrewBatchList_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		if (dce.CommandName.ToUpper().Equals("FIND"))
		{
			ViewState["PAGENUMBER"] = 1;
			BindData();
			SetPageNavigator();
		}
		if (dce.CommandName.ToUpper().Equals("EXCEL"))
		{
			ShowExcel();
		}
	}

	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		BindData();
	}

	private void BindData()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;
		string courseid = null;
		string[] alColumns = { "FLDBATCHNO", "FLDFROMDATE", "FLDTODATE", "FLDBATCHLOCATION", "FLDSTATUS", "FLDPUBLISHEDYN" };
		string[] alCaptions = { "Batch No", "From Date", "To Date", "Batch Location", "Status", "Published Y/N" };

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		if (Filter.CurrentCourseSelection == null)
		{
			courseid = "0";
			//Filter.CurrentCourseSelection = null;
		}
		else
		{
			courseid = Filter.CurrentCourseSelection;
		}

		DataSet ds = PhoenixRegistersBatch.BatchSearch(null
                , General.GetNullableInteger(courseid)
                , null
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , General.ShowRecords(null)
                , ref iRowCount
                , ref iTotalPageCount
                , null
                , null
                , null
                , 0
        );

		General.SetPrintOptions("gvBatchList", "Batch List", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvBatchList.DataSource = ds;
			gvBatchList.DataBind();

		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvBatchList);
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

	protected void gvBatchList_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;

			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
			if (e.CommandName.ToUpper().Equals("DELETE"))
			{
				PhoenixRegistersBatch.DeleteBatch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
							Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchId")).Text));
			}
			if (e.CommandName.ToUpper().Equals("BATCH"))
			{
				int batchid = int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchId")).Text);
			
				Filter.CurrentCourseSelection = ddlCourse.SelectedValue;
				ClientScript.RegisterStartupScript(GetType(), "Load", "<script type='text/javascript'>window.parent.location.href = '../Crew/CrewBatchMaster.aspx?calledfrom=training&batchid=" + batchid + "'; </script>");
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

	private bool IsValidEvaluation(string itemid, string sortorder)
	{
		ucError.HeaderMessage = "Please provide the following required information";
		int result;

		if (int.TryParse(itemid, out result) == false)
			ucError.ErrorMessage = "Item Name is required.";

		if (sortorder.Trim().Equals(""))
			ucError.ErrorMessage = "Sort Order is required.";




		return (!ucError.IsError);
	}
	protected void gvBatchList_RowEditing(object sender, GridViewEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = e.NewEditIndex;
		_gridView.SelectedIndex = e.NewEditIndex;
	}

	protected void gvBatchList_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
				//ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
				//if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
				Label l = (Label)e.Row.FindControl("lblBatchId");

				//LinkButton lb = (LinkButton)e.Row.FindControl("lnkBatchNo");
				//lb.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', '../Registers/RegistersBatchList.aspx?batchid=" + l.Text + "');return false;");

				//ImageButton db1 = (ImageButton)e.Row.FindControl("cmdEdit");
				//db1.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', '../Registers/RegistersBatchList.aspx?batchid=" + l.Text + "'); return false;");
			}
		}


	}

	protected void gvBatchList_Sorting(object sender, GridViewSortEventArgs se)
	{
		gvBatchList.SelectedIndex = -1;
		gvBatchList.EditIndex = -1;

		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
	}

	protected void cmdSearch_Click(object sender, EventArgs e)
	{
		BindData();
		SetPageNavigator();
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
		gvBatchList.SelectedIndex = -1;
		gvBatchList.EditIndex = -1;
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

    protected void ddlCourse_DataBound(object sender, EventArgs e)
    {
        ddlCourse.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCourse.SelectedValue != "Dummy")
        {
            Filter.CurrentCourseSelection = ddlCourse.SelectedValue;
        }
        else
        {
            Filter.CurrentCourseSelection = null;
        }
        BindData();
    }

	//protected void PrevClick(object sender, EventArgs e)
	//{
	//    try
	//    {

	//        SetPrimaryDetails(1, chkExcludeSunday.Checked == true ? 7 : 8, Convert.ToDateTime(txtFromDate.Text), null, chkExcludeSunday.Checked == true ? 1 : 0);
	//        BindCourse();
	//        Filter.CurrentCourseSelection = null;
	//        BindData();
	//        SetPageNavigator();
	//    }
	//    catch (Exception ex)
	//    {
	//        ucError.ErrorMessage = ex.Message;
	//        ucError.Visible = true;
	//    }
	//}
	//protected void NextClick(object sender, EventArgs e)
	//{
	//    SetPrimaryDetails(null, chkExcludeSunday.Checked == true ? 7 : 8, Convert.ToDateTime(txtFromDate.Text), null, chkExcludeSunday.Checked == true ? 1 : 0);
	//    BindCourse();
	//    Filter.CurrentCourseSelection = null;
	//    BindData();
	//    SetPageNavigator();
	//}

	//protected void WeekSetting(object sender, EventArgs e)
	//{
	//    try
	//    {
	//        int yn = 0;

	//        //if (ViewState["fromdate"].ToString() != "")
	//        //{
	//            DateTime dtfrom = Convert.ToDateTime(txtFromDate.Text);
	//            DateTime dtto = Convert.ToDateTime(txtToDate.Text);
	//            if (dtfrom > dtto)
	//            {
	//                yn = 1;
	//            }
	//        //}

	//        SetPrimaryDetails(null, 0, Convert.ToDateTime(txtFromDate.Text), yn, chkExcludeSunday.Checked == true ? 1 : 0);
	//        BindCourse();
	//        BindData();
	//        SetPageNavigator();
	//    }
	//    catch (Exception ex)
	//    {
	//        ucError.ErrorMessage = ex.Message;
	//        ucError.Visible = true;
	//    }
	//}
	//protected void SetPrimaryDetails(int? type, int? dateadd, DateTime date, int? yn, int? excludesunday)
	//{
	//    DataSet ds = PhoenixCrewCoursePlanner.SearchFirstLastWeekDay(type, dateadd, date, excludesunday);
	//    if (ds.Tables[0].Rows.Count > 0)
	//    {
	//        if (yn!= 0)
	//        {
	//            txtFromDate.Text = ds.Tables[0].Rows[0]["FLDFIRSTDATEOFWEEK"].ToString();
	//            ViewState["fromdate"] = txtFromDate.Text;
	//        }
	//        txtToDate.Text = ds.Tables[0].Rows[0]["FLDLASTDATEOFWEEK"].ToString();
	//        Session["FROMDATE"]=txtFromDate.Text;
	//        Session["TODATE"] = txtToDate.Text;
	//    }
	//}
//    protected void ExcludeSunday(object sender, EventArgs e)
//    {
//        SetPrimaryDetails(null, 0, Convert.ToDateTime(txtFromDate.Text), null, chkExcludeSunday.Checked == true ? 1 : 0);
//        BindCourse();
//        if (Filter.CurrentCourseSelection != null)
//        {
//            ddlCourse.SelectedValue = Filter.CurrentCourseSelection;
//        }
//        BindData();
//        SetPageNavigator();
//    }
}
