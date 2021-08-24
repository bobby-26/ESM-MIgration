using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;


public partial class PlannedMaintenanceComponentJobFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Go", "GO");
            MenuComponentFilter.MenuList = toolbarmain.Show();
          
 
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuComponentFilter_TabStripCommand(object sender, EventArgs e)
    {

        try
        {

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("GO"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtFunctionNumber", txtFunctionNumber.Text);
                criteria.Add("txtFunctionDescription", txtFunctionDescription.Text);
                criteria.Add("txtComponentNumber", txtComponentNumber.Text);
                criteria.Add("txtComponentName", txtComponentName.Text);               
                Filter.CurrentComponentFilterCriteria = criteria;
            }

            Response.Redirect("../PlannedMaintenance/PlannedMaintenanceFunction.aspx");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
}
