using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CommonPickListBudget : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        //toolbarmain.AddButton("Budget", "BUDGET", ToolBarDirection.Right);
        MenuBudget.MenuList = toolbarmain.Show();
        //MenuBudget.SetTrigger(pnlBudget);
        
        if (!IsPostBack)
        {
            confirm.Attributes.Add("style", "display:none");

            if (Request.QueryString["hardtypecode"] != null)
                ucBudgetGroup.HardTypeCode = Request.QueryString["hardtypecode"].ToString();      
            if (Request.QueryString["budgetgroup"] != null)
                ucBudgetGroup.SelectedHard = Request.QueryString["budgetgroup"].ToString();
            if (Request.QueryString["budgetdate"] != null)
                ViewState["budgetdate"] = Request.QueryString["budgetdate"].ToString();
            else
                ViewState["budgetdate"] = DateTime.Now.Date;  
 
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvBudget.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
                gvBudget.SelectedIndexes.Clear();
                gvBudget.EditIndexes.Clear();
                gvBudget.DataSource = null;
                gvBudget.Rebind();
            }
            //if (CommandName.ToUpper().Equals("BUDGET"))
            //{
            //    Response.Redirect("../Common/CommonBudgetGroupAllocationViewOnly.aspx?vesselid=" + Request.QueryString["vesselid"].ToString()
            //        + "&budgetgroup=" + Request.QueryString["budgetgroup"].ToString()
            //        + "&hardtypecode=" + Request.QueryString["hardtypecode"].ToString()
            //        + "&budgetdate=" + Request.QueryString["budgetdate"].ToString());
            //}
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
            decimal budgetamount = 0.00M;
            decimal committedamount = 0.00M;
            decimal paidamount = 0.00M;
            decimal variance = 0.00M;

            DataSet ds;
            string budgetgroup=(Request.QueryString["budgetgroup"]!=null)? Request.QueryString["budgetgroup"].ToString():null;
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
                gvBudget.CurrentPageIndex + 1,
                gvBudget.PageSize,
                ref iRowCount,
                ref iTotalPageCount,
                ref budgetamount,
                ref committedamount,
                ref paidamount,
                ref variance);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvBudget.DataSource = ds;
                gvBudget.VirtualItemCount = iRowCount;

                txtBudgetAmount.Text = String.Format("{0:##,###,###,##0.00}", budgetamount);
                txtCommittedAmount.Text = String.Format("{0:##,###,###,##0.00}", committedamount);
                txtPaidAmount.Text = String.Format("{0:##,###,###,##0.00}", paidamount);
                txtVariance.Text = String.Format("{0:##,###,###,##0.00}", variance);
            }
            else
            {
                DataTable dt = ds.Tables[0];
                txtBudgetAmount.Text = "0.00";
                txtCommittedAmount.Text = "0.00";
                txtPaidAmount.Text = "0.00";
                txtVariance.Text = "0.00";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBudget_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("PICKLIST"))
        {
            string Script = "";
            NameValueCollection nvc;

            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                    Script += "fnReloadList('codehelp1','ifMoreInfo',true);";
                else
                    Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                LinkButton lb = (LinkButton)e.Item.FindControl("lnkBudget");
                nvc.Add(lb.ID, lb.Text.ToString());
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblDescription");
                nvc.Add(lbl.ID, lbl.Text);
                RadLabel lblBudgetId = (RadLabel)e.Item.FindControl("lblBudgetId");
                nvc.Add(lblBudgetId.ID, lblBudgetId.Text);
                RadLabel lblBudgetGroupId = (RadLabel)e.Item.FindControl("lblBudgetGroupId");
                nvc.Add(lblBudgetGroupId.ID, lblBudgetGroupId.Text);
            }
            else
            {
                decimal variance = 0.00M;

                nvc = Filter.CurrentPickListSelection;

                LinkButton lb = (LinkButton)e.Item.FindControl("lnkBudget");
                nvc.Set(nvc.GetKey(1), lb.Text.ToString());
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblDescription");
                nvc.Set(nvc.GetKey(2), lbl.Text);
                RadLabel lblBudgetId = (RadLabel)e.Item.FindControl("lblBudgetId");
                nvc.Set(nvc.GetKey(3), lblBudgetId.Text);
                RadLabel lblBudgetGroupId = (RadLabel)e.Item.FindControl("lblBudgetGroupId");
                nvc.Set(nvc.GetKey(4), lblBudgetGroupId.Text);

                if (decimal.TryParse(txtVariance.Text, out variance))
                {
                    if (variance <= 0 && Request.QueryString["isvalidate"] == null)
                    {
                        RadWindowManager1.RadConfirm("Insufficient Budget. Do you wish to Continue ?", "confirm", 320, 150, null, "Confirm");

                        gvBudget.DataSource = null;
                        gvBudget.Rebind();
                        return;
                    }
                }

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                    Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
                else
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
        }
    }

    protected void CloseWindow(object sender, EventArgs e)
    {
            string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
            if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
            else
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";
            RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);        
    }
    protected void gvBudget_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
}
