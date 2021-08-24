using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using Telerik.Web.UI;

public partial class InventorySpareItemTransactionFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Go", "GO", ToolBarDirection.Right);
            MenuStockItemFilter.MenuList = toolbarmain.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        txtNumber.Focus();
    }

    protected void MenuStockItemFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList();";
            Script += "</script>" + "\n";

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GO"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtNumber", txtNumber.Text);
                criteria.Add("txtName", txtName.Text);
                Filter.CurrentSpareItemFilterCriteria = criteria;
                
            }
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
}
