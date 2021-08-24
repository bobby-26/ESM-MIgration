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
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using System.Drawing;
using Telerik.Web.UI;
public partial class CrewCoursePlannerFaculty : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
		SessionUtil.PageAccessRights(this.ViewState);
		PhoenixToolbar toolbar = new PhoenixToolbar();
		toolbar.AddImageButton("../Crew/CrewCourePlannerFaculty.aspx?batchid=" + Request.QueryString["batchid"] + "&includeholiday="+Request.QueryString["includeholiday"], "Export to Excel", "icon_xls.png", "Excel");
		toolbar.AddImageLink("javascript:CallPrint('gvCoursePlanner')", "Print Grid", "icon_print.png", "PRINT");
		MenuCrewFacultyList.AccessRights = this.ViewState;
		MenuCrewFacultyList.MenuList = toolbar.Show();
        PhoenixToolbar toolbarmenu = new PhoenixToolbar();
        toolbarmenu.AddButton("Batch", "BATCH");
        toolbarmenu.AddButton("Faculty", "FACULTY");
        MenuHeader.AccessRights = this.ViewState;
        MenuHeader.MenuList = toolbarmenu.Show();
        MenuHeader.SelectedMenuIndex = 1;
        if (!IsPostBack)
		{			
			ViewState["batchid"] = "";		
			if (Request.QueryString["batchid"] != null)
			{
				editbatch(Convert.ToInt32(Request.QueryString["batchid"]));	
			}
		}
		BindData();
		
    }

	protected void editbatch(int batchid)
	{
		DataSet ds= PhoenixRegistersBatch.EditBatch(batchid);
		if(ds.Tables[0].Rows.Count>0)
		{
			ViewState["courseid"] = ds.Tables[0].Rows[0]["FLDCOURSEID"].ToString();
		}
	}
	private void BindData()
	{
		try
		{
			string[] alColumns = { "FLDBATCH","FLDDATE", "FLDFACULTYNAMEAM", "FLDFACULTYNAMEPM" };
			string[] alCaptions = {"Batch No", "Date", "AM", "PM" };

			DataSet ds = PhoenixCrewCoursePlanner.ListCoursePlanner(Convert.ToInt32(Request.QueryString["includeholiday"]), Convert.ToInt32(Request.QueryString["batchid"]), Convert.ToInt32(Request.QueryString["excludesunday"]));

			General.SetPrintOptions("gvCoursePlanner", "Faculty Planner", alCaptions, alColumns, ds);

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvCoursePlanner.DataSource = ds;
				gvCoursePlanner.DataBind();
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvCoursePlanner);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

		if (dce.CommandName.ToUpper().Equals("BATCH"))
		{
			Response.Redirect("../Crew/CrewCoursePlannerBatch.aspx?batchid=" + Request.QueryString["batchid"] + "&courseid=" + ViewState["courseid"].ToString() + "&includeholiday=" + Request.QueryString["includeholiday"], false);
		}


	}
	protected void CrewFacultyList_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		if (dce.CommandName.ToUpper().Equals("FIND"))
		{
			ViewState["PAGENUMBER"] = 1;
			BindData();
			
		}
		if (dce.CommandName.ToUpper().Equals("EXCEL"))
		{
			ShowExcel();
		}
	}
	protected void gvCoursePlanner_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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
	protected void gvCoursePlanner_RowCommand(object sender, GridViewCommandEventArgs e)
	{

		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;

		}
	}
	protected void gvCoursePlanner_RowEditing(object sender, GridViewEditEventArgs e)
	{
		try
		{

			GridView _gridView = (GridView)sender;
			_gridView.SelectedIndex = e.NewEditIndex;

			_gridView.EditIndex = e.NewEditIndex;

			BindData();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvCoursePlanner_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;



			string dateam = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSplitAM")).Text;
			string datepm = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSplitPM")).Text;
			string facultyam = ((UserControlFacultyCode)_gridView.Rows[nCurrentRow].FindControl("ucFacultyAMEdit")).SelectedFacultyCode;
            string facultypm = ((UserControlFacultyCode)_gridView.Rows[nCurrentRow].FindControl("ucFacultyPMEdit")).SelectedFacultyCode;
             
			if (!IsValidCoursePlanner(ViewState["courseid"].ToString()))
			{
				ucError.Visible = true;
				return;
			}

			PhoenixCrewCoursePlanner.InsertCoursePlanner(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				  Convert.ToInt32(ViewState["courseid"].ToString())
				, Convert.ToDateTime(dateam)
				, Convert.ToDateTime(datepm)
				, General.GetNullableInteger(facultyam)
				, General.GetNullableInteger(facultypm)
				, Convert.ToInt32(Request.QueryString["batchid"])
				);
			_gridView.EditIndex = -1;
			BindData();
			String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");
			ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupopen, true);
		}

		catch (Exception ex)
		{
			BindData();
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}


	private bool IsValidCoursePlanner(string courseid)
	{
		ucError.HeaderMessage = "Please provide the following required information";

		if (courseid.Trim() == "")
			ucError.ErrorMessage = "Please select a course and then save the Plan";

		return (!ucError.IsError);
	}
	protected void gvCoursePlanner_ItemDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{

			DataRowView drvHoliday = (DataRowView)e.Row.DataItem;

			ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
			if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

			Label lblHoliday = (Label)e.Row.FindControl("lblHoliday");
			Label lblSundayHoliday = (Label)e.Row.FindControl("lblSundayholidayyn");
			Label lblDate = (Label)e.Row.FindControl("lblSplitAM");
			Label lblSplitam = (Label)e.Row.FindControl("lblFacultyAM");
			Label lblSplitpm = (Label)e.Row.FindControl("lblFacultyPM");
			if (lblHoliday.Text == "H" || lblSundayHoliday.Text=="H")
			{
				lblSplitam.Text = "Holiday";
				lblSplitpm.Text = "Holiday";
				ed.Visible = false;
				e.Row.BackColor = Color.FromName("silver");

			}


			if (Convert.ToDateTime(lblDate.Text).AddDays(1) < System.DateTime.Now)
			{
				ed.Visible = false;
			}

			UserControlFacultyCode ucFacultyAM = (UserControlFacultyCode)e.Row.FindControl("ucFacultyAMEdit");
			DataRowView drvFacultyAM = (DataRowView)e.Row.DataItem;
			if (ucFacultyAM != null) ucFacultyAM.SelectedFacultyCode = drvFacultyAM["FLDFACULTYAM"].ToString();

			UserControlFacultyCode ucFacultyPM = (UserControlFacultyCode)e.Row.FindControl("ucFacultyPMEdit");
			DataRowView drvFacultyPM = (DataRowView)e.Row.DataItem;
            if (ucFacultyPM != null) ucFacultyPM.SelectedFacultyCode = drvFacultyPM["FLDFACULTYPM"].ToString();

		}
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
	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		BindData();
		
	}
	protected void ShowExcel()
	{
		string[] alColumns = { "FLDBATCH","FLDDATE", "FLDFACULTYNAMEAM", "FLDFACULTYNAMEPM" };
		string[] alCaptions = { "Batch No", "AM", "PM" };

		DataSet ds = PhoenixCrewCoursePlanner.ListCoursePlanner(Convert.ToInt32(Request.QueryString["includeholiday"]), Convert.ToInt32(Request.QueryString["batchid"]), Convert.ToInt32(Request.QueryString["excludesunday"]));

		Response.AddHeader("Content-Disposition", "attachment; filename=FacultyPlanner.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3>Faculty Planner</h3></td>");
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
