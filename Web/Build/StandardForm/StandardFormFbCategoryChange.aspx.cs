using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.StandardForm;

public partial class StandardFormFbCategoryChange : System.Web.UI.Page
{
    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Move", "MOVE");
            toolbarmain.AddButton("Cancel", "CANCEL");
            MenuInventorySpareMove.AccessRights = this.ViewState;
            MenuInventorySpareMove.MenuList = toolbarmain.Show();
            MenuInventorySpareMove.SetTrigger(pnlSpareItemMove);

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                lblformId.Text = Request.QueryString["formId"].ToString();
                BindDetails();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void BindDetails()
    {
        DataTable dt;

        dt = PhoenixFormBuilder.FormEdit(new Guid(lblformId.Text));

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtCurrenCategory.Text = dr["FLDFORMCATEGORYNAME"].ToString();
            txtFormName.Text = dr["FLDFORMNAME"].ToString();
        }

        ddlCategoryList.DataSource = PhoenixFormBuilder.FormsCategoryList();
        ddlCategoryList.DataBind();

    }

    protected void MenuInventorySpareMove_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("MOVE"))
            {

                if (!IsValidSpareItemMove())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixFormBuilder.formCategoryChange(
                    new Guid(lblformId.Text), int.Parse(ddlCategoryList.SelectedValue));

                ucStatus.Text = "Form moved to "+ ddlCategoryList.SelectedItem.Text.TrimStart('-');
                BindDetails();

                string Script = "";
                Script += "fnReloadList('codehelp1','ifMoreInfo','keepopen');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", Script, true);
            }

            if (dce.CommandName.ToUpper().Equals("CANCEL"))
            {
                String script = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidSpareItemMove()
    {
        ucError.HeaderMessage = "Please provide the following required information";



        if (General.GetNullableInteger(ddlCategoryList.SelectedValue) == null)
            ucError.ErrorMessage = "'Move to' Location is required.";

        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
