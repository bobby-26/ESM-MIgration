using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;

public partial class InspectionToolTipUnsafeActsConditions : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["directincidentid"] != null)
                BindInspectionIncident();        
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }

    }

    private void BindInspectionIncident()
    {
        if (Request.QueryString["directincidentid"] != null && !string.IsNullOrEmpty(Request.QueryString["directincidentid"].ToString()))
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionUnsafeActsConditions.DirectIncidentEdit(new Guid(Request.QueryString["directincidentid"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                lbltNumber.Text = dr["FLDREFERENCENUMBER"].ToString();
                lbltCorrectiveAction.Text = dr["FLDACTIONTOBETAKEN"].ToString();
                txtRootCause.Text = dr["FLDROOTCAUSENAMELIST"].ToString();
            }
        }
    }
}
