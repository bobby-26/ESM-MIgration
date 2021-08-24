using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Text;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CrewOffshorePlannerGrid : PhoenixBasePage
{
    string vsltypelist = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshorePlannerGrid.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshorePlannerGrid.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshorePlannerGrid.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuExcel.AccessRights = this.ViewState;
            MenuExcel.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                BindVesselPlannerType();
                txtDate.Text = DateTime.Now.AddMonths(1).ToShortDateString();

                //ucRank.Bind(); 
 
                //StringBuilder strlist = new StringBuilder();

                //if (nvc != null)
                //{
                //    strlist.Append(General.GetNullableString(nvc.Get("ucRank")));
                //    SplitRank(strlist, ',');
                //}
            }

            BindData();
            //NameValueCollection nvc = Filter.CurrentPlannerGridFilterSelection;
            //if (nvc != null)
            //    ucRank.selectedlist = General.GetNullableString(nvc.Get("ucRank"));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                //txtDate.Text = DateTime.Parse(DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/01").ToString();
                txtDate.Text = DateTime.Now.AddMonths(1).ToShortDateString();
                ucRank.selectedlist = "";
                lstVesselType.SelectedValue = "DUMMY";

                BindSearch();
                BindData();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                BindSearch();
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindSearch()
    {
        vsltypelist = GetVesselTypeList();

        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();

        if (General.GetNullableString(ucRank.selectedlist) != null)
            criteria.Add("ucRank", ucRank.selectedlist);

        criteria.Add("date", txtDate.Text);

        if (General.GetNullableString(vsltypelist) != null)
            criteria.Add("ucVesselType", vsltypelist);

        Filter.CurrentPlannerGridFilterSelection = criteria;
    }

    protected void BindData()
    {
        vsltypelist = GetVesselTypeList();

        NameValueCollection nvc = Filter.CurrentPlannerGridFilterSelection;

        DataSet ds = PhoenixCrewOffshorePlanner.CrewOffshorePlannerGrid
            (
            nvc != null ? General.GetNullableDateTime(nvc.Get("date")) : General.GetNullableDateTime(txtDate.Text)
            , nvc != null ? General.GetNullableString(nvc.Get("ucRank")) : General.GetNullableString(ucRank.selectedlist)
            , nvc != null ? General.GetNullableString(nvc.Get("ucVesselType")) : General.GetNullableString(vsltypelist)
            );

        if (ds.Tables.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt1 = ds.Tables[0];
                sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;overflow:auto;width:100%;border-collapse:collapse;\"> ");

                DataTable dtTempHeader = ds.Tables[0];
                DataRow[] drvHeader = dtTempHeader.Select("FLDROWID = " + dt1.Rows[0]["FLDROWID"].ToString());
                sb.Append("<tr ><td align=\"center\" style=\"width:250px\"><b>Rank</b></td>");

                foreach (DataRow drTempHeader in drvHeader)
                {
                    sb.Append("<td style=\"width:250px\" align=\"center\"><b>");
                    sb.Append(drTempHeader["FLDCOLUMNHEADER"].ToString());
                    sb.Append("</b></td>");
                }
                sb.Append("</tr>");

                //PRINTING
                DataTable dt2 = ds.Tables[1];
                foreach (DataRow dr1 in dt2.Rows)
                {
                    DataTable dtTemp = ds.Tables[0];

                    DataRow[] drv = dtTemp.Select("FLDROWID = " + dr1["FLDROWID"].ToString());

                    sb.Append("<tr><td align='left' style=\"width:250px\"><b>" + drv[0]["FLDROWHEADER"].ToString() + "</b></td>");
                    foreach (DataRow drTemp in drv)
                    {
                        sb.Append("<td align=\"center\" style=\"width:250px\">");


                        //string link = "CrewOffshorePlannerGridDetails.aspx?rankid=" + drTemp["FLDROWID"].ToString() + "&vesseltypeid=" + drTemp["FLDCOLUMNID"].ToString() + "&date=" + drTemp["FLDDATE"].ToString() + "');return false;";
                        //sb.Append("<a ID='lnkGrid'  Visible='true' runat='server' Text='" + (drTemp["FLDVALUE"].ToString()) + "'" + "  href='" + link + "'</a>");
                        //sb.Append("<a ID='lnkGrid'  Visible='true' runat='server' Text='" + (drTemp["FLDVALUE"].ToString()) + "' onclick = \"javascript:Open('CrewPage','','CrewOffshorePlannerGridDetails.aspx?rankid=" + drTemp["FLDROWID"].ToString() + "&vesseltypeid=" + drTemp["FLDCOLUMNID"].ToString() + "&date=" + drTemp["FLDDATE"].ToString() + "');return false;\" href='CrewOffshorePlannerGrid.aspx'>" + drTemp["FLDVALUE"].ToString() + "</a>");

                        sb.Append("<a ID='lnkGrid'  Visible='true' runat='server'  href= CrewOffshorePlannerGridDetails.aspx?rankid=" + drTemp["FLDROWID"].ToString() + "&vesseltypeid=" + drTemp["FLDCOLUMNID"].ToString() + "&date=" + drTemp["FLDDATE"].ToString() + "');return false;>" + drTemp["FLDVALUE"].ToString() + "</a>");
                        sb.Append("</td>");
                    }

                    sb.Append("</tr>");
                }

                sb.Append("</table>");

                ltGrid.Text = sb.ToString();
            }
            else
            {
                sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");

                sb.Append("<tr style=\"height:10px;\" class=\"DataGrid-HeaderStyle\"><td style=\"height:15px;\"></td></tr>");
                sb.Append("<tr style=\"height:10px;\"><td align=\"center\" colspan=\"6\" style=\"color:Red;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

                sb.Append("</table>");

                ltGrid.Text = sb.ToString();
            }
        }
    }
    //protected void Url(object sender, EventArgs e)
    //{
    //    Response.Redirect("../Crew/CrewOffshorePlannerGridDetails.aspx?rankid=" + drTemp["FLDROWID"].ToString() + "&vesseltypeid=" + drTemp["FLDCOLUMNID"].ToString() + "&date=" + drTemp["FLDDATE"].ToString() + "');return false;");
    //}
    protected void ShowExcel()
    {
        vsltypelist = GetVesselTypeList();
        DataSet ds = PhoenixCrewOffshorePlanner.CrewOffshorePlannerGrid(General.GetNullableDateTime(txtDate.Text),
            General.GetNullableString(ucRank.selectedlist), General.GetNullableString(vsltypelist));

        Response.AddHeader("Content-Disposition", "attachment; filename=PlannerGrid.xls");
        Response.ContentType = "application/vnd.msexcel";
        int counter = 1;

        if (ds.Tables.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt1 = ds.Tables[0];
                sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;overflow:auto;width:100%;border-collapse:collapse;\"> ");

                DataTable dtTempHeader = ds.Tables[0];
                DataRow[] drvHeader = dtTempHeader.Select("FLDROWID = " + dt1.Rows[0]["FLDROWID"].ToString());
                sb.Append("<tr class=\"DataGrid-HeaderStyle\"><td align=\"center\" style=\"width:250px\"><b>Rank</b></td>");

                foreach (DataRow drTempHeader in drvHeader)
                {
                    sb.Append("<td style=\"width:250px\" align=\"center\"><b>");
                    sb.Append(drTempHeader["FLDCOLUMNHEADER"].ToString());
                    sb.Append("</b></td>");
                    counter = counter + 1;
                }
                sb.Append("</tr>");

                //PRINTING
                DataTable dt2 = ds.Tables[1];
                foreach (DataRow dr1 in dt2.Rows)
                {
                    DataTable dtTemp = ds.Tables[0];

                    DataRow[] drv = dtTemp.Select("FLDROWID = " + dr1["FLDROWID"].ToString());

                    sb.Append("<tr><td align='left' style=\"width:250px\"><b>" + drv[0]["FLDROWHEADER"].ToString() + "</b></td>");
                    foreach (DataRow drTemp in drv)
                    {
                        sb.Append("<td align=\"center\" style=\"width:250px\">");
                        sb.Append(drTemp["FLDVALUE"].ToString());
                        sb.Append("</td>");
                    }

                    sb.Append("</tr>");

                }

                sb.Append("</table>");

                ltGrid.Text = sb.ToString();
            }
            else
            {


                sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");

                sb.Append("<tr style=\"height:10px;\" class=\"DataGrid-HeaderStyle\"><td style=\"height:15px;\"></td></tr>");
                sb.Append("<tr style=\"height:10px;\"><td align=\"center\" colspan=\"6\" style=\"color:Red;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

                sb.Append("</table>");

                ltGrid.Text = sb.ToString();
            }

            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            //Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + counter.ToString() + "'><h5><center> SHIP MANAGEMENT</center></h5></td></tr>");
            Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + counter.ToString() + "'><h5><center>Planner Grid</center></h5></td></tr>");
            Response.Write("<tr><td style='font-family:Arial; font-size:10px;'>&nbsp;</td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write(sb.ToString());
            Response.End();
        }
    }

    protected void BindVesselPlannerType()
    {
        DataTable dt = PhoenixCrewOffshorePlanner.CrewOffshorePlannerVesselType(null);
       
        lstVesselType.Items.Clear();
        lstVesselType.Items.Add(new RadListBoxItem("--Select--", "DUMMY"));
        lstVesselType.AppendDataBoundItems = true;
        lstVesselType.DataSource = dt;
        lstVesselType.DataBind();
    }

    private string GetVesselTypeList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstVesselType.Items)
        {
            if (item.Selected == true)
            {

                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }

        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        return strlist.ToString();
    }
    //private void SplitRank(StringBuilder input, char separator)
    //{
    //    List<StringBuilder> results = new List<StringBuilder>();
    //    StringBuilder current = new StringBuilder();
    //    for (int i = 0; i < input.Length; ++i)
    //    {
    //        if (input[i] == separator)
    //        {
    //            results.Add(current);
    //            current = new StringBuilder();
    //        }
    //        else
    //            current.Append(input[i]);
    //    }

    //    if (current.Length > 0)
    //        results.Add(current);
    //    foreach(StringBuilder items in results)
    //    {
    //        ucRank.SelectedRankValue = items.ToString();
    //    }
    //    //ucRank.SelectedRankValue = current;
    //    //return results.ToArray();
    //}

}
