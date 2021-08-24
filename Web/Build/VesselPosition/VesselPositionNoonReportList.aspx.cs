using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselPosition;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class VesselPositionNoonReportList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarnoonreportlist = new PhoenixToolbar();
            toolbarnoonreportlist.AddFontAwesomeButton("../VesselPosition/VesselPositionNoonReportList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbarnoonreportlist.AddFontAwesomeButton("javascript:CallPrint('gvNoonReport')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarnoonreportlist.AddFontAwesomeButton("../VesselPosition/VesselPositionNoonReportList.aspx", "Noon Report Print", "<i class=\"fas fa-file-pdf\"></i>", "REPORT");
            NameValueCollection nvc = Filter.CurrentNoonReportListFilter;
            if (nvc != null && nvc["LaunchFromDB"] != null)
                toolbarnoonreportlist.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/VesselPosition/VesselPositionNoonReportListFilter.aspx?LaunchFromDB=1'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "SEARCH");
            else
                toolbarnoonreportlist.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/VesselPosition/VesselPositionNoonReportListFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbarnoonreportlist.AddFontAwesomeButton("../VesselPosition/VesselPositionNoonReportList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                toolbarnoonreportlist.AddFontAwesomeButton("../VesselPosition/VesselPositionNoonReportList.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                toolbarnoonreportlist.AddFontAwesomeButton("../VesselPosition/VesselPositionNoonReportList.aspx", "Reresh", "<i class=\"fa-redo-refresh\"></i>", "PORTUPDATE");
            MenuNoonReportList.AccessRights = this.ViewState;
            MenuNoonReportList.MenuList = toolbarnoonreportlist.Show();  
          
            if (!IsPostBack)
            {             
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["OVERDUE"] = Request.QueryString["overdue"] != null ? "1" : "";
                ViewState["REVIEW"] = Request.QueryString["review"] != null ? "1" : "";
                gvNoonReport.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (Request.QueryString["DB"] != null)
                    Filter.IsFromDashboard = Request.QueryString["DB"];


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
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void NoonReportList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Rebind();
        }
        if (CommandName.ToUpper().Equals("ADD"))
        {
           Response.Redirect("VesselPositionNoonReport.aspx?mode=NEW", false);
        }
        if (CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
        }
        if (CommandName.ToUpper().Equals("REPORT"))
        {
            Response.Redirect("../Reports/ReportsView.aspx?applicationcode=17&reportcode=NOONREPORT", false);
        }
        if (CommandName.ToUpper().Equals("PORTUPDATE"))
        {
            updateportnames();
            Rebind();
        }

    }
    private void updateportnames()
    {
        try
        {
            PhoenixVesselPositionNoonReport.NoonReportportUpdate();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ClearFilter()
    {
        Filter.CurrentNoonReportListFilter = null;
        Session["VESSELARRIVALID"] = null; 
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void gvNoonReport_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                        db.Visible = false;
                }
                LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (edit != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    edit.Visible = false;
                }
                LinkButton cmdReset = (LinkButton)e.Item.FindControl("cmdReset");
                if (cmdReset != null)
                {
                    cmdReset.Visible = drv["FLDRESETFLAG"].ToString() == "1" ? true : false;
                }

                LinkButton ImgSentYN = (LinkButton)e.Item.FindControl("ImgSentYN");
                if (ImgSentYN != null)
                {
                    ImgSentYN.Visible = drv["FLDCONFIRMEDYN"].ToString() == "1" ? false : true;
                }

                LinkButton ImgRangeAlert = (LinkButton)e.Item.FindControl("ImgRangeAlert");
                if (ImgRangeAlert != null)
                {
                    if (General.GetNullableString(drv["FLDRANGEALERT"].ToString()) != null)
                    {
                        ImgRangeAlert.Visible = true;
                        UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucRangeAlert");
                        if (uct != null)
                        {
                            uct.Position = ToolTipPosition.TopCenter;
                            uct.TargetControlId = ImgRangeAlert.ClientID;
                        }
                    }

                }
                LinkButton ImgReview = (LinkButton)e.Item.FindControl("cmdReview");
                if (ImgReview != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode==0)
                {
                    if (General.GetNullableInteger(drv["FLDREVIEWDYN"].ToString()) != null && General.GetNullableInteger(drv["FLDREVIEWDYN"].ToString())==0)
                    {
                        ImgReview.Visible = true;
                        ImgReview.Visible = SessionUtil.CanAccess(this.ViewState, ImgReview.CommandName);
                        
                        ImgReview.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/VesselPosition/VesselPositionNoonReportReview.aspx?VesselId=" + drv["FLDVESSELID"].ToString() + "&NoonReportID=" + drv["FLDNOONREPORTID"].ToString() + "');");
                    }
                }

                RadLabel lblSpeed = (RadLabel)e.Item.FindControl("lblSpeed");
                RadLabel lblCPSpeed = (RadLabel)e.Item.FindControl("lblCPSpeed");

                if (lblSpeed != null && lblCPSpeed != null)
                {
                    decimal? actualspeed = General.GetNullableDecimal(lblSpeed.Text);
                    decimal? cpspeed = General.GetNullableDecimal(lblCPSpeed.Text);
                    actualspeed = (actualspeed == null) ? 0 : actualspeed;
                    cpspeed = (cpspeed == null) ? 0 : cpspeed;
                }

                RadLabel lblHfoCons = (RadLabel)e.Item.FindControl("lblHfoCons");
                RadLabel lblCPHfoCons = (RadLabel)e.Item.FindControl("lblCPHfoCons");

                if (lblHfoCons != null && lblCPHfoCons != null)
                {
                    decimal? actualFOCons = General.GetNullableDecimal(lblHfoCons.Text);
                    decimal? cpFOCons = General.GetNullableDecimal(lblCPHfoCons.Text);
                    actualFOCons = (actualFOCons == null) ? 0 : actualFOCons;
                    cpFOCons = (cpFOCons == null) ? 0 : cpFOCons;
                }

                RadLabel lblMdoCons = (RadLabel)e.Item.FindControl("lblMdoCons");
                RadLabel lblCPMdoCons = (RadLabel)e.Item.FindControl("lblCPMdoCons");

                if (lblMdoCons != null && lblCPMdoCons != null)
                {
                    decimal? actualDOCons = General.GetNullableDecimal(lblMdoCons.Text);
                    decimal? cpDOCons = General.GetNullableDecimal(lblCPMdoCons.Text);
                    actualDOCons = (actualDOCons == null) ? 0 : actualDOCons;
                    cpDOCons = (cpDOCons == null) ? 0 : cpDOCons;
                }

                RadLabel lblPassingthroughHRA = (RadLabel)e.Item.FindControl("lblPassingthroughHRA");
                RadLabel lblCallingUSPort = (RadLabel)e.Item.FindControl("lblCallingUSPort");
                RadLabel lblEntryintoECA = (RadLabel)e.Item.FindControl("lblEntryintoECA");

                LinkButton ImgSymPassingthroughHRA = (LinkButton)e.Item.FindControl("ImgSymPassingthroughHRA");
                LinkButton ImgSymCallingUSPort = (LinkButton)e.Item.FindControl("ImgSymCallingUSPort");
                LinkButton ImgSymEntryintoECA = (LinkButton)e.Item.FindControl("ImgSymEntryintoECA");


                RadLabel lblMEfoCons = (RadLabel)e.Item.FindControl("lblMdoCons");
                RadLabel lblHFOCPHidden = (RadLabel)e.Item.FindControl("lblHFOCPHidden");

                if (lblMEfoCons != null && lblHFOCPHidden != null)
                {
                    decimal? actualFOCons = General.GetNullableDecimal(lblMEfoCons.Text);
                    decimal? cpFOCons = General.GetNullableDecimal(lblHFOCPHidden.Text);
                    if (actualFOCons != null && cpFOCons != null)
                    {
                        if (actualFOCons > cpFOCons)
                        {
                            if (drv["FLDVESSELSTATUS"].ToString() != "INPORT")
                            {
                                //lblMEfoCons.BackColor = System.Drawing.Color.Red;
                                //lblMEfoCons.ForeColor = System.Drawing.Color.White;
                                lblMEfoCons.Style.Add("Color", "Red !important;");
                            }
                        }
                    }

                }

                RadLabel lblHMDOCPHidden = (RadLabel)e.Item.FindControl("lblHMDOCPHidden");
                RadLabel lblDOCons = (RadLabel)e.Item.FindControl("lblDOCons");

                if (lblDOCons != null && lblHMDOCPHidden != null)
                {
                    decimal? actualDOCons = General.GetNullableDecimal(lblDOCons.Text);
                    decimal? cpDOCons = General.GetNullableDecimal(lblHMDOCPHidden.Text);
                    if (actualDOCons != null && cpDOCons != null)
                    {
                        if (actualDOCons > cpDOCons)
                        {
                            if (drv["FLDVESSELSTATUS"].ToString() != "INPORT")
                            {
                                //lblDOCons.BackColor = System.Drawing.Color.Red;
                                //lblDOCons.ForeColor = System.Drawing.Color.White;
                                lblDOCons.Style.Add("Color", "Red !important;");
                            }
                        }
                    }

                }

                RadLabel lblSpeedHidden = (RadLabel)e.Item.FindControl("lblSpeedHidden");
                RadLabel lblSpeed1 = (RadLabel)e.Item.FindControl("lblSpeed");

                if (lblSpeed1 != null && lblSpeedHidden != null)
                {
                    decimal? actualSpeed = General.GetNullableDecimal(lblSpeed1.Text);
                    decimal? cpSpeed = General.GetNullableDecimal(lblSpeedHidden.Text);
                    if (actualSpeed != null && cpSpeed != null)
                    {
                        if (actualSpeed < cpSpeed)
                        {
                            if (drv["FLDVESSELSTATUS"].ToString() != "INPORT")
                            {
                                //lblSpeed1.BackColor = System.Drawing.Color.Red;
                                //lblSpeed1.ForeColor = System.Drawing.Color.White;
                                lblSpeed1.Style.Add("Color", "Red !important;");
                            }
                        }
                    }

                }

                RadLabel windforce = (RadLabel)e.Item.FindControl("lblWindForce");

                if (windforce != null)
                {
                    decimal? windforceval = General.GetNullableDecimal(windforce.Text);
                    decimal? windforcevalconfig = General.GetNullableDecimal(drv["FLDWINDFORCEMAX"].ToString());
                    if (windforceval != null)
                    {
                        if (windforcevalconfig != null && windforceval > windforcevalconfig)
                        {
                            //windforce.BackColor = System.Drawing.Color.Red;
                            //windforce.ForeColor = System.Drawing.Color.White;
                            windforce.Style.Add("Color", "Red !important;");
                        }
                    }

                }

                if (ImgSymPassingthroughHRA != null)
                {
                    if (lblPassingthroughHRA != null)
                    {
                        if (lblPassingthroughHRA.Text == "YES")
                        {
                            ImgSymPassingthroughHRA.Visible = true;
                            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucPassingthroughHRA");
                            if (uct != null)
                            {
                                uct.Position = ToolTipPosition.TopCenter;
                                uct.TargetControlId = ImgSymPassingthroughHRA.ClientID;

                            }
                        }
                        else
                            ImgSymPassingthroughHRA.Visible = false;
                    }
                }

                if (ImgSymCallingUSPort != null)
                {
                    if (lblCallingUSPort != null)
                    {
                        if (lblCallingUSPort.Text == "YES")
                        {
                            ImgSymCallingUSPort.Visible = true;
                            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucCallingUSPort");
                            if (uct != null)
                            {
                                uct.Position = ToolTipPosition.TopCenter;
                                uct.TargetControlId = ImgSymCallingUSPort.ClientID;
                            }
                        }
                        else
                            ImgSymCallingUSPort.Visible = false;
                    }
                }

                if (ImgSymEntryintoECA != null)
                {
                    if (lblEntryintoECA != null)
                    {
                        if (lblEntryintoECA.Text == "YES")
                        {
                            ImgSymEntryintoECA.Visible = true;
                            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucEntryintoECA");
                            if (uct != null)
                            {
                                uct.Position = ToolTipPosition.TopCenter;
                                uct.TargetControlId = ImgSymEntryintoECA.ClientID;
                            }
                        }
                        else
                            ImgSymEntryintoECA.Visible = false;
                    }
                }
                RadLabel lblNextPort = (RadLabel)e.Item.FindControl("lblNextPort");
                if (lblNextPort != null)
                {
                    UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucNextPortNoonTip");
                    if (uct != null)
                    {
                        uct.Position = ToolTipPosition.TopCenter;
                        uct.TargetControlId = lblNextPort.ClientID;
                    }
                }

                RadLabel lblreviewedby = (RadLabel)e.Item.FindControl("lblreviewedby");
                if (lblreviewedby != null)
                {
                    UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucreviewedby");
                    if (uct != null)
                    {
                        uct.Position = ToolTipPosition.TopCenter;
                        uct.TargetControlId = lblreviewedby.ClientID;
                    }
                }
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0 && PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper()!="OWNER")
            {
                gvNoonReport.Columns[0].Visible = false;
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                gvNoonReport.Columns[26].Visible = false;
                gvNoonReport.Columns[27].Visible = false;
            }
            RadLabel lblIdlYn = (RadLabel)e.Item.FindControl("lblIdlYn");
            LinkButton imgFlag = (LinkButton)e.Item.FindControl("imgFlag");
            if (lblIdlYn != null && imgFlag != null && lblIdlYn.Text == "1")
            {
                imgFlag.Visible = true;
               
            }

            //if (e.Item.RowType == DataControlRowType.Header)
            //{
            //    if (ViewState["SORTEXPRESSION"] != null)
            //    {
            //        HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
            //        if (img != null)
            //        {
            //            if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
            //                img.Src = Session["images"] + "/arrowUp.png";
            //            else
            //                img.Src = Session["images"] + "/arrowDown.png";

            //            img.Visible = true;
            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns;
        string[] alCaptions;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "OWNER")
        {
            alColumns = new string[22] { "FLDNOONREPORTDATE", "FLDVOYAGENO", "FLDBALLASTYESNO", "FLDWINDFORCE", "FLDATPORTNEXTPORT", "FLDETA", "FLDMERPM", "FLDSLIP", "FLDLOGSPEED", "FLDCHARTERSPEED", "FLDHFOOILCONSUMPTIONQTY", "FLDMEFOOILCONSUMPTIONQTY", "FLDHFOCHARTERCOUNSUMPTION", "FLDMDOOILCONSUMPTIONQTY", "FLDMDOCHARTERCOUNSUMPTION", "FLDFOCATFINES", "FLDOILMAJORCARGOONBOARDYN", "FLDOILMAJOR", "FLDOVERDUE", "FLDISPASSINGTHROUGHHRA", "FLDISUSWATERS", "FLDECAYN" };
            alCaptions = new string[22] { "Date", "Voyage No.", "Ballast/Laden", "Wind Force", "At Port / Next Port", "ETA", "ME RPM", "Slip", "Speed", "C/P Speed", "FO Cons", "ME FO Cons", "ME FO C/P", "DO Cons", "DO C/P", "FO Cat Fines", "Oil Major Cargo", "Oil Major", "Over Due", "Passing through HRA", "Calling US Port With in 7 days", "Entry into ECA" };
        }
        else
        {
            alColumns = new string[23] { "FLDVESSELNAME", "FLDNOONREPORTDATE", "FLDVOYAGENO", "FLDBALLASTYESNO", "FLDWINDFORCE", "FLDATPORTNEXTPORT", "FLDETA", "FLDMERPM", "FLDSLIP", "FLDLOGSPEED", "FLDCHARTERSPEED", "FLDHFOOILCONSUMPTIONQTY", "FLDMEFOOILCONSUMPTIONQTY", "FLDHFOCHARTERCOUNSUMPTION", "FLDMDOOILCONSUMPTIONQTY", "FLDMDOCHARTERCOUNSUMPTION", "FLDFOCATFINES", "FLDOILMAJORCARGOONBOARDYN", "FLDOILMAJOR", "FLDOVERDUE", "FLDISPASSINGTHROUGHHRA", "FLDISUSWATERS", "FLDECAYN" };
            alCaptions = new string[23] { "VesselName", "Date", "Voyage No.", "Ballast/Laden", "Wind Force", "At Port / Next Port", "ETA", "ME RPM", "Slip", "Speed", "C/P Speed", "FO Cons", "ME FO Cons", "ME FO C/P", "DO Cons", "DO C/P", "FO Cat Fines", "Oil Major Cargo", "Oil Major", "Over Due", "Passing through HRA", "Calling US Port With in 7 days", "Entry into ECA" };
        }
		string sortexpression;
        int? sortdirection = null;
        int? isoverdue = General.GetNullableInteger(ViewState["OVERDUE"].ToString());
        int? isreview = General.GetNullableInteger(ViewState["REVIEW"].ToString());

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        if (Request.QueryString["overdue"] != null || Request.QueryString["review"] != null)
            Filter.CurrentNoonReportListFilter = null;
        NameValueCollection nvc = Filter.CurrentNoonReportListFilter;

        DataSet ds = new DataSet();
        if (Filter.IsFromDashboard != null && Filter.IsFromDashboard == "1")
        {
            ds = PhoenixVesselPositionNoonReport.NoonReportDashboardSearch(
                General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : string.Empty)
               , sortexpression
               , sortdirection
               , (int)ViewState["PAGENUMBER"]
               , gvNoonReport.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               , isoverdue
               , isreview);
        }
        else if (nvc != null)
        {
            ds = PhoenixVesselPositionNoonReport.NoonReportSearch(
            General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : string.Empty)
            , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty)
            , General.GetNullableDateTime(nvc != null ? nvc["txtETAFrom"] : string.Empty)
            , General.GetNullableDateTime(nvc != null ? nvc["txtETATo"] : string.Empty)
            , General.GetNullableDateTime(nvc != null ? nvc["txtReportFrom"] : string.Empty)
            , General.GetNullableDateTime(nvc != null ? nvc["txtReportTo"] : string.Empty)
            , General.GetNullableInteger(nvc != null ? nvc["UcPortfrom"] : string.Empty)
            , General.GetNullableInteger(nvc != null ? nvc["UcPortTo"] : string.Empty)
            , General.GetNullableInteger(nvc != null ? nvc["ddlMonth"] : string.Empty)
            , General.GetNullableInteger(nvc != null ? nvc["ddlYear"] : string.Empty)
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , gvNoonReport.PageSize
            , ref iRowCount
            , ref iTotalPageCount
            , isoverdue
            , isreview);
        }
        else
        {
            ds = PhoenixVesselPositionNoonReport.NoonReportDefaultSearch(
               General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : string.Empty)
              , sortexpression
              , sortdirection
              , (int)ViewState["PAGENUMBER"]
              , gvNoonReport.PageSize
              , ref iRowCount
              , ref iTotalPageCount);
        }
        Response.AddHeader("Content-Disposition", "attachment; filename=\"NoonReport.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Noon Report</h3></td>");
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

    protected void gvNoonReport_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper() == "EDIT")
            {
                string NoonReportID = ((RadLabel)e.Item.FindControl("lblNoonReportID")).Text;
                string ReportType = ((RadLabel)e.Item.FindControl("lblReportType")).Text;
                Session["NOONREPORTID"] = NoonReportID;
                Filter.CurrentNoonReportLaunchFrom = null;
                Response.Redirect("VesselPositionNoonReport.aspx?NoonReportID=" + NoonReportID, false);
                    
            }
            else if(e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionNoonReport.DeleteNoonReport(
                    (PhoenixSecurityContext.CurrentSecurityContext.UserCode), 
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblNoonReportID")).Text));

                Rebind();
            }
            else if(e.CommandName.ToUpper().Equals("RESET"))
            {
                PhoenixVesselPositionNoonReport.ResetLastNoonReport(
                    (PhoenixSecurityContext.CurrentSecurityContext.UserCode),
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblNoonReportID")).Text));

                Rebind();
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

    protected void gvNoonReport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvNoonReport.CurrentPageIndex + 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns;
        string[] alCaptions;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "OWNER")
        {
            alColumns = new string[22] { "FLDNOONREPORTDATE", "FLDVOYAGENO", "FLDBALLASTYESNO", "FLDWINDFORCE", "FLDATPORTNEXTPORT", "FLDETA", "FLDMERPM", "FLDSLIP", "FLDLOGSPEED", "FLDCHARTERSPEED", "FLDHFOOILCONSUMPTIONQTY", "FLDMEFOOILCONSUMPTIONQTY", "FLDHFOCHARTERCOUNSUMPTION", "FLDMDOOILCONSUMPTIONQTY", "FLDMDOCHARTERCOUNSUMPTION", "FLDFOCATFINES", "FLDOILMAJORCARGOONBOARDYN", "FLDOILMAJOR", "FLDOVERDUE", "FLDISPASSINGTHROUGHHRA", "FLDISUSWATERS", "FLDECAYN" };
            alCaptions = new string[22] { "Date", "Voyage No.", "Ballast/Laden", "Wind Force", "At Port / Next Port", "ETA", "ME RPM", "Slip", "Speed", "C/P Speed", "FO Cons", "ME FO Cons", "ME FO C/P", "DO Cons", "DO C/P", "FO Cat Fines", "Oil Major Cargo", "Oil Major", "Over Due", "Passing through HRA", "Calling US Port With in 7 days", "Entry into ECA" };
        }
        else
        {
            alColumns = new string[23] { "FLDVESSELNAME", "FLDNOONREPORTDATE", "FLDVOYAGENO", "FLDBALLASTYESNO", "FLDWINDFORCE", "FLDATPORTNEXTPORT", "FLDETA", "FLDMERPM", "FLDSLIP", "FLDLOGSPEED", "FLDCHARTERSPEED", "FLDHFOOILCONSUMPTIONQTY", "FLDMEFOOILCONSUMPTIONQTY", "FLDHFOCHARTERCOUNSUMPTION", "FLDMDOOILCONSUMPTIONQTY", "FLDMDOCHARTERCOUNSUMPTION", "FLDFOCATFINES", "FLDOILMAJORCARGOONBOARDYN", "FLDOILMAJOR", "FLDOVERDUE", "FLDISPASSINGTHROUGHHRA", "FLDISUSWATERS", "FLDECAYN" };
            alCaptions = new string[23] { "VesselName", "Date", "Voyage No.", "Ballast/Laden", "Wind Force", "At Port / Next Port", "ETA", "ME RPM", "Slip", "Speed", "C/P Speed", "FO Cons", "ME FO Cons", "ME FO C/P", "DO Cons", "DO C/P", "FO Cat Fines", "Oil Major Cargo", "Oil Major", "Over Due", "Passing through HRA", "Calling US Port With in 7 days", "Entry into ECA" };
        }

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        int? isoverdue = General.GetNullableInteger(ViewState["OVERDUE"].ToString());
        int? isreview = General.GetNullableInteger(ViewState["REVIEW"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (Request.QueryString["overdue"] != null || Request.QueryString["review"] != null)
            Filter.CurrentNoonReportListFilter = null;
        NameValueCollection nvc = Filter.CurrentNoonReportListFilter;
        DataSet ds;
        if(Filter.IsFromDashboard != null && Filter.IsFromDashboard == "1")
        {
            ds = PhoenixVesselPositionNoonReport.NoonReportDashboardSearch(
                General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : string.Empty)
               , sortexpression
               , sortdirection
               , (int)ViewState["PAGENUMBER"]
               , gvNoonReport.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               , isoverdue
               , isreview);
        }
        else if (nvc != null)
        {
            ds = PhoenixVesselPositionNoonReport.NoonReportSearch(
                General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : string.Empty)
               , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty)
               , General.GetNullableDateTime(nvc != null ? nvc["txtETAFrom"] : string.Empty)
               , General.GetNullableDateTime(nvc != null ? nvc["txtETATo"] : string.Empty)
               , General.GetNullableDateTime(nvc != null ? nvc["txtReportFrom"] : string.Empty)
               , General.GetNullableDateTime(nvc != null ? nvc["txtReportTo"] : string.Empty)
               , General.GetNullableInteger(nvc != null ? nvc["UcPortfrom"] : string.Empty)
               , General.GetNullableInteger(nvc != null ? nvc["UcPortTo"] : string.Empty)
               , General.GetNullableInteger(nvc != null ? nvc["ddlMonth"] : string.Empty)
               , General.GetNullableInteger(nvc != null ? nvc["ddlYear"] : string.Empty)
               , sortexpression
               , sortdirection
               , (int)ViewState["PAGENUMBER"]
               , gvNoonReport.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               , isoverdue
               , isreview);
        }
        else
        {
            ds = PhoenixVesselPositionNoonReport.NoonReportDefaultSearch(
                General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : string.Empty)
               , sortexpression
               , sortdirection
               , (int)ViewState["PAGENUMBER"]
               , gvNoonReport.PageSize
               , ref iRowCount
               , ref iTotalPageCount);
        }

        General.SetPrintOptions("gvNoonReport", "Noon Report", alCaptions, alColumns, ds);

            gvNoonReport.DataSource = ds;
        gvNoonReport.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvNoonReport.SelectedIndexes.Clear();
        gvNoonReport.EditIndexes.Clear();
        gvNoonReport.DataSource = null;
        gvNoonReport.Rebind();
    }

    protected void gvNoonReport_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
        Rebind();
    }
}
