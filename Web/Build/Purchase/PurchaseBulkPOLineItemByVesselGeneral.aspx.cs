using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PurchaseBulkPOLineItemByVesselGeneral : PhoenixBasePage
{  
    StringBuilder strvessellist = new StringBuilder();
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);        
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuBulkPO.AccessRights = this.ViewState;
        MenuBulkPO.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() != "" && Request.QueryString["callfrom"].ToString() == "schedulepo")
            {
                ViewState["callfrom"] = Request.QueryString["callfrom"].ToString();
                if (Filter.CurrentSelectedSchedulePO != null && Filter.CurrentSelectedSchedulePO.ToString() != "")
                    ViewState["ORDERID"] = Filter.CurrentSelectedSchedulePO.ToString();
                else
                    ViewState["ORDERID"] = "";

                if (Filter.CurrentSelectedSchedulePOLineItemId != null && Filter.CurrentSelectedSchedulePOLineItemId.ToString() != "")
                    ViewState["LINEITEMID"] = Filter.CurrentSelectedSchedulePOLineItemId.ToString();
                else
                    ViewState["LINEITEMID"] = "";
            }
            else
            {
                ViewState["callfrom"] = null;
                if (Filter.CurrentSelectedBulkOrderId != null && Filter.CurrentSelectedBulkOrderId.ToString() != "")
                    ViewState["ORDERID"] = Filter.CurrentSelectedBulkOrderId.ToString();
                else
                    ViewState["ORDERID"] = "";

                if (Filter.CurrentSelectedBulkOrderLineItemId != null && Filter.CurrentSelectedBulkOrderLineItemId.ToString() != "")
                    ViewState["LINEITEMID"] = Filter.CurrentSelectedBulkOrderLineItemId.ToString();
                else
                    ViewState["LINEITEMID"] = "";
            }
            BindVesselList();
            BulkPOEdit();
        }
    }

    private void BindVesselList()
    {
        DataSet ds = new DataSet();
        if (ViewState["callfrom"] == null)
            ds = PhoenixPurchaseBulkPurchase.BulkPOLineItemRestVesselList(null, "", null, string.Empty, null, General.GetNullableGuid(ViewState["LINEITEMID"].ToString()));
        else
            ds = PhoenixPurchaseSchedulePO.SchedulePOLineItemRestVesselList(null, "", null, string.Empty, null, General.GetNullableGuid(ViewState["LINEITEMID"].ToString()), General.GetNullableGuid(ViewState["ORDERID"].ToString()));
        chkVesselList.Items.Add("select");
        chkVesselList.DataSource = ds;
        chkVesselList.DataTextField = "FLDVESSELNAME";
        chkVesselList.DataValueField = "FLDVESSELID";
        chkVesselList.DataBind();        
    }

    protected void BulkPOEdit()
    {
        DataSet ds = PhoenixPurchaseBulkPurchase.BulkPOEdit(new Guid(ViewState["ORDERID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];          
            if (dr["FLDSTOCKTYPE"] != null && dr["FLDSTOCKTYPE"].ToString() != "" && dr["FLDSTOCKTYPE"].ToString() == "STORE")
            {
                string sealstoretype = PhoenixCommonRegisters.GetHardCode(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            ,97
                                            ,"SEA");
                if (dr["FLDSTOCKCLASSID"] != null && dr["FLDSTOCKCLASSID"].ToString() == sealstoretype)
                {
                    DataSet dsVesselList = new DataSet();

                    dsVesselList = PhoenixPurchaseBulkPurchase.BulkPOLineItemRestVesselList(null, "", null, string.Empty, null, General.GetNullableGuid(ViewState["LINEITEMID"].ToString()));
                    chkVesselList.Items.Add("select");
                    chkVesselList.DataSource = dsVesselList;
                    chkVesselList.DataTextField = "FLDVESSELNAME";
                    chkVesselList.DataValueField = "FLDVESSELID";
                    chkVesselList.DataBind();

                }
            }           
        }
    }

    private void GetSelectedVessel()
    {        
        foreach (ListItem item in chkVesselList.Items)
        {
            if (item.Selected == true)
            {
                strvessellist.Append(item.Value.ToString());
                strvessellist.Append(",");
            }
        }
        if (strvessellist.Length > 1)
        {
            strvessellist.Remove(strvessellist.Length - 1, 1);
        }
        if (strvessellist.ToString().Contains("Dummy"))
        {
            strvessellist = new StringBuilder();
            strvessellist.Append("Dummy");
        }
    }

    protected void BulkPO_TabStripCommand(object sender, EventArgs e)
    {
        String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
        String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                GetSelectedVessel();
                string strVessel = strvessellist.ToString();

                if (!IsValidInsert())
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["callfrom"] == null)
                {
                    PhoenixPurchaseBulkPurchase.BulkPOLineItemByVesselInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(ViewState["ORDERID"].ToString())
                                                            , new Guid(ViewState["LINEITEMID"].ToString())
                                                            , General.GetNullableString(strVessel)
                                                            , null
                                                            , null
                                                            , null);

                }
                else
                {
                    PhoenixPurchaseSchedulePO.SchedulePOLineItemByVesselInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(ViewState["ORDERID"].ToString())
                                                            , new Guid(ViewState["LINEITEMID"].ToString())
                                                            , General.GetNullableString(strVessel)
                                                            , null
                                                            , null);
                }

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
            }
            else
            {
                ucError.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidInsert()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["ORDERID"].ToString()) == null)
            ucError.Text = "First create a PO and add the Line Items before the vessel selection.";

        if (General.GetNullableGuid(ViewState["LINEITEMID"].ToString()) == null)
            ucError.Text = "First add the Line Items for the selected PO.";

        return (!ucError.IsError);

    }
}
