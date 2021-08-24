using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class RegisterUserFuntionalRoleDesgination : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegisterUserFuntionalRoleDesgination.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRoleDesignationMapping')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRoleDesignationMapping.AccessRights = this.ViewState;
            MenuRoleDesignationMapping.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                gvRoleDesignationMapping.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                BindRole();
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void BindRole()
    {
        ddlRole.DataSource = PhoenixRegistersRole.ListRole();
        ddlRole.DataTextField = "FLDROLENAME";
        ddlRole.DataValueField = "FLDROLEID";
        ddlRole.DataBind();
        ddlRole.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROLENAME", "FLDDESIGNATIONNAME" };
        string[] alCaptions = { "Role", "Designation" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersRole.RoleDesignationMappingSearch(General.GetNullableInteger(ddlRole.SelectedValue)
            , sortexpression, sortdirection,
            gvRoleDesignationMapping.CurrentPageIndex + 1,
            gvRoleDesignationMapping.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvRoleDesignationMapping", "Functional Role Designation", alCaptions, alColumns, ds);
        
        gvRoleDesignationMapping.DataSource = ds;
        gvRoleDesignationMapping.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROLENAME", "FLDDESIGNATIONNAME"};
        string[] alCaptions = { "Role", "Designation"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersRole.RoleDesignationMappingSearch(General.GetNullableInteger(ddlRole.SelectedValue)
            , sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        General.ShowExcel("Functional Role Designation", ds.Tables[0], alColumns, alCaptions, null, null);

    }
    protected void MenuRoleDesignationMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvRoleDesignationMapping_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRoleDesignationMapping.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = null;
        gvRoleDesignationMapping.Rebind();
    }

    protected void ddlRole_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvRoleDesignationMapping.Rebind();
    }
}