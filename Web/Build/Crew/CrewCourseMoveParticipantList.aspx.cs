using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewCourseMoveParticipantList : PhoenixBasePage
{

	string strCurrentBatchid = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			strCurrentBatchid = Request.QueryString["currentbatchid"];

			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddButton("Nomination to Participant", "NOMINATIONTOPARTICIPANT");
			toolbar.AddButton("Participant to Participant", "PARTICIPANTTOPARTICIPANT");
			CrewMenu.AccessRights = this.ViewState;
			CrewMenu.MenuList = toolbar.Show();

			PhoenixToolbar toolbardown = new PhoenixToolbar();
			toolbardown.AddButton("Save", "REQUEST");
			CrewCourseMoveParticipant.AccessRights = this.ViewState;
			CrewCourseMoveParticipant.MenuList = toolbardown.Show();

			if (!IsPostBack)
			{
				ddlBatch.DataSource = PhoenixCrewCourseEnrollment.CrewCourseEnrollmentBatchList(null,General.GetNullableInteger(strCurrentBatchid) );
				ddlBatch.DataTextField = "FLDBATCH";
				ddlBatch.DataValueField = "FLDBATCHID";
				ddlBatch.DataBind();
				CrewMenu.SelectedMenuIndex = 1;
				

			}
			BindData();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("NOMINATIONTOPARTICIPANT"))
			{
				Response.Redirect("..\\Crew\\CrewCourseAddToParticipant.aspx?batchid=" + strCurrentBatchid, false);
				return;

			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void CrewCourseMoveParticipantList_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("REQUEST"))
			{
				string csvEmployee = string.Empty;
				foreach (GridViewRow gv in gvParticipantList.Rows)
				{
					CheckBox ck = (CheckBox)gv.FindControl("chkSelect");
					if (ck.Checked)
					{
						csvEmployee += ((Label)gv.FindControl("lblEmployeeId")).Text + ",";
					}
				}
				if (!IsValidateRequest(csvEmployee.TrimEnd(',')))
				{
					ucError.Visible = true;
					return;
				}
				if (gvParticipantList.Rows.Count == 1 && gvParticipantList.Rows[0].Cells.Count == 1)
				{
					ucError.ErrorMessage = "No Employee found to move";
					ucError.Visible = true;
					return;
				}
				ucConfirm.Visible = true;
				ucConfirm.Text = "Are you sure you want to move the Seafarer ?";
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void InitiateMovement(object sender, EventArgs e)
	{
		try
		{
			UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
			if (ucCM.confirmboxvalue == 1)
			{
				string csvEmployee = string.Empty;
				foreach (GridViewRow gv in gvParticipantList.Rows)
				{
					CheckBox ck = (CheckBox)gv.FindControl("chkSelect");
					if (ck.Checked)
					{
						csvEmployee += ((Label)gv.FindControl("lblEmployeeId")).Text + ",";
					}
				}

				PhoenixCrewCourseEnrollment.UpdateCrewCourseEnrollmentBatch(csvEmployee.ToString(),
					General.GetNullableInteger(ddlBatch.SelectedValue), Convert.ToInt32(strCurrentBatchid));

				ucStatus.Text = "Successfully moved";
				BindData();
				
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void BindData()
	{
		try
		{
			int iRowCount = 0;
			int iTotalPageCount = 0;
			DataSet ds = PhoenixCrewCourseEnrollment.CrewCourseEnrollmentSearch(Convert.ToInt32(Session["COURSEID"] != null ? (Session["COURSEID"].ToString()) : Filter.CurrentCourseSelection),
				   null, General.GetNullableInteger(Request.QueryString["currentbatchid"].ToString()),
				   General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 153, "PTL")),
				   1
				   , null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, null, null, null, null, null);

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvParticipantList.DataSource = ds.Tables[0];
				gvParticipantList.DataBind();
			}
			else
			{
				ShowNoRecordsFound(ds.Tables[0], gvParticipantList);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvParticipantList_RowDataBound(object sender, GridViewRowEventArgs e)
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
	
	private bool IsValidateRequest(string csvLicence)
	{

		ucError.HeaderMessage = "Please provide the following required information";

		if (string.IsNullOrEmpty(csvLicence))
			ucError.ErrorMessage = "select atleast one or more seafarers.";

		if (General.GetNullableInteger(ddlBatch.SelectedValue)==null)
			ucError.ErrorMessage = "Batch is required.";

		return (!ucError.IsError);
	}
}
