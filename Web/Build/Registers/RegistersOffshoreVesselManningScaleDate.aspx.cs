using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersOffshoreVesselManningScaleDate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE");
            MenuDate.AccessRights = this.ViewState;
            MenuDate.MenuList = toolbarmain.Show();
        }
    }

    protected void MenuDate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (General.GetNullableDateTime(ucDate.Text) == null)
            {
                lblMessage.Text = "Effective Date is required.";
                return;
            }

            string Script = "";
            
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixRegistersVesselManningScale.UpdateManningScaleEffectiveDate(int.Parse(Filter.CurrentVesselMasterFilter),DateTime.Parse(ucDate.Text));

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    } 
}
