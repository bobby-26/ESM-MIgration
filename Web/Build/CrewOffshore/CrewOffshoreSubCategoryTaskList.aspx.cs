using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Data.SqlClient;
using SouthNests.Phoenix.CrewOffshore;

public partial class CrewOffshore_CrewOffshoreSubCategoryTaskList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;

                BindRank();
                BindCategory();
            }
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreSubCategoryTaskList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvOffshoreSubCategory')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreSubCategoryTaskAdd.aspx?id=" + ddlCompetenceCategory.SelectedValue.ToString() + "&sid=" + ddsubcategory.SelectedValue.ToString() + "')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCOURSE");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreSubCategoryTaskList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreSubCategoryTaskList.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");

            //  toolbar.AddFontAwesomeButton("", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCOURSE");


            MenuOffshoreSubCategory.AccessRights = this.ViewState;
            MenuOffshoreSubCategory.MenuList = toolbar.Show();
            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindCategory()
    {
        DataTable dt = PhoenixCrewOffshoreSubCategoryTask.CategoryList();
        ddlCompetenceCategory.Items.Clear();
        ddlCompetenceCategory.DataValueField = "FLDCATEGORYID";
        ddlCompetenceCategory.DataTextField = "FLDCATEGORYNAME";


        ddlCompetenceCategory.DataSource = dt;
        ddlCompetenceCategory.DataBind();
    }
    public void BindRank()
    {
        DataTable dt = PhoenixCrewOffshoreSubCategoryTask.RankList();
        ddlrank.Items.Clear();
        ddlrank.DataValueField = "FLDRANKID";
        ddlrank.DataTextField = "FLDRANKNAME";


        ddlrank.DataSource = dt;
        ddlrank.DataBind();
    }
    protected void ddlCompetenceCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        DataTable dt = PhoenixCrewOffshoreSubCategoryTask.SubCategoryList(General.GetNullableInteger(ddlCompetenceCategory.SelectedValue.ToString()));
        ddsubcategory.Items.Clear();
        ddsubcategory.DataValueField = "FLDSUBCATEGORYID";
        ddsubcategory.DataTextField = "FLDNAME";

        ddsubcategory.DataSource = dt;
        ddsubcategory.DataBind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDTASKNAME","FLDRANKNAME", "FLDASSESSORNAME" };
        string[] alCaptions = { "Task Name","Applies To" ,"Assessor" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCrewOffshoreSubCategoryTask.SearchSubCategoryTask(General.GetNullableInteger(ddsubcategory.SelectedValue.ToString())
                , General.GetNullableInteger(ddllevel.SelectedValue)
                , General.GetNullableString(ddlrank.SelectedValue.ToString())
                , null
                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                , General.ShowRecords(null)
                , ref iRowCount
                , ref iTotalPageCount
               );

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreSubCategory", "Competence SubCategory", alCaptions, alColumns, ds);
        gvOffshoreSubCategoryTask.DataSource = ds;
        gvOffshoreSubCategoryTask.VirtualItemCount = iRowCount;
        //gvOffshoreSubCategory.DataBind();



        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        // SetPageNavigator();
    }

    protected void gvOffshoreSubCategoryTask_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOffshoreSubCategoryTask.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOffshoreSubCategory_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if(CommandName.ToUpper()== "FIND")
            {
                BindData();
                gvOffshoreSubCategoryTask.Rebind();
            }


            if (CommandName.ToUpper() == "CLEAR")
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreSubCategoryTaskList.aspx");
            }
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
        string[] alColumns = { "FLDTASKNAME", "FLDRANKNAME", "FLDASSESSORNAME" };
        string[] alCaptions = { "Task Name", "Applies To", "Assessor" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

      


        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewOffshoreSubCategoryTask.SearchSubCategoryTask(General.GetNullableInteger(ddsubcategory.SelectedValue.ToString())
                , General.GetNullableInteger(ddllevel.SelectedValue)
                , General.GetNullableString(ddlrank.SelectedValue.ToString())
                , null
                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                , General.ShowRecords(null)
                , ref iRowCount
                , ref iTotalPageCount
               );

        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreCompetenceSubCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Training SubCategory</h3></td>");
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
    protected void ddsubcategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddsubcategory.SelectedIndex > 0)
        {
            BindData();
            gvOffshoreSubCategoryTask.Rebind();
        }
    }

    protected void gvOffshoreSubCategoryTask_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            string tid = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDTASKID"].ToString();
            if (cmdEdit != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName))
                    cmdEdit.Visible = false;
                cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreSubCategoryTaskAdd.aspx?tid=" + tid + "'); return true;");
            }
        }
    }

    protected void gvOffshoreSubCategoryTask_ItemCommand(object sender, GridCommandEventArgs e)
    {
       if(e.CommandName.ToUpper()=="DELETE")
        {
            string tid = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDTASKID"].ToString();
            PhoenixCrewOffshoreSubCategoryTask.DeleteCompetencyTask(General.GetNullableInteger(tid));
            BindData();
            gvOffshoreSubCategoryTask.Rebind();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvOffshoreSubCategoryTask.Rebind();
    }
}