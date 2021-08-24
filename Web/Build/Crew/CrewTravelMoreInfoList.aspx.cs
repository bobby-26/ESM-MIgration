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
using Telerik.Web.UI;
public partial class Crew_CrewTravelMoreInfoList : PhoenixBasePage
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
                    ViewState["familyId"] = Request.QueryString["familyId"].ToString();
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
        try
        {
            DataTable dt = PhoenixCrewTravelRequest.CrewTravelPlanMoreInfoList(General.GetNullableInteger(ViewState["empId"].ToString()), General.GetNullableInteger(ViewState["familyId"].ToString()));

            if (dt.Rows.Count > 0)
            {
                lblDOB.Text = General.GetDateTimeToString(dt.Rows[0]["FLDDATEOFBIRTH"].ToString());
                lblPPNo.Text = dt.Rows[0]["FLDPASSPORTNO"].ToString();
                lblCDC.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
                lblUSVisa.Text = dt.Rows[0]["FLDUSVISANUMBER"].ToString();
                lblOtherVisa.Text = dt.Rows[0]["FLDOTHERVISADETAILS"].ToString();
                lblZone.Text = dt.Rows[0]["FLDZONE"].ToString();
                lblAirport.Text = dt.Rows[0]["FLDAIRPORTNAME"].ToString();
                lblAirportCity.Text = dt.Rows[0]["FLDAIRPORTCITY"].ToString();
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
}
