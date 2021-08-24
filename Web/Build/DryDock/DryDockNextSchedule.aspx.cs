using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.DryDock;
using System.Text;

public partial class DryDockNextSchedule : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbarmain = new PhoenixToolbar();
			toolbarmain.AddButton("Save", "SAVE");
			MenuSchedule.AccessRights = this.ViewState;
			MenuSchedule.MenuList = toolbarmain.Show();

			cmdHiddenPick.Attributes.Add("style", "visibility:hidden;");
			if (!IsPostBack)
			{
				LoadStatus();
				BindFields();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private void BindFields()
	{
		try
		{
            DataTable dt = PhoenixDryDockSchedule.GetNextDryDockSchedule(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            DataRow dr = dt.Rows[0];

            ucLastDoneDate.Text = dr["FLDLASTDONEDATE"].ToString();
            ucDueDate.Text = dr["FLDDUEDATE"].ToString();
            txtWindowperiod.Text = dr["FLDWINDOWPERIOD"].ToString();
            txtRemarks.Text = dr["FLDREMARKS"].ToString();
            ucStatus.SelectedValue = dr["FLDSTATUS"].ToString();
            txtnoofalerts.Text = dr["FLDNOOFALERTS"].ToString();
            ViewState["OPERATIONMODE"] = "EDIT";

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private void LoadStatus()
	{
		try
		{
			DataSet ds = PhoenixDryDockSchedule.ListDryDockStatus();

			if (ds.Tables[0].Rows.Count > 0)
			{
				ucStatus.DataSource = ds.Tables[0];
				ucStatus.DataBind();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private bool IsValidSchedule()
	{

		ucError.HeaderMessage = "Please provide the following required information.";


		if (General.GetNullableDateTime(ucDueDate.Text) == null)
			ucError.ErrorMessage = "Due Date can not be blank.";


		if (General.GetNullableInteger(txtnoofalerts.Text) == null)
			ucError.ErrorMessage = "No of alerts can not be blank.";

		if (txtWindowperiod.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Window Period can not be blank.";


		return (!ucError.IsError);
	}


	protected void Schedule_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("SAVE"))
			{
				if (!IsValidSchedule())
				{
					ucError.Visible = true;
					return;
				}
                Guid? scheduleid = null;
                PhoenixDryDockSchedule.InsertDryDockSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , txtTitle.Text
                    , General.GetNullableDateTime(ucLastDoneDate.Text)
                    , General.GetNullableDateTime(ucDueDate.Text)
                    , General.GetNullableInteger(txtWindowperiod.Text)
                    , General.GetNullableInteger(txtnoofalerts.Text)
                    , General.GetNullableInteger(ucStatus.SelectedValue)
                    , General.GetNullableString(txtRemarks.Text)
                    , ref scheduleid);

				BindFields();

				string Script = "";
				Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
				Script += "fnReloadList('codehelp1','ifMoreInfo','');";
				Script += "parent.CloseCodeHelpWindow('MoreInfo');";
				Script += "</script>" + "\n";
				Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);


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

}
