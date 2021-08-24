using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI.Gantt;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CrewEmployeeSignOnOffGanttChart : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["VesselID"] != null)
            {
                ViewState["VESSELID"] = Request.QueryString["VesselID"].ToString();
                GanttEmployeeSignOnOff(int.Parse(Request.QueryString["VesselID"].ToString()));
            }
        }
    }

    private void GanttEmployeeSignOnOff(int Vesselid)
    {
        try
        {

            DataSet ds;

            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
            {
                ds = PhoenixCrewOffshoreCrewList.GanttEmployeeSignOnOff(Vesselid);                
            }
            else
            {

                ds = PhoenixCrewManagement.GanttEmployeeSignOnOff(Vesselid);
            }

            RadGantt1.DataSource = ds;
            RadGantt1.DataBind();

            if (ds.Tables[0].Rows.Count > 1)
            {
                RadGantt1.DataBindings.TasksDataBindings.IdField = ds.Tables[0].Rows[0]["FLDID"].ToString();             
                RadGantt1.DataBindings.TasksDataBindings.TitleField = ds.Tables[0].Rows[0]["FLDNAME"].ToString();
                RadGantt1.DataBindings.TasksDataBindings.StartField = ds.Tables[0].Rows[0]["FLDSTARTDATE"].ToString();
                RadGantt1.DataBindings.TasksDataBindings.EndField = ds.Tables[0].Rows[0]["FLDENDDATE"].ToString();
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}