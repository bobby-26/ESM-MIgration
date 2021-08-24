using System;
using System.Collections.Specialized;
using System.Collections;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionDashBoardDeficiencyOfficeList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDashBoardDeficiencyOfficeList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvDeficiency')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','', '" + Session["sitepath"] + "/Inspection/InspectionDashBoardDeficiencyOfficeListFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDashBoardDeficiencyOfficeList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionBulkDeficiencyClosure.aspx?'); return true;", "Bulk Closure", "<i class=\"fas fas fa-tasks\"></i>", "BULKOFFICECOMMENTS");
            }
            MenuDeficiency.AccessRights = this.ViewState;
            MenuDeficiency.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                InspectionFilter.CurrentDeficiencyDashboardFilter = null;

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;

                ViewState["STATUS"] = "";

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                if (Request.QueryString["STATUS"] != null && Request.QueryString["STATUS"].ToString() != string.Empty)
                    ViewState["STATUS"] = Request.QueryString["STATUS"].ToString();

                gvDeficiency.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void DeficiencyGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDREFERENCENUMBER", "FLDCOMPANYSHORTCODE", "FLDISSUEDDATE", "FLDSOURCE", "FLDSOURCEREFERENCENUMBER",
                                     "FLDTYPE", "FLDDEFICIENCYCATEGORY","FLDCHAPTERNAME", "FLDKEYNAME", "FLDISRCAREQUIREDYN", "FLDISRCACOMPLETEDYN", "FLDSTATUS" };
            string[] alCaptions = { "Reference Number", "Company", "Issued Date", "Source", "Source Ref", "Type", "Deficiency Category",
                                      "Chapter", "PSC Action Code / VIR Condition", "RCA required", "RCA completed", "Status" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentDeficiencyFilter;
            NameValueCollection Dashboardnvc = InspectionFilter.CurrentDeficiencyDashboardFilter;
            DataSet ds;

            ds = PhoenixInspectionOfficeDashboard.DashBoardDeficiencyOffoiceSearch
                  (General.GetNullableString(ViewState["STATUS"].ToString())
                     , sortexpression
                     , sortdirection
                     , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                     , iRowCount
                     , ref iRowCount
                     , ref iTotalPageCount
                     , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucCompany"] : null)
                     , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["uccategory"] : null)
                     , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ucinspection"] : null)
                     , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ucchapter"] : null)
                     , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucdefeciencytype"] : null)
                     , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucdefeciencycategory"] : null)
                     , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucsource"] : null)
                     , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["ucsourceno"] : null)
                     , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["ucreferenceno"] : null)
                     , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

            General.ShowExcel("OfficeDeficiencies", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Deficiency_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if(CommandName.ToUpper().Equals("CLEAR"))
            {
                InspectionFilter.CurrentDeficiencyDashboardFilter = null;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDREFERENCENUMBER", "FLDCOMPANYSHORTCODE", "FLDISSUEDDATE", "FLDSOURCE", "FLDSOURCEREFERENCENUMBER",
                                     "FLDTYPE", "FLDDEFICIENCYCATEGORY","FLDCHAPTERNAME", "FLDKEYNAME", "FLDISRCAREQUIREDYN", "FLDISRCACOMPLETEDYN", "FLDSTATUS" };
            string[] alCaptions = { "Reference Number", "Company", "Issued Date", "Source", "Source Ref", "Type", "Deficiency Category",
                                      "Chapter", "PSC Action Code / VIR Condition", "RCA required", "RCA completed", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection Dashboardnvc = InspectionFilter.CurrentDeficiencyDashboardFilter;

            DataSet ds;

            ds = PhoenixInspectionOfficeDashboard.DashBoardDeficiencyOffoiceSearch
                  (General.GetNullableString(ViewState["STATUS"].ToString())
                     , sortexpression
                     , sortdirection
                     , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                     , gvDeficiency.PageSize
                     , ref iRowCount
                     , ref iTotalPageCount
                     , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucCompany"] : null)
                     , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["uccategory"] : null)
                     , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ucinspection"] : null)
                     , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ucchapter"] : null)
                     , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucdefeciencytype"] : null)
                     , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucdefeciencycategory"] : null)
                     , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucsource"] : null)
                     , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["ucsourceno"] : null)
                     , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["ucreferenceno"] : null)
                     , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

            General.SetPrintOptions("gvDeficiency", "Office Deficiencies", alCaptions, alColumns, ds);

            gvDeficiency.DataSource = ds;
            gvDeficiency.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Filter.CurrentSelectedDeficiency == null)
                {
                    Filter.CurrentSelectedDeficiency = ds.Tables[0].Rows[0][0].ToString();
                    gvDeficiency.SelectedIndexes.Clear();
                }
                SetRowSelection();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                Filter.CurrentSelectedDeficiency = null;
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDeficiency_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
                RadLabel lblDeficiencyType = ((RadLabel)e.Item.FindControl("lblDeficiencyType"));
                RadLabel lblVesselid = ((RadLabel)e.Item.FindControl("lblVesselid"));
                //if (lblDeficiencyType != null && lblDeficiencyType.Text == "1")
                //    Response.Redirect("../Inspection/InspectionAuditDirectNonConformityMaster.aspx?DashboardYN=1&currenttab=DET&REVIEWDNC=" + Filter.CurrentSelectedDeficiency + "&Vesselid=" + lblVesselid.Text, false);
                //else if (lblDeficiencyType != null && lblDeficiencyType.Text == "2")
                //    Response.Redirect("../Inspection/InspectionDirectObservationMaster.aspx?DashboardYN=1&currenttab=DET&DIRECTOBSERVATIONID=" + Filter.CurrentSelectedDeficiency + "&Vesselid=" + lblVesselid.Text, false);
            }
            if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }
            if (e.CommandName.ToUpper().Equals("CANCELDEFICIENCY"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }
            if (e.CommandName.ToUpper().Equals("SHOWSOURCE"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }
            if (e.CommandName.ToUpper().Equals("CORRECTIVEACTIONS"))
            {
                string Deficiencyid = ((RadLabel)e.Item.FindControl("lblDeficiencyid")).Text;
                Response.Redirect("../Inspection/InspectionOfficeDeficiencyCARList.aspx?deficiencyid=" + Deficiencyid, false);
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }
            if (e.CommandName.ToUpper().Equals("DEFICIENCYREPORT"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }
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

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblDeficiencyid = (RadLabel)gvDeficiency.Items[rowindex].FindControl("lblDeficiencyid");
            if (lblDeficiencyid != null)
            {
                Filter.CurrentSelectedDeficiency = lblDeficiencyid.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeficiency_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            string Status = ViewState["STATUS"].ToString();

            if (e.Item is GridHeaderItem)
            {
                RadCheckBox chkAll = (RadCheckBox)e.Item.FindControl("chkAllRemittance");
                if (chkAll != null)
                {
                    if (Status == "CMP")
                    {
                        chkAll.Enabled = true;
                    }
                    else
                    {
                        chkAll.Enabled = false;
                    }
                }
            }
            if (e.Item is GridDataItem)
            {
                //if (!e.Item.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.RowState.Equals(DataControlRowState.Edit))
                //{

                RadCheckBox chkSelect = (RadCheckBox)e.Item.FindControl("chkSelect");
                if (chkSelect != null)
                {
                    if (Status == "CMP")
                    {
                        chkSelect.Enabled = true;
                    }
                    else
                    {
                        chkSelect.Enabled = false;
                    }
                }

                RadLabel lblDeficiencyType = ((RadLabel)e.Item.FindControl("lblDeficiencyType"));
                LinkButton lnkRefNumber = (LinkButton)e.Item.FindControl("lnkRefNumber");

                if (lnkRefNumber != null)
                {
                    RadLabel lblDeficiencyids = (RadLabel)e.Item.FindControl("lblDeficiencyid");

                    if (lblDeficiencyType != null && lblDeficiencyType.Text == "1")
                    {
                        lnkRefNumber.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionAuditDirectNonConformityMaster.aspx?currenttab=DET&DashboardYN=1&&REVIEWDNC=" + lblDeficiencyids.Text + "', '450%', '250%'); return true; ");
                    }
                    else if (lblDeficiencyType != null && lblDeficiencyType.Text == "2")
                    {
                        lnkRefNumber.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionDirectObservationMaster.aspx?currenttab=DET&DashboardYN=1&DIRECTOBSERVATIONID=" + lblDeficiencyids.Text + "', '450%', '250%'); return true; ");
                    }
                }

                DataRowView drv = (DataRowView)e.Item.DataItem;
                LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");
                RadLabel lblDeficiencyid = (RadLabel)e.Item.FindControl("lblDeficiencyid");
                if (cmdCancel != null && lblDeficiencyid != null)
                {
                    if (drv["FLDDEFICIENCYTYPE"].ToString().Equals("1"))
                        cmdCancel.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionCancelReasonUpdate.aspx?REFERENCEID=" + lblDeficiencyid.Text + "&TYPE=3','medium'); return true;");
                    if (drv["FLDDEFICIENCYTYPE"].ToString().Equals("2"))
                        cmdCancel.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionCancelReasonUpdate.aspx?REFERENCEID=" + lblDeficiencyid.Text + "&TYPE=4','medium'); return true;");
                }

                RadLabel lblStatusid = (RadLabel)e.Item.FindControl("lblStatusid");
                LinkButton cd = (LinkButton)e.Item.FindControl("cmdComplete");
                LinkButton cl = (LinkButton)e.Item.FindControl("cmdClose");
                if (lblStatusid != null && lblStatusid.Text.Equals(PhoenixCommonRegisters.GetHardCode(1, 146, "OPN")))
                {
                    if (cd != null) cd.Visible = true;
                    if (cl != null) cl.Visible = false;
                }
                if (lblStatusid != null && lblStatusid.Text.Equals(PhoenixCommonRegisters.GetHardCode(1, 146, "CMP")))
                {
                    if (cd != null) cd.Visible = false;
                    if (cl != null) cl.Visible = true;
                }
                if (lblStatusid != null && lblStatusid.Text.Equals(PhoenixCommonRegisters.GetHardCode(1, 146, "CLD")))
                {
                    if (cd != null) cd.Visible = false;
                    if (cl != null) cl.Visible = false;
                }
                HtmlGenericControl html = new HtmlGenericControl();
                LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
                RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
                if (att != null)
                {
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                    if (drv["FLDISATTACHMENT"].ToString() == "0")
                    {
                        html.InnerHtml = "<span class=\"icon\"><i class=\"fa-paperclip-na\"></i></span>";
                        att.Controls.Add(html);
                    }
                    else
                    {
                        html.InnerHtml = "<span class=\"icon\"><i class=\"fas fa-paperclip\"></i></span>";
                        att.Controls.Add(html);
                    }
                    att.Attributes.Add("onclick", "openNewWindow('att','', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                            + PhoenixModule.QUALITY + "&type=INSPECTIONDEFICIENCY&cmdname=DEFICIENCYUPLOAD&VESSELID=" + lblVesselid.Text + "'); return true;");
                }
                //RadLabel lblDeficiencyid = (RadLabel)e.Item.FindControl("lblDeficiencyid");

                //LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                //if (eb != null) eb.Attributes.Add("onclick", "openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionDeficiencyEdit.aspx?DEFICIENCYID=" + lblDeficiencyid.Text + "'); return true;");

                //LinkButton cmdCorrectiveActions = (LinkButton)e.Item.FindControl("cmdCorrectiveActions");
                //if (cmdCorrectiveActions != null)
                //{
                //    cmdCorrectiveActions.Visible = SessionUtil.CanAccess(this.ViewState, cmdCorrectiveActions.CommandName);
                //    if (drv["FLDCARPARRECORDEDYN"].ToString() == "0")
                //    {
                //        cmdCorrectiveActions.Controls.Remove(html);
                //        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-eye\"></i></span>";
                //        cmdCorrectiveActions.Controls.Add(html);
                //    }
                //}
                //}

                LinkButton lnkSourceRefNo = (LinkButton)e.Item.FindControl("lnkSourceRefNo");
                RadLabel lblRaisedfrom = (RadLabel)e.Item.FindControl("lblRaisedfrom");
                RadLabel lblSourceId = (RadLabel)e.Item.FindControl("lblSourceId");
                RadLabel lblVesselName = (RadLabel)e.Item.FindControl("lblVesselCode");
                if (lnkSourceRefNo != null)
                {
                    if (lblRaisedfrom.Text == "1") // Audit/Inspection
                    {
                        if (lblVesselName.Text.ToUpper().Equals("OFFICE"))
                        {
                            lnkSourceRefNo.Attributes.Add("onclick", "openNewWindow('source','', '" + Session["sitepath"] + "/Inspection/InspectionAuditOfficeRecordGeneral.aspx?AUDITSCHEDULEID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                        }
                        else
                        {
                            lnkSourceRefNo.Attributes.Add("onclick", "openNewWindow('source','', '" + Session["sitepath"] + "/Inspection/InspectionAuditRecordGeneral.aspx?AUDITSCHEDULEID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                        }
                    }
                    if ((lblDeficiencyType.Text == "1" && lblRaisedfrom.Text == "4") ||
                        (lblDeficiencyType.Text == "2" && lblRaisedfrom.Text == "2")) //Vetting
                    {
                        lnkSourceRefNo.Attributes.Add("onclick", "openNewWindow('source',''/Inspection/InspectionVettingReport.aspx?SCHEDULEID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                    }
                    if ((lblDeficiencyType.Text == "1" && lblRaisedfrom.Text == "3") ||
                        (lblDeficiencyType.Text == "2" && lblRaisedfrom.Text == "4")) //Open Reports
                    {
                        lnkSourceRefNo.Attributes.Add("onclick", "openNewWindow('source','', '" + Session["sitepath"] + "/Inspection/InspectionDirectIncidentGeneral.aspx?directincidentid=" + lblSourceId.Text + "&viewonly=1'); return true;");
                    }
                    if ((lblDeficiencyType.Text == "1" && lblRaisedfrom.Text == "2") ||
                        (lblDeficiencyType.Text == "2" && lblRaisedfrom.Text == "3")) //Direct
                    {
                        lnkSourceRefNo.Attributes.Add("onclick", "openNewWindow('source','', '" + Session["sitepath"] + "/Inspection/InspectionDeficiencyEdit.aspx?DEFICIENCYID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                    }
                }

                UserControlToolTip ucToolTipInspectionRefNo = (UserControlToolTip)e.Item.FindControl("ucToolTipInspectionRefNo");
                if (ucToolTipInspectionRefNo != null)
                {
                    ucToolTipInspectionRefNo.Position = ToolTipPosition.TopCenter;
                    ucToolTipInspectionRefNo.TargetControlId = lnkSourceRefNo.ClientID;
                }

                RadLabel lblDeficiencyyid = (RadLabel)e.Item.FindControl("lblDeficiencyid");
                //LinkButton cmdCar = (LinkButton)e.Item.FindControl("cmdCorrectiveActions");
                RadLabel lblVesselidd = (RadLabel)e.Item.FindControl("lblVesselid");
                //if (cmdCar != null)
                //{
                //    cmdCar.Visible = SessionUtil.CanAccess(this.ViewState, cmdCar.CommandName);
                //}

                LinkButton Cancel = (LinkButton)e.Item.FindControl("cmdCancel");
                if (Cancel != null)
                {
                    Cancel.Visible = SessionUtil.CanAccess(this.ViewState, Cancel.CommandName);
                }

                LinkButton cmdAtt = (LinkButton)e.Item.FindControl("cmdAtt");
                if (cmdAtt != null)
                {
                    cmdAtt.Visible = SessionUtil.CanAccess(this.ViewState, cmdAtt.CommandName);
                }

                LinkButton cmdDeficiencyReport = (LinkButton)e.Item.FindControl("cmdDeficiencyReport");
                if (cmdDeficiencyReport != null)
                {
                    cmdDeficiencyReport.Attributes.Add("onclick", "Openpopup('Report', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=DEFICIENCYREPORT&deficiencyid=" + lblDeficiencyyid.Text + "&vesselid=" + lblVesselidd.Text + "&showmenu=0&showexcel=NO');return true;");
                }

                if (cmdDeficiencyReport != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdDeficiencyReport.CommandName)) cmdDeficiencyReport.Visible = false;
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
        try
        {
            Rebind();
            SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        gvDeficiency.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvDeficiency.Items)
        {
            if (item.GetDataKeyValue("FLDDEFICIENCYID").ToString() == Filter.CurrentSelectedDeficiency.ToString())
            {
                gvDeficiency.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void gvDeficiency_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDeficiency.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeficiency_SortCommand(object sender, GridSortCommandEventArgs e)
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
    }

    protected void gvDeficiency_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel lblDeficiencyType = ((RadLabel)e.Item.FindControl("lblDeficiencyType"));
            RadLabel lblDeficiencyid = ((RadLabel)e.Item.FindControl("lblDeficiencyid"));

            if (lblDeficiencyType != null && lblDeficiencyType.Text == "1")
            {
                PhoenixInspectionAuditDirectNonConformity.DeleteAuditDirectNonConformity(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((RadLabel)e.Item.FindControl("lblDeficiencyid")).Text));
            }
            else if (lblDeficiencyType != null && lblDeficiencyType.Text == "2")
            {
                PhoenixInspectionObservation.DeleteInspectionDirectObservation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((RadLabel)e.Item.FindControl("lblDeficiencyid")).Text));
            }
            Filter.CurrentSelectedDeficiency = null;
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvDeficiency.SelectedIndexes.Clear();
        gvDeficiency.EditIndexes.Clear();
        gvDeficiency.DataSource = null;
        gvDeficiency.Rebind();
    }

    protected void gvDeficiency_PreRender(object sender, EventArgs e)
    {
        SetRowSelection();
    }
    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvDeficiency$ctl00$ctl02$ctl01$chkAllRemittance")
        {
            GridHeaderItem headerItem = gvDeficiency.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            RadCheckBox chkAll = (RadCheckBox)headerItem.FindControl("chkAllRemittance");
            foreach (GridDataItem row in gvDeficiency.MasterTableView.Items)
            {
                RadCheckBox cbSelected = (RadCheckBox)row.FindControl("chkSelect");
                if (cbSelected != null)
                {
                    if (chkAll.Checked == true)
                    {
                        cbSelected.Checked = true;
                    }
                    else
                    {
                        cbSelected.Checked = false;
                    }
                }
            }
        }
    }
    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedPvs = new ArrayList();
        Guid index = new Guid();
        foreach (GridDataItem gvrow in gvDeficiency.Items)
        {
            bool result = false;
            index = new Guid(gvrow.GetDataKeyValue("FLDDEFICIENCYID").ToString());

            if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;// ((RadCheckBox)gvrow.FindControl("chkSelect")).Checked;
            }

            // Check in the Session
            if (Filter.CurrentSelectedBulkDeficiencies != null)
            {
                SelectedPvs = (ArrayList)Filter.CurrentSelectedBulkDeficiencies;
            }
            if (result)
            {
                if (!SelectedPvs.Contains(index))
                    SelectedPvs.Add(index);
            }
            else
                SelectedPvs.Remove(index);
        }
        if (SelectedPvs != null && SelectedPvs.Count > 0)
            Filter.CurrentSelectedBulkDeficiencies = SelectedPvs;
    }
}

