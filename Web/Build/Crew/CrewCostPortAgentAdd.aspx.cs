using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewCostPortAgentAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuAddress.AccessRights = this.ViewState;
        MenuAddress.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["REQUESTID"] = null;
            ViewState["EVALUATIONPORTID"] = null;

            if (Request.QueryString["framename"] != null)
            {
                ViewState["framename"] = Request.QueryString["framename"].ToString();
            }
            if (Request.QueryString["REQUESTID"] != null)
            {
                ViewState["REQUESTID"] = Request.QueryString["REQUESTID"].ToString();
            }
            if (Request.QueryString["EVALUATIONPORTID"] != null)
            {
                ViewState["EVALUATIONPORTID"] = Request.QueryString["EVALUATIONPORTID"].ToString();
            }

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            gvAddress.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }
    protected void MenuAddress_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvAddress.CurrentPageIndex = 0;
                BindData();
                gvAddress.Rebind();
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
            DataSet ds;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string addresstype = "1255";
            string producttype = null;
            if (ViewState["addresstype"] != null)
                addresstype = ViewState["addresstype"].ToString();
            if (ViewState["producttype"] != null)
                producttype = ViewState["producttype"].ToString();

            ds = PhoenixCommonRegisters.AddressSearch(txtCode.Text
                , txtNameSearch.Text, null, null, null, General.GetNullableString(txtCountryNameSearch.Text)
                , addresstype, producttype, null, null, null, null, null, sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvAddress.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            gvAddress.DataSource = ds;
            gvAddress.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvAddress_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAddress.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvAddress_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                Guid? quoteid = null;


                RadLabel lblAddressCode = (RadLabel)e.Item.FindControl("lblAddressCode");

                PhoenixCrewCostEvaluationQuote.InsertCrewCostEvaluationQuote(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                       , new Guid(ViewState["REQUESTID"].ToString())
                       , General.GetNullableGuid(ViewState["EVALUATIONPORTID"].ToString())
                       , General.GetNullableInteger(lblAddressCode.Text)
                       , ref quoteid);
                if (quoteid != null)
                {
                    PhoenixCrewCostEvaluationQuote.InsertCrewCostEvaluationQuoteLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["REQUESTID"].ToString())
                        , quoteid
                        , General.GetNullableGuid(ViewState["EVALUATIONPORTID"].ToString())
                        );

                    PhoenixCrewCostEvaluationQuote.InsertCrewCostQuoteSectionTotal(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                       , new Guid(ViewState["REQUESTID"].ToString())
                       , General.GetNullableGuid(ViewState["EVALUATIONPORTID"].ToString())
                       , quoteid
                       );
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1','',true);", true);
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

    protected void gvAddress_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    protected void gvAddress_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

}
