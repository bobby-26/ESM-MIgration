using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class HR_PayRollSGEmployeeConfig : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar menu = new PhoenixToolbar();
        menu.AddButton("Payroll", "Toggle1", ToolBarDirection.Left);
        menu.AddButton("CPF", "Toggle2", ToolBarDirection.Left);
        menu.AddButton("SDL", "Toggle3", ToolBarDirection.Left);
        menu.AddButton("SHG funds", "Toggle4", ToolBarDirection.Left);
        menu.AddButton("FWL", "Toggle5", ToolBarDirection.Left);
        menu.AddButton("Employee", "Toggle6", ToolBarDirection.Left);
        //menu.AddButton("Employer", "Toggle7", ToolBarDirection.Left);

        gvmenu.MenuList = menu.Show();
        gvmenu.SelectedMenuIndex = 5;
        //ShowToolBar();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["PAYROLL"] = string.Empty;
            ViewState["EMPLOYEECODE"] = String.Empty;
            if (Request.QueryString["payrollid"] != null)
                ViewState["PAYROLL"] = Request.QueryString["payrollid"];
            gvemployeeconfig.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    //public void ShowToolBar()
    //{
    //    PhoenixToolbar toolbarmain = new PhoenixToolbar();
    //    toolbarmain.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/HR/PayRollSGEmployeeConfigAdd.aspx','false','550px','400px')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
    //    gvTabStrip.MenuList = toolbarmain.Show();
    //}

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvemployeeconfig.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("TOGGLE1"))
            {
                Response.Redirect("~/HR/PayRollTaxSingapore.aspx?payrollid=" + ViewState["PAYROLL"]);
            }
            if (CommandName.ToUpper().Equals("TOGGLE2"))
            {
                Response.Redirect("~/HR/PayRollSGCPF.aspx?payrollid=" + ViewState["PAYROLL"]);
            }
            if (CommandName.ToUpper().Equals("TOGGLE3"))
            {
                Response.Redirect("~/HR/PayRollSGSDL.aspx?payrollid=" + ViewState["PAYROLL"]);
            }
            if (CommandName.ToUpper().Equals("TOGGLE4"))
            {
                Response.Redirect("~/HR/PayRollSGETF.aspx?payrollid=" + ViewState["PAYROLL"]);
            }
            if (CommandName.ToUpper().Equals("TOGGLE5"))
            {
                Response.Redirect("~/HR/PayRollSGFWL.aspx?payrollid=" + ViewState["PAYROLL"]);
            }
            if (CommandName.ToUpper().Equals("TOGGLE6"))
            {
                Response.Redirect("~/HR/PayRollSGEmployeeConfig.aspx?payrollid=" + ViewState["PAYROLL"]);
            }
            //if (CommandName.ToUpper().Equals("TOGGLE7"))
            //{
            //    Response.Redirect("~/HR/PayRollSGEmployerConfig.aspx?payrollid=" + ViewState["PAYROLL"]);
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvemployeeconfig_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvemployeeconfig.CurrentPageIndex + 1;


        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PhoenixPayRollSingapore.EmployeecongigSearch(General.GetNullableString(ViewState["EMPLOYEECODE"].ToString()),(int)ViewState["PAGENUMBER"], gvemployeeconfig.PageSize, ref iRowCount, ref iTotalPageCount);
        gvemployeeconfig.DataSource = dt;
        gvemployeeconfig.VirtualItemCount = iRowCount;
    }

    protected void gvemployeeconfig_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem && e.Item.IsInEditMode == false)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton editBtn = (LinkButton)e.Item.FindControl("cmdEdit");
            if (editBtn != null)
            {
                
                if (string.IsNullOrWhiteSpace(drv["FLDEMPLOYEEID"].ToString()) == false)
                {

                    editBtn.Attributes.Add("onclick", "javascript: parent.openNewWindow('Filters', 'Payroll Employee Config Edit', 'HR/PayRollSGEmployeeConfigEdit.aspx?employeeid=" + drv["FLDEMPLOYEEID"].ToString() + "', 'false', '720px', '400px'); return false");
                }
            }
        }
    }

    protected void gvemployeeconfig_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "PAGE")
        {
            ViewState["PAGENUMBER"] = null;

        }

        if (e.CommandName == RadGrid.FilterCommandName)
        {
           ViewState["EMPLOYEECODE"] = gvemployeeconfig.MasterTableView.GetColumn("FLDEMPLOYEE").CurrentFilterValue;


            gvemployeeconfig.Rebind();

        }
    }
}