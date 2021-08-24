using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class VesselAccountsOrderFormUnConfirm : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Store Item Opening", "OPENING");
            toolbar.AddButton("Store Disposition", "DISPOSITION");
            toolbar.AddButton("Purchase Order UnConfirm", "UCPROVISIONANDBOND");
            toolbar.AddButton("Issue of Bonded Stores", "BONDEDSTOREISSUE");
           // toolbar.AddButton("Round Off", "ROUNDOFF");
            //  toolbar.AddButton("Rob Initialize", "ROB");
            MenuStoreAdmin.AccessRights = this.ViewState;
            MenuStoreAdmin.MenuList = toolbar.Show();
            MenuStoreAdmin.SelectedMenuIndex = 2;
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["ORDERID"] = null;
                ViewState["CURRENTTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvBondReq.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvBondReq.SelectedIndexes.Clear();
        gvBondReq.EditIndexes.Clear();
        gvBondReq.DataSource = null;
        gvBondReq.Rebind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDREFERENCENO", "FLDORDERDATE", "FLDSTOCKTYPE", "FLDORDERSTATUS" };
            string[] alCaptions = { "Order No", "Order Date", "Stock Type", "Order Status" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = Filter.CurrentVesselOrderFormFilter;
            DataSet ds = PhoenixVesselAccountsOrderForm.SearchOrderForm(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , nvc != null ? nvc.Get("txtRefNo") : null
                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStockType"] : string.Empty)
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvBondReq", "Requisition Bond and Provisions", alCaptions, alColumns, ds);

            gvBondReq.DataSource = ds;
            gvBondReq.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStoreAdmin_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("OPENING"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsStoreItemOpening.aspx";
            }
            else if (CommandName.ToUpper().Equals("DISPOSITION"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsStoreDisposition.aspx";
            }
            else if (CommandName.ToUpper().Equals("UCPROVISIONANDBOND"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsOrderFormUnConfirm.aspx";
            }
            else if (CommandName.ToUpper().Equals("BONDEDSTOREISSUE"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsAdminBondedStoreIssue.aspx";
            }
            else if (CommandName.ToUpper().Equals("ROUNDOFF"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsRoundOffUpdate.aspx";
            }
            else if (CommandName.ToUpper().Equals("ROB"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsOpeningRobExcelUpload.aspx";
            }
            Response.Redirect(ViewState["CURRENTTAB"].ToString(), false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
 
    protected void gvBondReq_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdunApprove");
            if (db != null)
            {
                if (drv["FLDORDERSTATUS"].ToString() != "Received") db.Visible = false;
                else db.Visible = true;
            }
        }
    }
    protected void gvBondReq_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UNCONFIRM"))
            {
                string id = ((RadLabel)e.Item.FindControl("lblOrderId")).Text;
                PhoenixVesselAccountsCorrections.VesselAccountOrderFormUnConfirm(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(id));
                Rebind();
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
    protected void gvBondReq_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBondReq.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
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
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
