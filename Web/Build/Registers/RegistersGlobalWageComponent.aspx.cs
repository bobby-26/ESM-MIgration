using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Data;

public partial class RegistersGlobalWageComponent : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersGlobalWageComponent.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvGWC')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersGlobalWageComponent.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersGlobalWageComponent.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            //toolbar.AddFontAwesomeButton("../Registers/RegistersGlobalWageComponentAdd.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDGWC");
            MenuRegisterGWC.AccessRights = this.ViewState;
            MenuRegisterGWC.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            ucConfirmDelete.Attributes.Add("style", "display:none");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["WAGECOMPID"] = "";
                gvGWC.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRegisterGWC_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtCode.Text = "";
                txtSearch.Text = "";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvGWC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvGWC.CurrentPageIndex + 1;
        BindData();
    }
    protected void Rebind()
    {
        gvGWC.SelectedIndexes.Clear();
        gvGWC.EditIndexes.Clear();
        //gvGWC.DataSource = null;
        gvGWC.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int active;
        string[] alColumns = { "FLDCODE", "FLDNAME", "FLDISACTIVEYN" };
        string[] alCaptions = { "Code", "Global Wage Component", "IsActive" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (chkActive.Checked == true)
        {
            active = 1;
        }
        else
        {
            active = 0;
        }
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegisterGlobalWageComponent.GloabalWageComponentSearch(
                    txtSearch.Text,
                    txtCode.Text.Trim(),
                    active,
                    sortexpression, sortdirection,
                    gvGWC.CurrentPageIndex+1,
                    gvGWC.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvGWC", "Global Wage Component", alCaptions, alColumns, ds);

        gvGWC.DataSource = ds;
        gvGWC.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int active;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCODE", "FLDNAME", "FLDISACTIVEYN" };
        string[] alCaptions = { "Code", "Global Wage Component", "IsActive" };

        string sortexpression;
        int? sortdirection = null;

        if (chkActive.Checked == true)
        {
            active = 1;
        }
        else
        {
            active = 0;
        }

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegisterGlobalWageComponent.GloabalWageComponentSearch(
                    txtSearch.Text.Trim(),
                    txtCode.Text.Trim(),
                    active,
                    sortexpression, sortdirection,
                    gvGWC.CurrentPageIndex + 1,
                    gvGWC.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.ShowExcel("Global Wage Component", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void gvGWC_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().ToString().Equals("EDIT"))
            return;

        if (e.CommandName.ToUpper().ToString().Equals("ADD"))
        {
            string shortcode = ((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text;
            string WagecompName = ((RadTextBox)e.Item.FindControl("txtWageComponentNameAdd")).Text;
            RadCheckBox isactive = (RadCheckBox)e.Item.FindControl("chkIsActiveAdd");

            if (!IsValid(shortcode, WagecompName))
            {
                ucError.Visible = true;
                e.Canceled = true;
                return;
            }

            PhoenixRegisterGlobalWageComponent.InsertGlobalWageComponent(PhoenixSecurityContext.CurrentSecurityContext.UserCode, WagecompName.Trim().ToString(), shortcode.Trim().ToString(), isactive.Checked == true ? 1 : 0);
            Rebind();

        }
        if (e.CommandName.ToUpper().ToString().Equals("SAVE"))
        {
            string shortcode = ((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text;
            string wagecompId = ((RadLabel)e.Item.FindControl("lblWageComponentIdEdit")).Text;
            string WagecompName = ((RadTextBox)e.Item.FindControl("lblWageComponentNameEdit")).Text;
            RadCheckBox isactive = (RadCheckBox)e.Item.FindControl("chkIsActiveEdit");

            if (!IsValid(shortcode, WagecompName))
            {
                ucError.Visible = true;
                e.Canceled = true;
                return;
            }
            PhoenixRegisterGlobalWageComponent.UpdateGlobalWageComponent(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(wagecompId), WagecompName.Trim().ToString(), shortcode.Trim().ToString(), isactive.Checked == true ? 1 : 0);
            Rebind();
        }
        if (e.CommandName.ToUpper().ToString().Equals("DELETE"))
        {

            string confirmtextunlock;

            //string wagecompId = ((RadLabel)e.Item.FindControl("lblWageComponentId")).Text;
            ViewState["WAGECOMPID"] = ((RadLabel)e.Item.FindControl("lblWageComponentId")).Text;


            confirmtextunlock = "Are you sure you want delete ?";
            RadWindowManager1.RadConfirm(confirmtextunlock, "DeleteRecord", 320, 150, null, "Confirm delete");            
        }


        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
            //Rebind();
        }
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixRegisterGlobalWageComponent.DeleteComponent(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["WAGECOMPID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvGWC_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvGWC_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    private new bool IsValid(string shortcode, string name)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(shortcode))
            ucError.ErrorMessage = "Code is required.";
        if (string.IsNullOrEmpty(name))
            ucError.ErrorMessage = "Component is required.";
        return (!ucError.IsError);
    }
}