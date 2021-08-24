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

public partial class CrewPD : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../Crew/CrewPD.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("javascript:CallPrint('gvSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarsub.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewPDFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarsub.AddFontAwesomeButton("../Crew/CrewPD.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");

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
                Filter.CrewPDFilterSelection = null;
                ViewState["PAGENUMBER"] = 1;
                gvSearch.CurrentPageIndex = 0;
                //BindData();
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
        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDEXPECTEDJOINDATE", "FLDPDSTATUS", "FLDOFFSIGNERNAME", "FLDRELIEVERRANK", "FLDRELIEFDUEDATE", "FLDVESSELNAME", "FLDPROPOSEDBY", "FLDSTATUS", "FLDMUMBAIREMARKS" };
        string[] alCaptions = { "Name", "Rank", "Planned Relief", "PD Status", "Name", "Rank", "Relief Due", "Vessel", "Proposed By", "Category", "Superintendent  Remarks" };

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CrewPDFilterSelection;
        DataTable dt = PhoenixCrewManagement.PDList(General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ddlPDStatus"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ucPrincipal"] : string.Empty)
                                                        , sortexpression, sortdirection
                                                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                        , gvSearch.PageSize
                                                        , ref iRowCount
                                                        , ref iTotalPageCount
                                                        , General.GetNullableInteger(nvc != null ? nvc["ddlUser"] : string.Empty));

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        if (ds.Tables.Count > 0)
            General.ShowExcel("Pending Approval", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;

        //BindData();
        gvSearch.Rebind();
    }

    protected void gvSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSearch.CurrentPageIndex + 1;

        BindData();
    }

    public void BindData()
    {

        try
        {
            string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDEXPECTEDJOINDATE", "FLDPDSTATUS", "FLDOFFSIGNERNAME", "FLDRELIEVERRANK", "FLDRELIEFDUEDATE", "FLDVESSELNAME", "FLDPROPOSEDBY", "FLDSTATUS", "FLDMUMBAIREMARKS" };
            string[] alCaptions = { "Name", "Rank", "Planned Relief", "PD Status", "Name", "Rank", "Relief Due", "Vessel", "Proposed By", "Category", "Superintendent  Remarks" };

            int iRowCount = 0;
            int iTotalPageCount = 0;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CrewPDFilterSelection;
            DataTable dt = PhoenixCrewManagement.PDList(General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ddlPDStatus"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ucPrincipal"] : string.Empty)
                                                       , sortexpression, sortdirection
                                                       , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                       , gvSearch.PageSize
                                                       , ref iRowCount
                                                       , ref iTotalPageCount
                                                        , General.GetNullableInteger(nvc != null ? nvc["ddlUser"] : string.Empty));

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvSearch", "Pending Approvals", alCaptions, alColumns, ds);


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




    protected void gvSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

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
   
    protected void gvSearch_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel interviewid = e.Item.FindControl("lblInterviewId") as RadLabel;
            RadTextBox rmk = e.Item.FindControl("txtMumbaiRemarks") as RadTextBox;
            if (!IsValidateRemarks(interviewid.Text))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixCrewManagement.UpdateCrewRelatedRemarks(int.Parse(interviewid.Text), PhoenixCrewAttachmentType.ASSESSMENT.ToString(), rmk.Text);
            BindData();
            gvSearch.Rebind();

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
            RadLabel lblCrewPlanId = (RadLabel)e.Item.FindControl("lblCrewPlanId");
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmployeeId");
            RadLabel vslid = (RadLabel)e.Item.FindControl("lblVessel");
            RadLabel rnk = (RadLabel)e.Item.FindControl("lblRank");
            RadLabel lblSupt = (RadLabel)e.Item.FindControl("lblSupt");
            RadLabel aptype = (RadLabel)e.Item.FindControl("lblAppType");
            RadLabel newapp = (RadLabel)e.Item.FindControl("lblNewApp");
            RadLabel pdtype = (RadLabel)e.Item.FindControl("lblPdType");

            LinkButton app = (LinkButton)e.Item.FindControl("cmdApprove");
            LinkButton pd = (LinkButton)e.Item.FindControl("cmdPDForm");
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton cm = (LinkButton)e.Item.FindControl("cmdComment");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkemployee");


            if (lbr != null)
            {
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
            }

            if (pd != null)
            {
                if (pd != null) pd.Visible = SessionUtil.CanAccess(this.ViewState, pd.CommandName);
                pd.Attributes.Add("onclick", "javascript:openNewWindow('approval', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=EMPLOYEECV&employeeid=" + empid.Text + "&vslid=" + vslid.Text + "&rankid=" + rnk.Text + "&showmenu=0');return false;");
            }

            if (app != null)
            {
               
                app.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '"+ Session["sitepath"] + "/Common/CommonCrewApproval.aspx?docid=" + lblCrewPlanId.Text + "&mod=" + PhoenixModule.CREW
                   + "&type=" + aptype.Text + "&user=" + lblSupt.Text + "&vslid=" + vslid.Text + (newapp.Text == "1" ? "&newapp=1" : string.Empty) + "&empid=" + empid.Text + "&pdtype=" + pdtype.Text + "');return false;");
            }



            RadLabel top4 = (RadLabel)e.Item.FindControl("lblTop4");
            //RadLabel rmk = (RadLabel)e.Item.FindControl("lblRemarks");
            //if (rmk != null)
            //{
            //    if (rmk.Text.Trim() != string.Empty)
            //    {
            //        if (app != null)
            //            app.Visible = true;
            //        if (ed != null)
            //            ed.Visible = false;
            //    }
            //    else
            //    {
            //        if (app != null)
            //            app.Visible = false;
            //        if (ed != null)
            //            ed.Visible = true;
            //    }
            //}

            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            if (app != null) app.Visible = SessionUtil.CanAccess(this.ViewState, app.CommandName);

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.ASSESSMENT + "'); return false;");
            }

            if (cm != null)
            {
                cm.Visible = SessionUtil.CanAccess(this.ViewState, cm.CommandName);
                cm.Attributes.Add("onclick", "javascript:openNewWindow('CrewComment','','" + Session["sitepath"] + "/Crew/CrewPendingApprovalRemarks.aspx?dtkey=" + lblDTKey.Text + "&empid=" + empid.Text + "'); return false;");
            }

            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblPDStatus");         
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
            uct.Position = ToolTipPosition.TopCenter;
            uct.TargetControlId = lbtn.ClientID;            

            if (pdtype != null && pdtype.Text == "2") // disable attachement approval for Offshore
                att.Visible = false;

            UserControlCommonToolTip ttip = (UserControlCommonToolTip)e.Item.FindControl("ucCommonToolTip");
            RadLabel lblsignonoffid = (RadLabel)e.Item.FindControl("lblsignonoffid");
            if (ttip != null)
            {
                ttip.Screen = "Crew/CrewPromotiondemotioninfo.aspx?empid=" + empid.Text;
            }

        }

    }

    private bool IsValidateRemarks(string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrEmpty(remarks))
            ucError.ErrorMessage = "Mumbai Remarks is required.";
        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvSearch_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }

        gvSearch.Rebind();
    }
}
