using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class VesselAccountsReimbursement : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Reimbursements", "REIMBURSEMENTS", ToolBarDirection.Right);
            toolbar.AddButton("Earnings/Deductions", "EARNINGDEDUCTION", ToolBarDirection.Right);
            MenuReimbursementGeneral.AccessRights = this.ViewState;
            MenuReimbursementGeneral.MenuList = toolbar.Show();
            MenuReimbursementGeneral.SelectedMenuIndex = 0;


            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton(Session["sitepath"] + "/VesselAccounts/VesselAccountsReimbursement.aspx?type=" + Request.QueryString["type"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvRem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/VesselAccounts/VesselAccountsReimbursementFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton(Session["sitepath"] + "/VesselAccounts/VesselAccountsReimbursement.aspx?type=" + Request.QueryString["type"], "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuReimbursement.AccessRights = this.ViewState;
            MenuReimbursement.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvRem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                //BindData();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuReimbursementGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EARNINGDEDUCTION"))
            {
                if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString() != "")
                {
                    if (Request.QueryString["type"].ToString() != "old")
                        Response.Redirect("../VesselAccounts/VesselAccountsEarningDeductionOnBoardList.aspx", true);
                }
                else
                    Response.Redirect("../VesselAccounts/VesselAccountsEarningDeduction.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuReimbursement_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvRem.SelectedIndexes.Clear();
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvRem.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDREFERENCENO", "FLDRANKCODE", "FLDFILENO", "FLDEMPLOYEENAME", "FLDEARNINGDEDUCTIONNAME", "FLDHARDNAME", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDAPPROVEDAMOUNT" };
                string[] alCaptions = { "Reference No", "Rank", "File No", "Name", "Reimbursement/Recovery", "Purpose", "Currency", "Amount", "Approved Amount" };

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                NameValueCollection nvc = Filter.CrewReimbursementFilterSelection;

                DataTable dt = PhoenixVesselAccountsReimbursement.SearchReimbursement(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                         , (byte?)General.GetNullableInteger(nvc != null ? nvc["chkActive"] : "1")
                                                                         , (byte?)General.GetNullableInteger(nvc != null ? nvc["ddlApproved"] : string.Empty)
                                                                         , General.GetNullableString(nvc != null ? nvc["txtName"] : string.Empty)
                                                                         , General.GetNullableString(nvc != null ? nvc["txtFileNo"] : string.Empty)
                                                                         , General.GetNullableInteger(nvc != null ? nvc["ddlEarDed"] : string.Empty)
                                                                         , General.GetNullableInteger(nvc != null ? nvc["ddlPurpose"] : string.Empty)
                                                                         , sortexpression, sortdirection
                                                                         , 1, iRowCount
                                                                        , ref iRowCount, ref iTotalPageCount);

                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Reimbursements/Recoveries", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CrewReimbursementFilterSelection = null;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvRem.Rebind();
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDREFERENCENO", "FLDRANKCODE", "FLDFILENO", "FLDEMPLOYEENAME", "FLDEARNINGDEDUCTIONNAME", "FLDHARDNAME", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDAPPROVEDAMOUNT" };
        string[] alCaptions = { "Reference No", "Rank", "File No", "Name", "Reimbursement/Recovery", "Purpose", "Currency", "Amount", "Approved Amount" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        NameValueCollection nvc = Filter.CrewReimbursementFilterSelection;

        DataTable dt = PhoenixVesselAccountsReimbursement.SearchReimbursement(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                        , (byte?)General.GetNullableInteger(nvc != null ? nvc["chkActive"] : "1")
                                                                        , (byte?)General.GetNullableInteger(nvc != null ? nvc["ddlApproved"] : string.Empty)
                                                                        , General.GetNullableString(nvc != null ? nvc["txtName"] : string.Empty)
                                                                        , General.GetNullableString(nvc != null ? nvc["txtFileNo"] : string.Empty)
                                                                        , General.GetNullableInteger(nvc != null ? nvc["ddlEarDed"] : string.Empty)
                                                                        , General.GetNullableInteger(nvc != null ? nvc["ddlPurpose"] : string.Empty)
                                                                        , sortexpression, sortdirection
                                                                        , (int)ViewState["PAGENUMBER"], gvRem.PageSize
                                                                        , ref iRowCount, ref iTotalPageCount);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvRem", "Reimbursements/Recoveries", alCaptions, alColumns, ds);

        gvRem.DataSource = dt;
        gvRem.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    private bool IsValidateReimbursement(string empid, string hardcode, string currency, string amt, string earorded, string desc)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableInteger(hardcode).HasValue)
            ucError.ErrorMessage = "Purpose is required";
        if (!General.GetNullableInteger(currency).HasValue)
            ucError.ErrorMessage = "Curreny is required";
        if (!General.GetNullableDecimal(amt).HasValue)
            ucError.ErrorMessage = "Amount is required";
        if (!General.GetNullableInteger(earorded).HasValue)
            ucError.ErrorMessage = "Reimbursement/Recovery is required";
        if (desc.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required";
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvRem.Rebind();
    }
    protected string GetName(string val)
    {
        string result = string.Empty;
        if (val == "1")
            result = "Reimbursement(B.O.C)";
        else if (val == "2")
            result = "Reimbursement(Monthly / OneTime)";
        else if (val == "3")
            result = "Reimbursement(E.O.C)";
        else if (val == "-1")
            result = "Recovery(B.O.C)";
        else if (val == "-2")
            result = "Recovery(Monthly / OneTime)";
        else if (val == "-3")
            result = "Recovery(E.O.C)";
        return result;
    }

    protected void gvRem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRem.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvRem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null && drv["FLDAPPROVEDYN"].ToString() == "1") ed.Visible = false;
            else if (ed != null && drv["FLDAPPROVEDYN"].ToString() == "0")
            {
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                att.Attributes.Add("onclick", "openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                       + PhoenixModule.CREW + "'); return false;");
                if (lblIsAtt.Text == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }
            }

            UserControlCurrency uc = (UserControlCurrency)e.Item.FindControl("ddlCurrency");
            if (uc != null) uc.SelectedCurrency = drv["FLDCURRENCYID"].ToString();

            RadDropDownList ddl = (RadDropDownList)e.Item.FindControl("ddlEarDed");
            if (ddl != null)
            {
                ddl.SelectedValue = drv["FLDEARNINGDEDUCTION"].ToString();
            }

            UserControlReimbursementRecovery hrd = (UserControlReimbursementRecovery)e.Item.FindControl("ddlDesc");
            if (hrd != null) hrd.SelectedHard = drv["FLDHARDCODE"].ToString();

        }
        if (e.Item is GridFooterItem)
        {
            RadDropDownList ddl = (RadDropDownList)e.Item.FindControl("ddlEarDedAdd");
            ddl.SelectedValue = "2";
            ddl.Enabled = false;
        }
    }

    protected void gvRem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem footer = (GridFooterItem)e.Item;
                string id = ((UserControlVesselEmployee)footer.FindControl("ddlEmployeeAdd")).SelectedEmployee;
                string hardcode = ((UserControlReimbursementRecovery)footer.FindControl("ddlDescAdd")).SelectedHard;
                string desc = ((RadTextBox)footer.FindControl("txtDescriptionAdd")).Text;
                string currency = ((UserControlCurrency)footer.FindControl("ddlCurrencyAdd")).SelectedCurrency;
                string amt = ((UserControlMaskNumber)footer.FindControl("txtAmountAdd")).Text;
                string earorded = ((RadDropDownList)footer.FindControl("ddlEarDedAdd")).SelectedValue;

                if (!IsValidateReimbursement(id, hardcode, currency, amt, earorded, desc))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsReimbursement.InsertVesselReimbursment(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , int.Parse(id), int.Parse(earorded), int.Parse(hardcode), int.Parse(currency), decimal.Parse(amt), desc);
                gvRem.EditIndexes.Clear();
                BindData();
                gvRem.Rebind();
            }
            if (e.CommandName.ToUpper() == "UPDATE")
            {
                GridEditableItem item = (GridEditableItem)e.Item;
                Guid id = new Guid(((RadLabel)item.FindControl("lblreimbursementid")).Text);
                string hardcode = ((UserControlReimbursementRecovery)item.FindControl("ddlDesc")).SelectedHard;
                string desc = ((RadTextBox)item.FindControl("txtDescriptionEdit")).Text;
                string currency = ((UserControlCurrency)item.FindControl("ddlCurrency")).SelectedCurrency;
                string amt = ((UserControlMaskNumber)item.FindControl("txtAmount")).Text;
                string earorded = ((RadDropDownList)item.FindControl("ddlEarDed")).SelectedValue;

                if (!IsValidateReimbursement("1", hardcode, currency, amt, earorded, desc))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsReimbursement.UpdateVesselReimbursment(id, int.Parse(earorded), int.Parse(hardcode), int.Parse(currency), decimal.Parse(amt), desc);
                gvRem.EditIndexes.Clear();
                BindData();
                gvRem.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

