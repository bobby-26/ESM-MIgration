using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselAccounts;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using SouthNests.Phoenix.Reports;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewAppraisal : PhoenixBasePage
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
                ViewState["OCCASIONNAME"] = "";
                ViewState["APPRAISALNEW"] = "";
                if (Request.QueryString["vslid"] != null)
                    ViewState["VSLID"] = Request.QueryString["vslid"];
                SetEmployeePrimaryDetails();
                ViewState["CANEDIT"] = "1";
                DataSet ds = PhoenixCrewAppraisal.AppraisalSecurityEdit(int.Parse(ViewState["RANKID"].ToString()), null);
                if (ds.Tables[0].Rows.Count > 0)
                    ViewState["CANEDIT"] = ds.Tables[0].Rows[0]["FLDCANEDIT"].ToString();
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                gvAQ.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            if (Filter.CurrentMenuCodeSelection != null && Filter.CurrentMenuCodeSelection.Contains("VAC-"))
            {
                //  uctitle.ShowMenu = "true";
            }
            if (!String.IsNullOrEmpty(ViewState["VSLID"].ToString()))
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
                CrewAppraisalMain.AccessRights = this.ViewState;
                CrewAppraisalMain.MenuList = toolbarmain.Show();
            }
            else
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    PhoenixToolbar toolbarmain = new PhoenixToolbar();

                    toolbarmain.AddButton("Export All", "ALLAPP", ToolBarDirection.Right);
                    CrewAppraisalMain.AccessRights = this.ViewState;
                    CrewAppraisalMain.MenuList = toolbarmain.Show();
                }
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewAppraisal.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAQ')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            if (ViewState["CANEDIT"].Equals("1") && !String.IsNullOrEmpty(ViewState["VSLID"].ToString()))
                toolbar.AddFontAwesomeButton("../Crew/CrewAppraisal.aspx", "Add New Appraisal", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            else if (ViewState["CANEDIT"].Equals("1") && String.IsNullOrEmpty(ViewState["VSLID"].ToString()))
                toolbar.AddFontAwesomeButton("../Crew/CrewAppraisal.aspx", "Add Appraisal from PM", "<i class=\"fa fa-plus-circle\"></i>", "CREWADD");
            MenuCrewAppraisal.AccessRights = this.ViewState;
            MenuCrewAppraisal.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvAQ.SelectedIndexes.Clear();
        gvAQ.EditIndexes.Clear();
        gvAQ.DataSource = null;
        gvAQ.Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE", "FLDAPPRAISALDATE", "FLDOCCASSIONFORREPORT", "FLDRANKNAME", "FLDPROMOTIONYESNO", "FLDAPPRAISALSTATUS", "FLDRECOMMENDEDSTATUSNAME" };
        string[] alCaptions = { "Vessel", "From", "To", "Appraisal On", "Occasion For Report", "Rank", "Promotion", "Status", "Fit for reemploy" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        DataSet ds = new DataSet();
        if (PhoenixSecurityContext.CurrentSecurityContext.UserType == "OWNER")
        {
            ds = PhoenixCrewAppraisal.SearchAppraisalOwner(
                  General.GetNullableInteger(Filter.CurrentCrewSelection)
                 , sortdirection
                 , 1
                 , iRowCount
                 , ref iRowCount
                 , ref iTotalPageCount
                 );
        }
        else
        {
            ds = PhoenixCrewAppraisal.SearchAppraisal(
                  General.GetNullableInteger(Filter.CurrentCrewSelection)
                  , General.GetNullableInteger(ViewState["VSLID"] != null ? ViewState["VSLID"].ToString() : string.Empty)
                 , sortdirection
                 , 1
                 , iRowCount
                 , ref iRowCount
                 , ref iTotalPageCount
                 );
        }


        General.ShowExcel("Appraisal", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void CrewAppraisalMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (!String.IsNullOrEmpty(ViewState["VSLID"].ToString()) && CommandName.ToUpper().Equals("BACK"))
        {
            Filter.CurrentNewApplicantSelection = null;
            if (Filter.CurrentMenuCodeSelection != null && Filter.CurrentMenuCodeSelection.Contains("VAC-"))
                Response.Redirect("..\\VesselAccounts\\VesselAccountsEmployeeAppraisalQuery.aspx");
            else
                Response.Redirect("..\\Crew\\CrewAppraisalSuperintendentQuery.aspx?vslid=" + ViewState["VSLID"]);
        }
        else if (CommandName.ToUpper().Equals("ALLAPP"))
        {
            try
            {
                DataTable sourceFiles = PhoenixCrewAppraisal.GetAllAppraisalAttachments(General.GetNullableInteger(Filter.CurrentCrewSelection));
                if (sourceFiles.Rows.Count > 0)
                {
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + sourceFiles.Rows[0]["FLDFILENO"].ToString() + "_Appraisals.pdf");

                    int f = 0;
                    // step 1: we create a new document
                    Document document = new Document();

                    // step 2: we create a writer that listens to the document		
                    PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);

                    //step 3: open the document
                    document.Open();
                    DataTable Tmpfiles = new DataTable();
                    Tmpfiles.Clear();
                    Tmpfiles.Columns.Add("NAME");
                    while (f < sourceFiles.Rows.Count)
                    {
                        string Tmpfilelocation = string.Empty;

                        if (sourceFiles.Rows[f][0].ToString() == "")
                        {
                            try
                            {
                                DataSet ds = PhoenixCrewReports.Appraisal(new Guid(sourceFiles.Rows[f][1].ToString()));
                                string[] rptfile = new string[9];
                                rptfile[0] = System.Web.HttpContext.Current.Server.MapPath("../Reports/ReportsCrewJOAppraisal.rpt");
                                rptfile[1] = "ReportsCrewJOAppraisalConduct.rpt";
                                rptfile[2] = "ReportsCrewJOAppraisalAttitude.rpt";
                                rptfile[3] = "ReportsCrewJOAppraisalJudgement.rpt";
                                rptfile[4] = "ReportsCrewJOAppraisalLeadership.rpt";
                                rptfile[5] = "ReportsCrewJOAppraisalProfessionalConduct.rpt";
                                rptfile[6] = "ReportsCrewJOAppraisalMandatoryCourses.rpt";
                                rptfile[7] = "ReportsCrewJOAppraisalValueAddedCourses.rpt";
                                rptfile[8] = "ReportsCrewJOAppraisalSuptSign.rpt";

                                Tmpfilelocation = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Crew/";
                                Tmpfilelocation = Tmpfilelocation + Convert.ToString(Guid.NewGuid()) + ".pdf";

                                //Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
                                //Tmpfilelocation = Tmpfilelocation + "Attachments/Crew/" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                                PhoenixReportClass.ExportReport(rptfile, Tmpfilelocation, ds);
                                Tmpfiles.Rows.Add(Tmpfilelocation);
                                PdfReader reader = new PdfReader(Tmpfilelocation);
                                // we retrieve the total number of pages			
                                int n = reader.NumberOfPages;
                                if (n > 0)
                                {
                                    PdfContentByte cb = writer.DirectContent;
                                    PdfImportedPage page;
                                    int rotation;
                                    int i = 0;
                                    while (i < n)
                                    {
                                        i++;

                                        document.SetPageSize(reader.GetPageSizeWithRotation(i));
                                        document.NewPage();
                                        page = writer.GetImportedPage(reader, i);
                                        rotation = reader.GetPageRotation(i);
                                        if (rotation == 90 || rotation == 270)
                                        {
                                            cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                                        }
                                        else
                                        {
                                            cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                                        }
                                    }
                                }

                            }
                            catch (Exception ex)
                            {

                                throw ex;
                            }
                        }
                        if ((sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".pdf") && File.Exists(sourceFiles.Rows[f][0].ToString()))
                        {
                            PdfReader reader = new PdfReader(sourceFiles.Rows[f][0].ToString());
                            // we retrieve the total number of pages			
                            int n = reader.NumberOfPages;
                            if (n > 0)
                            {
                                PdfContentByte cb = writer.DirectContent;
                                PdfImportedPage page;
                                int rotation;
                                int i = 0;
                                // step 4: we add content	
                                while (i < n) // loop thorugh total no of pages
                                {
                                    i++;

                                    document.SetPageSize(reader.GetPageSizeWithRotation(i));
                                    document.NewPage();
                                    page = writer.GetImportedPage(reader, i);
                                    rotation = reader.GetPageRotation(i);
                                    if (rotation == 90 || rotation == 270)
                                    {
                                        cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                                    }
                                    else
                                    {
                                        cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                                    }

                                }
                            }
                        }
                        else if (((sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".jpg") ||
                                (sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".jpeg") ||
                                (sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".gif") ||
                                (sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".png") ||
                                (sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".tiff") ||
                                (sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".tiff") ||
                                (sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".jif") ||
                                (sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".jfif") ||
                                (sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".bmp")) && File.Exists(sourceFiles.Rows[f][0].ToString()))
                        {
                            document.NewPage();
                            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(sourceFiles.Rows[f][0].ToString());
                            img.ScaleToFit((PageSize.A4.Width - 20), (PageSize.A4.Height - 20));
                            document.Add(img);
                        }
                        f++;

                    }
                    // step 5: we close the document	

                    document.Close();
                    Response.Write(document);
                    if (Tmpfiles.Rows.Count > 0)
                    {
                        int i = 0;
                        while (i < Tmpfiles.Rows.Count)
                        {
                            File.Delete(Tmpfiles.Rows[i][0].ToString());
                            i++;
                        }

                    }
                    Response.End();

                }
                else
                {
                    ucError.ErrorMessage = "No appraisal(attachments) found.";
                    ucError.Visible = true;
                }

            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }
    protected void MenuCrewAppraisal_TabStripCommand(object sender, EventArgs e)
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
                if (PhoenixSecurityContext.CurrentSecurityContext.UserType == "OWNER")
                {
                    Response.Redirect("../Owners/OwnersCrewAppraisal.aspx", false);
                }
                else
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                        Response.Redirect("~/CrewOffshore/CrewOffshoreAppraisalActivity.aspx?vslid=" + ViewState["VSLID"].ToString() + "&cmd=2", false);
                    else
                        Response.Redirect("CrewAppraisalActivity.aspx?vslid=" + ViewState["VSLID"].ToString() + "&cmd=2", false);
                }
            }
            if (CommandName.ToUpper().Equals("CREWADD"))
            {
                Filter.CurrentAppraisalSelection = null;
                if (PhoenixSecurityContext.CurrentSecurityContext.UserType == "OWNER")
                {
                    Response.Redirect("../Owners/OwnersCrewAppraisal.aspx", false);
                }
                else
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                        Response.Redirect("~/CrewOffshore/CrewOffshoreAppraisalActivity.aspx?cmd=" + "2", false);
                    else
                        Response.Redirect("CrewAppraisalActivity.aspx", false);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = new DataTable();
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                dt = PhoenixVesselAccountsEmployee.EditVesselCrew(PhoenixSecurityContext.CurrentSecurityContext.InstallCode, Convert.ToInt32(Filter.CurrentCrewSelection));
            else
                dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();

                ViewState["RANKID"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();

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
    protected void gvAQ_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "SELECT")
            {
                Filter.CurrentAppraisalSelection = ((RadLabel)e.Item.FindControl("lblAppraisalId")).Text;
                string OcassionName = ((RadLabel)e.Item.FindControl("lblOccassion")).Text;
                string OcassionId = ((RadLabel)e.Item.FindControl("lblOccassionId")).Text;

                GetOccasionName(int.Parse(OcassionId.ToString()), new Guid(Filter.CurrentAppraisalSelection));

                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                    Response.Redirect("~/CrewOffshore/CrewOffshoreAppraisalActivity.aspx?&vslid=" + ViewState["VSLID"].ToString() + "&cmd=2", false);
                else if (ViewState["OCCASSIONNAME"].ToString().ToUpper().Trim() == "OWF")
                {
                    Response.Redirect("../Owners/OwnersCrewAppraisal.aspx", false);
                }
                else if ((ViewState["OCCASSIONNAME"].ToString().ToUpper().Trim() == "MTR") && ViewState["APPRAISALNEW"].ToString() == "1")
                {
                    Response.Redirect("CrewAppraisalMidtenureactivity.aspx?vslid=" + ViewState["VSLID"].ToString(), false);
                }
                else
                {
                    Response.Redirect("CrewAppraisalActivity.aspx?&vslid=" + ViewState["VSLID"].ToString(), false);
                }
            }

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {

                string appraisalid = ((RadLabel)e.Item.FindControl("lblAppraisalIdEdit")).Text;
                string vessel = ((UserControlCommonVessel)e.Item.FindControl("ddlVesselEdit")).SelectedVessel;
                string fromdate = ((UserControlDate)e.Item.FindControl("txtFromDateEdit")).Text;
                string todate = ((UserControlDate)e.Item.FindControl("txtToDateEdit")).Text;
                string appraisaldate = ((UserControlDate)e.Item.FindControl("txtAppraisalDateEdit")).Text;
                string occassion = ((UserControlOccassionForReport)e.Item.FindControl("ddlOccassionForReportedit")).SelectedOccassion;

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
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string appraisalid = ((RadLabel)e.Item.FindControl("lblAppraisalId")).Text;
                PhoenixCrewAppraisal.DeleteAppraisal(new Guid(appraisalid));
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
    protected void gvAQ_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            string mnu = Filter.CurrentMenuCodeSelection;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                    del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                else if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    del.Visible = false;
            }
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAtt");
            if (db != null)
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                if (drv["FLDISATTACHMENT"].ToString() == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes["onclick"] = "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                + PhoenixModule.CREW + "&type=APPRAISAL&cmdname=APPRAISALUPLOAD'); return false;";
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton lbtnvesselname = (LinkButton)e.Item.FindControl("lbtnvesselname");
            if(lbtnvesselname!=null)
                lbtnvesselname.Enabled = SessionUtil.CanAccess(this.ViewState, lbtnvesselname.CommandName);
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
               
                if (drv["FLDACTIVEYN"].ToString().Equals("0"))
                    eb.Visible = false;

                if (drv["FLDOCCASSIONFORREPORT"].ToString().ToUpper().Trim().Equals("MID TENURE REVIEW"))
                    eb.Visible = false;

                if (ViewState["CANEDIT"].ToString() != "1")
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
                if (mnu.Contains("VAC-")) ucVessel.Enabled = false;
            }

            LinkButton mb = (LinkButton)e.Item.FindControl("cmdEmail");
            if (mb != null)
            {
                if (drv["FLDACTIVEYN"].ToString().Equals("1"))
                {
                    mb.Visible = true;
                    mb.Visible = SessionUtil.CanAccess(this.ViewState, mb.CommandName);

                    if (Filter.CurrentMenuCodeSelection != null && Filter.CurrentMenuCodeSelection.Contains("VAC-"))
                        mb.Attributes.Add("onclick", "javascript:return openNewWindow('VesselAccounts','','" + Session["sitepath"] + "/VesselAccounts/VesselAccountsCrewAppraisalMail.aspx?employeeid=" + drv["FLDEMPLOYEEID"].ToString() + "&vesselid=" + drv["FLDVESSELID"].ToString() + "'); return false;");
                    else
                        mb.Attributes.Add("onclick", "javascript:openNewWindow('VesselAccounts','','" + Session["sitepath"] + "/VesselAccounts/VesselAccountsCrewAppraisalMail.aspx?employeeid=" + drv["FLDEMPLOYEEID"].ToString() + "&vesselid=" + drv["FLDVESSELID"].ToString() + "'); return false;");
                }
            }

            LinkButton deb = (LinkButton)e.Item.FindControl("cmdDeBrief");
            if (deb != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0) deb.Visible = false;
                else
                {
                    if (drv["FLDRECOMMENDEDSTATUS"].ToString().Equals("2"))
                    {
                        deb.Visible = SessionUtil.CanAccess(this.ViewState, deb.CommandName);
                        deb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewBriefingDebriefingList.aspx?vesselid=" + drv["FLDVESSELID"].ToString() + "&appraisalid=" + drv["FLDCREWAPPRAISALID"].ToString() + (drv["FLDEMPLOYEEBRIEFINGDEBRIEFINGID"].ToString() != string.Empty ? "&CrewBriefingDebriefingId=" + drv["FLDEMPLOYEEBRIEFINGDEBRIEFINGID"].ToString() : "") + "'); return false;");
                    }
                    else deb.Visible = false;
                }
            }
            RadLabel lblmastername = (RadLabel)e.Item.FindControl("lblmastername");
            UserControlToolTip uctLastVessel = (UserControlToolTip)e.Item.FindControl("ucToolTipLastVessel");
            uctLastVessel.Position = ToolTipPosition.TopCenter;
            uctLastVessel.TargetControlId = lblmastername.ClientID;

            RadLabel lblhodname = (RadLabel)e.Item.FindControl("lblhodname");
            UserControlToolTip ucToolTiplblhodname = (UserControlToolTip)e.Item.FindControl("ucToolTiplblhodname");
            ucToolTiplblhodname.Position = ToolTipPosition.TopCenter;
            ucToolTiplblhodname.TargetControlId = lblhodname.ClientID;

            RadLabel lblseafarername = (RadLabel)e.Item.FindControl("lblseafarername");
            UserControlToolTip ucToolTiplbllblseafarername = (UserControlToolTip)e.Item.FindControl("ucToolTiplblseafarername");
            ucToolTiplbllblseafarername.Position = ToolTipPosition.TopCenter;
            ucToolTiplbllblseafarername.TargetControlId = lblseafarername.ClientID;
        }
    }
    protected void gvAQ_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAQ.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE", "FLDAPPRAISALDATE", "FLDOCCASSIONFORREPORT", "FLDRANKNAME", "FLDPROMOTIONYESNO", "FLDAPPRAISALSTATUS", "FLDRECOMMENDEDSTATUSNAME" };
        string[] alCaptions = { "Vessel", "From", "To", "Appraisal On", "Occasion For Report", "Rank", "Promotion", "Status", "Fit for reemploy" };

        int? sortdirection = 1; //DEFAULT DESC SORT
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {
            DataSet ds = new DataSet();
            if (PhoenixSecurityContext.CurrentSecurityContext.UserType == "OWNER")
            {
                ds = PhoenixCrewAppraisal.SearchAppraisalOwner(General.GetNullableInteger(Filter.CurrentCrewSelection)
                      , sortdirection
                      , (int)ViewState["PAGENUMBER"]
                      , gvAQ.PageSize
                      , ref iRowCount
                      , ref iTotalPageCount
                      );
            }
            else
            {
                ds = PhoenixCrewAppraisal.SearchAppraisal(General.GetNullableInteger(Filter.CurrentCrewSelection)
                    , General.GetNullableInteger(ViewState["VSLID"] != null ? ViewState["VSLID"].ToString() : string.Empty)
                    , sortdirection
                    , (int)ViewState["PAGENUMBER"]
                    , gvAQ.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    );
            }

            General.SetPrintOptions("gvAQ", "Appraisal", alCaptions, alColumns, ds);
            gvAQ.DataSource = ds.Tables[0];

            if (ds.Tables[0].Rows.Count > 0)
            {
                //if (Filter.CurrentAppraisalSelection == null)
                //    Filter.CurrentAppraisalSelection = ((Label)gvAQ.Rows[0].FindControl("lblAppraisalId")).Text;
            }
            else
                Filter.CurrentAppraisalSelection = null;
            gvAQ.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
        if (fromdate == null || !DateTime.TryParse(fromdate, out resultdate))
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


        if (appraisaldate == null || !DateTime.TryParse(appraisaldate, out resultdate))
            ucError.ErrorMessage = "Appraisal Date is required";

        return (!ucError.IsError);
    }
    public void GetOccasionName(int OccasionId, Guid AppraisalId)
    {
        string OccasionName = null;
        string Appraisalnew = "0";

        PhoenixCrewAppraisal.AppraisalOccasionName(OccasionId, AppraisalId, ref OccasionName, ref Appraisalnew);

        if (OccasionName == System.DBNull.Value.ToString())
            OccasionName = "";

        if (Appraisalnew == System.DBNull.Value.ToString())
            Appraisalnew = "0";

        ViewState["OCCASSIONNAME"] = OccasionName.ToString();
        ViewState["APPRAISALNEW"] = Appraisalnew.ToString();
    }

}
