using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class RegistersFunctionalRoleDesignationMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuMapping.MenuList = toolbar.Show();
            if (!IsPostBack)
            {

                ViewState["ROLEID"] = "";

                if (Request.QueryString["roleid"] != null)
                    ViewState["ROLEID"] = Request.QueryString["roleid"].ToString();

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
                if (ViewState["ROLEID"].ToString() != "")
                {
                    string source = General.RadCheckBoxList(cblDesignation);

                    PhoenixRegistersRole.UpdateFunctionalRoleDesignation(General.GetNullableInteger(ViewState["ROLEID"].ToString())
                         , General.GetNullableString(source));

                    ucStatus.Text = "Information Updated successfully.";
                }
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
        //cblDesignation.Items.Clear();
        cblDesignation.DataSource = PhoenixRegistersDesignation.ListDesignation();
        cblDesignation.DataBindings.DataTextField = "FLDDESIGNATIONNAME";
        cblDesignation.DataBindings.DataValueField = "FLDDESIGNATIONID";
        cblDesignation.DataBind();
    }
    private void BindData()
    {
        if (ViewState["ROLEID"].ToString() == "")
        {
            BindCheckbox();
        }
        else
        {
            DataSet ds = PhoenixRegistersRole.EditRole(int.Parse(ViewState["ROLEID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtRole.Text = dr["FLDROLENAME"].ToString();
                General.RadBindCheckBoxList(cblDesignation, dr["FLDDESIGNATION"].ToString());
            }
        }
    }
}