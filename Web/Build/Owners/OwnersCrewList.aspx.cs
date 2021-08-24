using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.OwnersCrewlist;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Owners;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using Telerik.Web.UI;

public partial class OwnersCrewList : PhoenixBasePage
{
    public string vesselname;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Owners/OwnersCrewList.aspx" + (Request.QueryString["access"] != null ? "?access=1" : string.Empty), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrewList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuCrewList.AccessRights = this.ViewState;
            MenuCrewList.MenuList = toolbar1.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["ACCESSTOCREWDETAILS"] = null;
                txtFromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString();
                txtToDate.Text = LastDayOfMonthFromDateTime(DateTime.Now).ToString();
                gvCrewList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            checkAccesstoCrewDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void checkAccesstoCrewDetails()
    {
        DataSet ds = PhoenixOwnersVessel.ListAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        int ownerid = 0;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FLDPRINCIPALID"].ToString()))
                ownerid = int.Parse(ds.Tables[0].Rows[0]["FLDPRINCIPALID"].ToString());
        }
        if (ownerid != 0)
        {
            DataSet ds1 = PhoenixRegistersOwnerMapping.EditOwnerMapping(ownerid);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                if (ds1.Tables[0].Rows[0]["FLDVIEWCREWDETAILS"].ToString() == "1")
                    ViewState["ACCESSTOCREWDETAILS"] = 1;
                else
                    ViewState["ACCESSTOCREWDETAILS"] = 0;
            }
            else
                ViewState["ACCESSTOCREWDETAILS"] = 0;
        }
        else
            ViewState["ACCESSTOCREWDETAILS"] = 1;
    }
    protected void Rebind()
    {
        gvCrewList.SelectedIndexes.Clear();
        gvCrewList.EditIndexes.Clear();
        gvCrewList.DataSource = null;
        gvCrewList.Rebind();
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ddlVessel.SelectedVessel))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    Rebind();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidFilter(string vessellist)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (vessellist.Equals("") || vessellist.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Select Vessel";
        }

        return (!ucError.IsError);
    }
    protected void CrewList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
                ShowExcel();
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
        string[] alColumns = { "FLDROWNUMBER", "FLDNAME", "FLDFILENO", "FLDRANKNAME", "FLDDATEOFBIRTH", "FLDPASSPORTNO", "FLDDOI",
                                 "FLDPLACEOFISSUE", "FLDDOE", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDSEAPORTNAME" };
        string[] alCaptions = { "Sr.No", "Name", "Emp.No", "Rank", "DOB", "Passport No", "DOI", "POI", "DOE", "Sign On Date", "Relief Due", "Joining Port" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixOwnersCrewlist.SearchVesseEmployee(General.GetNullableInteger(ddlVessel.SelectedVessel.ToString())
                                                                        , General.GetNullableDateTime(txtFromDate.Text)
                                                                       , General.GetNullableDateTime(txtToDate.Text)
                                                                        , sortexpression, sortdirection
                                                                        , (int)ViewState["PAGENUMBER"], iRowCount
                                                                        , ref iRowCount, ref iTotalPageCount);

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
                Response.Write("<td align='left'>");
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

        string[] alColumns = { "FLDROWNUMBER", "FLDNAME", "FLDFILENO", "FLDRANKNAME", "FLDDATEOFBIRTH", "FLDPASSPORTNO", "FLDDOI",
                                 "FLDPLACEOFISSUE", "FLDDOE", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDSEAPORTNAME" };
        string[] alCaptions = { "Sr.No", "Name", "Emp.No", "Rank", "DOB", "Passport No", "DOI", "POI", "DOE", "Sign On Date", "Relief Due", "Joining Port" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = PhoenixOwnersCrewlist.SearchVesseEmployee(General.GetNullableInteger(ddlVessel.SelectedVessel.ToString())
                                                                       , General.GetNullableDateTime(txtFromDate.Text)
                                                                       , General.GetNullableDateTime(txtToDate.Text)
                                                                       , sortexpression, sortdirection
                                                                       , (int)ViewState["PAGENUMBER"], gvCrewList.PageSize
                                                                       , ref iRowCount, ref iTotalPageCount);
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
    protected void gvCrewList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToString().ToUpper() == "SORT")
            return;
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        if (e.CommandName.ToString().ToUpper() == "")
        {
           }
    }
   
    protected void gvCrewList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            // RadLabel lblId = (RadLabel)item.FindControl("lblOverDue");
            RadLabel lblCrewId = (RadLabel)item.FindControl("lblCrewId");
            RadLabel lblrank = (RadLabel)item.FindControl("lblRankID");
            RadLabel lblCrew = (RadLabel)item.FindControl("lblCrew");

            LinkButton lb = (LinkButton)item.FindControl("lnkCrew");
            lb.Attributes.Add("onclick", "javascript:parent.openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + lblCrewId.Text + "'); return false;");
            //lb.Enabled = SessionUtil.CanAccess(this.ViewState, "PROFILE");

            LinkButton biodata = (LinkButton)item.FindControl("cmdBioData");
            biodata.Attributes.Add("onclick", "javascript:parent.openNewWindow('BioData','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=BIODATA&empid="
                        + lblCrewId.Text + "&showmenu=1'); return false;");
            biodata.Visible = SessionUtil.CanAccess(this.ViewState, biodata.CommandName);

            RadLabel empid = (RadLabel)item.FindControl("lblEmployeeid");          
            LinkButton appraisal = (LinkButton)item.FindControl("cmdAppraisal");
            if (appraisal != null)
            {
                appraisal.Visible = SessionUtil.CanAccess(this.ViewState, appraisal.CommandName);
              //  appraisal.Attributes.Add
            }
            RadLabel lblVessel = (RadLabel)item.FindControl("lblVesselName");

            if (lblVessel.Text != string.Empty)
            {
                vesselname = lblVessel.Text;
            }
            if (SessionUtil.CanAccess(this.ViewState, "PROFILE"))
            {
                lb.Visible = true;
                lblCrew.Visible = false;
            }
            else
            {
                lb.Visible = false;
                lblCrew.Visible = true;
            }

            //////////////////////////////////////////////////////////////////////////////////
         //       lb.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + lblCrewId.Text + "&familyid=" + drv["FLDFAMILYID"].ToString() + "'); return false;");
         
           
        }
       
    }
    protected void gvCrewList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName == "APPRAISAL")
            {
                Filter.CurrentCrewSelection = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", "parent.openNewWindow('Appraisal','','" + Session["sitepath"] + "/Crew/CrewAppraisal.aspx');", true);

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
        Rebind();
    }
    private DateTime LastDayOfMonthFromDateTime(DateTime dateTime)
    {
        DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
        return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
    }
}
