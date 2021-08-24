using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseRules : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseRules.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseRules.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/PurchaseRulesAdd.aspx','false','450px','180px')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseRules.aspx", "Include All", "<i class=\"fa fa-check-circle\"></i>", "SELECTALL");
            MenuPurchaseRules.AccessRights = this.ViewState;
            MenuPurchaseRules.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvPurchaseRule.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }   
}


    protected void MenuPurchaseRules_TabStripCommand(object sender, EventArgs e)
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

            if (CommandName.ToUpper().Equals("SELECTALL"))
            {
                PhoenixPurchaseRules.PurchaseRulesAllRequired(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Status()
                   );

                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private int Status()
    {
        if (ViewState["SELECTALL"] == null)
        {
            ViewState["SELECTALL"] = 0;
        }

        int status = int.Parse(ViewState["SELECTALL"].ToString());
        if (status == 0)
        {
            ViewState["SELECTALL"] = 1;
            return 1;
        }
        else
        {
            ViewState["SELECTALL"] = 0;
            return 0;
        }
    }



    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
      
            DataTable ds = PhoenixPurchaseRules.PurchaseRulesSearch(General.GetNullableString(txtshortcode.Text.Trim()),
                    General.GetNullableString(txtname.Text.Trim()),               
                  gvPurchaseRule.CurrentPageIndex + 1,
                  gvPurchaseRule.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

            gvPurchaseRule.DataSource = ds;
            gvPurchaseRule.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvPurchaseRule_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPurchaseRule.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvPurchaseRule.SelectedIndexes.Clear();
        gvPurchaseRule.EditIndexes.Clear();
        gvPurchaseRule.DataSource = null;
        gvPurchaseRule.Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvPurchaseRule.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvPurchaseRule_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["ID"] = ((RadLabel)e.Item.FindControl("lblId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }

            if (e.CommandName.ToString().ToUpper().Equals("ACTIVE"))
            {
                int RequiredYN = (((RadCheckBox)e.Item.FindControl("ChkActive")).Checked == true) ? 1 : 0;
                Guid? RuleId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblId")).Text);

                PhoenixPurchaseRules.PurchaseRulesRequired(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,                  
                    RequiredYN,
                     RuleId
                    );

                Rebind();
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
            PhoenixPurchaseRules.DeletePurchaseRules(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(ViewState["ID"].ToString()));

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPurchaseRule_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;

                RadCheckBox cb = (RadCheckBox)e.Item.FindControl("ChkActive");
                cb.Enabled = SessionUtil.CanAccess(this.ViewState, "ACTIVE");
                cb.Checked = DataBinder.Eval(e.Item.DataItem, "FLDREQUIRED").ToString().Equals("0") ? false : true;

                Guid? RuleId = General.GetNullableGuid(((RadLabel)item.FindControl("lblId")).Text);
                LinkButton edit = ((LinkButton)item.FindControl("cmdEdit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','','Purchase/PurchaseRulesEdit.aspx?RULEID=" + RuleId + "','false','450px','180px');return false");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}