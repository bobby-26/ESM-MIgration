using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.IO;
using SouthNests.Phoenix.Reports;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CrewOffshorePortalPendingAppraisal : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Appraisal", "APPRAISAL");
            //toolbarmain.AddButton("De-Briefing", "DEBRIEFING");
            toolbarmain.AddButton("Home", "HOME", ToolBarDirection.Right);

            CrewMain.AccessRights = this.ViewState;
            CrewMain.MenuList = toolbarmain.Show();
            CrewMain.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                DataTable dt = PhoenixCrewManagement.PortalEmployee(PhoenixSecurityContext.CurrentSecurityContext.UserCode);

                if (dt.Rows.Count > 0)
                {
                    Filter.CurrentCrewSelection = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                SetEmployeePrimaryDetails();

                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                gvAQ.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshorePortalPendingAppraisal.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAQ')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuCrewAppraisal.AccessRights = this.ViewState;
            MenuCrewAppraisal.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("APPRAISAL"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshorePortalPendingAppraisal.aspx?empid=" + Filter.CurrentCrewSelection, false);
        }
        if (CommandName.ToUpper().Equals("DEBRIEFING"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshorePortalDeBriefing.aspx?empid=" + Filter.CurrentCrewSelection, false);
        }
        if (CommandName.ToUpper().Equals("HOME"))
        {
            // Response.Redirect("../Dashboard/DashboardHome.aspx?Type=p&empid=" + Filter.CurrentCrewSelection);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                  "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
        }

    }

    protected void Rebind()
    {
        gvAQ.SelectedIndexes.Clear();
        gvAQ.EditIndexes.Clear();
        gvAQ.DataSource = null;
        gvAQ.Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE", "FLDAPPRAISALDATE", "FLDOCCASSIONFORREPORT", "FLDRANKNAME", "FLDPROMOTIONYESNO", "FLDAPPRAISALSTATUS" };
        string[] alCaptions = { "Vessel", "From", "To", "Appraisal On", "Occasion For Report", "Rank", "Promotion", "Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        DataSet ds = new DataSet();

        if (Filter.CurrentCrewSelection != null)
        {
            ds = PhoenixCrewAppraisal.PortalPendingAppraisalSearch(
                  General.GetNullableInteger(Filter.CurrentCrewSelection)
                 , sortdirection
                 , 1
                 , iRowCount
                 , ref iRowCount
                 , ref iTotalPageCount
                 );
        }
        else
        {
            gvAQ.DataSource = ds;
        }

        General.ShowExcel("Appraisal", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void MenuCrewAppraisal_TabStripCommand(object sender, EventArgs e)
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
            DataTable dt = new DataTable();

            dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAQ_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToString().ToUpper() == "SELECT")
            {
                string lblAppraisalId = ((RadLabel)e.Item.FindControl("lblAppraisalId")).Text;
                if (lblAppraisalId != "")
                {
                    Response.Redirect("..\\CrewOffshore\\CrewOffshoreAppraisalDetailforCrewComment.aspx?aprid=" + lblAppraisalId);
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
    protected void gvAQ_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            string mnu = Filter.CurrentMenuCodeSelection;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdAtt");
            if (db != null)
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                if (drv["FLDISATTACHMENT"].ToString() == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes["onclick"] = "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                + PhoenixModule.CREW + "&u=n" + " & type=APPRAISAL&cmdname=APPRAISALUPLOAD'); return false;";
            }

        }
    }
    protected void gvAQ_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAQ.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE", "FLDAPPRAISALDATE", "FLDOCCASSIONFORREPORT", "FLDRANKNAME", "FLDPROMOTIONYESNO", "FLDAPPRAISALSTATUS", "FLDRECOMMENDEDSTATUSNAME" };
        string[] alCaptions = { "Vessel", "From", "To", "Appraisal On", "Occasion For Report", "Rank", "Promotion", "Status", "Fit for reemploy" };

        int? sortdirection = 1; //DEFAULT DESC SORT
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {
            DataSet ds = new DataSet();

            if (Filter.CurrentCrewSelection != null)
            {
                ds = PhoenixCrewAppraisal.PortalPendingAppraisalSearch(General.GetNullableInteger(Filter.CurrentCrewSelection)
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , gvAQ.PageSize
                , ref iRowCount
                , ref iTotalPageCount
                );


                General.SetPrintOptions("gvAQ", "Appraisal", alCaptions, alColumns, ds);
                gvAQ.DataSource = ds.Tables[0];
                gvAQ.VirtualItemCount = iRowCount;

            }
            else
            {
                gvAQ.DataSource = ds;
            }

            Filter.CurrentAppraisalSelection = null;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
