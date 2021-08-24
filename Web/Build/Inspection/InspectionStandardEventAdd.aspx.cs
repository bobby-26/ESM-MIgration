using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Inspection_InspectionStandardEventAdd : PhoenixBasePage

{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuCARGeneral.AccessRights = this.ViewState;
            MenuCARGeneral.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["STANDARDEVENTID"] = "";

                if (Request.QueryString["STANDARDEVENTID"] != null && Request.QueryString["STANDARDEVENTID"].ToString() != string.Empty)
                    ViewState["STANDARDEVENTID"] = Request.QueryString["STANDARDEVENTID"].ToString();
                BindActivity();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindActivity()
    {
        if (ViewState["STANDARDEVENTID"] != null && ViewState["STANDARDEVENTID"].ToString() != string.Empty)
        {
            DataSet dt = PhoenixInspectionStandardEvent.ProcessStandardEventEdit(int.Parse(ViewState["STANDARDEVENTID"].ToString()));
            if (dt.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dt.Tables[0].Rows[0];
                txtcode.Text = dr["FLDEVENTCODE"].ToString();
                txtname.Text = dr["FLDEVENTNAME"].ToString();
                ucCategory.SelectedCategory = dr["FLDPROCESSID"].ToString();

            }
        }
    }

    protected void MenuCARGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["STANDARDEVENTID"] != null && ViewState["STANDARDEVENTID"].ToString() != string.Empty)
                    UpdateActivity();
                else
                    InsertActivity();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void InsertActivity()
    {
        if (!IsValidRAActivity())
        {
            ucError.Visible = true;
            return;
        }

        PhoenixInspectionStandardEvent.StandardEventInsert(int.Parse(ucCategory.SelectedCategory)
                                                          , txtcode.Text.Trim()
                                                          , txtname.Text.Trim()
                                                            );

        ucStatus.Text = "Information updated.";
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void UpdateActivity()
    {
        if (!IsValidRAActivity())
        {
            ucError.Visible = true;
            return;
        }

        PhoenixInspectionStandardEvent.StandardEventUpdate(
      int.Parse(ViewState["STANDARDEVENTID"].ToString())
      , int.Parse(ucCategory.SelectedCategory)
      , txtname.Text.Trim()
      , txtcode.Text.Trim()
   );

        ucStatus.Text = "Information updated.";
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    private bool IsValidRAActivity()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtcode.Text.Trim()) == null)
            ucError.ErrorMessage = "Code is required.";

        if (General.GetNullableString(txtname.Text.Trim()) == null)
            ucError.ErrorMessage = "Name is required.";

        if (General.GetNullableInteger(ucCategory.SelectedCategory) == null)
            ucError.ErrorMessage = "Process is required.";

        return (!ucError.IsError);
    }

    protected void ucCategory_TextChangedEvent(object sender, EventArgs e)
    {
       // Bindevent();
    }
}