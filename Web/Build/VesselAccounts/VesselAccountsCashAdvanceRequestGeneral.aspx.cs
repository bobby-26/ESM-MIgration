using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using SouthNests.Phoenix.Accounts;

public partial class VesselAccountsCashAdvanceRequestGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            //cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("General", "GENERAL");
            toolbar.AddButton("List", "LIST");

            MenuCashAdvanceRequest.AccessRights = this.ViewState;
            MenuCashAdvanceRequest.MenuList = toolbar.Show();
            MenuCashAdvanceRequest.SelectedMenuIndex = 0;
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAIDENABLE"] = "0";
                //    txtSupplierId.Attributes.Add("style", "display:none");
                ViewState["REQUESTID"] = Request.QueryString["REQUESTID"] == null ? null : Request.QueryString["REQUESTID"];
                txtDate.Text = DateTime.Now.ToShortDateString();
                if (ViewState["REQUESTID"] != null)
                    EditCashAdvanceRequest(new Guid(ViewState["REQUESTID"].ToString()));
                ddlCurrency.VesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ViewState["PAGENUMBER"] = 1;
                //  ViewState["NYS"] = 0;
                ViewState["SORTEXPRESSION"] = null;
                ucVesselSupplier.vessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                ViewState["SORTDIRECTION"] = null;
                gvCashLineItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            MainMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCashAdvanceRequest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERAL"))
                Response.Redirect("../VesselAccounts/VesselAccountsCashAdvanceRequestGeneral.aspx?requestid=" + ViewState["REQUESTID"] + "&pageno=" + ViewState["PAGENUMBER"], false);
            else if (CommandName.ToUpper().Equals("LIST"))
                Response.Redirect("../VesselAccounts/VesselAccountsCashAdvanceRequest.aspx", false);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCrewBond_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidCashAdvanceRequest())
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["REQUESTID"] == null)
                {
                    Guid Requestid = new Guid();
                    PhoenixVesselAccountsCashAdvanceRequest.InsertCashAdvanceRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , DateTime.Parse(txtDate.Text), General.GetNullableInteger(ucPort.SelectedValue), General.GetNullableGuid(ucVesselSupplier.SelectedValue)
                        , General.GetNullableDateTime(txtETA.Text), General.GetNullableDateTime(txtETD.Text), General.GetNullableInteger(ddlCurrency.SelectedCurrency), ref Requestid);
                    ViewState["REQUESTID"] = Requestid;
                    ucstatus.Text = "Requisition Created.";
                    ucstatus.Visible = true;
                    if (ViewState["REQUESTID"] != null)
                    {
                        MenuCashLineItem.Visible = true;
                        gvCashLineItem.Visible = true;
                    }
                    else
                    {
                        MenuCashLineItem.Visible = false;
                        gvCashLineItem.Visible = false;
                    }
                }
                else
                {
                    PhoenixVesselAccountsCashAdvanceRequest.UpdateCashAdvanceRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , DateTime.Parse(txtDate.Text), General.GetNullableInteger(ucPort.SelectedValue), General.GetNullableGuid(ucVesselSupplier.SelectedValue)
                        , General.GetNullableDateTime(txtETA.Text), General.GetNullableDateTime(txtETD.Text), General.GetNullableInteger(ddlCurrency.SelectedCurrency)
                        , new Guid(ViewState["REQUESTID"].ToString()));
                    ucstatus.Text = "Requisition Updated.";
                    ucstatus.Visible = true;
                }
                EditCashAdvanceRequest(new Guid(ViewState["REQUESTID"].ToString()));

            }
            else if (CommandName.ToUpper().Equals("SENDTOOFFICE"))
            {
                NameValueCollection nvc = Filter.CurrentVesselOrderFormFilter;
                PhoenixVesselAccountsCashAdvanceRequest.CashRequestSendtoOffice(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , new Guid(ViewState["REQUESTID"].ToString()));
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("PAID"))
            {
                PhoenixVesselAccountsCashAdvanceRequest.UpdateCashAdvancePaid(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , new Guid(ViewState["REQUESTID"].ToString()));
                EditCashAdvanceRequest(new Guid(ViewState["REQUESTID"].ToString()));
                Rebind();
            }
            MainMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditCashAdvanceRequest(Guid RequestId)
    {
        DataTable dt = PhoenixVesselAccountsCashAdvanceRequest.EditCashAdvanceRequest(RequestId);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ViewState["PAIDENABLE"] = dr["FLDPAIDENABLE"].ToString();
            //txtSupplierCode.Text = dr["FLDSUPPLIERSHORTNAME"].ToString();
            //txtSupplierCode.ToolTip = dr["FLDSUPPLIERSHORTNAME"].ToString();
            //txtSupplierName.Text = dr["FLDSUPPLIERNAME"].ToString();
            //txtSupplierName.ToolTip = dr["FLDSUPPLIERNAME"].ToString();
            //txtSupplierId.Text = dr["FLDSUPPLIERCODE"].ToString();
            txtDate.Text = dr["FLDDATE"].ToString();
            ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
            ucPort.SelectedValue = dr["FLDSEAPORTID"].ToString();
            ucPort.Text = dr["FLDSEAPORTNAME"].ToString();
            txtETA.Text = dr["FLDETA"].ToString();
            //txtETB.Text = dr["FLDETB"].ToString();
                txtETD.Text = dr["FLDETD"].ToString();
            ddlCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
            ucVesselSupplier.Text = dr["FLDSUPPLIERNAME"].ToString();
            ucVesselSupplier.SelectedValue = dr["FLDSUPPLIERCODE"].ToString();
            MainMenu();
        }
    }
    public bool IsValidCashAdvanceRequest()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        {

            if (General.GetNullableInteger(ddlCurrency.SelectedCurrency) == null)
                ucError.ErrorMessage = "Currency is required";

            if (General.GetNullableDateTime(txtDate.Text) == null)
                ucError.ErrorMessage = "Date is required";


        }
        return (!ucError.IsError);
    }


    private void MainMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        if (ViewState["REQUESTID"] != null && ViewState["REQUESTID"].ToString() != "" && ViewState["PAIDENABLE"].ToString() != "1")
        {
            toolbar.AddButton("Send to Office", "SENDTOOFFICE", ToolBarDirection.Right);
            toolbar.AddButton("Save", "UPDATE", ToolBarDirection.Right);
        }
        else if(ViewState["REQUESTID"] == null && ViewState["REQUESTID"].ToString() == "" && ViewState["PAIDENABLE"].ToString() != "1") 
        {
            toolbar.AddButton("Save", "UPDATE", ToolBarDirection.Right);
        }
        if (ViewState["PAIDENABLE"].ToString() == "1")
        {
            toolbar.AddButton("Paid", "PAID", ToolBarDirection.Right);
            toolbar.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"] + "&mod="
                        + PhoenixModule.VESSELACCOUNTS + "'); return false;", "Attachment", "", "ATTACHMENT", ToolBarDirection.Right);
        }
        MenuCrewBond.AccessRights = this.ViewState;
        MenuCrewBond.MenuList = toolbar.Show();
        PhoenixToolbar toolbarSub = new PhoenixToolbar();
        toolbarSub.AddFontAwesomeButton("../VesselAccounts/VesselAccountsCashAdvanceRequestGeneral.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbarSub.AddFontAwesomeButton("javascript:CallPrint('gvCashLineItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        if (ViewState["REQUESTID"] != null && ViewState["REQUESTID"].ToString() != "")
            toolbarSub.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/VesselAccounts/VesselAccountsCashAdvanceRequestAdd.aspx?requestid=" + (ViewState["REQUESTID"] != null ? ViewState["REQUESTID"].ToString() : "") + "', 'false', '1000px', '600px' )", "Add", "<i class=\"fa fa-plus-circle\"></i>", "NEW");//currency_mismatch.png
        //toolbarSub.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/VesselAccounts/VesselAccountsCashAdvanceRequestAdd.aspx?requestid=" + ViewState["REQUESTID"] != null ? ViewState["REQUESTID"].ToString() : "" + "');return false;", "", "<i class=\"fa fa-plus-circle\"></i>", "");
        MenuCashLineItem.AccessRights = this.ViewState;
        MenuCashLineItem.MenuList = toolbarSub.Show();
    }

    protected void MenuCashLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
            ShowExcel();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDTRANSACTIONCURRENCY", "FLDTRANSACTIONAMOUNT", "FLDSTATUS" };
        string[] alCaptions = { "Name", "Rank", "Currency", "Amount", "Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        DateTime dLastActiveDate = General.GetNullableDateTime(DateTime.Now.ToString()).Value;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentVesselOrderFormFilter;
        DataTable dt = PhoenixVesselAccountsCashAdvanceRequest.SearchCashAdvanceRequestLineItem(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                   , General.GetNullableGuid(ViewState["REQUESTID"] != null ? ViewState["REQUESTID"].ToString() : "")
                                   , sortexpression, sortdirection
                                   , (int)ViewState["PAGENUMBER"], iRowCount
                                   , ref iRowCount, ref iTotalPageCount);

        General.ShowExcel("Cash Advance Request", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }


    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDTRANSACTIONCURRENCY", "FLDTRANSACTIONAMOUNT", "FLDSTATUS" };
        string[] alCaptions = { "Name", "Rank", "Currency", "Amount", "Status" };


        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentVesselOrderFormFilter;
        DataTable dt = PhoenixVesselAccountsCashAdvanceRequest.SearchCashAdvanceRequestLineItem(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                   , General.GetNullableGuid(ViewState["REQUESTID"] != null ? ViewState["REQUESTID"].ToString() : "")
                                   , sortexpression, sortdirection
                                   , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                   , gvCashLineItem.PageSize
                                   , ref iRowCount, ref iTotalPageCount);


        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        ds.AcceptChanges();

        General.SetPrintOptions("gvCashLineItem", "Cash Line Item", alCaptions, alColumns, ds);

        gvCashLineItem.DataSource = dt;
        gvCashLineItem.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvCashLineItem.SelectedIndexes.Clear();
        gvCashLineItem.EditIndexes.Clear();
        gvCashLineItem.DataSource = null;
        gvCashLineItem.Rebind();
    }

    protected void gvCashLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCashLineItem.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCashLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvCashLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string EarningDeductionId = ((RadLabel)e.Item.FindControl("lblEarningDeductionId")).Text;
                PhoenixVesselAccountsCashAdvanceRequest.DeleteCashRequestLineitem(new Guid(EarningDeductionId));
            }
            if (e.CommandName == "Page")
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

}
