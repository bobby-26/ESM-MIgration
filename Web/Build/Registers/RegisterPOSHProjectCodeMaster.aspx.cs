using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Registers_RegisterPOSHProjectCodeMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar ProjectMasterTabs = new PhoenixToolbar();
        ProjectMasterTabs.AddButton("Project Code", "PROJECT");
        ProjectMasterTabs.AddButton("Line Item", "LINEITEM");

        cmdHiddenSubmit.Attributes.Add("style", "Display:None;");

        MenuProjectMaster.AccessRights = this.ViewState;
        MenuProjectMaster.MenuList = ProjectMasterTabs.Show();        
        MenuProjectMaster.SelectedMenuIndex = 0;  
        ifMoreInfo.Attributes["src"] = "../Registers/RegisterPOSHProjectCode.aspx";
        if (!IsPostBack)
        {            
            Session["PROJECTID"] = "0";
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (Session["PROJECTID"].ToString() != "0")
        {
            ifMoreInfo.Attributes["src"] = "../Registers/RegisterPOSHProjectLineItemPurchaseOrder.aspx?id=" + Session["PROJECTID"].ToString();
            MenuProjectMaster.SelectedMenuIndex = 1;
        }
    }

    //private void EditAccount()
    //{
    //    DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(Session["ACCOUNTID"].ToString()));
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        DataRow dr = ds.Tables[0].Rows[0];
    //        ViewState["USAGE"] = dr["FLDACCOUNTUSAGE"].ToString();
    //        ViewState["ACCOUNTLEVEL"] = dr["FLDACCOUNTLEVEL"].ToString();
    //    }
    //}

    protected void ProjectMaster_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("PROJECT"))
        {
            ifMoreInfo.Attributes["src"] = "../Registers/RegisterPOSHProjectCode.aspx?projectid=" + Session["PROJECTID"].ToString();
        }
        if (CommandName.ToUpper().Equals("LINEITEM"))
        {

            ifMoreInfo.Attributes["src"] = "../Registers/RegisterPOSHProjectLineItemPurchaseOrder.aspx?id=" + Session["PROJECTID"].ToString();
            MenuProjectMaster.SelectedMenuIndex = 1;
        }
    }
}