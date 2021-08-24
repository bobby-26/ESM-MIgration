using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;


public partial class AccountsFXContractAssignment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvFXContractAssignment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsFXContractAssignment.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvFXContractAssignment')", "Print Grid", "icon_print.png", "PRINT");

            MenuOrderLineItem.AccessRights = this.ViewState;
          //  MenuOrderLineItem.Title = "FX Contract";
            MenuOrderLineItem.MenuList = toolbargrid.Show();
           // MenuOrderLineItem.SetTrigger(pnlStockItemEntry);
           // Rebind();
           // SetPageNavigator();
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

        ds = PhoenixAccountsRemittance.ListFXContractRemittanceInstructions(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, int.Parse(ViewState["PAGENUMBER"].ToString())
                                            ,gvFXContractAssignment.PageSize
                                            , ref iRowCount, ref iTotalPageCount);

     
            gvFXContractAssignment.DataSource = ds;
        gvFXContractAssignment.VirtualItemCount = iRowCount;
       
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        string[] alColumns = { "FLDBANKACCOUNTNUMBER", "FLDDEBITCURRENCYCODE", "FLDPAYMENTCURRENCYCODE", "FLDTOTALPAYABLEAMOUNT", "FLDCONTRACTTYPENAME",
                                     "FLDFXCONTRACTREF", "FLDFXEXCHANGERATE" };
        string[] alCaptions = { "Bank Account", "Bank Account Currency", "Payment Currency", "Amount", "Contract Applicability", "FX Contract Ref",
                                      "Exchange Rate" };
        General.SetPrintOptions("gvFXContractAssignment", "Remittance Summary Approval", alCaptions, alColumns, ds);
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

    }

    protected void MenuSave_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDBANKACCOUNTNUMBER", "FLDDEBITCURRENCYCODE", "FLDPAYMENTCURRENCYCODE", "FLDTOTALPAYABLEAMOUNT", "FLDUSDEQUVALENT",
                                     "FLDUSDEQUVALENT", "FLDFXEXCHANGERATE" };
        string[] alCaptions = { "Bank Account", "Bank Account Currency", "Payment Currency", "Amount", "Contract Applicability", "FX Contract Ref",
                                      "Exchange Rate" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsRemittance.ListFXContractRemittanceInstructions(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 1
                                            , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                            , ref iRowCount, ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=FXContractAssignment.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>FX Contract Assignment</h3></td>");
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

    //protected void gvFXContractAssignment_PreRender(object sender, EventArgs e)
    //{
    //    GridDecorator.MergeRows(gvFXContractAssignment);
    //}

    public class GridDecorator
    {
        public static void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                string strCurrentBankAccountCurrency = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblPaymentCurrency")).Text;
                string strPreviousBankAccountCurrency = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblPaymentCurrency")).Text;

                string currentBankAccountNumber = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblBankAccount")).Text;
                string previousBankAccountNumber = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblBankAccount")).Text;

                if (currentBankAccountNumber == previousBankAccountNumber)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                           previousRow.Cells[0].RowSpan + 1;
                    previousRow.Cells[0].Visible = false;

                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                           previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;
                }
            }
        }
    }

    protected void Rebind()
    {
        gvFXContractAssignment.SelectedIndexes.Clear();
        gvFXContractAssignment.EditIndexes.Clear();
        gvFXContractAssignment.DataSource = null;
        gvFXContractAssignment.Rebind();
    }

    protected void gvFXContractAssignment_RowDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
        }
    }

  

    protected void gvFXContractAssignment_RowCommand(object sender, GridCommandEventArgs e)
    {
    
        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            if (int.Parse(((UserControlHard)e.Item.FindControl("ddlContractType")).SelectedHard) == 679)
            {
                if (!IsValidExchangeRate(((RadTextBox)e.Item.FindControl("txtFXContractRefEdit")).Text, ((RadTextBox)e.Item.FindControl("txtExchangeRateEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
            }
            PhoenixAccountsRemittance.UpdateRemittanceFXContract(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , int.Parse(((RadLabel)e.Item.FindControl("lblBankaccountid")).Text)
                , int.Parse(((RadLabel)e.Item.FindControl("lblRemittancecurrencyid")).Text)
                , int.Parse(((UserControlHard)e.Item.FindControl("ddlContractType")).SelectedHard)
                , ((RadTextBox)e.Item.FindControl("txtFXContractRefEdit")).Text
                , ((RadTextBox)e.Item.FindControl("txtExchangeRateEdit")).Text);

            PhoenixAccountsAllotmentRemittance.AllotmentRemittanceBatchListUpdate(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            Rebind();


        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        else
        {
            //_gridView.EditIndex = -1;
            Rebind();
        }
    }

    protected void gvFXContractAssignment_RowDeleting(object sender, GridCommandEventArgs de)
    {
        Rebind();
    }

    private bool IsValidExchangeRate(string strRef, string strExchangeRate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (strRef.Trim().ToUpper().Equals(""))
            ucError.ErrorMessage = "FX Contract Ref is required.";

        if (strExchangeRate.Trim().Equals(""))
            ucError.ErrorMessage = "Exchange Rate is required.";
        return (!ucError.IsError);
    }

  
    protected void gvFXContractAssignment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFXContractAssignment.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
