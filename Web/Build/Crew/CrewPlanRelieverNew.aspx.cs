using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Web;
using Telerik.Web.UI;
public partial class CrewPlanRelieverNew : PhoenixBasePage
{
    string strVessel = string.Empty;
    string strRankId = string.Empty;
    string strOffSigner = string.Empty;
    string strRelieverId = string.Empty;
    string strDate = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewPlanRelieverNew.aspx?empid=" + Request.QueryString["empid"] + "&vesselid=" + Request.QueryString["vesselid"] + "&rankid=" + Request.QueryString["rankid"] + "&relieverid=" + Request.QueryString["relieverid"] + "&IsTop4=" + Request.QueryString["IsTop4"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRelieverSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            RelieverMenu.AccessRights = this.ViewState;
            RelieverMenu.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["RPAGENUMBER"] = 1;
                ViewState["RSORTEXPRESSION"] = null;
                ViewState["RSORTDIRECTION"] = null;
                ViewState["RCURRENTINDEX"] = 1;
                ViewState["RROWCOUNT"] = 0;
                ViewState["RTOTALPAGECOUNT"] = 0;

                ViewState["EMPID"] = string.IsNullOrEmpty(Request.QueryString["empid"]) ? "" : Request.QueryString["empid"];
                ViewState["VESSELID"] = string.IsNullOrEmpty(Request.QueryString["vesselid"]) ? "" : Request.QueryString["vesselid"];
                ViewState["RANKID"] = string.IsNullOrEmpty(Request.QueryString["rankid"]) ? "" : Request.QueryString["rankid"];
                ViewState["RELIEFDATE"] = string.IsNullOrEmpty(Request.QueryString["reliefdate"]) ? "" : Request.QueryString["reliefdate"];
                ViewState["SELECTEDINDEX"] = string.IsNullOrEmpty(Request.QueryString["selectedindex"]) ? "" : Request.QueryString["selectedindex"];
                ViewState["ISTOP4"] = string.IsNullOrEmpty(Request.QueryString["IsTop4"]) ? "" : Request.QueryString["IsTop4"];
                ViewState["NEXTRANKEMPLOYEE"] = "";
                ViewState["Ispopup"] = string.IsNullOrEmpty(Request.QueryString["Ispopup"]) ? "": Request.QueryString["Ispopup"];
                ViewState["GROUPRANKID"] = string.IsNullOrEmpty(Request.QueryString["GroupRank"]) ? "" : Request.QueryString["GroupRank"];

                if (Request.QueryString["IsTop4"] != "1")
                {
                    gvRelieverMatrix.Visible = false;
                    lblCombinedExp.Visible = false;
                }

