using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionQuestions : PhoenixBasePage
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
                ViewState["INSPECTIONID"] = "";

                if (Request.QueryString["INSPECTIONID"] != null && Request.QueryString["INSPECTIONID"].ToString() != string.Empty)
                    ViewState["INSPECTIONID"] = Request.QueryString["INSPECTIONID"].ToString();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvQuestions.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            toolbar.AddFontAwesomeButton("../Inspection/InspectionQuestions.aspx?INSPECTIONID=" + ViewState["INSPECTIONID"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvQuestions')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('QuestionEdit','Add','Inspection/InspectionQuestionAdd.aspx?INSPECTIONID=" + ViewState["INSPECTIONID"].ToString()+ "')", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuQuestions.AccessRights = this.ViewState;
            MenuQuestions.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = new string[] { };
            string[] alCaptions = new string[] { };

            alColumns = new string[] { "FLDDMSQUESTION", "FLDSORTORDER", "FLDACTIVE" };
            alCaptions = new string[] { "Question", "Sort Order", "Active" };


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

            ds = PhoenixInspectionQuestion.InspectionQuestionSearch(General.GetNullableGuid(ViewState["INSPECTIONID"].ToString())
                                                                      , sortexpression
                                                                      , 0
                                                                      , 1
                                                                      , iRowCount
                                                                      , ref iRowCount
                                                                      , ref iTotalPageCount);

            General.ShowExcel("Inspection Question", ds.Tables[0], alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    protected void MenuQuestions_TabStripCommand(object sender, EventArgs e)
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
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = null;
        gvQuestions.Rebind();
    }

    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            int nRow = int.Parse(ViewState["NROW"].ToString());

            RadLabel lblQuestionid = (RadLabel)gvQuestions.Items[nRow].FindControl("lblQuestionid");

            PhoenixInspectionQuestion.InspectionQuestionDelete(General.GetNullableGuid(lblQuestionid.Text));

            ucStatus.Text = "Deleted Successfully";
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvQuestions_ItemCommand(object sender, GridCommandEventArgs e)
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

                RadWindowManager1.RadConfirm("Are you sure you want to delete this Inspection Question?", "Confirm", 320, 150, null, ".");

            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
                gvQuestions.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvQuestions_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvQuestions.CurrentPageIndex + 1;
        BindData();
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

        ds = PhoenixInspectionQuestion.InspectionQuestionSearch(General.GetNullableGuid(ViewState["INSPECTIONID"].ToString())
                                                                      , sortexpression
                                                                      , sortdirection
                                                                      , gvQuestions.CurrentPageIndex + 1
                                                                      , gvQuestions.PageSize
                                                                      , ref iRowCount
                                                                      , ref iTotalPageCount);

        General.SetPrintOptions("gvQuestions", "Inspection Question", alCaptions, alColumns, ds);
        gvQuestions.DataSource = ds;
        gvQuestions.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvQuestions_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
        RadLabel lblQuestionid = (RadLabel)e.Item.FindControl("lblQuestionid");
        if (cmdEdit != null)
        {
            cmdEdit.Attributes.Add("onclick", "openNewWindow('QuestionEdit', 'Edit', '" + Session["sitepath"] + "/Inspection/InspectionQuestionAdd.aspx?QUESTIONID=" + lblQuestionid.Text + "&INSPECTIONID=" + ViewState["INSPECTIONID"].ToString() +"');return true;");
        }

        LinkButton cmdAnswer = (LinkButton)e.Item.FindControl("cmdAnswer");
        if (cmdAnswer != null)
        {
            cmdAnswer.Attributes.Add("onclick", "openNewWindow('Option', 'Options', '" + Session["sitepath"] + "/Inspection/InspectionQuestionOptions.aspx?QUESTIONID=" + lblQuestionid.Text + "');return true;");
        }
    }

    protected void gvQuestions_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        Rebind();
    }
    protected void Rebind()
    {
        gvQuestions.SelectedIndexes.Clear();
        gvQuestions.EditIndexes.Clear();
        gvQuestions.DataSource = null;
        gvQuestions.Rebind();
    }

}