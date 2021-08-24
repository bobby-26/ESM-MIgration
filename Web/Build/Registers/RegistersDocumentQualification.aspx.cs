using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Configuration;
using Telerik.Web.UI;

public partial class RegistersDocumentQualification : PhoenixBasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersDocumentQualification.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDocumentQualification')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");        
        MenuRegistersDocumentQualification.AccessRights = this.ViewState;
        MenuRegistersDocumentQualification.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        //MenuTitle.AccessRights = this.ViewState;
        //MenuTitle.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvDocumentQualification.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDQUALIFICATION", "FLDEXPLANATION", "FLDPRESEAYN", "FLDACTIVEYNSTATUS" };
        string[] alCaptions = { "Qualification", "Explanation", "PreSeaYN", "Active Y/N" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

       
        ds = PhoenixRegistersDocumentQualification.DocumentQualificationSearch("", "", sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvDocumentQualification.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Qualification.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Qualification</h3></td>");
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
        Response.Write(ConfigurationManager.AppSettings["softwarename"].ToString());
        Response.End();
    }

    protected void RegistersDocumentQualification_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvDocumentQualification.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDQUALIFICATION", "FLDEXPLANATION","FLDPRESEAYN","FLDACTIVEYNSTATUS" };
        string[] alCaptions = { "Qualification", "Explanation","PreSeaYN","Active Y/N" };
        
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        
        DataSet ds = PhoenixRegistersDocumentQualification.DocumentQualificationSearch("", "", sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvDocumentQualification.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvDocumentQualification", "Qualification", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDocumentQualification.DataSource = ds;
            gvDocumentQualification.VirtualItemCount = iRowCount;
        }
        else
        {
            gvDocumentQualification.DataSource = "";
        }
        
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    }

 
    protected void gvDocumentQualification_Sorting(object sender, GridViewSortEventArgs se)
    {
     
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();      
    }
    

    private void InsertDocumentQualification(string Qualification, string Explanation, string preseaYN, string ActiveYN)
    {
        if (!IsValidDocumentQualification(Qualification.Trim(), Explanation.Trim()))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersDocumentQualification.InsertDocumentQualification(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , Qualification.Trim(), Explanation.Trim(), General.GetNullableInteger(preseaYN), General.GetNullableInteger(ActiveYN));
    }

    private void UpdateDocumentQualification(int QualificationId, string Qualification, string Explanation, string preseaYN, string ActiveYN)
    {
        if (!IsValidDocumentQualification(Qualification.Trim(), Explanation.Trim()))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersDocumentQualification.UpdateDocumentQualification(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , QualificationId, Qualification.Trim(), Explanation.Trim(), General.GetNullableInteger(preseaYN), General.GetNullableInteger(ActiveYN));
    }

    private bool IsValidDocumentQualification(string Qualification, string Explanation)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Qualification.Trim().Equals(""))
            ucError.ErrorMessage = "Qualification is required.";

        if (Explanation.Trim().Equals(""))
            ucError.ErrorMessage = "Explanation is required.";

        return (!ucError.IsError);
    }

    private void DeleteDocumentQualification(int QualificationId)
    {
        PhoenixRegistersDocumentQualification.DeleteDocumentQualification(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, QualificationId);
    }



    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvDocumentQualification_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string ispresea=string.Empty;
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;            
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertDocumentQualification(
                    ((RadTextBox)e.Item.FindControl("txtQualificationAdd")).Text.Trim(),
                    ((RadTextBox) e.Item.FindControl("txtExplanationAdd")).Text.Trim(),
                    ((RadCheckBox)e.Item.FindControl("chkPreSeaYNAdd")).Checked == true ? "1" : "0",
                    ((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked == true ? "1" : "0"
                );
                BindData();
                gvDocumentQualification.Rebind();               
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                RadCheckBox preseaynedit = ((RadCheckBox)e.Item.FindControl("chkPreSeaYNEdit"));
                RadCheckBox chkActiveYNEdit = ((RadCheckBox)e.Item.FindControl("chkActiveYNEdit"));
                if (preseaynedit != null)
                {
                    ispresea = preseaynedit.Checked == true ? "1" : "0";
                }

                UpdateDocumentQualification(
                     Int32.Parse(((RadLabel)e.Item.FindControl("lblQualificationIdEdit")).Text.Trim()),
                     ((RadLabel)e.Item.FindControl("lblQualificationEdit")).Text.Trim(),
                    ((RadTextBox)e.Item.FindControl("txtExplanationEdit")).Text.Trim(),
                    ispresea, ((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked == true ? "1" : "0"
                 );
                BindData();
                gvDocumentQualification.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDocumentQualification(Int32.Parse(((RadLabel)e.Item.FindControl("lblQualificationId")).Text));
                BindData();
                gvDocumentQualification.Rebind();
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

    protected void gvDocumentQualification_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDocumentQualification.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvDocumentQualification_ItemDataBound(object sender, GridItemEventArgs e)
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
            RadCheckBox chkActiveYNAdd = (RadCheckBox)e.Item.FindControl("chkActiveYNAdd");
            if (chkActiveYNAdd != null)
                chkActiveYNAdd.Checked = true;
        }
    }

    protected void gvDocumentQualification_SortCommand(object sender, GridSortCommandEventArgs e)
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
