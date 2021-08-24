using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections;
using Telerik.Web.UI;



public partial class AccountsRemittanceBatchInstructionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "Display:None");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Generate", "GENERATE");

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.Title = "Remittance";
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            //MenuOrderFormMain.SetTrigger(pnlRemittance);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsRemittanceBatchInstructionList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbargrid.AddImageLink("javascript:CallPrint('gvRemittence')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsRemittanceBatchInstructionList.aspx", "Exceptional Report", "task-list.png", "REPORT");

            //toolbargrid.AddImageLink("javascript:CallPrint('gvRemittence')", "Print Grid", "icon_print.png", "PRINT");
            //toolbargrid.AddImageButton("../Accounts/AccountsRemittanceFilter.aspx", "Find", "search.png", "FIND");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //  MenuOrderForm.SetTrigger(pnlRemittance);

            if (!IsPostBack)
            {
                Session["New"] = "N";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["REMITTENCEID"] = null;
                gvRemittence.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if (Request.QueryString["Batchid"] != null && Request.QueryString["Batchid"].Length >= 36)
                {
                    ViewState["Batchid"] = Request.QueryString["Batchid"].ToString();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRemittence_Sorting(object sender, GridSortCommandEventArgs e)
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
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        //int iTotalPageCount = 0;

        string[] alCaptions = { "Serial No.", "Bank Voucher Number", "Remittance Number", "Beneficiary Name", "Supplier Code", "Currency", "Remittance Amount", "Beneficiary Bank SWIFT Code", "Beneficiary Bank Account Number", "Intermediary Bank SWIFT Code", "Intermediary Bank Account Number", "Payment mode", "Account Code", "Account Description", "Bank Charge Basis" };
        string[] alColumns = { "FLDROW", "FLDVOUCHERNUMBER", "FLDREMITTANCENUMBERLIST", "FLDBENEFICIARYNAME", "FLDCODE", "FLDCURRENCYCODE", "FLDREMITTANCEAMOUNT", "FLDSWIFTCODE", "FLDACCOUNTNUMBER", "FLDISWIFTCODE", "FLDIIBANNUMBER", "FLDPAYMENTMODENAME", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDBANKCHARGEBASISNAME" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        //ds = PhoenixAccountsRemittance.RemittanceSearchforbankprocess(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, sortexpression, sortdirection);
        ds = PhoenixAccountsRemittance.BatchRemittanceInstructionList(ViewState["Batchid"].ToString(), sortexpression, sortdirection);


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
    protected void Rebind()
    {
        gvRemittence.SelectedIndexes.Clear();
        gvRemittence.EditIndexes.Clear();
        gvRemittence.DataSource = null;
        gvRemittence.Rebind();
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
            if (CommandName.ToUpper().Equals("REPORT"))
            {
                String scriptpopup = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=REMITTANCEBANKREPORT&Batchid=" + ViewState["Batchid"].ToString() + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["Batchid"] != null)
        {
            ds = PhoenixAccountsRemittance.BatchRemittanceInstructionList(ViewState["Batchid"].ToString(), sortexpression, sortdirection);

            string[] alCaptions = { "Voucher Number", "Beneficiary Name", "Supplier Code", "Supplier Name", "Currency","Amount"};
            string[] alColumns = { "FLDREMITTANCENUMBERLIST", "FLDBENEFICIARYNAME", "FLDCODE", "FLDNAME", "FLDCURRENCYCODE" , "FLDREMITTANCEAMOUNT" };

            General.SetPrintOptions("gvRemittence", "Accounts Remittance", alCaptions, alColumns, ds);


            gvRemittence.DataSource = ds;
            gvRemittence.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;

        }
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }



    protected void gvRemittence_ItemDataBound(Object sender, GridItemEventArgs e)
    {


        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {

                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (SessionUtil.CanAccess(this.ViewState, "cmdDelete"))
                {
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Please Confirm that Selected Remittance Instruction will be REMOVED from the banking platform MANUALLY if file is already uploaded'); return false;");
                }
                else
                {
                    if (db != null) db.Attributes.Add("style", "visibility:hidden");
                }



            }
        }
    }


    protected void gvRemittence_RowDeleting(object sender, GridCommandEventArgs e)
    {

        try
        {
            string strRemittanceInstructionId = ((RadLabel)e.Item.FindControl("lblRemittenceInstructionId")).Text;
            PhoenixAccountsRemittance.RemittanceBatchInstructionDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["Batchid"].ToString()), strRemittanceInstructionId);
            Rebind();
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
            if (CommandName.ToUpper().Equals("GENERATE"))
            {
                PhoenixAccountsRemittance.RemittanceBatchRegenerate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["Batchid"].ToString()));

                //string selectedagents = ",";
                //if (Session["CHECKED_ITEMS"] != null)
                //{
                //    ArrayList SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
                //    if (SelectedPvs != null && SelectedPvs.Count > 0)
                //    {
                //        foreach (string index in SelectedPvs)
                //        { selectedagents = selectedagents + index + ","; }
                //    }
                //}

                //if (selectedagents.Length > 10)
                //{
                //    PhoenixAccountsRemittance.RemittanceInstructionBatchGenerate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, selectedagents.Length > 1 ? selectedagents : null);
                //}
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedPvs = new ArrayList();
        string index = "";
        foreach (GridViewRow gvrow in gvRemittence.Items)
        {
            bool result = false;
            index = gvRemittence.MasterTableView.DataKeyValues[gvrow.RowIndex].ToString();
            if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;
            }

            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!SelectedPvs.Contains(index))
                    SelectedPvs.Add(index);
            }
            else
                SelectedPvs.Remove(index);
        }
        if (SelectedPvs != null && SelectedPvs.Count > 0)
            Session["CHECKED_ITEMS"] = SelectedPvs;
    }
    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvRemittence$ctl01$chkAllRemittance")
        {
            GridHeaderItem headerItem = gvRemittence.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            CheckBox chkAll = (CheckBox)headerItem.FindControl("chkAllRemittance");
            foreach (GridViewRow row in gvRemittence.Items)
            {
                CheckBox cbSelected = (CheckBox)row.FindControl("chkSelect");
                if (chkAll.Checked == true)
                {
                    cbSelected.Checked = true;
                }
                else
                {
                    cbSelected.Checked = false;
                }
            }
        }
    }



    private void SetRowSelection()
    {
        //gvRemittence.SelectedIndex = -1;
        //for (int i = 0; i < gvRemittence.Rows.Count; i++)
        //{
        //    if (gvRemittence.DataKeys[i].Value.ToString().Equals(ViewState["Remittenceid"].ToString()))
        //    {
        //        gvRemittence.SelectedIndex = i;
        //        PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvRemittence.Rows[i].FindControl("lnkRemittenceid")).Text;
        //    }
        //}
        if (gvRemittence.SelectedItems.Count > 0)
        {
            DataRowView drv = (DataRowView)gvRemittence.SelectedItems[0].DataItem;
            PhoenixAccountsVoucher.VoucherNumber = drv["FLDREMITTANCEINSTRUCTIONID"].ToString();
        }
        else
        {
            if (gvRemittence.MasterTableView.Items.Count > 0)
            {
                gvRemittence.MasterTableView.Items[0].Selected = true;
                PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvRemittence.Items[0].FindControl("lnkRemittenceid")).Text;
            }
        }
    }



    protected void gvRemittence_RowCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            int iRowno;
            iRowno = int.Parse(e.CommandArgument.ToString());
            BindPageURL(iRowno);
            SetRowSelection();
        }
        else if (e.CommandName == "Page")
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
            //ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceRequest.aspx?REMITTENCEID=" + ViewState["Remittenceid"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (Session["New"].ToString() == "Y")
        {
            Session["New"] = "N";
            if (gvRemittence.Items.Count > 0)
                BindPageURL(0);
            Rebind();
        }
    }

    protected void gvRemittence_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRemittence.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
