using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web;
using Telerik.Web.UI;
public partial class CrewPlanRelieveeInfo : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        NameValueCollection nvc = HttpUtility.ParseQueryString(Request.QueryString.ToString().Replace("amp;", "").Replace("amp%3b", ""));
        ViewState["SIGNONOFFID"] = nvc["signonoffid"];
        ViewState["CREWPLANID"] = nvc["crewplanid"];
        if (ViewState["CREWPLANID"] != null)
            Edit();
    }
    private void Edit()
    {
        DataTable dt = null;
        dt = PhoenixCrewPlanning.infoCrewPlan(General.GetNullableInteger(ViewState["SIGNONOFFID"].ToString()),General.GetNullableGuid(ViewState["CREWPLANID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            txtonsignername.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
            txtVessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
            txtoffsignername.Text = dt.Rows[0]["FLDOFFSIGNERNAME"].ToString();
            txtoffsignerRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
            ucport.Text = dt.Rows[0]["FLDSEAPORTNAME"].ToString();
            txtPlannedReliefDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDEXPECTEDJOINDATE"].ToString());
            txtcontractExpired.Text = General.GetDateTimeToString(dt.Rows[0]["FLDCONTRACTEXPRIEDDATE"].ToString());
            txtDateofReadiness.Text = General.GetDateTimeToString(dt.Rows[0]["FLDDATEOFREADINESS"].ToString());
            txtcrewchangeremarks.Text = dt.Rows[0]["FLDCREWCHANGEREMARKS"].ToString();
 
        }
    }

 }
