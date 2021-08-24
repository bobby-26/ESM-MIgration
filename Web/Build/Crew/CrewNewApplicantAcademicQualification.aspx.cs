using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class CrewNewApplicantAcademicQualification : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewNewApplicantAcademicQualification.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "ACADEMICEXCEL");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCrewAcademics')", "Print Grid", "<i class=\"fas fa-print\"></i>", "ACADEMICPRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewAcademic.aspx?empid=" + Filter.CurrentNewApplicantSelection + "')", "ACADEMICADD", "<i class=\"fa fa-plus-circle\"></i>", "ADDCREWACADEMIC");

            MenuCrewAcademic.AccessRights = this.ViewState;
            MenuCrewAcademic.MenuList = toolbargrid.Show();

            toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewNewApplicantAcademicQualification.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "PRESEAExcel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvPreSeaCourse')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRESEAPRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewPreSeaCourse.aspx?empid=" + Filter.CurrentNewApplicantSelection + "')", "PRESEAADD", "<i class=\"fa fa-plus-circle\"></i>", "ADDPRESEACOURSE");
            CrewPreSeaCourse.AccessRights = this.ViewState;
            CrewPreSeaCourse.MenuList = toolbargrid.Show();

            toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewNewApplicantAcademicQualification.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvAwardAndCertificate')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            CrewAwardandCertificate.AccessRights = this.ViewState;
            CrewAwardandCertificate.MenuList = toolbargrid.Show();

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
                divAwards.Visible = false;

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            CrewAcademic.Title = "Academic Document";
            CrewAcademic.AccessRights = this.ViewState;
            CrewAcademic.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["PAGENUMBERSUB"] = 1;
                ViewState["SORTEXPRESSIONSUB"] = null;
                ViewState["SORTDIRECTIONSUB"] = null;
                ViewState["CURRENTINDEXSUB"] = 1;

                ViewState["PAGENUMBERCERT"] = 1;
                ViewState["SORTEXPRESSIONCERT"] = null;
                ViewState["SORTDIRECTIONCERT"] = null;
                ViewState["CURRENTINDEXCERT"] = 1;
                SetEmployeePrimaryDetails();

                gvPreSeaCourse.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvAwardAndCertificate.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            //BindData();
            //SetPageNavigator();
            //BindCourseData();
            //SetPageNavigatorSub();
            //BindAwardCertificate();
            //SetPageNavigatorCert();
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
            if (CommandName.ToUpper().Equals("ACADEMICEXCEL"))
            {
                string[] alColumns = { "FLDQUALIFICATION", "FLDPERCENTAGE", "FLDDATEOFPASS" };
                string[] alCaptions = { "Certificate", "Percentage", "Passed Date" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                DataTable dt = PhoenixNewApplicantAcademic.ListEmployeeAcademic(Convert.ToInt32(Filter.CurrentNewApplicantSelection), null);
                General.ShowExcel("Academics Qualification", dt, alColumns, alCaptions, sortdirection, sortexpression);
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
            int iRowCount = 0;
            int iTotalPageCount = 0;
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
            DataTable dt = PhoenixCrewAcademicQualification.ListEmployeeAcademic(Convert.ToInt32(Filter.CurrentNewApplicantSelection), null);
            iRowCount = dt.Rows.Count;
            iTotalPageCount = 1;

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCrewAcademics", "Academics Qualification", alCaptions, alColumns, ds);
            gvCrewAcademics.DataSource = dt;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

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
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtEmployeeMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtEmployeeLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvCrewAcademics.Rebind();

        gvPreSeaCourse.Rebind();

    }



    protected void CrewPreSeaCourseMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("PRESEAEXCEL"))
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
                      Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()), null
                      , null, null, sortexpression, sortdirection
                      , (int)ViewState["PAGENUMBERSUB"]
                      , iRowCount
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
                      Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()), null
                      , null, null, sortexpression, sortdirection
                      , (int)ViewState["PAGENUMBERSUB"]
                      , gvPreSeaCourse.PageSize
                      , ref iRowCount
                      , ref iTotalPageCount);



            General.SetPrintOptions("gvPreSeaCourse", "Pre Sea Courses", alCaptions, alColumns, ds);

            gvPreSeaCourse.DataSource = ds;
            gvPreSeaCourse.VirtualItemCount = iRowCount;




            ViewState["ROWCOUNTSUB"] = iRowCount;
            ViewState["TOTALPAGECOUNTSUB"] = iTotalPageCount;

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

                DataSet ds = PhoenixCrewAwardAndCertificate.CrewAwardAndCertificateSearch(Convert.ToInt32(Filter.CurrentNewApplicantSelection)
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


            DataSet ds = PhoenixCrewAwardAndCertificate.CrewAwardAndCertificateSearch(Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                     , sortexpression, sortdirection, (int)ViewState["PAGENUMBERCERT"], gvAwardAndCertificate.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvAwardAndCertificate", "Award and Certificate", alCaptions, alColumns, ds);

            gvAwardAndCertificate.DataSource = ds.Tables[0];
            gvAwardAndCertificate.VirtualItemCount = iRowCount;

       

            ViewState["ROWCOUNTCERT"] = iRowCount;
            ViewState["TOTALPAGECOUNTCERT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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


   
    protected void gvCrewAcademics_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();

    }

    protected void gvCrewAcademics_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");


            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                HtmlGenericControl html = new HtmlGenericControl();
                if (lblIsAtt != null && lblIsAtt.Text == string.Empty && att != null)
                {
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray;\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                 + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.ACADEMICS + "&cmdname=COURSEUPLOAD'); return false;");

            }


            RadLabel l = (RadLabel)e.Item.FindControl("lblAcademicsId");

            LinkButton lb = (LinkButton)e.Item.FindControl("lblAcademicsname");
            lb.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewAcademic.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&ACADEMICID=" + l.Text + "');return false;");

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            db1.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Crew/CrewAcademic.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&ACADEMICID=" + l.Text + "');return false;");
            RadLabel lblPercentage = (RadLabel)e.Item.FindControl("lblPercentage");
            RadLabel Lblgrade = (RadLabel)e.Item.FindControl("Lblgrade");
            lblPercentage.Text = lblPercentage.Text + (Lblgrade.Text.Trim() != "" && lblPercentage.Text.Trim() != "" ? "/" : "") + Lblgrade.Text.Trim();

        }
    }

    protected void gvCrewAcademics_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToString().ToUpper() == "DELETE")
            {
                string trainingid = ((RadLabel)e.Item.FindControl("lblAcademicsId")).Text;

                PhoenixCrewAcademicQualification.DeleteEmployeeAcademic(Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                                                                , Convert.ToInt32(trainingid)
                                                                );
                BindData();
                gvCrewAcademics.Rebind();

                ResetFormControlValues(this);
                SetEmployeePrimaryDetails();

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
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

    protected void gvPreSeaCourse_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");

            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                HtmlGenericControl html = new HtmlGenericControl();
                if (lblIsAtt != null && lblIsAtt.Text == string.Empty && att != null)
                {
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray;\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.ACADEMICS + "&cmdname=PRESEAUPLOAD'); return false;");
            }




            RadLabel l = (RadLabel)e.Item.FindControl("lblPreseacourseId");

            LinkButton lb = (LinkButton)e.Item.FindControl("lblCourseName");
            lb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewPreSeaCourse.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&PRESEARCOURSEID=" + l.Text + "');return false;");

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            db1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewPreSeaCourse.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&PRESEARCOURSEID=" + l.Text + "');return false;");
            if (db1 != null) db1.Visible = SessionUtil.CanAccess(this.ViewState, db1.CommandName);
            if (drv["FLDQUALIFICATIONID"].ToString() == "")
            {
                db1.Visible = false;
            }
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipRemarks");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

        }

    }

    protected void gvPreSeaCourse_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToString().ToUpper() == "EDIT")
            {
                string academicid = ((RadLabel)e.Item.FindControl("lblPreseacourseId")).Text;
                e.Item.Selected = true;
            }
            else if (e.CommandName.ToString().ToUpper() == "DELETE")
            {
                string courseid = ((RadLabel)e.Item.FindControl("lblPreseacourseId")).Text;

                PhoenixCrewAcademicQualification.DeleteEmployeePreSeaCourse(Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                                                                , Convert.ToInt32(courseid)
                                                                );

                BindCourseData();
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

    protected void gvAwardAndCertificate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            LinkButton db = (LinkButton)e.Item.FindControl("cmdXDelete");
        
            LinkButton att = (LinkButton)e.Item.FindControl("cmdXAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");

            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
               
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
                LinkButton cme = (LinkButton)e.Item.FindControl("cmdXEdit");
                if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
            }
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            if (att != null)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                if (lblIsAtt != null && lblIsAtt.Text == string.Empty && att != null)
                {
                   
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray;\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                   + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.ACADEMICS + "&cmdname=AWARDANDCERTIFICATE'); return false;");

            }


            RadLabel lblawardid = (RadLabel)e.Item.FindControl("lblAwardId");

            UserControlQuick ddlcertificate = ((UserControlQuick)e.Item.FindControl("ddlCertificateEdit"));
            DataRowView drvCertificate = (DataRowView)e.Item.DataItem;
            if (ddlcertificate != null) ddlcertificate.SelectedValue = drvCertificate["FLDCERTIFICATE"].ToString();
        }

    }

    protected void gvAwardAndCertificate_ItemCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
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
                    , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                    );
                BindAwardCertificate();
                gvAwardAndCertificate.Rebind();

            }
            if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {
                string awardid = ((RadLabel)e.Item.FindControl("lblAwardIdEdit")).Text;
                string certificate = ((UserControlQuick)e.Item.FindControl("ddlCertificateEdit")).SelectedValue;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit")).Text;
                string remarks = ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text;

                if (!IsValidCertificate(certificate, dateofissue, remarks))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewAwardAndCertificate.UpdateCrewAwardAndCertificate(
                          PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , Convert.ToInt32(awardid)
                        , Convert.ToInt32(certificate)
                        , General.GetNullableDateTime(dateofissue)
                        , General.GetNullableString(remarks)
                        , Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                        );

                BindAwardCertificate();
                gvAwardAndCertificate.Rebind();
            }
            if (e.CommandName.ToUpper() == "DELETE")
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
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
}
