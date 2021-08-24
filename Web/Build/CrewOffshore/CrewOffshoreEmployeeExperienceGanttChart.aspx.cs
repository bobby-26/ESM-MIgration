using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI.Gantt;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;


public partial class CrewOffshoreEmployeeExperienceGanttChart : PhoenixBasePage
{
    string empid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
  
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            empid = Request.QueryString["empid"].ToString();
            DataSet ds = PhoenixCrewOffshoreOtherExperience.GanttEmployeeExperience(Convert.ToInt32(empid));

            RadGantt1.DataSource = ds;
            RadGantt1.DataBind();

            if (ds.Tables[0].Rows.Count > 1)
            {

                RadGantt1.DataBindings.TasksDataBindings.IdField = ds.Tables[0].Rows[0]["FLDID"].ToString();
                RadGantt1.DataBindings.TasksDataBindings.TitleField = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                RadGantt1.DataBindings.TasksDataBindings.StartField = ds.Tables[0].Rows[0]["FLDSTART"].ToString();
                RadGantt1.DataBindings.TasksDataBindings.EndField = ds.Tables[0].Rows[0]["FLDEND"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
