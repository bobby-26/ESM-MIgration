using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class PurchaseConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddImageButton("../Purchase/PurchaseConfiguration.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Purchase/PurchaseConfiguration.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuPurchaseconfiguration.AccessRights = this.ViewState;
            MenuPurchaseconfiguration.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvPurchaseconfiguration.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvPurchaseconfiguration.SelectedIndexes.Clear();
        gvPurchaseconfiguration.EditIndexes.Clear();
        gvPurchaseconfiguration.DataSource = null;
        gvPurchaseconfiguration.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;



        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseConfiguration.Searchpurchaseconfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableString(txtcode.Text),
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvPurchaseconfiguration.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);


        gvPurchaseconfiguration.DataSource = ds;
        gvPurchaseconfiguration.VirtualItemCount = iRowCount;



        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    protected void gvPurchaseconfiguration_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPurchaseconfiguration.CurrentPageIndex + 1;
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
                txtcode.Text = "";

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
    private void Insertpurchaseconfiguration(string Code, string value,string functionality, string description)
    {
        PhoenixPurchaseConfiguration.Insertpurchaseconfiguration(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Code, value, functionality, description);
        ucStatus.Text = "Information Added";
    }
    private void Updatepurchaseconfiguration(int id,string value, string functionality, string description)
    {
        PhoenixPurchaseConfiguration.Updatepurchaseconfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                id, value, functionality, description);
        ucStatus.Text = "Information updated";
    }

    private void deletepurchaseconfiguration(int id)
    {
        PhoenixPurchaseConfiguration.deletepurchaseconfiguration(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, id);
    }

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            deletepurchaseconfiguration(Int32.Parse(ViewState["ID"].ToString()));
            Rebind();
            ucStatus.Text = "Information deleted";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPurchaseconfiguration_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {



            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidconfiguration(((RadTextBox)e.Item.FindControl("txtcodeadd")).Text,
                   ((RadTextBox)e.Item.FindControl("txtvalueadd")).Text))

                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                Insertpurchaseconfiguration(
                    ((RadTextBox)e.Item.FindControl("txtcodeadd")).Text.Trim(),
                    ((RadTextBox)e.Item.FindControl("txtvalueadd")).Text.Trim(),
                    ((RadTextBox)e.Item.FindControl("txtFunctionalityadd")).Text.Trim(),
                     ((RadTextBox)e.Item.FindControl("txtDescriptionadd")).Text.Trim()
                    );
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidconfigurationupdate(
                   ((RadTextBox)e.Item.FindControl("txtvalueEdit")).Text))

                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                Updatepurchaseconfiguration((Int32.Parse(((RadLabel)e.Item.FindControl("lblIdedit")).Text)),
                     
                     ((RadTextBox)e.Item.FindControl("txtvalueEdit")).Text.Trim(),
                     ((RadTextBox)e.Item.FindControl("txtFunctionalityEdit")).Text.Trim(),
                     ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text.Trim()
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
    private bool IsValidconfigurationupdate(string value)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvPurchaseconfiguration;

       

        if (value.Trim().Equals(""))
            ucError.ErrorMessage = "Value is required.";



        return (!ucError.IsError);
    }
    private bool IsValidconfiguration(string code, string value)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvPurchaseconfiguration;

        if (code.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (value.Trim().Equals(""))
            ucError.ErrorMessage = "Value is required.";



        return (!ucError.IsError);
    }

    protected void gvPurchaseconfiguration_ItemDataBound(object sender, GridItemEventArgs e)
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