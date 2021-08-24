using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class InspectionDeficiencyCategoryVesselTypeMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuMapping.MenuList = toolbar.Show();
            if (!IsPostBack)
            {

            
              //  MenuMapping.SetTrigger(pnlTypeMapping);

                if (Request.QueryString["categoryid"] != null)
                    ViewState["CATEGORYID"] = Request.QueryString["categoryid"].ToString();

                if (Request.QueryString["category"] != null)
                    txtCategory.Text = Request.QueryString["category"].ToString();

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
                string source = ReadCheckBoxList(cblType);

                    PhoenixInspectionDeficiency.UpdateDeficiencyCategoryVesselType(General.GetNullableInteger(ViewState["CATEGORYID"].ToString())
                    , source);
                ucStatus.Text = "Vessel Types are mapped successfully.";

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
        cblType.Items.Clear();
        cblType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 81);
        cblType.DataBindings.DataTextField = "FLDHARDNAME";
        cblType.DataBindings.DataValueField = "FLDHARDCODE";
        cblType.DataBind();
    }

    private void BindData()
    {
        BindCheckbox();
        DataTable dt = PhoenixInspectionDeficiency.ListDeficiencyCategoryVesselType(General.GetNullableInteger(ViewState["CATEGORYID"].ToString()));

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow dr = (DataRow)dt.Rows[i];
            string s = dr["FLDVESSELTYPEID"].ToString();
            if (cblType.Items[i].Value == s)
                cblType.Items[i].Selected = true;
        }
       
    }
    private string ReadCheckBoxList(RadCheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ButtonListItem item in cbl.Items)
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
