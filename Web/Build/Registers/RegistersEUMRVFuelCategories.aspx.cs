using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersEUMRVFuelCategories : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Distribute", "SAVE",ToolBarDirection.Right);
        MenuFuelTypeMain.AccessRights = this.ViewState;
        MenuFuelTypeMain.MenuList = toolbar.Show();

        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {

        }
    }

    private void BindData()
    {
        try
        {
            int rowcont=0, pagecount=0;
            DataSet ds = PhoenixRegistersEUMRVFuelType.EUFuelTypeSearch(null,null,null,null,null,1, General.ShowRecords(null), ref rowcont, ref pagecount);
            gvEUMRVFuelType.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuFuelTypeMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixRegistersEUMRVFuelType.SendFuelTypetoOffice(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ucStatus.Text = "Data refreshed successfully.";
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEUMRVFuelType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
