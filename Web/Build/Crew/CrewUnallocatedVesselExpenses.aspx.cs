using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewUnallocatedVesselExpenses : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewUnallocatedVesselExpenses.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvExpense')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewUnallocatedVesselExpenses.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewUnallocatedVesselExpenses.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuExpense.AccessRights = this.ViewState;
            MenuExpense.MenuList = toolbar.Show();
            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvExpense.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCREWNAME", "FLDFILENO", "FLDEXPENSETYPE", "FLDPONUMBER", "FLDORDERDATE", "FLDOERDEREDUSER", "FLDAMOUNT", "FLDJOINEDVESSELNAME", "FLDTENTATIVEVESSELNAME", "FLDCHARGEDVESSELNAME", "FLDEXPENSESVOUCHER", "FLDCHARGINGVOUCHER", "FLDSTATUSNAME" };
            string[] alCaptions = { "Name", "File No.", "Expense Type", "PO Number", "Order Date", "Ordered By", "Amount", "Joined Vessel", "Tentative Vessel", "Charged Vessel", "Expenses Voucher", "Charging Voucher", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewUnallocatedVesselExpenses.SearchCrewEmployeeUnallocatedVesselExpenses(null
                , General.GetNullableString(txtEmployee.Text)
                , General.GetNullableString(txtFileNo.Text)
                , General.GetNullableDateTime(txtOrderdateFrom.Text)
                , General.GetNullableDateTime(txtOrderDateTo.Text)
                , General.GetNullableInteger(ucExpenseType.SelectedHard)
                , General.GetNullableString(txtPONumber.Text)
                , General.GetNullableInteger(ucTentativeVessel.SelectedVessel)
                , General.GetNullableInteger(ucJoinedVessel.SelectedVessel)
                , General.GetNullableInteger(ucChargedVessel.SelectedVessel)
                , General.GetNullableInteger(ucStatus.SelectedHard)
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , gvExpense.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            gvExpense.DataSource = ds;
            gvExpense.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            General.SetPrintOptions("gvExpense", "Unallocated Vessel Expense", alCaptions, alColumns, ds);
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

            string[] alColumns = { "FLDCREWNAME", "FLDFILENO", "FLDEXPENSETYPE", "FLDPONUMBER", "FLDORDERDATE", "FLDOERDEREDUSER", "FLDAMOUNT", "FLDJOINEDVESSELNAME", "FLDTENTATIVEVESSELNAME", "FLDCHARGEDVESSELNAME", "FLDEXPENSESVOUCHER", "FLDCHARGINGVOUCHER", "FLDSTATUSNAME" };
            string[] alCaptions = { "Name", "File No.", "Expense Type", "PO Number", "Order Date", "Ordered By", "Amount", "Joined Vessel", "Tentative Vessel", "Charged Vessel", "Expense Voucher", "Charging Voucher", "Status" };

            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixCrewUnallocatedVesselExpenses.SearchCrewEmployeeUnallocatedVesselExpenses(null
                , General.GetNullableString(txtEmployee.Text)
                , General.GetNullableString(txtFileNo.Text)
                , General.GetNullableDateTime(txtOrderdateFrom.Text)
                , General.GetNullableDateTime(txtOrderDateTo.Text)
                , General.GetNullableInteger(ucExpenseType.SelectedHard)
                , General.GetNullableString(txtPONumber.Text)
                , General.GetNullableInteger(ucTentativeVessel.SelectedVessel)
                , General.GetNullableInteger(ucJoinedVessel.SelectedVessel)
                , General.GetNullableInteger(ucChargedVessel.SelectedVessel)
                , General.GetNullableInteger(ucStatus.SelectedHard)
                , sortexpression
                , sortdirection
                , int.Parse(ViewState["PAGENUMBER"].ToString())
                , General.ShowRecords(null)
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
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
  


    private bool IsValidExpenses(string payment, string tentativevessel)
    {
        if (General.GetNullableInteger(payment) == null)
        {
            ucError.ErrorMessage = "Payment Mode is required.";
        }

        else if (payment.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 185, "CMP"))
        {
            if (General.GetNullableInteger(tentativevessel) == null)
            {
                ucError.ErrorMessage = "Tentative Vessel is required.";
            }
        }
        return (!ucError.IsError);
    }
    protected void MenuExpense_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvExpense.Rebind();

            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ClearFilter();
                ViewState["PAGENUMBER"] = 1;
                gvExpense.CurrentPageIndex = 0;
                gvExpense.Rebind();

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ClearFilter()
    {
        txtEmployee.Text = "";
        txtFileNo.Text = "";
        txtOrderdateFrom.Text = "";
        txtOrderDateTo.Text = "";
        ucExpenseType.SelectedHard = "";
        ucStatus.SelectedHard = "";
        txtPONumber.Text = "";
        ucJoinedVessel.SelectedVessel = "";
        ucTentativeVessel.SelectedVessel = "";
        ucChargedVessel.SelectedVessel = "";
    }

    protected void gvExpense_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvExpense.CurrentPageIndex + 1;
            BindData();
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
            if (e.CommandName.ToUpper() == "UPDATE")
            {


                string expenseid = ((RadLabel)e.Item.FindControl("lblExpensesidEdit")).Text;
                string paymentmode = ((UserControlHard)e.Item.FindControl("ucPaymentmodeEdit")).SelectedHard.ToString();
                string tentativevessel = ((UserControlVesselCommon)e.Item.FindControl("ucTentativeVesselEdit")).SelectedVessel.ToString();

                if (!IsValidExpenses(paymentmode
                    , tentativevessel
                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixCrewUnallocatedVesselExpenses.UpdateCrewUnallocatedVesselExpensesInformation(General.GetNullableGuid(expenseid)
                        , General.GetNullableInteger(paymentmode)
                        , General.GetNullableInteger(tentativevessel)
                        );
                }

                gvExpense.Rebind();

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

    protected void gvExpense_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                UserControlHard ucpayment = (UserControlHard)e.Item.FindControl("ucPaymentmodeEdit");
                UserControlVesselCommon vessel = (UserControlVesselCommon)e.Item.FindControl("ucTentativeVesselEdit");

                DataRowView drvpayment = (DataRowView)e.Item.DataItem;
                DataRowView drvvessel = (DataRowView)e.Item.DataItem;
                if (ucpayment != null) ucpayment.SelectedHard = drvpayment["FLDPAYMENTMODE"].ToString();
                if (vessel != null)
                {
                    vessel.VesselList = PhoenixRegistersVessel.ListCommonVessels(null, null, null, null, 1, null, null, null, 1, "VSL");
                    vessel.SelectedVessel = drvvessel["FLDTENTATIVEVESSEL"].ToString();
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
