using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewNewApplicantQueryActivity : PhoenixBasePage
{
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

                if (Request.QueryString["p"] != null)
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["p"]);
                else
                    ViewState["PAGENUMBER"] = 1;

                if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                {
                    ViewState["LAUNCHEDFROM"] = Request.QueryString["launchedfrom"].ToString();
                }

                if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
                    ViewState["pl"] = Request.QueryString["pl"].ToString();

                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../Crew/CrewNewApplicantQueryActivity.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarsub.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewNewApplicantQueryActivityFilter.aspx?launchedfrom=" + ViewState["LAUNCHEDFROM"].ToString() + "'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarsub.AddFontAwesomeButton("../Crew/CrewNewApplicantQueryActivity.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Clear Filter", " <i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbarsub.AddFontAwesomeButton("javascript:openNewWindow('Sync','','" + Session["sitepath"] + "/Crew/CrewSyncToPrese.aspx'); return false;", "Sync", "<i class=\"fas fa-sync-alt-ip\"></i>", "SYNC");

            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))

                toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?t=n" + (Request.QueryString.ToString() != string.Empty ? ("&" + Request.QueryString.ToString()) : string.Empty) + "&Newapp=yes", "Add", "<i class=\"fas fa-user-plus\"></i>", "NEW APPLICANT");
            else

                toolbarsub.AddFontAwesomeButton("../Crew/CrewNewApplicantPersonalGeneral.aspx?t=n" + (Request.QueryString.ToString() != string.Empty ? ("&" + Request.QueryString.ToString()) : string.Empty) + "&Newapp=yes", "Add", "<i class=\"fas fa-user-plus\"></i>", "NEW APPLICANT");

            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbarsub.Show();

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

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Rebind(); 
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {

                ViewState["PAGENUMBER"] = 1;
                Filter.CurrentNewApplicantFilterCriteria = null;
                gvCrewSearch.CurrentPageIndex = 0;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = new string[] { "" };

        if (ViewState["LAUNCHEDFROM"] != null && ViewState["LAUNCHEDFROM"].ToString() != "")
            alColumns = new string[]{ "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDAPPLIEDON", "FLDREMARKS", "FLDFILENO",
                                 "FLDZONE", "FLDSEAMANBOOKNO","FLDDOA","FLDBATCH", "FLDSTATUSDESCRIPTION", "FLDDATEOFBIRTH" };
        else
            alColumns = new string[]{ "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDAPPLIEDON", "FLDREMARKS", "FLDFILENO",
                                 "FLDZONE", "FLDSEAMANBOOKNO","FLDDOA","FLDBATCH", "FLDSTATUS", "FLDDATEOFBIRTH" };
        string[] alCaptions = { "Name", "Applied Rank", "Applied On", "Last Remarks", "File No","Zone",
                                "CDC No", "D.O.A.","Batch", "Status", "D.O.B." };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

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
                    nvc.Add("ddlStatus", string.Empty);
                    nvc.Add("ddlInActive", string.Empty);
                    nvc.Add("ddlCountry", string.Empty);
                    nvc.Add("ddlState", string.Empty);
                    nvc.Add("ddlCity", string.Empty);
                    nvc.Add("ddlCreatedBy", string.Empty);
                    nvc.Add("ddlPool", Request.QueryString["pl"]);
                    nvc.Add("txtnopage", string.Empty);
                }
                else
                {
                    nvc["ddlPool"] = Request.QueryString["pl"];
                }
            }
            DataTable dt = new DataTable();
            if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
            {

                dt = PhoenixNewApplicantManagement.OffshoreNewApplicantQueryActivity(nvc != null ? nvc.Get("lstNationality") : string.Empty
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
                                                                           , (byte?)General.GetNullableInteger(nvc != null ? nvc.Get("ddlInActive") : "1")
                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCountry") : string.Empty)
                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("ddlState") : string.Empty)
                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCity") : string.Empty)
                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCreatedBy") : string.Empty)
                                                                           , sortexpression, sortdirection
                                                                           , (int)ViewState["PAGENUMBER"], iRowCount
                                                                           , ref iRowCount, ref iTotalPageCount
                                                                           , nvc != null ? nvc.Get("ddlPool") : string.Empty);//string.Empty
            }
            else
            {
                dt = PhoenixNewApplicantManagement.NewApplicantQueryActivity(nvc != null ? nvc.Get("lstNationality") : string.Empty
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
                                                                        , (byte?)General.GetNullableInteger(nvc != null ? nvc.Get("ddlInActive") : "1")
                                                                        , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCountry") : string.Empty)
                                                                        , General.GetNullableInteger(nvc != null ? nvc.Get("ddlState") : string.Empty)
                                                                        , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCity") : string.Empty)
                                                                        , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCreatedBy") : string.Empty)
                                                                        , sortexpression, sortdirection
                                                                        , (int)ViewState["PAGENUMBER"], iRowCount
                                                                        , ref iRowCount, ref iTotalPageCount
                                                                        , nvc != null ? nvc.Get("ddlPool") : string.Empty);//string.Empty
            }

            Response.AddHeader("Content-Disposition", "attachment; filename=NewApplicant.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>New Applicant</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                Response.Write("<td width='20%'>");
                Response.Write("<b>" + alCaptions[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dr in dt.Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    Response.Write("<td>");
                    if (alColumns[i].ToUpper().Equals("FLDSTATUS"))
                        Response.Write(dr[alColumns[i]].ToString() + " " + dr["FLDSTATUSNAME"].ToString());
                    else
                        Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            return;
        }
    }

    protected void CrewQueryActivity_TabStripCommand(object sender, EventArgs e)
    {
        Filter.CurrentNewApplicantSelection = null;
        if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
            Response.Redirect("..\\CrewOffshore\\CrewOffshoreNewApplicantPersonalGeneral.aspx?t=n" + (Request.QueryString.ToString() != string.Empty ? ("&" + Request.QueryString.ToString()) : string.Empty) + "&Newapp=yes");
        else
            Response.Redirect("..\\Crew\\CrewNewApplicantPersonalGeneral.aspx?t=n" + (Request.QueryString.ToString() != string.Empty ? ("&" + Request.QueryString.ToString()) : string.Empty) + "&Newapp=yes");
    }

   
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvCrewSearch.Rebind();
    }

    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        string[] alColumns = new string[] { };


        if (ViewState["LAUNCHEDFROM"] != null && ViewState["LAUNCHEDFROM"].ToString() != "")
            alColumns = new string[]{ "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDAPPLIEDON", "FLDREMARKS", "FLDFILENO",
                                 "FLDZONE", "FLDSEAMANBOOKNO","FLDDOA","FLDBATCH" ,"FLDSTATUSDESCRIPTION", "FLDDATEOFBIRTH" };
        else
            alColumns = new string[]{ "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDAPPLIEDON", "FLDREMARKS", "FLDFILENO",
                                 "FLDZONE", "FLDSEAMANBOOKNO","FLDDOA","FLDBATCH", "FLDSTATUS", "FLDDATEOFBIRTH" };

        string[] alCaptions = { "Name", "Applied Rank", "Applied On", "Last Remarks", "File No","Zone",
                                "CDC No", "D.O.A.","Batch","Status", "D.O.B." };


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
                    nvc.Add("ddlStatus", string.Empty);
                    nvc.Add("ddlInActive", string.Empty);
                    nvc.Add("ddlCountry", string.Empty);
                    nvc.Add("ddlState", string.Empty);
                    nvc.Add("ddlCity", string.Empty);
                    nvc.Add("ddlCreatedBy", string.Empty);
                    nvc.Add("ddlPool", Request.QueryString["pl"]);
                }
                else
                {
                    nvc["ddlPool"] = Request.QueryString["pl"];
                }
            }
            DataTable dt = new DataTable();
            if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
            {
                dt = PhoenixNewApplicantManagement.OffshoreNewApplicantQueryActivity(nvc != null ? nvc.Get("lstNationality") : string.Empty
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
                                                                           , (byte?)General.GetNullableInteger(nvc != null ? nvc.Get("ddlInActive") : "1")
                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCountry") : string.Empty)
                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("ddlState") : string.Empty)
                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCity") : string.Empty)
                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCreatedBy") : string.Empty)
                                                                           , sortexpression, sortdirection
                                                                           , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                           , gvCrewSearch.PageSize
                                                                           , ref iRowCount, ref iTotalPageCount
                                                                           , nvc != null ? nvc.Get("ddlPool") : string.Empty);
            }
            else
            {
                dt = PhoenixNewApplicantManagement.NewApplicantQueryActivity(nvc != null ? nvc.Get("lstNationality") : string.Empty
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
                                                                      , (byte?)General.GetNullableInteger(nvc != null ? nvc.Get("ddlInActive") : "1")
                                                                      , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCountry") : string.Empty)
                                                                      , General.GetNullableInteger(nvc != null ? nvc.Get("ddlState") : string.Empty)
                                                                      , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCity") : string.Empty)
                                                                      , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCreatedBy") : string.Empty)
                                                                      , sortexpression, sortdirection
                                                                      , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                      , gvCrewSearch.PageSize
                                                                      , ref iRowCount, ref iTotalPageCount
                                                                      , nvc != null ? nvc.Get("ddlPool") : string.Empty);
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvCrewSearch", "New Applicant", alCaptions, alColumns, ds);

            gvCrewSearch.DataSource = ds;

            ViewState["ROWCOUNT"] = iRowCount;
            gvCrewSearch.VirtualItemCount = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        RadGrid _gridView = (RadGrid)sender;
        if (e.CommandName == "RowClick" || (e.CommandName == RadGrid.ExpandCollapseCommandName) && (!e.Item.Expanded))
        {
            bool lastState = e.Item.Expanded;

            if (e.CommandName == RadGrid.ExpandCollapseCommandName || e.CommandName == "RowClick")
            {
                lastState = !lastState;
            }

            //CollapseAllRows(lastState);
            e.Item.Expanded = !lastState;
        }

        if (e.CommandName.ToUpper() == "GETEMPLOYEE")
        {
            string archived = ((RadLabel)eeditedItem.FindControl("lblActiveyn")).Text;

            if (archived == "2")
            {
                ucError.ErrorMessage = "You can only edit the Active/In-Active employee";
                ucError.Visible = true;
            }
            else
            {
                Filter.CurrentNewApplicantSelection = ((RadLabel)eeditedItem.FindControl("lblEmployeeid")).Text;
                Session["REFRESHFLAG"] = null;
                if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                    Response.Redirect("..\\CrewOffshore\\CrewOffshoreNewApplicantPersonalGeneral.aspx?p=" + ViewState["PAGENUMBER"].ToString() + "&back=yes&launchedfrom=" + ViewState["LAUNCHEDFROM"].ToString() + "&pl=" + ViewState["pl"].ToString() + "&Newapp=yes");
                else
                    Response.Redirect("..\\Crew\\CrewNewApplicantPersonalGeneral.aspx?p=" + ViewState["PAGENUMBER"].ToString() + "&back=yes&launchedfrom=" + ViewState["LAUNCHEDFROM"].ToString() + "&pl=" + ViewState["pl"].ToString() + "&Newapp=yes");
            }
        }

        if (e.CommandName.ToUpper() == "REVERSAL")
        {


            string employeeid = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;

            PhoenixNewApplicantManagement.DeleteReversalNewApplicant( Convert.ToInt32(employeeid) );
            ucStatus.Text = "Applicant recovered.";
            Rebind();
          }
        if (e.CommandName.ToUpper() == "SYNC")
        {
            string employeeid = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;

            PhoenixNewApplicantManagement.NewApplicantSyncDataToPresea(General.GetNullableInteger(employeeid), null);
            ucStatus.Text = "Data synced successfully";

            Rebind();

        }
        if (e.CommandName.ToUpper() == "PHOENIXSYNCLOGIN")
        {
            Filter.CurrentNewApplicantSelection = ((RadLabel)eeditedItem.FindControl("lblEmployeeid")).Text;

            Response.Redirect("..\\Crew\\CrewServiceSyncLogin.aspx?empid=" + Filter.CurrentNewApplicantSelection);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper() == "DELETE")
        {
   
            string employeeid = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;

            PhoenixNewApplicantManagement.DeleteNewApplicant(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , Convert.ToInt32(employeeid)
                                                                );
            ucStatus.Text = "Applicant deleted.";
            Rebind();
        }

    }
    private void CollapseAllRows(bool laststate)
    {
        foreach (GridItem item in gvCrewSearch.MasterTableView.Items)
        {
            if (item.Expanded == laststate)
            {
                item.Expanded = !laststate;
            }         
        }
    }

    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            LinkButton ed = (LinkButton)item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton db = (LinkButton)item.FindControl("cmdDelete");
            db.Attributes.Add("onclick", "return fnConfirmDelete(event)");

            LinkButton sy = (LinkButton)item.FindControl("cmdSync");
            if (sy != null)
            {
                sy.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want sync the entry ?')");
                sy.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton dbat = (LinkButton)item.FindControl("cmdReversal");
            dbat.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to recover the deleted entry ?')");

            RadLabel empid = (RadLabel)item.FindControl("lblEmployeeid");
            LinkButton sg = (LinkButton)item.FindControl("imgActivity");
            sg.Attributes.Add("onclick", "openNewWindow('codehelpactivity', '', '" + Session["sitepath"] + "/Crew/CrewNewApplicantDateOfAvailability.aspx?empid=" + empid.Text + "');return false;");

            LinkButton imgSuitableCheck = (LinkButton)item.FindControl("imgSuitableCheck");
            imgSuitableCheck.Attributes.Add("onclick", "openNewWindow('codehelp', '', '" + Session["sitepath"] + "/Crew/CrewSuitabilityCheck.aspx?empid=" + empid.Text + "&newapplicant=true');return false;");

            LinkButton pd = (LinkButton)item.FindControl("cmdPDForm");
            if (pd != null)
            {
                pd.Visible = SessionUtil.CanAccess(this.ViewState, pd.CommandName);
                pd.Attributes.Add("onclick", "openNewWindow('PDForm', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + empid.Text + "&showmenu=0" + "&rowusercode=" +
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode + "');return false;");
            }

            if (ViewState["LAUNCHEDFROM"] != null && ViewState["LAUNCHEDFROM"].ToString() != "")
            {
               
                if (imgSuitableCheck != null) imgSuitableCheck.Visible = false;
            }
            else
            {
                if (sg != null) sg.Visible = true;
                if (imgSuitableCheck != null) imgSuitableCheck.Visible = true;
            }

            if (sg != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, sg.CommandName)) sg.Visible = false;
            }

            if (imgSuitableCheck != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgSuitableCheck.CommandName)) imgSuitableCheck.Visible = false;
            }

            LinkButton servicelogin = (LinkButton)item.FindControl("cmdServiceLogin");            
            if (servicelogin != null)
            {
                servicelogin.Visible = SessionUtil.CanAccess(this.ViewState, servicelogin.CommandName);          
            }

            RadLabel lblActiveyn = (RadLabel)item.FindControl("lblActiveyn");
            if (lblActiveyn.Text == "2")
            {

                if (dbat != null) dbat.Visible = SessionUtil.CanAccess(this.ViewState, dbat.CommandName);

                if (db != null) db.Visible = false;
                if (imgSuitableCheck != null) imgSuitableCheck.Visible = false;
                if (pd != null) pd.Visible = false;
                if (sg != null) sg.Visible = false;
                if (ed != null) ed.Visible = false;
                if (sy != null) sy.Visible = false;
            }
            else
            {
                if (dbat != null) dbat.Visible = false;
                if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
    }

    protected void Rebind()
    {
        gvCrewSearch.SelectedIndexes.Clear();
        gvCrewSearch.EditIndexes.Clear();
        gvCrewSearch.DataSource = null;
        gvCrewSearch.Rebind();
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
}
