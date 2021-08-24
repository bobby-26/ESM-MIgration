using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class PurchaseComercialInvoiceAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuFormGeneral.AccessRights = this.ViewState;
        MenuFormGeneral.MenuList = toolbarmain.Show();
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        if (!IsPostBack)
        {
            if (Request.QueryString["INVOICEID"] != null)
                ViewState["INVOICEID"] = Request.QueryString["INVOICEID"].ToString();
            ViewState["PAGENUMBER"] = "1";
            ucAddrAgent.AddressType = "135";
            binddata();

            rgvLine.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        bindmenu();


    }
    private void bindmenu()
    {
        PhoenixToolbar toolbarMenu = new PhoenixToolbar();
        if (ViewState["INVOICEID"] != null)
            toolbarMenu.AddImageLink("javascript:openNewWindow('codehelp2', '', 'Purchase/PurchaseComercialPOAdd.aspx?INVOICEID=" + ViewState["INVOICEID"] + "',false,'900','500'); return true;", "Add", "Add.png", "ADD");
        MenuPoAdd.AccessRights = this.ViewState;
        MenuPoAdd.MenuList = toolbarMenu.Show();
    }
    private void binddata()
    {
        try
        {
            if (ViewState["INVOICEID"] != null)
            {
                DataSet ds = PhoenixPurchaseComercialInvoice.ComercialInvoiceEdit(General.GetNullableGuid(ViewState["INVOICEID"].ToString()));
                DataRow dr = ds.Tables[0].Rows[0];
                ucCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();
                ucAddrAgent.SelectedValue= dr["FLDCONSIGNYID"].ToString();
                ucAddrAgent.Text= dr["FLDCONSIGNY"].ToString();
                txtInvoicedate.Text= dr["FLDINVOICEDATE"].ToString();
                txtInvoiceNumber.Text = dr["FLDINVOICENUMBER"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            rgvLine.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuFormGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["INVOICEID"] == null)
                {
                    Guid? invoiceid = null;
                    PhoenixPurchaseComercialInvoice.InsertComercialInvoice(General.GetNullableInteger(ucCompany.SelectedCompany), General.GetNullableInteger(ucAddrAgent.SelectedValue), General.GetNullableDateTime(txtInvoicedate.Text),ref invoiceid);
                    ViewState["INVOICEID"] = invoiceid;
                    string Script = "";
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += " fnReloadList('codehelp1','','');";
                    Script += "</script>" + "\n";
                    RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
                    bindmenu();
                }
                else
                {
                    PhoenixPurchaseComercialInvoice.UpdateComercialInvoice(General.GetNullableInteger(ucCompany.SelectedCompany), General.GetNullableInteger(ucAddrAgent.SelectedValue), General.GetNullableDateTime(txtInvoicedate.Text),General.GetNullableGuid(ViewState["INVOICEID"].ToString()));

                    string Script = "";
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += " fnReloadList('codehelp1','','');";
                    Script += "</script>" + "\n";
                    RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
                }
                binddata();
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void rgvLine_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : rgvLine.CurrentPageIndex + 1;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        Guid? invoiceid = null;
        if (ViewState["INVOICEID"] != null)
            invoiceid = General.GetNullableGuid(ViewState["INVOICEID"].ToString());
    DataSet ds = PhoenixPurchaseComercialInvoice.ComercialInvoiceLineitemSearch(
                                                                   invoiceid,
                                                                   sortexpression, 
                                                                   sortdirection,
                                                                   int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                   rgvLine.PageSize,
                                                                   ref iRowCount,
                                                                   ref iTotalPageCount);

        rgvLine.DataSource = ds;
        rgvLine.VirtualItemCount = iRowCount;
    }

    protected void rgvLine_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
   

        }
    }
    //protected void Rebind()
    //{
    //    rgvLine.SelectedIndexes.Clear();
    //    rgvLine.EditIndexes.Clear();
    //    rgvLine.DataSource = null;
    //    rgvLine.Rebind();
    //}
    protected void rgvLine_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void MenuPoAdd_TabStripCommand(object sender, EventArgs e)
    {

    }
}
