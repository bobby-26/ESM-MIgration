using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;

public partial class PlannedMaintenanceWorkOrderReportSimple : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarsav = new PhoenixToolbar();
            toolbarsav.AddButton("Save", "SAVE");
            toolbarsav.AddButton("Confirm", "CONFIRM");
            MenuWorkOrderGeneral.MenuList = toolbarsav.Show();  
            if (!IsPostBack)
            {
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
                DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.EditWorkOrderReport(new Guid(Request.QueryString["WORKORDERID"]));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    //txtDueDate.Text = dr["FLDWORKDONEDATE"].ToString();
                    txtWorkDoneDate.Text =General.GetDateTimeToString(dr["FLDWORKDONEDATE"].ToString());
                    txtWorkDuration.Text = dr["FLDWORKDURATION"].ToString();
                    chkCompleted.Checked = dr["FLDISCOMPLETED"].ToString() == "1" ? true : false;
                    chkUnplanned.Checked = dr["FLDUNPLANNEDWORK"].ToString() == "1" ? true : false;                 
                    ViewState["WORKORDERREPORTID"] = dr["FLDWORKORDERREPORTID"].ToString();
                    ViewState["OPERATIONMODE"] = "EDIT";
                }
                else
                {
                    ViewState["OPERATIONMODE"] = "ADD";
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

    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
    }

    protected void MenuWorkOrderGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidComponent())
                {
                    ucError.Visible = true;
                    return;
                }

                if ((String)ViewState["OPERATIONMODE"] == "EDIT")
                {

                    PhoenixPlannedMaintenanceWorkOrderReport.UpdateWorkOrderReport(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , General.GetNullableGuid(ViewState["WORKORDERREPORTID"].ToString()), General.GetNullableDateTime(txtWorkDoneDate.Text)
                    , General.GetNullableDecimal(txtWorkDuration.Text), General.GetNullableDecimal(txtCurrentValues.Text)
                    , chkCompleted.Checked ? "1" : "0", chkUnplanned.Checked ? "1" : "0"
                    , null,"0"
                    , "0","0"
                    , null); 
                   
                }

                if ((String)ViewState["OPERATIONMODE"] == "ADD")
                {
                    PhoenixPlannedMaintenanceWorkOrderReport.InsertWorkOrderReport(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                     , General.GetNullableGuid(ViewState["WORKORDERID"].ToString()), General.GetNullableDateTime(txtWorkDoneDate.Text)
                    , General.GetNullableDecimal(txtWorkDuration.Text), General.GetNullableDecimal(txtCurrentValues.Text)
                    , chkCompleted.Checked ? "1" : "0", chkUnplanned.Checked ? "1" : "0"
                    , null, "0"
                    , "0",  "0"
                    , null); 
                    
                }

                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            }
            if (dce.CommandName.ToUpper().Equals("CONFIRM"))
            {
              
                //PhoenixPlannedMaintenanceWorkOrderReport.ConfirmReports(PhoenixSecurityContext.CurrentSecurityContext.UserCode,General.GetNullableGuid(ViewState["WORKORDERID"].ToString(),General.GetNullableGuid(ViewState["WORKORDERREPORTID"].ToString()),PhoenixSecurityContext.CurrentSecurityContext.VesselID);
               
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
      if (ViewState["WORKORDERID"] == null || ViewState["WORKORDERID"].ToString() == "")
          ucError.ErrorMessage = "Work Oreder is required";
      if (txtWorkDoneDate.Text.Equals("") || General.GetNullableDateTime(txtWorkDoneDate.Text)==null)
          ucError.ErrorMessage = "Done date is required";
      if (txtWorkDuration.Text.Trim().Equals(""))
          ucError.ErrorMessage = "Work Dueration is required";  
       return (!ucError.IsError);
    }

}
