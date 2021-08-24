using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderReportGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarsav = new PhoenixToolbar();
            toolbarsav.AddButton("Confirm", "CONFIRM",ToolBarDirection.Right);
            toolbarsav.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuWorkOrderGeneral.MenuList = toolbarsav.Show();
            if (!IsPostBack)
            {
                ViewState["READDATE"] = null;
                ViewState["READVALUE"] = null;                
                ViewState["WORKORDERREPORTID"] = null;
                if (Request.QueryString["WORKORDERID"] != null)
                    ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
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

            if ((Request.QueryString["WORKORDERID"] != null) && (Request.QueryString["WORKORDERID"] != ""))
            {

                DataTable dt = PhoenixPlannedMaintenanceWorkOrderReport.ListWorkOrderReportLastCounter(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(Request.QueryString["WORKORDERID"]));
                if (dt.Rows.Count > 0)
                {
                    
                    txtReadDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDREADINGDATE"].ToString());
                    txtReadValue.Text = dt.Rows[0]["FLDCURRENTVALUE"].ToString();
                    txtCurrentValues.Text = dt.Rows[0]["FLDCURRENTVALUE"].ToString();                    
                    ViewState["READDATE"] = dt.Rows[0]["FLDREADINGDATE"].ToString() == string.Empty ? null : dt.Rows[0]["FLDREADINGDATE"].ToString();
                    ViewState["READVALUE"] = dt.Rows[0]["FLDCURRENTVALUE"].ToString() == string.Empty ? null : dt.Rows[0]["FLDCURRENTVALUE"].ToString();
                    //if (dt.Rows[0]["FLDHISTORYTEMPLATE"].ToString() == "1")
                    //{
                    //    chkHistory.Checked = true;
                    //}
                    if (dt.Rows[0]["FLDMANDATORY"].ToString().Equals("Y"))
                    {
                        chkHistory.Checked = true;
                        chkHistory.Enabled = false;
                    }
                    if (ViewState["READDATE"] == null) txtCurrentValues.ReadOnly = "true";
                }

                DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.EditWorkOrderReport(new Guid(Request.QueryString["WORKORDERID"]));
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["FLDWORKORDERREPORTID"].ToString() != string.Empty)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    //txtDueDate.Text = dr["FLDWORKDONEDATE"].ToString();
                    txtWorkDoneDate.Text = General.GetDateTimeToString(dr["FLDWORKDONEDATE"].ToString());
                    txtWorkDuration.Text = dr["FLDWORKDURATION"].ToString();
                    chkUnplanned.Checked = dr["FLDUNPLANNEDWORK"].ToString() == "1" ? true : false;
                    chkUsedParts.Checked = dr["FLDREPORTUSEDPARTS"].ToString() == "1" ? true : false;
                    if (dt.Rows.Count > 0 && dt.Rows[0]["FLDMANDATORY"].ToString().Equals("Y"))
                    {
                        chkHistory.Checked = true;
                        chkHistory.Enabled = false;
                    }
                    else
                    {
                        chkHistory.Checked = dr["FLDREPORTHISTORY"].ToString() == "1" ? true : false;
                    }
                    chkResources.Checked = dr["FLDREPORTUSEDRESOURCES"].ToString() == "1" ? true : false;
                    txtCurrentValues.Text = dr["FLDCURRENTVALUEs"].ToString();
                    ViewState["WORKORDERREPORTID"] = dr["FLDWORKORDERREPORTID"].ToString();
                    ViewState["OPERATIONMODE"] = "EDIT";
                    ucCounter.SelectedHard = "498";
                }
                else
                {
                    ViewState["OPERATIONMODE"] = "ADD";
                    ucCounter.SelectedHard = "498";
                    chkUsedParts.Checked = true;
                }
            }
            else
            {
                ViewState["OPERATIONMODE"] = "ADD";
                ucCounter.SelectedHard = "498";
                chkUsedParts.Checked = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
    }

    protected void MenuWorkOrderGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidComponent())
                {
                    ucError.Visible = true;
                    return;
                }

                string WorkDuration = txtWorkDuration.TextWithLiterals;

                if ((String)ViewState["OPERATIONMODE"] == "EDIT")
                {
                    PhoenixPlannedMaintenanceWorkOrderReport.UpdateWorkOrderReport(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , General.GetNullableGuid(ViewState["WORKORDERREPORTID"].ToString()), General.GetNullableDateTime(txtWorkDoneDate.Text)
                    , General.GetNullableDecimal(WorkDuration), General.GetNullableDecimal(txtCurrentValues.Text)
                    , null, chkUnplanned.Checked == true ? "1" : "0"
                    , null, chkUsedParts.Checked == true ? "1" : "0"
                    , chkResources.Checked == true ? "1" : "0", chkHistory.Checked == true ? "1" : "0"
                    , null);
                }
                else if ((String)ViewState["OPERATIONMODE"] == "ADD")
                {
                    PhoenixPlannedMaintenanceWorkOrderReport.InsertWorkOrderReport(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                     , General.GetNullableGuid(ViewState["WORKORDERID"].ToString()), General.GetNullableDateTime(txtWorkDoneDate.Text)
                    , General.GetNullableDecimal(WorkDuration), General.GetNullableDecimal(txtCurrentValues.Text)
                    , null, chkUnplanned.Checked == true ? "1" : "0"
                    , null, chkUsedParts.Checked == true ? "1" : "0"
                    , chkResources.Checked == true ? "1" : "0", chkHistory.Checked == true ? "1" : "0"
                    , null);
                }                
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            }
            else if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                if (ViewState["WORKORDERREPORTID"] == null || ViewState["WORKORDERREPORTID"].ToString() == string.Empty)
                {
                    ucError.ErrorMessage = "Save the Report Work before Confirm";
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceWorkOrderReport.WorkOrderReportConfirm(new Guid(ViewState["WORKORDERREPORTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                ucStatus.Text = "Work Order Closed";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidComponent()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (ViewState["WORKORDERID"] == null || ViewState["WORKORDERID"].ToString() == "")
            ucError.ErrorMessage = "Work Order is required";
        if ((General.GetNullableDateTime(txtWorkDoneDate.Text).ToString()).Equals("") || General.GetNullableDateTime(txtWorkDoneDate.Text).ToString() == null)
            ucError.ErrorMessage = "Date done  is required";
    
        if(DateTime.TryParse(txtWorkDoneDate.Text,out resultDate)
            && DateTime.Compare(resultDate, DateTime.Now) > 0)
            ucError.ErrorMessage = "Done Date should be earlier than Current Date.";

        if (ViewState["READDATE"] != null && ViewState["READVALUE"] != null
            && General.GetNullableDateTime(txtWorkDoneDate.Text).HasValue && General.GetNullableDecimal(txtCurrentValues.Text).HasValue)
        {
            DateTime donedate = DateTime.Parse(txtWorkDoneDate.Text);
            decimal counter = General.GetNullableDecimal(txtCurrentValues.Text).Value;
            TimeSpan datediff = donedate.Subtract(DateTime.Parse(ViewState["READDATE"].ToString()));
            decimal maxcounter = decimal.Parse(ViewState["READVALUE"].ToString());
            if (DateTime.Compare(donedate, DateTime.Parse(ViewState["READDATE"].ToString())) <= 0 && (counter > maxcounter || counter < (maxcounter + (datediff.Days * 26))))
            {
                ucError.ErrorMessage = "Counter should be between " + (maxcounter + (datediff.Days * 26)).ToString() + " to " + ViewState["READVALUE"].ToString();
            }
            else if (DateTime.Compare(donedate, DateTime.Parse(ViewState["READDATE"].ToString())) > 0 && (counter < maxcounter || counter > (maxcounter + (datediff.Days * 26))))
            {
                ucError.ErrorMessage = "Counter should be between " + ViewState["READVALUE"].ToString() + " to " + (maxcounter + (datediff.Days * 26)).ToString();
            }
        }
        return (!ucError.IsError);
    }

}
