using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewOffshoreTrainingNeeds : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeedsVessel.aspx");

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["employeeid"] = "";
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["coursetype"] = "";
                ViewState["examrequestid"] = "";
                ViewState["SELECTEDMENT"] = 0;
                if (Request.QueryString["coursetype"] != null && Request.QueryString["coursetype"].ToString() != "")
                    ViewState["coursetype"] = Request.QueryString["coursetype"].ToString();
                else
                    ViewState["coursetype"] = PhoenixCommonRegisters.GetHardCode(1, 103, "4");

                gvOffshoreTraining.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar toolbarsub = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreTrainingNeeds.aspx?coursetype=" + ViewState["coursetype"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOffshoreTraining')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreTrainingNeedsSearch.aspx?PendingNeedsYN=1&Vessel=0&Override=0", "Filter", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreTrainingNeeds.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreTrainingNeedsAdd.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDTRAINING");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreTrainingNeeds.aspx", "Refresh", "<i class=\"fas fa-sync\"></i>", "REFRESH");
            MenuOffshoreTraining.AccessRights = this.ViewState;
            MenuOffshoreTraining.MenuList = toolbar.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshoreTraining_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void CourseRequest_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string coursetype = "";

        if (CommandName.ToUpper().Equals("CBT"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "4");
            Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeeds.aspx?coursetype=" + coursetype, true);
        }
        else if (CommandName.ToUpper().Equals("HSEQA"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
            Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeeds.aspx?coursetype=" + coursetype, true);
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = new string[0];
        string[] alCaptions = new string[0];

        if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
        {
            alColumns = new string[] { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO", "FLDPENDINGCBTCOUNT", "FLDCOMPLETEDCBTCOUNT", "FLDOVERRIDECBTCOUNT", "FLDPENDINGTRAINIGCOUNT", "FLDCOMPLETEDTRAININGCOUNT", "FLDOVERRIDETRAININGCOUNT" };
            alCaptions = new string[] { "Name", "Rank", "File No", "Pending CBT", "Completed CBT", "Overridden CBT", "Pending Training", "Completed Training", "Overridden Training" };
        }
        else if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "7"))
        {
            alColumns = new string[] { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO", "FLDPENDINGCBTCOUNT", "FLDCOMPLETEDCBTCOUNT", "FLDOVERRIDECBTCOUNT", "FLDPENDINGTRAINIGCOUNT", "FLDCOMPLETEDTRAININGCOUNT", "FLDOVERRIDETRAININGCOUNT" };
            alCaptions = new string[] { "Name", "Rank", "File No", "Pending CBT", "Completed CBT", "Overridden CBT", "Pending Training", "Completed Training", "Overridden Training" };
        }
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentOffshoreTrainingNeedSearch;
        int? vesselid = null;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        else if (nvc != null)
        {
            if (General.GetNullableInteger(nvc.Get("ucVessel")) != null)
                vesselid = General.GetNullableInteger(nvc.Get("ucVessel"));
        }

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.Searchtrainingneedsummary
                    (vesselid
                      , nvc != null ? nvc["txtName"] : ""
                      , nvc != null ? nvc["txtFileNo"] : ""
                      , General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : "")
                      , sortexpression
                      , sortdirection
                      , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                      , iRowCount
                      , ref iRowCount
                      , ref iTotalPageCount);



        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreTrainingNeeds.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Pending Training Needs</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void TrainingNeed_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("COMPLETEDTRAININGNEED"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreCompletedTrainingNeed.aspx", true);
        }
        else if (CommandName.ToUpper().Equals("OVERRIDDENTRAININGNEED"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreOverrideTrainingNeed.aspx", true);
        }
    }

    protected void MenuOffshoreTraining_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentOffshoreTrainingNeedSearch = null;
                BindData();
                gvOffshoreTraining.Rebind();
            }
            else if (CommandName.ToUpper().Equals("REFRESH"))
            {
                PhoenixCrewOffshoreTrainingNeeds.RefreshTrainingNeedsSummary();
                // Filter.CurrentOffshoreTrainingNeedSearch = null;
                BindData();
                gvOffshoreTraining.Rebind();
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
        BindData();
        gvOffshoreTraining.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = new string[0];
        string[] alCaptions = new string[0];
        if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
        {
            alColumns = new string[] { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO", "FLDPENDINGCBTCOUNT", "FLDCOMPLETEDCBTCOUNT", "FLDOVERRIDECBTCOUNT", "FLDPENDINGTRAINIGCOUNT", "FLDCOMPLETEDTRAININGCOUNT", "FLDOVERRIDETRAININGCOUNT" };
            alCaptions = new string[] { "Name", "Rank", "File No", "Pending CBT", "Completed CBT", "Overridden CBT", "Pending Training", "Completed Training", "Overridden Training" };
        }
        else if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "7"))
        {
            alColumns = new string[] { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO", "FLDPENDINGCBTCOUNT", "FLDCOMPLETEDCBTCOUNT", "FLDOVERRIDECBTCOUNT", "FLDPENDINGTRAINIGCOUNT", "FLDCOMPLETEDTRAININGCOUNT", "FLDOVERRIDETRAININGCOUNT" };
            alCaptions = new string[] { "Name", "Rank", "File No", "Pending CBT", "Completed CBT", "Overridden CBT", "Pending Training", "Completed Training", "Overridden Training" };
        }

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentOffshoreTrainingNeedSearch;
        int? vesselid = null;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        else if (nvc != null)
        {
            if (General.GetNullableInteger(nvc.Get("ucVessel")) != null)
                vesselid = General.GetNullableInteger(nvc.Get("ucVessel"));
        }

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.Searchtrainingneedsummary(
                       vesselid
                       , nvc != null ? nvc["txtName"] : ""
                       , nvc != null ? nvc["txtFileNo"] : ""
                       , General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : "")
                       , sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        gvOffshoreTraining.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount);


        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreTraining", "Pending Training Needs", alCaptions, alColumns, ds);
        gvOffshoreTraining.DataSource = ds;
        gvOffshoreTraining.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }


    protected void cmdSearch_Click(object sender, EventArgs e)
    {

        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvOffshoreTraining.Rebind();
    }


    //protected void OnDataBound(object sender, EventArgs e)
    //{
    //    GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
    //    TableHeaderCell cell = new TableHeaderCell();
    //    cell.HorizontalAlign = HorizontalAlign.Center;
    //    cell.Text = "";
    //    cell.ColumnSpan = 3;
    //    row.Controls.Add(cell);

    //    cell = new TableHeaderCell();
    //    cell.HorizontalAlign = HorizontalAlign.Center;
    //    cell.ColumnSpan = 3;
    //    cell.Text = "CBT";
    //    row.Controls.Add(cell);

    //    cell = new TableHeaderCell();
    //    cell.HorizontalAlign = HorizontalAlign.Center;
    //    cell.ColumnSpan = 3;
    //    cell.Text = "Training Course";
    //    row.Controls.Add(cell);



    //    cell = new TableHeaderCell();
    //    cell.HorizontalAlign = HorizontalAlign.Center;
    //    cell.ColumnSpan = 1;
    //    cell.Text = "Action";
    //    row.Controls.Add(cell);


    //    gvOffshoreTraining.HeaderRow.Parent.Controls.AddAt(0, row);

    //}
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvOffshoreTraining_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOffshoreTraining.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOffshoreTraining_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("GETEMPLOYEE"))
            {

                string lblEmployeeid = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;

                if (int.Parse(ViewState["SELECTEDMENT"].ToString()) == 0)
                {
                    Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeedsCBTDetail.aspx?employeeid=" + lblEmployeeid + "&coursetype=" + ViewState["coursetype"].ToString(), true);
                }
                else
                {
                    Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeedsDetail.aspx?employeeid=" + lblEmployeeid + "&coursetype=" + ViewState["coursetype"].ToString(), true);
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

    protected void gvOffshoreTraining_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            e.Item.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#bbddff'");
            // when mouse leaves the row, change the bg color to its original value   
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");



            LinkButton lb = (LinkButton)e.Item.FindControl("lnkEployeeName");
            if (lb != null)
            {
                RadLabel lblemployeeid = ((RadLabel)e.Item.FindControl("lblEmployeeid"));

                lb.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + lblemployeeid.Text + "'); return false;");
            }
            if(PhoenixSecurityContext.CurrentSecurityContext.VesselID ==0)
                gvOffshoreTraining.MasterTableView.GetColumn("vessel").Visible = true;
            else
                gvOffshoreTraining.MasterTableView.GetColumn("vessel").Visible = false;


        }
    }
}
