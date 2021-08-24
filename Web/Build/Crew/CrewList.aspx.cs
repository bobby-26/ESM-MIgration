using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
using System.Web;

public partial class CrewList : PhoenixBasePage
{
    public string vesselname;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Crew/CrewList.aspx" + (Request.QueryString["access"] != null ? "?access=1" : string.Empty), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewListFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewList.aspx", "Clear Filter", " <i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuCrewList.AccessRights = this.ViewState;
            MenuCrewList.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            OCIMFMenu(string.Empty);
            if (!IsPostBack)
            {
                ViewState["chkRankExp"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                SetVesselDetails();
                gvCrewList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvBelowSafeScale.SelectedIndexes.Clear();
        gvBelowSafeScale.EditIndexes.Clear();
        gvBelowSafeScale.DataSource = null;
        gvBelowSafeScale.Rebind();
        gvCrewList.SelectedIndexes.Clear();
        gvCrewList.EditIndexes.Clear();
        gvCrewList.DataSource = null;
        gvCrewList.Rebind();
    }
    private void BindScale()
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentCrewListSelection;
            DataTable dt = new DataTable();
            if (!IsPostBack)
                dt = PhoenixCrewManagement.CrewBelowSafeScale(nvc != null ? General.GetNullableInteger(nvc.Get("ddlVessel")) : null);
            else
                dt = PhoenixCrewManagement.CrewBelowSafeScale(General.GetNullableInteger(ddlVessel.SelectedVessel));
            gvBelowSafeScale.DataSource = dt;
            if (dt.Rows.Count > 0)
            {
                lblBelowsafe.Visible = true;
                Span1.Visible = true;
            }
            else
            {
                lblBelowsafe.Visible = false;
                Span1.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBelowSafeScale_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindScale();
    }
    protected void SetVessel(Object sender, EventArgs e)
    {

        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
        if (chkonboard.Checked == true)
            criteria.Add("chkSailOnly", chkonboard.Checked == true ? "1" : null);

        Filter.CurrentCrewListSelection = criteria;

        NameValueCollection nvc = Filter.CurrentCrewListSelection;
        ddlVessel.SelectedVessel = nvc.Get("ddlVessel");
        SetVesselDetails();
        Rebind();
        OCIMFMenu(nvc.Get("ddlVessel"));
    }
    protected void SetVesselDetails()
    {
        NameValueCollection nvc = Filter.CurrentCrewListSelection;
        if (nvc != null)
        {
            if (nvc.Get("chkRankExp") != null)
                ViewState["chkRankExp"] = "1";
            if (nvc.Get("ddlVessel") != null)
                ddlVessel.SelectedVessel = nvc.Get("ddlVessel");
            if (nvc.Get("chkSailOnly") != null)
                chkonboard.Checked = true;
            else
                chkonboard.Checked = false;
        }
        DataTable dt = PhoenixCrewManagement.GetTotalCrewCount(nvc != null ? General.GetNullableInteger(nvc.Get("ddlVessel")) : null);
        if (dt.Rows.Count > 0)
        {
            txtVesselName.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
            txtLBCapacity.Text = dt.Rows[0]["FLDLIFEBOATCAPACITY"].ToString();
            txtTotalCrew.Text = dt.Rows[0]["FLDTOTALCREWONBOARD"].ToString();
            if (dt.Rows[0]["FLDLBCREACHED"].ToString() == "1")
                txtTotalCrew.Attributes.Add("style", "color:red !important");
            else
                txtTotalCrew.Attributes.Add("style", "color:black !important");
        }
    }
    protected void MenuCrew_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("VESSELPOSITION"))
            {
                if (ddlVessel.SelectedVessel == "Dummy")
                {
                    ucError.ErrorMessage = "Please Select Vessel to Proceed";
                    ucError.Visible = true;
                    return;
                }
                Response.Redirect("../Crew/CrewVesselPositionList.aspx?vesselid=" + ddlVessel.SelectedVessel + "&launched=" + "CrewList", false);
            }
            else if (CommandName.ToUpper().Equals("VESSELADMIN"))
            {
                if (ddlVessel.SelectedVessel == "Dummy")
                {
                    ucError.ErrorMessage = "Please Select Vessel to Proceed";
                    ucError.Visible = true;
                    return;
                }
                Response.Redirect("../Crew/CrewVesselMasterAdmin.aspx?vesselid=" + ddlVessel.SelectedVessel + "&launched=" + "CrewList", false);
            }
            else if(CommandName.ToUpper().Equals("CREWLIST"))
            {
                Response.Redirect("../Crew/CrewFormat.aspx?rid=1", false);
            }
            else if (CommandName.ToUpper().Equals("IMOCREWLIST"))
            {
                Response.Redirect("../Crew/CrewFormat.aspx?rid=2", false);
            }
            else if (CommandName.ToUpper().Equals("USCREWLIST"))
            {
                Response.Redirect("../Crew/CrewFormat.aspx?rid=3", false);
            }
            else if (CommandName.ToUpper().Equals("SPECCREWLIST"))
            {
                Response.Redirect("../Crew/CrewFormat.aspx?rid=4", false);
            }
            else if (CommandName.ToUpper().Equals("REVCREWLIST"))
            {
                Response.Redirect("../Crew/CrewFormat.aspx?rid=5", false);
            }
            else if (CommandName.ToUpper().Equals("OWNERCREWLIST"))
            {
                Response.Redirect("../Crew/CrewFormat.aspx?rid=6", false);
            }




        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentCrewListSelection = null;
                ViewState["chkRankExp"] = "";
                vesselname = "";
                txtVesselName.Text = "";
                txtLBCapacity.Text = "";
                txtTotalCrew.Text = "";
                ddlVessel.SelectedVessel = "";
                SetVesselDetails();
                ViewState["PAGENUMBER"] = 1;
                gvCrewList.CurrentPageIndex = 0;
                Rebind();
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

