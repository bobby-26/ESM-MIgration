using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionIncidentNearMissSubCategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionIncidentNearMissSubCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvIncidentSubCategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionIncidentNearMissSubCategory.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuIncidentSubCategory.AccessRights = this.ViewState;
            MenuIncidentSubCategory.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvIncidentSubCategory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["CALLFROM"] != null && Request.QueryString["CALLFROM"].ToString() == "MACHINERYDAMAGE")
                    ViewState["CALLFROM"] = Request.QueryString["CALLFROM"].ToString();
                else
                    ViewState["CALLFROM"] = "";

                SetDetails();
                BindCategory();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SetDetails()
    {
        if (ViewState["CALLFROM"] != null && ViewState["CALLFROM"].ToString() == "MACHINERYDAMAGE")
        {
            ucTitle.Text = "Machinery Damage SubCategory";

            ddlType.Items.Insert(0, new RadComboBoxItem("Machinery Damage", "4"));
            ddlType.Visible = false;
            lblType.Visible = false;
            gvIncidentSubCategory.Columns[0].Visible = false;
        }
        else
        {
            ucTitle.Text = "Incident / Near Miss SubCategory";

            ddlType.Items.Insert(0, new RadComboBoxItem("Incident", "1"));
            ddlType.Items.Insert(1, new RadComboBoxItem("Near Miss", "2"));
            ddlType.Items.Insert(2, new RadComboBoxItem("Serious Near Miss", "3"));
            ddlType.Visible = true;
            lblType.Visible = true;
        }
    }

    protected void BindCategory()
    {
        DataSet ds = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(General.GetNullableInteger(ddlType.SelectedValue));
        ucCategory.CategoryList = ds;
        ucCategory.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ucCategory.TypeId = ddlType.SelectedValue;            
            ucCategory.SelectedCategory = ds.Tables[0].Rows[0]["FLDINCIDENTNEARMISSCATEGORYID"].ToString();            
        }
        
    }

    protected void ddlType_Changed(object sender, EventArgs e)
    {
        ucCategory.SelectedCategory = string.Empty;
        ucCategory.SelectedValue = string.Empty;
        BindCategory();
        // Rebind();
        gvIncidentSubCategory.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string strTitle = "";
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCATEGORYNAME", "FLDNAME" };
        string[] alCaptions = { "Category", "Subcategory" };

        string sortexpression;
        int? sortdirection = null;

        if (ViewState["CALLFROM"] != null && ViewState["CALLFROM"].ToString() == "MACHINERYDAMAGE")
            strTitle = "Machinery Damage Subcategory";
        else
            strTitle = "Incident / Near Miss Subcategory";

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        //ucCategory.DataBind();
        ds = PhoenixInspectionIncidentNearMissSubCategory.IncidentNearMissSubCategorySearch(
                General.GetNullableGuid(ucCategory.SelectedCategory)
                , General.GetNullableString(txtName.Text)
                , sortexpression, sortdirection
                , 1
                , iRowCount
                , ref iRowCount
                , ref iTotalPageCount
                , General.GetNullableInteger(ddlType.SelectedValue));

        Response.AddHeader("Content-Disposition", "attachment; filename=IncidentSubCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>" + strTitle + "</h3></td>");
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

    protected void IncidentSubCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvIncidentSubCategory.CurrentPageIndex = 0;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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

    protected void Rebind()
    {
        gvIncidentSubCategory.SelectedIndexes.Clear();
        gvIncidentSubCategory.EditIndexes.Clear();
        gvIncidentSubCategory.DataSource = null;
        gvIncidentSubCategory.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string strTitle = "";

        string[] alColumns = { "FLDCATEGORYNAME", "FLDNAME" };
        string[] alCaptions = { "Category", "Subcategory" };

        if (ViewState["CALLFROM"] != null && ViewState["CALLFROM"].ToString() == "MACHINERYDAMAGE")
            strTitle = "Machinery Damage Subcategory";
        else
            strTitle = "Incident / Near Miss Subcategory";

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionIncidentNearMissSubCategory.IncidentNearMissSubCategorySearch(
                General.GetNullableGuid(ucCategory.SelectedCategory)
                , General.GetNullableString(txtName.Text)
                , sortexpression, sortdirection
                , gvIncidentSubCategory.CurrentPageIndex + 1
                , gvIncidentSubCategory.PageSize
                , ref iRowCount
                , ref iTotalPageCount
                , General.GetNullableInteger(ddlType.SelectedValue));

        General.SetPrintOptions("gvIncidentSubCategory", strTitle, alCaptions, alColumns, ds);

        gvIncidentSubCategory.DataSource = ds;
        gvIncidentSubCategory.VirtualItemCount = iRowCount;
    }

    protected void gvIncidentSubCategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridFooterItem)
            {
                if (e.CommandName.ToUpper().Equals("ADD"))
                {
                    if (!IsValidIncidentSubCategory(((RadTextBox)e.Item.FindControl("txtNameAdd")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    InsertIncidentSubCategory(ucCategory.SelectedCategory,
                        ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text
                    );
                    Rebind();
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Focus();
                }
            }
            if (e.Item is GridEditableItem)
            {
                if (e.CommandName.ToUpper().Equals("UPDATE"))
                {
                    if (!IsValidIncidentSubCategory(((RadTextBox)e.Item.FindControl("txtNameEdit")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    UpdateIncidentSubCategory(new Guid(((RadLabel)e.Item.FindControl("lblSubCategoryIdEdit")).Text),
                        new Guid(ucCategory.SelectedCategory),
                         ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text
                     );
                    Rebind();
                }

                else if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    DeleteIncidentSubCategory(new Guid(((RadLabel)e.Item.FindControl("lblSubCategoryId")).Text));
                    Rebind();
                }
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

    protected void gvIncidentSubCategory_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
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

    private void InsertIncidentSubCategory(string categoryid, string name)
    {
        PhoenixInspectionIncidentNearMissSubCategory.InsertIncidentNearMissSubCategory(new Guid(categoryid), name);
        ucStatus.Text = "Information added";
    }

    private void UpdateIncidentSubCategory(Guid subcategoryid, Guid categoryid, string name)
    {
        PhoenixInspectionIncidentNearMissSubCategory.UpdateIncidentNearMissSubCategory(subcategoryid, categoryid, name);
        ucStatus.Text = "Information updated";
    }

    private bool IsValidIncidentSubCategory(string name)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //GridView _gridView = gvIncidentSubCategory;
        if (General.GetNullableGuid(ucCategory.SelectedCategory) == null)
            ucError.ErrorMessage = "Category is required.";

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "SubCategory is required.";

        return (!ucError.IsError);
    }

    private void DeleteIncidentSubCategory(Guid subcategoryid)
    {
        PhoenixInspectionIncidentNearMissSubCategory.DeleteIncidentNearMissSubCategory(subcategoryid);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvIncidentSubCategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvIncidentSubCategory.CurrentPageIndex + 1;

        BindData();
    }

    protected void ucCategory_TextChangedEvent(object sender, EventArgs e)
    {
        gvIncidentSubCategory.Rebind();
    }
}
