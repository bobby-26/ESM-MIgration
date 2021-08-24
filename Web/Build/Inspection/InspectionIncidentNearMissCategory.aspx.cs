using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionIncidentNearMissCategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionIncidentNearMissCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvIncidentCategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionIncidentNearMissCategory.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuIncidentCategory.AccessRights = this.ViewState;
            MenuIncidentCategory.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvIncidentCategory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["CALLFROM"] != null && Request.QueryString["CALLFROM"].ToString() == "MACHINERYDAMAGE")
                    ViewState["CALLFROM"] = Request.QueryString["CALLFROM"].ToString();
                else
                    ViewState["CALLFROM"] = "";
                SetDetails();
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
            ucTitle.Text = "Machinery Damage Category";

            ddlType.Items.Insert(0, new RadComboBoxItem("Machinery Damage", "4"));
            ddlType.Visible = false;
            //tblGuidenceNotes.Visible = false;
            lblType.Visible = false;
            // gvIncidentCategory.Columns[1].Visible = false;
        }
        else
        {
            ucTitle.Text = "Incident / Near Miss Category";

            ddlType.Items.Insert(0, new RadComboBoxItem("Incident", "1"));
            ddlType.Items.Insert(1, new RadComboBoxItem("Near Miss", "2"));
            ddlType.Items.Insert(2, new RadComboBoxItem("Serious Near Miss", "3"));
            //  tblGuidenceNotes.Visible = true;
            lblType.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string strTitle = "";
        string[] alColumns = new string[3];
        string[] alCaptions = new string[3];

        if (ViewState["CALLFROM"] != null && ViewState["CALLFROM"].ToString() == "MACHINERYDAMAGE")
        {
            alColumns = new string[2];
            alCaptions = new string[2];

            alColumns[0] = "FLDSHORTCODE";
            alColumns[1] = "FLDNAME";
            alCaptions[0] = "Code";
            alCaptions[1] = "Name";
            strTitle = "Machinery Damage Category";
        }
        else
        {
            alColumns[0] = "FLDTYPENAME";
            alColumns[1] = "FLDSHORTCODE";
            alColumns[2] = "FLDNAME";

            alCaptions[0] = "Type";
            alCaptions[1] = "Code";
            alCaptions[2] = "Name";
            strTitle = "Incident / Near Miss Category";
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

        ds = PhoenixInspectionIncidentNearMissCategory.IncidentNearMissCategorySearch(General.GetNullableString(txtName.Text)
                , General.GetNullableInteger(ddlType.SelectedValue)
                , sortexpression, sortdirection
                , 1
                , iRowCount
                , ref iRowCount
                , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=IncidentCategory.xls");
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

    protected void IncidentCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvIncidentCategory.CurrentPageIndex = 0;
                ViewState["PAGENUMBER"] = 1;
                gvIncidentCategory.Rebind();
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string strTitle = "";
        string[] alColumns = new string[3];
        string[] alCaptions = new string[3];

        if (ViewState["CALLFROM"] != null && ViewState["CALLFROM"].ToString() == "MACHINERYDAMAGE")
        {
            alColumns = new string[2];
            alCaptions = new string[2];

            alColumns[0] = "FLDSHORTCODE";
            alColumns[1] = "FLDNAME";
            alCaptions[0] = "Code";
            alCaptions[1] = "Name";
            strTitle = "Machinery Damage Category";
        }
        else
        {
            alColumns[0] = "FLDTYPENAME";
            alColumns[1] = "FLDSHORTCODE";
            alColumns[2] = "FLDNAME";

            alCaptions[0] = "Type";
            alCaptions[1] = "Code";
            alCaptions[2] = "Name";
            strTitle = "Incident / Near Miss Category";
        }

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionIncidentNearMissCategory.IncidentNearMissCategorySearch(General.GetNullableString(txtName.Text)
                , General.GetNullableInteger(ddlType.SelectedValue)
                , sortexpression, sortdirection
                , gvIncidentCategory.CurrentPageIndex + 1
                , gvIncidentCategory.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

        General.SetPrintOptions("gvIncidentCategory", strTitle, alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["CALLFROM"] != null && ViewState["CALLFROM"].ToString() == "MACHINERYDAMAGE")
                gvIncidentCategory.Columns[0].Visible = false;
            else
                gvIncidentCategory.Columns[0].Visible = false;
        }
        else
        {
            DataTable dt = ds.Tables[0];
        }
        gvIncidentCategory.DataSource = ds;
        gvIncidentCategory.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void Rebind()
    {
        gvIncidentCategory.SelectedIndexes.Clear();
        gvIncidentCategory.EditIndexes.Clear();
        gvIncidentCategory.DataSource = null;
        gvIncidentCategory.Rebind();
    }
    protected void gvIncidentCategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidIncidentCategory(((RadTextBox)e.Item.FindControl("txtShortCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertIncidentCategory(
                    ((RadTextBox)e.Item.FindControl("txtShortCodeAdd")).Text.Trim()
                    , ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text.Trim()
                );
                ((RadTextBox)e.Item.FindControl("txtNameAdd")).Focus();
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidIncidentCategory(((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text, ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateIncidentCategory(
                    new Guid(((RadLabel)e.Item.FindControl("lblCategoryIdEdit")).Text)
                    , ((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text.Trim()
                    , ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text.Trim()
                 );
                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteIncidentCategory(new Guid(((RadLabel)e.Item.FindControl("lblCategoryId")).Text));
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvIncidentCategory_ItemDataBound(object sender, GridItemEventArgs e)
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

    private void InsertIncidentCategory(string code, string name)
    {
        PhoenixInspectionIncidentNearMissCategory.InsertIncidentNearMissCategory(int.Parse(ddlType.SelectedValue), code, name);
        ucStatus.Text = "Information added";
    }

    private void UpdateIncidentCategory(Guid categoryid, string code, string name)
    {
        PhoenixInspectionIncidentNearMissCategory.UpdateIncidentNearMissCategory(categoryid, int.Parse(ddlType.SelectedValue), code, name);
        ucStatus.Text = "Information updated";
    }

    private bool IsValidIncidentCategory(string code, string name)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (code.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        return (!ucError.IsError);
    }

    private void DeleteIncidentCategory(Guid categoryid)
    {
        PhoenixInspectionIncidentNearMissCategory.DeleteIncidentNearMissCategory(categoryid);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvIncidentCategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvIncidentCategory.CurrentPageIndex + 1;

        BindData();
    }

    protected void ddlType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        gvIncidentCategory.Rebind();
    }
}
