using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class VesselAccountsCashAdvanceSummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Summary", "GENERAL");
            toolbar1.AddButton("Receipts", "RECEIPTS");
            toolbar1.AddButton("Bond", "BONDEDSUMMARY");
            toolbar1.AddButton("Provision", "PROVISION");
            toolbar1.AddButton("Cash Advance", "CASHADVANCE");
            toolbar1.AddButton("Petty Cash", "PETTY");
            if (Filter.CurrentMenuCodeSelection == "VAC-PBL-PTC")
                toolbar1.AddButton("List", "BACK");
            MenuPettyCash.AccessRights = this.ViewState;
            MenuPettyCash.MenuList = toolbar1.Show();
            MenuPettyCash.SelectedMenuIndex = 4;
            if (!IsPostBack)
            {
                ViewState["vesselid"] = Filter.CurrentMenuCodeSelection == "VAC-PBL-PTC" ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : Request.QueryString["Vesselid"];
                txtlog.Text = "Log (" + Request.QueryString["fromdate"].ToString() + " - " + Request.QueryString["todate"].ToString() + ")";
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsCashAdvanceSummary.aspx?fromdate=" + Request.QueryString["fromdate"].ToString() + "&todate=" + Request.QueryString["todate"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBondReq')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuCTM.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBondReq_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void MenuPettyCash_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsPettyCashGeneral.aspx";
            }
            else if (CommandName.ToUpper().Equals("RECEIPTS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCashReceiptsSummary.aspx";
            }
            else if (CommandName.ToUpper().Equals("BONDEDSUMMARY"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsBondedSummary.aspx";
            }
            else if (CommandName.ToUpper().Equals("PROVISION"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsProvisionSummary.aspx";
            }
            else if (CommandName.ToUpper().Equals("CASHADVANCE"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsCashAdvanceSummary.aspx";
            }
            else if (CommandName.ToUpper().Equals("PETTY"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsPettyExpensesSummary.aspx";
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsPettyCash.aspx", false);
            }
            else
            {
                if (string.IsNullOrEmpty(Request.QueryString["type"]))
                {
                    Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"] + "?fromdate=" + Request.QueryString["fromdate"].ToString() + "&todate=" + Request.QueryString["todate"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString(), false);
                }
                else Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"] + "?fromdate=" + Request.QueryString["fromdate"].ToString() + "&todate=" + Request.QueryString["todate"].ToString() + "&type=n" + "&vesselid=" + ViewState["vesselid"].ToString(), false);
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

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDDATE", "FLDBASECURRENCYCODE", "FLDBASEAMOUNT", "FLDEXCHANGERATE", "FLDRPTCURRENCYCODE", "FLDAMOUNT" };
        string[] alCaptions = { "Name", "Date", "Base Currency", "Base Amount", "Exchange Rate", "Reported Currency", "Reported Amount" };
        try
        {
            DataTable dt = PhoenixVesselAccountsCTM.CaptainCashAdvanceSummary(int.Parse(ViewState["vesselid"].ToString())
                                                                       , General.GetNullableDateTime(Request.QueryString["fromdate"].ToString())
                                                                       , General.GetNullableDateTime(Request.QueryString["todate"].ToString())
                                                                      );

            gvBondReq.DataSource = dt;
            gvBondReq.VirtualItemCount = dt.Rows.Count;
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvBondReq", "Cash Advance", alCaptions, alColumns, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCTM_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDEMPLOYEENAME", "FLDDATE", "FLDBASECURRENCYCODE", "FLDBASEAMOUNT", "FLDEXCHANGERATE", "FLDRPTCURRENCYCODE", "FLDAMOUNT" };
                string[] alCaptions = { "Name", "Date", "Base Currency", "Base Amount", "Exchange Rate", "Reported Currency", "Reported Amount" };


                DataTable dt = PhoenixVesselAccountsCTM.CaptainCashAdvanceSummary(int.Parse(ViewState["vesselid"].ToString())
                                                                      , General.GetNullableDateTime(Request.QueryString["fromdate"].ToString())
                                                                       , General.GetNullableDateTime(Request.QueryString["todate"].ToString())
                                                                       );
                General.ShowExcel("Cash Advance", dt, alColumns, alCaptions, null, null);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
 
   
}