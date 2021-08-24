using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.Web.UI;

public partial class OwnersPurchaseFilter : PhoenixBasePage
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
                ucFormStatus.HardTypeCode = ((int)PhoenixHardTypeCode.FORMSTATUS).ToString();
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
                criteria.Add("createddate", txtCreatedDate.Text);
                criteria.Add("createdtodate", txtCreatedToDate.Text);
                criteria.Add("ucFormType", ucHard.SelectedHard);
                criteria.Add("txtFormType", txtFormNumber.Text);
                criteria.Add("ucStatus", ucFormStatus.SelectedHard);

                Filter.CurrentOwnerPurchaseFilter = criteria;

                //NameValueCollection makercriteria = new NameValueCollection();
                //makercriteria.Clear();
                //makercriteria.Add("txtMakerId", txtMakerId.Text);
                //Filter.CurrentMakerReference = makercriteria;

                String scriptpopupclose = String.Format("javascript:fnReloadList('Filter', null,null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
            }
            
            //Response.Redirect("../Owners/OwnersPurchaseOrderForm.aspx", false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
