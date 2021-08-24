using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionMOCSubCategoryMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["SUBCATEGORYID"] = "";

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);                
                MenuMapping.MenuList = toolbar.Show();
                //MenuMapping.SetTrigger(pnlInspectionMapping);
                if (Request.QueryString["subcategoryid"] != null)
                    ViewState["SUBCATEGORYID"] = Request.QueryString["subcategoryid"].ToString();

                BindCheckbox();
                BindData();
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void MenuMapping_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string source = ReadCheckBoxList(cbltypelist);

                PhoenixInspectionMOC.MOCSubCategoryMappingUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["SUBCATEGORYID"].ToString())
                    , source);
                ucStatus.Text = "Categories are mapped successfully.";

                BindData();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupopen, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindCheckbox()
    {
        cbltypelist.Items.Clear();
        cbltypelist.DataSource = PhoenixInspectionMOC.ListMOCHardType(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        cbltypelist.DataTextField = "FLDMOCHARDTYPENAME";
        cbltypelist.DataValueField = "FLDMOCHARDTYPECODE";
        cbltypelist.DataBind();
    }
    private void BindData()
    {
        BindCheckbox();
        DataTable dt = PhoenixInspectionMOC.MOCSubCategorymappingedit(General.GetNullableGuid(ViewState["SUBCATEGORYID"].ToString()));

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow dr = (DataRow)dt.Rows[i];
            txtmochardname.Text = dr["FLDMOCHARDNAME"].ToString();
            string s = dr["FLDTYPELIST"].ToString();
            General.BindCheckBoxList(cbltypelist, s);
        }
    }
    private string ReadCheckBoxList(CheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }
}
