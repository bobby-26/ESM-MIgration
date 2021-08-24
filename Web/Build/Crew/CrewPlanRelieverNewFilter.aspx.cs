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
public partial class CrewPlanRelieverNewFilter : PhoenixBasePage
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
            toolbar.AddFontAwesomeButton("../Crew/CrewPlanRelieverNewFilter.aspx?empid=" + Request.QueryString["empid"] + "&vesselid=" + Request.QueryString["vesselid"] + "&rankid=" + Request.QueryString["rankid"] + "&relieverid=" + Request.QueryString["relieverid"] + "&IsTop4=" + Request.QueryString["IsTop4"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRelieverSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewPlanRelieverNewFilter.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewPlanRelieverNewFilter.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../Crew/CrewPlanRelieverNewFilter.aspx", "New Applicant", "<i class=\"fas fa-user\"></i>", "NEWAPP");
            RelieverMenu.AccessRights = this.ViewState;
            RelieverMenu.MenuList = toolbar.Show();
         
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                txtVesselid.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["RCURRENTINDEX"] = 1;
                ViewState["ROWCOUNT"] = 0;
                ViewState["TOTALPAGECOUNT"] = 0;

                ViewState["EMPID"] = string.IsNullOrEmpty(Request.QueryString["empid"]) ? "" : Request.QueryString["empid"];
                ViewState["VESSELID"] = string.IsNullOrEmpty(Request.QueryString["vesselid"]) ? "" : Request.QueryString["vesselid"];
                ViewState["RANKID"] = string.IsNullOrEmpty(Request.QueryString["rankid"]) ? "" : Request.QueryString["rankid"];
                ViewState["RELIEFDATE"] = string.IsNullOrEmpty(Request.QueryString["reliefdate"]) ? "" : Request.QueryString["reliefdate"];
                ViewState["SELECTEDINDEX"] = string.IsNullOrEmpty(Request.QueryString["selectedindex"]) ? "" : Request.QueryString["selectedindex"];
                ViewState["ISTOP4"] = string.IsNullOrEmpty(Request.QueryString["IsTop4"]) ? "" : Request.QueryString["IsTop4"];
                ViewState["NEWAPP"] = 0;
                ViewState["Ispopup"] = string.IsNullOrEmpty(Request.QueryString["Ispopup"]) ? "" : Request.QueryString["Ispopup"];
                ViewState["GROUPRANKID"] = string.IsNullOrEmpty(Request.QueryString["GroupRank"]) ? "" : Request.QueryString["GroupRank"];

                cmdShowVessel.Attributes.Add("onclick", "return showPickList('spnPickListVessel', 'codehelp1', '', '../Common/CommonPickListVesselFilter.aspx', true);");

                SetEmployeePrimaryDetails();


                if (Filter.CurrentPlanRelieverFilterSelection != null)
                {
                    BindFilterData();
                }
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
    private void CreateTabs()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Relief Plan", "RELIEFPLAN");
        toolbarmain.AddButton("Reliever", "RELIEVER");
        toolbarmain.AddButton("Reliever Filter", "RELIEVERFILTER");

        CrewRelieverTabs.AccessRights = this.ViewState;
        CrewRelieverTabs.MenuList = toolbarmain.Show();
        CrewRelieverTabs.SelectedMenuIndex = 2;
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

            if (CommandName.ToUpper().Equals("RELIEVER"))
            {
                Response.Redirect("CrewPlanRelieverNew.aspx?empid=" + ViewState["EMPID"] + "&vesselid=" + ViewState["VESSELID"] + "&rankid=" + ViewState["RANKID"] + "&IsTop4=" + ViewState["ISTOP4"].ToString() + "&Ispopup=" + ViewState["Ispopup"] + "&GroupRank=" + ViewState["GROUPRANKID"], false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetFilter()
    {
        try
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Clear();
            nvc.Add("txtVesselid", txtVesselid.Text);
            nvc.Add("txtVesselName", txtVesselName.Text);
            Filter.CurrentPlanRelieverFilterSelection = nvc;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindFilterData()
    {
        try
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc = Filter.CurrentPlanRelieverFilterSelection;

            txtVesselid.Text = nvc != null ? nvc.Get("txtVesselid") : "";
            txtVesselName.Text = nvc != null ? nvc.Get("txtVesselName") : "";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindRelieverMatrixData()
    {

        try
        {

            DataTable dt = PhoenixCrewPlanning.CrewPlanRelieverMatrixList(General.GetNullableInteger(ViewState["RANKID"].ToString()), General.GetNullableInteger(ViewState["VESSELID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                DataRow[] drs = dt.Select("FLDRANKPOSTED <> " + ViewState["RANKID"].ToString());
                if (drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        txt1.Text = dr["FLDRANKEXP"].ToString();
                        txt2.Text = dr["FLDVESSELTYPEEXP"].ToString();
                    }
                }
            }

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
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFileNo.Text = dt.Rows[0]["FLDFILENO"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
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

            if (CommandName.ToUpper().Equals("FIND"))
            {
                Filter.CurrentPlanRelieverFilterSelection = null;
                SetFilter();
                
                BindRelieverData();
                gvRelieverSearch.Rebind();

            }
            if (CommandName.ToUpper().Equals("NEWAPP"))
            {
                Filter.CurrentPlanRelieverFilterSelection = null;
                SetFilter();                
                ViewState["NEWAPP"] = 1;
                BindRelieverData();
                gvRelieverSearch.Rebind();

            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentPlanRelieverFilterSelection = null;
                
                ddlRank.SelectedRank = "";
                ViewState["NEWAPP"] = 0;
                ViewState["PAGENUMBER"] = 1;
                gvRelieverSearch.CurrentPageIndex = 0;
                BindRelieverData();
                gvRelieverSearch.Rebind();
            }

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDEMPLOYEECODE", "FLDEMPLOYEENAME", "FLDEMPLOYEERANKNAME", "FLDEXPERIENCE", "FLDVESSELTYPEEXP", "FLDLASTSIGNOFFDATE", "FLDDOA", "FLDSTATUSDESCRIPTION" };
                string[] alCaptions = { "File No.", "Name", "Rank", "Rank Experience", "Vessel Type Experience", "Sign-off date", "DOA", "Status" };

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                NameValueCollection nvc = new NameValueCollection(); ;

                if (Filter.CurrentPlanRelieverFilterSelection != null)
                    nvc = Filter.CurrentPlanRelieverFilterSelection;
                else
                {
                    nvc.Add("txtVesselid", " ");
                }

                DataTable dt = PhoenixCrewPlanning.RelieverPlanFilterSearch(General.GetNullableInteger(ViewState["EMPID"].ToString())
                                                                , General.GetNullableInteger(nvc != null ? nvc.Get("txtVesselid") : ViewState["VESSELID"].ToString())
                                                                    , General.GetNullableInteger(ViewState["RANKID"].ToString())
                                                                    , 1 // Include Planned
                                                                    , General.GetNullableByte(ViewState["NEWAPP"].ToString())
                                                                    , General.GetNullableInteger(ddlRank.SelectedRank)
                                                                    , sortexpression, sortdirection
                                                                    , (int)ViewState["PAGENUMBER"]
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

    decimal r3;
    decimal r4;


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;        
        BindRelieverData();
        gvRelieverSearch.Rebind();
    }
    public void BindRelieverData()
    {

        int? iEmployeeId = null;
        int? iVesselId = null;
        int? iRankId = null;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        if (ViewState["EMPID"] != null) iEmployeeId = Int32.Parse(ViewState["EMPID"].ToString());
        if (ViewState["VESSELID"] != null) iVesselId = Int32.Parse(ViewState["VESSELID"].ToString());
        if (ViewState["RANKID"] != null) iRankId = Int32.Parse(ViewState["RANKID"].ToString());

        string[] alColumns = { "FLDEMPLOYEECODE", "FLDEMPLOYEENAME", "FLDEMPLOYEERANKNAME", "FLDEXPERIENCE", "FLDVESSELTYPEEXP", "FLDLASTSIGNOFFDATE", "FLDDOA", "FLDSTATUSDESCRIPTION" };
        string[] alCaptions = { "File no", "Name", "Rank", "Rank Experience", "Vessel Type Experience", "Sign-off date", "DOA", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 1; //defaulting descending order for Relief due date
        try
        {

            NameValueCollection nvc = new NameValueCollection(); ;

            if (Filter.CurrentPlanRelieverFilterSelection != null)
                nvc = Filter.CurrentPlanRelieverFilterSelection;
            else
            {
                nvc.Add("txtVesselid", iVesselId.ToString());
            }

            DataTable dt = PhoenixCrewPlanning.RelieverPlanFilterSearch(iEmployeeId
                                                                         , General.GetNullableInteger(nvc != null ? nvc.Get("txtVesselid") : iVesselId.ToString())
                                                                         , General.GetNullableInteger(iRankId.ToString())
                                                                         , 1 // Include Planned
                                                                         , General.GetNullableByte(ViewState["NEWAPP"].ToString())
                                                                         , General.GetNullableInteger(ddlRank.SelectedRank)
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


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

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


    protected void gvRelieverSearch_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRelieverSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;

            if (e.CommandName.ToUpper() == "PLANRELIEVER")
            {
                Filter.CurrentNewApplicantSelection = null;

                string relieverid = ((RadLabel)e.Item.FindControl("lblRelieverId")).Text;
                string newappyn = ((RadLabel)e.Item.FindControl("lblnewappyn")).Text;

                if (newappyn == "0")
                {
                    Filter.CurrentCrewSelection = relieverid.ToString();
                }
                else
                {
                    Filter.CurrentNewApplicantSelection = relieverid.ToString();
                }

                if (Filter.CurrentNewApplicantSelection != null)
                {
                    Response.Redirect("../Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + ((RadLabel)e.Item.FindControl("lblRelieverId")).Text + "&evaluation=1");

                }
                else if (Filter.CurrentCrewSelection != null)
                {
                    Response.Redirect("../Crew/CrewPersonalGeneral.aspx?empid=" + ((RadLabel)e.Item.FindControl("lblRelieverId")).Text + "&evaluation=1");

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
            RadLabel l = (RadLabel)e.Item.FindControl("lblPlanned");
            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            if (l.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/blue.png";
            }
            else
            {
                imgFlag.Visible = false;
            }

            RadLabel lblRelieverId = (RadLabel)e.Item.FindControl("lblRelieverId");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkReliever");
            RadLabel lblnewappyn = (RadLabel)e.Item.FindControl("lblnewappyn");
            RadLabel lblRankid = (RadLabel)e.Item.FindControl("lblRankid");
            RadLabel lblIsTop4 = (RadLabel)e.Item.FindControl("lblIsTop4");
            RadLabel lblrank = (RadLabel)e.Item.FindControl("lblEmployeeRank");

            string strnewpappyn;
            if (lblnewappyn.Text == "1")
            {
                strnewpappyn = "false";
            }
            else
            {
                strnewpappyn = "true";
            }
            if (lb != null)
            {
                lb.Enabled = SessionUtil.CanAccess(this.ViewState, lb.CommandName);
                lb.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + lblRelieverId.Text + "'); return false;");
            }

            LinkButton ib = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ib != null)
            {
                lb.Enabled = SessionUtil.CanAccess(this.ViewState, lb.CommandName);
                ib.Visible = SessionUtil.CanAccess(this.ViewState, ib.CommandName);
            }

            LinkButton imgSuitableCheck = (LinkButton)e.Item.FindControl("imgSuitableCheck");
            imgSuitableCheck.Attributes.Add("onclick", "openNewWindow('codehelp', '', '" + Session["sitepath"] + "/Crew/CrewSuitabilityCheck.aspx?empid=" + lblRelieverId.Text + "&vesselid=" + ViewState["VESSELID"].ToString() + "&personalmaster=" + strnewpappyn + "&rankid=" + lblRankid.Text + "');return false;");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");

            DataRowView drv = (DataRowView)e.Item.DataItem;
            decimal main;

            if (drv["FLDEXPERIENCE"].ToString() != "")
            {

                if (txt1.Text == "")
                {
                    main = 0.00m;
                }
                else
                {
                    main = decimal.Parse(txt1.Text);
                }
                r3 = main + decimal.Parse(drv["FLDEXPERIENCE"].ToString());
            }
            else
            {
                if (txt1.Text == "")
                {
                    main = 0.00m;
                }
                else
                {
                    main = decimal.Parse(txt1.Text);
                }
                r3 = main;

            }
            if (drv["FLDVESSELTYPEEXP"].ToString() != "")
            {

                if (txt2.Text == "")
                {
                    main = 0.00m;
                }
                else
                {
                    main = decimal.Parse(txt2.Text);
                }
                r4 = main + decimal.Parse(drv["FLDVESSELTYPEEXP"].ToString());
            }
            else
            {
                if (txt2.Text == "")
                {
                    main = 0.00m;
                }
                else
                {
                    main = decimal.Parse(txt2.Text);
                }
                r4 = main;


            }
            if (ViewState["ISTOP4"].ToString() == "1")
            {
                uct.Visible = true;

                uct.Text = "<table border='1'><tr><td></td><td>" + "<u>Rank Experience</u></td><td>" + "  " + "<u>Vessel Type Experience</u></td></tr>"
                            + "<tr><td>Reliever</td><td>" + drv["FLDEXPERIENCE"].ToString() + "</td><td>" + drv["FLDVESSELTYPEEXP"].ToString()
                            + "</td></tr><tr><td>Onboard</td><td>" + txt1.Text + "</td><td>" + txt2.Text
                            + "</td></tr><tr><td>Total</td><td>" + r3
                            + "</td><td>" + r4 + "</td></tr></table>";
                lblrank.Attributes.Add("onmouseover", "showTooltip(event,'" + uct.ToolTip + "', 'visible');");
                lblrank.Attributes.Add("onmouseout", "showTooltip(event,'" + uct.ToolTip + "', 'hidden');");

            }
        }
    }


    protected void gvRelieverSearch_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }


}