                SetEmployeePrimaryDetails();
                PopulateOilMajor();
            }

            BindRelieverMatrixData();
            CreateTabs();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(ViewState["EMPID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFileNo.Text = dt.Rows[0]["FLDFILENO"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
            }
            dt = PhoenixRegistersVesselBudget.FetchVesselBudget(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                            , General.GetNullableInteger(ViewState["RANKID"].ToString())
                                            , General.GetNullableDateTime(ViewState["RELIEFDATE"].ToString()));
            if (dt.Rows.Count > 0)
            {
                txtBudgetCode.Text = dt.Rows[0]["FLDCURRENCYCODE"].ToString() + " - " + dt.Rows[0]["FLDBUDGETEDWAGE"].ToString();
            }
            dt = PhoenixCrewPlanning.FetchCrewPlannedVesselInformation(General.GetNullableInteger(ViewState["VESSELID"].ToString()));
            {
                txtDryDock.Text = "Not Planned";
                if (dt.Rows[0]["FLDFROMDATE"].ToString() != string.Empty)
                    txtDryDock.Text = General.GetDateTimeToString(dt.Rows[0]["FLDFROMDATE"].ToString()) + " - " + General.GetDateTimeToString(dt.Rows[0]["FLDTODATE"].ToString());
                txtVessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
                txtSire.Text = "Not Planned";
                if (dt.Rows[0]["FLDSIREPLANDATE"].ToString() != string.Empty)
                    txtSire.Text = General.GetDateTimeToString(dt.Rows[0]["FLDSIREPLANDATE"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void CreateTabs()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Relief Plan", "RELIEFPLAN");
        toolbarmain.AddButton("Reliever", "RELIEVER");
        toolbarmain.AddButton("Reliever Filter", "RELIEVERFILTER");

        CrewRelieverTabs.AccessRights = this.ViewState;
        CrewRelieverTabs.MenuList = toolbarmain.Show();
        CrewRelieverTabs.SelectedMenuIndex = 1;
    }

    protected void CrewRelieverTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (CommandName.ToUpper().Equals("RELIEFPLAN"))
            {
                if (Request.QueryString["selectedindex"] != null)
                    Response.Redirect("CrewPlanRelievee.aspx?empid=" + ViewState["EMPID"] + "&vesselid=" + ViewState["VESSELID"] + "&rankid=" + ViewState["RANKID"] + "&selectedindex=" + ViewState["SELECTEDINDEX"].ToString() + "&Ispopup=" + ViewState["Ispopup"] + "&GroupRank=" + ViewState["GROUPRANKID"], false);
                else
                    Response.Redirect("CrewPlanRelievee.aspx?Ispopup=" + ViewState["Ispopup"] + "&GroupRank=" + ViewState["GROUPRANKID"], false);
            }
            if (CommandName.ToUpper().Equals("RELIEVERFILTER"))
            {
                Response.Redirect("CrewPlanRelieverNewFilter.aspx?empid=" + ViewState["EMPID"] + "&vesselid=" + ViewState["VESSELID"] + "&rankid=" + ViewState["RANKID"] + "&IsTop4=" + ViewState["ISTOP4"].ToString() + "&Ispopup=" + ViewState["Ispopup"] + "&GroupRank=" + ViewState["GROUPRANKID"], false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RelieverMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {

                int? iEmployeeId = null;
                int? iVesselId = null;
                int? iRankId = null;

                if (ViewState["EMPID"] != null) iEmployeeId = Int32.Parse(ViewState["EMPID"].ToString());
                if (ViewState["VESSELID"] != null) iVesselId = Int32.Parse(ViewState["VESSELID"].ToString());
                if (ViewState["RANKID"] != null) iRankId = Int32.Parse(ViewState["RANKID"].ToString());

                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDEMPLOYEECODE", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDZONE", "FLDRANKEXP", "FLDTYPEEXP", "FLDALLTYPEEXP", "FLDOPERATOREXP", "FLDLASTSIGNOFFDATE", "FLDDOA", "FLDASSESSMENTCHECK", "FLDSUITABILITYCHECK" };
                string[] alCaptions = { "File No.", "Name", "Rank", "Zone", "Rank Experience", "Vessel Type Experience", "All Type Experience", "Sign-off date", "DOA", "Assessment", "Suitability", "Status" };

                string sortexpression = (ViewState["RSORTEXPRESSION"] == null) ? null : (ViewState["RSORTEXPRESSION"].ToString());
                int? sortdirection = null;
                if (ViewState["RSORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["RSORTDIRECTION"].ToString());
                else
                    sortdirection = 1; //defaulting descending order for Relief due date

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                NameValueCollection nvc = Filter.CurrentPlanRelieverFilterSelection;
                DataTable dt = PhoenixCrewPlanning.SearchRelieverPlanSameRank(iVesselId, iRankId
                                                                            , General.GetNullableInteger(ddlOMM.SelectedValue)
                                                                            , General.GetNullableInteger(ViewState["NEXTRANKEMPLOYEE"].ToString())
                                                                            , sortexpression, sortdirection
                                                                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                            , gvRelieverSearch.PageSize
                                                                            , ref iRowCount, ref iTotalPageCount
                                                                         );

                
                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Reliever List", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
        //gvRelieverSearch.SelectedIndex = -1;
        BindRelieverData();
        gvRelieverSearch.Rebind();
    }

    public void BindRelieverData()
    {
        int? iEmployeeId = null;
        int? iVesselId = null;
        int? iRankId = null;

        if (ViewState["EMPID"] != null) iEmployeeId = Int32.Parse(ViewState["EMPID"].ToString());
        if (ViewState["VESSELID"] != null) iVesselId = Int32.Parse(ViewState["VESSELID"].ToString());
        if (ViewState["RANKID"] != null) iRankId = Int32.Parse(ViewState["RANKID"].ToString());

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEMPLOYEECODE", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDZONE", "FLDRANKEXP", "FLDTYPEEXP", "FLDALLTYPEEXP", "FLDOPERATOREXP", "FLDLASTSIGNOFFDATE", "FLDDOA", "FLDASSESSMENTCHECK", "FLDSUITABILITYCHECK" };
        string[] alCaptions = { "File No.", "Name", "Rank", "Zone", "Rank Experience", "Vessel Type Experience", "All Type Experience", "Sign-off date", "DOA", "Assessment", "Suitability", "Status" };

        string sortexpression = (ViewState["RSORTEXPRESSION"] == null) ? null : (ViewState["RSORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["RSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["RSORTDIRECTION"].ToString());
        else
            sortdirection = 1; //defaulting descending order for Relief due date
        try
        {

            NameValueCollection nvc = Filter.CurrentPlanRelieverFilterSelection;
            DataTable dt = PhoenixCrewPlanning.SearchRelieverPlanSameRank(iVesselId, iRankId
                                                                        , General.GetNullableInteger(ddlOMM.SelectedValue)
                                                                        , General.GetNullableInteger(ViewState["NEXTRANKEMPLOYEE"].ToString())
                                                                        , sortexpression, sortdirection
                                                                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                        , gvRelieverSearch.PageSize
                                                                        , ref iRowCount, ref iTotalPageCount
                                                                     );

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvRelieverSearch", "Reliever List", alCaptions, alColumns, ds);

            gvRelieverSearch.DataSource = dt;
            gvRelieverSearch.VirtualItemCount = iRowCount;

            if (dt.Rows.Count > 0)
            {
                gvRelieverSearch.MasterTableView.GetColumn("RANKEXP").HeaderText = "Rank (" + dt.Rows[0]["FLDREQRANKEXP"].ToString() + " M)";
                gvRelieverSearch.MasterTableView.GetColumn("VSLTYPEEXP").HeaderText = "Type (" + dt.Rows[0]["FLDREQTYPEEXP"].ToString() + " M)";
                gvRelieverSearch.MasterTableView.GetColumn("ALLTYPEEXP").HeaderText = "All Type (" + dt.Rows[0]["FLDREQALLTYPEEXP"].ToString() + " M)";
                gvRelieverSearch.MasterTableView.GetColumn("OPERATOREXP").HeaderText = "Operator (" + dt.Rows[0]["FLDREQOPERATOREXP"].ToString() + " M)";
            }

            ViewState["RROWCOUNT"] = iRowCount;
            ViewState["RTOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvRelieverSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRelieverSearch.CurrentPageIndex + 1;

        BindRelieverData();
    }


    protected void gvRelieverSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;

            if (e.CommandName.ToUpper() == "PLANRELIEVER")
            {
                string relieverid = ((RadLabel)e.Item.FindControl("lblRelieverId")).Text;

                Filter.CurrentCrewSelection = relieverid.ToString();

                if (Filter.CurrentCrewSelection != null)
                {
                    Response.Redirect("../Crew/CrewPersonalGeneral.aspx?empid=" + relieverid + "&evaluation=1");

                }
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

    protected void gvRelieverSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton lb = (LinkButton)e.Item.FindControl("lnkReliever");
            lb.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "'); return false;");

            LinkButton ib = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ib != null)
            {
                ib.Visible = SessionUtil.CanAccess(this.ViewState, ib.CommandName);
            }

            LinkButton imgSuitableCheck = (LinkButton)e.Item.FindControl("imgSuitableCheck");
            if (imgSuitableCheck != null)
                imgSuitableCheck.Attributes.Add("onclick", "javascript:openNewWindow('codehelp', '', '" + Session["sitepath"] + "/Crew/CrewSuitabilityCheck.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&vesselid=" + ViewState["VESSELID"].ToString() + "&personalmaster=true&rankid=" + drv["FLDRANKID"].ToString() + "');return false;");

        }
    }

    protected void gvRelieverSearch_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }


    protected void gvRelieverMatrix_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindRelieverMatrixData();
    }
    public void BindRelieverMatrixData()
    {

        try
        {

            DataTable dt = PhoenixCrewPlanning.CrewPlanRelieverOilMajorMatrixList(General.GetNullableInteger(ViewState["VESSELID"].ToString()), General.GetNullableInteger(ddlOMM.SelectedValue));

            gvRelieverMatrix.DataSource = dt;
            if (dt.Rows.Count > 0)
            {
                gvRelieverMatrix.MasterTableView.GetColumn("RANKEXP").HeaderText = "Rank (" + dt.Rows[0]["FLDREQRANKEXP"].ToString() + " M)";
                gvRelieverMatrix.MasterTableView.GetColumn("VSLTYPEEXP").HeaderText = "Type (" + dt.Rows[0]["FLDREQVESSELTYPEEXP"].ToString() + " M)";
                gvRelieverMatrix.MasterTableView.GetColumn("ALLTYPEEXP").HeaderText = "All Type (" + dt.Rows[0]["FLDREQALLTYPEEXP"].ToString() + " M)";
                gvRelieverMatrix.MasterTableView.GetColumn("OPERATOREXP").HeaderText = "Operator (" + dt.Rows[0]["FLDREQOPERATOREXP"].ToString() + " M)";

                gvRelieverMatrix.MasterTableView.GetColumn("NAME").FooterText = "Total";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    decimal r;
    decimal r2;
    decimal r3;
    decimal r4;


    protected void gvRelieverMatrix_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;

            int selectedRowIndex = dataItem.RowIndex;

        }

        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton lnk = (LinkButton)e.Item.FindControl("lnkName");
            if (lnk != null)
                lnk.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "'); return false;");

            if (drv["FLDRANKEXP"].ToString() != "")
            {
                r = r + decimal.Parse(drv["FLDRANKEXP"].ToString());
            }
            if (drv["FLDVESSELTYPEEXP"].ToString() != "")
            {
                r2 = r2 + decimal.Parse(drv["FLDVESSELTYPEEXP"].ToString());
            }
            if (drv["FLDALLTYPEEXP"].ToString() != "")
            {
                r3 = r3 + decimal.Parse(drv["FLDALLTYPEEXP"].ToString());
            }
            if (drv["FLDOPERATOREXP"].ToString() != "")
            {
                r4 = r4 + decimal.Parse(drv["FLDOPERATOREXP"].ToString());
            }

            if (drv["FLDRANKID"].ToString() != ViewState["RANKID"].ToString())
                ViewState["NEXTRANKEMPLOYEE"] = drv["FLDEMPLOYEEID"].ToString();
        }
        if (e.Item is GridFooterItem)
        {
            RadLabel lblRankExpFooter = (RadLabel)e.Item.FindControl("lblRankExpFooter");
            lblRankExpFooter.Text = r.ToString();
            RadLabel lblVesselTypeExpFooter = (RadLabel)e.Item.FindControl("lblVesselTypeExpFooter");
            lblVesselTypeExpFooter.Text = r2.ToString();
            RadLabel lblAllTypeExpFooter = (RadLabel)e.Item.FindControl("lblAllTypeExpFooter");
            lblAllTypeExpFooter.Text = r3.ToString();
            RadLabel lblOperatorExpFooter = (RadLabel)e.Item.FindControl("lblOperatorExpFooter");
            lblOperatorExpFooter.Text = r4.ToString();
        }
    }

    private void PopulateOilMajor()
    {
        DataTable dt = PhoenixCrewsOilMajorMatrix.ListOilMajorMatrixRank(General.GetNullableInteger(ViewState["RANKID"].ToString()));
        ddlOMM.DataTextField = "FLDMATRIXNAME";
        ddlOMM.DataValueField = "FLDMATRIXID";
        ddlOMM.DataSource = dt;
        ddlOMM.DataBind();
        
    }



}
