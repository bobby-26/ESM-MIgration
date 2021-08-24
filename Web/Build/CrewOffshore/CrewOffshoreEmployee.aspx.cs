using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CrewOffshoreEmployee : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreEmployee.aspx?" + Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreEmployee.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreEmployee.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                ViewState["LAUNCHEDFROM"] = "";
                ViewState["pl"] = "";
                ViewState["sid"] = "";

                if (Request.QueryString["p"] != null && Request.QueryString["p"].ToString() != string.Empty)
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["p"]);
                else
                    ViewState["PAGENUMBER"] = 1;

                if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                    ViewState["LAUNCHEDFROM"] = Request.QueryString["launchedfrom"].ToString();

                if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
                    ViewState["pl"] = Request.QueryString["pl"].ToString();

                if (Request.QueryString["sid"] != null && Request.QueryString["sid"].ToString() != "")
                    ViewState["sid"] = Request.QueryString["sid"].ToString();

                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvCrewSearch_PreRender(object sender, EventArgs e)
    {
        if (ViewState["sid"] != null)
        {
            DataTable dt = PhoenixCrewOffshoreEmployee.CrewOffshoreEmployeeSearchEdit(General.GetNullableInteger(ViewState["sid"].ToString()));
            if (dt.Rows.Count > 0)
            {
                string csvValue = "";

                if (dt.Rows[0]["FLDDISPLAYCOLUMNS"].ToString() != string.Empty)
                {
                    csvValue = dt.Rows[0]["FLDDISPLAYCOLUMNS"].ToString();
                }

                foreach (GridColumn column in gvCrewSearch.Columns)
                {
                    if (csvValue != "")
                    {
                        gvCrewSearch.MasterTableView.GetColumn(column.UniqueName).Display = false;

                        if (csvValue.Contains("," + column.UniqueName + ","))
                        {
                            gvCrewSearch.MasterTableView.GetColumn(column.UniqueName).Display = true;
                        }
                    }
                }

                gvCrewSearch.MasterTableView.GetColumn("Action").Display = true;
                gvCrewSearch.Rebind();

            }

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
                Response.Redirect("CrewOffshoreEmployeeSearch.aspx", true);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                int n;
                n = PhoenixCrewOffshoreEmployee.CrewOffshoreEmployeeClear(General.GetNullableInteger(ViewState["sid"].ToString()));
                ViewState["sid"] = "0";
                BindData();
                gvCrewSearch.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCrewSearch_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO","FLDSTATUS","FLDPRESENTVESSELNAME", "FLDPLANNEDVESSELNAME",
                                 "FLDLASTVESSELNAME", "FLDLASTSIGNOFFDATE", "FLDDOA",};
        string[] alCaptions = { "Name", "Rank", "File No", "Status", "Present Vessel", "Planned Vessel", "Last Vessel", "Last Sign-Off Date", "DOA" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewOffshoreEmployee.CrewOffshoreEmployeeSearch(General.GetNullableInteger(ViewState["sid"].ToString())
                                                                   , sortexpression, sortdirection
                                                                   , 1, iRowCount
                                                                   , ref iRowCount, ref iTotalPageCount
                                                               );

        General.ShowExcel("PersonnelMaster", dt, alColumns, alCaptions, sortdirection, sortexpression);
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

            DataTable dt = PhoenixCrewOffshoreEmployee.CrewOffshoreEmployeeSearch(General.GetNullableInteger(ViewState["sid"].ToString())
                                                                  , sortexpression, sortdirection
                                                                  , (int)ViewState["PAGENUMBER"], gvCrewSearch.PageSize
                                                                  , ref iRowCount, ref iTotalPageCount, ViewState["pl"].ToString()
                                                              );

            gvCrewSearch.DataSource = dt;
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

    protected void gvCrewSearch_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;

            if (e.CommandName.ToUpper() == "GETEMPLOYEE")
            {
                string familyid = "";//((Label)_gridView.Rows[nCurrentRow].FindControl("lblfamlyid")).Text;
                string newapp = ((RadLabel)e.Item.FindControl("lblNewApp")).Text;
                if (newapp.Equals("1"))
                {
                    Filter.CurrentNewApplicantSelection = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;
                    Response.Redirect("..\\CrewOffshore\\CrewOffshoreNewApplicantPersonalGeneral.aspx?p=" + ViewState["PAGENUMBER"].ToString() + "&back=yes&launchedfrom=" + ViewState["LAUNCHEDFROM"].ToString() + "&pl=" + ViewState["pl"].ToString() + "&crewsearch=" + ViewState["sid"].ToString());
                }
                else
                {
                    Filter.CurrentCrewSelection = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;
                    if (familyid == "")
                    {
                        Response.Redirect("..\\CrewOffshore\\CrewOffshorePersonalGeneral.aspx?p=" + ViewState["PAGENUMBER"].ToString() + "&back=yes&launchedfrom=" + ViewState["LAUNCHEDFROM"].ToString() + "&pl=" + ViewState["pl"].ToString() + "&crewsearch=" + ViewState["sid"].ToString());
                    }
                    else
                    {
                        Response.Redirect("..\\CrewOffshore\\CrewOffshorePersonalGeneral.aspx?empid=" + Filter.CurrentCrewSelection + "&familyid=" + familyid + "&p=" + ViewState["PAGENUMBER"].ToString() + "&back=yes&launchedfrom=" + ViewState["LAUNCHEDFROM"].ToString() + "&pl=" + ViewState["pl"].ToString() + "&crewsearch=" + ViewState["sid"].ToString(), false);
                    }
                }
            }
            if (e.CommandName == "SEAFARERLOGIN")
            {
                try
                {
                    Filter.CurrentCrewSelection = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;
                    PhoenixCrewOffshoreEmployee.CreateLoginAddress(int.Parse(Filter.CurrentCrewSelection));

                    ucStatus.Text = "Created Successfully";
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

            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            LinkButton ac = (LinkButton)e.Item.FindControl("imgActivity");

            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            if (ac != null)
            {
                ac.Visible = SessionUtil.CanAccess(this.ViewState, ac.CommandName);
                if (drv["FLDNEWAPP"] != null && drv["FLDNEWAPP"].ToString().Equals("1"))
                {
                    ac.Attributes.Add("onclick", "openNewWindow('codehelpactivity', '', '" + Session["sitepath"] + "/Crew/CrewNewApplicantActivitylGeneral.aspx?empid=" + empid.Text + "');return false;");
                }
                else
                {
                    if (drv["FLDSTATUSNAME"] != null && (drv["FLDSTATUSNAME"].ToString().Contains("ONB") || drv["FLDSTATUSNAME"].ToString().Contains("OBP")))
                    {
                        ac.Attributes.Add("onclick", "openNewWindow('codehelpactivity', '', '" + Session["sitepath"] + "/Crew/CrewActivityGeneral.aspx?empid=" + empid.Text + "&sg=0&ntbr=0&ds=" + drv["FLDDIRECTSIGNON"].ToString() + "&launchedfrom=offshore');return false;");
                    }
                    else
                    {
                        ac.Attributes.Add("onclick", "openNewWindow('codehelpactivity', '', '" + Session["sitepath"] + "/Crew/CrewActivityGeneral.aspx?empid=" + empid.Text + "&sg=0&ds=" + drv["FLDDIRECTSIGNON"].ToString() + "&launchedfrom=offshore');return false;");
                    }
                }
                //ac.Attributes.Add("onclick", "openNewWindow('codehelpactivity', '', '"+Session["sitepath"]+"/Crew/CrewActivityGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&sg=0&ds=0');return false;");
            }

            LinkButton login = (LinkButton)e.Item.FindControl("cmdSeafarerLogin");
            if (login != null)
            {
                login.Visible = SessionUtil.CanAccess(this.ViewState, login.CommandName);
            }
            RadLabel lblIsLoginCreated = (RadLabel)e.Item.FindControl("lblIsLoginCreated");
            if (lblIsLoginCreated.Text == "1")
            {
                if (login != null)
                {
                    login.Visible = false;
                }
            }


            LinkButton pd = (LinkButton)e.Item.FindControl("cmdPDForm");

            if (pd != null)
            {
                pd.Visible = SessionUtil.CanAccess(this.ViewState, pd.CommandName);
                pd.Attributes.Add("onclick", "openNewWindow('PDForm', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + drv["FLDEMPLOYEEID"].ToString() + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");
            }

            LinkButton hmDoc = (LinkButton)e.Item.FindControl("cmdHasMissingDoc");
            if (hmDoc != null)
            {
                hmDoc.Visible = SessionUtil.CanAccess(this.ViewState, pd.CommandName);
                hmDoc.Attributes.Add("onclick", "openNewWindow('PDForm', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEmployeeHasMissingDocuments.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&sid=" + ViewState["sid"].ToString() + "');return false;");
            }
            RadLabel lblPlannedVessel = (RadLabel)e.Item.FindControl("lblPlannedVessel");
            UserControlToolTip ucToolTipPlannedVessel = (UserControlToolTip)e.Item.FindControl("ucToolTipPlannedVessel");

            ucToolTipPlannedVessel.Position = ToolTipPosition.TopCenter;
            ucToolTipPlannedVessel.TargetControlId = lblPlannedVessel.ClientID;


        }
    }

    protected void gvCrewSearch_SortCommand(object sender, GridSortCommandEventArgs e)
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
