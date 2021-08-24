using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Data;
public partial class RegistersSupplierStoreTypeMapping_aspx : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuStockType.AccessRights = this.ViewState;
            MenuStockType.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                if (Request.QueryString["addresscode"] != null)
                    ViewState["ADDRESSCODE"] = Request.QueryString["addresscode"].ToString();
                BindStoreType();
                BindStoreTypeEdit();
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindStoreType()
    {
        try
        {
            chkStoreTypeMap.DataSource = PhoenixRegistersAddress.StoreTypeHard(((int)PhoenixHardTypeCode.STORETYPE)); ;
            chkStoreTypeMap.DataBindings.DataTextField = "FLDHARDNAME";
            chkStoreTypeMap.DataBindings.DataValueField = "FLDHARDCODE";
            chkStoreTypeMap.DataBind();
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuStockType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string StoreTypeMapId = string.Empty;
                foreach (ButtonListItem li in chkStoreTypeMap.Items)
                {
                    StoreTypeMapId += (li.Selected ? li.Value + "," : string.Empty);
                }
                StoreTypeMapId.TrimEnd(',');

                StoreTypeMappingInsertUpdate(StoreTypeMapId);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void StoreTypeMappingInsertUpdate(string StoreTypeMapId)
    {
        try
        {
            PhoenixRegistersAddress.InsertUpdateStoreTypeMapping(Convert.ToInt16(ViewState["ADDRESSCODE"].ToString()),
                                                                    General.GetNullableString(StoreTypeMapId));
            BindStoreTypeEdit();
            String script = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }
    protected void BindStoreTypeEdit()
    {
        DataSet ds = PhoenixRegistersAddress.BindEditStoreType(Convert.ToInt16(ViewState["ADDRESSCODE"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            string natureofwork = dr["FLDSTORETYPE"].ToString();

            if (!string.IsNullOrEmpty(natureofwork))
            {
                foreach (string val in natureofwork.Split(','))
                {
                    if (val.Trim() != "")
                    {
                        chkStoreTypeMap.SelectedValue = val;
                    }
                }
            }
        }
    }
}