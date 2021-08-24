using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;

public partial class InspectionToolTipDeficiency : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["deficiencyid"] != null && Request.QueryString["vesselid"] != null)
                BindData(Request.QueryString["deficiencyid"].ToString(), Request.QueryString["vesselid"].ToString());
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private void BindData(string deficiencyid, string vesselid)
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionDeficiency.DeficiencyEdit(new Guid(deficiencyid), General.GetNullableInteger(vesselid));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lbltNumber.Text = dr["FLDREFERENCENUMBER"].ToString();
            lblText.Text = dr["FLDDEFICIENCYDETAILS"].ToString();
            lbltCorrectiveAction.Text = dr["FLDCORRECTIVEACTION"].ToString();            
        }
    }
}
