using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class Registers_RegistersProjectLineItemPurchaseOrderFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);

        ProjectCodeList.AccessRights = this.ViewState;
        ProjectCodeList.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            BindPO();

            ViewState["PROJECTID"] = null;
            ViewState["ACCOUNTID"] = "";

            if (Request.QueryString["id"] != null)
            {
                ViewState["PROJECTID"] = Request.QueryString["id"].ToString();
            }
            if (Request.QueryString["accountid"] != null)
            {
                ViewState["ACCOUNTID"] = Request.QueryString["accountid"].ToString();
            }

            txtVendorId.Attributes.Add("style", "display:none");
            ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAccountsSupplier.aspx', true); ");

        }
    }

    public void BindPO()
    {
        ddlPOType.DataSource = PhoenixAccountsPO.ListAccountsPOType();
        ddlPOType.DataBind();
    }

    protected void ddlPOType_DataBound(object sender, EventArgs e)
    {
        ddlPOType.Items.Insert(0, new DropDownListItem("--Select--", ""));
    }

    protected void ProjectCodeList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();
            criteria.Add("txtPONumber", txtPONumber.Text.Trim());
            criteria.Add("txtVendorId", txtVendorId.Text.Trim());
            //criteria.Add("txtInvoiceNo", txtInvoiceNo.Text.Trim());
            criteria.Add("ddlPOType", ddlPOType.SelectedValue);

            Filter.ProjectCodePurchaseOrderListFilter = criteria;
        }

        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.ProjectCodePurchaseOrderListFilter = criteria;
        }

        Response.Redirect("../Registers/RegisterPOSHProjectLineItemPurchaseOrder.aspx?id=" + ViewState["PROJECTID"].ToString() + "&accountid=" + ViewState["ACCOUNTID"].ToString(), false);
    }
}