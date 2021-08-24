using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;

public partial class DashboardCrewAppraisalToolTip : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["Empid"] != null)
                BindData(int.Parse(Request.QueryString["Empid"].ToString()));
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private void BindData(int empid)
    {
        DataTable dt = new DataTable();
        dt = PhoenixDashboardTooltip.CrewAppraisal(empid);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            lblFileNoValue.Text = dr["FLDFILENO"].ToString();
            lblEmpIdValue.Text = dr["FLDEMPLOYEEID"].ToString();
            lblEmpNameValue.Text = dr["FLDNAME"].ToString();
            lblranknamevalue.Text =  dr["FLDRANKNAME"].ToString();

        }
    }
}
