using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System.Web.UI.HtmlControls;

public partial class PreSeaBatch : PhoenixBasePage
{
	protected override void Render(HtmlTextWriter writer)
	{
		foreach (GridViewRow r in gvBatch.Rows)
		{
			if (r.RowType == DataControlRowType.DataRow)
			{
				Page.ClientScript.RegisterForEventValidation
						(r.UniqueID + "$ctl00");
				Page.ClientScript.RegisterForEventValidation
						(r.UniqueID + "$ctl01");
			}
		}
		base.Render(writer);
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
            SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddImageButton("../PreSea/PreSeaBatch.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbar.AddImageLink("javascript:CallPrint('gvBatch')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("javascript:parent.Openpopup('AddBatch','','../PreSea/PreSeaBatchList.aspx?calledfrom=" + Filter.CurrentPreSeaCourseMasterSelection + "');return false;", "Add", "add.png", "ADDBATCH");            
            MenuPreSeaBatch.AccessRights = this.ViewState;
			MenuPreSeaBatch.MenuList = toolbar.Show();
			MenuPreSeaBatch.SetTrigger(pnlBatchEntry);

            PhoenixToolbar MainToolbar = new PhoenixToolbar();

            MainToolbar.AddButton("List", "BATCH");
            MainToolbar.AddButton("Details", "DETAIL");
            //MainToolbar.AddButton("Semester", "SEMESTER");
           
            MenuCourseMaster.AccessRights = this.ViewState;
            MenuCourseMaster.MenuList = MainToolbar.Show();

            MenuCourseMaster.AccessRights = this.ViewState;
            MenuCourseMaster.MenuList = MainToolbar.Show();

            MenuCourseMaster.SelectedMenuIndex = 0;
            
			if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;

                ddlCourse.SelectedCourse = Filter.CurrentPreSeaCourseMasterSelection;
                Filter.CurrentPreSeaCourseMasterBatchSelection = null;
                ddlCourse.Enabled = false;               
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

	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		BindData();
		SetPageNavigator();
	}

	private void BindData()
	{
		try
		{
			int iRowCount = 0;
			int iTotalPageCount = 0;
			string[] alColumns = { "FLDCOURSENAME", "FLDBATCH", "FLDFROMDATE", "FLDTODATE" };
			string[] alCaptions = { "Course", "Batch", "From Date", "To Date" };
			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;

			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

			DataSet ds = PhoenixPreSeaBatch.BatchSearch(
						null
                        , General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterSelection), General.GetNullableString(null)
						, sortexpression, sortdirection
						, (int)ViewState["PAGENUMBER"]
						, General.ShowRecords(null)
						, ref iRowCount
						, ref iTotalPageCount, null, null, null);

            General.SetPrintOptions("gvBatch", "Batch", alCaptions, alColumns, ds);

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvBatch.DataSource = ds;
				gvBatch.DataBind();
                gvBatch.SelectedIndex = 0;
                if (String.IsNullOrEmpty(Filter.CurrentPreSeaCourseMasterBatchSelection))
                    Filter.CurrentPreSeaCourseMasterBatchSelection = ((Label)gvBatch.Rows[0].FindControl("lblBatchId")).Text;

                SetRowSelection();
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvBatch);
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

	protected void PreSeaBatch_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("EXCEL"))
			{
				int iRowCount = 0;
				int iTotalPageCount = 0;

				string[] alColumns = { "FLDCOURSENAME", "FLDBATCH", "FLDFROMDATE", "FLDTODATE" };
				string[] alCaptions = { "Course", "Batch", "From Date", "To Date" };
				string sortexpression;
				int? sortdirection = null;

				sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

				if (ViewState["SORTDIRECTION"] != null)
					sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

				if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
					iRowCount = 10;
				else
					iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
				DataSet ds = PhoenixPreSeaBatch.BatchSearch(
										null
                                        , General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterSelection), General.GetNullableString(null)
										, sortexpression, sortdirection
										, (int)ViewState["PAGENUMBER"]
										, iRowCount
										, ref iRowCount
										, ref iTotalPageCount,null, null, null);      
                    //General.ShowExcel("Pre-Sea Batch", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
                    Response.AddHeader("Content-Disposition", "attachment; filename=Batch.xls");
                    Response.ContentType = "application/vnd.msexcel";
                    Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                    Response.Write("<tr>");
                    Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/sims1.png" + "' /></td>");
                    Response.Write("<td><h3>PreSea Courses </h3></td>");
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

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

