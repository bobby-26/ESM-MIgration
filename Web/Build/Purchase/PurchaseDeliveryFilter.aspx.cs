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
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseDeliveryFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("GO", "GO",ToolBarDirection.Right);
            MenuFormFilter.MenuList = toolbarmain.Show();
            //MenuFormFilter.SetTrigger(pnlDiscussion);  

            if (!IsPostBack)
            {
              
                ucFormType.HardTypeCode = ((int)PhoenixHardTypeCode.FORMSTATUS).ToString();
                ucFormType.ShortNameFilter = "ACT,PLD,CNC";

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GO"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtDeliveryNumber", txtDeliveryNumber.Text);
                criteria.Add("txtDocumentNumber", txtDocumentNumber.Text);
                criteria.Add("ucVessel", ucVessel.SelectedVessel);
                criteria.Add("ucFormType", ucFormType.SelectedHard.ToString());
                criteria.Add("txtFromDate",txtFromDate.Text);
                criteria.Add("txtToDate", txtTodate.Text);
                Filter.CurrentDeliveryFormFilterCriteria = criteria;                
            }

            Response.Redirect("../Purchase/PurchaseDeliveryDetail.aspx", false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
