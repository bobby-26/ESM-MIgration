using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsAllotmentRemittanceGeneratePrincipalWise : PhoenixBasePage
{
    decimal totalamount = decimal.Parse("0.0");
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsAllotmentRemittanceGeneratePrincipalWise.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvVoucherDetails')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbargrid.AddImageButton("../Accounts/AccountsInvoiceAllotmentPaymentVoucherFilter.aspx", "Find", "search.png", "FIND");
            //toolbargrid.AddImageButton("../Accounts/AccountsAllotmentRemittanceGeneratePrincipalWise.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //MenuOrderForm.SetTrigger(pnlorderform);
            PhoenixToolbar toolbargridHeader = new PhoenixToolbar();
            toolbargridHeader.AddButton("PMVs ", "PMV");
            toolbargridHeader.AddButton("Vessels", "VESSEL");
            toolbargridHeader.AddButton("Principals", "PRINCIPAL");

            MenuOrderFormHeader.AccessRights = this.ViewState;
            MenuOrderFormHeader.MenuList = toolbargridHeader.Show();
           // MenuOrderFormHeader.SetTrigger(pnlorderform);
            MenuOrderFormHeader.SelectedMenuIndex = 2;
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Generate Remittance", "NEW",ToolBarDirection.Right);

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            //MenuOrderFormMain.SetTrigger(pnlorderform);
            if (!IsPostBack)
            {
                Session["New"] = "N";
                if (Session["CHECKED_ITEMS"] != null)
                    Session.Remove("CHECKED_ITEMS");

                if (ViewState["TOTAL_AMOUNT"] != null)
                    ViewState.Remove("TOTAL_AMOUNT");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGEURL"] = null;
                ViewState["TOTAL_AMOUNT"] = "0.0";
                txtTotalSelectedAmount.Text = "0.0";
                gvVoucherDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVoucherDetails_Sorting(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
        Rebind();
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("NEW"))
            {
                ArrayList SelectedPvs = new ArrayList();
                string selectedOthers = ",";

                if (Session["CHECKED_ITEMS"] != null)
                {
                    SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
                    if (SelectedPvs != null && SelectedPvs.Count > 0)
                    {
                        foreach (string index in SelectedPvs)
                            selectedOthers = selectedOthers + index + ",";
                    }
                }

                if (SelectedPvs.Count > 0)
                {
                    PhoenixAccountsAllotmentRemittance.GenerateRemittancePrincipalWise(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, PhoenixSecurityContext.CurrentSecurityContext.UserCode, selectedOthers.Length > 1 ? selectedOthers : null);
                    Rebind();

                    string Scriptnew = "";
                    Scriptnew += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Scriptnew += "fnReloadList('code1', null, null);";
                    Scriptnew += "</script>" + "\n";

                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Scriptnew);

                    ucStatus.Text = "Remittance Generated";

                }
                else
                {
                    ucError.ErrorMessage = "Please select atleast one principal.";
                    ucError.Visible = true;
                    return;
                }
            }
            //Response.Redirect("../Accounts/AccountsAllotmentRemittanceGeneratePrincipalWise.aspx");

            string Script = "";
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('code1', null, null);";
            Script += "</script>" + "\n";

            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }


        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvVoucherDetails$ctl00$ctl02$ctl01$chkAllRemittance")
        {
            ArrayList SelectedPvs = new ArrayList();
            string amount = "";
            RadCheckBox chkAll = new RadCheckBox();
            foreach (GridHeaderItem headerItem in gvVoucherDetails.MasterTableView.GetItems(GridItemType.Header))
            {
                chkAll = (RadCheckBox)headerItem["Listcheckbox"].FindControl("chkAllRemittance"); // Get the header checkbox
            }
            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
            foreach (GridItem row in gvVoucherDetails.Items)
            {
                string index = ((RadLabel)row.FindControl("lblprincipalid")).Text;
                amount = ((RadLabel)row.FindControl("lblAmount")).Text;
                totalamount = Decimal.Parse(ViewState["TOTAL_AMOUNT"] == null ? "0.0" : ViewState["TOTAL_AMOUNT"].ToString());
                RadCheckBox cbSelected = (RadCheckBox)row.FindControl("chkSelect");
                if (cbSelected != null)
                {
                    if (chkAll.Checked == true)
                    {
                        cbSelected.Checked = true;
                        if (!SelectedPvs.Contains(index))
                        {
                            SelectedPvs.Add(index);
                            totalamount = totalamount + decimal.Parse(amount);
                            ViewState["TOTAL_AMOUNT"] = totalamount.ToString();
                        }
                    }
                    else
                    {
                        cbSelected.Checked = false;
                        if (SelectedPvs.Contains(index))
                        {
                            SelectedPvs.Remove(index);
                            totalamount = totalamount - decimal.Parse(amount);
                            ViewState["TOTAL_AMOUNT"] = totalamount.ToString();
                        }
                    }
                }
            }
            if (SelectedPvs != null && SelectedPvs.Count > 0)
                Session["CHECKED_ITEMS"] = SelectedPvs;
            txtTotalSelectedAmount.Text = String.Format("{0:n2}", decimal.Parse(ViewState["TOTAL_AMOUNT"].ToString()));

        }
    }
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "Principal", "Amount(USD)" };
        string[] alColumns = { "FLDPRINCIPALNAME", "FLDAMOUNT" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (Filter.CurrentInvoiceAllotmentPaymentVoucherSelection != null)
        {
            NameValueCollection nvc = Filter.CurrentInvoiceAllotmentPaymentVoucherSelection;

            ds = PhoenixAccountsAllotmentRemittance.RemittanceGeneratePaymentVoucherPrincipalWiseSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                        , sortexpression, sortdirection
                                                        , (int)ViewState["PAGENUMBER"]
                                                        , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                        , ref iRowCount, ref iTotalPageCount);
        }
        else
        {

            ds = PhoenixAccountsAllotmentRemittance.RemittanceGeneratePaymentVoucherPrincipalWiseSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                          , sortexpression, sortdirection
                                                          , (int)ViewState["PAGENUMBER"]
                                                          , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                          , ref iRowCount, ref iTotalPageCount);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=AllotmentPaymentVoucher.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Allotment Payment Voucher</h3></td>");
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

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentInvoiceAllotmentPaymentVoucherSelection = null;
                Session["CHECKED_ITEMS"] = null;
                ViewState["TOTAL_AMOUNT"] = "0.0";
                txtTotalSelectedAmount.Text = "0.0";
                Rebind();

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
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVoucherDetails.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (Filter.CurrentInvoiceAllotmentPaymentVoucherSelection != null)
        {
            NameValueCollection nvc = Filter.CurrentInvoiceAllotmentPaymentVoucherSelection;
            ds = PhoenixAccountsAllotmentRemittance.RemittanceGeneratePaymentVoucherPrincipalWiseSearch(
                    PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                    , sortexpression, sortdirection
                    , (int)ViewState["PAGENUMBER"]
                    , gvVoucherDetails.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);

        }
        else
        {
            ds = PhoenixAccountsAllotmentRemittance.RemittanceGeneratePaymentVoucherPrincipalWiseSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                       , sortexpression, sortdirection
                                       , (int)ViewState["PAGENUMBER"]
                                       , gvVoucherDetails.PageSize
                                       , ref iRowCount, ref iTotalPageCount);
        }




        string[] alCaptions = { "Principal", "Amount(USD)" };
        string[] alColumns = { "FLDPRINCIPALNAME", "FLDAMOUNT" };

        General.SetPrintOptions("gvVoucherDetails", "Allotment Payment Voucher Principal Wise", alCaptions, alColumns, ds);

            gvVoucherDetails.DataSource = ds;
        gvVoucherDetails.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    //protected void cmdSort_Click(object sender, EventArgs e)
    //{
    //    ImageButton ib = (ImageButton)sender;
    //    try
    //    {
    //        gvVoucherDetails.SelectedIndex = -1;
    //        ViewState["SORTEXPRESSION"] = ib.CommandName;
    //        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvVoucherDetails_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadCheckBox chk = (RadCheckBox)e.Item.FindControl("chkSelect");

            DataRowView dr = (DataRowView)e.Item.DataItem;

            if (chk != null && Session["CHECKED_ITEMS"] != null)
            {
                ArrayList SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
               string  index = dr["FLDPRINCIPALID"].ToString();
                if (SelectedPvs.Contains(index))
                {
                    chk.Checked = true;
                }
            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        try
        {
            ArrayList SelectedPvs = new ArrayList();

            string amount = "";

            foreach (GridItem gvrow in gvVoucherDetails.Items)
            {
                bool result = false;
                string index = ((RadLabel)gvrow.FindControl("lblprincipalid")).Text;
                amount = ((RadLabel)gvrow.FindControl("lblAmount")).Text;
                totalamount = Decimal.Parse(ViewState["TOTAL_AMOUNT"] == null ? "0.0" : ViewState["TOTAL_AMOUNT"].ToString());
                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];

                if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
                {
                    result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
                    if (!SelectedPvs.Contains(index))
                    {
                        totalamount = totalamount + decimal.Parse(amount);
                        ViewState["TOTAL_AMOUNT"] = totalamount.ToString();
                    }
                }
                else
                {
                    if (SelectedPvs.Contains(index))
                    {
                        totalamount = totalamount - decimal.Parse(amount);
                        ViewState["TOTAL_AMOUNT"] = totalamount.ToString();
                    }
                }

                if (result)
                {
                    if (!SelectedPvs.Contains(index))
                        SelectedPvs.Add(index);
                }
                else
                {
                    SelectedPvs.Remove(index);

                }
                txtTotalSelectedAmount.Text = String.Format("{0:n2}", decimal.Parse(ViewState["TOTAL_AMOUNT"].ToString()));
            }
            if (SelectedPvs != null && SelectedPvs.Count > 0)
                Session["CHECKED_ITEMS"] = SelectedPvs;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void GetSelectedPvs()
    {
        if (ViewState["TOTAL_AMOUNT"] != null)
        {
            txtTotalSelectedAmount.Text = String.Format("{0:n2}", decimal.Parse(ViewState["TOTAL_AMOUNT"].ToString()));
        }
        else
        {
            txtTotalSelectedAmount.Text = "0.0";
        }

        //if (Session["CHECKED_ITEMS"] != null)
        //{
        //    ArrayList SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
        //    if (SelectedPvs != null && SelectedPvs.Count > 0)
        //    {
        //        foreach (GridItem row in gvVoucherDetails.Items)
        //        {
        //            string index = ((RadLabel)row.FindControl("lblprincipalid")).Text.ToString();
        //            if (SelectedPvs.Contains(index))
        //            {
        //                RadCheckBox myCheckBox = (RadCheckBox)row.FindControl("chkSelect");
        //                myCheckBox.Checked = true;
        //            }
        //        }
        //    }
        //}
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    protected void MenuOrderFormHeader_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("PMV"))
            {
                Response.Redirect("../Accounts/AccountsAllotmentRemittanceGenerate.aspx");
            }
            if (CommandName.ToUpper().Equals("VESSEL"))
            {
                Response.Redirect("../Accounts/AccountsAllotmentRemittanceGenerateVesselWise.aspx");
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
        gvVoucherDetails.SelectedIndexes.Clear();
        gvVoucherDetails.EditIndexes.Clear();
        gvVoucherDetails.DataSource = null;
        gvVoucherDetails.Rebind();
    }

    protected void gvVoucherDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvVoucherDetails_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvVoucherDetails_PreRender(object sender, EventArgs e)
    {
    }
}
