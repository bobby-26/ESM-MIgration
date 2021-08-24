using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Framework;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class PurchaseSupplierPort : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (!IsPostBack)
            {
               
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvSupplierlist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvSupplierlist.SelectedIndexes.Clear();
        gvSupplierlist.EditIndexes.Clear();
        gvSupplierlist.DataSource = null;
        gvSupplierlist.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseSupplierPort.SupplierportSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvSupplierlist.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);


        gvSupplierlist.DataSource = ds;
        gvSupplierlist.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void gvSupplierlist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSupplierlist.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvSupplierlist.Rebind();
    }
    protected void gvSupplierlist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);


            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancle = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cancle != null) cancle.Visible = SessionUtil.CanAccess(this.ViewState, cancle.CommandName);

            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                RadDropDownList StockType = (RadDropDownList)e.Item.FindControl("ddlStockTypeedit");

                GridEditableItem i = (GridEditableItem)e.Item;
                DataRowView drv = (DataRowView)i.DataItem;
                StockType.SelectedValue = drv["FLDSTOCKTYPE"].ToString();
            }
        }
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            
        }
        if (e.Item is GridFooterItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    private void SupplierportInsert(int vendor, int port,int priority, string StockType)
    {
        PhoenixPurchaseSupplierPort.SupplierportInsert(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, vendor, port, priority, StockType);
        ucStatus.Text = "Information Added";
    }
    private void SupplierportUpdate(int id, int priority , string StockType)
    {
        PhoenixPurchaseSupplierPort.SupplierportUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                id, priority , StockType);
        ucStatus.Text = "Information updated";
    }

    private void SupplierportDelete(int id)
    {
        PhoenixPurchaseSupplierPort.SupplierportDelete(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, id);
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            SupplierportDelete(Int32.Parse(ViewState["ID"].ToString()));
            Rebind();
            ucStatus.Text = "Information deleted";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSupplierlist_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string ucpriority = ((UserControlNumber)e.Item.FindControl("ucpriorityadd")).Text;
                if (!IsValidAdd(((UserControlMultiColumnAddress)e.Item.FindControl("ddlmulticolumnaddress")).SelectedValue,
                  ((UserControlMultiColumnPort)e.Item.FindControl("ddlmulticolumnport")).SelectedValue, ucpriority))

                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                SupplierportInsert(
                    (int.Parse(((UserControlMultiColumnAddress)e.Item.FindControl("ddlmulticolumnaddress")).SelectedValue)),
                    (int.Parse(((UserControlMultiColumnPort)e.Item.FindControl("ddlmulticolumnport")).SelectedValue)), 
                    (int.Parse(((UserControlNumber)e.Item.FindControl("ucpriorityadd")).Text)),
                     ((RadDropDownList)e.Item.FindControl("ddlStockType")).SelectedValue);
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string ucpriorityupdate = ((UserControlNumber)e.Item.FindControl("ucpriorityedit")).Text;
                if (!IsValidUpdate(ucpriorityupdate))

                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                SupplierportUpdate(
                     (Int32.Parse(((RadLabel)e.Item.FindControl("lblidedit")).Text)),
                     (int.Parse(((UserControlNumber)e.Item.FindControl("ucpriorityedit")).Text)),
                            ((RadDropDownList)e.Item.FindControl("ddlStockTypeedit")).SelectedValue);
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["ID"] = ((RadLabel)e.Item.FindControl("lblid")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidAdd(string ddlmulticolumnaddress, string ddlmulticolumnport, string ucpriority)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvSupplierlist;

        if (ddlmulticolumnaddress.Equals("") || ddlmulticolumnaddress.Equals("Dummy"))
            ucError.ErrorMessage = "Vendor is required.";

        if (ddlmulticolumnport.Equals("") || ddlmulticolumnport.Equals("Dummy"))
            ucError.ErrorMessage = "Port is required.";

        if (string.IsNullOrEmpty(ucpriority))
           ucError.ErrorMessage = "Priority is required.";

        return (!ucError.IsError);
    }
    private bool IsValidUpdate(string ucpriorityupdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvSupplierlist;
        if (string.IsNullOrEmpty(ucpriorityupdate))
            ucError.ErrorMessage = "Priority is required.";
        return (!ucError.IsError);
    }


}