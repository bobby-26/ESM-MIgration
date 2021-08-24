using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Drawing;
using Telerik.Web.UI;

public partial class CrewOffshorePD : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvSearch.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation
    //                     (r.UniqueID + "$ctl00");
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl01");
    //        }
    //    }
    //    base.Render(writer);
    //}
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshorePD.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("javascript:CallPrint('gvSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarsub.AddFontAwesomeButton("javascript:openNewWindow('Filter','','"+Session["sitepath"]+"/CrewOffshore/CrewOffshorePDFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshorePD.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbarsub.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                gvSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PD_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CrewOffshorePDFilterSelection = null;
                BindData();
                gvSearch.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
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
        string[] alColumns = { "FLDRANKCODE", "FLDVESSELNAME", "FLDEMPLOYEENAME", "FLDCURRENCYCODE", "FLDDAILYRATEUSD", "FLDDPALLOWANCE", "FLDEXPECTEDJOINDATE", "FLDOFFSIGNERNAME", "FLDRELIEFDUEDATE", "FLDPROPOSEDBY", "FLDPROPOSALREMARKS", "FLDPDSTATUS" };
        string[] alCaptions = { "Rank", "Vessel", "Name", "Currency", "Daily Rate", "Daily DP Allowance", "Planned Relief", "Off-Signer", "End of Contract", "Proposed By", "Proposal Remarks", "Status" };

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CrewOffshorePDFilterSelection;
        DataTable dt = PhoenixCrewOffshoreApproveProposal.OffshorePDList(General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ucPrincipal"] : string.Empty)
                                                        , General.GetNullableInteger(nvc != null ? nvc["ddlUser"] : string.Empty)
                                                        , sortexpression, sortdirection
                                                        , (int)ViewState["PAGENUMBER"]
                                                        , General.ShowRecords(null)
                                                        , ref iRowCount
                                                        , ref iTotalPageCount
                                                       );

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        Response.AddHeader("Content-Disposition", "attachment; filename=Pending_Approvals.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Pending Approvals</center></h5></td></tr>");
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
       gvSearch.Rebind();
    }

    public void BindData()
    {

        try
        {
            string[] alColumns = { "FLDRANKCODE", "FLDVESSELNAME", "FLDEMPLOYEENAME", "FLDCURRENCYCODE", "FLDDAILYRATEUSD", "FLDDPALLOWANCE", "FLDEXPECTEDJOINDATE", "FLDOFFSIGNERNAME", "FLDRELIEFDUEDATE", "FLDPROPOSEDBY", "FLDPROPOSALREMARKS", "FLDPDSTATUS" };
            string[] alCaptions = { "Rank", "Vessel", "Name", "Currency", "Daily Rate", "Daily DP Allowance", "Planned Relief", "Off-Signer", "End of Contract", "Proposed By", "Proposal Remarks", "Status" };

            int iRowCount = 0;
            int iTotalPageCount = 0;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CrewOffshorePDFilterSelection;
            DataTable dt = PhoenixCrewOffshoreApproveProposal.OffshorePDList(General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ucPrincipal"] : string.Empty)
                                                        , General.GetNullableInteger(nvc != null ? nvc["ddlUser"] : string.Empty)
                                                        , sortexpression, sortdirection
                                                        , (int)ViewState["PAGENUMBER"]
                                                        , gvSearch.PageSize
                                                        , ref iRowCount
                                                        , ref iTotalPageCount
                                                        );

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvSearch", "Pending Approvals", alCaptions, alColumns, ds);
            gvSearch.DataSource = dt;
            gvSearch.VirtualItemCount = iRowCount;

          
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidateRemarks(string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrEmpty(remarks))
            ucError.ErrorMessage = "Mumbai Remarks is required.";
        return (!ucError.IsError);
    }
  

    public StateBag ReturnViewState()
    {
        return ViewState;
    }


    protected void gvSearch_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSearch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSearch_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvSearch_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
                && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                RadLabel lblCrewPlanId = (RadLabel)e.Item.FindControl("lblCrewPlanId");
                RadLabel empid = (RadLabel)e.Item.FindControl("lblEmployeeId");
                RadLabel vslid = (RadLabel)e.Item.FindControl("lblVessel");
                RadLabel rnk = (RadLabel)e.Item.FindControl("lblRank");
                RadLabel dailyrate = (RadLabel)e.Item.FindControl("lblDailyRate");
                RadLabel budgetedwage = (RadLabel)e.Item.FindControl("lblbudgetedwage");

                LinkButton imgApprove = (LinkButton)e.Item.FindControl("imgApprove");
                LinkButton imgReject = (LinkButton)e.Item.FindControl("imgReject");

                UserControlToolTip ucToolTipNW = (UserControlToolTip)e.Item.FindControl("ucToolTipNW");
                RadLabel lblDailyRate = (RadLabel)e.Item.FindControl("lblDailyRate");


                DataTable dt1 = PhoenixCrewOffshoreApproveProposal.OffshorePDInfo(General.GetNullableGuid(lblCrewPlanId.Text));

                if (dt1.Rows.Count > 0)
                {
                    string html = "<table>";

                    html += "<tr>";
                    html += "<td>" + "Budget Wages: " + dt1.Rows[0][0].ToString() + "</td>";
                    html += "</tr>";
                    html += "<tr>";
                    html += "<td>" + "1st Year Scale: " + dt1.Rows[0][1].ToString() + "</td>";
                    html += "</tr>";

                    html += "</table>";

                    //lblDailyRatetooltip.Text = html;
                    ucToolTipNW.Text = html;
                    ucToolTipNW.Position = ToolTipPosition.TopCenter;
                    ucToolTipNW.TargetControlId = lblDailyRate.ClientID;
                    //ucToolTipNW.Text = html;//dt1.Rows[0][0].ToString();//General.ShowGrid(dt1, alColumns, alCaptions);
                }
                else
                {
                    ucToolTipNW.Text = "";
                    ucToolTipNW.Position = ToolTipPosition.TopCenter;
                    ucToolTipNW.TargetControlId = lblDailyRate.ClientID;
                }
                  // ucToolTipNW.Text = "no records";//General.ShowGrid(dt1, alColumns, alCaptions);
                                                    //imgnotes.ToolTip = dt1.ToString();// General.ShowGrid(dt1, alColumns, alCaptions);
               // dailyrate.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'visible');");
                //dailyrate.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'hidden');");

                if (imgApprove != null)
                {
                    if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
                        imgApprove.Attributes.Add("onclick", "parent.openNewWindow('codehelpsuitability', '', '"+Session["sitepath"]+"/CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&personalmaster=true&popup=1&approval=1&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return true;");
                    else if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) == null)
                        imgApprove.Attributes.Add("onclick", "parent.openNewWindow('codehelpsuitability', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&newapplicant=true&popup=1&approval=1&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return true;");
                }

                if (imgReject != null)
                {
                    if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
                        imgReject.Attributes.Add("onclick", "parent.openNewWindow('codehelprejection', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreApprovalRejection.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString()
                            + "&personalmaster=true&popup=1&approval=1&crewplanid=" + drv["FLDCREWPLANID"].ToString()
                            + "&crewplandtkey=" + drv["FLDDTKEY"].ToString()
                            + "&calledfrom=pendingapproval" + "');return true;");

                    else if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) == null)
                        imgReject.Attributes.Add("onclick", "parent.openNewWindow('codehelprejection', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreApprovalRejection.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString()
                            + "&newapplicant=true&popup=1&approval=1&crewplanid=" + drv["FLDCREWPLANID"].ToString()
                            + "&crewplandtkey=" + drv["FLDDTKEY"].ToString()
                            + "&calledfrom=pendingapproval" + "');return true;");
                }

                LinkButton lbr = (LinkButton)e.Item.FindControl("lnkemployee");
                LinkButton lbrOffsigner = (LinkButton)e.Item.FindControl("lnkOffsignerEmployee");
                if (General.GetNullableDecimal(dailyrate.Text) > General.GetNullableDecimal(budgetedwage.Text))
                {
                    dailyrate.ForeColor = Color.Red;
                    dailyrate.Attributes.Add("style", "color:red !important;");

                }
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
                if (lbrOffsigner != null) lbrOffsigner.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + drv["FLDOFFSIGNERID"].ToString() + "'); return false;");

                RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTip");
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lbtn.ClientID;
                //lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                //lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
            else
                e.Item.Attributes["onclick"] = "";

            LinkButton Approve = (LinkButton)e.Item.FindControl("imgApprove");
            LinkButton lnkemployee = (LinkButton)e.Item.FindControl("lnkemployee");
            LinkButton lnkOffsignerEmployee = (LinkButton)e.Item.FindControl("lnkOffsignerEmployee");
            RadLabel lblName = (RadLabel)e.Item.FindControl("lblName");

            if (drv["FLDVIEWPARTICULARSYN"] != null && drv["FLDVIEWPARTICULARSYN"].ToString().Equals("1"))
            {
                lblName.Visible = false;
                lnkemployee.Visible = true;
            }
            else
            {
                lblName.Visible = true;
                lnkemployee.Visible = false;
            }

            RadLabel lblOffsignerName = (RadLabel)e.Item.FindControl("lblOffsignerName");

            if (drv["FLDOFFSIGNERVIEWPARTICULARSYN"] != null && drv["FLDOFFSIGNERVIEWPARTICULARSYN"].ToString().Equals("1"))
            {
                lblOffsignerName.Visible = false;
                lnkOffsignerEmployee.Visible = true;
            }
            else
            {
                lblOffsignerName.Visible = true;
                lnkOffsignerEmployee.Visible = false;
            }

            if (Approve != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, Approve.CommandName)) Approve.Visible = false;
            }
            if (lnkemployee != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, lnkemployee.CommandName)) lnkemployee.Visible = false;
            }
            if (lnkOffsignerEmployee != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, lnkOffsignerEmployee.CommandName)) lnkOffsignerEmployee.Visible = false;
            }
        }
    }
}
