using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class VesselAccountsPhoneCardRequisitionNew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsPhoneCardRequisitionNew.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvPhonReq')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsPhoneCardRequisitionNew.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsPhoneCardRequisitionNew.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsPhoneCardRequestionGeneralNew.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuBondReq.AccessRights = this.ViewState;
            MenuBondReq.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                ViewState["ARRANGEYN"] = "0";
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["REQUESTID"] = null;
                ViewState["ALLOWEDIT"] = "true";
                ViewState["CURRENTTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvPhonReq.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
            string[] alColumns = { "FLDREFERENCENO", "FLDREQUESTDATE", "FLDREQUESTSTATUS" };
            string[] alCaptions = { "Request No", "Requested on", "Status" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentVesselPhoneCardRequestFilter;
            DataSet ds = PhoenixVesselAccountsPhoneCardRequisition.SearchPhoneCradRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , nvc != null ? nvc.Get("txtRefNo") : null
                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            string name = "Requisition of Phone Cards";

            General.ShowExcel(name, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvPhonReq.SelectedIndexes.Clear();
        gvPhonReq.EditIndexes.Clear();
        gvPhonReq.DataSource = null;
        gvPhonReq.Rebind();
    }
    protected void MenuBondReq_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtRefNo", txtRefNo.Text);
                criteria.Add("txtFromDate", txtFromDate.Text);
                criteria.Add("txtToDate", txtToDate.Text);
                criteria.Add("ddlStatus", ddlStatus.SelectedPhoneCard);
                Filter.CurrentVesselPhoneCardRequestFilter = criteria;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["REQUESTID"] = null;
                txtRefNo.Text = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                ddlStatus.SelectedPhoneCard = "";
                Filter.CurrentVesselPhoneCardRequestFilter = null;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void OrderForm_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsPhoneCardRequestionGeneralNew.aspx?requestid=" + ViewState["REQUESTID"].ToString();
            }
            else if (CommandName.ToUpper().Equals("LINEITEM"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsPhoneCardRequisitionLineItemNew.aspx?requestid=" + ViewState["REQUESTID"].ToString();
            }
            else if (CommandName.ToUpper().Equals("PHONECARD"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsPhoneCardPinNumberApprovalNew.aspx?requestid=" + ViewState["REQUESTID"].ToString();
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDREFERENCENO", "FLDREQUESTDATE", "FLDREQUESTSTATUS" };
            string[] alCaptions = { "Request No", "Requested On", "Status" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = Filter.CurrentVesselPhoneCardRequestFilter;
            DataSet ds = PhoenixVesselAccountsPhoneCardRequisition.SearchPhoneCradRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , nvc != null ? nvc.Get("txtRefNo") : null
                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , gvPhonReq.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvPhonReq", "Requisition of Phone Cards", alCaptions, alColumns, ds);
            gvPhonReq.DataSource = ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["REQUESTID"] == null)
                {
                    ViewState["REQUESTID"] = ds.Tables[0].Rows[0]["FLDREQUESTID"].ToString();
                    if (!ds.Tables[0].Rows[0]["FLDREQUESTSTATUS"].ToString().Contains("Req")) ViewState["ALLOWEDIT"] = "false";
                }
                if (ViewState["CURRENTTAB"] == null)
                    ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsPhoneCardRequestionGeneralNew.aspx";
            }
            //  SetTabHighlight();
            gvPhonReq.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPhonReq_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPhonReq.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPhonReq_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                string requestid = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;

                Response.Redirect("../VesselAccounts/VesselAccountsPhoneCardRequisitionLineItemNew.aspx?requestid=" + requestid + "&arrangeyn=" + ViewState["ARRANGEYN"].ToString());
            }
            else if (e.CommandName == "APPROVE")
            {
                string requestid = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                string reqstatus = ((RadLabel)e.Item.FindControl("lblStatus")).Text;
                ViewState["REQUESTID"] = requestid;
                if (!reqstatus.Contains("Req")) ViewState["ALLOWEDIT"] = "false";
                PhoenixVesselAccountsPhoneCardRequisition.ConfirmPhoneCradRequest(new Guid(requestid));
                ViewState["REQUESTID"] = null;
                Rebind();
                ucStatus.Text = "Requisition sent to Office.";
            }
            if (e.CommandName == "ORDERCANCEL")
            {
                string requestid = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                string reqstatus = ((RadLabel)e.Item.FindControl("lblStatus")).Text;
                ViewState["REQUESTID"] = requestid;
                if (reqstatus.Contains("Req"))
                {
                    ViewState["REQUESTID"] = null;
                    PhoenixVesselAccountsPhoneCardRequisition.CancelPhoneCradRequest(new Guid(requestid));
                }
                Rebind();
            }
            if (e.CommandName == "SENDTOVESSEL")
            {
                string requestid = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                ViewState["REQUESTID"] = requestid;
                PhoenixVesselAccountsPhoneCardRequisition.SendPhoneCardReqToVessel(new Guid(requestid));
                Rebind();
                ucStatus.Text = "Phone Card sent to vessel.";
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
    protected void gvPhonReq_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
          
            LinkButton ab = (LinkButton)e.Item.FindControl("cmdApprove");
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            LinkButton cs = (LinkButton)e.Item.FindControl("cmdSend");
            RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                if (lblStatus.Text == "Requested")
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to cancel ?')");
                }
                if (!lblStatus.Text.Contains("Req")) db.Visible = false;
            }
            if (ab != null)
            {
                ab.Visible = SessionUtil.CanAccess(this.ViewState, ab.CommandName);
                ab.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to confirm ?')");
                if (drv["FLDACTIVEYN"].ToString() == "0") ab.Visible = false;
                if (!lblStatus.Text.Contains("Req")) ab.Visible = false;
            }
            if (cs != null)
            {
                cs.Visible = SessionUtil.CanAccess(this.ViewState, cs.CommandName);
                if (lblStatus.Text != "Pending" || Filter.CurrentMenuCodeSelection.Contains("VAC-PBL-PNC"))
                {
                    cs.Visible = false;
                }
            }
        }
    }


    //protected void gvPhonReq_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = sender as GridView;
    //        int nCurrentRow = e.RowIndex;
    //        string requestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRequestId")).Text;
    //        string reqstatus = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStatus")).Text;
    //        ViewState["REQUESTID"] = requestid;
    //        if (!reqstatus.Contains("Req")) ViewState["ALLOWEDIT"] = "false";
    //        PhoenixVesselAccountsPhoneCardRequisition.ConfirmPhoneCradRequest(new Guid(requestid));
    //        ViewState["REQUESTID"] = null;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["REQUESTID"] = null;
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void SetTabHighlight()
    //{
    //    try
    //    {
    //        RadToolBar dl = (RadToolBar)MenuOrderForm.FindControl("dlstTabs");
    //        if (dl.Items.Count > 0)
    //        {
    //            if (ViewState["CURRENTTAB"].ToString().Trim().Contains("VesselAccountsPhoneCardRequestionGeneralNew.aspx"))
    //            {
    //                MenuOrderForm.SelectedMenuIndex = 0;
    //            }
    //            else if (ViewState["CURRENTTAB"].ToString().Trim().Contains("VesselAccountsPhoneCardRequisitionLineItemNew.aspx"))
    //            {
    //                MenuOrderForm.SelectedMenuIndex = 1;
    //            }
    //            else if (ViewState["CURRENTTAB"].ToString().Trim().Contains("VesselAccountsPhoneCardPinNumberApprovalNew.aspx"))
    //            {
    //                MenuOrderForm.SelectedMenuIndex = 1;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}


}