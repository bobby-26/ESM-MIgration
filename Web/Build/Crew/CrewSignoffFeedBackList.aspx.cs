using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Data;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
public partial class Crew_CrewSignoffFeedBackList : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvFBQ.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl00");
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl01");
    //        }
    //    }
    //    base.Render(writer);
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Crew/CrewSignoffFeedBackList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFBQ')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Crew/CrewSignoffFeedBackList.aspx", "Add New Feedback", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        if (!IsPostBack)
        {
            ViewState["CURRENTVESSEL"] = "";
            ViewState["PAGENUMBER"] = 1;
            SetEmployeePrimaryDetails();
            gvFBQ.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        string sVesselId = string.IsNullOrEmpty(ViewState["CURRENTVESSEL"].ToString()) ? "" : ViewState["CURRENTVESSEL"].ToString();
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('VesselAccounts','','"+Session["sitepath"]+"/VesselAccounts/VesselAccountsCrewAppraisalMail.aspx?MailType=FEEDBACK&employeeid="
                  + Filter.CurrentCrewSelection + "&vesselid=" + sVesselId + "'); return false;", "Request feedback", "<i class=\"fas fa-envelope\"></i>", "REQUESTFEEDBACK");

        MenuCrewFeedBack.AccessRights = this.ViewState;
        MenuCrewFeedBack.MenuList = toolbar.Show();
        //BindData();
        //SetPageNavigator();
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
                ViewState["CURRENTVESSEL"] = string.IsNullOrEmpty(dt.Rows[0]["FLDPRESENTVESSEL"].ToString()) ? dt.Rows[0]["FLDLASTVESSEL"].ToString() : dt.Rows[0]["FLDPRESENTVESSEL"].ToString();
                ViewState["RANKID"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
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
        string[] alColumns = { "FLDVESSELNAME", "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDFEEDBACKDATE", "FLDRANKNAME" };
        string[] alCaptions = { "Vessel Name", "From Date", "To Date", "FeedBack Date", "Rank" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        string mnu = Filter.CurrentMenuCodeSelection;
        DataSet ds = PhoenixCrewSignoffFeedBackDetail.SearchFeedBack(
                General.GetNullableInteger(Filter.CurrentCrewSelection)
               , (int)ViewState["PAGENUMBER"]
               , General.ShowRecords(null)
               , ref iRowCount
               , ref iTotalPageCount
               );

        General.ShowExcel("Crew SignOff FeedBack", ds.Tables[0], alColumns, alCaptions, null, "");
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDVESSELNAME", "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDFEEDBACKDATE", "FLDRANKNAME" };
            string[] alCaptions = { "Vessel Name", "From Date", "To Date", "FeedBack Date", "Rank" };
            DataSet ds = PhoenixCrewSignoffFeedBackDetail.SearchFeedBack(
               General.GetNullableInteger(Filter.CurrentCrewSelection)
              , (int)ViewState["PAGENUMBER"]
              , General.ShowRecords(null)
              , ref iRowCount
              , ref iTotalPageCount
              );
            General.SetPrintOptions("gvFBQ", "Crew Sign Off FeedBack", alCaptions, alColumns, ds);

            gvFBQ.DataSource = ds.Tables[0];
            gvFBQ.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   


    protected void CrewFeedBackMain_TabStripCommand(object sender, EventArgs e)
    { }
    protected void MenuCrewFeedBack_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
                Response.Redirect("CrewSignoffFeedBackOfficeAdd.aspx?mode=ADD", false);
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvFBQ_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFBQ.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFBQ_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToString().ToUpper() == "SORT")
            return;


        RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
        RadLabel lblSignOnOffId = (RadLabel)e.Item.FindControl("lblSignOnOffId");
        RadLabel lblFromdate = (RadLabel)e.Item.FindControl("lblFromdate");
        RadLabel lblRankId = (RadLabel)e.Item.FindControl("lblRankId");
        if (e.CommandName.ToString().ToUpper() == "SELECT")
        {
            Response.Redirect("CrewSignoffFeedBackOfficeAdd.aspx?mode=EDIT&VesselId=" + lblVesselId.Text.Trim() + "&SignonoffId=" + lblSignOnOffId.Text.Trim()
                + "&SignOnDate=" + lblFromdate.Text.Trim() + "&Rankid=" + lblRankId.Text.Trim(), false);
        }
        if (e.CommandName.ToString().ToUpper().Equals("CMDEDIT"))
        {
            Response.Redirect("CrewSignoffFeedBackOfficeAdd.aspx?mode=EDIT&VesselId=" + lblVesselId.Text.Trim() + "&SignonoffId=" + lblSignOnOffId.Text.Trim()
                          + "&SignOnDate=" + lblFromdate.Text.Trim() + "&Rankid=" + lblRankId.Text.Trim(), false);
        }
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper() == "DELETE")
        {
            try
            {
               
                string sSignonOffId = ((RadLabel)e.Item.FindControl("lblSignOnOffId")).Text;
                PhoenixCrewSignoffFeedBackDetail.DeleteFeedBack(General.GetNullableInteger(Filter.CurrentCrewSelection), General.GetNullableInteger(sSignonOffId));
                gvFBQ.Rebind();

            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }

    protected void gvFBQ_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        string mnu = Filter.CurrentMenuCodeSelection;
        if (e.Item is GridDataItem)
        {

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            if (att != null) att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);

            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton lbtnvesselname = (LinkButton)e.Item.FindControl("lbtnvesselname");
            if (cmdEdit != null)
            {
                lbtnvesselname.Enabled = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
                cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
            }
            if (drv["FLDISATTACHMENT"].ToString() == string.Empty)
                att.ImageUrl = Session["images"] + "/no-attachment.png";

            att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                + PhoenixModule.CREW + "&type=FEEDBACK&cmdname=FEEDBACKUPLOAD'); return false;");
        }
    }
}
