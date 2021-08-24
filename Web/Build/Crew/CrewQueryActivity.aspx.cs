using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;

public partial class CrewQueryActivity : PhoenixBasePage
{
    private int _ItemPerRequest = 25;
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            if (!IsPostBack)
            {
                ViewState["LAUNCHEDFROM"] = "";
                ViewState["pl"] = "";
                ViewState["RCBEMPLOYEENAME"] = "";

                if (Request.QueryString["p"] != null && Request.QueryString["p"].ToString() != string.Empty)
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["p"]);
                else
                    ViewState["PAGENUMBER"] = 1;

                if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                    ViewState["LAUNCHEDFROM"] = Request.QueryString["launchedfrom"].ToString();

                if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
                    ViewState["pl"] = Request.QueryString["pl"].ToString();

                ViewState["ADDEMPLOYEE"] = "0";
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                SetEmployeeAdd();

            }

            PhoenixToolbar toolbarsub = new PhoenixToolbar();

            toolbarsub.AddFontAwesomeButton("../Crew/CrewQueryActivity.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarsub.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewQueryActivityFilter.aspx'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarsub.AddFontAwesomeButton("../Crew/CrewQueryActivity.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Clear Filter", " <i class=\"fas fa-eraser\"></i>", "CLEAR");
            if (ViewState["ADDEMPLOYEE"].ToString() == "1")
            {
                toolbarsub.AddFontAwesomeButton("../Crew/CrewNewApplicantPersonalGeneral.aspx?t=n" + (Request.QueryString.ToString() != string.Empty ? ("&" + Request.QueryString.ToString()) : string.Empty) + "&Newapp=yes&AddAsEmployee=yes", "Add", "<i class=\"fas fa-user-plus\"></i>", "NEW APPLICANT");
            }
            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbarsub.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetEmployeeAdd()
    {
        try
        {

            DataTable ds = PhoenixRegisterCrewConfiguration.ListCrewConfiguration("PERSONNELADD", 1);

            if (ds.Rows.Count > 0)
            {
                ViewState["ADDEMPLOYEE"] = "1";
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewQueryMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                Filter.CurrentNewApplicantFilterCriteria = null;
                foreach (GridFilteringItem filterItem in gvCrewSearch.MasterTableView.GetItems(GridItemType.FilteringItem))
                {
                    RadComboBox rcb = (RadComboBox)filterItem.FindControl("rcbEmployeename");
                    ViewState["RCBEMPLOYEENAME"] = "";
                    rcb.SelectedIndex = -1;
                    rcb.ClearSelection();
                    rcb.ClearCheckedItems();
                    rcb.SelectedValue = string.Empty;
                    rcb.Items.Clear();
                }

                gvCrewSearch.CurrentPageIndex = 0;
                //BindData();
                gvCrewSearch.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
        Filter.CurrentCrewSelection = null;
        Session["REFRESHFLAG"] = null;
        Response.Redirect("..\\Crew\\CrewPersonalGeneral.aspx?t=n");
    }

    protected void gvCrewSearch_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
        BindData();
    }

    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper() == "GETEMPLOYEE")
            {

                Filter.CurrentCrewSelection = ((RadLabel)eeditedItem.FindControl("lblEmployeeid")).Text;
                string familyid = ((RadLabel)eeditedItem.FindControl("lblfamlyid")).Text;
                Session["REFRESHFLAG"] = null;
                if (familyid == "")
                {
                    Response.Redirect("..\\Crew\\CrewPersonalGeneral.aspx?p=" + ViewState["PAGENUMBER"].ToString() + "&back=yes&launchedfrom=" + ViewState["LAUNCHEDFROM"].ToString() + "&pl=" + ViewState["pl"].ToString());
                }
                else
                {
                    Response.Redirect("..\\Crew\\CrewPersonalGeneral.aspx?empid=" + Filter.CurrentCrewSelection + "&familyid=" + familyid + "&p=" + ViewState["PAGENUMBER"].ToString() + "&back=yes&launchedfrom=" + ViewState["LAUNCHEDFROM"].ToString() + "&pl=" + ViewState["pl"].ToString(), false);
                }

            }
            else if (e.CommandName.ToUpper() == "SENDMAIL")
            {
                try
                {
                    Filter.CurrentCrewSelection = ((RadLabel)eeditedItem.FindControl("lblEmployeeid")).Text;
                    PhoenixCrewLicenceRequest.EmployeeDocsSendMail(null, General.GetNullableInteger(Filter.CurrentCrewSelection.ToString()));
                    ucStatus.Text = "Mail sent successfully";
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper() == "PHOENIXSYNCLOGIN")
            {
                Filter.CurrentCrewSelection = ((RadLabel)eeditedItem.FindControl("lblEmployeeid")).Text;

                Response.Redirect("..\\Crew\\CrewServiceSyncLogin.aspx?empid=" + Filter.CurrentCrewSelection);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            RadLabel empid = (RadLabel)item.FindControl("lblEmployeeid");
            RadLabel status = (RadLabel)item.FindControl("lblStatus");
            RadLabel lbldirectsignon = (RadLabel)item.FindControl("lbldirectsignon");
            LinkButton sg = (LinkButton)item.FindControl("imgActivity");

            if (status != null && (status.Text.Contains("ONB") || status.Text.Contains("OBP")))
            {
                sg.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Activities', '" + Session["sitepath"] + "/Crew/CrewActivityGeneral.aspx?empid=" + empid.Text + "&sg=0&ntbr=0&ds=" + lbldirectsignon.Text + "');return false;");
            }
            else
            {
                sg.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Activities', '" + Session["sitepath"] + "/Crew/CrewActivityGeneral.aspx?empid=" + empid.Text + "&sg=0&ds=" + lbldirectsignon.Text + "');return false;");
            }

            if (sg != null) sg.Visible = SessionUtil.CanAccess(this.ViewState, sg.CommandName);

            LinkButton imgSuitableCheck = (LinkButton)item.FindControl("imgSuitableCheck");
            imgSuitableCheck.Attributes.Add("onclick", "parent.openNewWindow('codehelp', '', '" + Session["sitepath"] + "/Crew/CrewSuitabilityCheck.aspx?empid=" + empid.Text + "&personalmaster=true');return false;");

            LinkButton imgSendMail = (LinkButton)item.FindControl("imgSendMail");
            if (imgSendMail != null) imgSendMail.Visible = SessionUtil.CanAccess(this.ViewState, imgSendMail.CommandName);

            LinkButton ed = (LinkButton)item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            RadLabel lbtn = (RadLabel)item.FindControl("lblNextVessel");
            UserControlToolTip uct = (UserControlToolTip)item.FindControl("ucToolTipVesselNames");
            uct.Position = ToolTipPosition.TopCenter;
            uct.TargetControlId = lbtn.ClientID;

            RadLabel lbLastvessel = (RadLabel)item.FindControl("lblLastVesselName");
            UserControlToolTip uctLastVessel = (UserControlToolTip)item.FindControl("ucToolTipLastVessel");
            uctLastVessel.Position = ToolTipPosition.TopCenter;
            uctLastVessel.TargetControlId = lbLastvessel.ClientID;

            RadLabel lbPresentVessel = (RadLabel)item.FindControl("lblPresentVesselName");
            UserControlToolTip uctPresentVessel = (UserControlToolTip)item.FindControl("ucToolTipPresentVessel");
            uctPresentVessel.Position = ToolTipPosition.TopCenter;
            uctPresentVessel.TargetControlId = lbPresentVessel.ClientID;

            RadLabel lbts = (RadLabel)item.FindControl("lblStatus");
            UserControlToolTip ucts = (UserControlToolTip)item.FindControl("ucToolTipStatus");
            ucts.Position = ToolTipPosition.TopCenter;
            ucts.TargetControlId = lbts.ClientID;

            LinkButton pd = (LinkButton)item.FindControl("cmdPDForm");
            LinkButton imgUnallocatedVesselExp = (LinkButton)item.FindControl("imgUnAllocatedVslExp");

            if (pd != null)
            {

                pd.Visible = SessionUtil.CanAccess(this.ViewState, pd.CommandName);
                pd.Attributes.Add("onclick", "openNewWindow('PDForm', 'PD Form', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + empid.Text + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");
            }

            if (imgUnallocatedVesselExp != null)
            {
                imgUnallocatedVesselExp.Visible = SessionUtil.CanAccess(this.ViewState, imgUnallocatedVesselExp.CommandName);
                imgUnallocatedVesselExp.Attributes.Add("onclick", "openNewWindow('CrewUnallocatedVesselExpensesEmployee', '', '" + Session["sitepath"] + "/Crew/CrewUnallocatedVesselExpensesEmployee.aspx?empid=" + empid.Text + "');return false;");
            }

            if (ViewState["LAUNCHEDFROM"] != null && ViewState["LAUNCHEDFROM"].ToString() != "")
            {
                if (imgSuitableCheck != null) imgSuitableCheck.Visible = false;
            }
            else
            {
                if (imgSuitableCheck != null) imgSuitableCheck.Visible = true;
            }

            if (imgSuitableCheck != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgSuitableCheck.CommandName)) imgSuitableCheck.Visible = false;
            }

            LinkButton servicelogin = (LinkButton)item.FindControl("cmdServiceLogin");
            RadLabel fileno = (RadLabel)item.FindControl("lblEmployeeCode");
            if (servicelogin != null)
            {
                servicelogin.Visible = SessionUtil.CanAccess(this.ViewState, servicelogin.CommandName);
                //     servicelogin.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/CommonServiceSyncLogin.aspx?fileno=" + fileno.Text + "',false,500,500);return false;");
            }
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvCrewSearch.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAME", "FLDRANKPOSTEDNAME", "FLDEMPLOYEECODE", "FLDLASTVESSEL", "FLDLASTSIGNOFFDATE",
                               "FLDPRESENTVESSEL", "FLDPRESENTSIGNONDATE", "FLDPLANNEDVESSELNAMES", "FLDDOA", "FLDSTATUSDESCRIPTION","FLDZONE","FLDBATCHNO","FLDDECIMALEXPERIENCE" };
        string[] alCaptions = { "Name", "Rank", "File No.", "Last Vessel", "Sign Off", "Present Vessel",
                                  "Sign On","Next Vessel","D.O.A.","Status","Zone","Batch","Exp(M)"  };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentNewApplicantFilterCriteria;
        if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
        {
            if (nvc == null)
            {
                nvc = new NameValueCollection();
                nvc.Add("txtName", string.Empty);
                nvc.Add("txtFileNumber", string.Empty);
                nvc.Add("txtRefNumber", string.Empty);
                nvc.Add("txtPassortNo", string.Empty);
                nvc.Add("txtSeamanbookNo", string.Empty);
                nvc.Add("ddlSailedRank", string.Empty);
                nvc.Add("ddlVesselType", string.Empty);
                nvc.Add("txtAppliedStartDate", string.Empty);
                nvc.Add("txtAppliedEndDate", string.Empty);
                nvc.Add("txtDOAStartDate", string.Empty);
                nvc.Add("txtDOAEndDate", string.Empty);
                nvc.Add("txtDOBStartDate", string.Empty);
                nvc.Add("txtDOBEndDate", string.Empty);
                nvc.Add("ddlCourse", string.Empty);
                nvc.Add("ddlLicences", string.Empty);
                nvc.Add("ddlEngineType", string.Empty);
                nvc.Add("ddlVisa", string.Empty);
                nvc.Add("ddlZone", string.Empty);
                nvc.Add("lstRank", string.Empty);
                nvc.Add("lstNationality", string.Empty);
                nvc.Add("ddlPrevCompany", string.Empty);
                nvc.Add("ddlActiveYN", string.Empty);
                nvc.Add("ddlStatus", string.Empty);
                nvc.Add("ddlCountry", string.Empty);
                nvc.Add("ddlState", string.Empty);
                nvc.Add("ddlCity", string.Empty);
                nvc.Add("txtNOKName", string.Empty);
                nvc.Add("chkIncludepastexp", "0");
                nvc.Add("ddlPool", Request.QueryString["pl"]);
                nvc.Add("ddlVessel", string.Empty);
                nvc.Add("lstBatch", string.Empty);
                nvc.Add("ddlFlag", string.Empty);
                nvc.Add("ucPrincipal", string.Empty);
                nvc.Add("ucPrincipalntbr", string.Empty);
            }
            else
            {
                nvc["ddlPool"] = Request.QueryString["pl"];
            }
        }

        DataTable dt = PhoenixCommonCrew.QueryActivity(nvc != null ? nvc.Get("lstNationality") : string.Empty
                                                                   , nvc != null ? nvc.Get("txtPassortNo") : string.Empty
                                                                   , nvc != null ? nvc.Get("txtSeamanbookNo") : string.Empty
                                                                   , nvc != null ? nvc.Get("txtFileNumber") : string.Empty
                                                                   , nvc != null ? nvc.Get("lstRank") : string.Empty
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlZone") : string.Empty)
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlSailedRank") : string.Empty)
                                                                   , nvc != null ? nvc.Get("ddlVesselType") : string.Empty
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlEngineType") : string.Empty)
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCourse") : string.Empty)
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlLicences") : string.Empty)
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlVisa") : string.Empty)
                                                                   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtAppliedStartDate") : string.Empty)
                                                                   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtAppliedEndDate") : string.Empty)
                                                                   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOAStartDate") : string.Empty)
                                                                   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOAEndDate") : string.Empty)
                                                                   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOBStartDate") : string.Empty)
                                                                   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOBEndDate") : string.Empty)
                                                                   , nvc != null ? nvc.Get("txtRefNumber") : string.Empty, nvc != null ? nvc.Get("txtName") : string.Empty
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlPrevCompany") : string.Empty)
                                                                   , nvc != null ? nvc.Get("ddlStatus") : string.Empty
                                                                   , nvc != null ? (byte?)General.GetNullableInteger(nvc.Get("ddlActiveYN")) : null
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCountry") : string.Empty)
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlState") : string.Empty)
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCity") : string.Empty)
                                                                   , sortexpression, sortdirection
                                                                   , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                   , iRowCount
                                                                   , ref iRowCount, ref iTotalPageCount
                                                                   , nvc != null ? nvc.Get("txtNOKName") : string.Empty, null
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("chkIncludepastexp") : string.Empty)
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlVessel") : string.Empty)
                                                                   , nvc != null ? nvc.Get("ddlPool") : string.Empty
                                                                   , nvc != null ? nvc.Get("lstBatch") : string.Empty
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlFlag") : string.Empty)
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ucPrincipal") : string.Empty)
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ucPrincipalntbr") : string.Empty));

        General.ShowExcel("Personnel Master", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAME", "FLDRANKPOSTEDNAME", "FLDEMPLOYEECODE", "FLDLASTVESSEL", "FLDLASTSIGNOFFDATE",
                               "FLDPRESENTVESSEL", "FLDPRESENTSIGNONDATE", "FLDPLANNEDVESSELNAMES", "FLDDOA", "FLDSTATUSDESCRIPTION","FLDZONE","FLDBATCHNO","FLDDECIMALEXPERIENCE" };
        string[] alCaptions = { "Name", "Rank", "File No.", "Last Vessel", "Sign Off", "Present Vessel",
                                  "Sign On","Next Vessel","D.O.A.","Status","Zone","Batch","Exp(M)"  };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {

            NameValueCollection nvc = Filter.CurrentNewApplicantFilterCriteria;
            if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
            {
                if (nvc == null)
                {
                    nvc = new NameValueCollection();
                    nvc.Add("txtName", string.Empty);
                    nvc.Add("txtFileNumber", string.Empty);
                    nvc.Add("txtRefNumber", string.Empty);
                    nvc.Add("txtPassortNo", string.Empty);
                    nvc.Add("txtSeamanbookNo", string.Empty);
                    nvc.Add("ddlSailedRank", string.Empty);
                    nvc.Add("ddlVesselType", string.Empty);
                    nvc.Add("txtAppliedStartDate", string.Empty);
                    nvc.Add("txtAppliedEndDate", string.Empty);
                    nvc.Add("txtDOAStartDate", string.Empty);
                    nvc.Add("txtDOAEndDate", string.Empty);
                    nvc.Add("txtDOBStartDate", string.Empty);
                    nvc.Add("txtDOBEndDate", string.Empty);
                    nvc.Add("ddlCourse", string.Empty);
                    nvc.Add("ddlLicences", string.Empty);
                    nvc.Add("ddlEngineType", string.Empty);
                    nvc.Add("ddlVisa", string.Empty);
                    nvc.Add("ddlZone", string.Empty);
                    nvc.Add("lstRank", string.Empty);
                    nvc.Add("lstNationality", string.Empty);
                    nvc.Add("ddlPrevCompany", string.Empty);
                    nvc.Add("ddlActiveYN", string.Empty);
                    nvc.Add("ddlStatus", string.Empty);
                    nvc.Add("ddlCountry", string.Empty);
                    nvc.Add("ddlState", string.Empty);
                    nvc.Add("ddlCity", string.Empty);
                    nvc.Add("txtNOKName", string.Empty);
                    nvc.Add("chkIncludepastexp", "0");
                    nvc.Add("ddlPool", Request.QueryString["pl"]);
                    nvc.Add("ddlVessel", string.Empty);
                    nvc.Add("lstBatch", string.Empty);
                    nvc.Add("ddlFlag", string.Empty);
                    nvc.Add("ucPrincipal", string.Empty);
                    nvc.Add("ucPrincipalntbr", string.Empty);
                }
                else
                {
                    nvc["ddlPool"] = Request.QueryString["pl"];
                }
            }
            DataTable dt = PhoenixCommonCrew.QueryActivity(nvc != null ? nvc.Get("lstNationality") : string.Empty
                                                                       , nvc != null ? nvc.Get("txtPassortNo") : string.Empty
                                                                       , nvc != null ? nvc.Get("txtSeamanbookNo") : string.Empty
                                                                       , nvc != null ? nvc.Get("txtFileNumber") : string.Empty
                                                                       , nvc != null ? nvc.Get("lstRank") : string.Empty
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlZone") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlSailedRank") : string.Empty)
                                                                       , nvc != null ? nvc.Get("ddlVesselType") : string.Empty
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlEngineType") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCourse") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlLicences") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlVisa") : string.Empty)
                                                                       , General.GetNullableDateTime(nvc != null ? nvc.Get("txtAppliedStartDate") : string.Empty)
                                                                       , General.GetNullableDateTime(nvc != null ? nvc.Get("txtAppliedEndDate") : string.Empty)
                                                                       , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOAStartDate") : string.Empty)
                                                                       , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOAEndDate") : string.Empty)
                                                                       , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOBStartDate") : string.Empty)
                                                                       , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOBEndDate") : string.Empty)
                                                                       , nvc != null ? nvc.Get("txtRefNumber") : string.Empty, nvc != null ? nvc.Get("txtName") : string.Empty
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlPrevCompany") : string.Empty)
                                                                       , nvc != null ? nvc.Get("ddlStatus") : string.Empty
                                                                       , nvc != null ? (byte?)General.GetNullableInteger(nvc.Get("ddlActiveYN")) : null
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCountry") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlState") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCity") : string.Empty)
                                                                       , sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()), gvCrewSearch.PageSize
                                                                       , ref iRowCount, ref iTotalPageCount
                                                                       , nvc != null ? nvc.Get("txtNOKName") : string.Empty, null
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("chkIncludepastexp") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlVessel") : string.Empty)
                                                                       , nvc != null ? nvc.Get("ddlPool") : string.Empty
                                                                       , nvc != null ? nvc.Get("lstBatch") : string.Empty
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlFlag") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ucPrincipal") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ucPrincipalntbr") : string.Empty));


            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvCrewSearch", "Personnel Master", alCaptions, alColumns, ds);


            gvCrewSearch.DataSource = ds;
            gvCrewSearch.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void ResetFormControlValues(Control parent)
    {
        try
        {
            foreach (Control c in parent.Controls)
            {
                if (c.Controls.Count > 0)
                {
                    ResetFormControlValues(c);
                }
                else
                {
                    switch (c.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.TextBox":
                            ((TextBox)c).Text = "";
                            break;
                        case "System.Web.UI.WebControls.CheckBox":
                            ((CheckBox)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.RadioButton":
                            ((RadioButton)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.DropDownList":
                            ((DropDownList)c).SelectedIndex = 0;
                            break;
                        case "System.Web.UI.WebControls.ListBox":
                            ((ListBox)c).SelectedIndex = 0;
                            break;

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



    protected void rcbEmployeename_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        RadComboBox ddl = (RadComboBox)sender;
        DataTable dt = new DataTable();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int pageNumber = 1;
        int itemOffset = e.NumberOfItems;
        int count = 0;
        if (e.NumberOfItems > 0)
        {
            pageNumber = ((int)Math.Ceiling((decimal)itemOffset / _ItemPerRequest)) + 1;
        }
        NameValueCollection nvc = Filter.CurrentNewApplicantFilterCriteria;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }
        nvc.Add("txtFileNumber", e.Text);

        dt = PhoenixCommonCrew.QueryActivity(string.Empty, string.Empty, string.Empty, nvc != null ? nvc.Get("txtFileNumber") : string.Empty, string.Empty
                                             , General.GetNullableInteger(string.Empty), General.GetNullableInteger(string.Empty)
                                             , string.Empty, General.GetNullableInteger(string.Empty), General.GetNullableInteger(string.Empty)
                                             , General.GetNullableInteger(string.Empty), General.GetNullableInteger(string.Empty)
                                             , General.GetNullableDateTime(string.Empty), General.GetNullableDateTime(string.Empty)
                                             , General.GetNullableDateTime(string.Empty), General.GetNullableDateTime(string.Empty)
                                             , General.GetNullableDateTime(string.Empty), General.GetNullableDateTime(string.Empty)
                                             , string.Empty, string.Empty, General.GetNullableInteger(string.Empty), string.Empty
                                             , null, General.GetNullableInteger(string.Empty), General.GetNullableInteger(string.Empty)
                                             , General.GetNullableInteger(string.Empty), sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                             , gvCrewSearch.PageSize, ref iRowCount, ref iTotalPageCount
                                             , string.Empty, null, General.GetNullableInteger(string.Empty), General.GetNullableInteger(string.Empty)
                                             , string.Empty, string.Empty, General.GetNullableInteger(string.Empty), General.GetNullableInteger(string.Empty)
                                             , General.GetNullableInteger(string.Empty));
        count = dt.Rows.Count;
        e.EndOfItems = (itemOffset + count) == iRowCount;
        foreach (DataRow dr in dt.Rows)
        {
            ddl.Items.Add(new RadComboBoxItem(dr["FLDEMPLOYEECODE"].ToString(), dr["FLDEMPLOYEECODE"].ToString()));
        }
        ddl.DataSource = dt;
        ddl.DataBind();
        string message = string.Empty;
        if (iRowCount <= 0)
            message = "No matches";
        else
            message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", (itemOffset + count), iRowCount);
        e.Message = message;
    }

    protected void rcbEmployeename_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ViewState["RCBEMPLOYEENAME"] = (o as RadComboBox).SelectedValue;

        NameValueCollection nvc = Filter.CurrentNewApplicantFilterCriteria;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }
        nvc.Add("txtFileNumber", ViewState["RCBEMPLOYEENAME"].ToString());
        Filter.CurrentNewApplicantFilterCriteria = nvc;
        //gvCrewSearch.DataSource = null;
        gvCrewSearch.Rebind();
    }

    protected void rcbEmployeename_PreRender(object sender, EventArgs e)
    {
        if (ViewState["RCBEMPLOYEENAME"] != null)
        {
            (sender as RadComboBox).SelectedValue = ViewState["RCBEMPLOYEENAME"].ToString();
            (sender as RadComboBox).Text = ViewState["RCBEMPLOYEENAME"].ToString();
        }
    }

    protected void RadMenu1_ItemClick(object sender, RadMenuEventArgs e)
    {
        try
        {
            int radGridClickedRowIndex;

            radGridClickedRowIndex = Convert.ToInt32(Request.Form["radGridClickedRowIndex"]);

            GridDataItem item = gvCrewSearch.Items[radGridClickedRowIndex];

            Filter.CurrentCrewSelection = ((RadLabel)item.FindControl("lblEmployeeid")).Text;
            string familyid = ((RadLabel)item.FindControl("lblfamlyid")).Text;
            RadLabel status = (RadLabel)item.FindControl("lblStatus");
            RadLabel lbldirectsignon = (RadLabel)item.FindControl("lbldirectsignon");

            string Script = "";
            switch (e.Item.Value)
            {
                case "EDIT":

                    Session["REFRESHFLAG"] = null;
                    if (familyid == "")
                    {
                        Response.Redirect("..\\Crew\\CrewPersonalGeneral.aspx?p=" + ViewState["PAGENUMBER"].ToString() + "&back=yes&launchedfrom=" + ViewState["LAUNCHEDFROM"].ToString() + "&pl=" + ViewState["pl"].ToString());
                    }
                    else
                    {
                        Response.Redirect("..\\Crew\\CrewPersonalGeneral.aspx?empid=" + Filter.CurrentCrewSelection + "&familyid=" + familyid + "&p=" + ViewState["PAGENUMBER"].ToString() + "&back=yes&launchedfrom=" + ViewState["LAUNCHEDFROM"].ToString() + "&pl=" + ViewState["pl"].ToString(), false);
                    }
                    break;
                case "PD":

                    Script = "";
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += " setTimeout(function(){ openNewWindow('spnPickReason', 'codehelp1', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + Filter.CurrentCrewSelection + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0 ','false','900','600')},500)";
                    Script += "</script>" + "\n";

                    RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
                    break;
                case "SUC":

                    Script = "";
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += " setTimeout(function(){ openNewWindow('spnPickReason', 'codehelp1', '" + Session["sitepath"] + "/Crew/CrewSuitabilityCheck.aspx?empid=" + Filter.CurrentCrewSelection + "&personalmaster=true','false','900','600')},500)";
                    Script += "</script>" + "\n";

                    RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
                    break;
                case "ACT":
                    Script = "";
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";

                    if (status != null && (status.Text.Contains("ONB") || status.Text.Contains("OBP")))
                    {
                        Script += "setTimeout(function(){ openNewWindow('spnPickReason', 'codehelp1', '" + Session["sitepath"] + "/Crew/CrewActivityGeneral.aspx?empid=" + Filter.CurrentCrewSelection + "&sg=0&ntbr=0&ds=" + lbldirectsignon.Text + "','false','900','600')},500)";
                    }
                    else
                    {
                        Script += "setTimeout(function(){ openNewWindow('spnPickReason', 'codehelp1', '" + Session["sitepath"] + "/Crew/CrewActivityGeneral.aspx?empid=" + Filter.CurrentCrewSelection + "&sg=0&ds=" + lbldirectsignon.Text + "','false','900','600')},500)";
                    }
                    Script += "</script>" + "\n";

                    RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
                    break;

                case "MAIL":

                    PhoenixCrewLicenceRequest.EmployeeDocsSendMail(null, General.GetNullableInteger(Filter.CurrentCrewSelection.ToString()));
                    ucStatus.Text = "Mail sent successfully";
                    ucStatus.Visible = false;
                    //gvCrewSearch.DataSource = null;
                    //gvCrewSearch.Rebind();
                    break;

                case "UVE":

                    Script = "";
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += " setTimeout(function(){ openNewWindow('spnPickReason', 'codehelp1', '" + Session["sitepath"] + "/Crew/CrewUnallocatedVesselExpensesEmployee.aspx?empid=" + Filter.CurrentCrewSelection + "','false','900','600')},500)";
                    Script += "</script>" + "\n";

                    RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
                    break;

                case "PHXSYNC":

                    Response.Redirect("..\\Crew\\CrewServiceSyncLogin.aspx?empid=" + Filter.CurrentCrewSelection);
                    break;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }




    }
}
