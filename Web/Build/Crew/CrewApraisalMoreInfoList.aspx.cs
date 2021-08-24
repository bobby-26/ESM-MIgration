using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using SouthNests.Phoenix.CrewCommon;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewReports;
using Telerik.Web.UI;
public partial class CrewApraisalMoreInfoList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["empId"] != null)
                {
                    ViewState["empId"] = Request.QueryString["empId"].ToString();
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

        DataTable dt = PhoenixCrewAppraisalSummary.CrewAppraisalMoreinfoList(General.GetNullableInteger(ViewState["empId"].ToString()));

        if (dt.Rows.Count > 0)
        {
            lblLastVessel.Text = dt.Rows[0]["FLDLASTVESSELNAME"].ToString();
            lblSignOffDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDSIGNONDATE"].ToString());
            lblPresentVessel.Text = dt.Rows[0]["FLDPRESENTVESSELNAME"].ToString();
            lblSignOn.Text = General.GetDateTimeToString(dt.Rows[0]["FLDSIGNONDATE"].ToString());
            lblNextVessel.Text = dt.Rows[0]["FLDPRESENTVESSELNAME"].ToString();
            lblDOA.Text = General.GetDateTimeToString(dt.Rows[0]["FLDDOA"].ToString());
        }
    }
}
