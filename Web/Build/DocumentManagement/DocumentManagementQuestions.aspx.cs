using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class DocumentManagementQuestions : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (!IsPostBack)
            {
                ucConfirm.Attributes.Add("style", "display:none");
                ViewState["SECTION"] = "";
                ViewState["SECTIONID"] = "";
                ViewState["REVISIONID"] = "";
                ViewState["PUBLISHEDYN"] = 0;

                if (Request.QueryString["SECTION"] != null && Request.QueryString["SECTION"].ToString() != string.Empty)
                    ViewState["SECTION"] = Request.QueryString["SECTION"].ToString();

                if (Request.QueryString["SECTIONID"] != null && Request.QueryString["SECTIONID"].ToString() != string.Empty)
                    ViewState["SECTIONID"] = Request.QueryString["SECTIONID"].ToString();

                if (Request.QueryString["REVISIONID"] != null && Request.QueryString["REVISIONID"].ToString() != string.Empty)
                {
                    ViewState["REVISIONID"] = Request.QueryString["REVISIONID"].ToString();
                }
                else
                {
                    ViewState["PUBLISHEDYN"] = 1;
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvDMSQuestion.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementQuestions.aspx?SECTIONID=" + ViewState["SECTIONID"].ToString() + "&REVISIONID=" + ViewState["REVISIONID"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDMSQuestion')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            if (ViewState["REVISIONID"] != null && ViewState["REVISIONID"].ToString() != string.Empty)
            {
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('QuestionEdit','Add','DocumentManagement/DocumentManagementQuestionAdd.aspx?SECTIONID=" + ViewState["SECTIONID"].ToString() + "&REVISIONID=" + ViewState["REVISIONID"].ToString() + "')", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            }
            toolbar.AddLinkButton("javascript:openNewWindow('QuestionEdit','Revision Remarks','DocumentManagement/DocumentManagementQuestionRevisionRemarks.aspx?SECTIONID=" + ViewState["SECTIONID"].ToString() + "&REVISIONID=" + ViewState["REVISIONID"].ToString() + "')", "Approve", "REVISE", ToolBarDirection.Right);

            MenuDMSQuestions.AccessRights = this.ViewState;
            MenuDMSQuestions.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void Rebind()
    {
        gvDMSQuestion.SelectedIndexes.Clear();
        gvDMSQuestion.EditIndexes.Clear();
        gvDMSQuestion.DataSource = null;
        gvDMSQuestion.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = new string[] { };
        string[] alCaptions = new string[] { };
        string doctitile = "";


        alColumns = new string[] { "FLDDMSQUESTION", "FLDSORTORDER", "FLDACTIVE" };
        alCaptions = new string[] { "Question", "Sort Order", "Active" };
        doctitile = "DMS Questions";


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

        DataSet ds = new DataSet();

        ds = PhoenixDocumentManagementQuestion.DMSQuestionSearch(General.GetNullableGuid(ViewState["SECTIONID"].ToString())
                                                                  , General.GetNullableGuid(ViewState["REVISIONID"].ToString())
                                                                  , sortexpression
                                                                  , 0
                                                                  , 1
                                                                  , iRowCount
                                                                  , ref iRowCount
                                                                  , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OpenReportList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>" + doctitile + "</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length).ToString() + "'><h3>");
        Response.Write("Section-"+ViewState["SECTION"].ToString());
        Response.Write("</h3></td>");
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
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = new string[] { };
        string[] alCaptions = new string[] { };
        alColumns = new string[] { "FLDDMSQUESTION", "FLDSORTORDER", "FLDACTIVE" };
        alCaptions = new string[] { "Question", "Sort Order", "Active" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixDocumentManagementQuestion.DMSQuestionSearch(General.GetNullableGuid(ViewState["SECTIONID"].ToString())
                                                                      , General.GetNullableGuid(ViewState["REVISIONID"].ToString())
                                                                      , sortexpression
                                                                      , sortdirection
                                                                      , gvDMSQuestion.CurrentPageIndex + 1
                                                                      , gvDMSQuestion.PageSize
                                                                      , ref iRowCount
                                                                      , ref iTotalPageCount);
        General.SetPrintOptions("gvDMSQuestion", "Test Question", alCaptions, alColumns, ds);
        gvDMSQuestion.DataSource = ds;
        gvDMSQuestion.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        //if(ViewState["PUBLISHEDYN"].ToString() == "1")
        //{
        //    gvDMSQuestion.Columns[3].Visible = false;
        //}
    }
    protected void MenuDMSQuestions_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void gvDMSQuestion_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["NROW"] = e.Item.ItemIndex;

                RadWindowManager1.RadConfirm("Are you sure you want to delete this DMS Question?", "Confirm", 320, 150, null, ".");

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

    protected void gvDMSQuestion_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDMSQuestion.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvDMSQuestion_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");

            if (ViewState["PUBLISHEDYN"].ToString() == "1")
            {
                if (cmdDelete != null)
                    cmdDelete.Visible = false;

                if (cmdEdit != null)
                    cmdEdit.Visible = false;
            }
            RadLabel lblQuestionid = (RadLabel)e.Item.FindControl("lblQuestionid");
            if (cmdEdit != null)
            {
                cmdEdit.Attributes.Add("onclick", "openNewWindow('QuestionEdit', 'Edit', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementQuestionAdd.aspx?QUESTIONID=" + lblQuestionid.Text + "&SECTIONID=" + ViewState["SECTIONID"].ToString() + "&REVISIONID=" + ViewState["REVISIONID"].ToString() + "');return true;");
            }

            LinkButton cmdAnswer = (LinkButton)e.Item.FindControl("cmdAnswer");
            if (cmdAnswer != null)
            {
                cmdAnswer.Attributes.Add("onclick", "openNewWindow('Option', 'Options', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementQuestionOptions.aspx?QUESTIONID=" + lblQuestionid.Text + "');return true;");
            }
        } 

    }

    protected void gvDMSQuestion_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        Rebind();
    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            int nRow = int.Parse(ViewState["NROW"].ToString());

            RadLabel lblQuestionid = (RadLabel)gvDMSQuestion.Items[nRow].FindControl("lblQuestionid");

            PhoenixDocumentManagementQuestion.DMSQuestionDelete(General.GetNullableGuid(lblQuestionid.Text));

            ucStatus.Text = "Deleted Successfully";
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvDMSQuestion.Rebind();
       
    } 
}