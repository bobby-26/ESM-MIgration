using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class AccountsAdditionalCommitments : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsAdditionalCommitments.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvAdvancePayment')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsAdditionalCommitmentsFilter.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsAdditionalCommitments.aspx", "Clear Filter", "clear-filter.png", "RESET");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //MenuOrderForm.SetTrigger(pnlOrderForm);

            if (!IsPostBack)
            {
                Session["New"] = "N";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ADVANCEPAYMENTID"] = null;
                ViewState["PAGEURL"] = null;

                if (Request.QueryString["ADDITIONALCOMMITMENTID"] != null)
                {
                    ViewState["ADDITIONALCOMMITMENTID"] = Request.QueryString["ADVANCEPAYMENTID"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAdditionalCommitmentsGeneral.aspx?ADDITIONALCOMMITMENTID=" + ViewState["ADDITIONALCOMMITMENTID"];
                }

                gvAdvancePayment.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

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
            DataSet ds = new DataSet();
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVESSELNAME", "FLDVESSELACCOUNTNAME", "FLDPONUMBER", "FLDSUPPLIERNAME", "FLDAMOUNT", "FLDBUDGETCODE", "FLDCOMMITEMENTDATE", "FLDREVERSEDDATE" , "FLDPROJECTCODE" };
            string[] alCaptions = { "Vessel", "Vessel Account", "PO Number", "Supplier Name", "Amount(USD)", "Budget Code", "Committed Date", "Reversed Date" , "Project Code" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            if (Filter.AdditionalCommitments != null)
            {

                NameValueCollection nvc = Filter.AdditionalCommitments;

                ds = PhoenixAccountsAdditionalCommitments.AccountsAdditionalCommitmentsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode

                                                                                         , General.GetNullableDateTime(nvc != null ? nvc["txtPOFrom"] : string.Empty)
                                                                                         , General.GetNullableDateTime(nvc != null ? nvc["txtPOTo"] : string.Empty)
                                                                                         , General.GetNullableDateTime(nvc != null ? nvc["txtRvFrom"] : string.Empty)
                                                                                         , General.GetNullableDateTime(nvc != null ? nvc["txtRvTo"] : string.Empty)
                                                                                          , General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : string.Empty)
                                                                                          , General.GetNullableString(nvc != null ? nvc["txtVendorId"] : string.Empty)
                                                                                          , General.GetNullableInteger(null)
                                                                                          , General.GetNullableString(nvc != null ? nvc["txtPONumber"] : string.Empty)
                                                                                          , (int)ViewState["PAGENUMBER"]
                                                                                            , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                                                            , ref iRowCount
                                                                                            , ref iTotalPageCount);
            }
            else
            {
                ds = PhoenixAccountsAdditionalCommitments.AccountsAdditionalCommitmentsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode

                                                                                    , null
                                                                                    , null
                                                                                    , null
                                                                                    , null
                                                                                     , null
                                                                                     , null
                                                                                     , null
                                                                                     , null
                                                                                     , (int)ViewState["PAGENUMBER"]
                                                                                       , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                                                       , ref iRowCount
                                                                                       , ref iTotalPageCount);

            }


            Response.AddHeader("Content-Disposition", "attachment; filename=Additional Commitments.xls");

            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Additional Commitments</h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("RESET"))
            {
                ClearFilter();
                ViewState["PAGENUMBER"] = 1;
                gvAdvancePayment.Rebind();
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

        string[] alColumns = { "FLDVESSELNAME", "FLDVESSELACCOUNTNAME", "FLDPONUMBER", "FLDSUPPLIERNAME", "FLDAMOUNT", "FLDBUDGETCODE", "FLDCOMMITEMENTDATE", "FLDREVERSEDDATE" , "FLDPROJECTCODE" };
        string[] alCaptions = { "Vessel", "Vessel Account", "PO Number", "Supplier Name", "Amount(USD)", "Budget Code", "Committed Date", "Reversed Date" , "Project Code" };
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        NameValueCollection nvc = Filter.AdditionalCommitments;
       // txtVendorId
        ds = PhoenixAccountsAdditionalCommitments.AccountsAdditionalCommitmentsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            
                                                                                    , General.GetNullableDateTime(nvc != null ? nvc["txtPOFrom"] : string.Empty)
                                                                                    , General.GetNullableDateTime(nvc != null ? nvc["txtPOTo"] : string.Empty)
                                                                                    , General.GetNullableDateTime(nvc != null ? nvc["txtRvFrom"] : string.Empty)
                                                                                    , General.GetNullableDateTime(nvc != null ? nvc["txtRvTo"] : string.Empty)
                                                                                     , General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : string.Empty)
                                                                                     , General.GetNullableString(nvc != null ? nvc["txtVendorId"] : string.Empty)
                                                                                     , General.GetNullableInteger(null)
                                                                                     , General.GetNullableString(nvc != null ? nvc["txtPONumber"] : string.Empty)
                                                                                     , gvAdvancePayment.CurrentPageIndex + 1
                                                                                     , gvAdvancePayment.PageSize
                                                                                     , ref iRowCount
                                                                                     , ref iTotalPageCount);

        General.SetPrintOptions("gvAdvancePayment", "Advance Payment", alCaptions, alColumns, ds);

        gvAdvancePayment.DataSource = ds;
        gvAdvancePayment.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        if (ds.Tables[0].Rows.Count > 0)
        {          
           

            if (ViewState["ADDITIONALCOMMITMENTID"] == null)
            {
                ViewState["ADDITIONALCOMMITMENTID"] = ds.Tables[0].Rows[0]["FLDADDITIONALCOMMITMENTID"].ToString();
                //gvAdvancePayment.SelectedIndex = 0;
            }

            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAdditionalCommitmentsGeneral.aspx?ADDITIONALCOMMITMENTID=" + ViewState["ADDITIONALCOMMITMENTID"];
            }
            SetRowSelection();
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAdditionalCommitmentsGeneral.aspx";
            }
            DataTable dt = ds.Tables[0];
            gvAdvancePayment.DataSource = ds;
        }

    }
    private void ClearFilter()
    {
        Filter.AdditionalCommitments = null;
        BindData();
        
    }

    private void SetRowSelection()
    {

        gvAdvancePayment.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvAdvancePayment.Items)
        {

            if (item.GetDataKeyValue("FLDADDITIONALCOMMITMENTID").ToString().Equals(ViewState["ADDITIONALCOMMITMENTID"].ToString()))
            {
                gvAdvancePayment.SelectedIndexes.Add(item.ItemIndex);
                //PhoenixAccountsVoucher.VoucherNumber = ((RadLabel)gvDeposit.Items[item.ItemIndex].FindControl("lblDepositid")).Text;
                //ViewState["DOCUMENTID"] = item.GetDataKeyValue("FLDDOCUMENTID").ToString();
            }
        }
    } 

   
  
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
       
    }


   
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //BindData();
        gvAdvancePayment.Rebind();

        if (Session["New"].ToString() == "Y")
        {            
            Session["New"] = "N";
            BindPageURL(0);
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)gvAdvancePayment.Items[rowindex];
            ViewState["ADDITIONALCOMMITMENTID"] = item.GetDataKeyValue("FLDADDITIONALCOMMITMENTID"); 
            //ViewState["ADDITIONALCOMMITMENTID"] = ((RadLabel)gvAdvancePayment.Items[rowindex].FindControl("lblPoNumber")).Text;
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAdditionalCommitmentsGeneral.aspx?ADDITIONALCOMMITMENTID=" + ViewState["ADDITIONALCOMMITMENTID"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAdvancePayment_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvAdvancePayment.SelectedIndexes.Add(e.NewSelectedIndex);
        BindPageURL(e.NewSelectedIndex);
    }

    protected void gvAdvancePayment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    protected void gvAdvancePayment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        RadGrid _gridView = (RadGrid)sender;

        int nCurrentRow = e.Item.ItemIndex;

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            BindPageURL(nCurrentRow);
            SetRowSelection();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    
    protected void gvAdvancePayment_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvAdvancePayment_EditCommand(object sender, GridCommandEventArgs e)
    {
        BindData();
    }
}
