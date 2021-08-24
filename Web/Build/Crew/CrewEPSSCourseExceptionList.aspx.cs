using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CrewEPSSCourseExceptionList : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Crew/CrewEPSSCourseExceptionList.aspx", "Import", "<i class=\"fas fa-list-alt\"></i>", "IMPORT");
            toolbar.AddFontAwesomeButton("../Crew/CrewEPSSCourseExceptionList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSCE')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewEPSSCourseExceptionList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");

            MenuCrewActivityLog.AccessRights = this.ViewState;
            MenuCrewActivityLog.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvSCE.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            // BindData();            
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
        string[] alColumns = { "FLDFILENO", "FLDCOURSE", "FLDEXAMDATE", "FLDSHIP", "FLDMESSAGE" };
        string[] alCaptions = { "File No", "Course", "Exam Date", "Done on", "Message" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewEPSS.SearchEPSSCourseException(null, txtfilenumber.Text
                                   , 1, iRowCount
                                   , ref iRowCount, ref iTotalPageCount);

        General.ShowExcel("EPSS Course Exception List", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void MenuCrewActivityLog_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("IMPORT"))
        {
            PhoenixCrewEPSS.InsertEPSSCourse();
        }
        
        else if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvSCE.CurrentPageIndex = 0;
            BindData();
            gvSCE.SelectedIndexes.Clear();
            gvSCE.EditIndexes.Clear();
            gvSCE.DataSource = null;
            gvSCE.Rebind();            
        }
    }
    public bool IsValidCourse(string issuedate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;

        if (!DateTime.TryParse(issuedate, out resultDate))
            ucError.ErrorMessage = "Issue Date is required.";
        else if (DateTime.TryParse(issuedate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be earlier than current date";
        }
        return (!ucError.IsError);
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDFILENO", "FLDCOURSE", "FLDEXAMDATE", "FLDSHIP", "FLDMESSAGE" };
        string[] alCaptions = { "File No", "Course", "Exam Date", "Done on", "Message" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCrewEPSS.SearchEPSSCourseException(General.GetNullableInteger(ddlExceptionList.SelectedValue)
                                   , txtfilenumber.Text
                                   , (int)ViewState["PAGENUMBER"], gvSCE.PageSize
                                   , ref iRowCount, ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        ds.AcceptChanges();
        General.SetPrintOptions("gvSCE", "EPSS Course Exception List", alCaptions, alColumns, ds);

        gvSCE.DataSource = dt;
        gvSCE.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvSCE_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null)
            {
                cme.Visible = SessionUtil.CanAccess(this.ViewState, cme.CommandName);
            }
        }
    }
    protected void gvSCE_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            string dtkey = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDDTKEY"].ToString();

            string fileno = ((RadTextBox)e.Item.FindControl("txtFileNo")).Text.Trim();
            string course = ((RadTextBox)e.Item.FindControl("txtCourse")).Text.Trim();
            string dateofissue = ((UserControlDate)e.Item.FindControl("txtExamDate")).Text;
            string doneon = ((RadTextBox)e.Item.FindControl("txtShip")).Text.Trim();
            string duration = ((RadTextBox)e.Item.FindControl("txtDuration")).Text.Trim();
            string attempts = ((UserControlMaskNumber)e.Item.FindControl("txtAttempts")).Text.Trim();

            if (!IsValidCourse(dateofissue))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixCrewEPSS.UpdateEPSSCourse(new Guid(dtkey), fileno, course, DateTime.Parse(dateofissue), doneon, duration, General.GetNullableInteger(attempts));
            BindData();
        }
        if (e.CommandName.ToUpper().Equals("PAGE"))
            ViewState["PAGENUMBER"] = null;
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
        }
    }
    protected void gvSCE_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSCE.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}