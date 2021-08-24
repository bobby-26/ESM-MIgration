using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class Crew_CrewCostingToolTip : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["Positionid"] != null && Request.QueryString["Positionid"] != "")
                ViewState["Positionid"] = Request.QueryString["Positionid"].ToString();

            BindData();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private void BindData()
    {
 
        DataTable dt = new DataTable();

        string Positionid;

        if (ViewState["Positionid"] != null)
        {
            Positionid = ViewState["Positionid"].ToString();
        }
        else
        {
            Positionid = null;
        }

        dt = PhoenixCrewVesselPosition.EditCrewVesselPositionCost(General.GetNullableInteger(Positionid));

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            lbltPoNumber.Text = dr["FLDREQUESTNO"].ToString();
            lbltCrewChangeVessel.Text = dr["FLDVESSELNAME"].ToString();
            lbltCrewChangeDate.Text = General.GetDateTimeToString(dr["FLDCREWCHANGEDATE"].ToString());
            lbltPort.Text = dr["FLDSEAPORTNAME"].ToString();
            lbltPortAgent.Text = dr["FLDAGENTNAME"].ToString();
            lbltETA.Text = General.GetDateTimeToString(dr["FLDETA"].ToString());
            lbltETD.Text = General.GetDateTimeToString(dr["FLDETD"].ToString());
            lbltQuotation.Text = dr["FLDQUOTEREFNO"].ToString();
            lbltNumberOfOnSigners.Text = dr["FLDNOOFJOINER"].ToString();
            lbltNumberOfOffSigners.Text = dr["FLDNOOFOFFSIGNER"].ToString();
            lbltTotal.Text = dr["FLDACTUALTOTALAMOUNT"].ToString();
        }
    }
}