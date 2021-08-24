using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Accounts_AccountsSupplierAccountMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // MenuVesselAccount.SetTrigger(pnlVesselAccount);
        txtusercodes.Attributes.Add("style", "visibility:hidden");
        lblEmail.Attributes.Add("style", "visibility:hidden");
        cmdHiddenPick.Attributes.Add("style", "visibility:hidden");
        txtUserCode.Attributes.Add("style", "visibility:hidden");
        txtVendorId.Attributes.Add("style", "visibility:hidden");

        PhoenixToolbar toolbarlist = new PhoenixToolbar();
        toolbarlist.AddImageButton("../Accounts/AccountsSupplierAccountMapping.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
        MenuVesselAccountlist.AccessRights = this.ViewState;
        MenuVesselAccountlist.MenuList = toolbarlist.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvSupplierAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListSupMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx', true); ");
            ImgAccountsUserIdPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListUser.aspx', true); ");
        }

    }

    protected void MenuVesselAccountlist_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtDesignation.Text = "";
                txtVenderName.Text = "";
                txtVendorCode.Text = "";
                txtVendorId.Text = "";
                txtUserCode.Text = "";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    private bool IsValidUserAccount()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (txtUserCode.Text.Equals(""))
            ucError.ErrorMessage = "Account user id required.";
        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = PhoenixAccountsSupplierMapping.PhoenixAccountsSupplierMappingsearch(General.GetNullableInteger(txtUserCode.Text)
                        , General.GetNullableInteger(txtVendorId.Text)
                        , sortexpression
                        , sortdirection
                        , int.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvSupplierAccount.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);


        gvSupplierAccount.DataSource = ds;
        gvSupplierAccount.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void Rebind()
    {
        gvSupplierAccount.SelectedIndexes.Clear();
        gvSupplierAccount.EditIndexes.Clear();
        gvSupplierAccount.DataSource = null;
        gvSupplierAccount.Rebind();
    }
    protected void gvSupplierAccount_delete(object sender, GridCommandEventArgs de)
    {

    }

    protected void gvSupplierAccount_RowDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {

        }
        if (e.Item is GridDataItem)
        {
            if (txtUserCode.Text != "")
            {
                ImageButton imgadd = (ImageButton)e.Item.FindControl("imgAdd");
                RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkIncludeyn");
                DataRowView drv = (DataRowView)e.Item.DataItem;
                cb.Checked = drv["FLDCHECKED"].ToString().Equals("1") ? true : false;
                if (cb.Checked == true)
                {
                    cb.Enabled = true;
                    imgadd.Enabled = false;
                }
                else
                {
                    cb.Enabled = false;
                    imgadd.Enabled = true;
                }
            }


        }
    }
    protected void gvSupplierAccount_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                if (txtUserCode.Text != "")
                {

                    RadLabel lblSuppliercode = ((RadLabel)e.Item.FindControl("lblSupplierCode"));

                    PhoenixAccountsSupplierMapping.SaveSupplierAccount(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(txtUserCode.Text), int.Parse(lblSuppliercode.Text)
                               );
                    Rebind();

                }
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblSuppliercode = ((RadLabel)e.Item.FindControl("lblSupplierCode"));

                PhoenixAccountsSupplierMapping.SaveSupplierAccount(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(txtUserCode.Text), int.Parse(lblSuppliercode.Text)
                           );
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



    protected void gvSupplierAccount_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSupplierAccount.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
