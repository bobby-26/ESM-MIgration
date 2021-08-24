using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class VesselAccounts_VesselAccountsPettyExpensesSummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Summary", "GENERAL");
            toolbar.AddButton("Receipts", "RECEIPTS");
            toolbar.AddButton("Bond", "BONDEDSUMMARY");
            toolbar.AddButton("Provision", "PROVISION");
            toolbar.AddButton("Cash Advance", "CASHADVANCE");
            toolbar.AddButton("Petty Cash", "PETTY");
            if (Filter.CurrentMenuCodeSelection == "VAC-PBL-PTC")
                toolbar.AddButton("List", "BACK");
            MenuPettyCash.AccessRights = this.ViewState;
            MenuPettyCash.MenuList = toolbar.Show();
            MenuPettyCash.SelectedMenuIndex = 5;
            if (!IsPostBack)
            {
                ViewState["vesselid"] = Filter.CurrentMenuCodeSelection == "VAC-PBL-PTC" ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : Request.QueryString["Vesselid"];
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                txtlog.Text = "Log (" + Request.QueryString["fromdate"].ToString() + " - " + Request.QueryString["todate"].ToString() + ")";
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddFontAwesomeButton("../VesselAccounts/VesselAccountsPettyExpensesSummary.aspx?fromdate=" + Request.QueryString["fromdate"].ToString() + "&todate=" + Request.QueryString["todate"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvPettyCash')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuCTM.MenuList = toolbarmain.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPettyCash_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            ViewState["PAGENUMBER"] = 1;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string[] alColumns = { "FLDSEAPORTNAME", "FLDDATE", "FLDPURPOSE", "FLDPAYMENTRECEIPT", "FLDAMOUNT" };
            string[] alCaptions = { "Port", "Expenses On", "Purpose", "Type", "Amount" };

            DataSet ds = PhoenixVesselAccountsCTM.SearchCaptainPettyCash(int.Parse(ViewState["vesselid"].ToString())
                                                       , General.GetNullableDateTime(Request.QueryString["fromdate"].ToString())
                                                       , General.GetNullableDateTime(Request.QueryString["todate"].ToString())
                                                       , sortexpression, sortdirection,
                                                      1, 1000,
                                                       ref iRowCount,
                                                       ref iTotalPageCount);
            General.SetPrintOptions("gvPettyCash", "Petty Expenses", alCaptions, alColumns, ds);


            gvPettyCash.DataSource = ds;
            gvPettyCash.VirtualItemCount = ds.Tables[0].Rows.Count;
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
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                ViewState["PAGENUMBER"] = 1;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                string[] alColumns = { "FLDSEAPORTNAME", "FLDDATE", "FLDPURPOSE", "FLDPAYMENTRECEIPT", "FLDAMOUNT" };
                string[] alCaptions = { "Port", "Expenses On", "Purpose", "Type", "Amount" };

                DataSet ds = PhoenixVesselAccountsCTM.SearchCaptainPettyCash(int.Parse(ViewState["vesselid"].ToString())
                                                           , General.GetNullableDateTime(Request.QueryString["fromdate"].ToString())
                                                           , General.GetNullableDateTime(Request.QueryString["todate"].ToString())
                                                           , sortexpression, sortdirection,
                                                           1, 1000,
                                                           ref iRowCount,
                                                           ref iTotalPageCount);

                General.ShowExcel("Petty Expenses", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}