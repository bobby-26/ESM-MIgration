using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaTraineeMapping : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			
			if (!IsPostBack)
			{
				ViewState["confirmedyn"] = "";
				ViewState["batchid"] = "";
				if (Request.QueryString["batchid"] != null)
				{
					ddlBatch.SelectedBatch = Request.QueryString["batchid"];
					ViewState["batchid"] = Request.QueryString["batchid"];
				}
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;
				PhoenixToolbar toolbar = new PhoenixToolbar();
				toolbar.AddButton("Roll No", "ROLLNOMAPPING");
				toolbar.AddButton("Section", "SECTIONMAPPING");
				toolbar.AddButton("Practical", "PRACTICALMAPPING");
				PreSeaQuery.AccessRights = this.ViewState;
				PreSeaQuery.MenuList = toolbar.Show();
				PreSeaQuery.SelectedMenuIndex = 0;
				PhoenixToolbar toolbarmain = new PhoenixToolbar();
				toolbarmain.AddButton("Confirm", "CONFIRM");
				PreSeaSub.AccessRights = this.ViewState;
				
				if (General.GetNullableInteger(Request.QueryString["batchid"]) != null)
				{
					DataTable dt = PhoenixPreSeaBatchManager.ListBatchDetails(General.GetNullableInteger(Request.QueryString["batchid"]));

					if (dt.Rows.Count > 0)
					{
						DataRow dr = dt.Rows[0];
						ViewState["confirmedyn"] = dr["FLDISFINALISED"].ToString();
						if (ViewState["confirmedyn"].ToString() == "1")
						{
							PreSeaSub.Visible = false;
						}
						else
						{
							PreSeaSub.Visible = true;
							PreSeaSub.MenuList = toolbarmain.Show();
						}
					}
					else
					{
						ViewState["confirmedyn"] = "";

					}
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
	protected void gvCB_RowCommand(object sender, GridViewCommandEventArgs e)
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
	protected void PreSeaQuery_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

		if (dce.CommandName.ToUpper().Equals("SECTIONMAPPING"))
		{
			Response.Redirect("..\\PreSea\\PreSeaTraineeSection.aspx?batchid=" + General.GetNullableInteger(ddlBatch.SelectedBatch), false);
		}
		if (dce.CommandName.ToUpper().Equals("PRACTICALMAPPING"))
		{
			Response.Redirect("..\\PreSea\\PreSeaTraineePractical.aspx?batchid=" + General.GetNullableInteger(ddlBatch.SelectedBatch), false);
		}
		
	}
	protected void PreSeaSub_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("CONFIRM"))
			{
				if (!IsValidBatchConfirm())
				{
					ucError.Visible = true;
					return;
				}
				ucConfirm.Visible = true;
				ucConfirm.Text = "You wont be able to make any more changes to the batch.Click on Ok to continue Or click on Cancel to continue editing?";
            
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void ConfirmBatch(object sender, EventArgs e)
	{
		try
		{
			UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
			if (ucCM.confirmboxvalue == 1)
			{

				PhoenixPreSeaTrainee.ConfirmPreSeaTraineeBatch(Convert.ToInt32(ddlBatch.SelectedBatch), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
				OnBatchChanged(null, null);
				BindData();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private bool IsValidBatchConfirm()
	{
		ucError.HeaderMessage = "Please provide the following required information";

		if(General.GetNullableInteger(ddlBatch.SelectedBatch)==null)
			ucError.ErrorMessage = "Batch is required.";
		
		return (!ucError.IsError);
	}
	protected void OnBatchChanged(object sender, EventArgs e)
	{
		ViewState["batchid"] = ddlBatch.SelectedBatch;
		
		PhoenixToolbar toolbarmain = new PhoenixToolbar();
		toolbarmain.AddButton("Confirm", "CONFIRM");
		PreSeaSub.AccessRights = this.ViewState;
		PreSeaSub.MenuList = toolbarmain.Show();
		if (General.GetNullableInteger(ddlBatch.SelectedBatch) != null)
		{
			DataTable dt = PhoenixPreSeaBatchManager.ListBatchDetails(General.GetNullableInteger(ddlBatch.SelectedBatch));
			
			if (dt.Rows.Count > 0)
			{
				DataRow dr = dt.Rows[0];
				ViewState["confirmedyn"] = dr["FLDISFINALISED"].ToString();

				if (ViewState["confirmedyn"].ToString() == "1")
				{
					PreSeaSub.Visible = false;
				}
				else
				{
					PreSeaSub.Visible = true;
				}
			}
			else
			{
				ViewState["confirmedyn"] = "";
				
			}
		}
		BindData();
	}
	private void BindData()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		string[] alColumns = { "FLDROWID", "FLDROLLNUMBER", "FLDTRAINEENAME", "FLDIMUENTRANCEROLLNO", "FLDCREATEDDATE" };
		string[] alCaptions = { "Sl no", "Roll Number", "Trainee Name", "IMU Entrance roll no", "Created date" };

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;

		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());



		DataSet ds = PhoenixPreSeaTrainee.PreSeaTraineeMappingSearch(General.GetNullableInteger(ViewState["batchid"].ToString()), 
																	(int)ViewState["PAGENUMBER"]
																   , General.ShowRecords(null)
																   , ref iRowCount
																   , ref iTotalPageCount);

		General.SetPrintOptions("gvCB", "Roll no Mapping", alCaptions, alColumns, ds);
		if (ds.Tables[0].Rows.Count > 0)
		{

			gvCB.DataSource = ds.Tables[0];
			gvCB.DataBind();
		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvCB);
		}
		ViewState["ROWCOUNT"] = iRowCount;
		ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
		SetPageNavigator();
	}

	protected void cmdHiddenSubmit_Click(object sender,EventArgs e)
	{
		BindData();
		SetPageNavigator();
	}
	protected void gvCB_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindData();
		SetPageNavigator();
	}


	protected void gvCB_RowEditing(object sender, GridViewEditEventArgs de)
	{
		GridView _gridView = (GridView)sender;

		_gridView.EditIndex = de.NewEditIndex;
		_gridView.SelectedIndex = de.NewEditIndex;

		BindData();
	}
	protected void gvCB_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;
			string preseatraineeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblPreSeaTraineeIdEdit")).Text;
			string rollnumber = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRollnumberEdit")).Text;
			string imurollno = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtIMURollnoEdit")).Text;
			string TraineeId = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtEmployeeIdAdd")).Text;
			string imuapplicableyn = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblIsIMUApplicable")).Text;


			if (!IsValidMapping(TraineeId, imuapplicableyn, imurollno))
			{
				ucError.Visible = true;
				return;
			}
			if (preseatraineeid == "")
			{
				PhoenixPreSeaTrainee.InsertPreSeaTrainee(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
														Convert.ToInt64(TraineeId),
														General.GetNullableString(rollnumber),
														Convert.ToInt32(ddlBatch.SelectedBatch),
														General.GetNullableString(imurollno)
														);
				_gridView.EditIndex = -1;
				BindData();
			}
			else
			{
				PhoenixPreSeaTrainee.UpdatePreSeaTrainee(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
														Convert.ToInt64(preseatraineeid),
														Convert.ToInt64(TraineeId),
														General.GetNullableString(rollnumber),
														Convert.ToInt32(ddlBatch.SelectedBatch),
														General.GetNullableString(imurollno)
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
	protected void gvCB_RowDataBound(Object sender, GridViewRowEventArgs e)
	{
		try
		{
		DataRowView drv = (DataRowView)e.Row.DataItem;
		if (e.Row.RowType == DataControlRowType.Header)
		{
			if (ViewState["confirmedyn"].ToString() == "1")
			{
				e.Row.Cells[6].Visible = false;
			
			}
			
		}
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
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

				HtmlGenericControl gc = (HtmlGenericControl)e.Row.FindControl("spnPickListEmployeeAdd");
				ImageButton emp = (ImageButton)e.Row.FindControl("btnShowEmployeeAdd");
				if (emp != null) emp.Attributes.Add("onclick", "showPickList('" + gc.ClientID + "', 'codehelp1', '', '../PreSea/PreSeaTraineeQueryFilter.aspx?batchid=" + ddlBatch.SelectedBatch + "', false); return false;");

				ImageButton imgHandover = (ImageButton)e.Row.FindControl("cmdHandover");
				Label lbltraineeid = (Label)e.Row.FindControl("lblTraineeid");
				if (lbltraineeid != null && imgHandover!=null)
				{
					if (lbltraineeid.Text == "")
					{
						imgHandover.Visible = false;
					}

					else
					{
						imgHandover.Visible = true;

					}

				}
				if (imgHandover != null)
				{
					imgHandover.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'PreSeaAdmissionHandOver.aspx?traineeid=" + lbltraineeid.Text
						+ "&batchid=" + ddlBatch.SelectedBatch + "');return false;");
				}
				
			}
			
			if (ViewState["confirmedyn"].ToString() == "1")
			{
				e.Row.Cells[6].Visible = false;
				
			}
		
		}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
		
	}
	protected void gvCB_RowDeleting(object sender, GridViewDeleteEventArgs de)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = de.RowIndex;
			string preseatraineeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblPreSeaTraineeId")).Text;
			PhoenixPreSeaTrainee.DeletePreSeaTrainee(PhoenixSecurityContext.CurrentSecurityContext.UserCode,Convert.ToInt64(preseatraineeid));
			_gridView.EditIndex = -1;
			BindData();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private bool IsValidMapping(string EmployeeId, string imuapplicableyn,string imurollno)
	{

		ucError.HeaderMessage = "Please provide the following required information";

		if (!General.GetNullableInteger(EmployeeId).HasValue)
			ucError.ErrorMessage = "Trainee Name is required.";

		if (imuapplicableyn == "1")
		{
			if (General.GetNullableString(imurollno)==null)
				ucError.ErrorMessage = "IMU Entrance roll no is required.";
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
			gvCB.SelectedIndex = -1;
			gvCB.EditIndex = -1;
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
