using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsAllotmentRemittanceMaster : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
        //SetTabHighlight();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            btnconfirm.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Allotment", "REMITTANCE");
            toolbarmain.AddButton("Line Item", "LINEITEM");
            toolbarmain.AddButton("History", "HISTORY");


            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            // MenuOrderFormMain.SetTrigger(pnlRemittance);
            MenuOrderFormMain.SelectedMenuIndex = 0;

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsAllotmentRemittanceMaster.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvRemittence')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsAllotmentRemittanceFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsAllotmentRemittanceMaster.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsAllotmentRemittanceBatchUpdate.aspx')", "Batch Update", "<i class=\"fas fa-user-plus\"></i>", "BATCH");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsAllotmentRemittanceCurrencyConverterBatch.aspx')", "Batch Currency Converter", "<i class=\"fas fa-currency-converter\"></i>", "CURRENCYCONVERTER");//currency_mismatch.png
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsAllotmentRemittanceMaster.aspx", "Submit for MD Approval", "<i class=\"fas fa-owner\"></i>", "MDAPPROVAL");//owner.png

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            // MenuOrderForm.SetTrigger(pnlRemittance);

            if (!IsPostBack)
            {
                Session["New"] = "N";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["REMITTANCEID"] = null;
                ViewState["INDEXNO"] = 0;
                if (Request.QueryString["REMITTANCEID"] != null)
                {
                    ViewState["REMITTANCEID"] = Request.QueryString["REMITTANCEID"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAllotmentRemittanceRequest.aspx?REMITTANCEID=" + ViewState["REMITTANCEID"];
                }
                gvRemittence.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        //Rebind();
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("REMITTANCE"))
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAllotmentRemittanceRequest.aspx?REMITTANCEID=" + ViewState["REMITTANCEID"].ToString();
                MenuOrderFormMain.SelectedMenuIndex = 0;
            }
            if (CommandName.ToUpper().Equals("LINEITEM") && ViewState["REMITTANCEID"] != null && ViewState["REMITTANCEID"].ToString() != string.Empty)
            {
                //Response.Redirect("../Accounts/AccountsAllotmentRemittanceRequestLineItem.aspx?REMITTANCEID=" + ViewState["REMITTANCEID"]);
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAllotmentRemittanceRequestLineItem.aspx?REMITTANCEID=" + ViewState["REMITTANCEID"];
                MenuOrderFormMain.SelectedMenuIndex = 1;
            }
            if (CommandName.ToUpper().Equals("HISTORY") && ViewState["REMITTANCEID"] != null && ViewState["REMITTANCEID"].ToString() != string.Empty)
            {
                //Response.Redirect("../Accounts/AccountsAllotmentRemittanceHistory.aspx?REMITTANCEID=" + ViewState["REMITTANCEID"]);
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAllotmentRemittanceHistory.aspx?REMITTANCEID=" + ViewState["REMITTANCEID"];
                MenuOrderFormMain.SelectedMenuIndex = 2;
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
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "Date", "Remittance No.", "File No.", "Name", "Status", "Payment mode", "Bank Charge Basis", "Currency", "Remittance Amount" };
        string[] alColumns = { "FLDCREATEDDATE", "FLDREMITTANCENUMBER", "FLDSUPPLIERCODE", "FLDSUPPLIERNAME", "FLDREMITTANCESTATUS", "FLDREMITTANCEPAYMENTMODENAME", "FLDREMITTANCEBANKCHARGENAME", "FLDCURRENCYCODE", "FLDREMITTANCEAMOUNT" };


        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (Filter.CurrentAllotmentRemittenceSelection != null)
        {
            NameValueCollection nvc = Filter.CurrentAllotmentRemittenceSelection;
            ds = PhoenixAccountsAllotmentRemittance.RemittanceSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                    , General.GetNullableString(nvc.Get("txtRemittenceNumberSearch").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtName").Trim())
                    , General.GetNullableString(nvc.Get("txtFileNumber").Trim())
                    , General.GetNullableInteger(nvc.Get("ddlCurrencyCode").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtVoucherFromdateSearch").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtVoucherTodateSearch").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ucRemittanceStatus").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ddlVessel").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ddlPaymentmode").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ddlPrincipal").ToString().Trim())
                    , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount);
        }
        else
        {

            ds = PhoenixAccountsAllotmentRemittance.RemittanceSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, "", null, null, null, null, null, null, null, null, null
                                                       , sortexpression, sortdirection
                                                       , (int)ViewState["PAGENUMBER"]
                                                       , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                       , ref iRowCount, ref iTotalPageCount);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=AllotmentRemittance.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Allotment Remittance</h3></td>");
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
                Filter.CurrentAllotmentRemittenceSelection = null;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("MDAPPROVAL"))
            {
                RadWindowManager1.RadConfirm("Are you sure want to submit for MD Approval?", "btnconfirm", 320, 150, null, "confirm");

                //ucConfirm.Visible = true;
                //ucConfirm.Text = "Are you sure want to submit for MD Approval?";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixAccountsAllotmentRemittance.PrepareRemittanceInstruction(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            String scriptinsert = String.Format("javascript:fnReloadList('codehelp1');");

            ucStatus.Text = "Submitted for MD Approval";

            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void BindData()
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRemittence.CurrentPageIndex + 1;
        int index;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (Filter.CurrentAllotmentRemittenceSelection != null)
        {
            NameValueCollection nvc = Filter.CurrentAllotmentRemittenceSelection;
            ds = PhoenixAccountsAllotmentRemittance.RemittanceSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                    , General.GetNullableString(nvc.Get("txtRemittenceNumberSearch").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtName").Trim())
                    , General.GetNullableString(nvc.Get("txtFileNumber").Trim())
                    , General.GetNullableInteger(nvc.Get("ddlCurrencyCode").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtVoucherFromdateSearch"))
                    , General.GetNullableString(nvc.Get("txtVoucherTodateSearch"))
                    , General.GetNullableInteger(nvc.Get("ucRemittanceStatus").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ddlVessel").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ddlPaymentmode").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ddlPrincipal").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ddlBankAccount").ToString().Trim())
                    , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvRemittence.PageSize, ref iRowCount, ref iTotalPageCount);
        }
        else
        {

            ds = PhoenixAccountsAllotmentRemittance.RemittanceSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, "", null, null, null, null, null, null, null, null, null,null
                                                       , sortexpression, sortdirection
                                                       , (int)ViewState["PAGENUMBER"]
                                                       , gvRemittence.PageSize
                                                       , ref iRowCount, ref iTotalPageCount);
        }

        string[] alCaptions = { "Date", "Remittance No.", "File No.", "Name", "Status", "Payment mode", "Bank Charge Basis", "Currency", "Remittance Amount" };
        string[] alColumns = { "FLDCREATEDDATE", "FLDREMITTANCENUMBER", "FLDSUPPLIERCODE", "FLDSUPPLIERNAME", "FLDREMITTANCESTATUS", "FLDREMITTANCEPAYMENTMODENAME", "FLDREMITTANCEBANKCHARGENAME", "FLDCURRENCYCODE", "FLDREMITTANCEAMOUNT" };

        General.SetPrintOptions("gvRemittence", "Allotment Remittance", alCaptions, alColumns, ds);

        gvRemittence.DataSource = ds;
        gvRemittence.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {

            int rowcount = ds.Tables[0].Rows.Count;

            if (ViewState["REMITTANCEID"] == null)
            {
                index = 0;
                ViewState["REMITTANCEID"] = ds.Tables[0].Rows[index]["FLDREMITTANCEID"].ToString();
                //gvRemittence.SelectedIndex = index;
            }

            if (ViewState["PAGEURL"] == null)
            {
                index = 0;
                NameValueCollection nvc = Filter.CurrentAllotmentRemittencegvindex;
                if (Filter.CurrentAllotmentRemittencegvindex != null)
                {
                    index = Convert.ToInt32(nvc.Get("gvindexno").ToString().Trim());
                    if (index <= rowcount - 1)
                    {
                        ViewState["INDEXNO"] = index;
                    }
                    else
                    {
                        index = rowcount - 1;
                        ViewState["INDEXNO"] = index;
                    }
                }
                ViewState["REMITTANCEID"] = ds.Tables[0].Rows[index]["FLDREMITTANCEID"].ToString();
                //gvRemittence.SelectedIndex = index;
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAllotmentRemittanceRequest.aspx?REMITTANCEID= " + ViewState["REMITTANCEID"].ToString() + "&gvindex=" + ViewState["INDEXNO"];

            }

        }
        else
        {
            DataTable dt = ds.Tables[0];
            if (ViewState["PAGEURL"] == null)
            {
                index = -1;
                ViewState["INDEXNO"] = index;
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAllotmentRemittanceRequest.aspx?gvindex=" + ViewState["INDEXNO"];
            }
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    //protected void cmdSort_Click(object sender, EventArgs e)
    //{
    //    ImageButton ib = (ImageButton)sender;
    //    try
    //    {
    //        gvRemittence.SelectedIndex = -1;
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


    protected void gvRemittence_RowDeleting(object sender, GridCommandEventArgs de)
    {
        try
        {
            Guid RemittanceId = new Guid(((RadLabel)de.Item.FindControl("lblRemittenceId")).Text);

            PhoenixAccountsAllotmentRemittance.DeleteAllotmentRemittance(RemittanceId);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        Rebind();
    }

    protected void gvRemittence_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");

            LinkButton da = (LinkButton)e.Item.FindControl("cmdApprove");
            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdRejection");


            string hardcodeRRJ = "";
            string hardcodePBB = "";
            DataSet ds = PhoenixRegistersHard.EditHardCode(1, 136, "RRJ");

            DataSet ds1 = PhoenixRegistersHard.EditHardCode(1, 136, "PBB");

            hardcodeRRJ = ds.Tables[0].Rows[0]["FLDHARDCODE"].ToString();

            hardcodePBB = ds1.Tables[0].Rows[0]["FLDHARDCODE"].ToString();


            if (drv["FLDSTATUSID"].ToString().Equals(hardcodeRRJ) || drv["FLDSTATUSID"].ToString().Equals(hardcodePBB))
            {
                if (db != null) db.Attributes.Add("style", "visibility:hidden");
                if (da != null) da.Attributes.Add("style", "visibility:visible");
                if (db1 != null) db1.Attributes.Add("style", "visibility:visible");


            }
            else if (SessionUtil.CanAccess(this.ViewState, "cmdDelete"))
            {
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure want to delete?'); return false;");
            }
            else
            {
                if (db != null) db.Attributes.Add("style", "visibility:hidden");
                //if (da != null) da.Attributes.Add("style", "visibility:hidden");
            }

            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsAllotmentRemittanceRejectionDetails.aspx?remittanceId=" + drv["FLDREMITTANCEID"].ToString() + "&VESSELID=" + drv["FLDVESSELID"].ToString() + "&remittancenumber=" + drv["FLDREMITTANCENUMBER"].ToString() + "&employeebankaccountid=" + drv["FLDEMPLOYEEBANKID"].ToString() + "');return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }

            //ImageButton app = (ImageButton)e.Row.FindControl("cmdApprove");
            if (SessionUtil.CanAccess(this.ViewState, "cmdApprove"))
            {
                if (da != null) da.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure want to approve the remittance?'); return false;");
            }
            //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvRemittence, "Select$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    private void SetRowSelection()
    {
        gvRemittence.SelectedIndexes.Clear();
        for (int i = 0; i < gvRemittence.Items.Count; i++)
        {
            if (gvRemittence.MasterTableView.Items[i].GetDataKeyValue("FLDREMITTANCEID").ToString().Equals(ViewState["REMITTANCEID"].ToString()))
            {
                gvRemittence.MasterTableView.Items[i].Selected = true;
                PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvRemittence.Items[i].FindControl("lnkRemittenceid")).Text;
            }
        }
    }


    protected void gvRemittence_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {


            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                string remittanceid = ((RadLabel)e.Item.FindControl("lblRemittenceId")).Text;
                string statusid = ((RadLabel)e.Item.FindControl("lblStatusId")).Text;

                int iRowno;
                iRowno = e.Item.ItemIndex;
                ViewState["INDEXNO"] = iRowno;
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("gvindexno", ViewState["INDEXNO"].ToString());
                Filter.CurrentAllotmentRemittencegvindex = criteria;
                BindPageURL(iRowno);
                Rebind();
                SetRowSelection();

            }
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                string remittanceid = ((RadLabel)e.Item.FindControl("lblRemittenceId")).Text;
                string statusid = ((RadLabel)e.Item.FindControl("lblStatusId")).Text;

                PhoenixAccountsAllotmentRemittanceRejection.AllotmentRemittanceRejectionApprove(
                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                    , new Guid(remittanceid)
                                    , int.Parse(statusid));
                Rebind();

                ucStatus.Text = "Approved Successfully.";

            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
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

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["REMITTANCEID"] = ((RadLabel)gvRemittence.Items[rowindex].FindControl("lblRemittenceId")).Text;
            PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvRemittence.Items[rowindex].FindControl("lnkRemittenceid")).Text;
            gvRemittence.MasterTableView.Items[rowindex].Selected = true;
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAllotmentRemittanceRequest.aspx?REMITTANCEID=" + ViewState["REMITTANCEID"].ToString() + "&gvindex=" + rowindex;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
        if (Session["New"].ToString() == "Y")
        {
            Session["New"] = "N";
            BindPageURL(0);
        }
    }
    protected void Rebind()
    {
        //gvRemittence.DataSource = null;
        gvRemittence.Rebind();
    }

    protected void gvRemittence_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
        //SetRowSelection();
    }
}
