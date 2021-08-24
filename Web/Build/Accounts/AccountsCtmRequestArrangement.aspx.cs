using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;


public partial class AccountsCtmRequestArrangement : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {

                PhoenixToolbar toolbarmain = new PhoenixToolbar();

                toolbarmain.AddImageButton("../Accounts/AccountsCtmRequestArrangement.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbarmain.AddImageLink("javascript:CallPrint('gvCTM')", "Print Grid", "icon_print.png", "PRINT");
                toolbarmain.AddImageButton("../Accounts/AccountsCtmRequestArrangement.aspx", "Find", "search.png", "FIND");
                toolbarmain.AddImageButton("../Accounts/AccountsCtmRequestArrangement.aspx", "Clear", "clear-filter.png", "CLEAR");
                MenuCTM.AccessRights = this.ViewState;
                MenuCTM.MenuList = toolbarmain.Show();


                if (ddlport.SelectedSeaport == "")
                {
                    ddlport.SelectedSeaport = "0";
                }

                ViewState["CTMID"] = null;
                ViewState["ACTIVEYN"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["CAPTAINCASHID"] = null;

                if (Request.QueryString["defaultFilter"] == null)
                    chkDefaultFilter.Checked = true;
                else
                    chkDefaultFilter.Checked = false;
                gvCTM.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuCTM_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                Rebind();

            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                string[] alColumns = { "FLDDATE", "FLDVESSELNAME", "FLDREQUESTNUMBER", "FLDETA", "FLDETD", "FLDSEAPORTNAME", "FLDAMOUNT", "FLDPAYMENTVOUCHERNUMBER", "FLDSTATUS", "FLDPAYMENTVOUCHERSTATUS" };
                string[] alCaptions = { "Request On", "Vessel", "Request No.", "ETA", "ETD", "Port", "Amount", "Payment Voucher Number", "Status", "Payment Status" };


                DataSet ds = PhoenixAccountsCtm.CtmSearch(General.GetNullableInteger(ddlVessel.SelectedVessel)
                        , General.GetNullableInteger(ddlport.SelectedSeaport)
                        , General.GetNullableString(txtRefNo.Text)
                        , General.GetNullableString(txtPaymentVoucherNo.Text)
                        , General.GetNullableInteger(ddlStatus.SelectedHard)
                        , General.GetNullableDateTime(txtFromDate.Text)
                        , General.GetNullableDateTime(txtToDate.Text)
                        , General.GetNullableInteger(chkDefaultFilter.Checked == true ? "1" : "")
                        , sortexpression, sortdirection
                        , 1
                        , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount);


                General.ShowExcel("CTM Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                chkDefaultFilter.Checked = false;
                Filter.CurrentCtmOfficeFilter = null;
                Rebind();
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
        gvCTM.SelectedIndexes.Clear();
        gvCTM.EditIndexes.Clear();
        gvCTM.DataSource = null;
        gvCTM.Rebind();
    }
    public void BindData()
    {

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;
            string[] alColumns = { "FLDDATE", "FLDVESSELNAME", "FLDREQUESTNUMBER", "FLDETA", "FLDETD", "FLDSEAPORTNAME", "FLDAMOUNT", "FLDPAYMENTVOUCHERNUMBER", "FLDSTATUS", "FLDPAYMENTVOUCHERSTATUS" };
            string[] alCaptions = { "Request On", "Vessel", "Request No.", "ETA", "ETD", "Port", "Amount", "Payment Voucher Number", "Status", "Payment Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixAccountsCtm.CtmSearch(General.GetNullableInteger(ddlVessel.SelectedVessel)
                     , General.GetNullableInteger(ddlport.SelectedSeaport)
                     , General.GetNullableString(txtRefNo.Text)
                     , General.GetNullableString(txtPaymentVoucherNo.Text)
                     , General.GetNullableInteger(ddlStatus.SelectedHard)
                     , General.GetNullableDateTime(txtFromDate.Text)
                     , General.GetNullableDateTime(txtToDate.Text)
                     , General.GetNullableInteger(chkDefaultFilter.Checked == true ? "1" : "")
                     , sortexpression, sortdirection
                     , 1
                     , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvCTM", "CTM Request", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["CTMID"] == null)
                {
                    ViewState["CTMID"] = ds.Tables[0].Rows[0]["FLDCAPTAINCASHID"].ToString();
                    ViewState["ACTIVEYN"] = ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString();
                }
                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsCtmRequestArrangementGeneral.aspx";
                }
            }
            gvCTM.DataSource = ds;
            gvCTM.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCTM_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event, 'Are you sure you want to cancel ?'); return false;");
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && drv["FLDACTIVEYN"].ToString() == "1" && drv["FLDPAYMENTVOUCHERID"].ToString() == "")
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                else
                    db.Visible = false;
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
                  + PhoenixModule.ACCOUNTS + "',false);");
            }
            LinkButton Details = (LinkButton)e.Item.FindControl("cmdDetails");
            if (Details != null)
            {
                Details.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                Details.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=5&showexcel=no&showword=no&reportcode=CTM&vesselid=" + drv["FLDVESSELID"].ToString() + "&captaincashid=" + drv["FLDCAPTAINCASHID"].ToString() + "',false);");
            }
        }
    }
    protected void gvCTM_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid? id = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblCaptainCashId")).Text);
                PhoenixAccountsCtm.CtmCancelUpdate(id.Value);
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                string ctmid = ((RadLabel)e.Item.FindControl("lblCaptainCashId")).Text;
                string activey = ((RadLabel)e.Item.FindControl("lblEditable")).Text;
                string peningstatus = ((RadLabel)e.Item.FindControl("lblStatus")).Text;
                ViewState["CAPTAINCASHID"] = ((RadLabel)e.Item.FindControl("lblCaptainCashId")).Text;
                ViewState["CTMID"] = ctmid;
                ViewState["ACTIVEYN"] = activey;
                if (ViewState["CTMID"] == null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsCtmRequestArrangementGeneral.aspx";
                }
                if (ViewState["CAPTAINCASHID"] != null)
                    Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?CTMID=" + ViewState["CAPTAINCASHID"] + "&a=" + ViewState["ACTIVEYN"], false);
                else
                    Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + "?CTMID=" + ViewState["CTMID"] + "&a=" + ViewState["ACTIVEYN"], false);

                if (peningstatus == "Pending")
                {
                    ViewState["ACTIVEYN"] = "0";
                }
                Rebind();
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
    protected void gvCTM_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCTM.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
