using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersFlagExperience : PhoenixBasePage
{
	protected override void Render(HtmlTextWriter writer)
	{
		foreach (GridViewRow r in gvFlagExperience.Rows)
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
			toolbar.AddImageButton("../Registers/RegistersFlagExperience.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
			toolbar.AddImageLink("javascript:CallPrint('gvFlagExperience')", "Print Grid", "icon_print.png", "PRINT");	
			toolbar.AddImageButton("../Registers/RegistersFlagExperience.aspx", "Find", "search.png", "FIND");
            MenuRegistersFlagExperience.AccessRights = this.ViewState;
			MenuRegistersFlagExperience.MenuList = toolbar.Show();
            
		
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
			string[] alColumns = { "FLDFLAGNAME", "FLDRANKNAME", "FLDMINSEATIMEEXP", "FLDTANKEREXP" };
			string[] alCaptions = { "Flag", "Rank", "Minimum Sea Time Exp", "Tanker Exp" };
			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;

			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

			DataSet ds = PhoenixRegistersFlagExperience.FlagExperienceSearch(
						General.GetNullableInteger(ucFlag.SelectedFlag),General.GetNullableInteger(ucRank.SelectedRank), null, null
						, sortexpression, sortdirection
						, (int)ViewState["PAGENUMBER"]
						, General.ShowRecords(null)
						, ref iRowCount
						, ref iTotalPageCount);
			General.SetPrintOptions("gvFlagExperience", "Flag Experience", alCaptions, alColumns, ds);
			if (ds.Tables[0].Rows.Count > 0)
			{
				gvFlagExperience.DataSource = ds;
				gvFlagExperience.DataBind();
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvFlagExperience);
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

	protected void RegistersFlagExperience_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("EXCEL"))
			{
				int iRowCount = 0;
				int iTotalPageCount = 0;
				string[] alColumns = { "FLDFLAGNAME", "FLDRANKNAME", "FLDMINSEATIMEEXP", "FLDTANKEREXP" };
				string[] alCaptions = { "Flag", "Rank", "Minimum Sea Time Exp", "Tanker Exp" };
				string sortexpression;
				int? sortdirection = null;

				sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

				if (ViewState["SORTDIRECTION"] != null)
					sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
				if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
					iRowCount = 10;
				else
					iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
				DataSet ds = PhoenixRegistersFlagExperience.FlagExperienceSearch(
						General.GetNullableInteger(ucFlag.SelectedFlag), General.GetNullableInteger(ucRank.SelectedRank), null, null
						, sortexpression, sortdirection
						, (int)ViewState["PAGENUMBER"]
						, iRowCount
						, ref iRowCount
						, ref iTotalPageCount);

				if (ds.Tables.Count > 0)
					General.ShowExcel("Flag Experience", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
			}
			
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvFlagExperience_CancelingEdit(object sender, GridViewCancelEditEventArgs ce)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
	}

	protected void gvFlagExperience_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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

	protected void gvFlagExperience_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		if (e.CommandName.ToUpper().Equals("SORT"))
			return;
		GridView _gridView = (GridView)sender;
		int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
		try
		{
			
			if (e.CommandName.ToString().ToUpper() == "ADD")
			{


				string flag = ((UserControlFlag)_gridView.FooterRow.FindControl("ucFlagAdd")).SelectedFlag;
				string rank = ((UserControlRank)_gridView.FooterRow.FindControl("ucRankAdd")).SelectedRank;
				string seaexp = ((TextBox)_gridView.FooterRow.FindControl("txtMinimumSeaTimeExpAdd")).Text;
				string tankexp = ((TextBox)_gridView.FooterRow.FindControl("txtTankerExpAdd")).Text;

				if (!IsValidFlagExperience(flag, rank, seaexp, tankexp))
				{
					ucError.Visible = true;
					return;
				}


				PhoenixRegistersFlagExperience.InsertFlagExperience(
					  PhoenixSecurityContext.CurrentSecurityContext.UserCode
					, Convert.ToInt32(flag)
					, Convert.ToInt32(rank)
					, General.GetNullableInteger(seaexp)
					, General.GetNullableInteger(tankexp)
					);

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
	private bool IsValidFlagExperience(string flag, string rank, string seaexp, string tankerexp)
	{
		Int16 resultInt;

		ucError.HeaderMessage = "Please provide the following required information";

		if (!Int16.TryParse(flag, out resultInt))
			ucError.ErrorMessage = "Flag  is required";

		if (!Int16.TryParse(rank, out resultInt))
			ucError.ErrorMessage = "Rank is required";

		if (string.IsNullOrEmpty(seaexp) && string.IsNullOrEmpty(tankerexp))
		{
			ucError.ErrorMessage = "Either Sea Time exp or Tanker exp  is required";
		}
		else if ((!Int16.TryParse(seaexp, out resultInt) || (!Int16.TryParse(tankerexp, out resultInt))))
		{
		    ucError.ErrorMessage = "Not a valid month";
		}
		

		return (!ucError.IsError);
	}


	protected void gvFlagExperience_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;
			string flagexpid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblflagexperienceid")).Text;

			PhoenixRegistersFlagExperience.DeleteFlagExperience(
				Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
				, Convert.ToInt32(flagexpid));
			_gridView.EditIndex = -1;
			_gridView.SelectedIndex = -1;
			BindData();
			SetPageNavigator();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;

		}

	}

	protected void gvFlagExperience_RowEditing(object sender, GridViewEditEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;

			_gridView.EditIndex = e.NewEditIndex;
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
	protected void gvFlagExperience_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.SelectedIndex = e.NewSelectedIndex;
		_gridView.EditIndex = -1;
		BindData();
		SetPageNavigator();
		

	}
	protected void gvFlagExperience_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState != DataControlRowState.Edit && e.Row.RowState != (DataControlRowState.Alternate | DataControlRowState.Edit))
		{

			LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
			string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
			e.Row.Attributes["onclick"] = _jsDouble;

		}
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
			   && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
			{
				ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
			}
			else
			{
				e.Row.Attributes["onclick"] = "";
			}
			UserControlFlag ucFlag = (UserControlFlag)e.Row.FindControl("ucFlagEdit");
			DataRowView drvFlag = (DataRowView)e.Row.DataItem;
			if (ucFlag != null) ucFlag.SelectedFlag = drvFlag["FLDFLAGID"].ToString();
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
	}

	protected void gvFlagExperience_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = e.RowIndex;
		try
		{
			string flagexpid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblflagexperienceidEdit")).Text;
			string flag = ((UserControlFlag)_gridView.Rows[nCurrentRow].FindControl("ucFlagAdd")).SelectedFlag;
			string rank = ((UserControlRank)_gridView.Rows[nCurrentRow].FindControl("ucRankEdit")).SelectedRank;
			string seaexp = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtMinimumSeaTimeExpEdit")).Text;
			string tankexp = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTankerExpEdit")).Text;

			if (!IsValidFlagExperience(flag, rank, seaexp, tankexp))
			{
				ucError.Visible = true;
				return;
			}

			PhoenixRegistersFlagExperience.UpdateFlagExperience(
				PhoenixSecurityContext.CurrentSecurityContext.UserCode
				, Convert.ToInt32(flagexpid)
				, Convert.ToInt32(flag)
				, Convert.ToInt32(rank)
				, General.GetNullableInteger(seaexp)
				, General.GetNullableInteger(tankexp)
			   );
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
		_gridView.EditIndex = -1;
		SetPageNavigator();
		BindData();
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
			gvFlagExperience.SelectedIndex = -1;
			gvFlagExperience.EditIndex = -1;
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

	protected void cmdSort_Click(object sender, EventArgs e)
	{

		ImageButton ib = (ImageButton)sender;
		try
		{
			ViewState["SORTEXPRESSION"] = ib.CommandName;
			ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
			BindData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvFlagExperience_RowCreated(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow
		   && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
		   && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
		{
			e.Row.TabIndex = -1;
			e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvFlagExperience, "Edit$" + e.Row.RowIndex.ToString(), false);
		}

	}
	protected void gvFlagExperience_Sorting(object sender, GridViewSortEventArgs se)
	{
		gvFlagExperience.EditIndex = -1;
		gvFlagExperience.SelectedIndex = -1;
		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
	}
}
