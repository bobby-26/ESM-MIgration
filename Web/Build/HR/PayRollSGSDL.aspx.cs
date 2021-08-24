using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class HR_PayRollSGSDL : System.Web.UI.Page
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
        gvmenu.SelectedMenuIndex = 2;

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            gvSGSDL.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["PAYROLL"] = Request.QueryString["payrollid"];
            LoadConfig();
        }
        int status = 0;
        PhoenixPayRollSingapore.PayrollStatus(General.GetNullableInteger(ViewState["PAYROLL"].ToString()), ref status);
        ShowToolBar(status);

    }
    public void ShowToolBar(int status)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddFontAwesomeButton("../HR/PayRollSGSDL.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvSGSDL')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        if (status == 1)
        {
            toolbarmain.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/HR/PayRollSGSDLAdd.aspx?payrollid=" + ViewState["PAYROLL"] + "','false','480px','250px')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        }
        toolbarmain.AddFontAwesomeButton("../HR/PayRollSGSDL.aspx", "Search", "<i class=\"fa fa-search\"></i>", "SEARCH");
        gvTabStrip.MenuList = toolbarmain.Show();
    }

    public void LoadConfig()
    {
        radcbconfig.DataSource = PhoenixPayRollSingapore.PayrollConfigList();
        radcbconfig.DataTextField = "FLDDESCRIPTION";
        radcbconfig.DataValueField = "FLDSGPRITID";
        radcbconfig.DataBind();
        if (General.GetNullableInteger(ViewState["PAYROLL"].ToString()) != null)
        {
            radcbconfig.SelectedValue = ViewState["PAYROLL"].ToString();
        }
        else
        {
            radcbconfig.SelectedIndex = 0;

            ViewState["PAYROLL"] = radcbconfig.SelectedValue;
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
                int? payrollid = General.GetNullableInteger(radcbconfig.SelectedValue);
                int iRowCount = 0;
                int iTotalPageCount = 0;
                DataTable dt = PhoenixPayRollSingapore.PayrollSGSDLSearch(payrollid, (int)ViewState["PAGENUMBER"], gvSGSDL.PageSize, ref iRowCount, ref iTotalPageCount);
              

                string[] alColumns = { "FLDEMPLOYERSDLCONTPERCENTAGE", "FLDMINEMPLOYERSDLCONT", "FLDMAXEMPLOYERSDLCONT" };
                string[] alCaptions = { " SDL payable by Employer in Total Wage (%)", "Minimum SDL Payable (SGD)", "Maximum SDL Payable (SGD)" };

                General.ShowExcel("SDL Configuration", dt, alColumns, alCaptions, null, null);
            }
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                if (!IsValidSearch())
                {
                    ucError.Visible = true;

                }
                gvSGSDL.Rebind();
                ViewState["PAYROLL"] = radcbconfig.SelectedValue;
                int status = 0;
                PhoenixPayRollSingapore.PayrollStatus(General.GetNullableInteger(ViewState["PAYROLL"].ToString()), ref status);
                ShowToolBar(status);
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
            gvSGSDL.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvSGSDL_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSGSDL.CurrentPageIndex + 1;

        int? payrollid = General.GetNullableInteger(radcbconfig.SelectedValue);
        int iRowCount = 0;
        int iTotalPageCount = 0;
       
            DataTable dt = PhoenixPayRollSingapore.PayrollSGSDLSearch(payrollid, (int)ViewState["PAGENUMBER"], gvSGSDL.PageSize, ref iRowCount, ref iTotalPageCount);
        gvSGSDL.DataSource = dt;
        gvSGSDL.VirtualItemCount = iRowCount;

        string[] alColumns = { "FLDEMPLOYERSDLCONTPERCENTAGE", "FLDMINEMPLOYERSDLCONT", "FLDMAXEMPLOYERSDLCONT" };
        string[] alCaptions = { " SDL payable by Employer in Total Wage (%)", "Minimum SDL Payable (SGD)", "Maximum SDL Payable (SGD)"};

        DataSet ds = dt.DataSet;
        General.SetPrintOptions("gvSGSDL", "SDL Configuration", alCaptions, alColumns, ds);

    }

    protected void gvSGSDL_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem && e.Item.IsInEditMode == false)
        {


            LinkButton editBtn = (LinkButton)e.Item.FindControl("cmdEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (editBtn != null)
            {
                if (drv["FLDSTATUS"].ToString() == "0")
                {
                    editBtn.Visible = false;

                }
                if (string.IsNullOrWhiteSpace(drv["FLDSGPRSDLID"].ToString()) == false)
                {

                    editBtn.Attributes.Add("onclick", "javascript: parent.openNewWindow('Filters', 'SDL Edit', 'HR/PayRollSGSDLEdit.aspx?sdlid=" + drv["FLDSGPRSDLID"].ToString() + "', 'false', '480px', '250px'); return false");
                }
            }
        }
    }
    public bool IsValidSearch()
    {


        ucError.HeaderMessage = "Please provide following information.";
        if (General.GetNullableString(radcbconfig.SelectedValue) == null)
        {
            ucError.ErrorMessage = "Please select the Payroll configuration.";

        }
        return (!ucError.IsError);
    }

    protected void gvSGSDL_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "PAGE")
        {
            ViewState["PAGENUMBER"] = null;

        }
    }
}