using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
public partial class CrewCourseEnrollmentDetails : PhoenixBasePage
{
	
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddButton("Save", "SAVE");
			MenuCourseEnrollmentDetails.AccessRights = this.ViewState;
			MenuCourseEnrollmentDetails.MenuList = toolbar.Show();
			
			if (!IsPostBack)
			{
				ucBatch.CourseId = Session["COURSEID"].ToString();
				
				PrincipalManagerClick(null,null);
				CrewEnrollmentEdit( new Guid(Request.QueryString["enrollmentid"].ToString()));
			
			}
		

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	
	private void CrewEnrollmentEdit(Guid enrollmentid)
	{
		DataSet ds;
		ds = PhoenixCrewCourseEnrollment.EditCrewCourseEnrollment(enrollmentid);

		if (ds.Tables[0].Rows.Count > 0)
		{

			txtEmployeeCode.Text = ds.Tables[0].Rows[0]["FLDEMPLOYEECODE"].ToString();
			txtEmail.Text = ds.Tables[0].Rows[0]["FLDEMAIL"].ToString();
			txtEmployeeName.Text = ds.Tables[0].Rows[0]["FLDEMPLOYEENAME"].ToString();
			txtRank.Text = ds.Tables[0].Rows[0]["FLDRANKNAME"].ToString();
			txtPriority.Text = ds.Tables[0].Rows[0]["FLDPRIORITY"].ToString();
			txtCost.Text = ds.Tables[0].Rows[0]["FLDCOST"].ToString();
			ucCurrency.SelectedCurrency = ds.Tables[0].Rows[0]["FLDCURRENCYID"].ToString();
			ucBatch.SelectedBatch = ds.Tables[0].Rows[0]["FLDBATCHID"].ToString();
			if (ds.Tables[0].Rows[0]["FLDBILLTO"].ToString() != "")
			{
				rblCostCenter.SelectedValue = "2";
				ucPrincipal.SelectedAddress = ds.Tables[0].Rows[0]["FLDBILLTO"].ToString();
			}
			txtRemarks.Text = ds.Tables[0].Rows[0]["FLDREMARKS"].ToString();
		}
	}
	protected void PrincipalManagerClick(object sender, EventArgs e)
	{
		if (rblCostCenter.SelectedValue == "1")
		{
			ucPrincipal.Enabled = false;
		}
		else
		{
			ucPrincipal.Enabled = true;
		}
	}
	protected void CourseEnrollmentDetails_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

		try
		{

			if (dce.CommandName.ToUpper().Equals("SAVE"))
			{
				if (!IsValidEnrollmentDetails())
				{
					ucError.Visible = true;
					return;
				}
				SaveEnrollmentDetails();

			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void SaveEnrollmentDetails()
	{
		try
		{


			PhoenixCrewCourseEnrollment.UpdateCrewCourseEnrollment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				new Guid(Request.QueryString["enrollmentid"]), General.GetNullableInteger(ucBatch.SelectedBatch), Convert.ToInt32(txtPriority.Text), General.GetNullableDecimal(txtCost.Text),
				General.GetNullableInteger(ucCurrency.SelectedCurrency), General.GetNullableInteger(ucPrincipal.SelectedAddress),
				null, General.GetNullableString(txtRemarks.Text), General.GetNullableInteger(Request.QueryString["typelist"]));

			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo');", true);
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	

	
	private bool IsValidEnrollmentDetails()
	{
		ucError.HeaderMessage = "Please provide the following required information";
	
		int result;

		if (txtPriority.Text == "")
			ucError.ErrorMessage = "Priority is required.";

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
		return (!ucError.IsError);

	}

}
