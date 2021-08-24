using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Purchase_PurchaseFormConverter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuStockItemGeneral.Title= "Convert     (  " + PhoenixPurchaseOrderForm.FormNumber + "     )";
        MenuStockItemGeneral.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            rdoListFormType.AppendDataBoundItems = true;
            rdoListFormType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMTYPE));
            rdoListFormType.DataBindings.DataTextField = "FLDHARDNAME";
            rdoListFormType.DataBindings.DataValueField = "FLDHARDCODE";
            rdoListFormType.DataBind();
            rdoListFormStatus.AppendDataBoundItems = true;
            if (Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null)
            {
                rdoListFormStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMSTATUS));
            }
            else
            {
                rdoListFormType.Enabled = false;
                rdoListFormStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMSTATUS), 0, "ACT,PKD");
            }
            rdoListFormStatus.DataBindings.DataTextField = "FLDHARDNAME";
            rdoListFormStatus.DataBindings.DataValueField = "FLDHARDCODE";
            rdoListFormStatus.DataBind();
        }
        if (!IsPostBack)
        {
            if (Request.QueryString["orderid"] != null)
            {
                ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                BindData();
            }

        }
    }

    private void BindData()
    {

        DataSet ds = new DataSet();
        ds = PhoenixPurchaseOrderForm.EditOrderForm(new Guid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            rdoListFormType.SelectedValue = dr["FLDFORMTYPE"].ToString();
            rdoListFormStatus.SelectedValue = dr["FLDFORMSTATUS"].ToString();
        }
    }
    protected void InventoryStockItemGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            UpdateFormStatus();
        }

        if (CommandName.ToUpper().Equals("BACK"))
        {
            if (ViewState["orderid"] != null)
            {
                Response.Redirect("../Purchase/PurchaseFormDetails.aspx?orderid=" + ViewState["orderid"].ToString());
            }
        }

    }
    private void UpdateFormStatus()
    {
        try
        {
            if (!IsValidForm())
            {
                ucError.Visible = true;
                return;
            }

            PhoenixPurchaseOrderForm.ConvertOrderForm(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString()), Int32.Parse(rdoListFormStatus.SelectedValue.ToString()), Int32.Parse(rdoListFormType.SelectedValue.ToString()));
            InsertOrderFormHistory();
            String script = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void InsertOrderFormHistory()
    {
        PhoenixPurchaseOrderForm.InsertOrderFormHistory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);
    }
    private bool IsValidForm()
    {

        return (!ucError.IsError);
    }
}
