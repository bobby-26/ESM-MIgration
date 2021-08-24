using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Text;
using System.Web.UI;

public partial class DryDockScheduleGeneral : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			//cmdHiddenSubmit.Attributes.Add("style", "display:none;");
			PhoenixToolbar toolbarmain = new PhoenixToolbar();
			toolbarmain.AddButton("Project", "PROJECT");
			toolbarmain.AddButton("Quotation", "QUOTATION");
			MenuRequisitionGeneral.AccessRights = this.ViewState;
			MenuRequisitionGeneral.MenuList = toolbarmain.Show();
			MenuRequisitionGeneral.SelectedMenuIndex = 0;
			if (!IsPostBack)
			{
				ifMoreInfo.Attributes["src"] = "../DryDock/DryDockProject.aspx";
			}

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void RequisitionGeneral_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		
		   if (dce.CommandName.ToUpper().Equals("QUOTATION"))
			{
				if (Filter.CurrentSelectedDryDockProject == null || Filter.CurrentSelectedDryDockProject == "")
				{
					ShowRequisitionError();
					return;
				}
                ucTitle.Text = "Quotation"; 
				ifMoreInfo.Attributes["src"] = "../DryDock/DryDockQuotation.aspx?SCHEDULEID=" + Filter.CurrentSelectedDryDockSchedule;
			}
           else if (dce.CommandName.ToUpper().Equals("PROJECT"))
			{
				MenuRequisitionGeneral.SelectedMenuIndex = 1;
                ucTitle.Text = "Requisition";
				ifMoreInfo.Attributes["src"] = "../DryDock/DryDockProject.aspx?SCHEDULEID=" + Filter.CurrentSelectedDryDockSchedule;
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private void ShowRequisitionError()
	{
		ucError.HeaderMessage = "Navigation Error";
		ucError.ErrorMessage = "Select a Requisition to Create/View the Queries and Quotations";
		ucError.Visible = true;
	}
}
