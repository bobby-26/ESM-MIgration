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
using System.Web.Profile;
using Telerik.Web.UI;
using System.Collections;
using SouthNests.Phoenix.Common;
public partial class Accounts_AccountsAllotmentRequestList : System.Web.UI.Page
{
    string header = "Please provide the following required information", error = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Accounts/AccountsAllotmentRequestList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAllotmentList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            toolbar.AddFontAwesomeButton("../Accounts/AccountsAllotmentRequestList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Accounts/AccountsAllotmentRequestList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");


            MenuAllotmentList.AccessRights = this.ViewState;
            MenuAllotmentList.MenuList = toolbar.Show();


            if (!IsPostBack)
            {
                ddlVessel.SelectedVessel = String.Empty;


                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvAllotmentList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        ViewState["PAGENUMBER"] = 1;
        gvAllotmentList.Rebind();

    }
    protected void ddlvessel_textchangedevent(object sender, System.EventArgs e)
    {
        gvAllotmentList.Rebind();
    }


    protected void MenuAllotmentList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {

                string vessel = ddlVessel.SelectedVessel;
                string month = ddlMonth.SelectedMonth;
                string year = ddlyear.SelectedYearinstr;

                if (IsValidRequestList(vessel, month, year, ref header, ref error))
                {
                    RadWindowManager1.RadAlert(error, 400, 175, header, null);
                    // ucError.Visible = true;
                    return;
                }

                PhoenixAccountsAllotment.AccountsAllotmentRequestRefresh(int.Parse(ddlVessel.SelectedVessel)
                                                                              , int.Parse(ddlMonth.SelectedMonth)
                                                                              , int.Parse(ddlyear.SelectedYearinstr)
                                                                                );


                gvAllotmentList.CurrentPageIndex = 0;
                gvAllotmentList.Rebind();

            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlVessel.SelectedVessel = "";
                ddlMonth.SelectedMonth = "";
                ddlyear.SelectedYearinstr = "";
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

    private bool IsValidRequestList(string vessel, string month, string year, ref string headermessage, ref string errormessage)
    {

        errormessage = "";


        if ((General.GetNullableInteger(vessel) == null))
            errormessage = errormessage + "vessel is required.</br>";


        if ((General.GetNullableInteger(month) == null))
            errormessage = errormessage + "month is required.</br>";

        if (string.IsNullOrEmpty(year))
            errormessage = errormessage + "year is required.</br>";

        if (!errormessage.Length.Equals(0))
        {
            return true;
        }
        else
            return false;


    }

    protected void ShowExcel()
    {
        try
        {

            string[] alColumns = { "FLDRANKCODE", "FLDEMPLOYEENAME", "FLDBANKACCOUNTNUMBER", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDALLOTMENTTYPENAME", "FLDYEAR", "FLDFINALBALANCE" };
            string[] alCaptions = { "Rank", "Name", "Bank Account", "Currency", "Amount", "Type", "Year", "Balance(USD)" };

            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            DataSet ds = PhoenixAccountsAllotment.AccountsAllotmentRequestSearch(
                                                                              // General.GetNullableInteger(ddlVessel.SelectedVessel) == null ? 0 : ddlVessel.SelectedValue
                                                                              int.Parse(ddlVessel.SelectedVessel)
                                                                              , int.Parse(ddlMonth.SelectedMonth)
                                                                              , int.Parse(ddlyear.SelectedYearinstr)
                                                                                );

            if (ds.Tables.Count > 0)
                General.ShowExcel("AllotmentRequestList", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

            Response.AddHeader("Content-Disposition", "attachment; filename=Allotment_List" + ".xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
            Response.Write("<h3><center>Allotment List</center></h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {

        string[] alColumns = { "FLDRANKCODE", "FLDEMPLOYEENAME", "FLDBANKACCOUNTNUMBER", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDALLOTMENTTYPENAME", "FLDYEAR", "FLDFINALBALANCE" };
        string[] alCaptions = { "Rank", "Name", "Bank Account", "Currency", "Amount", "Type", "Year", "Balance(USD)" };

        DataSet ds = PhoenixAccountsAllotment.AccountsAllotmentRequestSearch(
                                                                       // General.GetNullableInteger(ddlVessel.SelectedVessel) == null ? 0 : ddlVessel.SelectedValue
                                                                       General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                       , General.GetNullableInteger(ddlMonth.SelectedMonth)
                                                                       , General.GetNullableInteger(ddlyear.SelectedYearinstr)
                                                                        );

        General.SetPrintOptions("gvAllotmentList", "AllotmentList", alCaptions, alColumns, ds);
        gvAllotmentList.DataSource = ds;
        gvAllotmentList.VirtualItemCount = ds.Tables[0].Rows.Count;

    }

    protected void Rebind()
    {
        gvAllotmentList.SelectedIndexes.Clear();
        gvAllotmentList.EditIndexes.Clear();
        //gvAllotmentList.DataSource = null;
        gvAllotmentList.Rebind();
    }

    protected void gvAllotmentList_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ViewState["LockedYN"] = ((RadLabel)e.Item.FindControl("lblLockedYN")).Text;

            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)

                cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            LinkButton cmdGenerateAllotment = (LinkButton)e.Item.FindControl("cmdGenerateAllotment");
            if (cmdGenerateAllotment != null)

                cmdGenerateAllotment.Visible = SessionUtil.CanAccess(this.ViewState, cmdGenerateAllotment.CommandName);

            {
                if (ViewState["LockedYN"].ToString() == "1")
                {
                    cmdEdit.Visible = false;
                    cmdGenerateAllotment.Visible = false;
                }


            }

        }
        if (e.Item is GridEditableItem)
        {


            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }


        if (e.Item.IsInEditMode)
        {
            UserControlMaskNumber Amount = (UserControlMaskNumber)e.Item.FindControl("txtAmountEdit");
            RadComboBox Type = (RadComboBox)e.Item.FindControl("ddlType");
            string AllotmentType = ((RadLabel)e.Item.FindControl("lblAllotmentType")).Text;
            UserControlCurrency ddlCurrency = (UserControlCurrency)e.Item.FindControl("ddlCurrencyEdit");
            string Currency = ((RadLabel)e.Item.FindControl("lblCurrencyEdit")).Text;

            if (ddlCurrency != null)
            {
                ddlCurrency.SelectedCurrency = Currency;
            }
            if (Type != null)
            {
                Type.SelectedValue = AllotmentType;
            }

        }

    }

    protected void gvAllotmentList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            //ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAllotmentList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAllotmentList_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("GENERATEALLOTMENT"))
            {
                ViewState["RequestId"] = (e.Item as GridDataItem).GetDataKeyValue("FLDALLOTMENTREQUESTID").ToString();
                ViewState["SignOffId"] = ((RadLabel)e.Item.FindControl("lblSignOnOffId")).Text;
                ViewState["EmployeeId"] = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
                ViewState["VesselId"] = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                ViewState["month"] = ddlMonth.SelectedMonth;
                ViewState["year"] = ddlyear.SelectedYearinstr;
                string amount = ((RadLabel)e.Item.FindControl("lblAmount")).Text;
                string currency = ((RadLabel)e.Item.FindControl("lblCurrency")).Text;
                ViewState["Type"] = ((RadLabel)e.Item.FindControl("lblTypeValue")).Text;

                if (IsValidAllotmentList(amount, currency, ref header, ref error))
                {
                    RadWindowManager1.RadAlert(error, 400, 175, header, null);
                    // ucError.Visible = true;
                    return;
                }

                RadWindowManager1.RadConfirm("Are you sure you want to generate allotment?", "DeleteRecord", 320, 150, null, "generate");
                return;

            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                // string AllotmentRequestId = ((RadLabel)e.Item.FindControl("lblAllotmentRequestId")).ToString();
                string AllotmentRequestId = (e.Item as GridDataItem).GetDataKeyValue("FLDALLOTMENTREQUESTID").ToString();
                UserControlCurrency ucCurrecy = ((UserControlCurrency)e.Item.FindControl("ddlCurrencyEdit"));
                string currency = ucCurrecy.SelectedCurrency;
                string amount = ((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit")).Text;
                string ddlType = ((RadComboBox)e.Item.FindControl("ddlType")).SelectedValue;

                if (IsValidAllotmentList(amount, currency, ref header, ref error))
                {
                    RadWindowManager1.RadAlert(error, 400, 175, header, null);
                    //  ucError.Visible = true;
                    e.Canceled = true;
                    return;
                }


                PhoenixAccountsAllotment.UpdateAllotmentRequest(new Guid(AllotmentRequestId), int.Parse(currency), decimal.Parse(amount), General.GetNullableInteger(ddlType));
                Rebind();
                ucStatus.Text = "Information updated";
            }

            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
                //Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {

            PhoenixAccountsAllotment.AllotmentRequestGenerate(new Guid(ViewState["RequestId"].ToString()), int.Parse(ViewState["SignOffId"].ToString()),
                                                             int.Parse(ViewState["EmployeeId"].ToString()), int.Parse(ViewState["VesselId"].ToString()), int.Parse(ViewState["year"].ToString()), int.Parse(ViewState["month"].ToString()),
                                                             General.GetNullableInteger(ViewState["Type"].ToString()));

            Rebind();
            ucStatus.Text = "Allotment Generated";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidAllotmentList(string amount, string currency, ref string headermessage, ref string errormessage)
    {
        errormessage = "";


        if (string.IsNullOrEmpty(amount))
            errormessage = errormessage + "Amount is required.</br>";


        if ((General.GetNullableInteger(currency) == null))
            errormessage = errormessage + "currency is required.</br>";

        // return (!ucError.IsError);
        if (!errormessage.Length.Equals(0))
        {
            return true;
        }
        else
            return false;
    }

    protected void cmdCalculate_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string vessel = ddlVessel.SelectedVessel;
            string month = ddlMonth.SelectedMonth;
            string year = ddlyear.SelectedYearinstr;
            string date = txtDate.Text;

            if (IsValidAllotmentRequestList(vessel, month, year, date, ref header, ref error))
            {
                RadWindowManager1.RadAlert(error, 400, 175, header, null);
                // ucError.Visible = true;
                return;
            }
            PhoenixAccountsAllotment.AccountsAllotmentRequestBOWcalculation(int.Parse(ddlVessel.SelectedVessel)
                                                                       , int.Parse(ddlMonth.SelectedMonth)
                                                                       , int.Parse(ddlyear.SelectedYearinstr)
                                                                       , DateTime.Parse(txtDate.Text)
                                                                        );

            gvAllotmentList.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidAllotmentRequestList(string vessel, string month, string year, string date, ref string headermessage, ref string errormessage)
    {

        errormessage = "";


        if ((General.GetNullableInteger(vessel) == null))
            errormessage = errormessage + "vessel is required.</br>";


        if ((General.GetNullableInteger(month) == null))
            errormessage = errormessage + "month is required.</br>";

        if (string.IsNullOrEmpty(year))
            errormessage = errormessage + "year is required.</br>";

        if (string.IsNullOrEmpty(date))
            errormessage = errormessage + "BOW calculation date is required.</br>";

        if (!errormessage.Length.Equals(0))
        {
            return true;
        }
        else
            return false;

        //  return (!ucError.IsError);
    }
}