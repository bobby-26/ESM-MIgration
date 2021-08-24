using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewNewApplicantAppraisal : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["VSLID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                if (Request.QueryString["vslid"] != null)
                    ViewState["VSLID"] = Request.QueryString["vslid"];

                SetEmployeePrimaryDetails();
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                gvAQ.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantAppraisal.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAQ')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            if (!String.IsNullOrEmpty(ViewState["VSLID"].ToString()))
                toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantAppraisal.aspx", "Add New Appraisal", "<i class=\"fa fa-plus-circle\"></i>", "ADD");            
            else if (String.IsNullOrEmpty(ViewState["VSLID"].ToString()))
                toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantAppraisal.aspx", "Add New Appraisal", "<i class=\"fa fa-plus-circle\"></i>", "CREWADD");

            CrewAppraisal.AccessRights = this.ViewState;
            CrewAppraisal.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();

            BindData();                        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvAQ.Rebind();
    }

    protected void CrewAppraisal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
                Filter.CurrentAppraisalSelection = null;
                Response.Redirect("CrewNewApplicantAppraisalActivity.aspx", false);
            }
            if (CommandName.ToUpper().Equals("CREWADD"))
            {
                Filter.CurrentAppraisalSelection = null;
                Response.Redirect("CrewNewApplicantAppraisalActivity.aspx", false);
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

        string[] alColumns = { "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE", "FLDAPPRAISALDATE", "FLDOCCASSIONFORREPORT" };
        string[] alCaptions = { "Vessel Name", "From Date", "To Date", "Appraisal Date", "Occasion for Report" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixCrewAppraisal.SearchAppraisal(
                General.GetNullableInteger(Filter.CurrentNewApplicantSelection)
               , General.GetNullableInteger(Request.QueryString["vslid"])
               , sortdirection
               , int.Parse(ViewState["PAGENUMBER"].ToString())
               , gvAQ.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               );

        Response.AddHeader("Content-Disposition", "attachment; filename=NewApplicantAppraisal.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>New Applicant - Appraisal</h3></td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtEmployeeMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtEmployeeLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();

                ViewState["RANKID"] = dt.Rows[0]["FLDRANK"].ToString();

                string Rcategory = null;

                PhoenixCrewAppraisalProfile.GetRankCategory(int.Parse(ViewState["RANKID"].ToString()), ref Rcategory);

                if (Rcategory == System.DBNull.Value.ToString())
                    Rcategory = "0";

                ViewState["Rcategory"] = Rcategory.ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvAQ_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToString().ToUpper() == "SORT")
            return;

        GridView _gridView = (GridView)sender;
        int nCurrentRow = int.Parse(e.CommandArgument.ToString());
       
        if (e.CommandName.ToString().ToUpper() == "SELECT")
        {
            Filter.CurrentAppraisalSelection = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAppraisalId")).Text;
            Response.Redirect("CrewNewApplicantAppraisalActivity.aspx", false);
        }
    }

    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? sortdirection = 1; //DEFAULT DESC SORT
        string[] alColumns = { "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE", "FLDAPPRAISALDATE", "FLDOCCASSIONFORREPORT" };
        string[] alCaptions = { "Vessel Name", "From Date", "To Date", "Appraisal Date", "Occasion for Report" };
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {

            DataSet ds = PhoenixCrewAppraisal.SearchAppraisal(
                General.GetNullableInteger(Filter.CurrentNewApplicantSelection)
               , General.GetNullableInteger(Request.QueryString["vslid"])
               , sortdirection
               , int.Parse(ViewState["PAGENUMBER"].ToString())
               , gvAQ.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               );

            General.SetPrintOptions("gvAQ", "New Applicant - Appraisal", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvAQ.DataSource = ds;
                gvAQ.VirtualItemCount = iRowCount;

                //if (Filter.CurrentAppraisalSelection == null)
                //{
                //    gvAQ.SelectedIndex = 0;
                //    Filter.CurrentAppraisalSelection = ((Label)gvAQ.Rows[0].FindControl("lblAppraisalId")).Text;
                //}
            }
            else
            {               
                gvAQ.DataSource = "";
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidateAppraisal(string vessel, string fromdate, string todate, string occassion, string appraisaldate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        int result;
        DateTime resultdate;
        if (!int.TryParse(vessel, out result))
            ucError.ErrorMessage = "Vessel is required.";
        if (fromdate == null || !DateTime.TryParse(fromdate, out  resultdate))
            ucError.ErrorMessage = "From Date is required";
        if (todate == null || !DateTime.TryParse(todate, out resultdate))
            ucError.ErrorMessage = "To Date is required";
        else if (!string.IsNullOrEmpty(fromdate)
              && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }
        if (occassion.ToUpper() == "DUMMY" || occassion == "")
        {
            ucError.Text = "Please Select Occassion For Report";
        }
        if (appraisaldate == null || !DateTime.TryParse(appraisaldate, out  resultdate))
            ucError.ErrorMessage = "Appraisal Date is required";

        return (!ucError.IsError);
    }

    protected void gvAQ_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAQ.CurrentPageIndex + 1;
        BindData();

    }

    protected void gvAQ_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string appraisalid = ((RadLabel)eeditedItem.FindControl("lblAppraisalId")).Text;
                PhoenixCrewAppraisal.DeleteAppraisal(new Guid(appraisalid));
                BindData();
                gvAQ.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string appraisalid = ((RadLabel)eeditedItem.FindControl("lblAppraisalIdEdit")).Text;
                string vessel = ((UserControlCommonVessel)eeditedItem.FindControl("ddlVesselEdit")).SelectedVessel;
                string fromdate = ((UserControlDate)eeditedItem.FindControl("txtFromDateEdit")).Text;
                string todate = ((UserControlDate)eeditedItem.FindControl("txtToDateEdit")).Text;
                string appraisaldate = ((UserControlDate)eeditedItem.FindControl("txtAppraisalDateEdit")).Text;
                string occassion = ((UserControlOccassionForReport)eeditedItem.FindControl("ddlOccassionForReportedit")).SelectedOccassion;

                if (!IsValidateAppraisal(vessel, fromdate, todate, occassion, appraisaldate))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewAppraisal.UpdateAppraisal(new Guid(appraisalid)
                                                    , DateTime.Parse(fromdate)
                                                    , DateTime.Parse(todate)
                                                    , int.Parse(vessel)
                                                    , General.GetNullableDateTime(appraisaldate)
                                                    , int.Parse(occassion)
                                                    , null
                                        );
                BindData();
                gvAQ.Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "SELECT")
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                Filter.CurrentAppraisalSelection = ((RadLabel)eeditedItem.FindControl("lblAppraisalId")).Text;
                Response.Redirect("CrewNewApplicantAppraisalActivity.aspx", false);
            }
            else if (e.CommandName == "Page")
            {
                ViewState["FEPAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAQ_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");                
                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                        db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    else if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                        db.Visible = false;
                }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);

                if (drv["FLDISATTACHMENT"].ToString() == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.CREW + "&type=APPRAISAL&cmdname=NAPPRAISALUPLOAD'); return false;");
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                if (drv["FLDACTIVEYN"].ToString().Equals("0"))
                    eb.Visible = false;
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && eb.Visible)
                    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }
            UserControlOccassionForReport ddlOccassionForReportedit = (UserControlOccassionForReport)e.Item.FindControl("ddlOccassionForReportedit");

            if (ddlOccassionForReportedit != null)
                ddlOccassionForReportedit.SelectedOccassion = drv["FLDOCCASIONID"].ToString();

            UserControlCommonVessel ucVessel = (UserControlCommonVessel)e.Item.FindControl("ddlVesselEdit");

            if (ucVessel != null)
            {
                ucVessel.VesselList = PhoenixRegistersVessel.ListCommonVessels(null, null, null, null, 1, null, null, null, 1, "VSL");
                ucVessel.SelectedVessel = drv["FLDVESSELID"].ToString();
            }

        }
      
        if (e.Item is GridFooterItem)
        {
            LinkButton ad = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
        }
    }
}
