using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Crew_CrewBatchMoreInfoList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["batchId"] != null)
                {
                    ViewState["batchId"] = Request.QueryString["batchId"].ToString();
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
    protected void BindData()
    {
        DataTable dt = PhoenixCrewInstituteBatch.CrewBatchDurationEdit(null, new Guid(ViewState["batchId"].ToString()));

        if (dt.Rows.Count > 0)
        {
            txtStartDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDSTARTDATE"].ToString());
            txtEndDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDENDDATE"].ToString());
            txtCourse.Text = dt.Rows[0]["FLDCOURSE"].ToString();
        }
    }
}