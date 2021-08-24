using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewCandidateEvaluationAdd : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);
		cmdHiddenSubmit.Attributes.Add("style", "display:None;");

        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Save", "SAVE",ToolBarDirection.Right);
        CrewMenuGeneral.MenuList = toolbarsub.Show();

        if (!IsPostBack)
		{
			ViewState["CVSL"] = -1;
			ViewState["CRNK"] = -1;
			SetEmployeePrimaryDetails();

		}
	}
    
	protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (!IsValidate(ddlVessel.SelectedVessel))
			{
				ucError.Visible = true;
				return;
			}
			if (CommandName.ToUpper().Equals("SAVE"))
			{			
				SaveCrewInterviewSummary();

			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	
	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		
	}

	public void SaveCrewInterviewSummary()
	{
		string Script = "";
		Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
		byte? s = null;
		try
		{
		PhoenixNewApplicantInterviewSummary.InsertNewApplicantInterviewSummary(Convert.ToInt32(Filter.CurrentCrewSelection)
																				, "0,0,0,0"
																				, ",,,"
																				, null
																				, null
																				, null
																				, ",,,,,"
																				, null
																				, null
																				, null
																				, s
																				, byte.Parse("0")
																				, null
																				, General.GetNullableInteger(ddlVessel.SelectedVessel)
																				, null
																				, null
																				, null
																				, null
																				, int.Parse(ViewState["RANKPOSTED"].ToString())
																				, General.GetNullableInteger(ddlOffSigner.SelectedCrew)
																				, DateTime.Parse(txtJoinDate.Text)
																				, null
																				, null
																				, byte.Parse("0")
																				, null
																				, null
																				, null
																				, null
																				, null
																				, null
																				, null
																				, null
																				, null
																				, null
																				, null
																				, int.Parse(ddlRank.SelectedRank)
																				,  byte.Parse("0")
																				);
	

		ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, true);
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private bool IsValidate(string strVessel)
	{
		ucError.HeaderMessage = "Please provide the following required information";
		int resultInt;
		DateTime resultDate;

		if (!int.TryParse(strVessel, out resultInt))
		{
			ucError.ErrorMessage = "Expected Joining Vessel is required.";
		}
		else if (int.TryParse(strVessel, out resultInt))
		{
			DataSet ds = PhoenixRegistersVessel.EditVessel(Convert.ToInt32(strVessel));
			if (ds.Tables[0].Rows[0]["FLDENGINETYPE"].ToString() == "")
				ucError.ErrorMessage = "Please go to vessel master and map engine type for the vessel " + ds.Tables[0].Rows[0]["FLDVESSELNAME"];
		}
		if (!int.TryParse(ddlRank.SelectedRank, out resultInt))
		{
			ucError.ErrorMessage = "Rank of the Person to be Relieved.";
		}
		
		if (!DateTime.TryParse(txtJoinDate.Text, out resultDate))
		{
			ucError.ErrorMessage = "Expected Joining Date is required.";
		}
		else if (DateTime.TryParse(txtJoinDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) < 0)
		{
			ucError.ErrorMessage = "Expected Joining Date should be later than current date";
		}
		
		if (chkNoRelevee.Checked != true)
		{
			if (!int.TryParse(ddlOffSigner.SelectedCrew, out resultInt))
			{
				ucError.ErrorMessage = "OffSigner is required.";
			}
		}
	
		return (!ucError.IsError);
	}
	protected void OnNoRelieveeClick(object sender, EventArgs e)
	{
		BindOffSigner(sender, e);
		SetOffsignerMandatory();
	}

	private void SetOffsignerMandatory()
	{
		if (chkNoRelevee.Checked == true)
		{
			ddlOffSigner.SelectedCrew = String.Empty;
			ddlOffSigner.Enabled = false;
			ddlOffSigner.CssClass = "";
		}
		else
		{
			ddlOffSigner.Enabled = true;
			ddlOffSigner.CssClass = "input_mandatory";
		}
	}
	private void ResetFormControlValues(Control parent)
	{

		try
		{
			foreach (Control c in parent.Controls)
			{
				if (c.Controls.Count > 0)
				{
					ResetFormControlValues(c);
				}
				else
				{
					switch (c.GetType().ToString())
					{
						case "System.Web.UI.WebControls.TextBox":
							((TextBox)c).Text = "";
							break;
						case "System.Web.UI.WebControls.CheckBox":
							((CheckBox)c).Checked = false;
							break;
						case "System.Web.UI.WebControls.RadioButton":
							((RadioButton)c).Checked = false;
							break;
						case "System.Web.UI.WebControls.DropDownList":
							((DropDownList)c).SelectedIndex = 0;
							break;
						case "System.Web.UI.WebControls.ListBox":
							((ListBox)c).SelectedIndex = 0;
							break;

					}
				}
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}

	public void SetEmployeePrimaryDetails()
	{
		try
		{
			DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
			if (dt.Rows.Count > 0)
			{
				txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
				txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
				txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
				txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
				txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
				ViewState["status"] = dt.Rows[0]["FLDSTATUSNAME"].ToString();				
				ddlRank.SelectedRank = dt.Rows[0]["FLDRANKPOSTED"].ToString();
				ViewState["RANKPOSTED"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	
	protected void BindOffSigner(Object sender, EventArgs e)
	{
        UserControlVesselCommon vsl = ddlVessel;
		UserControlRank rank = ddlRank;
		UserControlCrewOnboard cob = ddlOffSigner;

		int? VesselId = General.GetNullableInteger(vsl.SelectedVessel);
		int? RankId = General.GetNullableInteger(rank.SelectedRank);

        if (VesselId.HasValue)
        {
            bool bind = false;
            if (int.Parse(ViewState["CVSL"].ToString()) != VesselId.Value)
            {
                ViewState["CVSL"] = VesselId.Value;
                bind = true;
            }
            if (RankId.HasValue && int.Parse(ViewState["CRNK"].ToString()) != RankId.Value)
            {
                ViewState["CRNK"] = RankId.Value;
                bind = true;
            }
            if (bind)
            {
                cob.SelectedCrew = "";
                cob.OnboardList = PhoenixCrewManagement.ListCrewOnboard(VesselId.Value, RankId);
            }
        }
        else
        {
            cob.SelectedCrew = "";
            cob.OnboardList = PhoenixCrewManagement.ListCrewOnboard(General.GetNullableInteger(vsl.SelectedVessel), General.GetNullableInteger(rank.SelectedRank));
        }
	}

	protected bool checkIsTop4Rank(int? rankid)
	{
		int res = 0;
		DataSet ds = PhoenixNewApplicantInterviewSummary.GetTopOfficerRank(General.GetNullableInteger(rankid.ToString()), ref res);
		if (res == 1)
			return true;
		else
			return false;
	}
}
