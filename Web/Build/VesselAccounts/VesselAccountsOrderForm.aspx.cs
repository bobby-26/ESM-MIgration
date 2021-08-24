using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;

public partial class VesselAccountsOrderForm : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsOrderForm.aspx?storeclass=" + Request.QueryString["storeclass"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvBondReq')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsOrderForm.aspx?storeclass=" + Request.QueryString["storeclass"].ToString(), "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsOrderForm.aspx?storeclass=" + Request.QueryString["storeclass"].ToString(), "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsOrderForm.aspx?storeclass=" + Request.QueryString["storeclass"].ToString(), "Add New", "<i class=\"fa fa-plus-circle\"></i>", "NEW");
            MenuBondReq.AccessRights = this.ViewState;
            MenuBondReq.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                //if (Request.QueryString["storeclass"].ToString() == "412")
                //    Title1.Text = "Requisition of Bond";
                //else if (Request.QueryString["storeclass"].ToString() == "411")
                //    Title1.Text = "Requisition of Provisions";
                ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["pageno"] == null ? "1" : Request.QueryString["pageno"].ToString());
                PhoenixVesselAccountsOrderForm.OrderFormFinalAmountBulkUpdate(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, null, null, null);
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["ORDERFORMID"] = "";
                ViewState["STORECLASS"] = "";
                ViewState["NEWPROCESS"] = "";
                ViewState["VESSELID"] = null;

                ViewState["STORECLASS"] = Request.QueryString["storeclass"].ToString();
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    imgReqSenttoARC.Visible = false;
                    lblReqSentARC.Visible = false;
                }
                else lblReqSent.Text = " * Received from Vessel";
                gvBondReq.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDREFERENCENO", "FLDORDERDATE", "FLDORDERSTATUS", "FLDRECEIVEDDATE" };
            string[] alCaptions = { "Order No", "Order Date", "Order Status", "Recieved on" };
            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentVesselOrderFormFilter;
            DataSet ds = PhoenixVesselAccountsOrderForm.SearchOrderForm(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                , nvc != null ? nvc.Get("txtRefNo") : null
                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                                                                , General.GetNullableInteger(Request.QueryString["storeclass"].ToString())
                                                                , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                                                                , sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Requisition of Bond", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvBondReq.SelectedIndexes.Clear();
        gvBondReq.EditIndexes.Clear();
        //gvBondReq.DataSource = null;
        gvBondReq.Rebind();
    }
    protected void MenuBondReq_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtRefNo", txtRefNo.Text);
                criteria.Add("txtFromDate", txtFromDate.Text);
                criteria.Add("txtToDate", txtToDate.Text);
                criteria.Add("ddlStatus", ddlStatus.SelectedHard);

                Filter.CurrentVesselOrderFormFilter = criteria;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["ORDERID"] = null;
                txtRefNo.Text = string.Empty;
                txtFromDate.Text = string.Empty;
                txtToDate.Text = string.Empty;
                ddlStatus.SelectedHard = "0";
                Filter.CurrentVesselOrderFormFilter = null;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("NEW"))
                Response.Redirect("../VesselAccounts/VesselAccountsOrderFormGeneral.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&pageno=" + ViewState["PAGENUMBER"] + "&NEWPROCESS=" + ViewState["NEWPROCESS"], false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDREFERENCENO", "FLDORDERDATE", "FLDORDERSTATUS", "FLDRECEIVEDDATE" };
            string[] alCaptions = { "Order No", "Order Date", "Order Status", "Recieved on" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = Filter.CurrentVesselOrderFormFilter;
            DataSet ds = PhoenixVesselAccountsOrderForm.SearchOrderForm(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , nvc != null ? nvc.Get("txtRefNo") : null
                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                            , General.GetNullableInteger(Request.QueryString["storeclass"].ToString())
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                            , sortexpression, sortdirection
                            , gvBondReq.CurrentPageIndex + 1
                            , gvBondReq.PageSize, ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvBondReq", "Requisition of Bond", alCaptions, alColumns, ds);
            gvBondReq.DataSource = ds;
            gvBondReq.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            if (ds.Tables[0].Rows.Count > 0)
                ViewState["NEWPROCESS"] = ds.Tables[0].Rows[0]["FLDNEWPROCESSYN"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBondReq_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBondReq.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBondReq_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "COPY")
            {
                string orderid = ((RadLabel)e.Item.FindControl("lblOrderId")).Text;
                PhoenixVesselAccountsOrderForm.CopyOrderForm(new Guid(orderid));
                ViewState["ORDERID"] = null;
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {

                string orderid = ((RadLabel)e.Item.FindControl("lblOrderId")).Text;
                string dtkey = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
                string newprocess = ((RadLabel)e.Item.FindControl("lblNewProcess")).Text;
                string IsstockYN = ((RadLabel)e.Item.FindControl("lblIsStockYN")).Text;
                Response.Redirect("../VesselAccounts/VesselAccountsOrderFormGeneral.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&orderid=" + orderid + "&pageno=" + ViewState["PAGENUMBER"] + "&dtkey=" + dtkey + "&NewProcess=" + newprocess + "&ISSTOCKYN=" + IsstockYN, false);
            }
            else if (e.CommandName == "APPROVE")
            {
                //string requestid = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                //string reqstatus = ((RadLabel)e.Item.FindControl("lblStatus")).Text;
                //ViewState["REQUESTID"] = requestid;
                //if (!reqstatus.Contains("Req")) ViewState["ALLOWEDIT"] = "false";
                //PhoenixVesselAccountsPhoneCardRequisition.ConfirmPhoneCradRequest(new Guid(requestid));
                //ViewState["REQUESTID"] = null;
                //Rebind();
                //ucStatus.Text = "Requisition sent to Office.";
            }
            if (e.CommandName == "DELETE")
            {
                string id = ((RadLabel)e.Item.FindControl("lblOrderId")).Text;
                PhoenixVesselAccountsOrderForm.DeleteOrderForm(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                Rebind();
            }
            if (e.CommandName.ToString().ToUpper() == "SENDTOVENDOR")
            {
                try
                {
                    ViewState["ORDERFORMID"] = ((RadLabel)e.Item.FindControl("lblOrderId")).Text;
                    RadWindowManager1.RadConfirm("Are you sure you want to send Requisition to Arc Marine?", "ConfirmSend", 320, 150, null, "Send to Arc Marine");

                    //ucConfirmSent.HeaderMessage = "Are you sure you want to send Requisition to Arc Marine?";
                    //ucConfirmSent.ErrorMessage = "";
                    //ucConfirmSent.Visible = true;
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            if (e.CommandName.ToString().ToUpper() == "SENDTOOFFICE")
            {
                try
                {
                    ViewState["ORDERFORMID"] = ((RadLabel)e.Item.FindControl("lblOrderId")).Text;
                    RadWindowManager1.RadConfirm("Are you sure you want to send Requisition to Office? Once sent to Office cannot be modify the line items.", "ConfirmSendtooffice", 320, 150, null, "Send to Office");
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            if (e.CommandName.ToString().ToUpper() == "CREATEPOREQ")
            {
                try
                {
                    ViewState["ORDERFORMID"] = ((RadLabel)e.Item.FindControl("lblOrderId")).Text;
                    ViewState["VESSELID"] = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                    //string storeclass = Request.QueryString["storeclass"].ToString();
                    RadWindowManager1.RadConfirm("Are you sure you want Create Purchase Order Requisition?", "ConfirmPOCreate", 320, 150, null, "Create PO Requisition");
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }

            }
            else if (e.CommandName == "Page")
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
    protected void gvBondReq_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            // LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton cpy = (LinkButton)e.Item.FindControl("cmdCopy");
            if (cpy != null)
            {
                cpy.Visible = SessionUtil.CanAccess(this.ViewState, cpy.CommandName);
                cpy.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to Copy ?'); return false;");
            }
            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
            {
                if (drv["FLDORDERSTATUS"].ToString().ToUpper() == "PENDING")
                {
                    cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
                }
                else
                {
                    cmdDelete.Visible = false;
                }
            }
            LinkButton cmdCreatePO = (LinkButton)e.Item.FindControl("cmdCreatePOReq");
            if (drv["FLDNEWPROCESSYN"].ToString().ToUpper() == "0")
            {
                gvBondReq.MasterTableView.GetColumn("POFORMNO").Display = false;
                gvBondReq.MasterTableView.GetColumn("POFORMSTATUS").Display = false;
            }
            else
            {
                gvBondReq.MasterTableView.GetColumn("POFORMNO").Display = true;
                gvBondReq.MasterTableView.GetColumn("POFORMSTATUS").Display = true;
            }
            if (cmdCreatePO != null)
            {
                if (drv["FLDORDERSTATUS"].ToString().ToUpper() == "PENDING" && drv["FLDNEWPROCESSYN"].ToString().ToUpper() == "1")
                {
                    cmdCreatePO.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
                }
                else
                {
                    cmdCreatePO.Visible = false;
                }
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                  + PhoenixModule.VESSELACCOUNTS + "'); return false;");

            }

            LinkButton cmdSend = (LinkButton)e.Item.FindControl("cmdSend");
            if (cmdSend != null)
            {
                cmdSend.Visible = SessionUtil.CanAccess(this.ViewState, cmdSend.CommandName);
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    cmdSend.Visible = false;
            }

            LinkButton cmdsendtoARC = (LinkButton)e.Item.FindControl("cmdsendtoARC");
            if (cmdsendtoARC != null)
            {
                cmdsendtoARC.Visible = SessionUtil.CanAccess(this.ViewState, cmdsendtoARC.CommandName);
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                    cmdsendtoARC.Visible = false;
            }

            LinkButton cmdApprove = (LinkButton)e.Item.FindControl("cmdApprove");
            if (cmdApprove != null)
            {
                cmdApprove.Visible = SessionUtil.CanAccess(this.ViewState, cmdApprove.CommandName);
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                    cmdApprove.Visible = false;

                cmdApprove.Attributes.Add("onclick", "openNewWindow('Details', '', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsOrderFormQuoteApproval.aspx?ORDERID=" + drv["FLDORDERID"].ToString() + "',false,400,350);return false;");
                //cmdApprove.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','../VesselAccounts/VesselAccountsOrderFormQuoteApproval.aspx?ORDERID=" + drv["FLDORDERID"].ToString() + "','large'); return true;");
            }

            Image imgReqSent = (Image)e.Item.FindControl("imgReqSent");
            if (imgReqSent != null && drv["FLDSENDTOOFFICEYN"] != null && drv["FLDSENDTOOFFICEYN"].ToString() == "1")
            {
                imgReqSent.Visible = true;
            }

            Image imgReqSenttoARC = (Image)e.Item.FindControl("imgReqSenttoARC");
            if (imgReqSenttoARC != null && drv["FLDREQSENTTOARCYN"] != null && drv["FLDREQSENTTOARCYN"].ToString() == "1")
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                    imgReqSenttoARC.Visible = true;
            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
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
    protected void ucConfirmSent_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = PhoenixVesselAccountsOrderForm.VesselOrderFormXml(General.GetNullableGuid(ViewState["ORDERFORMID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                string orderformxml = dt.Rows[0]["FLDORDERFORMXML"].ToString();
                string orderlinexml = dt.Rows[0]["FLDORDERLINEXML"].ToString();
                string addressxml = dt.Rows[0]["FLDADDRESSXML"].ToString();
                PhoenixVesselAccountsOrderForm.InsertOrderFormLineItemExtension(General.GetNullableGuid(ViewState["ORDERFORMID"].ToString()));
                Rebind();
                ucStatus.Text = "Requisition is sent to Arc Marine.";
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
        Rebind();
    }
    protected void ucConfirmSenttoOffice_OnClick(object sender, EventArgs e)
    {
        try
        {

            PhoenixVesselAccountsOrderForm.sendtoOfficeupdate(General.GetNullableGuid(ViewState["ORDERFORMID"].ToString()));
            BindData();
            ucStatus.Text = "Requisition is sent to Office.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmCancel_Click(object sender, EventArgs e)
    {

    }

    protected void ucConfirmPoCreate_Click(object sender, EventArgs e)
    {
        try
        {

            PhoenixVesselAccountsOrderForm.CreatePORequesition(General.GetNullableGuid(ViewState["ORDERFORMID"].ToString())
                                , int.Parse(ViewState["VESSELID"].ToString())
                                , int.Parse(ViewState["STORECLASS"].ToString()));
            BindData();
            ucStatus.Text = "PO Requisition is Created";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
