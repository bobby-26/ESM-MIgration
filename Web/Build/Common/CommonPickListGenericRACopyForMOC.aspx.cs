using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class CommonPickListGenericRACopyForMOC : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
            MenuPortAgent.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["type"] = "3";

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["COMPANYID"] = "";
                ViewState["ComponentId"] = "";
                ViewState["JobId"] = "";
                ViewState["RaId"] = "";
                ViewState["WorkorderId"] = "";

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }
                ViewState["STATUS"] = Request.QueryString["status"];
                BindCategory();
            }
            if (Request.QueryString["vesselid"] != null)
                ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();

            if (Request.QueryString["MOCID"] != null)
                ViewState["MOCID"] = Request.QueryString["MOCID"].ToString();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRA_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRA.CurrentPageIndex + 1;
            BindData();
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
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        
            DataTable dt = PhoenixInspectionDailyWorkPlanActivity.RiskAssessmentGenericList(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                        , General.GetNullableString(null)
                                                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                        , General.ShowRecords(null)
                                                        , ref iRowCount
                                                        , ref iTotalPageCount
                                                        , txtActivity.Text
                                                        , General.GetNullableInteger(ddlCategory.SelectedValue)
                                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));


                gvRA.DataSource = dt;
                gvRA.VirtualItemCount = iRowCount;
                ViewState["ROWCOUNT"] = iRowCount;



        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvRA_RowCommand(object sender, GridCommandEventArgs e)
    {
        Guid Raid = new Guid();
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        string Script = "";
        NameValueCollection nvc;
        nvc = Filter.CurrentPickListSelection;
        try
        {
            RadLabel lblRAId = (RadLabel)e.Item.FindControl("lblRAId");

            PhoenixInspectionMOC.RaTemplateCopyForMOCJob(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , new Guid(lblRAId.Text)
                                                                            , ref Raid
                                                                            , General.GetNullableGuid(ViewState["MOCID"].ToString()));

            if (ViewState["MOCID"] != null && ViewState["MOCID"].ToString() != "")
            {
                PhoenixInspectionRiskAssessmentGenericExtn.updateInspectionMOCRiskAssessmentGeneric(new Guid(ViewState["MOCID"].ToString())
                                                                           , Raid);
            }

            DataTable dt = PhoenixInspectionRiskAssessmentGenericExtn.GenericRaEdit(Raid);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc.Set(nvc.GetKey(1), dr["FLDRAREFNO"].ToString());
                nvc.Set(nvc.GetKey(2), dr["FLDRISKASSESSMENT"].ToString());
                nvc.Set(nvc.GetKey(3), dr["FLDRISKASSESSMENTGENERICID"].ToString());
                nvc.Set(nvc.GetKey(4), dr["FLDTYPE"].ToString());


            }
            Filter.CurrentPickListSelection = nvc;
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvRA_RowDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            
            //if (rblType.SelectedValue != "1")
            //{
            //    e.Item.Cells[3].Visible = false;
            //}
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

   
    protected void BindCategory()
    {
        DataSet ds = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentActivity(null);

        ddlCategory.DataSource = ds;
        ddlCategory.DataTextField = "FLDNAME";
        ddlCategory.DataValueField = "FLDACTIVITYID";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
