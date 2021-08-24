using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class Registers_RegistersSupplierInvoiceApprovalUserMap : PhoenixBasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Supplier Configuration", "SUPPLIERCONFIG");
            toolbar.AddButton("Banking Details", "BANK");
            toolbar.AddButton("TDS Supplier Register", "TDS");
            toolbar.AddButton("GST Configuration", "GST");
            MenuDPO.AccessRights = this.ViewState;
            MenuDPO.MenuList = toolbar.Show();
            MenuDPO.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
                txtaddresscode.Text = ViewState["ADDRESSCODE"].ToString();
                ifMoreInfo.Attributes["src"] = "../Registers/RegistersSupplierConfiguration.aspx?addresscode=" + ViewState["ADDRESSCODE"];
                EditTab();
                ViewState["gst"] = txtgst.Text;
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuDPO_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("BANK"))
            {
                MenuDPO.SelectedMenuIndex = 1;
                ifMoreInfo.Attributes["src"] = "../Registers/RegistersBankInformationList.aspx?addresscode=" + ViewState["ADDRESSCODE"] + "&toolbar=hide";
            }
            else if (CommandName.ToUpper().Equals("TDS") && ViewState["ADDRESSCODE"] != null)
            {
                MenuDPO.SelectedMenuIndex = 2;
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsSupplierTDSMapping.aspx?addresscode=" + ViewState["ADDRESSCODE"];
            }
            else if (CommandName.ToUpper().Equals("SUPPLIERCONFIG") && ViewState["ADDRESSCODE"] != null)
            {
                ifMoreInfo.Attributes["src"] = "../Registers/RegistersSupplierConfiguration.aspx?addresscode=" + ViewState["ADDRESSCODE"];
            }
            else if (CommandName.ToUpper().Equals("GST") && ViewState["gst"] != null && ViewState["gst"].ToString() != "0")
            {
                MenuDPO.SelectedMenuIndex = 3;
                ifMoreInfo.Attributes["src"] = "../Registers/RegisterGST.aspx?addresscode=" + ViewState["ADDRESSCODE"];
            }
            else
            {
                if (!IsValidLineItem())
                {
                    string errormessage = "";
                    errormessage = ucError.ErrorMessage;
                    errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                    RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);

                    return;
                }
            }
           // MenuDPO.SelectedMenuIndex = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void EditTab()
    {

        try
        {
            if (ViewState["ADDRESSCODE"] != null && ViewState["ADDRESSCODE"].ToString() != string.Empty)
            {
                DataSet ds = PhoenixAccountsGST.GSTDetailsedit(int.Parse(txtaddresscode.Text));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtgst.Text = ds.Tables[0].Rows[0]["FLDISGSTAPPLICABLE"].ToString();
                    //ViewState["gst"] = txtgst.Text;

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["ORDERID"] = null;
            // BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidLineItem()
    {
        ucError.HeaderMessage = "Please make the following changes";
        ucError.clear = "";

        if (ViewState["gst"] != null || ViewState["gst"].ToString() != string.Empty)
            ucError.ErrorMessage = "GST not configured for the supplier. Please configure GST for the supplier.";
        return (!ucError.IsError);
    }

}

