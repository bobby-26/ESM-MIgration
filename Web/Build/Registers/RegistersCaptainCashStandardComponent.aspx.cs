using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersCaptainCashStandardComponent : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCaptainCashStandardComponent.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCCStdComp')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuCCStandardComponent.AccessRights = this.ViewState;
            MenuCCStandardComponent.MenuList = toolbar.Show();
            if (!IsPostBack)
                gvCCStdComp.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        string[] alColumns = { "FLDLOGTYPENAME", "FLDHARDNAME", "FLDDESCRIPTION", "FLDBUDGETCODE" };
        string[] alCaptions = { "Component Type", "Component Sub Type", "Description", "Budget Code" };
        DataTable dt = PhoenixRegistersPortageBillStandardComponent.ListCaptainCashComponent(null);
        General.ShowExcel("Captain Cash Standard Component", dt, alColumns, alCaptions, null, string.Empty);
    }
    protected void Rebind()
    {
        gvCCStdComp.SelectedIndexes.Clear();
        gvCCStdComp.EditIndexes.Clear();
        gvCCStdComp.DataSource = null;
        gvCCStdComp.Rebind();
    }
    protected void CCStandardComponent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
                ShowExcel();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        string[] alColumns = { "FLDLOGTYPENAME", "FLDHARDNAME", "FLDDESCRIPTION", "FLDBUDGETCODE" };
        string[] alCaptions = { "Component Type", "Component Sub Type", "Description", "Budget Code" };

        DataTable dt = PhoenixRegistersPortageBillStandardComponent.ListCaptainCashComponent(null);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvCCStdComp", "Captain Cash", alCaptions, alColumns, ds);
        gvCCStdComp.DataSource = dt;
        gvCCStdComp.VirtualItemCount = ds.Tables[0].Rows.Count;
    }
    protected void gvCCStdComp_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            RadComboBox ct = (RadComboBox)e.Item.FindControl("ddlComponentTypeEdit");
            if (ct != null)
            {
                DataSet dsCL = PhoenixRegistersPortageBillStandardComponent.ListPortageBillComponentHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 2, 0, "OOB,OCB,PRC,BNC,CRP,ROD,CTM,OCP,OCA,CCA,DEC");
                ct.DataSource = dsCL;
                ct.DataBind();
                ct.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                ct.SelectedValue = drv["FLDLOGTYPE"].ToString();
                ddlComponentType_SelectedIndexChanged(ct, null);
            }
            UserControlHard hrd = (UserControlHard)e.Item.FindControl("ddlHardEdit");
            if (hrd != null)
                hrd.SelectedHard = drv["FLDWAGEHEADID"].ToString();
            LinkButton ib1 = (LinkButton)e.Item.FindControl("btnShowBudgetEdit");
            if (ib1 != null) ib1.Attributes.Add("onclick", "showPickList('spnPickListBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudget.aspx?budgetgroup=102&hardtypecode=30', false); return false;");

        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            RadComboBox at = (RadComboBox)e.Item.FindControl("ddlComponentTypeAdd");
            if (at != null)
            {
                DataSet dsCL = PhoenixRegistersPortageBillStandardComponent.ListPortageBillComponentHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 2, 0, "OOB,OCB,PRC,BNC,CRP,ROD,CTM,OCP,OCA,CCA,DEC");
                at.DataSource = dsCL;
                at.DataBind();
                at.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
            }
            UserControlHard hrd = (UserControlHard)e.Item.FindControl("ddlHardAdd");
            LinkButton ib1 = (LinkButton)e.Item.FindControl("btnShowBudgetAdd");
            if (ib1 != null) ib1.Attributes.Add("onclick", "showPickList('spnPickListBudgetAdd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudget.aspx?budgetgroup=102&hardtypecode=30', false); return false;");

        }
    }

    private bool IsValidPBStandardComponent(string logtype, string desc, string budgetid)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableInteger(logtype).HasValue)
            ucError.ErrorMessage = "Component Type is required.";

        if (desc.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        if (!General.GetNullableInteger(budgetid).HasValue)
            ucError.ErrorMessage = "Budget Code is required.";

        return (!ucError.IsError);
    }

    protected void ddlComponentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            RadComboBox ddl = (RadComboBox)sender;
            UserControlHard subtype;
            GridDataItem row = (GridDataItem)ddl.Parent.Parent;
            subtype = row.FindControl("ddlHardEdit") as UserControlHard;
            if (General.GetNullableInteger(ddl.SelectedValue).HasValue && (General.GetNullableInteger(ddl.SelectedValue).Value == 13 || General.GetNullableInteger(ddl.SelectedValue).Value == 14))
            {
                subtype.HardList = PhoenixRegistersHard.ListHard(1, 229, 0, "AIB,AIC,AIP");
                subtype.Enabled = true;
            }
            else if (General.GetNullableInteger(ddl.SelectedValue).HasValue && (General.GetNullableInteger(ddl.SelectedValue).Value == 19 || General.GetNullableInteger(ddl.SelectedValue).Value == 20))
            {
                subtype.HardList = PhoenixRegistersHard.ListHard(1, 229, 0, "AIC");
                subtype.Enabled = true;
            }
            else if (General.GetNullableInteger(ddl.SelectedValue).HasValue && (General.GetNullableInteger(ddl.SelectedValue).Value == 15 || General.GetNullableInteger(ddl.SelectedValue).Value == 16))
            {
                subtype.HardList = PhoenixRegistersHard.ListHard(1, 229, 0, "AIO,AIH,AIS");
                subtype.Enabled = true;
            }
            if (General.GetNullableInteger(ddl.SelectedValue).HasValue && (General.GetNullableInteger(ddl.SelectedValue).Value == 29))
            {
                subtype.HardList = PhoenixRegistersHard.ListHard(1, 229, 0, "AIB");
                subtype.Enabled = true;
            }
            else if (General.GetNullableInteger(ddl.SelectedValue).HasValue && (General.GetNullableInteger(ddl.SelectedValue).Value == 17 || General.GetNullableInteger(ddl.SelectedValue).Value == 18 || General.GetNullableInteger(ddl.SelectedValue).Value == 21))
            {
                subtype.HardList = PhoenixRegistersHard.ListHard(1, 229, 0, "AIB,AIP");
                subtype.Enabled = true;
            }
            else
            {
                subtype.SelectedHard = "";
                subtype.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlComponentType1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            RadComboBox ddl = (RadComboBox)sender;
            UserControlHard subtype;

            GridFooterItem f = (GridFooterItem)ddl.Parent.Parent;
            subtype = f.FindControl("ddlHardAdd") as UserControlHard;
            if (General.GetNullableInteger(ddl.SelectedValue).HasValue && (General.GetNullableInteger(ddl.SelectedValue).Value == 13 || General.GetNullableInteger(ddl.SelectedValue).Value == 14))
            {
                subtype.HardList = PhoenixRegistersHard.ListHard(1, 229, 0, "AIB,AIC,AIP");
                subtype.Enabled = true;
            }
            else if (General.GetNullableInteger(ddl.SelectedValue).HasValue && (General.GetNullableInteger(ddl.SelectedValue).Value == 19 || General.GetNullableInteger(ddl.SelectedValue).Value == 20))
            {
                subtype.HardList = PhoenixRegistersHard.ListHard(1, 229, 0, "AIC");
                subtype.Enabled = true;
            }
            else if (General.GetNullableInteger(ddl.SelectedValue).HasValue && (General.GetNullableInteger(ddl.SelectedValue).Value == 15 || General.GetNullableInteger(ddl.SelectedValue).Value == 16))
            {
                subtype.HardList = PhoenixRegistersHard.ListHard(1, 229, 0, "AIO,AIH,AIS");
                subtype.Enabled = true;
            }
            if (General.GetNullableInteger(ddl.SelectedValue).HasValue && (General.GetNullableInteger(ddl.SelectedValue).Value == 29))
            {
                subtype.HardList = PhoenixRegistersHard.ListHard(1, 229, 0, "AIB");
                subtype.Enabled = true;
            }
            else if (General.GetNullableInteger(ddl.SelectedValue).HasValue && (General.GetNullableInteger(ddl.SelectedValue).Value == 17 || General.GetNullableInteger(ddl.SelectedValue).Value == 18 || General.GetNullableInteger(ddl.SelectedValue).Value == 21))
            {
                subtype.HardList = PhoenixRegistersHard.ListHard(1, 229, 0, "AIB,AIP");
                subtype.Enabled = true;
            }
            else
            {
                subtype.SelectedHard = "";
                subtype.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCCStdComp_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                string logtype = ((RadComboBox)e.Item.FindControl("ddlComponentTypeAdd")).SelectedValue;
                string wagehead = ((UserControlHard)e.Item.FindControl("ddlHardAdd")).SelectedHard;
                string code = ((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text;
                string desc = ((RadTextBox)e.Item.FindControl("txtDescAdd")).Text;
                string budget = ((RadTextBox)e.Item.FindControl("txtBudgetIdAdd")).Text;
                if (!IsValidPBStandardComponent(logtype, desc, budget))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersPortageBillStandardComponent.InsertPortageBillComponent(int.Parse(logtype),
                            General.GetNullableInteger(wagehead), code, desc, General.GetNullableInteger(budget));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                Guid id = new Guid(((RadLabel)e.Item.FindControl("lbldtkeyedit")).Text);
                string logtype = ((RadComboBox)e.Item.FindControl("ddlComponentTypeEdit")).SelectedValue;
                string wagehead = ((UserControlHard)e.Item.FindControl("ddlHardEdit")).SelectedHard;
                string code = ((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text;
                string desc = ((RadTextBox)e.Item.FindControl("txtDescEdit")).Text;
                string budget = ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text;
                if (!IsValidPBStandardComponent(logtype, desc, budget))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersPortageBillStandardComponent.UpdatePortageBillComponent(id, int.Parse(logtype)
                    , General.GetNullableInteger(wagehead), code, desc, General.GetNullableInteger(budget));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {

                string dtkey = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
                PhoenixRegistersPortageBillStandardComponent.DeleteAirlines(new Guid(dtkey));
                Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCCStdComp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
