using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Crew_CrewFlagCourseDone : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        
        toolbar.AddFontAwesomeButton("../Crew/CrewFlagCourseDone.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFlagCourseDone')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Crew/CrewFlagCourseDone.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("../Crew/CrewFlagCourseDone.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        
        MenuFlagCourseDone.AccessRights = this.ViewState;
        MenuFlagCourseDone.MenuList = toolbar.Show();
        //  MenuFlagCourseDone.SetTrigger(pnlBatch);
        
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            BindCourse();
            gvFlagCourseDone.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        //BindData();
    }
  
    protected void ddlCourse_DataBound(object sender, EventArgs e)
    {
        ddlCourse.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void MenuFlagCourseDone_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            if (!IsValidSearch(ddlCourse.SelectedValue,ddlFlag.SelectedFlag,txtFromDate.Text,txtToDate.Text))
            {
                ucError.Visible = true;
                return;
            }

            ViewState["PAGENUMBER"] = 1;
            BindData();
            
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
            
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ViewState["PAGENUMBER"] = 1;
            //  txtnopage.Text = "";
            ddlFlag.SelectedFlag = "";
            ddlCourse.SelectedIndex = 0;
            txtFromDate.Text = "";
            txtToDate.Text = "";
            
            BindData();
        }
    }
    private void BindCourse()
    {
        DataSet ds = PhoenixRegistersDocumentCourse.ListPostSeaCourse(null);
        ddlCourse.DataSource = ds;
        ddlCourse.DataBind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDDATEOFBIRTH", "FLDSEAMANBOOKNO", "FLDSTATUSNAME", "FLDCOURSE", "FLDINSTITUTENAME", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDSIGNONDATE", "FLDVESSELNAME", "FLDFLAGNAME" };
        string[] alCaptions = { "S.No", "File No", "Name", "Rank","D.O.B","CDC NO" ,"Status","Course", "Institute", "Place of Issue", "Date of Issue", "Sign On Date", "Vessel", "Flag" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewFlagCourseDone.CrewFlagCourseDoneSearch(General.GetNullableInteger(ddlCourse.SelectedValue)
                                                            , General.GetNullableInteger(ddlFlag.SelectedFlag)
                                                            , General.GetNullableDateTime(txtFromDate.Text)
                                                            , General.GetNullableDateTime(txtToDate.Text)
                                                            , sortexpression
                                                            , sortdirection
                                                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                            , gvFlagCourseDone.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount);
        General.SetPrintOptions("gvFlagCourseDone", "Flag Course Done", alCaptions, alColumns, ds);

        gvFlagCourseDone.DataSource = ds;
        gvFlagCourseDone.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    private bool IsValidSearch(string course,string flag,string fromdate, string todate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableInteger(course) == null)
            ucError.ErrorMessage = "Course is required";

        if (General.GetNullableInteger(flag)==null)
            ucError.ErrorMessage = "Flag is required";

        if (string.IsNullOrEmpty(fromdate))
            ucError.ErrorMessage = "From Date is required";

        if (string.IsNullOrEmpty(fromdate))
            ucError.ErrorMessage = "To Date is required";
        return (!ucError.IsError);
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDDATEOFBIRTH", "FLDSEAMANBOOKNO", "FLDSTATUSNAME", "FLDCOURSE", "FLDINSTITUTENAME", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDSIGNONDATE", "FLDVESSELNAME", "FLDFLAGNAME" };
        string[] alCaptions = { "S.No", "File No", "Name", "Rank", "D.O.B", "CDC NO", "Status", "Course", "Institute", "Place of Issue", "Date of Issue", "Sign On Date", "Vessel", "Flag" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        if (!IsPostBack)
        {

             ds = PhoenixCrewFlagCourseDone.CrewFlagCourseDoneSearch(General.GetNullableInteger(ddlCourse.SelectedValue)
                                                                , General.GetNullableInteger(ddlFlag.SelectedFlag)
                                                                , General.GetNullableDateTime(txtFromDate.Text)
                                                                , General.GetNullableDateTime(txtToDate.Text)
                                                                , sortexpression
                                                                , sortdirection
                                                                , 1
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);
        }
        else
        {
            if (ddlCourse.SelectedValue.ToString().Equals("") && ddlFlag.SelectedFlag.ToString().Equals(""))
            {
                return;
            }
            else
            {
                ds = PhoenixCrewFlagCourseDone.CrewFlagCourseDoneSearch(General.GetNullableInteger(ddlCourse.SelectedValue)
                                                                 , General.GetNullableInteger(ddlFlag.SelectedFlag)
                                                                 , General.GetNullableDateTime(txtFromDate.Text)
                                                                 , General.GetNullableDateTime(txtToDate.Text)
                                                                 , sortexpression
                                                                 , sortdirection
                                                                 , 1
                                                                 , iRowCount
                                                                 , ref iRowCount
                                                                 , ref iTotalPageCount);
            }
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewFlagCourseDone.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");        
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Flag Course Done</center></h5></td></tr>");        
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("<br />");        
        Response.Write("<tr><td colspan='14'><b>Course Done Date: </b>" + txtFromDate.Text +"-"+txtToDate.Text+ "</td></tr>");        
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='10%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;' align='left'>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void gvFlagCourseDone_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadLabel lblEmpId = (RadLabel)e.Item.FindControl("lblEmpId");
                LinkButton lnkName = (LinkButton)e.Item.FindControl("lblName");
                if (lblEmpId != null && lnkName != null)
                    lnkName.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + lblEmpId.Text + "'); return false;");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFlagCourseDone_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFlagCourseDone.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFlagCourseDone_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("PAGE"))
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