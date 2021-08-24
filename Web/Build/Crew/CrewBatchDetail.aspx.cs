using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewBatchDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
		
		SessionUtil.PageAccessRights(this.ViewState);
		PhoenixToolbar toolbar = new PhoenixToolbar();
		toolbar.AddButton("Save", "SAVE");
		MenuBatchDetails.AccessRights = this.ViewState;
		MenuBatchDetails.MenuList = toolbar.Show();
		if (!IsPostBack)
		{
			if (Filter.CurrentCourseSelection != null)
			{
				EditCourse();
			}
			ddlDepartment.DataSource = PhoenixRegistersTrainingDepartment.ListTrainingDepartment();
			ddlDepartment.DataTextField = "FLDDEPARTMENTNAME";
			ddlDepartment.DataValueField = "FLDDEPARTMENTID";
			ddlDepartment.DataBind();
			BindSignatory(null,null);
            if (Request.QueryString["batchid"] != null)
            {
                EditBatchDetails(Convert.ToInt32(Request.QueryString["batchid"]));
            }
			
		}
		if (ucStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,152,"CNL"))
		{
			txtNotes.CssClass = "input_mandatory";
		}
		

    }
	protected void BindSignatory(object sender,EventArgs e)
	{
		ddlSignatory.DataSource = PhoenixCrewCourseDesignation.Listdesignationmapping(General.GetNullableInteger(ddlDepartment.SelectedValue));
		ddlSignatory.DataTextField = "FLDUSERNAME";
		ddlSignatory.DataValueField = "FLDUSERCODE";
		ddlSignatory.DataBind();

		ddlCourseOfficer.DataSource = PhoenixCrewCourseDesignation.Listdesignationmapping(General.GetNullableInteger(ddlDepartment.SelectedValue));
		ddlCourseOfficer.DataTextField = "FLDUSERNAME";
		ddlCourseOfficer.DataValueField = "FLDUSERCODE";
		ddlCourseOfficer.DataBind();

	}
	protected void EditCourse()
	{
		try
		{

			int courseid = Convert.ToInt32(Filter.CurrentCourseSelection);
			DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(courseid);
			if (ds.Tables[0].Rows.Count > 0)
			{
				ucCourse.SelectedCourse = ds.Tables[0].Rows[0]["FLDDOCUMENTID"].ToString();
				ucCourseType.SelectedHard = ds.Tables[0].Rows[0]["FLDDOCUMENTTYPE"].ToString();

                if (Request.QueryString["batchid"] == null)
                {
                    txtStartTime.Text = ds.Tables[0].Rows[0]["FLDSTARTTIME"].ToString();
                    txtEndTime.Text = ds.Tables[0].Rows[0]["FLDENDTIME"].ToString();
                }

			}
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	
	protected void BindVenueDetails(object sender, EventArgs e)
	{
		if (ucBatchVenue.SelectedAddress != "Dummy")
		{
			ProviderDetails(Convert.ToInt64(ucBatchVenue.SelectedAddress));
		}

	}
	protected void EditBatchDetails(int batchid)
	{
		try
		{

			DataSet ds = PhoenixRegistersBatch.EditBatch(batchid);
			if (ds.Tables[0].Rows.Count > 0)
			{

				txtBatchNo.Text = ds.Tables[0].Rows[0]["FLDBATCH"].ToString();
				ucStatus.SelectedHard = ds.Tables[0].Rows[0]["FLDSTATUS"].ToString();
				txtStatusDate.Text = ds.Tables[0].Rows[0]["FLDMODIFIEDDATE"].ToString();
				ucStartTime.Text = ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString();
				if (General.GetNullableDateTime(ds.Tables[0].Rows[0]["FLDSTARTTIME"].ToString()) != null)
				{
					txtStartTime.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FLDSTARTTIME"]).ToString("hh:mm tt");
				}
				ucEndTime.Text = ds.Tables[0].Rows[0]["FLDTODATE"].ToString();
				if (General.GetNullableDateTime(ds.Tables[0].Rows[0]["FLDENDTIME"].ToString()) != null)
				{
					txtEndTime.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FLDENDTIME"]).ToString("hh:mm tt");
				}
				txtMinimumParticipant.Text = ds.Tables[0].Rows[0]["FLDMINPARTICIPANT"].ToString();
				txtMaximumParticipant.Text = ds.Tables[0].Rows[0]["FLDMAXPARTICIPANT"].ToString();
				txtNotes.Text = ds.Tables[0].Rows[0]["FLDNOTES"].ToString();
				ucLanguage.SelectedQuick = ds.Tables[0].Rows[0]["FLDLANGUAGE"].ToString();
				ucBatchVenue.SelectedAddress = ds.Tables[0].Rows[0]["FLDVENUEID"].ToString();
				txtVenueAddress.Text = ds.Tables[0].Rows[0]["FLDVENUEADDRESS"].ToString();
				txtVenuePrimaryContact.Text = ds.Tables[0].Rows[0]["FLDVENUECONTACT"].ToString();
				txtVenueCity.Text = ds.Tables[0].Rows[0]["FLDVENUECITYNAME"].ToString();
				txtVenuePhoneno.Text = ds.Tables[0].Rows[0]["FLDVENUEPHONENNO"].ToString();
				txtVenueState.Text = ds.Tables[0].Rows[0]["FLDVENUESTATENAME"].ToString();
				txtVenueEmail.Text = ds.Tables[0].Rows[0]["FLDVENUEEMAIL"].ToString();
				txtVenueCountry.Text = ds.Tables[0].Rows[0]["FLDVENUECOUNTRYNAME"].ToString();
				txtVenuePostalCode.Text = ds.Tables[0].Rows[0]["FLDVENUEPOSTALCODE"].ToString();
				ddlDepartment.SelectedValue = ds.Tables[0].Rows[0]["FLDDEPARTMENTID"].ToString();
				BindSignatory(null,null);
				if (General.GetNullableInteger(ddlDepartment.SelectedValue )!= null)
				{
					ddlSignatory.SelectedValue = ds.Tables[0].Rows[0]["FLDFINALSIGNATORY"].ToString();
					ddlCourseOfficer.SelectedValue = ds.Tables[0].Rows[0]["FLDCOURSEOFFICER"].ToString();
				}
				if (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDVENUEID"].ToString()) != null)
				{
					ProviderDetails(Convert.ToInt64(ds.Tables[0].Rows[0]["FLDVENUEID"].ToString()));
				}
				
			}
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	protected void ProviderDetails(Int64 adddressid)
	{
		DataSet ds = PhoenixRegistersBatch.EditAddress(adddressid);

	 if (ds.Tables[0].Rows.Count > 0)
	 {
			 ucBatchVenue.SelectedAddress = ds.Tables[0].Rows[0]["FLDADDRESSCODE"].ToString();
			 txtVenueAddress.Text = ds.Tables[0].Rows[0]["FLDADDRESS"].ToString();
			 txtVenuePrimaryContact.Text = ds.Tables[0].Rows[0]["FLDCONTACT"].ToString();
			 txtVenueCity.Text = ds.Tables[0].Rows[0]["FLDCITYNAME"].ToString();
			 txtVenuePhoneno.Text = ds.Tables[0].Rows[0]["FLDPHONENNO"].ToString();
			 txtVenueState.Text = ds.Tables[0].Rows[0]["FLDSTATENAME"].ToString();
			 txtVenueEmail.Text = ds.Tables[0].Rows[0]["FLDEMAIL"].ToString();
			 txtVenueCountry.Text = ds.Tables[0].Rows[0]["FLDCOUNTRYNAME"].ToString();
			 txtVenuePostalCode.Text = ds.Tables[0].Rows[0]["FLDPOSTALCODE"].ToString();
		}
	

	}
	protected void BatchDetails_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("SAVE"))
			{
				if (!IsValidBatchDetails())
				{
					ucError.Visible = true;
					return;
				}
				SaveBatchDetails();
			}
			
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void SaveBatchDetails()
	{
		try
		{
			DateTime startdatetime = DateTime.Parse(General.GetNullableDateTime(ucStartTime.Text).Value.ToShortDateString() + " " + txtStartTime.Text);
			DateTime enddatetime = DateTime.Parse(General.GetNullableDateTime(ucEndTime.Text).Value.ToShortDateString() + " " + txtEndTime.Text);

			PhoenixCrewBatchDetail.UpdateBatchDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Convert.ToInt32(Request.QueryString["batchid"]),
				Convert.ToInt32(ucStatus.SelectedHard),
				General.GetNullableDateTime(startdatetime.ToString()),
				General.GetNullableDateTime(enddatetime.ToString()),
				General.GetNullableInteger(txtMinimumParticipant.Text),
				General.GetNullableInteger(txtMaximumParticipant.Text),
				null,
				null,
				General.GetNullableString(txtNotes.Text),
				General.GetNullableInteger(ucLanguage.SelectedQuick),
				General.GetNullableInteger(ucBatchVenue.SelectedAddress),
				General.GetNullableInteger(ucBatchVenue.SelectedAddress),
				General.GetNullableInteger(Filter.CurrentCourseSelection),
				General.GetNullableInteger(ddlDepartment.SelectedValue),
				General.GetNullableInteger(ddlSignatory.SelectedValue),
				General.GetNullableInteger(ddlCourseOfficer.SelectedValue));
			ucStatusConf.Text = "Batch Details updated";
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private bool IsValidBatchDetails()
	{

		ucError.HeaderMessage = "Please provide the following required information";
		int result;
		DateTime resultDate;
		if (ucStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 152, "CNL") && txtNotes.Text=="")
		{
			ucError.ErrorMessage = "Notes is required during cancellation.";
		}

		if (string.IsNullOrEmpty(txtMinimumParticipant.Text))
			ucError.ErrorMessage = "Minimum Participants  is required.";

		if (string.IsNullOrEmpty(txtMaximumParticipant.Text))
			ucError.ErrorMessage = "Maximum Participants  is required.";

		if (!string.IsNullOrEmpty(txtMinimumParticipant.Text) || (!string.IsNullOrEmpty(txtMaximumParticipant.Text)))
		{
			Int32 dtmin = Convert.ToInt32(txtMinimumParticipant.Text);
			Int32 dtmax = Convert.ToInt32(txtMaximumParticipant.Text);
			if (dtmax < dtmin)
			{
				ucError.ErrorMessage = "Maximum Participants cannot be less than the Minimum Participants.";
			}

		}
		if (int.TryParse(ucStatus.SelectedHard, out result) == false)
			ucError.ErrorMessage = "Status  is required.";
		if (int.TryParse(ucBatchVenue.SelectedAddress, out result) == false)
			ucError.ErrorMessage = "Venue  is required.";
        if (string.IsNullOrEmpty(ucStartTime.Text))
            ucError.ErrorMessage = "Start date is required.";
        if (string.IsNullOrEmpty(ucEndTime.Text))
            ucError.ErrorMessage = "End date is required.";       
        if (!DateTime.TryParse(General.GetNullableDateTime(ucStartTime.Text).Value.ToShortDateString() + " " + txtStartTime.Text, out resultDate))
		 {
			 ucError.ErrorMessage = "Start  Time is required.";
		 }
		  if (!DateTime.TryParse(General.GetNullableDateTime(ucEndTime.Text).Value.ToShortDateString() + " " + txtEndTime.Text, out resultDate))
		 {
			 ucError.ErrorMessage = "End Time is required.";
		 }
		
		return (!ucError.IsError);
	}



}
