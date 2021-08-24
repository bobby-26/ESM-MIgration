using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Web.Profile;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class CommonPickListJHA : PhoenixBasePage
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
            MenuPortAgent.MenuList = toolbarmain.Show();


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();
                }

                BindCategory();

                gvPortAgent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);


                if (Request.QueryString["vesselid"] != null)
                {
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
                    ucVessel.Company = ViewState["COMPANYID"].ToString();
                    ucVessel.bind();
                    ucVessel.DataBind();
                    ucVessel.SelectedVessel = ViewState["VESSELID"].ToString();
                    ucVessel.Enabled = false;
                    ucVessel.Visible = false;
                }
            }

            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPortAgent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvPortAgent.Rebind();
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
        BindData();
        gvPortAgent.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;        
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentJobHazard.RiskAssessmentJobHazardSearch(
                    null,
                    null,
                    General.GetNullableInteger(ddlCategory.SelectedValue),
                    null,
                    null,
                    null, null,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvPortAgent.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount,
                    General.GetNullableInteger(ucVessel.SelectedVessel),
                    txtJob.Text,
                    General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        gvPortAgent.DataSource = ds;
        gvPortAgent.VirtualItemCount = iRowCount;
    

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
       
    }

 
   

 

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvPortAgent.Rebind();
       
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void BindCategory()
    {
        ddlCategory.Items.Add(new RadComboBoxItem("--Select--", "DUMMY"));
        ddlCategory.DataSource = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(5,General.GetNullableInteger(ViewState["COMPANYID"].ToString())); 
        ddlCategory.DataBind();
       
    }

    protected void gvPortAgent_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPortAgent.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPortAgent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

   
        string Script = "";
        NameValueCollection nvc;
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

        if (e.CommandName == "Select")
        {
            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                RadLabel lblRefNo = (RadLabel)e.Item.FindControl("lblRefNo");
                nvc.Add(lblRefNo.ID, lblRefNo.Text);
                RadLabel lblJob = (RadLabel)e.Item.FindControl("lblJob");
                nvc.Add(lblJob.ID, lblJob.Text);
                RadLabel lblJobHazardid = (RadLabel)e.Item.FindControl("lblJobHazardid");
                nvc.Add(lblJobHazardid.ID, lblJobHazardid.Text);
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
                nvc = new NameValueCollection();
                nvc = Filter.CurrentPickListSelection;

                RadLabel lblRefNo = (RadLabel)e.Item.FindControl("lblRefNo");
                nvc.Set(nvc.GetKey(1), lblRefNo.Text);

                RadLabel lblJob = (RadLabel)e.Item.FindControl("lblJob");
                nvc.Set(nvc.GetKey(2), lblJob.Text);

                RadLabel lblJobHazardid = (RadLabel)e.Item.FindControl("lblJobHazardid");
                nvc.Set(nvc.GetKey(3), lblJobHazardid.Text);
            }

            Filter.CurrentPickListSelection = nvc;
        }
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }

    protected void gvPortAgent_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvPortAgent_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void ddlCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvPortAgent.Rebind();
    }
}
