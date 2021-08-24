using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using System.Web;
using Telerik.Web.UI;

public partial class PurchaseDelivery : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();


        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                toolbar.AddButton("Vessel Email", "VESSELEMAIL",ToolBarDirection.Right);
                toolbar.AddButton("Create PO", "CREATEPO", ToolBarDirection.Right);
                toolbar.AddButton("Fwdr Email", "FORWARDEREMAIL", ToolBarDirection.Right);
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
                MenuDelivery.AccessRights = this.ViewState; 
                MenuDelivery.MenuList = toolbar.Show();
            }
            
            txtForwarderId.Attributes.Add("style", "visibility:hidden");
            txtAgentId.Attributes.Add("style", "visibility:hidden");

            txtFormNumber.Text = PhoenixPurchaseOrderForm.FormNumber;

            if (!IsPostBack)
            {
                txtAgentId.Attributes.Add("style", "visibility:hidden");

                ucStatus.HardTypeCode = ((int)PhoenixHardTypeCode.FORMSTATUS).ToString();
                ViewState["SaveStatus"] = "New";
                if (Request.QueryString["deliveryid"] != null)
                 {
                    ViewState["deliveryid"] = Request.QueryString["deliveryid"].ToString();
                    BindFields(ViewState["deliveryid"].ToString());
                    ViewState["SaveStatus"] = "Edit";
                 }              
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindFields(string deliveryid)
    {
        DataSet ds = PhoenixPurchaseDelivery.Editdelivery(new Guid(deliveryid));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            lblDeliveryId.Text = dr["FLDDELIVERYID"].ToString();
            lblOrderId.Text = dr["FLDORDERID"].ToString();
            txtDeliveryNumber.Text = dr["FLDDELIVERYNUMBER"].ToString();
            txtTitle.Text = dr["FLDTITLE"].ToString();
            txtHawb.Text = dr["FLDHAWB"].ToString();
            txtNoOfPackages.Text = dr["FLDNUMBEROFPACKAGES"].ToString();
            ucTotalWeight.Text = String.Format("{0:#,###,##0.00}", dr["FLDTOTALWEIGHT"]);
            txtAmount.Text = String.Format("{0:#,###,##0.00}", dr["FLDVALUE"]);            
            txtForwarderCode.Text = dr["FLDFORWARDERCODE"].ToString();
            txtForwarderName.Text = dr["FLDFORWARDERNAME"].ToString();
            txtForwarderId.Text = dr["FLDFORWARDER"].ToString();
            txtLocation.Text = dr["FLDLOCATION"].ToString();
            txtOrigin.Text = dr["FLDORIGIN"].ToString();
            txtShipmentMode.Text = dr["FLDSHIPMENTMODE"].ToString();
            txtShortNote.Text = dr["FLDSHORTNOTE"].ToString();
            txtReceivedForwarder.Text = General.GetDateTimeToString(dr["FLDFORWARDERRECEIVEDDATE"].ToString());
            ucStatus.SelectedHard = dr["FLDSTATUS"].ToString();
            txtAgentName.Text = dr["FLDAGENTNAME"].ToString();
            txtAgentId.Text = dr["FLDAGENTID"].ToString();
            txtAgentCode.Text = dr["FLDAGENTCODE"].ToString();
            txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
            ucCurrency.SelectedCurrency = dr["FLDCURRENCY"].ToString();
            ddlDGR.SelectedValue = dr["FLDISDGR"].ToString();
            ucPortMulti.SelectedValue = dr["FLDPORT"].ToString();
            ucPortMulti.Text = dr["FLDPORTNAME"].ToString();
            ucETA.Text = dr["FLDETA"].ToString();
            ucETB.Text = dr["FLDETB"].ToString();
            ucDeliveyBy.Text = dr["FLDDELIVERYBY"].ToString();
            txtFormNo.Text = dr["FLDFORMNO"].ToString();
            //ucCommonToolTip.Screen="Registers/RegistersToolTipAddress?addresscode="+dr["FLDAGENTID"].ToString();
        }
    }

    protected void MenuDelivery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDelivery())
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["SaveStatus"].ToString().Equals("Edit"))
                {
                    UpdateDelivery();
                }
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                Response.Redirect("../Purchase/PurchaseDeliveryNew.aspx");              
            }
            if (CommandName.ToUpper().Equals("FORWARDEREMAIL"))
            {
                String script = String.Format("javascript:openNewWindow('codehelp1','','"+Session["sitepath"]+"/Purchase/PurchaseDeliveryForwarderEmail.aspx?deliveryid=" + ViewState["deliveryid"].ToString() + "');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            }
            if (CommandName.ToUpper().Equals("CREATEPO"))
            {
                PhoenixPurchaseDelivery.CreatePO(new Guid(ViewState["deliveryid"].ToString()));

                ucStatusMessage.Text = "PO created Successfully.";
                ucStatusMessage.Visible = true;

                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            if (CommandName.ToUpper().Equals("VESSELEMAIL"))
            {
                String script = String.Format("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Purchase/PurchaseDeliveryVesselEmail.aspx?deliveryid=" + ViewState["deliveryid"].ToString() + "');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void UpdateDelivery()
    {
        PhoenixPurchaseDelivery.Updatedelivery(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(ViewState["deliveryid"].ToString()), 
            txtShipmentMode.Text,
            General.GetNullableInteger(txtNoOfPackages.Text),
            General.GetNullableDecimal(ucTotalWeight.Text),
            General.GetNullableDecimal(txtAmount.Text),
            General.GetNullableInteger(txtForwarderId.Text),
            null,
            General.GetNullableDateTime(txtReceivedForwarder.Text),
            General.GetNullableInteger(ucCurrency.SelectedCurrency),
            General.GetNullableInteger(ddlDGR.SelectedValue),
            txtShortNote.Text, null, txtHawb.Text, txtOrigin.Text, txtLocation.Text
            ,General.GetNullableString(txtTitle.Text)
            ,General.GetNullableInteger(txtAgentId.Text)
            ,General.GetNullableDateTime(ucETA.Text)
            ,General.GetNullableDateTime(ucETB.Text)
            ,General.GetNullableInteger(ucPortMulti.SelectedValue)
            ,General.GetNullableDateTime(ucDeliveyBy.Text)
            ,General.GetNullableInteger(ucStatus.SelectedHard));
    }

    private bool IsValidDelivery()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtTitle.Text) == null)
            ucError.ErrorMessage = "Title is required.";

        //if (General.GetNullableInteger(txtForwarderId.Text) == null)
        //    ucError.ErrorMessage = "Forwarder is required.";

        //if (General.GetNullableInteger(txtAgentId.Text) == null)
        //    ucError.ErrorMessage = "Agent is required.";

        if (General.GetNullableDecimal(txtAmount.Text) != null)
        {
            if (General.GetNullableInteger(ucCurrency.SelectedCurrency) == null)
                ucError.ErrorMessage = "Currency is required.";
        }
        

        return (!ucError.IsError);
    }
}
