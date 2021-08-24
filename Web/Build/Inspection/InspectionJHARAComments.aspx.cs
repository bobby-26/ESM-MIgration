using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionJHARAComments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();


        //toolbar.AddFontAwesomeButton("../Inspection/InspectionJHARAComments.aspx", "Add New", "<i class=\"fa fa-plus\"></i>", "ADD");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionJHARAComments.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        toolbar.AddFontAwesomeButton("javascript: CallPrint('gvRiskAssessment')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionJHARACommentsSearch.aspx?')", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
        //toolbar.AddFontAwesomeButton("../Inspection/InspectionJHARAComments.aspx", "Find Section", "<i class=\"fa fa-search-minus\"></i>", "SECTIONSEARCH");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionJHARAComments.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionBulkReviewOfficeRemarks.aspx?')", "Bulk Office Comments", "<i class=\"fa fa-comments\"></i>", "BULKOFFICECOMMENTS");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionJHARAComments.aspx", "Bulk Review", "<i class=\"fa fa-check-square\"></i>", "BULKREVIEW");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionJHARAComments.aspx", "Bulk Archive", "<i class=\"fa fa-download\"></i>", "BULKARCHIVE");

        MenuComments.AccessRights = this.ViewState;
        MenuComments.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            confirmarchive.Attributes.Add("style", "display:none;");
            confirmReview.Attributes.Add("style", "display:none;");

            ViewState["REFID"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["TYPE"] = "0";
            ViewState["COMPANYID"] = "";

            NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvcCompany.Get("QMS");
                //ucVessel.Company = nvcCompany.Get("QMS");
                //ucVessel.bind();
            }
            if (Request.QueryString["filter"] != null)
            {
                if (Request.QueryString["filter"].ToString() == "0")
                {
                    InspectionFilter.CurrentRACommentFilter = null;
                }
            }
            //if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            //{
            //    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            //    ucVessel.Enabled = false;
            //}
            //BindCategory();

            gvRiskAssessment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string vesselid = "";
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDRANUMBER", "FLDCOMMENTS", "FLDSOURCE", "FLDPOSTEDBY", "FLDACCEPTEDBY", "FLDDUEDATE", "FLDOFFICEREMARKS", "FLDREVIEWEDBY", "FLDREVIEWEDDATE", "FLDCOMPLETIONDATE" };
        string[] alCaptions = { "RA/JHA No.", "Comment", "Source", "Posted by", "Accepted By", "Due", "Office Remarks", "Reviewed By", "Reviewed On", "Completed On" };

        NameValueCollection nvc = InspectionFilter.CurrentRACommentFilter;

        if (InspectionFilter.CurrentRACommentFilter == null)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

        if (nvc != null && General.GetNullableInteger(nvc["ucVessel"]) == null)
        {
            nvc["ucVessel"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }

        DataSet ds = PhoenixInspectionRiskAssessmentGenericExtn.RACommentSearch(
                     nvc != null ? General.GetNullableString(nvc.Get("RefNo").ToString()) : null
                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                    , gvRiskAssessment.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , sortexpression, sortdirection
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlRAJHASections")) : null
                    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlSource")) : null
                    );

        General.SetPrintOptions("gvRiskAssessment", "RA Comments", alCaptions, alColumns, ds);

        gvRiskAssessment.DataSource = ds;
        gvRiskAssessment.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void ShowExcel()
    {
        try
        {

            int iRowCount = 0;
            int iTotalPageCount = 0;
            string vesselid = "";
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string[] alColumns = { "FLDRANUMBER", "FLDCOMMENTS", "FLDSOURCE", "FLDPOSTEDBY", "FLDACCEPTEDBY", "FLDDUEDATE", "FLDOFFICEREMARKS", "FLDREVIEWEDBY", "FLDREVIEWEDDATE", "FLDCOMPLETIONDATE" };
            string[] alCaptions = { "RA/JHA No.", "Comment", "Source", "Posted by", "Accepted By", "Due", "Office Remarks", "Reviewed By", "Reviewed On", "Completed On" };

            NameValueCollection nvc = InspectionFilter.CurrentRACommentFilter;

            if (InspectionFilter.CurrentRACommentFilter == null)
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            if (nvc != null && General.GetNullableInteger(nvc["ucVessel"]) == null)
            {
                nvc["ucVessel"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            }

            DataSet ds = PhoenixInspectionRiskAssessmentGenericExtn.RACommentSearch(
                         nvc != null ? General.GetNullableString(nvc.Get("RefNo").ToString()) : null
                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvRiskAssessment.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount
                        , sortexpression, sortdirection
                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                        , nvc != null ? General.GetNullableInteger(nvc.Get("ddlRAJHASections")) : null
                        , nvc != null ? General.GetNullableInteger(nvc.Get("ddlSource")) : null
                        );

            General.ShowExcel("RA Comments", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvRiskAssessment.SelectedIndexes.Clear();
        gvRiskAssessment.EditIndexes.Clear();
        gvRiskAssessment.DataSource = null;
        gvRiskAssessment.Rebind();
    }
    protected void MenuComments_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADD"))
        {
            Response.Redirect("../Inspection/InspectionJHARAComments.aspx?viewonly=1", false);
        }

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("SEARCH"))
        {
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            InspectionFilter.CurrentRACommentFilter = null;
            BindData();
            gvRiskAssessment.Rebind();
        }
        if (CommandName.ToUpper().Equals("DELETE"))
        {
        }
        if (CommandName.ToUpper().Equals("BULKARCHIVE"))
        {
            RadWindowManager1.RadConfirm("Are you sure you want to archive all the selected comments.? Y/N", "confirmarchive", 320, 150, null, "Confirm");
        }
        if (CommandName.ToUpper().Equals("BULKREVIEW"))
        {
            RadWindowManager2.RadConfirm("Are you sure you want to review all the selected comments.? Y/N", "confirmReview", 320, 150, null, "Confirm");
        }
    }

    protected void gvRiskAssessment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadLabel lblRiskassessmentId = (RadLabel)e.Item.FindControl("lblRiskassessmentId");
        RadLabel lblCommentId = (RadLabel)e.Item.FindControl("lblCommentId");
        LinkButton txtCommentId = (LinkButton)e.Item.FindControl("lnkcomments");
        LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
        if (eb != null)
        {
            if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                eb.Visible = false;

            eb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionCommentsEdit.aspx?COMMENTID=" + lblCommentId.Text + "&RISKASSESSMENTID=" + lblRiskassessmentId.Text + "'); return true;");
        }

        RadLabel txtCommentsId = (RadLabel)e.Item.FindControl("lblCommentId");
        LinkButton cmdMoreInfo = (LinkButton)e.Item.FindControl("cmdMoreInfo");
        if (cmdMoreInfo != null)
        {
            cmdMoreInfo.Attributes.Add("onclick", "openNewWindow('codeHelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionJHARACommentsMoreInfo.aspx?CommentsId=" + txtCommentsId.Text + "');return false;");
        }

        DataRowView dv = (DataRowView)e.Item.DataItem;
        LinkButton cmdArchive = (LinkButton)e.Item.FindControl("cmdArchive");
        RadCheckBox chkSelect = (RadCheckBox)e.Item.FindControl("chkSelect");
        LinkButton cmdReview = (LinkButton)e.Item.FindControl("cmdReview");

        if (cmdArchive != null && dv["FLDARCHIVEDYN"].ToString() == "1")
        {
            cmdArchive.Visible = false;
            chkSelect.Visible = false;
        }


        if (cmdArchive != null && !SessionUtil.CanAccess(this.ViewState, cmdArchive.CommandName))
            cmdArchive.Visible = false;

        if (cmdReview != null && !SessionUtil.CanAccess(this.ViewState, cmdReview.CommandName))
            cmdReview.Visible = false;

        if (!e.Item.IsInEditMode)
        {
            if (cmdArchive != null)
            {
                cmdArchive.Attributes.Add("onclick", "return fnConfirmTelerik(event,'Do you want to archive this comment.'); return false;");
            }

            if (cmdReview != null)
            {
                cmdReview.Attributes.Add("onclick", "return fnConfirmTelerik(event,'Do you want to review this comment.'); return false;");
            }
        }
    }

    protected void gvRiskAssessment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRiskAssessment.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRiskAssessment_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        GridEditableItem eeditedItem = gce.Item as GridEditableItem;
        RadGrid _gridView = (RadGrid)sender;
        int nCurrentRow = gce.Item.ItemIndex;

        if (gce.CommandName.ToUpper().Equals("ARCHIVE"))
        {
            BindPageURL(nCurrentRow);
            RadLabel lblCommentId = ((RadLabel)gce.Item.FindControl("lblCommentId"));

            PhoenixInspectionRiskAssessmentGenericExtn.RACommentArchive(
                                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                     , new Guid(lblCommentId.Text)
                                     , 1
                                     );
            ucStatus.Text = "Comment is archived.";
            BindData();
            if (Filter.CurrentSelectedComments != null && ViewState["SELECTEDCOMMENTS"].ToString() == "Search")
            {
                // BindSelectedSection();
                // Filter.CurrentSelectedComments = null;
            }
            gvRiskAssessment.Rebind();
        }

        else if (gce.CommandName.ToUpper().Equals("REVIEW"))
        {
            BindPageURL(nCurrentRow);
            RadLabel lblCommentId = ((RadLabel)gce.Item.FindControl("lblCommentId"));

            PhoenixInspectionRiskAssessmentGenericExtn.RACommentReviewSinglepdate(
                                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                     , new Guid(lblCommentId.Text)
                                     );
            ucStatus.Text = "Comment is reviewed.";
            BindData();
            //if (Filter.CurrentSelectedComments != null && ViewState["SELECTEDCOMMENTS"].ToString() == "Search")
            //{
            //    //BindSelectedSection();
            //    //Filter.CurrentSelectedComments = null;
            //}
            gvRiskAssessment.Rebind();

        }

        if (gce.CommandName.ToUpper().Equals("DELETE"))
        {
            RadLabel lblCommentId = (RadLabel)gce.Item.FindControl("lblCommentId");
            PhoenixInspectionRiskAssessmentGenericExtn.RACommentDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(lblCommentId.Text));
            ViewState["DMSCOMMENTSID"] = "";
            ucStatus.Text = "Comment is deleted.";
            BindData();
            if (Filter.CurrentSelectedComments != null && ViewState["SELECTEDCOMMENTS"].ToString() == "Search")
            {
                // BindSelectedSection();
                //Filter.CurrentSelectedComments = null;
            }
            gvRiskAssessment.Rebind();
        }

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvRiskAssessment.Rebind();
    }
    private void SetRowSelectionForSearch()
    {
        foreach (GridDataItem item in gvRiskAssessment.Items)
        {
            if (item.GetDataKeyValue("FLDRISKASSESSMENTCOMMENTID").ToString().Equals(ViewState["RACOMMENTSID"].ToString()))
            {
                ArrayList selectedvalue = new ArrayList();
                CheckBox cb = (CheckBox)gvRiskAssessment.Items[item.ItemIndex].FindControl("chkSelect");
                cb.Checked = true;
                if (cb.Checked == true)
                {
                    if (!selectedvalue.Contains(new Guid(ViewState["RACOMMENTSID"].ToString())))
                        selectedvalue.Add(new Guid(ViewState["RACOMMENTSID"].ToString()));
                }
                else
                    selectedvalue.Remove(new Guid(ViewState["RACOMMENTSID"].ToString()));

                if (selectedvalue != null && selectedvalue.Count > 0)
                    Filter.CurrentSelectedComments = selectedvalue;
            }
        }
    }

    private void SetRowSelection()
    {
        foreach (GridDataItem item in gvRiskAssessment.Items)
        {
            if (item.GetDataKeyValue("FLDRISKASSESSMENTCOMMENTID").ToString().Equals(ViewState["RACOMMENTSID"].ToString()))
            {
                gvRiskAssessment.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }
    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblCommentId = (RadLabel)gvRiskAssessment.Items[rowindex].FindControl("lblCommentId");
            if (lblCommentId != null)
            {
                ViewState["RACOMMENTSID"] = lblCommentId.Text;
            }
            SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CheckAll(Object sender, EventArgs e)
    {
        RadCheckBox headerCheckBox = (sender as RadCheckBox);
        ArrayList SelectedForms = new ArrayList();
        Guid index = new Guid();

        foreach (GridDataItem dataItem in gvRiskAssessment.MasterTableView.Items)
        {
            bool result = false;
            if (headerCheckBox.Checked == true)
            {
                if (dataItem.GetDataKeyValue("FLDRISKASSESSMENTCOMMENTID").ToString() != "")
                {
                    index = new Guid(dataItem.GetDataKeyValue("FLDRISKASSESSMENTCOMMENTID").ToString());
                    (dataItem.FindControl("chkSelect") as RadCheckBox).Checked = true;
                    result = true;
                }
                //dataItem.Selected = true;
            }
            else
            {
                (dataItem.FindControl("chkSelect") as RadCheckBox).Checked = false;
                Filter.CurrentSelectedComments = null;
            }

            // Check in the Session
            if (Filter.CurrentSelectedComments != null)
                SelectedForms = (ArrayList)Filter.CurrentSelectedComments;
            if (result)
            {
                if (!SelectedForms.Contains(index))
                    SelectedForms.Add(index);
            }
            else
                SelectedForms.Remove(index);
        }
        if (SelectedForms != null && SelectedForms.Count > 0)
            Filter.CurrentSelectedComments = SelectedForms;
    }

    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedSections = new ArrayList();
        Guid index = new Guid();

        foreach (GridDataItem item in gvRiskAssessment.Items)
        {
            bool result = false;

            if (item.GetDataKeyValue("FLDRISKASSESSMENTCOMMENTID").ToString() != "")
            {
                index = new Guid(item.GetDataKeyValue("FLDRISKASSESSMENTCOMMENTID").ToString());

                if (((RadCheckBox)(item.FindControl("chkSelect"))).Checked == true)
                {
                    result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
                }

                // Check in the Session
                if (Filter.CurrentSelectedComments != null)
                    SelectedSections = (ArrayList)Filter.CurrentSelectedComments;
                if (result)
                {
                    if (!SelectedSections.Contains(index))
                        SelectedSections.Add(index);
                }
                else
                    SelectedSections.Remove(index);
            }
        }
        if (SelectedSections != null && SelectedSections.Count > 0)
            Filter.CurrentSelectedComments = SelectedSections;
    }

    private void GetSelectedPvs()
    {
        if (Filter.CurrentSelectedComments != null)
        {
            ArrayList SelectedSections = (ArrayList)Filter.CurrentSelectedComments;
            Guid index = new Guid();
            if (SelectedSections != null && SelectedSections.Count > 0)
            {
                foreach (GridDataItem row in gvRiskAssessment.Items)
                {
                    //CheckBox ChkPlan = (CheckBox)row["ClientSelectColumn"].Controls[0];
                    //CheckBox chk = (CheckBox)(row.Cells[0].FindControl("chkSelect"));
                    RadCheckBox chk = (RadCheckBox)(row.Cells[0].FindControl("chkSelect"));
                    index = new Guid(row.GetDataKeyValue("FLDRISKASSESSMENTCOMMENTID").ToString());
                    if (SelectedSections.Contains(index))
                    {
                        RadCheckBox myCheckBox = (RadCheckBox)row.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }

    }

    protected void confirmarchive_Click(object sender, EventArgs e)
    {
        try
        {
            if (Filter.CurrentSelectedComments != null)
            {
                ArrayList selectedcomments = (ArrayList)Filter.CurrentSelectedComments;
                if (selectedcomments != null && selectedcomments.Count > 0)
                {
                    foreach (Guid commentid in selectedcomments)
                    {
                        PhoenixInspectionRiskAssessmentGenericExtn.RACommentArchive(
                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , new Guid(commentid.ToString())
                                                 , 1
                                                 );
                    }
                }
            }
            //Filter.CurrentSelectedComments = null;
            ucStatus.Text = "Comments are archived.";
            BindData();
            //if (Filter.CurrentSelectedComments != null && ViewState["SELECTEDCOMMENTS"].ToString() == "Search")
            //{
            //    // BindSelectedSection();
            //    //Filter.CurrentSelectedComments = null;
            //}
            gvRiskAssessment.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void confirmReview_Click(object sender, EventArgs e)
    {
        try
        {
            if (Filter.CurrentSelectedComments != null)
            {
                ArrayList selectedcomments = (ArrayList)Filter.CurrentSelectedComments;
                if (selectedcomments != null && selectedcomments.Count > 0)
                {
                    foreach (Guid commentid in selectedcomments)
                    {
                        PhoenixInspectionRiskAssessmentGenericExtn.RACommentReviewSinglepdate(
                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , new Guid(commentid.ToString())
                                                 );
                    }
                }
            }
            //Filter.CurrentSelectedComments = null;
            ucStatus.Text = "Comments are reviewed.";
            BindData();
            //if (Filter.CurrentSelectedComments != null && ViewState["SELECTEDCOMMENTS"].ToString() == "Search")
            //{
            //    //BindSelectedSection();
            //    //Filter.CurrentSelectedComments = null;
            //}
            gvRiskAssessment.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

}