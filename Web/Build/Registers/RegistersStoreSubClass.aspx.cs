using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class RegistersStoreSubClass : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddImageButton("../Registers/RegistersStoreSubClass.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Registers/RegistersStoreSubClass.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuPurchaseconfiguration.AccessRights = this.ViewState;
            MenuPurchaseconfiguration.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

              if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvSubclass.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ddlStockClass.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();
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
        gvSubclass.SelectedIndexes.Clear();
        gvSubclass.EditIndexes.Clear();
        gvSubclass.DataSource = null;
        gvSubclass.Rebind();
    }
    protected void ddlstock_TextChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvSubclass.Rebind();

    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersStoreSubClass.StoresubclassSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableInteger(ddlStockClass.SelectedHard),
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvSubclass.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);


        gvSubclass.DataSource = ds;
        gvSubclass.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    protected void gvSubclass_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSubclass.CurrentPageIndex + 1;
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
        Rebind();
    }

    protected void Purchaseconfiguration_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {

                ViewState["PAGENUMBER"] = 1;
                Rebind();

            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlStockClass.SelectedHard = "";
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
    private void StoresubclassInsert(int storeclassid, string subclass)
    {
        PhoenixRegistersStoreSubClass.StoresubclassInsert(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, storeclassid, subclass);
        ucStatus.Text = "Information Added";
    }
    private void StoresubclassUpdate(int subclassid,  string subclass)
    {
        PhoenixRegistersStoreSubClass.StoresubclassUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                subclassid,  subclass);
        ucStatus.Text = "Information updated";
    }

    private void StoresubclassDelete(int subclassid)
    {
        PhoenixRegistersStoreSubClass.StoresubclassDelete(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, subclassid);
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            StoresubclassDelete(Int32.Parse(ViewState["ID"].ToString()));
            Rebind();
            ucStatus.Text = "Information deleted";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSubclass_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidconfiguration(((UserControlHard)e.Item.FindControl("ddlStockClassadd")).SelectedHard,
                   ((RadTextBox)e.Item.FindControl("txtsubclassadd")).Text))

                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                StoresubclassInsert(
                    (int.Parse(((UserControlHard)e.Item.FindControl("ddlStockClassadd")).SelectedHard)),
                    ((RadTextBox)e.Item.FindControl("txtsubclassadd")).Text.Trim()
                    );
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidconfigurationupdate(
                   ((RadTextBox)e.Item.FindControl("txtsubclassedit")).Text))

                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                StoresubclassUpdate((Int32.Parse(((RadLabel)e.Item.FindControl("lblIdedit")).Text)),
                    ((RadTextBox)e.Item.FindControl("txtsubclassedit")).Text.Trim()
                     );
                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["ID"] = ((RadLabel)e.Item.FindControl("lblId")).Text;
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
    private bool IsValidconfigurationupdate(string subclass)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvSubclass;
       if (subclass.Trim().Equals(""))
            ucError.ErrorMessage = "SubClass is required.";
       return (!ucError.IsError);
    }
    private bool IsValidconfiguration(string ddlstock, string subclass)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvSubclass;

        if (ddlstock.Equals("Dummy") || ddlstock.Equals(""))
            ucError.ErrorMessage = "Store Type is required.";

        if (subclass.Trim().Equals(""))
            ucError.ErrorMessage = "SubClass is required.";
        return (!ucError.IsError);
    }

    protected void gvSubclass_ItemDataBound(object sender, GridItemEventArgs e)
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
}