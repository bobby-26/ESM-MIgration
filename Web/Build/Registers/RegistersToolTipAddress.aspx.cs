using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersToolTipAddress : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (General.GetNullableInteger(Request.QueryString["addresscode"]) != null)
                BindData(long.Parse(Request.QueryString["addresscode"].ToString()));
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private void BindData(long addresscode)
    {
        
        DataSet ds = new DataSet();
        ds = PhoenixRegistersAddress.EditAddressToolTip(addresscode);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lblName.Text=dr["FLDNAME"].ToString();
            lblAddress.Text = dr["FLDADDRESS1"].ToString();
            lblCity.Text = dr["FLDCITYNAME"].ToString();
            lblState.Text=dr["FLDSTATENAME"].ToString();
            lblCountry.Text=dr["FLDCOUNTRYNAME"].ToString();
            lblEmail.Text=dr["FLDEMAIL1"].ToString();
            lblPhone.Text=dr["FLDPHONE1"].ToString();
            
       }
    }
}
