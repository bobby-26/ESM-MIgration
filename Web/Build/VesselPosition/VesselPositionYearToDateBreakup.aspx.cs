using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionYearToDateBreakup : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
           
        
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

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
    protected void MenuNRRangeConfig_TabStripCommand(object sender, EventArgs e)
    {
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

   

    protected void gvMenuYeartodatequaterreport_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionYearToDateQuaterReport.VoyageSummaryDelete(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblsummaryid")).Text));
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMenuYeartodatequaterreport_ItemDataBound(Object sender,GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

        }
    }
    

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvMenuYeartodatequaterreport_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        string vesselid;
        vesselid = Request.QueryString["Vesselid"].ToString();

        DataSet ds = PhoenixVesselPositionYearToDateQuaterReport.VoyageSummaryList(
           General.GetNullableInteger(vesselid), General.GetNullableInteger(Request.QueryString["Year"].ToString()), General.GetNullableInteger(Request.QueryString["Quarter"].ToString()));

        gvMenuYeartodatequaterreport.DataSource = ds;
    }
    protected void Rebind()
    {
        gvMenuYeartodatequaterreport.SelectedIndexes.Clear();
        gvMenuYeartodatequaterreport.EditIndexes.Clear();
        gvMenuYeartodatequaterreport.DataSource = null;
        gvMenuYeartodatequaterreport.Rebind();
    }

    protected void UcVessel_TextChangedEvent(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void ddlYear_TextChanged(object sender, EventArgs e)
    {
        Rebind();
    }
}
