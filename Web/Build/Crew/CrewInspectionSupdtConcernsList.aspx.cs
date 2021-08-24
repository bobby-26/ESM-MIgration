using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class Crew_CrewInspectionSupdtConcernsList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewInspectionSupdtConcernsList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSupdtConcerns')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewInspectionSupdtConcernsListFilter.aspx?iframIgnore=True'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuSupdtConcerns.AccessRights = this.ViewState;
            MenuSupdtConcerns.MenuList = toolbargrid.Show();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                SetEmployeePrimaryDetails();

                gvSupdtConcerns.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (PhoenixGeneralSettings.CurrentGeneralSetting.ShowSupdtFeedback == 1)
                toolbar.AddButton("Events", "EVENTS");
            toolbar.AddButton("OC 28", "OC28");
            toolbar.AddButton("Score", "SCORE");
            MainMenuSupdtConcerns.AccessRights = this.ViewState;
            MainMenuSupdtConcerns.MenuList = toolbar.Show();
            MainMenuSupdtConcerns.SelectedMenuIndex = 0;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MainMenuSupdtConcerns_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("OC28"))
            {
                Response.Redirect("../Crew/CrewEmployeeSuperintendentAssessment.aspx", false);
            }
            if (CommandName.ToUpper().Equals("SCORE"))
            {
                Response.Redirect("../Inspection/InspectionProsperScorePersonnelMaster.aspx?empid=" + General.GetNullableInteger(Filter.CurrentCrewSelection), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvSupdtConcerns.Rebind();

    }
    protected void MenuSupdtConcerns_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
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


    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string[] alColumns = { "FLDVESSELNAME", "FLDRANKNAME", "FLDSOURCENAME", "FLDSOURCEDATE", "FLDSOURCEREFERENCENUMBER", "FLDCHAPTER", "FLDDEFTYPE", "FLDDESCRIPTION", "FLDSUPERINTENDENT", "FLDREMARKSDATE", "FLDFEEDBACKCATEGORY", "FLDFEEDBACKSUBCATEGORY", "FLDREMARKSTOOLTIP", "FLDKEYANCHOR" };
            string[] alCaptions = { "Vessel", "Rank", "Event", "Event Date", "Ref No", "Chapter", "Def Type", "Description", "Supt", "Date", "Feedback Cat", "Feedback Sub-Cat", "Supt Remarks", "Key Anchor" };

            NameValueCollection nvc = Filter.SupdtConcernsListFilters;
            if (nvc == null)
            {
                nvc = new NameValueCollection();
                nvc.Add("txtName", string.Empty);
                nvc.Add("txtFileNumber", string.Empty);
                nvc.Add("lstRank", string.Empty);
                nvc.Add("ddlVessel", string.Empty);
                nvc.Add("ddlVesselType", string.Empty);
                nvc.Add("ddlFeedbackCategory", string.Empty);
                nvc.Add("ddlFeedbackSubCategory", string.Empty);
                nvc.Add("ddlEvent", string.Empty);
                nvc.Add("ucRecordedDate", string.Empty);
                nvc.Add("ucFromDate", string.Empty);
                nvc.Add("ucToDate", string.Empty);
            }

            DataSet ds = PhoenixCrewInspectionSupdtConcerns.SearchEmployeeSuptConcerns(Convert.ToInt32(Filter.CurrentCrewSelection)
                , General.GetNullableString(nvc.Get("txtName"))
                , General.GetNullableString(nvc.Get("txtFileNumber"))
                , General.GetNullableString(nvc.Get("lstRank"))
                , General.GetNullableInteger(nvc.Get("ddlVessel"))
                , General.GetNullableString(nvc.Get("ddlVesselType"))
                , General.GetNullableGuid(nvc.Get("ddlFeedbackCategory"))
                , General.GetNullableGuid(nvc.Get("ddlFeedbackSubCategory"))
                , General.GetNullableGuid(nvc.Get("ddlEvent"))
                , General.GetNullableDateTime(nvc.Get("ucRecordedDate"))
                , General.GetNullableDateTime(nvc.Get("ucFromDate"))
                , General.GetNullableDateTime(nvc.Get("ucToDate"))
                , 1
                , iRowCount
                , ref iRowCount
                , ref iTotalPageCount);
            General.ShowExcel("Events", ds.Tables[0], alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDVESSELNAME", "FLDRANKNAME", "FLDSOURCENAME", "FLDSOURCEDATE", "FLDSOURCEREFERENCENUMBER", "FLDCHAPTER", "FLDDEFTYPE", "FLDDESCRIPTION", "FLDSUPERINTENDENT", "FLDREMARKSDATE", "FLDFEEDBACKCATEGORY", "FLDFEEDBACKSUBCATEGORY", "FLDREMARKSTOOLTIP", "FLDKEYANCHOR" };
            string[] alCaptions = { "Vessel", "Rank", "Event", "Event Date", "Ref No", "Chapter", "Def Type", "Description", "Supt", "Date", "Feedback Cat", "Feedback Sub-Cat", "Supt Remarks", "Key Anchor" };

            NameValueCollection nvc = Filter.SupdtConcernsListFilters;
            if (nvc == null)
            {
                nvc = new NameValueCollection();
                nvc.Add("txtName", string.Empty);
                nvc.Add("txtFileNumber", string.Empty);
                nvc.Add("lstRank", string.Empty);
                nvc.Add("ddlVessel", string.Empty);
                nvc.Add("ddlVesselType", string.Empty);
                nvc.Add("ddlFeedbackCategory", string.Empty);
                nvc.Add("ddlFeedbackSubCategory", string.Empty);
                nvc.Add("ddlEvent", string.Empty);
                nvc.Add("ucRecordedDate", string.Empty);
                nvc.Add("ucFromDate", string.Empty);
                nvc.Add("ucToDate", string.Empty);
            }

            DataSet ds = PhoenixCrewInspectionSupdtConcerns.SearchEmployeeSuptConcerns(Convert.ToInt32(Filter.CurrentCrewSelection)
                , General.GetNullableString(nvc.Get("txtName"))
                , General.GetNullableString(nvc.Get("txtFileNumber"))
                , General.GetNullableString(nvc.Get("lstRank"))
                , General.GetNullableInteger(nvc.Get("ddlVessel"))
                , General.GetNullableString(nvc.Get("ddlVesselType"))
                , General.GetNullableGuid(nvc.Get("ddlFeedbackCategory"))
                , General.GetNullableGuid(nvc.Get("ddlFeedbackSubCategory"))
                , General.GetNullableGuid(nvc.Get("ddlEvent"))
                , General.GetNullableDateTime(nvc.Get("ucRecordedDate"))
                , General.GetNullableDateTime(nvc.Get("ucFromDate"))
                , General.GetNullableDateTime(nvc.Get("ucToDate"))
               , (int)ViewState["PAGENUMBER"]
               , gvSupdtConcerns.PageSize
               , ref iRowCount
               , ref iTotalPageCount);

            General.SetPrintOptions("gvSupdtConcerns", "Events", alCaptions, alColumns, ds);

            gvSupdtConcerns.DataSource = ds;
            gvSupdtConcerns.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvSupdtConcerns_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSupdtConcerns.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSupdtConcerns_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvSupdtConcerns_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucRemarks");
                RadLabel lb = (RadLabel)e.Item.FindControl("lblRemarks");
                if (lb != null)
                {
                    lb.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
                    lb.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");
                }

                DataRowView drv = (DataRowView)e.Item.DataItem;
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
                RadLabel lblEmployeeFeedBackid = (RadLabel)e.Item.FindControl("lblEmployeeFeedBackid");
                RadLabel lblActualCloseOutDate = (RadLabel)e.Item.FindControl("lblActualCloseOutDate");

                ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0") att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('codeHelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text
                         + "&mod=" + PhoenixModule.QUALITY
                         + "&cmdname=AUDITINSPECTIONUPLOAD"
                         + "&VESSELID=" + lblVesselid.Text
                         + "&u=1'); return false;");


                ImageButton db1 = (ImageButton)e.Item.FindControl("cmdClose");
                if (db1 != null)
                {

                    db1.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Crew/CrewInspectionSupdtConcernsListClosingRemarks.aspx?EmployeeFeedBackid=" + lblEmployeeFeedBackid.Text + "&Date=" + lblActualCloseOutDate.Text + "'); return false;");
                }

                RadLabel lblExpectedClosingDate = (RadLabel)e.Item.FindControl("lblExpectedClosingDate");

                System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
                if (lblExpectedClosingDate != null)
                {
                    DateTime? d = General.GetNullableDateTime(lblExpectedClosingDate.Text);
                    if (d.HasValue)
                    {
                        TimeSpan t = d.Value - DateTime.Now;
                        if (t.Days >= 0 && t.Days < 30)
                        {
                            //e.Row.CssClass = "rowyellow";
                            imgFlag.Visible = true;
                            imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                        }
                        else if (t.Days < 0)
                        {
                            //e.Row.CssClass = "rowred";
                            imgFlag.Visible = true;
                            imgFlag.ImageUrl = Session["images"] + "/red.png";
                        }
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
}