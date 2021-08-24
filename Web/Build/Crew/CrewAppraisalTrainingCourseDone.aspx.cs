using System;
using System.Data;
using System.Web.UI;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewAppraisalTrainingCourseDone : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["CURRENTINDEX"] = 0;
            ViewState["empId"] = Request.QueryString["empid"].ToString();
            ViewState["SIGNONOFFID"] = Request.QueryString["SIGNONOFFID"].ToString();
            ViewState["SIGNONDATE"] = Request.QueryString["SIGNONDATE"].ToString();
        }
    }

    protected void BindData()
    {
        try
        {

            DataTable dt = PhoenixCrewAppraisal.ListAppraisalCourseDone(int.Parse(ViewState["empId"].ToString()), General.GetNullableInteger(ViewState["SIGNONOFFID"].ToString()), General.GetNullableDateTime(ViewState["SIGNONDATE"].ToString()));
            gvCrewAppraisalTraining.DataSource = dt;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewAppraisalTraining_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}