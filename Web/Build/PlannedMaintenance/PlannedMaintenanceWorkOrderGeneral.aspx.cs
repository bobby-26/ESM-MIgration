using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderGeneral : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbarmain = new PhoenixToolbar();
			toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
			MenuWorkOrderGeneral.AccessRights = this.ViewState;
			MenuWorkOrderGeneral.MenuList = toolbarmain.Show();
			//MenuWorkOrderGeneral.SetTrigger(pnlMenuWorkOrderGeneral);
			txtComponentId.Attributes.Add("style", "visibility:hidden");
			txtJobId.Attributes.Add("style", "visibility:hidden");
			txtReason.Attributes.Add("style", "visibility:hidden");
			txtComponentJobId.Attributes.Add("style", "visibility:hidden");
			if (!IsPostBack)
			{
				//cmdHiddenPick.Attributes.Add("style", "display:none;");
				ucMainType.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTTYPE)).ToString();
				ucMaintClass.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCLASS)).ToString();
				ucMainCause.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCAUSE)).ToString();
				ucFrequency.HardTypeCode = ((int)(PhoenixHardTypeCode.PERIODICFREQUENCY)).ToString();
				ucStatus.HardTypeCode = ((int)(PhoenixHardTypeCode.WORKORDERSTATUS)).ToString();
				ucCounterType.HardTypeCode = "111";
				if (Request.QueryString["WORKORDERID"] != null)
					ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
				BindFields();
				txtDueDate.Enabled = SessionUtil.CanAccess(this.ViewState, "DUEDATE");
				ViewState["PAGENUMBER"] = string.IsNullOrEmpty(Request.QueryString["PAGENUMBER"]) ? "1" : Request.QueryString["PAGENUMBER"].ToString();
                ViewState["RISKYN"] = string.Empty;
                
            }
            if (ViewState["RISKYN"].ToString().ToUpper().Equals("N"))
            {
                RAhide(false);
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
			if ((Request.QueryString["WORKORDERID"] != null) && (Request.QueryString["WORKORDERID"] != ""))
			{
				DataSet ds = PhoenixPlannedMaintenanceWorkOrder.EditWorkOrder(new Guid(Request.QueryString["WORKORDERID"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
				DataRow dr = ds.Tables[0].Rows[0];
				txtWorkOrderNumber.Text = dr["FLDWORKORDERNUMBER"].ToString();
				txtTitle.Text = dr["FLDWORKORDERNAME"].ToString();
                txtCreatedBy.Text = dr["FLDCREATEDBYNAME"].ToString();
				txtComponentId.Text = dr["FLDCOMPONENTID"].ToString();
				txtFrequency.Text = dr["FLDFREQUENCY"].ToString();
				ucFrequency.SelectedHard = dr["FLDFREQUENCYTYPE"].ToString();
				ucDiscipline.SelectedDiscipline = dr["FLDPLANNINGDISCIPLINE"].ToString();
				ucHistory.SelectedHistoryTemplate = dr["FLDHISTORYTEMPLATE"].ToString();
				txtWindow.Text = dr["FLDPLANNINGWINDOWINDAYS"].ToString();
				//txtDueDate.Text = General.GetDateTimeToString(dr["FLDPLANNINGDUEDATE"].ToString());
				txtDueDate.Text = dr["FLDPLANNINGDUEDATE"].ToString();
                txtLastDone.Text = General.GetDateTimeToString(dr["FLDJOBLASTDONEDATE"].ToString());
				//txtStartedDate.Text = General.GetDateTimeToString(dr["FLDWORKORDERSTARTEDDATE"].ToString());
				txtStartedDate.Text = dr["FLDWORKORDERSTARTEDDATE"].ToString();
                txtCoplitedDate.Text = General.GetDateTimeToString(dr["FLDWORKORDERCOMPLETEDDATE"].ToString());
				txtDuration.Text = dr["FLDPLANNINGESTIMETDURATION"].ToString();
				ucMainType.SelectedQuick = dr["FLDWORKMAINTNANCETYPE"].ToString();
				ucMaintClass.SelectedQuick = dr["FLDWORKMAINTNANCECLASS"].ToString();
				ucMainCause.SelectedQuick = dr["FLDWORKMAINTNANCECAUSE"].ToString();
				txtComponentCode.Text = dr["FLDCOMPONENTNUMBER"].ToString();
				txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
				txtPriority.Text = dr["FLDPLANINGPRIORITY"].ToString();
				ucStatus.SelectedHard = dr["FLDWORKORDERSTATUS"].ToString();
				txtOverduedays.Text = dr["FLDOVERDUEDATE"].ToString();
				lblOverdue.Text = General.GetNullableInteger(dr["FLDOVERDUEDATE"].ToString()).HasValue && General.GetNullableInteger(dr["FLDOVERDUEDATE"].ToString()).Value < 0 ? "Overdue (Days)" : "Due (Days)";
				if (dr["FLDWORKISUNEXPECTED"].ToString().Equals("1"))
					chkUnexpected.Checked = true;
				ucWTOApproval.SelectedHard = dr["FLDAPPROVALTYPE"].ToString();
				cmdShowReason.Attributes.Add("onclick", "return showPickList('spnPickReason', 'codehelp1', '', '../PlannedMaintenance/PlannedMaintenanceRemarksPopup.aspx?framename=ifMoreInfo&WORKORDERID=" + Request.QueryString["WORKORDERID"] + "', true);");
				txtJobId.Text = dr["FLDJOBID"].ToString();
				txtComponentJobId.Text = dr["FLDCOMPONENTJOBID"].ToString();
				ucCounterType.SelectedHard = dr["FLDCOUNTERTYPE"].ToString();
				ucCounterFrequency.Text = dr["FLDCOUNTERFREQUENCY"].ToString();
				txtDueHours.Text = dr["FLDDUEHOURS"].ToString();
				txtOverDueHrs.Text = dr["FLDOVERDUEHOURS"].ToString();
				txtWindowsHrs.Text = dr["FLDWINDOWHOURS"].ToString();
				ddlRescheduleReason.SelectedQuick = dr["FLDPOSTPONEREASONID"].ToString();
				if (dr["FLDAPPROVALREQUIRED"].ToString() == "1")
				{
					txtPosponeDate.Text = string.Format("{0:dd/MMM/yyyy}", dr["FLDPOSTPONEDATE"]);
				}
				else
				{
					txtPosponeDate.Visible = false;
					lblPostPonedDate.Visible = false;
				}
				ViewState["OPERATIONMODE"] = "EDIT";
				//if (dr["FLDISRAMANDATORY"].ToString().Equals("1"))
				//{
				//	divRA.Visible = true;
				//}
				//else
				//{
				//	divRA.Visible = false;
				//}
				ViewState["RaId"] = dr["FLDRAID"].ToString();
				ViewState["COMPONENTID"] = dr["FLDCOMPONENTID"].ToString();
				ViewState["JOBID"] = dr["FLDJOBID"].ToString();

                DataTable dt = PhoenixPlannedMaintenanceWorkOrder.WorkOrderRAEdit(new Guid(Request.QueryString["WORKORDERID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    DataRow drr = dt.Rows[0];
                    txtRAId.Text = drr["FLDRAID"].ToString();
                    txtRANumber.Text = drr["FLDRAREFNO"].ToString();
                    txtRaType.Text = drr["FLDTYPE"].ToString();
                    txtRA.Text = drr["FLDRISKASSESSMENT"].ToString();
                }
                if (txtRAId.Text == "")
                {
                    cmdRA.Visible = false;
                    lnkCreateRA.Text = "Create";
                }
                else
                {
                    cmdRA.Visible = true;
                    lnkCreateRA.Text = "Edit/View";
                }
                ViewState["RISKYN"] = dr["FLDREPORTWORKYN"].ToString();
                if (ViewState["RISKYN"].ToString().ToUpper().Equals("N"))
                {
                    RAhide(false);
                }

            }
			else
			{
				ViewState["OPERATIONMODE"] = "ADD";
			}


		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void MenuWorkOrderGeneral_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
			{
				if (!IsValidComponent(txtComponentId.Text, txtTitle.Text))
				{
					ucError.Visible = true;
					return;
				}

				if ((String)ViewState["OPERATIONMODE"] == "EDIT")
				{
					if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
					{
						PhoenixPlannedMaintenanceWorkOrder.UpdateVesselWorkOrder(General.GetNullableGuid(ViewState["WORKORDERID"].ToString()).Value, PhoenixSecurityContext.CurrentSecurityContext.VesselID
						   , General.GetNullableInteger(ucHistory.SelectedHistoryTemplate), General.GetNullableInteger(ucMainType.SelectedQuick)
						   , General.GetNullableInteger(ucMaintClass.SelectedQuick), General.GetNullableInteger(ucMainCause.SelectedQuick));

						if (txtRAId.Text != "")
						{
							PhoenixPlannedMaintenanceWorkOrder.WorkorderRAmapping(General.GetNullableGuid(ViewState["WORKORDERID"].ToString())
																					, General.GetNullableGuid(txtRAId.Text));
						}

					}
					else
					{
						PhoenixPlannedMaintenanceWorkOrder.UpdateWorkOrder(PhoenixSecurityContext.CurrentSecurityContext.VesselID
						, General.GetNullableGuid(ViewState["WORKORDERID"].ToString()), txtWorkOrderNumber.Text, txtTitle.Text
						, General.GetNullableInteger(txtFrequency.Text), General.GetNullableInteger(ucFrequency.SelectedHard)
						, General.GetNullableInteger(ucHistory.SelectedHistoryTemplate), General.GetNullableInteger(ucStatus.SelectedHard)
						, null, General.GetNullableInteger(txtPriority.Text)
						, General.GetNullableInteger(txtDuration.Text), General.GetNullableDateTime(txtDueDate.Text)
						, General.GetNullableInteger(ucDiscipline.SelectedDiscipline), General.GetNullableInteger(txtWindow.Text)
						, General.GetNullableDateTime(txtStartedDate.Text), General.GetNullableDateTime(txtCoplitedDate.Text)
						, General.GetNullableInteger(ucMainType.SelectedQuick), General.GetNullableInteger(ucMaintClass.SelectedQuick)
						, General.GetNullableInteger(ucMainCause.SelectedQuick), chkUnexpected.Checked == true ? "1" : "0"
						, General.GetNullableInteger(ucWTOApproval.SelectedHard), General.GetNullableString(txtReason.Text)
						, General.GetNullableInteger(ddlRescheduleReason.SelectedQuick));
					}
					BindFields();
				}

				if ((String)ViewState["OPERATIONMODE"] == "ADD")
				{
					string workorderid = null;
					PhoenixPlannedMaintenanceWorkOrder.InsertWorkOrder(PhoenixSecurityContext.CurrentSecurityContext.VesselID
					, txtWorkOrderNumber.Text, txtTitle.Text
					, General.GetNullableGuid(txtComponentId.Text), General.GetNullableGuid(txtComponentJobId.Text), General.GetNullableGuid(txtJobId.Text)
					, General.GetNullableInteger(txtFrequency.Text), General.GetNullableInteger(ucFrequency.SelectedHard)
					, General.GetNullableInteger(ucHistory.SelectedHistoryTemplate), General.GetNullableInteger(ucStatus.SelectedHard)
					, null, General.GetNullableInteger(txtPriority.Text)
					, General.GetNullableInteger(txtDuration.Text), General.GetNullableDateTime(txtDueDate.Text)
					, General.GetNullableInteger(ucDiscipline.SelectedDiscipline), General.GetNullableInteger(txtWindow.Text)
					, General.GetNullableDateTime(txtStartedDate.Text), General.GetNullableDateTime(txtCoplitedDate.Text)
					, General.GetNullableInteger(ucMainType.SelectedQuick), General.GetNullableInteger(ucMaintClass.SelectedQuick)
					, General.GetNullableInteger(ucMainCause.SelectedQuick), chkUnexpected.Checked == true ? "1" : "0"
					, General.GetNullableInteger(ucWTOApproval.SelectedHard), null, ref workorderid);
				}

				String script = String.Format("javascript:parent.fnReloadList('code1');");
				ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

			}
			if (CommandName.ToUpper().Equals("NEW"))
			{
				txtComponentId.Text = "";
				txtFrequency.Text = "";
				ucFrequency.SelectedHard = "";
				ucDiscipline.SelectedDiscipline = "";
				ucHistory.SelectedHistoryTemplate = "";
				txtWindow.Text = "";
				txtDueDate.Text = "";
				txtOverduedays.Text = "";
				txtDuration.Text = "";
				ucMainType.SelectedQuick = "";
				ucMaintClass.SelectedQuick = "";
				ucMainCause.SelectedQuick = "";
				txtComponentCode.Text = "";
				txtComponentName.Text = "";
				txtPriority.Text = "3";
				txtWorkOrderNumber.Text = "";
				txtTitle.Text = "";
				ucStatus.SelectedHard = "";
				ViewState["OPERATIONMODE"] = "ADD";
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private bool IsValidComponent(string componentnumber, string componentname)
	{
		ucError.HeaderMessage = "Please provide the following required information";
		DateTime resultdate;

		if (componentname.Trim().Equals(""))
			ucError.ErrorMessage = "Job Description is required";

		if (!string.IsNullOrEmpty(General.GetNullableDateTime(txtStartedDate.Text).ToString()) && !DateTime.TryParse(General.GetNullableDateTime(txtStartedDate.Text).ToString(), out resultdate))
			ucError.ErrorMessage = "Started Date is not a valid date format.";

		else if (General.GetNullableDateTime(txtStartedDate.Text) != null && General.GetNullableDateTime(txtStartedDate.Text) > General.GetNullableDateTime(DateTime.Now.ToShortDateString()))
		{
			ucError.ErrorMessage = "Started Date should be less than current date";
		}
		//else if (!string.IsNullOrEmpty(txtStartedDate.Text) && DateTime.TryParse(txtStartedDate.Text, out resultdate) && DateTime.Compare(DateTime.Now, resultdate) > 0)
		//{
		//    ucError.ErrorMessage = "Started Date should be later than current date";
		//}

		if (!string.IsNullOrEmpty(General.GetNullableDateTime(txtDueDate.Text).ToString()) && !DateTime.TryParse(General.GetNullableDateTime(txtDueDate.Text).ToString(), out resultdate))
			ucError.ErrorMessage = "Due Date is not a valid date format.";
		else if (!string.IsNullOrEmpty(General.GetNullableDateTime(txtDueDate.Text).ToString()) && DateTime.TryParse(General.GetNullableDateTime(txtDueDate.Text).ToString(), out resultdate) && DateTime.Compare(resultdate, DateTime.Now) < 0 && txtReason.Text.Trim() != string.Empty)
		{
			ucError.ErrorMessage = "Due Date should be later than current date";
		}
		else if (!string.IsNullOrEmpty(General.GetNullableDateTime(txtDueDate.Text).ToString()) && !string.IsNullOrEmpty(txtLastDone.Text) && DateTime.TryParse(General.GetNullableDateTime(txtDueDate.Text).ToString(), out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(txtLastDone.Text)) < 0)
		{
			ucError.ErrorMessage = "Due Date should be later than Last Done date";
		}

		if (!string.IsNullOrEmpty(txtCoplitedDate.Text) && !DateTime.TryParse(txtCoplitedDate.Text, out resultdate))
			ucError.ErrorMessage = "Completed Date is not a valid date format.";
		else if (!string.IsNullOrEmpty(txtCoplitedDate.Text) && DateTime.TryParse(txtCoplitedDate.Text, out resultdate)
			&& DateTime.TryParse(txtCoplitedDate.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
		{
			ucError.ErrorMessage = "Completed Date should be earlier than Current Date";
		}
		return (!ucError.IsError);
	}
	protected void lnkCreateRA_Click1(object sender, EventArgs e)
	{
		//ViewState["WORKORDERID"] = "";
		//ViewState["WORKORDERNO"] = "";
		//ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "redirect", "parent.location.href='../Inspection/InspectionRAMachinery.aspx?status=&RAjob=1&WORKORDERID=" + ViewState["WORKORDERID"].ToString() + "&WORKORDERNO=" + ViewState["WORKORDERNO"].ToString() + "'", true);
	}

	protected void imgShowRA_Click(object sender, ImageClickEventArgs e)
	{
		string script = string.Format("javascript:showPickList('spnRA', 'codehelp1', '', '../Common/CommonPickListMachineryRACopyforPmsJob.aspx?catid=3&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&status=4,5&ComponentId=" + ViewState["COMPONENTID"].ToString() + "&JobId=" + ViewState["JOBID"].ToString() + "&RaId=" + ViewState["RaId"].ToString() + "&WorkorderId="+ ViewState["WORKORDERID"].ToString() +"', true);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
	}
	protected void cmdRA_Click(object sender, ImageClickEventArgs e)
	{
		if (txtRAId.Text != "")
		{
			string scriptpopup = string.Format("javascript:parent.Openpopup('codehelp1', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + txtRAId.Text + "&showmenu=0&showexcel=NO');");
			ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);
		}
	}

	protected void lnkCreateRA_Click(object sender, EventArgs e)
	{
		string raStatus;
		string RAcreatedIn;
		if (txtRAId.Text != "")
		{
			DataTable dt = PhoenixPlannedMaintenanceWorkOrder.GetRaStatus(new Guid(txtRAId.Text));
			if (dt.Rows.Count > 0)
			{
				DataRow dr = dt.Rows[0];
				raStatus = dr["FLDSTATUS"].ToString();
				RAcreatedIn = dr["FLDISCREATEDBYOFFICE"].ToString();
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "redirect", "parent.location.href='../Inspection/InspectionRAMachinery.aspx?FromWorkorder=1&status=" + raStatus + "&machineryid=" + txtRAId.Text + "&RaCreatedIn=" + RAcreatedIn + "&PAGENUMBER=" + ViewState["PAGENUMBER"] + "&WORKORDERID=" + ViewState["WORKORDERID"] + "'", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "redirect", "parent.location.href='../Inspection/InspectionRAMachineryDetails.aspx?FromWorkorder=1&status=" + raStatus + "&machineryid=" + txtRAId.Text + "&RaCreatedIn=" + RAcreatedIn + "&PAGENUMBER=" + ViewState["PAGENUMBER"] + "&WORKORDERID=" + ViewState["WORKORDERID"] + "'", true);
                }
			}
		}
		else
		{
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "redirect", "parent.location.href='../Inspection/InspectionRAMachinery.aspx?status=&FromWorkorder=1&WORKORDERID=" + ViewState["WORKORDERID"].ToString() + "&PAGENUMBER=" + ViewState["PAGENUMBER"].ToString() + "'", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "redirect", "parent.location.href='../Inspection/InspectionRAMachineryDetails.aspx?status=&FromWorkorder=1&WORKORDERID=" + ViewState["WORKORDERID"].ToString() + "&PAGENUMBER=" + ViewState["PAGENUMBER"].ToString() + "'", true);
            }
		}
		
	}
	protected void cmdHiddenPick_Click(object sender, EventArgs e)
	{

		if (txtRAId.Text == "")
		{
			cmdRA.Visible = false;
			lnkCreateRA.Text = "Create";
		}
		else
		{
			cmdRA.Visible = true;
			lnkCreateRA.Text = "Edit/View";
		}

	}

    private void RAhide (bool val)
    {
        lblRA.Visible = val;
        txtRANumber.Visible = val;
        txtRA.Visible = val;
        imgShowRA.Visible = val;
        cmdRA.Visible = val;
        lnkCreateRA.Visible = val;
    }
}
