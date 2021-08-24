using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
using System.Text;

public partial class CrewOffshoreReliefPlan : PhoenixBasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreReliefPlan.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                toolbarsub.AddFontAwesomeButton("javascript:parent.openNewWindow('Filter','','" + Session["sitepath"] + "/CrewOffShore/CrewOffShoreReliefPlanFilter.aspx','','800px','500px'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            else
                toolbarsub.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/CrewOffShore/CrewOffShoreReliefPlanFilter.aspx','','800px','500px'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");

            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreReliefPlan.aspx" + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty), "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreReliefPlan.aspx" + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty), "Map Event", "<i class=\"fas fa-calendar-alt\"></i>", "MAPEVENT");

            QueryMenu.AccessRights = this.ViewState;
            QueryMenu.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                ucRank.AppendDataBoundItems = true;
                ddlVessel.AppendDataBoundItems = "true";
                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBER1"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["EMPID"] = "";
                ViewState["hidemenu"] = "";
                ViewState["launchedfrom"] = "";
                ViewState["pl"] = "";
                ViewState["VESSELID"] = "";

                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();

                if (Request.QueryString["selectedindex"] != null)
                {
                    gvSearch.Items[Request.QueryString["selectedindex"].ToString()].Selected = true;// = int.Parse(Request.QueryString["selectedindex"].ToString());
                    ViewState["EMPID"] = Request.QueryString["empid"].ToString();
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
                    ViewState["RANKID"] = Request.QueryString["rankid"].ToString();
                }

                if (Request.QueryString["hidemenu"] != null && Request.QueryString["hidemenu"].ToString() != "")
                {
                    ViewState["hidemenu"] = Request.QueryString["hidemenu"].ToString();

                }

                if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                    ViewState["launchedfrom"] = Request.QueryString["launchedfrom"].ToString();

                if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
                    ViewState["pl"] = Request.QueryString["pl"].ToString();
                txtReliefDue.Text = "90";
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ddlVessel.bind();
                    ddlVessel.Enabled = false;
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                }
                else
                {
                    if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
                    {
                        ddlVessel.SelectedVessel = ViewState["VESSELID"].ToString();
                        ddlVessel.bind();
                    }
                }

                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtReliefDue", txtReliefDue.Text);
                criteria.Add("ucRank", ucRank.SelectedRank);
                criteria.Add("ucVessel", ddlVessel.SelectedVessel);
                criteria.Add("status", "1");
                Filter.CurrentPlanRelieveeFilterSelection = criteria;
                Gvplanningalert.PageSize = 100;// int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }



            CreateTabs();
            CrewRelieverTabs.SelectedMenuIndex = 0;

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
        toolbarmain.AddButton("Relief Plan", "RELIEFPLAN", ToolBarDirection.Right);
        CrewRelieverTabs.AccessRights = this.ViewState;
        CrewRelieverTabs.MenuList = toolbarmain.Show();
        CrewRelieverTabs.SelectedMenuIndex = 0;
    }
    protected void CrewRelieverTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("RELIEVER"))
            {
                if (General.GetNullableInteger(ViewState["EMPID"].ToString()) == null)
                {
                    CrewRelieverTabs.SelectedMenuIndex = 0;
                    ucError.ErrorMessage = "Please select a Relivee";
                    ucError.Visible = true;
                    return;
                }
            }
            if (CommandName.ToUpper().Equals("RELIEVER"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreReliever.aspx?hidemenu=" + ViewState["hidemenu"] + "&launchedfrom=" + ViewState["launchedfrom"] + "&pl=" + ViewState["pl"] + "&rankid=" + ViewState["RANKID"].ToString() + "&vesselid=" + ViewState["VESSELID"].ToString() + "&offsignerid=" + ViewState["EMPID"] + "&reliefdate=" + ViewState["RELIEFDATE"] + "&vsltype=" + ViewState["VSLTYPE"], false);
            }
            else if (CommandName.ToUpper().Equals("RELIEFPLAN"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreReliefPlan.aspx" + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void QueryMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentPlanRelieveeFilterSelection = null;
                Filter.CurrentCrewReliefPlanEventFilter = null;
                ddlVessel.SelectedVessel = "";
                txtReliefDue.Text = "90";
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ddlVessel.bind();
                    ddlVessel.Enabled = false;
                }

                BindData();
                gvSearch.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper() == "MAPEVENT")
            {
                GetSelectedCrewPlan();

                NameValueCollection nvc = Filter.CurrentCrewReliefPlanEventFilter;

                string crewplanlist = General.GetNullableString(nvc != null ? nvc.Get("crewplanlist") : string.Empty);
                string signonoffiflist = General.GetNullableString(nvc != null ? nvc.Get("signonoffiflist") : string.Empty);
                
                if (!isValidMapEvent(crewplanlist, signonoffiflist))
                {
                    ucError.Visible = true;
                    return;
                }

                if (crewplanlist != null || signonoffiflist != null)
                { 
                    String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewPlanReliefEventAdd.aspx?VESSELID=" + ddlVessel.SelectedVessel + "',false,1000,500);");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected bool isValidMapEvent(string csvCrewPlanList, string csvSignonoffidList)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((General.GetNullableInteger(ddlVessel.SelectedVessel) == null) || ddlVessel.SelectedVessel == "0")
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableString(csvCrewPlanList) == null && General.GetNullableString(csvSignonoffidList) == null)
            ucError.ErrorMessage = "Please select valid crew change";


        return (!ucError.IsError);
    }


    protected void GetSelectedCrewPlan()   
    {
        StringBuilder strlist = new StringBuilder();
        StringBuilder strsignonofflist = new StringBuilder();
        Filter.CurrentCrewReliefPlanEventFilter = null;

        if (gvSearch.MasterTableView.Items.Count > 0)
        {
            foreach (GridDataItem gvr in gvSearch.MasterTableView.Items)
            {
                RadCheckBox chkAdd = (RadCheckBox)gvr.FindControl("chkPlan");

                if (chkAdd.Checked == true)
                {
                    Label lblCrewPlanId = (Label)gvr.FindControl("lblCrewPlanId");
                    Label lblSignonoffid = (Label)gvr.FindControl("lblSignonoffid");

                    if(lblCrewPlanId.Text!= "")
                    { 
                        strlist.Append(lblCrewPlanId.Text);
                        strlist.Append(",");
                    }

                    if (lblSignonoffid.Text != "")
                    {
                        strsignonofflist.Append(lblSignonoffid.Text);
                        strsignonofflist.Append(",");
                    }
            }
            }

            if (strlist.Length > 1)
            {
                strlist.Remove(strlist.Length - 1, 1);
            }
            if (strsignonofflist.Length > 1)
            {
                strsignonofflist.Remove(strsignonofflist.Length - 1, 1);
            }

        }


        NameValueCollection criteria = new NameValueCollection();

        criteria.Add("crewplanlist", strlist.ToString());
        criteria.Add("signonoffiflist", strsignonofflist.ToString());

        Filter.CurrentCrewReliefPlanEventFilter = criteria;

  

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;

        BindData();
        gvSearch.Rebind();


    }




    protected bool isValidUpdate(string joindate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(joindate) == null)
            ucError.ErrorMessage = "Joining Date is required.";

        return (!ucError.IsError);
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDCHARTERERNAME", "FLDCHARTERERREMAININGDAYS", "FLDNAME","FLDRANKNAME","FLDOFFSIGNERNATIONALITY","FLDOFFSIGNERCERTIFICATE","FLDOFFSIGNERDAILYRATE", "FLDOFFSIGNERDPALLOWANCE",
                                 "FLDOFFSIGNERJOINDATE", "FLDDAYSONBOARD", "FLDRELIEFDUEDATE", "FLDOFFSIGNER90DAYSRELIEF", "FLDRELIEVERNAME", "FLDRELIEVERNATIONALITY",
                                 "FLDRELIEVERRANK","FLDONSIGNERCERTIFICATE","FLDCURRENCYCODE","FLDRELIEVERDAILYRATE","FLDRELIVERDPALLOWANCE", "FLDEXPECTEDJOINDATE", "FLDSEAPORTNAME",
                                 "FLDPROPOSALREMARKS", "FLDPDSTATUS" };
        string[] alCaptions = { "Vessel", "Charterer", "Days remaining for Charterer", "Off-Signer","Rank","Nationality","Additional Certificate","Daily Rate (USD)", "Daily DP Allowance (USD)", "Sign On Date", "Days Onboard",
                                  "End of Contract", "Max Tour of Duty", "Reliever Name", "Nationality", "Proposed Rank",
                                  "Additional Certificate","Currency","Daily Rate", "Daily DP Allowance","Expected Joining Date", "Joining Port", "Remarks", "Offshore Stage / Employee Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        byte? filteryn = 0; setVessel();
        NameValueCollection nvc = Filter.CurrentPlanRelieveeFilterSelection;

        if (nvc != null && !string.IsNullOrEmpty(nvc.Get("ucVesselType")))
        {
            filteryn = (byte?)1;
        }

        DataTable dt = PhoenixCrewOffshoreCrewChange.RelieveePlanQueryActivity(nvc != null ? nvc.Get("txtName") : string.Empty
                                                                    , nvc != null ? nvc.Get("ucRank") : string.Empty
                                                                    , nvc != null ? nvc.Get("ucVessel") : string.Empty
                                                                    , nvc != null ? nvc.Get("ucVesselType") : string.Empty
                                                                    , nvc != null ? nvc.Get("ucZone") : string.Empty
                                                                    , nvc != null ? nvc.Get("ucPool") : string.Empty
                                                                    , nvc != null ? nvc.Get("lstNationality") : string.Empty
                                                                    , null
                                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("txtReliefDue")) : null
                                                                    , sortexpression, sortdirection
                                                                    , (int)ViewState["PAGENUMBER"], iRowCount
                                                                    , ref iRowCount, ref iTotalPageCount
                                                                    , nvc != null ? nvc.Get("ucPrincipal") : string.Empty
                                                                    , Request.QueryString["access"] != null ? (byte?)General.GetNullableInteger("1") : null
                                                                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
                                                                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
                                                                    , filteryn
                                                                    , nvc != null ? nvc.Get("ddlEmployeeStatus") : string.Empty
                                                                    , nvc != null ? nvc.Get("ddlOffshoreStage") : string.Empty
                                                                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtExpectedDate")) : null
                                                                    , nvc != null ? nvc.Get("ucPort") : string.Empty
                                                                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtExpectedtoDate")) : null);

        General.ShowExcel("Relief Plan", dt, alColumns, alCaptions, null, sortexpression);
    }
    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            byte? filteryn = 0;
            setVessel();
            NameValueCollection nvc = Filter.CurrentPlanRelieveeFilterSelection;

            if (nvc != null && !string.IsNullOrEmpty(nvc.Get("ucVesselType")))
            {
                filteryn = (byte?)1;
            }

            DataTable dt = PhoenixCrewOffshoreCrewChange.RelieveePlanQueryActivity(nvc != null ? nvc.Get("txtName") : string.Empty
                                                                        , nvc != null ? nvc.Get("ucRank") : string.Empty
                                                                        , nvc != null ? nvc.Get("ucVessel") : string.Empty
                                                                        , nvc != null ? nvc.Get("ucVesselType") : string.Empty
                                                                        , nvc != null ? nvc.Get("ucZone") : string.Empty
                                                                        , nvc != null ? nvc.Get("ucPool") : string.Empty
                                                                        , nvc != null ? nvc.Get("lstNationality") : string.Empty
                                                                        , null
                                                                        , nvc != null ? General.GetNullableInteger(nvc.Get("txtReliefDue")) : null
                                                                        , sortexpression, sortdirection
                                                                        , (int)ViewState["PAGENUMBER"], gvSearch.PageSize
                                                                        , ref iRowCount, ref iTotalPageCount
                                                                        , nvc != null ? nvc.Get("ucPrincipal") : string.Empty
                                                                        , Request.QueryString["access"] != null ? (byte?)General.GetNullableInteger("1") : null
                                                                        , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
                                                                        , nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
                                                                        , filteryn
                                                                        , nvc != null ? nvc.Get("ddlEmployeeStatus") : string.Empty
                                                                        , nvc != null ? nvc.Get("ddlOffshoreStage") : string.Empty
                                                                        , nvc != null ? General.GetNullableDateTime(nvc.Get("txtExpectedDate")) : null

                                                                        , nvc != null ? nvc.Get("ucPort") : string.Empty
                                                                         , nvc != null ? General.GetNullableDateTime(nvc.Get("txtExpectedtoDate")) : null
                                                                        );
            gvSearch.DataSource = dt;
            gvSearch.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindDataAlert()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        try
        {
            NameValueCollection nvc = Filter.CurrentPlanRelieveeFilterSelection;

            DataTable dt = PhoenixCrewOffshoreCrewChange.CrewPlanAlert(ddlVessel.SelectedVessel != "0" ? General.GetNullableString(ddlVessel.SelectedVessel) : string.Empty
                                                                            , (int)ViewState["PAGENUMBER1"], Gvplanningalert.PageSize
                                                                            , ref iRowCount, ref iTotalPageCount);

            Gvplanningalert.DataSource = dt;
            Gvplanningalert.VirtualItemCount = iRowCount;



            ViewState["ROWCOUNT1"] = iRowCount;
            ViewState["TOTALPAGECOUNT1"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void ddlVessel_TextChanged(object sender, EventArgs e)
    {
        setvessel_textChange();
        BindData();
        gvSearch.Rebind();
    }
    protected void ucRank_TextChanged(object sender, EventArgs e)
    {
        setvessel_textChange();
        BindData();
        gvSearch.Rebind();

    }
    protected void setvessel_textChange()
    {
        NameValueCollection nvc = Filter.CurrentPlanRelieveeFilterSelection;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
            nvc.Clear();
            nvc.Add("ucVessel", ddlVessel.SelectedVessel);
            nvc.Add("txtReliefDue", txtReliefDue.Text);
            nvc.Add("ucRank", ucRank.SelectedRank);
            nvc.Add("status", "1");
            Filter.CurrentPlanRelieveeFilterSelection = nvc;
        }
        else
        {
            nvc.Remove("ucVessel");
            nvc.Add("ucVessel", ddlVessel.SelectedVessel);
            nvc.Remove("txtReliefDue");
            nvc.Add("txtReliefDue", txtReliefDue.Text);
            nvc.Remove("ucRank");
            nvc.Add("ucRank", ucRank.SelectedRank);
            nvc.Remove("status");
            nvc.Add("status", "1");
            Filter.CurrentPlanRelieveeFilterSelection = nvc;
            txtReliefDue.Text = nvc.Get("txtReliefDue");
            ucRank.SelectedRank = nvc.Get("ucRank") == "Dummy" ? string.Empty : nvc.Get("ucRank");
            ddlVessel.SelectedVessel = nvc.Get("ucVessel") == "Dummy" ? string.Empty : nvc.Get("ucVessel");
        }
    }
    protected void setVessel()
    {
        NameValueCollection nvc = Filter.CurrentPlanRelieveeFilterSelection;

        if (nvc == null)
        {
            nvc = new NameValueCollection();
            nvc.Clear();
            nvc.Add("ucVessel", ddlVessel.SelectedVessel);
            nvc.Add("txtReliefDue", txtReliefDue.Text);
            nvc.Add("ucRank", ucRank.SelectedRank);
            nvc.Add("status", "1");
            Filter.CurrentPlanRelieveeFilterSelection = nvc;
        }
        else
        {
            if (nvc.Get("status") == "0")
            {
                txtReliefDue.Text = nvc.Get("txtReliefDue");
                ucRank.SelectedRank = nvc.Get("ucRank") == "Dummy" ? string.Empty : nvc.Get("ucRank");
                ddlVessel.SelectedVessel = nvc.Get("ucVessel") == "Dummy" ? string.Empty : nvc.Get("ucVessel");
            }
        }
    }
    protected void imgSearch_Click(object sender, EventArgs e)
    {
        setvessel_textChange();
        BindData();
        gvSearch.Rebind();
    }

    protected void Gvplanningalert_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER1"] = ViewState["PAGENUMBER1"] != null ? ViewState["PAGENUMBER1"] : Gvplanningalert.CurrentPageIndex + 1;
            BindDataAlert();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void Gvplanningalert_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER1"] = null;
        }
    }



    protected void gvSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSearch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;


            ////gvSearch.SelectedIndex = e.NewSelectedIndex;
            //string empid = ((Label)e.Item.FindControl("lblEmployeeId")).Text;
            //string vesselid = ((Label)e.Item.FindControl("lblVesselId")).Text;
            //string rankid = ((Label)e.Item.FindControl("lblRankid")).Text;
            //string reliefdate = ((Label)e.Item.FindControl("lblReliefDue")).Text;
            //string vsltype = ((Label)e.Item.FindControl("lblVesseltype")).Text;
            //ViewState["EMPID"] = empid;
            //ViewState["VESSELID"] = vesselid;
            //ViewState["RANKID"] = rankid;
            //ViewState["RELIEFDATE"] = reliefdate;
            //ViewState["VSLTYPE"] = vsltype;

            if (e.CommandName.ToUpper() == "SHOW")
            {
                ViewState["RPAGENUMBER"] = 1;


                string empid = ((Label)e.Item.FindControl("lblEmployeeId")).Text;
                string relieverid = ((Label)e.Item.FindControl("lblRelieverId")).Text;
                string vesselid = ((Label)e.Item.FindControl("lblVesselId")).Text;
                string rankid = ((Label)e.Item.FindControl("lblRankid")).Text;
                string reliefdate = ((Label)e.Item.FindControl("lblReliefDue")).Text;
                string RelieverRankId = ((Label)e.Item.FindControl("lblRelieverRankId")).Text;
                string vsltype = ((Label)e.Item.FindControl("lblVesseltype")).Text;



                ViewState["EMPID"] = empid;
                ViewState["VESSELID"] = vesselid;
                ViewState["RANKID"] = rankid;
                ViewState["RELIEFDATE"] = reliefdate;
                ViewState["VSLTYPE"] = vsltype;

                Response.Redirect("../CrewOffshore/CrewOffshoreReliever.aspx?hidemenu=" + ViewState["hidemenu"] + "&launchedfrom=" + ViewState["launchedfrom"] + "&pl=" + ViewState["pl"] + "&rankid=" + ViewState["RANKID"].ToString() + "&vesselid=" + ViewState["VESSELID"].ToString() + "&offsignerid=" + empid + "&reliefdate=" + ViewState["RELIEFDATE"] + "&vsltype=" + vsltype, false);
            }

            if (e.CommandName.ToUpper().Equals("SELECTROW"))
            {

                string empid = ((Label)e.Item.FindControl("lblEmployeeId")).Text;
                string vesselid = ((Label)e.Item.FindControl("lblVesselId")).Text;
                string rankid = ((Label)e.Item.FindControl("lblRankid")).Text;
                string reliefdate = ((Label)e.Item.FindControl("lblReliefDue1")).Text;


                ViewState["EMPID"] = empid;
                ViewState["VESSELID"] = vesselid;
                ViewState["RANKID"] = rankid;
                ViewState["RELIEFDATE"] = reliefdate;
            }
            if (e.CommandName.ToUpper() == "CREWCHANGEREQUEST")
            {
                string vesselid = ((Label)e.Item.FindControl("lblVesselId")).Text;

                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ucRank", string.Empty);//ucRank.selectedlist);
                criteria.Add("ucVessel", vesselid);
                criteria.Add("ucZone", string.Empty);//ucZone.selectedlist);
                Filter.CurrentCrewChangePlanFilterSelection = criteria;

                Response.Redirect("../Crew/CrewChangeRequest.aspx" + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty) + (Request.QueryString["access"] != null ? "&access=1" : string.Empty), false);
            }
            if (e.CommandName.ToUpper() == "SELECTEVENT")
            {
                string eventid = ((Label)e.Item.FindControl("lblEventId")).Text;

                String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewPlanEventDetail.aspx?eventid=" + eventid + "');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);
            }

            else if (e.CommandName.ToUpper() == "APPROVETRAVEL")
            {

                BindData();
                gvSearch.Rebind();
            }
            else if (e.CommandName.ToUpper() == "APPROVESIGNON")
            {

                BindData();
                gvSearch.Rebind();
            }
            else if (e.CommandName.ToUpper() == "SEND")
            {
                string employeeid = ((Label)e.Item.FindControl("lblRelieverId")).Text;
                string rankid = ((Label)e.Item.FindControl("lblRankid")).Text;
                string vesselid = ((Label)e.Item.FindControl("lblVesselId")).Text;
                string offsignerid = ((Label)e.Item.FindControl("lblEmployeeId")).Text;
                string joindate = ((Label)e.Item.FindControl("lblJoinDate")).Text;

                PhoenixCrewOffshoreCrewChange.CrewSignOnInsert(null,
                                                                    General.GetNullableInteger(employeeid),
                                                                    int.Parse(rankid),
                                                                    int.Parse(vesselid),
                                                                    General.GetNullableInteger(offsignerid)
                                                                    , null
                                                                    , General.GetNullableInteger(null)
                                                                    , null
                                                                    , null
                                                                    , DateTime.Parse(joindate)
                                                                    , null
                                                                    , null
                                                                    , null
                                                                    , null//chkAdditionalRank.Checked ? byte.Parse("1") : byte.Parse("0")
                                                                    , null
                                                                    , null
                                                                    , null
                                                                    , null
                                                                    );
                PhoenixVesselAccountsEmployee.SendCrewDataToVessel(General.GetNullableInteger(employeeid).Value, int.Parse(vesselid), null);
                ucStatus.Text = "Information sent to vessel successfully.";

                BindData();
                gvSearch.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper() == "UPDATE")
            {

                try
                {
                    string crewplanid = ((Label)e.Item.FindControl("lblCrewPlanId")).Text;
                    string joindate = ((UserControlDate)e.Item.FindControl("txtJoiningDate")).Text;
                    string joinport = ((UserControlSeaport)e.Item.FindControl("ddlPlannedPort")).SelectedSeaport;
                    string OfferLetterSigned = ((CheckBox)e.Item.FindControl("chkOfferLetterSigned")).Checked == true ? "1" : "0";

                    if (!isValidUpdate(joindate))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixCrewOffshoreCrewChange.UpdateCrewPlan(new Guid(crewplanid), DateTime.Parse(joindate), General.GetNullableInteger(joinport)
                        , General.GetNullableInteger(OfferLetterSigned));
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }

                BindData();
                gvSearch.Rebind();
            }
            else if (e.CommandName.ToUpper() == "DELETE")
            {
                try
                {


                    string crewPlanId = ((Label)e.Item.FindControl("lblCrewPlanId")).Text;
                    string empid = ((Label)e.Item.FindControl("lblRelieverId")).Text;
                    PhoenixCrewPlanning.DeleteCrewPlan(new Guid(crewPlanId), int.Parse(empid));
                    BindData();
                    gvSearch.Rebind();

                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem || e.Item is GridNestedViewItem)
        {

            // Get the LinkButton control in the first cell

            DataRowView drv = (DataRowView)e.Item.DataItem;
            Label lblCrewPlanId = (Label)e.Item.FindControl("lblCrewPlanId");
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            Label lblRelieveeId = (Label)e.Item.FindControl("lblEmployeeId");
            Label lblRelieverId = (Label)e.Item.FindControl("lblRelieverId");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkRelievee");
            Label empid = (Label)e.Item.FindControl("lblEmployeeId");
            Label relieverid = (Label)e.Item.FindControl("lblRelieverId");
            Label VesselId = (Label)e.Item.FindControl("lblVesselId");
            Label rank = (Label)e.Item.FindControl("lblRankId");
            Label joindate = (Label)e.Item.FindControl("lblJoinDate");
            Label lblpdstatus = (Label)e.Item.FindControl("lblPDStatus");
            Label lblRelieverRankId = (Label)e.Item.FindControl("lblRelieverRankId");
            LinkButton pd = (LinkButton)e.Item.FindControl("cmdPDForm");
            LinkButton sho = (LinkButton)e.Item.FindControl("cmdShow");
            Label lblPDStatusID = (Label)e.Item.FindControl("lblPDStatusID");
            LinkButton md = (LinkButton)e.Item.FindControl("cmdMedical");
            LinkButton co = (LinkButton)e.Item.FindControl("cmdCourse");
            LinkButton cmdIniTravel = (LinkButton)e.Item.FindControl("cmdIniTravel");
            LinkButton cmdApproveTravel = (LinkButton)e.Item.FindControl("cmdApproveTravel");
            LinkButton cmdOfferLetter = (LinkButton)e.Item.FindControl("cmdOfferLetter");
            LinkButton cmdApproveSignOn = (LinkButton)e.Item.FindControl("cmdApproveSignOn");
            LinkButton cmdAppLetter = (LinkButton)e.Item.FindControl("cmdAppLetter");
            LinkButton cmdCancelAppointment = (LinkButton)e.Item.FindControl("cmdCancelAppointment");
            UserControlCommonToolTip ucCommonToolTip = (UserControlCommonToolTip)e.Item.FindControl("ucCommonToolTip");
            LinkButton cmdAppointmentLetter = (LinkButton)e.Item.FindControl("cmdAppointmentLetter");
            Label lblRelieverNationality = (Label)e.Item.FindControl("lblRelieverNationality");
            Label lblDailyRateReliever = (Label)e.Item.FindControl("lblDailyRateReliever");
            Label lblcurrency = (Label)e.Item.FindControl("lblcurrency");
            Label lblOnboardDays = (Label)e.Item.FindControl("lblOnboardDays");
            LinkButton ccmdDocChecklist = (LinkButton)e.Item.FindControl("cmdDocChecklist");


            UserControlToolTip ucToolTipNW = (UserControlToolTip)e.Item.FindControl("ucToolTipNW");
            string[] alColumns = { "FLDBUDGETTEDWAGE", "FLD1STTEARSCALE" };
            string[] alCaptions = { "Budget Wages", "1st Year Scale" };
            DataTable dt1 = new DataTable();
            if (lblCrewPlanId != null)
            {
                dt1 = PhoenixCrewOffshoreApproveProposal.OffshorePDInfo(General.GetNullableGuid(lblCrewPlanId.Text));
            }

            if (dt1.Rows.Count > 0 && ucToolTipNW != null)
            {
                string html = "<table>";
                //add header row
                //html += "<tr>";
                //for (int i = 0; i < 2; i++)
                //    html += "<td>" + dt.Columns[i].ColumnName + "</td>";
                //html += "</tr>";
                //add rows

                html += "<tr>";
                html += "<td>" + "Budget Wages: " + dt1.Rows[0][0].ToString() + "</td>";
                html += "</tr>";
                html += "<tr>";
                html += "<td>" + "1st Year Scale: " + dt1.Rows[0][1].ToString() + "</td>";
                html += "</tr>";

                html += "</table>";
                ucToolTipNW.Text = html;//dt1.Rows[0][0].ToString();//General.ShowGrid(dt1, alColumns, alCaptions);
                ucToolTipNW.Position = ToolTipPosition.TopCenter;
                //ucToolTipNW.TargetControlId = lblPlannedVessel.ClientID;
            }
            else
               if (ucToolTipNW != null) ucToolTipNW.Text = "no records";//General.ShowGrid(dt1, alColumns, alCaptions);
                                                                        //imgnotes.ToolTip = dt1.ToString();// General.ShowGrid(dt1, alColumns, alCaptions);
            if (ucToolTipNW != null) ucToolTipNW.Position = ToolTipPosition.TopCenter;
            //ucToolTipNW.TargetControlId = lblPlannedVessel.ClientID;
            //if (lblDailyRateReliever != null) lblDailyRateReliever.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'visible');");
            //if (lblDailyRateReliever != null) lblDailyRateReliever.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'hidden');");

            LinkButton cmdRemark = (LinkButton)e.Item.FindControl("cmdRemark");
            if (cmdRemark != null && lblCrewPlanId != null && lblRelieverId != null)
                cmdRemark.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreReliefPlanRemarks.aspx?crewplanid=" + lblCrewPlanId.Text + "&empid=" + lblRelieverId.Text + "','medium'); return true;");

            //RadCheckBox chkPlan = (RadCheckBox)e.Item.FindControl("chkPlan");
            //if (lblCrewPlanId != null && lblCrewPlanId.Text != "")
            //{
            //    if (chkPlan != null)
            //    {
            //        chkPlan.Enabled = true;
            //    }
            //}


            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
            && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                if (lbr != null) lbr.Attributes.Add("onclick", "javascript:parent.openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblRelieveeId.Text + "&launchedfrom=offshore'); return true;");

                LinkButton lbrv = (LinkButton)e.Item.FindControl("lnkReliever");
                if (lbrv != null) lbrv.Attributes.Add("onclick", "javascript:parent.openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblRelieverId.Text + "&launchedfrom=offshore" + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "'); return true;");

                if (drv["FLDRELIEFDUEDATE"].ToString() != "")
                {
                    if (Convert.ToDateTime(drv["FLDRELIEFDUEDATE"]) < DateTime.Now.AddDays(-1))
                    {
                        Label reliefduedate = (Label)e.Item.FindControl("lblReliefDue");
                        if (reliefduedate != null) reliefduedate.Attributes.Add("style", "color:red !important;");  //reliefduedate.CssClass = "redfont"; }
                    }
                }



                if ((drv["FLDISDEFINEDNATIONALITY"] != null && drv["FLDISDEFINEDNATIONALITY"].ToString().Equals("0")) || (drv["FLDISDEFINEDRATE"] != null && drv["FLDISDEFINEDRATE"].ToString().Equals("0")))
                {
                    if (lbrv != null)
                        lbrv.Attributes.Add("style", "color:red !important;");//lbrv.CssClass = "redfont";
                }

                if (drv["FLDEXCEEDMAXDUTY"].ToString().Equals("1"))
                {
                    if (lblOnboardDays != null) lblOnboardDays.Attributes.Add("style", "color:red !important;");//lblOnboardDays.CssClass = "redfont";
                }

                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to deplan the seafarer ?')");
                    if (lblCrewPlanId.Text == string.Empty) { db.Visible = false; eb.Visible = false; }
                    else
                    {
                        foreach (TableCell c in e.Item.Cells)
                            c.CssClass = "bluefont";                    
                    }
                }
                else
                {
                    e.Item.Attributes["onclick"] = "";
                }

                if (drv["FLDCREWPLANID"].ToString() == "" && eb != null)
                {
                    eb.Visible = false;
                    cmdRemark.Visible = false;
                }

                string newapplicant = "";

                if (lblRelieverId != null && !string.IsNullOrEmpty(lblRelieverId.Text))
                {
                    if (lblPDStatusID != null && lblPDStatusID.Text != PhoenixCommonRegisters.GetHardCode(1, 99, "AWA") && lblPDStatusID.Text != PhoenixCommonRegisters.GetHardCode(1, 99, "APR")) // Proposed,Approval rejected
                    {
                        if (co != null)
                        {
                            if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != null)
                            {
                                co.Attributes.Add("onclick", "javascript:parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreCourseMissing.aspx?empid=" + lblRelieverId.Text + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                                co.Visible = true;
                            }
                            else
                            {
                                co.Attributes.Add("onclick", "javascript:openNewWindow('chml', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreCourseMissing.aspx?empid=" + lblRelieverId.Text + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                                co.Visible = true;
                            }
                        }
                    }
                    else
                        if (co != null) co.Visible = false;
                }

                if (relieverid != null && !string.IsNullOrEmpty(relieverid.Text))
                {
                    if (pd != null) pd.Attributes.Add("onclick", "parent.openNewWindow('CrewPage', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=EMPLOYEECV&employeeid=" + relieverid.Text + "&vslid=" + VesselId.Text + "&rankid=" + rank.Text + "');return false;");
                    if (ucCommonToolTip != null) ucCommonToolTip.Visible = true;
                }
                else
                {
                    if (pd != null) pd.Visible = false;
                    if (ucCommonToolTip != null) ucCommonToolTip.Visible = false;

                }

                DataTable dt = new DataTable();
                if (lblCrewPlanId != null) dt = PhoenixCrewOffshoreCrewChange.WaivedDocumentList(General.GetNullableGuid(lblCrewPlanId.Text));
                if (dt.Rows.Count == 0)
                {
                    if (ucCommonToolTip != null)
                        ucCommonToolTip.Visible = false;
                }
                else
                {
                    if (ucCommonToolTip != null)
                        ucCommonToolTip.Visible = true;
                }

                if (empid != null && empid.Text == string.Empty && sho != null) sho.Visible = false;
                if (lblRelieverId != null && lblRelieverId.Text == string.Empty && cmdAppointmentLetter != null) cmdAppointmentLetter.Visible = false;

                if (lblCrewPlanId != null && lblCrewPlanId.Text == "")
                {
                    db.Visible = false;
                }

                if (cmdAppLetter != null && lblRelieverId != null)
                {
                    cmdAppLetter.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreAppointmentLetter.aspx?employeeid="
                        + lblRelieverId.Text
                        + "&crewplanid=" + drv["FLDCREWPLANID"].ToString()
                        + "&appointmentletterid=" + drv["FLDAPPOINTMENTLETTERID"].ToString()
                        + "&popup=1" + "');return false;");
                }
                if (cmdCancelAppointment != null && lblRelieverId != null)
                {
                    cmdCancelAppointment.Attributes.Add("onclick", "javascript:parent.openNewWindow('CAPP','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreCancelAppointmentReasonUpdate.aspx?CREWPLANID=" + drv["FLDCREWPLANID"].ToString()
                                + "&APPOINTMENTLETTERID=" + drv["FLDAPPOINTMENTLETTERID"].ToString()
                                + "&EMPLOYEEID=" + lblRelieverId.Text + "','medium'); return true;");
                }
                if (cmdAppointmentLetter != null)
                {
                    cmdAppointmentLetter.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=11&reportcode=APPOINMENTLETTERPDF&showmenu=0&showword=0&showexcel=0"
                        + "&crewplanid=" + drv["FLDCREWPLANID"].ToString()
                        + "&appointmentletterid=" + drv["FLDAPPOINTMENTLETTERID"].ToString()
                        + "&popup=1" + "');return false;");
                    //cmdAppointmentLetter.Attributes.Add("onclick", "parent.Openpopup('chml', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=11&reportcode=APPOINMENTLETTER&showmenu=0&showword=0&showexcel=0"
                    //    + "&crewplanid=554AA460-ECD1-E411-A56C-D8D385A9EF98"
                    //    + "&appointmentletterid=85DB2437-EDD1-E411-A56C-D8D385A9EF98"
                    //    + "&popup=1" + "');return false;");
                }
                if (lblCrewPlanId != null && lblCrewPlanId.Text != "" && md != null && relieverid != null) // crew change plan buttons
                {
                    if (!string.IsNullOrEmpty(relieverid.Text))
                    {
                        if (lblPDStatusID != null && lblPDStatusID.Text != PhoenixCommonRegisters.GetHardCode(1, 99, "AWA") && lblPDStatusID.Text != PhoenixCommonRegisters.GetHardCode(1, 99, "APR")) // Proposed, Approval rejected
                        {
                            if (md != null && lblRelieverId != null) md.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/Crew/CrewMedicalSlip.aspx?empid=" + lblRelieverId.Text + "&vslid=" + VesselId.Text + "');return false;");
                            if (md != null) md.Visible = true;
                        }
                        else
                        {
                            if (md != null) md.Visible = false;
                        }
                    }
                    else
                        if (md != null) md.Visible = false;

                    if (drv["FLDNEWAPP"].ToString() == "1")
                    {
                        newapplicant = "1";
                    }
                    string personalmaster = "";

                    if (newapplicant == "1")
                    {
                        personalmaster = "";
                    }
                    else
                    {
                        personalmaster = "1";
                        newapplicant = "";
                    }

                    if (lblPDStatusID != null && lblPDStatusID.Text == PhoenixCommonRegisters.GetHardCode(1, 99, "APV")) // Approval for vessel
                    {
                        if (ccmdDocChecklist != null)
                        {
                            ccmdDocChecklist.Visible = true;
                            ccmdDocChecklist.Attributes.Add("onclick", "javascript:parent.openNewWindow('CAPP','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreOfficeDocumentChecklist.aspx?Crewplanid=" + drv["FLDCREWPLANID"].ToString() + "'); return true;");
                        }
                        LinkButton email = (LinkButton)e.Item.FindControl("cmdDocumentCheckLIstmail");
                        if (email != null)
                        {
                            email.Visible = true;
                            email.Visible = SessionUtil.CanAccess(this.ViewState, email.CommandName);

                            email.Attributes.Add("onclick", "javascript:return openNewWindow('VesselAccounts','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDocumentCheckListMail.aspx?Crewplanid=" + drv["FLDCREWPLANID"].ToString() + "'); return false;");
                        }
                        if (cmdOfferLetter != null)
                        {
                            cmdOfferLetter.Visible = true;
                            cmdOfferLetter.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreOfferLetter.aspx?employeeid="
                            + lblRelieverId.Text
                            + "&crewplanid=" + drv["FLDCREWPLANID"].ToString()
                            + "&appointmentletterid=" + drv["FLDAPPOINTMENTLETTERID"].ToString()
                            + "&popup=1" + "');return false;");
                        }
                        if (cmdApproveTravel != null)
                        {
                            cmdApproveTravel.Visible = true;
                            cmdApproveTravel.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?empid=" + relieverid.Text
                                + "&vesselid=" + VesselId.Text
                                + "&rankid=" + rank.Text
                                + "&crewplanid=" + drv["FLDCREWPLANID"].ToString()
                                + "&trainingmatrixid=" + drv["FLDTRAININGMATRIXID"].ToString()
                                + "&offsignerid=" + empid.Text
                                + "&reliefdate=" + joindate.Text
                                + "&personalmaster=" + personalmaster
                                + "&newapplicant=" + newapplicant
                                + "&popup=1');return false;");
                        }

                        if (cmdIniTravel != null) cmdIniTravel.Visible = true;
                        if (cmdApproveSignOn != null) cmdApproveSignOn.Visible = false;
                        if (cmdAppLetter != null) cmdAppLetter.Visible = false;
                        if (cmdCancelAppointment != null) cmdCancelAppointment.Visible = false;
                    }
                    else if (lblPDStatusID != null && lblPDStatusID.Text == PhoenixCommonRegisters.GetHardCode(1, 99, "AFT")) // Approved for Travel
                    {
                        if (ccmdDocChecklist != null)
                        {
                            ccmdDocChecklist.Visible = true;
                            ccmdDocChecklist.Attributes.Add("onclick", "javascript:parent.openNewWindow('CAPP','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreOfficeDocumentChecklist.aspx?Crewplanid=" + drv["FLDCREWPLANID"].ToString() + "'); return true;");
                        }
                        if (cmdApproveTravel != null) cmdApproveTravel.Visible = false;
                        if (cmdAppLetter != null) cmdAppLetter.Visible = true;
                        if (cmdCancelAppointment != null) cmdCancelAppointment.Visible = true;
                        if (!string.IsNullOrEmpty(relieverid.Text))
                        {
                            if (cmdIniTravel != null) cmdIniTravel.Visible = true;
                            if (cmdApproveSignOn != null)
                            {
                                cmdApproveSignOn.Visible = true;
                                cmdApproveSignOn.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?empid=" + relieverid.Text
                                + "&vesselid=" + VesselId.Text
                                + "&rankid=" + rank.Text
                                + "&crewplanid=" + drv["FLDCREWPLANID"].ToString()
                                + "&trainingmatrixid=" + drv["FLDTRAININGMATRIXID"].ToString()
                                + "&offsignerid=" + empid.Text
                                + "&reliefdate=" + joindate.Text
                                + "&personalmaster=" + personalmaster
                                + "&newapplicant=" + newapplicant
                                + "&popup=1');return false;");
                            }
                        }
                    }
                    else if (lblPDStatusID != null && lblPDStatusID.Text == PhoenixCommonRegisters.GetHardCode(1, 99, "TLG")) // Travelling
                    {
                        if (cmdApproveTravel != null) cmdApproveTravel.Visible = false;
                        if (cmdAppLetter != null) cmdAppLetter.Visible = true;
                        if (cmdCancelAppointment != null) cmdCancelAppointment.Visible = false;
                        if (!string.IsNullOrEmpty(relieverid.Text))
                        {
                            if (cmdIniTravel != null) cmdIniTravel.Visible = true;
                            if (cmdApproveSignOn != null)
                            {
                                cmdApproveSignOn.Visible = true;
                                cmdApproveSignOn.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?empid=" + relieverid.Text
                                + "&vesselid=" + VesselId.Text
                                + "&rankid=" + rank.Text
                                + "&crewplanid=" + drv["FLDCREWPLANID"].ToString()
                                + "&trainingmatrixid=" + drv["FLDTRAININGMATRIXID"].ToString()
                                + "&offsignerid=" + empid.Text
                                + "&reliefdate=" + joindate.Text
                                + "&personalmaster=" + personalmaster
                                + "&newapplicant=" + newapplicant
                                + "&popup=1');return false;");
                            }
                        }
                    }
                    else if (lblPDStatusID != null && lblPDStatusID.Text == PhoenixCommonRegisters.GetHardCode(1, 99, "AFS")) // Approved for sign on                        
                    {
                        if (cmdApproveTravel != null) cmdApproveTravel.Visible = false;
                        if (cmdApproveSignOn != null) cmdApproveSignOn.Visible = false;
                        if (cmdIniTravel != null) cmdIniTravel.Visible = false;
                        if (cmdAppLetter != null) cmdAppLetter.Visible = true;
                        if (cmdCancelAppointment != null) cmdCancelAppointment.Visible = true;
                        if (eb != null) eb.Visible = false;
                        if (cmdRemark != null) cmdRemark.Visible = false;
                        if (db != null) db.Visible = false;
                        if (md != null) md.Visible = false;
                        if (co != null) co.Visible = false;
                        if (sho != null) sho.Visible = false;
                        if (cmdCancelAppointment != null)
                        {
                            cmdCancelAppointment.Attributes.Add("onclick", "javascript:parent.openNewWindow('CAPP','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreCancelAppointmentReasonUpdate.aspx?CREWPLANID=" + drv["FLDCREWPLANID"].ToString()
                                + "&APPOINTMENTLETTERID=" + drv["FLDAPPOINTMENTLETTERID"].ToString()
                                + "&EMPLOYEEID=" + lblRelieverId.Text
                                + "&ISCREWYN=1" + "','medium'); return true;");
                        }
                    }
                }

                Label lblSTCWFlag = (Label)e.Item.FindControl("lblSTCWFlag");
                Label lblCharterer = (Label)e.Item.FindControl("lblCharterer");
                Label lblCompany = (Label)e.Item.FindControl("lblCompany");
                Label lblTrainingmatrixid = (Label)e.Item.FindControl("lblTrainingmatrixid");
                Image imgFlagP = (Image)e.Item.FindControl("ImgFlagP");
                Image imgFlagT = (Image)e.Item.FindControl("ImgFlagT");
                Image imgFlagS = (Image)e.Item.FindControl("ImgFlagS");
                Image imgFlagA = (Image)e.Item.FindControl("ImgFlagA");
                if (relieverid != null && !string.IsNullOrEmpty(relieverid.Text))
                {
                    if (imgFlagP != null) imgFlagP.Attributes.Add("onclick", "parent.openNewWindow('course', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreComplianceCheckList.aspx?employeeid=" + relieverid.Text + "&trainingmatrixid=" + lblTrainingmatrixid.Text + "&stage=1&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                    if (imgFlagT != null) imgFlagT.Attributes.Add("onclick", "parent.openNewWindow('course', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreComplianceCheckList.aspx?employeeid=" + relieverid.Text + "&trainingmatrixid=" + lblTrainingmatrixid.Text + "&stage=2&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                    if (imgFlagS != null) imgFlagS.Attributes.Add("onclick", "parent.openNewWindow('course', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreComplianceCheckList.aspx?employeeid=" + relieverid.Text + "&trainingmatrixid=" + lblTrainingmatrixid.Text + "&stage=3&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                    if (imgFlagA != null) imgFlagA.Attributes.Add("onclick", "parent.openNewWindow('course', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreComplianceCheckList.aspx?employeeid=" + relieverid.Text + "&trainingmatrixid=" + lblTrainingmatrixid.Text + "&stage=4&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                }

                if (drv["FLDPROPOSECOMPLIANCE"] != null && drv["FLDPROPOSECOMPLIANCE"].ToString() != "" && imgFlagP != null)
                {
                    if (drv["FLDPROPOSECOMPLIANCE"].ToString().Equals("1"))
                    {
                        imgFlagP.ImageUrl = Session["images"] + "/Green.png";
                        imgFlagP.Visible = true;
                    }
                    else if (drv["FLDPROPOSECOMPLIANCE"].ToString().Equals("2"))
                    {
                        imgFlagP.ImageUrl = Session["images"] + "/Yellow.png";
                        imgFlagP.Visible = true;
                    }
                    else if (drv["FLDPROPOSECOMPLIANCE"].ToString().Equals("3"))
                    {
                        imgFlagP.ImageUrl = Session["images"] + "/Red.png";
                        imgFlagP.Visible = true;

                    }
                    else
                    {
                        imgFlagP.Visible = false;

                    }
                }

                if (drv["FLDTRAVELCOMPLIANCE"] != null && drv["FLDTRAVELCOMPLIANCE"].ToString() != "" && imgFlagT != null)
                {
                    if (drv["FLDTRAVELCOMPLIANCE"].ToString().Equals("1"))
                    {
                        imgFlagT.ImageUrl = Session["images"] + "/Green.png";
                        imgFlagT.Visible = true;
                    }
                    else if (drv["FLDTRAVELCOMPLIANCE"].ToString().Equals("2"))
                    {
                        imgFlagT.ImageUrl = Session["images"] + "/Yellow.png";
                        imgFlagT.Visible = true;
                    }
                    else if (drv["FLDTRAVELCOMPLIANCE"].ToString().Equals("3"))
                    {
                        imgFlagT.ImageUrl = Session["images"] + "/Red.png";
                        imgFlagT.Visible = true;
                    }
                    else
                    {
                        imgFlagT.Visible = false;
                    }
                }

                if (drv["FLDSIGNONCOMPLIANCE"] != null && drv["FLDSIGNONCOMPLIANCE"].ToString() != "" && imgFlagS != null)
                {
                    if (drv["FLDSIGNONCOMPLIANCE"].ToString().Equals("1"))
                    {
                        imgFlagS.ImageUrl = Session["images"] + "/Green.png";
                        imgFlagS.Visible = true;

                    }
                    else if (drv["FLDSIGNONCOMPLIANCE"].ToString().Equals("2"))
                    {
                        imgFlagS.ImageUrl = Session["images"] + "/Yellow.png";
                        imgFlagS.Visible = true;

                    }
                    else if (drv["FLDSIGNONCOMPLIANCE"].ToString().Equals("3"))
                    {
                        imgFlagS.ImageUrl = Session["images"] + "/Red.png";
                        imgFlagS.Visible = true;

                    }
                    else
                    {
                        imgFlagS.Visible = false;
                    }
                }
                if (drv["FLDAPPROVALCOMPLIANCE"] != null && drv["FLDAPPROVALCOMPLIANCE"].ToString() != "" && imgFlagA != null)
                {
                    if (drv["FLDAPPROVALCOMPLIANCE"].ToString().Equals("1"))
                    {
                        imgFlagA.ImageUrl = Session["images"] + "/Green.png";
                        imgFlagA.Visible = true;
                    }
                    else if (drv["FLDAPPROVALCOMPLIANCE"].ToString().Equals("2"))
                    {
                        imgFlagA.ImageUrl = Session["images"] + "/Yellow.png";
                        imgFlagA.Visible = true;
                    }
                    else if (drv["FLDAPPROVALCOMPLIANCE"].ToString().Equals("3"))
                    {
                        imgFlagA.ImageUrl = Session["images"] + "/Red.png";
                        imgFlagA.Visible = true;

                    }
                    else
                    {
                        imgFlagA.Visible = false;

                    }
                }
            }
            else
            {
                e.Item.Attributes["onclick"] = "";
            }

            Label lblRemarks = (Label)e.Item.FindControl("lblRemarks");
            LinkButton imgRemarks = (LinkButton)e.Item.FindControl("imgRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
            if (imgRemarks != null)
            {
                if (lblRemarks != null)
                {
                    if (lblRemarks.Text != "")
                    {
                        imgRemarks.Visible = true;

                        if (uct != null)
                        {
                            uct.Position = ToolTipPosition.TopCenter;
                            uct.TargetControlId = imgRemarks.ClientID;
                            //imgRemarks.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                            //imgRemarks.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                        }
                    }
                    else
                        imgRemarks.Visible = false;
                }
            }

            HtmlImage imgAddCertOnSigner = (HtmlImage)e.Item.FindControl("imgAddCertOnSigner");
            HtmlImage imgAddCertOffSigner = (HtmlImage)e.Item.FindControl("imgAddCertOffSigner");

            DataRowView drvCert = (DataRowView)e.Item.DataItem;

            if (imgAddCertOffSigner != null)
            {
                if (General.GetNullableInteger(drvCert["FLDEMPLOYEEID"].ToString()) != null && General.GetNullableString(drvCert["FLDOFFSIGNERCERTIFICATE"].ToString()) != null)
                {
                    imgAddCertOffSigner.Visible = true;
                    imgAddCertOffSigner.Attributes.Add("onclick", "parent.openNewWindow('MoreInfo','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreAdditionalDocuments.aspx?empid=" + drvCert["FLDEMPLOYEEID"].ToString() + "')");
                }
                else
                    imgAddCertOffSigner.Visible = false;
            }
            if (imgAddCertOnSigner != null)
            {
                if (General.GetNullableInteger(drvCert["FLDRELIEVERID"].ToString()) != null && General.GetNullableString(drvCert["FLDONSIGNERCERTIFICATE"].ToString()) != null)
                {
                    imgAddCertOnSigner.Visible = true;
                    imgAddCertOnSigner.Attributes.Add("onclick", "parent.openNewWindow('MoreInfo','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreAdditionalDocuments.aspx?empid=" + drvCert["FLDRELIEVERID"].ToString() + "')");
                }
                else
                    imgAddCertOnSigner.Visible = false;
            }

            UserControlSeaport ucSeaPort = (UserControlSeaport)e.Item.FindControl("ddlPlannedPort");
            DataRowView drvSeaPort = (DataRowView)e.Item.DataItem;
            if (ucSeaPort != null)
                ucSeaPort.SelectedSeaport = drvSeaPort["FLDSEAPORTID"].ToString();

            LinkButton lnkRelievee = (LinkButton)e.Item.FindControl("lnkRelievee");
            Label lblName = (Label)e.Item.FindControl("lblName");
            LinkButton lnkReliever = (LinkButton)e.Item.FindControl("lnkReliever");
            Label lblRelieverName = (Label)e.Item.FindControl("lblRelieverName");

            if (drv["FLDOFFSIGNERVIEWPARTICULARSYN"] != null && drv["FLDOFFSIGNERVIEWPARTICULARSYN"].ToString().Equals("1") && lblName != null)
            {
                lblName.Visible = false;
                lnkRelievee.Visible = true;
            }
            else
            {
                if (lblName != null) lblName.Visible = true;
                if (lnkRelievee != null) lnkRelievee.Visible = false;
            }

            if (drv["FLDRELIEVERVIEWPARTICULARSYN"] != null && drv["FLDRELIEVERVIEWPARTICULARSYN"].ToString().Equals("1") && lblRelieverName != null && lblRelieverName != null)
            {
                lblRelieverName.Visible = false;
                lnkReliever.Visible = true;
            }
            else
            {
                if (lblRelieverName != null) lblRelieverName.Visible = true;
                if (lnkReliever != null) lnkReliever.Visible = false;
            }

            if ((drv["FLDISDEFINEDNATIONALITY"] != null && drv["FLDISDEFINEDNATIONALITY"].ToString().Equals("0")) || (drv["FLDISDEFINEDRATE"] != null && drv["FLDISDEFINEDRATE"].ToString().Equals("0")))
            {
                if (lblRelieverName != null) lblRelieverName.Attributes.Add("style", "color:red !important;"); //lblRelieverName.CssClass = "redfont"; }
            }

            if (drv["FLDISDEFINEDNATIONALITY"] != null && drv["FLDISDEFINEDNATIONALITY"].ToString().Equals("0"))
            {
                if (lblRelieverNationality != null) lblRelieverNationality.Attributes.Add("style", "color:red !important;");//  lblRelieverNationality.CssClass = "redfont";
            }
            if (drv["FLDISDEFINEDRATE"] != null && drv["FLDISDEFINEDRATE"].ToString().Equals("0"))
            {
                if (lblDailyRateReliever != null) { lblcurrency.Attributes.Add("style", "color:red !important;"); lblDailyRateReliever.Attributes.Add("style", "color:red !important;"); }//  lblDailyRateReliever.CssClass = "redfont";
            }

            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName)) eb.Visible = false;
            }
            if (cmdRemark != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdRemark.CommandName)) cmdRemark.Visible = false;
            }
            if (pd != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, pd.CommandName)) pd.Visible = false;
            }
            if (sho != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, sho.CommandName)) sho.Visible = false;
            }
            if (md != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, md.CommandName)) md.Visible = false;
            }
            if (co != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, co.CommandName)) co.Visible = false;
            }
            if (cmdIniTravel != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdIniTravel.CommandName)) cmdIniTravel.Visible = false;
            }
            if (cmdApproveTravel != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdApproveTravel.CommandName)) cmdApproveTravel.Visible = false;
            }
            if (cmdApproveSignOn != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdApproveSignOn.CommandName)) cmdApproveSignOn.Visible = false;
            }
            if (cmdAppLetter != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdAppLetter.CommandName)) cmdAppLetter.Visible = false;
            }
            if (cmdCancelAppointment != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCancelAppointment.CommandName)) cmdCancelAppointment.Visible = false;
            }
            if (cmdAppointmentLetter != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdAppointmentLetter.CommandName)) cmdAppointmentLetter.Visible = false;
            }
        }

    }



    protected void gvSearch_SortCommand(object sender, GridSortCommandEventArgs e)
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
}
