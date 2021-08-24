using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersMedicalDeclaration : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersMedicalDeclaration.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDocumentMedical')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersMedicalDeclaration.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Registers/RegistersMedicalDeclaration.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");        
        MenuRegistersDocumentMedical.AccessRights = this.ViewState;
        MenuRegistersDocumentMedical.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvDocumentMedical.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        toolbar = new PhoenixToolbar();
        //MenuTitle.AccessRights = this.ViewState;
        //MenuTitle.MenuList = toolbar.Show();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDNAMEOFDECLARATION" };
        string[] alCaptions = { "Declaration" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string MedicalSearch = (txtSearchMedical.Text == null) ? "" : txtSearchMedical.Text;

        ds = PhoenixRegistersDocumentMedical.DocumentDeclarationSearch(MedicalSearch, sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvDocumentMedical.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=MedicalDeclaration.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Medical Declaration</h3></td>");
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

    protected void RegistersDocumentMedical_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvDocumentMedical.Rebind();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            txtSearchMedical.Text = "";
            BindData();
            gvDocumentMedical.Rebind();
        }        
        else if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();            
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAMEOFDECLARATION" };
        string[] alCaptions = { "Declaration" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string MedicalSearch = (txtSearchMedical.Text == null) ? "" : txtSearchMedical.Text;

        DataSet ds = PhoenixRegistersDocumentMedical.DocumentDeclarationSearch(MedicalSearch, sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvDocumentMedical.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvDocumentMedical", "Medical Declaration", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDocumentMedical.DataSource = ds;
            gvDocumentMedical.VirtualItemCount = iRowCount;
        }
        else
        {
            gvDocumentMedical.DataSource = "";
        }
    }
    private void InsertDocumentMedical(string nameofmedical)
    {
        if (!IsValidDocumentMedical(nameofmedical))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersDocumentMedical.InsertDocumentDeclaration(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , nameofmedical);
    }

    private void UpdateDocumentMedical(int documentmedicalid, string nameofmedical)
    {
        if (!IsValidDocumentMedical(nameofmedical))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersDocumentMedical.UpdateDocumentDeclaration(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , documentmedicalid
            , nameofmedical.Trim());
    }

    private bool IsValidDocumentMedical(string nameofmedical)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (nameofmedical.Trim().Equals(""))
            ucError.ErrorMessage = "Declaration is required.";

        return (!ucError.IsError);
    }

    private void DeleteDocumentMedical(int documentmedicalid)
    {
        PhoenixRegistersDocumentMedical.DeleteDocumentDeclaration(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , documentmedicalid);
    }
    
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvDocumentMedical_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if(e.CommandName.ToUpper().Equals("SORT")) return;
       
       else if (e.CommandName.ToUpper().Equals("ADD"))
        {
            InsertDocumentMedical(
                ((RadTextBox)e.Item.FindControl("txtNameOfMedicalAdd")).Text
            );
            BindData();
            gvDocumentMedical.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            string name = ((RadTextBox)e.Item.FindControl("txtNameOfMedicalEdit")).Text;
            if (!IsValidDocumentMedical(name))
            {
                ucError.Visible = true;
                return;
            }
            UpdateDocumentMedical(
                  Int32.Parse(((RadLabel)e.Item.FindControl("lblDocumentMedicalIdEdit")).Text),
                  name
              );
            BindData();
            gvDocumentMedical.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            DeleteDocumentMedical(Int32.Parse(((RadLabel)e.Item.FindControl("lblDocumentMedicalId")).Text));
            BindData();
            gvDocumentMedical.Rebind();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvDocumentMedical_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDocumentMedical.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvDocumentMedical_ItemDataBound(object sender, GridItemEventArgs e)
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

    protected void gvDocumentMedical_SortCommand(object sender, GridSortCommandEventArgs e)
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
