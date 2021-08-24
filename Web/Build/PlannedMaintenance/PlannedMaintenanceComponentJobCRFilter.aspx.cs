using System;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class PlannedMaintenanceComponentJobCRFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Go", "GO",ToolBarDirection.Right);
            MenuComponentFilter.MenuList = toolbarmain.Show();
            txtCompNumber.Focus();
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

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GO"))
            {
                NameValueCollection componentjob = new NameValueCollection();
                componentjob.Clear();
                componentjob.Add("txtCompNumber", txtCompNumber.Text);
                componentjob.Add("txtCompName", txtCompName.Text);
                componentjob.Add("txtJobcode", txtJobcode.Text);
                componentjob.Add("txtJobTitle", txtJobTitle.Text);
                componentjob.Add("ucDiscipline", ucDiscipline.SelectedDiscipline);

                Filter.CurrentComponentJobFilter = componentjob;
                Response.Redirect("PlannedMaintenanceComponentJobCR.aspx", false);     
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
}
