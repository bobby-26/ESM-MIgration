using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;

public partial class AccountsRemittanceMaster : PhoenixBasePage
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

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("History", "HISTORY", ToolBarDirection.Right);
            toolbarmain.AddButton("Submit for MD Approval", "SUBMITFORMDAPPROVAL" ,ToolBarDirection.Right);
            toolbarmain.AddButton("Invoice", "INVOICE", ToolBarDirection.Right);
            toolbarmain.AddButton("Line Items", "LINEITEMS", ToolBarDirection.Right);
            toolbarmain.AddButton("Remittance", "REMITTANCE", ToolBarDirection.Right);

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            //  MenuOrderFormMain.SetTrigger(pnlRemittance);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsRemittanceMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvRemittence')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsRemittanceFilter.aspx", "Find", "search.png", "FIND");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            // MenuOrderForm.SetTrigger(pnlRemittance);

            if (!IsPostBack)
            {
                Session["New"] = "N";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["REMITTENCEID"] = null;
                ViewState["INDEXNO"] = 0;
                if (Request.QueryString["REMITTENCEID"] != null)
                {
                    ViewState["Remittenceid"] = Request.QueryString["REMITTENCEID"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceRequest.aspx?REMITTENCEID=" + ViewState["Remittenceid"];
                }
                MenuOrderFormMain.SelectedMenuIndex = 4;
            }
            gvRemittence.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
            if (CommandName.ToUpper().Equals("REMITTANCE"))
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceRequest.aspx?REMITTENCEID=" + ViewState["Remittenceid"];
            }
            if (CommandName.ToUpper().Equals("LINEITEMS") && ViewState["Remittenceid"] != null && ViewState["Remittenceid"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsRemittanceRequestLineItem.aspx?REMITTENCEID=" + ViewState["Remittenceid"]);
            }
            if (CommandName.ToUpper().Equals("INVOICE") && ViewState["Remittenceid"] != null && ViewState["Remittenceid"].ToString() != string.Empty)
            {
                ViewState["Indexno"] = 0;
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("gvindexno", ViewState["Indexno"].ToString());
                Filter.CurrentRemittenceinvoicegvindex = criteria;
                Response.Redirect("../Accounts/AccountsRemittanceInvoiceMaster.aspx?REMITTANCEID=" + ViewState["Remittenceid"]);
            }

            if (CommandName.ToUpper().Equals("SUBMITFORMDAPPROVAL"))
            {
                PhoenixAccountsRemittance.PrepareRemittanceInstruction(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);

                MenuOrderFormMain.SelectedMenuIndex = 1;
            }
            if (CommandName.ToUpper().Equals("HISTORY") && ViewState["Remittenceid"] != null && ViewState["Remittenceid"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsRemittanceHistory.aspx?REMITTANCEID=" + ViewState["Remittenceid"]);
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

        string[] alCaptions = { "Remittance Number", "Supplier Code", "Supplier Name", "Status", "Payment mode", "Bank Charge Basis", "Account Code", "Account Description", "Currency", "Remittance Amount" };
        string[] alColumns = { "FLDREMITTANCENUMBER", "FLDSUPPLIERCODE", "FLDSUPPLIERNAME", "FLDREMITTANCESTATUS", "FLDREMITTANCEPAYMENTMODENAME", "FLDREMITTANCEBANKCHARGENAME", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDCURRENCYCODE", "FLDREMITTANCEAMOUNT" };


        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentRemittenceRequestSelection;
        ds = PhoenixAccountsRemittance.RemittanceSearch(null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                    (nvc != null) ? General.GetNullableString(nvc.Get("txtRemittenceNumberSearch").ToString().Trim()) : null
                   , (nvc != null) ? General.GetNullableInteger(nvc.Get("ddlAccountCode").ToString().Trim()) : null
                   , (nvc != null) ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode").ToString().Trim()) : null
                   , (nvc != null) ? General.GetNullableString(nvc.Get("txtVoucherFromdateSearch")) : null
                   , (nvc != null) ? General.GetNullableString(nvc.Get("txtVoucherTodateSearch")) : null
                   , (nvc != null) ? General.GetNullableInteger(nvc.Get("ucRemittanceStatus").ToString().Trim()) : null
                   , sortexpression
                   , sortdirection
                   , gvRemittence.CurrentPageIndex + 1
                   , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                   , ref iRowCount
                   , ref iTotalPageCount
                   , (nvc != null) ? General.GetNullableInteger(nvc.Get("txtSupplierCode").ToString().Trim()) : null
                   , (nvc != null) ? General.GetNullableInteger(nvc.Get("chkIsZeroAmount").ToString()) : null
                   , nvc != null ? General.GetNullableInteger(nvc.Get("ddlPaymentmode")) : null
                   , (nvc != null) ? General.GetNullableString(nvc.Get("txtbatchno").ToString().Trim()) : null
                   , (nvc != null) ? General.GetNullableString(nvc.Get("txtvouchernumber").ToString().Trim()) : null
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int index;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentRemittenceRequestSelection;
        ds = PhoenixAccountsRemittance.RemittanceSearch(null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                    (nvc != null) ? General.GetNullableString(nvc.Get("txtRemittenceNumberSearch").ToString().Trim()) : null
                   , (nvc != null) ? General.GetNullableInteger(nvc.Get("ddlAccountCode").ToString().Trim()) : null
                   , (nvc != null) ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode").ToString().Trim()) : null
                   , (nvc != null) ? General.GetNullableString(nvc.Get("txtVoucherFromdateSearch")) : null
                   , (nvc != null) ? General.GetNullableString(nvc.Get("txtVoucherTodateSearch")) : null
                   , (nvc != null) ? General.GetNullableInteger(nvc.Get("ucRemittanceStatus").ToString().Trim()) : null
                   , sortexpression
                   , sortdirection
                   , gvRemittence.CurrentPageIndex + 1
                   , gvRemittence.PageSize
                   , ref iRowCount
                   , ref iTotalPageCount
                   , (nvc != null) ? General.GetNullableInteger(nvc.Get("txtSupplierCode").ToString().Trim()) : null
                   , (nvc != null) ? General.GetNullableInteger(nvc.Get("chkIsZeroAmount").ToString()) : null
                   , nvc != null ? General.GetNullableInteger(nvc.Get("ddlPaymentmode")) : null
                   ,(nvc != null) ? General.GetNullableString(nvc.Get("txtbatchno").ToString().Trim()) : null
                   , (nvc != null) ? General.GetNullableString(nvc.Get("txtvouchernumber").ToString().Trim()) : null
                    );

        string[] alCaptions = { "Remittance Number", "Supplier Code", "Supplier Name", "Status", "Payment mode", "Bank Charge Basis", "Account Code", "Account Description", "Currency", "Remittance Amount" };
        string[] alColumns = { "FLDREMITTANCENUMBER", "FLDSUPPLIERCODE", "FLDSUPPLIERNAME", "FLDREMITTANCESTATUS", "FLDREMITTANCEPAYMENTMODENAME", "FLDREMITTANCEBANKCHARGENAME", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDCURRENCYCODE", "FLDREMITTANCEAMOUNT" };

        General.SetPrintOptions("gvRemittence", "Remittance", alCaptions, alColumns, ds);

        gvRemittence.DataSource = ds;
        gvRemittence.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            int rowcount = ds.Tables[0].Rows.Count;

            if (ViewState["Remittenceid"] == null)
            {
                index = 0;
                ViewState["Remittenceid"] = ds.Tables[0].Rows[index]["FLDREMITTANCEID"].ToString();
                //gvRemittence.SelectedIndex = index;
            }

            if (ViewState["PAGEURL"] == null)
            {
                index = 0;
                NameValueCollection nvc1 = Filter.CurrentRemittencegvindex;
                if (Filter.CurrentRemittencegvindex != null)
                {
                    index = Convert.ToInt32(nvc1.Get("gvindexno").ToString().Trim());
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
                ViewState["Remittenceid"] = ds.Tables[0].Rows[index]["FLDREMITTANCEID"].ToString();
                // gvRemittence.SelectedIndex = index;
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceRequest.aspx?REMITTENCEID= " + ViewState["Remittenceid"].ToString() + "&gvindex=" + ViewState["INDEXNO"];

            }
            SetRowSelection();
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                index = -1;
                ViewState["INDEXNO"] = index;
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceRequest.aspx?gvindex=" + ViewState["INDEXNO"];
            }
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvRemittence_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
              && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                string strRemittance = ((RadLabel)e.Item.FindControl("lblRemittenceId")).Text;
                ImageButton db1 = (ImageButton)e.Item.FindControl("cmdOnHold");
                if (db1 != null)
                    db1.Attributes.Add("onclick", "javascript:openNewWindow('REMONHOLD','','" + Session["sitepath"] + "/ACCOUNTS/AccountsRemittanceOnHold.aspx" + "?REMITTENCEID=" + strRemittance + "'); return false;");
            }

            RadLabel lblAccountCode = (RadLabel)e.Item.FindControl("lblAccountCode");
            UserControlToolTip ucToolTiplblAccountName = (UserControlToolTip)e.Item.FindControl("ucToolTiplblAccountName");

            if (lblAccountCode != null && ucToolTiplblAccountName != null)
            {
                lblAccountCode.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTiplblAccountName.ToolTip + "', 'visible');");
                lblAccountCode.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTiplblAccountName.ToolTip + "', 'hidden');");
            }
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDATTACHMENTCOUNT"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                else
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:openNewWindow('att','','Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.ACCOUNTS + "'); return false;");
            }
        }
    }

    private void SetRowSelection()
    {
        gvRemittence.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvRemittence.Items)
        {
            if (item.GetDataKeyValue("FLDREMITTANCEID").ToString().Equals(ViewState["Remittenceid"].ToString()))
            {
                gvRemittence.SelectedIndexes.Add(item.ItemIndex);
                PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvRemittence.Items[item.ItemIndex].FindControl("lnkRemittenceid")).Text;
            }
        }
    }

    protected void gvRemittence_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //GridView _gridView = (GridView)sender;
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            int iRowno;
            iRowno = e.Item.ItemIndex;
            ViewState["INDEXNO"] = iRowno;
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("gvindexno", ViewState["INDEXNO"].ToString());
            Filter.CurrentRemittencegvindex = criteria;
            BindPageURL(iRowno);

            SetRowSelection();
            Rebind();
        }
        if (e.CommandName.ToUpper().Equals("PAGE"))
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
            gvRemittence.MasterTableView.Items[rowindex].Selected = true;
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceRequest.aspx?REMITTENCEID= " + ViewState["Remittenceid"].ToString() + "&gvindex=" + rowindex;

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

        if (Session["New"].ToString() == "Y")
        {
            //gvRemittence.SelectedIndex = 0;
            Session["New"] = "N";
            BindPageURL(0);
        }
    }

    protected void Rebind()
    {
        gvRemittence.SelectedIndexes.Clear();
        gvRemittence.EditIndexes.Clear();
        gvRemittence.DataSource = null;
        gvRemittence.Rebind();
    }
    protected void gvRemittence_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
