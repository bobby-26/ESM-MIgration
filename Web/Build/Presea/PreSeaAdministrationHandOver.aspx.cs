using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaAdministrationHandOver : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddImageButton("../PreSea/PreSeaAdministrationHandOver.aspx?batchid=" + Request.QueryString["batchid"] + "&traineeid=" + Request.QueryString["traineeid"], "Export to Excel", "icon_xls.png", "Excel");
			toolbar.AddImageLink("javascript:CallPrint('gvAdministrationHandOver')", "Print Grid", "icon_print.png", "PRINT");
			MenuPreSeaAdministrationHandOver.AccessRights = this.ViewState;
			MenuPreSeaAdministrationHandOver.MenuList = toolbar.Show();
			if (!IsPostBack)
			{

				if (Request.QueryString["batchid"] != null)
				{
					ddlBatch.SelectedBatch = Request.QueryString["batchid"];
					ddlBatch.Enabled = false;
				}
				if (Request.QueryString["traineeid"] != null)
				{
					DataTable dt = PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantList(General.GetNullableInteger(Request.QueryString["traineeid"]));
					if (dt.Rows.Count > 0)
					{
						txtTrainee.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString();
					}
				}
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;

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
	protected void PreSeaAdministrationHandOver_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

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

	protected void gvAdministrationHandOver_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;

			GridView _gridView = (GridView)sender;

			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

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

		string[] alColumns = { "FLDQUICKNAME", "FLDAdministrationYN", "FLDAdministrationREMARK", "FLDAdministrationCHECKBY" };
		string[] alCaptions = { "Documents", "Administration", "Remarks", "Checked by" };

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;

		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());



		DataSet ds = PhoenixPreSeaHandOver.PreSeaHandOverSearch(General.GetNullableInteger(Request.QueryString["batchid"].ToString())
																  , Convert.ToInt64(Request.QueryString["traineeid"].ToString())
																  , sortexpression
																  , sortdirection
																  , (int)ViewState["PAGENUMBER"]
																  , General.ShowRecords(null)
																  , ref iRowCount
																  , ref iTotalPageCount);

		General.SetPrintOptions("gvAdministrationHandOver", "Administration Handover", alCaptions, alColumns, ds);
		if (ds.Tables[0].Rows.Count > 0)
		{

			gvAdministrationHandOver.DataSource = ds.Tables[0];
			gvAdministrationHandOver.DataBind();
		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvAdministrationHandOver);
		}
		ViewState["ROWCOUNT"] = iRowCount;
		ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
		SetPageNavigator();
	}

	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;


		string[] alColumns = { "FLDQUICKNAME", "FLDAdministrationYN", "FLDAdministrationREMARK", "FLDAdministrationCHECKBY" };
		string[] alCaptions = { "Documents", "Administration", "Remarks", "Checked by" };

		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

		DataSet ds = PhoenixPreSeaHandOver.PreSeaHandOverSearch(General.GetNullableInteger(Request.QueryString["batchid"].ToString())
																  , Convert.ToInt64(Request.QueryString["traineeid"].ToString())
																  , sortexpression
																  , sortdirection
																  , (int)ViewState["PAGENUMBER"]
																  , General.ShowRecords(null)
																  , ref iRowCount
																  , ref iTotalPageCount);

		Response.AddHeader("Content-Disposition", "attachment; filename=PreSeaAdministrationHandOver.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3>Administration Handover</h3></td>");
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
	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		BindData();
		SetPageNavigator();
	}
	protected void gvAdministrationHandOver_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindData();
		SetPageNavigator();
	}


	protected void gvAdministrationHandOver_RowEditing(object sender, GridViewEditEventArgs de)
	{
		GridView _gridView = (GridView)sender;

		_gridView.EditIndex = de.NewEditIndex;
		_gridView.SelectedIndex = de.NewEditIndex;

		BindData();
	}
	protected void gvAdministrationHandOver_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;
			string handoverid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblHandOverIdEdit")).Text;
			string documentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDocumentidedit")).Text;
			string remarks = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAdministrationRemarksEdit")).Text;
			int administration = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkAdministrationEdit")).Checked) ? 1 : 0;
			if (!IsValidHandOver(remarks))
			{
				ucError.Visible = true;
				return;
			}

			if (handoverid == "")
			{
				PhoenixPreSeaHandOver.InsertPreSeaAdministrationHandOver(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
														Convert.ToInt32(Request.QueryString["batchid"]),
														Convert.ToInt64(Request.QueryString["traineeid"]),
														Convert.ToInt32(documentid),
														administration,
														General.GetNullableString(remarks)
														);
				_gridView.EditIndex = -1;
				BindData();
			}
			else
			{
				PhoenixPreSeaHandOver.UpdatePreSeaAdministrationHandOver(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
														Convert.ToInt32(handoverid),
														Convert.ToInt32(Request.QueryString["batchid"]),
														Convert.ToInt64(Request.QueryString["traineeid"]),
														Convert.ToInt32(documentid),
														administration,
														General.GetNullableString(remarks)
														);
				_gridView.EditIndex = -1;
				BindData();
			}

			


		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvAdministrationHandOver_RowDataBound(Object sender, GridViewRowEventArgs e)
	{
		DataRowView drv = (DataRowView)e.Row.DataItem;
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
			if (del != null)
			{
				del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
				del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
			}

			ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
			if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

			ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
			if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

			ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
			if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);



		}

	}
	
	private bool IsValidHandOver(string remarks)
	{

		ucError.HeaderMessage = "Please provide the following required information";

		if (General.GetNullableString(remarks) == null)
		{
			ucError.ErrorMessage = "Remarks is required.";
		}

		return (!ucError.IsError);
	}
	protected void PreSeaBatch_TabStripCommand(object sender, EventArgs e)
	{
		//Filter.CurrentPreSeaNewApplicantSelection = null;
		//Session["REFRESHFLAG"] = null;
		//Response.Redirect("..\\PreSea\\PreSeaTraineeQueryFilter.aspx", false);
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
			gvAdministrationHandOver.SelectedIndex = -1;
			gvAdministrationHandOver.EditIndex = -1;
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
}
