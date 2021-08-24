using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseOrderPartPaid : PhoenixBasePage
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
                rgvPartPaid.PageSize = General.ShowRecords(null);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ORDERID"] = "";

                if (Request.QueryString["vendorid"] != null)
                    ViewState["VENDORID"] = Request.QueryString["vendorid"].ToString();
                else
                    ViewState["VENDORID"] = "";

                if (Request.QueryString["OrderId"] != null)
                    ViewState["ORDERID"] = Request.QueryString["OrderId"].ToString();

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddImageLink("javascript:openNewWindow('NAFA','','Reports/ReportsView.aspx?applicationcode=3&reportcode=PARTPAID&orderid=" + ViewState["ORDERID"].ToString() + "'); return false;", "Request Form", "", "REPORT",ToolBarDirection.Right);
                MenuPurchasePartPaid.AccessRights = this.ViewState;
                MenuPurchasePartPaid.MenuList = toolbarmain.Show();

                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseOrderPartPaid.aspx?OrderId=" + Request.QueryString["OrderId"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
                toolbargrid.AddFontAwesomeButton("javascript:CallPrint('rgvPartPaid')", "Print", "<i class=\"fas fa-print\"></i>", "Print");
                MenuOrderForm.AccessRights = this.ViewState;
                MenuOrderForm.MenuList = toolbargrid.Show();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void InsertOrderPartPaid(Guid orderid, decimal amount, string description)
    {
        PhoenixPurchaseOrderPartPaid.InsertOrderPartPaid(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            orderid, amount, description, Filter.CurrentPurchaseVesselSelection);
    }

    private void UpdateOrderPartPaid(Guid orderpartpaidid, Guid orderid, decimal amount, string description)
    {
        PhoenixPurchaseOrderPartPaid.UpdateOrderPartPaid(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            orderpartpaidid, orderid, amount, description, Filter.CurrentPurchaseVesselSelection);

        ucStatus.Text = "Order Part Paid information updated";
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
    
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }

    private void InsertOrderFormHistory()
    {
        PhoenixPurchaseOrderForm.InsertOrderFormHistory(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableGuid(Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : ""), 
            Filter.CurrentPurchaseVesselSelection);
    }

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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

    protected void MenuPurchasePartPaid_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
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
        int iRowCount = 0;
        int iTotalPageCount = 0;  

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDDESCRIPTION", "FLDAMOUNT",  "FLDVOUCHERNUMBER", "FLDVOUCHERDATE" };
        string[] alCaptions = { "Description", "Amount",  "Voucher Number", "Voucher Date" };
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

     ds = PhoenixPurchaseOrderPartPaid.OrderPartPaidSearch(orderid, sortexpression, sortdirection,
            1,
            rgvPartPaid.VirtualItemCount,
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

    protected void rgvPartPaid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDESCRIPTION", "FLDAMOUNT",  "FLDVOUCHERNUMBER", "FLDVOUCHERDATE" };
        string[] alCaptions = { "Description", "Amount", "Voucher Number", "Voucher Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string strorderid = Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : "";

        Guid orderid = new Guid(strorderid);
        
        txtOrderNumber.Text = Filter.CurrentPurchaseFormNumberSelection;
        

        DataSet ds = PhoenixPurchaseOrderPartPaid.OrderPartPaidSearch(orderid, sortexpression, sortdirection,
            rgvPartPaid.CurrentPageIndex+1,
            rgvPartPaid.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        rgvPartPaid.DataSource = ds;
        rgvPartPaid.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("rgvPartPaid", "Part Paid", alCaptions, alColumns, ds);
    }

    protected void rgvPartPaid_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        try
        {
            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList(null, 'yes', 'yes');";
            Script += "</script>" + "\n";

            string strorderid = Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : "";
            Guid orderid = new Guid(strorderid);
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                if (!IsValidOrderPartPaid(
                    ((UserControlDecimal)item["ACTION"].FindControl("txtAmountAdd")).Text,
                    ((RadTextBox)item["ACTION"].FindControl("txtDescriptionAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertOrderPartPaid(
                    orderid,
                    decimal.Parse(((UserControlDecimal)item["ACTION"].FindControl("txtAmountAdd")).Text),
                    ((RadTextBox)item["ACTION"].FindControl("txtDescriptionAdd")).Text
                );
                ((RadTextBox)item["ACTION"].FindControl("txtDescriptionAdd")).Focus();


                RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, true);
                rgvPartPaid.Rebind();
            }

            //else if (e.CommandName.ToUpper().Equals("SAVE"))
            //{
            //    GridDataItem item = (GridDataItem)e.Item;
            //    if (!IsValidOrderPartPaid(
            //        ((UserControlDecimal)item["AMOUNT"].FindControl("txtAmountEdit")).Text,
            //        ((RadTextBox)item["DESCRIPTION"].FindControl("txtDescriptionEdit")).Text))
            //    {
            //        ucError.Visible = true;
            //        return;
            //    }
            //    string strorderpartpaidid =item.GetDataKeyValue("FLDORDERPARTPAIDID").ToString();

            //    Guid orderpartpaidid = new Guid(strorderpartpaidid);

            //    UpdateOrderPartPaid(
            //        orderpartpaidid,
            //         orderid,
            //        decimal.Parse(((UserControlDecimal)item["AMOUNT"].FindControl("txtAmountEdit")).Text),
            //        ((RadTextBox)item["DESCRIPTION"].FindControl("txtDescriptionEdit")).Text
            //     );
            //    InsertOrderFormHistory();

            //    RadScriptManager.RegisterStartupScript(Page,typeof(Page), "BookMarkScript", Script,true);
            //}

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                DeleteOrderPartPaid(new Guid(item.GetDataKeyValue("FLDORDERPARTPAIDID").ToString()));
                InsertOrderFormHistory();
                RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, true);
            }

            else if (e.CommandName.ToUpper().Equals("PAYMENTCANCEL"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixPurchaseOrderPartPaid.PartPaidCancel(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , General.GetNullableGuid(item.GetDataKeyValue("FLDORDERPARTPAIDID").ToString()));

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }

    protected void rgvPartPaid_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;

            if (!e.Item.IsInEditMode)
            {
                LinkButton db = (LinkButton)item["ACTION"].FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            
            //Label lb = (Label)e.Row.FindControl("lblstatus");
            LinkButton imgb = (LinkButton)item["ACTION"].FindControl("cmdApprove");
            LinkButton cmdEdit = (LinkButton)item["ACTION"].FindControl("cmdEdit");
            LinkButton cmdDelete = (LinkButton)item["ACTION"].FindControl("cmdDelete");
            LinkButton lnkDescription = (LinkButton)item["DESCRIPTION"].FindControl("lnkDescription");
            LinkButton imgCancel = (LinkButton)item["ACTION"].FindControl("imgCancel");
            LinkButton imgApprove = (LinkButton)item["ACTION"].FindControl("cmdApprove");
            LinkButton cmdApprovalHistory = (LinkButton)item["ACTION"].FindControl("cmdApprovalHistory");

            if (imgb != null && General.GetNullableString(item.GetDataKeyValue("FLDSHORTNAME").ToString())!=null && item.GetDataKeyValue("FLDSHORTNAME").ToString().ToUpper() == "APP")
            {
                imgb.Visible = false;

                if (cmdDelete != null)
                    cmdDelete.Visible = false;
                if (cmdEdit != null)
                    cmdEdit.Visible = false;
                if (lnkDescription != null)
                    lnkDescription.CommandName = "";
                item.Attributes["onclick"] = "";

                if (cmdApprovalHistory != null)
                {
                    cmdApprovalHistory.Visible = true;

                    cmdApprovalHistory.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','Accounts/AccountsApprovalHistory.aspx?docid=" + drv["FLDORDERPARTPAIDID"].ToString() + "'); return false;");
                }
            }
            else
            {
                if (imgCancel != null && General.GetNullableString(item.GetDataKeyValue("FLDSHORTNAME").ToString()) != null && item.GetDataKeyValue("FLDSHORTNAME").ToString().ToUpper() == "PAV")
                {
                    if (cmdDelete != null)
                        cmdDelete.Visible = false;

                    if (cmdEdit != null)
                        cmdEdit.Visible = false;

                    if (imgb != null)
                    {
                        imgb.ToolTip = "Approve";
                        imgb.Attributes.Add("onclick", "openNewWindow('approval', '', 'Common/CommonApproval.aspx?docid=" + drv["FLDORDERPARTPAIDID"].ToString() + "&mod=" + PhoenixModule.PURCHASE
                            + "&type=" + drv["FLDAPPROVALTYPE"].ToString() + "&user=" + drv["FLDTECHDIRECTOR"].ToString() + "," + drv["FLDFLEETMANAGER"].ToString() + "," + drv["FLDSUPT"].ToString() + "&subtype=PARTPAID');return false;");
                        imgb.Visible = SessionUtil.CanAccess(this.ViewState, imgb.CommandName);
                    }

                    imgCancel.Visible = true;

                    if (cmdApprovalHistory != null)
                        cmdApprovalHistory.Visible = true;
                    cmdApprovalHistory.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','Accounts/AccountsApprovalHistory.aspx?docid=" + drv["FLDORDERPARTPAIDID"].ToString() + "'); return false;");

                }
                else if (General.GetNullableString(item.GetDataKeyValue("FLDSHORTNAME").ToString()) != null && item.GetDataKeyValue("FLDSHORTNAME").ToString().ToUpper() == "CNL")
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
                    item.Attributes["onclick"] = "";
                }
                else
                {
                    if (imgb != null)
                    {
                        imgb.ToolTip = "Approve";
                        imgb.Attributes.Add("onclick", "openNewWindow('approval', '', 'Common/CommonApproval.aspx?docid=" + drv["FLDORDERPARTPAIDID"].ToString() + "&mod=" + PhoenixModule.PURCHASE
                            + "&type=" + drv["FLDAPPROVALTYPE"].ToString() + "&user=" + drv["FLDTECHDIRECTOR"].ToString() + "," + drv["FLDFLEETMANAGER"].ToString() + "," + drv["FLDSUPT"].ToString() + "&subtype=PARTPAID');return false;");
                        imgb.Visible = SessionUtil.CanAccess(this.ViewState, imgb.CommandName);
                    }

                    if (cmdApprovalHistory != null)
                        cmdApprovalHistory.Visible = false;
                }

                RadLabel lbtn = (RadLabel)item["REMARKS"].FindControl("lblRemarks");
                UserControlToolTip uct = (UserControlToolTip)item["REMARKS"].FindControl("ucRemarksTT");
                if (lbtn != null)
                {
                    lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }
            }
            LinkButton cmdAtt = (LinkButton)item["ACTION"].FindControl("cmdAtt");
            if (cmdAtt != null)
            {
                cmdAtt.Visible = SessionUtil.CanAccess(this.ViewState, cmdAtt.CommandName);

                //Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
                //Label lblIsAtt = (Label)e.Row.FindControl("lblIsAtt");

                //if (lblIsAtt.Text == string.Empty)
                //    cmdAtt.ImageUrl = Session["images"] + "/no-attachment.png";

                if (General.GetNullableInteger(drv["FLDPAYMENTVOUCHERYN"].ToString()) == 1)
                {
                    cmdAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + item.GetDataKeyValue("FLDDTKEY").ToString() + "&mod="
                           + PhoenixModule.PURCHASE + "&type=ADVANCEPAYMENT&U=t" + "&DocSource=PARTPAID'); return false;");
                }
                else
                {
                    cmdAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + item.GetDataKeyValue("FLDDTKEY").ToString() + "&mod="
                        + PhoenixModule.PURCHASE + "&type=ADVANCEPAYMENT" + "&DocSource=PARTPAID'); return false;");
                }


                LinkButton cmdRep = (LinkButton)item["ACTION"].FindControl("cmdReport");
                Guid? OrderPartPaidid = General.GetNullableGuid(item.GetDataKeyValue("FLDORDERPARTPAIDID").ToString());

                if (cmdRep != null)
                {
                    cmdRep.Visible = SessionUtil.CanAccess(this.ViewState, cmdRep.CommandName);

                    cmdRep.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','Reports/ReportsView.aspx?applicationcode=3&reportcode=PARTPAID&OrderPartPaidid=" + OrderPartPaidid + "'); return false;");
                }
            }

        }
        if (e.Item is GridFooterItem)
        {
            GridFooterItem item = (GridFooterItem)e.Item;
            LinkButton db = (LinkButton)item["ACTION"].FindControl("cmdAdd");
            if (db != null)
            {
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
    }

    protected void rgvPartPaid_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('Codehelp1','ifMoreInfo','Y');";
            Script += "</script>" + "\n";

            GridEditableItem item = (GridEditableItem)e.Item;


            if (!IsValidOrderPartPaid(
                    ((UserControlDecimal)item["AMOUNT"].FindControl("txtAmountEdit")).Text,
                    ((RadTextBox)item["DESCRIPTION"].FindControl("txtDescriptionEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            string strorderid = Request.QueryString["OrderId"] != null ? Request.QueryString["OrderId"].ToString() : "";
            string strorderpartpaidid = item.GetDataKeyValue("FLDORDERPARTPAIDID").ToString();

            Guid orderid = new Guid(strorderid);
            Guid orderpartpaidid = new Guid(strorderpartpaidid);

            UpdateOrderPartPaid(
                orderpartpaidid,
                 orderid,
                decimal.Parse(((UserControlDecimal)item["AMOUNT"].FindControl("txtAmountEdit")).Text),
                ((RadTextBox)item["DESCRIPTION"].FindControl("txtDescriptionEdit")).Text
             );

            RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, true);
        }
       catch(Exception ex)
        {
            e.Canceled = true;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }

    protected void rgvPartPaid_EditCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        //rgvPartPaid.SelectedIndexes.Add(e.Item.ItemIndex);
    }

    protected void rgvPartPaid_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }
    private void DeleteOrderPartPaid(Guid orderpartpaidid)
    {
        PhoenixPurchaseOrderPartPaid.DeleteOrderPartPaid(PhoenixSecurityContext.CurrentSecurityContext.UserCode, orderpartpaidid);
    }
}
