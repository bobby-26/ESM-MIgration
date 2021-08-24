using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;
public partial class VesselAccountsPhoneCardRequisitionLineItemNew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargridsummary = new PhoenixToolbar();
            toolbargridsummary.AddFontAwesomeButton("../VesselAccounts/VesselAccountsPhoneCardRequisitionLineItemNew.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargridsummary.AddFontAwesomeButton("javascript:CallPrint('gvSummary')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuPhoneCardSummary.AccessRights = this.ViewState;
            MenuPhoneCardSummary.MenuList = toolbargridsummary.Show();
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsPhoneCardRequisitionLineItemNew.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvPhoneReqLine')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuPhoneReq.AccessRights = this.ViewState;
            MenuPhoneReq.MenuList = toolbargrid.Show();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("List", "LIST");
            toolbar.AddButton("Line Item", "LINEITEM");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbar.Show();
            MenuOrderForm.SelectedMenuIndex = 1;
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            if (!IsPostBack)
            {
                ViewState["REQUESTID"] = Request.QueryString["REQUESTID"] == null ? null : Request.QueryString["REQUESTID"]; ;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["REQSTATUS"] = "";
                ViewState["ARRANGEYN"] = Request.QueryString["ARRANGEYN"].ToString();
                EditPhoneRequisition(new Guid(ViewState["REQUESTID"].ToString()));
                gvPhoneReqLine.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditPhoneRequisition(Guid gRequestId)
    {
        try
        {
            DataTable dt = PhoenixVesselAccountsPhoneCardRequisition.EditPhoneCradRequest(gRequestId);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
                txtRefNo.Text = dr["FLDREFERENCENO"].ToString();
                txtRequestDate.Text = dr["FLDREQUESTDATE"].ToString();
                txtRequestDate.Enabled = false;
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
            string[] alColumns = { "FLDNAME", "FLDQUANTITY", "FLDEMPNAME" };
            string[] alCaptions = { "Phone Card", "Quantity", "Employee" };
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
            DataSet ds = PhoenixVesselAccountsPhoneCardRequisition.SearchPhoneCradRequestLineItem(new Guid(ViewState["REQUESTID"].ToString())
                                      , sortexpression, sortdirection
                                      , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                      , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
            string title = "Phone Card Requisition ";
            General.ShowExcel(title, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPhoneReq_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
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
    protected void MenuPhoneCardSummary_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowSummaryExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowSummaryExcel()
    {
        try
        {
            string[] alColumns = { "FLDNAME", "FLDQUANTITY" };
            string[] alCaptions = { "Phone Card", "Quantity" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            //ViewState["REQUESTID"] = "8445F967-3082-E611-A383-B8AC6F1DBD2C";
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixVesselAccountsPhoneCardRequisition.PhoneCardRequisitionSummarySearch(ViewState["REQUESTID"] == null ? null : General.GetNullableGuid(ViewState["REQUESTID"].ToString()));
            General.ShowExcel("Phone Card Summary", ds.Tables[0], alColumns, alCaptions, 0, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPhoneReqLine_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPhoneReqLine.CurrentPageIndex + 1;
            BindData();
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
            string[] alColumns = { "FLDNAME", "FLDQUANTITY", "FLDEMPNAME" };
            string[] alCaptions = { "Phone Card", "Quantity", "Employee" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = Filter.CurrentVesselPhoneCardRequestFilter;
            DataSet ds = PhoenixVesselAccountsPhoneCardRequisition.SearchPhoneCradRequestLineItemNew(new Guid(ViewState["REQUESTID"].ToString())
                             , sortexpression, sortdirection
                             , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                             , gvPhoneReqLine.PageSize, ref iRowCount, ref iTotalPageCount);
            string title = "Phone Card Requisition ";
            if (ds.Tables[0].Rows.Count > 0)
            {
                title += "<br/> Request No : " + ds.Tables[0].Rows[0]["FLDREFERENCENO"].ToString() + "<br/> Request Date : " + DateTime.Parse(ds.Tables[0].Rows[0]["FLDREQUESTDATE"].ToString()).ToString("dd/MM/yyyy");
            }
            General.SetPrintOptions("gvPhoneReqLine", title, alCaptions, alColumns, ds);
            gvPhoneReqLine.DataSource = ds;
            gvPhoneReqLine.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSummary_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    { BindSummary(); }
    private void BindSummary()
    {
        string[] alColumns = { "FLDNAME", "FLDQUANTITY" };
        string[] alCaptions = { "Phone Card", "Quantity" };
        DataSet ds = PhoenixVesselAccountsPhoneCardRequisition.PhoneCardRequisitionSummarySearch(ViewState["REQUESTID"] == null ? null : General.GetNullableGuid(ViewState["REQUESTID"].ToString()));
        General.SetPrintOptions("gvSummary", "Phone Card Summary", alCaptions, alColumns, ds);
        gvSummary.DataSource = ds;
        
    }
    protected void gvPhoneReqLine_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridEditableItem)
        {
            UserControlVesselEmployee empl = ((UserControlVesselEmployee)e.Item.FindControl("ddlEmployeeEdit"));
            if (empl != null)
            {
                empl.bind();
                empl.SelectedEmployee = drv["FLDEMPLOYEEID"].ToString();
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            RadLabel lbStatus = (RadLabel)e.Item.FindControl("lblStatus");
            RadLabel lbOrderStatus = (RadLabel)e.Item.FindControl("lblOrderStatus");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!String.IsNullOrEmpty(drv["FLDISSUEDDATE"].ToString())) db.Visible = false;
                if (drv["FLDACTIVEYN"].ToString() == "0") db.Visible = false;
            }
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                if (drv["FLDACTIVEYN"].ToString() == "0") eb.Visible = false;
            }

        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ab = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ab != null)
                ab.Visible = SessionUtil.CanAccess(this.ViewState, ab.CommandName);
            UserControlVesselEmployee emp = ((UserControlVesselEmployee)e.Item.FindControl("ddlEmployee"));
            emp.bind();
            UserControls_UserControlPhoneCard phncard = ((UserControls_UserControlPhoneCard)e.Item.FindControl("ddlPhoneCard"));
            phncard.PhoneCardList = PhoenixVesselAccountsPhoneCardQuantity.ListPhoneCardConfiguration(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        }
    }
    protected void gvPhoneReqLine_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                if (!IsValidRequest(
                    (((UserControlVesselEmployee)e.Item.FindControl("ddlEmployee")).SelectedEmployee),
                    (((UserControls_UserControlPhoneCard)e.Item.FindControl("ddlPhoneCard")).SelectedPhoneCard),
                    (((UserControlMaskNumber)e.Item.FindControl("txtQuantity")).Text)))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsPhoneCardRequisition.InsertPhoneCradRequestLineItem(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                    , new Guid(ViewState["REQUESTID"].ToString())
                                    , int.Parse(((UserControlVesselEmployee)e.Item.FindControl("ddlEmployee")).SelectedEmployee)
                                    , new Guid(((UserControls_UserControlPhoneCard)e.Item.FindControl("ddlPhoneCard")).SelectedPhoneCard)
                                    , decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("txtQuantity")).Text));
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {

                string requestlineid = ((RadLabel)e.Item.FindControl("lblRequestLineIdEdit")).Text;
                RadTextBox txtCard = (RadTextBox)e.Item.FindControl("txtCardNumberEdit");
                UserControlMaskNumber txtQty = (UserControlMaskNumber)e.Item.FindControl("txtQuantityEdit");
                UserControlDate txtissuedate = (UserControlDate)e.Item.FindControl("txtIssueDateEdit");
                UserControlVesselEmployee employeeid = (UserControlVesselEmployee)e.Item.FindControl("ddlEmployeeEdit");
                decimal qty = decimal.Parse(txtQty.Text.Trim());
                string CardNumber = txtCard.Text.Trim();

                if (!IsValidRequestUpdate(((UserControlMaskNumber)e.Item.FindControl("txtQuantityEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsPhoneCardRequisition.UpdatePhoneCradRequestLineItem(new Guid(requestlineid)
                              , decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("txtQuantityEdit")).Text)
                              , int.Parse(employeeid.SelectedEmployee));
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string id = ((RadLabel)e.Item.FindControl("lblRequestLineIdEdit")).Text.Trim();
                PhoenixVesselAccountsPhoneCardRequisition.DeletePhoneCradRequestLineItem(new Guid(id));
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
    protected void OrderForm_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("LINEITEM"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsPhoneCardRequisitionLineItemNew.aspx?requestid=" + ViewState["REQUESTID"] + "&arrangeyn=" + ViewState["ARRANGEYN"], false);
            }
            else if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsPhoneCardRequisitionNew.aspx", false);
            }
            else if (CommandName.ToUpper().Equals("ARRANGE"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsPhoneCardPinNumberApprovalnew.aspx?requestid=" + ViewState["REQUESTID"] + "&arrangeyn=" + ViewState["ARRANGEYN"], false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
    protected void Rebind()
    {
        gvPhoneReqLine.SelectedIndexes.Clear();
        gvPhoneReqLine.EditIndexes.Clear();
        gvPhoneReqLine.DataSource = null;
        gvPhoneReqLine.Rebind();
        gvSummary.SelectedIndexes.Clear();
        gvSummary.EditIndexes.Clear();
        gvSummary.DataSource = null;
        gvSummary.Rebind();
    }
    private bool IsValidRequest(string empid, string storeid, string qty)
    {

        if (String.IsNullOrEmpty(storeid))
        {
            ucError.ErrorMessage = "Phone Card is required";
        }
        if (String.IsNullOrEmpty(qty))
        {
            ucError.ErrorMessage = "Quantity is required";
        }
        if (String.IsNullOrEmpty(empid))
        {
            ucError.ErrorMessage = "Employee is required";
        }
        else if (decimal.Parse(qty) <= 0)
        {
            ucError.ErrorMessage = "Quantity should be greater than zero";
        }
        return (!ucError.IsError);
    }
    private bool IsValidRequestUpdate(string qty)
    {
        if (String.IsNullOrEmpty(qty))
        {
            ucError.ErrorMessage = "Quantity is required";
        }
        else if (decimal.Parse(qty) <= 0)
        {
            ucError.ErrorMessage = "Quantity should be greater than zero";
        }
        return (!ucError.IsError);
    }
}