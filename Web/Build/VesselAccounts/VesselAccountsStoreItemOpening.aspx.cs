using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class VesselAccountsStoreItemOpening : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Store Item Opening", "OPENING");
        toolbar.AddButton("Store Disposition", "DISPOSITION");
        toolbar.AddButton("Purchase Order UnConfirm", "UCPROVISIONANDBOND");
        toolbar.AddButton("Issue of Bonded Stores", "BONDEDSTOREISSUE");
        //toolbar.AddButton("Round Off", "ROUNDOFF");
      //  toolbar.AddButton("Rob Initialize", "ROB");
        MenuStoreAdmin.AccessRights = this.ViewState;
        MenuStoreAdmin.MenuList = toolbar.Show();
        MenuStoreAdmin.SelectedMenuIndex = 0;

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuStockItem.AccessRights = this.ViewState;
        MenuStockItem.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            if ((Request.QueryString["txtnumber"] != null) && (Request.QueryString["txtnumber"] != ""))
                txtNumberSearch.Text = Request.QueryString["txtnumber"].ToString().TrimEnd("00.00.00".ToCharArray()).TrimEnd("__.__.__".ToCharArray());
            if ((Request.QueryString["txtname"] != null) && (Request.QueryString["txtname"] != ""))
                txtStockItemNameSearch.Text = Request.QueryString["txtname"].ToString();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvStoreItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
    protected void Rebind()
    {
        gvStoreItem.SelectedIndexes.Clear();
        gvStoreItem.EditIndexes.Clear();
        gvStoreItem.DataSource = null;
        gvStoreItem.Rebind();
    }
    protected void MenuStockItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixVesselAccountsCorrections.SearchOpeningStoreItem(PhoenixSecurityContext.CurrentSecurityContext.VesselID, txtNumberSearch.Text, txtStockItemNameSearch.Text,
                General.GetNullableInteger(ddlStockClass.SelectedHard), sortexpression, sortdirection,
               Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), gvStoreItem.PageSize, ref iRowCount, ref iTotalPageCount);

            gvStoreItem.DataSource = dt;
            gvStoreItem.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStoreItem_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
        }
    }
    private bool IsValidQuantity(string id, string quantity, string price, string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(id))
            ucError.ErrorMessage = "Item is required.";

        if (!General.GetNullableDecimal(quantity).HasValue)
        {
            ucError.ErrorMessage = "Quantity is required.";
        }
        if (!General.GetNullableDecimal(price).HasValue)
        {
            ucError.ErrorMessage = "Total Price is required.";
        }
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Closing Date is required.";
        }

        return (!ucError.IsError);
    }
    protected void gvStoreItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                Guid id = new Guid(((RadLabel)e.Item.FindControl("lblStoreitemid")).Text);

                string quantity = ((UserControlMaskNumber)e.Item.FindControl("txtQuantityEdit")).Text;
                string price = ((UserControlMaskNumber)e.Item.FindControl("txtPriceEdit")).Text;
                string date = ((UserControlDate)e.Item.FindControl("txtClosingDate")).Text;
                string dispositionid = ((RadLabel)e.Item.FindControl("lblDispositionID")).Text;

                int flag;
                if (dispositionid != "") flag = 1;
                else flag = 0;
                if (!IsValidQuantity(id.ToString(), quantity, price, date))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsCorrections.UpdateOpeningStoreItem(PhoenixSecurityContext.CurrentSecurityContext.VesselID, id
                    , decimal.Parse(quantity), decimal.Parse(price), flag, General.GetNullableDateTime(date).Value);
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
    protected void gvStoreItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvStoreItem.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
