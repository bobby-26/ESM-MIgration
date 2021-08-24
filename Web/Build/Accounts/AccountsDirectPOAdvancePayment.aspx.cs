using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class Accounts_AccountsDirectPOAdvancePayment : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvOrderPartPaid.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvOrderPartPaid.UniqueID, "Edit$" + r.RowIndex.ToString());
    //        }
    //    }
    //    base.Render(writer);
    //}

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
                ViewState["ORDERID"] = "";
                ViewState["VESSELID"] = "";
                gvOrderPartPaid.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if (Request.QueryString["orderid"] != null)
                    ViewState["ORDERID"] = Request.QueryString["orderid"].ToString();
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsDirectPOAdvancePayment.aspx?OrderId=" + Request.QueryString["OrderId"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvOrderPartPaid')", "Print Grid", "icon_print.png", "Print");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //BindData();
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

        string[] alColumns = { "FLDDESCRIPTION", "FLDAMOUNT", "FLDVOUCHERNUMBER", "FLDVOUCHERDATE" };
        string[] alCaptions = { "Description", "Amount", "Voucher Number", "Voucher Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dts = PhoenixAccountsInvoice.InvoiceDirectPOEdit(new Guid(ViewState["ORDERID"].ToString()));

        if (dts.Rows.Count > 0)
        {
            DataRow dr = dts.Rows[0];

            txtOrderNumber.Text = dr["FLDFORMNO"].ToString();
            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
        }

        DataSet ds = PhoenixAccountsInvoice.DirectPOAdvancePaymentSearch(General.GetNullableGuid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvOrderPartPaid.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        gvOrderPartPaid.DataSource = ds;
        gvOrderPartPaid.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvOrderPartPaid", "Part Paid", alCaptions, alColumns, ds);
    }


    protected void gvOrderPartPaid_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            string strorderid = Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : "";

            Guid orderid = new Guid(strorderid);

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string Script = "";
                Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
                Script += "fnReloadList(null, 'yes', 'yes');";
                Script += "</script>" + "\n";
                if (!IsValidOrderPartPaid(
                    ((RadTextBox)e.Item.FindControl("txtAmountAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertOrderPartPaid(
                    orderid,
                    decimal.Parse(((RadTextBox)e.Item.FindControl("txtAmountAdd")).Text),
                    ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text
                );
                ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Focus();
                Rebind();

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string Script = "";
                Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
                Script += "fnReloadList(null, 'yes', 'yes');";
                Script += "</script>" + "\n";
                if (!IsValidOrderPartPaid(
                    ((RadTextBox)e.Item.FindControl("txtAmountEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                string strorderpartpaidid = ((RadLabel)e.Item.FindControl("lblOrderPartPaidIdEdit")).Text;

                Guid orderpartpaidid = new Guid(strorderpartpaidid);

                UpdateOrderPartPaid(
                    orderpartpaidid,
                    decimal.Parse(((RadTextBox)e.Item.FindControl("txtAmountEdit")).Text),
                    ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text
                 );
                Rebind();

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string Script = "";
                Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
                Script += "fnReloadList(null, 'yes', 'yes');";
                Script += "</script>" + "\n";
                DeleteOrderPartPaid(new Guid(((RadLabel)e.Item.FindControl("lblOrderPartPaidId")).Text));
                Rebind();
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                return;
            }

            else if (e.CommandName.ToUpper().Equals("PAYMENTCANCEL"))
            {
                PhoenixAccountsInvoice.PartPaidCancel(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblOrderPartPaidId")).Text));
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvOrderPartPaid_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            RadLabel lb = (RadLabel)e.Item.FindControl("lblstatus");
            ImageButton imgb = (ImageButton)e.Item.FindControl("cmdApprove");
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
            LinkButton lnkDescription = (LinkButton)e.Item.FindControl("lnkDescription");
            ImageButton imgCancel = (ImageButton)e.Item.FindControl("imgCancel");
            ImageButton imgApprove = (ImageButton)e.Item.FindControl("cmdApprove");
            ImageButton cmdApprovalHistory = (ImageButton)e.Item.FindControl("cmdApprovalHistory");

            if (imgb != null && lb != null && lb.Text == "APP")
            {
                imgb.Visible = false;
                if (cmdDelete != null)
                    cmdDelete.Visible = false;
                if (cmdEdit != null)
                    cmdEdit.Visible = false;
                if (lnkDescription != null)
                    lnkDescription.CommandName = "";
                e.Item.Attributes["onclick"] = "";

                if (cmdApprovalHistory != null)
                {
                    cmdApprovalHistory.Visible = true;
                    cmdApprovalHistory.Attributes.Add("onclick", "javascript:Openpopup('codehelp','','../Accounts/AccountsApprovalHistory.aspx?docid=" + drv["FLDORDERPARTPAIDID"].ToString() + "'); return false;");
                }
            }
            else
            {
                if (imgCancel != null && lb != null && lb.Text == "PAV")
                {
                    if (cmdDelete != null)
                        cmdDelete.Visible = false;

                    if (cmdEdit != null)
                        cmdEdit.Visible = false;

                    if (imgb != null)
                    {
                        imgb.ToolTip = "Approve";
                        imgb.Attributes.Add("onclick", "openNewWindow('approval', '', '" + Session["sitepath"] + "/Common/CommonApproval.aspx?docid=" + drv["FLDORDERPARTPAIDID"].ToString() + "&mod=" + PhoenixModule.ACCOUNTS
        + "&type=" + drv["FLDAPPROVALTYPE"].ToString() + "&user=" + drv["FLDTECHDIRECTOR"].ToString() + "," + drv["FLDFLEETMANAGER"].ToString() + "," + drv["FLDSUPT"].ToString() + "&subtype=DIRECTPO');return false;");

                        imgb.Visible = SessionUtil.CanAccess(this.ViewState, imgb.CommandName);
                    }

                    imgCancel.Visible = true;

                    if (cmdApprovalHistory != null)
                    {
                        cmdApprovalHistory.Visible = true;
                        cmdApprovalHistory.Attributes.Add("onclick", "javascript:Openpopup('codehelp','','../Accounts/AccountsApprovalHistory.aspx?docid=" + drv["FLDORDERPARTPAIDID"].ToString() + "'); return false;");
                    }

                }

                else if (lb != null && lb.Text == "CNL")
                {
                    if (cmdApprovalHistory != null)
                        cmdApprovalHistory.Visible = false;

                    imgb.Visible = false;
                    if (cmdDelete != null)
                        cmdDelete.Visible = false;
                    if (cmdEdit != null)
                        cmdEdit.Visible = false;
                    if (lnkDescription != null)
                        lnkDescription.CommandName = "";
                    if (imgCancel != null)
                        imgCancel.Visible = false;
                    if (imgApprove != null)
                        imgApprove.Visible = false;

                    e.Item.Attributes["onclick"] = "";
                }
                else
                {
                    if (imgb != null)
                    {
                        imgb.ToolTip = "Approve";
                        imgb.Attributes.Add("onclick", "openNewWindow('approval', '', '" + Session["sitepath"] + "/Common/CommonApproval.aspx?docid=" + drv["FLDORDERPARTPAIDID"].ToString() + "&mod=" + PhoenixModule.ACCOUNTS
                                + "&type=" + drv["FLDAPPROVALTYPE"].ToString() + "&user=" + drv["FLDTECHDIRECTOR"].ToString() + "," + drv["FLDFLEETMANAGER"].ToString() + "," + drv["FLDSUPT"].ToString() + "&subtype=DIRECTPO');return false;");

                        imgb.Visible = SessionUtil.CanAccess(this.ViewState, imgb.CommandName);
                    }
                    if (cmdApprovalHistory != null)
                        cmdApprovalHistory.Visible = false;
                }
            }

            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucRemarksTT");
            if (lbtn != null)
            {
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }

            Guid? lblDtkey = General.GetNullableGuid(drv["FLDDTKEY"].ToString());
            int? attachmentFlag = General.GetNullableInteger(drv["FLDATTACHMENTFLAG"].ToString());
            ImageButton cmdAttachment = (ImageButton)e.Item.FindControl("cmdAttachment");
            if (cmdAttachment != null)
            {
                if (attachmentFlag == 1)
                {

                    cmdAttachment.Attributes.Add("onclick", "openNewWindow('codehelp','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                        + PhoenixModule.ACCOUNTS + "&U=" + attachmentFlag.ToString() + "&DocSource=PARTPAID');return true;");
                }
                else
                {
                    cmdAttachment.Attributes.Add("onclick", "openNewWindow('codehelp','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                        + PhoenixModule.ACCOUNTS + "&DocSource=PARTPAID');return true;");
                }

                cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            }
            ImageButton cmdNoAttachment = (ImageButton)e.Item.FindControl("cmdNoAttachment");
            if (cmdNoAttachment != null)
            {
                if (attachmentFlag == 1)
                {
                    cmdNoAttachment.Attributes.Add("onclick", "openNewWindow('codehelp','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                    + PhoenixModule.ACCOUNTS + "&U=" + attachmentFlag.ToString() + "&DocSource=PARTPAID');return true;");
                }
                else
                {
                    cmdNoAttachment.Attributes.Add("onclick", "openNewWindow('codehelp','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                    + PhoenixModule.ACCOUNTS + "&DocSource=PARTPAID');return true;");
                }

                cmdNoAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdNoAttachment.CommandName);
            }
            ImageButton sb = (ImageButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton iab = (ImageButton)e.Item.FindControl("cmdAttachment");
            ImageButton inab = (ImageButton)e.Item.FindControl("cmdNoAttachment");
            if (iab != null) iab.Visible = true;
            if (inab != null) inab.Visible = false;
            int? n = General.GetNullableInteger(drv["FLDATTACHMENTCOUNT"].ToString());
            if (n == 0)
            {
                if (iab != null) iab.Visible = false;
                if (inab != null) inab.Visible = true;
            }

        }
        if (e.Item is GridFooterItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvOrderPartPaid_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvOrderPartPaid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOrderPartPaid.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void InsertOrderPartPaid(Guid orderid, decimal amount, string description)
    {
        PhoenixAccountsInvoice.DirectPOAdvancePaymentInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, orderid, description, amount, General.GetNullableInteger(ViewState["VESSELID"].ToString()));
    }

    private void UpdateOrderPartPaid(Guid orderpartpaidid, decimal amount, string description)
    {
        PhoenixAccountsInvoice.DirectPOAdvancePaymentUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, orderpartpaidid, description, amount);
    }

    private bool IsValidOrderPartPaid(string amount, string description)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";

        if (description.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        return (!ucError.IsError);
    }

    private void DeleteOrderPartPaid(Guid orderpartpaidid)
    {
        PhoenixAccountsInvoice.DirectPOAdvancePaymentDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, orderpartpaidid);
    }

    private void ApprovedRequestAdvance(Guid orderpartpaidid)
    {
        string strorderid = Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : "";

        //DataSet ds = PhoenixPurchaseOrderPartPaid.ApprovedRequestAdvance(
        //    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
        //    orderpartpaidid,
        //    new Guid(strorderid),
        //    Filter.CurrentPurchaseVesselSelection);

        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    DataRow dr = ds.Tables[0].Rows[0];
        //    PhoenixAccountsAdvancePayment.AdvancePaymentInsert(
        //        PhoenixSecurityContext.CurrentSecurityContext.UserCode
        //        , int.Parse(dr["FLDVENDORID"].ToString())
        //        , decimal.Parse(dr["FLDAMOUNT"].ToString())
        //        , DateTime.Parse(DateTime.Now.ToString())
        //        , int.Parse(dr["FLDCURRENCYID"].ToString())
        //        , null
        //        , dr["FLDFORMNO"].ToString()
        //        , null
        //        , new Guid(strorderid)
        //        , int.Parse(PhoenixCommonRegisters.GetHardCode(1, 124, "SAS")),
        //         General.GetNullableInteger(dr["FLDBILLTOCOMPANYID"].ToString()), // companyid
        //         General.GetNullableInteger(dr["FLDBUDGETCODEID"].ToString()), // budget code
        //         General.GetNullableInteger(dr["FLDVESSELID"].ToString()), // vessel id
        //         General.GetNullableInteger(dr["FLDBANKID"].ToString()),// bankid
        //         General.GetNullableGuid(dr["FLDORDERPARTPAIDID"].ToString()) // part paid id
        //         );
        //}
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDDESCRIPTION", "FLDAMOUNT", "FLDVOUCHERNUMBER", "FLDVOUCHERDATE" };
        string[] alCaptions = { "Description", "Amount", "Voucher Number", "Voucher Date" };
        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string strorderid = Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : "";

        Guid orderid = new Guid(strorderid);

        ds = PhoenixAccountsInvoice.DirectPOAdvancePaymentSearch(General.GetNullableGuid(ViewState["ORDERID"].ToString()), sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=Part_Paid.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Part Paid</h3></td>");
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
        gvOrderPartPaid.SelectedIndexes.Clear();
        gvOrderPartPaid.EditIndexes.Clear();
        gvOrderPartPaid.DataSource = null;
        gvOrderPartPaid.Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
