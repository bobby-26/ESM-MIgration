using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Web;

public partial class CrewOffshoreNewApplicantList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            //PhoenixToolbar toolbar1 = new PhoenixToolbar();
            //CrewQueryActivity.Title = "New Applicant";
            //CrewQueryActivity.AccessRights = this.ViewState;
            //CrewQueryActivity.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["LAUNCHEDFROM"] = "";
                ViewState["pl"] = "";

                if (Request.QueryString["p"] != null)
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["p"]);
                else
                    ViewState["PAGENUMBER"] = 1;




                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["LAUNCHEDFROM"] = Request.QueryString["launchedfrom"].ToString();
            }

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreNewApplicantList.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarsub.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantQueryActivityFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreNewApplicantList.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?t=n" + (Request.QueryString.ToString() != string.Empty ? ("&" + Request.QueryString.ToString()) : string.Empty) + "&Newapp=yes", "Add", "<i class=\"fas fa-user-plus\"></i>", "NEW APPLICANT");

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
                BindData();
                gvCrewSearch.Rebind();

            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {

                ViewState["PAGENUMBER"] = 1;
                Filter.CurrentNewApplicantFilterCriteria = null;

                BindData();
                gvCrewSearch.Rebind();
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

        alColumns = new string[]{ "FLDEMPLOYEENAME", "FLDRANKCODE","FLDPHONENUMBER", "FLDAPPLIEDON", "FLDREMARKS", "FLDFILENO",
                                 "FLDZONE", "FLDSEAMANBOOKNO","FLDDOA", "FLDSTATUSDESCRIPTION", "FLDDATEOFBIRTH" };

        string[] alCaptions = { "Name", "Applied Rank","Contact No", "Applied On", "Last Remarks", "File No","Zone",
                                "CDC No", "D.O.A.", "Status", "D.O.B." };

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


            DataTable dt = new DataTable();
            //if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
            //{

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
                                                                       , nvc != null ? nvc.Get("ddlPool") : string.Empty);
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
        Response.Redirect("..\\CrewOffshore\\CrewOffshoreNewApplicantPersonalGeneral.aspx?t=n" + (Request.QueryString.ToString() != string.Empty ? ("&" + Request.QueryString.ToString()) : string.Empty) + "&Newapp=yes");
       
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
        alColumns = new string[]{ "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDAPPLIEDON", "FLDREMARKS", "FLDFILENO",
                                 "FLDZONE", "FLDSEAMANBOOKNO","FLDDOA", "FLDSTATUSDESCRIPTION", "FLDDATEOFBIRTH" };

     

        string[] alCaptions = { "Name", "Applied Rank", "Applied On", "Last Remarks", "File No","Zone",
                                "CDC No", "D.O.A.", "Status", "D.O.B." };


        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {

            NameValueCollection nvc = Filter.CurrentNewApplicantFilterCriteria;
            //if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
            //{
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
            //    else
            //    {
            //        nvc["ddlPool"] = Request.QueryString["pl"];
            //    }
            //}
            DataTable dt = new DataTable();
            //if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
            //{
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
                                                                       , (int)ViewState["PAGENUMBER"], gvCrewSearch.PageSize
                                                                       , ref iRowCount, ref iTotalPageCount
                                                                       , nvc != null ? nvc.Get("ddlPool") : string.Empty);
            //}
            //else
            //{
            //    dt = PhoenixNewApplicantManagement.NewApplicantQueryActivity(nvc != null ? nvc.Get("lstNationality") : string.Empty
            //                                                          , nvc != null ? nvc.Get("txtPassortNo") : string.Empty
            //                                                          , nvc != null ? nvc.Get("txtSeamanbookNo") : string.Empty
            //                                                          , nvc != null ? nvc.Get("txtFileNumber") : string.Empty
            //                                                          , nvc != null ? nvc.Get("lstRank") : string.Empty
            //                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ddlZone") : string.Empty)
            //                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ddlSailedRank") : string.Empty)
            //                                                          , nvc != null ? nvc.Get("ddlVesselType") : string.Empty
            //                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ddlEngineType") : string.Empty)
            //                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCourse") : string.Empty)
            //                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ddlLicences") : string.Empty)
            //                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ddlVisa") : string.Empty)
            //                                                          , General.GetNullableDateTime(nvc != null ? nvc.Get("txtAppliedStartDate") : string.Empty)
            //                                                          , General.GetNullableDateTime(nvc != null ? nvc.Get("txtAppliedEndDate") : string.Empty)
            //                                                          , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOAStartDate") : string.Empty)
            //                                                          , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOAEndDate") : string.Empty)
            //                                                          , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOBStartDate") : string.Empty)
            //                                                          , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOBEndDate") : string.Empty)
            //                                                          , nvc != null ? nvc.Get("txtRefNumber") : string.Empty, nvc != null ? nvc.Get("txtName") : string.Empty
            //                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ddlPrevCompany") : string.Empty)
            //                                                          , nvc != null ? nvc.Get("ddlStatus") : string.Empty
            //                                                          , (byte?)General.GetNullableInteger(nvc != null ? nvc.Get("ddlInActive") : "1")
            //                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCountry") : string.Empty)
            //                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ddlState") : string.Empty)
            //                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCity") : string.Empty)
            //                                                          , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCreatedBy") : string.Empty)
            //                                                          , sortexpression, sortdirection
            //                                                          , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
            //                                                          , ref iRowCount, ref iTotalPageCount
            //                                                          , nvc != null ? nvc.Get("ddlPool") : string.Empty);
            //}

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvCrewSearch", "New Applicant", alCaptions, alColumns, ds);
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
    //protected void gvCrewSearch_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = e.RowIndex;
    //    string employeeid = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeid")).Text;
    //    PhoenixNewApplicantManagement.DeleteNewApplicant(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(employeeid));
    //    BindData();
    //    SetPageNavigator();
    //}

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

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;


            if (e.CommandName.ToUpper() == "GETEMPLOYEE")
            {
                string archived = ((RadLabel)e.Item.FindControl("lblActiveyn")).Text;

                if (archived == "2")
                {
                    ucError.ErrorMessage = "You can only edit the Active/In-Active employee";
                    ucError.Visible = true;
                }
                else
                {
                    Filter.CurrentNewApplicantSelection = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;
                    Session["REFRESHFLAG"] = null;
                    Response.Redirect("..\\CrewOffshore\\CrewOffshoreNewApplicantPersonalGeneral.aspx?p=" + ViewState["PAGENUMBER"].ToString() + "&back=yes&launchedfrom=" + ViewState["LAUNCHEDFROM"].ToString() + "&pl=" + ViewState["pl"].ToString() + "&Newapp=yes");

                }
            }
            if (e.CommandName.ToUpper() == "REVERSAL")
            {
                string employeeid = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;

                PhoenixNewApplicantManagement.DeleteReversalNewApplicant(Convert.ToInt32(employeeid));

                BindData();
                gvCrewSearch.Rebind();

            }
            if (e.CommandName.ToUpper() == "SENDMAIL")
            {
                string employeeid = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;
                Filter.CurrentNewApplicantSelection = employeeid;


                Response.Redirect("..\\Crew\\CrewNewApplicantCorrespondenceEmail.aspx?empid=" + employeeid.ToString() + "&LAUNCH=OFFSHORE");
            }
            if (e.CommandName.ToUpper() == "REMARKS")
            {
                string employeeid = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;
                Filter.CurrentNewApplicantSelection = employeeid;

                String scriptpopup = String.Format("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewNewApplicantRemarks.aspx?empid=" + employeeid.ToString() + "');");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            }
            if (e.CommandName == "SEAFARERLOGIN")
            {
                try
                {
                    Filter.CurrentNewApplicantSelection = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;
                    PhoenixNewApplicantManagement.CreateLoginAddress(int.Parse(Filter.CurrentNewApplicantSelection));

                    ucStatus.Text = "Created Successfully";
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }

            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper() == "DELETE")
            {


                string employeeid = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;

                PhoenixNewApplicantManagement.DeleteNewApplicant(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , Convert.ToInt32(employeeid)
                                                                    );

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

    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            db.Attributes.Add("onclick", "return fnConfirmDelete(event)");

            LinkButton dbat = (LinkButton)e.Item.FindControl("cmdReversal");
            dbat.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to recover the deleted entry ?')");

            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            LinkButton sg = (LinkButton)e.Item.FindControl("imgActivity");
            sg.Attributes.Add("onclick", "openNewWindow('codehelpactivity', '', '" + Session["sitepath"] + "/Crew/CrewNewApplicantActivitylGeneral.aspx?empid=" + empid.Text + "');return false;");

            LinkButton imgSuitableCheck = (LinkButton)e.Item.FindControl("imgSuitableCheck");
            imgSuitableCheck.Attributes.Add("onclick", "openNewWindow('codehelp', '', '" + Session["sitepath"] + "/Crew/CrewSuitabilityCheck.aspx?empid=" + empid.Text + "&newapplicant=true');return false;");

            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
            if (lbtn != null) lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            if (lbtn != null) lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

            LinkButton pd = (LinkButton)e.Item.FindControl("cmdPDForm");
            if (pd != null)
            {
                pd.Visible = SessionUtil.CanAccess(this.ViewState, pd.CommandName);
                pd.Attributes.Add("onclick", "openNewWindow('PDForm', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + empid.Text + "&showmenu=0" + "&rowusercode=" +
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode + "');return false;");
            }

         
            RadLabel lblstatus = (RadLabel)e.Item.FindControl("lblStatus");
            if (lblstatus != null)
            {
                lblstatus.Text = drv["FLDSTATUSDESCRIPTION"].ToString();
            }

            if (imgSuitableCheck != null) imgSuitableCheck.Visible = false;


            if (sg != null)
            {
                sg.Visible = SessionUtil.CanAccess(this.ViewState, sg.CommandName);
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

            RadLabel lblActiveyn = (RadLabel)e.Item.FindControl("lblActiveyn");
            if (lblActiveyn.Text == "2")
            {

                if (dbat != null) dbat.Visible = SessionUtil.CanAccess(this.ViewState, dbat.CommandName);

                if (db != null) db.Visible = false;
                if (imgSuitableCheck != null) imgSuitableCheck.Visible = false;
                if (pd != null) pd.Visible = false;
                if (sg != null) sg.Visible = false;
                if (ed != null) ed.Visible = false;
            }
            else
            {
                if (dbat != null) dbat.Visible = false;
                if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }


        }

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
    }
}
