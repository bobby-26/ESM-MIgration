using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewUnallocatedVesselExpensesEmployee : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar2 = new PhoenixToolbar();
            CrewExpenseTab.AccessRights = this.ViewState;
            CrewExpenseTab.MenuList = toolbar2.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewUnallocatedVesselExpensesEmployee.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvExpense')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuExpense.AccessRights = this.ViewState;
            MenuExpense.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["EMPLOYEEID"] = "";
                if (Request.QueryString["empid"] != null)
                    ViewState["EMPLOYEEID"] = Request.QueryString["empid"].ToString();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                SetEmployeePrimaryDetails();

                gvExpense.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuExpense_TabStripCommand(object sender, EventArgs e)
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
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDFILENO", "FLDCREWNAME", "FLDEXPENSETYPE", "FLDPONUMBER", "FLDORDERDATE", "FLDOERDEREDUSER", "FLDAMOUNT", "FLDPAYMENTMODENAME", "FLDJOINEDVESSELNAME", "FLDTENTATIVEVESSELNAME", "FLDCHARGEDVESSELNAME", "FLDEXPENSESVOUCHER", "FLDCHARGINGVOUCHER", "FLDSTATUSNAME" };
            string[] alCaptions = { "File No.", "Name", "Expense Type", "PO No.", "Order Date", "Ordered By", "Amount(USD)", "Payment Mode", "Joined Vessel", "Tentative Vessel", "Charged Vessel", "Expenses Voucher", "Charging Voucher", "Status" };

            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixCrewUnallocatedVesselExpenses.SearchCrewEmployeeUnallocatedVesselExpenses(General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString())
                , null
                , null
                , null
                , null
                , null
                , null
                , null
                , null
                , null
                , null
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , iRowCount
                , ref iRowCount
                , ref iTotalPageCount);

            if (ds.Tables.Count > 0)
                General.ShowExcel("Unallocated Vessel Expenses", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvExpense_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvExpense.CurrentPageIndex + 1;

        BindData();
    }

    public void BindData()
    {

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDFILENO", "FLDCREWNAME", "FLDEXPENSETYPE", "FLDPONUMBER", "FLDORDERDATE", "FLDOERDEREDUSER", "FLDAMOUNT", "FLDPAYMENTMODENAME", "FLDJOINEDVESSELNAME", "FLDTENTATIVEVESSELNAME", "FLDCHARGEDVESSELNAME", "FLDEXPENSESVOUCHER", "FLDCHARGINGVOUCHER", "FLDSTATUSNAME" };
            string[] alCaptions = { "File No.", "Name", "Expense Type", "PO No.", "Order Date", "Ordered By", "Amount(USD)", "Payment Mode", "Joined Vessel", "Tentative Vessel", "Charged Vessel", "Expenses Voucher", "Charging Voucher", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewUnallocatedVesselExpenses.SearchCrewEmployeeUnallocatedVesselExpenses(General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString())
                                                                                        , null
                                                                                        , null
                                                                                        , null
                                                                                        , null
                                                                                        , null
                                                                                        , null
                                                                                        , null
                                                                                        , null
                                                                                        , null
                                                                                        , null
                                                                                        , sortexpression
                                                                                        , sortdirection
                                                                                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                        , gvExpense.PageSize
                                                                                        , ref iRowCount
                                                                                        , ref iTotalPageCount);

            gvExpense.DataSource = ds;
            gvExpense.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            General.SetPrintOptions("gvExpense", "Unallocated Vessel Expenses", alCaptions, alColumns, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvExpense_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvExpense.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}
