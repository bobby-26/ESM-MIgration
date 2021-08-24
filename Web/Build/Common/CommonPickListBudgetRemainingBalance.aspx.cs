using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using  SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class CommonPickListBudgetRemainingBalance : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        //toolbarmain.AddButton("Budget", "BUDGET");
        toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        MenuBudget.MenuList = toolbarmain.Show();
        
        if (!IsPostBack)
        {
            rgvBudget.PageSize = General.ShowRecords(null);
            if (Request.QueryString["hardtypecode"] != null)
                ucBudgetGroup.HardTypeCode = Request.QueryString["hardtypecode"].ToString();      
            if (Request.QueryString["budgetgroup"] != null)
                ucBudgetGroup.SelectedHard = Request.QueryString["budgetgroup"].ToString();
            if (Request.QueryString["budgetdate"] != null)
                ViewState["budgetdate"] = Request.QueryString["budgetdate"].ToString();
            else
                ViewState["budgetdate"] = DateTime.Now.Date;

            if (Request.QueryString["framename"] != null)
            {
                ViewState["framename"] = Request.QueryString["framename"].ToString();
            }
            if (Request.QueryString["windowname"] != null)
                ViewState["windowname"] = Request.QueryString["windowname"].ToString();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            RemainingBalanceEdit();
        }
        
    }

    protected void MenuBudget_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                rgvBudget.Rebind();
                RemainingBalanceEdit();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void RemainingBalanceEdit()
    {
        DataSet ds = new DataSet();

        ds = PhoenixCommonRegisters.BudgetGroupRemainingBalanceEdit(General.GetNullableInteger(Request.QueryString["vesselid"])
                                                                    , General.GetNullableDateTime(ViewState["budgetdate"].ToString())
                                                                    , General.GetNullableInteger((ucBudgetGroup.SelectedHard == "")? Request.QueryString["budgetgroup"].ToString(): ucBudgetGroup.SelectedHard)
                                                                    );

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtBudgetAmount.Text = String.Format("{0:##,###,###,##0.00}", ds.Tables[0].Rows[0]["FLDBUDGETPERMONTH"].ToString());
            txtCommittedAmount.Text = String.Format("{0:##,###,###,##0.00}", ds.Tables[0].Rows[0]["FLDCOMMITTEDAMOUNT"].ToString());
            txtPaidAmount.Text = String.Format("{0:##,###,###,##0.00}", ds.Tables[0].Rows[0]["FLDACTUALAMOUNT"].ToString());
            txtVariance.Text = String.Format("{0:##,###,###,##0.00}", ds.Tables[0].Rows[0]["FLDREMAININGBUDGET"].ToString());
            txtApprovedNotOrdered.Text = String.Format("{0:##,###,###,##0.00}", ds.Tables[0].Rows[0]["FLDYETTOORDERAMOUNT"].ToString());
        }
        else
        {
            txtBudgetAmount.Text = "";
            txtCommittedAmount.Text = "";
            txtPaidAmount.Text = "";
            txtVariance.Text = "";
            txtApprovedNotOrdered.Text = "";
        }

    }
    protected void rgvBudget_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        decimal budgetamount = 0.00M;
        decimal committedamount = 0.00M;
        decimal paidamount = 0.00M;
        decimal variance = 0.00M;

        DataSet ds;
        string budgetgroup = (Request.QueryString["budgetgroup"] != null) ? Request.QueryString["budgetgroup"].ToString() : null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCommonRegisters.BudgetSearch(
            1
            , txtBudgetSearch.Text
            , txtDescriptionNameSearch.Text
            , null
            , General.GetNullableInteger((ucBudgetGroup.SelectedHard == "") ? budgetgroup : ucBudgetGroup.SelectedHard)
            , General.GetNullableDateTime(ViewState["budgetdate"].ToString())  // date
            , General.GetNullableInteger(Request.QueryString["vesselid"])// vessel
            , sortexpression, sortdirection,
            rgvBudget.CurrentPageIndex+1,
            rgvBudget.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            ref budgetamount,
            ref committedamount,
            ref paidamount,
            ref variance);

        rgvBudget.DataSource = ds;
        rgvBudget.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void rgvBudget_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        
        if(e.CommandName.ToUpper().Equals("SELECT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            string Script = "";
            NameValueCollection nvc;

            if (Request.QueryString["POPUP"] != null)
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (ViewState["framename"] != null)
                    Script += "populateTelerikPick('" + ViewState["windowname"].ToString() + "','" + ViewState["framename"].ToString() + "','codehelp1');";
                else
                    Script += "fnClosePickList('codehelp1');";
                Script += "</script>" + "\n";

                decimal variance = 0.00M;

                nvc = Filter.CurrentPickListSelection;

                LinkButton lb = (LinkButton)item["CODE"].FindControl("lnkBudget");
                nvc.Set(nvc.GetKey(1), lb.Text.ToString());
                RadLabel lbl = (RadLabel)item["DESCRIPTION"].FindControl("lblDescription");
                nvc.Set(nvc.GetKey(2), lbl.Text);
                RadLabel lblBudgetId = (RadLabel)item["CODE"].FindControl("lblBudgetId");
                nvc.Set(nvc.GetKey(3), lblBudgetId.Text);
                RadLabel lblBudgetGroupId = (RadLabel)item["GROUP"].FindControl("lblBudgetGroupId");
                nvc.Set(nvc.GetKey(4), lblBudgetGroupId.Text);

                Filter.CurrentSelectedESMBudgetCode = lblBudgetId.Text;

                if (decimal.TryParse(txtVariance.Text, out variance))
                {
                    RadWindowManager1.RadConfirm("Insufficient Budget. Do you want to Continue ?", "confirmCallbackFn", 300, 150, null, "Confirm");
                    return;
                }
            }
            else if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                    Script += "fnReloadList('codehelp1','ifMoreInfo',true);";
                else
                    Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                LinkButton lb = (LinkButton)item["CODE"].FindControl("lnkBudget");
                nvc.Add(lb.ID, lb.Text.ToString());
                RadLabel lbl = (RadLabel)item["DESCRIPTION"].FindControl("lblDescription");
                nvc.Add(lbl.ID, lbl.Text);
                RadLabel lblBudgetId = (RadLabel)item["CODE"].FindControl("lblBudgetId");
                nvc.Add(lblBudgetId.ID, lblBudgetId.Text);
                RadLabel lblBudgetGroupId = (RadLabel)item["GROUP"].FindControl("lblBudgetGroupId");
                nvc.Add(lblBudgetGroupId.ID, lblBudgetGroupId.Text);

                Filter.CurrentSelectedESMBudgetCode = lblBudgetId.Text;
            }
            else
            {
                decimal variance = 0.00M;

                nvc = Filter.CurrentPickListSelection;

                LinkButton lb = (LinkButton)item["CODE"].FindControl("lnkBudget");
                nvc.Set(nvc.GetKey(1), lb.Text.ToString());
                RadLabel lbl = (RadLabel)item["DESCRIPTION"].FindControl("lblDescription");
                nvc.Set(nvc.GetKey(2), lbl.Text);
                RadLabel lblBudgetId = (RadLabel)item["CODE"].FindControl("lblBudgetId");
                nvc.Set(nvc.GetKey(3), lblBudgetId.Text);
                RadLabel lblBudgetGroupId = (RadLabel)item["GROUP"].FindControl("lblBudgetGroupId");
                nvc.Set(nvc.GetKey(4), lblBudgetGroupId.Text);

                Filter.CurrentSelectedESMBudgetCode = lblBudgetId.Text;

                if (decimal.TryParse(txtVariance.Text, out variance))
                {
                    RadWindowManager1.RadConfirm("Insufficient Budget. Do you want to Continue ?", "confirmCallbackFn", 300, 150, null, "Confirm");
                    return;
                    //if (variance <= 0 && Request.QueryString["isvalidate"] == null)
                    //{
                    //    ucConfirm.HeaderMessage = "Please Confirm";
                    //    ucConfirm.Text = "Insufficient Budget Continue ?";
                    //    ucConfirm.Visible = true;
                    //    ucConfirm.CancelText = "No";
                    //    ucConfirm.OKText = "Yes";
                    //    return;
                    //}
                }
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                    Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
                else
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
            }
            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page,typeof(Page), "BookMarkScript", Script,false);
        }
        
    }

    protected void CloseWindow(object sender, EventArgs e)
    {
        string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
        if (Request.QueryString["POPUP"] != null)
        {
            
            if (ViewState["framename"] != null)
                Script += "populateTelerikPick('" + ViewState["windowname"].ToString() + "','" + ViewState["framename"].ToString() + "','codehelp1');";
            else
                Script += "fnClosePickList('codehelp1');";
            Script += "</script>" + "\n";
        }
        else
        {
            if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
            else
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";
        }
        
        RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);

        //if (sender != null && ((UserControlConfirmMessage)sender).confirmboxvalue == 1)
        //{
            
        //}
    }


    protected void ucBudgetGroup_TextChangedEvent(object sender, EventArgs e)
    {
        rgvBudget.Rebind();
        RemainingBalanceEdit();
    }
}
