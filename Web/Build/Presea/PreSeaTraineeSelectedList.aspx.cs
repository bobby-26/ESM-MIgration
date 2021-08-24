using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PreSea;
using System.Web.UI;

public partial class PreSeaTraineeSelectedList : PhoenixBasePage
{

	string strBatchid = string.Empty;
	string strSectionid = string.Empty;
	string strPracticalid = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			strBatchid = Request.QueryString["batchid"];
			strSectionid = Request.QueryString["sectionid"];
			strPracticalid = Request.QueryString["practicalid"];
			if (!IsPostBack)
			{
				
				PhoenixToolbar toolbar = new PhoenixToolbar();
				toolbar.AddButton("Save", "SAVE");
				PreSeaTraineeMenu.MenuList = toolbar.Show();
	
			}
			BindData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void PreSeaTraineeMenu_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("SAVE"))
			{
				if (gvTraineeList.Rows.Count == 1 && gvTraineeList.Rows[0].Cells.Count == 1)
				{
					ucError.ErrorMessage = "No Trainees found to save";
					ucError.Visible = true;
					return;
				}
				string csvTraineeid = string.Empty;
				foreach (GridViewRow gv in gvTraineeList.Rows)
				{
					CheckBox ck = (CheckBox)gv.FindControl("chkSelect");
					if (ck.Checked && ck.Enabled)
					{
						csvTraineeid += ((Label)gv.FindControl("lblTraineeId")).Text + ",";
					}
				}
				if (!IsValidateRequest(csvTraineeid.TrimEnd(',')))
				{
					ucError.Visible = true;
					return;
				}
				if (strPracticalid == null)
				{

					PhoenixPreSeaTrainee.UpdatePreSeaTraineeSection(
						PhoenixSecurityContext.CurrentSecurityContext.UserCode,
						int.Parse(strSectionid),
						 csvTraineeid,
						 int.Parse(strBatchid)
					  );
				}
				else
				{
					PhoenixPreSeaTrainee.UpdatePreSeaTraineePractical(
						PhoenixSecurityContext.CurrentSecurityContext.UserCode,
						int.Parse(strSectionid),
						 csvTraineeid,
						 int.Parse(strBatchid),
						 int.Parse(strPracticalid)
					  );
				}
				String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1',null, 'keeppopupopen');");
				ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupopen, true);
				
				BindData();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	
	private bool IsValidateRequest(string csvcourse)
	{
		
		ucError.HeaderMessage = "Please provide the following required information";

		if (string.IsNullOrEmpty(csvcourse))
			ucError.ErrorMessage = "Select atleast one or more Trainees.";
	

		return (!ucError.IsError);
	}
	protected void BindData()
	{
		try
		{
			DataSet ds = ds = new DataSet();
			if (strPracticalid == null)
			{
				 ds = PhoenixPreSeaTrainee.ListPreSeaTrainee(int.Parse(strBatchid), null);
			}
			else
			{
				ds = PhoenixPreSeaTrainee.ListPreSeaTrainee(int.Parse(strBatchid), General.GetNullableInteger(strSectionid));
			}
			if (ds.Tables[0].Rows.Count > 0)
			{
				gvTraineeList.DataSource = ds.Tables[0];
				gvTraineeList.DataBind();
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvTraineeList);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvTraineeList_RowDataBound(object sender, GridViewRowEventArgs e)
	{

		if (e.Row.RowType == DataControlRowType.DataRow)
		{
		
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
