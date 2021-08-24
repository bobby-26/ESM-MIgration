using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CommonPickListDeliveryAddress : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        if (!IsPostBack)
        {
            rgvDeliveryAddress.PageSize = General.ShowRecords(null);
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ddlPurpose.DataSource = PhoenixRegistersContactPurpose.ListContactPurpose();
            ddlPurpose.DataTextField = "FLDPURPOSENAME";
            ddlPurpose.DataValueField = "FLDPURPOSEID";
            ddlPurpose.DataBind();

            if (Request.QueryString["addresscode"] != null)
                ViewState["addresscode"] = Request.QueryString["addresscode"].ToString();
        }

    }
    protected void ddlPurpose_SelectedIndexChanged(object sender, EventArgs e)
    {
        rgvDeliveryAddress.Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        rgvDeliveryAddress.Rebind();
    }

    protected void rgvDeliveryAddress_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        

        DataSet ds = PhoenixPurchaseOrderForm.ListAddressPurpose(General.GetNullableInteger(ViewState["addresscode"].ToString()), General.GetNullableInteger(ddlPurpose.SelectedValue));
        rgvDeliveryAddress.DataSource = ds;
    }
    protected void rgvDeliveryAddress_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            
            string Script = "";
            NameValueCollection nvc;

            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                RadLabel lblAddressContactid = (RadLabel)item["NAME"].FindControl("lblAddressContactID");
                nvc.Set(nvc.GetKey(1), lblAddressContactid.Text);
                LinkButton lblAddressName = (LinkButton)item["NAME"].FindControl("lnkUserName");
                nvc.Set(nvc.GetKey(2), lblAddressName.Text);
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                //Label lblAddressName = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAddressName");
                nvc.Set(nvc.GetKey(1), ((LinkButton)item["NAME"].FindControl("lnkUserName")).Text);
                //Label lblAddressContactid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblAddressContactId");
                nvc.Set(nvc.GetKey(2), item.GetDataKeyValue("FLDADDRESSCONTACTID").ToString());
            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page,typeof(Page), "BookMarkScript", Script,false);
        }
        
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
