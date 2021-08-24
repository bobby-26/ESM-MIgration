using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;

public partial class Registers_RegistersAdminAssetFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO");
            toolbar.AddButton("Cancel", "CANCEL");
            AssetSearchFilter.AccessRights = this.ViewState;
            AssetSearchFilter.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                BindAssetType();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindAssetType()
    {
        try
        {
            ddlAssetType.Items.Clear();
            ListItem li = new ListItem("--Select--", "");
            DataTable dt = PhoenixRegistersAssetType.ListAssetType(1, 1);
            ddlAssetType.DataSource = dt;
            ddlAssetType.DataBind();
            ddlAssetType.Items.Insert(0, li);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void AssetSearchFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList();";
            Script += "</script>" + "\n";
            if (dce.CommandName.ToUpper().Equals("GO"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtSearch", txtSearch.Text);
                criteria.Add("ddlLocation", ddlLocation.SelectedZone);
                criteria.Add("txtDescription", txtDescription.Text);
                criteria.Add("ddlAssetType", ddlAssetType.SelectedValue);
                criteria.Add("txtMaker", txtMaker.Text);
                criteria.Add("txtModel", txtModel.Text);
                criteria.Add("txtSerialNo", txtSerialNo.Text);
                criteria.Add("FindType", "ASSETFIND");

                Filter.CurrentAssetSearchFilter = criteria;
                Filter.CurrentDisposedAssetSearchFilter = null;
            }
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
