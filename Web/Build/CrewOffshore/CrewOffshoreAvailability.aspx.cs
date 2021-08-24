using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Web.UI;

public partial class CrewOffshoreAvailability : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreAvailability.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreAvailabilitySearch.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Filter", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreAvailability.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbarsub.Show();


            if (!IsPostBack)
            {
                if (Request.QueryString["p"] != null && Request.QueryString["p"].ToString() != string.Empty)
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["p"]);
                else
                    ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = 1;
                ViewState["CURRENTINDEX"] = 1;

                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            // BindData();
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
                Response.Redirect("CrewOffshoreAvailabilitySearch.aspx", true);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentOffshoreAvailabilitySearch = null;
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

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKCODE","FLDNATIONALITYNAME", "FLDFILENO", "FLDPLANNEDVESSELNAME", "FLDLASTVESSELNAME",
                                 "FLDLASTSIGNOFFDATE", "FLDDAILYRATEUSD", "FLDFLDDPALLOWANCEUSD","FLDEXPECTEDSALARY","FLDDOA", "FLDLASTCONTACTDATE","FLDREMARKS","FLDEMAILID"};
        string[] alCaptions = { "Name", "Rank", "Nationality","File No","Planned Vessel" , "Last Vessel",
                                  "Last Sign-Off Date", "Last drawn salary/day", "DP Allowance/day","expected salary/day", "DOA", "Last Contact","Comments","Email ID" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string txtDOAFrom;
        string txtDOAto;
        string txtcontactFrom;
        DateTime now = DateTime.Now;
        DateTime lastonemonth = now.AddDays(-30);
        DateTime nexttwomonths = now.AddDays(60);
        txtDOAFrom = lastonemonth.ToString("dd/MM/yyyy");
        txtDOAto = nexttwomonths.ToString("dd/MM/yyyy");
        txtcontactFrom = lastonemonth.ToString("dd/MM/yyyy");

        NameValueCollection nvc = Filter.CurrentOffshoreAvailabilitySearch;
        DataTable dt = PhoenixCrewOffshoreEmployee.CrewOffshoreEmployeeAvailabilitySearch(nvc != null ? nvc["txtName"] : ""
                                                                  , nvc != null ? nvc["txtFileNo"] : ""
                                                                  , General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : "")
                                                                  , General.GetNullableDateTime(nvc != null ? nvc["txtDOAFrom"] : txtDOAFrom)
                                                                  , General.GetNullableDateTime(nvc != null ? nvc["txtDOATo"] : txtDOAto)
                                                                  , General.GetNullableDateTime(nvc != null ? nvc["txtContactFrom"] : txtcontactFrom)
                                                                  , General.GetNullableDateTime(nvc != null ? nvc["txtContactTo"] : "")
                                                                  , General.GetNullableInteger(nvc != null ? nvc["chkincludentbr"] : "")
                                                                  , General.GetNullableInteger(nvc != null ? nvc["ddlstatus"] : "")
                                                                  , General.GetNullableInteger(nvc != null ? nvc["ddlNationality"] : "")
                                                                  , sortexpression, sortdirection
                                                                  , 1, iRowCount
                                                                  , ref iRowCount, ref iTotalPageCount
                                                                  , (Request.QueryString["pl"] != null ? Request.QueryString["pl"] : string.Empty)
                                                               );

        General.ShowExcel("Availability", dt, alColumns, alCaptions, sortdirection, sortexpression);
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
            string txtDOAFrom;
            string txtDOAto;
            string txtcontactFrom;
            DateTime now = DateTime.Now;
            DateTime lastonemonth = now.AddDays(-30);
            DateTime nexttwomonths = now.AddDays(60);
            txtDOAFrom = lastonemonth.ToString("dd/MM/yyyy");
            txtDOAto = nexttwomonths.ToString("dd/MM/yyyy");
            txtcontactFrom = lastonemonth.ToString("dd/MM/yyyy");

            NameValueCollection nvc = Filter.CurrentOffshoreAvailabilitySearch;
            DataTable dt = PhoenixCrewOffshoreEmployee.CrewOffshoreEmployeeAvailabilitySearch(nvc != null ? nvc["txtName"] : ""
                                                                      , nvc != null ? nvc["txtFileNo"] : ""
                                                                      , General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : "")
                                                                      , General.GetNullableDateTime(nvc != null ? nvc["txtDOAFrom"] : txtDOAFrom)
                                                                      , General.GetNullableDateTime(nvc != null ? nvc["txtDOATo"] : txtDOAto)
                                                                      , General.GetNullableDateTime(nvc != null ? nvc["txtContactFrom"] : txtcontactFrom)
                                                                      , General.GetNullableDateTime(nvc != null ? nvc["txtContactTo"] : "")
                                                                      , General.GetNullableInteger(nvc != null ? nvc["chkincludentbr"] : "")
                                                                      , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : "")
                                                                      , General.GetNullableInteger(nvc != null ? nvc["ddlNationality"] : "")
                                                                      , sortexpression, sortdirection
                                                                      , (int)ViewState["PAGENUMBER"], gvCrewSearch.PageSize
                                                                      , ref iRowCount, ref iTotalPageCount
                                                                      , (Request.QueryString["pl"] != null ? Request.QueryString["pl"] : string.Empty)
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

    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {


                LinkButton ac = (LinkButton)e.Item.FindControl("imgActivity");


                if (ac != null)
                {
                    ac.Attributes.Add("onclick", "javascript:openNewWindow('codehelpactivity', '', '" + Session["sitepath"] + "/Crew/CrewActivityGeneral.aspx?empid=" + DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEEID").ToString() + "&sg=0&ds=0&launchedfrom=offshore');return false;");
                }
                LinkButton imgSuitability = (LinkButton)e.Item.FindControl("imgSuitableCheck");
                if (imgSuitability != null)
                {
                    if (DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEECODE") != null && General.GetNullableString(DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEECODE").ToString()) != null)
                        imgSuitability.Attributes.Add("onclick", "javascript:openNewWindow('codehelpsuitability', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?empid=" + DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEEID").ToString() + "&personalmaster=true&popup=1');return false;");
                    else if (DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEECODE") != null && General.GetNullableString(DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEECODE").ToString()) == null)
                        imgSuitability.Attributes.Add("onclick", "javascript:openNewWindow('codehelpsuitability', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?empid=" + DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEEID").ToString() + "&newapplicant=true&popup=1');return false;");
                }

                LinkButton lnkEployeeName = (LinkButton)e.Item.FindControl("lnkEployeeName");
                RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");
                RadLabel lblRankId = (RadLabel)e.Item.FindControl("lblRankId");
                RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
                if (lnkEployeeName != null)
                {
                    if (DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEECODE") != null && General.GetNullableString(DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEECODE").ToString()) != null)
                        lnkEployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                    else
                        lnkEployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                    //lnkEployeeName.Attributes.Add("onclick", "Openpopup('PDForm', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + lblEmployeeid.Text + "&Itemusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");
                }

                LinkButton pd = (LinkButton)e.Item.FindControl("cmdPDForm");
                if (pd != null)
                    pd.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=EMPLOYEECV&employeeid=" + lblEmployeeid.Text + "&rankid=" + lblRankId.Text + "');return false;");

                ImageButton reverse = (ImageButton)e.Item.FindControl("cmdreverse");
                if (reverse != null)
                    reverse.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreReverseVesselAssessment.aspx?employeeid=" + lblEmployeeid.Text + "&rankid=" + lblRankId.Text + "');return false;");

                //if (DataBinder.Eval(e.Item.DataItem,"FLDSTATUSNAME"] != null && DataBinder.Eval(e.Item.DataItem,"FLDSTATUSNAME"].ToString().ToUpper() == "NWA" && lblStatus != null)
                //{
                //    lblStatus.Text = "New Applicant";
                //}
                RadLabel lblPlannedVessel = (RadLabel)e.Item.FindControl("lblPlannedVessel");
                UserControlToolTip ucToolTipPlannedVessel = (UserControlToolTip)e.Item.FindControl("ucToolTipPlannedVessel");

                ucToolTipPlannedVessel.Position = ToolTipPosition.TopCenter;
                ucToolTipPlannedVessel.TargetControlId = lblPlannedVessel.ClientID;

                RadLabel lblName = (RadLabel)e.Item.FindControl("lblName");
                if (DataBinder.Eval(e.Item.DataItem, "FLDVIEWPARTICULARSYN") != null && DataBinder.Eval(e.Item.DataItem, "FLDVIEWPARTICULARSYN").ToString().Equals("1"))
                {
                    lblName.Visible = false;
                    lnkEployeeName.Visible = true;
                }
                else
                {
                    lblName.Visible = true;
                    lnkEployeeName.Visible = false;
                }

                if (ac != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, ac.CommandName)) ac.Visible = false;
                }
                if (imgSuitability != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgSuitability.CommandName)) imgSuitability.Visible = false;
                }
                if (lnkEployeeName != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, lnkEployeeName.CommandName)) lnkEployeeName.Visible = false;
                }
                if (pd != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, pd.CommandName)) pd.Visible = false;
                }
                //RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
                //UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipRemarks");
                //lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                //lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

                RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");

                LinkButton ImgRemarks = (LinkButton)e.Item.FindControl("ImgRemarks");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipRemarks");
                if (ImgRemarks != null)
                {
                    if (lbtn != null)
                    {
                        if (lbtn.Text != "")
                        {
                            ImgRemarks.Visible = true;
                            uct.Position = ToolTipPosition.TopCenter;
                            uct.TargetControlId = ImgRemarks.ClientID;
                        }
                        else
                            ImgRemarks.Visible = false;
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

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;
            //GridView _gridView = (GridView)sender;
            // int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            //if (e.CommandName.ToUpper() == "GETEMPLOYEE")
            //{
            //    Filter.CurrentCrewSelection = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeid")).Text;
            //    Response.Redirect("..\\CrewOffshore\\CrewOffshoreEmployeeGeneral.aspx?p=" + ViewState["PAGENUMBER"].ToString());
            //}    
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
}
