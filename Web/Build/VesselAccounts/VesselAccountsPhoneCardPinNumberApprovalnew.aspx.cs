using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class VesselAccountsPhoneCardPinNumberApprovalnew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbargridsummary = new PhoenixToolbar();
            toolbargridsummary.AddImageButton("../VesselAccounts/VesselAccountsPhoneCardRequisitionLineItemNew.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargridsummary.AddImageLink("javascript:CallPrint('gvSummary')", "Print Grid", "icon_print.png", "PRINT");
            MenuPhoneCardSummary.AccessRights = this.ViewState;
            MenuPhoneCardSummary.MenuList = toolbargridsummary.Show();
            PhoenixToolbar toolbargridLineItem = new PhoenixToolbar();
            toolbargridLineItem.AddImageButton("../VesselAccounts/VesselAccountsPhoneCardRequisitionLineItemNew.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargridLineItem.AddImageLink("javascript:CallPrint('gvPhoneReqLine')", "Print Grid", "icon_print.png", "PRINT");
            MenuPhoneReqLineItem.AccessRights = this.ViewState;
            MenuPhoneReqLineItem.MenuList = toolbargridLineItem.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsPhoneCardPinNumberApprovalnew.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvPhnCrdPinNo')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsPhoneCardPinNumberApprovalnew.aspx", "Populate row", "add.png", "POPROW");
            MenuPhoneReq.AccessRights = this.ViewState;
            MenuPhoneReq.MenuList = toolbargrid.Show();
            PhoenixToolbar toolbarnew = new PhoenixToolbar();
            toolbarnew.AddButton("List", "LIST");
            toolbarnew.AddButton("Arrange Phone cards", "ARRANGE");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbarnew.Show();
            MenuOrderForm.SelectedMenuIndex = 1;
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                txtVendorId.Attributes.Add("style", "display:none");
                ViewState["REQUESTID"] = Request.QueryString["REQUESTID"];
                ViewState["ALLOWEDIT"] = "true";
                ViewState["CURRENTTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["PAGENUMBERLI"] = 1;
                ViewState["SORTEXPRESSIONLI"] = null;
                ViewState["SORTDIRECTIONLI"] = null;

                ViewState["CURRENTINDEX"] = 1;
                ViewState["ARRANGEYN"] = Request.QueryString["ARRANGEYN"];
                if (ViewState["REQUESTID"] != null)
                {
                    EditPhoneRequisition(new Guid(ViewState["REQUESTID"].ToString()));
                    EditPhoneCardRequestion();
                }
            }
            MainMenu();
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
            string[] alColumns = { "FLDREFERENCENO", "FLDNAME", "FLDPHONECARDNUMBER", "FLDPINNUMBER" };
            string[] alCaptions = { "Request No.", "Phone Card", "Phone Card No.", "Pin No." };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataSet ds = PhoenixVesselAccountsPhoneCardPinNumber.SearchPhoneCardPinNumber(new Guid(ViewState["REQUESTID"].ToString())
                            , sortexpression, sortdirection, Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), gvPhnCrdPinNo.PageSize
                            , ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvPhnCrdPinNo", "Phone Card Pin Number", alCaptions, alColumns, ds);
            gvPhnCrdPinNo.DataSource = ds;
            gvPhnCrdPinNo.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindSummary()
    {
        string[] alColumns = { "FLDNAME", "FLDQUANTITY" };
        string[] alCaptions = { "Phone Card", "Quantity" };
        DataSet ds = PhoenixVesselAccountsPhoneCardRequisition.PhoneCardRequisitionSummarySearch(ViewState["REQUESTID"] == null ? null : General.GetNullableGuid(ViewState["REQUESTID"].ToString()));
        General.SetPrintOptions("gvSummary", "Phone Card Summary", alCaptions, alColumns, ds);
        gvSummary.DataSource = ds;
        gvSummary.VirtualItemCount = ds.Tables[0].Rows.Count;
    }
    private void BindLineItem()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNAME", "FLDQUANTITY", "FLDEMPNAME" };
            string[] alCaptions = { "Phone Card", "Quantity", "Employee" };
            string sortexpression = (ViewState["SORTEXPRESSIONLI"] == null) ? null : (ViewState["SORTEXPRESSIONLI"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONLI"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONLI"].ToString());
            NameValueCollection nvc = Filter.CurrentVesselPhoneCardRequestFilter;
            DataSet ds = PhoenixVesselAccountsPhoneCardRequisition.SearchPhoneCradRequestLineItem(new Guid(ViewState["REQUESTID"].ToString()), sortexpression
                , sortdirection, Convert.ToInt16(ViewState["PAGENUMBERLI"].ToString()), gvPhoneReqLine.PageSize, ref iRowCount, ref iTotalPageCount);
            string title = "Phone Card Requisition ";
            if (ds.Tables[0].Rows.Count > 0)
            {
                title += "<br/> Request No : " + ds.Tables[0].Rows[0]["FLDREFERENCENO"].ToString() + "<br/> Request Date : " + DateTime.Parse(ds.Tables[0].Rows[0]["FLDREQUESTDATE"].ToString()).ToString("dd/MM/yyyy");
            }
            General.SetPrintOptions("gvPhoneReqLine", title, alCaptions, alColumns, ds);
            gvPhoneReqLine.DataSource = ds;
            gvPhoneReqLine.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNTLI"] = iRowCount;
            ViewState["TOTALPAGECOUNTLI"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditPhoneCardRequestion()
    {
        DataTable dt = PhoenixVesselAccountsPhoneCardRequisition.EditPhoneCardRequestion(new Guid(ViewState["REQUESTID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtVendorCode.Text = dr["FLDSUPPLIERCODE"].ToString();
            txtVendorName.Text = dr["FLDSUPPLIERNAME"].ToString();
            txtVendorId.Text = dr["FLDADDRESSCODE"].ToString();
            ucBudgetCodeEdit.SelectedBudgetCode = dr["FLDBUDGETID"].ToString();
            ddlCompany.SelectedCompany = dr["FLDBILLTOCOMPANY"].ToString();
            ddlAccountDetails.SelectedValue = dr["FLDVESSELACCOUNTID"].ToString();
            MainMenu();
        }
    }
    private void MainMenu()
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuPhonReq.AccessRights = this.ViewState;
            MenuPhonReq.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPhonReq_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["REQUESTID"] != null)
                {
                    if (!IsValidMapping(txtVendorId.Text.Trim(), ucBudgetCodeEdit.SelectedBudgetCode, ddlCompany.SelectedCompany, ddlAccountDetails.SelectedValue))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixVesselAccountsPhoneCardRequisition.PhoneCardRequestMappingInsert(new Guid(ViewState["REQUESTID"].ToString()),
                    General.GetNullableInteger(ucBudgetCodeEdit.SelectedBudgetCode.ToString()), General.GetNullableInteger(ddlCompany.SelectedCompany.ToString()), General.GetNullableInteger(txtVendorId.Text), General.GetNullableInteger(ddlAccountDetails.SelectedValue.ToString()));
                    ucStatus.Text = "Supplier is mapped.";
                }
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
            if (CommandName.ToUpper().Equals("LIST"))
                Response.Redirect("../VesselAccounts/VesselAccountsPhoneCardArrange.aspx", false);
            else if (CommandName.ToUpper().Equals("ARRANGE"))
                Response.Redirect("../VesselAccounts/VesselAccountsPhoneCardPinNumberApprovalnew.aspx?requestid=" + ViewState["REQUESTID"] + "&arrangeyn=" + ViewState["ARRANGEYN"], false);
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
                ShowSummaryExcel();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPhoneReqLineItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBERLI"] = 1;
                BindLineItem();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
                ShowLineItemExcel();
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
            string[] alColumns = { "FLDREFERENCENO", "FLDNAME", "FLDPHONECARDNUMBER", "FLDPINNUMBER" };
            string[] alCaptions = { "Request No.", "Phone Card", "Phone Card No.", "Pin No." };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixVesselAccountsPhoneCardPinNumber.SearchPhoneCardPinNumber(new Guid(ViewState["REQUESTID"].ToString())
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , iRowCount, ref iRowCount, ref iTotalPageCount);
            string title = "Phone Card";
            General.ShowExcel(title, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
            DataSet ds = PhoenixVesselAccountsPhoneCardRequisition.PhoneCardRequisitionSummarySearch(ViewState["REQUESTID"] == null ? null : General.GetNullableGuid(ViewState["REQUESTID"].ToString()));
            General.ShowExcel("Phone Card Summary", ds.Tables[0], alColumns, alCaptions, 0, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowLineItemExcel()
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
                                      , sortexpression, sortdirection, Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                      , iRowCount, ref iRowCount, ref iTotalPageCount);
            string title = "Phone Card Requisition ";
            General.ShowExcel(title, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
            BindData();
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
            if (CommandName.ToUpper().Equals("EXCEL"))
                ShowExcel();
            else if (CommandName.ToUpper().Equals("POPROW"))
            {
                PhoenixVesselAccountsPhoneCardPinNumber.ArrangePhoneCardPopulate(new Guid(ViewState["REQUESTID"].ToString()));
                Rebind(); 
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidRequest(string storeid, string pinno, string cardNumber)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(storeid))
        {
            ucError.ErrorMessage = "Phone Card is required";
        }
        if (String.IsNullOrEmpty(cardNumber))
        {
            ucError.ErrorMessage = "Phone Card No. is required";
        }
        if (String.IsNullOrEmpty(pinno))
        {
            ucError.ErrorMessage = "PIN No is required";
        }
        return (!ucError.IsError);
    }
    private bool IsValidMapping(string vendorid, string budgetId, string btoc, string VslAct)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(vendorid))
        {
            ucError.ErrorMessage = "Supplier is required";
        }
        if (String.IsNullOrEmpty(budgetId) || budgetId.ToString().Equals("Dummy"))
        {
            ucError.ErrorMessage = "Budget Code is required";
        }
        if (String.IsNullOrEmpty(btoc) || btoc.ToString().Equals("Dummy"))
        {
            ucError.ErrorMessage = "Bill to Company is required";
        }
        if (String.IsNullOrEmpty(VslAct) || VslAct.ToString().Equals("Dummy"))
        {
            ucError.ErrorMessage = "VesselAccount is required";
        }
        return (!ucError.IsError);
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
                ddlAccountDetails.DataSource = PhoenixVesselAccountsPhoneCardRequisition.VesselAccountMap(General.GetNullableInteger(dr["FLDVESSELID"].ToString()));
                ddlAccountDetails.DataBind();
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
        try
        {
            //gvPhnCrdPinNo.EditIndex = -1;
            //gvPhnCrdPinNo.SelectedIndex = -1;
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSummary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindSummary();
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
            ViewState["PAGENUMBERLI"] = ViewState["PAGENUMBERLI"] != null ? ViewState["PAGENUMBERLI"] : gvPhoneReqLine.CurrentPageIndex + 1;
            BindLineItem();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPhoneReqLine_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERLI"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPhnCrdPinNo_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPhnCrdPinNo.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPhnCrdPinNo_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                RadTextBox txtPinNo = (RadTextBox)e.Item.FindControl("txtPinNumberEdit");
                RadTextBox txtPhnCrdNo = (RadTextBox)e.Item.FindControl("txtPhnCrdNoEdit");
                string PinNoid = ((RadLabel)e.Item.FindControl("lblPinNoIdEdit")).Text;
                string txtStoreItemId = ((RadLabel)e.Item.FindControl("lblPhoneCardid")).Text;

                if (!IsValidRequest(txtStoreItemId, txtPinNo.Text.Trim(), txtPhnCrdNo.Text.Trim()))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsPhoneCardPinNumber.UpdatePhoneCardPinNumber(new Guid(PinNoid), new Guid(ViewState["REQUESTID"].ToString())
                                         , new Guid(txtStoreItemId), txtPinNo.Text.Trim(), txtPhnCrdNo.Text.Trim());
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string pinid = ((RadLabel)e.Item.FindControl("lblpinid")).Text;
                PhoenixVesselAccountsPhoneCardPinNumber.DeletePhoneCardPinNumber(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                          , new Guid(pinid));
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
    protected void Rebind()
    {
        gvPhnCrdPinNo.SelectedIndexes.Clear();
        gvPhnCrdPinNo.EditIndexes.Clear();
        gvPhnCrdPinNo.DataSource = null;
        gvPhnCrdPinNo.Rebind();
    }
    protected void gvPhnCrdPinNo_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
    }
}