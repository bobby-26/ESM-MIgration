using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Web;

public partial class CrewReportAllExpiringDocuments : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuCrewExpiringDocList.AccessRights = this.ViewState;
            MenuCrewExpiringDocList.MenuList = toolbarmain.Show();


            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewReportAllExpiringDocuments.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            MenuCrewExpiringDocs.AccessRights = this.ViewState;
            MenuCrewExpiringDocs.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CPAGENUMBER"] = 1;
                ViewState["CSORTEXPRESSION"] = null;
                ViewState["CSORTDIRECTION"] = null;
                ViewState["OPAGENUMBER"] = 1;
                ViewState["OSORTEXPRESSION"] = null;
                ViewState["OSORTDIRECTION"] = null;
                ViewState["TPAGENUMBER"] = 1;
                ViewState["TSORTEXPRESSION"] = null;
                ViewState["TSORTDIRECTION"] = null;
                ViewState["MPAGENUMBER"] = 1;
                ViewState["MSORTEXPRESSION"] = null;
                ViewState["MSORTDIRECTION"] = null;
                ViewState["VESSELMANDATORY"] = null;
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvCrewCourse.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvCrewMedical.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvCrewOtherDoc.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvCrewTravel.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCrew.SelectedIndexes.Clear();
        gvCrew.EditIndexes.Clear();
        gvCrew.DataSource = null;
        gvCrew.Rebind();

        gvCrewCourse.SelectedIndexes.Clear();
        gvCrewCourse.EditIndexes.Clear();
        gvCrewCourse.DataSource = null;
        gvCrewCourse.Rebind();

        gvCrewMedical.SelectedIndexes.Clear();
        gvCrewMedical.EditIndexes.Clear();
        gvCrewMedical.DataSource = null;
        gvCrewMedical.Rebind();

        gvCrewOtherDoc.SelectedIndexes.Clear();
        gvCrewOtherDoc.EditIndexes.Clear();
        gvCrewOtherDoc.DataSource = null;
        gvCrewOtherDoc.Rebind();

        gvCrewTravel.SelectedIndexes.Clear();
        gvCrewTravel.EditIndexes.Clear();
        gvCrewTravel.DataSource = null;
        gvCrewTravel.Rebind();
    }

    private void ShowReport()
    {
        try
        {
            Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindLicenceData()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = CrewReportExpiringSearch.CrewReportExpiringLicense(
                           General.GetNullableString(ucVessel.SelectedVessel.ToString()),
                           (ucNationality.SelectedList) == "Dummy" ? null : General.GetNullableString(ucNationality.SelectedList),
                           (ucNationality1.SelectedNationalityValue.ToString()) == "Dummy" ? null : General.GetNullableString(ucNationality1.SelectedNationalityValue.ToString()),
                           (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch.ToString()),
                           General.GetNullableInteger(ddlSelectFrom.SelectedHard),
                           General.GetNullableInteger(txtNextDays.Text),
                           sortexpression, sortdirection,
                           Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                           gvCrew.PageSize,
                           ref iRowCount,
                           ref iTotalPageCount
                           , (ucRank.selectedlist) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist));


        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    private void BindCourseData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string sortexpression = (ViewState["CSORTEXPRESSION"] == null) ? null : (ViewState["CSORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["CSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["CSORTDIRECTION"].ToString());

        ds = CrewReportExpiringSearch.CrewReportExpiringCourse(
                           General.GetNullableInteger(ucVessel.SelectedVessel.ToString()),
                          (ucNationality.SelectedList) == "Dummy" ? null : General.GetNullableString(ucNationality.SelectedList),
                          (ucNationality1.SelectedList) == "Dummy" ? null : General.GetNullableString(ucNationality1.SelectedList),
                          (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch.ToString()),
                          General.GetNullableInteger(ddlSelectFrom.SelectedHard),
                          General.GetNullableInteger(txtNextDays.Text),
                           sortexpression, sortdirection,
                           Int32.Parse(ViewState["CPAGENUMBER"].ToString()),
                           gvCrewCourse.PageSize,
                           ref iRowCount,
                           ref iTotalPageCount,
                           (ucRank.selectedlist) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist));


        gvCrewCourse.DataSource = ds;
        gvCrewCourse.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    private void BindOtherDocumentData()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["OSORTEXPRESSION"] == null) ? null : (ViewState["OSORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["OSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["OSORTDIRECTION"].ToString());

        ds = CrewReportExpiringSearch.CrewReportExpiringOtherDocument(
                        General.GetNullableInteger(ucVessel.SelectedVessel.ToString()),
                        General.GetNullableString(ucNationality.SelectedList == "Dummy" ? null : ucNationality.SelectedList),
                        General.GetNullableInteger(ucBatch.SelectedBatch.ToString() == "Dummy" ? null : ucBatch.SelectedBatch.ToString()),
                        General.GetNullableInteger(ddlSelectFrom.SelectedHard),
                        General.GetNullableInteger(txtNextDays.Text),
                        sortexpression, sortdirection,
                        Int32.Parse(ViewState["OPAGENUMBER"].ToString()),
                        gvCrewOtherDoc.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount,
                        (ucRank.selectedlist) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist));

        gvCrewOtherDoc.DataSource = ds;
        gvCrewOtherDoc.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    private void BindTravelDocumentData()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["TSORTEXPRESSION"] == null) ? null : (ViewState["TSORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["TSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["TSORTDIRECTION"].ToString());

        ds = CrewReportExpiringSearch.CrewReportExpiringTravel(
                           General.GetNullableInteger(ucVessel.SelectedVessel.ToString()),
                           (ucNationality.SelectedList) == "Dummy" ? null : General.GetNullableString(ucNationality.SelectedList),
                           (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch.ToString()),
                           General.GetNullableInteger(ddlSelectFrom.SelectedHard),
                           General.GetNullableInteger(txtNextDays.Text),
                           sortexpression, sortdirection,
                           Int32.Parse(ViewState["TPAGENUMBER"].ToString()),
                           gvCrewTravel.PageSize,
                           ref iRowCount,
                           ref iTotalPageCount, (ucRank.selectedlist) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist));


        gvCrewTravel.DataSource = ds;
        gvCrewTravel.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    private void BindMedicalData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string sortexpression = (ViewState["MSORTEXPRESSION"] == null) ? null : (ViewState["MSORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["MSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["MSORTDIRECTION"].ToString());

        ds = CrewReportExpiringSearch.CrewReportExpiringMedical(
                           General.GetNullableInteger(ucVessel.SelectedVessel.ToString()),
                           (ucNationality.SelectedList) == "Dummy" ? null : General.GetNullableString(ucNationality.SelectedList),
                           (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch.ToString()),
                           General.GetNullableInteger(ddlSelectFrom.SelectedHard),
                           General.GetNullableInteger(txtNextDays.Text),
                           sortexpression, sortdirection,
                           Int32.Parse(ViewState["MPAGENUMBER"].ToString()),
                           gvCrewMedical.PageSize,
                           ref iRowCount,
                           ref iTotalPageCount, (ucRank.selectedlist) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist));

        gvCrewMedical.DataSource = ds;
        gvCrewMedical.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void MenuCrewExpiringDocList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ucVessel.SelectedVessel, ddlSelectFrom.SelectedHard, txtNextDays.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    NameValueCollection criteria = new NameValueCollection();
                    criteria.Clear();
                    criteria.Add("ucVessel", ucVessel.SelectedVessel.ToString());
                    criteria.Add("ucNationality", ucNationality.SelectedNationalityValue.ToString());
                    criteria.Add("ucBatch", ucBatch.SelectedBatch.ToString());
                    criteria.Add("ucNationality1", ucNationality1.SelectedNationalityValue.ToString());
                    criteria.Add("rblSelectFrom", ddlSelectFrom.SelectedHard);
                    criteria.Add("NextDays", txtNextDays.Text);
                    criteria.Add("ucRank", ucRank.selectedlist);
                    Filter.CurrentExpiringDocumentsFilter = criteria;
                    ViewState["PAGENUMBER"] = 1;
                    Rebind();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCrewExpiringDocs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string date = DateTime.Now.ToShortDateString();
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            int incre = 1;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDBATCH", "FLDVESSELNAME", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDDOCUMENTNAME", "FLDDOCNATIONALITY", "FLDEXPIRYDATE" };
                string[] alCaptions = { "S.No.", "File No", "Name", "Rank", "Batch", "Vessel", "Sign On Date", "Relief Due", "Document", "Licence Nationality", "Expiry date" };
                string[] FilteralColumns = { "FLDSELECTEDNATIONALITY", "FLDSELECTEDSTATUS", "FLDSELECTEDVESSEL", "FLDSELECTEDTRAININGBATCH", "FLDSELECTEDDAYS" };
                string[] FilteralCaptions = { "Nationality", "Status", "Vessel", "Training batch", "Documents Expiring In Next" };
                
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                ViewState["PAGENUMBER"] = 1;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                ds = CrewReportExpiringSearch.CrewReportExpiringLicense(
                           General.GetNullableString(ucVessel.SelectedVessel.ToString()),
                           (ucNationality.SelectedList) == "Dummy" ? null : General.GetNullableString(ucNationality.SelectedList),
                           (ucNationality1.SelectedNationalityValue.ToString()) == "Dummy" ? null : General.GetNullableString(ucNationality1.SelectedNationalityValue.ToString()),
                           (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch.ToString()),
                           General.GetNullableInteger(ddlSelectFrom.SelectedHard),
                           General.GetNullableInteger(txtNextDays.Text),
                           sortexpression, sortdirection,
                           (int)ViewState["PAGENUMBER"],
                           iRowCount,
                           ref iRowCount,
                           ref iTotalPageCount
                           , (ucRank.selectedlist) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist));

                Response.AddHeader("Content-Disposition", "attachment; filename=All_Expiring_Documents.xls");
                Response.ContentType = "application/vnd.msexcel";
                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
                Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>All Expiring Documents</center></h5></td></tr>");
                //Response.Write("<tr><td colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>From:" + fromdates + "To:" + todatess + "</center></h5></td></tr>");
                Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' ><b>Date:</b>" + date + "</td></tr>");
                //Response.Write("<tr><td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
                Response.Write("</tr>");
                Response.Write("</TABLE>");
                General.ShowFilterCriteriaInExcel(ds, FilteralCaptions, FilteralColumns);
                Response.Write("<br />");
                Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
                Response.Write("<tr>");
                for (int i = 0; i < alCaptions.Length; i++)
                {
                    Response.Write("<td width='20%' align='center'>");
                    Response.Write("<b>" + alCaptions[i] + "</b>");
                    Response.Write("</td>");
                }
                Response.Write("</tr>");


                if (ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Response.Write("<tr>");
                        for (int i = 0; i < alColumns.Length; i++)
                        {
                            if (alColumns[i].ToString() == "FLDDOCUMENTNAME")
                                alColumns[i] = "FLDLICENCE";
                            if (i != 0)
                            {
                                Response.Write("<td align='left'>");
                                Response.Write(dr[alColumns[i]]);
                                Response.Write("</td>");
                            }
                            else
                            {
                                Response.Write("<td align='center'>");
                                Response.Write(incre++);
                                Response.Write("</td>");
                            }
                        }

                    }
                }
                string Csortexpression = (ViewState["CSORTEXPRESSION"] == null) ? null : (ViewState["CSORTEXPRESSION"].ToString());
                int? Csortdirection = null;
                if (ViewState["CSORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["CSORTDIRECTION"].ToString());
                ds = CrewReportExpiringSearch.CrewReportExpiringCourse(
                               General.GetNullableInteger(ucVessel.SelectedVessel.ToString()),
                              (ucNationality.SelectedList) == "Dummy" ? null : General.GetNullableString(ucNationality.SelectedList),
                              (ucNationality1.SelectedList) == "Dummy" ? null : General.GetNullableString(ucNationality1.SelectedList),
                              (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch.ToString()),
                              General.GetNullableInteger(ddlSelectFrom.SelectedHard),
                              General.GetNullableInteger(txtNextDays.Text),
                               Csortexpression, Csortdirection,
                               Int32.Parse(ViewState["CPAGENUMBER"].ToString()),
                               General.ShowRecords(null),
                               ref iRowCount,
                               ref iTotalPageCount, (ucRank.selectedlist) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist));
                if (ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Response.Write("<tr>");
                        for (int i = 0; i < alColumns.Length; i++)
                        {
                            if (alColumns[i].ToString() == "FLDLICENCE" || alColumns[i].ToString() == "FLDDOCUMENTNAME")
                                alColumns[i] = "FLDCOURSE";
                            if (i != 0)
                            {
                                Response.Write("<td align='left'>");
                                Response.Write(dr[alColumns[i]]);
                                Response.Write("</td>");
                            }
                            else
                            {
                                Response.Write("<td align='center'>");
                                Response.Write(incre++);
                                Response.Write("</td>");
                            }
                        }

                    }
                }
                string Osortexpression = (ViewState["OSORTEXPRESSION"] == null) ? null : (ViewState["OSORTEXPRESSION"].ToString());
                int? Osortdirection = null;
                if (ViewState["OSORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["OSORTDIRECTION"].ToString());
                ds = CrewReportExpiringSearch.CrewReportExpiringOtherDocument(
                       General.GetNullableInteger(ucVessel.SelectedVessel.ToString()),
                       General.GetNullableString(ucNationality.SelectedList == "Dummy" ? null : ucNationality.SelectedList),
                       General.GetNullableInteger(ucBatch.SelectedBatch.ToString() == "Dummy" ? null : ucBatch.SelectedBatch.ToString()),
                       General.GetNullableInteger(ddlSelectFrom.SelectedHard),
                       General.GetNullableInteger(txtNextDays.Text),
                       Osortexpression, Osortdirection,
                       Int32.Parse(ViewState["OPAGENUMBER"].ToString()),
                       General.ShowRecords(null),
                       ref iRowCount,
                       ref iTotalPageCount, (ucRank.selectedlist) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist));

                if (ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Response.Write("<tr>");
                        for (int i = 0; i < alColumns.Length; i++)
                        {
                            if (alColumns[i].ToString() == "FLDCOURSE" || alColumns[i].ToString() == "FLDLICENCE")
                                alColumns[i] = "FLDDOCUMENTNAME";
                            if (i != 0)
                            {
                                Response.Write("<td align='left'>");
                                Response.Write(dr[alColumns[i]]);
                                Response.Write("</td>");
                            }
                            else
                            {
                                Response.Write("<td align='center'>");
                                Response.Write(incre++);
                                Response.Write("</td>");
                            }
                        }

                    }
                }
                string Tsortexpression = (ViewState["TSORTEXPRESSION"] == null) ? null : (ViewState["TSORTEXPRESSION"].ToString());
                int? Tsortdirection = null;
                if (ViewState["TSORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["TSORTDIRECTION"].ToString());
                ds = CrewReportExpiringSearch.CrewReportExpiringTravel(
                          General.GetNullableInteger(ucVessel.SelectedVessel.ToString()),
                          (ucNationality.SelectedList) == "Dummy" ? null : General.GetNullableString(ucNationality.SelectedList),
                          (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch.ToString()),
                          General.GetNullableInteger(ddlSelectFrom.SelectedHard),
                          General.GetNullableInteger(txtNextDays.Text),
                          Tsortexpression, Tsortdirection,
                          Int32.Parse(ViewState["TPAGENUMBER"].ToString()),
                          General.ShowRecords(null),
                          ref iRowCount,
                          ref iTotalPageCount, (ucRank.selectedlist) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist));

                if (ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Response.Write("<tr>");
                        for (int i = 0; i < alColumns.Length; i++)
                        {
                            if (alColumns[i].ToString() == "FLDCOURSE" || alColumns[i].ToString() == "FLDLICENCE")
                                alColumns[i] = "FLDDOCUMENTNAME";
                            if (i != 0)
                            {
                                Response.Write("<td align='left'>");
                                Response.Write(dr[alColumns[i]]);
                                Response.Write("</td>");
                            }
                            else
                            {
                                Response.Write("<td align='center'>");
                                Response.Write(incre++);
                                Response.Write("</td>");
                            }
                        }

                    }
                }
                string Msortexpression = (ViewState["MSORTEXPRESSION"] == null) ? null : (ViewState["MSORTEXPRESSION"].ToString());
                int? Msortdirection = null;
                if (ViewState["MSORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["MSORTDIRECTION"].ToString());
                ds = CrewReportExpiringSearch.CrewReportExpiringMedical(
                          General.GetNullableInteger(ucVessel.SelectedVessel.ToString()),
                          (ucNationality.SelectedList) == "Dummy" ? null : General.GetNullableString(ucNationality.SelectedList),
                          (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch.ToString()),
                          General.GetNullableInteger(ddlSelectFrom.SelectedHard),
                          General.GetNullableInteger(txtNextDays.Text),
                          Msortexpression, Msortdirection,
                          Int32.Parse(ViewState["MPAGENUMBER"].ToString()),
                          General.ShowRecords(null),
                          ref iRowCount,
                          ref iTotalPageCount, (ucRank.selectedlist) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist));
                if (ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Response.Write("<tr>");
                        for (int i = 0; i < alColumns.Length; i++)
                        {
                            if (alColumns[i].ToString() == "FLDCOURSE" || alColumns[i].ToString() == "FLDLICENCE")
                                alColumns[i] = "FLDDOCUMENTNAME";
                            if (i != 0)
                            {
                                Response.Write("<td align='left'>");
                                Response.Write(dr[alColumns[i]]);
                                Response.Write("</td>");
                            }
                            else
                            {
                                Response.Write("<td align='center'>");
                                Response.Write(incre++);
                                Response.Write("</td>");
                            }
                        }
                    }
                }

                Response.Write("</TABLE>");
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            RadLabel lblempcode = (RadLabel)e.Item.FindControl("lblEmpCode");
            if (lblempcode.Text != "")
            {
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "&licence=1&cid=2'); return false;");
            }
            else
            {
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + empid.Text + "&licence=1'); return false;");
            }
        }

    }
    protected void gvCrew_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        BindLicenceData();
    }
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

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
    public bool IsValidFilter(string vessellist, string rblselectfrom, string days)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (rblselectfrom.Equals("") || rblselectfrom.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Status";
        }
        if (days.Equals(""))
        {
            ucError.ErrorMessage = "Enter Documents Expiring in Next";
        }
        return (!ucError.IsError);
    }


    protected void gvCrewCourse_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            Label lblempcode = (Label)e.Item.FindControl("lblEmpCode");
            if (lblempcode.Text != "")
            {
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "&course=1'); return false;");
            }
            else
            {
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + empid.Text + "&course=1'); return false;");
            }
        }

    }
    protected void gvCrewCourse_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvCrewCourse_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewCourse.CurrentPageIndex + 1;

        BindCourseData();
    }
    protected void gvCrewCourse_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

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

    protected void gvCrewOtherDoc_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            RadLabel lblempcode = (RadLabel)e.Item.FindControl("lblEmpCode");
            if (lblempcode.Text != "")
            {
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "&Otherdocuments=1'); return false;");
            }
            else
            {
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + empid.Text + "&Otherdocuments=1'); return false;");
            }
        }
    }
    protected void gvCrewOtherDoc_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvCrewOtherDoc_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewOtherDoc.CurrentPageIndex + 1;

        BindOtherDocumentData();
    }
    protected void gvCrewOtherDoc_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

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



    protected void gvCrewTravel_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            RadLabel lblempcode = (RadLabel)e.Item.FindControl("lblEmpCode");
            if (lblempcode.Text != "")
            {
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "&documents=1'); return false;");
            }
            else
            {
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + empid.Text + "&documents=1'); return false;");
            }
        }
    }
    protected void gvCrewTravel_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvCrewTravel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewTravel.CurrentPageIndex + 1;

        BindTravelDocumentData();
    }
    protected void gvCrewTravel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

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



    protected void gvCrewMedical_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            RadLabel lblempcode = (RadLabel)e.Item.FindControl("lblEmpCode");
            if (lblempcode.Text != "")
            {
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "&med=1'); return false;");
            }
            else
            {
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + empid.Text + "&med=1'); return false;");
            }
        }
    }
    protected void gvCrewMedical_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvCrewMedical_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewMedical.CurrentPageIndex + 1;

        BindMedicalData();
    }
    protected void gvCrewMedical_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

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
