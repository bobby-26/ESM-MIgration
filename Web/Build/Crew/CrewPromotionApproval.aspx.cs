using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class CrewPromotionApproval : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../Crew/CrewPromotionApproval.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("javascript:CallPrint('gvSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarsub.AddFontAwesomeButton("../Crew/CrewPromotionApproval.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarsub.AddFontAwesomeButton("../Crew/CrewPromotionApproval.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbarsub.Show();
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PD_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtName.Text = "";
                txtFileNumber.Text = "";
                ddlPDStatus.SelectedHard = "";
                ddlRank.SelectedRank = "";
                ViewState["PAGENUMBER"] = 1;
                gvSearch.CurrentPageIndex = 0;
                BindData();
                gvSearch.Rebind();
            }
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
    protected void ShowExcel()
    {
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKFROM", "FLDRANKTO", "FLDSTATUSPD", "FLDDATE", "FLDSTATUSNAME", "FLDREMARKS" };
        string[] alCaptions = { "File No.", "Name", "From Rank", "To Rank", "Request Type", "Request Date", "Status", "Remarks" };

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CrewPDFilterSelection;

        DataTable dt = PhoenixCrewManagement.CrewPromotionApprovalSearch(General.GetNullableString(txtFileNumber.Text.Trim())
                                                       , General.GetNullableString(txtName.Text.Trim())
                                                       , General.GetNullableInteger(ddlRank.SelectedRank)
                                                       , General.GetNullableInteger(ddlPDStatus.SelectedHard)
                                                       , sortexpression, sortdirection
                                                       , (int)ViewState["PAGENUMBER"]
                                                       , gvSearch.PageSize
                                                       , ref iRowCount
                                                       , ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        if (ds.Tables.Count > 0)
            General.ShowExcel("Promotion/Demotion Approvals", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvSearch.Rebind();

    }

    public void BindData()
    {
        try
        {
            string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKFROM", "FLDRANKTO", "FLDSTATUSPD", "FLDDATE", "FLDSTATUSNAME", "FLDREMARKS" };
            string[] alCaptions = { "File No.", "Name", "From Rank", "To Rank", "Request Type", "Request Date", "Status", "Remarks" };

            int iRowCount = 0;
            int iTotalPageCount = 0;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CrewPDFilterSelection;

            DataTable dt = PhoenixCrewManagement.CrewPromotionApprovalSearch(General.GetNullableString(txtFileNumber.Text.Trim())
                                                      , General.GetNullableString(txtName.Text.Trim())
                                                      , General.GetNullableInteger(ddlRank.SelectedRank)
                                                      , General.GetNullableInteger(ddlPDStatus.SelectedHard)
                                                      , sortexpression, sortdirection
                                                      , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                      , gvSearch.PageSize
                                                      , ref iRowCount
                                                      , ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvSearch", "Promotion/Demotion Approvals", alCaptions, alColumns, ds);

            gvSearch.DataSource = dt;
            gvSearch.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSearch.CurrentPageIndex + 1;

        BindData();
    }


    protected void gvSearch_ItemCommand(object sender, GridCommandEventArgs e)
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
    protected void gvSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmployeeId");
            RadLabel aptype = (RadLabel)e.Item.FindControl("lblAppType");
            RadLabel lblRank = (RadLabel)e.Item.FindControl("lblRank");
            RadLabel lblSupt = (RadLabel)e.Item.FindControl("lblSupt");
            RadLabel pdtype = (RadLabel)e.Item.FindControl("lblPdType");

            LinkButton app = (LinkButton)e.Item.FindControl("cmdApprove");
            LinkButton pd = (LinkButton)e.Item.FindControl("cmdPDForm");
            LinkButton cm = (LinkButton)e.Item.FindControl("cmdComment");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkemployee");


            if (cm != null)
            {
                cm.Visible = SessionUtil.CanAccess(this.ViewState, cm.CommandName);
                cm.Attributes.Add("onclick", "javascript:openNewWindow('CrewComment','','" + Session["sitepath"] + "/Crew/CrewPendingApprovalRemarks.aspx?dtkey=" + lblDTKey.Text + "&empid=" + empid.Text + "'); return false;");
            }


            if (lbr != null)
            {
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
            }

            if (pd != null)
            {
                if (pd != null) pd.Visible = SessionUtil.CanAccess(this.ViewState, pd.CommandName);
                pd.Attributes.Add("onclick", "javascript:openNewWindow('approval', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=EMPLOYEECV&employeeid=" + empid.Text + "&vslid=&rankid=" + lblRank.Text + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");
            }

            if (app != null)
            {
                if (app != null) app.Visible = SessionUtil.CanAccess(this.ViewState, app.CommandName);
                app.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/CommonCrewApproval.aspx?docid=" + lblDTKey.Text + "&mod=" + PhoenixModule.CREW
            + "&type=" + aptype.Text + "&empid=" + empid.Text + "&pdtype=" + pdtype.Text + "&subtype=PROMOTION" + "');return false;");
            }
            
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblPDStatus");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");            
            uct.Position = ToolTipPosition.TopCenter;
            uct.TargetControlId = lbtn.ClientID;
            
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

                att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                        + PhoenixModule.CREW + "'); return false;");
            }
            
        }
    }


}
