using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class CrewOffshoreEvaluationList : PhoenixBasePage
{
    public string strPostedRankId;


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["ISRATING"] = "";
                ViewState["EMPID"] = "";
                ViewState["NEWAPP"] = "";
                ViewState["RANKPOSTED"] = "";
                if (Request.QueryString["empid"] != null && Request.QueryString["empid"].ToString() != "")
                    ViewState["EMPID"] = Request.QueryString["empid"].ToString();
                if (Request.QueryString["newapp"] != null && Request.QueryString["newapp"].ToString() != "")
                    ViewState["NEWAPP"] = Request.QueryString["NEWAPP"].ToString();

                SetEmployeePrimaryDetails();
                gvIntSum.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreEvaluationList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
                toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreEvaluationAdd.aspx?newapp=" + ViewState["NEWAPP"].ToString() + "&iframIgnore=True", "Add New Proposal", "<i class=\"fa fa-plus-circle\"></i>", "ADDINTSUMMARY");

           // toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEvaluationAdd.aspx?newapp=" + ViewState["NEWAPP"].ToString() + "')", "Add New Proposal", "<i class=\"fa fa-plus-circle\"></i>", "ADDINTSUMMARY");

            MenuIntSummary.AccessRights = this.ViewState;
            MenuIntSummary.MenuList = toolbar.Show();


            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            TabStrip1.AccessRights = this.ViewState;
            TabStrip1.Title = "Assesment";
            TabStrip1.MenuList = toolbar1.Show();
            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected int? CheckIfOffshoreEmployee()
    {
        int? offshore = null;
        PhoenixCrewOffshoreEmployee.CheckIfOffshoreEmployee(int.Parse(ViewState["EMPID"].ToString()), ref offshore);
        return offshore;
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = 1;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixCrewOffshoreInterview.SearchOffshoreNewApplicantInterviewSummary(Int32.Parse(ViewState["EMPID"].ToString())
                                                                                     , sortexpression, sortdirection,
                                                                                      (int)ViewState["PAGENUMBER"],
                                                                                      gvIntSum.PageSize,
                                                                                      ref iRowCount,
                                                                                      ref iTotalPageCount);
            gvIntSum.DataSource = dt;
            gvIntSum.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuIntSummary_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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

        string[] alColumns = { };
        string[] alCaptions = { };

        if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
        {
            alColumns = new string[] { "FLDRANKNAME", "FLDASSESSMENT", "FLDSALAGREED", "FLDTOTALSCORE", "FLDPDSTATUS" };
            alCaptions = new string[] { "Proposed Rank", "Interviewed By", "Daily Rate($)", "Score", "Status" };
        }

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewOffshoreInterview.SearchOffshoreNewApplicantInterviewSummary(Int32.Parse(ViewState["EMPID"].ToString())
                                                                                     , sortexpression, sortdirection,
                                                                                      (int)ViewState["PAGENUMBER"],
                                                                                      iRowCount,
                                                                                      ref iRowCount,
                                                                                      ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=Assessment.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Assessment</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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
    }



    protected void cmdSort_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton ib = (ImageButton)sender;

            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidReference(string companyname, string phonenumber)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (companyname.Trim() == string.Empty)
            ucError.ErrorMessage = "Company Name is required";
        if (companyname.Trim() == string.Empty)
            ucError.ErrorMessage = "Phone Number is required";
        return (!ucError.IsError);
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(ViewState["EMPID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                ViewState["RANKPOSTED"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
                if (dt.Rows[0]["FLDISOFFICER"].ToString() == "1")
                {

                    ViewState["ISRATING"] = "0";
                }
                else
                {
                    ViewState["ISRATING"] = "1";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvIntSum.Rebind();

    }
    protected void gvIntSum_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvIntSum.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvIntSum_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                RadLabel lblIntStatus = (RadLabel)e.Item.FindControl("lblInterviewStatus");
                LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
                if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
                RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

                if (lblIsAtt != null && lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();

                    html.InnerHtml = "<span class=\"icon\" style=\"color: gray;\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);

                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.ASSESSMENT + (lblIntStatus.Text != string.Empty ? "&u=" : string.Empty) + "&ratingyn=" + ViewState["ISRATING"] + "&cmdname=PROPOSALUPLOAD'); return false;");

                ////////////////////  PD Attachment
                RadLabel lblShowPD = (RadLabel)e.Item.FindControl("lblShowPD");
                RadLabel lblCrewPlanId = (RadLabel)e.Item.FindControl("lblCrewPlanId");
                RadLabel lblPDDTKey = (RadLabel)e.Item.FindControl("lblPDDTKey");
                LinkButton cmdPDAtt = (LinkButton)e.Item.FindControl("cmdPDAtt");
                RadLabel lblIsPDAtt = (RadLabel)e.Item.FindControl("lblIsPDAtt");
                RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
                string vslid = "";

                if (!SessionUtil.CanAccess(this.ViewState, cmdPDAtt.CommandName))
                {
                    cmdPDAtt.Visible = false;
                }

                if (lblIsPDAtt != null && lblIsPDAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();

                    html.InnerHtml = "<span class=\"icon\" style=\"color: gray;\"><i class=\"fas fa-paperclip\"></i></span>";
                    cmdPDAtt.Controls.Add(html);

                }

                if (lblVesselid != null)
                    vslid = lblVesselid.Text;

                if (lblCrewPlanId != null && lblCrewPlanId.Text == string.Empty)
                    cmdPDAtt.Visible = false;
                else if (lblCrewPlanId != null && lblCrewPlanId.Text != string.Empty)
                    cmdPDAtt.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewApprovedPDAttachment.aspx?dtkey=" + lblCrewPlanId.Text + "&cmdname=PDUPLOAD&vslid=" + vslid.ToString() + "'); return false;");
                if (lblShowPD.Text == "0")
                {
                    cmdPDAtt.Visible = false;
                }

                ///////////////////

                RadLabel l = (RadLabel)e.Item.FindControl("lblInterviewId");


                if (Filter.CurrentCrewLaunchedFrom != null && Filter.CurrentCrewLaunchedFrom.ToString() != "")
                {
                    //lb.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', '../CrewOffshore/CrewOffshoreEvaluationEdit.aspx?interviewid=" + l.Text + "');return false;");                    
                    // cme.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreEvaluationEdit.aspx?interviewid=" + l.Text + "&newapp=" + ViewState["NEWAPP"].ToString() + "');return false;");
                    cme.Attributes.Add("onclick", "../CrewOffshore/CrewOffshoreEvaluationEdit.aspx?interviewid=" + l.Text + "&newapp=" + ViewState["NEWAPP"].ToString() + "&dt=" + lblDTKey.Text);

                }

                RadLabel lbtn = (RadLabel)e.Item.FindControl("lblPDStatus");

                LinkButton dp = (LinkButton)e.Item.FindControl("cmdDeplan");
                if (dp != null)
                {
                    dp.Visible = SessionUtil.CanAccess(this.ViewState, dp.CommandName);
                    dp.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to deplan the seafarer ?')");
                }
                LinkButton dl = (LinkButton)e.Item.FindControl("cmdDelete");
                if (dl != null)
                {
                    dl.Visible = SessionUtil.CanAccess(this.ViewState, dp.CommandName);
                    dl.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to delete the assessment for the seafarer ?')");
                }

                LinkButton interviewsheet = (LinkButton)e.Item.FindControl("cmdInterviewSheet");
                if (interviewsheet != null)
                {
                    interviewsheet.Visible = SessionUtil.CanAccess(this.ViewState, interviewsheet.CommandName);
                    interviewsheet.Attributes.Add("onclick", "openNewWindow('InterviewSheet', '', '"+Session["sitepath"]+"/Reports/ReportsViewWithSubReport.aspx?applicationcode=11&reportcode=OFFSHOREINTERVIEWSHEETNEW&interviewid=" + l.Text + "&showmenu=0&showword=NO&showexcel=NO');return true;");
                }

                if (drv["FLDPDSTATUS"].ToString().Equals(string.Empty))
                {
                    dp.Visible = false;
                }

                if (drv["FLDPROPOSALSTATUS"].ToString().ToUpper().Equals("CLOSED"))
                {
                    dp.Visible = false;
                }
            }
        }
    }

    protected void gvIntSum_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper() == "DEPLAN")
            {
                string crewPlanId = ((RadLabel)e.Item.FindControl("lblCrewPlanId")).Text;
                PhoenixCrewPlanning.DeleteCrewPlan(new Guid(crewPlanId), General.GetNullableInteger(ViewState["EMPID"].ToString()).Value);
                BindData();
                gvIntSum.Rebind();

                String scriptpopupclose = String.Format("javascript:parent.CloseWindow('codehelp1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);

            }
            if (e.CommandName.ToUpper() == "DELETE")
            {
                PhoenixCrewOffshoreInterview.DeleteAssessment(int.Parse(((RadLabel)e.Item.FindControl("lblInterviewId")).Text));
                BindData();
                gvIntSum.Rebind();

            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper() == "EDIT")
            {

                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                Response.Redirect("../CrewOffshore/CrewOffshoreEvaluationEdit.aspx?interviewid=" + int.Parse(((RadLabel)e.Item.FindControl("lblInterviewId")).Text) + "&newapp=" + ViewState["NEWAPP"].ToString() + "&dt=" + lblDTKey.Text);
                BindData();
                gvIntSum.Rebind();

            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
