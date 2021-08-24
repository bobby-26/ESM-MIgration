using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersCountryVisaDocument : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersCountryVisaDocument.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCountryVisaDocument')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        
        MenuRegistersCountryVisaDocument.AccessRights = this.ViewState;
        MenuRegistersCountryVisaDocument.MenuList = toolbar.Show();        
        
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["VISAID"] = Request.QueryString["visaid"];
            gvCountryVisaDocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            lblTitle.Text = Request.QueryString["countryname"];
        }
        toolbar = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.Title = lblTitle.Text;
        MenuTitle.MenuList = toolbar.Show();
    }

    protected void RegistersCountryVisaDocument_TabStripCommand(object sender, EventArgs e)
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
    
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDHARDNAME", "FLDDOCUMENTSPECIFICATION" };
        string[] alCaptions = { "Document Name", "Document Specification" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersCountryVisaDocument.CountryVisaDocumentSearch(
            General.GetNullableInteger(ViewState["VISAID"].ToString()),
            null, sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"], gvCountryVisaDocument.PageSize,
            ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvCountryVisaDocument", "Country Visa Document", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCountryVisaDocument.DataSource = ds;
            gvCountryVisaDocument.VirtualItemCount = iRowCount;
        }
        else
        {
            gvCountryVisaDocument.DataSource = "";
        }
    }

    private bool IsValidCountryVisaDocument(string documentID, string documentspec)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(documentID) == null)
            ucError.ErrorMessage = "Document Name is required.";

        if (documentspec.Trim().Equals(""))
            ucError.ErrorMessage = "Document Specification is required.";

        return (!ucError.IsError);
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDHARDNAME", "FLDDOCUMENTSPECIFICATION" };
        string[] alCaptions = { "Document Name", "Document Specification" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersCountryVisaDocument.CountryVisaDocumentSearch(
            Convert.ToInt32(ViewState["VISAID"].ToString()), 
            null, sortexpression, sortdirection, 
            (int)ViewState["PAGENUMBER"], gvCountryVisaDocument.PageSize, 
            ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=CountryVisa.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Country Visa Document</h3></td>");
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

   
    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }

    private void InsertCountryVisaDocument(string documentid, string documentspecification)
    {
        if (!IsValidCountryVisaDocument(
                documentid,
                documentspecification))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersCountryVisaDocument.InsertCountryVisaDocument(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
            General.GetNullableInteger(ViewState["VISAID"].ToString()), 
            General.GetNullableInteger(documentid), documentspecification);
    }

    private void UpdateCountryVisaDocument(string visaDocumentid, string documentid, string documentspecification)
    {
        if (!IsValidCountryVisaDocument(
                documentid,
                documentspecification))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersCountryVisaDocument.Updatecountryvisadocument(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
            General.GetNullableInteger(visaDocumentid), 
            General.GetNullableInteger(ViewState["VISAID"].ToString()), 
            General.GetNullableInteger(documentid), documentspecification);
    }

    private void DeleteCountryVisaDocument(string visadocumentid)
    {
        PhoenixRegistersCountryVisaDocument.DeleteCountryVisaDocument(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(visadocumentid));
    }


    protected void gvCountryVisaDocument_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCountryVisaDocument.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCountryVisaDocument_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertCountryVisaDocument(
                   ((UserControlHard)e.Item.FindControl("ddlDocumentNameAdd")).SelectedHard,
                    ((RadTextBox)e.Item.FindControl("txtDocumentSpecificationAdd")).Text);
                BindData();
                gvCountryVisaDocument.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteCountryVisaDocument(((RadLabel)e.Item.FindControl("lblVisaDocumentID")).Text);
                BindData();
                gvCountryVisaDocument.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                UpdateCountryVisaDocument(
              ((RadLabel)e.Item.FindControl("lblVisaDocumentIDEdit")).Text,
              ((UserControlHard)e.Item.FindControl("ddlDocumentNameEdit")).SelectedHard,
              ((RadTextBox)e.Item.FindControl("txtDocumentSpecificationEdit")).Text
              );
                BindData();
                gvCountryVisaDocument.Rebind();
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

    protected void MenuRegistersCountryVisaDocument_TabStripCommand(object sender, EventArgs e)
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

    protected void gvCountryVisaDocument_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
                        
            UserControlHard ucHard = (UserControlHard)e.Item.FindControl("ddlDocumentNameEdit");
            DataRowView drvHard = (DataRowView)e.Item.DataItem;
            if (ucHard != null) ucHard.SelectedHard = drvHard["FLDDOCUMENTID"].ToString();

            RadLabel lblDocumentSpecification = (RadLabel)e.Item.FindControl("lblDocumentSpecification");
            UserControlToolTip ucDocumentSpecTT = (UserControlToolTip)e.Item.FindControl("ucDocumentSpecTT");
            if (lblDocumentSpecification != null)
            {
                lblDocumentSpecification.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucDocumentSpecTT.ToolTip + "', 'visible');");
                lblDocumentSpecification.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucDocumentSpecTT.ToolTip + "', 'hidden');");
            }
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

    protected void gvCountryVisaDocument_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
