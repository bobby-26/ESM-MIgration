using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionDeficiencyOfficeList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','','Inspection/InspectionDeficiencyAdd.aspx')", "Create Deficiency", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDeficiencyOfficeList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionOfficeDeficiencyfilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvDeficiency')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDeficiencyOfficeList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuDeficiency.AccessRights = this.ViewState;
            MenuDeficiency.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                string Status = PhoenixCommonRegisters.GetHardCode(1, 146, "OPN");

                DateTime now = DateTime.Now;
                string FromDate = now.Date.AddMonths(-6).ToShortDateString();
                string ToDate = DateTime.Now.ToShortDateString();

                ViewState["FROMDATE"] = FromDate.ToString();
                ViewState["TODATE"] = ToDate.ToString();
                ViewState["Status"] = Status.ToString();

                VesselConfiguration();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvDeficiency.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                //MenuDeficiencyGeneral.SelectedMenuIndex = 1;
            }

            //PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
            //MenuDeficiencyGeneral.AccessRights = this.ViewState;
            //MenuDeficiencyGeneral.MenuList = toolbarmain.Show();
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
                Response.Redirect("../Inspection/InspectionOfficeDeficiencyfilter.aspx");
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
                                     "FLDTYPE", "FLDDEFICIENCYCATEGORY", "FLDDEFICIENCYDETAILS","FLDCHAPTERNAME", "FLDKEYNAME", "FLDISRCAREQUIREDYN", "FLDISRCACOMPLETEDYN", "FLDSTATUS", "FLDCLOSEDDATE" };
            string[] alCaptions = { "Reference Number", "Vessel", "Issued Date", "Source", "Source Ref", "Type", "Deficiency Category", "Deficiency Details",
                                      "Chapter", "PSC Action Code / VIR Condition", "RCA required", "RCA completed", "Status", "Closed" };

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
            DataSet ds;

            ds = PhoenixInspectionDeficiency.DeficiencyOfficeSearch
                 (General.GetNullableInteger(nvc != null ? (nvc["ucDefeciencyType"] == "0" ? null : nvc["ucDefeciencyType"].ToString()) : null),
                  sortexpression,
                  sortdirection,
                  Convert.ToInt16(ViewState["PAGENUMBER"].ToString()),
                  iRowCount,
                  ref iRowCount,
                  ref iTotalPageCount,
                  0,
                 General.GetNullableString(nvc != null ? nvc["txtRefNo"] : null),
                 //General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : null),
                 //General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : null),
                 //General.GetNullableInteger(nvc != null ? nvc["ucStatus"] : null),
                 General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : ViewState["FROMDATE"].ToString()),
                 General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : ViewState["TODATE"].ToString()),
                 General.GetNullableInteger(nvc != null ? nvc["ucStatus"] : ViewState["Status"].ToString()),
                 General.GetNullableString(nvc != null ? nvc["txtSourceRefNo"] : null),
                 General.GetNullableInteger(nvc != null ? nvc["chkRCAReqd"] : null),
                 General.GetNullableInteger(nvc != null ? nvc["chkRCACompleted"] : null),
                 General.GetNullableInteger(nvc != null ? nvc["chkRCAPending"] : null),
                 General.GetNullableInteger(nvc != null ? nvc["ucCategory"] : null),
                 General.GetNullableInteger(nvc != null ? nvc["ucInspectionType"] : null),
                 General.GetNullableInteger(nvc != null ? nvc["ucInspectionCategory"] : null),
                 General.GetNullableGuid(nvc != null ? nvc["ucInspection"] : null),
                 General.GetNullableGuid(nvc != null ? nvc["ucChapter"] : null),
                 General.GetNullableInteger(nvc != null ? nvc["ddlSource"] : null),
                 General.GetNullableString(nvc != null ? nvc["txtKey"] : null),
                 General.GetNullableInteger(nvc != null ? nvc["ucCompany"] : null));
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
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentDeficiencyFilter = null;
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
                                     "FLDTYPE", "FLDDEFICIENCYCATEGORY","FLDDEFICIENCYDETAILS", "FLDCHAPTERNAME", "FLDKEYNAME", "FLDISRCAREQUIREDYN", "FLDISRCACOMPLETEDYN", "FLDSTATUS", "FLDCLOSEDDATE" };
            string[] alCaptions = { "Reference Number", "Vessel", "Issued Date", "Source", "Source Ref", "Type", "Deficiency Category", "Deficiency Details",
                                      "Chapter", "PSC Action Code / VIR Condition", "RCA required", "RCA completed", "Status", "Closed" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            NameValueCollection nvc = Filter.CurrentDeficiencyFilter;
            DataSet ds;

            ds = PhoenixInspectionDeficiency.DeficiencyOfficeSearch
                 (General.GetNullableInteger(nvc != null ? (nvc["ucDefeciencyType"] == "0" ? null : nvc["ucDefeciencyType"].ToString()) : null),
                  sortexpression,
                  sortdirection,
                  Convert.ToInt16(ViewState["PAGENUMBER"].ToString()),
                  gvDeficiency.PageSize,
                  ref iRowCount,
                  ref iTotalPageCount,
                  0,
                 General.GetNullableString(nvc != null ? nvc["txtRefNo"] : null),
                 //General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : null),
                 //General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : null),
                 //General.GetNullableInteger(nvc != null ? nvc["ucStatus"] : null),
                 General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : ViewState["FROMDATE"].ToString()),
                 General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : ViewState["TODATE"].ToString()),
                 General.GetNullableInteger(nvc != null ? nvc["ucStatus"] : ViewState["Status"].ToString()),
                 General.GetNullableString(nvc != null ? nvc["txtSourceRefNo"] : null),
                 General.GetNullableInteger(nvc != null ? nvc["chkRCAReqd"] : null),
                 General.GetNullableInteger(nvc != null ? nvc["chkRCACompleted"] : null),
                 General.GetNullableInteger(nvc != null ? nvc["chkRCAPending"] : null),
                 General.GetNullableInteger(nvc != null ? nvc["ucCategory"] : null),
                 General.GetNullableInteger(nvc != null ? nvc["ucInspectionType"] : null),
                 General.GetNullableInteger(nvc != null ? nvc["ucInspectionCategory"] : null),
                 General.GetNullableGuid(nvc != null ? nvc["ucInspection"] : null),
                 General.GetNullableGuid(nvc != null ? nvc["ucChapter"] : null),
                 General.GetNullableInteger(nvc != null ? nvc["ddlSource"] : null),
                 General.GetNullableString(nvc != null ? nvc["txtKey"] : null),
                 General.GetNullableInteger(nvc != null ? nvc["ucCompany"] : null));

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
    //protected void gvDeficiency_SelectIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    gvDeficiency.SelectedIndex = se.NewSelectedIndex;
    //    Filter.CurrentSelectedDeficiency = ((RadLabel)gvDeficiency.Rows[se.NewSelectedIndex].FindControl("lblDeficiencyid")).Text;
    //    Rebind();
    //}

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
                if (lblDeficiencyType != null && lblDeficiencyType.Text == "1")
                    Response.Redirect("../Inspection/InspectionAuditDirectNonConformityMaster.aspx?currenttab=DET&REVIEWDNC=" + Filter.CurrentSelectedDeficiency + "&Vesselid=" + lblVesselid.Text, false);
                else if (lblDeficiencyType != null && lblDeficiencyType.Text == "2")
                    Response.Redirect("../Inspection/InspectionDirectObservationMaster.aspx?currenttab=DET&DIRECTOBSERVATIONID=" + Filter.CurrentSelectedDeficiency + "&Vesselid=" + lblVesselid.Text, false);
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
            if (e.Item is GridDataItem)
            {
                //if (!e.Item.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.RowState.Equals(DataControlRowState.Edit))
                //{
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

                RadLabel IssuedDate = (RadLabel)e.Item.FindControl("lblIssuedDate");

                if (drv["FLDOVERDUEYN"].ToString().Equals("1"))
                {
                    IssuedDate.BackColor = System.Drawing.Color.Red;
                    IssuedDate.Attributes.Add("style", "font-weight:bold;");
                    IssuedDate.ToolTip = "Overdue for Review";
                }
                else if (drv["FLDOVERDUEYN"].ToString().Equals("2"))
                {
                    IssuedDate.BackColor = System.Drawing.Color.Orange;
                    IssuedDate.Attributes.Add("style", "font-weight:bold;");
                    IssuedDate.ToolTip = "Overdue for Closure";
                }
                else if (drv["FLDOVERDUEYN"].ToString().Equals("3"))
                {
                    IssuedDate.BackColor = System.Drawing.Color.Brown;
                    IssuedDate.Attributes.Add("style", "font-weight:bold;");
                    IssuedDate.ToolTip = "Overdue for Closure";
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
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                        att.Controls.Add(html);
                    }
                    else
                    {
                        html.InnerHtml = "<span class=\"icon\" style=\"color:skyblue\"><i class=\"fas fa-paperclip\"></i></span>";
                        att.Controls.Add(html);
                    }
                    att.Attributes.Add("onclick", "openNewWindow('att','', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                            + PhoenixModule.QUALITY + "&type=INSPECTIONDEFICIENCY&cmdname=DEFICIENCYUPLOAD&VESSELID=" + lblVesselid.Text + "'); return true;");
                }
                //RadLabel lblDeficiencyid = (RadLabel)e.Item.FindControl("lblDeficiencyid");

                //LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                //if (eb != null) eb.Attributes.Add("onclick", "openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionDeficiencyEdit.aspx?DEFICIENCYID=" + lblDeficiencyid.Text + "'); return true;");

                LinkButton cmdCorrectiveActions = (LinkButton)e.Item.FindControl("cmdCorrectiveActions");
                if (cmdCorrectiveActions != null)
                {
                    cmdCorrectiveActions.Visible = SessionUtil.CanAccess(this.ViewState, cmdCorrectiveActions.CommandName);
                    if (drv["FLDCARPARRECORDEDYN"].ToString() == "0")
                    {
                        cmdCorrectiveActions.Controls.Remove(html);
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-eye\"></i></span>";
                        cmdCorrectiveActions.Controls.Add(html);
                    }
                }
                //}

                LinkButton lnkSourceRefNo = (LinkButton)e.Item.FindControl("lnkSourceRefNo");
                RadLabel lblRaisedfrom = (RadLabel)e.Item.FindControl("lblRaisedfrom");
                RadLabel lblSourceId = (RadLabel)e.Item.FindControl("lblSourceId");
                RadLabel lblDeficiencyType = (RadLabel)e.Item.FindControl("lblDeficiencyType");
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
                LinkButton cmdCar = (LinkButton)e.Item.FindControl("cmdCorrectiveActions");
                RadLabel lblVesselidd = (RadLabel)e.Item.FindControl("lblVesselid");
                if (cmdCar != null)
                {
                    cmdCar.Visible = SessionUtil.CanAccess(this.ViewState, cmdCar.CommandName);
                }

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
                    //cmdDeficiencyReport.Attributes.Add("onclick", "Openpopup('Report', '', '" + Session["sitepath"] + "/Inspection/InspectionDeficiencyReport.aspx?DEFICIENCYID=" + lblDeficiencyyid.Text + "&VESSELID=" + lblVesselidd.Text + "');return true;");
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
}

