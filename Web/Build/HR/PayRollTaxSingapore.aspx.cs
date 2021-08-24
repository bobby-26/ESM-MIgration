using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PayRoll_PayRollTaxSingapore : PhoenixBasePage
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
        gvmenu.SelectedMenuIndex = 0;
        ShowToolBar();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["PAYROLL"] = string.Empty;
            if (Request.QueryString["payrollid"] != null)
                ViewState["PAYROLL"] = Request.QueryString["payrollid"];
            gvtaxsingapore.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddFontAwesomeButton("../HR/PayRollTaxSingapore.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvtaxsingapore')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        toolbarmain.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/HR/PayRollTaxSingaporeAdd.aspx','false','550px','400px')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        gvTabStrip.MenuList = toolbarmain.Show();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvtaxsingapore.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

     #region Grid Events
    protected void gvtaxsingapore_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvtaxsingapore.CurrentPageIndex + 1;

       
        int iRowCount = 0;
        int iTotalPageCount = 0;
      
            DataTable dt  = PhoenixPayRollSingapore.TaxSingaporeSearch( (int)ViewState["PAGENUMBER"], gvtaxsingapore.PageSize, ref iRowCount, ref iTotalPageCount);
        gvtaxsingapore.DataSource = dt;
        gvtaxsingapore.VirtualItemCount = iRowCount;
        if(dt.Rows.Count>0)
        { 
        DataRow dr = dt.Rows[0];
         if (General.GetNullableInteger(ViewState["PAYROLL"].ToString()) == null)
         {
             gvtaxsingapore.SelectedIndexes.Clear();
             gvtaxsingapore.SelectedIndexes.Add(0);
         ViewState["PAYROLL"] = dr["FLDSGPRITID"].ToString();
         }
        }
        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDDESCRIPTION", "FLDFROMDATE" , "FLDTODATE" , "FLDCURRENCYNAME", "FLDREVISIONREMARKS" , "FLDSTATUS" };
        string[] alCaptions = { "Description", "From", "To", "Currency", "Revision Notes", "Status" };
        General.SetPrintOptions("gvtaxsingapore", "Payroll Configurations", alCaptions, alColumns, ds);
    }

    protected void gvtaxsingapore_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem && e.Item.IsInEditMode == false)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if(General.GetNullableInteger(ViewState["PAYROLL"].ToString()) != null)
            { 
            if (drv["FLDSGPRITID"].ToString() == ViewState["PAYROLL"].ToString())
            {
                    gvtaxsingapore.SelectedIndexes.Clear();
                    e.Item.Selected = true;

            }
            }
            LinkButton editBtn = (LinkButton)e.Item.FindControl("cmdEdit");
            if (editBtn != null)
            {
                if (drv["FLDSTATUS"].ToString() == "Inactive")
                {
                    editBtn.Visible = false;

                }
                if (string.IsNullOrWhiteSpace(drv["FLDSGPRITID"].ToString()) == false)
                {
                  
                    editBtn.Attributes.Add("onclick", "javascript: parent.openNewWindow('Filters', 'Payroll Edit', 'HR/PayRollTaxSingaporeEdit.aspx?payrollid=" + drv["FLDSGPRITID"].ToString() + "', 'false', '550px', '400px'); return false");
                }
            }
        }
    }

   
    #endregion


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

                DataTable dt = PhoenixPayRollSingapore.TaxSingaporeSearch((int)ViewState["PAGENUMBER"], gvtaxsingapore.PageSize, ref iRowCount, ref iTotalPageCount);

                string[] alColumns = { "FLDDESCRIPTION", "FLDFROMDATE", "FLDTODATE", "FLDCURRENCYNAME", "FLDREVISIONREMARKS", "FLDSTATUS" };
                string[] alCaptions = { "Description", "From", "To", "Currency", "Revision Notes", "Status" };
                General.ShowExcel( "Payroll Configurations",dt, alColumns, alCaptions, null, null);
            }
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

    protected void gvtaxsingapore_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {


            if ( e.CommandName.ToUpper().Equals("ROWCLICK"))
            {
                ViewState["PAYROLL"] = ((RadLabel)e.Item.FindControl("radlblpayrollid")).Text;
                e.Item.Selected = true;
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
}