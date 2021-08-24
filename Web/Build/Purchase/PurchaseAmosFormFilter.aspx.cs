using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;


public partial class PurchaseAmosFormFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("GO", "GO");
            MenuFormFilter.MenuList = toolbarmain.Show();
            MenuFormFilter.SetTrigger(pnlDiscussion);  

               

            if (!IsPostBack)
            {
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuFormFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("GO"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ucVessel", ucVessel.SelectedVessel);
                criteria.Add("txtNumber", txtFormNumber.Text);
                criteria.Add("txtTitle", txtFromTitle.Text);             
                Filter.CurrentAmosFormFilterCriteria = criteria;
            }
            
            Response.Redirect("../Purchase/PurchaseAmosForm.aspx",false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
