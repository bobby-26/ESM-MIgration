using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using Telerik.Web.UI;
public partial class InspectionPurchaseFormAddress : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        BindField();
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ddlState.Enabled = false;
            BindField();
        }
    }
    protected void BindField()
    {
        try
        {
            if (!string.IsNullOrEmpty(Request.QueryString["ADDRESSCODE"].ToString()))
            {
                Int64 addresscode = Convert.ToInt64(Request.QueryString["ADDRESSCODE"].ToString());
                DataSet dsaddress = PhoenixRegistersAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode, addresscode);

                if (dsaddress.Tables.Count > 0)
                {
                    DataRow draddress = dsaddress.Tables[0].Rows[0];

                    txtName.Text = draddress["FLDNAME"].ToString();
                    txtAddress1.Text = draddress["FLDADDRESS1"].ToString();
                    txtAddress2.Text = draddress["FLDADDRESS2"].ToString();
                    txtAddress3.Text = draddress["FLDADDRESS3"].ToString();
                    txtAddress4.Text = draddress["FLDADDRESS4"].ToString();
                    ucCountry.SelectedCountry = draddress["FLDCOUNTRYID"].ToString();
                    txtPostalCode.Text = draddress["FLDPOSTALCODE"].ToString();
                    txtPhone1.Text = draddress["FLDPHONE1"].ToString();
                    txtPhone1.ISDCode = draddress["FLDISDCODE"].ToString();
                    txtPhone2.Text = draddress["FLDPHONE2"].ToString();
                    txtFax1.Text = draddress["FLDFAX1"].ToString();
                    txtFax2.Text = draddress["FLDFAX2"].ToString();
                    txtEmail1.Text = draddress["FLDEMAIL1"].ToString();
                    txtEmail2.Text = draddress["FLDEMAIL2"].ToString();
                    txtURL.Text = draddress["FLDURL"].ToString();

                    ddlState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ucCountry.SelectedCountry));
                    ddlState.SelectedState = draddress["FLDSTATE"].ToString();

                    ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ucCountry.SelectedCountry), General.GetNullableInteger(ddlState.SelectedState));

                    ddlCity.SelectedCity = draddress["FLDCITY"].ToString();
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }    

}
