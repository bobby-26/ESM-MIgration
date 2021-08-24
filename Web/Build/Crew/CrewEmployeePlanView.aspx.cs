using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Crew_CrewEmployeePlanView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Crew/CrewEmployeePlanView.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Crew/CrewEmployeePlanView.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
        MenuEmployeePlanner.AccessRights = this.ViewState;
        MenuEmployeePlanner.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            DateTime today = DateTime.Today;
            var thisWeekStart = today.AddDays(-(int)today.DayOfWeek);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            txtPlanFrom.Text = thisWeekStart.ToString();
            txtPlanTo.Text = thisWeekEnd.ToString();
            //cmdHiddenSubmit.Attributes.Add("style", "display:none");         
        }
        ViewState["EDIT"] = "0";
        BindData();
    }

    protected void MenuEmployeePlanner_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                BindData();
                gvEmpPlan.Rebind();               
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlRank.SelectedRank = "";
                ucVessel.SelectedVessel = "";
                txtFileNo.Text = "";
                BindData();
                ViewState["EDIT"] = "1";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
     
        DataSet ds = PhoenixCrewEmployeePlanner.EmployeePlanList(General.GetNullableString(txtFileNo.Text)
                                                                , General.GetNullableInteger(ddlRank.SelectedRank)
                                                                , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                , General.GetNullableDateTime(txtPlanFrom.Text)
                                                                , General.GetNullableDateTime(txtPlanTo.Text));

        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[1];
            if (ds.Tables[0].Rows.Count > 0)
            {
                //adding columns dynamically
                if (ViewState["EDIT"].ToString() != "1")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GridBoundColumn field = new GridBoundColumn();
                        field.HeaderText = General.GetNullableString(dt.Rows[i]["FLDDAY"].ToString());
                        field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                       // field.HeaderStyle.Width = Unit.Percentage(5.0);
                        field.Resizable = false;
                        gvEmpPlan.Columns.Insert(gvEmpPlan.Columns.Count, field);
                    }
                }
                gvEmpPlan.DataSource = ds;
                gvEmpPlan.DataBind();
                ViewState["EDIT"] = "1";
            }
            else
            {
                gvEmpPlan.DataSource = "";
            }
        }
    }

    protected void ShowExcel()
    {

        string[] alColumns = { "FLDOCCASSIONNAME", "FLDRANGEFROM", "FLDRANGETO", "FLDRANGENAME", "FLDSHORTCODE" };
        string[] alCaptions = { "Occasion", "Range From", "Range To", "Grade", "Code" };


        DataSet ds = PhoenixCrewEmployeePlanner.EmployeePlanList(General.GetNullableString(txtFileNo.Text)
                                                               , General.GetNullableInteger(ddlRank.SelectedRank)
                                                               , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                               , General.GetNullableDateTime(txtPlanFrom.Text)
                                                               , General.GetNullableDateTime(txtPlanTo.Text));

        Response.AddHeader("Content-Disposition", "attachment; filename=AppraisalRatingsConfig.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Appraisal Ratings Config</h3></td>");
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

    protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SHOW"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(txtPlanFrom.Text))
            ucError.ErrorMessage = "Plan from date is required.";

        if (string.IsNullOrEmpty(txtPlanTo.Text))
            ucError.ErrorMessage = "Plan to date is required.";
        
        return (!ucError.IsError);
    }
    protected void gvEmpPlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {            
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEmpPlan_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            DataSet ds = (DataSet)gvEmpPlan.DataSource;
            if (drv.Row.Table.Columns.Count > 0)
            {
                string tooltip = string.Empty;
                string referencedtkey = drv["FLDDTKEY"].ToString();

                DataTable header = ds.Tables[1];
                DataTable data = ds.Tables[2];

                for (int i = 0; i < header.Rows.Count; i++)
                {
                    RadLabel lblCourse = new RadLabel();
                    for (int j = 0; j < data.Rows.Count; j++)
                    {
                        DataRow[] dr = data.Select("FLDDATE = '" + header.Rows[i]["FLDDATE"].ToString() + "'" + " AND FLDREFERENCEDTKEY = '" + referencedtkey + "'");

                        if (dr.Length > 0)
                        {                           
                            lblCourse.Text = drv["FLDABBREVIATION"].ToString();
                        }
                    }
                    e.Item.Cells[i + 6].Controls.Add(lblCourse);
                }
            }
        }
    }
}