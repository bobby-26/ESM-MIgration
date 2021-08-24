using System;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Text;
using SouthNests.Phoenix.Inspection;

public partial class Inspection_InspectionDrilldoneedit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            
            ViewState["DRILLSCHEDULEID"] = General.GetNullableGuid(Request.QueryString["drillscheduleid"]);
            Guid? drillscheduleid = General.GetNullableGuid(ViewState["DRILLSCHEDULEID"].ToString());
            DataTable dt = PhoenixInspectionDrillSchedule.drillscheduleeditlist(drillscheduleid);
            if (dt.Rows.Count > 0)
            {
                radduedate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDDRILLDUEDATE"].ToString()); ;
                radlastdonedate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDDRILLLASTDONEDATE"].ToString());

            }
        }

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Save", "SAVE", ToolBarDirection.Right);

        Tabstripdrillschedule.MenuList = toolbargrid.Show();
    }

    protected void Tabstripdrillschedule_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? drillscheduleid = General.GetNullableGuid(ViewState["DRILLSCHEDULEID"].ToString());
               
                DateTime? duedate = General.GetNullableDateTime(radduedate.Text);
                DateTime? Lastdonedate = General.GetNullableDateTime(radlastdonedate.Text);

                if (!IsValidDrillDetails(duedate, Lastdonedate))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionDrillSchedule.DrillscheduleLastdoneupdate(rowusercode, drillscheduleid, Lastdonedate, duedate);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private bool IsValidDrillDetails(DateTime? duedate, DateTime? Lastdonedate)
    {

        ucError.HeaderMessage = "Please provide the following required information";


        if (duedate == null )
        {
            ucError.ErrorMessage = "Drill Due Date";
        }

        if (Lastdonedate == null )
        {
            ucError.ErrorMessage = "Drill Last Done Date";
        }


        return (!ucError.IsError);
    }
}