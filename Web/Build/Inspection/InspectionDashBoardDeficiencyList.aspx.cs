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
public partial class InspectionDashBoardDeficiencyList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDashBoardDeficiencyList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvDeficiency')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','', '" + Session["sitepath"] + "/Inspection/InspectionDashBoardDeficiencyListFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDashBoardDeficiencyList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionBulkDeficiencyClosure.aspx?'); return true;", "Bulk Closure", "<i class=\"fas fas fa-tasks\"></i>", "BULKOFFICECOMMENTS");
            }
            MenuDeficiency.AccessRights = this.ViewState;
            MenuDeficiency.MenuList = toolbargrid.Show();


            if (!IsPostBack)
            {
                InspectionFilter.CurrentDeficiencyDashboardFilter = null;

                ViewState["STATUS"] = "";

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["VESSELID"] = "";

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                if (Request.QueryString["STATUS"] != null && Request.QueryString["STATUS"].ToString() != string.Empty)
                    ViewState["STATUS"] = Request.QueryString["STATUS"].ToString();

                if (Request.QueryString["vslid"] != null && Request.QueryString["vslid"].ToString() != string.Empty)
                    ViewState["VESSELID"] = Request.QueryString["vslid"].ToString();

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

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Response.Redirect("../Inspection/InspectionDefeciencyFilter.aspx");
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDREFERENCENUMBER", "FLDVESSELNAME", "FLDISSUEDDATE", "FLDSOURCE", "FLDSOURCEREFERENCENUMBER",
                                     "FLDTYPE", "FLDDEFICIENCYCATEGORY", "FLDCHAPTERNAME", "FLDKEYNAME", "FLDISRCAREQUIREDYN", "FLDISRCACOMPLETEDYN", "FLDSTATUS" };
            string[] alCaptions = { "Reference Number", "Vessel", "Issued Date", "Source", "Source Ref", "Type", "Deficiency Category",
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

            NameValueCollection nvc = FilterDashboard.OfficeDashboardFilterCriteria;
            NameValueCollection Dashboardnvc = InspectionFilter.CurrentDeficiencyDashboardFilter;

            DataSet ds;

            ds = PhoenixInspectionOfficeDashboard.DashBoardDeficiencyShipSearch
                (General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? ViewState["VESSELID"].ToString() : (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null))
                 , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null))
                 , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null))
                 , General.GetNullableInteger(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["Owner"] : null))
                 , General.GetNullableString(ViewState["STATUS"].ToString())
                 , sortexpression
                 , sortdirection
                 , 1
                 , iRowCount
                 , ref iRowCount
                 , ref iTotalPageCount
                 , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["uctype"] : null)
                 , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["uccategory"] : null)
                 , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ucinspection"] : null)
                 , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ucchapter"] : null)
                 , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucdefeciencytype"] : null)
                 , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucdefeciencycategory"] : null)
                 , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucsource"] : null)
                 , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["ucsourceno"] : null)
                 , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["ucreferenceno"] : null));

            General.ShowExcel("Deficiencies", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

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

            if (CommandName.ToUpper().Equals("CLEAR"))
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

            string[] alColumns = { "FLDREFERENCENUMBER", "FLDVESSELNAME", "FLDISSUEDDATE", "FLDSOURCE", "FLDSOURCEREFERENCENUMBER",
                                     "FLDTYPE", "FLDDEFICIENCYCATEGORY", "FLDCHAPTERNAME", "FLDKEYNAME", "FLDISRCAREQUIREDYN", "FLDISRCACOMPLETEDYN", "FLDSTATUS" };
            string[] alCaptions = { "Reference Number", "Vessel", "Issued Date", "Source", "Source Ref", "Type", "Deficiency Category",
                                      "Chapter", "PSC Action Code / VIR Condition", "RCA required", "RCA completed", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = FilterDashboard.OfficeDashboardFilterCriteria;
            NameValueCollection Dashboardnvc = InspectionFilter.CurrentDeficiencyDashboardFilter;

            DataSet ds;

            ds = PhoenixInspectionOfficeDashboard.DashBoardDeficiencyShipSearch
                 (General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? ViewState["VESSELID"].ToString() : (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null))
                  , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null))
                  , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null))
                  , General.GetNullableInteger(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["Owner"] : null))
                  , General.GetNullableString(ViewState["STATUS"].ToString())
                  , sortexpression
                  , sortdirection
                  , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                  , gvDeficiency.PageSize
                  , ref iRowCount
                  , ref iTotalPageCount
                  , General.GetNullableInteger(Dashboardnvc !=null? Dashboardnvc["uctype"] : null)
                  , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["uccategory"] : null)
                  , General.GetNullableGuid(Dashboardnvc !=null ? Dashboardnvc["ucinspection"] :null)
                  , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ucchapter"] : null)
                  , General.GetNullableInteger(Dashboardnvc !=null ? Dashboardnvc["ucdefeciencytype"] : null)
                  , General.GetNullableInteger(Dashboardnvc !=null ? Dashboardnvc["ucdefeciencycategory"] : null)
                  , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucsource"] : null)
                  , General.GetNullableString(Dashboardnvc !=null ? Dashboardnvc["ucsourceno"] : null)
                  , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["ucreferenceno"] : null));

            General.SetPrintOptions("gvDeficiency", "Deficiencies", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Filter.CurrentSelectedDeficiency == null)
                {
                    Filter.CurrentSelectedDeficiency = ds.Tables[0].Rows[0][0].ToString();
                    gvDeficiency.SelectedIndexes.Clear();
                }

            }
            else
            {
                DataTable dt = ds.Tables[0];
                Filter.CurrentSelectedDeficiency = null;
            }

            gvDeficiency.DataSource = ds;
            gvDeficiency.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
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
                //    RadLabel lblDeficiencyType = ((RadLabel)e.Item.FindControl("lblDeficiencyType"));
                //    if (lblDeficiencyType != null && lblDeficiencyType.Text == "1")
                //        Response.Redirect("../Inspection/InspectionAuditDirectNonConformityMaster.aspx?currenttab=DET&REVIEWDNC=" + Filter.CurrentSelectedDeficiency, false);
                //    else if (lblDeficiencyType != null && lblDeficiencyType.Text == "2")
                //        Response.Redirect("../Inspection/InspectionDirectObservationMaster.aspx?currenttab=DET&DIRECTOBSERVATIONID=" + Filter.CurrentSelectedDeficiency, false);
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
                Response.Redirect("../Inspection/InspectionDeficiencyCARList.aspx?deficiencyid=" + Deficiencyid, false);
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

                DataRowView drv = (DataRowView)e.Item.DataItem;
                LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");
                RadLabel lblDeficiencyid = (RadLabel)e.Item.FindControl("lblDeficiencyid");
                if (cmdCancel != null && lblDeficiencyid != null)
                {
                    if (drv["FLDDEFICIENCYTYPE"].ToString().Equals("1"))
                        cmdCancel.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionCancelReasonUpdate.aspx?REFERENCEID=" + lblDeficiencyid.Text + "&TYPE=3','','450%','250%'); return true;");
                    if (drv["FLDDEFICIENCYTYPE"].ToString().Equals("2"))
                        cmdCancel.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionCancelReasonUpdate.aspx?REFERENCEID=" + lblDeficiencyid.Text + "&TYPE=4','','450%','250%'); return true;");
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

                RadCheckBox chkSelectall = (RadCheckBox)e.Item.FindControl("chkAllRemittance");
                RadLabel lblStatusId = (RadLabel)e.Item.FindControl("lblStatusId");

                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    chkSelect.Visible = false;
                }

            }

            RadLabel lblDeficiencyyid = (RadLabel)e.Item.FindControl("lblDeficiencyid");
            LinkButton lnkSourceRefNo = (LinkButton)e.Item.FindControl("lnkSourceRefNo");
            RadLabel lblRaisedfrom = (RadLabel)e.Item.FindControl("lblRaisedfrom");
            RadLabel lblSourceId = (RadLabel)e.Item.FindControl("lblSourceId");
            RadLabel lblVesselName = (RadLabel)e.Item.FindControl("lblVesselCode");
            RadLabel lblDeficiency = ((RadLabel)e.Item.FindControl("lblDeficiencyType"));

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
                if ((lblDeficiency.Text == "1" && lblRaisedfrom.Text == "4") ||
                    (lblDeficiency.Text == "2" && lblRaisedfrom.Text == "2")) //Vetting
                {
                    lnkSourceRefNo.Attributes.Add("onclick", "openNewWindow('source','', '" + Session["sitepath"] + "/Inspection/InspectionVettingReport.aspx?SCHEDULEID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                }
                if ((lblDeficiency.Text == "1" && lblRaisedfrom.Text == "3") ||
                    (lblDeficiency.Text == "2" && lblRaisedfrom.Text == "4")) //Open Reports
                {
                    lnkSourceRefNo.Attributes.Add("onclick", "openNewWindow('source','', '" + Session["sitepath"] + "/Inspection/InspectionDirectIncidentGeneral.aspx?directincidentid=" + lblSourceId.Text + "&viewonly=1'); return true;");
                }
                if ((lblDeficiency.Text == "1" && lblRaisedfrom.Text == "2") ||
                    (lblDeficiency.Text == "2" && lblRaisedfrom.Text == "3")) //Direct
                {
                    lnkSourceRefNo.Attributes.Add("onclick", "openNewWindow('source','', '" + Session["sitepath"] + "/Inspection/InspectionDeficiencyEdit.aspx?DashBoard=1&DEFICIENCYID=" + lblDeficiencyyid.Text + "&viewonly=1'); return true;");
                }
            }

            UserControlToolTip ucToolTipInspectionRefNo = (UserControlToolTip)e.Item.FindControl("ucToolTipInspectionRefNo");
            if (ucToolTipInspectionRefNo != null)
            {
                ucToolTipInspectionRefNo.Position = ToolTipPosition.TopCenter;
                ucToolTipInspectionRefNo.TargetControlId = lnkSourceRefNo.ClientID; 
            }

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
                cmdDeficiencyReport.Attributes.Add("onclick", "openNewWindow('Report', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=DEFICIENCYREPORT&deficiencyid=" + lblDeficiencyyid.Text + "&vesselid=" + lblVesselidd.Text + "&showmenu=0&showexcel=NO');return true;");
            }

            if (cmdDeficiencyReport != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdDeficiencyReport.CommandName)) cmdDeficiencyReport.Visible = false;
            }
            LinkButton lnkvessel = (LinkButton)e.Item.FindControl("lnkvessel");
            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblVesselId");
            if (lnkvessel != null)
            {
                lnkvessel.Attributes.Add("onclick", "javascript:top.openNewWindow('maint','Details','Dashboard/DashboardVesselDetails.aspx?vesselid=" + lblvesselid.Text + "');");
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
            Filter.CurrentSelectedDeficiency = null;
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

    private void GetSelectedPvs()
    {
        if (Filter.CurrentSelectedDeficiency != null)
        {
            ArrayList SelectedPvs = (ArrayList)Filter.CurrentSelectedBulkDeficiencies;
            Guid index = new Guid();
            if (SelectedPvs != null && SelectedPvs.Count > 0)
            {
                foreach (GridViewRow row in gvDeficiency.Items)
                {
                    RadCheckBox chk = (RadCheckBox)(row.Cells[0].FindControl("chkSelect"));
                    index = new Guid(gvDeficiency.MasterTableView.Items[0].GetDataKeyValue("FLDDEFICIENCYID").ToString());
                    if (SelectedPvs.Contains(index))
                    {
                        RadCheckBox myCheckBox = (RadCheckBox)row.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
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
            Rebind();
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
}
