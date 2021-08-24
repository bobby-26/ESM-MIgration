using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewBriefingDebriefing : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarMenu = new PhoenixToolbar();
        CrewBriefingDebriefingTab.AccessRights = this.ViewState;
        CrewBriefingDebriefingTab.MenuList = toolbarMenu.Show();

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Crew/CrewBriefingDebriefing.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('dgCrewBriefingDebriefing')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewBriefingDebriefingList.aspx?empid=" + Filter.CurrentCrewSelection + "')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCREWBRIEFINGDEBRIEFING");
        MenuCrewBriefingDebriefing.AccessRights = this.ViewState;
        MenuCrewBriefingDebriefing.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            dgCrewBriefingDebriefing.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            SetEmployeePrimaryDetails();
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


    protected void CrewBriefingDebriefing_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE", "FLDSUBJECT", "FLDINSTRUCTOR", "FLDMODIFIEDDATE", "FLDMODIFIEDBY" };
            string[] alCaptions = { "Vessel", "From Date", "To Date", "Subject", "Instructor", "Created Date", "Created By" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixCrewBriefingDebriefing.BriefingDebriefingSearch(
                Int32.Parse(Filter.CurrentCrewSelection.ToString())
                , sortexpression, sortdirection
                , 1
                , iRowCount
                , ref iRowCount
                , ref iTotalPageCount);

            General.ShowExcel("Briefing De-Briefing", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        dgCrewBriefingDebriefing.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE", "FLDSUBJECT", "FLDINSTRUCTOR", "FLDMODIFIEDDATE", "FLDMODIFIEDBY" };
        string[] alCaptions = { "Vessel", "From Date", "To Date", "Subject", "Instructor", "Created Date", "Created By" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewBriefingDebriefing.BriefingDebriefingSearch(
            Int32.Parse(Filter.CurrentCrewSelection.ToString())
            , sortexpression, sortdirection
            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
            , dgCrewBriefingDebriefing.PageSize
            , ref iRowCount
            , ref iTotalPageCount);

        dgCrewBriefingDebriefing.DataSource = ds;
        dgCrewBriefingDebriefing.VirtualItemCount = iRowCount;

        General.SetPrintOptions("dgCrewBriefingDebriefing", "Briefing De-Briefing", alCaptions, alColumns, ds);

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void dgCrewBriefingDebriefing_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgCrewBriefingDebriefing.CurrentPageIndex + 1;

        BindData();
    }


    protected void dgCrewBriefingDebriefing_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT")) return;
            
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

    protected void dgCrewBriefingDebriefing_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        int briefingdebriefingid = Int32.Parse(((RadLabel)e.Item.FindControl("lblCrewBriefingDebriefingId")).Text);

        PhoenixCrewBriefingDebriefing.DeleteBriefingDebriefing(
           PhoenixSecurityContext.CurrentSecurityContext.UserCode
           , briefingdebriefingid);

        BindData();
        dgCrewBriefingDebriefing.Rebind();        
    }

    protected void dgCrewBriefingDebriefing_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void dgCrewBriefingDebriefing_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            RadLabel l = (RadLabel)e.Item.FindControl("lblCrewBriefingDebriefingId");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkVessel");
            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName))
                {
                    cme.Visible = false;
                    lb.Enabled = false;
                }
                if (l != null)
                {
                    cme.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewBriefingDebriefingList.aspx?CrewBriefingDebriefingId=" + l.Text + "');return false;");
                    lb.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewBriefingDebriefingList.aspx?CrewBriefingDebriefingId=" + l.Text + "');return false;");
                }
            }
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                        + PhoenixModule.CREW + "'); return false;");
            }
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        dgCrewBriefingDebriefing.Rebind();
    }

}
