using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;

public partial class CrewAcademicQualification : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridDataItem r in gvCrewAcademics.Items)
        {
            if (r is GridDataItem)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        foreach (GridDataItem r in gvPreSeaCourse.Items)
        {
            if (r is GridDataItem)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewAcademicQualification.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCrewAcademics')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewAcademic.aspx?empid=" + Filter.CurrentCrewSelection + "'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCREWACADEMIC");

            MenuCrewAcademic.AccessRights = this.ViewState;
            MenuCrewAcademic.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarMenu = new PhoenixToolbar();
            MenuTab.AccessRights = this.ViewState;
            MenuTab.MenuList = toolbarMenu.Show();
            
            toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewAcademicQualification.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvPreSeaCourse')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewPreSeaCourse.aspx?empid=" + Filter.CurrentCrewSelection + "'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDPRESEACOURSE");

            CrewPreSeaCourse.AccessRights = this.ViewState;
            CrewPreSeaCourse.MenuList = toolbargrid.Show();

            toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewAcademicQualification.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvAwardAndCertificate')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            CrewAwardandCertificate.AccessRights = this.ViewState;
            CrewAwardandCertificate.MenuList = toolbargrid.Show();
            
            if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
            {
                CrewAwardandCertificate.Visible = false;
                gvAwardAndCertificate.Visible = false;
                lblAwardandCertificate.Text = "";
            }
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvCrewAcademics.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBERSUB"] = 1;
                ViewState["SORTEXPRESSIONSUB"] = null;
                ViewState["SORTDIRECTIONSUB"] = null;
                ViewState["CURRENTINDEXSUB"] = 1;

                gvPreSeaCourse.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBERCERT"] = 1;
                ViewState["SORTEXPRESSIONCERT"] = null;
                ViewState["SORTDIRECTIONCERT"] = null;
                ViewState["CURRENTINDEXCERT"] = 1;

                gvAwardAndCertificate.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                SetEmployeePrimaryDetails();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void CrewAcademicMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDQUALIFICATION", "FLDPERCENTAGE", "FLDDATEOFPASS" };
                string[] alCaptions = { "Certificate", "Percentage", "Passed Date" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                DataTable dt = PhoenixNewApplicantAcademic.ListEmployeeAcademic(Convert.ToInt32(Filter.CurrentCrewSelection), null);
                General.ShowExcel("Academic Qualification", dt, alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvCrewAcademics_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewAcademics.CurrentPageIndex + 1;
            BindData();
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
            int iRowCount = 0;

            string[] alColumns = { "FLDQUALIFICATION", "FLDPERCENTAGE", "FLDDATEOFPASS" };
            string[] alCaptions = { "Certificate", "Percentage", "Passed Date" };
            string academicid = null;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["EMPLOYEEACADEMICID"] != null)
            {
                academicid = ViewState["EMPLOYEEACADEMICID"].ToString();
            }
            DataTable dt = PhoenixCrewAcademicQualification.ListEmployeeAcademic(Convert.ToInt32(Filter.CurrentCrewSelection), null);
            iRowCount = dt.Rows.Count;
            //iTotalPageCount = 1;

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCrewAcademics", "Academics Qualification", alCaptions, alColumns, ds);

            gvCrewAcademics.DataSource = ds;
            gvCrewAcademics.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewAcademics_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                _gridView.MasterTableView.ClearEditItems();
            }

            if (e.CommandName == "DELETE")
            {
                string trainingid = ((RadLabel)e.Item.FindControl("lblAcademicsId")).Text;
                PhoenixCrewAcademicQualification.DeleteEmployeeAcademic(Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                                                                , Convert.ToInt32(trainingid)
                                                                );

                gvCrewAcademics.SelectedIndexes.Clear();
                gvCrewAcademics.EditIndexes.Clear();
                gvCrewAcademics.DataSource = null;
                gvCrewAcademics.Rebind();
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



    protected void gvCrewAcademics_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null)
        {
            del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            del.Attributes.Add("onclick", "return fnConfirmDelete(event)");
        }
        LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAtt");
        RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
        RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
        if (cmdAttachment != null)
        {
            cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            if (lblIsAtt.Text == string.Empty)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                cmdAttachment.Controls.Add(html);
            }
            cmdAttachment.Attributes.Add("onclick", "javascript:parent.Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                   + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.ACADEMICS + "&cmdname=COURSEUPLOAD'); return false;");

        }
        LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
        RadLabel l = (RadLabel)e.Item.FindControl("lblAcademicsId");
        if (cmdEdit != null && l != null)
        {
            cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
            cmdEdit.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewAcademic.aspx?empid=" + Filter.CurrentCrewSelection + "&ACADEMICID=" + l.Text + "');return false;");
        }
        RadLabel lblPercentage = (RadLabel)e.Item.FindControl("lblPercentage");
        RadLabel Lblgrade = (RadLabel)e.Item.FindControl("Lblgrade");


        if (lblPercentage != null && Lblgrade != null)
            lblPercentage.Text = lblPercentage.Text + (Lblgrade.Text.Trim() != "" && lblPercentage.Text.Trim() != "" ? "/" : "") + Lblgrade.Text.Trim();


    }


    protected void CrewPreSeaCourseMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDQUALIFICATION", "FLDINSTITUTIONNAME", "FLDREMARKS", "FLDPERCENTAGE", "FLDPASSDATE" };
                string[] alCaptions = { "Certificate", "Institution", "Remarks", "Percentage", "Passed Date" };
                string sortexpression;
                int? sortdirection = null;
                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["SORTDIRECTIONSUB"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTIONSUB"].ToString());
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNTSUB"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNTSUB"].ToString());

                DataSet ds = PhoenixCrewAcademicQualification.EmployeePreSeaCourseSearch(
                      Int32.Parse(Filter.CurrentCrewSelection.ToString()), null
                      , null, null, sortexpression, sortdirection
                      , (int)ViewState["PAGENUMBERSUB"]
                      , gvPreSeaCourse.PageSize
                      , ref iRowCount
                      , ref iTotalPageCount);

                Response.AddHeader("Content-Disposition", "attachment; filename=PreSeaCourses.xls");
                Response.ContentType = "application/vnd.msexcel";
                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                Response.Write("<tr>");
                Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
                Response.Write("<td><h3>PreSea Courses</h3></td>");
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvPreSeaCourse_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBERSUB"] = ViewState["PAGENUMBERSUB"] != null ? ViewState["PAGENUMBERSUB"] : gvPreSeaCourse.CurrentPageIndex + 1;

            BindCourseData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindCourseData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDQUALIFICATION", "FLDINSTITUTIONNAME", "FLDREMARKS", "FLDPERCENTAGE", "FLDPASSDATE" };
            string[] alCaptions = { "Certificate", "Institution", "Remarks", "Percentage", "Passed Date" };
            string sortexpression = (ViewState["SORTEXPRESSIONSUB"] == null) ? null : (ViewState["SORTEXPRESSIONSUB"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONSUB"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONSUB"].ToString());


            DataSet ds = PhoenixCrewAcademicQualification.EmployeePreSeaCourseSearch(
                      Int32.Parse(Filter.CurrentCrewSelection.ToString()), null
                      , null, null, sortexpression, sortdirection
                      , (int)ViewState["PAGENUMBERSUB"]
                      , gvPreSeaCourse.PageSize
                      , ref iRowCount
                      , ref iTotalPageCount);

            General.SetPrintOptions("gvPreSeaCourse", "Pre Sea Courses", alCaptions, alColumns, ds);

            gvPreSeaCourse.DataSource = ds;
            gvPreSeaCourse.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  

    protected void gvPreSeaCourse_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null)
        {
            del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            del.Attributes.Add("onclick", "return fnConfirmDelete(event)");
        }
        LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAtt");
        RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
        RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
        if (cmdAttachment != null)
        {
            cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            if (lblIsAtt.Text == string.Empty)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                cmdAttachment.Controls.Add(html);
            }
            cmdAttachment.Attributes.Add("onclick", "javascript:parent.Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                   + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.ACADEMICS + "&cmdname=PRESEAUPLOAD'); return false;");

        }
        LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
        RadLabel l = (RadLabel)e.Item.FindControl("lblPreseacourseId");
        if (cmdEdit != null && l != null)
        {
            cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
            cmdEdit.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewPreSeaCourse.aspx?empid=" + Filter.CurrentCrewSelection + "&PRESEARCOURSEID=" + l.Text + "');return false;");
        }

        RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
        UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipRemarks");
        if (lbtn != null)
        {
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
        }
    }

    protected void gvPreSeaCourse_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                _gridView.MasterTableView.ClearEditItems();
            }

            if (e.CommandName == "DELETE")
            {
                string courseid = ((RadLabel)e.Item.FindControl("lblPreseacourseId")).Text;
                PhoenixCrewAcademicQualification.DeleteEmployeePreSeaCourse(Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                                                                , Convert.ToInt32(courseid)
                                                                );

                gvPreSeaCourse.SelectedIndexes.Clear();
                gvPreSeaCourse.EditIndexes.Clear();
                gvPreSeaCourse.DataSource = null;
                gvPreSeaCourse.Rebind();

            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERSUB"] = null;
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


   
    protected void gvAwardAndCertificate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBERCERT"] = ViewState["PAGENUMBERCERT"] != null ? ViewState["PAGENUMBERCERT"] : gvAwardAndCertificate.CurrentPageIndex + 1;

            BindAwardCertificate();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindAwardCertificate()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDROWNUMBER", "FLDCERTIFICATENAME", "FLDISSUEDATE", "FLDREMARKS" };
            string[] alCaptions = { "Sl No", "Award/Certificate", "Issue Date", "Remarks" };


            string sortexpression = (ViewState["SORTEXPRESSIONCERT"] == null) ? null : (ViewState["SORTEXPRESSIONCERT"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONCERT"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONCERT"].ToString());


            DataSet ds = PhoenixCrewAwardAndCertificate.CrewAwardAndCertificateSearch(Convert.ToInt32(Filter.CurrentCrewSelection)
                     , sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBERCERT"].ToString()), gvAwardAndCertificate.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvAwardAndCertificate", "Award and Certificate", alCaptions, alColumns, ds);
            gvAwardAndCertificate.DataSource = ds;
            gvAwardAndCertificate.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void AwardandCertificateMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDROWNUMBER", "FLDCERTIFICATENAME", "FLDISSUEDATE", "FLDREMARKS" };
                string[] alCaptions = { "Sl No", "Award/Certificate", "Issue Date", "Remarks" };


                string sortexpression = (ViewState["SORTEXPRESSIONCERT"] == null) ? null : (ViewState["SORTEXPRESSIONCERT"].ToString());
                int? sortdirection = null;

                if (ViewState["SORTDIRECTIONCERT"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTIONCERT"].ToString());


                if (ViewState["ROWCOUNTCERT"] == null || Int32.Parse(ViewState["ROWCOUNTCERT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNTCERT"].ToString());

                DataSet ds = PhoenixCrewAwardAndCertificate.CrewAwardAndCertificateSearch(Convert.ToInt32(Filter.CurrentCrewSelection)
                     , sortexpression, sortdirection, (int)ViewState["PAGENUMBERCERT"], iRowCount, ref iRowCount, ref iTotalPageCount);

                General.ShowExcel("Award and Certificate", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void gvAwardAndCertificate_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                _gridView.MasterTableView.ClearEditItems();
            }
            if (e.CommandName == "EDIT")
            {
                e.Item.OwnerTableView.IsItemInserted = false;
            }
            if (e.CommandName.ToString().ToUpper() == "ADD")
            {
                string certificate = ((UserControlQuick)e.Item.FindControl("ddlCertificateAdd")).SelectedValue;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateAdd")).Text;
                string remarks = ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text;

                if (!IsValidCertificate(certificate, dateofissue, remarks))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewAwardAndCertificate.InsertCrewAwardAndCertificate(
                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(certificate)
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableString(remarks)
                    , Convert.ToInt32(Filter.CurrentCrewSelection)
                    );

                BindAwardCertificate();
                gvAwardAndCertificate.Rebind();

            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string awardid = ((RadLabel)e.Item.FindControl("lblAwardIdEdit")).Text;
                string certificate = ((UserControlQuick)e.Item.FindControl("ddlCertificateEdit")).SelectedValue;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit")).Text;
                string remarks = ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text;

                if (awardid != "")
                {
                    if (!IsValidCertificate(certificate, dateofissue, remarks))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixCrewAwardAndCertificate.UpdateCrewAwardAndCertificate(
                              PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , Convert.ToInt32(awardid)
                            , Convert.ToInt32(certificate)
                            , General.GetNullableDateTime(dateofissue)
                            , General.GetNullableString(remarks)
                            , Convert.ToInt32(Filter.CurrentCrewSelection)
                            );

                    BindAwardCertificate();
                    gvAwardAndCertificate.Rebind();
                }
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string awardid = ((RadLabel)e.Item.FindControl("lblAwardId")).Text;

                PhoenixCrewAwardAndCertificate.DeleteCrewAwardAndCertificate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        Convert.ToInt32(awardid));
                BindAwardCertificate();
                gvAwardAndCertificate.Rebind();

            }

            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERCERT"] = null;
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvAwardAndCertificate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }
            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            if (cmdAttachment != null)
            {
                cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    cmdAttachment.Controls.Add(html);
                }
                cmdAttachment.Attributes.Add("onclick", "javascript:parent.Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                       + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.ACADEMICS + "&cmdname=AWARDANDCERTIFICATE'); return false;");

            }
        }
        if (e.Item.IsInEditMode)
        {
            UserControlQuick ddlcertificate = (UserControlQuick)e.Item.FindControl("ddlCertificateEdit");
            DataRowView drvCertificate = (DataRowView)e.Item.DataItem;
            if (ddlcertificate != null) ddlcertificate.SelectedValue = drvCertificate["FLDCERTIFICATE"].ToString();
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }


        }
    }

    
    private bool IsValidCertificate(string certificate, string issuedate, string remarks)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(certificate) == null)
            ucError.ErrorMessage = "Award/Certificate is required";

        if (General.GetNullableDateTime(issuedate) == null)
            ucError.ErrorMessage = "Issue Date is required";

        if (remarks.Trim() == "")
            ucError.ErrorMessage = "Remarks is required";

        return (!ucError.IsError);
    }



    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
            if (dt.Rows.Count > 0)
            {

                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    private void ResetFormControlValues(Control parent)
    {

        try
        {
            ViewState["EMPLOYEEACADEMICID"] = null;

            foreach (Control c in parent.Controls)
            {
                if (c.Controls.Count > 0)
                {
                    ResetFormControlValues(c);
                }
                else
                {
                    switch (c.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.TextBox":
                            ((TextBox)c).Text = "";
                            break;
                        case "System.Web.UI.WebControls.CheckBox":
                            ((CheckBox)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.RadioButton":
                            ((RadioButton)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.DropDownList":
                            ((DropDownList)c).SelectedIndex = 0;
                            break;
                        case "System.Web.UI.WebControls.ListBox":
                            ((ListBox)c).SelectedIndex = 0;
                            break;

                    }
                }
            }
            SetEmployeePrimaryDetails();
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
        BindCourseData();
        BindAwardCertificate();

    }
}
