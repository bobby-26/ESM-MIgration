using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportDGSignOn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Sign On", "SIGNON");
        toolbarmain.AddButton("Sign Off", "SIGNOFF");
        CrewMenu.AccessRights = this.ViewState;
        CrewMenu.MenuList = toolbarmain.Show();
        CrewMenu.SelectedMenuIndex = 0;

        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
        CrewMenuGeneral.AccessRights = this.ViewState;
        CrewMenuGeneral.MenuList = toolbarsub.Show();

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddFontAwesomeButton("../Crew/CrewReportDGSignOn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar1.AddFontAwesomeButton("../Crew/CrewReportDGSignOn.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuShowExcel.AccessRights = this.ViewState;
        MenuShowExcel.MenuList = toolbar1.Show();
       
        if (!IsPostBack)
        {
            //ucNatioanlity.SelectedNationality = "97";


            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            NameValueCollection nvc = Filter.CurrentSelecetedSignOnOffAcrossFleet;

            if (nvc != null)
            {
                ucManager.SelectedAddress = nvc.Get("ucManager");
                ucDate.Text = nvc.Get("ucDate");
                ucDate1.Text = nvc.Get("ucDate1");
                if (nvc.Get("ucVesselType").Equals("Dummy"))
                {
                    ucVesselType.SelectedVesselTypeValue = "";
                }
                else
                {
                    ucVesselType.SelectedVesselTypeValue = nvc.Get("ucVesselType");
                }
                if (nvc.Get("ucPrincipal").Equals("Dummy"))
                {
                    ucPrinicipal.SelectedValue = "";
                }
                else
                {
                    ucPrinicipal.SelectedValue = nvc.Get("ucPrincipal");
                }
            }


            gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        //ShowReport();
        //SetPageNavigator();

    }
    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SIGNOFF"))
        {
            Response.Redirect("../Crew/CrewReportDGSignOff.aspx", false);
        }

    }
    protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            if (!IsValidFilter(ucManager.SelectedAddress.ToString(), ucDate.Text, ucDate1.Text))
            {
                ucError.Visible = true;
                return;
            }
            else
            {
                ViewState["PAGENUMBER"] = 1;

                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ucManager", ucManager.SelectedAddress);
                criteria.Add("ucDate", ucDate.Text);
                criteria.Add("ucDate1", ucDate1.Text);
                criteria.Add("ucPrincipal", ucPrinicipal.SelectedList);
                criteria.Add("ucVesselType", ucVesselType.SelectedVesselTypeValue);

                Filter.CurrentSelecetedSignOnOffAcrossFleet = criteria;

                ShowReport();
                gvCrew.Rebind();

            }
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
                ucManager.SelectedAddress = "";
                ucDate.Text = "";
                ucDate1.Text = DateTime.Now.ToShortDateString();

                ShowReport();
                gvCrew.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROW", "FLDEMPLOYEENAME", "FLDINDOSNO", "FLDSEAMANBOOKNO", "FLDGRANKCODE", "FLDVESSELNAME", "FLDDGFLAGCODE", "FLDSIGNONDATE", "FLDDATEOFSIGNOFF", "FLDDATEOFCOMPLETION", "FLDREMARKS" };
        string[] alCaptions = { "Sr.No", "Name of Seafarer", "INDoS No", " CDC No.", " Rank", "Name of Vessel", "Flag of the Vessel", "Date Of Commencement of Contract", "Date Of Signing 'Off'", "Date of  Completion of Contract/Arriving India", "Remarks" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentSelecetedSignOnOffAcrossFleet;

        ds = PhoenixCrewSignOnReport.CrewDGSignOnReport(
        General.GetNullableInteger(nvc != null ? nvc["ucManager"] : ""),
            General.GetNullableString(nvc != null ? nvc["ucPrinicipal"] : ""),
            General.GetNullableString(nvc != null ? nvc["ucVesselType"] : ""),
            General.GetNullableInteger(ucNatioanlity.SelectedNationality),
            General.GetNullableDateTime(nvc != null ? nvc["ucDate"] : ""),
            General.GetNullableDateTime(nvc != null ? nvc["ucDate1"] : ""),
             sortexpression, sortdirection,
             Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvCrew.PageSize,
             ref iRowCount,
             ref iTotalPageCount);


        General.SetPrintOptions("gvCrew", "Sign On Across Fleet", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCrew.DataSource = ds;

            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
        }
        else
        {

            gvCrew.DataSource = ds;
            ViewState["ROWSINGRIDVIEW"] = 0;
        }
        gvCrew.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROW", "FLDEMPLOYEENAME", "FLDINDOSNO", "FLDSEAMANBOOKNO", "FLDGRANKCODE", "FLDVESSELNAME", "FLDDGFLAGCODE", "FLDSIGNONDATE", "FLDDATEOFSIGNOFF", "FLDDATEOFCOMPLETION", "FLDREMARKS" };
        string[] alCaptions = { "Sr.No", "Name of Seafarer", "INDoS No", " CDC No.", " Rank", "Name of Vessel", "Flag of the Vessel", "Date Of Commencement of Contract", "Date Of Signing 'Off'", "Date of  Completion of Contract/Arriving India", "Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewSignOnReport.CrewDGSignOnReport(
                   (ucManager.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucManager.SelectedAddress),
                   (ucPrinicipal.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPrinicipal.SelectedList),
                   (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                   General.GetNullableInteger(ucNatioanlity.SelectedNationality),
                   General.GetNullableDateTime(ucDate.Text),
                   General.GetNullableDateTime(ucDate1.Text),
                   sortexpression, sortdirection,
                   Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);

        //General.ShowExcel("Content-Disposition", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        Response.AddHeader("Content-Disposition", "attachment; filename=DGShippingSignOn.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>REPORT TO BE FORWARDED TO DIRECTOR SEAMENS EMLOYMENT OFFICE BY THE RECRUITMENT AND PLACEMENT AGENCIES(FORM - IIIA)</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Rule 4(3)(a) and 4(6)(a) clause viii of Form-III of notification vide G.S.R. 182(E) dated 18.3.2005 regarding M.S. (R&PS) Rules 2005</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>1,Name of RPS Agency:"+ ds.Tables[0].Rows[0]["FLDSELECTEDMANAGER"].ToString() + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>,,,,,Licence No.:</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>2,Period,From:" + ds.Tables[0].Rows[0]["FLDSELECTEDFROMDATE"].ToString() + "</td><td colspan='" + (alColumns.Length).ToString() + "'>To:"+ ds.Tables[0].Rows[0]["FLDSELECTEDTODATE"].ToString() + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>3,Details of Seafarers engaged :</td></tr>");

        Response.Write("</TABLE>");
        //General.ShowFilterCriteriaInExcel(ds, alColumns, alCaptions);
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
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
        //Response.AddHeader("Content-Disposition", "attachment; filename=DGShippingSignOn.csv");
        //Response.ContentType = "application/vnd.text";
        //Response.Write("REPORT TO BE FORWARDED TO DIRECTOR SEAMENS EMLOYMENT OFFICE BY THE RECRUITMENT AND PLACEMENT AGENCIES (FORM-IIIA)");
        //Response.Write("\r\n");
        //Response.Write("Rule 4(3)(a) and 4(6)(a) clause viii of Form-III of notification vide G.S.R. 182(E) dated 18.3.2005 regarding M.S. (R&PS) Rules 2005");
        //Response.Write("\r\n");
        //Response.Write("\r\n");
        //Response.Write("1,Name of RPS Agency:,,");
        //Response.Write(ds.Tables[0].Rows[0]["FLDSELECTEDMANAGER"].ToString());
        //Response.Write(",,,,,Licence No.:");
        //Response.Write("\r\n");
        //Response.Write("\r\n");
        //Response.Write("2,Period,From:," + ds.Tables[0].Rows[0]["FLDSELECTEDFROMDATE"].ToString() + ",,,,,To:," + ds.Tables[0].Rows[0]["FLDSELECTEDTODATE"].ToString());
        //Response.Write("\r\n");
        //Response.Write("\r\n");
        //Response.Write("3,Details of Seafarers engaged :,");
        //Response.Write("\r\n");
        //Response.Write("\r\n");
        //for (int i = 0; i < alCaptions.Length; i++)
        //{
        //    Response.Write(alCaptions[i] + ",");
        //}
        //Response.Write("\r\n");
        //foreach (DataRow dr in ds.Tables[0].Rows)
        //{
        //    for (int i = 0; i < alColumns.Length; i++)
        //    {
        //        Response.Write(dr[alColumns[i]] + ",");
        //    }
        //    Response.Write("\r\n");
        //}
        //Response.End();
    }


    public bool IsValidFilter(string managerid, string fromdate, string todate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridview = gvCrew;

        if (managerid.Equals("") || managerid.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Manager is Required";
        }
        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From Date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To Date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }
        return (!ucError.IsError);

    }

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName == "Page")
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

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
                && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
                LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
                lbr.Attributes.Add("onclick", "javascript:parent.openNewWindow('CrewSignOn','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "&familyid=" + drv["FLDFAMILYID"].ToString() + "'); return false;");
            }
        }
    }
}
