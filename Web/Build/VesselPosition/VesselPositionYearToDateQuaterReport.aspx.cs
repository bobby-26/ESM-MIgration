using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselPosition;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Linq;
using OfficeOpenXml.Drawing;
using System.Web;
using Telerik.Web.UI;

public partial class VesselPositionYearToDateQuaterReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
           
        
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                for (int i = 2005; i <= DateTime.Now.Year; i++)
                {
                    ddlYear.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                }
              //  ddlYear.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

                ddlYear.SelectedValue = DateTime.Now.Year.ToString();

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                }



                UcVessel.DataBind();
                UcVessel.bind();
                gvMenuYeartodatequaterreport.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }

            PhoenixToolbar toolbartap = new PhoenixToolbar();
            toolbartap.AddButton("ESI", "ESI");
            toolbartap.AddButton("Quarterly EEOI", "EEOI");
            toolbartap.AddButton("BDN", "BDN");
            toolbartap.AddButton("Chart", "CHART");
            MenuTab.AccessRights = this.ViewState;
            MenuTab.MenuList = toolbartap.Show();
            MenuTab.SelectedMenuIndex = 1;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionYearToDateQuaterReport.aspx", "Export Report", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode==0)
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','"+Session["sitepath"]+"/VesselPosition/VesselPositionYearToDateTargetEEOI.aspx?VesselId=" + UcVessel.SelectedVessel+"')", "Set Target EEOI", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            //toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionYearToDateQuaterReport.aspx", "Breakup", "<i class=\"fas fa-search\"></i>", "BREAKUP");
            MenuYeartodatequaterreport.AccessRights = this.ViewState;
            MenuYeartodatequaterreport.MenuList = toolbar.Show();

           // ScriptManager.GetCurrent(this).RegisterPostBackControl(MenuYeartodatequaterreport);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("BDN"))
        {
            Response.Redirect("../VesselPosition/VesselPositionBunkerReceiptList.aspx");
        }

        if (CommandName.ToUpper().Equals("CHART"))
        {
            Response.Redirect("../VesselPosition/VesselPositionESIChart.aspx");
        }
        if (CommandName.ToUpper().Equals("ESI"))
            Response.Redirect("../VesselPosition/VesselPositionESIRegister.aspx");
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string vesselid;
        vesselid = UcVessel.SelectedVessel;

        DataSet ds = PhoenixVesselPositionYearToDateQuaterReport.YearToDateQuaterReportSearch(
            General.GetNullableInteger(vesselid), int.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(ddlYear.SelectedValue), General.GetNullableInteger(ddlPeriod.SelectedValue));
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            string path = HttpContext.Current.Server.MapPath("~\\Template\\VesselPosition"+ @"\\EEOI_Template.xlsx");

            var file = new FileInfo(path);

            if (file.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                   // ExcelWorksheet ws = excelPackage.Workbook.Worksheets.First();
                    var wb = excelPackage.Workbook;

                    wb.Names["VesselName"].Value = UcVessel.SelectedVesselName;
                    wb.Names["Year"].Value = ddlYear.SelectedValue.ToString();

                    
                    wb.Names["qtr_first"].Value = "FALSE";
                    wb.Names["qtr_second"].Value = "FALSE";
                    wb.Names["qtr_third"].Value = "FALSE";
                    wb.Names["qtr_fourth"].Value = "FALSE";

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        wb.Names["EEOI_Target"].Value = dt.Rows[0]["FLDTARGETEEOI"].ToString();
                    }
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (dt.Rows[i]["FLDQUATER"].ToString() == "1")
                        {
                            wb.Names["Jan_Mar_EEOI_Achived"].Value = dt.Rows[i]["FLDACTUALEEOI"].ToString();
                            wb.Names["Jan_Mar_Remark"].Value = dt.Rows[i]["FLDNONCOMPLIANCERESON"].ToString();
                            wb.Names["Jan_Mar_Followed"].Value = dt.Rows[i]["FLDEMISSIONREDUCTIONMEASUREFOLLOWEDCODE"].ToString();
                            wb.Names["Jan_Mar_Propossed"].Value = dt.Rows[i]["FLDEMISSIONREDUCTIONMEASUREPROPOSSEDCODE"].ToString();
                            wb.Names["qtr_first"].Value = "TRUE";
                        }
                        if (dt.Rows[i]["FLDQUATER"].ToString() == "2")
                        {
                            wb.Names["Jan_Jun_EEOI_Achived"].Value = dt.Rows[i]["FLDACTUALEEOI"].ToString();
                            wb.Names["Jan_Jun_Remark"].Value = dt.Rows[i]["FLDNONCOMPLIANCERESON"].ToString();
                            wb.Names["Jan_Jun_Followed"].Value = dt.Rows[i]["FLDEMISSIONREDUCTIONMEASUREFOLLOWEDCODE"].ToString();
                            wb.Names["Jan_Jun_Propossed"].Value = dt.Rows[i]["FLDEMISSIONREDUCTIONMEASUREPROPOSSEDCODE"].ToString();
                            wb.Names["qtr_second"].Value = "TRUE";
                        }

                        if (dt.Rows[i]["FLDQUATER"].ToString() == "3")
                        {
                            wb.Names["Jan_Sept_EEOI_Achived"].Value = dt.Rows[i]["FLDACTUALEEOI"].ToString();
                            wb.Names["Jan_Sep_Remark"].Value = dt.Rows[i]["FLDNONCOMPLIANCERESON"].ToString();
                            wb.Names["Jan_Sep_Followed"].Value = dt.Rows[i]["FLDEMISSIONREDUCTIONMEASUREFOLLOWEDCODE"].ToString();
                            wb.Names["Jan_Sep_Propossed"].Value = dt.Rows[i]["FLDEMISSIONREDUCTIONMEASUREPROPOSSEDCODE"].ToString();
                            wb.Names["qtr_third"].Value = "TRUE";
                        }

                        if (dt.Rows[i]["FLDQUATER"].ToString() == "4")
                        {
                            wb.Names["Jan_Dec_EEOI_Achived"].Value = dt.Rows[i]["FLDACTUALEEOI"].ToString();
                            wb.Names["Jan_Dec_Remark"].Value = dt.Rows[i]["FLDNONCOMPLIANCERESON"].ToString();
                            wb.Names["Jan_Dec_Followed"].Value = dt.Rows[i]["FLDEMISSIONREDUCTIONMEASUREFOLLOWED"].ToString();
                            wb.Names["Jan_Dec_Propossed"].Value = dt.Rows[i]["FLDEMISSIONREDUCTIONMEASUREPROPOSSED"].ToString();
                            wb.Names["qtr_fourth"].Value = "TRUE";
                        }
                    }

                    //excelPackage.Workbook.Properties.Title = "Attempts";
                    this.Response.ClearHeaders();
                    this.Response.ClearContent();
                    this.Response.Clear();
                    this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    this.Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}", "YTDQuarterlyEEOI.xlsx"));
                    this.Response.BinaryWrite(excelPackage.GetAsByteArray());
                    this.Response.Flush();
                    this.Response.Close();
                }
            }
        }
    }

    protected void MenuNRRangeConfig_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("BREAKUP"))
        {
            string script = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/VesselPosition/VesselPositionYearToDateBreakup.aspx?VesselId=" + UcVessel.SelectedVessel + "&Year=" + ddlYear.SelectedValue.ToString() + "&Quarter=" + ddlPeriod.SelectedValue + "',false,400,200)";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);
        }
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

   

    protected void gvMenuYeartodatequaterreport_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionYearToDateQuaterReport.DeleteYearToDateQuaterReport(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblytdreviewid")).Text));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("RECALCULATE"))
            {
                PhoenixVesselPositionYearToDateQuaterReport.RecalculateQuaterReport(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblytdachivedid")).Text));
                ucStatus.Text = "Re-Calculated Successfully.";
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("Page"))
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

    protected void gvMenuYeartodatequaterreport_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            Guid? ytdachivedid, ytdreviewid;

            ytdachivedid = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblytdachivedidEdit")).Text);
            ytdreviewid = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblytdreviewidEdit")).Text);

            CheckBoxList chkallowed = (CheckBoxList)e.Item.FindControl("txtMethodFollowedEdit");
            string ActionAllowedId = "";
            foreach (ListItem li in chkallowed.Items)
            {
                if (li.Selected)
                {
                    ActionAllowedId += li.Value + ",";
                }
            }

            CheckBoxList chkpropossed = (CheckBoxList)e.Item.FindControl("txtMethodPropossedEdit");
            string ActionpropossedId = "";
            foreach (ListItem li in chkpropossed.Items)
            {
                if (li.Selected)
                {
                    ActionpropossedId += li.Value + ",";
                }
            }
            if (ytdreviewid == null)
            {
                PhoenixVesselPositionYearToDateQuaterReport.InsertYearToDateQuaterReport(
                    General.GetNullableInteger(UcVessel.SelectedVessel),
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblytdachivedidEdit")).Text),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtReasonEdit")).Text),
                    General.GetNullableString(ActionAllowedId),
                     General.GetNullableString(ActionpropossedId));
            }
            if (ytdreviewid != null)
            {
                PhoenixVesselPositionYearToDateQuaterReport.UpdateYearToDateQuaterReport(
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblytdreviewidEdit")).Text),
                    General.GetNullableString(((RadTextBox)e.Item.FindControl("txtReasonEdit")).Text),
                    General.GetNullableString(ActionAllowedId),
                     General.GetNullableString(ActionpropossedId));
            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMenuYeartodatequaterreport_ItemDataBound(Object sender,GridItemEventArgs e)
    {
         if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            DataSet Ds = PhoenixVesselPositionYearToDateQuaterReport.EEOIActivityList();

            CheckBoxList chlallowed = (CheckBoxList)e.Item.FindControl("txtMethodFollowedEdit");
            if (chlallowed != null)
            {
                
                chlallowed.DataSource = Ds;
                chlallowed.DataTextField = "FLDCODE";
                chlallowed.DataValueField = "FLDEEOIACTIVITYBESTPRACTICEID";
                chlallowed.DataBind();

                foreach (ListItem li in chlallowed.Items)
                {
                    string[] slist = drv["FLDEMISSIONREDUCTIONMEASUREFOLLOWEDLIST"].ToString().Split(',');
                    foreach (string s in slist)
                    {
                        if (li.Value.Equals(s))
                        {
                            li.Selected = true;
                        }
                    }
                }
            }
            CheckBoxList chlpropossed = (CheckBoxList)e.Item.FindControl("txtMethodPropossedEdit");
            if (chlpropossed != null)
            {

                chlpropossed.DataSource = Ds;
                chlpropossed.DataTextField = "FLDCODE";
                chlpropossed.DataValueField = "FLDEEOIACTIVITYBESTPRACTICEID";
                chlpropossed.DataBind();

                foreach (ListItem li in chlpropossed.Items)
                {
                    string[] slist = drv["FLDEMISSIONREDUCTIONMEASUREPROPOSSEDLIST"].ToString().Split(',');
                    foreach (string s in slist)
                    {
                        if (li.Value.Equals(s))
                        {
                            li.Selected = true;
                        }
                    }
                }
            }
            LinkButton lb = (LinkButton)e.Item.FindControl("cmdDelete");
            if (lb != null && General.GetNullableGuid(drv["FLDYTDEEOIACHIVEDREVIEWID"].ToString()) == null)
            {
                lb.Visible = false;
            }
            LinkButton lbb = (LinkButton)e.Item.FindControl("cmdBreakup");
            if (lbb != null)
            {
                lbb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/VesselPosition/VesselPositionYearToDateBreakup.aspx?VesselId=" + UcVessel.SelectedVessel + "&Year=" + ddlYear.SelectedValue.ToString() + "&Quarter=" + ddlPeriod.SelectedValue + "',false,1000,500)");
                
            }
        }
    }
    

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvMenuYeartodatequaterreport_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMenuYeartodatequaterreport.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string vesselid;
        vesselid = UcVessel.SelectedVessel;

        DataSet ds = PhoenixVesselPositionYearToDateQuaterReport.YearToDateQuaterReportSearch(
            General.GetNullableInteger(vesselid), int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvMenuYeartodatequaterreport.PageSize, ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(ddlYear.SelectedValue), General.GetNullableInteger(ddlPeriod.SelectedValue));

        gvMenuYeartodatequaterreport.DataSource = ds;

        gvMenuYeartodatequaterreport.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvMenuYeartodatequaterreport.SelectedIndexes.Clear();
        gvMenuYeartodatequaterreport.EditIndexes.Clear();
        gvMenuYeartodatequaterreport.DataSource = null;
        gvMenuYeartodatequaterreport.Rebind();
    }

    protected void UcVessel_TextChangedEvent(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void ddlYear_TextChanged(object sender, EventArgs e)
    {
        Rebind();
    }
}
