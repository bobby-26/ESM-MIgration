using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewCommon;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
using SouthNests.Phoenix.CrewOffshore;

public partial class CrewOffshore_CrewOffshoreAddressDetailsFrame : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            SetEmployeeAddress();
        }
    }
    public void SetEmployeeAddress()
    {
        try
        {
            DataTable dt = PhoenixCrewAddress.ListEmployeeAddress(Convert.ToInt32(Filter.CurrentCrewSelection));
            if (dt.Rows.Count > 0)
            {

                txtAddressLine1.Text = dt.Rows[0]["FLDADDRESS1"].ToString()+","+
                                        dt.Rows[0]["FLDADDRESS2"].ToString() + "," +
                                        dt.Rows[0]["FLDADDRESS3"].ToString() + "," +
                                        dt.Rows[0]["FLDADDRESS4"].ToString();

                txtcountry.Text = dt.Rows[0]["FLDCOUNTRYNAME"].ToString();
               txtstate.Text = dt.Rows[0]["FLDSTATENAME"].ToString();
                txtcity.Text = dt.Rows[0]["FLDCITYNAME"].ToString();
                txtpostalcode.Text = dt.Rows[0]["FLDPOSTALCODE"].ToString();
                txtphno.Text  = dt.Rows[0]["FLDISDCODE"].ToString() + dt.Rows[0]["FLDPHONENUMBER"].ToString();
                txtemail.Text = dt.Rows[0]["FLDEMAIL"].ToString();
                txtmobile.Text = dt.Rows[0]["FLDMOBILENUMBER"].ToString();
                ViewState["EMPLOYEEPERMANENTADDRESSID"] = dt.Rows[0]["FLDEMPLOYEEADDRESSID"].ToString();
                

                txtLocalAddressLine1.Text = dt.Rows[1]["FLDADDRESS1"].ToString() + "," +
                                            dt.Rows[1]["FLDADDRESS2"].ToString() + "," +
                                            dt.Rows[1]["FLDADDRESS3"].ToString() + "," +
                                            dt.Rows[1]["FLDADDRESS4"].ToString();

                txtlocalcountry.Text = dt.Rows[1]["FLDCOUNTRYNAME"].ToString();
                txtlocalstate.Text = dt.Rows[1]["FLDSTATENAME"].ToString();
                txtlocalcity.Text = dt.Rows[1]["FLDCITYNAME"].ToString();
                txtlocalpostalcode.Text = dt.Rows[1]["FLDPOSTALCODE"].ToString();
                txtlocalphno.Text = dt.Rows[1]["FLDISDCODE"].ToString() + dt.Rows[1]["FLDPHONENUMBER"].ToString();
                ViewState["EMPLOYEELOCALADDRESSID"] = dt.Rows[1]["FLDEMPLOYEEADDRESSID"].ToString();
                txtlocalmobile.Text = dt.Rows[1]["FLDMOBILENUMBER"].ToString();

                //txtPhoneNumber2.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                //txtPhoneNumber2.Text = dt.Rows[0]["FLDPHONENUMBER2"].ToString();
                //txtMobileNumber2.Text = dt.Rows[0]["FLDMOBILENUMBER2"].ToString();
                //txtMobileNumber3.Text = dt.Rows[0]["FLDMOBILENUMBER3"].ToString();
                //ucAirport.SelectedAirport = dt.Rows[0]["FLDNEARESTAIRPORT"].ToString();

                //txtLocalPhoneNumber2.ISDCode = dt.Rows[1]["FLDISDCODE"].ToString();
                //txtLocalPhoneNumber2.Text = dt.Rows[1]["FLDPHONENUMBER2"].ToString();
                //txtLocalMobileNumber2.Text = dt.Rows[1]["FLDMOBILENUMBER2"].ToString();
                //txtLocalMobileNumber3.Text = dt.Rows[1]["FLDMOBILENUMBER3"].ToString();
                //ucLocRelation.SelectedQuick = dt.Rows[1]["FLDRELATIONNO"].ToString();
                //txtLastUpdatedBy.Text = dt.Rows[0]["FLDLASTMODIFIEDBY"].ToString();
                //txtLastUpdateDate.Text = string.Format("{0:dd/MMM/yyy}", dt.Rows[0]["FLDMODIFIEDDATE"]);
                //ddlPortofEngagement.SelectedSeaport = dt.Rows[0]["FLDPORTOFENGAGEMENT"].ToString();
            }
        }
        catch
        {
           
        }
    }
}