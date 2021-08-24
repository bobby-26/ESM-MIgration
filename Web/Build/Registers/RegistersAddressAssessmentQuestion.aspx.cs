using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;


public partial class Registers_RegistersAddressAssessmentQuestion : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersAddressAssessmentQuestion.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvQuestion')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersAddressAssessmentQuestion.aspx?" + Request.QueryString, "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Registers/RegistersAddressAssessmentQuestion.aspx?" + Request.QueryString, "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "RESET");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('BookMarkScript','','Registers/RegistersAddressAssessmentQuestionAdd.aspx',false,450,300)", "Category Level", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        MenuRegistersQuestion.AccessRights = this.ViewState;
        MenuRegistersQuestion.MenuList = toolbar.Show();

        gvQuestion.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
    }

    protected void gvQuestion_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvQuestion.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRegistersQuestion_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvQuestion.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtquestion.Text = "";
                chkisactive.Checked = false;
                ViewState["PAGENUMBER"] = 1;

                gvQuestion.Rebind();
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
        string[] alColumns = { "FLDQUESTION", "FLDISACTIVE", "FLDREQUIREREMARK" };
        string[] alCaptions = { "Question", "Active Y/N", "Require Remark ?", };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixAddressAssessmentQuestion.Questionsearch(txtquestion.Text
                                                                           , General.GetNullableInteger(chkisactive.Checked == true ? "1" : "")
                                                                           , sortdirection
                                                                           , (int)ViewState["PAGENUMBER"]
                                                                           , gvQuestion.PageSize
                                                                           , ref iRowCount
                                                                           , ref iTotalPageCount
                                                                     );


        Response.AddHeader("Institute Assessment Question", "attachment; filename=Institute_Assessment_Question.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Department</h3></td>");
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
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDQUESTION", "FLDISACTIVE", "FLDREQUIREREMARK" };
        string[] alCaptions = { "Question", "Active Y/N", "Require Remark ?", };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = PhoenixAddressAssessmentQuestion.Questionsearch(txtquestion.Text
                                                                          , General.GetNullableInteger(chkisactive.Checked == true ? "1" : "")
                                                                           , sortdirection
                                                                           , (int)ViewState["PAGENUMBER"]
                                                                           , gvQuestion.PageSize
                                                                           , ref iRowCount
                                                                           , ref iTotalPageCount
                                                                     );
        General.SetPrintOptions("gvQuestion", "Address Assessment Question", alCaptions, alColumns, ds);
        gvQuestion.DataSource = ds;
        gvQuestion.VirtualItemCount = iRowCount;

    }

    protected void gvQuestion_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblQuestionId = (RadLabel)e.Item.FindControl("lblQuestionId");
            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                cmdEdit.Attributes.Add("onclick", "openNewWindow('BookMarkScript','','" + Session["sitepath"] + "/Registers/RegistersAddressAssessmentQuestionAdd.aspx?qid=" + lblQuestionId.Text + "',false,450,300);");

        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvQuestion.Rebind();
    }

    protected void gvQuestion_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SELECT")
        {
            RadLabel lblQuestionId = (RadLabel)e.Item.FindControl("lblQuestionId");
            Response.Redirect("RegisterAddressAssessmentOptions.aspx?qid=" + lblQuestionId.Text + "", false);
        }
    }
}