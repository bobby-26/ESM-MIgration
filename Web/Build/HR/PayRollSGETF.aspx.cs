using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class HR_PayRollSGETF : PhoenixBasePage
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
       // menu.AddButton("Employer", "Toggle7", ToolBarDirection.Left);
        gvmenu.MenuList = menu.Show();
        gvmenu.SelectedMenuIndex = 3;

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["ETFID"] = string.Empty;
            gvSGETF.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvETFRate.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["PAYROLL"] = Request.QueryString["payrollid"];
            LoadConfig();
        }
        int status = 0;
        PhoenixPayRollSingapore.PayrollStatus(General.GetNullableInteger(ViewState["PAYROLL"].ToString()), ref status);
        ViewState["STATUS"] = status;
        ShowToolBar(status);
        Ratemenu(status);
    }
    public void ShowToolBar(int status)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddFontAwesomeButton("../HR/PayRollSGETF.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvSGETF')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        if (status == 1)
        {
            toolbarmain.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/HR/PayRollSGETFAdd.aspx?payrollid=" + ViewState["PAYROLL"] + "','false','400px','270px')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        }
        toolbarmain.AddFontAwesomeButton("../HR/PayRollSGETF.aspx", "Search", "<i class=\"fa fa-search\"></i>", "SEARCH");
        gvTabStrip.MenuList = toolbarmain.Show();
    }
    public void Ratemenu(int? status)
    {
        PhoenixToolbar t = new PhoenixToolbar();
        t.AddFontAwesomeButton("../HR/PayRollSGETF.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        t.AddFontAwesomeButton("javascript:CallPrint('gvETFRate')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        if (status == 1)
        {
            t.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','HR/PayRollSGETFRateAdd.aspx?etfid=" + ViewState["ETFID"] + "&payrollid="+ ViewState["PAYROLL"] + "','false','420px','280px')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        }
        
        tabETFRates.AccessRights = this.ViewState;
        tabETFRates.MenuList = t.Show();
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
                int iRowCount = 0;
                int iTotalPageCount = 0;

                DataTable dt = PhoenixPayRollSingapore.PayrollSGETFSearch((int)ViewState["PAGENUMBER"], gvSGETF.PageSize, ref iRowCount, ref iTotalPageCount);
               
                string[] alColumns = { "FLDETHNICFUNDSHORTCODE", "FLDETHNICFUNDNAME", "FLDETHNICFUNDDESCRIPTION", "FLDHARDNAME" };
                string[] alCaptions = { "Shortcode", "Name", "Description", "Race" };

                General.ShowExcel( "SHG Funds Configuration",dt, alColumns,alCaptions, null, null);
            }
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                if (!IsValidSearch())
                {
                    ucError.Visible = true;

                }
                Rebind();
                ViewState["PAYROLL"] = radcbconfig.SelectedValue;
                int status = 0;
                PhoenixPayRollSingapore.PayrollStatus(General.GetNullableInteger(ViewState["PAYROLL"].ToString()), ref status);
                ViewState["STATUS"] = status;
                ShowToolBar(status);
                Ratemenu(status);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvSGETF.SelectedIndexes.Clear();
        gvSGETF.EditIndexes.Clear();
        gvSGETF.DataSource = null;
        gvSGETF.Rebind();

        gvETFRate.SelectedIndexes.Clear();
        gvETFRate.EditIndexes.Clear();
        gvETFRate.DataSource = null;
        gvETFRate.Rebind();


    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
    protected void gvSGETF_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSGETF.CurrentPageIndex + 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        
            DataTable dt = PhoenixPayRollSingapore.PayrollSGETFSearch( (int)ViewState["PAGENUMBER"], gvSGETF.PageSize, ref iRowCount, ref iTotalPageCount);
        gvSGETF.DataSource = dt;
        gvSGETF.VirtualItemCount = iRowCount;
        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDETHNICFUNDSHORTCODE", "FLDETHNICFUNDNAME", "FLDETHNICFUNDDESCRIPTION", "FLDHARDNAME" };
        string[] alCaptions = { "Shortcode", "Name", "Description","Race" };

        General.SetPrintOptions("gvSGETF", "SHG Funds Configuration", alCaptions, alColumns, ds);

        if (dt.Rows.Count>0)
        {
            tabETFRates.Visible = true;
            if (General.GetNullableGuid(ViewState["ETFID"].ToString())== null)
            {
                DataRow dr = dt.Rows[0];
                gvSGETF.SelectedIndexes.Clear();
                gvSGETF.SelectedIndexes.Add(0);
                ViewState["ETFID"] = dr["FLDSGPRETHNICFUNDID"].ToString();
               

            }

        }

    }

    protected void gvSGETF_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem && e.Item.IsInEditMode == false)
        {


            LinkButton editBtn = (LinkButton)e.Item.FindControl("cmdEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (editBtn != null)
            {
                
                if (string.IsNullOrWhiteSpace(drv["FLDSGPRETHNICFUNDID"].ToString()) == false)
                {

                    editBtn.Attributes.Add("onclick", "javascript: parent.openNewWindow('Filters', 'SHG Edit', 'HR/PayRollSGETFEdit.aspx?etfid=" + drv["FLDSGPRETHNICFUNDID"].ToString() + "', 'false', '400px', '270px'); return false");
                }
            }
        }
    }
    protected void gvSGETF_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            

            if (e.CommandName.ToUpper().Equals("FUND")|| e.CommandName.ToUpper().Equals("ROWCLICK"))
            {
                ViewState["ETFID"] = ((RadLabel)e.Item.FindControl("lblfundId")).Text;
                e.Item.Selected = true;
                Ratemenu(General.GetNullableInteger(ViewState["STATUS"].ToString()));

                gvETFRate.Rebind();

            }
            if (e.CommandName.ToUpper() == "PAGE")
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
  

    protected void gvETFRate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        Guid? ETFid = General.GetNullableGuid(ViewState["ETFID"].ToString());
        int? payrollid = General.GetNullableInteger(radcbconfig.SelectedValue);

        int iRowCount = 0;
        int iTotalPageCount = 0;
        
     
        DataTable dt = PhoenixPayRollSingapore.PayrollSGETFRateSearch(payrollid, ETFid, gvETFRate.CurrentPageIndex +1, gvSGETF.PageSize, ref iRowCount, ref iTotalPageCount);
        gvETFRate.DataSource = dt;
        gvETFRate.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDMINGROSSWAGE", "FLDMAXGROSSWAGE", "FLDETNICFUNDMONTHLYCONTRIBUTION" };
        string[] alCaptions = { " Minimum Gross Wage(SGD)", "Maximum Gross Wage (SGD)", "Monthly Contribution (SGD)" };

        General.SetPrintOptions("gvETFRate", "SHG Funds Configuration", alCaptions, alColumns, ds);
    }

    protected void gvETFRate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem && e.Item.IsInEditMode == false)
        {


            LinkButton editBtn = (LinkButton)e.Item.FindControl("cmdEdit1");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (editBtn != null)
            {
                if (drv["FLDSTATUS"].ToString() == "0")
                {
                    editBtn.Visible = false;

                }
                if (string.IsNullOrWhiteSpace(drv["FLDSGPRETHNICFUNDRATEID"].ToString()) == false)
                {

                    editBtn.Attributes.Add("onclick", "javascript: parent.openNewWindow('Filters', 'SDL Edit', 'HR/PayRollSGETFRateEdit.aspx?etfrateid=" + drv["FLDSGPRETHNICFUNDRATEID"].ToString() + "', 'false', '400px', '270px'); return false");
                }
            }
        }

    }





    protected void gvETFRate_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "PAGE")
        {
            ViewState["PAGENUMBER"] = null;

        }
    }

    protected void tabETFRates_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                Guid? ETFid = General.GetNullableGuid(ViewState["ETFID"].ToString());
                int? payrollid = General.GetNullableInteger(radcbconfig.SelectedValue);

                int iRowCount = 0;
                int iTotalPageCount = 0;
                DataTable dt  = PhoenixPayRollSingapore.PayrollSGETFRateSearch(payrollid, ETFid, gvETFRate.CurrentPageIndex + 1, gvSGETF.PageSize, ref iRowCount, ref iTotalPageCount);

                string[] alColumns = { "FLDMINGROSSWAGE", "FLDMAXGROSSWAGE", "FLDETNICFUNDMONTHLYCONTRIBUTION" };
                string[] alCaptions = { " Minimum Gross Wage(SGD)", "Maximum Gross Wage (SGD)", "Monthly Contribution (SGD)" };

                General.ShowExcel("SHG Funds Configuration", dt, alColumns, alCaptions, null, null);
            }
            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}