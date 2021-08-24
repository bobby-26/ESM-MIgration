using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewFamilyExperience : PhoenixBasePage
{
	private const string SCRIPT_DOFOCUS = @"window.setTimeout('DoFocus()', 1);
            function DoFocus()
            {try {document.getElementById('REQUEST_LASTFOCUS').focus();
            } catch (ex) {}}";

	string strEmployeeId = string.Empty;
	string familyid = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuFamilyExperience.MenuList = toolbar.Show();
            
			if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
				strEmployeeId = string.Empty;
			else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
				strEmployeeId = Request.QueryString["empid"];
			else if (!string.IsNullOrEmpty(Filter.CurrentCrewSelection))
				strEmployeeId = Filter.CurrentCrewSelection;
			familyid = Request.QueryString["familyid"];

			HookOnFocus(this.Page as Control);
			if (!IsPostBack)
			{
				
				ViewState["SIGNOFFID"] = string.Empty;
				if (Request.QueryString["signonoffid"] != null)
				{
					ViewState["SIGNOFFID"] = Request.QueryString["signonoffid"];
					CrewSignOnEdit(int.Parse(familyid), General.GetNullableInteger(ViewState["SIGNOFFID"].ToString()));
				}
				Page.ClientScript.RegisterStartupScript(
				typeof(CrewFamilyExperience),
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
	private void CrewSignOnEdit(int iFamilyId, int? signonoffid)
	{
		DataTable dt = PhoenixCrewFamilyNok.EditCrewFamilyExperience(iFamilyId, signonoffid);

		if (dt.Rows.Count > 0)
		{

			txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
			txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
			txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
			ucVessel.SelectedVessel = dt.Rows[0]["FLDSIGNONVESSELID"].ToString();
			ddlPort.SelectedSeaport = dt.Rows[0]["FLDSIGNONSEAPORTID"].ToString();
			ddlSignOnReason.SelectedSignOnReason = dt.Rows[0]["FLDSIGNONREASONID"].ToString();
			txtSignOnDate.Text = string.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime((dt.Rows[0]["FLDSIGNONDATE"].ToString())));
			txtDuration.Text = dt.Rows[0]["FLDDURATION"].ToString();
			txtReliefDueDate.Text = string.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime((dt.Rows[0]["FLDRELIEFDUEDATE"].ToString())));
			txtSignonRemarks.Text = dt.Rows[0]["FLDSIGNONREMARKS"].ToString();
			ucCountry.SelectedCountry = dt.Rows[0]["FLDCOUNTRYCODE"].ToString();
			ViewState["SIGNONOFF"] = dt.Rows[0]["FLDSTATUS"].ToString();
			ViewState["SIGNOFFID"] = dt.Rows[0]["FLDSIGNONOFFID"].ToString();
			txtSignOffDate.Text = string.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime((dt.Rows[0]["FLDSIGNOFFDATE"].ToString())));
			ddlSignOffPort.SelectedSeaport = dt.Rows[0]["FLDSIGNOFFSEAPORTID"].ToString();
			ddlSignOffReason.SelectedSignOffReason = dt.Rows[0]["FLDSIGNOFFREASONID"].ToString();
			txtSignOffRemarks.Text = dt.Rows[0]["FLDSIGNOFFREMARKS"].ToString();
		
		}
	}

	protected void FilterSeaport(object sender, EventArgs e)
	{
		ddlPort.SeaportList = PhoenixRegistersSeaport.EditSeaport(null, General.GetNullableInteger(ucCountry.SelectedCountry));

	}
	protected void CrewFamilyExperience_TabStripCommand(object sender, EventArgs e)
	{
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
		{
			String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
			if (CommandName.ToUpper().Equals("SAVE"))
			{
				if (!IsValidCrewFamilyExperience())
				{
					ucError.Visible = true;
					return;
				}

				if (ViewState["SIGNOFFID"].ToString() == "")
				{
					PhoenixCrewFamilyNok.InsertCrewFamilyExperience(int.Parse(familyid)
															, DateTime.Parse(txtSignOnDate.Text)
														   , int.Parse(ddlPort.SelectedSeaport)
															, General.GetNullableInteger(ddlSignOnReason.SelectedSignOnReason)
															, General.GetNullableDateTime(txtReliefDueDate.Text)
															, txtSignonRemarks.Text
															, decimal.Parse(txtDuration.Text)
															, General.GetNullableInteger(ucVessel.SelectedVessel)
															, General.GetNullableDateTime(txtSignOffDate.Text)
															, General.GetNullableInteger(ddlSignOffPort.SelectedSeaport)
															, General.GetNullableInteger(ddlSignOffReason.SelectedSignOffReason)
															, txtSignOffRemarks.Text
															);
					

				}
				else
				{
					PhoenixCrewFamilyNok.UpdateCrewFamilyExperience(int.Parse(familyid)
															, Convert.ToInt32(ViewState["SIGNOFFID"].ToString())
															, DateTime.Parse(txtSignOnDate.Text)
														    , int.Parse(ddlPort.SelectedSeaport)
															, General.GetNullableInteger(ddlSignOnReason.SelectedSignOnReason)
															, General.GetNullableDateTime(txtReliefDueDate.Text)
															, txtSignonRemarks.Text
															, decimal.Parse(txtDuration.Text)
															, General.GetNullableInteger(ucVessel.SelectedVessel)
															, General.GetNullableDateTime(txtSignOffDate.Text)
															, General.GetNullableInteger(ddlSignOffPort.SelectedSeaport)
															, General.GetNullableInteger(ddlSignOffReason.SelectedSignOffReason)
															, txtSignOffRemarks.Text
															);
				}
				ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
				

			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	
	private bool IsValidateRemarks(string remarks)
	{
		ucError.HeaderMessage = "Please provide the following required information";

		if (string.IsNullOrEmpty(remarks))
			ucError.ErrorMessage = "Remarks is required for proceeding.";
		return (!ucError.IsError);
	}
	protected void CalculateReliefDue(object sender, EventArgs e)
	{
		UserControlDate d = sender as UserControlDate;
		if (d != null)
		{
			if (txtReliefDueDate.Text != null && txtSignOnDate.Text != null)
			{
				DateTime sg = Convert.ToDateTime(txtSignOnDate.Text);
				DateTime rd = Convert.ToDateTime(txtReliefDueDate.Text);
				TimeSpan s = rd - sg;
				int isnegative = s.Days;

				double x = isnegative / 30.00;
				txtDuration.Text = Convert.ToString(x);
				txtDuration.Text = txtDuration.Text.Substring(0, txtDuration.Text.IndexOf('.') + 2);

			}
		}
		else if (txtDuration.Text != "" && txtSignOnDate.Text != null)
		{

			double months = Convert.ToDouble(txtDuration.Text);
			int days = Convert.ToInt32(30 * months);
			txtReliefDueDate.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Parse(txtSignOnDate.Text).AddDays(days));

		}

	}
	public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
	{
		Double result;
		return Double.TryParse(val, NumberStyle,
			System.Globalization.CultureInfo.CurrentCulture, out result);
	}
	private bool IsValidCrewFamilyExperience()
	{
		ucError.HeaderMessage = "Please provide the following required information";
		DateTime resultDate;
		int resultInt;

		if (General.GetNullableInteger(familyid) == null)
			ucError.ErrorMessage = "Select a Family Member from Family/NoK.";

		if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
			ucError.ErrorMessage = "Vessel is required";

		if (General.GetNullableInteger(ddlPort.SelectedSeaport) == null)
			ucError.ErrorMessage = "Sign-On Port is required.";

		if (!DateTime.TryParse(txtSignOnDate.Text, out resultDate))
			ucError.ErrorMessage = "Sign-On Date is required.";

		if (txtDuration.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Duration is required.";

		if (General.GetNullableDateTime(txtReliefDueDate.Text) == null)
			ucError.ErrorMessage = "Relief Due is required.";


		if (General.GetNullableInteger(ddlSignOnReason.SelectedSignOnReason) == null)
			ucError.ErrorMessage = "Sign-on Reason is required";

		if (txtSignonRemarks.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Sign-on remarks is required.";


		if (General.GetNullableDateTime(txtSignOffDate.Text) == null)
		{
			ucError.ErrorMessage = "Sign-Off Date is required.";
		}
		else
		{
			if (DateTime.Compare(DateTime.Parse(txtSignOnDate.Text), DateTime.Parse(txtSignOffDate.Text)) > 0)
			{
				ucError.ErrorMessage = "SignOff Date should not be less than SignOn Date";
			}
		}

		if (!int.TryParse(ddlPort.SelectedSeaport, out resultInt))
			ucError.ErrorMessage = "Sign-Off Port is required.";

		if (!int.TryParse(ddlSignOffReason.SelectedSignOffReason, out resultInt))
			ucError.ErrorMessage = "Sign-Off Reason is required.";

		if (txtSignOffRemarks.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Sign-off remarks is required.";


		return (!ucError.IsError);

	}
	

}
