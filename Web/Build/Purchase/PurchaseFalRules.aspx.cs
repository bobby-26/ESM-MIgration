using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class PurchaseFalRules : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseFalRules.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseFalRules.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/PurchaseFalRulesAdd.aspx','false','850px','500px')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            MenuPurchaseFalRules.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                gvPurchaseFalRule.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPurchaseFalRule_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPurchaseFalRule.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataTable ds = PhoenixPurchaseFalRules.PurchaseFalRulesSearch
                (General.GetNullableString(txtshortcode.Text),
                General.GetNullableString(txtname.Text),
                  gvPurchaseFalRule.CurrentPageIndex + 1,
                  gvPurchaseFalRule.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

            gvPurchaseFalRule.DataSource = ds;
            gvPurchaseFalRule.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuPurchaseFalRules_TabStripCommand(object sender, EventArgs e)
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
                txtshortcode.Text = "";
                txtname.Text = "";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvPurchaseFalRule.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvPurchaseFalRule.SelectedIndexes.Clear();
        gvPurchaseFalRule.EditIndexes.Clear();
        gvPurchaseFalRule.DataSource = null;
        gvPurchaseFalRule.Rebind();
    }


    protected void gvPurchaseFalRule_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;

                RadCheckBox cb = (RadCheckBox)e.Item.FindControl("ChkActive");
                cb.Enabled = SessionUtil.CanAccess(this.ViewState, "ACTIVE");
                cb.Checked = DataBinder.Eval(e.Item.DataItem, "FLDISACTIVE").ToString().Equals("0") ? false : true;

                Guid? ApproveRuleId = General.GetNullableGuid(((RadLabel)item.FindControl("lblId")).Text);
                LinkButton edit = ((LinkButton)item.FindControl("cmdEdit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','','Purchase/PurchaseFalRulesEdit.aspx?APPROVERULEID=" + ApproveRuleId + "','false','850px','500px');return false");
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvPurchaseFalRule_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["ID"] = ((RadLabel)e.Item.FindControl("lblId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixPurchaseFalRules.PurchaseFalApproveRuleDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
              General.GetNullableGuid(ViewState["ID"].ToString()));

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }





}