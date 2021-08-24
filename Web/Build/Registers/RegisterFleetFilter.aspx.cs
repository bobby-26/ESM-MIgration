using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Registers_RegisterFleetFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
            fleetfilter.MenuList = toolbar.Show();        
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {

        NameValueCollection nvc = Filter.CurrentFleetFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }
        if (nvc != null && !IsPostBack)
        {
            txtFleetCode.Text = (nvc.Get("txtFleetCode") == null) ? "" : nvc.Get("txtFleetCode").ToString();
            txtSearch.Text = (nvc.Get("txtSearch") == null) ? "" : nvc.Get("txtSearch").ToString();
            ddlFleetType.SelectedValue = (General.GetNullableInteger(nvc.Get("ddlFleetType")) == null) ? "" : nvc.Get("ddlFleetType").ToString();
            ddlVessel.SelectedVessel = (General.GetNullableInteger(nvc.Get("ddlVessel")) == null) ? "" : nvc.Get("ddlVessel").ToString();
            ucVesselType.SelectedVesseltype = (General.GetNullableInteger(nvc.Get("ucVesselType")) == null) ? "" : nvc.Get("ucVesselType").ToString();
            ucPrincipal.SelectedAddress = (General.GetNullableInteger(nvc.Get("ucPrincipal")) == null) ? "" : nvc.Get("ucPrincipal").ToString();
            ucFlag.SelectedFlag = (General.GetNullableInteger(nvc.Get("ucFlag")) == null) ? "" : nvc.Get("ucFlag").ToString();
        }
    }

    protected void fleetfilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";
        if (CommandName.ToUpper().Equals("GO"))
        {
            if (ddlFleetType.SelectedValue == "")
            {
                ucError.ErrorMessage = "Fleet Type is Required";
                ucError.Visible = true;
                return;
            }

            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("txtFleetCode", txtFleetCode.Text.Trim());
            criteria.Add("txtSearch", txtSearch.Text.Trim());
            criteria.Add("ddlFleetType", ddlFleetType.SelectedValue);
            criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
            criteria.Add("ucVesselType", ucVesselType.SelectedVesseltype);
            criteria.Add("ucPrincipal", ucPrincipal.SelectedAddress);
            criteria.Add("ucFlag", ucFlag.SelectedFlag);
            
            Filter.CurrentFleetFilter = criteria;
        }
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}