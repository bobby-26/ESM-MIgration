using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;

public partial class InspectionVesselCheckitemsAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Migrate", "SAVE", ToolBarDirection.Right);
            Menuvessel.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {

                ucVessel.Enabled = true;
                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();
                }
               
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Menuvessel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
           
            int? Vessel = General.GetNullableInteger(ucVessel.SelectedVessel);
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidApprove(Vessel))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionVesselCheckItems.InspectionCheckItemMappingInsert(Vessel);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                             "BookMarkScript", "fnReloadList('Add', 'ifMoreInfo', null);", true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private bool IsValidApprove(int? Vessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";
 
        if (Vessel == null)
            ucError.ErrorMessage = "Vessel is required.";
        return (!ucError.IsError);
    }

}
