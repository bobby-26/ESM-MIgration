using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;


public partial class RegistersToolTipVesselPrincipalDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["vesselid"] != null)
                BindData(Request.QueryString["vesselid"].ToString());
        }
        catch { }
    }

    private void BindData(string vesselid)
    {
        if (vesselid == "")
            vesselid = "0";

        DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(vesselid));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lbltxtShipName.Text = dr["FLDVESSELNAME"].ToString();
            lbltxtTakeoverDate.Text = General.GetDateTimeToString(dr["FLDORGDATELEFT"].ToString());
            lbltxtBuildDate.Text = General.GetDateTimeToString(dr["FLDDATELEFT"].ToString());
            lbltxtOwner.Text = dr["FLDOWNERNAME"].ToString();
            lbltxtRegisteredOwner.Text = dr["FLDPRINCIPALNAME"].ToString();
            lbltxtFlag.Text = dr["FLDFLAGNAME"].ToString();
            lbltxtClassificationSociety.Text = dr["FLDCLASSNAMEVALUE"].ToString();
            lbltxtBHP.Text = dr["FLDBHP"].ToString();
            lbltxtCharterer.Text = dr["FLDCHARTERERNAME"].ToString();
            lbltxtManager.Text = dr["FLDPRIMARYMANAGERNAME"].ToString();
        }
    }
}
