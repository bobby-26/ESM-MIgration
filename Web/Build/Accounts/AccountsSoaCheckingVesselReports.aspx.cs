using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsSoaCheckingVesselReports : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("SOA", "SOA");
            toolbarmain.AddButton("Line Items", "LINEITEMS");
            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.MenuList = toolbarmain.Show();
            MenuGeneral.SelectedMenuIndex = 1;

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Details", "DETAILS");
            toolbarsub.AddButton("Report", "REPORT");

            MenuGenralSub.AccessRights = this.ViewState;
            MenuGenralSub.MenuList = toolbarsub.Show();
            MenuGenralSub.SelectedMenuIndex = 1;

            PhoenixToolbar toolbarReport = new PhoenixToolbar();
            toolbarReport.AddButton("Show Report", "SHOW",ToolBarDirection.Right);
            MenuReports.AccessRights = this.ViewState;
            MenuReports.MenuList = toolbarReport.Show();

            if (!IsPostBack)
            {
                ViewState["debitnotereference"] = Request.QueryString["debitnotereference"].ToString();

                if (General.GetNullableGuid(Request.QueryString["debitnoteid"].ToString()) != null)
                    ViewState["debitnotereferenceid"] = Request.QueryString["debitnoteid"].ToString();
                else
                    ViewState["debitnotereferenceid"] = "";

                if (Request.QueryString["Status"] != null)
                {
                    ViewState["Status"] = Request.QueryString["Status"].ToString();
                }
                else
                {
                    ViewState["Status"] = "";
                }

                ViewState["accountid"] = Request.QueryString["accountid"].ToString();
                ViewState["Ownerid"] = Request.QueryString["Ownerid"].ToString();
                ViewState["FLDVOUCHERDETAILID"] = "";

                ddlMonth.DataBind();
                ddlMonth.Items.Insert(0, "--Select--");

                ucAsOnDate.Visible = false;
            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuGenralSub_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("DETAILS"))
            {
                Response.Redirect("../Accounts/AccountsSoaCheckingLineItems.aspx?debitnotereference=" + ViewState["debitnotereference"].ToString() + "&accountid=" + Request.QueryString["accountid"].ToString() + "&Ownerid=" + ViewState["Ownerid"].ToString() + "&VesselId=" + Request.QueryString["VesselId"].ToString() + "&debitnoteid=" + ViewState["debitnotereferenceid"] + "&Status=" + ViewState["Status"], true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SOA"))
            {
                Response.Redirect("../Accounts/AccountsSoaChecking.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuReports_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SHOW"))
            {
                if (!IsValidData())
                {
                    ucError.Visible = true;
                    return;
                }
                if (ddlType.SelectedValue == "1")
                {
                    string subreportcode = "";
                    if (ddlSubType.SelectedValue == "Accumulated")
                        subreportcode = "AVRE";
                    if (ddlSubType.SelectedValue == "Monthly")
                        subreportcode = "MVRE";
                    if (ddlSubType.SelectedValue == "Yearly")
                        subreportcode = "YVRE";
                    String scriptpopup = String.Format(
                        "javascript:parent.openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELVARIANCE&vessel=" + Request.QueryString["VesselId"].ToString() + "&type=" + ddlSubType.SelectedValue + "&AsOnDate=" + ucAsOnDate.Text + "&debitnotereference=" + ViewState["debitnotereference"].ToString() + "&debitnoteid=" + ViewState["debitnotereferenceid"]+"&ownerid=" + ViewState["Ownerid"]+"&subreportcode="+ subreportcode+"&showmenu=0');");

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                if (ddlType.SelectedValue == "2")
                {
                    String scriptpopup = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELSUMMARYBALANCE&vessel=" + Request.QueryString["VesselId"].ToString() + "&month=" + ddlMonth.SelectedValue + "&year=" + ucYear.SelectedQuick + "&debitnotereference=" + ViewState["debitnotereference"].ToString() + "&debitnoteid=" + ViewState["debitnotereferenceid"] + "&ownerid=" + ViewState["Ownerid"] + "&type=VESSELSUMMARYBALANCE&subreportcode=FDP&showmenu=0');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                if (ddlType.SelectedValue == "3")
                {
                    String scriptpopup = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELTRAILBALANCE&accountId=" + Request.QueryString["accountid"].ToString() + "&month=" + ddlMonth.SelectedValue + "&year=" + ucYear.SelectedQuick + "&debitnotereference=" + ViewState["debitnotereference"].ToString() + "&debitnoteid=" + ViewState["debitnotereferenceid"] + "&ownerid=" + ViewState["Ownerid"] + "&subreportcode=VTBA&type=VESSELTRAILBALANCE&showmenu=0');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                if (ddlType.SelectedValue == "7")
                {
                    String scriptpopup = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELTRAILBALANCEYTD&accountId=" + Request.QueryString["accountid"].ToString() + "&month=" + ddlMonth.SelectedValue + "&year=" + ucYear.SelectedQuick + "&debitnotereference=" + ViewState["debitnotereference"].ToString() + "&debitnoteid=" + ViewState["debitnotereferenceid"] + "&ownerid=" + ViewState["Ownerid"] + "&showmenu=0&subreportcode=VTBY');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                if (ddlType.SelectedValue == "8")
                {
                    String scriptpopup = String.Format(
                        "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELTRAILBALANCEYTDOWNER&accountId=" + General.GetNullableInteger(Request.QueryString["accountid"].ToString()) + "&debitnoteid=" + General.GetNullableString(ViewState["debitnotereferenceid"].ToString()) + "&ownerid=" + ViewState["Ownerid"].ToString() + "&showmenu=0&subreportcode=VTBO');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                if (ddlType.SelectedValue == "5")
                {
                    String scriptpopup = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=5&reportcode=STATEMENTOFOWNERACCOUNTSUMMARY&Debitnotereference=" + ViewState["debitnotereference"].ToString() + "&ownerid=" + ViewState["Ownerid"] + "&ownerid=" + ViewState["Ownerid"] + "&type=STATEMENTOFOWNERACCOUNTSUMMARY&showmenu=0&subreportcode=SENO');");
                   // Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?Debitnotereference=" + lblStatementReference.Text + "&applicationcode=5&reportcode=STATEMENTOFOWNERACCOUNTSUMMARY", false);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }

                if (ddlType.SelectedValue == "6")
                {
                    String scriptpopup = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=5&reportcode=STATEMENTOFACCOUNTSUMMARYESMBUDGET&Debitnotereference=" + ViewState["debitnotereference"].ToString() + "&showmenu=0&subreportcode=SENE');");
                    // Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?Debitnotereference=" + lblStatementReference.Text + "&applicationcode=5&reportcode=STATEMENTOFOWNERACCOUNTSUMMARY", false);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                if (ddlType.SelectedValue == "4")
                {
                    string subtype = "", budget = "";
                    if (ddlSubType.SelectedValue == "1")
                    {
                        subtype = "105";
                        budget = "1";
                    }
                    if (ddlSubType.SelectedValue == "2")
                    {
                        subtype = "106";
                        budget = "1";
                    }
                    if (ddlSubType.SelectedValue == "3")
                    {
                        subtype = "107";
                        budget = "1";
                    }
                    if (ddlSubType.SelectedValue == "4")
                    {
                        subtype = "105";
                        budget = "0";
                    }
                    if (ddlSubType.SelectedValue == "5")
                    {
                        subtype = "106";
                        budget = "0";
                    }
                    if (ddlSubType.SelectedValue == "6")
                    {
                        subtype = "107";
                        budget = "0";
                    }
                    String scriptpopup = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=5&reportcode=VARIANCEREPORT&vessel=" + Request.QueryString["VesselId"].ToString() +
                        "&month=" + ddlMonth.SelectedValue + "&year=" + ucYear.SelectedQuick + "&budgetgroup=" + subtype + "&budgetyn=" + budget + "&debitnotereference=" + ViewState["debitnotereference"].ToString() + "&debitnoteid=" + ViewState["debitnotereferenceid"] + "&showmenu=0');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }

                if (ddlType.SelectedValue == "9")
                {                 
                    String scriptpopup = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=5&reportcode=STATEMENTOFOWNERACCOUNTWITHOUTBUDGET&Debitnotereference=" + ViewState["debitnotereference"].ToString() + "&ownerid=" + ViewState["Ownerid"] + "&ownerid=" + ViewState["Ownerid"] + "&type=STATEMENTOFOWNERACCOUNTWITHOUTBUDGET&showmenu=0&subreportcode=SENO');");
                    // Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?Debitnotereference=" + lblStatementReference.Text + "&applicationcode=5&reportcode=STATEMENTOFOWNERACCOUNTSUMMARY", false);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlType.SelectedValue == "1")
        {
            ddlSubType.CssClass = "input";
            ddlSubType.Enabled = true;
            //ucAsOnDate.CssClass = "input";
            //ucAsOnDate.Enabled = true;

            ucYear.CssClass = "readonlytextbox"; 
            ucYear.Enabled = false;
            ddlMonth.CssClass = "readonlytextbox";
            ddlMonth.Enabled = false;

            ddlSubType.Items.Clear();
            ddlSubType.Items.Insert(0, new RadComboBoxItem("--Select--", "0"));
            ddlSubType.Items.Insert(1, new RadComboBoxItem("Accumulated Variance", "Accumulated"));
            ddlSubType.Items.Insert(2, new RadComboBoxItem("Monthly Variance", "Monthly"));
            ddlSubType.Items.Insert(3, new RadComboBoxItem("Yearly Variance", "Yearly"));
        }
        else if (ddlType.SelectedValue == "2")
        {
            ucYear.CssClass = "input";
            ucYear.Enabled = true;
            ddlMonth.CssClass = "input";
            ddlMonth.Enabled = true;

            ddlSubType.CssClass = "readonlytextbox";
            ddlSubType.Enabled = false;
            //ucAsOnDate.CssClass = "readonlytextbox";
            //ucAsOnDate.Enabled = false;
        }
        else if (ddlType.SelectedValue == "3")
        {
            ucYear.CssClass = "input";
            ucYear.Enabled = true;
            ddlMonth.CssClass = "input";
            ddlMonth.Enabled = true;

            ddlSubType.CssClass = "readonlytextbox";
            ddlSubType.Enabled = false;
            //ucAsOnDate.CssClass = "readonlytextbox";
            //ucAsOnDate.Enabled = false; 
            
            ddlSubType.Items.Clear();
            ddlSubType.Items.Insert(0, new RadComboBoxItem("--Select--", "0"));
        }
        else if (ddlType.SelectedValue == "4")
        {
            ucYear.CssClass = "input";
            ucYear.Enabled = true;
            ddlMonth.CssClass = "input";
            ddlMonth.Enabled = true;
            ddlSubType.CssClass = "input";
            ddlSubType.Enabled = true;

            //ucAsOnDate.CssClass = "readonlytextbox";
            //ucAsOnDate.Enabled = false;

            ddlSubType.Items.Clear();
            ddlSubType.Items.Insert(0, new RadComboBoxItem("--Select--", "0"));
            ddlSubType.Items.Insert(1, new RadComboBoxItem("Budgeted Stores", "1"));
            ddlSubType.Items.Insert(2, new RadComboBoxItem("Budgeted Spares", "2"));
            ddlSubType.Items.Insert(3, new RadComboBoxItem("Budgeted Repairs", "3"));
            ddlSubType.Items.Insert(4, new RadComboBoxItem("Non-Budgeted Stores", "4"));
            ddlSubType.Items.Insert(5, new RadComboBoxItem("Non-Budgeted Spares", "5"));
            ddlSubType.Items.Insert(6, new RadComboBoxItem("Non-Budgeted Repairs", "6"));
        }
        else
        {
            ucYear.CssClass = "readonlytextbox";
            ucYear.Enabled = false;
            ddlMonth.CssClass = "readonlytextbox";
            ddlMonth.Enabled = false;
            ddlSubType.CssClass = "readonlytextbox";
            ddlSubType.Enabled = false;
            //ucAsOnDate.CssClass = "readonlytextbox";
            //ucAsOnDate.Enabled = false;

            ddlSubType.Items.Clear();
            ddlSubType.Items.Insert(0, new RadComboBoxItem("--Select--", "0"));
        }
    }

    public bool IsValidData()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ddlType.SelectedValue) == null)
            ucError.ErrorMessage = "Type is required.";

        if (General.GetNullableInteger(ddlType.SelectedValue) == 1)
        {
            if (General.GetNullableInteger(ddlSubType.SelectedValue) == 0)
                ucError.ErrorMessage = "Sub type is required.";
            //if (General.GetNullableDateTime(ucAsOnDate.Text) == null)
            //    ucError.ErrorMessage = "As on date is required";
        }

        //if (General.GetNullableInteger(ddlType.SelectedValue) == 2)
        //{
        //    if (General.GetNullableInteger(ucYear.SelectedQuick) == null)
        //        ucError.ErrorMessage = "Year is required.";
        //    if (General.GetNullableInteger(ddlMonth.SelectedValue) == null)
        //        ucError.ErrorMessage = "Month is required.";
        //}

        //if (General.GetNullableInteger(ddlType.SelectedValue) == 3)
        //{
        //    if (General.GetNullableInteger(ucYear.SelectedQuick) == null)
        //        ucError.ErrorMessage = "Year is required.";
        //    if (General.GetNullableInteger(ddlMonth.SelectedValue) == null)
        //        ucError.ErrorMessage = "Month is required.";
        //}

        if (General.GetNullableInteger(ddlType.SelectedValue) == 4)
        {
            if (General.GetNullableInteger(ddlSubType.SelectedValue) == 0)
                ucError.ErrorMessage = "Sub type is required.";
            //if (General.GetNullableInteger(ucYear.SelectedQuick) == null)
            //    ucError.ErrorMessage = "Year is required.";
            //if (General.GetNullableInteger(ddlMonth.SelectedValue) == null)
            //    ucError.ErrorMessage = "Month is required.";
        }
        return (!ucError.IsError);
    }

}
