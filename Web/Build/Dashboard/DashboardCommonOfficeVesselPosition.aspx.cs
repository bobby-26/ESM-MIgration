using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using System.Web.UI.HtmlControls;

public partial class DashboardCommonOfficeVesselPosition : PhoenixBasePage
{
    public string vesselposition;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            cmdHiddenPick.Attributes.Add("style", "visibility:hidden");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindVesselList();
            }

            BindData();
            GetVesselPositions();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindVesselList()
    {
        DataSet ds = PhoenixRegistersVessel.ListFleetWiseVessel(null, null, null, null, (ucFleet.SelectedFleet == "" || ucFleet.SelectedFleet == "Dummy") ? null : ucFleet.SelectedFleet);
        ddlVessel.DataSource = ds;
        ddlVessel.DataTextField = "FLDVESSELNAME";
        ddlVessel.DataValueField = "FLDVESSELID";
        ddlVessel.DataBind();
        ddlVessel.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    private void GetVesselPositions()
    {
        DataSet ds = PhoenixCommonDashboard.DashboardVesselSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ucFleet.SelectedFleet), General.GetNullableInteger(ddlVessel.SelectedValue));

        String Locations = "[";

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {

            DataRow dr = ds.Tables[0].Rows[i];
            //bypass empty rows 
            if (dr["FLDDECIMALLAT"].ToString().Trim().Length == 0)
                continue;
            if (dr["FLDDECIMALLONG"].ToString().Trim().Length == 0)
                continue;
            Locations += "['" + dr["FLDVESSELNAME"].ToString() + "','" + dr["FLDIMONUMBER"].ToString() + "'," + dr["FLDDECIMALLAT"].ToString() + "," + dr["FLDDECIMALLONG"].ToString() + ",'" + dr["FLDDETAILS"] + "','" + dr["FLDVESSELCODE"] + "'],";

        }
        vesselposition = Locations.TrimEnd(',') + "]";

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", "InitializeMap();", true);
    }

    private void BindData()
    {
        string[] alColumns = { "FLDVESSELNAME", "FLDVESSELCODE", "FLDLATITUDE", "FLDLONGITUDE", "FLDSPEED", "FLDETA", "FLDVESSELTYPE" };
        string[] alCaptions = { "Vessel Name", "Vessel Code", "Lat", "Long", "Speed", "ETA", "Type" };

        DataSet ds = PhoenixCommonDashboard.DashboardVesselSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ucFleet.SelectedFleet), General.GetNullableInteger(ddlVessel.SelectedValue));

        General.SetPrintOptions("gvVesselList", "Vessel List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVesselList.DataSource = ds;
            gvVesselList.DataBind();
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;

        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvVesselList);
            ViewState["ROWSINGRIDVIEW"] = 0;
        }
    }

    protected void MenuVesselList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("OLD"))
            {
                Response.Redirect("../Dashboard/DashboardVesselSynchronizationStatus.aspx", false);
            }

            if (dce.CommandName.ToUpper().Equals("ALERTS"))
            {
                Response.Redirect("../Dashboard/DashboardAlerts.aspx", false);
            }
            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                {
                    Response.Redirect("../Dashboard/DashboardCommonOfficeVesselPosition.aspx", true);
                }
                else
                {
                    Response.Redirect("../Dashboard/DashboardVesselParticulars.aspx", true);
                }
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
        string[] alColumns = { "FLDVESSELNAME", "FLDVESSELCODE", "FLDLATITUDE", "FLDLONGITUDE", "FLDSPEED", "FLDETA", "FLDVESSELTYPE" };
        string[] alCaptions = { "Vessel Name", "Vessel Code", "Lat", "Long", "Speed", "ETA", "Type" };

        DataSet ds = PhoenixCommonDashboard.DashboardVesselSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ucFleet.SelectedFleet), General.GetNullableInteger(ddlVessel.SelectedValue));

        Response.AddHeader("Content-Disposition", "attachment; filename=VesselList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h3><center>Vessel List</center></h3></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();

    }

    protected void gvVesselList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            _gridView.SelectedIndex = nCurrentRow;

            Label lblVesselName = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselName"));
            Label lblVesselId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId"));

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                string Script = "";
                Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
                Script += "showVessel();";
                Script += "</script>" + "\n";

                PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(lblVesselId.Text);
                PhoenixSecurityContext.CurrentSecurityContext.VesselName = lblVesselName.Text;
                Filter.CurrentOrderFormFilterCriteria = null;

                ucStatus.Text = "Current Vessel: " + PhoenixSecurityContext.CurrentSecurityContext.VesselName;

                DataSet ds = PhoenixGeneralSettings.GetUserOptions(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                PhoenixGeneralSettings.CurrentGeneralSetting = new GeneralSetting(ds);
                SessionUtil.ReBuildMenu();

                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselList_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblVesselId = ((Label)e.Row.FindControl("lblVesselId"));

            ImageButton cmdcrewlist = (ImageButton)e.Row.FindControl("cmdCrewList");
            if (cmdcrewlist != null)
            {
                cmdcrewlist.Visible = SessionUtil.CanAccess(this.ViewState, cmdcrewlist.CommandName);
                cmdcrewlist.Attributes.Add("onclick", "parent.Openpopup('history', '', '../Dashboard/DashboardVesselCrewList.aspx?vesselid=" + lblVesselId.Text + "&launchedfrom=office');return false;");
            }

            ImageButton cmdSummary = (ImageButton)e.Row.FindControl("cmdSummary");

            if (cmdSummary != null)
            {
                cmdSummary.Visible = SessionUtil.CanAccess(this.ViewState, cmdcrewlist.CommandName);
                cmdSummary.Attributes.Add("onclick", "parent.Openpopup('history', '', '../Dashboard/DashboardSummary.aspx?vesselid=" + lblVesselId.Text + "&launchedfrom=office');return false;");
            }



        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {

        }
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        
    }

    protected void ucFleet_Changed(object sender, EventArgs e)
    {
        BindVesselList();
        BindData();
        GetVesselPositions();
    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        BindData();
        GetVesselPositions();
    }

    protected void SetNavication(object sender, EventArgs e)
    {
        try
        {
            if (rblNavication.SelectedValue == "1")
                Response.Redirect("DashboardCommonOffice.aspx", false);
            else
                Response.Redirect("DashboardCommonOfficeVesselPosition.aspx", false);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
