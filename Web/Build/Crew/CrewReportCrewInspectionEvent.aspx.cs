using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class Crew_CrewReportCrewInspectionEvent : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MainMenuSupdtConcerns.AccessRights = this.ViewState;
            MainMenuSupdtConcerns.MenuList = toolbar.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewReportCrewInspectionEvent.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSupdtConcerns')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewReportCrewInspectionEvent.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuSupdtConcerns.AccessRights = this.ViewState;
            MenuSupdtConcerns.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                BindInspectionEvent();
                BindFeedbackCategory();
                BindFeedbackSubCategory();

                txtStartDate.Text = DateTime.UtcNow.Date.AddMonths(-3).ToShortDateString();
                txtEndDate.Text = DateTime.UtcNow.Date.ToString();
                gvSupdtConcerns.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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
        gvSupdtConcerns.SelectedIndexes.Clear();
        gvSupdtConcerns.EditIndexes.Clear();
        gvSupdtConcerns.DataSource = null;
        gvSupdtConcerns.Rebind();
    }

    private void BindReport()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
          
            string[] alColumns = { "FLDEMPLOYEENAME", "FLDFILENO", "FLDVESSELNAME", "FLDRANKNAME", "FLDSOURCENAME", "FLDSOURCEDATE", "FLDSOURCEREFERENCENUMBER", "FLDCHAPTER", "FLDDEFTYPE", "FLDDESCRIPTION", "FLDSUPERINTENDENT", "FLDREMARKSTOOLTIP", "FLDREMARKSDATE", "FLDFEEDBACKCATEGORY", "FLDFEEDBACKSUBCATEGORY", "FLDKEYANCHOR" };
            string[] alCaptions = { "Employee", "File No", "Vessel", "Rank", "Event", "Event Date", "Reference No", "Chapter", "Deficiency Type", "Description", "Superintendent", "Remarks", "Remarks Date", "Feedback Category", "SubCategory", "Key Anchor" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            
            DataSet ds = PhoenixCrewInspectionSupdtConcerns.SearchEmployeeSuptConcernsReport(
                General.GetNullableString(txtName.Text.Trim())
                , General.GetNullableString(txtFileNumber.Text.Trim())
                , General.GetNullableInteger(ddlRank.SelectedRank)
                , General.GetNullableInteger(ucVessel.SelectedVessel)
                , General.GetNullableInteger(ucVesselType.SelectedVesseltype)
                , General.GetNullableGuid(ddlFeedbackCategory.SelectedValue)
                , General.GetNullableGuid(ddlFeedbackSubCategory.SelectedValue)
                , General.GetNullableGuid(ddlEvent.SelectedValue)
                , General.GetNullableDateTime(txtStartDate.Text)
                , General.GetNullableDateTime(txtEndDate.Text)
                , General.GetNullableDateTime(txtEventDate.Text)
                , General.GetNullableInteger(chkaction.Checked == true ? "1" : null)
                , General.GetNullableInteger(ddlstatus.SelectedValue)
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , gvSupdtConcerns.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            General.SetPrintOptions("gvSupdtConcerns", "Evaluation", alCaptions, alColumns, ds);


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

    protected void MainMenuSupdtConcerns_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidDetails())
                {
                    ucError.Visible = true;
                    return;
                }

                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidDetails()
    {

        ucError.HeaderMessage = "Please provide the following required information";
        if ((General.GetNullableDateTime(txtStartDate.Text) == null) && (General.GetNullableDateTime(txtEventDate.Text) == null))
            ucError.ErrorMessage = "Any one Date Filter is Required";
        if ((General.GetNullableDateTime(txtEndDate.Text) == null) && (General.GetNullableDateTime(txtEventDate.Text) == null))
            ucError.ErrorMessage = "Any one Date Filter is Required";
        if ((General.GetNullableDateTime(txtStartDate.Text) == null) && (General.GetNullableDateTime(txtEndDate.Text) == null) && (General.GetNullableDateTime(txtEventDate.Text) == null))
            ucError.ErrorMessage = "Any one Date Filter is Required";
        return (!ucError.IsError);
    }

    protected void MenuSupdtConcerns_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDEMPLOYEENAME", "FLDFILENO", "FLDVESSELNAME", "FLDRANKNAME", "FLDSOURCENAME", "FLDSOURCEDATE", "FLDSOURCEREFERENCENUMBER", "FLDCHAPTER", "FLDDEFTYPE", "FLDDESCRIPTION", "FLDSUPERINTENDENT", "FLDREMARKSTOOLTIP", "FLDREMARKSDATE", "FLDFEEDBACKCATEGORY", "FLDFEEDBACKSUBCATEGORY", "FLDKEYANCHOR" };
                string[] alCaptions = { "Employee", "File No", "Vessel", "Rank", "Event", "Event Date", "Reference No", "Chapter", "Deficiency Type", "Description", "Superintendent", "Remarks", "Remarks Date", "Feedback Category", "SubCategory", "Key Anchor" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewInspectionSupdtConcerns.SearchEmployeeSuptConcernsReport(
                General.GetNullableString(txtName.Text.Trim())
                , General.GetNullableString(txtFileNumber.Text.Trim())
                , General.GetNullableInteger(ddlRank.SelectedRank)
                , General.GetNullableInteger(ucVessel.SelectedVessel)
                , General.GetNullableInteger(ucVesselType.SelectedVesseltype)
                , General.GetNullableGuid(ddlFeedbackCategory.SelectedValue)
                , General.GetNullableGuid(ddlFeedbackSubCategory.SelectedValue)
                , General.GetNullableGuid(ddlEvent.SelectedValue)
                , General.GetNullableDateTime(txtStartDate.Text)
                , General.GetNullableDateTime(txtEndDate.Text)
                , General.GetNullableDateTime(txtEventDate.Text)
                , General.GetNullableInteger(chkaction.Checked == true ? "1" : null)
                , General.GetNullableInteger(ddlstatus.SelectedValue)
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , gvSupdtConcerns.PageSize
                , ref iRowCount
                , ref iTotalPageCount);
                General.ShowExcel("Evaluation", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtName.Text = "";
                txtFileNumber.Text = "";
                txtEventDate.Text = "";
                txtStartDate.Text = DateTime.UtcNow.Date.AddMonths(-3).ToShortDateString();
                txtEndDate.Text = DateTime.UtcNow.Date.ToString();
                ddlEvent.SelectedValue = "Dummy";
                ddlFeedbackCategory.SelectedValue = "Dummy";
                ddlFeedbackSubCategory.SelectedValue = "Dummy";
                ddlRank.SelectedRank = "0";
                ucVessel.SelectedVessel = "";
                ucVesselType.SelectedVesseltype = "";
                ViewState["PAGENUMBER"] = 1;
                Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSupdtConcerns_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {

                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucRemarks");
                RadLabel lb = (RadLabel)e.Item.FindControl("lblRemarks");
                if (lb != null)
                {
                    lb.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
                    lb.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");
                }
                //if (!e.Item.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
                //&& !e.Item.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
                //{
                DataRowView drv = (DataRowView)e.Item.DataItem;
                LinkButton lbe = (LinkButton)e.Item.FindControl("lnkEmployee");
                if (lbe != null)
                {
                    lbe.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&evaluationevent=1" + "'); return false;");
                }

                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblDTKey");

                LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
                if (att != null) {
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName); }
                                
            }
            if (e.Item is GridDataItem)
            {
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
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void BindInspectionEvent()
    {
        ddlEvent.DataSource = PhoenixInspectionEvent.ListInspectionEvent(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , null);
        ddlEvent.DataTextField = "FLDEVENTNAME";
        ddlEvent.DataValueField = "FLDINSPECTIONEVENTID";
        ddlEvent.DataBind();
        ddlEvent.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

    }
    protected void BindFeedbackCategory()
    {
        ddlFeedbackCategory.DataSource = PhoenixInspectionFeedBackCategory.ListFeedBackCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , null);
        ddlFeedbackCategory.DataTextField = "FLDFEEDBACKCATEGORYNAME";
        ddlFeedbackCategory.DataValueField = "FLDFEEDBACKCATEGORYID";
        ddlFeedbackCategory.DataBind();
        ddlFeedbackCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

    }
    protected void BindFeedbackSubCategory()
    {
        ddlFeedbackSubCategory.DataSource = PhoenixInspectionFeedbackSubCategory.ListFeedbackSubCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
        , null);
        ddlFeedbackSubCategory.DataTextField = "FLDFEEDBACKSUBCATEGORYNAME";
        ddlFeedbackSubCategory.DataValueField = "FLDFEEDBACKSUBCATEGORYID";
        ddlFeedbackSubCategory.DataBind();
        ddlFeedbackSubCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void gvSupdtConcerns_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvSupdtConcerns_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSupdtConcerns.CurrentPageIndex + 1;

        BindReport();
    }
    protected void gvSupdtConcerns_ItemCommand(object sender, GridCommandEventArgs e)
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