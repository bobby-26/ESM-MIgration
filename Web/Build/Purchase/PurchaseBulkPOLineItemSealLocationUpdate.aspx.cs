using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class PurchaseBulkPOLineItemSealLocationUpdate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Update", "UPDATE",ToolBarDirection.Right);
        MenuLocation.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            if (Request.QueryString["LINEITEMID"] != null && Request.QueryString["LINEITEMID"].ToString() != string.Empty)
            {
                ViewState["LINEITEMID"] = Request.QueryString["LINEITEMID"].ToString();                
            }
            if (Request.QueryString["STOREITEMID"] != null && Request.QueryString["STOREITEMID"].ToString() != string.Empty)
            {
                ViewState["STOREITEMID"] = Request.QueryString["STOREITEMID"].ToString();
            }
            BindLocationList();
            BulkPOLineItemEdit();
        }
    }

    private void BindLocationList()
    {
        if (ViewState["STOREITEMID"] != null)
        {
            string storeitemlocationid = null;
            DataSet ds = PhoenixPurchaseBulkPurchase.LocationList(new Guid(ViewState["STOREITEMID"].ToString()));
            
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["FLDISDEFAULT"] != null && dr["FLDISDEFAULT"].ToString() == "1")
                    storeitemlocationid = dr["FLDSTOREITEMLOCATIONID"].ToString();
            }
            ddlLocation.DataSource = ds.Tables[0];
            ddlLocation.DataTextField = "FLDLOCATIONNAME";
            ddlLocation.DataValueField = "FLDSTOREITEMLOCATIONID";            
            ddlLocation.DataBind();

            if (storeitemlocationid != null)
                ddlLocation.SelectedValue = storeitemlocationid;
        }
        ddlLocation.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void BulkPOLineItemEdit()
    {
        if (ViewState["LINEITEMID"] != null)
        {
            DataSet ds = PhoenixPurchaseBulkPurchase.BulkPOLineItemEdit(new Guid(ViewState["LINEITEMID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtReceivedQty.Text = dr["FLDRECEIVEDQUANTITY"].ToString();
            }
        }
    }
    protected void MenuLocation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (General.GetNullableGuid(ddlLocation.SelectedValue) == null)
            {
                lblMessage.Text = "Location is required.";
                return;
            }

            if (ViewState["LINEITEMID"] != null && ViewState["STOREITEMID"] != null)
            {
                if (CommandName.ToUpper().Equals("UPDATE"))
                {
                    PhoenixPurchaseBulkPurchase.SealLocationUpdate(
                                                                   PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                   , new Guid(ViewState["LINEITEMID"].ToString())
                                                                   , new Guid(ViewState["STOREITEMID"].ToString())
                                                                   , new Guid(ddlLocation.SelectedValue)
                                                                   , decimal.Parse(txtReceivedQty.Text));

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null, null);", true);
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }  
}
