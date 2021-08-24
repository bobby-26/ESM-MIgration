using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Southnests.Phoenix.AdministrationAssetAMC;
public partial class Registers_RegistersAdminAmcDoneDate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE");
        MenuAssetMenu.AccessRights = this.ViewState;
        MenuAssetMenu.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            if(Request.QueryString["AMCID"] != null)
                ViewState["AMCID"] = Request.QueryString["AMCID"];

            if (Request.QueryString["ASSETID"] != null)
                ViewState["ASSETID"] = Request.QueryString["ASSETID"];

            if (Request.QueryString["ADDRESSID"] != null)
                ViewState["ADDRESSID"] = Request.QueryString["ADDRESSID"];

            if (Request.QueryString["DURATION"] != null)
                ViewState["DURATION"] = Request.QueryString["DURATION"];

            if (Request.QueryString["POREFERENCE"] != null)
                ViewState["POREFERENCE"] = Request.QueryString["POREFERENCE"];

            if (Request.QueryString["ASSETNAME"] != null)
                txtAssetName.Text= Request.QueryString["ASSETNAME"];

            if (Request.QueryString["ZONEID"] != null)
                ViewState["ZONEID"] = Request.QueryString["ZONEID"];
        }
    }

    protected void MenuAssetMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if ((ViewState["AMCID"] != null))
            {
                lblMessage.Text = "";
                if (dce.CommandName.ToUpper().Equals("SAVE"))
                {
                    if (General.GetNullableDateTime(ucDoneDate.Text) == null)
                    {
                        lblMessage.Text = "Done Date is Required.";
                        return;
                    }
                    PhoenixAdministrationAssetAMC.UpdateAssetAMCDoneDate
                        (
                            new Guid(ViewState["AMCID"].ToString())
                            ,new Guid(ViewState["ASSETID"].ToString())
                            ,int.Parse(ViewState["ADDRESSID"].ToString())
                            ,ViewState["POREFERENCE"].ToString()
                            ,int.Parse(ViewState["DURATION"].ToString())
                            ,General.GetNullableDateTime(ucDoneDate.Text)
                            ,1
                            ,General.GetNullableInteger(ViewState["ZONEID"].ToString())
                        );
                    lblMessage.Text = "Done Date Updated Successfully.";
                    String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }
}
