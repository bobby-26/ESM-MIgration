using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersCourse2Seagull : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCourse2Seagull.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSC')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCourse2Seagull.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCourse2Seagull.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");
            MenuRegistersSeagull.AccessRights = this.ViewState;
            MenuRegistersSeagull.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            //MenuTitle.AccessRights = this.ViewState;
            //MenuTitle.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvSC.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alColumns = { "FLDSEAGULLCOURSE", "FLDCOURSE" };
        string[] alCaptions = { "Seagull Course", "Course" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixRegistersCourse2Seagull.SearchSeagullCourse(txtSeagullCourse.Text,
            1,
            gvSC.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.ShowExcel("Seagull Course Mapping", dt, alColumns, alCaptions, null, null);
    }

    protected void MenuRegistersSeagull_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvSC.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("RESET"))
            {
                ViewState["PAGENUMBER"] = 1;
                ClearFilter();
                BindData();
                gvSC.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ClearFilter()
    {
        txtSeagullCourse.Text = "";
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSEAGULLCOURSE", "FLDCOURSE" };
        string[] alCaptions = { "Seagull Course", "Course" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixRegistersCourse2Seagull.SearchSeagullCourse(txtSeagullCourse.Text,
            (int)ViewState["PAGENUMBER"],
             gvSC.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvSC", "Seagull Course Mapping", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvSC.DataSource = ds;
            gvSC.VirtualItemCount = iRowCount;
        }
        else
        {
            gvSC.DataSource = "";
        }
    }
    private bool IsValidSeagullCourse(string course, string seagullcourse)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (seagullcourse.Trim().Equals(""))
            ucError.ErrorMessage = "Seagull Course is required.";

        if (!General.GetNullableInteger(course).HasValue)
            ucError.ErrorMessage = "Course is required.";

        return (!ucError.IsError);
    }


    protected DataSet ListCourse()
    {
        DataSet ds = PhoenixRegistersDocumentCourse.ListDocumentCourse(451);
        DataRow[] drs = PhoenixRegistersDocumentCourse.ListDocumentCourse(1579).Tables[0].Select("FLDDOCUMENTID > 0");
        foreach (DataRow dr in drs)
            ds.Tables[0].ImportRow(dr);
        return ds;
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvSC_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string courseid = ((UserControlCourse)e.Item.FindControl("ddlCourseAdd")).SelectedCourse;
                string seagullcourse = ((RadTextBox)e.Item.FindControl("txtSeagullCourseAdd")).Text;

                if (!IsValidSeagullCourse(courseid, seagullcourse))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixRegistersCourse2Seagull.InsertSeagullCourse(int.Parse(courseid), seagullcourse);
                    BindData();
                    gvSC.Rebind();
                }
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string dtkey = (e.Item as GridEditableItem).GetDataKeyValue("FLDDTKEY").ToString();
                string courseid = ((UserControlCourse)e.Item.FindControl("ddlCourseEdit")).SelectedCourse;
                string seagullcourse = ((RadTextBox)e.Item.FindControl("txtSeagullCourseEdit")).Text;

                if (!IsValidSeagullCourse(courseid, seagullcourse))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersCourse2Seagull.UpdateSeagullCourse(new Guid(dtkey), int.Parse(courseid), seagullcourse);
                BindData();
                gvSC.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string dtkey = (e.Item as GridDataItem).GetDataKeyValue("FLDDTKEY").ToString();
                PhoenixRegistersCourse2Seagull.DeleteSeagullCourse(new Guid(dtkey));
                BindData();
                gvSC.Rebind();
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

    protected void gvSC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSC.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvSC_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null)
        {
            del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
        }

        LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

        UserControlCourse course = (UserControlCourse)e.Item.FindControl("ddlCourseEdit");
        DataRowView drvHard = (DataRowView)e.Item.DataItem;
        if (course != null) course.SelectedCourse = drvHard["FLDPHOENIXCOURSEID"].ToString();
    }
}
    