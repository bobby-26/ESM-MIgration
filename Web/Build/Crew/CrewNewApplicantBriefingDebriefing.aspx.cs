using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewNewApplicantBriefingDebriefing : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantBriefingDebriefing.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('dgCrewBriefingDebriefing')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewNewApplicantBriefingDebriefingList.aspx'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCREWBRIEFINGDEBRIEFING");        
        MenuCrewBriefingDebriefing.AccessRights = this.ViewState;
        MenuCrewBriefingDebriefing.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            SetEmployeePrimaryDetails();
            dgCrewBriefingDebriefing.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        BindData();
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
                Int32.Parse(Filter.CurrentNewApplicantSelection.ToString())
                , sortexpression, sortdirection
                , int.Parse(ViewState["PAGENUMBER"].ToString())
                , dgCrewBriefingDebriefing.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            General.ShowExcel("Crew Briefing De-Briefing", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
            Int32.Parse(Filter.CurrentNewApplicantSelection.ToString())
            , sortexpression, sortdirection
            , int.Parse(ViewState["PAGENUMBER"].ToString())
            , dgCrewBriefingDebriefing.PageSize
            , ref iRowCount
            , ref iTotalPageCount);
        General.SetPrintOptions("dgCrewBriefingDebriefing", "Crew Briefing De-Briefing", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            dgCrewBriefingDebriefing.DataSource = ds;
            dgCrewBriefingDebriefing.VirtualItemCount = iRowCount;
        }
        else
        {
            dgCrewBriefingDebriefing.DataSource = "";
        }        
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));

            if (dt.Rows.Count > 0)
            {                
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DeleteCrewBriefingDebriefing(int briefingdebriefingid)
    {
        PhoenixCrewBriefingDebriefing.DeleteBriefingDebriefing(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , briefingdebriefingid);
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        dgCrewBriefingDebriefing.Rebind();
    }
    

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void dgCrewBriefingDebriefing_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT")) return;        

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            DeleteCrewBriefingDebriefing(Int32.Parse(((RadLabel)eeditedItem.FindControl("lblCrewBriefingDebriefingId")).Text));
            BindData();
            dgCrewBriefingDebriefing.Rebind();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void dgCrewBriefingDebriefing_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgCrewBriefingDebriefing.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void dgCrewBriefingDebriefing_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
            RadLabel l = (RadLabel)e.Item.FindControl("lblCrewBriefingDebriefingId");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkVessel");
            if (lb != null)
                lb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewNewApplicantBriefingDebriefingList.aspx?CrewBriefingDebriefingId=" + l.Text + "');return false;");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.CREW + "'); return false;");
            }
        }
    }
}
