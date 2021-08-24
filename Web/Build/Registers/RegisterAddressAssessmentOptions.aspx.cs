using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Registers;

public partial class Registers_RegisterAddressAssessmentOptions : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
      

        if (!IsPostBack)
        {
            ViewState["qid"] = "";
            if (Request.QueryString["qid"] != null)
            {
                ViewState["qid"] = Request.QueryString["qid"].ToString();
                DataTable dt = PhoenixAddressAssessmentQuestion.InsterviewAssessmentQuestionList(General.GetNullableInteger(ViewState["qid"].ToString()),null);
                lblQuestionName.Text = dt.Rows[0]["FLDQUESTION"].ToString();
            }
            gvOption.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegisterAddressAssessmentOptions.aspx?e=1", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOption')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('BookMarkScript','','Registers/RegisterAddressAssessmentOptionsAdd.aspx?qid=" + ViewState["qid"] .ToString()+ "',false,450,300)", "Category Level", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

        MenuRegistersQuestionOptions.AccessRights = this.ViewState;
        MenuRegistersQuestionOptions.MenuList = toolbar.Show();

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Back", "BACK", ToolBarDirection.Right);

        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar1.Show();

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvOption.Rebind();
    }
    protected void gvOption_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOption.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDQUESTION", "FLDOPTIONNAME" };
        string[] alCaptions = { "Question", "Options" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = PhoenixRegistersAddressAssessmentOptions.QuestionOptionsearch(General.GetNullableInteger(ViewState["qid"].ToString())
                                                                            , sortexpression
                                                                           , sortdirection
                                                                           , (int)ViewState["PAGENUMBER"]
                                                                           , gvOption.PageSize
                                                                           , ref iRowCount
                                                                           , ref iTotalPageCount
                                                                     );
        General.SetPrintOptions("gvOption", "Address Assessment Question Options", alCaptions, alColumns, ds);
        gvOption.DataSource = ds;
        gvOption.VirtualItemCount = iRowCount;
    }

    protected void MenuRegistersQuestionOptions_TabStripCommand(object sender, EventArgs e)
    {
        
    }

    protected void gvOption_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblQuestionId = (RadLabel)e.Item.FindControl("lblQuestionId");
            RadLabel lblOptionid = (RadLabel)e.Item.FindControl("lblOptionid");
            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                cmdEdit.Attributes.Add("onclick", "openNewWindow('BookMarkScript','','" + Session["sitepath"] + "/Registers/RegisterAddressAssessmentOptionsAdd.aspx?qid=" + lblQuestionId.Text + "&oid="+ lblOptionid.Text +"',false,450,300);");

        }
    }

    protected void MenuTitle_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper() == "BACK")
        {
            Response.Redirect("RegistersAddressAssessmentQuestion.aspx", false);
        }
    }
}