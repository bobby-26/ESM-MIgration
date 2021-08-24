using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsAllotmentRemittanceBatchInstructionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsAllotmentRemittanceBatchInstructionList.aspx?" + Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
           // MenuOrderForm.SetTrigger(pnlRemittance);
            if (!IsPostBack)
            {
                ViewState["remittanceid"] = null;
                Session["New"] = "N";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["TAXANDCHARGESYN"] = 0;
                ViewState["VOUCHERID"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["REMITTENCEID"] = null;
                ViewState["Batchid"] = null;
                if (Request.QueryString["Batchid"] != null && Request.QueryString["Batchid"].Length >= 36)
                {
                    ViewState["Batchid"] = Request.QueryString["Batchid"].ToString();
                }
                BatchEdit();
                gvRemittence.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            toolbargrid = new PhoenixToolbar();
            toolbargrid.AddButton("List", "BATCHLIST");
            toolbargrid.AddButton("Line Item", "LINEITEM");
            toolbargrid.AddButton("History", "HISTORY");
            if (ViewState["TAXANDCHARGESYN"].ToString() == "1")
                toolbargrid.AddButton("Tax and Charges", "TAXANDCHARGES");
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbargrid.Show();
            MenuOrderFormMain.SelectedMenuIndex = 1;
            toolbargrid = new PhoenixToolbar();
            toolbargrid.AddButton("Post Voucher", "POST", ToolBarDirection.Right);
            toolbargrid.AddButton("Save", "SAVE",ToolBarDirection.Right);       
            Menusub.AccessRights = this.ViewState;
            Menusub.MenuList = toolbargrid.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "S.No.", "Bank Voucher No.", "Remittance No.", "Payment Voucher No.", "File No.", "Amount", "Benificiary", "Beneficiary Bank SWIFT Code", "Beneficiary Bank", "Beneficiary Bank Acct.No." };
        string[] alColumns = { "FLDROWNUM", "FLDVOUCHERNUMBER", "FLDREMITTANCENUMBERLIST", "FLDPAYMENTVOUCHERLIST", "FLDEMPLOYEECODE", "FLDREMITTANCEAMOUNT", "FLDBENEFICIARYNAME", "FLDBANKSWIFTCODE", "FLDBANKNAME", "FLDACCOUNTNUMBER" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsAllotmentRemittance.BatchRemittanceInstructionList(ViewState["Batchid"].ToString(), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
                                                            , iRowCount
                                                            , ref iRowCount
                                                            , ref iTotalPageCount
                                                            );
        Response.AddHeader("Content-Disposition", "attachment; filename=AccountRemittance.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Remittance</h3></td>");
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
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("BATCHLIST"))
            {
                Response.Redirect("AccountsAllotmentRemittanceBatchList.aspx", false);
            }
            if (CommandName.ToUpper().Equals("HISTORY"))
            {
                Response.Redirect("AccountsAllotmentRemittanceBatchDownLoadHistory.aspx?type=" + ViewState["TAXANDCHARGESYN"].ToString() + "&Batchid=" + ViewState["Batchid"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("LINEITEM"))
            {
                Response.Redirect("AccountsAllotmentRemittanceBatchInstructionList.aspx?Batchid=" + ViewState["Batchid"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("TAXANDCHARGES"))
            {
                Response.Redirect("AccountsAllotmentRemittanceBatchTaxandChargesList.aspx?type=" + ViewState["TAXANDCHARGESYN"].ToString() + "&Batchid=" + ViewState["Batchid"].ToString(), false);
            }
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
                BindData();
            }
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
    protected void Menusub_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("POST"))
            {

                if (ucError.IsError)
                {
                    ucError.Visible = true;
                    return;
                }
                try
                {
                    PhoenixAccountsAllotmentRemittance.AllotmentBatchPostValidation(new Guid(ViewState["Batchid"].ToString()));
                    PhoenixAccountsAllotmentRemittance.RemittancePostAsync(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                           , ViewState["Batchid"].ToString()
                                                                           , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (General.GetNullableDateTime(txtpaydate.Text) == null)
                    ucError.ErrorMessage = "Payment date is required.";

                if (ucError.IsError)
                {
                    ucError.Visible = true;
                    return;
                }
                try
                {

                    PhoenixAccountsAllotmentRemittance.RemittanceInstructionBatchPaymentdateUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(ViewState["Batchid"].ToString())
                                                                                    , General.GetNullableDateTime(txtpaydate.Text)
                                                                                    , General.GetNullableInteger(ChkTaxchargesYN.Checked == true ? "1" : "0"));
                    ucStatus.Text = "Batch Updated.";
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
            }
            gvRemittence.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BatchEdit()
    {
        DataSet ds = new DataSet();
        ds = PhoenixAccountsAllotmentRemittance.RemittanceBatchEdit(new Guid(ViewState["Batchid"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["VOUCHERID"] = ds.Tables[0].Rows[0]["FLDVOUCHERID"].ToString();
            ViewState["TAXANDCHARGESYN"] = ds.Tables[0].Rows[0]["FLDTAXANDCHARGESYN"].ToString();
            txtAccountCode.Text = ds.Tables[0].Rows[0]["FLDBANKACCOUNT"].ToString();
            txtBatchNo.Text = ds.Tables[0].Rows[0]["FLDBATCHNUMBER"].ToString();
            txtpaydate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDPAYMENTDATE"].ToString());
            txtPaymentMode.Text = ds.Tables[0].Rows[0]["FLDPAYMENTMODENAME"].ToString();
            ChkTaxchargesYN.Checked = ds.Tables[0].Rows[0]["FLDTAXANDCHARGESYN"].ToString() == "1" ? true : false;
            if (ds.Tables[0].Rows[0]["FLDBANKCHARGESVOUCHERID"].ToString() != "")
                ChkTaxchargesYN.Enabled = false;
            else
                ChkTaxchargesYN.Enabled = true;
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alCaptions = { "S.No.", "Bank Voucher No.", "Remittance No.", "Payment Voucher No.", "File No.", "Amount", "Benificiary", "Beneficiary Bank SWIFT Code", "Beneficiary Bank", "Beneficiary Bank Acct.No." };
        string[] alColumns = { "FLDROWNUM", "FLDVOUCHERNUMBER", "FLDREMITTANCENUMBERLIST", "FLDPAYMENTVOUCHERLIST", "FLDEMPLOYEECODE", "FLDREMITTANCEAMOUNT", "FLDBENEFICIARYNAME", "FLDBANKSWIFTCODE", "FLDBANKNAME", "FLDACCOUNTNUMBER" };

        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["Batchid"] != null)
        {
            ds = PhoenixAccountsAllotmentRemittance.BatchRemittanceInstructionList(ViewState["Batchid"].ToString(), sortexpression, sortdirection
                                                            , (int)ViewState["PAGENUMBER"]
                                                            , gvRemittence.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount);
            General.SetPrintOptions("gvRemittence", "Remittance", alCaptions, alColumns, ds);

            gvRemittence.DataSource = ds;
            gvRemittence.VirtualItemCount = iRowCount;
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
 
    protected void gvRemittence_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
           // e.Item.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#bbddff'");
            // when mouse leaves the row, change the bg color to its original value   
           // e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
            //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (SessionUtil.CanAccess(this.ViewState, "cmdDelete"))
                {
                    if (db != null)
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Please Confirm that Selected Remittance Instruction will be REMOVED from the banking platform MANUALLY if file is already uploaded'); return false;");
                }
                else
                {
                    if (db != null) db.Attributes.Add("style", "visibility:hidden");
                }

                if (ViewState["remittanceid"] != null)
                {
                    RadLabel lblremittanceid = (RadLabel)e.Item.FindControl("lblremittanceid");
                    if (lblremittanceid != null)
                    {
                        if (ViewState["remittanceid"].ToString() == lblremittanceid.Text)
                        {
                           // AddNewRow(gvRemittence, e.Item.RowIndex, lblremittanceid.Text, 1);
                            ImageButton cmdBDetails = (ImageButton)e.Item.FindControl("cmdBDetails");
                            if (cmdBDetails != null)
                                cmdBDetails.ImageUrl = Session["images"] + "/downarrow.png";
                        }
                    }
                }
            }
        }
    }
    protected void gvRemittence_RowDeleting(object sender, GridCommandEventArgs e)
    {

        try
        {
            string strRemittanceInstructionId = ((RadLabel)e.Item.FindControl("lblRemittenceInstructionId")).Text;
            string strAllotmentRemittanceBatchId = ((RadLabel)e.Item.FindControl("lblAllotmentRemittanceBatchId")).Text;
            PhoenixAccountsAllotmentRemittance.RemittanceBatchInstructionDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["Batchid"].ToString()), strRemittanceInstructionId);

            if (!strAllotmentRemittanceBatchId.Equals(string.Empty))
            {
                PhoenixAccountsAllotmentRemittance.AllotmentRemittanceBatchLineItemUpdate(new Guid(strAllotmentRemittanceBatchId));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        gvRemittence.Rebind();
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvRemittence.Rebind();
    }

    protected void gvRemittence_RowCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            int iRowno;
            iRowno = e.Item.RowIndex;
            BindPageURL(iRowno);
        }
        else if (e.CommandName.ToUpper().Equals("PAGE"))
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["Remittenceid"] = ((RadLabel)gvRemittence.Items[rowindex].FindControl("lblRemittenceId")).Text;
            PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvRemittence.Items[rowindex].FindControl("lnkRemittenceid")).Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvRemittence.Rebind();
    }

    protected void gvRemittence_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRemittence.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvRemittence_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
    {
        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
        string lblremittanceid = dataItem.GetDataKeyValue("FLDREMITTANCEIDLIST").ToString();
        DataSet ds = PhoenixAccountsAllotmentRemittance.Editremittance(lblremittanceid);      
        e.DetailTableView.DataSource = ds.Tables[0];
    }
}
