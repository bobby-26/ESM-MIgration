using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;


public partial class CrewOffshoreTrainingCategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreTrainingCategory.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvOffshoreCategory')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreTrainingCategory.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuOffshoreCategory.AccessRights = this.ViewState;
            MenuOffshoreCategory.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvOffshoreCategory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            BindData();

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
        gvOffshoreCategory.Rebind();
    }

    protected void cmdSearch_Click(object sender, EventArgs e)

    {
      
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCATEGORYNAME", "FLDRANKGROUPNAME" };
        string[] alCaptions = { "Category", "Rank Category" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewOffshoreTrainingCategory.SearchTrainingCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            null, sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount, General.GetNullableInteger(ddlcompetencecategory.SelectedHard));

        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreCompetenceCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Training Category</h3></td>");
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

    protected void MenuOffshoreCategory_TabStripCommand(object sender, EventArgs e)
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

                String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreTrainingCategoryAdd.aspx?');");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCATEGORYNAME", "FLDRANKGROUPNAME" };
        string[] alCaptions = { "Category", "Rank Category" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCrewOffshoreTrainingCategory.SearchTrainingCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            null, sortexpression, sortdirection,
                //Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                //General.ShowRecords(null),
                Int32.Parse(ViewState["PAGENUMBER"].ToString()), gvOffshoreCategory.PageSize,
                ref iRowCount,
                ref iTotalPageCount,
                General.GetNullableInteger(ddlcompetencecategory.SelectedHard));

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreCategory", "Competence Category", alCaptions, alColumns, ds);


        gvOffshoreCategory.DataSource = ds;
        gvOffshoreCategory.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    private bool IsValidData(string Category, string rank)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Category.Equals(""))
            ucError.ErrorMessage = "Category is required.";

        if (rank == null || rank == "")
            ucError.ErrorMessage = "Rank Category is required";

        return (!ucError.IsError);
    }


    protected void gvOffshoreCategory_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                int Categoryid = int.Parse((e.Item as GridDataItem).GetDataKeyValue("FLDCATEGORYID").ToString());
                // int Categoryid = int.Parse(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixCrewOffshoreTrainingCategory.DeleteTrainingCategory(Categoryid);
                Rebind();
            }
             if (e.CommandName == "Page")
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
    protected void Rebind()
    {
        gvOffshoreCategory.SelectedIndexes.Clear();
        gvOffshoreCategory.EditIndexes.Clear();
        gvOffshoreCategory.DataSource = null;
        gvOffshoreCategory.Rebind();
    }

    protected void gvOffshoreCategory_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        if (e.Item is GridEditableItem)
        {
            int Categoryid = int.Parse((e.Item as GridDataItem).GetDataKeyValue("FLDCATEGORYID").ToString());

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "openNewWindow('Training Category', '','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreTrainingCategoryAdd.aspx?CategoryID=" + Categoryid + "'); return false;");
            }
        }
    }

    protected void gvOffshoreCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOffshoreCategory.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlcompetencecategory_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
}
