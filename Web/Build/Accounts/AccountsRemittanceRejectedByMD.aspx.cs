using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;


public partial class AccountsRemittanceRejectedByMD : PhoenixBasePage
{
    public static decimal dTotalUSDAmount = 0;
    public static decimal dSumUSDAmount = 0;
    public static string strdTotalUSDAmount = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbarmain.AddButton("Approve", "APPROVED");

            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.Title = "Rejected Remittance";
            MenuLineItem.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                //PhoenixToolbar toolbarmain = new PhoenixToolbar();
                //toolbarmain.AddButton("Approve", "APPROVED");

                //MenuLineItem.AccessRights = this.ViewState;
                //MenuLineItem.MenuList = toolbarmain.Show();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvRemittance.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
          
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsRemittanceRejectedByMD.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvRemittance')", "Print Grid", "icon_print.png", "PRINT");
            MenuOrderLineItem.AccessRights = this.ViewState;
            MenuOrderLineItem.MenuList = toolbargrid.Show();
           // MenuOrderLineItem.SetTrigger(pnlStockItemEntry);
          //  BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsRemittance.ListRemittanceRejectedByMD(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, int.Parse(ViewState["PAGENUMBER"].ToString())
                                            , gvRemittance.PageSize
                                            , ref iRowCount, ref iTotalPageCount);

        //if (ds.Tables[0].Rows.Count > 0)
        //{
            gvRemittance.DataSource = ds;
            gvRemittance.VirtualItemCount=iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;

        //}
        //else
        //{
        //    DataTable dt = ds.Tables[0];
        //    ShowNoRecordsFound(dt, gvRemittance);
        //}

        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        string[] alColumns = { "FLDBANKACCOUNTNUMBER", "FLDSUPPLIERNAME", "FLDPAYMENTCURRENCYCODE", "FLDTOTALPAYABLEAMOUNT", "FLDUSDEQUVALENT" };
        string[] alCaptions = { "Bank Account", "Supplier", "Currency", "Amount", "USD Equivalent" };
        General.SetPrintOptions("gvRemittance", "Remittance Summary Approval", alCaptions, alColumns, ds);
    }

    protected void MenuOrderLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void MenuLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("APPROVED"))
        {
            PhoenixAccountsRemittance.RemittanceInstructionApprovedByMD(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            ucStatus.Text = "Remittance Approved";
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDBANKACCOUNTNUMBER", "FLDSUPPLIERNAME", "FLDPAYMENTCURRENCYCODE", "FLDTOTALPAYABLEAMOUNT", "FLDUSDEQUVALENT" };
        string[] alCaptions = { "Bank Account", "Supplier", "Currency", "Amount", "USD Equivalent" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsRemittance.ListRemittanceRejectedByMD(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 1
                                            , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                            , ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=RemittanceSummaryApproval.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Remittance Summary Approval</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    //protected void gvRemittance_PreRender(object sender, EventArgs e)
    //{
    //    GridDecorator.MergeRows(gvRemittance);
    //}


    protected void gvRemittance_RowCommand(object sender, GridCommandEventArgs e)
    {
     
        if (e.CommandName.ToUpper().Equals("REJECT"))
        {
            PhoenixAccountsRemittance.RemittanceInstructionRejectedByMD(((RadLabel)e.Item.FindControl("lblBankInstructionId")).Text, PhoenixSecurityContext.CurrentSecurityContext.UserCode);
          //  BindData();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        else
        {
          //  BindData();

        }

    }

    protected void gvRemittance_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton cmdReject = (ImageButton)e.Item.FindControl("cmdReject");
            if (cmdReject != null)
            if (!SessionUtil.CanAccess(this.ViewState, cmdReject.CommandName)) cmdReject.Visible = false;

            if (e.Item.DataItem != null)
            {
                ((RadLabel)e.Item.FindControl("lblSumUSDEquivalent")).Text = ((RadLabel)e.Item.FindControl("lblUSDEquivalent")).Text;
                if (((RadLabel)e.Item.FindControl("lblUSDEquivalent")).Text != string.Empty)
                    dSumUSDAmount = dSumUSDAmount + decimal.Parse(((RadLabel)e.Item.FindControl("lblUSDEquivalent")).Text);
            }
            strdTotalUSDAmount = String.Format("{0:n}", dSumUSDAmount);
        }

    }

    public class GridDecorator
    {
        public static void MergeRows(GridView gridView)
        {
            decimal dBankACSumAmount = 0;
            dTotalUSDAmount = 0;
            if (gridView.Rows.Count > 2)
            {
                dBankACSumAmount = decimal.Parse(((Label)gridView.Rows[gridView.Rows.Count - 1].FindControl("lblUSDEquivalent")).Text);
                ((Label)gridView.Rows[gridView.Rows.Count - 1].FindControl("lblSumUSDEquivalent")).Text = String.Format("{0:n}", dBankACSumAmount);
                dTotalUSDAmount = dBankACSumAmount;
            }

            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];
                string currentBankAccountNumber = ((Label)gridView.Rows[rowIndex].FindControl("lblBankAccount")).Text;
                string previousBankAccountNumber = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblBankAccount")).Text;

                if (currentBankAccountNumber == previousBankAccountNumber)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                           previousRow.Cells[0].RowSpan + 1;

                    row.Cells[5].RowSpan = previousRow.Cells[5].RowSpan < 2 ? 2 :
                                           previousRow.Cells[5].RowSpan + 1;

                    dBankACSumAmount = dBankACSumAmount + decimal.Parse(((Label)gridView.Rows[rowIndex].FindControl("lblUSDEquivalent")).Text);
                    previousRow.Cells[0].Visible = false;
                    previousRow.Cells[5].Visible = false;
                }
                else
                {
                    dBankACSumAmount = decimal.Parse(((Label)gridView.Rows[rowIndex].FindControl("lblUSDEquivalent")).Text);
                }

                ((Label)gridView.Rows[rowIndex].FindControl("lblSumUSDEquivalent")).Text = String.Format("{0:n}", dBankACSumAmount);
                dTotalUSDAmount = dTotalUSDAmount + decimal.Parse(((Label)gridView.Rows[rowIndex].FindControl("lblUSDEquivalent")).Text);
            }
        }
    }

  

    protected void gvRemittance_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRemittance.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
