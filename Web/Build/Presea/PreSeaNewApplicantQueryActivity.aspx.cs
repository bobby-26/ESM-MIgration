using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PreSeaNewApplicantQueryActivity : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddImageButton("../PreSea/PreSeaNewApplicantQueryActivity.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbarsub.AddImageLink("javascript:Openpopup('Filter','','PreSeaNewApplicantQueryActivityFilter.aspx'); return false;", "Filter", "search.png", "FIND");
            toolbarsub.AddImageButton("../PreSea/PreSeaNewApplicantQueryActivity.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbarsub.AddImageLink("javascript:Openpopup('AddPreseaApplicant','','PreSeaNewApplicantRegister.aspx')", "Add", "add.png", "ADD");

            toolbarsub.AddImageButton("../PreSea/PreSeaNewApplicantQueryActivity.aspx", "Call Letter", "call-letter.png", "CallLetter");
            toolbarsub.AddImageButton("../PreSea/PreSeaNewApplicantQueryActivity.aspx", "Offer Letter", "offer-letter.png", "OfferLetter");
            toolbarsub.AddImageButton("../PreSea/PreSeaNewApplicantQueryActivity.aspx", "Admission Letter", "addmission-letter.png", "AdmissionLetter");
            toolbarsub.AddImageButton("../PreSea/PreSeaNewApplicantQueryActivity.aspx", "Refund Letter", "refund-letter.png", "RefundLetter");

            PreSeaQueryMenu.AccessRights = this.ViewState;
            PreSeaQueryMenu.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["p"] != null)
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["p"]);
                else
                    ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["APPLIEDBATCH"] = null;
                ViewState["TRAININGBATCH"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetPageNavigator()
    {
        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }
    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
        {
            int result;
            if (Int32.TryParse(txtnopage.Text, out result))
            {
                ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

                if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


                if (0 >= Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = 1;

                if ((int)ViewState["PAGENUMBER"] == 0)
                    ViewState["PAGENUMBER"] = 1;

                txtnopage.Text = ViewState["PAGENUMBER"].ToString();
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvPreSeaSearch.SelectedIndex = -1;
            gvPreSeaSearch.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PreSeaQueryMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            NameValueCollection nvc = Filter.CurrentPreSeaNewApplicantFilterCriteria;
            string course = string.Empty;
            string batch = string.Empty;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("SEARCH"))
            {
                BindData();
                SetPageNavigator();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentPreSeaNewApplicantFilterCriteria = null;
                BindData();
                SetPageNavigator();
            }
            else if (dce.CommandName.ToUpper().Equals("CALLLETTER"))
            {
                course = nvc != null ? nvc.Get("ucPreSeaCourse") : string.Empty;
                
                if (!IsValidLetterInfo(course))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ucConfirm.HeaderMessage = "Please Confirm";
                    ucConfirm.Text = "Is the call leter finalised for the exam venue? If yes, click on 'Yes' to send to candidates.If no, click on 'No' to cancel." ;
                    ucConfirm.Visible = true;
                    ucConfirm.CancelText = "No";
                    ucConfirm.OKText = "Yes";
                    ((RadButton)ucConfirm.FindControl("cmdNo")).Focus();
                    return;
                }
            }
            else if (dce.CommandName.ToUpper().Equals("OFFERLETTER"))
            {
                course = nvc != null ? nvc.Get("ucPreSeaCourse") : string.Empty;

                if (!IsValidLetterInfo(course))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    string candidates = ",";
                    SelectedCandtdates(ref candidates);
                    string script = "parent.Openpopup('Bank','','../Reports/ReportsViewWithSubReport.aspx?applicationcode=10&reportcode=OFFERLETTER&showword=no&showexcel=no&showmenu=0&candidateslist=" + candidates + "&course=" + course + "&batch=" + batch + "');";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
                }
            }
            else if (dce.CommandName.ToUpper().Equals("ADMISSIONLETTER"))
            {
                course = nvc != null ? nvc.Get("ucPreSeaCourse") : string.Empty;


                if (!IsValidLetterInfo(course))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    string candidates = ",";
                    SelectedCandtdates(ref candidates);

                    string script = "parent.Openpopup('Bank','','../Reports/ReportsView.aspx?applicationcode=10&reportcode=ADMISSIONLETTER&showword=no&showexcel=no&showmenu=0&candidateslist=" + candidates + "&course=" + course + "&batch=" + batch + "');";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
                }
            }
            else if (dce.CommandName.ToUpper().Equals("REFUNDLETTER"))
            {
                course = nvc != null ? nvc.Get("ucPreSeaCourse") : string.Empty;

                if (!IsValidLetterInfo(course))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    string candidates = ",";
                    SelectedCandtdates(ref candidates);

                    string script = "parent.Openpopup('Bank','','../Reports/ReportsView.aspx?applicationcode=10&reportcode=REFUNDLETTER&showword=no&showexcel=no&showmenu=0&candidateslist=" + candidates + "&course=" + course + "&batch=" + batch + "');";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PreSeaQuery_TabStripCommand(object sender, EventArgs e)
    {
        Filter.CurrentPreSeaNewApplicantSelection = null;
        Session["REFRESHFLAG"] = null;
        Response.Redirect("..\\PreSea\\PreSeaPersonalGeneral.aspx?t=n");
    }
    protected void gvPreSeaSearch_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void gvPreSeaSearch_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper() == "GETEMPLOYEE")
            {

                Filter.CurrentPreSeaNewApplicantSelection = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeid")).Text;                                
                Response.Redirect("..\\PreSea\\PreSeaNewApplicantPersonalGeneral.aspx?p=" + ViewState["PAGENUMBER"].ToString() + "&back=yes");                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPreSeaSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                Label empid = (Label)e.Row.FindControl("lblEmployeeid");

                ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            }              

            ImageButton imgAct = (ImageButton)e.Row.FindControl("imgInterview");
            if (imgAct != null)
            {
                Label lblEmployeeid = (Label)e.Row.FindControl("lblEmployeeid");                        

                imgAct.Visible = SessionUtil.CanAccess(this.ViewState, imgAct.CommandName);
                imgAct.Attributes.Add("onclick", "Openpopup('PDForm', '', '../PreSea/PreSeaEntranceExamInterview.aspx?candidateid=" + lblEmployeeid.Text + "');return false;");
            }
            ImageButton imgCon = (ImageButton)e.Row.FindControl("imgConfirm");

            DataRowView drv1 = (DataRowView)e.Row.DataItem;
            string candidate = drv1["FLDEMPLOYEEID"].ToString();
            string batch = drv1["FLDAPPLIEDBATCH"].ToString();
            string course = drv1["FLDCOURSEID"].ToString();
            string interviewid = drv1["FLDINTERVIEWID"].ToString();

            if (imgCon != null)
            {
                if (drv1["FLDSTATUS"].ToString() != PhoenixCommonRegisters.GetHardCode(1, 255, "NAP") && drv1["FLDSTATUS"].ToString() != PhoenixCommonRegisters.GetHardCode(1, 255, "INV") && drv1["FLDSTATUS"].ToString() != PhoenixCommonRegisters.GetHardCode(1, 255, "NRC"))
                {
                    imgCon.Visible = true;

                    imgCon.Attributes.Add("onclick", "Openpopup('CandidateConfirm', '', '../PreSea/PreSeaCandidateConfirmation.aspx?candidateId=" + candidate + "&batch=" + batch + "&interviewid=" + interviewid + "&course=" + course + "'); return false;");
                    imgCon.Enabled = SessionUtil.CanAccess(this.ViewState, imgCon.CommandName);
                }
                else
                    imgCon.Visible = false;
            }
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
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

            NameValueCollection nvc = Filter.CurrentPreSeaNewApplicantFilterCriteria;
            DataTable dt = PhoenixPreSeaNewApplicantManagement.PreSeaNewApplicantQueryActivity(
                                                                                                 nvc != null ? nvc.Get("txtName") : string.Empty
                                                                                               , General.GetNullableDateTime(nvc != null ? nvc.Get("txtAppliedStartDate") : string.Empty)
                                                                                               , General.GetNullableDateTime(nvc != null ? nvc.Get("txtAppliedEndDate") : string.Empty)
                                                                                               , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOBStartDate") : string.Empty)
                                                                                               , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOBEndDate") : string.Empty)
                                                                                               , nvc != null ? nvc.Get("lstNationality") : string.Empty
                                                                                               , null
                                                                                               , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCountry") : string.Empty)
                                                                                               , General.GetNullableInteger(nvc != null ? nvc.Get("ddlState") : string.Empty)
                                                                                               , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCity") : string.Empty)
                                                                                               , General.GetNullableInteger(nvc != null ? nvc.Get("ucBatch") : string.Empty)
                                                                                               , General.GetNullableInteger(nvc != null ? nvc.Get("ddlQualificaiton") : string.Empty)
                                                                                               , General.GetNullableInteger(nvc != null ? nvc.Get("ucPreSeaCourse") : string.Empty)
                                                                                               , General.GetNullableInteger(nvc != null ? nvc.Get("ucExamVenue1") : string.Empty)
                                                                                               , General.GetNullableInteger(nvc != null ? nvc.Get("ucExamVenue2") : string.Empty)
                                                                                               , General.GetNullableString(nvc != null ? nvc.Get("strApplicantStatus") : string.Empty)                                                                                               
                                                                                               , General.GetNullableInteger(nvc != null ? nvc.Get("rblRecordedBy") : string.Empty)
                                                                                               , General.GetNullableString(nvc != null ? (nvc.Get("txtRecordedByName") == "," ? string.Empty : nvc.Get("txtRecordedByName")) : string.Empty)
                                                                                               , sortexpression, sortdirection
                                                                                               , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                                                               , ref iRowCount, ref iTotalPageCount);


            if (dt.Rows.Count > 0)
            {
                gvPreSeaSearch.DataSource = dt;
                gvPreSeaSearch.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvPreSeaSearch);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

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

        string[] alColumns = { "FLDFIRSTNAME", "FLDMIDDLENAME", "FLDLASTNAME", "FLDDATEOFBIRTH", "FLDCOURSENAME", "FLDBATCHNAME", "FLDEXAMVENUE1", "FLDEXAMVENUE2", "FLDEXAMVENUENAME", "FLDRECORDEDBYNAME" };
        string[] alCaptions = { "First Name", "Middle Name", "Last Name", "Date of Birth", "Course Name", "Applied Batch", "Exam Venue1", "Exam Venue2", "Called Venue", "Recoreded By" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = new NameValueCollection();
        DataTable dt = PhoenixPreSeaNewApplicantManagement.PreSeaNewApplicantQueryActivity(nvc != null ? nvc.Get("txtName") : string.Empty
                                                                                           , General.GetNullableDateTime(nvc != null ? nvc.Get("txtAppliedStartDate") : string.Empty)
                                                                                           , General.GetNullableDateTime(nvc != null ? nvc.Get("txtAppliedEndDate") : string.Empty)
                                                                                           , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOBStartDate") : string.Empty)
                                                                                           , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOBEndDate") : string.Empty)
                                                                                           , nvc != null ? nvc.Get("lstNationality") : string.Empty
                                                                                           , null
                                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCountry") : string.Empty)
                                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("ddlState") : string.Empty)
                                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCity") : string.Empty)
                                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("ucBatch") : string.Empty)
                                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("ddlQualificaiton") : string.Empty)
                                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("ucPreSeaCourse") : string.Empty)
                                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("ucExamVenue1") : string.Empty)
                                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("ucExamVenue2") : string.Empty)
                                                                                           , General.GetNullableString(nvc != null ? nvc.Get("strApplicantStatus") : string.Empty)
                                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("rblRecordedBy") : string.Empty)
                                                                                           , General.GetNullableString(nvc != null ? (nvc.Get("txtRecordedByName") == "," ? string.Empty : nvc.Get("txtRecordedByName")) : string.Empty)
                                                                                           , sortexpression
                                                                                           , sortdirection
                                                                                           , (int)ViewState["PAGENUMBER"]
                                                                                           , General.ShowRecords(null)
                                                                                           , ref iRowCount
                                                                                           , ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=PreSeaApplicant.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>PreSea Applicant</h3></td>");
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
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void ucConfirm_OnClick(object sender, EventArgs e)
    {
        if (sender != null)
        {
            NameValueCollection nvc = Filter.CurrentPreSeaNewApplicantFilterCriteria;

            string candidates = ",";
            string venues = ",";
            string batch = string.Empty;
            SelectedCandtdates(ref candidates, ref venues);
            string course = nvc != null ? nvc.Get("ucPreSeaCourse") : string.Empty;
            string script = "parent.Openpopup('Bank','','../Reports/ReportsViewWithSubReport.aspx?applicationcode=10&reportcode=CALLLETTER&showword=no&showexcel=no&showmenu=0&candidateslist=" + candidates + "&course=" + course + "&venuelist=" + venues + "&isfinal=" + ((UserControlConfirmMessage)sender).confirmboxvalue + "&batch=" + batch + "');";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);

        }

    }
    private void SelectedCandtdates(ref string Candidates, ref string Venues)
    {
        if (gvPreSeaSearch.Rows.Count > 0)
        {
            foreach (GridViewRow row in gvPreSeaSearch.Rows)
            {
                Label lblCandidate = (Label)row.FindControl("lblEmployeeid");
                Label lblVenueId1 = (Label)row.FindControl("lblVenueId1");
                Label lblVenueId2 = (Label)row.FindControl("lblVenueId2");
                CheckBox chkCandidate = (CheckBox)row.FindControl("chkItem");
                CheckBox chkChoose2nd = (CheckBox)row.FindControl("chkChoose2nd");

                if (chkCandidate.Checked == true)
                {
                    Candidates += lblCandidate.Text + ",";
                    Venues += chkChoose2nd.Checked ? lblVenueId2.Text + "," : lblVenueId1.Text + ",";
                }
            }
        }
    }
    private bool IsValidLetterInfo(string course)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        bool candiatecheck = false;

        if (General.GetNullableInteger(course) == null)
        {
            ucError.ErrorMessage = "Course to be filtered ";
        }
        candiatecheck = IsCandidateChecked(candiatecheck);

        if (!candiatecheck)
            ucError.ErrorMessage = "Select atleast one candidate";
        return (!ucError.IsError);

    }

    private void SelectedCandtdates(ref string Candidates)
    {
        if (gvPreSeaSearch.Rows.Count > 0)
        {
            foreach (GridViewRow row in gvPreSeaSearch.Rows)
            {
                Label lblCandidate = (Label)row.FindControl("lblEmployeeid");

                CheckBox cb = (CheckBox)row.FindControl("chkItem");

                if (cb.Checked == true)
                {
                    Candidates += lblCandidate.Text + ",";
                }
            }
        }
    }

    private bool IsCandidateChecked(bool candiatecheck)
    {
        foreach (GridViewRow gvr in gvPreSeaSearch.Rows)
        {
            CheckBox chk = (CheckBox)gvr.FindControl("chkItem");
            if (chk != null)
            {
                if (chk.Checked == true)
                {
                    candiatecheck = true;
                    break;
                }
            }
        }
        return candiatecheck;
    }
}



