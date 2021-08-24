using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Text;
using SouthNests.Phoenix.PreSea;

public partial class Presea_PreSeaBuddyPlanner : PhoenixBasePage
{
    DataSet dsdates = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Status", "STATUS");
        MenuBuddyPlanner.MenuList = toolbar.Show();
       
        if (!IsPostBack)
        {
            BindFaculty();
        }
        BindPlanner();
        //ucCalender.SelectedDate = DateTime.Today;
    }

    protected void BindPlanner()
    {
        ucCalender.SelectedDates.Clear();

        dsdates = PhoenixPreSeaBuddyPlanner.PreSeaBuddyPlannerSearch(General.GetNullableInteger(ddlFaculty.SelectedValue));
    }


    protected void MenuBuddyPlanner_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("PLANNER"))
        {
            BindPlanner();
        }

        if (dce.CommandName.ToUpper().Equals("STATUS"))
        {
            Response.Redirect("../Presea/PreSeaBuddyPlannerStatus.aspx", true);
        }
    }

    protected void BindFaculty()
    {
        ddlFaculty.DataSource = PhoenixPreSeaBuddyPlanner.PreSeaFacultySearch();
        ddlFaculty.DataTextField = "FLDUSERNAME";
        ddlFaculty.DataValueField = "FLDUSERCODE";
        ddlFaculty.DataBind();
        ddlFaculty.Items.Insert(0, new ListItem("--Select--", "DUMMY"));
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    protected void ucCalender_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFaculty.SelectedValue == "" || ddlFaculty.SelectedValue.ToUpper().Equals("DUMMY"))
        {
            ucError.ErrorMessage = "Faculty is Required";
            ucError.Visible = true;
            return;
        }
        else
        {
            DateTime selecteddate = ucCalender.SelectedDate;

            PhoenixPreSeaBuddyPlanner.PreSeaBuddyPlannerInsert(General.GetNullableInteger(ddlFaculty.SelectedValue), selecteddate);

            BindPlanner();
        }
    }

    protected void ucCalender_OnDayRender(object sender, DayRenderEventArgs e)
    {
        DateTime nextDate;
        if (dsdates != null)
        {
           
            foreach (DataRow dr in dsdates.Tables[0].Rows)
            {
                nextDate = (DateTime)dr["FLDPLANNEDDDATE"];
                if (nextDate == e.Day.Date)
                {
                    e.Cell.CssClass = "datagrid_selectedstyle";
                }
            }
        }
        e.Day.IsSelectable = !e.Day.IsOtherMonth;
            
    }

    protected void ucCalender_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        if (ddlFaculty.SelectedValue == "" || ddlFaculty.SelectedValue.ToUpper().Equals("DUMMY"))
        {
            ucError.ErrorMessage = "Faculty is Required";
            ucError.Visible = true;

            Calendar cl = (Calendar)sender;
            cl.VisibleDate = DateTime.Today.Date;

            return;
        }
    }
}
