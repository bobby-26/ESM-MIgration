using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;


public partial class Registers_RegistersDebriefingCategory : PhoenixBasePage
{
 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersDebriefingCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDebriefing')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersDebriefingCategory.aspx" , "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersDebriefingCategory.aspx" , "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");            
            MenuRegistersDebriefing.AccessRights = this.ViewState;
            MenuRegistersDebriefing.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvDebriefing.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRegistersDebriefing_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvDebriefing.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtCode.Text = "";
                txtName.Text = "";
                BindData();
                gvDebriefing.Rebind();
            }
            if(CommandName.ToUpper().Equals("EXCEL"))
            {
                Showexcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void Showexcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCODE", "FLDCATEGORYNAME", "FLDSORTORDER" };
        string[] alCaptions = { "Code", " Name", "Sort Order" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersDebriefingCategory.CategorySearch(txtCode.Text, txtName.Text,
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            gvDebriefing.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=De-Briefing Category.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>De-Briefing Category</h3></td>");
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


    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCODE", "FLDCATEGORYNAME", "FLDSORTORDER" };
        string[] alCaptions = { "Code", " Name", "Sort Order" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = PhoenixRegistersDebriefingCategory.CategorySearch(txtCode.Text, txtName.Text, 
            sortexpression, sortdirection,(int)ViewState["PAGENUMBER"], 
            gvDebriefing.PageSize, 
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvDebriefing", "De-Briefing Category", alCaptions, alColumns, ds);


        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDebriefing.DataSource = ds;
            gvDebriefing.VirtualItemCount = iRowCount;
        }
        else
        {
            gvDebriefing.DataSource = "";
        }
    }

    

    //protected void gvDebriefing_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            
    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {
    //            if (!IsValidCategory(((RadTextBox)_gridView.FooterRow.FindControl("txtCodeAdd")).Text,
    //                ((RadTextBox)_gridView.FooterRow.FindControl("txtNameAdd")).Text,
    //                ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtSortAdd")).Text))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            InsertCategory(
    //               ((RadTextBox)_gridView.FooterRow.FindControl("txtCodeAdd")).Text.Trim(),
    //               ((RadTextBox)_gridView.FooterRow.FindControl("txtNameAdd")).Text.Trim(),
    //               int.Parse(((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtSortAdd")).Text)
    //               );
    //            BindData();
    //            ((RadTextBox)_gridView.FooterRow.FindControl("txtCodeAdd")).Focus();
    //        }
    //        if (e.CommandName.ToUpper().Equals("UPDATE"))
    //        {

    //            if (!IsValidCategory(((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtCodeEdit")).Text
    //                , ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtNameEdit")).Text,
    //                ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtSortEdit")).Text))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }
    //            UpdateCategory(
    //                 Int16.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCategoryEditId")).Text.Trim()),
    //                 ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtCodeEdit")).Text.Trim()
    //                 , ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtNameEdit")).Text.Trim(),
    //                  Int16.Parse(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtSortEdit")).Text)
    //                              );
    //            _gridView.EditIndex = -1;
    //            BindData();
            
    //        }

    //        else if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            DeleteCategory(int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCategoryId")).Text));
    //            BindData();
    //        }
    //    }
    //    catch(Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private void InsertCategory(string code,string name,int sortorder)
    {
        PhoenixRegistersDebriefingCategory.InsertCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, code, name, sortorder);
    }

    private void UpdateCategory(int categoryid,string code,string name,int sortorder)
    {
        PhoenixRegistersDebriefingCategory.UpdateCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,categoryid, code, name, sortorder);

    }

    private void DeleteCategory(int categoryid)
    {
        PhoenixRegistersDebriefingCategory.DeleteCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,categoryid);

    }
    //protected void gvDebriefing_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSION"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (db != null)
    //        {
    //            db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //        db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
    //        }
    //        ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

    //        ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
    //        if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

    //        ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
    //        if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

    //    }       
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //        {
    //            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
    //            if (db != null)
    //            {
    //                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
    //                    db.Visible = false;
    //            }
    //        }
    //}

   
    private bool IsValidCategory(string code, string name, string sortorder)
    {
        ucError.HeaderMessage = "Please provide the following required information";
       
        if (code.Trim().Equals("")&&code.TrimStart().Equals("")&&code.TrimEnd().Equals(""))
            ucError.ErrorMessage = "Code is required.";
        if (name.Trim().Equals("") && name.TrimStart().Equals("") && name.TrimEnd().Equals(""))
            ucError.ErrorMessage = "Name is required.";
        if(sortorder.Trim().Equals(""))
            ucError.ErrorMessage = "Sort Order is required.";
        return (!ucError.IsError);
    }

   
    public StateBag ReturnViewState()
    {
        return ViewState;
    }



    protected void gvDebriefing_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {        
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidCategory(((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("txtSortAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertCategory(
                   ((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text.Trim(),
                   ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text.Trim(),
                   int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSortAdd")).Text)
                   );
                BindData();
                gvDebriefing.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidCategory(((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text
                    , ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("txtSortEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateCategory(
                     Int16.Parse(((RadLabel)e.Item.FindControl("lblCategoryEditId")).Text.Trim()),
                     ((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text.Trim()
                     , ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text.Trim(),
                      Int16.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSortEdit")).Text)
                                  );                
                BindData();
                gvDebriefing.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteCategory(int.Parse(((RadLabel)e.Item.FindControl("lblCategoryId")).Text));
                BindData();
                gvDebriefing.Rebind();
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

    protected void gvDebriefing_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDebriefing.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvDebriefing_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
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

    protected void gvDebriefing_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
}
