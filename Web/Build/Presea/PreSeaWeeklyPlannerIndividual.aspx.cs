using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System.Web;

public partial class PreSeaWeeklyPlannerIndividual : PhoenixBasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../PreSea/PreSeaWeeklyPlannerIndividual.aspx", "Export to Excel", "icon_xls.png", "Excel");
                MenuPreSea.AccessRights = this.ViewState;
                MenuPreSea.MenuList = toolbar.Show();

                ViewState["STAFFID"] = "";
                ViewState["DATE"] = "";
                ViewState["TYPE"] = "";

                if (Request.QueryString["staffid"] != null)
                    ViewState["STAFFID"] = Request.QueryString["staffid"].ToString();
                if (Request.QueryString["date"] != null)
                    ViewState["DATE"] = Request.QueryString["date"].ToString();
                if (Request.QueryString["type"] != null)
                    ViewState["TYPE"] = Request.QueryString["type"].ToString();

                BindWeeklyPlan();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindWeeklyPlan()
    {

        try
        {
            string[] alColumns = { "FLDSTAFFNAME", "FLDDATE", "FLDTIMESLOT", "FLDPRESEACOURSENAME", "FLDBATCH", "FLDSEMESTERID", "FLDSECTIONNAME", "FLDSUBJECTNAME", "FLDROOMNAME" };
            string[] alCaptions = { "Staff Name", "Date","Time","Course","Batch","Semester","Section","Subject","Class Room" };

            DataSet ds = new DataSet();
            ds = PhoenixPreSeaWeeklyPlanner.ListFacultyWeeklyTimeTableDetails(
                1, General.GetNullableInteger(ViewState["STAFFID"].ToString())
                , General.GetNullableDateTime(ViewState["DATE"].ToString())
                , General.GetNullableInteger(ViewState["TYPE"].ToString())
                );

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtName.Text = ds.Tables[0].Rows[0]["FLDSTAFFNAME"].ToString();
                gvWeeklyPlanner.DataSource = ds;
                gvWeeklyPlanner.DataBind();
            }
            else
            {
                ShowNoRecordsFound(ds.Tables[0], gvWeeklyPlanner);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSTAFFNAME", "FLDDATE", "FLDTIMESLOT", "FLDPRESEACOURSENAME", "FLDBATCH", "FLDSEMESTERID", "FLDSECTIONNAME", "FLDPRACTICALNAME", "FLDSUBJECTNAME", "FLDROOMNAME" };
        string[] alCaptions = { "Staff Name", "Date", "Time", "Course", "Batch", "Semester", "Section","Practical", "Subject", "Class Room" };

        ds = PhoenixPreSeaWeeklyPlanner.ListFacultyWeeklyTimeTableDetails(
                1, General.GetNullableInteger(ViewState["STAFFID"].ToString())
                , General.GetNullableDateTime(ViewState["DATE"].ToString())
                , General.GetNullableInteger(ViewState["TYPE"].ToString())
                );

        Response.AddHeader("Content-Disposition", "attachment; filename=WeeklyPlanIndividual.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Weekly Plan Individual</h3></td>");
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
}
