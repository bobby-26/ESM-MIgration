using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using System.Text;
using Telerik.Web.UI;
public partial class CrewCourseOrganization : PhoenixBasePage
{
	private const string SCRIPT_DOFOCUS = @"window.setTimeout('DoFocus()', 1);
            function DoFocus()
            {
                try {
                    document.getElementById('REQUEST_LASTFOCUS').focus();
                } catch (ex) {}
            }";
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddButton("New", "NEW");
			toolbar.AddButton("Save", "SAVE");
			MenuCourseOrganization.AccessRights = this.ViewState;
			MenuCourseOrganization.MenuList = toolbar.Show();
			
			if (!IsPostBack)
			{
				ucTime.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					151, "DAY");
                cblInstitution.DataSource = PhoenixRegistersInstitutionMapping.ListAddress("138");
				cblInstitution.DataTextField = "FLDNAME";
				cblInstitution.DataValueField = "FLDADDRESSCODE";
				cblInstitution.DataBind();
				ViewState["organizationid"] = "";
				if (Session["COURSEID"] != null)
				{
					EditCourseContent();
					
				}
				HookOnFocus(this.Page as Control);
				Page.ClientScript.RegisterStartupScript(
				 typeof(CrewCourseOrganization),
				 "ScriptDoFocus",
				 SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
				 true);
				
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	protected void EditCourseContent()
	{
		try
		{

			int courseid = Convert.ToInt32(Session["COURSEID"].ToString());
			DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(courseid);
			if (ds.Tables[0].Rows.Count > 0)
			{

				ucCourse.SelectedCourse = ds.Tables[0].Rows[0]["FLDDOCUMENTID"].ToString();
				ucCourseType.SelectedHard = ds.Tables[0].Rows[0]["FLDDOCUMENTTYPE"].ToString();
				if (ds.Tables[0].Rows[0]["FLDDOCUMENTTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					103, "0") || ds.Tables[0].Rows[0]["FLDDOCUMENTTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					103, "6"))
				{
					txtApprovalpermonth.CssClass = "input_mandatory";
				}

			}
			DataSet ds1 = PhoenixRegistersCourseOrganization.EditCourseOrganization(courseid);
			if (ds1.Tables[0].Rows.Count > 0)
			{

				txtDay.Text = ds1.Tables[0].Rows[0]["FLDDAY"].ToString();
				txtHour.Text = ds1.Tables[0].Rows[0]["FLDHOUR"].ToString();
				ucTime.SelectedHard = ds1.Tables[0].Rows[0]["FLDMONTHDAYHOUR"].ToString();
				ucLanguage.SelectedQuick = ds1.Tables[0].Rows[0]["FLDLANGUAGEID"].ToString();
				txtMinimumParticipant.Text = ds1.Tables[0].Rows[0]["FLDMINSTRENGTH"].ToString();
				txtMaximumParticipant.Text = ds1.Tables[0].Rows[0]["FLDMAXSTRENGTH"].ToString();
				txtCost.Text = ds1.Tables[0].Rows[0]["FLDCOST"].ToString();
				ucCurrency.SelectedCurrency = ds1.Tables[0].Rows[0]["FLDCURRENCYID"].ToString();
				txtNotes.Text = ds1.Tables[0].Rows[0]["FLDNOTES"].ToString();
				//txtWebsite.Text = ds1.Tables[0].Rows[0]["FLDWEBSITE"].ToString();
				//chkAvailability.Checked = ds1.Tables[0].Rows[0]["FLDAVAILABILITYYN"].ToString() == "1" ? true : false;
				ucApprovalStatus.SelectedQuick = ds1.Tables[0].Rows[0]["FLDADDPROVALSTATUS"].ToString();
				ViewState["organizationid"] = ds1.Tables[0].Rows[0]["FLDORGANIZATIONID"].ToString();
				
				txtApprovalpermonth.Text = ds1.Tables[0].Rows[0]["FLDCOURSEPERMONTH"].ToString();
				txtApprovalperyear.Text = ds1.Tables[0].Rows[0]["FLDCOURSEPERYEAR"].ToString();
                txtStartTime.Text = ds1.Tables[0].Rows[0]["FLDSTARTTIME"].ToString();
                txtEndTime.Text = ds1.Tables[0].Rows[0]["FLDENDTIME"].ToString();
				string[] inst = ds1.Tables[0].Rows[0]["FLDINSTITUTIONID"].ToString().Split(',');
				foreach (string item in inst)
				{
					if (item.Trim() != "")
					{
						if (cblInstitution.Items.FindByValue(item) != null)
							cblInstitution.Items.FindByValue(item).Selected = true;
					}
				}
				txtWrittenPassMarks.Text = ds1.Tables[0].Rows[0]["FLDPASSMARKS"].ToString();
				txtWrittenPassPercentage.Text = ds1.Tables[0].Rows[0]["FLDWRITTENPASSPERCENT"].ToString();
				txtCBTPassMarks.Text = ds1.Tables[0].Rows[0]["FLDCBTMAXMARKS"].ToString();
				txtCBTPassPercentage.Text = ds1.Tables[0].Rows[0]["FLDCBTPASSPERCENT"].ToString();
			}
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	private void HookOnFocus(Control CurrentControl)
	{
		if ((CurrentControl is TextBox) ||
			(CurrentControl is DropDownList))

			(CurrentControl as WebControl).Attributes.Add(
			   "onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");
		if (CurrentControl.HasControls())

			foreach (Control CurrentChildControl in CurrentControl.Controls)
				HookOnFocus(CurrentChildControl);
	}
	protected void CalculatePerYear(object sender, EventArgs e)
	{
		if (txtApprovalpermonth.Text != "")
		{
			txtApprovalperyear.Text = Convert.ToString( Convert.ToInt32(txtApprovalpermonth.Text) * 12);
		}
	}

	protected void Reset()
	{

		txtDay.Text = "";
		txtHour.Text="";
		ucTime.SelectedHard = "";
		ucLanguage.SelectedQuick = "";
		txtMinimumParticipant.Text = "";
		txtMaximumParticipant.Text = "";
		txtCost.Text = "";
		ucCurrency.SelectedCurrency = "";
		//txtWebsite.Text = "";
		txtNotes.Text = "";
		txtWrittenPassMarks.Text = "";
		txtWrittenPassPercentage.Text = "";
		txtCBTPassMarks.Text = "";
		txtCBTPassPercentage.Text = "";
		//chkAvailability.Checked = false;
		foreach (ListItem item in cblInstitution.Items)
		{
			item.Selected = false;

		}
	}

	protected void CourseOrganization_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("SAVE"))
			{
				SaveCourseOrganization();
			}
			if (dce.CommandName.ToUpper().Equals("NEW"))
			{
				Reset();

			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}


	protected void SaveCourseOrganization()
	{
		try
		{
			StringBuilder strinstitution = new StringBuilder();
			foreach (ListItem item in cblInstitution.Items)
			{
				if (item.Selected == true)
				{
					strinstitution.Append(item.Value.ToString());
					strinstitution.Append(",");
				}
			}

			if (strinstitution.Length > 1)
			{
				strinstitution.Remove(strinstitution.Length - 1, 1);
			}

			if (!IsValidCourseOrganization(strinstitution.ToString()))
			{
				ucError.Visible = true;
				return;
			}
			if (ViewState["organizationid"].ToString() == "")
			{
				PhoenixRegistersCourseOrganization.InsertCourseOrganization(
				PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Convert.ToInt32(Session["COURSEID"].ToString()),
				General.GetNullableInteger(ucTime.SelectedHard),
				General.GetNullableInteger(ucLanguage.SelectedQuick),
				General.GetNullableInteger(txtMinimumParticipant.Text),
				General.GetNullableInteger(txtMaximumParticipant.Text),
				General.GetNullableDecimal(txtCost.Text),
				General.GetNullableInteger(ucCurrency.SelectedCurrency),
				General.GetNullableString(txtNotes.Text),
				null,//General.GetNullableString(txtWebsite.Text),
				null,//chkAvailability.Checked == true ? 1 : 0,
				General.GetNullableInteger(ucApprovalStatus.SelectedQuick),
				General.GetNullableDecimal(txtWrittenPassMarks.Text),
				strinstitution.ToString(),
				General.GetNullableInteger(txtApprovalpermonth.Text),
				General.GetNullableInteger(txtApprovalperyear.Text),
                General.GetNullableString(txtStartTime.Text),
                General.GetNullableString(txtEndTime.Text),
				General.GetNullableInteger(txtDay.Text),
				General.GetNullableDecimal(txtHour.Text),
				General.GetNullableDecimal(txtWrittenPassPercentage.Text),
				General.GetNullableDecimal(txtCBTPassMarks.Text),
				General.GetNullableDecimal(txtCBTPassPercentage.Text)
				);
				EditCourseContent();
				ucStatus.Text = "Course Organization Information added.";

			}
			else
			{
				PhoenixRegistersCourseOrganization.UpdateCourseOrganization(
				PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Convert.ToInt32(ViewState["organizationid"].ToString()),
				Convert.ToInt32(Session["COURSEID"].ToString()),
				General.GetNullableInteger(ucTime.SelectedHard),
				General.GetNullableInteger(ucLanguage.SelectedQuick),
				General.GetNullableInteger(txtMinimumParticipant.Text),
				General.GetNullableInteger(txtMaximumParticipant.Text),
				General.GetNullableDecimal(txtCost.Text),
				General.GetNullableInteger(ucCurrency.SelectedCurrency),
				General.GetNullableString(txtNotes.Text),
				null,//General.GetNullableString(txtWebsite.Text),
				null,//chkAvailability.Checked == true ? 1 : 0,
				General.GetNullableInteger(ucApprovalStatus.SelectedQuick),
				General.GetNullableDecimal(txtWrittenPassMarks.Text),
				strinstitution.ToString(),
				General.GetNullableInteger(txtApprovalpermonth.Text),
                General.GetNullableInteger(txtApprovalperyear.Text),
                General.GetNullableString(txtStartTime.Text),
                General.GetNullableString(txtEndTime.Text),
				General.GetNullableInteger(txtDay.Text),
				General.GetNullableDecimal(txtHour.Text),
				General.GetNullableDecimal(txtWrittenPassPercentage.Text),
				General.GetNullableDecimal(txtCBTPassMarks.Text),
				General.GetNullableDecimal(txtCBTPassPercentage.Text));

			}

			ucStatus.Text = "Course Organization Information updated.";


		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private bool IsValidCourseOrganization(string institution)
	{

		ucError.HeaderMessage = "Please provide the following required information";
		int result;

		if ((string.IsNullOrEmpty(txtDay.Text)) || int.TryParse(ucTime.SelectedHard, out result) == false)
			ucError.ErrorMessage = "Day  is required.";

		if (!string.IsNullOrEmpty(txtHour.Text))
		{
			if( Convert.ToDouble(txtHour.Text)>=6.00)
				ucError.ErrorMessage = "Hours shud be less than 6.0";
		}

		if (General.GetNullableDateTime(DateTime.Today.ToString("dd/MM/yyyy") + " " + txtStartTime.Text) == null)
		{
			ucError.ErrorMessage = "Start  Time is required.";
		}
		if (General.GetNullableDateTime(DateTime.Today.ToString("dd/MM/yyyy") + " " + txtEndTime.Text) == null)
		{
			ucError.ErrorMessage = "End  Time is required.";
		}
		if (string.IsNullOrEmpty(txtMinimumParticipant.Text))
			ucError.ErrorMessage = "Minimum Participants  is required.";

		if (string.IsNullOrEmpty(txtMaximumParticipant.Text))
			ucError.ErrorMessage = "Maximum Participants  is required.";

		if (General.GetNullableDecimal(txtWrittenPassMarks.Text)==null)
			ucError.ErrorMessage = "Written Pass Marks is required.";

		if (General.GetNullableDecimal(txtWrittenPassPercentage.Text) == null)
			ucError.ErrorMessage = "Written Pass % is required.";

		if (General.GetNullableDecimal(txtCBTPassMarks.Text) == null)
			ucError.ErrorMessage = "CBT Pass Marks is required.";

		if (General.GetNullableDecimal(txtCBTPassPercentage.Text) == null)
			ucError.ErrorMessage = "CBT Pass % is required.";

		if (!string.IsNullOrEmpty(txtMinimumParticipant.Text) || (!string.IsNullOrEmpty(txtMaximumParticipant.Text)))
		{
			Int32 dtmin=Convert.ToInt32(txtMinimumParticipant.Text);
			Int32 dtmax = Convert.ToInt32(txtMaximumParticipant.Text);
			if (dtmax < dtmin)
			{
				ucError.ErrorMessage = "Maximum Participants cannot be less than the Minimum Participants.";
			}

		}
		if (institution.Trim().Length == 0)
			ucError.ErrorMessage = "Select Atleast one Institution.";

		if ((!string.IsNullOrEmpty(txtCost.Text)))
		{
			if (int.TryParse(ucCurrency.SelectedCurrency, out result) == false)
				ucError.ErrorMessage = "Currency for Cost  is required.";
		}
		else if (!int.TryParse(ucCurrency.SelectedCurrency, out result) == false)
		{
			if((string.IsNullOrEmpty(txtCost.Text)))
			ucError.ErrorMessage = "Cost  is required.";
		}

		if (txtApprovalpermonth.CssClass == "input_mandatory txtNumber small")
		{
			if ((string.IsNullOrEmpty(txtApprovalpermonth.Text)))
				ucError.ErrorMessage = "Approval per month  is required.";
		}


		return (!ucError.IsError);
	}




	public StateBag ReturnViewState()
	{
		return ViewState;
	}


}
