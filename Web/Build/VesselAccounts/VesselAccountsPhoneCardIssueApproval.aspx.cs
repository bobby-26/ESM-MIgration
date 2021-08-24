using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class VesselAccountsPhoneCardIssueApproval : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsPhoneCardIssueApproval.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvPhnCrdPinNo')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsPhoneCardIssueApproval.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsPhoneCardIssueApproval.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuPhoneReq.AccessRights = this.ViewState;
            MenuPhoneReq.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["CURRENTTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvPhnCrdPinNo.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
            string[] alColumns = { "FLDREFERENCENO", "FLDNAME", "FLDPHONECARDNUMBER", "FLDPINNUMBER", "FLDEMPLOYEENAME", "FLDISSUEDATE" };
            string[] alCaptions = { "Request No.", "Phone Card", "Phone Card No.", "PIN No.", "Employee", "Issued On" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = Filter.CurrentVesselPhoneCardIssueFilter;
            DataSet ds = PhoenixVesselAccountsPhoneCardPinNumber.SearchIssueofPhoneCard(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                               General.GetNullableString(nvc != null ? nvc["txtRefNo"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                             , General.GetNullableString(nvc != null ? nvc["txtCardNo"] : string.Empty)
                            , General.GetNullableInteger(nvc != null ? nvc["ddlEmployee"] : string.Empty)
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , gvPhnCrdPinNo.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvPhnCrdPinNo", "Issue Of Phone Card", alCaptions, alColumns, ds);
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
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDREFERENCENO", "FLDNAME", "FLDPHONECARDNUMBER", "FLDPINNUMBER", "FLDEMPLOYEENAME", "FLDISSUEDATE" };
            string[] alCaptions = { "Request No.", "Phone Card", "Phone Card No.", "PIN No.", "Employee", "Issued On" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentVesselPhoneCardRequestFilter;
            DataSet ds = PhoenixVesselAccountsPhoneCardPinNumber.SearchIssueofPhoneCard(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                General.GetNullableString(null), null, null, null, General.GetNullableString(null), null, sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , iRowCount, ref iRowCount, ref iTotalPageCount);
            string title = "Issue Of Phone Card";
            General.ShowExcel(title, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

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
    private bool IsValidConfirmIssue(string empid, string Issuedate)
    {
        ucError.HeaderMessage = "Please provide the following required information before Confirm";
        if (String.IsNullOrEmpty(empid))
        {
            ucError.ErrorMessage = "Employee is required";
        }
        if (String.IsNullOrEmpty(Issuedate))
        {
            ucError.ErrorMessage = "Issued On is required";
        }
        return (!ucError.IsError);
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
    protected void gvPhnCrdPinNo_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {

                UserControlDate txtissuedate = (UserControlDate)e.Item.FindControl("txtIssueDateEdit");
                UserControlVesselEmployee employeeid = (UserControlVesselEmployee)e.Item.FindControl("ddlEmployeeEdit");
                string PinNoid = ((RadLabel)e.Item.FindControl("lblPinNoId")).Text;
                if (!IsValidRequestUpdate(employeeid.SelectedEmployee, txtissuedate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsPhoneCardPinNumber.UpdatePhoneCardPinNumberIssueUpdate(new Guid(PinNoid), General.GetNullableInteger(employeeid.SelectedEmployee),
                               General.GetNullableDateTime(txtissuedate.Text));
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string id = ((RadLabel)e.Item.FindControl("lblPinNoId")).Text.Trim();
                PhoenixVesselAccountsPhoneCardPinNumber.DeletePhoneCardPinNumberissue(new Guid(id));
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
               
                RadLabel lblissuedate = (RadLabel)e.Item.FindControl("lblIssueDate");
                string PinNoid = ((RadLabel)e.Item.FindControl("lblPinNoId")).Text;
                string empid = ((RadLabel)e.Item.FindControl("lblempid")).Text;
                if (!IsValidConfirmIssue(empid, lblissuedate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsPhoneCardPinNumber.VesselPhoneCardPinNumberIssueConfirm(new Guid(PinNoid));
                //  ucStatus.Text = "Phone Card Confirmed.";
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
    protected void MenuPhoneReq_TabStripCommand(object sender, EventArgs e)
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
                criteria.Add("txtRefNo", txtRefNo.Text.Trim());
                criteria.Add("txtCardNo", txtCardNo.Text.Trim());
                criteria.Add("ddlEmployee", ddlEmployee.SelectedEmployee);
                criteria.Add("txtFromDate", txtFromDate.Text);
                criteria.Add("txtToDate", txtToDate.Text);
                criteria.Add("ddlStatus", ddlStatus.SelectedPhoneCard);
                Filter.CurrentVesselPhoneCardIssueFilter = criteria;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["REQUESTID"] = null;
                Filter.CurrentVesselPhoneCardIssueFilter = null;
                txtCardNo.Text = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtRefNo.Text = "";
                ddlEmployee.SelectedEmployee = "";
                ddlStatus.SelectedPhoneCard = "";
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPhnCrdPinNo_ItemDataBound(Object sender, GridItemEventArgs e)
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


            DataRowView drv1 = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                //if (drv["FLDCONFIRMEDYN"].ToString() == "1") db.Visible = false;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton ab = (LinkButton)e.Item.FindControl("cmdApprove");
            if (ab != null)
            {
                ab.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to confirm Issue?')");
                ab.Visible = SessionUtil.CanAccess(this.ViewState, ab.CommandName);
                if (drv["FLDCONFIRMEDYN"].ToString() == "1") ab.Visible = false;
            }
            UserControls_UserControlPhoneCard phncard = ((UserControls_UserControlPhoneCard)e.Item.FindControl("ddlPhoneCardEdit"));

            if (phncard != null) phncard.SelectedValue = drv["FLDSTOREITEMID"].ToString();

            UserControlVesselEmployee emp2 = ((UserControlVesselEmployee)e.Item.FindControl("ddlEmployeeEdit"));
            if (emp2 != null)
            {
                emp2.bind();
                emp2.SelectedEmployee = drv["FLDEMPLOYEEID"].ToString();
            }
        }

    }
    private bool IsValidRequestUpdate(string empid, string Issuedate)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (String.IsNullOrEmpty(empid))
        {
            ucError.ErrorMessage = "Employee is required";
        }
        if (String.IsNullOrEmpty(Issuedate))
        {
            ucError.ErrorMessage = "Issued On is required";
        }
        return (!ucError.IsError);
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