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

public partial class PurchaseQuotationReason : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    { if(Request.QueryString["launch"] == null)
        { 
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Confirm", "UPDATE", ToolBarDirection.Right);
        if (Request.QueryString["minvendor"].ToString() == "1")
            toolbarmain.AddButton("Save Reason", "SAVE", ToolBarDirection.Right);
            MenuFormGeneral.MenuList = toolbarmain.Show();
        }
        MenuFormGeneral.AccessRights = this.ViewState;
       
        if(!IsPostBack)
        {
            ViewState["orderid"] = Request.QueryString["orderid"].ToString();
            ViewState["Quotationid"] = Request.QueryString["quoationid"].ToString();
            ViewState["minvendor"] = Request.QueryString["minvendor"].ToString();
            ViewState["higquote"] = Request.QueryString["higquote"].ToString();
            ViewState["congifnumberofquotes"] = Request.QueryString["configquote"].ToString();

            ViewState["PAGENUMBER"] = "1";
            binddata();

            rgvLine.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        lblMSG.Text = "Reason for less than " + ViewState["congifnumberofquotes"] + " quotes";
        if (Request.QueryString["minvendor"].ToString() == "0")
            tbl.Visible = false;
        
        if (Request.QueryString["higquote"].ToString() == "0")
            grid.Visible = false;
        if (Request.QueryString["OEM"].ToString() == "0")
            divgvnongenuine.Visible = true;
        if (Request.QueryString["launch"] == null)
        {
            radlblnongenuine.Visible = true;
            RadLabel1.Visible = true;
        }
        else
        {
            RadLabel3.Visible = true;
            RadLabel2.Visible = true;
        }
    }

    private void binddata()
    {
        try
        {
            DataSet ds = PhoenixPurchaseQuotation.OrderfromStatusEdit(General.GetNullableGuid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);
            DataRow dr = ds.Tables[0].Rows[0];
            if (General.GetNullableInteger(dr["FLDMINUMUM3VENDORREASONID"].ToString()) != null)
                ucReason.SelectedValue = dr["FLDMINUMUM3VENDORREASONID"].ToString();
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
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
                if (General.GetNullableInteger(ucReason.SelectedValue) == null)
                {
                    ucError.ErrorMessage = "Please select the reason";
                    ucError.Visible = true;
                    return;
                }
                PhoenixPurchaseQuotation.OrderfromStatusUpdate(General.GetNullableGuid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection, General.GetNullableInteger(ucReason.SelectedValue), General.GetNullableString(ucReason.SelectedText));
            }
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                UpdateSelectVendorForPo(ViewState["Quotationid"].ToString());
                InsertOrderFormHistory();

                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " fnReloadList();";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UpdateSelectVendorForPo(string quotationid)
    {

            PhoenixPurchaseQuotation.UpdateQuotation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString()), new Guid(quotationid), Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMTYPE), "PO")));

            PhoenixPurchaseQuotation.SendApprovalInsert
                    (PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString()), "yes", 1);
        PhoenixPurchaseFalLevel.FalApprovalLevelsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(quotationid), Filter.CurrentPurchaseStockType ,"PCH"); // 865 is Quick code for purchaseform

    }
    private void InsertOrderFormHistory()
    {
        PhoenixPurchaseOrderForm.InsertOrderFormHistory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);
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

        DataSet ds = PhoenixPurchaseQuotation.QuotationLineHigherValueSearch(General.GetNullableGuid(ViewState["Quotationid"].ToString()), sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()),
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
            DataRowView drv = (DataRowView)e.Item.DataItem;
            UserControlQuick ucReasonEdit = (UserControlQuick)e.Item.FindControl("ucReasonEdit");
            RadComboBox ucReasonEdit1 = (RadComboBox)e.Item.FindControl("ucReasonEdit1");
            if (ucReasonEdit1 != null)
            {
                ucReasonEdit1.DataSource = PhoenixRegistersQuick.ListQuick(1, 180);

                ucReasonEdit1.DataBind();
            }

            if (ucReasonEdit1 != null && General.GetNullableInteger(drv["FLDHIGHERVALUEVENDORREASONID"].ToString()) != null)
            {
                ucReasonEdit1.SelectedValue = drv["FLDHIGHERVALUEVENDORREASONID"].ToString();
            }
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (Request.QueryString["launch"] != null)
            {
                edit.Visible = false;
            }
        }
    }
    protected void Rebind()
    {
        rgvLine.SelectedIndexes.Clear();
        rgvLine.EditIndexes.Clear();
        rgvLine.DataSource = null;
        rgvLine.Rebind();
    }
    protected void rgvLine_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        else if (e.CommandName == "Update")
        {
            RadComboBox ucReasonEdit1 = (RadComboBox)e.Item.FindControl("ucReasonEdit1");
            GridDataItem item = (GridDataItem)e.Item;
            PhoenixPurchaseQuotation.OrderLineResonUpdate(General.GetNullableGuid(item.GetDataKeyValue("FLDORDERLINEID").ToString()), Filter.CurrentPurchaseVesselSelection, General.GetNullableInteger(ucReasonEdit1.SelectedValue), General.GetNullableString(ucReasonEdit1.Text));
            Rebind();
        }
    }

    protected void gvnongenuine_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvnongenuine.CurrentPageIndex + 1;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseQuotation.QuotationLineOEMitemSearch(General.GetNullableGuid(ViewState["Quotationid"].ToString()), sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()),
           gvnongenuine.PageSize,
           ref iRowCount,
           ref iTotalPageCount);

        gvnongenuine.DataSource = ds;
        gvnongenuine.VirtualItemCount = iRowCount;
    }

    protected void gvnongenuine_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            
            RadComboBox ucReasonEdit1 = (RadComboBox)e.Item.FindControl("ucnonoemReasonEdit1");
            if (ucReasonEdit1 != null)
            {
                ucReasonEdit1.DataSource = PhoenixRegistersQuick.ListQuick(1, 180);

                ucReasonEdit1.DataBind();
            }

            if (ucReasonEdit1 != null && General.GetNullableInteger(drv["FLDNONOEMREASONID"].ToString()) != null)
            {
                ucReasonEdit1.SelectedValue = drv["FLDNONOEMREASONID"].ToString();
            }
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (Request.QueryString["launch"] != null)
            {
                edit.Visible = false;
            }
        }

    }

    protected void gvnongenuine_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        else if (e.CommandName == "Update")
        {
            RadComboBox ucReasonEdit1 = (RadComboBox)e.Item.FindControl("ucnonoemReasonEdit1");
            GridDataItem item = (GridDataItem)e.Item;
            PhoenixPurchaseQuotation.OrderLineOEMResonUpdate(General.GetNullableGuid(item.GetDataKeyValue("FLDORDERLINEID").ToString()), Filter.CurrentPurchaseVesselSelection, General.GetNullableInteger(ucReasonEdit1.SelectedValue), General.GetNullableString(ucReasonEdit1.Text));
            gvnongenuineRebind();
        }

    }

    protected void gvnongenuineRebind()
    {
        gvnongenuine.SelectedIndexes.Clear();
        gvnongenuine.EditIndexes.Clear();
        gvnongenuine.DataSource = null;
        gvnongenuine.Rebind();
    }
}
