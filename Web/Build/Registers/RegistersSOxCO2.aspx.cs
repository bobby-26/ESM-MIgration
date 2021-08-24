using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersSOxCO2 : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (Request.QueryString["SOxCO2Id"] != null)
            {
                toolbar.AddButton("Save", "SAVE");
                ViewState["SOxCO2Id"] = Request.QueryString["SOxCO2Id"].ToString();

                SOxEdit(new Guid(Request.QueryString["SOxCO2Id"].ToString()));
            }
            else
            {
                toolbar.AddButton("Save", "SAVE");
            }
            MenuSOx.AccessRights = this.ViewState;
            MenuSOx.MenuList = toolbar.Show();
        }
    }

    private bool IsValidSOx()
    {
        if (General.GetNullableGuid(ucTypeOfFuel.SelectedOilType) == null)
            ucError.ErrorMessage = "Type of Fuel is required.";

        if (ddlLocation.SelectedValue == "")
            ucError.ErrorMessage = "Location is required.";

        return (!ucError.IsError);
    }

    protected void SOx_TabStripCommand(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('codehelp1', null, null);";
        Script += "</script>" + "\n";

        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            try
            {
                if (!IsValidSOx())
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["SOxCO2Id"] != null)
                {
                    PhoenixRegistersSOxCO2.UpdateSOXCO2(
                        new Guid((ViewState["SOxCO2Id"].ToString())),
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableDecimal(rblBSLimit.SelectedValue),
                        General.GetNullableDecimal(txtWeightage.Text),
                        ddlLocation.SelectedValue,
                        General.GetNullableGuid(ucTypeOfFuel.SelectedOilType),
                        txtReference.Text,
                        General.GetNullableDecimal(txtCarbonContent.Text),
                        General.GetNullableDecimal(txtCf.Text));
                }
                else
                {
                    PhoenixRegistersSOxCO2.InsertSOXCO2(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableDecimal(rblBSLimit.SelectedValue),
                        General.GetNullableDecimal(txtWeightage.Text),
                        ddlLocation.SelectedValue,
                        General.GetNullableGuid(ucTypeOfFuel.SelectedOilType),
                        txtReference.Text,
                        General.GetNullableDecimal(txtCarbonContent.Text),
                        General.GetNullableDecimal(txtCf.Text));
                }
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
    }

    private void SOxEdit(Guid soxco2id)
    {
        DataSet ds = PhoenixRegistersSOxCO2.EditSOXCO2(soxco2id);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtWeightage.Text = dr["FLDWEIGHTAGE"].ToString();
            txtReference.Text = dr["FLDREFERENCE"].ToString();
            txtCarbonContent.Text = dr["FLDCARBONCONTENT"].ToString();
            txtCf.Text = dr["FLDCF"].ToString();

            rblBSLimit.SelectedValue = dr["FLDBSLIMIT"].ToString();
            ddlLocation.SelectedValue = dr["FLDLOCATION"].ToString();
            ucTypeOfFuel.SelectedOilType = dr["FLDOILTYPE"].ToString();
        }
    }
}
