using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionEUMRVPrimaryManager : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {

                SessionUtil.PageAccessRights(this.ViewState);

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                if (Request.QueryString["Lanchfrom"].ToString() == "0")
                    toolbarmain.AddButton("EUMRV Procedure", "PROCEDUREDETAIL",ToolBarDirection.Right);

                MenuProcedureDetailList.AccessRights = this.ViewState;
                MenuProcedureDetailList.MenuList = toolbarmain.Show();

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
                MenuCompanyInformation.AccessRights = this.ViewState;
                MenuCompanyInformation.MenuList = toolbar.Show();

                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            DataSet ds = PhoenixVesselPositionEUMRV.EUMRVBasicData(0);
            if (ds.Tables[1].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[1];
                txtNameofthecompany.Text = dt.Rows[0]["FLDCOMPANYNAME"].ToString();
                txtIMONo.Text= dt.Rows[0]["FLDIMONUMBER"].ToString();
                txtAddress1.Text = dt.Rows[0]["FLDADDRESS1"].ToString();
                txtAddress2.Text = dt.Rows[0]["FLDADDRESS2"].ToString();
                txtCity.Text = dt.Rows[0]["FLDCITYNAME"].ToString();
                txtStateProvinceRegion.Text = dt.Rows[0]["FLDSTATENAME"].ToString();
                txtPostalCode.Text = dt.Rows[0]["FLDPOSTALCODE"].ToString();
                txtCountry.Text = dt.Rows[0]["FLDCONTRY"].ToString();
                txtContactPerson.Text = dt.Rows[0]["FLDINCHARGE"].ToString();
                txtTelephoneNumber.Text = dt.Rows[0]["FLDPHONE2"].ToString();
                txtEmailAddress.Text = dt.Rows[0]["FLDEMAIL1"].ToString();


            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    protected void MenuCompanyInformation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixVesselPositionEUMRV.InsertCompanyInformation(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableString(txtContactPerson.Text),
                General.GetNullableString(txtTelephoneNumber.Text),
                General.GetNullableString(txtEmailAddress.Text)
                );
                ucStatus.Text = "Saved Successfully";
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void MenuProcedureDetailList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("PROCEDUREDETAIL"))
        {
            if (Request.QueryString["Lanchfrom"].ToString() == "1")
                Response.Redirect("../VesselPosition/VesselPositionEUMRVShipSpecificProcedure.aspx?Lanchfrom=1");
            else
                Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedure.aspx?Lanchfrom=0");
        }
    }
}
