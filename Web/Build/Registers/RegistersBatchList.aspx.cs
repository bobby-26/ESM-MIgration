using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersBatchList : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();           
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuBatchList.AccessRights = this.ViewState;
            MenuBatchList.MenuList = toolbar.Show();

            if (!IsPostBack)
			{
                ViewState["BATCHID"] = Request.QueryString["batchid"];
				
				ViewState["BATCHID"] = "";
				chkExcludeSunday.Checked = true;
				chkIncludeHoliday.Checked = true;
				if (Session["COURSEID"] != null && Request.QueryString["calledfrom"] != null)
				{	
					ucCourse.SelectedCourse = Session["COURSEID"].ToString();
					ucCourse.Enabled = false;
					lblNoofSeats.Visible = false;
					txtNoOfSeats.Visible = false;
					chkClosedYN.Enabled = false;
				}
				if (Request.QueryString["batchid"] != null)
				{
					
					BatchEdit();
					chkPublishYN.Enabled = true;
				}
				else 
				{
					ucStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
						152, "PLN");
				}
				if (Request.QueryString["calledfrom"] != null)
				{
					ucCourseInstitution.Visible = true;
					ucInstitution.Visible = false;
					ucCourseInstitution.Course = Session["COURSEID"].ToString() == "" ? Filter.CurrentCourseSelection : Session["COURSEID"].ToString();
				}				
			}
            txtNotes.CssClass = "input";
            ddlPreseayn.SelectedValue = "0";
            if (ucStatus.SelectedHard == "753")
            {
                txtNotes.CssClass = "input_mandatory";
            }
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	
	protected void BatchList_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            String scriptClosePopup = String.Format("javascript:fnReloadList('codehelp1');");

			String scriptKeepPopupOpen = String.Format("javascript:fnReloadList('codehelp1', null, 'yes');");


			if (CommandName.ToUpper().Equals("NEW"))
			{
				ViewState["BATCHID"] = "";
				Reset();
			}
			if (CommandName.ToUpper().Equals("SAVE"))
			{
				if (!IsValidBatch())
				{
					ucError.Visible = true;
					return;
				}
				
					if (ViewState["BATCHID"].ToString() == "")
					{

					PhoenixRegistersBatch.InsertBatch(
					PhoenixSecurityContext.CurrentSecurityContext.UserCode
					, Request.QueryString["calledfrom"] == null ? Convert.ToInt64(ucInstitution.SelectedAddress) : Convert.ToInt64(ucCourseInstitution.SelectedInstitution)
					, Convert.ToInt32(ucCourse.SelectedCourse)
					,Convert.ToDateTime(txtFromDate.Text)
					, Convert.ToDateTime(txtToDate.Text)
					, General.GetNullableInteger(txtNoOfSeats.Text)
					, General.GetNullableString(txtDuration.Text)
					, General.GetNullableInteger(chkClosedYN.Checked == true ? "1" : "0")
					, General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					152, "PLN")),Request.QueryString["calledfrom"] == null ? 0:1, General.GetNullableInteger(chkExcludeSunday.Checked == true ? "1" : "0"), General.GetNullableInteger(chkIncludeHoliday.Checked == true ? "1" : "0")
                    , null, null, General.GetNullableByte(ddlPreseayn.SelectedValue)
					, General.GetNullableString(txtBatch.Text)
                    , General.GetNullableString(txtNotes.Text)
					);
					ScriptManager.RegisterClientScriptBlock(this, this.GetType(),"BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo');", true);
				  
					}
					else 
					{
		
					PhoenixRegistersBatch.UpdateBatch(
					PhoenixSecurityContext.CurrentSecurityContext.UserCode
					,Convert.ToInt32(ViewState["BATCHID"].ToString())
				    ,Request.QueryString["calledfrom"] == null ? Convert.ToInt64(ucInstitution.SelectedAddress) : Convert.ToInt64(ucCourseInstitution.SelectedInstitution)
					,Convert.ToInt32(ucCourse.SelectedCourse)
					,Convert.ToDateTime(txtFromDate.Text)
					,Convert.ToDateTime(txtToDate.Text)
					,General.GetNullableInteger(txtNoOfSeats.Text)
					,General.GetNullableString(txtDuration.Text)
					,General.GetNullableInteger(chkClosedYN.Checked == true ? "1" : "0")
					, General.GetNullableInteger(ucStatus.SelectedHard)
					, General.GetNullableInteger(chkPublishYN.Checked == true ? "1" : "0"), null
					, General.GetNullableInteger(chkExcludeSunday.Checked == true ? "1" : "0")
					, General.GetNullableInteger(chkIncludeHoliday.Checked == true ? "1" : "0")
                    , General.GetNullableByte(ddlPreseayn.SelectedValue)
					, General.GetNullableString(txtBatch.Text)
                    , General.GetNullableString(txtNotes.Text)
                    );

					ScriptManager.RegisterClientScriptBlock(this, this.GetType(),"BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo');", true);
					
					}
				}
			}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void CalculateDuration(object sender, EventArgs e)
	{

		UserControlDate d = sender as UserControlDate;
		if (d != null)
		{
			if (txtFromDate.Text != null && txtToDate.Text != null)
			{
				DateTime fd = Convert.ToDateTime(txtFromDate.Text);
				DateTime sd = Convert.ToDateTime(txtToDate.Text);
				TimeSpan s = sd - fd;

				txtDuration.Text = Convert.ToString(s.Days + 1);

			}
		}
		

	}
	private void Reset()
	{
		ViewState["BATCHID"] = "";
		ucCourseInstitution.SelectedInstitution = "";
		ucInstitution.SelectedAddress = "";
		ucCourse.SelectedCourse = "";
		txtBatch.Text = "";
		txtFromDate.Text = "";
		txtToDate.Text = "";
		txtNoOfSeats.Text = "";
		txtDuration.Text = "";
		chkClosedYN.Checked = false;
		
	}

	protected void BatchEdit()
	{
		try
		{

			int batchid = Convert.ToInt32(Request.QueryString["batchid"]);
			DataSet ds = PhoenixRegistersBatch.EditBatch(batchid);

			if (ds.Tables.Count > 0)
			{
				DataRow dr = ds.Tables[0].Rows[0];
				ucInstitution.SelectedAddress = dr["FLDINSTITUTIONID"].ToString();
				ucCourseInstitution.SelectedInstitution = dr["FLDINSTITUTIONID"].ToString();
				ucCourse.SelectedCourse = dr["FLDCOURSEID"].ToString();
				txtBatch.Text = dr["FLDBATCH"].ToString();
				txtFromDate.Text = dr["FLDFROMDATE"].ToString();
				txtToDate.Text = dr["FLDTODATE"].ToString();
				txtNoOfSeats.Text = dr["FLDNOOFSEATS"].ToString();
				txtDuration.Text = dr["FLDDURATION"].ToString();
				chkClosedYN.Checked = dr["FLDCLOSEDYN"].ToString() == "1" ? true : false;
				ViewState["BATCHID"] = dr["FLDBATCHID"].ToString();
				ucStatus.SelectedHard= dr["FLDSTATUS"].ToString();
				chkPublishYN.Checked = dr["FLDPUBLISHEDYN"].ToString() == "1" ? true : false;
                ddlPreseayn.SelectedValue = dr["FLDISPRESEABATCH"].ToString();
				if (dr["FLDCLOSEDYN"].ToString() == "1")
				{
					DisableAll();
				}
				if (dr["FLDSTATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					152, "OPN"))
				{
					chkClosedYN.Enabled = true;
				}
                txtNotes.Text = dr["FLDNOTES"].ToString();
                //if (chkPreSeaYN.Checked == true)
				//{
				//    txtBatch.ReadOnly = false;
				//    txtBatch.CssClass = "input_mandatory";
				//}
			}
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void DisableAll()
	{
		ucCourse.Enabled = false;
		ucInstitution.Enabled = false;
		txtFromDate.Enabled = false;
		txtToDate.Enabled = false;
		txtBatch.Enabled = false;

	}
	private bool IsValidBatch()
	{
		Int32 resultInt;
	
		ucError.HeaderMessage = "Please provide the following required information";
		if (Request.QueryString["calledfrom"] == null)
		{
			if (!Int32.TryParse(ucInstitution.SelectedAddress, out resultInt))
				ucError.ErrorMessage = "Institution is required";
		}
		else
		{
			if (!Int32.TryParse(ucCourseInstitution.SelectedInstitution, out resultInt))
				ucError.ErrorMessage = "Institution is required";
		}

		if (!Int32.TryParse(ucCourse.SelectedCourse, out resultInt))
			ucError.ErrorMessage = "Course is required";

		if (string.IsNullOrEmpty(txtFromDate.Text))
			ucError.ErrorMessage = "From Date is required.";
		//else if (DateTime.TryParse(txtFromDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
		//{
		//    ucError.ErrorMessage = "From Date should be earlier than current date";
		//}

		if (string.IsNullOrEmpty(txtToDate.Text))
			ucError.ErrorMessage = "To Date is required.";
		//else if (DateTime.TryParse(txtToDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
		//{
		//    ucError.ErrorMessage = "To Date should be earlier than current date";
		//}

		if (txtNoOfSeats.Text != "")
		{
			if (!Int32.TryParse(txtNoOfSeats.Text, out resultInt))
				ucError.ErrorMessage = "Enter a valid number";
		}
		
		if (string.IsNullOrEmpty(txtBatch.Text))
			ucError.ErrorMessage = "Batch is required.";

        if((ucStatus.SelectedHard == "753")&&(General.GetNullableString(txtNotes.Text)==null))
            ucError.ErrorMessage = "Notes required";

		return (!ucError.IsError);
	}

}
