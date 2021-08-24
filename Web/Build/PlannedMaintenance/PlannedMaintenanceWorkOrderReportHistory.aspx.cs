using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderReportHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuJobDetail.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes["style"] = "display:none";
            ViewState["WORKORDERREPORTID"] = "";
            ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];
            ViewState["FORMURL"] = string.Empty;
            ViewState["DONEID"] = null;
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            BindFields();
        }
    }    
    protected void MenuJobDetail_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if(!IsValidDetail())
                {
                    ucError.Visible = true;
                    return;
                }
                if ((ViewState["WORKORDERREPORTID"] != null) && (ViewState["WORKORDERREPORTID"].ToString() != ""))
                {
                    PhoenixPlannedMaintenanceWorkOrderReport.UpdateWorkOrderReportHistory
                    (
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["WORKORDERREPORTID"].ToString()), "0", txtJobDetail.Content.ToString()
                    );
                    ucStatus.Text = "Details Saved.";

                }
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

            if ((ViewState["WORKORDERID"] != null) && (ViewState["WORKORDERID"].ToString() != ""))
            {                
                DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.EditWorkOrderReport(new Guid(ViewState["WORKORDERID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtJobDetail.Content = dr["FLDHISTORY"].ToString();
                    ViewState["WORKORDERREPORTID"] = dr["FLDWORKORDERREPORTID"].ToString();
                    ViewState["REPORTFORMURL"] = dr["FLDFORMURL"].ToString();
                    ViewState["dtkey"] = dr["FLDDTKEY"].ToString();
				}
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
        try
        {
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidDetail()
    {
        ucError.HeaderMessage = "Please provide the following required information";        
        if (General.SanitizeHtml(txtJobDetail.Content.ToString()).Length < 10)
            ucError.ErrorMessage = "Enter minimum of 10 characters";        
        return (!ucError.IsError);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
