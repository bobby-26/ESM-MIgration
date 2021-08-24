using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class DocumentManagementQuestionOptions : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        { 
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (!IsPostBack)
            {
                ViewState["QUESTIONID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                if (Request.QueryString["QUESTIONID"] != null && Request.QueryString["QUESTIONID"].ToString() != string.Empty)
                {
                    ViewState["QUESTIONID"] = Request.QueryString["QUESTIONID"].ToString();
                    DataSet dm = new DataSet();
                    dm = PhoenixDocumentManagementQuestion.DMSQuestionEdit(General.GetNullableGuid(ViewState["QUESTIONID"].ToString()));
                    if (dm.Tables[0].Rows.Count > 0)
                    {
                        txtQuestion.Text = dm.Tables[0].Rows[0]["FLDDMSQUESTION"].ToString();
                        gvDMSQuestionOptions.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                    }

                }
            }
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementQuestionOptions.aspx?QUESTIONID=" + ViewState["QUESTIONID"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDMSQuestionOptions')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('OptionEdit','Add','DocumentManagement/DocumentManagementQuestionOptionAdd.aspx?QUESTIONID=" + ViewState["QUESTIONID"].ToString() + "')", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuDMSQuestionOptions.AccessRights = this.ViewState;
            MenuDMSQuestionOptions.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void Rebind()
    {
        gvDMSQuestionOptions.SelectedIndexes.Clear();
        gvDMSQuestionOptions.EditIndexes.Clear();
        gvDMSQuestionOptions.DataSource = null;
        gvDMSQuestionOptions.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = new string[] { };
        string[] alCaptions = new string[] { };
        string doctitile = "";


        alColumns = new string[] {"FLDDMSOPTION", "FLDSORTORDER", "FLDCORRECTANSWER", "FLDACTIVE" };
        alCaptions = new string[] { "Answers", "Sort Order", "Correct Answer YN", "Active" };
        doctitile = "DMS Question Answers";


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

        ds = PhoenixDocumentManagementQuestion.DMSQuestionOptionSearch(General.GetNullableGuid(ViewState["QUESTIONID"].ToString())
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
        Response.Write("Question-" + txtQuestion.Text);
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

        alColumns = new string[] { "FLDDMSOPTION", "FLDSORTORDER", "FLDCORRECTANSWER", "FLDACTIVE" };
        alCaptions = new string[] { "Answers", "Sort Order", "Correct Answer YN", "Active" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixDocumentManagementQuestion.DMSQuestionOptionSearch(General.GetNullableGuid(ViewState["QUESTIONID"].ToString())
                                                                      , sortexpression
                                                                      , sortdirection
                                                                      , gvDMSQuestionOptions.CurrentPageIndex + 1
                                                                      , gvDMSQuestionOptions.PageSize
                                                                      , ref iRowCount
                                                                      , ref iTotalPageCount);

        General.SetPrintOptions("gvDMSQuestionOptions", "Test Options", alCaptions, alColumns, ds);
        gvDMSQuestionOptions.DataSource = ds;
        gvDMSQuestionOptions.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void MenuDMSQuestionOptions_TabStripCommand(object sender, EventArgs e)
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

    protected void gvDMSQuestionOptions_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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

                RadWindowManager1.RadConfirm("Are you sure you want to delete this DMS Option?", "Confirm", 320, 150, null, "DMSAnswer");

            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDMSQuestionOptions_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDMSQuestionOptions.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDMSQuestionOptions_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            RadLabel lblOptionid = (RadLabel)e.Item.FindControl("lblOptionid");
            if (cmdEdit != null)
            {
                cmdEdit.Attributes.Add("onclick", "openNewWindow('OptionEdit', 'Edit', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementQuestionOptionAdd.aspx?OPTIONID=" + lblOptionid.Text + "&QUESTIONID=" + ViewState["QUESTIONID"].ToString() + "');return true;");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDMSQuestionOptions_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        try
        {
            ViewState["SORTEXPRESSION"] = e.SortExpression;

            if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
                ViewState["SORTDIRECTION"] = 1;
            else
                ViewState["SORTDIRECTION"] = 0;

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            int nRow = int.Parse(ViewState["NROW"].ToString());

            RadLabel lblOptionid = (RadLabel)gvDMSQuestionOptions.Items[nRow].FindControl("lblOptionid");

            PhoenixDocumentManagementQuestion.DMSQuestionOptionDelete(General.GetNullableGuid(lblOptionid.Text));

            ucStatus.Text = "Deleted Successfully";
            gvDMSQuestionOptions.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }
}