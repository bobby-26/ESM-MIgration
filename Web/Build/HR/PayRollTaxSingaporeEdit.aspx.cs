using System;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class HR_PayRollTaxSingaporeEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        ShowToolBar();
        if(!IsPostBack)
            LoadData();
    }

    public void LoadData()
    {
        DataTable dt = PhoenixPayRollSingapore.TaxSingaporeDetail(General.GetNullableInteger(Request.QueryString["payrollid"]));
        if (dt.Rows.Count > 0)
        {
            radtbdescription.Text = dt.Rows[0]["FLDDESCRIPTION"].ToString();
            ucfromdate.Text = dt.Rows[0]["FLDFROMDATE"].ToString();
            uctodate.Text = dt.Rows[0]["FLDTODATE"].ToString();
            radddcurrency.SelectedCurrency = dt.Rows[0]["FLDCURRENCYID"].ToString();
            radstatus.SelectedValue = dt.Rows[0]["FLDSTATUS"].ToString();
            radtbchanges.Text = dt.Rows[0]["FLDREVISIONREMARKS"].ToString();
        }


    }
    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("Update", "UPDATE", ToolBarDirection.Right);


        gvTabStrip.MenuList = toolbarmain.Show();
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (IsValidReport())
            {
                ucError.Visible = true;
                return;
            }

            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixPayRollSingapore.TaxSingaporeUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, radtbdescription.Text, General.GetNullableDateTime(ucfromdate.Text), General.GetNullableDateTime(uctodate.Text), General.GetNullableInteger(radddcurrency.SelectedCurrency), General.GetNullableInteger(Request.QueryString["payrollid"]),General.GetNullableString(radtbchanges.Text),General.GetNullableInteger(radstatus.SelectedValue));
            }

            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidReport()
    {
        ucError.HeaderMessage = "Please provide the following ";
        if (string.IsNullOrWhiteSpace(ucfromdate.Text))
        {
            ucError.ErrorMessage = "From date ";
        }
        if (General.GetNullableDateTime(ucfromdate.Text) != null && General.GetNullableDateTime(uctodate.Text) != null)
        {
            if (!(General.GetNullableDateTime(uctodate.Text) > General.GetNullableDateTime(ucfromdate.Text)))
                ucError.ErrorMessage = "To date should be greater than from date. ";
        }
        if (string.IsNullOrWhiteSpace(radddcurrency.SelectedCurrency))
        {
            ucError.ErrorMessage = "Currency ";
        }

        if (string.IsNullOrWhiteSpace(radtbdescription.Text))
        {
            ucError.ErrorMessage = "Description ";
        }

        if (General.GetNullableInteger(radstatus.SelectedValue) ==0 && General.GetNullableDateTime(uctodate.Text) == null)
        {
            ucError.ErrorMessage = "While deactivating the configuration To date should be provided.";
        }
        return ucError.IsError;
    }
}