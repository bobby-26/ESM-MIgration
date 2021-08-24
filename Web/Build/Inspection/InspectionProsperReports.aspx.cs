using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Inspection_InspectionProsperReports : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionProsperReports.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            // toolbar.AddImageLink("javascript:CallPrint('gvCrewSearch')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionProsperReports.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionProsperReports.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            //toolbar.AddImageButton("../Inspection/InspectionProsperReports.aspx", "Refresh Score", "refresh.png", "REFRESH");
            ProsperMenu.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                NameValueCollection nvc = new NameValueCollection();
                nvc = filterprosper.CurrentProsperFilter;
                bindlevel();
                bindcategory();
                bindCardstatus();
                bindvesseltype();
                if (nvc != null)
                {

                    ddlrank.DataBind();
                    // ucLevelType.DataBind();
                    txtname.Text = nvc.Get("txtname");
                    ddlrank.SelectedRank = nvc.Get("ddrank").Replace("Dummy", "");
                    //ucLevelType.SelectedHard = nvc.Get("ucLevelType").Replace("Dummy", "");
                    ddlLevel.SelectedValue = nvc.Get("ucLevelType");
                    ddlcategory.SelectedValue = nvc.Get("ddlcategory");
                    ddlvesseltype.SelectedValue = nvc.Get("ddlvesseltype");
                    txtminscore.Text = nvc.Get("minscore");
                    txtmaxscore.Text = nvc.Get("maxscore");

                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                //bindlevel();
                BindData();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    public void bindvesseltype()
    {
        try
        {
            DataTable dt = PhoenixProsper.ProsperVesselTypeList();


            if (dt.Rows.Count > 0)
            {

                ddlvesseltype.DataTextField = "FLDTYPEDESCRIPTION";
                ddlvesseltype.DataValueField = "FLDTYPE";

                ddlvesseltype.DataSource = dt;
                ddlvesseltype.DataBind();
                ddlvesseltype.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                ddlvesseltype.SelectedIndex = 0;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    public void bindlevel()
    {
        try
        {
            DataTable dt = PhoenixProsper.ProsperEmployeeLevelList();

            if (dt.Rows.Count > 0)
            {

                ddlLevel.DataTextField = "FLDHARDNAME";
                ddlLevel.DataValueField = "FLDHARDCODE";

                ddlLevel.DataSource = dt;
                ddlLevel.DataBind();
                ddlLevel.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                ddlLevel.SelectedIndex = 0;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    public void bindcategory()
    {
        ddlcategory.Items.Clear();
        ddlcategory.DataSource = PhoenixRegisterProsperMeasureMapping.ProsperCategoryList();
        ddlcategory.DataTextField = "FLDCATEGORYNAME";
        ddlcategory.DataValueField = "FLDCATEGORYID";
        ddlcategory.DataBind();
        ddlcategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlcategory.SelectedIndex = 0;
    }
    public void bindCardstatus()
    {
        try
        {
            ddlcardstatus.Items.Clear();
            ddlcardstatus.DataSource = PhoenixRegisterProsperCardstatusMapping.ProsperCardstatusList();
            ddlcardstatus.DataTextField = "FLDCARDSTATUSNAME";
            ddlcardstatus.DataValueField = "FLDCARDSTATUSID";
            ddlcardstatus.DataBind();
            ddlcardstatus.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlcardstatus.SelectedIndex = 0;
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void gvProspercomplist_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nCurrentRow = e.Item.ItemIndex;

                RadLabel lblemployeeid = ((RadLabel)e.Item.FindControl("lblEmployeeid"));
                RadLabel lblfrom = ((RadLabel)e.Item.FindControl("lblfrom"));
                RadLabel lblto = ((RadLabel)e.Item.FindControl("lblto"));
                RadLabel lblcycleid = ((RadLabel)e.Item.FindControl("lblcycleid"));

                if (e.CommandName.ToUpper().Equals("SORT"))
                    return;

                if (e.CommandName.ToUpper().Equals("GETEMPLOYEE"))
                {

                    //RadLabel lblfamilyid = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblfamilyid"); 
                    LinkButton lb = (LinkButton)e.Item.FindControl("lnkEmployeeid");
                    lb.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + lblemployeeid.Text + "'); return false;");
                    //lb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreCrewList.aspx'); return false;");
                    //javascript: parent.openNewWindow('codehelp1', 'Crew List - " + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreCrewList.aspx');

                }

                if (e.CommandName.ToUpper().Equals("NAVIGATE"))
                {
                    BindPageURL(nCurrentRow);



                    filterprosper.CurrentSelectedProsperEmployee = lblemployeeid.Text;
                    // SetRowSelection();
                    Response.Redirect("../Inspection/InspectionProsperEmployeewiseList.aspx?empid=" + lblemployeeid.Text + "&cycleid=" + lblcycleid.Text + "&from=" + lblfrom.Text + "&to=" + lblto.Text + "&REVIEWDNC=" + filterprosper.CurrentSelectedProsperEmployee, false);

                }
                if (e.CommandName.ToUpper().Equals("REFRESH"))
                {
                    PhoenixProsper.ProsperResponsibleEmployeeUpdate(General.GetNullableInteger(lblemployeeid.Text.ToString()));
                    PhoenixProsper.ProsperEmployeeScoreRefresh(General.GetNullableInteger(lblcycleid.Text));
                    //***to update cummulative DURATION SEA TIME COMPANY EXPERIENCE***//
                    PhoenixProsper.ProsperCummulativeSeaTimeCompanyExp(General.GetNullableInteger(lblemployeeid.Text.ToString()), General.GetNullableInteger(lblcycleid.Text));
                    Response.Redirect("../Inspection/InspectionProsperEmployeewiseList.aspx?empid=" + lblemployeeid.Text + "&cycleid=" + lblcycleid.Text + "&from=" + lblfrom.Text + "&to=" + lblto.Text + "&REVIEWDNC=" + filterprosper.CurrentSelectedProsperEmployee, false);
                }
                if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    int cycleid = int.Parse(_gridView.Items[nCurrentRow].ToString());

                    PhoenixProsper.ProsperEmployeeScoreDelete(cycleid);
                    BindData();
                }
                else if (e.CommandName.ToUpper().Equals("UPDATE"))
                {

                    int cycleid = int.Parse(_gridView.Items[nCurrentRow].ToString());
                    string fromdate = ((UserControlDate)e.Item.FindControl("txtfromdateedit")).Text;
                    string todate = ((UserControlDate)e.Item.FindControl("txttodateedit")).Text;

                    PhoenixProsper.ProsperEmployeeCycleUpdate(
                         cycleid
                        , General.GetNullableDateTime(fromdate)
                        , General.GetNullableDateTime(todate));

                    BindData();
                }
                else if (e.CommandName == "Page")
                {
                    ViewState["PAGENUMBER"] = null;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblemployeeid = (RadLabel)gvProspercomplist.Items[rowindex].FindControl("lblEmployeeid");
            if (lblemployeeid != null)
            {
                filterprosper.CurrentSelectedDeficiency = lblemployeeid.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetRowSelection()
    {
        //gvProspercomplist.SelectedIndex = -1;
        //for (int i = 0; i < gvProspercomplist.Rows.Count; i++)
        //{
        //    if (filterprosper.CurrentSelectedProsperEmployee != null)
        //    {
        //        if (gvProspercomplist.DataKeys[i].Value.ToString().Equals(filterprosper.CurrentSelectedProsperEmployee.ToString()))
        //        {
        //            gvProspercomplist.SelectedIndex = i;

        //        }
        //    }
    }

    protected void Prosper_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                NameValueCollection criteria = new NameValueCollection();

                criteria.Clear();
                criteria.Add("ddrank", ddlrank.SelectedRank);
                // criteria.Add("ucLevelType", ucLevelType.SelectedHard);
                criteria.Add("ddlLevel", ddlLevel.SelectedValue);
                criteria.Add("txtname", txtname.Text);
                criteria.Add("ddlcategory", ddlcategory.SelectedValue);
                criteria.Add("ddlvesseltype", ddlvesseltype.SelectedValue);
                criteria.Add("minscore", txtminscore.Text);
                criteria.Add("maxscore", txtmaxscore.Text);

                filterprosper.CurrentProsperFilter = criteria;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvProspercomplist.Rebind();

            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {

                txtname.Text = "";
                ddlrank.SelectedRank = "";
                // ucLevelType.SelectedHard = "";
                ddlLevel.SelectedIndex = 0;
                ddlcategory.SelectedIndex = 0;
                ddlvesseltype.SelectedIndex = 0;
                ddlcardstatus.SelectedIndex = 0;
                txtminscore.Text = "";
                txtmaxscore.Text = "";
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ddrank", ddlrank.SelectedRank);
                //criteria.Add("ucLevelType", ucLevelType.SelectedHard);
                criteria.Add("ddlLevel", ddlLevel.SelectedValue);
                criteria.Add("txtname", txtname.Text);
                criteria.Add("ddlcategory", ddlcategory.SelectedValue);
                criteria.Add("ddlvesseltype", ddlvesseltype.SelectedValue);
                criteria.Add("minscore", txtminscore.Text);
                criteria.Add("maxscore", txtmaxscore.Text);

                filterprosper.CurrentProsperFilter = criteria;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvProspercomplist.Rebind();
            }
            if (CommandName.ToUpper().Equals("REFRESH"))
            {
                PhoenixProsper.ProsperEmployeeCycleRefresh();
                BindData();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
    }
    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        NameValueCollection nvc = new NameValueCollection();
        nvc = filterprosper.CurrentProsperFilter;

        try
        {
            string rank;
            string level;
            string name;

            if (nvc != null)
            {
                rank = nvc.Get("ddrank");
                level = nvc.Get("ucLevelType");
                name = nvc.Get("txtname");
            }
            else
            {
                rank = "";
                level = "";
                name = "";
            }
            DataTable dt = PhoenixProsper.ProsperSummarySearch(
                              General.GetNullableInteger(rank.Replace("Dummy", ""))
                            , General.GetNullableInteger(ddlLevel.SelectedValue) //General.GetNullableInteger(level.Replace("Dummy", ""))
                            , General.GetNullableInteger(ddlvesseltype.SelectedValue)
                            , General.GetNullableGuid(ddlcategory.SelectedValue)
                            , name
                            , General.GetNullableInteger(txtminscore.Text)
                            , General.GetNullableInteger(txtmaxscore.Text)
                            , General.GetNullableGuid(ddlcardstatus.SelectedValue)
                            , sortexpression
                            , sortdirection
                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                            //, gvProspercomplist.CurrentPageIndex + 1
                            , gvProspercomplist.PageSize
                            , ref iRowCount
                            , ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            gvProspercomplist.DataSource = dt;
            gvProspercomplist.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {

            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvProspercomplist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblComponentId = (RadLabel)e.Item.FindControl("lblComponentId");
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }
            RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkEmployeeid");
            if (lb != null)
            { 
                lb.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                //lb.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "'); return false;");
            }
            RadLabel lnkmeasureName = (RadLabel)e.Item.FindControl("lnkmeasureName");
            if (lnkmeasureName != null)
            {
                UserControlToolTip ucToolTiplastmodify = (UserControlToolTip)e.Item.FindControl("ucToolTiplastmodify");
                lnkmeasureName.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTiplastmodify.ToolTip + "', 'visible');");
                lnkmeasureName.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTiplastmodify.ToolTip + "', 'hidden');");
            }
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdXAtt");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.PROSPER + "&cmdname=PROSPER'); return false;");
            }
        }
    }
    protected void gvProspercomplist_Sorting(object sender, GridViewSortEventArgs se)
    {
        //gvProspercomplist.EditIndex = -1;
        //gvProspercomplist.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = new NameValueCollection();
        nvc = filterprosper.CurrentProsperFilter;


        try
        {
            string rank;
            string level;
            string name;

            if (nvc != null)
            {
                rank = nvc.Get("ddrank");
                level = nvc.Get("ucLevelType");
                name = nvc.Get("txtname");
            }
            else
            {
                rank = "";
                level = "";
                name = "";
            }

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDFILENO", "FLDFIRSTNAME", "FLDRANKCODE", "FLDCYCLESTARTDATE", "FLDCYCLEENDDATE", "FLDVESSELINCYCLE", "FLDSCORE", "FLDTOTALVESSEL", "FLDHISTORY", "FLDCARDSTATUS" };
            string[] alCaptions = { "File No", "Name", "Rank", "Start Date", "End Date", "No. of Vessel in the cycle", "Score", "Total Vessel", "History Average", "Card Status" };


            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            DataTable dt = PhoenixProsper.ProsperSummarySearch(
                                  General.GetNullableInteger(rank.Replace("Dummy", ""))
                                , General.GetNullableInteger(ddlLevel.SelectedValue) //General.GetNullableInteger(level.Replace("Dummy", ""))
                                , General.GetNullableInteger(ddlvesseltype.SelectedValue)
                                , General.GetNullableGuid(ddlcategory.SelectedValue)
                                , name
                                , General.GetNullableInteger(txtminscore.Text)
                                , General.GetNullableInteger(txtmaxscore.Text)
                                , General.GetNullableGuid(ddlcardstatus.SelectedValue)
                                , sortexpression
                                , sortdirection
                                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                , General.ShowRecords(null)
                                , ref iRowCount
                                , ref iTotalPageCount);

            ds.Tables.Add(dt.Copy());

            Response.AddHeader("Content-Disposition", "attachment; filename=ProsperScoreList.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Performance Report</h3></td>");
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
            foreach (DataRow dr in ds.Tables[0].Rows)
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvProspercomplist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? gvProspercomplist.CurrentPageIndex + 1 : ViewState["PAGENUMBER"];
        BindData();
    }
}
