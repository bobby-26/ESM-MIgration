using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewCourseRegistration : PhoenixBasePage
{


	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddImageButton("../Crew/CrewCourseRegistration.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbar.AddImageLink("javascript:CallPrint('gvCrewRegistration')", "Print Grid", "icon_print.png", "PRINT");
			toolbar.AddImageLink("javascript:parent.Openpopup('codehelp1','','../Crew/CrewCourseEmployeeList.aspx?typelist=2&batchid="+Request.QueryString["batchid"]+"&type=" 
							+ PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 153, "NML") + "')",
							"Add Seafarer to Registration List", "add.png", "ADDNOMINATION");
			MenuCrewRegistration.AccessRights = this.ViewState;
			MenuCrewRegistration.MenuList = toolbar.Show();
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
			MenuCrewRegistration.SetTrigger(pnlCrewRegistration);

			if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;
				if (Request.QueryString["batchid"] != null)
				{
					EditBatchDetails(Convert.ToInt32(Request.QueryString["batchid"]));
				}
			
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
		try
		{
			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void EditBatchDetails(int batchid)
	{
		try
		{

			DataSet ds = PhoenixRegistersBatch.EditBatch(batchid);
			if (ds.Tables[0].Rows.Count > 0)
			{

				txtCourseName.Text = ds.Tables[0].Rows[0]["FLDCOURSENAME"].ToString();
				txtBatchNo.Text = ds.Tables[0].Rows[0]["FLDBATCH"].ToString();
				txtFromDate.Text =Convert.ToDateTime( ds.Tables[0].Rows[0]["FLDFROMDATE"]).ToString("dd/MMM/yyyy");
				txtToDate.Text = Convert.ToDateTime( ds.Tables[0].Rows[0]["FLDTODATE"]).ToString("dd/MMM/yyyy");
			}
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		DataSet ds = new DataSet();
		string[] alColumns = { "FLDNAME", "FLDRANK", "FLDPRIORITY", "FLDAPPROVEDDATE", "FLDBATCHNO", };
		string[] alCaptions = { "Employee Name", "Rank", "Priority", "Approved Date", "Batch No" };
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


		 ds = PhoenixCrewCourseEnrollment.CrewCourseEnrollmentSearch(Convert.ToInt32(Filter.CurrentCourseSelection),
		   null, General.GetNullableInteger(Request.QueryString["batchid"]),
		   General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 153, "NML")),
		   1
		   , sortexpression, sortdirection,
		   (int)ViewState["PAGENUMBER"],
		  iRowCount,
		   ref iRowCount,
		   ref iTotalPageCount, null, null, null, null, null);

		 Response.AddHeader("Content-Disposition", "attachment; filename=CrewCourseRegistration.xls");
		 Response.ContentType = "application/vnd.msexcel";
		 Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		 Response.Write("<tr>");
		 Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		 Response.Write("<td><h3>Registration List</h3></td>");
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

	protected void CrewRegistration_TabStripCommand(object sender, EventArgs e)
	{
		try
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

		string[] alColumns = { "FLDNAME", "FLDRANK", "FLDDATEOFBIRTH", "FLDFILENO", "FLDCDCNO","FLDPASSPORTNO","FLDINDOSNO","FLDSTATUS"};
		string[] alCaptions = { "Employee Name", "Rank", "Date of Birth", "File No", "CDC No","Passport No","INDOS No","Status" };

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		DataSet ds = PhoenixCrewCourseEnrollment.CrewCourseEnrollmentSearch(Convert.ToInt32(Filter.CurrentCourseSelection),
		   null, General.GetNullableInteger(Request.QueryString["batchid"]),
		   General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 153, "NML")),
		   1
		   , sortexpression, sortdirection,
		   (int)ViewState["PAGENUMBER"],
		  General.ShowRecords(null),
		   ref iRowCount,
		   ref iTotalPageCount, null, null, null, null, null);


		General.SetPrintOptions("gvCrewRegistration", "Registration List", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvCrewRegistration.DataSource = ds;
			gvCrewRegistration.DataBind();

		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvCrewRegistration);
		}

		ViewState["ROWCOUNT"] = iRowCount;
		ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

		//DropDownList ddlemployee = (DropDownList)gvCrewRegistration.FooterRow.FindControl("ddlEmployee");
		//ddlemployee.DataSource = PhoenixCrewCourseEnrollment.CrewEnrollmentEmployeeSearch(General.GetNullableInteger(Session["COURSEID"].ToString())
		//                                     , 1
		//                                     , sortexpression, sortdirection
		//                                     , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
		//                                     , ref iRowCount, ref iTotalPageCount);
		//ddlemployee.DataTextField = "FLDEMPLOYEENAME";
		//ddlemployee.DataValueField = "FLDEMPLOYEEID";
		//ddlemployee.DataBind();
	}

	protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;

		ViewState["SORTEXPRESSION"] = ib.CommandName;
		ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

		BindData();
		SetPageNavigator();
	}

	protected void gvCrewRegistration_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;

			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
			if (e.CommandName.ToUpper().Equals("DELETE"))
			{
				PhoenixCrewCourseEnrollment.DeleteCrewCourseEnrollment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
							new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblEnrollmentId")).Text),1);
			}
			else if (e.CommandName.ToString().ToUpper() == "ADD")
			{


				_gridView.EditIndex = -1;
				string empid = ((DropDownList)_gridView.FooterRow.FindControl("ddlEmployee")).SelectedValue;


				if (!IsValidEmployee(empid))
				{
					ucError.Visible = true;
					return;
				}
				//int iMessageCode = 0;
				//string iMessageText = "";

				//PhoenixCrewCourseEnrollment.InsertCrewCourseEnrollment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				//    Convert.ToInt32(Session["COURSEID"].ToString()), empid,
				//    General.GetNullableInteger(Request.QueryString["type"].ToString()), 1,
				//    ucConfirm.confirmboxvalue, ref iMessageCode, ref iMessageText,
				//    General.GetNullableInteger(Request.QueryString["typelist"].ToString()),
				//    General.GetNullableInteger(ucBatch.SelectedBatch));

				//if (iMessageCode == 1)
				//    throw new ApplicationException(iMessageText);
				//Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

				BindData();
				SetPageNavigator();
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
	private bool IsValidEmployee(string employeeid)
	{
		Int16 resultInt;

		ucError.HeaderMessage = "Please provide the following required information";

		if (!Int16.TryParse(employeeid, out resultInt))
			ucError.ErrorMessage = "Employee is required";

		return (!ucError.IsError);
	}
	protected void gvCrewRegistration_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		BindData();
		SetPageNavigator();
	}
	
	protected void gvCrewRegistration_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
				 && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
			{
				DataRowView drv = (DataRowView)e.Row.DataItem;
				ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
				if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
				
				Label l = (Label)e.Row.FindControl("lblEnrollmentId");

				LinkButton lbr = (LinkButton)e.Row.FindControl("lnkemployee");

				if (drv["FLDNEWAPP"].ToString() == "1")
				{
					lbr.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','CrewNewApplicantPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "'); return false;");
				}
				else
				{
					lbr.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','CrewPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "'); return false;");
				}
			}
		}


	}
	protected void gvCrewRegistration_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.SelectedIndex = e.NewSelectedIndex;
		_gridView.EditIndex = -1;

	}
	protected void gvCrewRegistration_Sorting(object sender, GridViewSortEventArgs se)
	{
		gvCrewRegistration.SelectedIndex = -1;
		gvCrewRegistration.EditIndex = -1;

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
		gvCrewRegistration.SelectedIndex = -1;
		gvCrewRegistration.EditIndex = -1;
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
}