    protected void CourseMaster_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (Filter.CurrentPreSeaCourseMasterBatchSelection == null)
            {
                ucError.ErrorMessage = "Please select the Batch";
                ucError.Visible = true;
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("BATCH"))
            {
                Response.Redirect("../PreSea/PreSeaBatch.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("DETAIL"))
            {
                Response.Redirect("../PreSea/PreSeaBatchPlanDetails.aspx");
            }
            //else if (dce.CommandName.ToUpper().Equals("SEMESTER"))
            //{
            //    Response.Redirect("../PreSea/PreSeaBatchSemester.aspx");
            //}
               
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

	protected void gvBatch_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindData();
	}

	protected void gvBatch_RowCommand(object sender, GridViewCommandEventArgs e)
	{

		if (e.CommandName.ToUpper().Equals("SORT"))
			return;
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
       // Label l = (Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchId");
        if (e.CommandName.ToString().ToUpper() == "SELECT")
        {
            string BatchId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchId")).Text;
            Filter.CurrentPreSeaCourseMasterBatchSelection = BatchId;
            Response.Redirect("../PreSea/PreSeaBatchPlanDetails.aspx");
        }
        _gridView.EditIndex = -1;
		BindData();
	}

	protected void gvBatch_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;
			string batchid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchId")).Text;

			PhoenixPreSeaBatch.DeleteBatch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,Convert.ToInt32(batchid));
			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;

		}
	}

	protected void gvBatch_RowEditing(object sender, GridViewEditEventArgs e)
	{
		try
		{
			 GridView _gridView = (GridView)sender;
			_gridView.SelectedIndex = e.NewEditIndex;
			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	
	protected void gvBatch_Sorting(object sender, GridViewSortEventArgs se)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
	}

	protected void gvBatch_RowDataBound(object sender, GridViewRowEventArgs e)
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
            LinkButton lbtn = (LinkButton)e.Row.FindControl("lnkBtachName");
            lbtn.Attributes.Add("onclick", "parent.Openpopup('AddAddress', '', '../PreSea/PreSeaBatchList.aspx?batchid=" + lbtn.CommandArgument + "'); return false;");

            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
                edit.Attributes.Add("onclick", "parent.Openpopup('AddAddress', '', '../PreSea/PreSeaBatchList.aspx?batchid=" + lbtn.CommandArgument + "'); return false;");
            }

            ImageButton detail = (ImageButton)e.Row.FindControl("cmdSelect");
            if (detail != null)
            {
                detail.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            }

			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
				&& !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
			{
				del.Attributes.Add("onclick", "return fnConfirmDelete(event)");
			}
            ImageButton imgTimeSlot = (ImageButton)e.Row.FindControl("imgTimeSlot");
            if (imgTimeSlot != null)
            {
                Label lblBatchId = (Label)e.Row.FindControl("lblBatchId");

                imgTimeSlot.Visible = SessionUtil.CanAccess(this.ViewState, imgTimeSlot.CommandName);
                imgTimeSlot.Attributes.Add("onclick", "parent.Openpopup('RollNo', '', '../PreSea/PreSeaBatchTimeSlot.aspx?batchid=" + lblBatchId.Text + "');return false;");
            }

			UserControlAddressType ucAddressType = (UserControlAddressType)e.Row.FindControl("ucInstitutionEdit");
			DataRowView drv = (DataRowView)e.Row.DataItem;
			if (ucAddressType != null) ucAddressType.SelectedAddress = drv["FLDINSTITUTIONID"].ToString();

            UserControlPreSeaCourse ucCourse = (UserControlPreSeaCourse)e.Row.FindControl("ddlCourseEdit");
			DataRowView drvCourse = (DataRowView)e.Row.DataItem;
			if (ucCourse != null) ucCourse.SelectedCourse = drvCourse["FLDCOURSE"].ToString();
		}

	}

	protected void gvBatch_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.SelectedIndex = e.NewSelectedIndex;
        string BatchId = ((Label)_gridView.Rows[e.NewSelectedIndex].FindControl("lblBatchId")).Text;
        Filter.CurrentPreSeaCourseMasterBatchSelection = BatchId;
		_gridView.EditIndex = -1;
		BindData();        
		SetPageNavigator();
        //Response.Redirect("../PreSea/PreSeaBatchPlanDetails.aspx");
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
			SetPageNavigator();
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
			gvBatch.SelectedIndex = -1;
			gvBatch.EditIndex = -1;
			if (ce.CommandName == "prev")
				ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
			else
				ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

			BindData();
			SetPageNavigator();
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

	protected void cmdSort_Click(object sender, EventArgs e)
	{
		try
		{
			ImageButton ib = (ImageButton)sender;

			ViewState["SORTEXPRESSION"] = ib.CommandName;
			ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
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

	public StateBag ReturnViewState()
	{
		return ViewState;
	}

    private void SetRowSelection()
    {
        gvBatch.SelectedIndex = -1;
        for (int i = 0; i < gvBatch.Rows.Count; i++)
        {
            if (gvBatch.DataKeys[i].Value.ToString().Equals(Filter.CurrentPreSeaCourseMasterBatchSelection))
            {
                gvBatch.SelectedIndex = i;

            }
        }
    }
	
}
