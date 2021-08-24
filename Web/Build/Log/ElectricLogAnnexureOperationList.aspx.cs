using SouthNests.Phoenix.Framework;
using System;
using System.Web.UI;

public partial class Log_ElectricLogAnnexureOperationList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnFuelChangeOver.Visible = SessionUtil.CanAccess(this.ViewState, "FUEL");
        btnNoX.Visible = SessionUtil.CanAccess(this.ViewState, "NOX");
        btnEngineParameter.Visible = SessionUtil.CanAccess(this.ViewState, "ENGINE");
        btnODS.Visible = SessionUtil.CanAccess(this.ViewState, "ODS");
        if (!IsPostBack)
        {
            setButtonClick();
        }

    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void setButtonClick()
    {
        btnFuelChangeOver.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('oilLog','','{0}/Log/ElectricLogFuelChangeOverOperationList.aspx', true, null, null, null, null, {{'closeAlert': true }});", Session["sitepath"]));
        btnNoX.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('oilLog','','{0}/Log/ElectricLogNoXOperationList.aspx', true, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnEngineParameter.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectronicLogAnnexVIEngineParameters.aspx', true, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnODS.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectronicLogAnnexVIODS.aspx', true, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
    }


    protected void lnkConfiguration_Click(object sender, EventArgs e)
    {
        string script = string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogAnnexureConfig.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]);
        ScriptManager.RegisterClientScriptBlock(this, this.Page.GetType(), "configClick", script, true);
    }
}