        string date = DateTime.Now.ToShortDateString();

        string[] alColumns = { "FLDROWNUMBER", "FLDEMPLOYEENAME", "FLDEMPLOYEECODE", "FLDRANKNAME", "FLDNATIONALITYNAME", "FLDSTATUS", "FLDPASSPORTNO", "FLDDATEOFBIRTH", "FLDSIGNONDATE", "FLDDECIMALEXPERIENCE", "FLDRELIEFDUEDATE", "FLDSEAMANBOOKNO" };
        string[] alCaptions = { "Sr.No", "Name", "Employee Code", "Rank", "Nationality", "Status", "PP No.", "Birth Date", "(Exp.) Join", "Exp(M)", "Relief Due", "SMBK No" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int sortdirection;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentCrewListSelection;
        DataSet ds = PhoenixCrewManagement.SearchCrewOnboard(General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                            , nvc != null ? nvc.Get("lstRank") : string.Empty
                                                            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtOnDate")) : null
                                                            , nvc != null ? (byte?)General.GetNullableInteger(nvc.Get("chkSailOnly")) : null
                                                            , byte.Parse(Request.QueryString["access"] != null ? "1" : "0"), 1
                                                            , sortexpression, sortdirection
                                                            , (int)ViewState["PAGENUMBER"], iRowCount
                                                            , ref iRowCount, ref iTotalPageCount
                                                            , nvc != null ? nvc.Get("lstNationality") : string.Empty
                                                            , nvc != null ? nvc.Get("txtName") : string.Empty
                                                            , nvc != null ? General.GetNullableInteger(nvc.Get("rblExtraCrew")) : null
                                                            , null);
        //DataTable dt = ds.Tables[0].Copy();
        //General.ShowExcel("Crew List For " + vesselname, dt, alColumns, alCaptions, sortdirection, sortexpression);
        Response.AddHeader("Content-Disposition", "attachment; filename=CrewList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h3><center>Crew List for " + vesselname + "</center></h3></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDEMPLOYEENAME", "FLDEMPLOYEECODE", "FLDRANKNAME", "FLDNATIONALITYNAME", "FLDSTATUS", "FLDPASSPORTNO", "FLDDATEOFBIRTH", "FLDSIGNONDATE", "FLDDECIMALEXPERIENCE", "FLDRELIEFDUEDATE", "FLDSEAMANBOOKNO" };
        string[] alCaptions = { "Sr.No", "Name", "Employee Code", "Rank", "Nationality", "Status", "PP No.", "Birth Date", "(Exp.) Join", "Exp(M)", "Relief Due", "SMBK No" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        NameValueCollection nvc = Filter.CurrentCrewListSelection;


        DataSet ds = PhoenixCrewManagement.SearchCrewOnboard(nvc != null ? General.GetNullableInteger(nvc.Get("ddlVessel")) : null
                                                            , nvc != null ? nvc.Get("lstRank") : string.Empty
                                                            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtOnDate")) : null
                                                            , nvc != null ? (byte?)General.GetNullableInteger(nvc.Get("chkSailOnly")) : null
                                                            , byte.Parse(Request.QueryString["access"] != null ? "1" : "0"), 1
                                                            , sortexpression, sortdirection
                                                            , (int)ViewState["PAGENUMBER"], gvCrewList.PageSize
                                                            , ref iRowCount, ref iTotalPageCount
                                                            , nvc != null ? nvc.Get("lstNationality") : string.Empty
                                                            , nvc != null ? nvc.Get("txtName") : string.Empty
                                                            , nvc != null ? General.GetNullableInteger(nvc.Get("rblExtraCrew")) : null
                                                            , null);

        General.SetPrintOptions("gvCrewList", "Crew List", alCaptions, alColumns, ds);
        gvCrewList.DataSource = ds;
        gvCrewList.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvCrewList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewList.CurrentPageIndex + 1;

        BindData();
    }
    protected void gvCrewList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            RadLabel lblId = (RadLabel)item.FindControl("lblOverDue");
            RadLabel lbldue = (RadLabel)item.FindControl("lblDue");
            RadLabel type = (RadLabel)item.FindControl("lblType");
            RadLabel lblreflieprior = (RadLabel)item.FindControl("lblreflieprior");
            RadLabel lblsrno = (RadLabel)item.FindControl("lblsrno");
            RadLabel lblranknmae = (RadLabel)item.FindControl("lblranknmae");
            RadLabel lblnatinality = (RadLabel)item.FindControl("lblnatinality");
            RadLabel lblstatsname = (RadLabel)item.FindControl("lblstatsname");
            RadLabel lblsignondate = (RadLabel)item.FindControl("lblsignondate");
            RadLabel lblPDStatus = (RadLabel)item.FindControl("lblPDStatus");
            RadLabel lblReliefdate = (RadLabel)item.FindControl("lblReliefdate");
            RadLabel lblCDC = (RadLabel)item.FindControl("lblCDC");
            if (lblId.Text == "1")
            {
                lblsrno.Attributes.Add("style", "color:red !important");
                lblsrno.Attributes.Add("style", "color:red !important");
                lblranknmae.Attributes.Add("style", "color:red !important");
                lblnatinality.Attributes.Add("style", "color:red !important");
                lblstatsname.Attributes.Add("style", "color:red !important");
                lblsignondate.Attributes.Add("style", "color:red !important");
                lblPDStatus.Attributes.Add("style", "color:red !important");
                lblReliefdate.Attributes.Add("style", "color:red !important");
                lblCDC.Attributes.Add("style", "color:red !important");
            }
            if (type.Text == "2")
            {
                lblsrno.Attributes.Add("style", "color:blue !important");
                lblranknmae.Attributes.Add("style", "color:blue !important");
                lblnatinality.Attributes.Add("style", "color:blue !important");
                lblstatsname.Attributes.Add("style", "color:blue !important");
                lblsignondate.Attributes.Add("style", "color:blue !important");
                lblPDStatus.Attributes.Add("style", "color:blue !important");
                lblReliefdate.Attributes.Add("style", "color:blue !important");
                lblCDC.Attributes.Add("style", "color:blue !important");
            }
            if (lbldue.Text == "1")
            {
                lblsrno.Attributes.Add("style", "color:purple !important");
                lblranknmae.Attributes.Add("style", "color:purple !important");
                lblnatinality.Attributes.Add("style", "color:purple !important");
                lblstatsname.Attributes.Add("style", "color:purple !important");
                lblsignondate.Attributes.Add("style", "color:purple !important");
                lblPDStatus.Attributes.Add("style", "color:purple !important");
                lblReliefdate.Attributes.Add("style", "color:purple !important");
                lblCDC.Attributes.Add("style", "color:purple !important");
            }
            if (lblreflieprior.Text != "")
            {
                System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml(lblreflieprior.Text);
                lblsrno.Attributes.Add("style", "color:"+col+" !important");
                lblranknmae.Attributes.Add("style", "color:"+col+" !important");
                lblnatinality.Attributes.Add("style", "color:"+col+" !important");
                lblstatsname.Attributes.Add("style", "color:"+col+" !important");
                lblsignondate.Attributes.Add("style", "color:"+col+" !important");
                lblPDStatus.Attributes.Add("style", "color:"+col+" !important");
                lblReliefdate.Attributes.Add("style", "color:"+col+" !important");
                lblCDC.Attributes.Add("style", "color:"+col+" !important");

            }
            //if (drv["FLDVESSELSIGNEDOFF"].ToString() == "1") e.Row.CssClass = "greenfont";
            NameValueCollection nvc = Filter.CurrentCrewListSelection;

            if (nvc != null && nvc.Get("rblExtraCrew") == "1" && drv["FLDEXTRACREW"].ToString() == "1")
            {

                lblsrno.Attributes.Add("style", "color:maroon !important");
                lblranknmae.Attributes.Add("style", "color:maroon !important");
                lblnatinality.Attributes.Add("style", "color:maroon !important");
                lblstatsname.Attributes.Add("style", "color:maroon !important");
                lblsignondate.Attributes.Add("style", "color:maroon !important");
                lblPDStatus.Attributes.Add("style", "color:maroon !important");
                lblReliefdate.Attributes.Add("style", "color:maroon !important");
                lblCDC.Attributes.Add("style", "color:maroon !important");
            }
            if (nvc != null && nvc.Get("rblExtraCrew") == "2" && drv["FLDEXTRACREW"].ToString() == "1")
            {
                {

                    lblsrno.Attributes.Add("style", "color:green !important");
                    lblranknmae.Attributes.Add("style", "color:green !important");
                    lblnatinality.Attributes.Add("style", "color:green !important");
                    lblstatsname.Attributes.Add("style", "color:green !important");
                    lblsignondate.Attributes.Add("style", "color:green !important");
                    lblPDStatus.Attributes.Add("style", "color:green !important");
                    lblReliefdate.Attributes.Add("style", "color:green !important");
                    lblCDC.Attributes.Add("style", "color:green !important");
                }
            }

            RadLabel lblCrewId = (RadLabel)item.FindControl("lblCrewId");
            RadLabel ConId = (RadLabel)item.FindControl("lblConId");
            LinkButton lb = (LinkButton)item.FindControl("lnkCrew");
            RadLabel lblcrew = (RadLabel)item.FindControl("lblCrewName");
            RadLabel lblTTO = (RadLabel)item.FindControl("lblTTO");
            RadLabel newapp = (RadLabel)item.FindControl("lblNewApp");
            if (lblTTO.Text == "1") { lblcrew.Visible = true; lb.Visible = false; }
            else { lblcrew.Visible = false; lb.Visible = true; }
            if (newapp.Text == "1")
                lb.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + lblCrewId.Text + "&familyid=" + drv["FLDFAMILYID"].ToString() + "'); return false;");
            else
                lb.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + lblCrewId.Text + "&familyid=" + drv["FLDFAMILYID"].ToString() + "'); return false;");

            RadLabel empid = (RadLabel)item.FindControl("lblEmployeeid");
            RadLabel familyid = (RadLabel)item.FindControl("lblfamilyid");
            RadLabel empincrewchangeplan = (RadLabel)item.FindControl("lblCrewPlanYN");
            LinkButton sync = (LinkButton)item.FindControl("cmdSync");
            LinkButton sg = (LinkButton)item.FindControl("imgActivity");
            LinkButton con = (LinkButton)item.FindControl("cmdGenContract");
            LinkButton trv = (LinkButton)item.FindControl("cmdIniTravel");
            RadLabel rankid = (RadLabel)item.FindControl("lblrankid");
            RadLabel pdstatusid = (RadLabel)item.FindControl("lblPDStatusid");
            RadLabel lblVessel = (RadLabel)item.FindControl("lblVesselName");

            if (empincrewchangeplan != null && empincrewchangeplan.Text == "0") { if (trv != null) trv.Visible = true; }
            string vslid = "";
            if (nvc != null) vslid = nvc.Get("ddlVessel");

            if (lblVessel.Text != string.Empty) vesselname = lblVessel.Text;

            if (sync != null) sync.Visible = SessionUtil.CanAccess(this.ViewState, sync.CommandName);
            if (sg != null) sg.Visible = SessionUtil.CanAccess(this.ViewState, sg.CommandName);
            LinkButton imgSuitableCheck = (LinkButton)item.FindControl("imgSuitableCheck");
            imgSuitableCheck.Attributes.Add("onclick", "parent.openNewWindow('codehelp', '', '" + Session["sitepath"] + "/Crew/CrewSuitabilityCheck.aspx?empid=" + empid.Text + "&rnkid=" + rankid.Text + "&vessel=" + txtVesselName.Text + "&vslid=" + vslid.ToString() + "&personalmaster=true');return false;");
            if (con != null) con.Visible = SessionUtil.CanAccess(this.ViewState, con.CommandName);
            if (trv != null) con.Visible = SessionUtil.CanAccess(this.ViewState, con.CommandName);

            if (trv != null)
                //trv.Attributes.Add("onclick", "parent.openNewWindow('CrewPage', '', 'CrewTravelOffSignersTravelRequest.aspx?empid=" + empid.Text + "&rnkid=" + rankid.Text + "&vslid=" + vslid.ToString() + "&date=" + drv["FLDSIGNONDATE"].ToString() + "');return false;");
                trv.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/Crew/CrewTravelOffSignersTravelRequest.aspx?empid=" + empid.Text + "&vslid=" + vslid.ToString() + "&vessel=" + txtVesselName.Text + "');return false;");
            if (!string.IsNullOrEmpty(empid.Text) && type.Text.Equals("1"))
            {
                if (drv["FLDSIGNOFFDATE"].ToString() != string.Empty)
                {
                    con.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=CREWCONTRACT&contractid=" + drv["FLDCONTRACTID"].ToString() + "&planid=" + drv["FLDCREWPLANID"].ToString() + "&reffrom=cl&showmenu=0&accessfrom=2');return false;");
                    con.Visible = false;
                }
                else
                    con.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/Crew/CrewContract.aspx?empid=" + empid.Text + "&rnkid=" + rankid.Text + "&vslid=" + vslid.ToString() + "&date=" + drv["FLDSIGNONDATE"].ToString() + "&planid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
            }
            else
                con.Visible = false;

            if (drv["FLDVESSELSIGNEDOFF"].ToString() == "1" && con != null)
                con.Visible = false;

            if (type.Text == "1" && drv["FLDFAMILYID"].ToString() == string.Empty)
                sg.Attributes.Add("onclick", "parent.openNewWindow('CrewPage', '', '" + Session["sitepath"] + "/Crew/CrewActivityGeneral.aspx?empid=" + empid.Text + "&r=1&ntbr=0&inact=0&doa=0');return false;");
            else if (drv["FLDFAMILYID"].ToString() != string.Empty)
            {
                sg.Attributes.Add("onclick", "parent.openNewWindow('CrewPage', '', '" + Session["sitepath"] + "/Crew/CrewSignOnExtendReduce.aspx?empid=" + empid.Text + "&familyid=" + familyid.Text + "');return false;");
                lb.Text = drv["FLDFAMILYNAME"].ToString();
                con.Visible = false;
                sg.Visible = true;
            }
            else
                sg.Visible = false;

            if (drv["FLDSIGNOFFDATE"].ToString() != string.Empty || drv["FLDSIGNONOFFID"].ToString() == string.Empty)
                sync.Visible = false;
        }
        if (e.Item is GridNestedViewItem)
        {
            GridNestedViewItem nestedview = (GridNestedViewItem)e.Item;

            if (drv["FLDDTKEY"].ToString() != string.Empty)
            {
                //Image imgPhoto = (Image)nestedview.FindControl("imgPhoto");
                //if (imgPhoto != null)
                //    imgPhoto.ImageUrl = "../Common/Download.aspx?dtkey=" + filepath.Text;
                //Session["sitepath"]+"/Common/Download.aspx?dtkey=" + drv["FLDDTKEY"].ToString();
                //Session["sitepath"] + "/Common/Download.aspx?dtkey=" + drv["FLDDTKEY"].ToString();
                //HttpContext.Current.Session["sitepath"] + "/attachments/" + drv["FLDFILEPATH"].ToString() + "?time=" + DateTime.Now.TimeOfDay;

                RadLabel lblexpx = (RadLabel)nestedview.FindControl("lblexpx");
                RadLabel lblExp = (RadLabel)nestedview.FindControl("lblExp");
                RadLabel lblexpc = (RadLabel)nestedview.FindControl("lblexpc");
                if (ViewState["chkRankExp"].ToString() == "1")
                {
                    if (lblexpx != null) lblexpx.Visible = true;
                    if (lblExp != null) lblExp.Visible = true; if (lblexpc != null) lblexpc.Visible = true;
                }
                else
                {
                    if (lblexpx != null) lblexpx.Visible = false;
                    if (lblExp != null) lblExp.Visible = false; if (lblexpc != null) lblexpc.Visible = false;
                }
            }

        }
    }
    protected void gvCrewList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;
            if (e.CommandName.ToUpper() == "SEND")
            {
                try
                {
                    NameValueCollection nvc = Filter.CurrentCrewListSelection;
                    if (nvc != null)
                    {
                        string employeeid = ((RadLabel)eeditedItem.FindControl("lblEmployeeId")).Text;
                        string familyid = ((RadLabel)eeditedItem.FindControl("lblFamilyId")).Text;
                        PhoenixVesselAccountsEmployee.SendCrewDataToVessel(General.GetNullableInteger(employeeid).Value, int.Parse(nvc.Get("ddlVessel")), General.GetNullableInteger(familyid));
                        ucStatus.Text = "Successfully Sent to Vessel";
                    }
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName == "ExpandCollapse")
            {
                RadLabel filepath = (RadLabel)eeditedItem.FindControl("lblFilePath");
                RadLabel dtkey = (RadLabel)eeditedItem.FindControl("lblDtkey");
                Image imgPhoto = (e.Item as GridDataItem).ChildItem.FindControl("imgPhoto") as Image;
                DataTable dta = PhoenixCommonFileAttachment.AttachmentList(new Guid(dtkey.Text), "CREWIMAGE");

                if (dta.Rows.Count > 0)
                {
                    if (imgPhoto != null)
                        imgPhoto.ImageUrl = "../Common/Download.aspx?dtkey=" + dta.Rows[0]["FLDDTKEY"].ToString();
                }
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
        SetVesselDetails();
        Rebind();
        NameValueCollection nvc = Filter.CurrentCrewListSelection;
        OCIMFMenu(nvc.Get("ddlVessel"));
    }
    protected void OCIMFMenu(string VesselId)
    {
        if (VesselId == null)
            VesselId = ddlVessel.SelectedVessel;
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Vessel Position", "VESSELPOSITION");
        toolbar.AddButton("Admin", "VESSELADMIN");
        toolbar.AddImageLink("javascript:openNewWindow('OCIMF','','" + Session["sitepath"] + "/Crew/CrewOcimfLoginAccount.aspx?vslid=" + VesselId + "'); return false;", "OCIMF", "", "OCIMF", ToolBarDirection.Left);
        toolbar.AddButton("Crew Format", "CREWFORMAT", CreateCrewFormatSubTab(string.Empty, 0, string.Empty));

        MenuCrewOCIMF.AccessRights = this.ViewState;
        MenuCrewOCIMF.MenuList = toolbar.Show();
    }
    private PhoenixToolbar CreateCrewFormatSubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Crew List", "CREWLIST");
        toolbarsub.AddButton("IMO Crew List", "IMOCREWLIST");
        toolbarsub.AddButton("US Crew List", "USCREWLIST");
        toolbarsub.AddButton("Specific Crew List", "SPECCREWLIST");
        toolbarsub.AddButton("Revised Crew List", "REVCREWLIST"); 
        toolbarsub.AddButton("Revised Crew List", "OWNERCREWLIST");
        return toolbarsub;
    }

    protected void gvCrewList_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
        BindData();
    }
}
