using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Text;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class CrewCostEvaluationFinalPortCostAnalysis : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            PhoenixToolbar toolbar = new PhoenixToolbar();        
            toolbar.AddButton("Request", "REQUEST");
            toolbar.AddButton("Port Cost Details", "DETAILS");
            toolbar.AddButton("Port Cost Analysis", "ANALYSYS");

            MenuCrewCost.AccessRights = this.ViewState;
            MenuCrewCost.MenuList = toolbar.Show();
            MenuCrewCost.SelectedMenuIndex = 2;
            
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);

                if (Request.QueryString["REQUESTID"] != null)
                    ViewState["REQUESTID"] = Request.QueryString["REQUESTID"].ToString();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["TOTALPAGECOUNT"] = null;

                SetPortCostAnalysisInformation();

                gvAnalysis.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPortCostAnalysisInformation()
    {
        DataTable dtReq = PhoenixCrewCostEvaluationRequest.EditCrewCostEvaluationRequest(new Guid(ViewState["REQUESTID"].ToString()));
        if (dtReq.Rows.Count > 0)
        {
            DataRow dr = dtReq.Rows[0];
            txtRequestNo.Text = dr["FLDREQUESTNO"].ToString();
            txtCrewChangeDate.Text = dr["FLDCREWCHANGEDATE"].ToString();
            txtVessel.Text = dr["FLDVESSELNAME"].ToString();
            txtPortName.Text = dr["FLDSEAPORTNAME"].ToString();
            txtOffSigners.Text = dr["FLDNOOFOFFSIGNER"].ToString();
            txtOnSigners.Text = dr["FLDNOOFJOINER"].ToString();
        }
        DataTable dt = PhoenixCrewCostEvaluationQuote.EditCrewPortCostAnalysisInformation(new Guid(ViewState["REQUESTID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtQtnNo.Text = dr["FLDQUOTEREFNO"].ToString();
            txtETA.Text = dr["FLDETA"].ToString();
            txtETD.Text = dr["FLDETD"].ToString();
        }
    }

    protected void MenuCrewCost_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string requestid = null;

            if (ViewState["REQUESTID"] != null)
                requestid = ViewState["REQUESTID"].ToString();

            if (CommandName.ToUpper().Equals("REQUEST"))
            {
                Response.Redirect("../Crew/CrewCostEvaluationRequest.aspx?requestid=" + ViewState["REQUESTID"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("DETAILS"))
            {
                Response.Redirect("../Crew/CrewCostEvaluationRequestGeneral.aspx?requestid=" + ViewState["REQUESTID"].ToString(), false);
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
        string[] alColumns = { "FLDSECTIONTYPE", "FLDSECTION", "FLDAMOUNT", "FLDACTUALAMOUNT", "FLDBUDGETCODE", "FLDCHECKEDYN", "FLDCHECKINGREMARKS" };
        string[] alCaptions = { "Section Type", "Section", "Amount", "Actual Amount", "Budget Code", "Check", "Remarks" };
        string requestid = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["REQUESTID"] != null)
            requestid = ViewState["REQUESTID"].ToString();

        DataSet ds = PhoenixCrewCostEvaluationQuote.CrewCostQuoteLineItemAnalysisSearch(new Guid(requestid)
                                                    , (int)ViewState["PAGENUMBER"]
                                                    , gvAnalysis.PageSize
                                                    , ref iRowCount
                                                    , ref iTotalPageCount);

        gvAnalysis.DataSource = ds;
        gvAnalysis.VirtualItemCount = iRowCount;

        General.SetPrintOptions("gvAnalysis", "Port Cost Analysis", alCaptions, alColumns, ds);

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvAnalysis_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAnalysis.CurrentPageIndex + 1;

        BindData();
    }


    protected void gvAnalysis_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
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

    protected void gvAnalysis_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string lineitemid = ((RadLabel)e.Item.FindControl("lblLineItemIdEdit")).Text;
            UserControlMaskNumber ucactualamount = (UserControlMaskNumber)e.Item.FindControl("txtActualAmountEdit");
            string actualamount = ucactualamount.Text;
            string budgetid = ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text;
            string quoteid = ((RadLabel)e.Item.FindControl("lblQuoteId")).Text;
            RadCheckBox chkCheckedYN = (RadCheckBox)e.Item.FindControl("chkCheckEdit");
            string checkingremarks = ((RadTextBox)e.Item.FindControl("txtCheckingRemarksEdit")).Text.ToString();

            if (!IsValidCheck(ucactualamount.Text, budgetid))
            {
                ucError.Visible = true;
                return;
            }
            else
            {
                PhoenixCrewCostEvaluationQuote.UpdateCostQuoteLineItemActuals(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["REQUESTID"].ToString())
                    , new Guid(quoteid)
                    , new Guid(lineitemid)
                    , Convert.ToDecimal(actualamount)
                    , Convert.ToInt16(budgetid)
                    , General.GetNullableInteger(chkCheckedYN.Checked == true ? "1" : "0")
                    , General.GetNullableString(checkingremarks));
            }

            BindData();
            gvAnalysis.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private decimal TotalAmount = 0;
    private decimal ActualTotalAmount = 0;

    protected void gvAnalysis_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            decimal.TryParse(drv["FLDQUOTETOTALAMOUNT"].ToString(), out TotalAmount);
            decimal.TryParse(drv["FLDACTUALTOTALAMOUNT"].ToString(), out ActualTotalAmount);
            
        }
        if (e.Item is GridEditableItem)
        {
            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:none");
            tb1 = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:none");
            tb1 = (RadTextBox)e.Item.FindControl("txtBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "display:none");
            LinkButton ib1 = (LinkButton)e.Item.FindControl("btnShowBudgetEdit");
            if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudget.aspx?budgetgroup=102&hardtypecode=30&vesselid='); ");
        }
        if (e.Item is GridFooterItem)
        {

            GridFooterItem footer = (GridFooterItem)e.Item;
            (footer["AMOUNT"].FindControl("lblTotalA") as RadLabel).Text = TotalAmount.ToString();
            (footer["ACTUAL"].FindControl("lblTotalActual") as RadLabel).Text = ActualTotalAmount.ToString();

        }
    }

    private bool IsValidCheck(string actualamount, string budgetid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDecimal(actualamount) == null)
            ucError.ErrorMessage = "Actual amount is required.";

        if (General.GetNullableInteger(budgetid) == null)
            ucError.ErrorMessage = "Budget code is required.";

        return (!ucError.IsError);
    }


}
