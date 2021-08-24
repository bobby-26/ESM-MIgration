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

public partial class Crew_CrewAppraisalKPIScorelist : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["CURRENTINDEX"] = 0;
            if (Request.QueryString["CAID"] == null)
            {
                ViewState["empId"] = Request.QueryString["empid"].ToString();
                ViewState["SIGNONOFFID"] = Request.QueryString["SIGNONOFFID"].ToString();
                ViewState["SIGNONDATE"] = Request.QueryString["SIGNONDATE"].ToString();
            }
        }
    }
    protected void gvCrewAppraisalkpi_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void BindData()
    {
        try
        {
            if (Request.QueryString["CAID"] != null)
            {
                DataSet ds = PhoenixCrewAppraisal.kpiscorehistorylist(General.GetNullableGuid(Request.QueryString["CAID"].ToString()));
                gvCrewAppraisalkpi.DataSource = ds.Tables[0];
                gvkpiincident.DataSource = ds.Tables[1];
            }
            else
            {
                float totalcount = 0;
                DataSet ds = PhoenixCrewAppraisal.AppraisalKPIScore(int.Parse(ViewState["empId"].ToString()), General.GetNullableInteger(ViewState["SIGNONOFFID"].ToString()), ref totalcount);
                gvCrewAppraisalkpi.DataSource = ds.Tables[0];
                gvkpiincident.DataSource = ds.Tables[1];
                gvkpiincident.DataBind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}